ƒ
yC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IPatientAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface  
IPatientAdminService )
{ 
Task 
< 
List 
< 

PatientDto 
> 
> 
GetAllPatientsAsync 2
(2 3
)3 4
;4 5
Task		 
<		 

PatientDto		 
?		 
>		 
GetPatientByIdAsync		 -
(		- .
int		. 1
	patientId		2 ;
)		; <
;		< =
Task 
< 

PatientDto 
> 
CreatePatientAsync +
(+ ,
CreatePatientDto, <
request= D
)D E
;E F
Task 
< 

PatientDto 
? 
> 
UpdatePatientAsync ,
(, -
int- 0
	patientId1 :
,: ;
UpdatePatientDto< L
requestM T
)T U
;U V
Task 
< 
bool 
> 
DeletePatientAsync %
(% &
int& )
	patientId* 3
)3 4
;4 5
} 
} É
xC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IDoctorAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface 
IDoctorAdminService (
{ 
Task 
< 
List 
< 
	DoctorDto 
> 
> 
GetAllDoctorsAsync 0
(0 1
)1 2
;2 3
Task		 
<		 
	DoctorDto		 
?		 
>		 
GetDoctorByIdAsync		 +
(		+ ,
int		, /
doctorId		0 8
)		8 9
;		9 :
Task 
< 
	DoctorDto 
> 
CreateDoctorAsync )
() *
CreateDoctorDto* 9
request: A
)A B
;B C
Task 
< 
	DoctorDto 
? 
> 
UpdateDoctorAsync *
(* +
int+ .
doctorId/ 7
,7 8
UpdateDoctorDto9 H
requestI P
)P Q
;Q R
Task 
< 
bool 
> 
DeleteDoctorAsync $
($ %
int% (
doctorId) 1
)1 2
;2 3
Task 
< 
	DoctorDto 
? 
> #
ToggleDoctorStatusAsync 0
(0 1
int1 4
doctorId5 =
)= >
;> ?
} 
} Ü	
qC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IAuthService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface 
IAuthService !
{ 
Task 
< 
AuthResponseDto 
> 

LoginAsync (
(( )
LoginDto) 1
loginDto2 :
): ;
;; <
Task		 
LogoutAsync		 
(		 
)		 
;		 
Task 
< 
bool 
>  
IsAuthenticatedAsync '
(' (
)( )
;) *
Task 
< 
string 
? 
> $
GetCurrentUserEmailAsync .
(. /
)/ 0
;0 1
Task 
< 
string 
? 
> #
GetCurrentUserRoleAsync -
(- .
). /
;/ 0
} 
} Ö
~C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IAppointementAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface $
IAppointmentAdminService -
{ 
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "#
GetAllAppointmentsAsync# :
(: ;
); <
;< =
Task		 
<		 
AppointmentDto		 
?		 
>		 #
GetAppointmentByIdAsync		 5
(		5 6
int		6 9
appointmentId		: G
)		G H
;		H I
Task 
< 
AppointmentDto 
> "
CreateAppointmentAsync 3
(3 4 
CreateAppointmentDto4 H
requestI P
)P Q
;Q R
Task 
< 
AppointmentDto 
? 
> "
UpdateAppointmentAsync 4
(4 5
int5 8
appointmentId9 F
,F G 
UpdateAppointmentDtoH \
request] d
)d e
;e f
Task 
< 
bool 
> "
DeleteAppointmentAsync )
() *
int* -
appointmentId. ;
); <
;< =
} 
} œ
{C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IAdminDashboardService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface "
IAdminDashboardService +
{ 
Task 
< #
AdminDashboardReportDto $
>$ %#
GetDashboardReportAsync& =
(= >
)> ?
;? @
} 
}		 πL
rC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\PatientAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class 
PatientAdminService $
:% & 
IPatientAdminService' ;
{ 
private 
static 
readonly 
List  $
<$ %

PatientDto% /
>/ 0
Patients1 9
=: ;
new< ?
(? @
)@ A
{		 	
new

 

PatientDto

 
{ 
	PatientId 
= 
$num 
, 
FullName 
= 
$str '
,' (
DateOfBirth 
= 
new !
DateTime" *
(* +
$num+ /
,/ 0
$num1 2
,2 3
$num4 6
)6 7
,7 8
Gender 
= 
$str 
,  
Email 
= 
$str 0
,0 1
PhoneNumber 
= 
$str *
,* +
InsuranceId 
= 
$str '
,' (
CreatedDate 
= 
new !
DateTime" *
(* +
$num+ /
,/ 0
$num1 2
,2 3
$num4 6
)6 7
} 
, 
new 

PatientDto 
{ 
	PatientId 
= 
$num 
, 
FullName 
= 
$str (
,( )
DateOfBirth 
= 
new !
DateTime" *
(* +
$num+ /
,/ 0
$num1 2
,2 3
$num4 6
)6 7
,7 8
Gender 
= 
$str !
,! "
Email 
= 
$str 1
,1 2
PhoneNumber 
= 
$str *
,* +
InsuranceId 
= 
$str '
,' (
CreatedDate 
= 
new !
DateTime" *
(* +
$num+ /
,/ 0
$num1 2
,2 3
$num4 6
)6 7
} 
, 
new   

PatientDto   
{!! 
	PatientId"" 
="" 
$num"" 
,"" 
FullName## 
=## 
$str## &
,##& '
DateOfBirth$$ 
=$$ 
new$$ !
DateTime$$" *
($$* +
$num$$+ /
,$$/ 0
$num$$1 3
,$$3 4
$num$$5 6
)$$6 7
,$$7 8
Gender%% 
=%% 
$str%%  
,%%  !
Email&& 
=&& 
$str&& /
,&&/ 0
PhoneNumber'' 
='' 
$str'' *
,''* +
InsuranceId(( 
=(( 
null(( "
,((" #
CreatedDate)) 
=)) 
new)) !
DateTime))" *
())* +
$num))+ /
,))/ 0
$num))1 2
,))2 3
$num))4 6
)))6 7
}** 
}++ 	
;++	 

public-- 
Task-- 
<-- 
List-- 
<-- 

PatientDto-- #
>--# $
>--$ %
GetAllPatientsAsync--& 9
(--9 :
)--: ;
{.. 	
var// 
patients// 
=// 
Patients// #
.00 
OrderBy00 
(00 
p00 
=>00 
p00 
.00  
	PatientId00  )
)00) *
.11 
ToList11 
(11 
)11 
;11 
return33 
Task33 
.33 

FromResult33 "
(33" #
patients33# +
)33+ ,
;33, -
}44 	
public66 
Task66 
<66 

PatientDto66 
?66 
>66  
GetPatientByIdAsync66! 4
(664 5
int665 8
	patientId669 B
)66B C
{77 	
var88 
patient88 
=88 
Patients88 "
.88" #
FirstOrDefault88# 1
(881 2
p882 3
=>884 6
p887 8
.888 9
	PatientId889 B
==88C E
	patientId88F O
)88O P
;88P Q
return:: 
Task:: 
.:: 

FromResult:: "
(::" #
patient::# *
)::* +
;::+ ,
};; 	
public== 
Task== 
<== 

PatientDto== 
>== 
CreatePatientAsync==  2
(==2 3
CreatePatientDto==3 C
request==D K
)==K L
{>> 	
int?? 
nextId?? 
=?? 
Patients?? !
.??! "
Any??" %
(??% &
)??& '
?@@ 
Patients@@ 
.@@ 
Max@@ 
(@@ 
p@@  
=>@@! #
p@@$ %
.@@% &
	PatientId@@& /
)@@/ 0
+@@1 2
$num@@3 4
:AA 
$numAA 
;AA 
varCC 
patientCC 
=CC 
newCC 

PatientDtoCC (
{DD 
	PatientIdEE 
=EE 
nextIdEE "
,EE" #
FullNameFF 
=FF 
requestFF "
.FF" #
FullNameFF# +
,FF+ ,
DateOfBirthGG 
=GG 
requestGG %
.GG% &
DateOfBirthGG& 1
.GG1 2
DateGG2 6
,GG6 7
GenderHH 
=HH 
requestHH  
.HH  !
GenderHH! '
,HH' (
EmailII 
=II 
requestII 
.II  
EmailII  %
,II% &
PhoneNumberJJ 
=JJ 
requestJJ %
.JJ% &
PhoneNumberJJ& 1
,JJ1 2
InsuranceIdKK 
=KK 
requestKK %
.KK% &
InsuranceIdKK& 1
,KK1 2
CreatedDateLL 
=LL 
DateTimeLL &
.LL& '
TodayLL' ,
}MM 
;MM 
PatientsOO 
.OO 
AddOO 
(OO 
patientOO  
)OO  !
;OO! "
returnQQ 
TaskQQ 
.QQ 

FromResultQQ "
(QQ" #
patientQQ# *
)QQ* +
;QQ+ ,
}RR 	
publicTT 
TaskTT 
<TT 

PatientDtoTT 
?TT 
>TT  
UpdatePatientAsyncTT! 3
(TT3 4
intTT4 7
	patientIdTT8 A
,TTA B
UpdatePatientDtoTTC S
requestTTT [
)TT[ \
{UU 	
varVV 
patientVV 
=VV 
PatientsVV "
.VV" #
FirstOrDefaultVV# 1
(VV1 2
pVV2 3
=>VV4 6
pVV7 8
.VV8 9
	PatientIdVV9 B
==VVC E
	patientIdVVF O
)VVO P
;VVP Q
ifXX 
(XX 
patientXX 
isXX 
nullXX 
)XX  
{YY 
returnZZ 
TaskZZ 
.ZZ 

FromResultZZ &
<ZZ& '

PatientDtoZZ' 1
?ZZ1 2
>ZZ2 3
(ZZ3 4
nullZZ4 8
)ZZ8 9
;ZZ9 :
}[[ 
patient]] 
.]] 
FullName]] 
=]] 
request]] &
.]]& '
FullName]]' /
;]]/ 0
patient^^ 
.^^ 
DateOfBirth^^ 
=^^  !
request^^" )
.^^) *
DateOfBirth^^* 5
.^^5 6
Date^^6 :
;^^: ;
patient__ 
.__ 
Gender__ 
=__ 
request__ $
.__$ %
Gender__% +
;__+ ,
patient`` 
.`` 
Email`` 
=`` 
request`` #
.``# $
Email``$ )
;``) *
patientaa 
.aa 
PhoneNumberaa 
=aa  !
requestaa" )
.aa) *
PhoneNumberaa* 5
;aa5 6
patientbb 
.bb 
InsuranceIdbb 
=bb  !
requestbb" )
.bb) *
InsuranceIdbb* 5
;bb5 6
returndd 
Taskdd 
.dd 

FromResultdd "
<dd" #

PatientDtodd# -
?dd- .
>dd. /
(dd/ 0
patientdd0 7
)dd7 8
;dd8 9
}ee 	
publicgg 
Taskgg 
<gg 
boolgg 
>gg 
DeletePatientAsyncgg ,
(gg, -
intgg- 0
	patientIdgg1 :
)gg: ;
{hh 	
varii 
patientii 
=ii 
Patientsii "
.ii" #
FirstOrDefaultii# 1
(ii1 2
pii2 3
=>ii4 6
pii7 8
.ii8 9
	PatientIdii9 B
==iiC E
	patientIdiiF O
)iiO P
;iiP Q
ifkk 
(kk 
patientkk 
iskk 
nullkk 
)kk  
{ll 
returnmm 
Taskmm 
.mm 

FromResultmm &
(mm& '
falsemm' ,
)mm, -
;mm- .
}nn 
Patientspp 
.pp 
Removepp 
(pp 
patientpp #
)pp# $
;pp$ %
returnrr 
Taskrr 
.rr 

FromResultrr "
(rr" #
truerr# '
)rr' (
;rr( )
}ss 	
}tt 
}uu ˚L
qC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\DoctorAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class 
DoctorAdminService #
:$ %
IDoctorAdminService& 9
{ 
private 
static 
readonly 
List  $
<$ %
	DoctorDto% .
>. /
Doctors0 7
=8 9
new: =
(= >
)> ?
{		 	
new

 
	DoctorDto

 
{ 
DoctorId 
= 
$num 
, 
FullName 
= 
$str '
,' (
Email 
= 
$str 0
,0 1
Specialisation 
=  
$str! 7
,7 8
YearsOfExperience !
=" #
$num$ &
,& '
ConsultationFee 
=  !
$num" %
,% &
IsActive 
= 
true 
} 
, 
new 
	DoctorDto 
{ 
DoctorId 
= 
$num 
, 
FullName 
= 
$str '
,' (
Email 
= 
$str 0
,0 1
Specialisation 
=  
$str! /
,/ 0
YearsOfExperience !
=" #
$num$ &
,& '
ConsultationFee 
=  !
$num" &
,& '
IsActive 
= 
true 
} 
, 
new 
	DoctorDto 
{ 
DoctorId   
=   
$num   
,   
FullName!! 
=!! 
$str!! '
,!!' (
Email"" 
="" 
$str"" 0
,""0 1
Specialisation## 
=##  
$str##! 0
,##0 1
YearsOfExperience$$ !
=$$" #
$num$$$ %
,$$% &
ConsultationFee%% 
=%%  !
$num%%" %
,%%% &
IsActive&& 
=&& 
true&& 
}'' 
}(( 	
;((	 

public** 
Task** 
<** 
List** 
<** 
	DoctorDto** "
>**" #
>**# $
GetAllDoctorsAsync**% 7
(**7 8
)**8 9
{++ 	
var,, 
doctors,, 
=,, 
Doctors,, !
.-- 
OrderBy-- 
(-- 
d-- 
=>-- 
d-- 
.--  
DoctorId--  (
)--( )
... 
ToList.. 
(.. 
).. 
;.. 
return00 
Task00 
.00 

FromResult00 "
(00" #
doctors00# *
)00* +
;00+ ,
}11 	
public33 
Task33 
<33 
	DoctorDto33 
?33 
>33 
GetDoctorByIdAsync33  2
(332 3
int333 6
doctorId337 ?
)33? @
{44 	
var55 
doctor55 
=55 
Doctors55  
.55  !
FirstOrDefault55! /
(55/ 0
d550 1
=>552 4
d555 6
.556 7
DoctorId557 ?
==55@ B
doctorId55C K
)55K L
;55L M
return77 
Task77 
.77 

FromResult77 "
(77" #
doctor77# )
)77) *
;77* +
}88 	
public:: 
Task:: 
<:: 
	DoctorDto:: 
>:: 
CreateDoctorAsync:: 0
(::0 1
CreateDoctorDto::1 @
request::A H
)::H I
{;; 	
int<< 
nextId<< 
=<< 
Doctors<<  
.<<  !
Any<<! $
(<<$ %
)<<% &
?== 
Doctors== 
.== 
Max== 
(== 
d== 
=>==  "
d==# $
.==$ %
DoctorId==% -
)==- .
+==/ 0
$num==1 2
:>> 
$num>> 
;>> 
var@@ 
doctor@@ 
=@@ 
new@@ 
	DoctorDto@@ &
{AA 
DoctorIdBB 
=BB 
nextIdBB !
,BB! "
FullNameCC 
=CC 
requestCC "
.CC" #
FullNameCC# +
,CC+ ,
EmailDD 
=DD 
requestDD 
.DD  
EmailDD  %
,DD% &
SpecialisationEE 
=EE  
requestEE! (
.EE( )
SpecialisationEE) 7
,EE7 8
YearsOfExperienceFF !
=FF" #
requestFF$ +
.FF+ ,
YearsOfExperienceFF, =
,FF= >
ConsultationFeeGG 
=GG  !
requestGG" )
.GG) *
ConsultationFeeGG* 9
,GG9 :
IsActiveHH 
=HH 
requestHH "
.HH" #
IsActiveHH# +
}II 
;II 
DoctorsKK 
.KK 
AddKK 
(KK 
doctorKK 
)KK 
;KK  
returnMM 
TaskMM 
.MM 

FromResultMM "
(MM" #
doctorMM# )
)MM) *
;MM* +
}NN 	
publicPP 
TaskPP 
<PP 
	DoctorDtoPP 
?PP 
>PP 
UpdateDoctorAsyncPP  1
(PP1 2
intPP2 5
doctorIdPP6 >
,PP> ?
UpdateDoctorDtoPP@ O
requestPPP W
)PPW X
{QQ 	
varRR 
doctorRR 
=RR 
DoctorsRR  
.RR  !
FirstOrDefaultRR! /
(RR/ 0
dRR0 1
=>RR2 4
dRR5 6
.RR6 7
DoctorIdRR7 ?
==RR@ B
doctorIdRRC K
)RRK L
;RRL M
ifTT 
(TT 
doctorTT 
isTT 
nullTT 
)TT 
{UU 
returnVV 
TaskVV 
.VV 

FromResultVV &
<VV& '
	DoctorDtoVV' 0
?VV0 1
>VV1 2
(VV2 3
nullVV3 7
)VV7 8
;VV8 9
}WW 
doctorYY 
.YY 
FullNameYY 
=YY 
requestYY %
.YY% &
FullNameYY& .
;YY. /
doctorZZ 
.ZZ 
EmailZZ 
=ZZ 
requestZZ "
.ZZ" #
EmailZZ# (
;ZZ( )
doctor[[ 
.[[ 
Specialisation[[ !
=[[" #
request[[$ +
.[[+ ,
Specialisation[[, :
;[[: ;
doctor\\ 
.\\ 
YearsOfExperience\\ $
=\\% &
request\\' .
.\\. /
YearsOfExperience\\/ @
;\\@ A
doctor]] 
.]] 
ConsultationFee]] "
=]]# $
request]]% ,
.]], -
ConsultationFee]]- <
;]]< =
doctor^^ 
.^^ 
IsActive^^ 
=^^ 
request^^ %
.^^% &
IsActive^^& .
;^^. /
return`` 
Task`` 
.`` 

FromResult`` "
<``" #
	DoctorDto``# ,
?``, -
>``- .
(``. /
doctor``/ 5
)``5 6
;``6 7
}aa 	
publiccc 
Taskcc 
<cc 
boolcc 
>cc 
DeleteDoctorAsynccc +
(cc+ ,
intcc, /
doctorIdcc0 8
)cc8 9
{dd 	
varee 
doctoree 
=ee 
Doctorsee  
.ee  !
FirstOrDefaultee! /
(ee/ 0
dee0 1
=>ee2 4
dee5 6
.ee6 7
DoctorIdee7 ?
==ee@ B
doctorIdeeC K
)eeK L
;eeL M
ifgg 
(gg 
doctorgg 
isgg 
nullgg 
)gg 
{hh 
returnii 
Taskii 
.ii 

FromResultii &
(ii& '
falseii' ,
)ii, -
;ii- .
}jj 
Doctorsll 
.ll 
Removell 
(ll 
doctorll !
)ll! "
;ll" #
returnnn 
Tasknn 
.nn 

FromResultnn "
(nn" #
truenn# '
)nn' (
;nn( )
}oo 	
publicqq 
Taskqq 
<qq 
	DoctorDtoqq 
?qq 
>qq #
ToggleDoctorStatusAsyncqq  7
(qq7 8
intqq8 ;
doctorIdqq< D
)qqD E
{rr 	
varss 
doctorss 
=ss 
Doctorsss  
.ss  !
FirstOrDefaultss! /
(ss/ 0
dss0 1
=>ss2 4
dss5 6
.ss6 7
DoctorIdss7 ?
==ss@ B
doctorIdssC K
)ssK L
;ssL M
ifuu 
(uu 
doctoruu 
isuu 
nulluu 
)uu 
{vv 
returnww 
Taskww 
.ww 

FromResultww &
<ww& '
	DoctorDtoww' 0
?ww0 1
>ww1 2
(ww2 3
nullww3 7
)ww7 8
;ww8 9
}xx 
doctorzz 
.zz 
IsActivezz 
=zz 
!zz 
doctorzz %
.zz% &
IsActivezz& .
;zz. /
return|| 
Task|| 
.|| 

FromResult|| "
<||" #
	DoctorDto||# ,
?||, -
>||- .
(||. /
doctor||/ 5
)||5 6
;||6 7
}}} 	
}~~ 
} ™%
jC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\AuthService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
bool 
_isAuthenticated %
;% &
private

 
string

 
?

 
_currentUserEmail

 )
;

) *
private 
string 
? 
_currentUserRole (
;( )
private 
const 
string 

AdminEmail '
=( )
$str* @
;@ A
private 
const 
string 
AdminPassword *
=+ ,
$str- 8
;8 9
public 
Task 
< 
AuthResponseDto #
># $

LoginAsync% /
(/ 0
LoginDto0 8
loginDto9 A
)A B
{ 	
if 
( 
loginDto 
. 
Email 
. 
Equals %
(% &

AdminEmail& 0
,0 1
StringComparison2 B
.B C
OrdinalIgnoreCaseC T
)T U
&&V X
loginDto 
. 
Password !
==" $
AdminPassword% 2
)2 3
{ 
_isAuthenticated  
=! "
true# '
;' (
_currentUserEmail !
=" #

AdminEmail$ .
;. /
_currentUserRole  
=! "
$str# *
;* +
return 
Task 
. 

FromResult &
(& '
new' *
AuthResponseDto+ :
{ 
	IsSuccess 
= 
true  $
,$ %
Message 
= 
$str 1
,1 2
AccessToken 
=  !
$str" 4
,4 5
Role   
=   
$str   "
,  " #
Email!! 
=!! 

AdminEmail!! &
}"" 
)"" 
;"" 
}## 
_isAuthenticated%% 
=%% 
false%% $
;%%$ %
_currentUserEmail&& 
=&& 
null&&  $
;&&$ %
_currentUserRole'' 
='' 
null'' #
;''# $
return)) 
Task)) 
.)) 

FromResult)) "
())" #
new))# &
AuthResponseDto))' 6
{** 
	IsSuccess++ 
=++ 
false++ !
,++! "
Message,, 
=,, 
$str,, 6
,,,6 7
AccessToken-- 
=-- 
string-- $
.--$ %
Empty--% *
,--* +
Role.. 
=.. 
string.. 
... 
Empty.. #
,..# $
Email// 
=// 
string// 
.// 
Empty// $
}00 
)00 
;00 
}11 	
public33 
Task33 
LogoutAsync33 
(33  
)33  !
{44 	
_isAuthenticated55 
=55 
false55 $
;55$ %
_currentUserEmail66 
=66 
null66  $
;66$ %
_currentUserRole77 
=77 
null77 #
;77# $
return99 
Task99 
.99 
CompletedTask99 %
;99% &
}:: 	
public<< 
Task<< 
<<< 
bool<< 
><<  
IsAuthenticatedAsync<< .
(<<. /
)<</ 0
{== 	
return>> 
Task>> 
.>> 

FromResult>> "
(>>" #
_isAuthenticated>># 3
)>>3 4
;>>4 5
}?? 	
publicAA 
TaskAA 
<AA 
stringAA 
?AA 
>AA $
GetCurrentUserEmailAsyncAA 5
(AA5 6
)AA6 7
{BB 	
returnCC 
TaskCC 
.CC 

FromResultCC "
(CC" #
_currentUserEmailCC# 4
)CC4 5
;CC5 6
}DD 	
publicFF 
TaskFF 
<FF 
stringFF 
?FF 
>FF #
GetCurrentUserRoleAsyncFF 4
(FF4 5
)FF5 6
{GG 	
returnHH 
TaskHH 
.HH 

FromResultHH "
(HH" #
_currentUserRoleHH# 3
)HH3 4
;HH4 5
}II 	
}JJ 
}KK ≥{
vC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\AppointmentAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class #
AppointmentAdminService (
:) *$
IAppointmentAdminService+ C
{ 
private 
static 
readonly 
List  $
<$ %
AppointmentDto% 3
>3 4
Appointments5 A
=B C
newD G
(G H
)H I
{		 	
new

 
AppointmentDto

 
{ 
AppointmentId 
= 
$num  !
,! "
	PatientId 
= 
$num 
, 
PatientName 
= 
$str *
,* +
DoctorId 
= 
$num 
, 

DoctorName 
= 
$str )
,) *
ScheduledDate 
= 
DateTime  (
.( )
Today) .
,. /
TimeSlot 
= 
$str 0
,0 1
Status 
=  
AppointmentStatusDto -
.- .
Pending. 5
,5 6
CancellationReason "
=# $
null% )
,) *
CreatedDate 
= 
DateTime &
.& '
Today' ,
., -
AddDays- 4
(4 5
-5 6
$num6 7
)7 8
} 
, 
new 
AppointmentDto 
{ 
AppointmentId 
= 
$num  !
,! "
	PatientId 
= 
$num 
, 
PatientName 
= 
$str +
,+ ,
DoctorId 
= 
$num 
, 

DoctorName 
= 
$str )
,) *
ScheduledDate 
= 
DateTime  (
.( )
Today) .
,. /
TimeSlot 
= 
$str 0
,0 1
Status   
=    
AppointmentStatusDto   -
.  - .
	Confirmed  . 7
,  7 8
CancellationReason!! "
=!!# $
null!!% )
,!!) *
CreatedDate"" 
="" 
DateTime"" &
.""& '
Today""' ,
."", -
AddDays""- 4
(""4 5
-""5 6
$num""6 7
)""7 8
}## 
,## 
new$$ 
AppointmentDto$$ 
{%% 
AppointmentId&& 
=&& 
$num&&  !
,&&! "
	PatientId'' 
='' 
$num'' 
,'' 
PatientName(( 
=(( 
$str(( )
,(() *
DoctorId)) 
=)) 
$num)) 
,)) 

DoctorName** 
=** 
$str** )
,**) *
ScheduledDate++ 
=++ 
DateTime++  (
.++( )
Today++) .
.++. /
AddDays++/ 6
(++6 7
$num++7 8
)++8 9
,++9 :
TimeSlot,, 
=,, 
$str,, 0
,,,0 1
Status-- 
=--  
AppointmentStatusDto-- -
.--- .
	Cancelled--. 7
,--7 8
CancellationReason.. "
=..# $
$str..% E
,..E F
CreatedDate// 
=// 
DateTime// &
.//& '
Today//' ,
.//, -
AddDays//- 4
(//4 5
-//5 6
$num//6 7
)//7 8
}00 
,00 
new11 
AppointmentDto11 
{22 
AppointmentId33 
=33 
$num33  !
,33! "
	PatientId44 
=44 
$num44 
,44 
PatientName55 
=55 
$str55 *
,55* +
DoctorId66 
=66 
$num66 
,66 

DoctorName77 
=77 
$str77 )
,77) *
ScheduledDate88 
=88 
DateTime88  (
.88( )
Today88) .
.88. /
AddDays88/ 6
(886 7
-887 8
$num888 9
)889 :
,88: ;
TimeSlot99 
=99 
$str99 0
,990 1
Status:: 
=::  
AppointmentStatusDto:: -
.::- .
	Completed::. 7
,::7 8
CancellationReason;; "
=;;# $
null;;% )
,;;) *
CreatedDate<< 
=<< 
DateTime<< &
.<<& '
Today<<' ,
.<<, -
AddDays<<- 4
(<<4 5
-<<5 6
$num<<6 7
)<<7 8
}== 
,== 
new>> 
AppointmentDto>> 
{?? 
AppointmentId@@ 
=@@ 
$num@@  !
,@@! "
	PatientIdAA 
=AA 
$numAA 
,AA 
PatientNameBB 
=BB 
$strBB +
,BB+ ,
DoctorIdCC 
=CC 
$numCC 
,CC 

DoctorNameDD 
=DD 
$strDD )
,DD) *
ScheduledDateEE 
=EE 
DateTimeEE  (
.EE( )
TodayEE) .
.EE. /
AddDaysEE/ 6
(EE6 7
$numEE7 8
)EE8 9
,EE9 :
TimeSlotFF 
=FF 
$strFF 0
,FF0 1
StatusGG 
=GG  
AppointmentStatusDtoGG -
.GG- .
PendingGG. 5
,GG5 6
CancellationReasonHH "
=HH# $
nullHH% )
,HH) *
CreatedDateII 
=II 
DateTimeII &
.II& '
TodayII' ,
}JJ 
}KK 	
;KK	 

publicMM 
TaskMM 
<MM 
ListMM 
<MM 
AppointmentDtoMM '
>MM' (
>MM( )#
GetAllAppointmentsAsyncMM* A
(MMA B
)MMB C
{NN 	
varOO 
appointmentsOO 
=OO 
AppointmentsOO +
.PP 
OrderByDescendingPP "
(PP" #
aPP# $
=>PP% '
aPP( )
.PP) *
ScheduledDatePP* 7
)PP7 8
.QQ 
ThenByQQ 
(QQ 
aQQ 
=>QQ 
aQQ 
.QQ 
TimeSlotQQ '
)QQ' (
.RR 
ToListRR 
(RR 
)RR 
;RR 
returnTT 
TaskTT 
.TT 

FromResultTT "
(TT" #
appointmentsTT# /
)TT/ 0
;TT0 1
}UU 	
publicWW 
TaskWW 
<WW 
AppointmentDtoWW "
?WW" #
>WW# $#
GetAppointmentByIdAsyncWW% <
(WW< =
intWW= @
appointmentIdWWA N
)WWN O
{XX 	
varYY 
appointmentYY 
=YY 
AppointmentsYY *
.ZZ 
FirstOrDefaultZZ 
(ZZ  
aZZ  !
=>ZZ" $
aZZ% &
.ZZ& '
AppointmentIdZZ' 4
==ZZ5 7
appointmentIdZZ8 E
)ZZE F
;ZZF G
return\\ 
Task\\ 
.\\ 

FromResult\\ "
(\\" #
appointment\\# .
)\\. /
;\\/ 0
}]] 	
public__ 
Task__ 
<__ 
AppointmentDto__ "
>__" #"
CreateAppointmentAsync__$ :
(__: ; 
CreateAppointmentDto__; O
request__P W
)__W X
{`` 	
intaa 
nextIdaa 
=aa 
Appointmentsaa %
.aa% &
Anyaa& )
(aa) *
)aa* +
?bb 
Appointmentsbb 
.bb 
Maxbb "
(bb" #
abb# $
=>bb% '
abb( )
.bb) *
AppointmentIdbb* 7
)bb7 8
+bb9 :
$numbb; <
:cc 
$numcc 
;cc 
varee 
appointmentee 
=ee 
newee !
AppointmentDtoee" 0
{ff 
AppointmentIdgg 
=gg 
nextIdgg  &
,gg& '
	PatientIdhh 
=hh 
requesthh #
.hh# $
	PatientIdhh$ -
,hh- .
PatientNameii 
=ii 
GetPatientNameii ,
(ii, -
requestii- 4
.ii4 5
	PatientIdii5 >
)ii> ?
,ii? @
DoctorIdjj 
=jj 
requestjj "
.jj" #
DoctorIdjj# +
,jj+ ,

DoctorNamekk 
=kk 
GetDoctorNamekk *
(kk* +
requestkk+ 2
.kk2 3
DoctorIdkk3 ;
)kk; <
,kk< =
ScheduledDatell 
=ll 
requestll  '
.ll' (
ScheduledDatell( 5
.ll5 6
Datell6 :
,ll: ;
TimeSlotmm 
=mm 
requestmm "
.mm" #
TimeSlotmm# +
,mm+ ,
Statusnn 
=nn 
requestnn  
.nn  !
Statusnn! '
,nn' (
CancellationReasonoo "
=oo# $
requestoo% ,
.oo, -
Statusoo- 3
==oo4 6 
AppointmentStatusDtooo7 K
.ooK L
	CancelledooL U
?pp 
$strpp *
:qq 
nullqq 
,qq 
CreatedDaterr 
=rr 
DateTimerr &
.rr& '
Todayrr' ,
}ss 
;ss 
Appointmentsuu 
.uu 
Adduu 
(uu 
appointmentuu (
)uu( )
;uu) *
returnww 
Taskww 
.ww 

FromResultww "
(ww" #
appointmentww# .
)ww. /
;ww/ 0
}xx 	
publiczz 
Taskzz 
<zz 
AppointmentDtozz "
?zz" #
>zz# $"
UpdateAppointmentAsynczz% ;
(zz; <
intzz< ?
appointmentIdzz@ M
,zzM N 
UpdateAppointmentDtozzO c
requestzzd k
)zzk l
{{{ 	
var|| 
appointment|| 
=|| 
Appointments|| *
.}} 
FirstOrDefault}} 
(}}  
a}}  !
=>}}" $
a}}% &
.}}& '
AppointmentId}}' 4
==}}5 7
appointmentId}}8 E
)}}E F
;}}F G
if 
( 
appointment 
is 
null #
)# $
{
ÄÄ 
return
ÅÅ 
Task
ÅÅ 
.
ÅÅ 

FromResult
ÅÅ &
<
ÅÅ& '
AppointmentDto
ÅÅ' 5
?
ÅÅ5 6
>
ÅÅ6 7
(
ÅÅ7 8
null
ÅÅ8 <
)
ÅÅ< =
;
ÅÅ= >
}
ÇÇ 
appointment
ÑÑ 
.
ÑÑ 
	PatientId
ÑÑ !
=
ÑÑ" #
request
ÑÑ$ +
.
ÑÑ+ ,
	PatientId
ÑÑ, 5
;
ÑÑ5 6
appointment
ÖÖ 
.
ÖÖ 
PatientName
ÖÖ #
=
ÖÖ$ %
GetPatientName
ÖÖ& 4
(
ÖÖ4 5
request
ÖÖ5 <
.
ÖÖ< =
	PatientId
ÖÖ= F
)
ÖÖF G
;
ÖÖG H
appointment
ÜÜ 
.
ÜÜ 
DoctorId
ÜÜ  
=
ÜÜ! "
request
ÜÜ# *
.
ÜÜ* +
DoctorId
ÜÜ+ 3
;
ÜÜ3 4
appointment
áá 
.
áá 

DoctorName
áá "
=
áá# $
GetDoctorName
áá% 2
(
áá2 3
request
áá3 :
.
áá: ;
DoctorId
áá; C
)
ááC D
;
ááD E
appointment
àà 
.
àà 
ScheduledDate
àà %
=
àà& '
request
àà( /
.
àà/ 0
ScheduledDate
àà0 =
.
àà= >
Date
àà> B
;
ààB C
appointment
ââ 
.
ââ 
TimeSlot
ââ  
=
ââ! "
request
ââ# *
.
ââ* +
TimeSlot
ââ+ 3
;
ââ3 4
appointment
ää 
.
ää 
Status
ää 
=
ää  
request
ää! (
.
ää( )
Status
ää) /
;
ää/ 0
appointment
ãã 
.
ãã  
CancellationReason
ãã *
=
ãã+ ,
request
ãã- 4
.
ãã4 5
Status
ãã5 ;
==
ãã< >"
AppointmentStatusDto
ãã? S
.
ããS T
	Cancelled
ããT ]
?
åå 
request
åå 
.
åå  
CancellationReason
åå ,
:
çç 
null
çç 
;
çç 
return
èè 
Task
èè 
.
èè 

FromResult
èè "
<
èè" #
AppointmentDto
èè# 1
?
èè1 2
>
èè2 3
(
èè3 4
appointment
èè4 ?
)
èè? @
;
èè@ A
}
êê 	
public
íí 
Task
íí 
<
íí 
bool
íí 
>
íí $
DeleteAppointmentAsync
íí 0
(
íí0 1
int
íí1 4
appointmentId
íí5 B
)
ííB C
{
ìì 	
var
îî 
appointment
îî 
=
îî 
Appointments
îî *
.
ïï 
FirstOrDefault
ïï 
(
ïï  
a
ïï  !
=>
ïï" $
a
ïï% &
.
ïï& '
AppointmentId
ïï' 4
==
ïï5 7
appointmentId
ïï8 E
)
ïïE F
;
ïïF G
if
óó 
(
óó 
appointment
óó 
is
óó 
null
óó #
)
óó# $
{
òò 
return
ôô 
Task
ôô 
.
ôô 

FromResult
ôô &
(
ôô& '
false
ôô' ,
)
ôô, -
;
ôô- .
}
öö 
Appointments
úú 
.
úú 
Remove
úú 
(
úú  
appointment
úú  +
)
úú+ ,
;
úú, -
return
ûû 
Task
ûû 
.
ûû 

FromResult
ûû "
(
ûû" #
true
ûû# '
)
ûû' (
;
ûû( )
}
üü 	
private
°° 
static
°° 
string
°° 
GetPatientName
°° ,
(
°°, -
int
°°- 0
	patientId
°°1 :
)
°°: ;
{
¢¢ 	
return
££ 
	patientId
££ 
switch
££ #
{
§§ 
$num
•• 
=>
•• 
$str
•• !
,
••! "
$num
¶¶ 
=>
¶¶ 
$str
¶¶ "
,
¶¶" #
$num
ßß 
=>
ßß 
$str
ßß  
,
ßß  !
$num
®® 
=>
®® 
$str
®® (
,
®®( )
_
©© 
=>
©© 
$"
©© 
$str
©©  
{
©©  !
	patientId
©©! *
}
©©* +
"
©©+ ,
}
™™ 
;
™™ 
}
´´ 	
private
≠≠ 
static
≠≠ 
string
≠≠ 
GetDoctorName
≠≠ +
(
≠≠+ ,
int
≠≠, /
doctorId
≠≠0 8
)
≠≠8 9
{
ÆÆ 	
return
ØØ 
doctorId
ØØ 
switch
ØØ "
{
∞∞ 
$num
±± 
=>
±± 
$str
±± !
,
±±! "
$num
≤≤ 
=>
≤≤ 
$str
≤≤ !
,
≤≤! "
$num
≥≥ 
=>
≥≥ 
$str
≥≥ !
,
≥≥! "
$num
¥¥ 
=>
¥¥ 
$str
¥¥ #
,
¥¥# $
_
µµ 
=>
µµ 
$"
µµ 
$str
µµ 
{
µµ  
doctorId
µµ  (
}
µµ( )
"
µµ) *
}
∂∂ 
;
∂∂ 
}
∑∑ 	
}
∏∏ 
}ππ ã
tC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\AdminDashboardService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class !
AdminDashboardService &
:' ("
IAdminDashboardService) ?
{ 
public 
Task 
< #
AdminDashboardReportDto +
>+ ,#
GetDashboardReportAsync- D
(D E
)E F
{		 	
var

 
report

 
=

 
new

 #
AdminDashboardReportDto

 4
{ 
TotalDoctors 
= 
$num !
,! "
TotalPatients 
= 
$num  "
," #
TotalAppointments !
=" #
$num$ &
,& '
TodaysAppointments "
=# $
$num% &
,& '
TodaysPatients 
=  
$num! "
," #
CompletedToday 
=  
$num! "
," #
PendingToday 
= 
$num  
,  !
CancelledToday 
=  
$num! "
} 
; 
return 
Task 
. 

FromResult "
(" #
report# )
)) *
;* +
} 	
} 
} ï
XC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Program.cs
var 
builder 
= "
WebAssemblyHostBuilder $
.$ %
CreateDefault% 2
(2 3
args3 7
)7 8
;8 9
builder		 
.		 
RootComponents		 
.		 
Add		 
<		 
App		 
>		 
(		  
$str		  &
)		& '
;		' (
builder 
. 
RootComponents 
. 
Add 
< 

HeadOutlet %
>% &
(& '
$str' 4
)4 5
;5 6
builder 
. 
Services 
. 
	AddScoped 
( 
sp 
=>  
new 

HttpClient 
{ 
BaseAddress 
= 
new 
Uri 
( 
builder %
.% &
HostEnvironment& 5
.5 6
BaseAddress6 A
)A B
} 
) 
; 
builder 
. 
Services 
. 
	AddScoped 
< 
IAuthService '
,' (
AuthService) 4
>4 5
(5 6
)6 7
;7 8
builder 
. 
Services 
. 
	AddScoped 
< "
IAdminDashboardService 1
,1 2!
AdminDashboardService3 H
>H I
(I J
)J K
;K L
builder 
. 
Services 
. 
	AddScoped 
< 
IDoctorAdminService .
,. /
DoctorAdminService0 B
>B C
(C D
)D E
;E F
builder 
. 
Services 
. 
	AddScoped 
<  
IPatientAdminService /
,/ 0
PatientAdminService1 D
>D E
(E F
)F G
;G H
builder 
. 
Services 
. 
	AddScoped 
< $
IAppointmentAdminService 3
,3 4#
AppointmentAdminService5 L
>L M
(M N
)N O
;O P
await 
builder 
. 
Build 
( 
) 
. 
RunAsync 
( 
)  
;  !§
oC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Patients\UpdatePatientDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Patients) 1
{ 
public 

class 
UpdatePatientDto !
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
	MinLength	 
( 
$num 
, 
ErrorMessage "
=# $
$str% W
)W X
]X Y
public		 
string		 
FullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
string		/ 5
.		5 6
Empty		6 ;
;		; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
Phone	 
( 
ErrorMessage 
= 
$str B
)B C
]C D
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
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
} »
iC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Patients\PatientDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Patients) 1
{ 
public 

class 

PatientDto 
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
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
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} ﬁ
oC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Patients\CreatePatientDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Patients) 1
{ 
public 

class 
CreatePatientDto !
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
	MinLength	 
( 
$num 
, 
ErrorMessage "
=# $
$str% W
)W X
]X Y
public		 
string		 
FullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
string		/ 5
.		5 6
Empty		6 ;
;		; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
DateTime4 <
.< =
Today= B
.B C
AddYearsC K
(K L
-L M
$numM O
)O P
;P Q
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
Phone	 
( 
ErrorMessage 
= 
$str B
)B C
]C D
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
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
} ∫
mC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Doctors\UpdateDoctorDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Doctors) 0
{ 
public 

class 
UpdateDoctorDto  
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ;
); <
]< =
[ 	
	MinLength	 
( 
$num 
, 
ErrorMessage "
=# $
$str% V
)V W
]W X
public		 
string		 
FullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
string		/ 5
.		5 6
Empty		6 ;
;		; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! >
)> ?
]? @
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
string5 ;
.; <
Empty< A
;A B
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% T
)T U
]U V
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
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) S
)S T
]T U
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} »
gC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Doctors\DoctorDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Doctors) 0
{ 
public 

class 
	DoctorDto 
{ 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
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
public		 
string		 
Email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
=		* +
string		, 2
.		2 3
Empty		3 8
;		8 9
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
string5 ;
.; <
Empty< A
;A B
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} Í
mC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Doctors\CreateDoctorDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Doctors) 0
{ 
public 

class 
CreateDoctorDto  
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ;
); <
]< =
[ 	
	MinLength	 
( 
$num 
, 
ErrorMessage "
=# $
$str% V
)V W
]W X
public		 
string		 
FullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
string		/ 5
.		5 6
Empty		6 ;
;		; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! >
)> ?
]? @
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
string5 ;
.; <
Empty< A
;A B
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% T
)T U
]U V
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
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) S
)S T
]T U
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
true- 1
;1 2
} 
} ñ
wC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Dashboard\AdminDashboardReportDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
	Dashboard) 2
{ 
public 

class #
AdminDashboardReportDto (
{ 
public 
int 
TotalDoctors 
{  !
get" %
;% &
set' *
;* +
}, -
public 
int 
TotalPatients  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
int		 
TotalAppointments		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		1 2
public 
int 
TodaysAppointments %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TodaysPatients !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
CompletedToday !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
PendingToday 
{  !
get" %
;% &
set' *
;* +
}, -
public 
int 
CancelledToday !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ò
cC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Auth\LoginDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Auth) -
{ 
public 

class 
LoginDto 
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public		 
string		 
Email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
=		* +
string		, 2
.		2 3
Empty		3 8
;		8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 8
)8 9
]9 :
public 
string 
Password 
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
;; <
public 
bool 

RememberMe 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ÿ
jC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Auth\AuthResponseDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Auth) -
{ 
public 

class 
AuthResponseDto  
{ 
public 
bool 
	IsSuccess 
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
public		 
string		 
AccessToken		 !
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
string 
Role 
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
;7 8
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
} 
} ”
wC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Appointments\UpdateAppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Appointments) 5
{ 
public 

class  
UpdateAppointmentDto %
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 7
)7 8
]8 9
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ O
)O P
]P Q
public		 
int		 
	PatientId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ N
)N O
]O P
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
=  
$str! >
)> ?
]? @
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 9
)9 :
]: ;
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public  
AppointmentStatusDto #
Status$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} Ÿ
xC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Appointments\CreateAppointementDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Appointments) 5
{ 
public 

class  
CreateAppointmentDto %
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 7
)7 8
]8 9
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ O
)O P
]P Q
public		 
int		 
	PatientId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ N
)N O
]O P
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
=  
$str! >
)> ?
]? @
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
DateTime6 >
.> ?
Today? D
;D E
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 9
)9 :
]: ;
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public  
AppointmentStatusDto #
Status$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
=9 : 
AppointmentStatusDto; O
.O P
PendingP W
;W X
} 
} ◊
qC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Appointments\AppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Appointments) 5
{ 
public 

class 
AppointmentDto 
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
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public		 
string		 
PatientName		 !
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
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 

DoctorName  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
string1 7
.7 8
Empty8 =
;= >
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
TimeSlot 
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
public  
AppointmentStatusDto #
Status$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} £
xC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Dtos\Appointments\AppointementStatusDto.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Dtos$ (
.( )
Appointments) 5
{ 
public 

enum  
AppointmentStatusDto $
{ 
Pending 
, 
	Confirmed 
, 
	Completed 
, 
	Cancelled 
}		 
}

 