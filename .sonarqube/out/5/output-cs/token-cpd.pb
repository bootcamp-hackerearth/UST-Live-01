ˆ	
^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IUserService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IUserService !
{ 
Task 
< 
User 
? 
> 
GetByEmailAsync #
(# $
string$ *
email+ 0
)0 1
;1 2
Task		 
<		 
User		 
?		 
>		 "
GetByRefreshTokenAsync		 *
(		* +
string		+ 1
refreshToken		2 >
)		> ?
;		? @
Task 
< 
bool 
> 
EmailExistsAsync #
(# $
string$ *
email+ 0
)0 1
;1 2
Task 
CreateAsync 
( 
User 
user "
)" #
;# $
Task 
UpdateAsync 
( 
User 
user "
)" #
;# $
Task 
SaveChangesAsync 
( 
) 
;  
} 
} Œ
aC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IPatientService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IPatientService $
{ 
Task 
< 
IEnumerable 
< 

PatientDto #
># $
>$ %
GetAllAsync& 1
(1 2
)2 3
;3 4
Task		 
<		 

PatientDto		 
?		 
>		 
GetByIdAsync		 &
(		& '
int		' *
id		+ -
)		- .
;		. /
Task 
< 
IEnumerable 
< "
PatientSearchResultDto /
>/ 0
>0 1
SearchByNameAsync2 C
(C D
stringD J
nameK O
)O P
;P Q
Task 
< 

PatientDto 
> 
CreateAsync $
($ %
CreatePatientDto% 5
dto6 9
)9 :
;: ;
Task 
UpdateAsync 
( 
int 
id 
,  
UpdatePatientDto! 1
dto2 5
)5 6
;6 7
Task 
DeactivateAsync 
( 
int  
id! #
)# $
;$ %
Task 
ActivateAsync 
( 
int 
id !
)! "
;" #
} 
} å	
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IHealthRecordService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface  
IHealthRecordService )
{ 
Task 
< 
HealthRecordDto 
? 
> 
GetByIdAsync +
(+ ,
int, /
id0 2
)2 3
;3 4
Task		 
<		 
HealthRecordDto		 
?		 
>		 #
GetByAppointmentIdAsync		 6
(		6 7
int		7 :
appointmentId		; H
)		H I
;		I J
Task 
< 
HealthRecordDto 
> 
CreateAsync )
() *!
CreateHealthRecordDto* ?
dto@ C
)C D
;D E
Task 
UpdateAsync 
( 
int 
id 
,  !
UpdateHealthRecordDto! 6
dto7 :
): ;
;; <
} 
} œ
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IDoctorService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IDoctorService #
{ 
Task 
< 
IEnumerable 
< 
	DoctorDto "
>" #
># $
GetAllAsync% 0
(0 1
string1 7
?7 8
sortBy9 ?
,? @
intA D
?D E
specialisationF T
)T U
;U V
Task		 
<		 
IEnumerable		 
<		 
	DoctorDto		 "
>		" #
>		# $*
GetActiveBySpecialisationAsync		% C
(		C D
int		D G
specialisation		H V
)		V W
;		W X
Task 
< 
	DoctorDto 
? 
> 
GetByIdAsync %
(% &
int& )
id* ,
), -
;- .
Task 
< 
	DoctorDto 
> 
CreateAsync #
(# $
CreateDoctorDto$ 3
dto4 7
)7 8
;8 9
Task 
UpdateAsync 
( 
int 
id 
,  
UpdateDoctorDto! 0
dto1 4
)4 5
;5 6
Task 
< 
IEnumerable 
< 
int 
> 
>  
GetAvailabilityAsync 3
(3 4
int4 7
doctorId8 @
,@ A
DateOnlyB J
dateK O
)O P
;P Q
Task 
< #
DoctorCreationResultDto $
>$ %(
CreateDoctorWithAccountAsync& B
(B C
CreateDoctorDtoC R
dtoS V
)V W
;W X
Task 
ActivateAsync 
( 
int 
id !
)! "
;" #
Task 
DeactivateAsync 
( 
int  
id! #
)# $
;$ %
} 
} ´
^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IAuthService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IAuthService !
{ 
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
AuthResponseDto, ;
?; <
Data= A
)A B
>B C
RegisterAsyncD Q
(Q R
RegisterDtoR ]
request^ e
)e f
;f g
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
AuthResponseDto, ;
?; <
Data= A
)A B
>B C 
RegisterPatientAsyncD X
(X Y
RegisterPatientDtoY k
requestl s
)s t
;t u
Task

 
<

 
(

 
bool

 
Success

 
,

 
string

 "
Message

# *
,

* +
AuthResponseDto

, ;
?

; <
Data

= A
)

A B
>

B C

LoginAsync

D N
(

N O
LoginDto

O W
request

X _
)

_ `
;

` a
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
AuthResponseDto, ;
?; <
Data= A
)A B
>B C
RefreshTokenAsyncD U
(U V
RefreshTokenDtoV e
requestf m
)m n
;n o
} 
} ë
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IAppointmentService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IAppointmentService (
{ 
Task 
< 
IEnumerable 
< !
AppointmentDetailsDto .
>. /
>/ 0
GetAllAsync1 <
(< =
)= >
;> ?
Task		 
<		 !
AppointmentDetailsDto		 "
?		" #
>		# $
GetByIdAsync		% 1
(		1 2
int		2 5
id		6 8
)		8 9
;		9 :
Task 
< 
IEnumerable 
< (
PatientAppointmentHistoryDto 5
>5 6
>6 7"
GetPatientHistoryAsync8 N
(N O
intO R
	patientIdS \
)\ ]
;] ^
Task 
< 
IEnumerable 
< !
DoctorScheduleItemDto .
>. /
>/ 0'
GetDoctorTodayScheduleAsync1 L
(L M
intM P
doctorIdQ Y
)Y Z
;Z [
Task 
< 
IEnumerable 
< !
DoctorScheduleItemDto .
>. /
>/ 0&
GetDoctorWeekScheduleAsync1 K
(K L
int 
doctorId 
, 
DateOnly 
	startDate 
, 
DateOnly 
endDate 
) 
; 
Task 
< 
AppointmentDto 
> 
CreateAsync (
(( ) 
CreateAppointmentDto) =
dto> A
)A B
;B C
Task 
< 
IEnumerable 
< !
DoctorScheduleItemDto .
>. /
>/ 0*
GetDoctorUpcomingScheduleAsync1 O
(O P
intP S
doctorIdT \
)\ ]
;] ^
Task 
UpdateAsync 
( 
int 
id 
,   
UpdateAppointmentDto! 5
dto6 9
)9 :
;: ;
Task 
UpdateStatusAsync 
( 
int "
id# %
,% &&
UpdateAppointmentStatusDto' A
dtoB E
)E F
;F G
Task 
ConfirmAsync 
( 
int 
id  
)  !
;! "
Task 
CompleteAsync 
( 
int 
id !
)! "
;" #
Task 
CancelAsync 
( 
int 
id 
,   
CancelAppointmentDto! 5
dto6 9
)9 :
;: ;
}   
}!! ≠
_C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IAdminService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IAdminService "
{ 
Task 
< 
AdminDashboardDto 
> 
GetDashboardAsync  1
(1 2
)2 3
;3 4
Task		 
<		 
AdminStatisticsDto		 
>		  
GetStatisticsAsync		! 3
(		3 4
)		4 5
;		5 6
Task 
< 
IEnumerable 
< 
UserManagementDto *
>* +
>+ ,
GetUsersAsync- :
(: ;
); <
;< =
Task 
< 
UserManagementDto 
? 
>  
GetUserByIdAsync! 1
(1 2
int2 5
id6 8
)8 9
;9 :
} 
} ®
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\UserService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public 

class 
UserService 
: 
IUserService +
{ 
private		 
readonly		 
IUserRepository		 (
_userRepository		) 8
;		8 9
public 
UserService 
( 
IUserRepository 
userRepository *
)* +
{ 	
_userRepository 
= 
userRepository ,
;, -
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByEmailAsync! 0
(0 1
string 
email 
) 
{ 	
return 
await 
_userRepository (
. 
GetByEmailAsync  
(  !
email! &
)& '
;' (
} 	
public 
async 
Task 
< 
User 
? 
>  "
GetByRefreshTokenAsync! 7
(7 8
string 
refreshToken 
)  
{ 	
return 
await 
_userRepository (
. "
GetByRefreshTokenAsync '
(' (
refreshToken( 4
)4 5
;5 6
} 	
public 
async 
Task 
< 
bool 
> 
EmailExistsAsync  0
(0 1
string   
email   
)   
{!! 	
return"" 
await"" 
_userRepository"" (
.## 
EmailExistsAsync## !
(##! "
email##" '
)##' (
;##( )
}$$ 	
public&& 
async&& 
Task&& 
CreateAsync&& %
(&&% &
User'' 
user'' 
)'' 
{(( 	
await)) 
_userRepository)) !
.** 
AddAsync** 
(** 
user** 
)** 
;**  
}++ 	
public-- 
async-- 
Task-- 
UpdateAsync-- %
(--% &
User.. 
user.. 
).. 
{// 	
await00 
_userRepository00 !
.11 
UpdateAsync11 
(11 
user11 !
)11! "
;11" #
}22 	
public44 
async44 
Task44 
SaveChangesAsync44 *
(44* +
)44+ ,
{55 	
await66 
_userRepository66 !
.77 
SaveChangesAsync77 !
(77! "
)77" #
;77# $
}88 	
}99 
}:: Ær
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\PatientService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public 

class 
PatientService 
:  !
IPatientService" 1
{		 
private

 
readonly

 
IPatientRepository

 +
_patientRepository

, >
;

> ?
public 
PatientService 
( 
IPatientRepository 0
patientRepository1 B
)B C
{ 	
_patientRepository 
=  
patientRepository! 2
;2 3
} 	
public 
async 
Task 
< 
IEnumerable %
<% &

PatientDto& 0
>0 1
>1 2
GetAllAsync3 >
(> ?
)? @
{ 	
var 
patients 
= 
await  
_patientRepository! 3
.3 4
GetAllAsync4 ?
(? @
)@ A
;A B
return 
patients 
. 
Select "
(" #
MapToPatientDto# 2
)2 3
;3 4
} 	
public 
async 
Task 
< 

PatientDto $
?$ %
>% &
GetByIdAsync' 3
(3 4
int4 7
id8 :
): ;
{ 	
var 
patient 
= 
await 
_patientRepository  2
.2 3
GetByIdAsync3 ?
(? @
id@ B
)B C
;C D
return 
patient 
== 
null "
? 
null 
: 
MapToPatientDto !
(! "
patient" )
)) *
;* +
} 	
public!! 
async!! 
Task!! 
<!! 
IEnumerable!! %
<!!% &"
PatientSearchResultDto!!& <
>!!< =
>!!= >
SearchByNameAsync!!? P
(!!P Q
string!!Q W
name!!X \
)!!\ ]
{"" 	
var## 
patients## 
=## 
await##  
_patientRepository##! 3
.##3 4
SearchByNameAsync##4 E
(##E F
name##F J
)##J K
;##K L
return%% 
patients%% 
.%% 
Select%% "
(%%" #
p%%# $
=>%%% '
new%%( +"
PatientSearchResultDto%%, B
{&& 
	PatientId'' 
='' 
p'' 
.'' 
	PatientId'' '
,''' (
FullName(( 
=(( 
p(( 
.(( 
FullName(( %
,((% &
IsActive)) 
=)) 
p)) 
.)) 
IsActive)) %
}** 
)** 
;** 
}++ 	
public-- 
async-- 
Task-- 
<-- 

PatientDto-- $
>--$ %
CreateAsync--& 1
(--1 2
CreatePatientDto--2 B
dto--C F
)--F G
{.. 	
ValidatePatient// 
(// 
dto// 
)//  
;//  !
var11 
patient11 
=11 
new11 
Patient11 %
{22 
FullName33 
=33 
dto33 
.33 
FullName33 '
.33' (
Trim33( ,
(33, -
)33- .
,33. /
DateOfBirth44 
=44 
dto44 !
.44! "
DateOfBirth44" -
,44- .
Gender55 
=55 
dto55 
.55 
Gender55 #
,55# $
PhoneNumber66 
=66 
dto66 !
.66! "
PhoneNumber66" -
.66- .
Trim66. 2
(662 3
)663 4
,664 5
Email77 
=77 
dto77 
.77 
Email77 !
?77! "
.77" #
Trim77# '
(77' (
)77( )
??77) +
String77+ 1
.771 2
Empty772 7
,777 8
InsuranceNumber88 
=88  !
dto88" %
.88% &
InsuranceNumber88& 5
?885 6
.886 7
Trim887 ;
(88; <
)88< =
,88= >
IsActive99 
=99 
true99 
}:: 
;:: 
await<< 
_patientRepository<< $
.<<$ %
AddAsync<<% -
(<<- .
patient<<. 5
)<<5 6
;<<6 7
await== 
_patientRepository== $
.==$ %
SaveChangesAsync==% 5
(==5 6
)==6 7
;==7 8
return?? 
MapToPatientDto?? "
(??" #
patient??# *
)??* +
;??+ ,
}@@ 	
publicBB 
asyncBB 
TaskBB 
UpdateAsyncBB %
(BB% &
intBB& )
idBB* ,
,BB, -
UpdatePatientDtoBB. >
dtoBB? B
)BBB C
{CC 	
ValidatePatientDD 
(DD 
dtoDD 
)DD  
;DD  !
varFF 
patientFF 
=FF 
awaitFF 
_patientRepositoryFF  2
.FF2 3
GetByIdAsyncFF3 ?
(FF? @
idFF@ B
)FFB C
;FFC D
ifHH 
(HH 
patientHH 
==HH 
nullHH 
)HH  
throwII 
newII  
KeyNotFoundExceptionII .
(II. /
$"II/ 1
$strII1 A
{IIA B
idIIB D
}IID E
$strIIE P
"IIP Q
)IIQ R
;IIR S
patientKK 
.KK 
FullNameKK 
=KK 
dtoKK "
.KK" #
FullNameKK# +
.KK+ ,
TrimKK, 0
(KK0 1
)KK1 2
;KK2 3
patientLL 
.LL 
DateOfBirthLL 
=LL  !
dtoLL" %
.LL% &
DateOfBirthLL& 1
;LL1 2
patientMM 
.MM 
GenderMM 
=MM 
dtoMM  
.MM  !
GenderMM! '
;MM' (
patientNN 
.NN 
PhoneNumberNN 
=NN  !
dtoNN" %
.NN% &
PhoneNumberNN& 1
.NN1 2
TrimNN2 6
(NN6 7
)NN7 8
;NN8 9
patientOO 
.OO 
EmailOO 
=OO 
dtoOO 
.OO  
EmailOO  %
?OO% &
.OO& '
TrimOO' +
(OO+ ,
)OO, -
??OO. 0
StringOO1 7
.OO7 8
EmptyOO8 =
;OO= >
patientPP 
.PP 
InsuranceNumberPP #
=PP$ %
dtoPP& )
.PP) *
InsuranceNumberPP* 9
?PP9 :
.PP: ;
TrimPP; ?
(PP? @
)PP@ A
;PPA B
awaitRR 
_patientRepositoryRR $
.RR$ %
UpdateAsyncRR% 0
(RR0 1
patientRR1 8
)RR8 9
;RR9 :
awaitSS 
_patientRepositorySS $
.SS$ %
SaveChangesAsyncSS% 5
(SS5 6
)SS6 7
;SS7 8
}TT 	
publicVV 
asyncVV 
TaskVV 
DeactivateAsyncVV )
(VV) *
intVV* -
idVV. 0
)VV0 1
{WW 	
varXX 
patientXX 
=XX 
awaitXX 
_patientRepositoryXX  2
.XX2 3
GetByIdAsyncXX3 ?
(XX? @
idXX@ B
)XXB C
;XXC D
ifZZ 
(ZZ 
patientZZ 
==ZZ 
nullZZ 
)ZZ  
throw[[ 
new[[  
KeyNotFoundException[[ .
([[. /
$"[[/ 1
$str[[1 A
{[[A B
id[[B D
}[[D E
$str[[E P
"[[P Q
)[[Q R
;[[R S
patient]] 
.]] 
IsActive]] 
=]] 
false]] $
;]]$ %
await__ 
_patientRepository__ $
.__$ %
UpdateAsync__% 0
(__0 1
patient__1 8
)__8 9
;__9 :
await`` 
_patientRepository`` $
.``$ %
SaveChangesAsync``% 5
(``5 6
)``6 7
;``7 8
}aa 	
publiccc 
asynccc 
Taskcc 
ActivateAsynccc '
(cc' (
intcc( +
idcc, .
)cc. /
{dd 	
varee 
patientee 
=ee 
awaitee 
_patientRepositoryee  2
.ee2 3
GetByIdAsyncee3 ?
(ee? @
idee@ B
)eeB C
;eeC D
ifgg 
(gg 
patientgg 
==gg 
nullgg 
)gg  
throwhh 
newhh  
KeyNotFoundExceptionhh .
(hh. /
$"hh/ 1
$strhh1 A
{hhA B
idhhB D
}hhD E
$strhhE P
"hhP Q
)hhQ R
;hhR S
patientjj 
.jj 
IsActivejj 
=jj 
truejj #
;jj# $
awaitll 
_patientRepositoryll $
.ll$ %
UpdateAsyncll% 0
(ll0 1
patientll1 8
)ll8 9
;ll9 :
awaitmm 
_patientRepositorymm $
.mm$ %
SaveChangesAsyncmm% 5
(mm5 6
)mm6 7
;mm7 8
}nn 	
privatepp 
staticpp 
voidpp 
ValidatePatientpp +
(pp+ ,
CreatePatientDtopp, <
dtopp= @
)pp@ A
{qq 	
ifrr 
(rr 
stringrr 
.rr 
IsNullOrWhiteSpacerr )
(rr) *
dtorr* -
.rr- .
FullNamerr. 6
)rr6 7
)rr7 8
throwss 
newss 
ArgumentExceptionss +
(ss+ ,
$strss, G
)ssG H
;ssH I
ifuu 
(uu 
dtouu 
.uu 
DateOfBirthuu 
>uu  !
DateOnlyuu" *
.uu* +
FromDateTimeuu+ 7
(uu7 8
DateTimeuu8 @
.uu@ A
TodayuuA F
)uuF G
)uuG H
throwvv 
newvv 
ArgumentExceptionvv +
(vv+ ,
$strvv, T
)vvT U
;vvU V
ifxx 
(xx 
dtoxx 
.xx 
DateOfBirthxx 
<xx  !
DateOnlyxx" *
.xx* +
FromDateTimexx+ 7
(xx7 8
DateTimexx8 @
.xx@ A
TodayxxA F
.xxF G
AddYearsxxG O
(xxO P
-xxP Q
$numxxQ T
)xxT U
)xxU V
)xxV W
throwyy 
newyy 
ArgumentExceptionyy +
(yy+ ,
$stryy, D
)yyD E
;yyE F
if{{ 
({{ 
string{{ 
.{{ 
IsNullOrWhiteSpace{{ )
({{) *
dto{{* -
.{{- .
PhoneNumber{{. 9
){{9 :
){{: ;
throw|| 
new|| 
ArgumentException|| +
(||+ ,
$str||, G
)||G H
;||H I
}}} 	
private 
static 
void 
ValidatePatient +
(+ ,
UpdatePatientDto, <
dto= @
)@ A
{
ÄÄ 	
if
ÅÅ 
(
ÅÅ 
string
ÅÅ 
.
ÅÅ  
IsNullOrWhiteSpace
ÅÅ )
(
ÅÅ) *
dto
ÅÅ* -
.
ÅÅ- .
FullName
ÅÅ. 6
)
ÅÅ6 7
)
ÅÅ7 8
throw
ÇÇ 
new
ÇÇ 
ArgumentException
ÇÇ +
(
ÇÇ+ ,
$str
ÇÇ, G
)
ÇÇG H
;
ÇÇH I
if
ÑÑ 
(
ÑÑ 
dto
ÑÑ 
.
ÑÑ 
DateOfBirth
ÑÑ 
>
ÑÑ  !
DateOnly
ÑÑ" *
.
ÑÑ* +
FromDateTime
ÑÑ+ 7
(
ÑÑ7 8
DateTime
ÑÑ8 @
.
ÑÑ@ A
Today
ÑÑA F
)
ÑÑF G
)
ÑÑG H
throw
ÖÖ 
new
ÖÖ 
ArgumentException
ÖÖ +
(
ÖÖ+ ,
$str
ÖÖ, T
)
ÖÖT U
;
ÖÖU V
if
áá 
(
áá 
dto
áá 
.
áá 
DateOfBirth
áá 
<
áá  !
DateOnly
áá" *
.
áá* +
FromDateTime
áá+ 7
(
áá7 8
DateTime
áá8 @
.
áá@ A
Today
ááA F
.
ááF G
AddYears
ááG O
(
ááO P
-
ááP Q
$num
ááQ T
)
ááT U
)
ááU V
)
ááV W
throw
àà 
new
àà 
ArgumentException
àà +
(
àà+ ,
$str
àà, D
)
ààD E
;
ààE F
if
ää 
(
ää 
string
ää 
.
ää  
IsNullOrWhiteSpace
ää )
(
ää) *
dto
ää* -
.
ää- .
PhoneNumber
ää. 9
)
ää9 :
)
ää: ;
throw
ãã 
new
ãã 
ArgumentException
ãã +
(
ãã+ ,
$str
ãã, G
)
ããG H
;
ããH I
}
åå 	
private
éé 
static
éé 

PatientDto
éé !
MapToPatientDto
éé" 1
(
éé1 2
Patient
éé2 9
patient
éé: A
)
ééA B
{
èè 	
return
êê 
new
êê 

PatientDto
êê !
{
ëë 
	PatientId
íí 
=
íí 
patient
íí #
.
íí# $
	PatientId
íí$ -
,
íí- .
FullName
ìì 
=
ìì 
patient
ìì "
.
ìì" #
FullName
ìì# +
,
ìì+ ,
DateOfBirth
îî 
=
îî 
patient
îî %
.
îî% &
DateOfBirth
îî& 1
,
îî1 2
Gender
ïï 
=
ïï 
patient
ïï  
.
ïï  !
Gender
ïï! '
,
ïï' (
PhoneNumber
ññ 
=
ññ 
patient
ññ %
.
ññ% &
PhoneNumber
ññ& 1
,
ññ1 2
Email
óó 
=
óó 
patient
óó 
.
óó  
Email
óó  %
,
óó% &
InsuranceId
òò 
=
òò 
patient
òò %
.
òò% &
InsuranceNumber
òò& 5
,
òò5 6
IsActive
ôô 
=
ôô 
patient
ôô "
.
ôô" #
IsActive
ôô# +
}
öö 
;
öö 
}
õõ 	
}
úú 
}ùù ém
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\HealthRecordService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public		 

class		 
HealthRecordService		 $
:		% & 
IHealthRecordService		' ;
{

 
private 
readonly #
IHealthRecordRepository 0#
_healthRecordRepository1 H
;H I
private 
readonly "
IAppointmentRepository /"
_appointmentRepository0 F
;F G
private 
readonly 
IPatientRepository +
_patientRepository, >
;> ?
private 
readonly 
IDoctorRepository *
_doctorRepository+ <
;< =
public 
HealthRecordService "
(" ##
IHealthRecordRepository #"
healthRecordRepository$ :
,: ;"
IAppointmentRepository "!
appointmentRepository# 8
,8 9
IPatientRepository 
patientRepository 0
,0 1
IDoctorRepository 
doctorRepository .
). /
{ 	#
_healthRecordRepository #
=$ %"
healthRecordRepository& <
;< ="
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
_patientRepository 
=  
patientRepository! 2
;2 3
_doctorRepository 
= 
doctorRepository  0
;0 1
} 	
public 
async 
Task 
< 
HealthRecordDto )
?) *
>* +
GetByIdAsync, 8
(8 9
int9 <
id= ?
)? @
{ 	
var 
record 
= 
await #
_healthRecordRepository 6
.6 7
GetByIdAsync7 C
(C D
idD F
)F G
;G H
return   
record   
==   
null   !
?!! 
null!! 
:"" 
MapToDto"" 
("" 
record"" !
)""! "
;""" #
}## 	
public%% 
async%% 
Task%% 
<%% 
HealthRecordDto%% )
?%%) *
>%%* +#
GetByAppointmentIdAsync%%, C
(%%C D
int%%D G
appointmentId%%H U
)%%U V
{&& 	
var'' 
record'' 
='' 
await(( #
_healthRecordRepository(( -
.((- .#
GetByAppointmentIdAsync((. E
(((E F
appointmentId((F S
)((S T
;((T U
return** 
record** 
==** 
null** !
?++ 
null++ 
:,, 
MapToDto,, 
(,, 
record,, !
),,! "
;,," #
}-- 	
public// 
async// 
Task// 
<// 
HealthRecordDto// )
>//) *
CreateAsync//+ 6
(//6 7!
CreateHealthRecordDto//7 L
dto//M P
)//P Q
{00 	
ValidateCreateDto11 
(11 
dto11 !
)11! "
;11" #
var33 
appointment33 
=33 
await44 "
_appointmentRepository44 ,
.44, -
GetByIdAsync44- 9
(449 :
dto44: =
.44= >
AppointmentId44> K
)44K L
;44L M
if66 
(66 
appointment66 
==66 
null66 #
)66# $
throw77 
new77  
KeyNotFoundException77 .
(77. /
$str77/ G
)77G H
;77H I
if99 
(99 
appointment99 
.99 
Status99 "
!=99# %
AppointmentStatus99& 7
.997 8
	Completed998 A
)99A B
{:: 
throw;; 
new;; %
InvalidOperationException;; 3
(;;3 4
$str<< S
)<<S T
;<<T U
}== 
var?? 
existingRecord?? 
=??  
await@@ #
_healthRecordRepository@@ -
.@@- .#
GetByAppointmentIdAsync@@. E
(@@E F
dtoAA 
.AA 
AppointmentIdAA %
)AA% &
;AA& '
ifCC 
(CC 
existingRecordCC 
!=CC !
nullCC" &
)CC& '
{DD 
throwEE 
newEE %
InvalidOperationExceptionEE 3
(EE3 4
$strFF J
)FFJ K
;FFK L
}GG 
varII 
patientII 
=II 
awaitJJ 
_patientRepositoryJJ (
.JJ( )
GetByIdAsyncJJ) 5
(JJ5 6
dtoJJ6 9
.JJ9 :
	PatientIdJJ: C
)JJC D
;JJD E
ifLL 
(LL 
patientLL 
==LL 
nullLL 
)LL  
throwMM 
newMM  
KeyNotFoundExceptionMM .
(MM. /
$strMM/ C
)MMC D
;MMD E
varOO 
doctorOO 
=OO 
awaitPP 
_doctorRepositoryPP '
.PP' (
GetByIdAsyncPP( 4
(PP4 5
dtoPP5 8
.PP8 9
DoctorIdPP9 A
)PPA B
;PPB C
ifRR 
(RR 
doctorRR 
==RR 
nullRR 
)RR 
throwSS 
newSS  
KeyNotFoundExceptionSS .
(SS. /
$strSS/ B
)SSB C
;SSC D
ifUU 
(UU 
appointmentUU 
.UU 
	PatientIdUU %
!=UU& (
dtoUU) ,
.UU, -
	PatientIdUU- 6
)UU6 7
{VV 
throwWW 
newWW %
InvalidOperationExceptionWW 3
(WW3 4
$strXX 9
)XX9 :
;XX: ;
}YY 
if[[ 
([[ 
appointment[[ 
.[[ 
DoctorId[[ $
!=[[% '
dto[[( +
.[[+ ,
DoctorId[[, 4
)[[4 5
{\\ 
throw]] 
new]] %
InvalidOperationException]] 3
(]]3 4
$str^^ 8
)^^8 9
;^^9 :
}__ 
varaa 
recordaa 
=aa 
newaa 
HealthRecordaa )
{bb 
AppointmentIdcc 
=cc 
dtocc  #
.cc# $
AppointmentIdcc$ 1
,cc1 2
	PatientIddd 
=dd 
dtodd 
.dd  
	PatientIddd  )
,dd) *
DoctorIdee 
=ee 
dtoee 
.ee 
DoctorIdee '
,ee' (
	Diagnosisff 
=ff 
dtoff 
.ff  
	Diagnosisff  )
!ff) *
.ff* +
Trimff+ /
(ff/ 0
)ff0 1
,ff1 2
Prescriptiongg 
=gg 
dtogg "
.gg" #
Prescriptiongg# /
!gg/ 0
.gg0 1
Trimgg1 5
(gg5 6
)gg6 7
,gg7 8
Noteshh 
=hh 
dtohh 
.hh 
Noteshh !
?hh! "
.hh" #
Trimhh# '
(hh' (
)hh( )
,hh) *
	CreatedOnii 
=ii 
DateTimeii $
.ii$ %
UtcNowii% +
}jj 
;jj 
awaitll #
_healthRecordRepositoryll )
.ll) *
AddAsyncll* 2
(ll2 3
recordll3 9
)ll9 :
;ll: ;
awaitmm #
_healthRecordRepositorymm )
.mm) *
SaveChangesAsyncmm* :
(mm: ;
)mm; <
;mm< =
returnoo 
MapToDtooo 
(oo 
recordoo "
)oo" #
;oo# $
}pp 	
publicrr 
asyncrr 
Taskrr 
UpdateAsyncrr %
(rr% &
intss 
idss 
,ss !
UpdateHealthRecordDtott !
dtott" %
)tt% &
{uu 	
ValidateUpdateDtovv 
(vv 
dtovv !
)vv! "
;vv" #
varxx 
recordxx 
=xx 
awaityy #
_healthRecordRepositoryyy -
.yy- .
GetByIdAsyncyy. :
(yy: ;
idyy; =
)yy= >
;yy> ?
if{{ 
({{ 
record{{ 
=={{ 
null{{ 
){{ 
throw|| 
new||  
KeyNotFoundException|| .
(||. /
$"}} 
$str}} $
{}}$ %
id}}% '
}}}' (
$str}}( 3
"}}3 4
)}}4 5
;}}5 6
record 
. 
	Diagnosis 
= 
dto "
." #
	Diagnosis# ,
!, -
.- .
Trim. 2
(2 3
)3 4
;4 5
record
ÄÄ 
.
ÄÄ 
Prescription
ÄÄ 
=
ÄÄ  !
dto
ÄÄ" %
.
ÄÄ% &
Prescription
ÄÄ& 2
!
ÄÄ2 3
.
ÄÄ3 4
Trim
ÄÄ4 8
(
ÄÄ8 9
)
ÄÄ9 :
;
ÄÄ: ;
record
ÅÅ 
.
ÅÅ 
Notes
ÅÅ 
=
ÅÅ 
dto
ÅÅ 
.
ÅÅ 
Notes
ÅÅ $
?
ÅÅ$ %
.
ÅÅ% &
Trim
ÅÅ& *
(
ÅÅ* +
)
ÅÅ+ ,
;
ÅÅ, -
await
ÉÉ %
_healthRecordRepository
ÉÉ )
.
ÉÉ) *
UpdateAsync
ÉÉ* 5
(
ÉÉ5 6
record
ÉÉ6 <
)
ÉÉ< =
;
ÉÉ= >
await
ÑÑ %
_healthRecordRepository
ÑÑ )
.
ÑÑ) *
SaveChangesAsync
ÑÑ* :
(
ÑÑ: ;
)
ÑÑ; <
;
ÑÑ< =
}
ÖÖ 	
private
áá 
static
áá 
void
áá 
ValidateCreateDto
áá -
(
áá- .#
CreateHealthRecordDto
àà !
dto
àà" %
)
àà% &
{
ââ 	
if
ää 
(
ää 
dto
ää 
.
ää 
AppointmentId
ää !
<=
ää" $
$num
ää% &
)
ää& '
throw
ãã 
new
ãã 
ArgumentException
ãã +
(
ãã+ ,
$str
åå 0
)
åå0 1
;
åå1 2
if
éé 
(
éé 
dto
éé 
.
éé 
	PatientId
éé 
<=
éé  
$num
éé! "
)
éé" #
throw
èè 
new
èè 
ArgumentException
èè +
(
èè+ ,
$str
êê ,
)
êê, -
;
êê- .
if
íí 
(
íí 
dto
íí 
.
íí 
DoctorId
íí 
<=
íí 
$num
íí  !
)
íí! "
throw
ìì 
new
ìì 
ArgumentException
ìì +
(
ìì+ ,
$str
îî +
)
îî+ ,
;
îî, -
if
ññ 
(
ññ 
string
ññ 
.
ññ  
IsNullOrWhiteSpace
ññ )
(
ññ) *
dto
ññ* -
.
ññ- .
	Diagnosis
ññ. 7
)
ññ7 8
)
ññ8 9
throw
óó 
new
óó 
ArgumentException
óó +
(
óó+ ,
$str
òò ,
)
òò, -
;
òò- .
if
öö 
(
öö 
string
öö 
.
öö  
IsNullOrWhiteSpace
öö )
(
öö) *
dto
öö* -
.
öö- .
Prescription
öö. :
)
öö: ;
)
öö; <
throw
õõ 
new
õõ 
ArgumentException
õõ +
(
õõ+ ,
$str
úú /
)
úú/ 0
;
úú0 1
}
ùù 	
private
üü 
static
üü 
void
üü 
ValidateUpdateDto
üü -
(
üü- .#
UpdateHealthRecordDto
†† !
dto
††" %
)
††% &
{
°° 	
if
¢¢ 
(
¢¢ 
string
¢¢ 
.
¢¢  
IsNullOrWhiteSpace
¢¢ )
(
¢¢) *
dto
¢¢* -
.
¢¢- .
	Diagnosis
¢¢. 7
)
¢¢7 8
)
¢¢8 9
throw
££ 
new
££ 
ArgumentException
££ +
(
££+ ,
$str
§§ ,
)
§§, -
;
§§- .
if
¶¶ 
(
¶¶ 
string
¶¶ 
.
¶¶  
IsNullOrWhiteSpace
¶¶ )
(
¶¶) *
dto
¶¶* -
.
¶¶- .
Prescription
¶¶. :
)
¶¶: ;
)
¶¶; <
throw
ßß 
new
ßß 
ArgumentException
ßß +
(
ßß+ ,
$str
®® /
)
®®/ 0
;
®®0 1
}
©© 	
private
´´ 
static
´´ 
HealthRecordDto
´´ &
MapToDto
´´' /
(
´´/ 0
HealthRecord
¨¨ 
record
¨¨ 
)
¨¨  
{
≠≠ 	
return
ÆÆ 
new
ÆÆ 
HealthRecordDto
ÆÆ &
{
ØØ 
HealthRecordId
∞∞ 
=
∞∞  
record
∞∞! '
.
∞∞' (
HealthRecordId
∞∞( 6
,
∞∞6 7
AppointmentId
±± 
=
±± 
record
±±  &
.
±±& '
AppointmentId
±±' 4
,
±±4 5
	PatientId
≤≤ 
=
≤≤ 
record
≤≤ "
.
≤≤" #
	PatientId
≤≤# ,
,
≤≤, -
DoctorId
≥≥ 
=
≥≥ 
record
≥≥ !
.
≥≥! "
DoctorId
≥≥" *
,
≥≥* +
	Diagnosis
¥¥ 
=
¥¥ 
record
¥¥ "
.
¥¥" #
	Diagnosis
¥¥# ,
,
¥¥, -
Prescription
µµ 
=
µµ 
record
µµ %
.
µµ% &
Prescription
µµ& 2
,
µµ2 3
Notes
∂∂ 
=
∂∂ 
record
∂∂ 
.
∂∂ 
Notes
∂∂ $
}
∑∑ 
;
∑∑ 
}
∏∏ 	
}
ππ 
}∫∫ €´
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\DoctorService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public		 

class		 
DoctorService		 
:		  
IDoctorService		! /
{

 
private 
readonly 
IDoctorRepository *
_doctorRepository+ <
;< =
private 
readonly 
IUserService %
_userService& 2
;2 3
public 
DoctorService 
( 
IDoctorRepository 
doctorRepository .
,. /
IUserService 
userService $
)$ %
{ 	
_doctorRepository 
= 
doctorRepository  0
;0 1
_userService 
= 
userService &
;& '
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
	DoctorDto& /
>/ 0
>0 1
GetAllAsync2 =
(= >
string> D
?D E
sortByF L
,L M
intN Q
?Q R
specialisationS a
)a b
{ 	
var 
doctors 
= 
await 
_doctorRepository  1
.1 2
GetAllAsync2 =
(= >
sortBy> D
,D E
specialisationF T
)T U
;U V
return 
doctors 
. 
Select !
(! "
MapToDoctorDto" 0
)0 1
;1 2
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
	DoctorDto& /
>/ 0
>0 1*
GetActiveBySpecialisationAsync2 P
(P Q
intQ T
specialisationU c
)c d
{ 	
if 
( 
! 
Enum 
. 
	IsDefined 
(  
typeof  &
(& ' 
DoctorSpecialisation' ;
); <
,< =
specialisation> L
)L M
)M N
throw   
new   
ArgumentException   +
(  + ,
$str  , L
)  L M
;  M N
var"" 
doctors"" 
="" 
await"" 
_doctorRepository""  1
.""1 2*
GetActiveBySpecialisationAsync""2 P
(""P Q
specialisation""Q _
)""_ `
;""` a
return$$ 
doctors$$ 
.$$ 
Select$$ !
($$! "
MapToDoctorDto$$" 0
)$$0 1
;$$1 2
}%% 	
public'' 
async'' 
Task'' 
<'' 
	DoctorDto'' #
?''# $
>''$ %
GetByIdAsync''& 2
(''2 3
int''3 6
id''7 9
)''9 :
{(( 	
var)) 
doctor)) 
=)) 
await)) 
_doctorRepository)) 0
.))0 1
GetByIdAsync))1 =
())= >
id))> @
)))@ A
;))A B
return++ 
doctor++ 
==++ 
null++ !
?,, 
null,, 
:-- 
MapToDoctorDto--  
(--  !
doctor--! '
)--' (
;--( )
}.. 	
public00 
async00 
Task00 
<00 
	DoctorDto00 #
>00# $
CreateAsync00% 0
(000 1
CreateDoctorDto001 @
dto00A D
)00D E
{11 	
ValidateDoctor22 
(22 
dto22 
)22 
;22  
var44 
doctor44 
=44 
new44 
Doctor44 #
{55 
FullName66 
=66 
dto66 
.66 
FullName66 '
.66' (
Trim66( ,
(66, -
)66- .
,66. /
Email77 
=77 
dto77 
.77 
Email77 !
.77! "
Trim77" &
(77& '
)77' (
.77( )
ToLower77) 0
(770 1
)771 2
,772 3
Specialisation88 
=88  
(88! " 
DoctorSpecialisation88" 6
)886 7
dto887 :
.88: ;
Specialisation88; I
,88I J
YearsOfExperience99 !
=99" #
dto99$ '
.99' (
YearsOfExperience99( 9
,999 :
ConsultationFee:: 
=::  !
dto::" %
.::% &
ConsultationFee::& 5
,::5 6
IsActive;; 
=;; 
true;; 
}<< 
;<< 
await>> 
_doctorRepository>> #
.>># $
AddAsync>>$ ,
(>>, -
doctor>>- 3
)>>3 4
;>>4 5
await?? 
_doctorRepository?? #
.??# $
SaveChangesAsync??$ 4
(??4 5
)??5 6
;??6 7
returnAA 
MapToDoctorDtoAA !
(AA! "
doctorAA" (
)AA( )
;AA) *
}BB 	
publicDD 
asyncDD 
TaskDD 
UpdateAsyncDD %
(DD% &
intDD& )
idDD* ,
,DD, -
UpdateDoctorDtoDD. =
dtoDD> A
)DDA B
{EE 	
ValidateDoctorFF 
(FF 
dtoFF 
)FF 
;FF  
varHH 
doctorHH 
=HH 
awaitHH 
_doctorRepositoryHH 0
.HH0 1
GetByIdAsyncHH1 =
(HH= >
idHH> @
)HH@ A
;HHA B
ifJJ 
(JJ 
doctorJJ 
==JJ 
nullJJ 
)JJ 
throwKK 
newKK  
KeyNotFoundExceptionKK .
(KK. /
$"KK/ 1
$strKK1 @
{KK@ A
idKKA C
}KKC D
$strKKD O
"KKO P
)KKP Q
;KKQ R
doctorMM 
.MM 
FullNameMM 
=MM 
dtoMM !
.MM! "
FullNameMM" *
.MM* +
TrimMM+ /
(MM/ 0
)MM0 1
;MM1 2
doctorNN 
.NN 
SpecialisationNN !
=NN" #
(NN$ % 
DoctorSpecialisationNN% 9
)NN9 :
dtoNN: =
.NN= >
SpecialisationNN> L
;NNL M
doctorOO 
.OO 
YearsOfExperienceOO $
=OO% &
dtoOO' *
.OO* +
YearsOfExperienceOO+ <
;OO< =
doctorPP 
.PP 
ConsultationFeePP "
=PP# $
dtoPP% (
.PP( )
ConsultationFeePP) 8
;PP8 9
awaitRR 
_doctorRepositoryRR #
.RR# $
UpdateAsyncRR$ /
(RR/ 0
doctorRR0 6
)RR6 7
;RR7 8
awaitSS 
_doctorRepositorySS #
.SS# $
SaveChangesAsyncSS$ 4
(SS4 5
)SS5 6
;SS6 7
}TT 	
publicVV 
asyncVV 
TaskVV 
<VV 
IEnumerableVV %
<VV% &
intVV& )
>VV) *
>VV* + 
GetAvailabilityAsyncVV, @
(VV@ A
intVVA D
doctorIdVVE M
,VVM N
DateOnlyVVO W
dateVVX \
)VV\ ]
{WW 	
varXX 
doctorXX 
=XX 
awaitYY 
_doctorRepositoryYY '
.YY' (
GetByIdAsyncYY( 4
(YY4 5
doctorIdYY5 =
)YY= >
;YY> ?
if[[ 
([[ 
doctor[[ 
==[[ 
null[[ 
)[[ 
throw\\ 
new\\  
KeyNotFoundException\\ .
(\\. /
$str]] '
)]]' (
;]]( )
var__ 
bookedSlots__ 
=__ 
await`` 
_doctorRepository`` '
.``' (
GetBookedSlotsAsync``( ;
(``; <
doctorIdaa 
,aa 
datebb 
)bb 
;bb 
vardd 
allSlotsdd 
=dd 
Enumee 
.ee 
	GetValuesee 
<ee 
AppointmentTimeSlotee 2
>ee2 3
(ee3 4
)ee4 5
.ff 
Selectff 
(ff 
xff 
=>ff  
(ff! "
intff" %
)ff% &
xff& '
)ff' (
;ff( )
returnhh 
allSlotshh 
.hh 
Excepthh "
(hh" #
bookedSlotshh# .
)hh. /
;hh/ 0
}ii 	
publickk 
asynckk 
Taskkk 
<kk #
DoctorCreationResultDtokk 1
>kk1 2(
CreateDoctorWithAccountAsynckk3 O
(kkO P
CreateDoctorDtokkP _
dtokk` c
)kkc d
{ll 	
ValidateDoctormm 
(mm 
dtomm 
)mm 
;mm  
ifoo 
(oo 
awaitoo 
_userServiceoo "
.oo" #
EmailExistsAsyncoo# 3
(oo3 4
dtooo4 7
.oo7 8
Emailoo8 =
)oo= >
)oo> ?
{pp 
throwqq 
newqq 
ArgumentExceptionqq +
(qq+ ,
$strqq, C
)qqC D
;qqD E
}rr 
vartt 
doctortt 
=tt 
newtt 
Doctortt #
{uu 
FullNamevv 
=vv 
dtovv 
.vv 
FullNamevv '
.vv' (
Trimvv( ,
(vv, -
)vv- .
,vv. /
Emailww 
=ww 
dtoww 
.ww 
Emailww !
.ww! "
Trimww" &
(ww& '
)ww' (
.ww( )
ToLowerww) 0
(ww0 1
)ww1 2
,ww2 3
Specialisationxx 
=xx  
(xx! " 
DoctorSpecialisationxx" 6
)xx6 7
dtoxx7 :
.xx: ;
Specialisationxx; I
,xxI J
YearsOfExperienceyy !
=yy" #
dtoyy$ '
.yy' (
YearsOfExperienceyy( 9
,yy9 :
ConsultationFeezz 
=zz  !
dtozz" %
.zz% &
ConsultationFeezz& 5
,zz5 6
IsActive{{ 
={{ 
true{{ 
}|| 
;|| 
await~~ 
_doctorRepository~~ #
.~~# $
AddAsync~~$ ,
(~~, -
doctor~~- 3
)~~3 4
;~~4 5
await 
_doctorRepository #
.# $
SaveChangesAsync$ 4
(4 5
)5 6
;6 7
var
ÅÅ 
temporaryPassword
ÅÅ !
=
ÅÅ" #'
GenerateTemporaryPassword
ÅÅ$ =
(
ÅÅ= >
)
ÅÅ> ?
;
ÅÅ? @
var
ÉÉ 
user
ÉÉ 
=
ÉÉ 
new
ÉÉ 
User
ÉÉ 
{
ÑÑ 
Email
ÖÖ 
=
ÖÖ 
doctor
ÖÖ 
.
ÖÖ 
Email
ÖÖ $
,
ÖÖ$ %
PasswordHash
ÜÜ 
=
ÜÜ 
HashPassword
ÜÜ +
(
ÜÜ+ ,
temporaryPassword
ÜÜ, =
)
ÜÜ= >
,
ÜÜ> ?
Role
áá 
=
áá 
UserRole
áá 
.
áá  
Doctor
áá  &
,
áá& '
ReferenceId
àà 
=
àà 
doctor
àà $
.
àà$ %
DoctorId
àà% -
,
àà- .
CreatedDate
ââ 
=
ââ 
DateTime
ââ &
.
ââ& '
UtcNow
ââ' -
}
ää 
;
ää 
await
åå 
_userService
åå 
.
åå 
CreateAsync
åå *
(
åå* +
user
åå+ /
)
åå/ 0
;
åå0 1
await
çç 
_userService
çç 
.
çç 
SaveChangesAsync
çç /
(
çç/ 0
)
çç0 1
;
çç1 2
return
èè 
new
èè %
DoctorCreationResultDto
èè .
{
êê 
DoctorId
ëë 
=
ëë 
doctor
ëë !
.
ëë! "
DoctorId
ëë" *
,
ëë* +
FullName
íí 
=
íí 
doctor
íí !
.
íí! "
FullName
íí" *
,
íí* +
Email
ìì 
=
ìì 
doctor
ìì 
.
ìì 
Email
ìì $
,
ìì$ %
TemporaryPassword
îî !
=
îî" #
temporaryPassword
îî$ 5
}
ïï 
;
ïï 
}
ññ 	
public
òò 
async
òò 
Task
òò 
ActivateAsync
òò '
(
òò' (
int
òò( +
id
òò, .
)
òò. /
{
ôô 	
var
öö 
doctor
öö 
=
öö 
await
öö 
_doctorRepository
öö 0
.
öö0 1
GetByIdAsync
öö1 =
(
öö= >
id
öö> @
)
öö@ A
;
ööA B
if
úú 
(
úú 
doctor
úú 
==
úú 
null
úú 
)
úú 
throw
ùù 
new
ùù "
KeyNotFoundException
ùù .
(
ùù. /
$"
ùù/ 1
$str
ùù1 @
{
ùù@ A
id
ùùA C
}
ùùC D
$str
ùùD O
"
ùùO P
)
ùùP Q
;
ùùQ R
doctor
üü 
.
üü 
IsActive
üü 
=
üü 
true
üü "
;
üü" #
await
°° 
_doctorRepository
°° #
.
°°# $
UpdateAsync
°°$ /
(
°°/ 0
doctor
°°0 6
)
°°6 7
;
°°7 8
await
¢¢ 
_doctorRepository
¢¢ #
.
¢¢# $
SaveChangesAsync
¢¢$ 4
(
¢¢4 5
)
¢¢5 6
;
¢¢6 7
}
££ 	
public
•• 
async
•• 
Task
•• 
DeactivateAsync
•• )
(
••) *
int
••* -
id
••. 0
)
••0 1
{
¶¶ 	
var
ßß 
doctor
ßß 
=
ßß 
await
ßß 
_doctorRepository
ßß 0
.
ßß0 1
GetByIdAsync
ßß1 =
(
ßß= >
id
ßß> @
)
ßß@ A
;
ßßA B
if
©© 
(
©© 
doctor
©© 
==
©© 
null
©© 
)
©© 
throw
™™ 
new
™™ "
KeyNotFoundException
™™ .
(
™™. /
$"
™™/ 1
$str
™™1 @
{
™™@ A
id
™™A C
}
™™C D
$str
™™D O
"
™™O P
)
™™P Q
;
™™Q R
doctor
¨¨ 
.
¨¨ 
IsActive
¨¨ 
=
¨¨ 
false
¨¨ #
;
¨¨# $
await
ÆÆ 
_doctorRepository
ÆÆ #
.
ÆÆ# $
UpdateAsync
ÆÆ$ /
(
ÆÆ/ 0
doctor
ÆÆ0 6
)
ÆÆ6 7
;
ÆÆ7 8
await
ØØ 
_doctorRepository
ØØ #
.
ØØ# $
SaveChangesAsync
ØØ$ 4
(
ØØ4 5
)
ØØ5 6
;
ØØ6 7
}
∞∞ 	
private
≤≤ 
static
≤≤ 
void
≤≤ 
ValidateDoctor
≤≤ *
(
≤≤* +
CreateDoctorDto
≤≤+ :
dto
≤≤; >
)
≤≤> ?
{
≥≥ 	
if
¥¥ 
(
¥¥ 
string
¥¥ 
.
¥¥  
IsNullOrWhiteSpace
¥¥ )
(
¥¥) *
dto
¥¥* -
.
¥¥- .
FullName
¥¥. 6
)
¥¥6 7
)
¥¥7 8
throw
µµ 
new
µµ 
ArgumentException
µµ +
(
µµ+ ,
$str
µµ, F
)
µµF G
;
µµG H
if
∑∑ 
(
∑∑ 
!
∑∑ 
Enum
∑∑ 
.
∑∑ 
	IsDefined
∑∑ 
(
∑∑  
typeof
∑∑  &
(
∑∑& '"
DoctorSpecialisation
∑∑' ;
)
∑∑; <
,
∑∑< =
dto
∑∑> A
.
∑∑A B
Specialisation
∑∑B P
)
∑∑P Q
)
∑∑Q R
throw
∏∏ 
new
∏∏ 
ArgumentException
∏∏ +
(
∏∏+ ,
$str
∏∏, L
)
∏∏L M
;
∏∏M N
if
∫∫ 
(
∫∫ 
dto
∫∫ 
.
∫∫ 
YearsOfExperience
∫∫ %
<
∫∫& '
$num
∫∫( )
||
∫∫* ,
dto
∫∫- 0
.
∫∫0 1
YearsOfExperience
∫∫1 B
>
∫∫C D
$num
∫∫E G
)
∫∫G H
throw
ªª 
new
ªª 
ArgumentException
ªª +
(
ªª+ ,
$str
ªª, X
)
ªªX Y
;
ªªY Z
if
ΩΩ 
(
ΩΩ 
dto
ΩΩ 
.
ΩΩ 
ConsultationFee
ΩΩ #
<=
ΩΩ$ &
$num
ΩΩ' (
)
ΩΩ( )
throw
ææ 
new
ææ 
ArgumentException
ææ +
(
ææ+ ,
$str
ææ, Y
)
ææY Z
;
ææZ [
if
¿¿ 
(
¿¿ 
string
¿¿ 
.
¿¿  
IsNullOrWhiteSpace
¿¿ )
(
¿¿) *
dto
¿¿* -
.
¿¿- .
Email
¿¿. 3
)
¿¿3 4
)
¿¿4 5
throw
¡¡ 
new
¡¡ 
ArgumentException
¡¡ +
(
¡¡+ ,
$str
¡¡, @
)
¡¡@ A
;
¡¡A B
}
¬¬ 	
private
ƒƒ 
static
ƒƒ 
void
ƒƒ 
ValidateDoctor
ƒƒ *
(
ƒƒ* +
UpdateDoctorDto
ƒƒ+ :
dto
ƒƒ; >
)
ƒƒ> ?
{
≈≈ 	
if
∆∆ 
(
∆∆ 
string
∆∆ 
.
∆∆  
IsNullOrWhiteSpace
∆∆ )
(
∆∆) *
dto
∆∆* -
.
∆∆- .
FullName
∆∆. 6
)
∆∆6 7
)
∆∆7 8
throw
«« 
new
«« 
ArgumentException
«« +
(
««+ ,
$str
««, F
)
««F G
;
««G H
if
…… 
(
…… 
!
…… 
Enum
…… 
.
…… 
	IsDefined
…… 
(
……  
typeof
……  &
(
……& '"
DoctorSpecialisation
……' ;
)
……; <
,
……< =
dto
……> A
.
……A B
Specialisation
……B P
)
……P Q
)
……Q R
throw
   
new
   
ArgumentException
   +
(
  + ,
$str
  , L
)
  L M
;
  M N
if
ÃÃ 
(
ÃÃ 
dto
ÃÃ 
.
ÃÃ 
YearsOfExperience
ÃÃ %
<
ÃÃ& '
$num
ÃÃ( )
||
ÃÃ* ,
dto
ÃÃ- 0
.
ÃÃ0 1
YearsOfExperience
ÃÃ1 B
>
ÃÃC D
$num
ÃÃE G
)
ÃÃG H
throw
ÕÕ 
new
ÕÕ 
ArgumentException
ÕÕ +
(
ÕÕ+ ,
$str
ÕÕ, X
)
ÕÕX Y
;
ÕÕY Z
if
œœ 
(
œœ 
dto
œœ 
.
œœ 
ConsultationFee
œœ #
<=
œœ$ &
$num
œœ' (
)
œœ( )
throw
–– 
new
–– 
ArgumentException
–– +
(
––+ ,
$str
––, Y
)
––Y Z
;
––Z [
}
—— 	
private
”” 
static
”” 
string
”” '
GenerateTemporaryPassword
”” 7
(
””7 8
)
””8 9
{
‘‘ 	
return
’’ 
$"
’’ 
$str
’’ 
{
’’ 
Random
’’  
.
’’  !
Shared
’’! '
.
’’' (
Next
’’( ,
(
’’, -
$num
’’- 3
,
’’3 4
$num
’’5 ;
)
’’; <
}
’’< =
"
’’= >
;
’’> ?
}
÷÷ 	
private
ÿÿ 
static
ÿÿ 
string
ÿÿ 
HashPassword
ÿÿ *
(
ÿÿ* +
string
ÿÿ+ 1
password
ÿÿ2 :
)
ÿÿ: ;
{
ŸŸ 	
using
⁄⁄ 
var
⁄⁄ 
sha256
⁄⁄ 
=
⁄⁄ 
System
⁄⁄ %
.
⁄⁄% &
Security
⁄⁄& .
.
⁄⁄. /
Cryptography
⁄⁄/ ;
.
⁄⁄; <
SHA256
⁄⁄< B
.
⁄⁄B C
Create
⁄⁄C I
(
⁄⁄I J
)
⁄⁄J K
;
⁄⁄K L
var
‹‹ 
bytes
‹‹ 
=
‹‹ 
System
‹‹ 
.
‹‹ 
Text
‹‹ #
.
‹‹# $
Encoding
‹‹$ ,
.
‹‹, -
UTF8
‹‹- 1
.
‹‹1 2
GetBytes
‹‹2 :
(
‹‹: ;
password
‹‹; C
)
‹‹C D
;
‹‹D E
var
›› 
hash
›› 
=
›› 
sha256
›› 
.
›› 
ComputeHash
›› )
(
››) *
bytes
››* /
)
››/ 0
;
››0 1
return
ﬂﬂ 
Convert
ﬂﬂ 
.
ﬂﬂ 
ToBase64String
ﬂﬂ )
(
ﬂﬂ) *
hash
ﬂﬂ* .
)
ﬂﬂ. /
;
ﬂﬂ/ 0
}
‡‡ 	
private
‚‚ 
static
‚‚ 
	DoctorDto
‚‚  
MapToDoctorDto
‚‚! /
(
‚‚/ 0
Doctor
‚‚0 6
doctor
‚‚7 =
)
‚‚= >
{
„„ 	
return
‰‰ 
new
‰‰ 
	DoctorDto
‰‰  
{
ÂÂ 
DoctorId
ÊÊ 
=
ÊÊ 
doctor
ÊÊ !
.
ÊÊ! "
DoctorId
ÊÊ" *
,
ÊÊ* +
FullName
ÁÁ 
=
ÁÁ 
doctor
ÁÁ !
.
ÁÁ! "
FullName
ÁÁ" *
,
ÁÁ* +
Email
ËË 
=
ËË 
doctor
ËË 
.
ËË 
Email
ËË $
,
ËË$ %
Specialisation
ÈÈ 
=
ÈÈ  
(
ÈÈ! "
int
ÈÈ" %
)
ÈÈ% &
doctor
ÈÈ& ,
.
ÈÈ, -
Specialisation
ÈÈ- ;
,
ÈÈ; <
YearsOfExperience
ÍÍ !
=
ÍÍ" #
doctor
ÍÍ$ *
.
ÍÍ* +
YearsOfExperience
ÍÍ+ <
,
ÍÍ< =
ConsultationFee
ÎÎ 
=
ÎÎ  !
doctor
ÎÎ" (
.
ÎÎ( )
ConsultationFee
ÎÎ) 8
,
ÎÎ8 9
IsActive
ÏÏ 
=
ÏÏ 
doctor
ÏÏ !
.
ÏÏ! "
IsActive
ÏÏ" *
}
ÌÌ 
;
ÌÌ 
}
ÓÓ 	
}
ÔÔ 
} ≠
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\AuthService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
IUserRepository (
_userRepository) 8
;8 9
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
private 
readonly 
IPatientRepository +
_patientRepository, >
;> ?
public 
AuthService 
( 
IUserRepository 
userRepository *
,* +
IPatientRepository 
patientRepository 0
,0 1
IConfiguration 
configuration (
)( )
{ 	
_userRepository 
= 
userRepository ,
;, -
_patientRepository 
=  
patientRepository! 2
;2 3
_configuration 
= 
configuration *
;* +
} 	
public 
async 
Task 
< 
( 
bool 
Success  '
,' (
string) /
Message0 7
,7 8
AuthResponseDto9 H
?H I
DataJ N
)N O
>O P
RegisterAsyncQ ^
(^ _
RegisterDto_ j
requestk r
)r s
{ 	
if   
(   
request   
.   
Password    
!=  ! #
request  $ +
.  + ,
ConfirmPassword  , ;
)  ; <
{!! 
return"" 
("" 
false"" 
,"" 
$str"" 8
,""8 9
null"": >
)""> ?
;""? @
}## 
if%% 
(%% 
await%% 
_userRepository%% %
.%%% &
EmailExistsAsync%%& 6
(%%6 7
request%%7 >
.%%> ?
Email%%? D
)%%D E
)%%E F
{&& 
return'' 
('' 
false'' 
,'' 
$str'' 6
,''6 7
null''8 <
)''< =
;''= >
}(( 
var** 
user** 
=** 
new** 
User** 
{++ 
Email,, 
=,, 
request,, 
.,,  
Email,,  %
.,,% &
Trim,,& *
(,,* +
),,+ ,
.,,, -
ToLower,,- 4
(,,4 5
),,5 6
,,,6 7
PasswordHash-- 
=-- 
HashPassword-- +
(--+ ,
request--, 3
.--3 4
Password--4 <
)--< =
,--= >
Role.. 
=.. 
request.. 
... 
Role.. #
,..# $
CreatedDate// 
=// 
DateTime// &
.//& '
UtcNow//' -
}00 
;00 
var22 
refreshToken22 
=22  
GenerateRefreshToken22 3
(223 4
)224 5
;225 6
user44 
.44 
RefreshToken44 
=44 
refreshToken44  ,
;44, -
user55 
.55 "
RefreshTokenExpiryTime55 '
=55( )
DateTime55* 2
.552 3
UtcNow553 9
.559 :
AddDays55: A
(55A B
$num55B C
)55C D
;55D E
await77 
_userRepository77 !
.77! "
AddAsync77" *
(77* +
user77+ /
)77/ 0
;770 1
await88 
_userRepository88 !
.88! "
SaveChangesAsync88" 2
(882 3
)883 4
;884 5
var:: 
accessToken:: 
=:: 
GenerateToken:: +
(::+ ,
user::, 0
)::0 1
;::1 2
return<< 
(<< 
true== 
,== 
$str>> /
,>>/ 0
new?? 
AuthResponseDto?? #
{@@ 
AccessTokenAA 
=AA  !
accessTokenAA" -
,AA- .
RefreshTokenBB  
=BB! "
refreshTokenBB# /
,BB/ 0
EmailCC 
=CC 
userCC  
.CC  !
EmailCC! &
,CC& '
RoleDD 
=DD 
userDD 
.DD  
RoleDD  $
.DD$ %
ToStringDD% -
(DD- .
)DD. /
}EE 
)EE 
;EE 
}FF 	
publicHH 
asyncHH 
TaskHH 
<HH 
(HH 
boolHH 
SuccessHH  '
,HH' (
stringHH( .
MessageHH/ 6
,HH6 7
AuthResponseDtoHH8 G
?HHG H
DataHHI M
)HHM N
>HHN O 
RegisterPatientAsyncHHO c
(HHc d
RegisterPatientDtoHHd v
requestHHw ~
)HH~ 
{II 	
ifJJ 
(JJ 
requestJJ 
.JJ 
PasswordJJ  
!=JJ! #
requestJJ$ +
.JJ+ ,
ConfirmPasswordJJ, ;
)JJ; <
{KK 
returnLL 
(LL 
falseLL 
,LL 
$strLL 8
,LL8 9
nullLL: >
)LL> ?
;LL? @
}MM 
ifOO 
(OO 
awaitOO 
_userRepositoryOO %
.OO% &
EmailExistsAsyncOO& 6
(OO6 7
requestPP 
.PP 
EmailPP 
)PP 
)PP 
{QQ 
returnRR 
(RR 
falseRR 
,RR 
$strRR 6
,RR6 7
nullRR8 <
)RR< =
;RR= >
}SS 
varUU 
patientUU 
=UU 
newUU 
PatientUU %
{VV 
FullNameWW 
=WW 
requestWW "
.WW" #
FullNameWW# +
.WW+ ,
TrimWW, 0
(WW0 1
)WW1 2
,WW2 3
DateOfBirthXX 
=XX 
requestXX %
.XX% &
DateOfBirthXX& 1
,XX1 2
GenderYY 
=YY 
requestYY  
.YY  !
GenderYY! '
,YY' (
PhoneNumberZZ 
=ZZ 
requestZZ %
.ZZ% &
PhoneNumberZZ& 1
.ZZ1 2
TrimZZ2 6
(ZZ6 7
)ZZ7 8
,ZZ8 9
Email[[ 
=[[ 
request[[ 
.[[  
Email[[  %
.[[% &
Trim[[& *
([[* +
)[[+ ,
.[[, -
ToLower[[- 4
([[4 5
)[[5 6
,[[6 7
InsuranceNumber\\ 
=\\  !
request\\" )
.\\) *
InsuranceNumber\\* 9
,\\9 :
IsActive]] 
=]] 
true]] 
}^^ 
;^^ 
await`` 
_patientRepository`` $
.``$ %
AddAsync``% -
(``- .
patient``. 5
)``5 6
;``6 7
awaitaa 
_patientRepositoryaa $
.aa$ %
SaveChangesAsyncaa% 5
(aa5 6
)aa6 7
;aa7 8
varcc 
usercc 
=cc 
newcc 
Usercc 
{dd 
Emailee 
=ee 
requestee 
.ee  
Emailee  %
.ee% &
Trimee& *
(ee* +
)ee+ ,
.ee, -
ToLoweree- 4
(ee4 5
)ee5 6
,ee6 7
PasswordHashff 
=ff 
HashPasswordff +
(ff+ ,
requestff, 3
.ff3 4
Passwordff4 <
)ff< =
,ff= >
Rolegg 
=gg 
UserRolegg 
.gg  
Patientgg  '
,gg' (
ReferenceIdhh 
=hh 
patienthh %
.hh% &
	PatientIdhh& /
}ii 
;ii 
awaitkk 
_userRepositorykk !
.kk! "
AddAsynckk" *
(kk* +
userkk+ /
)kk/ 0
;kk0 1
awaitll 
_userRepositoryll !
.ll! "
SaveChangesAsyncll" 2
(ll2 3
)ll3 4
;ll4 5
varnn 
accessTokennn 
=nn 
GenerateTokenoo 
(oo 
useroo "
)oo" #
;oo# $
varqq 
refreshTokenqq 
=qq  
GenerateRefreshTokenrr $
(rr$ %
)rr% &
;rr& '
usertt 
.tt 
RefreshTokentt 
=tt 
refreshTokentt  ,
;tt, -
useruu 
.uu "
RefreshTokenExpiryTimeuu '
=uu( )
DateTimevv 
.vv 
UtcNowvv 
.vv  
AddDaysvv  '
(vv' (
$numvv( )
)vv) *
;vv* +
awaitxx 
_userRepositoryxx !
.xx! "
SaveChangesAsyncxx" 2
(xx2 3
)xx3 4
;xx4 5
returnzz 
(zz 
true{{ 
,{{ 
$str|| 2
,||2 3
new}} 
AuthResponseDto}} #
{~~ 
AccessToken 
=  !
accessToken" -
,- .
RefreshToken
ÄÄ  
=
ÄÄ! "
refreshToken
ÄÄ# /
,
ÄÄ/ 0
Email
ÅÅ 
=
ÅÅ 
user
ÅÅ  
.
ÅÅ  !
Email
ÅÅ! &
,
ÅÅ& '
Role
ÇÇ 
=
ÇÇ 
user
ÇÇ 
.
ÇÇ  
Role
ÇÇ  $
.
ÇÇ$ %
ToString
ÇÇ% -
(
ÇÇ- .
)
ÇÇ. /
}
ÉÉ 
)
ÉÉ 
;
ÉÉ 
}
ÑÑ 	
public
ÜÜ 
async
ÜÜ 
Task
ÜÜ 
<
ÜÜ 
(
ÜÜ 
bool
ÜÜ 
Success
ÜÜ  '
,
ÜÜ' (
string
ÜÜ) /
Message
ÜÜ0 7
,
ÜÜ7 8
AuthResponseDto
ÜÜ9 H
?
ÜÜH I
Data
ÜÜJ N
)
ÜÜN O
>
ÜÜO P

LoginAsync
áá 
(
áá 
LoginDto
áá 
request
áá  '
)
áá' (
{
àà 	
var
ââ 
user
ââ 
=
ââ 
await
ââ 
_userRepository
ââ ,
.
ââ, -
GetByEmailAsync
ââ- <
(
ââ< =
request
ää 
.
ää 
Email
ää 
.
ää 
Trim
ää "
(
ää" #
)
ää# $
.
ää$ %
ToLower
ää% ,
(
ää, -
)
ää- .
)
ää. /
;
ää/ 0
if
åå 
(
åå 
user
åå 
==
åå 
null
åå 
)
åå 
{
çç 
return
éé 
(
éé 
false
éé 
,
éé 
$str
éé ;
,
éé; <
null
éé= A
)
ééA B
;
ééB C
}
èè 
var
ëë 
hashedPassword
ëë 
=
ëë  
HashPassword
ëë! -
(
ëë- .
request
ëë. 5
.
ëë5 6
Password
ëë6 >
)
ëë> ?
;
ëë? @
if
ìì 
(
ìì 
user
ìì 
.
ìì 
PasswordHash
ìì !
!=
ìì" $
hashedPassword
ìì% 3
)
ìì3 4
{
îî 
return
ïï 
(
ïï 
false
ïï 
,
ïï 
$str
ïï ;
,
ïï; <
null
ïï= A
)
ïïA B
;
ïïB C
}
ññ 
var
òò 
accessToken
òò 
=
òò 
GenerateToken
òò +
(
òò+ ,
user
òò, 0
)
òò0 1
;
òò1 2
var
öö 
refreshToken
öö 
=
öö "
GenerateRefreshToken
öö 3
(
öö3 4
)
öö4 5
;
öö5 6
user
úú 
.
úú 
RefreshToken
úú 
=
úú 
refreshToken
úú  ,
;
úú, -
user
ùù 
.
ùù $
RefreshTokenExpiryTime
ùù '
=
ùù( )
DateTime
ùù* 2
.
ùù2 3
UtcNow
ùù3 9
.
ùù9 :
AddDays
ùù: A
(
ùùA B
$num
ùùB C
)
ùùC D
;
ùùD E
await
üü 
_userRepository
üü !
.
üü! "
UpdateAsync
üü" -
(
üü- .
user
üü. 2
)
üü2 3
;
üü3 4
await
†† 
_userRepository
†† !
.
††! "
SaveChangesAsync
††" 2
(
††2 3
)
††3 4
;
††4 5
return
¢¢ 
(
¢¢ 
true
££ 
,
££ 
$str
§§ #
,
§§# $
new
•• 
AuthResponseDto
•• #
{
¶¶ 
AccessToken
ßß 
=
ßß  !
accessToken
ßß" -
,
ßß- .
RefreshToken
®®  
=
®®! "
refreshToken
®®# /
,
®®/ 0
Email
©© 
=
©© 
user
©©  
.
©©  !
Email
©©! &
,
©©& '
Role
™™ 
=
™™ 
user
™™ 
.
™™  
Role
™™  $
.
™™$ %
ToString
™™% -
(
™™- .
)
™™. /
}
´´ 
)
´´ 
;
´´ 
}
¨¨ 	
public
ÆÆ 
async
ÆÆ 
Task
ÆÆ 
<
ÆÆ 
(
ÆÆ 
bool
ÆÆ 
Success
ÆÆ  '
,
ÆÆ' (
string
ÆÆ) /
Message
ÆÆ0 7
,
ÆÆ7 8
AuthResponseDto
ÆÆ9 H
?
ÆÆH I
Data
ÆÆJ N
)
ÆÆN O
>
ÆÆO P
RefreshTokenAsync
ØØ 
(
ØØ 
RefreshTokenDto
ØØ -
request
ØØ. 5
)
ØØ5 6
{
∞∞ 	
var
±± 
user
±± 
=
±± 
await
±± 
_userRepository
±± ,
.
≤≤ $
GetByRefreshTokenAsync
≤≤ '
(
≤≤' (
request
≤≤( /
.
≤≤/ 0
RefreshToken
≤≤0 <
)
≤≤< =
;
≤≤= >
if
¥¥ 
(
¥¥ 
user
¥¥ 
==
¥¥ 
null
¥¥ 
)
¥¥ 
{
µµ 
return
∂∂ 
(
∂∂ 
false
∂∂ 
,
∂∂ 
$str
∂∂ 7
,
∂∂7 8
null
∂∂9 =
)
∂∂= >
;
∂∂> ?
}
∑∑ 
if
ππ 
(
ππ 
!
ππ 
user
ππ 
.
ππ $
RefreshTokenExpiryTime
ππ ,
.
ππ, -
HasValue
ππ- 5
||
ππ6 8
user
∫∫ 
.
∫∫ $
RefreshTokenExpiryTime
∫∫ +
.
∫∫+ ,
Value
∫∫, 1
<=
∫∫2 4
DateTime
∫∫5 =
.
∫∫= >
UtcNow
∫∫> D
)
∫∫D E
{
ªª 
return
ºº 
(
ºº 
false
ºº 
,
ºº 
$str
ºº ;
,
ºº; <
null
ºº= A
)
ººA B
;
ººB C
}
ΩΩ 
var
øø 
newAccessToken
øø 
=
øø  
GenerateToken
øø! .
(
øø. /
user
øø/ 3
)
øø3 4
;
øø4 5
var
¡¡ 
newRefreshToken
¡¡ 
=
¡¡  !"
GenerateRefreshToken
¡¡" 6
(
¡¡6 7
)
¡¡7 8
;
¡¡8 9
user
√√ 
.
√√ 
RefreshToken
√√ 
=
√√ 
newRefreshToken
√√  /
;
√√/ 0
user
ƒƒ 
.
ƒƒ $
RefreshTokenExpiryTime
ƒƒ '
=
ƒƒ( )
DateTime
ƒƒ* 2
.
ƒƒ2 3
UtcNow
ƒƒ3 9
.
ƒƒ9 :
AddDays
ƒƒ: A
(
ƒƒA B
$num
ƒƒB C
)
ƒƒC D
;
ƒƒD E
await
∆∆ 
_userRepository
∆∆ !
.
∆∆! "
UpdateAsync
∆∆" -
(
∆∆- .
user
∆∆. 2
)
∆∆2 3
;
∆∆3 4
await
«« 
_userRepository
«« !
.
««! "
SaveChangesAsync
««" 2
(
««2 3
)
««3 4
;
««4 5
return
…… 
(
…… 
true
   
,
   
$str
ÀÀ /
,
ÀÀ/ 0
new
ÃÃ 
AuthResponseDto
ÃÃ #
{
ÕÕ 
AccessToken
ŒŒ 
=
ŒŒ  !
newAccessToken
ŒŒ" 0
,
ŒŒ0 1
RefreshToken
œœ  
=
œœ! "
newRefreshToken
œœ# 2
,
œœ2 3
Email
–– 
=
–– 
user
––  
.
––  !
Email
––! &
,
––& '
Role
—— 
=
—— 
user
—— 
.
——  
Role
——  $
.
——$ %
ToString
——% -
(
——- .
)
——. /
}
““ 
)
““ 
;
““ 
}
”” 	
private
’’ 
string
’’ 
GenerateToken
’’ $
(
’’$ %
User
’’% )
user
’’* .
)
’’. /
{
÷÷ 	
var
◊◊ 
jwtSettings
◊◊ 
=
◊◊ 
_configuration
◊◊ ,
.
◊◊, -

GetSection
◊◊- 7
(
◊◊7 8
$str
◊◊8 =
)
◊◊= >
;
◊◊> ?
var
ŸŸ 
key
ŸŸ 
=
ŸŸ 
new
ŸŸ "
SymmetricSecurityKey
ŸŸ .
(
ŸŸ. /
Encoding
⁄⁄ 
.
⁄⁄ 
UTF8
⁄⁄ 
.
⁄⁄ 
GetBytes
⁄⁄ &
(
⁄⁄& '
jwtSettings
⁄⁄' 2
[
⁄⁄2 3
$str
⁄⁄3 8
]
⁄⁄8 9
!
⁄⁄9 :
)
⁄⁄: ;
)
⁄⁄; <
;
⁄⁄< =
var
‹‹ 
credentials
‹‹ 
=
‹‹ 
new
‹‹ ! 
SigningCredentials
‹‹" 4
(
‹‹4 5
key
›› 
,
››  
SecurityAlgorithms
ﬁﬁ "
.
ﬁﬁ" #

HmacSha256
ﬁﬁ# -
)
ﬁﬁ- .
;
ﬁﬁ. /
var
‡‡ 
claims
‡‡ 
=
‡‡ 
new
‡‡ 
List
‡‡ !
<
‡‡! "
Claim
‡‡" '
>
‡‡' (
{
·· 
new
‚‚ 
Claim
‚‚ 
(
‚‚ %
JwtRegisteredClaimNames
„„ +
.
„„+ ,
Sub
„„, /
,
„„/ 0
user
‰‰ 
.
‰‰ 
UserId
‰‰ 
.
‰‰  
ToString
‰‰  (
(
‰‰( )
)
‰‰) *
)
‰‰* +
,
‰‰+ ,
new
ÊÊ 
Claim
ÊÊ 
(
ÊÊ %
JwtRegisteredClaimNames
ÁÁ +
.
ÁÁ+ ,
Email
ÁÁ, 1
,
ÁÁ1 2
user
ËË 
.
ËË 
Email
ËË 
)
ËË 
,
ËË  
new
ÍÍ 
Claim
ÍÍ 
(
ÍÍ %
JwtRegisteredClaimNames
ÎÎ +
.
ÎÎ+ ,
Jti
ÎÎ, /
,
ÎÎ/ 0
Guid
ÏÏ 
.
ÏÏ 
NewGuid
ÏÏ  
(
ÏÏ  !
)
ÏÏ! "
.
ÏÏ" #
ToString
ÏÏ# +
(
ÏÏ+ ,
)
ÏÏ, -
)
ÏÏ- .
,
ÏÏ. /
new
ÓÓ 
Claim
ÓÓ 
(
ÓÓ 

ClaimTypes
ÔÔ 
.
ÔÔ 
NameIdentifier
ÔÔ -
,
ÔÔ- .
user
 
.
 
UserId
 
.
  
ToString
  (
(
( )
)
) *
)
* +
,
+ ,
new
ÚÚ 
Claim
ÚÚ 
(
ÚÚ 

ClaimTypes
ÛÛ 
.
ÛÛ 
Role
ÛÛ #
,
ÛÛ# $
user
ÙÙ 
.
ÙÙ 
Role
ÙÙ 
.
ÙÙ 
ToString
ÙÙ &
(
ÙÙ& '
)
ÙÙ' (
)
ÙÙ( )
}
ıı 
;
ıı 
var
˜˜ 
token
˜˜ 
=
˜˜ 
new
˜˜ 
JwtSecurityToken
˜˜ ,
(
˜˜, -
issuer
¯¯ 
:
¯¯ 
jwtSettings
¯¯ #
[
¯¯# $
$str
¯¯$ ,
]
¯¯, -
,
¯¯- .
audience
˘˘ 
:
˘˘ 
jwtSettings
˘˘ %
[
˘˘% &
$str
˘˘& 0
]
˘˘0 1
,
˘˘1 2
claims
˙˙ 
:
˙˙ 
claims
˙˙ 
,
˙˙ 
expires
˚˚ 
:
˚˚ 
DateTime
˚˚ !
.
˚˚! "
UtcNow
˚˚" (
.
˚˚( )

AddMinutes
˚˚) 3
(
˚˚3 4
int
¸¸ 
.
¸¸ 
Parse
¸¸ 
(
¸¸ 
jwtSettings
¸¸ )
[
¸¸) *
$str
¸¸* H
]
¸¸H I
!
¸¸I J
)
¸¸J K
)
¸¸K L
,
¸¸L M 
signingCredentials
˝˝ "
:
˝˝" #
credentials
˝˝$ /
)
˝˝/ 0
;
˝˝0 1
return
ˇˇ 
new
ˇˇ %
JwtSecurityTokenHandler
ˇˇ .
(
ˇˇ. /
)
ˇˇ/ 0
.
ÄÄ 

WriteToken
ÄÄ 
(
ÄÄ 
token
ÄÄ !
)
ÄÄ! "
;
ÄÄ" #
}
ÅÅ 	
private
ÉÉ 
static
ÉÉ 
string
ÉÉ "
GenerateRefreshToken
ÉÉ 2
(
ÉÉ2 3
)
ÉÉ3 4
{
ÑÑ 	
return
ÖÖ 
Convert
ÖÖ 
.
ÖÖ 
ToBase64String
ÖÖ )
(
ÖÖ) *#
RandomNumberGenerator
ÜÜ %
.
ÜÜ% &
GetBytes
ÜÜ& .
(
ÜÜ. /
$num
ÜÜ/ 1
)
ÜÜ1 2
)
ÜÜ2 3
;
ÜÜ3 4
}
áá 	
private
ââ 
static
ââ 
string
ââ 
HashPassword
ââ *
(
ââ* +
string
ââ+ 1
password
ââ2 :
)
ââ: ;
{
ää 	
using
ãã 
var
ãã 
sha256
ãã 
=
ãã 
SHA256
ãã %
.
ãã% &
Create
ãã& ,
(
ãã, -
)
ãã- .
;
ãã. /
byte
çç 
[
çç 
]
çç 
bytes
çç 
=
çç 
Encoding
çç #
.
çç# $
UTF8
çç$ (
.
çç( )
GetBytes
çç) 1
(
çç1 2
password
çç2 :
)
çç: ;
;
çç; <
byte
èè 
[
èè 
]
èè 
hash
èè 
=
èè 
sha256
èè  
.
èè  !
ComputeHash
èè! ,
(
èè, -
bytes
èè- 2
)
èè2 3
;
èè3 4
return
ëë 
Convert
ëë 
.
ëë 
ToBase64String
ëë )
(
ëë) *
hash
ëë* .
)
ëë. /
;
ëë/ 0
}
íí 	
}
ìì 
}îî ∫ú
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\AppointmentService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{		 
public

 

class

 
AppointmentService

 #
:

$ %
IAppointmentService

& 9
{ 
private 
readonly "
IAppointmentRepository /"
_appointmentRepository0 F
;F G
private 
readonly 
IPatientRepository +
_patientRepository, >
;> ?
private 
readonly 
IDoctorRepository *
_doctorRepository+ <
;< =
public 
AppointmentService !
(! ""
IAppointmentRepository "!
appointmentRepository# 8
,8 9
IPatientRepository 
patientRepository 0
,0 1
IDoctorRepository 
doctorRepository .
). /
{ 	"
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
_patientRepository 
=  
patientRepository! 2
;2 3
_doctorRepository 
= 
doctorRepository  0
;0 1
} 	
public 
async 
Task 
< 
IEnumerable %
<% &!
AppointmentDetailsDto& ;
>; <
>< =
GetAllAsync> I
(I J
)J K
{ 	
var 
appointments 
= 
await $"
_appointmentRepository% ;
.; <
GetAllAsync< G
(G H
)H I
;I J
return 
appointments 
.  
Select  &
(& '&
MapToAppointmentDetailsDto' A
)A B
;B C
}   	
public"" 
async"" 
Task"" 
<"" !
AppointmentDetailsDto"" /
?""/ 0
>""0 1
GetByIdAsync""2 >
(""> ?
int""? B
id""C E
)""E F
{## 	
var$$ 
appointment$$ 
=$$ 
await$$ #"
_appointmentRepository$$$ :
.$$: ;
GetByIdAsync$$; G
($$G H
id$$H J
)$$J K
;$$K L
if&& 
(&& 
appointment&& 
==&& 
null&& #
)&&# $
return'' 
null'' 
;'' 
return)) &
MapToAppointmentDetailsDto)) -
())- .
appointment)). 9
)))9 :
;)): ;
}** 	
public,, 
async,, 
Task,, 
<,, 
IEnumerable,, %
<,,% &(
PatientAppointmentHistoryDto,,& B
>,,B C
>,,C D"
GetPatientHistoryAsync,,E [
(,,[ \
int,,\ _
	patientId,,` i
),,i j
{-- 	
var.. 
appointments.. 
=.. 
await.. $"
_appointmentRepository..% ;
...; <
GetByPatientIdAsync..< O
(..O P
	patientId..P Y
)..Y Z
;..Z [
return00 
appointments00 
.00  
Select00  &
(00& '
a00' (
=>00) +
new00, /(
PatientAppointmentHistoryDto000 L
{11 
AppointmentId22 
=22 
a22  !
.22! "
AppointmentId22" /
,22/ 0
ScheduledDate33 
=33 
a33  !
.33! "
ScheduledDate33" /
,33/ 0
TimeSlot44 
=44 
(44 
int44 
)44  
a44  !
.44! "
TimeSlot44" *
,44* +
DoctorId55 
=55 
a55 
.55 
DoctorId55 %
,55% &

DoctorName66 
=66 
a66 
.66 
Doctor66 %
.66% &
FullName66& .
,66. /
Status77 
=77 
(77 
int77 
)77 
a77 
.77  
Status77  &
}88 
)88 
;88 
}99 	
public;; 
async;; 
Task;; 
<;; 
IEnumerable;; %
<;;% &!
DoctorScheduleItemDto;;& ;
>;;; <
>;;< ='
GetDoctorTodayScheduleAsync;;> Y
(;;Y Z
int;;Z ]
doctorId;;^ f
);;f g
{<< 	
var== 
appointments== 
=== 
await>> "
_appointmentRepository>> ,
.>>, -'
GetDoctorTodayScheduleAsync>>- H
(>>H I
doctorId?? 
,?? 
DateOnly@@ 
.@@ 
FromDateTime@@ )
(@@) *
DateTime@@* 2
.@@2 3
Today@@3 8
)@@8 9
)@@9 :
;@@: ;
returnBB 
appointmentsBB 
.BB  
SelectBB  &
(BB& '!
MapDoctorScheduleItemBB' <
)BB< =
;BB= >
}CC 	
publicEE 
asyncEE 
TaskEE 
<EE 
IEnumerableEE %
<EE% &!
DoctorScheduleItemDtoEE& ;
>EE; <
>EE< =&
GetDoctorWeekScheduleAsyncEE> X
(EEX Y
intFF 
doctorIdFF 
,FF 
DateOnlyGG 
	startDateGG 
,GG 
DateOnlyHH 
endDateHH 
)HH 
{II 	
varJJ 
appointmentsJJ 
=JJ 
awaitKK "
_appointmentRepositoryKK ,
.KK, -&
GetDoctorWeekScheduleAsyncKK- G
(KKG H
doctorIdLL 
,LL 
	startDateMM 
,MM 
endDateNN 
)NN 
;NN 
returnPP 
appointmentsPP 
.PP  
SelectPP  &
(PP& '!
MapDoctorScheduleItemPP' <
)PP< =
;PP= >
}QQ 	
publicSS 
asyncSS 
TaskSS 
<SS 
AppointmentDtoSS (
>SS( )
CreateAsyncSS* 5
(SS5 6 
CreateAppointmentDtoSS6 J
dtoSSK N
)SSN O
{TT 	
awaitUU  
ValidateBookingAsyncUU &
(UU& '
dtoVV 
.VV 
	PatientIdVV 
,VV 
dtoWW 
.WW 
DoctorIdWW 
,WW 
dtoXX 
.XX 
ScheduledDateXX !
,XX! "
dtoYY 
.YY 
TimeSlotYY 
)YY 
;YY 
var[[ 
appointment[[ 
=[[ 
new[[ !
Appointment[[" -
{\\ 
	PatientId]] 
=]] 
dto]] 
.]]  
	PatientId]]  )
,]]) *
DoctorId^^ 
=^^ 
dto^^ 
.^^ 
DoctorId^^ '
,^^' (
ScheduledDate__ 
=__ 
dto__  #
.__# $
ScheduledDate__$ 1
,__1 2
TimeSlot`` 
=`` 
(`` 
AppointmentTimeSlot`` /
)``/ 0
dto``0 3
.``3 4
TimeSlot``4 <
,``< =
Statusaa 
=aa 
AppointmentStatusaa *
.aa* +
Pendingaa+ 2
}bb 
;bb 
awaitdd "
_appointmentRepositorydd (
.dd( )
AddAsyncdd) 1
(dd1 2
appointmentdd2 =
)dd= >
;dd> ?
awaitee "
_appointmentRepositoryee (
.ee( )
SaveChangesAsyncee) 9
(ee9 :
)ee: ;
;ee; <
returngg 
MapToAppointmentDtogg &
(gg& '
appointmentgg' 2
)gg2 3
;gg3 4
}hh 	
publicjj 
asyncjj 
Taskjj 
UpdateAsyncjj %
(jj% &
intjj& )
idjj* ,
,jj, - 
UpdateAppointmentDtojj. B
dtojjC F
)jjF G
{kk 	
varll 
appointmentll 
=ll 
awaitll #"
_appointmentRepositoryll$ :
.ll: ;
GetByIdAsyncll; G
(llG H
idllH J
)llJ K
;llK L
ifnn 
(nn 
appointmentnn 
==nn 
nullnn #
)nn# $
throwoo 
newoo  
KeyNotFoundExceptionoo .
(oo. /
$"oo/ 1
$stroo1 =
{oo= >
idoo> @
}oo@ A
$strooA L
"ooL M
)ooM N
;ooN O
ifqq 
(qq 
appointmentqq 
.qq 
Statusqq "
==qq# %
AppointmentStatusqq& 7
.qq7 8
	Completedqq8 A
)qqA B
throwrr 
newrr %
InvalidOperationExceptionrr 3
(rr3 4
$strrr4 `
)rr` a
;rra b
iftt 
(tt 
appointmenttt 
.tt 
Statustt "
==tt# %
AppointmentStatustt& 7
.tt7 8
	Cancelledtt8 A
)ttA B
throwuu 
newuu %
InvalidOperationExceptionuu 3
(uu3 4
$struu4 `
)uu` a
;uua b
awaitww &
ValidateUpdateBookingAsyncww ,
(ww, -
appointmentxx 
.xx 
AppointmentIdxx )
,xx) *
appointmentyy 
.yy 
	PatientIdyy %
,yy% &
dtozz 
.zz 
DoctorIdzz 
,zz 
dto{{ 
.{{ 
ScheduledDate{{ !
,{{! "
dto|| 
.|| 
TimeSlot|| 
)|| 
;|| 
appointment~~ 
.~~ 
DoctorId~~  
=~~! "
dto~~# &
.~~& '
DoctorId~~' /
;~~/ 0
appointment 
. 
ScheduledDate %
=& '
dto( +
.+ ,
ScheduledDate, 9
;9 :
appointment
ÄÄ 
.
ÄÄ 
TimeSlot
ÄÄ  
=
ÄÄ! "
(
ÄÄ# $!
AppointmentTimeSlot
ÄÄ$ 7
)
ÄÄ7 8
dto
ÄÄ8 ;
.
ÄÄ; <
TimeSlot
ÄÄ< D
;
ÄÄD E
await
ÇÇ $
_appointmentRepository
ÇÇ (
.
ÇÇ( )
UpdateAsync
ÇÇ) 4
(
ÇÇ4 5
appointment
ÇÇ5 @
)
ÇÇ@ A
;
ÇÇA B
await
ÉÉ $
_appointmentRepository
ÉÉ (
.
ÉÉ( )
SaveChangesAsync
ÉÉ) 9
(
ÉÉ9 :
)
ÉÉ: ;
;
ÉÉ; <
}
ÑÑ 	
public
ÜÜ 
async
ÜÜ 
Task
ÜÜ 
UpdateStatusAsync
ÜÜ +
(
ÜÜ+ ,
int
ÜÜ, /
id
ÜÜ0 2
,
ÜÜ2 3(
UpdateAppointmentStatusDto
ÜÜ4 N
dto
ÜÜO R
)
ÜÜR S
{
áá 	
var
àà 
appointment
àà 
=
àà 
await
àà #$
_appointmentRepository
àà$ :
.
àà: ;
GetByIdAsync
àà; G
(
ààG H
id
ààH J
)
ààJ K
;
ààK L
if
ää 
(
ää 
appointment
ää 
==
ää 
null
ää #
)
ää# $
throw
ãã 
new
ãã "
KeyNotFoundException
ãã .
(
ãã. /
$"
ãã/ 1
$str
ãã1 =
{
ãã= >
id
ãã> @
}
ãã@ A
$str
ããA L
"
ããL M
)
ããM N
;
ããN O
if
çç 
(
çç 
!
çç 
Enum
çç 
.
çç 
	IsDefined
çç 
(
çç  
typeof
çç  &
(
çç& '
AppointmentStatus
çç' 8
)
çç8 9
,
çç9 :
dto
çç; >
.
çç> ?
Status
çç? E
)
ççE F
)
ççF G
throw
éé 
new
éé 
ArgumentException
éé +
(
éé+ ,
$str
éé, I
)
ééI J
;
ééJ K
var
êê 
	newStatus
êê 
=
êê 
(
êê 
AppointmentStatus
êê .
)
êê. /
dto
êê/ 2
.
êê2 3
Status
êê3 9
;
êê9 :
if
íí 
(
íí 
appointment
íí 
.
íí 
Status
íí "
==
íí# %
AppointmentStatus
íí& 7
.
íí7 8
	Completed
íí8 A
)
ííA B
throw
ìì 
new
ìì '
InvalidOperationException
ìì 3
(
ìì3 4
$str
ìì4 `
)
ìì` a
;
ììa b
if
ïï 
(
ïï 
appointment
ïï 
.
ïï 
Status
ïï "
==
ïï# %
AppointmentStatus
ïï& 7
.
ïï7 8
	Cancelled
ïï8 A
)
ïïA B
throw
ññ 
new
ññ '
InvalidOperationException
ññ 3
(
ññ3 4
$str
ññ4 `
)
ññ` a
;
ñña b
switch
òò 
(
òò 
	newStatus
òò 
)
òò 
{
ôô 
case
öö 
AppointmentStatus
öö &
.
öö& '
Pending
öö' .
:
öö. /
throw
õõ 
new
õõ '
InvalidOperationException
õõ 7
(
õõ7 8
$str
õõ8 m
)
õõm n
;
õõn o
case
ùù 
AppointmentStatus
ùù &
.
ùù& '
	Confirmed
ùù' 0
:
ùù0 1
if
ûû 
(
ûû 
appointment
ûû #
.
ûû# $
Status
ûû$ *
!=
ûû+ -
AppointmentStatus
ûû. ?
.
ûû? @
Pending
ûû@ G
)
ûûG H
throw
üü 
new
üü !'
InvalidOperationException
üü" ;
(
üü; <
$str
üü< i
)
üüi j
;
üüj k
appointment
°° 
.
°°  
Status
°°  &
=
°°' (
AppointmentStatus
°°) :
.
°°: ;
	Confirmed
°°; D
;
°°D E
break
¢¢ 
;
¢¢ 
case
§§ 
AppointmentStatus
§§ &
.
§§& '
	Completed
§§' 0
:
§§0 1
if
•• 
(
•• 
appointment
•• #
.
••# $
Status
••$ *
!=
••+ -
AppointmentStatus
••. ?
.
••? @
	Confirmed
••@ I
)
••I J
throw
¶¶ 
new
¶¶ !'
InvalidOperationException
¶¶" ;
(
¶¶; <
$str
¶¶< k
)
¶¶k l
;
¶¶l m
appointment
®® 
.
®®  
Status
®®  &
=
®®' (
AppointmentStatus
®®) :
.
®®: ;
	Completed
®®; D
;
®®D E
break
©© 
;
©© 
case
´´ 
AppointmentStatus
´´ &
.
´´& '
	Cancelled
´´' 0
:
´´0 1
if
¨¨ 
(
¨¨ 
string
¨¨ 
.
¨¨  
IsNullOrWhiteSpace
¨¨ 1
(
¨¨1 2
dto
¨¨2 5
.
¨¨5 6 
CancellationReason
¨¨6 H
)
¨¨H I
)
¨¨I J
throw
≠≠ 
new
≠≠ !
ArgumentException
≠≠" 3
(
≠≠3 4
$str
≠≠4 V
)
≠≠V W
;
≠≠W X
appointment
ØØ 
.
ØØ  
Status
ØØ  &
=
ØØ' (
AppointmentStatus
ØØ) :
.
ØØ: ;
	Cancelled
ØØ; D
;
ØØD E
appointment
∞∞ 
.
∞∞   
CancellationReason
∞∞  2
=
∞∞3 4
dto
∞∞5 8
.
∞∞8 9 
CancellationReason
∞∞9 K
.
∞∞K L
Trim
∞∞L P
(
∞∞P Q
)
∞∞Q R
;
∞∞R S
break
±± 
;
±± 
default
≥≥ 
:
≥≥ 
throw
¥¥ 
new
¥¥ 
ArgumentException
¥¥ /
(
¥¥/ 0
$str
¥¥0 M
)
¥¥M N
;
¥¥N O
}
µµ 
await
∑∑ $
_appointmentRepository
∑∑ (
.
∑∑( )
UpdateAsync
∑∑) 4
(
∑∑4 5
appointment
∑∑5 @
)
∑∑@ A
;
∑∑A B
await
∏∏ $
_appointmentRepository
∏∏ (
.
∏∏( )
SaveChangesAsync
∏∏) 9
(
∏∏9 :
)
∏∏: ;
;
∏∏; <
}
ππ 	
public
ªª 
async
ªª 
Task
ªª 
ConfirmAsync
ªª &
(
ªª& '
int
ªª' *
id
ªª+ -
)
ªª- .
{
ºº 	
var
ΩΩ 
appointment
ΩΩ 
=
ΩΩ 
await
ΩΩ #$
_appointmentRepository
ΩΩ$ :
.
ΩΩ: ;
GetByIdAsync
ΩΩ; G
(
ΩΩG H
id
ΩΩH J
)
ΩΩJ K
;
ΩΩK L
if
øø 
(
øø 
appointment
øø 
==
øø 
null
øø #
)
øø# $
throw
¿¿ 
new
¿¿ "
KeyNotFoundException
¿¿ .
(
¿¿. /
)
¿¿/ 0
;
¿¿0 1
if
¬¬ 
(
¬¬ 
appointment
¬¬ 
.
¬¬ 
Status
¬¬ "
!=
¬¬# %
AppointmentStatus
¬¬& 7
.
¬¬7 8
Pending
¬¬8 ?
)
¬¬? @
throw
√√ 
new
√√ '
InvalidOperationException
√√ 3
(
√√3 4
$str
√√4 a
)
√√a b
;
√√b c
appointment
≈≈ 
.
≈≈ 
Status
≈≈ 
=
≈≈  
AppointmentStatus
≈≈! 2
.
≈≈2 3
	Confirmed
≈≈3 <
;
≈≈< =
await
«« $
_appointmentRepository
«« (
.
««( )
UpdateAsync
««) 4
(
««4 5
appointment
««5 @
)
««@ A
;
««A B
await
»» $
_appointmentRepository
»» (
.
»»( )
SaveChangesAsync
»») 9
(
»»9 :
)
»»: ;
;
»»; <
}
…… 	
public
ÀÀ 
async
ÀÀ 
Task
ÀÀ 
CompleteAsync
ÀÀ '
(
ÀÀ' (
int
ÀÀ( +
id
ÀÀ, .
)
ÀÀ. /
{
ÃÃ 	
var
ÕÕ 
appointment
ÕÕ 
=
ÕÕ 
await
ÕÕ #$
_appointmentRepository
ÕÕ$ :
.
ÕÕ: ;
GetByIdAsync
ÕÕ; G
(
ÕÕG H
id
ÕÕH J
)
ÕÕJ K
;
ÕÕK L
if
œœ 
(
œœ 
appointment
œœ 
==
œœ 
null
œœ #
)
œœ# $
throw
–– 
new
–– "
KeyNotFoundException
–– .
(
––. /
)
––/ 0
;
––0 1
if
““ 
(
““ 
appointment
““ 
.
““ 
Status
““ "
!=
““# %
AppointmentStatus
““& 7
.
““7 8
	Confirmed
““8 A
)
““A B
throw
”” 
new
”” '
InvalidOperationException
”” 3
(
””3 4
$str
””4 c
)
””c d
;
””d e
appointment
’’ 
.
’’ 
Status
’’ 
=
’’  
AppointmentStatus
’’! 2
.
’’2 3
	Completed
’’3 <
;
’’< =
await
◊◊ $
_appointmentRepository
◊◊ (
.
◊◊( )
UpdateAsync
◊◊) 4
(
◊◊4 5
appointment
◊◊5 @
)
◊◊@ A
;
◊◊A B
await
ÿÿ $
_appointmentRepository
ÿÿ (
.
ÿÿ( )
SaveChangesAsync
ÿÿ) 9
(
ÿÿ9 :
)
ÿÿ: ;
;
ÿÿ; <
}
ŸŸ 	
public
€€ 
async
€€ 
Task
€€ 
CancelAsync
€€ %
(
€€% &
int
€€& )
id
€€* ,
,
€€, -"
CancelAppointmentDto
€€. B
dto
€€C F
)
€€F G
{
‹‹ 	
var
›› 
appointment
›› 
=
›› 
await
›› #$
_appointmentRepository
››$ :
.
››: ;
GetByIdAsync
››; G
(
››G H
id
››H J
)
››J K
;
››K L
if
ﬂﬂ 
(
ﬂﬂ 
appointment
ﬂﬂ 
==
ﬂﬂ 
null
ﬂﬂ #
)
ﬂﬂ# $
throw
‡‡ 
new
‡‡ "
KeyNotFoundException
‡‡ .
(
‡‡. /
)
‡‡/ 0
;
‡‡0 1
if
‚‚ 
(
‚‚ 
appointment
‚‚ 
.
‚‚ 
Status
‚‚ "
==
‚‚# %
AppointmentStatus
‚‚& 7
.
‚‚7 8
	Completed
‚‚8 A
)
‚‚A B
throw
„„ 
new
„„ '
InvalidOperationException
„„ 3
(
„„3 4
$str
„„4 a
)
„„a b
;
„„b c
if
ÂÂ 
(
ÂÂ 
appointment
ÂÂ 
.
ÂÂ 
Status
ÂÂ "
==
ÂÂ# %
AppointmentStatus
ÂÂ& 7
.
ÂÂ7 8
	Cancelled
ÂÂ8 A
)
ÂÂA B
throw
ÊÊ 
new
ÊÊ '
InvalidOperationException
ÊÊ 3
(
ÊÊ3 4
$str
ÊÊ4 T
)
ÊÊT U
;
ÊÊU V
if
ËË 
(
ËË 
string
ËË 
.
ËË  
IsNullOrWhiteSpace
ËË )
(
ËË) *
dto
ËË* -
.
ËË- . 
CancellationReason
ËË. @
)
ËË@ A
)
ËËA B
throw
ÈÈ 
new
ÈÈ 
ArgumentException
ÈÈ +
(
ÈÈ+ ,
$str
ÈÈ, N
)
ÈÈN O
;
ÈÈO P
appointment
ÎÎ 
.
ÎÎ 
Status
ÎÎ 
=
ÎÎ  
AppointmentStatus
ÎÎ! 2
.
ÎÎ2 3
	Cancelled
ÎÎ3 <
;
ÎÎ< =
appointment
ÏÏ 
.
ÏÏ  
CancellationReason
ÏÏ *
=
ÏÏ+ ,
dto
ÏÏ- 0
.
ÏÏ0 1 
CancellationReason
ÏÏ1 C
.
ÏÏC D
Trim
ÏÏD H
(
ÏÏH I
)
ÏÏI J
;
ÏÏJ K
await
ÓÓ $
_appointmentRepository
ÓÓ (
.
ÓÓ( )
UpdateAsync
ÓÓ) 4
(
ÓÓ4 5
appointment
ÓÓ5 @
)
ÓÓ@ A
;
ÓÓA B
await
ÔÔ $
_appointmentRepository
ÔÔ (
.
ÔÔ( )
SaveChangesAsync
ÔÔ) 9
(
ÔÔ9 :
)
ÔÔ: ;
;
ÔÔ; <
}
 	
public
ÚÚ 
async
ÚÚ 
Task
ÚÚ 
<
ÚÚ 
IEnumerable
ÚÚ %
<
ÚÚ% &#
DoctorScheduleItemDto
ÚÚ& ;
>
ÚÚ; <
>
ÚÚ< =,
GetDoctorUpcomingScheduleAsync
ÚÚ> \
(
ÚÚ\ ]
int
ÚÚ] `
doctorId
ÚÚa i
)
ÚÚi j
{
ÛÛ 	
var
ÙÙ 
	startDate
ÙÙ 
=
ÙÙ 
DateOnly
ÙÙ $
.
ÙÙ$ %
FromDateTime
ÙÙ% 1
(
ÙÙ1 2
DateTime
ÙÙ2 :
.
ÙÙ: ;
Today
ÙÙ; @
)
ÙÙ@ A
;
ÙÙA B
var
ıı 
endDate
ıı 
=
ıı 
	startDate
ıı #
.
ıı# $
AddDays
ıı$ +
(
ıı+ ,
$num
ıı, -
)
ıı- .
;
ıı. /
var
˜˜ 
appointments
˜˜ 
=
˜˜ 
await
¯¯ $
_appointmentRepository
¯¯ ,
.
¯¯, -(
GetDoctorWeekScheduleAsync
¯¯- G
(
¯¯G H
doctorId
˘˘ 
,
˘˘ 
	startDate
˙˙ 
,
˙˙ 
endDate
˚˚ 
)
˚˚ 
;
˚˚ 
return
˝˝ 
appointments
˝˝ 
.
˝˝  
Select
˝˝  &
(
˝˝& '#
MapDoctorScheduleItem
˝˝' <
)
˝˝< =
;
˝˝= >
}
˛˛ 	
private
ÄÄ 
async
ÄÄ 
Task
ÄÄ "
ValidateBookingAsync
ÄÄ /
(
ÄÄ/ 0
int
ÄÄ0 3
	patientId
ÄÄ4 =
,
ÄÄ= >
int
ÄÄ? B
doctorId
ÄÄC K
,
ÄÄK L
DateOnly
ÄÄM U
date
ÄÄV Z
,
ÄÄZ [
int
ÄÄ\ _
timeSlot
ÄÄ` h
)
ÄÄh i
{
ÅÅ 	
var
ÇÇ 
patient
ÇÇ 
=
ÇÇ 
await
ÇÇ  
_patientRepository
ÇÇ  2
.
ÇÇ2 3
GetByIdAsync
ÇÇ3 ?
(
ÇÇ? @
	patientId
ÇÇ@ I
)
ÇÇI J
;
ÇÇJ K
if
ÑÑ 
(
ÑÑ 
patient
ÑÑ 
==
ÑÑ 
null
ÑÑ 
)
ÑÑ  
throw
ÖÖ 
new
ÖÖ "
KeyNotFoundException
ÖÖ .
(
ÖÖ. /
$str
ÖÖ/ C
)
ÖÖC D
;
ÖÖD E
if
áá 
(
áá 
!
áá 
patient
áá 
.
áá 
IsActive
áá !
)
áá! "
throw
àà 
new
àà '
InvalidOperationException
àà 3
(
àà3 4
$str
àà4 a
)
ààa b
;
ààb c
var
ää 
doctor
ää 
=
ää 
await
ää 
_doctorRepository
ää 0
.
ää0 1
GetByIdAsync
ää1 =
(
ää= >
doctorId
ää> F
)
ääF G
;
ääG H
if
åå 
(
åå 
doctor
åå 
==
åå 
null
åå 
)
åå 
throw
çç 
new
çç "
KeyNotFoundException
çç .
(
çç. /
$str
çç/ B
)
ççB C
;
ççC D
if
èè 
(
èè 
!
èè 
doctor
èè 
.
èè 
IsActive
èè  
)
èè  !
throw
êê 
new
êê '
InvalidOperationException
êê 3
(
êê3 4
$str
êê4 F
)
êêF G
;
êêG H
if
íí 
(
íí 
date
íí 
<
íí 
DateOnly
íí 
.
íí  
FromDateTime
íí  ,
(
íí, -
DateTime
íí- 5
.
íí5 6
Today
íí6 ;
)
íí; <
)
íí< =
throw
ìì 
new
ìì 
ArgumentException
ìì +
(
ìì+ ,
$str
ìì, U
)
ììU V
;
ììV W
if
ïï 
(
ïï 
!
ïï 
Enum
ïï 
.
ïï 
	IsDefined
ïï 
(
ïï  
typeof
ïï  &
(
ïï& '!
AppointmentTimeSlot
ïï' :
)
ïï: ;
,
ïï; <
timeSlot
ïï= E
)
ïïE F
)
ïïF G
throw
ññ 
new
ññ 
ArgumentException
ññ +
(
ññ+ ,
$str
ññ, G
)
ññG H
;
ññH I
if
òò 
(
òò 
await
òò $
_appointmentRepository
òò ,
.
òò, -6
(ExistsSamePatientSameDoctorSameDateAsync
òò- U
(
òòU V
	patientId
òòV _
,
òò_ `
doctorId
òòa i
,
òòi j
date
òòk o
)
òòo p
)
òòp q
throw
ôô 
new
ôô '
InvalidOperationException
ôô 3
(
ôô3 4
$str
ôô4 
)ôô Ä
;ôôÄ Å
if
õõ 
(
õõ 
await
õõ $
_appointmentRepository
õõ ,
.
õõ, -4
&ExistsSamePatientSameSlotSameDateAsync
õõ- S
(
õõS T
	patientId
õõT ]
,
õõ] ^
date
õõ_ c
,
õõc d
timeSlot
õõe m
)
õõm n
)
õõn o
throw
úú 
new
úú '
InvalidOperationException
úú 3
(
úú3 4
$str
úú4 p
)
úúp q
;
úúq r
if
ûû 
(
ûû 
await
ûû $
_appointmentRepository
ûû ,
.
ûû, -3
%ExistsSameDoctorSameSlotSameDateAsync
ûû- R
(
ûûR S
doctorId
ûûS [
,
ûû[ \
date
ûû] a
,
ûûa b
timeSlot
ûûc k
)
ûûk l
)
ûûl m
throw
üü 
new
üü '
InvalidOperationException
üü 3
(
üü3 4
$str
üü4 b
)
üüb c
;
üüc d
}
†† 	
private
¢¢ 
async
¢¢ 
Task
¢¢ (
ValidateUpdateBookingAsync
¢¢ 5
(
¢¢5 6
int
¢¢6 9
appointmentId
¢¢: G
,
¢¢G H
int
¢¢I L
	patientId
¢¢M V
,
¢¢V W
int
¢¢X [
doctorId
¢¢\ d
,
¢¢d e
DateOnly
¢¢f n
date
¢¢o s
,
¢¢s t
int
¢¢u x
timeSlot¢¢y Å
)¢¢Å Ç
{
££ 	
var
§§ 
patient
§§ 
=
§§ 
await
§§  
_patientRepository
§§  2
.
§§2 3
GetByIdAsync
§§3 ?
(
§§? @
	patientId
§§@ I
)
§§I J
;
§§J K
if
¶¶ 
(
¶¶ 
patient
¶¶ 
==
¶¶ 
null
¶¶ 
)
¶¶  
throw
ßß 
new
ßß "
KeyNotFoundException
ßß .
(
ßß. /
$str
ßß/ C
)
ßßC D
;
ßßD E
if
©© 
(
©© 
!
©© 
patient
©© 
.
©© 
IsActive
©© !
)
©©! "
throw
™™ 
new
™™ '
InvalidOperationException
™™ 3
(
™™3 4
$str
™™4 a
)
™™a b
;
™™b c
var
¨¨ 
doctor
¨¨ 
=
¨¨ 
await
¨¨ 
_doctorRepository
¨¨ 0
.
¨¨0 1
GetByIdAsync
¨¨1 =
(
¨¨= >
doctorId
¨¨> F
)
¨¨F G
;
¨¨G H
if
ÆÆ 
(
ÆÆ 
doctor
ÆÆ 
==
ÆÆ 
null
ÆÆ 
)
ÆÆ 
throw
ØØ 
new
ØØ "
KeyNotFoundException
ØØ .
(
ØØ. /
$str
ØØ/ B
)
ØØB C
;
ØØC D
if
±± 
(
±± 
!
±± 
doctor
±± 
.
±± 
IsActive
±±  
)
±±  !
throw
≤≤ 
new
≤≤ '
InvalidOperationException
≤≤ 3
(
≤≤3 4
$str
≤≤4 F
)
≤≤F G
;
≤≤G H
if
¥¥ 
(
¥¥ 
date
¥¥ 
<
¥¥ 
DateOnly
¥¥ 
.
¥¥  
FromDateTime
¥¥  ,
(
¥¥, -
DateTime
¥¥- 5
.
¥¥5 6
Today
¥¥6 ;
)
¥¥; <
)
¥¥< =
throw
µµ 
new
µµ 
ArgumentException
µµ +
(
µµ+ ,
$str
µµ, U
)
µµU V
;
µµV W
if
∑∑ 
(
∑∑ 
!
∑∑ 
Enum
∑∑ 
.
∑∑ 
	IsDefined
∑∑ 
(
∑∑  
typeof
∑∑  &
(
∑∑& '!
AppointmentTimeSlot
∑∑' :
)
∑∑: ;
,
∑∑; <
timeSlot
∑∑= E
)
∑∑E F
)
∑∑F G
throw
∏∏ 
new
∏∏ 
ArgumentException
∏∏ +
(
∏∏+ ,
$str
∏∏, G
)
∏∏G H
;
∏∏H I
if
∫∫ 
(
∫∫ 
await
∫∫ $
_appointmentRepository
∫∫ ,
.
∫∫, -6
(ExistsSamePatientSameDoctorSameDateAsync
∫∫- U
(
∫∫U V
	patientId
∫∫V _
,
∫∫_ `
doctorId
∫∫a i
,
∫∫i j
date
∫∫k o
,
∫∫o p
appointmentId
∫∫q ~
)
∫∫~ 
)∫∫ Ä
throw
ªª 
new
ªª '
InvalidOperationException
ªª 3
(
ªª3 4
$str
ªª4 
)ªª Ä
;ªªÄ Å
if
ΩΩ 
(
ΩΩ 
await
ΩΩ $
_appointmentRepository
ΩΩ ,
.
ΩΩ, -4
&ExistsSamePatientSameSlotSameDateAsync
ΩΩ- S
(
ΩΩS T
	patientId
ΩΩT ]
,
ΩΩ] ^
date
ΩΩ_ c
,
ΩΩc d
timeSlot
ΩΩe m
,
ΩΩm n
appointmentId
ΩΩo |
)
ΩΩ| }
)
ΩΩ} ~
throw
ææ 
new
ææ '
InvalidOperationException
ææ 3
(
ææ3 4
$str
ææ4 p
)
ææp q
;
ææq r
if
¿¿ 
(
¿¿ 
await
¿¿ $
_appointmentRepository
¿¿ ,
.
¿¿, -3
%ExistsSameDoctorSameSlotSameDateAsync
¿¿- R
(
¿¿R S
doctorId
¿¿S [
,
¿¿[ \
date
¿¿] a
,
¿¿a b
timeSlot
¿¿c k
,
¿¿k l
appointmentId
¿¿m z
)
¿¿z {
)
¿¿{ |
throw
¡¡ 
new
¡¡ '
InvalidOperationException
¡¡ 3
(
¡¡3 4
$str
¡¡4 b
)
¡¡b c
;
¡¡c d
}
¬¬ 	
private
ƒƒ 
static
ƒƒ 
AppointmentDto
ƒƒ %!
MapToAppointmentDto
ƒƒ& 9
(
ƒƒ9 :
Appointment
ƒƒ: E
appointment
ƒƒF Q
)
ƒƒQ R
{
≈≈ 	
return
∆∆ 
new
∆∆ 
AppointmentDto
∆∆ %
{
«« 
AppointmentId
»» 
=
»» 
appointment
»»  +
.
»»+ ,
AppointmentId
»», 9
,
»»9 :
	PatientId
…… 
=
…… 
appointment
…… '
.
……' (
	PatientId
……( 1
,
……1 2
DoctorId
   
=
   
appointment
   &
.
  & '
DoctorId
  ' /
,
  / 0
ScheduledDate
ÀÀ 
=
ÀÀ 
appointment
ÀÀ  +
.
ÀÀ+ ,
ScheduledDate
ÀÀ, 9
,
ÀÀ9 :
TimeSlot
ÃÃ 
=
ÃÃ 
(
ÃÃ 
int
ÃÃ 
)
ÃÃ  
appointment
ÃÃ  +
.
ÃÃ+ ,
TimeSlot
ÃÃ, 4
,
ÃÃ4 5
Status
ÕÕ 
=
ÕÕ 
(
ÕÕ 
int
ÕÕ 
)
ÕÕ 
appointment
ÕÕ )
.
ÕÕ) *
Status
ÕÕ* 0
,
ÕÕ0 1 
CancellationReason
ŒŒ "
=
ŒŒ# $
appointment
ŒŒ% 0
.
ŒŒ0 1 
CancellationReason
ŒŒ1 C
}
œœ 
;
œœ 
}
–– 	
private
”” 
static
”” #
AppointmentDetailsDto
”” ,(
MapToAppointmentDetailsDto
””- G
(
””G H
Appointment
””H S
appointment
””T _
)
””_ `
{
‘‘ 	
return
’’ 
new
’’ #
AppointmentDetailsDto
’’ ,
{
÷÷ 
AppointmentId
◊◊ 
=
◊◊ 
appointment
◊◊  +
.
◊◊+ ,
AppointmentId
◊◊, 9
,
◊◊9 :
	PatientId
ÿÿ 
=
ÿÿ 
appointment
ÿÿ '
.
ÿÿ' (
	PatientId
ÿÿ( 1
,
ÿÿ1 2
PatientName
ŸŸ 
=
ŸŸ 
appointment
ŸŸ )
.
ŸŸ) *
Patient
ŸŸ* 1
?
ŸŸ1 2
.
ŸŸ2 3
FullName
ŸŸ3 ;
??
ŸŸ< >
string
ŸŸ? E
.
ŸŸE F
Empty
ŸŸF K
,
ŸŸK L
DoctorId
⁄⁄ 
=
⁄⁄ 
appointment
⁄⁄ &
.
⁄⁄& '
DoctorId
⁄⁄' /
,
⁄⁄/ 0

DoctorName
€€ 
=
€€ 
appointment
€€ (
.
€€( )
Doctor
€€) /
?
€€/ 0
.
€€0 1
FullName
€€1 9
??
€€: <
string
€€= C
.
€€C D
Empty
€€D I
,
€€I J
ScheduledDate
‹‹ 
=
‹‹ 
appointment
‹‹  +
.
‹‹+ ,
ScheduledDate
‹‹, 9
,
‹‹9 :
TimeSlot
›› 
=
›› 
(
›› 
int
›› 
)
››  
appointment
››  +
.
››+ ,
TimeSlot
››, 4
,
››4 5
Status
ﬁﬁ 
=
ﬁﬁ 
(
ﬁﬁ 
int
ﬁﬁ 
)
ﬁﬁ 
appointment
ﬁﬁ )
.
ﬁﬁ) *
Status
ﬁﬁ* 0
,
ﬁﬁ0 1 
CancellationReason
ﬂﬂ "
=
ﬂﬂ# $
appointment
ﬂﬂ% 0
.
ﬂﬂ0 1 
CancellationReason
ﬂﬂ1 C
}
‡‡ 
;
‡‡ 
}
·· 	
private
„„ 
static
„„ #
DoctorScheduleItemDto
„„ ,#
MapDoctorScheduleItem
„„- B
(
„„B C
Appointment
„„C N
appointment
„„O Z
)
„„Z [
{
‰‰ 	
return
ÂÂ 
new
ÂÂ #
DoctorScheduleItemDto
ÂÂ ,
{
ÊÊ 
AppointmentId
ÁÁ 
=
ÁÁ 
appointment
ÁÁ  +
.
ÁÁ+ ,
AppointmentId
ÁÁ, 9
,
ÁÁ9 :
ScheduledDate
ËË 
=
ËË 
appointment
ËË  +
.
ËË+ ,
ScheduledDate
ËË, 9
,
ËË9 :
TimeSlot
ÈÈ 
=
ÈÈ 
(
ÈÈ 
int
ÈÈ 
)
ÈÈ  
appointment
ÈÈ  +
.
ÈÈ+ ,
TimeSlot
ÈÈ, 4
,
ÈÈ4 5
	PatientId
ÍÍ 
=
ÍÍ 
appointment
ÍÍ '
.
ÍÍ' (
	PatientId
ÍÍ( 1
,
ÍÍ1 2
PatientName
ÎÎ 
=
ÎÎ 
appointment
ÎÎ )
.
ÎÎ) *
Patient
ÎÎ* 1
.
ÎÎ1 2
FullName
ÎÎ2 :
,
ÎÎ: ;
Status
ÏÏ 
=
ÏÏ 
(
ÏÏ 
int
ÏÏ 
)
ÏÏ 
appointment
ÏÏ )
.
ÏÏ) *
Status
ÏÏ* 0
}
ÌÌ 
;
ÌÌ 
}
ÓÓ 	
}
ÔÔ 
} ç3
cC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\AdminService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public 

class 
AdminService 
: 
IAdminService  -
{ 
private		 
readonly		 
IAdminRepository		 )
_repository		* 5
;		5 6
public 
AdminService 
( 
IAdminRepository ,

repository- 7
)7 8
{ 	
_repository 
= 

repository $
;$ %
} 	
public 
async 
Task 
< 
AdminDashboardDto +
>+ ,
GetDashboardAsync- >
(> ?
)? @
{ 	
return 
new 
AdminDashboardDto (
{ 
TotalPatients 
= 
await  %
_repository& 1
.1 2
CountPatientsAsync2 D
(D E
)E F
,F G
ActivePatients 
=  
await! &
_repository' 2
.2 3$
CountActivePatientsAsync3 K
(K L
)L M
,M N
TotalDoctors 
= 
await $
_repository% 0
.0 1
CountDoctorsAsync1 B
(B C
)C D
,D E
ActiveDoctors 
= 
await  %
_repository& 1
.1 2#
CountActiveDoctorsAsync2 I
(I J
)J K
,K L
TodayAppointments !
=" #
await$ )
_repository* 5
.5 6'
CountTodayAppointmentsAsync6 Q
(Q R
)R S
,S T
PendingAppointments #
=$ %
await& +
_repository, 7
.7 8)
CountPendingAppointmentsAsync8 U
(U V
)V W
,W X!
CompletedAppointments %
=& '
await( -
_repository. 9
.9 :+
CountCompletedAppointmentsAsync: Y
(Y Z
)Z [
} 
; 
} 	
public 
async 
Task 
< 
AdminStatisticsDto ,
>, -
GetStatisticsAsync. @
(@ A
)A B
{ 	
return   
new   
AdminStatisticsDto   )
{!! 
Patients"" 
="" 
await""  
_repository""! ,
."", -
CountPatientsAsync""- ?
(""? @
)""@ A
,""A B
Doctors## 
=## 
await## 
_repository##  +
.##+ ,
CountDoctorsAsync##, =
(##= >
)##> ?
,##? @
Appointments$$ 
=$$ 
await$$ $
_repository$$% 0
.$$0 1'
CountTodayAppointmentsAsync$$1 L
($$L M
)$$M N
,$$N O
HealthRecords%% 
=%% 
await%%  %
_repository%%& 1
.%%1 2#
CountHealthRecordsAsync%%2 I
(%%I J
)%%J K
}&& 
;&& 
}'' 	
public)) 
async)) 
Task)) 
<)) 
IEnumerable)) %
<))% &
UserManagementDto))& 7
>))7 8
>))8 9
GetUsersAsync)): G
())G H
)))H I
{** 	
var++ 
users++ 
=++ 
await++ 
_repository++ )
.++) *
GetUsersAsync++* 7
(++7 8
)++8 9
;++9 :
var-- 
result-- 
=-- 
new-- 
List-- !
<--! "
UserManagementDto--" 3
>--3 4
(--4 5
)--5 6
;--6 7
foreach// 
(// 
var// 
u// 
in// 
users// #
)//# $
{00 
var11 
roleText11 
=11 
u11  
.11  !
Role11! %
.11% &
ToString11& .
(11. /
)11/ 0
;110 1
result33 
.33 
Add33 
(33 
new33 
UserManagementDto33 0
{44 
UserId55 
=55 
u55 
.55 
UserId55 %
,55% &
Email66 
=66 
u66 
.66 
Email66 #
,66# $
Role77 
=77 
roleText77 #
,77# $
IsActive88 
=88 
await88 $
_repository88% 0
.880 1(
ResolveUserActiveStatusAsync881 M
(88M N
u88N O
.88O P
Email88P U
,88U V
roleText88W _
)88_ `
}99 
)99 
;99 
}:: 
return<< 
result<< 
;<< 
}== 	
public?? 
async?? 
Task?? 
<?? 
UserManagementDto?? +
???+ ,
>??, -
GetUserByIdAsync??. >
(??> ?
int??? B
id??C E
)??E F
{@@ 	
varAA 
userAA 
=AA 
awaitAA 
_repositoryAA (
.AA( )
GetUserByIdAsyncAA) 9
(AA9 :
idAA: <
)AA< =
;AA= >
ifCC 
(CC 
userCC 
==CC 
nullCC 
)CC 
returnDD 
nullDD 
;DD 
varFF 
roleTextFF 
=FF 
userFF 
.FF  
RoleFF  $
.FF$ %
ToStringFF% -
(FF- .
)FF. /
;FF/ 0
returnHH 
newHH 
UserManagementDtoHH (
{II 
UserIdJJ 
=JJ 
userJJ 
.JJ 
UserIdJJ $
,JJ$ %
EmailKK 
=KK 
userKK 
.KK 
EmailKK "
,KK" #
RoleLL 
=LL 
roleTextLL 
,LL  
IsActiveMM 
=MM 
awaitMM  
_repositoryMM! ,
.MM, -(
ResolveUserActiveStatusAsyncMM- I
(MMI J
userMMJ N
.MMN O
EmailMMO T
,MMT U
roleTextMMV ^
)MM^ _
}NN 
;NN 
}OO 	
}PP 
}QQ ¢
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IUserRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IUserRepository $
{ 
Task 
< 
User 
? 
> 
GetByIdAsync  
(  !
int! $
id% '
)' (
;( )
Task		 
<		 
User		 
?		 
>		 
GetByEmailAsync		 #
(		# $
string		$ *
email		+ 0
)		0 1
;		1 2
Task

 
<

 
IEnumerable

 
<

 
User

 
>

 
>

 
GetByRoleAsync

  .
(

. /
UserRole

/ 7
role

8 <
)

< =
;

= >
Task 
AddAsync 
( 
User 
user 
)  
;  !
Task 
UpdateAsync 
( 
User 
user "
)" #
;# $
Task 
< 
bool 
> 
EmailExistsAsync #
(# $
string$ *
email+ 0
)0 1
;1 2
Task 
SaveChangesAsync 
( 
) 
;  
Task 
< 
User 
? 
> "
GetByRefreshTokenAsync *
(* +
string+ 1
refreshToken2 >
)> ?
;? @
} 
} ‡
hC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IPatientRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IPatientRepository '
{ 
Task 
< 
IEnumerable 
< 
Patient  
>  !
>! "
GetAllAsync# .
(. /
)/ 0
;0 1
Task 
< 
Patient 
? 
> 
GetByIdAsync #
(# $
int$ '
id( *
)* +
;+ ,
Task		 
<		 
IEnumerable		 
<		 
Patient		  
>		  !
>		! "
SearchByNameAsync		# 4
(		4 5
string		5 ;
name		< @
)		@ A
;		A B
Task

 
AddAsync

 
(

 
Patient

 
patient

 %
)

% &
;

& '
Task 
UpdateAsync 
( 
Patient  
patient! (
)( )
;) *
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ı
mC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IHealthRecordRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface #
IHealthRecordRepository ,
{ 
Task 
< 
HealthRecord 
? 
> 
GetByIdAsync (
(( )
int) ,
id- /
)/ 0
;0 1
Task 
< 
HealthRecord 
? 
> #
GetByAppointmentIdAsync 3
(3 4
int4 7
appointmentId8 E
)E F
;F G
Task		 
AddAsync		 
(		 
HealthRecord		 "
record		# )
)		) *
;		* +
Task

 
UpdateAsync

 
(

 
HealthRecord

 %
record

& ,
)

, -
;

- .
Task 
SaveChangesAsync 
( 
) 
;  
} 
} Â

hC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IGenericRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IGenericRepository '
<' (
T( )
>) *
where+ 0
T1 2
:3 4
class5 :
{ 
Task 
< 
IEnumerable 
< 
T 
> 
> 
GetAllAsync (
(( )
)) *
;* +
Task 
< 
T 
? 
> 
GetByIdAsync 
( 
int !
id" $
)$ %
;% &
Task		 
AddAsync		 
(		 
T		 
entity		 
)		 
;		  
Task 
UpdateAsync 
( 
T 
entity !
)! "
;" #
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
} 
} ë
gC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IDoctorRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IDoctorRepository &
{ 
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !
GetAllAsync" -
(- .
string. 4
?4 5
sortBy6 <
,< =
int> A
?A B
specialisationC Q
)Q R
;R S
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !*
GetActiveBySpecialisationAsync" @
(@ A
intA D
specialisationE S
)S T
;T U
Task		 
<		 
Doctor		 
?		 
>		 
GetByIdAsync		 "
(		" #
int		# &
id		' )
)		) *
;		* +
Task

 
AddAsync

 
(

 
Doctor

 
doctor

 #
)

# $
;

$ %
Task 
UpdateAsync 
( 
Doctor 
doctor  &
)& '
;' (
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
< 
IEnumerable 
< 
int 
> 
> 
GetBookedSlotsAsync 2
(2 3
int3 6
doctorId7 ?
,? @
DateOnly@ H
dateI M
)M N
;N O
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ∞!
lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IAppointmentRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface "
IAppointmentRepository +
{ 
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &
GetAllAsync' 2
(2 3
)3 4
;4 5
Task 
< 
Appointment 
? 
> 
GetByIdAsync '
(' (
int( +
id, .
). /
;/ 0
Task		 
<		 
IEnumerable		 
<		 
Appointment		 $
>		$ %
>		% &
GetByPatientIdAsync		' :
(		: ;
int		; >
	patientId		? H
)		H I
;		I J
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &'
GetDoctorTodayScheduleAsync' B
(B C
intC F
doctorIdG O
,O P
DateOnlyQ Y
todayZ _
)_ `
;` a
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &&
GetDoctorWeekScheduleAsync' A
(A B
intB E
doctorIdF N
,N O
DateOnlyP X
	startDateY b
,b c
DateOnlyd l
endDatem t
)t u
;u v
Task 
< 
bool 
> 4
(ExistsSamePatientSameDoctorSameDateAsync ;
(; <
int< ?
	patientId@ I
,I J
intK N
doctorIdO W
,W X
DateOnlyY a
dateb f
)f g
;g h
Task 
< 
bool 
> 2
&ExistsSamePatientSameSlotSameDateAsync 9
(9 :
int: =
	patientId> G
,G H
DateOnlyI Q
dateR V
,V W
intX [
timeSlot\ d
)d e
;e f
Task 
< 
bool 
> 1
%ExistsSameDoctorSameSlotSameDateAsync 8
(8 9
int9 <
doctorId= E
,E F
DateOnlyG O
dateP T
,T U
intV Y
timeSlotZ b
)b c
;c d
Task 
< 
bool 
> 4
(ExistsSamePatientSameDoctorSameDateAsync ;
(; <
int< ?
	patientId@ I
,I J
intK N
doctorIdO W
,W X
DateOnlyY a
dateb f
,f g
inth k
appointmentIdl y
)y z
;z {
Task 
< 
bool 
> 2
&ExistsSamePatientSameSlotSameDateAsync 9
(9 :
int: =
	patientId> G
,G H
DateOnlyI Q
dateR V
,V W
intX [
timeSlot\ d
,d e
intf i
appointmentIdj w
)w x
;x y
Task 
< 
bool 
> 1
%ExistsSameDoctorSameSlotSameDateAsync 8
(8 9
int9 <
doctorId= E
,E F
DateOnlyG O
dateP T
,T U
intV Y
timeSlotZ b
,b c
intd g
appointmentIdh u
)u v
;v w
Task 
AddAsync 
( 
Appointment !
appointment" -
)- .
;. /
Task 
UpdateAsync 
( 
Appointment $
appointment% 0
)0 1
;1 2
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ¸
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Interface\IAdminRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
	Interface& /
{ 
public 

	interface 
IAdminRepository %
{ 
Task 
< 
int 
> 
CountPatientsAsync $
($ %
)% &
;& '
Task		 
<		 
int		 
>		 $
CountActivePatientsAsync		 *
(		* +
)		+ ,
;		, -
Task 
< 
int 
> 
CountDoctorsAsync #
(# $
)$ %
;% &
Task 
< 
int 
> #
CountActiveDoctorsAsync )
() *
)* +
;+ ,
Task 
< 
int 
> '
CountTodayAppointmentsAsync -
(- .
). /
;/ 0
Task 
< 
int 
> )
CountPendingAppointmentsAsync /
(/ 0
)0 1
;1 2
Task 
< 
int 
> +
CountCompletedAppointmentsAsync 1
(1 2
)2 3
;3 4
Task 
< 
int 
> #
CountHealthRecordsAsync )
() *
)* +
;+ ,
Task 
< 
IEnumerable 
< 
User 
> 
> 
GetUsersAsync  -
(- .
). /
;/ 0
Task 
< 
bool 
> (
ResolveUserActiveStatusAsync /
(/ 0
string0 6
email7 <
,< =
string> D
roleE I
)I J
;J K
Task 
< 
User 
? 
> 
GetUserByIdAsync $
($ %
int% (
id) +
)+ ,
;, -
} 
} €$
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\UserRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{		 
[

 #
ExcludeFromCodeCoverage

 
]

 
public 

class 
UserRepository 
:  !
IUserRepository" 1
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
UserRepository 
( 
HealthAxisDbContext 1
context2 9
)9 :
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByIdAsync! -
(- .
int. 1
id2 4
)4 5
{ 	
return 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
UserId, 2
==3 5
id6 8
)8 9
;9 :
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByEmailAsync! 0
(0 1
string1 7
email8 =
)= >
{ 	
return 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
Email, 1
==2 4
email5 :
): ;
;; <
} 	
public   
async   
Task   
<   
IEnumerable   %
<  % &
User  & *
>  * +
>  + ,
GetByRoleAsync  - ;
(  ; <
UserRole  < D
role  E I
)  I J
{!! 	
return"" 
await"" 
_context"" !
.""! "
Users""" '
.## 
Where## 
(## 
u## 
=>## 
u## 
.## 
Role## "
==### %
role##& *
)##* +
.$$ 
ToListAsync$$ 
($$ 
)$$ 
;$$ 
}%% 	
public'' 
async'' 
Task'' 
AddAsync'' "
(''" #
User''# '
user''( ,
)'', -
{(( 	
await)) 
_context)) 
.)) 
Users))  
.))  !
AddAsync))! )
())) *
user))* .
))). /
;))/ 0
}** 	
public,, 
Task,, 
UpdateAsync,, 
(,,  
User,,  $
user,,% )
),,) *
{-- 	
_context.. 
... 
Users.. 
... 
Update.. !
(..! "
user.." &
)..& '
;..' (
return// 
Task// 
.// 
CompletedTask// %
;//% &
}00 	
public22 
async22 
Task22 
<22 
bool22 
>22 
EmailExistsAsync22  0
(220 1
string221 7
email228 =
)22= >
{33 	
return44 
await44 
_context44 !
.44! "
Users44" '
.55 
AnyAsync55 
(55 
u55 
=>55 
u55  
.55  !
Email55! &
==55' )
email55* /
)55/ 0
;550 1
}66 	
public88 
async88 
Task88 
SaveChangesAsync88 *
(88* +
)88+ ,
{99 	
await:: 
_context:: 
.:: 
SaveChangesAsync:: +
(::+ ,
)::, -
;::- .
};; 	
public== 
async== 
Task== 
<== 
User== 
?== 
>==  "
GetByRefreshTokenAsync==! 7
(==7 8
string==8 >
refreshToken==? K
)==K L
{>> 	
return?? 
await?? 
_context?? !
.??! "
Users??" '
.@@ 
FirstOrDefaultAsync@@ $
(@@$ %
u@@% &
=>@@' )
u@@* +
.@@+ ,
RefreshToken@@, 8
==@@9 ;
refreshToken@@< H
)@@H I
;@@I J
}AA 	
}BB 
}CC ¨&
lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\PatientRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
[		 #
ExcludeFromCodeCoverage		 
]		 
public

 

class

 
PatientRepository

 "
:

# $
IPatientRepository

% 7
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
PatientRepository  
(  !
HealthAxisDbContext! 4
context5 <
)< =
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Patient& -
>- .
>. /
GetAllAsync0 ;
(; <
)< =
{ 	
return 
await 
_context !
.! "
Patients" *
. 
OrderBy 
( 
p 
=> 
p 
.  
	PatientId  )
)) *
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByIdAsync$ 0
(0 1
int1 4
id5 7
)7 8
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
	PatientId, 5
==6 8
id9 ;
); <
;< =
} 	
public   
async   
Task   
<   
IEnumerable   %
<  % &
Patient  & -
>  - .
>  . /
SearchByNameAsync  0 A
(  A B
string  B H
name  I M
)  M N
{!! 	
if"" 
("" 
string"" 
."" 
IsNullOrWhiteSpace"" )
("") *
name""* .
)"". /
)""/ 0
return## 
new## 
List## 
<##  
Patient##  '
>##' (
(##( )
)##) *
;##* +
name%% 
=%% 
name%% 
.%% 
Trim%% 
(%% 
)%% 
;%% 
return'' 
await'' 
_context'' !
.''! "
Patients''" *
.(( 
Where(( 
((( 
p(( 
=>(( 
p(( 
.(( 
IsActive(( &
&&((' )
p((* +
.((+ ,
FullName((, 4
.((4 5
Contains((5 =
(((= >
name((> B
)((B C
)((C D
.)) 
OrderBy)) 
()) 
p)) 
=>)) 
p)) 
.))  
FullName))  (
)))( )
.** 
ToListAsync** 
(** 
)** 
;** 
}++ 	
public-- 
async-- 
Task-- 
AddAsync-- "
(--" #
Patient--# *
patient--+ 2
)--2 3
{.. 	
await// 
_context// 
.// 
Patients// #
.//# $
AddAsync//$ ,
(//, -
patient//- 4
)//4 5
;//5 6
}00 	
public22 
Task22 
UpdateAsync22 
(22  
Patient22  '
patient22( /
)22/ 0
{33 	
_context44 
.44 
Patients44 
.44 
Update44 $
(44$ %
patient44% ,
)44, -
;44- .
return55 
Task55 
.55 
CompletedTask55 %
;55% &
}66 	
public88 
async88 
Task88 
<88 
bool88 
>88 
ExistsAsync88  +
(88+ ,
int88, /
id880 2
)882 3
{99 	
return:: 
await:: 
_context:: !
.::! "
Patients::" *
.::* +
AnyAsync::+ 3
(::3 4
p::4 5
=>::6 8
p::9 :
.::: ;
	PatientId::; D
==::E G
id::H J
)::J K
;::K L
};; 	
public== 
async== 
Task== 
SaveChangesAsync== *
(==* +
)==+ ,
{>> 	
await?? 
_context?? 
.?? 
SaveChangesAsync?? +
(??+ ,
)??, -
;??- .
}@@ 	
}AA 
}BB ñ
gC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\HealthRecord.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
[		 #
ExcludeFromCodeCoverage		 
]		 
public

 

class

 "
HealthRecordRepository

 '
:

( )#
IHealthRecordRepository

* A
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public "
HealthRecordRepository %
(% &
HealthAxisDbContext& 9
context: A
)A B
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
HealthRecord &
?& '
>' (
GetByIdAsync) 5
(5 6
int6 9
id: <
)< =
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
. 
Include 
( 
hr 
=> 
hr !
.! "
Doctor" (
)( )
. 
Include 
( 
hr 
=> 
hr !
.! "
Appointment" -
)- .
. 
FirstOrDefaultAsync $
($ %
hr% '
=>( *
hr+ -
.- .
HealthRecordId. <
=== ?
id@ B
)B C
;C D
} 	
public 
async 
Task 
< 
HealthRecord &
?& '
>' (#
GetByAppointmentIdAsync) @
(@ A
intA D
appointmentIdE R
)R S
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
.   
Include   
(   
hr   
=>   
hr   !
.  ! "
Doctor  " (
)  ( )
.!! 
Include!! 
(!! 
hr!! 
=>!! 
hr!! !
.!!! "
Appointment!!" -
)!!- .
."" 
FirstOrDefaultAsync"" $
(""$ %
hr""% '
=>""( *
hr""+ -
.""- .
AppointmentId"". ;
==""< >
appointmentId""? L
)""L M
;""M N
}## 	
public%% 
async%% 
Task%% 
AddAsync%% "
(%%" #
HealthRecord%%# /
record%%0 6
)%%6 7
{&& 	
await'' 
_context'' 
.'' 
HealthRecords'' (
.''( )
AddAsync'') 1
(''1 2
record''2 8
)''8 9
;''9 :
}(( 	
public** 
Task** 
UpdateAsync** 
(**  
HealthRecord**  ,
record**- 3
)**3 4
{++ 	
_context,, 
.,, 
HealthRecords,, "
.,," #
Update,,# )
(,,) *
record,,* 0
),,0 1
;,,1 2
return-- 
Task-- 
.-- 
CompletedTask-- %
;--% &
}.. 	
public00 
async00 
Task00 
SaveChangesAsync00 *
(00* +
)00+ ,
{11 	
await22 
_context22 
.22 
SaveChangesAsync22 +
(22+ ,
)22, -
;22- .
}33 	
}44 
}55 °!
lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\GenericRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
[ #
ExcludeFromCodeCoverage 
] 
public		 

class		 
GenericRepository		 "
<		" #
T		# $
>		$ %
:		& '
IGenericRepository		( :
<		: ;
T		; <
>		< =
where

 
T

 
:

 
class

 
{ 
	protected 
readonly 
HealthAxisDbContext .
_context/ 7
;7 8
	protected 
readonly 
DbSet  
<  !
T! "
>" #
_dbSet$ *
;* +
public 
GenericRepository  
(  !
HealthAxisDbContext! 4
context5 <
)< =
{ 	
_context 
= 
context 
; 
_dbSet 
= 
context 
. 
Set  
<  !
T! "
>" #
(# $
)$ %
;% &
} 	
public 
virtual 
async 
Task !
<! "
IEnumerable" -
<- .
T. /
>/ 0
>0 1
GetAllAsync2 =
(= >
)> ?
{ 	
return 
await 
_dbSet 
.  
ToListAsync  +
(+ ,
), -
;- .
} 	
public 
virtual 
async 
Task !
<! "
T" #
?# $
>$ %
GetByIdAsync& 2
(2 3
int3 6
id7 9
)9 :
{ 	
return 
await 
_dbSet 
.  
	FindAsync  )
() *
id* ,
), -
;- .
} 	
public 
virtual 
async 
Task !
AddAsync" *
(* +
T+ ,
entity- 3
)3 4
{   	
await!! 
_dbSet!! 
.!! 
AddAsync!! !
(!!! "
entity!!" (
)!!( )
;!!) *
await"" 
_context"" 
."" 
SaveChangesAsync"" +
(""+ ,
)"", -
;""- .
}## 	
public%% 
virtual%% 
async%% 
Task%% !
UpdateAsync%%" -
(%%- .
T%%. /
entity%%0 6
)%%6 7
{&& 	
_dbSet'' 
.'' 
Update'' 
('' 
entity''  
)''  !
;''! "
await(( 
_context(( 
.(( 
SaveChangesAsync(( +
(((+ ,
)((, -
;((- .
})) 	
public++ 
virtual++ 
async++ 
Task++ !
DeleteAsync++" -
(++- .
int++. 1
id++2 4
)++4 5
{,, 	
var-- 
entity-- 
=-- 
await-- 
_dbSet-- %
.--% &
	FindAsync--& /
(--/ 0
id--0 2
)--2 3
;--3 4
if// 
(// 
entity// 
!=// 
null// 
)// 
{00 
_dbSet11 
.11 
Remove11 
(11 
entity11 $
)11$ %
;11% &
await22 
_context22 
.22 
SaveChangesAsync22 /
(22/ 0
)220 1
;221 2
}33 
}44 	
public66 
virtual66 
async66 
Task66 !
<66! "
bool66" &
>66& '
ExistsAsync66( 3
(663 4
int664 7
id668 :
)66: ;
{77 	
return88 
await88 
_dbSet88 
.88  
	FindAsync88  )
(88) *
id88* ,
)88, -
!=88. 0
null881 5
;885 6
}99 	
}:: 
};; ˚@
kC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\DoctorRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{		 
[

 #
ExcludeFromCodeCoverage

 
]

 
public 

class 
DoctorRepository !
:" #
IDoctorRepository$ 5
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
DoctorRepository 
(  
HealthAxisDbContext  3
context4 ;
); <
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Doctor& ,
>, -
>- .
GetAllAsync/ :
(: ;
string; A
?A B
sortByC I
,I J
intK N
?N O
specialisationP ^
)^ _
{ 	

IQueryable 
< 
Doctor 
> 
query $
=% &
_context' /
./ 0
Doctors0 7
;7 8
if 
( 
specialisation 
. 
HasValue '
)' (
{ 
if 
( 
! 
Enum 
. 
	IsDefined #
(# $
typeof$ *
(* + 
DoctorSpecialisation+ ?
)? @
,@ A
specialisationB P
.P Q
ValueQ V
)V W
)W X
throw 
new 
ArgumentException /
(/ 0
$str0 P
)P Q
;Q R
var  
doctorSpecialisation (
=) *
(+ , 
DoctorSpecialisation, @
)@ A
specialisationA O
.O P
ValueP U
;U V
query 
= 
query 
. 
Where #
(# $
d$ %
=>& (
d) *
.* +
Specialisation+ 9
==: < 
doctorSpecialisation= Q
)Q R
;R S
} 
query!! 
=!! 
sortBy!! 
?!! 
.!! 
ToLower!! #
(!!# $
)!!$ %
switch!!& ,
{"" 
$str## 
=>## 
query## 
.##  
OrderBy##  '
(##' (
d##( )
=>##* ,
d##- .
.##. /
FullName##/ 7
)##7 8
,##8 9
$str$$ 
=>$$ 
query$$ $
.$$$ %
OrderByDescending$$% 6
($$6 7
d$$7 8
=>$$9 ;
d$$< =
.$$= >
FullName$$> F
)$$F G
,$$G H
$str%% 
=>%% 
query%% 
.%% 
OrderBy%% %
(%%% &
d%%& '
=>%%( *
d%%+ ,
.%%, -
DoctorId%%- 5
)%%5 6
,%%6 7
_&& 
=>&& 
query&& 
.&& 
OrderBy&& "
(&&" #
d&&# $
=>&&% '
d&&( )
.&&) *
DoctorId&&* 2
)&&2 3
}'' 
;'' 
return)) 
await)) 
query)) 
.)) 
ToListAsync)) *
())* +
)))+ ,
;)), -
}** 	
public,, 
async,, 
Task,, 
<,, 
IEnumerable,, %
<,,% &
Doctor,,& ,
>,,, -
>,,- .*
GetActiveBySpecialisationAsync,,/ M
(,,M N
int,,N Q
specialisation,,R `
),,` a
{-- 	
if.. 
(.. 
!.. 
Enum.. 
... 
	IsDefined.. 
(..  
typeof..  &
(..& ' 
DoctorSpecialisation..' ;
)..; <
,..< =
specialisation..> L
)..L M
)..M N
throw// 
new// 
ArgumentException// +
(//+ ,
$str//, L
)//L M
;//M N
var11  
doctorSpecialisation11 $
=11% &
(11' ( 
DoctorSpecialisation11( <
)11< =
specialisation11= K
;11K L
return33 
await33 
_context33 !
.33! "
Doctors33" )
.44 
Where44 
(44 
d44 
=>44 
d44 
.44 
IsActive44 &
&&44' )
d44* +
.44+ ,
Specialisation44, :
==44; = 
doctorSpecialisation44> R
)44R S
.55 
OrderBy55 
(55 
d55 
=>55 
d55 
.55  
FullName55  (
)55( )
.66 
ToListAsync66 
(66 
)66 
;66 
}77 	
public99 
async99 
Task99 
<99 
Doctor99  
?99  !
>99! "
GetByIdAsync99# /
(99/ 0
int990 3
id994 6
)996 7
{:: 	
return;; 
await;; 
_context;; !
.;;! "
Doctors;;" )
.;;) *
	FindAsync;;* 3
(;;3 4
id;;4 6
);;6 7
;;;7 8
}<< 	
public>> 
async>> 
Task>> 
AddAsync>> "
(>>" #
Doctor>># )
doctor>>* 0
)>>0 1
{?? 	
await@@ 
_context@@ 
.@@ 
Doctors@@ "
.@@" #
AddAsync@@# +
(@@+ ,
doctor@@, 2
)@@2 3
;@@3 4
}AA 	
publicCC 
TaskCC 
UpdateAsyncCC 
(CC  
DoctorCC  &
doctorCC' -
)CC- .
{DD 	
_contextEE 
.EE 
DoctorsEE 
.EE 
UpdateEE #
(EE# $
doctorEE$ *
)EE* +
;EE+ ,
returnFF 
TaskFF 
.FF 
CompletedTaskFF %
;FF% &
}GG 	
publicII 
asyncII 
TaskII 
<II 
IEnumerableII %
<II% &
intII& )
>II) *
>II* +
GetBookedSlotsAsyncII, ?
(II? @
intII@ C
doctorIdIID L
,IIL M
DateOnlyIIM U
dateIIV Z
)IIZ [
{JJ 	
returnKK 
awaitKK 
_contextKK !
.KK! "
AppointmentsKK" .
.LL 
WhereLL 
(LL 
aLL 
=>LL 
aMM 
.MM 
DoctorIdMM 
==MM !
doctorIdMM" *
&&MM+ -
aNN 
.NN 
ScheduledDateNN #
==NN$ &
dateNN' +
&&NN, .
aOO 
.OO 
StatusOO 
!=OO 
AppointmentStatusOO  1
.OO1 2
	CancelledOO2 ;
)OO; <
.PP 
SelectPP 
(PP 
aPP 
=>PP 
(PP 
intPP !
)PP! "
aPP" #
.PP# $
TimeSlotPP$ ,
)PP, -
.QQ 
ToListAsyncQQ 
(QQ 
)QQ 
;QQ 
}RR 	
publicTT 
asyncTT 
TaskTT 
<TT 
boolTT 
>TT 
ExistsAsyncTT  +
(TT+ ,
intTT, /
idTT0 2
)TT2 3
{UU 	
returnVV 
awaitVV 
_contextVV !
.VV! "
DoctorsVV" )
.VV) *
AnyAsyncVV* 2
(VV2 3
dVV3 4
=>VV5 7
dVV8 9
.VV9 :
DoctorIdVV: B
==VVC E
idVVF H
)VVH I
;VVI J
}WW 	
publicYY 
asyncYY 
TaskYY 
SaveChangesAsyncYY *
(YY* +
)YY+ ,
{ZZ 	
await[[ 
_context[[ 
.[[ 
SaveChangesAsync[[ +
([[+ ,
)[[, -
;[[- .
}\\ 	
}]] 
}^^ ”
pC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\AppointmentRepository.cs
	namespace		 	
S3_HealthAxisApi		
 
.		 

Repository		 %
.		% &
Implementation		& 4
{

 
[ #
ExcludeFromCodeCoverage 
] 
public 

class !
AppointmentRepository &
:' ("
IAppointmentRepository) ?
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public !
AppointmentRepository $
($ %
HealthAxisDbContext% 8
context9 @
)@ A
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Appointment& 1
>1 2
>2 3
GetAllAsync4 ?
(? @
)@ A
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
. 
Include 
( 
a 
=> 
a 
.  
Doctor  &
)& '
. 
OrderByDescending "
(" #
a# $
=>% '
a( )
.) *
ScheduledDate* 7
)7 8
. 
ThenBy 
( 
a 
=> 
a 
. 
TimeSlot '
)' (
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
Appointment %
?% &
>& '
GetByIdAsync( 4
(4 5
int5 8
id9 ;
); <
{   	
return!! 
await!! 
_context!! !
.!!! "
Appointments!!" .
."" 
Include"" 
("" 
a"" 
=>"" 
a"" 
.""  
Patient""  '
)""' (
.## 
Include## 
(## 
a## 
=>## 
a## 
.##  
Doctor##  &
)##& '
.$$ 
FirstOrDefaultAsync$$ $
($$$ %
a$$% &
=>$$' )
a$$* +
.$$+ ,
AppointmentId$$, 9
==$$: <
id$$= ?
)$$? @
;$$@ A
}%% 	
public'' 
async'' 
Task'' 
<'' 
IEnumerable'' %
<''% &
Appointment''& 1
>''1 2
>''2 3
GetByPatientIdAsync''4 G
(''G H
int''H K
	patientId''L U
)''U V
{(( 	
return)) 
await)) 
_context)) !
.))! "
Appointments))" .
.** 
Include** 
(** 
a** 
=>** 
a** 
.**  
Doctor**  &
)**& '
.++ 
Where++ 
(++ 
a++ 
=>++ 
a++ 
.++ 
	PatientId++ '
==++( *
	patientId+++ 4
)++4 5
.,, 
OrderByDescending,, "
(,," #
a,,# $
=>,,% '
a,,( )
.,,) *
ScheduledDate,,* 7
),,7 8
.-- 
ThenBy-- 
(-- 
a-- 
=>-- 
a-- 
.-- 
TimeSlot-- '
)--' (
... 
ToListAsync.. 
(.. 
).. 
;.. 
}// 	
public11 
async11 
Task11 
<11 
IEnumerable11 %
<11% &
Appointment11& 1
>111 2
>112 3'
GetDoctorTodayScheduleAsync114 O
(11O P
int11P S
doctorId11T \
,11\ ]
DateOnly11^ f
today11g l
)11l m
{22 	
return33 
await33 
_context33 !
.33! "
Appointments33" .
.44 
Include44 
(44 
a44 
=>44 
a44 
.44  
Patient44  '
)44' (
.55 
Where55 
(55 
a55 
=>55 
a55 
.55 
DoctorId55 &
==55' )
doctorId55* 2
&&553 5
a556 7
.557 8
ScheduledDate558 E
==55F H
today55I N
)55N O
.66 
OrderBy66 
(66 
a66 
=>66 
a66 
.66  
TimeSlot66  (
)66( )
.77 
ToListAsync77 
(77 
)77 
;77 
}88 	
public:: 
async:: 
Task:: 
<:: 
IEnumerable:: %
<::% &
Appointment::& 1
>::1 2
>::2 3&
GetDoctorWeekScheduleAsync::4 N
(::N O
int::O R
doctorId::S [
,::[ \
DateOnly::] e
	startDate::f o
,::o p
DateOnly::q y
endDate	::z Å
)
::Å Ç
{;; 	
return<< 
await<< 
_context<< !
.<<! "
Appointments<<" .
.== 
Include== 
(== 
a== 
=>== 
a== 
.==  
Patient==  '
)==' (
.>> 
Where>> 
(>> 
a>> 
=>>> 
a>> 
.>> 
DoctorId>> &
==>>' )
doctorId>>* 2
&&>>3 5
a?? 
.?? 
ScheduledDate?? +
>=??, .
	startDate??/ 8
&&??9 ;
a@@ 
.@@ 
ScheduledDate@@ +
<=@@, .
endDate@@/ 6
)@@6 7
.AA 
OrderByAA 
(AA 
aAA 
=>AA 
aAA 
.AA  
ScheduledDateAA  -
)AA- .
.BB 
ThenByBB 
(BB 
aBB 
=>BB 
aBB 
.BB 
TimeSlotBB '
)BB' (
.CC 
ToListAsyncCC 
(CC 
)CC 
;CC 
}DD 	
publicFF 
asyncFF 
TaskFF 
<FF 
boolFF 
>FF 4
(ExistsSamePatientSameDoctorSameDateAsyncFF  H
(FFH I
intFFI L
	patientIdFFM V
,FFV W
intFFX [
doctorIdFF\ d
,FFd e
DateOnlyFFf n
dateFFo s
)FFs t
{GG 	
returnHH 
awaitHH 
_contextHH !
.HH! "
AppointmentsHH" .
.HH. /
AnyAsyncHH/ 7
(HH7 8
aHH8 9
=>HH: <
aII 
.II 
	PatientIdII 
==II 
	patientIdII (
&&II) +
aJJ 
.JJ 
DoctorIdJJ 
==JJ 
doctorIdJJ &
&&JJ' )
aKK 
.KK 
ScheduledDateKK 
==KK  "
dateKK# '
&&KK( *
aLL 
.LL 
StatusLL 
!=LL 
AppointmentStatusLL -
.LL- .
	CancelledLL. 7
)LL7 8
;LL8 9
}MM 	
publicOO 
asyncOO 
TaskOO 
<OO 
boolOO 
>OO 2
&ExistsSamePatientSameSlotSameDateAsyncOO  F
(OOF G
intOOG J
	patientIdOOK T
,OOT U
DateOnlyOOV ^
dateOO_ c
,OOc d
intOOe h
timeSlotOOi q
)OOq r
{PP 	
ifQQ 
(QQ 
!QQ 
EnumQQ 
.QQ 
	IsDefinedQQ 
(QQ  
typeofQQ  &
(QQ& '
AppointmentTimeSlotQQ' :
)QQ: ;
,QQ; <
timeSlotQQ= E
)QQE F
)QQF G
throwRR 
newRR 
ArgumentExceptionRR +
(RR+ ,
$strRR, L
)RRL M
;RRM N
varTT 
slotEnumTT 
=TT 
(TT 
AppointmentTimeSlotTT /
)TT/ 0
timeSlotTT0 8
;TT8 9
returnVV 
awaitVV 
_contextVV !
.VV! "
AppointmentsVV" .
.VV. /
AnyAsyncVV/ 7
(VV7 8
aVV8 9
=>VV: <
aWW 
.WW 
	PatientIdWW 
==WW 
	patientIdWW (
&&WW) +
aXX 
.XX 
ScheduledDateXX 
==XX  "
dateXX# '
&&XX( *
aYY 
.YY 
TimeSlotYY 
==YY 
slotEnumYY &
&&YY' )
aZZ 
.ZZ 
StatusZZ 
!=ZZ 
AppointmentStatusZZ -
.ZZ- .
	CancelledZZ. 7
)ZZ7 8
;ZZ8 9
}[[ 	
public]] 
async]] 
Task]] 
<]] 
bool]] 
>]] 1
%ExistsSameDoctorSameSlotSameDateAsync]]  E
(]]E F
int]]F I
doctorId]]J R
,]]R S
DateOnly]]T \
date]]] a
,]]a b
int]]c f
timeSlot]]g o
)]]o p
{^^ 	
if__ 
(__ 
!__ 
Enum__ 
.__ 
	IsDefined__ 
(__  
typeof__  &
(__& '
AppointmentTimeSlot__' :
)__: ;
,__; <
timeSlot__= E
)__E F
)__F G
throw`` 
new`` 
ArgumentException`` +
(``+ ,
$str``, L
)``L M
;``M N
varbb 
slotEnumbb 
=bb 
(bb 
AppointmentTimeSlotbb /
)bb/ 0
timeSlotbb0 8
;bb8 9
returndd 
awaitdd 
_contextdd !
.dd! "
Appointmentsdd" .
.dd. /
AnyAsyncdd/ 7
(dd7 8
add8 9
=>dd: <
aee 
.ee 
DoctorIdee 
==ee 
doctorIdee &
&&ee' )
aff 
.ff 
ScheduledDateff 
==ff  "
dateff# '
&&ff( *
agg 
.gg 
TimeSlotgg 
==gg 
slotEnumgg &
&&gg' )
ahh 
.hh 
Statushh 
!=hh 
AppointmentStatushh -
.hh- .
	Cancelledhh. 7
)hh7 8
;hh8 9
}ii 	
publickk 
asynckk 
Taskkk 
<kk 
boolkk 
>kk 4
(ExistsSamePatientSameDoctorSameDateAsynckk  H
(kkH I
intkkI L
	patientIdkkM V
,kkV W
intkkX [
doctorIdkk\ d
,kkd e
DateOnlykkf n
datekko s
,kks t
intkku x
appointmentId	kky Ü
)
kkÜ á
{ll 	
returnmm 
awaitmm 
_contextmm !
.mm! "
Appointmentsmm" .
.mm. /
AnyAsyncmm/ 7
(mm7 8
amm8 9
=>mm: <
ann 
.nn 
AppointmentIdnn 
!=nn  "
appointmentIdnn# 0
&&nn1 3
aoo 
.oo 
	PatientIdoo 
==oo 
	patientIdoo (
&&oo) +
app 
.pp 
DoctorIdpp 
==pp 
doctorIdpp &
&&pp' )
aqq 
.qq 
ScheduledDateqq 
==qq  "
dateqq# '
&&qq( *
arr 
.rr 
Statusrr 
!=rr 
AppointmentStatusrr -
.rr- .
	Cancelledrr. 7
)rr7 8
;rr8 9
}ss 	
publicuu 
asyncuu 
Taskuu 
<uu 
booluu 
>uu 2
&ExistsSamePatientSameSlotSameDateAsyncuu  F
(uuF G
intuuG J
	patientIduuK T
,uuT U
DateOnlyuuV ^
dateuu_ c
,uuc d
intuue h
timeSlotuui q
,uuq r
intuus v
appointmentId	uuw Ñ
)
uuÑ Ö
{vv 	
returnww 
awaitww 
_contextww !
.ww! "
Appointmentsww" .
.ww. /
AnyAsyncww/ 7
(ww7 8
aww8 9
=>ww: <
axx 
.xx 
AppointmentIdxx 
!=xx  "
appointmentIdxx# 0
&&xx1 3
ayy 
.yy 
	PatientIdyy 
==yy 
	patientIdyy (
&&yy) +
azz 
.zz 
ScheduledDatezz 
==zz  "
datezz# '
&&zz( *
({{ 
int{{ 
){{ 
a{{ 
.{{ 
TimeSlot{{ 
=={{  "
timeSlot{{# +
&&{{, .
a|| 
.|| 
Status|| 
!=|| 
AppointmentStatus|| -
.||- .
	Cancelled||. 7
)||7 8
;||8 9
}}} 	
public 
async 
Task 
< 
bool 
> 1
%ExistsSameDoctorSameSlotSameDateAsync  E
(E F
intF I
doctorIdJ R
,R S
DateOnlyT \
date] a
,a b
intc f
timeSlotg o
,o p
intq t
appointmentId	u Ç
)
Ç É
{
ÄÄ 	
return
ÅÅ 
await
ÅÅ 
_context
ÅÅ !
.
ÅÅ! "
Appointments
ÅÅ" .
.
ÅÅ. /
AnyAsync
ÅÅ/ 7
(
ÅÅ7 8
a
ÅÅ8 9
=>
ÅÅ: <
a
ÇÇ 
.
ÇÇ 
AppointmentId
ÇÇ 
!=
ÇÇ  "
appointmentId
ÇÇ# 0
&&
ÇÇ1 3
a
ÉÉ 
.
ÉÉ 
DoctorId
ÉÉ 
==
ÉÉ 
doctorId
ÉÉ &
&&
ÉÉ' )
a
ÑÑ 
.
ÑÑ 
ScheduledDate
ÑÑ 
==
ÑÑ  "
date
ÑÑ# '
&&
ÑÑ( *
(
ÖÖ 
int
ÖÖ 
)
ÖÖ 
a
ÖÖ 
.
ÖÖ 
TimeSlot
ÖÖ 
==
ÖÖ  "
timeSlot
ÖÖ# +
&&
ÖÖ, .
a
ÜÜ 
.
ÜÜ 
Status
ÜÜ 
!=
ÜÜ 
AppointmentStatus
ÜÜ -
.
ÜÜ- .
	Cancelled
ÜÜ. 7
)
ÜÜ7 8
;
ÜÜ8 9
}
áá 	
public
ââ 
async
ââ 
Task
ââ 
AddAsync
ââ "
(
ââ" #
Appointment
ââ# .
appointment
ââ/ :
)
ââ: ;
{
ää 	
await
ãã 
_context
ãã 
.
ãã 
Appointments
ãã '
.
ãã' (
AddAsync
ãã( 0
(
ãã0 1
appointment
ãã1 <
)
ãã< =
;
ãã= >
}
åå 	
public
éé 
Task
éé 
UpdateAsync
éé 
(
éé  
Appointment
éé  +
appointment
éé, 7
)
éé7 8
{
èè 	
_context
êê 
.
êê 
Appointments
êê !
.
êê! "
Update
êê" (
(
êê( )
appointment
êê) 4
)
êê4 5
;
êê5 6
return
ëë 
Task
ëë 
.
ëë 
CompletedTask
ëë %
;
ëë% &
}
íí 	
public
îî 
async
îî 
Task
îî 
<
îî 
bool
îî 
>
îî 
ExistsAsync
îî  +
(
îî+ ,
int
îî, /
id
îî0 2
)
îî2 3
{
ïï 	
return
ññ 
await
ññ 
_context
ññ !
.
ññ! "
Appointments
ññ" .
.
ññ. /
AnyAsync
ññ/ 7
(
ññ7 8
a
ññ8 9
=>
ññ: <
a
ññ= >
.
ññ> ?
AppointmentId
ññ? L
==
ññM O
id
ññP R
)
ññR S
;
ññS T
}
óó 	
public
ôô 
async
ôô 
Task
ôô 
SaveChangesAsync
ôô *
(
ôô* +
)
ôô+ ,
{
öö 	
await
õõ 
_context
õõ 
.
õõ 
SaveChangesAsync
õõ +
(
õõ+ ,
)
õõ, -
;
õõ- .
}
úú 	
}
ùù 
}ûû π6
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\AdminRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{		 
[

 #
ExcludeFromCodeCoverage

 
]

 
public 

class 
AdminRepository  
:! "
IAdminRepository# 3
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
AdminRepository 
( 
HealthAxisDbContext 
context  '
)' (
{ 	
_context 
= 
context 
; 
} 	
public 
Task 
< 
int 
> 
CountPatientsAsync +
(+ ,
), -
=> 
_context 
. 
Patients  
.  !

CountAsync! +
(+ ,
), -
;- .
public 
Task 
< 
int 
> $
CountActivePatientsAsync 1
(1 2
)2 3
=> 
_context 
. 
Patients  
. 

CountAsync 
( 
p 
=>  
p! "
." #
IsActive# +
)+ ,
;, -
public 
Task 
< 
int 
> 
CountDoctorsAsync *
(* +
)+ ,
=> 
_context 
. 
Doctors 
.  

CountAsync  *
(* +
)+ ,
;, -
public 
Task 
< 
int 
> #
CountActiveDoctorsAsync 0
(0 1
)1 2
=>   
_context   
.   
Doctors   
.!! 

CountAsync!! 
(!! 
d!! 
=>!!  
d!!! "
.!!" #
IsActive!!# +
)!!+ ,
;!!, -
public## 
Task## 
<## 
int## 
>## '
CountTodayAppointmentsAsync## 4
(##4 5
)##5 6
=>$$ 
_context$$ 
.$$ 
Appointments$$ $
.%% 

CountAsync%% 
(%% 
a%% 
=>%%  
a&& 
.&& 
ScheduledDate&& #
==&&$ &
DateOnly'' 
.'' 
FromDateTime'' )
('') *
DateTime''* 2
.''2 3
Today''3 8
)''8 9
)''9 :
;'': ;
public)) 
Task)) 
<)) 
int)) 
>)) )
CountPendingAppointmentsAsync)) 6
())6 7
)))7 8
=>** 
_context** 
.** 
Appointments** $
.++ 

CountAsync++ 
(++ 
a++ 
=>++  
a,, 
.,, 
Status,, 
==,, 
AppointmentStatus,,  1
.,,1 2
Pending,,2 9
),,9 :
;,,: ;
public.. 
Task.. 
<.. 
int.. 
>.. +
CountCompletedAppointmentsAsync.. 8
(..8 9
)..9 :
=>// 
_context// 
.// 
Appointments// $
.00 

CountAsync00 
(00 
a00 
=>00  
a11 
.11 
Status11 
==11 
AppointmentStatus11  1
.111 2
	Completed112 ;
)11; <
;11< =
public33 
Task33 
<33 
int33 
>33 #
CountHealthRecordsAsync33 0
(330 1
)331 2
=>44 
_context44 
.44 
HealthRecords44 %
.44% &

CountAsync44& 0
(440 1
)441 2
;442 3
public66 
async66 
Task66 
<66 
IEnumerable66 %
<66% &
User66& *
>66* +
>66+ ,
GetUsersAsync77 
(77 
)77 
{88 	
return99 
await99 
_context99 !
.99! "
Users99" '
.:: 
OrderBy:: 
(:: 
u:: 
=>:: 
u:: 
.::  
UserId::  &
)::& '
.;; 
ToListAsync;; 
(;; 
);; 
;;; 
}<< 	
public>> 
async>> 
Task>> 
<>> 
User>> 
?>> 
>>>  
GetUserByIdAsync>>! 1
(>>1 2
int>>2 5
id>>6 8
)>>8 9
{?? 	
return@@ 
await@@ 
_context@@ !
.@@! "
Users@@" '
.AA 
FirstOrDefaultAsyncAA $
(AA$ %
uBB 
=>BB 
uBB 
.BB 
UserIdBB !
==BB" $
idBB% '
)BB' (
;BB( )
}CC 	
publicDD 
asyncDD 
TaskDD 
<DD 
boolDD 
>DD (
ResolveUserActiveStatusAsyncDD  <
(DD< =
stringDD= C
emailDDD I
,DDI J
stringDDK Q
roleDDR V
)DDV W
{EE 	
ifFF 
(FF 
stringFF 
.FF 
IsNullOrWhiteSpaceFF )
(FF) *
emailFF* /
)FF/ 0
||FF1 3
stringFF4 :
.FF: ;
IsNullOrWhiteSpaceFF; M
(FFM N
roleFFN R
)FFR S
)FFS T
returnGG 
falseGG 
;GG 
switchII 
(II 
roleII 
)II 
{JJ 
caseKK 
$strKK 
:KK 
returnLL 
trueLL 
;LL  
caseNN 
$strNN 
:NN 
returnOO 
awaitOO  
_contextOO! )
.OO) *
DoctorsOO* 1
.PP 
AnyAsyncPP !
(PP! "
dPP" #
=>PP$ &
dPP' (
.PP( )
EmailPP) .
==PP/ 1
emailPP2 7
&&PP8 :
dPP; <
.PP< =
IsActivePP= E
)PPE F
;PPF G
caseRR 
$strRR 
:RR 
returnSS 
awaitSS  
_contextSS! )
.SS) *
PatientsSS* 2
.TT 
AnyAsyncTT !
(TT! "
pTT" #
=>TT$ &
pTT' (
.TT( )
EmailTT) .
==TT/ 1
emailTT2 7
&&TT8 :
pTT; <
.TT< =
IsActiveTT= E
)TTE F
;TTF G
defaultVV 
:VV 
returnWW 
falseWW  
;WW  !
}XX 
}YY 	
}[[ 
}\\ ßW
FC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
. 
AddJsonOptions 
( 
options 
=> 
{ 
options 
. !
JsonSerializerOptions %
.% & 
PropertyNamingPolicy& :
=; <
JsonNamingPolicy 
. 
	CamelCase &
;& '
} 
) 
; 
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
options &
=>' )
{ 
options   
.   

SwaggerDoc   
(   
$str!! 
,!! 
new"" 
OpenApiInfo"" 
{## 	
Title$$ 
=$$ 
$str$$ $
,$$$ %
Version%% 
=%% 
$str%% 
,%% 
Description&& 
=&& 
$str&& A
}'' 	
)''	 

;''
 
options(( 
.(( !
AddSecurityDefinition(( !
(((! "
$str)) 
,)) 
new** !
OpenApiSecurityScheme** !
{++ 	
Name,, 
=,, 
$str,, "
,,," #
In-- 
=-- 
ParameterLocation-- "
.--" #
Header--# )
,--) *
Type.. 
=.. 
SecuritySchemeType.. %
...% &
Http..& *
,..* +
Scheme// 
=// 
$str// 
,// 
BearerFormat00 
=00 
$str00  
,00  !
Description11 
=11 
$str11 W
}22 	
)22	 

;22
 
options33 
.33 "
AddSecurityRequirement33 "
(33" #
document33# +
=>33, .
new44 &
OpenApiSecurityRequirement44 &
{55 	
[66 
new66 *
OpenApiSecuritySchemeReference66 /
(66/ 0
$str77 
,77 
document88 
)88 
]88 
=88 
[88 
]88 
}99 	
)99	 

;99
 
}:: 
):: 
;:: 
builderAA 
.AA 
ServicesAA 
.AA 
AddDbContextAA 
<AA 
HealthAxisDbContextAA 1
>AA1 2
(AA2 3
optionsAA3 :
=>AA; =
{BB 
optionsCC 
.CC 
UseSqlServerCC 
(CC 
builderDD 
.DD 
ConfigurationDD 
.DD 
GetConnectionStringDD 1
(DD1 2
$strDD2 E
)DDE F
)DDF G
;DDG H
}EE 
)EE 
;EE 
builderKK 
.KK 
ServicesKK 
.KK 
AddAuthenticationKK "
(KK" #
JwtBearerDefaultsLL 
.LL  
AuthenticationSchemeLL *
)LL* +
.MM 
AddJwtBearerMM 
(MM 
optionsMM 
=>MM 
{NN 
varOO 
jwtOO 
=OO 
builderOO 
.OO 
ConfigurationOO '
.OO' (

GetSectionOO( 2
(OO2 3
$strOO3 8
)OO8 9
;OO9 :
optionsQQ 
.QQ %
TokenValidationParametersQQ )
=QQ* +
newRR %
TokenValidationParametersRR )
{SS 
ValidateIssuerTT 
=TT  
trueTT! %
,TT% &
ValidIssuerUU 
=UU 
jwtUU !
[UU! "
$strUU" *
]UU* +
,UU+ ,
ValidateAudienceWW  
=WW! "
trueWW# '
,WW' (
ValidAudienceXX 
=XX 
jwtXX  #
[XX# $
$strXX$ .
]XX. /
,XX/ 0
ValidateLifetimeZZ  
=ZZ! "
trueZZ# '
,ZZ' ($
ValidateIssuerSigningKey\\ (
=\\) *
true\\+ /
,\\/ 0
IssuerSigningKey^^  
=^^! "
new__  
SymmetricSecurityKey__ ,
(__, -
Encoding``  
.``  !
UTF8``! %
.``% &
GetBytes``& .
(``. /
jwt``/ 2
[``2 3
$str``3 8
]``8 9
!``9 :
)``: ;
)``; <
,``< =
	ClockSkewbb 
=bb 
TimeSpanbb $
.bb$ %
Zerobb% )
}cc 
;cc 
}dd 
)dd 
;dd 
builderff 
.ff 
Servicesff 
.ff 
AddAuthorizationff !
(ff! "
)ff" #
;ff# $
builderll 
.ll 
Servicesll 
.ll 
AddCorsll 
(ll 
optionsll  
=>ll! #
{mm 
optionsnn 
.nn 
	AddPolicynn 
(nn 
$stroo 
,oo 
policypp 
=>pp 
{qq 	
policyrr 
.ss 
WithOriginsss 
(ss 
$strss 5
)ss5 6
.tt 
AllowAnyHeadertt 
(tt  
)tt  !
.uu 
AllowAnyMethoduu 
(uu  
)uu  !
;uu! "
}vv 	
)vv	 

;vv
 
}ww 
)ww 
;ww 
builder}} 
.}} 
Services}} 
.}} 
	AddScoped}} 
<}} 
IPatientRepository}} -
,}}- .
PatientRepository}}/ @
>}}@ A
(}}A B
)}}B C
;}}C D
builder~~ 
.~~ 
Services~~ 
.~~ 
	AddScoped~~ 
<~~ 
IDoctorRepository~~ ,
,~~, -
DoctorRepository~~. >
>~~> ?
(~~? @
)~~@ A
;~~A B
builder 
. 
Services 
. 
	AddScoped 
< "
IAppointmentRepository 1
,1 2!
AppointmentRepository3 H
>H I
(I J
)J K
;K L
builderÄÄ 
.
ÄÄ 
Services
ÄÄ 
.
ÄÄ 
	AddScoped
ÄÄ 
<
ÄÄ %
IHealthRecordRepository
ÄÄ 2
,
ÄÄ2 3$
HealthRecordRepository
ÄÄ4 J
>
ÄÄJ K
(
ÄÄK L
)
ÄÄL M
;
ÄÄM N
builderÅÅ 
.
ÅÅ 
Services
ÅÅ 
.
ÅÅ 
	AddScoped
ÅÅ 
<
ÅÅ 
IUserRepository
ÅÅ *
,
ÅÅ* +
UserRepository
ÅÅ, :
>
ÅÅ: ;
(
ÅÅ; <
)
ÅÅ< =
;
ÅÅ= >
builderÇÇ 
.
ÇÇ 
Services
ÇÇ 
.
ÇÇ 
	AddScoped
ÇÇ 
<
ÇÇ 
IAdminRepository
ÇÇ +
,
ÇÇ+ ,
AdminRepository
ÇÇ- <
>
ÇÇ< =
(
ÇÇ= >
)
ÇÇ> ?
;
ÇÇ? @
builderàà 
.
àà 
Services
àà 
.
àà 
	AddScoped
àà 
<
àà 
IPatientService
àà *
,
àà* +
PatientService
àà, :
>
àà: ;
(
àà; <
)
àà< =
;
àà= >
builderââ 
.
ââ 
Services
ââ 
.
ââ 
	AddScoped
ââ 
<
ââ 
IDoctorService
ââ )
,
ââ) *
DoctorService
ââ+ 8
>
ââ8 9
(
ââ9 :
)
ââ: ;
;
ââ; <
builderää 
.
ää 
Services
ää 
.
ää 
	AddScoped
ää 
<
ää !
IAppointmentService
ää .
,
ää. / 
AppointmentService
ää0 B
>
ääB C
(
ääC D
)
ääD E
;
ääE F
builderãã 
.
ãã 
Services
ãã 
.
ãã 
	AddScoped
ãã 
<
ãã "
IHealthRecordService
ãã /
,
ãã/ 0!
HealthRecordService
ãã1 D
>
ããD E
(
ããE F
)
ããF G
;
ããG H
builderåå 
.
åå 
Services
åå 
.
åå 
	AddScoped
åå 
<
åå 
IAuthService
åå '
,
åå' (
AuthService
åå) 4
>
åå4 5
(
åå5 6
)
åå6 7
;
åå7 8
builderçç 
.
çç 
Services
çç 
.
çç 
	AddScoped
çç 
<
çç 
IAdminService
çç (
,
çç( )
AdminService
çç* 6
>
çç6 7
(
çç7 8
)
çç8 9
;
çç9 :
builderéé 
.
éé 
Services
éé 
.
éé 
	AddScoped
éé 
<
éé 
IUserService
éé '
,
éé' (
UserService
éé) 4
>
éé4 5
(
éé5 6
)
éé6 7
;
éé7 8
varíí 
app
íí 
=
íí 	
builder
íí
 
.
íí 
Build
íí 
(
íí 
)
íí 
;
íí 
ifññ 
(
ññ 
app
ññ 
.
ññ 
Environment
ññ 
.
ññ 
IsDevelopment
ññ !
(
ññ! "
)
ññ" #
)
ññ# $
{óó 
app
òò 
.
òò 

UseSwagger
òò 
(
òò 
)
òò 
;
òò 
app
öö 
.
öö 
UseSwaggerUI
öö 
(
öö 
options
öö 
=>
öö 
{
õõ 
options
úú 
.
úú 
SwaggerEndpoint
úú 
(
úú  
$str
ùù &
,
ùù& '
$str
ûû 
)
ûû  
;
ûû  !
options
†† 
.
†† 
RoutePrefix
†† 
=
†† 
string
†† $
.
††$ %
Empty
††% *
;
††* +
}
°° 
)
°° 
;
°° 
}¢¢ 
app§§ 
.
§§ 
UseMiddleware
§§ 
<
§§ !
ExceptionMiddleware
§§ %
>
§§% &
(
§§& '
)
§§' (
;
§§( )
app¶¶ 
.
¶¶ !
UseHttpsRedirection
¶¶ 
(
¶¶ 
)
¶¶ 
;
¶¶ 
app®® 
.
®® 
UseCors
®® 
(
®® 
$str
®® 
)
®® 
;
®® 
app™™ 
.
™™ 
UseAuthentication
™™ 
(
™™ 
)
™™ 
;
™™ 
app¨¨ 
.
¨¨ 
UseAuthorization
¨¨ 
(
¨¨ 
)
¨¨ 
;
¨¨ 
appÆÆ 
.
ÆÆ 
MapControllers
ÆÆ 
(
ÆÆ 
)
ÆÆ 
;
ÆÆ 
app≤≤ 
.
≤≤ 
Run
≤≤ 
(
≤≤ 
)
≤≤ 	
;
≤≤	 
ö
JC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\User.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
User 
{ 
[ 	
Key	 
] 
public		 
int		 
UserId		 
{		 
get		 
;		  
set		! $
;		$ %
}		& '
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public 
string 
PasswordHash "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
[ 	
Required	 
] 
public 
UserRole 
Role 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
? 
ReferenceId 
{  !
get" %
;% &
set' *
;* +
}, -
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
DateTime4 <
.< =
UtcNow= C
;C D
public 
string 
? 
RefreshToken #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
DateTime 
? "
RefreshTokenExpiryTime /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
} 
} ˆ!
MC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Patient.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Patient 
{ 
[ 	
Key	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! <
)

< =
]

= >
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) T
)T U
]U V
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
=  
$str! =
)= >
]> ?
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
Phone	 
( 
ErrorMessage 
= 
$str <
)< =
]= >
[ 	
StringLength	 
( 
$num 
) 
] 
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
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 5
)5 6
]6 7
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% =
)= >
]> ?
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public 
InsuranceStatus 
InsuranceStatus .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
[!! 	
StringLength!!	 
(!! 
$num!! 
)!! 
]!! 
public"" 
string"" 
?"" 
InsuranceNumber"" &
{""' (
get"") ,
;"", -
set"". 1
;""1 2
}""3 4
public$$ 
bool$$ 
IsActive$$ 
{$$ 
get$$ "
;$$" #
set$$$ '
;$$' (
}$$) *
=$$+ ,
true$$- 1
;$$1 2
public'' 
ICollection'' 
<'' 
Appointment'' &
>''& '
Appointments''( 4
{''5 6
get''7 :
;'': ;
set''< ?
;''? @
}''A B
=''C D
new''E H
List''I M
<''M N
Appointment''N Y
>''Y Z
(''Z [
)''[ \
;''\ ]
public)) 
ICollection)) 
<)) 
HealthRecord)) '
>))' (
HealthRecords))) 6
{))7 8
get))9 <
;))< =
set))> A
;))A B
}))C D
=))E F
new))G J
List))K O
<))O P
HealthRecord))P \
>))\ ]
())] ^
)))^ _
;))_ `
}** 
}++  
RC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\HealthRecord.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
HealthRecord 
{ 
[ 	
Key	 
] 
public 
int 
HealthRecordId !
{" #
get$ '
;' (
set) ,
;, -
}. /
[

 	
Required

	 
]

 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
Appointment 
Appointment &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
null7 ;
!; <
;< =
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 9
)9 :
]: ;
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
	Diagnosis 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
string0 6
.6 7
Empty7 <
;< =
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Prescription "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
[!! 	
StringLength!!	 
(!! 
$num!! 
)!! 
]!! 
public"" 
string"" 
?"" 
Notes"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}"") *
public$$ 
DateTime$$ 
	CreatedOn$$ !
{$$" #
get$$$ '
;$$' (
set$$) ,
;$$, -
}$$. /
=$$0 1
DateTime$$2 :
.$$: ;
UtcNow$$; A
;$$A B
}%% 
}&& è
LC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Doctor.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
[ 
Index 

(
 
nameof 
( 
Email 
) 
, 
IsUnique "
=# $
true% )
)) *
]* +
public		 

class		 
Doctor		 
{

 
[ 	
Key	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ;
); <
]< =
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* U
)U V
]V W
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
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
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! >
)> ?
]? @
public  
DoctorSpecialisation #
Specialisation$ 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
[ 	
Required	 
( 
ErrorMessage 
=  
$str! C
)C D
]D E
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% Q
)Q R
]R S
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
( 
ErrorMessage 
=  
$str! @
)@ A
]A B
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage )
=* +
$str, Y
)Y Z
]Z [
public   
decimal   
ConsultationFee   &
{  ' (
get  ) ,
;  , -
set  . 1
;  1 2
}  3 4
public"" 
bool"" 
IsActive"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}"") *
=""+ ,
true""- 1
;""1 2
public%% 
ICollection%% 
<%% 
Appointment%% &
>%%& '
Appointments%%( 4
{%%5 6
get%%7 :
;%%: ;
set%%< ?
;%%? @
}%%A B
=&& 
new&& 
List&& 
<&& 
Appointment&& "
>&&" #
(&&# $
)&&$ %
;&&% &
public(( 
ICollection(( 
<(( 
HealthRecord(( '
>((' (
HealthRecords(() 6
{((7 8
get((9 <
;((< =
set((> A
;((A B
}((C D
=)) 
new)) 
List)) 
<)) 
HealthRecord)) #
>))# $
())$ %
)))% &
;))& '
}** 
}++ …
QC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Appointment.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Appointment 
{ 
[		 	
Key			 
]		 
public

 
int

 
AppointmentId

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

- .
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
[ 	
Required	 
] 
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
public 
AppointmentTimeSlot "
TimeSlot# +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
AppointmentStatus  
Status! '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
= 
AppointmentStatus 
.  
Pending  '
;' (
[ 	
StringLength	 
( 
$num 
) 
] 
public   
string   
?   
CancellationReason   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  6 7
public"" 
HealthRecord"" 
?"" 
HealthRecord"" )
{""* +
get"", /
;""/ 0
set""1 4
;""4 5
}""6 7
}## 
}$$ á
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260622111729_DoctorUpdates.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{ 
public 

partial 
class 
DoctorUpdates &
:' (
	Migration) 2
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
} 	
} 
} Å
cC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260622111123_emailadded.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{ 
public 

partial 
class 

emailadded #
:$ %
	Migration& /
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
} 	
} 
} ¸
mC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260619044738_AddUniqueDoctorEmail.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{ 
public 

partial 
class  
AddUniqueDoctorEmail -
:. /
	Migration0 9
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
	AddColumn &
<& '
string' -
>- .
(. /
name 
: 
$str 
, 
table 
: 
$str  
,  !
type 
: 
$str %
,% &
	maxLength 
: 
$num 
, 
nullable 
: 
false 
,  
defaultValue 
: 
$str  
)  !
;! "
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
, 
column 
: 
$str 
,  
value 
: 
$str 
) 
; 
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
, 
column   
:   
$str   
,    
value!! 
:!! 
$str!! 
)!! 
;!! 
migrationBuilder## 
.## 
CreateIndex## (
(##( )
name$$ 
:$$ 
$str$$ (
,$$( )
table%% 
:%% 
$str%%  
,%%  !
column&& 
:&& 
$str&& 
,&&  
unique'' 
:'' 
true'' 
)'' 
;'' 
}(( 	
	protected++ 
override++ 
void++ 
Down++  $
(++$ %
MigrationBuilder++% 5
migrationBuilder++6 F
)++F G
{,, 	
migrationBuilder-- 
.-- 
	DropIndex-- &
(--& '
name.. 
:.. 
$str.. (
,..( )
table// 
:// 
$str//  
)//  !
;//! "
migrationBuilder11 
.11 

DropColumn11 '
(11' (
name22 
:22 
$str22 
,22 
table33 
:33 
$str33  
)33  !
;33! "
}44 	
}55 
}66 ﬁ≈
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260617093733_testing.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{ 
public		 

partial		 
class		 
testing		  
:		! "
	Migration		# ,
{

 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str #
,# $
columns 
: 
table 
=> !
new" %
{ 
Id 
= 
table 
. 
Column %
<% &
string& ,
>, -
(- .
type. 2
:2 3
$str4 C
,C D
nullableE M
:M N
falseO T
)T U
,U V
Name 
= 
table  
.  !
Column! '
<' (
string( .
>. /
(/ 0
type0 4
:4 5
$str6 E
,E F
	maxLengthG P
:P Q
$numR U
,U V
nullableW _
:_ `
truea e
)e f
,f g
NormalizedName "
=# $
table% *
.* +
Column+ 1
<1 2
string2 8
>8 9
(9 :
type: >
:> ?
$str@ O
,O P
	maxLengthQ Z
:Z [
$num\ _
,_ `
nullablea i
:i j
truek o
)o p
,p q
ConcurrencyStamp $
=% &
table' ,
., -
Column- 3
<3 4
string4 :
>: ;
(; <
type< @
:@ A
$strB Q
,Q R
nullableS [
:[ \
true] a
)a b
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 5
,5 6
x7 8
=>9 ;
x< =
.= >
Id> @
)@ A
;A B
} 
) 
; 
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str #
,# $
columns 
: 
table 
=> !
new" %
{ 
Id   
=   
table   
.   
Column   %
<  % &
string  & ,
>  , -
(  - .
type  . 2
:  2 3
$str  4 C
,  C D
nullable  E M
:  M N
false  O T
)  T U
,  U V
UserName!! 
=!! 
table!! $
.!!$ %
Column!!% +
<!!+ ,
string!!, 2
>!!2 3
(!!3 4
type!!4 8
:!!8 9
$str!!: I
,!!I J
	maxLength!!K T
:!!T U
$num!!V Y
,!!Y Z
nullable!![ c
:!!c d
true!!e i
)!!i j
,!!j k
NormalizedUserName"" &
=""' (
table"") .
."". /
Column""/ 5
<""5 6
string""6 <
>""< =
(""= >
type""> B
:""B C
$str""D S
,""S T
	maxLength""U ^
:""^ _
$num""` c
,""c d
nullable""e m
:""m n
true""o s
)""s t
,""t u
Email## 
=## 
table## !
.##! "
Column##" (
<##( )
string##) /
>##/ 0
(##0 1
type##1 5
:##5 6
$str##7 F
,##F G
	maxLength##H Q
:##Q R
$num##S V
,##V W
nullable##X `
:##` a
true##b f
)##f g
,##g h
NormalizedEmail$$ #
=$$$ %
table$$& +
.$$+ ,
Column$$, 2
<$$2 3
string$$3 9
>$$9 :
($$: ;
type$$; ?
:$$? @
$str$$A P
,$$P Q
	maxLength$$R [
:$$[ \
$num$$] `
,$$` a
nullable$$b j
:$$j k
true$$l p
)$$p q
,$$q r
EmailConfirmed%% "
=%%# $
table%%% *
.%%* +
Column%%+ 1
<%%1 2
bool%%2 6
>%%6 7
(%%7 8
type%%8 <
:%%< =
$str%%> C
,%%C D
nullable%%E M
:%%M N
false%%O T
)%%T U
,%%U V
PasswordHash&&  
=&&! "
table&&# (
.&&( )
Column&&) /
<&&/ 0
string&&0 6
>&&6 7
(&&7 8
type&&8 <
:&&< =
$str&&> M
,&&M N
nullable&&O W
:&&W X
true&&Y ]
)&&] ^
,&&^ _
SecurityStamp'' !
=''" #
table''$ )
.'') *
Column''* 0
<''0 1
string''1 7
>''7 8
(''8 9
type''9 =
:''= >
$str''? N
,''N O
nullable''P X
:''X Y
true''Z ^
)''^ _
,''_ `
ConcurrencyStamp(( $
=((% &
table((' ,
.((, -
Column((- 3
<((3 4
string((4 :
>((: ;
(((; <
type((< @
:((@ A
$str((B Q
,((Q R
nullable((S [
:(([ \
true((] a
)((a b
,((b c
PhoneNumber)) 
=))  !
table))" '
.))' (
Column))( .
<)). /
string))/ 5
>))5 6
())6 7
type))7 ;
:)); <
$str))= L
,))L M
nullable))N V
:))V W
true))X \
)))\ ]
,))] ^ 
PhoneNumberConfirmed** (
=**) *
table**+ 0
.**0 1
Column**1 7
<**7 8
bool**8 <
>**< =
(**= >
type**> B
:**B C
$str**D I
,**I J
nullable**K S
:**S T
false**U Z
)**Z [
,**[ \
TwoFactorEnabled++ $
=++% &
table++' ,
.++, -
Column++- 3
<++3 4
bool++4 8
>++8 9
(++9 :
type++: >
:++> ?
$str++@ E
,++E F
nullable++G O
:++O P
false++Q V
)++V W
,++W X

LockoutEnd,, 
=,,  
table,,! &
.,,& '
Column,,' -
<,,- .
DateTimeOffset,,. <
>,,< =
(,,= >
type,,> B
:,,B C
$str,,D T
,,,T U
nullable,,V ^
:,,^ _
true,,` d
),,d e
,,,e f
LockoutEnabled-- "
=--# $
table--% *
.--* +
Column--+ 1
<--1 2
bool--2 6
>--6 7
(--7 8
type--8 <
:--< =
$str--> C
,--C D
nullable--E M
:--M N
false--O T
)--T U
,--U V
AccessFailedCount.. %
=..& '
table..( -
...- .
Column... 4
<..4 5
int..5 8
>..8 9
(..9 :
type..: >
:..> ?
$str..@ E
,..E F
nullable..G O
:..O P
false..Q V
)..V W
}// 
,// 
constraints00 
:00 
table00 "
=>00# %
{11 
table22 
.22 

PrimaryKey22 $
(22$ %
$str22% 5
,225 6
x227 8
=>229 ;
x22< =
.22= >
Id22> @
)22@ A
;22A B
}33 
)33 
;33 
migrationBuilder55 
.55 
CreateTable55 (
(55( )
name66 
:66 
$str66 (
,66( )
columns77 
:77 
table77 
=>77 !
new77" %
{88 
Id99 
=99 
table99 
.99 
Column99 %
<99% &
int99& )
>99) *
(99* +
type99+ /
:99/ 0
$str991 6
,996 7
nullable998 @
:99@ A
false99B G
)99G H
.:: 

Annotation:: #
(::# $
$str::$ 8
,::8 9
$str::: @
)::@ A
,::A B
RoleId;; 
=;; 
table;; "
.;;" #
Column;;# )
<;;) *
string;;* 0
>;;0 1
(;;1 2
type;;2 6
:;;6 7
$str;;8 G
,;;G H
nullable;;I Q
:;;Q R
false;;S X
);;X Y
,;;Y Z
	ClaimType<< 
=<< 
table<<  %
.<<% &
Column<<& ,
<<<, -
string<<- 3
><<3 4
(<<4 5
type<<5 9
:<<9 :
$str<<; J
,<<J K
nullable<<L T
:<<T U
true<<V Z
)<<Z [
,<<[ \

ClaimValue== 
===  
table==! &
.==& '
Column==' -
<==- .
string==. 4
>==4 5
(==5 6
type==6 :
:==: ;
$str==< K
,==K L
nullable==M U
:==U V
true==W [
)==[ \
}>> 
,>> 
constraints?? 
:?? 
table?? "
=>??# %
{@@ 
tableAA 
.AA 

PrimaryKeyAA $
(AA$ %
$strAA% :
,AA: ;
xAA< =
=>AA> @
xAAA B
.AAB C
IdAAC E
)AAE F
;AAF G
tableBB 
.BB 

ForeignKeyBB $
(BB$ %
nameCC 
:CC 
$strCC F
,CCF G
columnDD 
:DD 
xDD  !
=>DD" $
xDD% &
.DD& '
RoleIdDD' -
,DD- .
principalTableEE &
:EE& '
$strEE( 5
,EE5 6
principalColumnFF '
:FF' (
$strFF) -
,FF- .
onDeleteGG  
:GG  !
ReferentialActionGG" 3
.GG3 4
CascadeGG4 ;
)GG; <
;GG< =
}HH 
)HH 
;HH 
migrationBuilderJJ 
.JJ 
CreateTableJJ (
(JJ( )
nameKK 
:KK 
$strKK (
,KK( )
columnsLL 
:LL 
tableLL 
=>LL !
newLL" %
{MM 
IdNN 
=NN 
tableNN 
.NN 
ColumnNN %
<NN% &
intNN& )
>NN) *
(NN* +
typeNN+ /
:NN/ 0
$strNN1 6
,NN6 7
nullableNN8 @
:NN@ A
falseNNB G
)NNG H
.OO 

AnnotationOO #
(OO# $
$strOO$ 8
,OO8 9
$strOO: @
)OO@ A
,OOA B
UserIdPP 
=PP 
tablePP "
.PP" #
ColumnPP# )
<PP) *
stringPP* 0
>PP0 1
(PP1 2
typePP2 6
:PP6 7
$strPP8 G
,PPG H
nullablePPI Q
:PPQ R
falsePPS X
)PPX Y
,PPY Z
	ClaimTypeQQ 
=QQ 
tableQQ  %
.QQ% &
ColumnQQ& ,
<QQ, -
stringQQ- 3
>QQ3 4
(QQ4 5
typeQQ5 9
:QQ9 :
$strQQ; J
,QQJ K
nullableQQL T
:QQT U
trueQQV Z
)QQZ [
,QQ[ \

ClaimValueRR 
=RR  
tableRR! &
.RR& '
ColumnRR' -
<RR- .
stringRR. 4
>RR4 5
(RR5 6
typeRR6 :
:RR: ;
$strRR< K
,RRK L
nullableRRM U
:RRU V
trueRRW [
)RR[ \
}SS 
,SS 
constraintsTT 
:TT 
tableTT "
=>TT# %
{UU 
tableVV 
.VV 

PrimaryKeyVV $
(VV$ %
$strVV% :
,VV: ;
xVV< =
=>VV> @
xVVA B
.VVB C
IdVVC E
)VVE F
;VVF G
tableWW 
.WW 

ForeignKeyWW $
(WW$ %
nameXX 
:XX 
$strXX F
,XXF G
columnYY 
:YY 
xYY  !
=>YY" $
xYY% &
.YY& '
UserIdYY' -
,YY- .
principalTableZZ &
:ZZ& '
$strZZ( 5
,ZZ5 6
principalColumn[[ '
:[[' (
$str[[) -
,[[- .
onDelete\\  
:\\  !
ReferentialAction\\" 3
.\\3 4
Cascade\\4 ;
)\\; <
;\\< =
}]] 
)]] 
;]] 
migrationBuilder__ 
.__ 
CreateTable__ (
(__( )
name`` 
:`` 
$str`` (
,``( )
columnsaa 
:aa 
tableaa 
=>aa !
newaa" %
{bb 
LoginProvidercc !
=cc" #
tablecc$ )
.cc) *
Columncc* 0
<cc0 1
stringcc1 7
>cc7 8
(cc8 9
typecc9 =
:cc= >
$strcc? N
,ccN O
nullableccP X
:ccX Y
falseccZ _
)cc_ `
,cc` a
ProviderKeydd 
=dd  !
tabledd" '
.dd' (
Columndd( .
<dd. /
stringdd/ 5
>dd5 6
(dd6 7
typedd7 ;
:dd; <
$strdd= L
,ddL M
nullableddN V
:ddV W
falseddX ]
)dd] ^
,dd^ _
ProviderDisplayNameee '
=ee( )
tableee* /
.ee/ 0
Columnee0 6
<ee6 7
stringee7 =
>ee= >
(ee> ?
typeee? C
:eeC D
$streeE T
,eeT U
nullableeeV ^
:ee^ _
trueee` d
)eed e
,eee f
UserIdff 
=ff 
tableff "
.ff" #
Columnff# )
<ff) *
stringff* 0
>ff0 1
(ff1 2
typeff2 6
:ff6 7
$strff8 G
,ffG H
nullableffI Q
:ffQ R
falseffS X
)ffX Y
}gg 
,gg 
constraintshh 
:hh 
tablehh "
=>hh# %
{ii 
tablejj 
.jj 

PrimaryKeyjj $
(jj$ %
$strjj% :
,jj: ;
xjj< =
=>jj> @
newjjA D
{jjE F
xjjG H
.jjH I
LoginProviderjjI V
,jjV W
xjjX Y
.jjY Z
ProviderKeyjjZ e
}jjf g
)jjg h
;jjh i
tablekk 
.kk 

ForeignKeykk $
(kk$ %
namell 
:ll 
$strll F
,llF G
columnmm 
:mm 
xmm  !
=>mm" $
xmm% &
.mm& '
UserIdmm' -
,mm- .
principalTablenn &
:nn& '
$strnn( 5
,nn5 6
principalColumnoo '
:oo' (
$stroo) -
,oo- .
onDeletepp  
:pp  !
ReferentialActionpp" 3
.pp3 4
Cascadepp4 ;
)pp; <
;pp< =
}qq 
)qq 
;qq 
migrationBuilderss 
.ss 
CreateTabless (
(ss( )
namett 
:tt 
$strtt '
,tt' (
columnsuu 
:uu 
tableuu 
=>uu !
newuu" %
{vv 
UserIdww 
=ww 
tableww "
.ww" #
Columnww# )
<ww) *
stringww* 0
>ww0 1
(ww1 2
typeww2 6
:ww6 7
$strww8 G
,wwG H
nullablewwI Q
:wwQ R
falsewwS X
)wwX Y
,wwY Z
RoleIdxx 
=xx 
tablexx "
.xx" #
Columnxx# )
<xx) *
stringxx* 0
>xx0 1
(xx1 2
typexx2 6
:xx6 7
$strxx8 G
,xxG H
nullablexxI Q
:xxQ R
falsexxS X
)xxX Y
}yy 
,yy 
constraintszz 
:zz 
tablezz "
=>zz# %
{{{ 
table|| 
.|| 

PrimaryKey|| $
(||$ %
$str||% 9
,||9 :
x||; <
=>||= ?
new||@ C
{||D E
x||F G
.||G H
UserId||H N
,||N O
x||P Q
.||Q R
RoleId||R X
}||Y Z
)||Z [
;||[ \
table}} 
.}} 

ForeignKey}} $
(}}$ %
name~~ 
:~~ 
$str~~ E
,~~E F
column 
: 
x  !
=>" $
x% &
.& '
RoleId' -
,- .
principalTable
ÄÄ &
:
ÄÄ& '
$str
ÄÄ( 5
,
ÄÄ5 6
principalColumn
ÅÅ '
:
ÅÅ' (
$str
ÅÅ) -
,
ÅÅ- .
onDelete
ÇÇ  
:
ÇÇ  !
ReferentialAction
ÇÇ" 3
.
ÇÇ3 4
Cascade
ÇÇ4 ;
)
ÇÇ; <
;
ÇÇ< =
table
ÉÉ 
.
ÉÉ 

ForeignKey
ÉÉ $
(
ÉÉ$ %
name
ÑÑ 
:
ÑÑ 
$str
ÑÑ E
,
ÑÑE F
column
ÖÖ 
:
ÖÖ 
x
ÖÖ  !
=>
ÖÖ" $
x
ÖÖ% &
.
ÖÖ& '
UserId
ÖÖ' -
,
ÖÖ- .
principalTable
ÜÜ &
:
ÜÜ& '
$str
ÜÜ( 5
,
ÜÜ5 6
principalColumn
áá '
:
áá' (
$str
áá) -
,
áá- .
onDelete
àà  
:
àà  !
ReferentialAction
àà" 3
.
àà3 4
Cascade
àà4 ;
)
àà; <
;
àà< =
}
ââ 
)
ââ 
;
ââ 
migrationBuilder
ãã 
.
ãã 
CreateTable
ãã (
(
ãã( )
name
åå 
:
åå 
$str
åå (
,
åå( )
columns
çç 
:
çç 
table
çç 
=>
çç !
new
çç" %
{
éé 
UserId
èè 
=
èè 
table
èè "
.
èè" #
Column
èè# )
<
èè) *
string
èè* 0
>
èè0 1
(
èè1 2
type
èè2 6
:
èè6 7
$str
èè8 G
,
èèG H
nullable
èèI Q
:
èèQ R
false
èèS X
)
èèX Y
,
èèY Z
LoginProvider
êê !
=
êê" #
table
êê$ )
.
êê) *
Column
êê* 0
<
êê0 1
string
êê1 7
>
êê7 8
(
êê8 9
type
êê9 =
:
êê= >
$str
êê? N
,
êêN O
nullable
êêP X
:
êêX Y
false
êêZ _
)
êê_ `
,
êê` a
Name
ëë 
=
ëë 
table
ëë  
.
ëë  !
Column
ëë! '
<
ëë' (
string
ëë( .
>
ëë. /
(
ëë/ 0
type
ëë0 4
:
ëë4 5
$str
ëë6 E
,
ëëE F
nullable
ëëG O
:
ëëO P
false
ëëQ V
)
ëëV W
,
ëëW X
Value
íí 
=
íí 
table
íí !
.
íí! "
Column
íí" (
<
íí( )
string
íí) /
>
íí/ 0
(
íí0 1
type
íí1 5
:
íí5 6
$str
íí7 F
,
ííF G
nullable
ííH P
:
ííP Q
true
ííR V
)
ííV W
}
ìì 
,
ìì 
constraints
îî 
:
îî 
table
îî "
=>
îî# %
{
ïï 
table
ññ 
.
ññ 

PrimaryKey
ññ $
(
ññ$ %
$str
ññ% :
,
ññ: ;
x
ññ< =
=>
ññ> @
new
ññA D
{
ññE F
x
ññG H
.
ññH I
UserId
ññI O
,
ññO P
x
ññQ R
.
ññR S
LoginProvider
ññS `
,
ññ` a
x
ññb c
.
ññc d
Name
ññd h
}
ññi j
)
ññj k
;
ññk l
table
óó 
.
óó 

ForeignKey
óó $
(
óó$ %
name
òò 
:
òò 
$str
òò F
,
òòF G
column
ôô 
:
ôô 
x
ôô  !
=>
ôô" $
x
ôô% &
.
ôô& '
UserId
ôô' -
,
ôô- .
principalTable
öö &
:
öö& '
$str
öö( 5
,
öö5 6
principalColumn
õõ '
:
õõ' (
$str
õõ) -
,
õõ- .
onDelete
úú  
:
úú  !
ReferentialAction
úú" 3
.
úú3 4
Cascade
úú4 ;
)
úú; <
;
úú< =
}
ùù 
)
ùù 
;
ùù 
migrationBuilder
üü 
.
üü 
CreateIndex
üü (
(
üü( )
name
†† 
:
†† 
$str
†† 2
,
††2 3
table
°° 
:
°° 
$str
°° )
,
°°) *
column
¢¢ 
:
¢¢ 
$str
¢¢  
)
¢¢  !
;
¢¢! "
migrationBuilder
§§ 
.
§§ 
CreateIndex
§§ (
(
§§( )
name
•• 
:
•• 
$str
•• %
,
••% &
table
¶¶ 
:
¶¶ 
$str
¶¶ $
,
¶¶$ %
column
ßß 
:
ßß 
$str
ßß (
,
ßß( )
unique
®® 
:
®® 
true
®® 
,
®® 
filter
©© 
:
©© 
$str
©© 6
)
©©6 7
;
©©7 8
migrationBuilder
´´ 
.
´´ 
CreateIndex
´´ (
(
´´( )
name
¨¨ 
:
¨¨ 
$str
¨¨ 2
,
¨¨2 3
table
≠≠ 
:
≠≠ 
$str
≠≠ )
,
≠≠) *
column
ÆÆ 
:
ÆÆ 
$str
ÆÆ  
)
ÆÆ  !
;
ÆÆ! "
migrationBuilder
∞∞ 
.
∞∞ 
CreateIndex
∞∞ (
(
∞∞( )
name
±± 
:
±± 
$str
±± 2
,
±±2 3
table
≤≤ 
:
≤≤ 
$str
≤≤ )
,
≤≤) *
column
≥≥ 
:
≥≥ 
$str
≥≥  
)
≥≥  !
;
≥≥! "
migrationBuilder
µµ 
.
µµ 
CreateIndex
µµ (
(
µµ( )
name
∂∂ 
:
∂∂ 
$str
∂∂ 1
,
∂∂1 2
table
∑∑ 
:
∑∑ 
$str
∑∑ (
,
∑∑( )
column
∏∏ 
:
∏∏ 
$str
∏∏  
)
∏∏  !
;
∏∏! "
migrationBuilder
∫∫ 
.
∫∫ 
CreateIndex
∫∫ (
(
∫∫( )
name
ªª 
:
ªª 
$str
ªª "
,
ªª" #
table
ºº 
:
ºº 
$str
ºº $
,
ºº$ %
column
ΩΩ 
:
ΩΩ 
$str
ΩΩ )
)
ΩΩ) *
;
ΩΩ* +
migrationBuilder
øø 
.
øø 
CreateIndex
øø (
(
øø( )
name
¿¿ 
:
¿¿ 
$str
¿¿ %
,
¿¿% &
table
¡¡ 
:
¡¡ 
$str
¡¡ $
,
¡¡$ %
column
¬¬ 
:
¬¬ 
$str
¬¬ ,
,
¬¬, -
unique
√√ 
:
√√ 
true
√√ 
,
√√ 
filter
ƒƒ 
:
ƒƒ 
$str
ƒƒ :
)
ƒƒ: ;
;
ƒƒ; <
}
≈≈ 	
	protected
»» 
override
»» 
void
»» 
Down
»»  $
(
»»$ %
MigrationBuilder
»»% 5
migrationBuilder
»»6 F
)
»»F G
{
…… 	
migrationBuilder
   
.
   
	DropTable
   &
(
  & '
name
ÀÀ 
:
ÀÀ 
$str
ÀÀ (
)
ÀÀ( )
;
ÀÀ) *
migrationBuilder
ÕÕ 
.
ÕÕ 
	DropTable
ÕÕ &
(
ÕÕ& '
name
ŒŒ 
:
ŒŒ 
$str
ŒŒ (
)
ŒŒ( )
;
ŒŒ) *
migrationBuilder
–– 
.
–– 
	DropTable
–– &
(
––& '
name
—— 
:
—— 
$str
—— (
)
——( )
;
——) *
migrationBuilder
”” 
.
”” 
	DropTable
”” &
(
””& '
name
‘‘ 
:
‘‘ 
$str
‘‘ '
)
‘‘' (
;
‘‘( )
migrationBuilder
÷÷ 
.
÷÷ 
	DropTable
÷÷ &
(
÷÷& '
name
◊◊ 
:
◊◊ 
$str
◊◊ (
)
◊◊( )
;
◊◊) *
migrationBuilder
ŸŸ 
.
ŸŸ 
	DropTable
ŸŸ &
(
ŸŸ& '
name
⁄⁄ 
:
⁄⁄ 
$str
⁄⁄ #
)
⁄⁄# $
;
⁄⁄$ %
migrationBuilder
‹‹ 
.
‹‹ 
	DropTable
‹‹ &
(
‹‹& '
name
›› 
:
›› 
$str
›› #
)
››# $
;
››$ %
}
ﬁﬁ 	
}
ﬂﬂ 
}‡‡ £
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260616160604_UpdatedUsersMOdel.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{ 
public		 

partial		 
class		 
UpdatedUsersMOdel		 *
:		+ ,
	Migration		- 6
{

 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
	AddColumn &
<& '
string' -
>- .
(. /
name 
: 
$str $
,$ %
table 
: 
$str 
, 
type 
: 
$str %
,% &
nullable 
: 
true 
) 
;  
migrationBuilder 
. 
	AddColumn &
<& '
DateTime' /
>/ 0
(0 1
name 
: 
$str .
,. /
table 
: 
$str 
, 
type 
: 
$str !
,! "
nullable 
: 
true 
) 
;  
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 

DropColumn '
(' (
name 
: 
$str $
,$ %
table   
:   
$str   
)   
;    
migrationBuilder"" 
."" 

DropColumn"" '
(""' (
name## 
:## 
$str## .
,##. /
table$$ 
:$$ 
$str$$ 
)$$ 
;$$  
}%% 	
}&& 
}'' Ç
cC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260616051459_UsersAdded.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{ 
public		 

partial		 
class		 

UsersAdded		 #
:		$ %
	Migration		& /
{

 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str 
, 
columns 
: 
table 
=> !
new" %
{ 
UserId 
= 
table "
." #
Column# )
<) *
int* -
>- .
(. /
type/ 3
:3 4
$str5 :
,: ;
nullable< D
:D E
falseF K
)K L
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
Email 
= 
table !
.! "
Column" (
<( )
string) /
>/ 0
(0 1
type1 5
:5 6
$str7 F
,F G
	maxLengthH Q
:Q R
$numS V
,V W
nullableX `
:` a
falseb g
)g h
,h i
PasswordHash  
=! "
table# (
.( )
Column) /
</ 0
string0 6
>6 7
(7 8
type8 <
:< =
$str> M
,M N
nullableO W
:W X
falseY ^
)^ _
,_ `
Role 
= 
table  
.  !
Column! '
<' (
int( +
>+ ,
(, -
type- 1
:1 2
$str3 8
,8 9
nullable: B
:B C
falseD I
)I J
,J K
ReferenceId 
=  !
table" '
.' (
Column( .
<. /
int/ 2
>2 3
(3 4
type4 8
:8 9
$str: ?
,? @
nullableA I
:I J
trueK O
)O P
,P Q
CreatedDate 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% /
,/ 0
x1 2
=>3 5
x6 7
.7 8
UserId8 >
)> ?
;? @
} 
) 
; 
} 	
	protected!! 
override!! 
void!! 
Down!!  $
(!!$ %
MigrationBuilder!!% 5
migrationBuilder!!6 F
)!!F G
{"" 	
migrationBuilder## 
.## 
	DropTable## &
(##& '
name$$ 
:$$ 
$str$$ 
)$$ 
;$$ 
}%% 	
}&& 
}'' Å∑
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Migrations\20260615042816_InitialCreate.cs
	namespace 	
S3_HealthAxisApi
 
. 

Migrations %
{		 
public 

partial 
class 
InitialCreate &
:' (
	Migration) 2
{ 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str 
,  
columns 
: 
table 
=> !
new" %
{ 
DoctorId 
= 
table $
.$ %
Column% +
<+ ,
int, /
>/ 0
(0 1
type1 5
:5 6
$str7 <
,< =
nullable> F
:F G
falseH M
)M N
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
FullName 
= 
table $
.$ %
Column% +
<+ ,
string, 2
>2 3
(3 4
type4 8
:8 9
$str: I
,I J
	maxLengthK T
:T U
$numV Y
,Y Z
nullable[ c
:c d
falsee j
)j k
,k l
Specialisation "
=# $
table% *
.* +
Column+ 1
<1 2
int2 5
>5 6
(6 7
type7 ;
:; <
$str= B
,B C
nullableD L
:L M
falseN S
)S T
,T U
YearsOfExperience %
=& '
table( -
.- .
Column. 4
<4 5
int5 8
>8 9
(9 :
type: >
:> ?
$str@ E
,E F
nullableG O
:O P
falseQ V
)V W
,W X
ConsultationFee #
=$ %
table& +
.+ ,
Column, 2
<2 3
decimal3 :
>: ;
(; <
type< @
:@ A
$strB Q
,Q R
	precisionS \
:\ ]
$num^ `
,` a
scaleb g
:g h
$numi j
,j k
nullablel t
:t u
falsev {
){ |
,| }
IsActive 
= 
table $
.$ %
Column% +
<+ ,
bool, 0
>0 1
(1 2
type2 6
:6 7
$str8 =
,= >
nullable? G
:G H
falseI N
)N O
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 1
,1 2
x3 4
=>5 7
x8 9
.9 :
DoctorId: B
)B C
;C D
} 
) 
; 
migrationBuilder!! 
.!! 
CreateTable!! (
(!!( )
name"" 
:"" 
$str""  
,""  !
columns## 
:## 
table## 
=>## !
new##" %
{$$ 
	PatientId%% 
=%% 
table%%  %
.%%% &
Column%%& ,
<%%, -
int%%- 0
>%%0 1
(%%1 2
type%%2 6
:%%6 7
$str%%8 =
,%%= >
nullable%%? G
:%%G H
false%%I N
)%%N O
.&& 

Annotation&& #
(&&# $
$str&&$ 8
,&&8 9
$str&&: @
)&&@ A
,&&A B
FullName'' 
='' 
table'' $
.''$ %
Column''% +
<''+ ,
string'', 2
>''2 3
(''3 4
type''4 8
:''8 9
$str'': H
,''H I
	maxLength''J S
:''S T
$num''U W
,''W X
nullable''Y a
:''a b
false''c h
)''h i
,''i j
DateOfBirth(( 
=((  !
table((" '
.((' (
Column((( .
<((. /
DateOnly((/ 7
>((7 8
(((8 9
type((9 =
:((= >
$str((? E
,((E F
nullable((G O
:((O P
false((Q V
)((V W
,((W X
Gender)) 
=)) 
table)) "
.))" #
Column))# )
<))) *
int))* -
>))- .
()). /
type))/ 3
:))3 4
$str))5 :
,)): ;
nullable))< D
:))D E
false))F K
)))K L
,))L M
PhoneNumber** 
=**  !
table**" '
.**' (
Column**( .
<**. /
string**/ 5
>**5 6
(**6 7
type**7 ;
:**; <
$str**= K
,**K L
	maxLength**M V
:**V W
$num**X Z
,**Z [
nullable**\ d
:**d e
false**f k
)**k l
,**l m
Email++ 
=++ 
table++ !
.++! "
Column++" (
<++( )
string++) /
>++/ 0
(++0 1
type++1 5
:++5 6
$str++7 F
,++F G
	maxLength++H Q
:++Q R
$num++S V
,++V W
nullable++X `
:++` a
false++b g
)++g h
,++h i
InsuranceStatus,, #
=,,$ %
table,,& +
.,,+ ,
Column,,, 2
<,,2 3
int,,3 6
>,,6 7
(,,7 8
type,,8 <
:,,< =
$str,,> C
,,,C D
nullable,,E M
:,,M N
false,,O T
),,T U
,,,U V
InsuranceNumber-- #
=--$ %
table--& +
.--+ ,
Column--, 2
<--2 3
string--3 9
>--9 :
(--: ;
type--; ?
:--? @
$str--A O
,--O P
	maxLength--Q Z
:--Z [
$num--\ ^
,--^ _
nullable--` h
:--h i
true--j n
)--n o
,--o p
IsActive.. 
=.. 
table.. $
...$ %
Column..% +
<..+ ,
bool.., 0
>..0 1
(..1 2
type..2 6
:..6 7
$str..8 =
,..= >
nullable..? G
:..G H
false..I N
)..N O
}// 
,// 
constraints00 
:00 
table00 "
=>00# %
{11 
table22 
.22 

PrimaryKey22 $
(22$ %
$str22% 2
,222 3
x224 5
=>226 8
x229 :
.22: ;
	PatientId22; D
)22D E
;22E F
}33 
)33 
;33 
migrationBuilder55 
.55 
CreateTable55 (
(55( )
name66 
:66 
$str66 $
,66$ %
columns77 
:77 
table77 
=>77 !
new77" %
{88 
AppointmentId99 !
=99" #
table99$ )
.99) *
Column99* 0
<990 1
int991 4
>994 5
(995 6
type996 :
:99: ;
$str99< A
,99A B
nullable99C K
:99K L
false99M R
)99R S
.:: 

Annotation:: #
(::# $
$str::$ 8
,::8 9
$str::: @
)::@ A
,::A B
	PatientId;; 
=;; 
table;;  %
.;;% &
Column;;& ,
<;;, -
int;;- 0
>;;0 1
(;;1 2
type;;2 6
:;;6 7
$str;;8 =
,;;= >
nullable;;? G
:;;G H
false;;I N
);;N O
,;;O P
DoctorId<< 
=<< 
table<< $
.<<$ %
Column<<% +
<<<+ ,
int<<, /
><</ 0
(<<0 1
type<<1 5
:<<5 6
$str<<7 <
,<<< =
nullable<<> F
:<<F G
false<<H M
)<<M N
,<<N O
ScheduledDate== !
===" #
table==$ )
.==) *
Column==* 0
<==0 1
DateOnly==1 9
>==9 :
(==: ;
type==; ?
:==? @
$str==A G
,==G H
nullable==I Q
:==Q R
false==S X
)==X Y
,==Y Z
TimeSlot>> 
=>> 
table>> $
.>>$ %
Column>>% +
<>>+ ,
int>>, /
>>>/ 0
(>>0 1
type>>1 5
:>>5 6
$str>>7 <
,>>< =
nullable>>> F
:>>F G
false>>H M
)>>M N
,>>N O
Status?? 
=?? 
table?? "
.??" #
Column??# )
<??) *
int??* -
>??- .
(??. /
type??/ 3
:??3 4
$str??5 :
,??: ;
nullable??< D
:??D E
false??F K
)??K L
,??L M
CancellationReason@@ &
=@@' (
table@@) .
.@@. /
Column@@/ 5
<@@5 6
string@@6 <
>@@< =
(@@= >
type@@> B
:@@B C
$str@@D S
,@@S T
	maxLength@@U ^
:@@^ _
$num@@` c
,@@c d
nullable@@e m
:@@m n
true@@o s
)@@s t
}AA 
,AA 
constraintsBB 
:BB 
tableBB "
=>BB# %
{CC 
tableDD 
.DD 

PrimaryKeyDD $
(DD$ %
$strDD% 6
,DD6 7
xDD8 9
=>DD: <
xDD= >
.DD> ?
AppointmentIdDD? L
)DDL M
;DDM N
tableEE 
.EE 

ForeignKeyEE $
(EE$ %
nameFF 
:FF 
$strFF @
,FF@ A
columnGG 
:GG 
xGG  !
=>GG" $
xGG% &
.GG& '
DoctorIdGG' /
,GG/ 0
principalTableHH &
:HH& '
$strHH( 1
,HH1 2
principalColumnII '
:II' (
$strII) 3
,II3 4
onDeleteJJ  
:JJ  !
ReferentialActionJJ" 3
.JJ3 4
RestrictJJ4 <
)JJ< =
;JJ= >
tableKK 
.KK 

ForeignKeyKK $
(KK$ %
nameLL 
:LL 
$strLL B
,LLB C
columnMM 
:MM 
xMM  !
=>MM" $
xMM% &
.MM& '
	PatientIdMM' 0
,MM0 1
principalTableNN &
:NN& '
$strNN( 2
,NN2 3
principalColumnOO '
:OO' (
$strOO) 4
,OO4 5
onDeletePP  
:PP  !
ReferentialActionPP" 3
.PP3 4
RestrictPP4 <
)PP< =
;PP= >
}QQ 
)QQ 
;QQ 
migrationBuilderSS 
.SS 
CreateTableSS (
(SS( )
nameTT 
:TT 
$strTT %
,TT% &
columnsUU 
:UU 
tableUU 
=>UU !
newUU" %
{VV 
HealthRecordIdWW "
=WW# $
tableWW% *
.WW* +
ColumnWW+ 1
<WW1 2
intWW2 5
>WW5 6
(WW6 7
typeWW7 ;
:WW; <
$strWW= B
,WWB C
nullableWWD L
:WWL M
falseWWN S
)WWS T
.XX 

AnnotationXX #
(XX# $
$strXX$ 8
,XX8 9
$strXX: @
)XX@ A
,XXA B
AppointmentIdYY !
=YY" #
tableYY$ )
.YY) *
ColumnYY* 0
<YY0 1
intYY1 4
>YY4 5
(YY5 6
typeYY6 :
:YY: ;
$strYY< A
,YYA B
nullableYYC K
:YYK L
falseYYM R
)YYR S
,YYS T
	PatientIdZZ 
=ZZ 
tableZZ  %
.ZZ% &
ColumnZZ& ,
<ZZ, -
intZZ- 0
>ZZ0 1
(ZZ1 2
typeZZ2 6
:ZZ6 7
$strZZ8 =
,ZZ= >
nullableZZ? G
:ZZG H
falseZZI N
)ZZN O
,ZZO P
DoctorId[[ 
=[[ 
table[[ $
.[[$ %
Column[[% +
<[[+ ,
int[[, /
>[[/ 0
([[0 1
type[[1 5
:[[5 6
$str[[7 <
,[[< =
nullable[[> F
:[[F G
false[[H M
)[[M N
,[[N O
	Diagnosis\\ 
=\\ 
table\\  %
.\\% &
Column\\& ,
<\\, -
string\\- 3
>\\3 4
(\\4 5
type\\5 9
:\\9 :
$str\\; K
,\\K L
	maxLength\\M V
:\\V W
$num\\X \
,\\\ ]
nullable\\^ f
:\\f g
false\\h m
)\\m n
,\\n o
Prescription]]  
=]]! "
table]]# (
.]]( )
Column]]) /
<]]/ 0
string]]0 6
>]]6 7
(]]7 8
type]]8 <
:]]< =
$str]]> N
,]]N O
	maxLength]]P Y
:]]Y Z
$num]][ _
,]]_ `
nullable]]a i
:]]i j
false]]k p
)]]p q
,]]q r
Notes^^ 
=^^ 
table^^ !
.^^! "
Column^^" (
<^^( )
string^^) /
>^^/ 0
(^^0 1
type^^1 5
:^^5 6
$str^^7 G
,^^G H
	maxLength^^I R
:^^R S
$num^^T X
,^^X Y
nullable^^Z b
:^^b c
true^^d h
)^^h i
,^^i j
	CreatedOn__ 
=__ 
table__  %
.__% &
Column__& ,
<__, -
DateTime__- 5
>__5 6
(__6 7
type__7 ;
:__; <
$str__= H
,__H I
nullable__J R
:__R S
false__T Y
)__Y Z
}`` 
,`` 
constraintsaa 
:aa 
tableaa "
=>aa# %
{bb 
tablecc 
.cc 

PrimaryKeycc $
(cc$ %
$strcc% 7
,cc7 8
xcc9 :
=>cc; =
xcc> ?
.cc? @
HealthRecordIdcc@ N
)ccN O
;ccO P
tabledd 
.dd 

ForeignKeydd $
(dd$ %
nameee 
:ee 
$stree K
,eeK L
columnff 
:ff 
xff  !
=>ff" $
xff% &
.ff& '
AppointmentIdff' 4
,ff4 5
principalTablegg &
:gg& '
$strgg( 6
,gg6 7
principalColumnhh '
:hh' (
$strhh) 8
,hh8 9
onDeleteii  
:ii  !
ReferentialActionii" 3
.ii3 4
Restrictii4 <
)ii< =
;ii= >
tablejj 
.jj 

ForeignKeyjj $
(jj$ %
namekk 
:kk 
$strkk A
,kkA B
columnll 
:ll 
xll  !
=>ll" $
xll% &
.ll& '
DoctorIdll' /
,ll/ 0
principalTablemm &
:mm& '
$strmm( 1
,mm1 2
principalColumnnn '
:nn' (
$strnn) 3
,nn3 4
onDeleteoo  
:oo  !
ReferentialActionoo" 3
.oo3 4
Restrictoo4 <
)oo< =
;oo= >
tablepp 
.pp 

ForeignKeypp $
(pp$ %
nameqq 
:qq 
$strqq C
,qqC D
columnrr 
:rr 
xrr  !
=>rr" $
xrr% &
.rr& '
	PatientIdrr' 0
,rr0 1
principalTabless &
:ss& '
$strss( 2
,ss2 3
principalColumntt '
:tt' (
$strtt) 4
,tt4 5
onDeleteuu  
:uu  !
ReferentialActionuu" 3
.uu3 4
Restrictuu4 <
)uu< =
;uu= >
}vv 
)vv 
;vv 
migrationBuilderxx 
.xx 

InsertDataxx '
(xx' (
tableyy 
:yy 
$stryy  
,yy  !
columnszz 
:zz 
newzz 
[zz 
]zz 
{zz  
$strzz! +
,zz+ ,
$strzz- >
,zz> ?
$strzz@ J
,zzJ K
$strzzL V
,zzV W
$strzzX h
,zzh i
$strzzj }
}zz~ 
,	zz Ä
values{{ 
:{{ 
new{{ 
object{{ "
[{{" #
,{{# $
]{{$ %
{|| 
{}} 
$num}} 
,}} 
$num}}  
,}}  !
$str}}" -
,}}- .
true}}/ 3
,}}3 4
$num}}5 6
,}}6 7
$num}}8 9
}}}: ;
,}}; <
{~~ 
$num~~ 
,~~ 
$num~~ !
,~~! "
$str~~# 0
,~~0 1
true~~2 6
,~~6 7
$num~~8 9
,~~9 :
$num~~; =
}~~> ?
} 
) 
; 
migrationBuilder
ÅÅ 
.
ÅÅ 

InsertData
ÅÅ '
(
ÅÅ' (
table
ÇÇ 
:
ÇÇ 
$str
ÇÇ !
,
ÇÇ! "
columns
ÉÉ 
:
ÉÉ 
new
ÉÉ 
[
ÉÉ 
]
ÉÉ 
{
ÉÉ  
$str
ÉÉ! ,
,
ÉÉ, -
$str
ÉÉ. ;
,
ÉÉ; <
$str
ÉÉ= D
,
ÉÉD E
$str
ÉÉF P
,
ÉÉP Q
$str
ÉÉR Z
,
ÉÉZ [
$str
ÉÉ\ m
,
ÉÉm n
$strÉÉo Ä
,ÉÉÄ Å
$strÉÉÇ å
,ÉÉå ç
$strÉÉé õ
}ÉÉú ù
,ÉÉù û
values
ÑÑ 
:
ÑÑ 
new
ÑÑ 
object
ÑÑ "
[
ÑÑ" #
,
ÑÑ# $
]
ÑÑ$ %
{
ÖÖ 
{
ÜÜ 
$num
ÜÜ 
,
ÜÜ 
new
ÜÜ 
DateOnly
ÜÜ %
(
ÜÜ% &
$num
ÜÜ& *
,
ÜÜ* +
$num
ÜÜ, -
,
ÜÜ- .
$num
ÜÜ/ 1
)
ÜÜ1 2
,
ÜÜ2 3
$str
ÜÜ4 G
,
ÜÜG H
$str
ÜÜI W
,
ÜÜW X
$num
ÜÜY Z
,
ÜÜZ [
$str
ÜÜ\ e
,
ÜÜe f
$num
ÜÜg h
,
ÜÜh i
true
ÜÜj n
,
ÜÜn o
$str
ÜÜp |
}
ÜÜ} ~
,
ÜÜ~ 
{
áá 
$num
áá 
,
áá 
new
áá 
DateOnly
áá %
(
áá% &
$num
áá& *
,
áá* +
$num
áá, .
,
áá. /
$num
áá0 1
)
áá1 2
,
áá2 3
$str
áá4 F
,
ááF G
$str
ááH U
,
ááU V
$num
ááW X
,
ááX Y
$str
ááZ c
,
áác d
$num
ááe f
,
ááf g
true
ááh l
,
áál m
$str
áán z
}
áá{ |
}
àà 
)
àà 
;
àà 
migrationBuilder
ää 
.
ää 

InsertData
ää '
(
ää' (
table
ãã 
:
ãã 
$str
ãã %
,
ãã% &
columns
åå 
:
åå 
new
åå 
[
åå 
]
åå 
{
åå  
$str
åå! 0
,
åå0 1
$str
åå2 F
,
ååF G
$str
ååH R
,
ååR S
$str
ååT _
,
åå_ `
$str
ååa p
,
ååp q
$str
åår z
,
ååz {
$stråå| Ü
}ååá à
,ååà â
values
çç 
:
çç 
new
çç 
object
çç "
[
çç" #
]
çç# $
{
çç% &
$num
çç' (
,
çç( )
null
çç* .
,
çç. /
$num
çç0 1
,
çç1 2
$num
çç3 4
,
çç4 5
new
çç6 9
DateOnly
çç: B
(
ççB C
$num
ççC G
,
ççG H
$num
ççI J
,
ççJ K
$num
ççL N
)
ççN O
,
ççO P
$num
ççQ R
,
ççR S
$num
ççT U
}
ççV W
)
ççW X
;
ççX Y
migrationBuilder
èè 
.
èè 
CreateIndex
èè (
(
èè( )
name
êê 
:
êê 
$str
êê G
,
êêG H
table
ëë 
:
ëë 
$str
ëë %
,
ëë% &
columns
íí 
:
íí 
new
íí 
[
íí 
]
íí 
{
íí  
$str
íí! +
,
íí+ ,
$str
íí- <
,
íí< =
$str
íí> H
}
ííI J
,
ííJ K
unique
ìì 
:
ìì 
true
ìì 
)
ìì 
;
ìì 
migrationBuilder
ïï 
.
ïï 
CreateIndex
ïï (
(
ïï( )
name
ññ 
:
ññ 
$str
ññ 1
,
ññ1 2
table
óó 
:
óó 
$str
óó %
,
óó% &
column
òò 
:
òò 
$str
òò #
)
òò# $
;
òò$ %
migrationBuilder
öö 
.
öö 
CreateIndex
öö (
(
öö( )
name
õõ 
:
õõ 
$str
õõ 6
,
õõ6 7
table
úú 
:
úú 
$str
úú &
,
úú& '
column
ùù 
:
ùù 
$str
ùù '
,
ùù' (
unique
ûû 
:
ûû 
true
ûû 
)
ûû 
;
ûû 
migrationBuilder
†† 
.
†† 
CreateIndex
†† (
(
††( )
name
°° 
:
°° 
$str
°° 1
,
°°1 2
table
¢¢ 
:
¢¢ 
$str
¢¢ &
,
¢¢& '
column
££ 
:
££ 
$str
££ "
)
££" #
;
££# $
migrationBuilder
•• 
.
•• 
CreateIndex
•• (
(
••( )
name
¶¶ 
:
¶¶ 
$str
¶¶ 2
,
¶¶2 3
table
ßß 
:
ßß 
$str
ßß &
,
ßß& '
column
®® 
:
®® 
$str
®® #
)
®®# $
;
®®$ %
}
©© 	
	protected
¨¨ 
override
¨¨ 
void
¨¨ 
Down
¨¨  $
(
¨¨$ %
MigrationBuilder
¨¨% 5
migrationBuilder
¨¨6 F
)
¨¨F G
{
≠≠ 	
migrationBuilder
ÆÆ 
.
ÆÆ 
	DropTable
ÆÆ &
(
ÆÆ& '
name
ØØ 
:
ØØ 
$str
ØØ %
)
ØØ% &
;
ØØ& '
migrationBuilder
±± 
.
±± 
	DropTable
±± &
(
±±& '
name
≤≤ 
:
≤≤ 
$str
≤≤ $
)
≤≤$ %
;
≤≤% &
migrationBuilder
¥¥ 
.
¥¥ 
	DropTable
¥¥ &
(
¥¥& '
name
µµ 
:
µµ 
$str
µµ 
)
µµ  
;
µµ  !
migrationBuilder
∑∑ 
.
∑∑ 
	DropTable
∑∑ &
(
∑∑& '
name
∏∏ 
:
∏∏ 
$str
∏∏  
)
∏∏  !
;
∏∏! "
}
ππ 	
}
∫∫ 
}ªª Ä
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Middleware\RequestLoggingMiddleware.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Middleware #
{ 
public 

class $
RequestLoggingMiddleware )
{ 
private 
readonly 
RequestDelegate (
_next) .
;. /
private 
readonly 
ILogger  
<  !$
RequestLoggingMiddleware! 9
>9 :
_logger; B
;B C
public $
RequestLoggingMiddleware '
(' (
RequestDelegate		 
next		  
,		  !
ILogger

 
<

 $
RequestLoggingMiddleware

 ,
>

, -
logger

. 4
)

4 5
{ 	
_next 
= 
next 
; 
_logger 
= 
logger 
; 
} 	
public 
async 
Task 
InvokeAsync %
(% &
HttpContext& 1
context2 9
)9 :
{ 	
_logger 
. 
LogInformation "
(" #
$str 3
,3 4
context 
. 
Request 
.  
Method  &
,& '
context 
. 
Request 
.  
Path  $
)$ %
;% &
await 
_next 
( 
context 
)  
;  !
_logger 
. 
LogInformation "
(" #
$str 1
,1 2
context 
. 
Response  
.  !

StatusCode! +
)+ ,
;, -
} 	
} 
} ß#
]C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Middleware\ExceptionMiddleware.cs
	namespace 	
S3_HealthAxisApi
 
. 

Middleware %
{ 
public 

class 
ExceptionMiddleware $
{ 
private 
readonly 
RequestDelegate (
_next) .
;. /
public

 
ExceptionMiddleware

 "
(

" #
RequestDelegate

# 2
next

3 7
)

7 8
{ 	
_next 
= 
next 
; 
} 	
public 
async 
Task 
InvokeAsync %
(% &
HttpContext& 1
context2 9
)9 :
{ 	
try 
{ 
await 
_next 
( 
context #
)# $
;$ %
} 
catch 
( 
	Exception 
ex 
)  
{ 
await  
HandleExceptionAsync *
(* +
context 
, 
ex 
) 
; 
} 
} 	
private 
static 
async 
Task ! 
HandleExceptionAsync" 6
(6 7
HttpContext 
context 
,  
	Exception 
	exception 
)  
{   	
context!! 
.!! 
Response!! 
.!! 
ContentType!! (
=!!) *
$str"" "
;""" #
var$$ 
response$$ 
=$$ 
new$$ 
ErrorResponse$$ ,
{%% 
Message&& 
=&& 
	exception&& #
.&&# $
Message&&$ +
}'' 
;'' 
switch)) 
()) 
	exception)) 
))) 
{** 
case++  
KeyNotFoundException++ )
:++) *
context,, 
.,, 
Response,, $
.,,$ %

StatusCode,,% /
=,,0 1
(-- 
int-- 
)-- 
HttpStatusCode-- +
.--+ ,
NotFound--, 4
;--4 5
break.. 
;.. 
case00 
ArgumentException00 &
:00& '
context11 
.11 
Response11 $
.11$ %

StatusCode11% /
=110 1
(22 
int22 
)22 
HttpStatusCode22 +
.22+ ,

BadRequest22, 6
;226 7
break33 
;33 
case55 %
InvalidOperationException55 .
:55. /
context66 
.66 
Response66 $
.66$ %

StatusCode66% /
=660 1
(77 
int77 
)77 
HttpStatusCode77 +
.77+ ,

BadRequest77, 6
;776 7
break88 
;88 
case:: '
UnauthorizedAccessException:: 0
:::0 1
context;; 
.;; 
Response;; $
.;;$ %

StatusCode;;% /
=;;0 1
(<< 
int<< 
)<< 
HttpStatusCode<< +
.<<+ ,
Unauthorized<<, 8
;<<8 9
break== 
;== 
default?? 
:?? 
context@@ 
.@@ 
Response@@ $
.@@$ %

StatusCode@@% /
=@@0 1
(AA 
intAA 
)AA 
HttpStatusCodeAA +
.AA+ ,
InternalServerErrorAA, ?
;AA? @
responseCC 
.CC 
MessageCC $
=CC% &
$strDD 7
;DD7 8
breakEE 
;EE 
}FF 
varHH 
jsonHH 
=HH 
JsonSerializerII 
.II 
	SerializeII (
(II( )
responseII) 1
)II1 2
;II2 3
awaitKK 
contextKK 
.KK 
ResponseKK "
.KK" #

WriteAsyncKK# -
(KK- .
jsonKK. 2
)KK2 3
;KK3 4
}LL 	
}MM 
publicOO 

classOO 
ErrorResponseOO 
{PP 
publicQQ 
stringQQ 
MessageQQ 
{QQ 
getQQ  #
;QQ# $
setQQ% (
;QQ( )
}QQ* +
=RR 
stringRR 
.RR 
EmptyRR 
;RR 
}SS 
}TT ı]
PC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Data\AppDbContext.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Data 
{ 
public		 

class		 
HealthAxisDbContext		 $
:		% &
IdentityDbContext		' 8
<		8 9
IdentityUser		9 E
>		E F
{

 
public 
HealthAxisDbContext "
(" #
DbContextOptions# 3
<3 4
HealthAxisDbContext4 G
>G H
optionsI P
)P Q
: 
base 
( 
options 
) 
{ 	
} 	
public 
DbSet 
< 
Patient 
> 
Patients &
=>' )
Set* -
<- .
Patient. 5
>5 6
(6 7
)7 8
;8 9
public 
DbSet 
< 
Doctor 
> 
Doctors $
=>% '
Set( +
<+ ,
Doctor, 2
>2 3
(3 4
)4 5
;5 6
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
=>/ 1
Set2 5
<5 6
Appointment6 A
>A B
(B C
)C D
;D E
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
=>1 3
Set4 7
<7 8
HealthRecord8 D
>D E
(E F
)F G
;G H
public 
DbSet 
< 
User 
> 
Users  
=>! #
Set$ '
<' (
User( ,
>, -
(- .
). /
;/ 0
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
modelBuilder 
. 
Entity 
<  
Appointment  +
>+ ,
(, -
)- .
. 
HasOne 
( 
a 
=> 
a 
. 
Patient &
)& '
.   
WithMany   
(   
p   
=>   
p    
.    !
Appointments  ! -
)  - .
.!! 
HasForeignKey!! 
(!! 
a!!  
=>!!! #
a!!$ %
.!!% &
	PatientId!!& /
)!!/ 0
."" 
OnDelete"" 
("" 
DeleteBehavior"" (
.""( )
Restrict"") 1
)""1 2
;""2 3
modelBuilder$$ 
.$$ 
Entity$$ 
<$$  
Appointment$$  +
>$$+ ,
($$, -
)$$- .
.%% 
HasOne%% 
(%% 
a%% 
=>%% 
a%% 
.%% 
Doctor%% %
)%%% &
.&& 
WithMany&& 
(&& 
d&& 
=>&& 
d&&  
.&&  !
Appointments&&! -
)&&- .
.'' 
HasForeignKey'' 
('' 
a''  
=>''! #
a''$ %
.''% &
DoctorId''& .
)''. /
.(( 
OnDelete(( 
((( 
DeleteBehavior(( (
.((( )
Restrict(() 1
)((1 2
;((2 3
modelBuilder++ 
.++ 
Entity++ 
<++  
Appointment++  +
>+++ ,
(++, -
)++- .
.,, 
HasIndex,, 
(,, 
a,, 
=>,, 
new,, "
{-- 
a.. 
... 
DoctorId.. 
,.. 
a// 
.// 
ScheduledDate// #
,//# $
a00 
.00 
TimeSlot00 
}11 
)11 
.22 
IsUnique22 
(22 
)22 
;22 
modelBuilder77 
.77 
Entity77 
<77  
HealthRecord77  ,
>77, -
(77- .
)77. /
.88 
HasOne88 
(88 
hr88 
=>88 
hr88  
.88  !
Appointment88! ,
)88, -
.99 
WithOne99 
(99 
a99 
=>99 
a99 
.99  
HealthRecord99  ,
)99, -
.:: 
HasForeignKey:: 
<:: 
HealthRecord:: +
>::+ ,
(::, -
hr::- /
=>::0 2
hr::3 5
.::5 6
AppointmentId::6 C
)::C D
.;; 
OnDelete;; 
(;; 
DeleteBehavior;; (
.;;( )
Restrict;;) 1
);;1 2
;;;2 3
modelBuilder== 
.== 
Entity== 
<==  
HealthRecord==  ,
>==, -
(==- .
)==. /
.>> 
HasOne>> 
(>> 
hr>> 
=>>> 
hr>>  
.>>  !
Patient>>! (
)>>( )
.?? 
WithMany?? 
(?? 
p?? 
=>?? 
p??  
.??  !
HealthRecords??! .
)??. /
.@@ 
HasForeignKey@@ 
(@@ 
hr@@ !
=>@@" $
hr@@% '
.@@' (
	PatientId@@( 1
)@@1 2
.AA 
OnDeleteAA 
(AA 
DeleteBehaviorAA (
.AA( )
RestrictAA) 1
)AA1 2
;AA2 3
modelBuilderCC 
.CC 
EntityCC 
<CC  
HealthRecordCC  ,
>CC, -
(CC- .
)CC. /
.DD 
HasOneDD 
(DD 
hrDD 
=>DD 
hrDD  
.DD  !
DoctorDD! '
)DD' (
.EE 
WithManyEE 
(EE 
dEE 
=>EE 
dEE  
.EE  !
HealthRecordsEE! .
)EE. /
.FF 
HasForeignKeyFF 
(FF 
hrFF !
=>FF" $
hrFF% '
.FF' (
DoctorIdFF( 0
)FF0 1
.GG 
OnDeleteGG 
(GG 
DeleteBehaviorGG (
.GG( )
RestrictGG) 1
)GG1 2
;GG2 3
modelBuilderJJ 
.JJ 
EntityJJ 
<JJ  
HealthRecordJJ  ,
>JJ, -
(JJ- .
)JJ. /
.KK 
HasIndexKK 
(KK 
hrKK 
=>KK 
hrKK  "
.KK" #
AppointmentIdKK# 0
)KK0 1
.LL 
IsUniqueLL 
(LL 
)LL 
;LL 
modelBuilderPP 
.PP 
EntityPP 
<PP  
DoctorPP  &
>PP& '
(PP' (
)PP( )
.QQ 
PropertyQQ 
(QQ 
dQQ 
=>QQ 
dQQ  
.QQ  !
ConsultationFeeQQ! 0
)QQ0 1
.RR 
HasPrecisionRR 
(RR 
$numRR  
,RR  !
$numRR" #
)RR# $
;RR$ %
modelBuilderVV 
.VV 
EntityVV 
<VV  
PatientVV  '
>VV' (
(VV( )
)VV) *
.VV* +
HasDataVV+ 2
(VV2 3
newWW 
PatientWW 
{XX 
	PatientIdYY 
=YY 
$numYY  !
,YY! "
FullNameZZ 
=ZZ 
$strZZ -
,ZZ- .
DateOfBirth[[ 
=[[  !
new[[" %
DateOnly[[& .
([[. /
$num[[/ 3
,[[3 4
$num[[5 6
,[[6 7
$num[[8 :
)[[: ;
,[[; <
Gender\\ 
=\\ 
Gender\\ #
.\\# $
Male\\$ (
,\\( )
PhoneNumber]] 
=]]  !
$str]]" .
,]]. /
Email^^ 
=^^ 
$str^^ /
,^^/ 0
InsuranceStatus__ #
=__$ %
InsuranceStatus__& 5
.__5 6
Active__6 <
,__< =
InsuranceNumber`` #
=``$ %
$str``& /
,``/ 0
IsActiveaa 
=aa 
trueaa #
}bb 
,bb 
newcc 
Patientcc 
{dd 
	PatientIdee 
=ee 
$numee  !
,ee! "
FullNameff 
=ff 
$strff ,
,ff, -
DateOfBirthgg 
=gg  !
newgg" %
DateOnlygg& .
(gg. /
$numgg/ 3
,gg3 4
$numgg5 7
,gg7 8
$numgg9 :
)gg: ;
,gg; <
Genderhh 
=hh 
Genderhh #
.hh# $
Femalehh$ *
,hh* +
PhoneNumberii 
=ii  !
$strii" .
,ii. /
Emailjj 
=jj 
$strjj .
,jj. /
InsuranceStatuskk #
=kk$ %
InsuranceStatuskk& 5
.kk5 6
Activekk6 <
,kk< =
InsuranceNumberll #
=ll$ %
$strll& /
,ll/ 0
IsActivemm 
=mm 
truemm #
}nn 
)oo 
;oo 
modelBuilderss 
.ss 
Entityss 
<ss  
Doctorss  &
>ss& '
(ss' (
)ss( )
.ss) *
HasDatass* 1
(ss1 2
newtt 
Doctortt 
{uu 
DoctorIdvv 
=vv 
$numvv  
,vv  !
FullNameww 
=ww 
$strww *
,ww* +
Specialisationxx "
=xx# $ 
DoctorSpecialisationxx% 9
.xx9 :
GeneralPractitionerxx: M
,xxM N
YearsOfExperienceyy %
=yy& '
$numyy( )
,yy) *
ConsultationFeezz #
=zz$ %
$numzz& -
,zz- .
IsActive{{ 
={{ 
true{{ #
}|| 
,|| 
new}} 
Doctor}} 
{~~ 
DoctorId 
= 
$num  
,  !
FullName
ÄÄ 
=
ÄÄ 
$str
ÄÄ ,
,
ÄÄ, -
Specialisation
ÅÅ "
=
ÅÅ# $"
DoctorSpecialisation
ÅÅ% 9
.
ÅÅ9 :
Cardiologist
ÅÅ: F
,
ÅÅF G
YearsOfExperience
ÇÇ %
=
ÇÇ& '
$num
ÇÇ( *
,
ÇÇ* +
ConsultationFee
ÉÉ #
=
ÉÉ$ %
$num
ÉÉ& .
,
ÉÉ. /
IsActive
ÑÑ 
=
ÑÑ 
true
ÑÑ #
}
ÖÖ 
)
ÜÜ 
;
ÜÜ 
modelBuilder
åå 
.
åå 
Entity
åå 
<
åå  
Appointment
åå  +
>
åå+ ,
(
åå, -
)
åå- .
.
åå. /
HasData
åå/ 6
(
åå6 7
new
çç 
Appointment
çç 
{
éé 
AppointmentId
èè !
=
èè" #
$num
èè$ %
,
èè% &
	PatientId
êê 
=
êê 
$num
êê  !
,
êê! "
DoctorId
ëë 
=
ëë 
$num
ëë  
,
ëë  !
ScheduledDate
íí !
=
íí" #
new
íí$ '
DateOnly
íí( 0
(
íí0 1
$num
íí1 5
,
íí5 6
$num
íí7 8
,
íí8 9
$num
íí: <
)
íí< =
,
íí= >
TimeSlot
ìì 
=
ìì !
AppointmentTimeSlot
ìì 2
.
ìì2 3
TenAM
ìì3 8
,
ìì8 9
Status
îî 
=
îî 
AppointmentStatus
îî .
.
îî. /
Pending
îî/ 6
}
ïï 
)
ññ 
;
ññ 
}
óó 	
}
òò 
}ôô ß>
\C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\PatientController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[ 
Route 

(
 
$str 
) 
] 
[		 
ApiController		 
]		 
[

 
	Authorize

 
]

 
public 

class 
PatientsController #
:$ %
ControllerBase& 4
{ 
private 
readonly 
IPatientService (
_patientService) 8
;8 9
public 
PatientsController !
(! "
IPatientService" 1
patientService2 @
)@ A
{ 	
_patientService 
= 
patientService ,
;, -
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
)0 1
{ 	
var 
patients 
= 
await  
_patientService! 0
.0 1
GetAllAsync1 <
(< =
)= >
;> ?
return 
Ok 
( 
patients 
) 
;  
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str )
)) *
]* +
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
int1 4
id5 7
)7 8
{   	
var!! 
patient!! 
=!! 
await!! 
_patientService!!  /
.!!/ 0
GetByIdAsync!!0 <
(!!< =
id!!= ?
)!!? @
;!!@ A
if## 
(## 
patient## 
==## 
null## 
)##  
{$$ 
return%% 
NotFound%% 
(%%  
$"%%  "
$str%%" 2
{%%2 3
id%%3 5
}%%5 6
$str%%6 A
"%%A B
)%%B C
;%%C D
}&& 
return(( 
Ok(( 
((( 
patient(( 
)(( 
;(( 
})) 	
[++ 	
HttpGet++	 
(++ 
$str++ 
)++ 
]++ 
[,, 	
	Authorize,,	 
(,, 
Roles,, 
=,, 
$str,, )
),,) *
],,* +
public-- 
async-- 
Task-- 
<-- 
IActionResult-- '
>--' (
Search--) /
(--/ 0
[.. 
	FromQuery.. 
].. 
string.. 
name.. #
)..# $
{// 	
var00 
patients00 
=00 
await11 
_patientService11 %
.11% &
SearchByNameAsync11& 7
(117 8
name118 <
)11< =
;11= >
return33 
Ok33 
(33 
patients33 
)33 
;33  
}44 	
[66 	
HttpPost66	 
]66 
public77 
async77 
Task77 
<77 
IActionResult77 '
>77' (
Create77) /
(77/ 0
CreatePatientDto88 
dto88  
)88  !
{99 	
try:: 
{;; 
var<< 
patient<< 
=<< 
await== 
_patientService== )
.==) *
CreateAsync==* 5
(==5 6
dto==6 9
)==9 :
;==: ;
return?? 
CreatedAtAction?? &
(??& '
nameof@@ 
(@@ 
GetById@@ "
)@@" #
,@@# $
newAA 
{AA 
idAA 
=AA 
patientAA &
.AA& '
	PatientIdAA' 0
}AA1 2
,AA2 3
patientBB 
)BB 
;BB 
}CC 
catchDD 
(DD 
ArgumentExceptionDD $
exDD% '
)DD' (
{EE 
returnFF 

BadRequestFF !
(FF! "
exFF" $
.FF$ %
MessageFF% ,
)FF, -
;FF- .
}GG 
}HH 	
[JJ 	
HttpPutJJ	 
(JJ 
$strJJ 
)JJ 
]JJ 
publicKK 
asyncKK 
TaskKK 
<KK 
IActionResultKK '
>KK' (
UpdateKK) /
(KK/ 0
intLL 
idLL 
,LL 
UpdatePatientDtoMM 
dtoMM  
)MM  !
{NN 	
tryOO 
{PP 
awaitQQ 
_patientServiceQQ %
.QQ% &
UpdateAsyncQQ& 1
(QQ1 2
idQQ2 4
,QQ4 5
dtoQQ6 9
)QQ9 :
;QQ: ;
returnSS 
	NoContentSS  
(SS  !
)SS! "
;SS" #
}TT 
catchUU 
(UU  
KeyNotFoundExceptionUU '
exUU( *
)UU* +
{VV 
returnWW 
NotFoundWW 
(WW  
exWW  "
.WW" #
MessageWW# *
)WW* +
;WW+ ,
}XX 
catchYY 
(YY 
ArgumentExceptionYY $
exYY% '
)YY' (
{ZZ 
return[[ 

BadRequest[[ !
([[! "
ex[[" $
.[[$ %
Message[[% ,
)[[, -
;[[- .
}\\ 
}]] 	
[__ 	
HttpPut__	 
(__ 
$str__ $
)__$ %
]__% &
[`` 	
	Authorize``	 
(`` 
Roles`` 
=`` 
$str`` "
)``" #
]``# $
publicaa 
asyncaa 
Taskaa 
<aa 
IActionResultaa '
>aa' (
Activateaa) 1
(aa1 2
intaa2 5
idaa6 8
)aa8 9
{bb 	
trycc 
{dd 
awaitee 
_patientServiceee %
.ee% &
ActivateAsyncee& 3
(ee3 4
idee4 6
)ee6 7
;ee7 8
returngg 
	NoContentgg  
(gg  !
)gg! "
;gg" #
}hh 
catchii 
(ii  
KeyNotFoundExceptionii '
exii( *
)ii* +
{jj 
returnkk 
NotFoundkk 
(kk  
exkk  "
.kk" #
Messagekk# *
)kk* +
;kk+ ,
}ll 
}mm 	
[oo 	
HttpPutoo	 
(oo 
$stroo &
)oo& '
]oo' (
[pp 	
	Authorizepp	 
(pp 
Rolespp 
=pp 
$strpp "
)pp" #
]pp# $
publicqq 
asyncqq 
Taskqq 
<qq 
IActionResultqq '
>qq' (

Deactivateqq) 3
(qq3 4
intqq4 7
idqq8 :
)qq: ;
{rr 	
tryss 
{tt 
awaituu 
_patientServiceuu %
.uu% &
DeactivateAsyncuu& 5
(uu5 6
iduu6 8
)uu8 9
;uu9 :
returnww 
	NoContentww  
(ww  !
)ww! "
;ww" #
}xx 
catchyy 
(yy  
KeyNotFoundExceptionyy '
exyy( *
)yy* +
{zz 
return{{ 
NotFound{{ 
({{  
ex{{  "
.{{" #
Message{{# *
){{* +
;{{+ ,
}|| 
}}} 	
}~~ 
} µ0
aC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\HealthRecordController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[ 
Route 

(
 
$str 
) 
] 
[		 
ApiController		 
]		 
[

 
	Authorize

 
]

 
public 

class #
HealthRecordsController (
:) *
ControllerBase+ 9
{ 
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public #
HealthRecordsController &
(& ' 
IHealthRecordService  
healthRecordService! 4
)4 5
{ 	 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
int1 4
id5 7
)7 8
{ 	
var 
record 
= 
await  
_healthRecordService *
.* +
GetByIdAsync+ 7
(7 8
id8 :
): ;
;; <
if 
( 
record 
== 
null 
) 
{ 
return 
NotFound 
(  
$" 
$str $
{$ %
id% '
}' (
$str( 3
"3 4
)4 5
;5 6
} 
return!! 
Ok!! 
(!! 
record!! 
)!! 
;!! 
}"" 	
[$$ 	
HttpGet$$	 
($$ 
$str$$ 2
)$$2 3
]$$3 4
public%% 
async%% 
Task%% 
<%% 
IActionResult%% '
>%%' (
GetByAppointment%%) 9
(%%9 :
int&& 
appointmentId&& 
)&& 
{'' 	
var(( 
record(( 
=(( 
await))  
_healthRecordService)) *
.** #
GetByAppointmentIdAsync** ,
(**, -
appointmentId**- :
)**: ;
;**; <
if,, 
(,, 
record,, 
==,, 
null,, 
),, 
{-- 
return.. 
NotFound.. 
(..  
$str// C
)//C D
;//D E
}00 
return22 
Ok22 
(22 
record22 
)22 
;22 
}33 	
[55 	
HttpPost55	 
]55 
[66 	
	Authorize66	 
(66 
Roles66 
=66 
$str66 )
)66) *
]66* +
public77 
async77 
Task77 
<77 
IActionResult77 '
>77' (
Create77) /
(77/ 0
[88 
FromBody88 
]88 !
CreateHealthRecordDto88 ,
dto88- 0
)880 1
{99 	
try:: 
{;; 
var<< 
record<< 
=<< 
await==  
_healthRecordService== .
.==. /
CreateAsync==/ :
(==: ;
dto==; >
)==> ?
;==? @
return?? 
CreatedAtAction?? &
(??& '
nameof@@ 
(@@ 
GetById@@ "
)@@" #
,@@# $
newAA 
{AA 
idAA 
=AA 
recordAA %
.AA% &
HealthRecordIdAA& 4
}AA5 6
,AA6 7
recordBB 
)BB 
;BB 
}CC 
catchDD 
(DD  
KeyNotFoundExceptionDD '
exDD( *
)DD* +
{EE 
returnFF 
NotFoundFF 
(FF  
exFF  "
.FF" #
MessageFF# *
)FF* +
;FF+ ,
}GG 
catchHH 
(HH 
ArgumentExceptionHH $
exHH% '
)HH' (
{II 
returnJJ 

BadRequestJJ !
(JJ! "
exJJ" $
.JJ$ %
MessageJJ% ,
)JJ, -
;JJ- .
}KK 
catchLL 
(LL %
InvalidOperationExceptionLL ,
exLL- /
)LL/ 0
{MM 
returnNN 

BadRequestNN !
(NN! "
exNN" $
.NN$ %
MessageNN% ,
)NN, -
;NN- .
}OO 
}PP 	
[RR 	
HttpPutRR	 
(RR 
$strRR 
)RR 
]RR 
[SS 	
	AuthorizeSS	 
(SS 
RolesSS 
=SS 
$strSS )
)SS) *
]SS* +
publicTT 
asyncTT 
TaskTT 
<TT 
IActionResultTT '
>TT' (
UpdateTT) /
(TT/ 0
intUU 
idUU 
,UU 
[VV 
FromBodyVV 
]VV !
UpdateHealthRecordDtoVV ,
dtoVV- 0
)VV0 1
{WW 	
tryXX 
{YY 
awaitZZ  
_healthRecordServiceZZ *
.[[ 
UpdateAsync[[  
([[  !
id[[! #
,[[# $
dto[[% (
)[[( )
;[[) *
return]] 
	NoContent]]  
(]]  !
)]]! "
;]]" #
}^^ 
catch__ 
(__  
KeyNotFoundException__ '
ex__( *
)__* +
{`` 
returnaa 
NotFoundaa 
(aa  
exaa  "
.aa" #
Messageaa# *
)aa* +
;aa+ ,
}bb 
catchcc 
(cc 
ArgumentExceptioncc $
excc% '
)cc' (
{dd 
returnee 

BadRequestee !
(ee! "
exee" $
.ee$ %
Messageee% ,
)ee, -
;ee- .
}ff 
}gg 	
}hh 
}ii ∑:
[C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\DoctorController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[ 
Route 

(
 
$str 
) 
] 
[		 
ApiController		 
]		 
[

 
	Authorize

 
]

 
public 

class 
DoctorsController "
:# $
ControllerBase% 3
{ 
private 
readonly 
IDoctorService '
_doctorService( 6
;6 7
public 
DoctorsController  
(  !
IDoctorService 
doctorService (
)( )
{ 	
_doctorService 
= 
doctorService *
;* +
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
[ 
	FromQuery 
] 
string 
? 
sortBy  &
,& '
[ 
	FromQuery 
] 
int 
? 
specialisation +
)+ ,
{ 	
var 
doctors 
= 
await 
_doctorService $
.$ %
GetAllAsync% 0
(0 1
sortBy 
, 
specialisation "
)" #
;# $
return 
Ok 
( 
doctors 
) 
; 
}   	
["" 	
HttpGet""	 
("" 
$str"" 
)"" 
]"" 
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
GetById##) 0
(##0 1
int$$ 
id$$ 
)$$ 
{%% 	
var&& 
doctor&& 
=&& 
await'' 
_doctorService'' $
.''$ %
GetByIdAsync''% 1
(''1 2
id''2 4
)''4 5
;''5 6
if)) 
()) 
doctor)) 
==)) 
null)) 
))) 
{** 
return++ 
NotFound++ 
(++  
$",, 
$str,, %
{,,% &
id,,& (
},,( )
$str,,) 4
",,4 5
),,5 6
;,,6 7
}-- 
return// 
Ok// 
(// 
doctor// 
)// 
;// 
}00 	
[22 	
HttpGet22	 
(22 
$str22 6
)226 7
]227 8
public33 
async33 
Task33 
<33 
IActionResult33 '
>33' (
GetBySpecialisation44 
(44  
int55 
specialisation55 "
)55" #
{66 	
var77 
doctors77 
=77 
await88 
_doctorService88 $
.99 *
GetActiveBySpecialisationAsync99 3
(993 4
specialisation:: &
)::& '
;::' (
return<< 
Ok<< 
(<< 
doctors<< 
)<< 
;<< 
}== 	
[?? 	
HttpPost??	 
]?? 
[@@ 	
	Authorize@@	 
(@@ 
Roles@@ 
=@@ 
$str@@ "
)@@" #
]@@# $
publicAA 
asyncAA 
TaskAA 
<AA 
IActionResultAA '
>AA' (
CreateAA) /
(AA/ 0
[BB 
FromBodyBB 
]BB 
CreateDoctorDtoBB &
dtoBB' *
)BB* +
{CC 	
varDD 
doctorDD 
=DD 
awaitEE 
_doctorServiceEE $
.FF (
CreateDoctorWithAccountAsyncFF 1
(FF1 2
dtoFF2 5
)FF5 6
;FF6 7
returnHH 
CreatedAtActionHH "
(HH" #
nameofII 
(II 
GetByIdII 
)II 
,II  
newJJ 
{JJ 
idJJ 
=JJ 
doctorJJ !
.JJ! "
DoctorIdJJ" *
}JJ+ ,
,JJ, -
doctorKK 
)KK 
;KK 
}LL 	
[NN 	
HttpPutNN	 
(NN 
$strNN 
)NN 
]NN 
[OO 	
	AuthorizeOO	 
(OO 
RolesOO 
=OO 
$strOO "
)OO" #
]OO# $
publicPP 
asyncPP 
TaskPP 
<PP 
IActionResultPP '
>PP' (
UpdatePP) /
(PP/ 0
intQQ 
idQQ 
,QQ 
[RR 
FromBodyRR 
]RR 
UpdateDoctorDtoRR &
dtoRR' *
)RR* +
{SS 	
awaitTT 
_doctorServiceTT  
.TT  !
UpdateAsyncTT! ,
(TT, -
idUU 
,UU 
dtoVV 
)VV 
;VV 
returnXX 
	NoContentXX 
(XX 
)XX 
;XX 
}YY 	
[[[ 	
HttpGet[[	 
([[ 
$str[[ (
)[[( )
][[) *
public\\ 
async\\ 
Task\\ 
<\\ 
IActionResult\\ '
>\\' (
GetAvailability]] 
(]] 
int^^ 
id^^ 
,^^ 
[__ 
	FromQuery__ 
]__ 
DateOnly__ $
date__% )
)__) *
{`` 	
varaa 
slotsaa 
=aa 
awaitbb 
_doctorServicebb $
.cc  
GetAvailabilityAsynccc )
(cc) *
iddd 
,dd 
dateee 
)ee 
;ee 
returngg 
Okgg 
(gg 
slotsgg 
)gg 
;gg 
}hh 	
[jj 	
HttpPutjj	 
(jj 
$strjj $
)jj$ %
]jj% &
[kk 	
	Authorizekk	 
(kk 
Roleskk 
=kk 
$strkk "
)kk" #
]kk# $
publicll 
asyncll 
Taskll 
<ll 
IActionResultll '
>ll' (
Activatell) 1
(ll1 2
intmm 
idmm 
)mm 
{nn 	
awaitoo 
_doctorServiceoo  
.oo  !
ActivateAsyncoo! .
(oo. /
idoo/ 1
)oo1 2
;oo2 3
returnqq 
	NoContentqq 
(qq 
)qq 
;qq 
}rr 	
[tt 	
HttpPuttt	 
(tt 
$strtt &
)tt& '
]tt' (
[uu 	
	Authorizeuu	 
(uu 
Rolesuu 
=uu 
$struu "
)uu" #
]uu# $
publicvv 
asyncvv 
Taskvv 
<vv 
IActionResultvv '
>vv' (

Deactivatevv) 3
(vv3 4
intww 
idww 
)ww 
{xx 	
awaityy 
_doctorServiceyy  
.yy  !
DeactivateAsyncyy! 0
(yy0 1
idyy1 3
)yy3 4
;yy4 5
return{{ 
	NoContent{{ 
({{ 
){{ 
;{{ 
}|| 	
}}} 
} Ñ%
YC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\AuthController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public		 

class		 
AuthController		 
:		  !
ControllerBase		" 0
{

 
private 
readonly 
IAuthService %
_authService& 2
;2 3
public 
AuthController 
( 
IAuthService *
authService+ 6
)6 7
{ 	
_authService 
= 
authService &
;& '
} 	
[ 	
HttpPost	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Register) 1
(1 2
RegisterDto2 =
dto> A
)A B
{ 	
var 
result 
= 
await 
_authService +
.+ ,
RegisterAsync, 9
(9 :
dto: =
)= >
;> ?
if 
( 
! 
result 
. 
Success 
)  
{ 
return 

BadRequest !
(! "
result" (
.( )
Message) 0
)0 1
;1 2
} 
return 
Ok 
( 
result 
. 
Data !
)! "
;" #
} 	
[ 	
HttpPost	 
( 
$str $
)$ %
]% &
public   
async   
Task   
<   
IActionResult   '
>  ' (
RegisterPatient  ) 8
(  8 9
RegisterPatientDto  9 K
dto  L O
)  O P
{!! 	
var"" 
result"" 
="" 
await## 
_authService## "
.##" # 
RegisterPatientAsync### 7
(##7 8
dto##8 ;
)##; <
;##< =
if%% 
(%% 
!%% 
result%% 
.%% 
Success%% 
)%%  
{&& 
return'' 

BadRequest'' !
(''! "
result''" (
.''( )
Message'') 0
)''0 1
;''1 2
}(( 
return** 
Ok** 
(** 
result** 
.** 
Data** !
)**! "
;**" #
}++ 	
[-- 	
HttpPost--	 
(-- 
$str-- 
)-- 
]-- 
public.. 
async.. 
Task.. 
<.. 
IActionResult.. '
>..' (
Login..) .
(... /
LoginDto../ 7
dto..8 ;
)..; <
{// 	
var00 
result00 
=00 
await00 
_authService00 +
.00+ ,

LoginAsync00, 6
(006 7
dto007 :
)00: ;
;00; <
if22 
(22 
!22 
result22 
.22 
Success22 
)22  
{33 
return44 
Unauthorized44 #
(44# $
result44$ *
.44* +
Message44+ 2
)442 3
;443 4
}55 
return77 
Ok77 
(77 
result77 
.77 
Data77 !
)77! "
;77" #
}88 	
[:: 	
HttpPost::	 
(:: 
$str:: !
)::! "
]::" #
public;; 
async;; 
Task;; 
<;; 
IActionResult;; '
>;;' (
RefreshToken;;) 5
(;;5 6
RefreshTokenDto;;6 E
dto;;F I
);;I J
{<< 	
var== 
result== 
=== 
await>> 
_authService>> "
.>>" #
RefreshTokenAsync>># 4
(>>4 5
dto>>5 8
)>>8 9
;>>9 :
if@@ 
(@@ 
!@@ 
result@@ 
.@@ 
Success@@ 
)@@  
{AA 
returnBB 
UnauthorizedBB #
(BB# $
resultBB$ *
.BB* +
MessageBB+ 2
)BB2 3
;BB3 4
}CC 
returnEE 
OkEE 
(EE 
resultEE 
.EE 
DataEE !
)EE! "
;EE" #
}FF 	
}II 
}JJ Ãw
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\AppointmentController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[ 
Route 

(
 
$str 
) 
] 
[		 
ApiController		 
]		 
[

 
	Authorize

 
]

 
public 

class "
AppointmentsController '
:( )
ControllerBase* 8
{ 
private 
readonly 
IAppointmentService ,
_appointmentService- @
;@ A
public "
AppointmentsController %
(% &
IAppointmentService 
appointmentService  2
)2 3
{ 	
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
ActionResult &
<& '
IEnumerable' 2
<2 3!
AppointmentDetailsDto3 H
>H I
>I J
>J K
GetAllL R
(R S
)S T
{ 	
var 
appointments 
= 
await $
_appointmentService% 8
.8 9
GetAllAsync9 D
(D E
)E F
;F G
return 
Ok 
( 
appointments "
)" #
;# $
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
int1 4
id5 7
)7 8
{ 	
var   
appointment   
=   
await!! 
_appointmentService!! )
.!!) *
GetByIdAsync!!* 6
(!!6 7
id!!7 9
)!!9 :
;!!: ;
if## 
(## 
appointment## 
==## 
null## #
)### $
return$$ 
NotFound$$ 
($$  
$"%% 
$str%% "
{%%" #
id%%# %
}%%% &
$str%%& 1
"%%1 2
)%%2 3
;%%3 4
return'' 
Ok'' 
('' 
appointment'' !
)''! "
;''" #
}(( 	
[** 	
HttpGet**	 
(** 
$str** *
)*** +
]**+ ,
public++ 
async++ 
Task++ 
<++ 
IActionResult++ '
>++' (
GetPatientHistory++) :
(++: ;
int,, 
	patientId,, 
),, 
{-- 	
var.. 
appointments.. 
=.. 
await// 
_appointmentService// )
.00 "
GetPatientHistoryAsync00 +
(00+ ,
	patientId00, 5
)005 6
;006 7
return22 
Ok22 
(22 
appointments22 "
)22" #
;22# $
}33 	
[55 	
HttpGet55	 
(55 
$str55 .
)55. /
]55/ 0
[66 	
	Authorize66	 
(66 
Roles66 
=66 
$str66 )
)66) *
]66* +
public77 
async77 
Task77 
<77 
IActionResult77 '
>77' ("
GetDoctorTodaySchedule77) ?
(77? @
int88 
doctorId88 
)88 
{99 	
var:: 
schedule:: 
=:: 
await;; 
_appointmentService;; )
.<< '
GetDoctorTodayScheduleAsync<< 0
(<<0 1
doctorId<<1 9
)<<9 :
;<<: ;
return>> 
Ok>> 
(>> 
schedule>> 
)>> 
;>>  
}?? 	
[AA 	
HttpGetAA	 
(AA 
$strAA -
)AA- .
]AA. /
[BB 	
	AuthorizeBB	 
(BB 
RolesBB 
=BB 
$strBB )
)BB) *
]BB* +
publicCC 
asyncCC 
TaskCC 
<CC 
IActionResultCC '
>CC' (!
GetDoctorWeekScheduleCC) >
(CC> ?
intDD 
doctorIdDD 
,DD 
[EE 
	FromQueryEE 
]EE 
DateOnlyEE  
	startDateEE! *
,EE* +
[FF 
	FromQueryFF 
]FF 
DateOnlyFF  
endDateFF! (
)FF( )
{GG 	
varHH 
scheduleHH 
=HH 
awaitII 
_appointmentServiceII )
.JJ &
GetDoctorWeekScheduleAsyncJJ /
(JJ/ 0
doctorIdKK  
,KK  !
	startDateLL !
,LL! "
endDateMM 
)MM  
;MM  !
returnOO 
OkOO 
(OO 
scheduleOO 
)OO 
;OO  
}PP 	
[RR 	
HttpGetRR	 
(RR 
$strRR -
)RR- .
]RR. /
[SS 	
	AuthorizeSS	 
(SS 
RolesSS 
=SS 
$strSS )
)SS) *
]SS* +
publicTT 
asyncTT 
TaskTT 
<TT 
IActionResultTT '
>TT' (%
GetDoctorUpcomingScheduleTT) B
(TTB C
intTTC F
doctorIdTTG O
)TTO P
{UU 	
varVV 
resultVV 
=VV 
awaitWW 
_appointmentServiceWW )
.XX *
GetDoctorUpcomingScheduleAsyncXX 3
(XX3 4
doctorIdYY  
)YY  !
;YY! "
return[[ 
Ok[[ 
([[ 
result[[ 
)[[ 
;[[ 
}\\ 	
[^^ 	
HttpPost^^	 
]^^ 
[__ 	
	Authorize__	 
(__ 
Roles__ 
=__ 
$str__ *
)__* +
]__+ ,
public`` 
async`` 
Task`` 
<`` 
IActionResult`` '
>``' (
Create``) /
(``/ 0
[aa 
FromBodyaa 
]aa  
CreateAppointmentDtoaa +
dtoaa, /
)aa/ 0
{bb 	
trycc 
{dd 
varee 
appointmentee 
=ee  !
awaitff 
_appointmentServiceff -
.ff- .
CreateAsyncff. 9
(ff9 :
dtoff: =
)ff= >
;ff> ?
returnhh 
CreatedAtActionhh &
(hh& '
nameofii 
(ii 
GetByIdii "
)ii" #
,ii# $
newjj 
{jj 
idjj 
=jj 
appointmentjj *
.jj* +
AppointmentIdjj+ 8
}jj9 :
,jj: ;
appointmentkk 
)kk  
;kk  !
}ll 
catchmm 
(mm 
ArgumentExceptionnn !
exnn" $
)nn$ %
{oo 
returnpp 

BadRequestpp !
(pp! "
expp" $
.pp$ %
Messagepp% ,
)pp, -
;pp- .
}qq 
catchrr 
(rr %
InvalidOperationExceptionss )
exss* ,
)ss, -
{tt 
returnuu 

BadRequestuu !
(uu! "
exuu" $
.uu$ %
Messageuu% ,
)uu, -
;uu- .
}vv 
catchww 
(ww  
KeyNotFoundExceptionxx $
exxx% '
)xx' (
{yy 
returnzz 
NotFoundzz 
(zz  
exzz  "
.zz" #
Messagezz# *
)zz* +
;zz+ ,
}{{ 
}|| 	
[~~ 	
HttpPut~~	 
(~~ 
$str~~ 
)~~ 
]~~ 
[ 	
	Authorize	 
( 
Roles 
= 
$str *
)* +
]+ ,
public
ÄÄ 
async
ÄÄ 
Task
ÄÄ 
<
ÄÄ 
IActionResult
ÄÄ '
>
ÄÄ' (
Update
ÄÄ) /
(
ÄÄ/ 0
int
ÅÅ 
id
ÅÅ 
,
ÅÅ 
[
ÇÇ 
FromBody
ÇÇ 
]
ÇÇ "
UpdateAppointmentDto
ÇÇ +
dto
ÇÇ, /
)
ÇÇ/ 0
{
ÉÉ 	
try
ÑÑ 
{
ÖÖ 
await
ÜÜ !
_appointmentService
ÜÜ )
.
áá 
UpdateAsync
áá  
(
áá  !
id
áá! #
,
áá# $
dto
áá% (
)
áá( )
;
áá) *
return
ââ 
	NoContent
ââ  
(
ââ  !
)
ââ! "
;
ââ" #
}
ää 
catch
ãã 
(
ãã "
KeyNotFoundException
åå $
ex
åå% '
)
åå' (
{
çç 
return
éé 
NotFound
éé 
(
éé  
ex
éé  "
.
éé" #
Message
éé# *
)
éé* +
;
éé+ ,
}
èè 
catch
êê 
(
êê 
ArgumentException
ëë !
ex
ëë" $
)
ëë$ %
{
íí 
return
ìì 

BadRequest
ìì !
(
ìì! "
ex
ìì" $
.
ìì$ %
Message
ìì% ,
)
ìì, -
;
ìì- .
}
îî 
catch
ïï 
(
ïï '
InvalidOperationException
ññ )
ex
ññ* ,
)
ññ, -
{
óó 
return
òò 

BadRequest
òò !
(
òò! "
ex
òò" $
.
òò$ %
Message
òò% ,
)
òò, -
;
òò- .
}
ôô 
}
öö 	
[
úú 	
HttpPut
úú	 
(
úú 
$str
úú #
)
úú# $
]
úú$ %
[
ùù 	
	Authorize
ùù	 
(
ùù 
Roles
ùù 
=
ùù 
$str
ùù )
)
ùù) *
]
ùù* +
public
ûû 
async
ûû 
Task
ûû 
<
ûû 
IActionResult
ûû '
>
ûû' (
Confirm
ûû) 0
(
ûû0 1
int
ûû1 4
id
ûû5 7
)
ûû7 8
{
üü 	
try
†† 
{
°° 
await
¢¢ !
_appointmentService
¢¢ )
.
££ 
ConfirmAsync
££ !
(
££! "
id
££" $
)
££$ %
;
££% &
return
•• 
	NoContent
••  
(
••  !
)
••! "
;
••" #
}
¶¶ 
catch
ßß 
(
ßß "
KeyNotFoundException
®® $
)
®®$ %
{
©© 
return
™™ 
NotFound
™™ 
(
™™  
)
™™  !
;
™™! "
}
´´ 
catch
¨¨ 
(
¨¨ '
InvalidOperationException
≠≠ )
ex
≠≠* ,
)
≠≠, -
{
ÆÆ 
return
ØØ 

BadRequest
ØØ !
(
ØØ! "
ex
ØØ" $
.
ØØ$ %
Message
ØØ% ,
)
ØØ, -
;
ØØ- .
}
∞∞ 
}
±± 	
[
≥≥ 	
HttpPut
≥≥	 
(
≥≥ 
$str
≥≥ $
)
≥≥$ %
]
≥≥% &
[
¥¥ 	
	Authorize
¥¥	 
(
¥¥ 
Roles
¥¥ 
=
¥¥ 
$str
¥¥ )
)
¥¥) *
]
¥¥* +
public
µµ 
async
µµ 
Task
µµ 
<
µµ 
IActionResult
µµ '
>
µµ' (
Complete
µµ) 1
(
µµ1 2
int
µµ2 5
id
µµ6 8
)
µµ8 9
{
∂∂ 	
try
∑∑ 
{
∏∏ 
await
ππ !
_appointmentService
ππ )
.
∫∫ 
CompleteAsync
∫∫ "
(
∫∫" #
id
∫∫# %
)
∫∫% &
;
∫∫& '
return
ºº 
	NoContent
ºº  
(
ºº  !
)
ºº! "
;
ºº" #
}
ΩΩ 
catch
ææ 
(
ææ "
KeyNotFoundException
øø $
)
øø$ %
{
¿¿ 
return
¡¡ 
NotFound
¡¡ 
(
¡¡  
)
¡¡  !
;
¡¡! "
}
¬¬ 
catch
√√ 
(
√√ '
InvalidOperationException
ƒƒ )
ex
ƒƒ* ,
)
ƒƒ, -
{
≈≈ 
return
∆∆ 

BadRequest
∆∆ !
(
∆∆! "
ex
∆∆" $
.
∆∆$ %
Message
∆∆% ,
)
∆∆, -
;
∆∆- .
}
«« 
}
»» 	
[
   	
HttpPut
  	 
(
   
$str
   
)
   
]
    
[
ÀÀ 	
	Authorize
ÀÀ	 
]
ÀÀ 
public
ÃÃ 
async
ÃÃ 
Task
ÃÃ 
<
ÃÃ 
IActionResult
ÃÃ '
>
ÃÃ' (
UpdateStatus
ÃÃ) 5
(
ÃÃ5 6
int
ÃÃ6 9
id
ÃÃ: <
,
ÃÃ< =(
UpdateAppointmentStatusDto
ÃÃ> X
dto
ÃÃY \
)
ÃÃ\ ]
{
ÕÕ 	
await
ŒŒ !
_appointmentService
ŒŒ %
.
œœ 
UpdateStatusAsync
œœ "
(
œœ" #
id
œœ# %
,
œœ% &
dto
œœ' *
)
œœ* +
;
œœ+ ,
return
—— 
	NoContent
—— 
(
—— 
)
—— 
;
—— 
}
““ 	
[
‘‘ 	
HttpPut
‘‘	 
(
‘‘ 
$str
‘‘ "
)
‘‘" #
]
‘‘# $
[
’’ 	
	Authorize
’’	 
(
’’ 
Roles
’’ 
=
’’ 
$str
’’ *
)
’’* +
]
’’+ ,
public
÷÷ 
async
÷÷ 
Task
÷÷ 
<
÷÷ 
IActionResult
÷÷ '
>
÷÷' (
Cancel
÷÷) /
(
÷÷/ 0
int
◊◊ 
id
◊◊ 
,
◊◊ 
[
ÿÿ 
FromBody
ÿÿ 
]
ÿÿ "
CancelAppointmentDto
ÿÿ +
dto
ÿÿ, /
)
ÿÿ/ 0
{
ŸŸ 	
try
⁄⁄ 
{
€€ 
await
‹‹ !
_appointmentService
‹‹ )
.
›› 
CancelAsync
››  
(
››  !
id
››! #
,
››# $
dto
››% (
)
››( )
;
››) *
return
ﬂﬂ 
	NoContent
ﬂﬂ  
(
ﬂﬂ  !
)
ﬂﬂ! "
;
ﬂﬂ" #
}
‡‡ 
catch
·· 
(
·· "
KeyNotFoundException
‚‚ $
)
‚‚$ %
{
„„ 
return
‰‰ 
NotFound
‰‰ 
(
‰‰  
)
‰‰  !
;
‰‰! "
}
ÂÂ 
catch
ÊÊ 
(
ÊÊ 
ArgumentException
ÁÁ !
ex
ÁÁ" $
)
ÁÁ$ %
{
ËË 
return
ÈÈ 

BadRequest
ÈÈ !
(
ÈÈ! "
ex
ÈÈ" $
.
ÈÈ$ %
Message
ÈÈ% ,
)
ÈÈ, -
;
ÈÈ- .
}
ÍÍ 
catch
ÎÎ 
(
ÎÎ '
InvalidOperationException
ÏÏ )
ex
ÏÏ* ,
)
ÏÏ, -
{
ÌÌ 
return
ÓÓ 

BadRequest
ÓÓ !
(
ÓÓ! "
ex
ÓÓ" $
.
ÓÓ$ %
Message
ÓÓ% ,
)
ÓÓ, -
;
ÓÓ- .
}
ÔÔ 
}
 	
}
ÒÒ 
}ÚÚ Ì
ZC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\AdminController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[		 
	Authorize		 
(		 
Roles		 
=		 
$str		 
)		 
]		  
[

 
Route

 

(


 
$str

 
)

 
]

 
[ 
ApiController 
] 
public 

class 
AdminController  
:! "
ControllerBase# 1
{ 
private 
readonly 
IAdminService &
_adminService' 4
;4 5
public 
AdminController 
( 
IAdminService 
adminService &
)& '
{ 	
_adminService 
= 
adminService (
;( )
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetDashboard 
( 
) 
{ 	
return 
Ok 
( 
await 
_adminService #
. 
GetDashboardAsync &
(& '
)' (
)( )
;) *
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (
GetStatistics!! 
(!! 
)!! 
{"" 	
return## 
Ok## 
(## 
await$$ 
_adminService$$ #
.%% 
GetStatisticsAsync%% '
(%%' (
)%%( )
)%%) *
;%%* +
}&& 	
[(( 	
HttpGet((	 
((( 
$str(( 
)(( 
](( 
public)) 
async)) 
Task)) 
<)) 
IActionResult)) '
>))' (
GetUsers** 
(** 
)** 
{++ 	
return,, 
Ok,, 
(,, 
await-- 
_adminService-- #
... 
GetUsersAsync.. "
(.." #
)..# $
)..$ %
;..% &
}// 	
[11 	
HttpGet11	 
(11 
$str11 
)11 
]11 
public22 
async22 
Task22 
<22 
IActionResult22 '
>22' (
GetUser33 
(33 
int33 
id33 
)33 
{44 	
var55 
user55 
=55 
await66 
_adminService66 #
.77 
GetUserByIdAsync77 %
(77% &
id77& (
)77( )
;77) *
if99 
(99 
user99 
==99 
null99 
)99 
return:: 
NotFound:: 
(::  
)::  !
;::! "
return<< 
Ok<< 
(<< 
user<< 
)<< 
;<< 
}== 	
}>> 
}?? 