Ž&
qC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Utilities\ValidationMessages.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
	Utilities %
{ 
public 

static 
class 
ValidationMessages *
{ 
public 
const 
string 
FullNameRequired ,
=- .
$str/ G
;G H
public		 
const		 
string		 !
InvalidFullNameFormat		 1
=		2 3
$str		4 g
;		g h
public 
const 
string 
DateOfBirthRequired /
=0 1
$str2 N
;N O
public 
const 
string %
DateOfBirthCannotBeFuture 5
=6 7
$str8 `
;` a
public 
const 
string ,
 DateOfBirthYearMustBe1900OrLater <
== >
$str? j
;j k
public 
const 
string 
GenderRequired *
=+ ,
$str- B
;B C
public 
const 
string 
PhoneNumberRequired /
=0 1
$str2 M
;M N
public 
const 
string $
InvalidPhoneNumberFormat 4
=5 6
$str7 e
;e f
public 
const 
string 
EmailRequired )
=* +
$str, @
;@ A
public 
const 
string 
InvalidEmailFormat .
=/ 0
$str1 V
;V W
public 
const 
string "
SpecialisationRequired 2
=3 4
$str5 R
;R S
public 
const 
string "
InvalidExperienceRange 2
=3 4
$str5 d
;d e
public 
const 
string "
InvalidConsultationFee 2
=3 4
$str5 d
;d e
public!! 
const!! 
string!! 
PatientRequired!! +
=!!, -
$str!!. M
;!!M N
public## 
const## 
string## 
DoctorRequired## *
=##+ ,
$str##- K
;##K L
public%% 
const%% 
string%% !
ScheduledDateRequired%% 1
=%%2 3
$str%%4 Q
;%%Q R
public'' 
const'' 
string'' 
TimeSlotRequired'' ,
=''- .
$str''/ G
;''G H
public)) 
const)) 
string)) %
AppointmentStatusRequired)) 5
=))6 7
$str))8 Y
;))Y Z
public++ 
const++ 
string++ 
VisitDateRequired++ -
=++. /
$str++0 I
;++I J
public-- 
const-- 
string-- 
DiagnosisRequired-- -
=--. /
$str--0 H
;--H I
public// 
const// 
string//  
PrescriptionRequired// 0
=//1 2
$str//3 N
;//N O
public11 
const11 
string11  
ProviderNameRequired11 0
=111 2
$str113 O
;11O P
public33 
const33 
string33  
PolicyNumberRequired33 0
=331 2
$str333 O
;33O P
public55 
const55 
string55 
ExpiryDateRequired55 .
=55/ 0
$str551 K
;55K L
public77 
const77 
string77 #
InsuranceStatusRequired77 3
=774 5
$str776 U
;77U V
public99 
const99 
string99 
PasswordRequired99 ,
=99- .
$str99/ F
;99F G
public;; 
const;; 
string;; 
UserRoleRequired;; ,
=;;- .
$str;;/ G
;;;G H
public== 
const== 
string== 
ReferenceIdRequired== /
===0 1
$str==2 M
;==M N
public?? 
const?? 
string?? %
ScheduledDateCannotBePast?? 5
=??6 7
$str??8 ^
;??^ _
publicAA 
constAA 
stringAA #
AppointmentDateRequiredAA 3
=AA4 5
$strAA6 U
;AAU V
publicCC 
constCC 
stringCC 
InvalidTimeSlotCC +
=CC, -
$strCC. g
;CCg h
publicEE 
constEE 
stringEE 
AppointmentRequiredEE /
=EE0 1
$strEE2 U
;EEU V
}GG 
}HH ä
oC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Utilities\ValidationLimits.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
	Utilities %
{ 
public 

static 
class 
ValidationLimits (
{ 
public 
const 
int 
FullNameLength '
=( )
$num* -
;- .
public 
const 
int 
EmailLength $
=% &
$num' *
;* +
public

 
const

 
int

 
PhoneNumberLength

 *
=

+ ,
$num

- /
;

/ 0
public 
const 
int 
PolicyNumberLength +
=, -
$num. 0
;0 1
public 
const 
int 
ProviderNameLength +
=, -
$num. 1
;1 2
public 
const 
int 
TimeSlotLength '
=( )
$num* ,
;, -
public 
const 
int $
CancellationReasonLength 1
=2 3
$num4 7
;7 8
public 
const 
int 
DiagnosisLength (
=) *
$num+ .
;. /
public 
const 
int 
PrescriptionLength +
=, -
$num. 1
;1 2
public 
const 
int 
NotesLength $
=% &
$num' +
;+ ,
public 
const 
int 
MinExperience &
=' (
$num) *
;* +
public 
const 
int 
MaxExperience &
=' (
$num) +
;+ ,
public 
const 
string 
MinCoverageAmount -
=. /
$str0 3
;3 4
public   
const   
string   
MaxCoverageAmount   -
=  . /
$str  0 ;
;  ; <
public"" 
const"" 
string"" 
MinConsultationFee"" .
=""/ 0
$str""1 7
;""7 8
public$$ 
const$$ 
string$$ 
MaxConsultationFee$$ .
=$$/ 0
$str$$1 8
;$$8 9
}%% 
}&& ÷
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Utilities\RegexPatterns.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
	Utilities %
{ 
public 

static 
class 
RegexPatterns %
{ 
public 
const 
string 
FullName $
=% &
$str' 7
;7 8
public 
const 
string 
PhoneNumber '
=( )
$str* 5
;5 6
}		 
}

 Â
cC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Enums\UserRole.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
Enums !
{ 
public 

enum 
Role 
{ 
Admin 
= 
$num 
, 
Doctor 
, 
Patient 
} 
}		 ü
jC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Enums\InsuranceStatus.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
Enums !
{ 
public 

enum 
InsuranceStatus 
{ 
Active 
= 
$num 
, 
Expired 
, 
	Suspended 
, 
Pending 
}		 
}

 ¿
aC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Enums\Gender.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
Enums !
{ 
public 

enum 
Gender 
{ 
Male 
= 
$num 
, 
Female 
, 
Other 
} 
}		 ï
oC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Enums\DoctorSpecialisation.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
Enums !
{ 
public 

enum 
Specialisation 
{ 

Cardiology 
, 
	Neurology 
, 
Dermatology 
, 
Orthopedics 
, 

Pediatrics		 
,		 

Gynecology

 
,

 
Oncology 
, 

Psychiatry 
, 
Ophthalmology 
, 
ENT 
, 
Pulmonology 
, 
Gastroenterology 
, 

Nephrology 
, 
Urology 
, 
Endocrinology 
, 
	Radiology 
, 
GeneralSurgery 
, 
Anesthesiology 
, 
EmergencyMedicine 
, 
GeneralMedicine 
} 
} Ó
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\Enums\AppointmentStatus.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
Enums !
;! "
public 
enum 
AppointmentStatus 
{ 
Pending 
, 
	Confirmed 
, 
	Completed 
, 
	Cancelled 
}		 Í-
rC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Patient\UpdatePatientDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Patient! (
{ 
public 

class 
UpdatePatientDto !
:" #
IValidatableObject$ 6
{ 
private		 
static		 
readonly		 
DateTime		  (
MinimumDateOfBirth		) ;
=		< =
new

 
DateTime

 
(

 
$num 
, 	
$num 
, 
$num 
, 
$num 
, 
$num 
, 
$num 
, 
DateTimeKind 
. 
Unspecified 
) 
; 
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
FullNameRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits &
.& '
FullNameLength' 5
)5 6
]6 7
[ 	
RegularExpression	 
( 
RegexPatterns 
. 
FullName "
," #
ErrorMessage 
= 
ValidationMessages -
.- .!
InvalidFullNameFormat. C
)C D
]D E
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DateOfBirthRequired4 G
)G H
]H I
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
GenderRequired4 B
)B C
]C D
public   
Gender   
Gender   
{   
get   "
;  " #
set  $ '
;  ' (
}  ) *
["" 	
Required""	 
("" 
ErrorMessage"" 
=""  
ValidationMessages""! 3
.""3 4
PhoneNumberRequired""4 G
)""G H
]""H I
[## 	
StringLength##	 
(## 
ValidationLimits## &
.##& '
PhoneNumberLength##' 8
)##8 9
]##9 :
[$$ 	
RegularExpression$$	 
($$ 
RegexPatterns%% 
.%% 
PhoneNumber%% %
,%%% &
ErrorMessage&& 
=&& 
ValidationMessages&& -
.&&- .$
InvalidPhoneNumberFormat&&. F
)&&F G
]&&G H
public'' 
string'' 
PhoneNumber'' !
{''" #
get''$ '
;''' (
set'') ,
;'', -
}''. /
=''0 1
string''2 8
.''8 9
Empty''9 >
;''> ?
[)) 	
Required))	 
()) 
ErrorMessage)) 
=))  
ValidationMessages))! 3
.))3 4
EmailRequired))4 A
)))A B
]))B C
[** 	
EmailAddress**	 
(** 
ErrorMessage** "
=**# $
ValidationMessages**% 7
.**7 8
InvalidEmailFormat**8 J
)**J K
]**K L
[++ 	
StringLength++	 
(++ 
ValidationLimits++ &
.++& '
EmailLength++' 2
)++2 3
]++3 4
public,, 
string,, 
Email,, 
{,, 
get,, !
;,,! "
set,,# &
;,,& '
},,( )
=,,* +
string,,, 2
.,,2 3
Empty,,3 8
;,,8 9
public.. 
string.. 
?.. 
InsuranceId.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}../ 0
public00 
IEnumerable00 
<00 
ValidationResult00 +
>00+ ,
Validate00- 5
(005 6
ValidationContext006 G
validationContext00H Y
)00Y Z
{11 	
if22 
(22 
DateOfBirth22 
.22 
Date22  
>22! "
DateTime22# +
.22+ ,
Today22, 1
)221 2
{33 
yield44 
return44 
new44  
ValidationResult44! 1
(441 2
$str55 <
,55< =
new66 
[66 
]66 
{66 
nameof66 "
(66" #
DateOfBirth66# .
)66. /
}660 1
)661 2
;662 3
}77 
if99 
(99 
DateOfBirth99 
.99 
Date99  
<99! "
MinimumDateOfBirth99# 5
.995 6
Date996 :
)99: ;
{:: 
yield;; 
return;; 
new;;  
ValidationResult;;! 1
(;;1 2
$str<< >
,<<> ?
new== 
[== 
]== 
{== 
nameof== "
(==" #
DateOfBirth==# .
)==. /
}==0 1
)==1 2
;==2 3
}>> 
}?? 	
}@@ 
}AA ‚
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Patient\PatientDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Patient! (
{ 
public 

class 

PatientDto 
{ 
public		 
int		 
	PatientId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
int 
Age 
{ 
get 
; 
set !
;! "
}# $
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} Í-
rC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Patient\CreatePatientDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Patient! (
{ 
public 

class 
CreatePatientDto !
:" #
IValidatableObject$ 6
{ 
private		 
static		 
readonly		 
DateTime		  (
MinimumDateOfBirth		) ;
=		< =
new		> A
DateTime

 
(

 	
$num 
, 
$num 
, 
$num 
, 
$num 
, 
$num 
, 
$num 
, 
DateTimeKind 
. 
Unspecified $
)$ %
;% &
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
FullNameRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits &
.& '
FullNameLength' 5
)5 6
]6 7
[ 	
RegularExpression	 
( 
RegexPatterns 
. 
FullName "
," #
ErrorMessage 
= 
ValidationMessages -
.- .!
InvalidFullNameFormat. C
)C D
]D E
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DateOfBirthRequired4 G
)G H
]H I
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
GenderRequired4 B
)B C
]C D
public   
Gender   
Gender   
{   
get   "
;  " #
set  $ '
;  ' (
}  ) *
["" 	
Required""	 
("" 
ErrorMessage"" 
=""  
ValidationMessages""! 3
.""3 4
PhoneNumberRequired""4 G
)""G H
]""H I
[## 	
StringLength##	 
(## 
ValidationLimits## &
.##& '
PhoneNumberLength##' 8
)##8 9
]##9 :
[$$ 	
RegularExpression$$	 
($$ 
RegexPatterns%% 
.%% 
PhoneNumber%% %
,%%% &
ErrorMessage&& 
=&& 
ValidationMessages&& -
.&&- .$
InvalidPhoneNumberFormat&&. F
)&&F G
]&&G H
public'' 
string'' 
PhoneNumber'' !
{''" #
get''$ '
;''' (
set'') ,
;'', -
}''. /
=''0 1
string''2 8
.''8 9
Empty''9 >
;''> ?
[)) 	
Required))	 
()) 
ErrorMessage)) 
=))  
ValidationMessages))! 3
.))3 4
EmailRequired))4 A
)))A B
]))B C
[** 	
EmailAddress**	 
(** 
ErrorMessage** "
=**# $
ValidationMessages**% 7
.**7 8
InvalidEmailFormat**8 J
)**J K
]**K L
[++ 	
StringLength++	 
(++ 
ValidationLimits++ &
.++& '
EmailLength++' 2
)++2 3
]++3 4
public,, 
string,, 
Email,, 
{,, 
get,, !
;,,! "
set,,# &
;,,& '
},,( )
=,,* +
string,,, 2
.,,2 3
Empty,,3 8
;,,8 9
public.. 
string.. 
?.. 
InsuranceId.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}../ 0
public00 
IEnumerable00 
<00 
ValidationResult00 +
>00+ ,
Validate00- 5
(005 6
ValidationContext006 G
validationContext00H Y
)00Y Z
{11 	
if22 
(22 
DateOfBirth22 
.22 
Date22  
>22! "
DateTime22# +
.22+ ,
Today22, 1
)221 2
{33 
yield44 
return44 
new44  
ValidationResult44! 1
(441 2
$str55 <
,55< =
new66 
[66 
]66 
{66 
nameof66 "
(66" #
DateOfBirth66# .
)66. /
}660 1
)661 2
;662 3
}77 
if99 
(99 
DateOfBirth99 
.99 
Date99  
<99! "
MinimumDateOfBirth99# 5
.995 6
Date996 :
)99: ;
{:: 
yield;; 
return;; 
new;;  
ValidationResult;;! 1
(;;1 2
$str<< >
,<<> ?
new== 
[== 
]== 
{== 
nameof== "
(==" #
DateOfBirth==# .
)==. /
}==0 1
)==1 2
;==2 3
}>> 
}?? 	
}@@ 
}AA Ø
vC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\HealthRecord\HealthRecordDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
HealthRecord! -
;- .
public 
class 
HealthRecordDto 
{ 
public 

int 
RecordId 
{ 
get 
; 
set "
;" #
}$ %
public 

int 
AppointmentId 
{ 
get "
;" #
set$ '
;' (
}) *
public		 

int		 
DoctorId		 
{		 
get		 
;		 
set		 "
;		" #
}		$ %
public 

int 
	PatientId 
{ 
get 
; 
set  #
;# $
}% &
public 

DateTime 
	VisitDate 
{ 
get  #
;# $
set% (
;( )
}* +
public 

string 
	Diagnosis 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 

string 
Prescription 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 

string 
Notes 
{ 
get 
; 
set "
;" #
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
} Š

|C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\HealthRecord\CreateHealthRecordDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
HealthRecord! -
{ 
public 

class !
CreateHealthRecordDto &
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
	Diagnosis 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
string0 6
.6 7
Empty7 <
;< =
public		 
string		 
Prescription		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		/ 0
=		1 2
string		3 9
.		9 :
Empty		: ?
;		? @
public 
string 
Notes 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
} 
} ¨
vC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Doctor\UpdateDoctorStatusDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Doctor! '
{ 
public 

class !
UpdateDoctorStatusDto &
{ 
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ¨
pC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Doctor\UpdateDoctorDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Doctor! '
{ 
public 

class 
UpdateDoctorDto  
{ 
[		 	
Required			 
(		 
ErrorMessage		 
=		  
ValidationMessages		! 3
.		3 4
FullNameRequired		4 D
)		D E
]		E F
[

 	
StringLength

	 
(

 
ValidationLimits

 &
.

& '
FullNameLength

' 5
)

5 6
]

6 7
[ 	
RegularExpression	 
( 
RegexPatterns (
.( )
FullName) 1
,1 2
ErrorMessage3 ?
=@ A
ValidationMessagesB T
.T U!
InvalidFullNameFormatU j
)j k
]k l
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4"
SpecialisationRequired4 J
)J K
]K L
public 
Specialisation 
Specialisation ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
[ 	
Range	 
( 
ValidationLimits 
.  
MinExperience  -
,- .
ValidationLimits/ ?
.? @
MaxExperience@ M
,M N
ErrorMessage 
= 
ValidationMessages -
.- ."
InvalidExperienceRange. D
)D E
]E F
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Range	 
( 
typeof 
( 
decimal 
) 
, 
ValidationLimits  0
.0 1
MinConsultationFee1 C
,C D
ValidationLimitsE U
.U V
MaxConsultationFeeV h
,h i
ErrorMessage 
= 
ValidationMessages -
.- ."
InvalidConsultationFee. D
)D E
]E F
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ¡
jC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Doctor\DoctorDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Doctor! '
;' (
public 
class 
	DoctorDto 
{ 
public 

int 
DoctorId 
{ 
get 
; 
set "
;" #
}$ %
public		 

string		 
FullName		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
=		) *
string		+ 1
.		1 2
Empty		2 7
;		7 8
public 

Specialisation 
Specialisation (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 

int 
YearsOfExperience  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 

decimal 
ConsultationFee "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 

bool 
IsActive 
{ 
get 
; 
set  #
;# $
}% &
public 

int $
UpcomingAppointmentCount '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
} í

vC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Doctor\DoctorAvailabilityDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Doctor! '
;' (
public 
sealed 
class !
DoctorAvailabilityDto )
{ 
public 

int 
DoctorId 
{ 
get 
; 
set "
;" #
}$ %
public 

string 
FullName 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
public		 

bool		 
IsActive		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
public 

DateTime 
Date 
{ 
get 
; 
set  #
;# $
}% &
public 

List 
< 
string 
> 
AvailableSlots &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
new7 :
(: ;
); <
;< =
} ”
pC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Doctor\CreateDoctorDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Doctor! '
{ 
public 

class 
CreateDoctorDto  
{ 
[		 	
Required			 
(		 
ErrorMessage		 
=		  
ValidationMessages		! 3
.		3 4
FullNameRequired		4 D
)		D E
]		E F
[

 	
StringLength

	 
(

 
ValidationLimits

 &
.

& '
FullNameLength

' 5
)

5 6
]

6 7
[ 	
RegularExpression	 
( 
RegexPatterns (
.( )
FullName) 1
,1 2
ErrorMessage3 ?
=@ A
ValidationMessagesB T
.T U!
InvalidFullNameFormatU j
)j k
]k l
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public 
string 
TemporaryPassword '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
=6 7
string8 >
.> ?
Empty? D
;D E
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4"
SpecialisationRequired4 J
)J K
]K L
public 
Specialisation 
Specialisation ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
[ 	
Range	 
( 
ValidationLimits 
.  
MinExperience  -
,- .
ValidationLimits/ ?
.? @
MaxExperience@ M
,M N
ErrorMessage 
= 
ValidationMessages -
.- ."
InvalidExperienceRange. D
)D E
]E F
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Range	 
( 
typeof 
( 
decimal 
) 
, 
ValidationLimits  0
.0 1
MinConsultationFee1 C
,C D
ValidationLimitsE U
.U V
MaxConsultationFeeV h
,h i
ErrorMessage 
= 
ValidationMessages -
.- ."
InvalidConsultationFee. D
)D E
]E F
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
}   ¨	
qC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Common\PaginationParams.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Common! '
;' (
public 
class 
PaginationParams 
{ 
private 
const 
int 
MaxPageSize !
=" #
$num$ &
;& '
public 

int 

PageNumber 
{ 
get 
;  
set! $
;$ %
}& '
=( )
$num* +
;+ ,
private		 
int		 
	_pageSize		 
=		 
$num		 
;		 
public 

int 
PageSize 
{ 
get 
=> 
	_pageSize 
; 
set 
=> 
	_pageSize 
= 
value  
>! "
MaxPageSize# .
?/ 0
MaxPageSize1 <
:= >
value? D
;D E
} 
} Â
nC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Common\PagedResponse.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Common! '
;' (
public 
class 
PagedResponse 
< 
T 
> 
{ 
public 

List 
< 
T 
> 
Items 
{ 
get 
; 
set  #
;# $
}% &
=' (
new) ,
(, -
)- .
;. /
public 

List 
< 
T 
> 
Data 
{ 
get		 
=>		 
Items		 
;		 
set

 
=>

 
Items

 
=

 
value

 
;

 
} 
public 

int 

PageNumber 
{ 
get 
;  
set! $
;$ %
}& '
public 

int 
PageSize 
{ 
get 
; 
set "
;" #
}$ %
public 

int 
TotalRecords 
{ 
get !
;! "
set# &
;& '
}( )
public 

int 

TotalPages 
{ 
get 
;  
set! $
;$ %
}& '
} “	
nC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Common\ErrorResponse.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Common! '
{ 
public 

class 
ErrorResponse 
{ 
public 
int 

StatusCode 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
Message 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
public		 
DateTime		 
	TimeStamp		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
public 
string 
Path 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
} 
} Ó

fC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Auth\UserDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Auth! %
;% &
public 
class 
UserDto 
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
FullName 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
public		 

string		 
Email		 
{		 
get		 
;		 
set		 "
;		" #
}		$ %
=		& '
string		( .
.		. /
Empty		/ 4
;		4 5
public 

string 
Role 
{ 
get 
; 
set !
;! "
}# $
=% &
string' -
.- .
Empty. 3
;3 4
public 

bool 
IsActive 
{ 
get 
; 
set  #
;# $
}% &
} –
jC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Auth\RegisterDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Auth! %
{ 
public 

class 
RegisterDto 
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public		 
DateTime		 
DateOfBirth		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		0 1
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} €
uC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Auth\RefreshTokenRequestDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Auth! %
{ 
public 

class "
RefreshTokenRequestDto '
{ 
public 
string 
RefreshToken "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
} 
} ç	
gC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Auth\LoginDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Auth! %
;% &
public 
class 
LoginDto 
{ 
[ 
Required 
( 
ErrorMessage 
= 
$str 1
)1 2
]2 3
[ 
EmailAddress 
( 
ErrorMessage 
=  
$str! 8
)8 9
]9 :
public		 

string		 
Email		 
{		 
get		 
;		 
set		 "
;		" #
}		$ %
=		& '
string		( .
.		. /
Empty		/ 4
;		4 5
[ 
Required 
( 
ErrorMessage 
= 
$str 4
)4 5
]5 6
public 

string 
Password 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
} Ì

pC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Auth\ChangePasswordDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Auth! %
{ 
public 

class 
ChangePasswordDto "
{ 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
string 
OldPassword !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public		 
string		 
NewPassword		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
=		0 1
string		2 8
.		8 9
Empty		9 >
;		> ?
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
} 
} ·
kC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Auth\AuthResponse.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Auth! %
;% &
public 
class 
AuthResponse 
{ 
public 

bool 
Success 
{ 
get 
; 
set "
;" #
}$ %
public 

string 
UserId 
{ 
get 
; 
set  #
;# $
}% &
=' (
string) /
./ 0
Empty0 5
;5 6
public		 

string		 
AccessToken		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
=		, -
string		. 4
.		4 5
Empty		5 :
;		: ;
public 

string 
RefreshToken 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 

string 
Message 
{ 
get 
;  
set! $
;$ %
}& '
=( )
string* 0
.0 1
Empty1 6
;6 7
public 

int 
	ExpiresIn 
{ 
get 
; 
set  #
;# $
}% &
public 

bool "
RequiresPasswordChange &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
false7 <
;< =
} ÷
€C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Appointment\UpdateAppointmentStatusDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Appointment! ,
{ 
public 

class &
UpdateAppointmentStatusDto +
{ 
[		 	
Required			 
(		 
ErrorMessage

 
=

 
ValidationMessages

 -
.

- .%
AppointmentStatusRequired

. G
)

G H
]

H I
public 
AppointmentStatus  
Status! '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
StringLength	 
( 
ValidationLimits 
. $
CancellationReasonLength 5
)5 6
]6 7
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} ¬
zC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Appointment\CreateAppointmentDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Appointment! ,
{ 
public 

class  
CreateAppointmentDto %
{ 
[		 	
Required			 
(		 
ErrorMessage		 
=		  
ValidationMessages		! 3
.		3 4
PatientRequired		4 C
)		C D
]		D E
public

 
int

 
	PatientId

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

) *
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DoctorRequired4 B
)B C
]C D
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4#
AppointmentDateRequired4 K
)K L
]L M
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
TimeSlotRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits 
. 
TimeSlotLength +
,+ ,
ErrorMessage 
= 
ValidationMessages -
.- .
InvalidTimeSlot. =
)= >
]> ?
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
} 
} È
tC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\HealthcareAxis.Shared\DTOs\Appointment\AppointmentDto.cs
	namespace 	

HealthAxis
 
. 
Shared 
. 
DTOs  
.  !
Appointment! ,
{ 
public 

class 
AppointmentDto 
{ 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
int		 
	PatientId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
AppointmentStatus  
Status! '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} 