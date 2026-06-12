import re

# Read the project file
with open('HealthCareApi.csproj', 'r') as f:
    content = f.read()

# Find and replace the VSToolsPath section
pattern = r'  <PropertyGroup>\s+<VSToolsPath Condition="\'"\$\(VSToolsPath\)\'" == \'""\'">(\$\(MSBuildExtensionsPath32\)\\Microsoft\\VisualStudio\\v\$\(VisualStudioVersion\))</VSToolsPath>\s+</PropertyGroup>'

replacement = '''  <PropertyGroup>
    <VSToolsPath Condition="'$(VSToolsPath)' == ''">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)</VSToolsPath>
    <VSToolsPath Condition="!Exists('$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets')">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v17.0</VSToolsPath>
  </PropertyGroup>'''

content = re.sub(pattern, replacement, content)

# Also update the Import statement
old_import = '<Import Project="$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets" Condition="\'$(VSToolsPath)\' != \'\'" />'
new_import = '<Import Project="$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets" Condition="Exists(\'$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets\')" />'

content = content.replace(old_import, new_import)

# Write back
with open('HealthCareApi.csproj', 'w') as f:
    f.write(content)

print("Project file updated successfully")
