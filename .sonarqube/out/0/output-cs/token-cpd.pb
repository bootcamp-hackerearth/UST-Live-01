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
} ä
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
< 
AppointmentDto '
>' (
>( )
GetAllAsync* 5
(5 6
)6 7
;7 8
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
}∫∫ Õ©
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
,66. /
Specialisation77 
=77  
(77! " 
DoctorSpecialisation77" 6
)776 7
dto777 :
.77: ;
Specialisation77; I
,77I J
YearsOfExperience88 !
=88" #
dto88$ '
.88' (
YearsOfExperience88( 9
,889 :
ConsultationFee99 
=99  !
dto99" %
.99% &
ConsultationFee99& 5
,995 6
IsActive:: 
=:: 
true:: 
};; 
;;; 
await== 
_doctorRepository== #
.==# $
AddAsync==$ ,
(==, -
doctor==- 3
)==3 4
;==4 5
await>> 
_doctorRepository>> #
.>># $
SaveChangesAsync>>$ 4
(>>4 5
)>>5 6
;>>6 7
return@@ 
MapToDoctorDto@@ !
(@@! "
doctor@@" (
)@@( )
;@@) *
}AA 	
publicCC 
asyncCC 
TaskCC 
UpdateAsyncCC %
(CC% &
intCC& )
idCC* ,
,CC, -
UpdateDoctorDtoCC. =
dtoCC> A
)CCA B
{DD 	
ValidateDoctorEE 
(EE 
dtoEE 
)EE 
;EE  
varGG 
doctorGG 
=GG 
awaitGG 
_doctorRepositoryGG 0
.GG0 1
GetByIdAsyncGG1 =
(GG= >
idGG> @
)GG@ A
;GGA B
ifII 
(II 
doctorII 
==II 
nullII 
)II 
throwJJ 
newJJ  
KeyNotFoundExceptionJJ .
(JJ. /
$"JJ/ 1
$strJJ1 @
{JJ@ A
idJJA C
}JJC D
$strJJD O
"JJO P
)JJP Q
;JJQ R
doctorLL 
.LL 
FullNameLL 
=LL 
dtoLL !
.LL! "
FullNameLL" *
.LL* +
TrimLL+ /
(LL/ 0
)LL0 1
;LL1 2
doctorMM 
.MM 
SpecialisationMM !
=MM" #
(MM$ % 
DoctorSpecialisationMM% 9
)MM9 :
dtoMM: =
.MM= >
SpecialisationMM> L
;MML M
doctorNN 
.NN 
YearsOfExperienceNN $
=NN% &
dtoNN' *
.NN* +
YearsOfExperienceNN+ <
;NN< =
doctorOO 
.OO 
ConsultationFeeOO "
=OO# $
dtoOO% (
.OO( )
ConsultationFeeOO) 8
;OO8 9
awaitQQ 
_doctorRepositoryQQ #
.QQ# $
UpdateAsyncQQ$ /
(QQ/ 0
doctorQQ0 6
)QQ6 7
;QQ7 8
awaitRR 
_doctorRepositoryRR #
.RR# $
SaveChangesAsyncRR$ 4
(RR4 5
)RR5 6
;RR6 7
}SS 	
publicUU 
asyncUU 
TaskUU 
<UU 
IEnumerableUU %
<UU% &
intUU& )
>UU) *
>UU* + 
GetAvailabilityAsyncUU, @
(UU@ A
intUUA D
doctorIdUUE M
,UUM N
DateOnlyUUN V
dateUUW [
)UU[ \
{VV 	
varWW 
doctorWW 
=WW 
awaitXX 
_doctorRepositoryXX '
.XX' (
GetByIdAsyncXX( 4
(XX4 5
doctorIdXX5 =
)XX= >
;XX> ?
ifZZ 
(ZZ 
doctorZZ 
==ZZ 
nullZZ 
)ZZ 
throw[[ 
new[[  
KeyNotFoundException[[ .
([[. /
$str\\ '
)\\' (
;\\( )
var^^ 
bookedSlots^^ 
=^^ 
await__ 
_doctorRepository__ '
.__' (
GetBookedSlotsAsync__( ;
(__; <
doctorId`` 
,`` 
dateaa 
)aa 
;aa 
varcc 
allSlotscc 
=cc 
Enumdd 
.dd 
	GetValuesdd 
<dd 
AppointmentTimeSlotdd 2
>dd2 3
(dd3 4
)dd4 5
.ee 
Selectee 
(ee 
xee 
=>ee  
(ee! "
intee" %
)ee% &
xee& '
)ee' (
;ee( )
returngg 
allSlotsgg 
.gg 
Exceptgg "
(gg" #
bookedSlotsgg# .
)gg. /
;gg/ 0
}hh 	
publicjj 
asyncjj 
Taskjj 
<jj #
DoctorCreationResultDtojj 1
>jj1 2(
CreateDoctorWithAccountAsynckk  
(kk  !
CreateDoctorDtoll 
dtoll 
)ll 
{mm 	
ValidateDoctornn 
(nn 
dtonn 
)nn 
;nn  
ifpp 
(pp 
awaitpp 
_userServicepp "
.pp" #
EmailExistsAsyncpp# 3
(pp3 4
dtopp4 7
.pp7 8
Emailpp8 =
)pp= >
)pp> ?
{qq 
throwrr 
newrr 
ArgumentExceptionrr +
(rr+ ,
$strss +
)ss+ ,
;ss, -
}tt 
varvv 
doctorvv 
=vv 
newvv 
Doctorvv #
{ww 
FullNamexx 
=xx 
dtoxx 
.xx 
FullNamexx '
.xx' (
Trimxx( ,
(xx, -
)xx- .
,xx. /
Emailyy 
=yy 
dtoyy 
.yy 
Emailyy !
.yy! "
Trimyy" &
(yy& '
)yy' (
.yy( )
ToLoweryy) 0
(yy0 1
)yy1 2
,yy2 3
Specialisationzz 
=zz  
({{  
DoctorSpecialisation{{ )
){{) *
dto{{* -
.{{- .
Specialisation{{. <
,{{< =
YearsOfExperience|| !
=||" #
dto}} 
.}} 
YearsOfExperience}} )
,}}) *
ConsultationFee~~ 
=~~  !
dto 
. 
ConsultationFee '
,' (
IsActive
ÄÄ 
=
ÄÄ 
true
ÄÄ 
}
ÅÅ 
;
ÅÅ 
await
ÉÉ 
_doctorRepository
ÉÉ #
.
ÉÉ# $
AddAsync
ÉÉ$ ,
(
ÉÉ, -
doctor
ÉÉ- 3
)
ÉÉ3 4
;
ÉÉ4 5
await
ÑÑ 
_doctorRepository
ÑÑ #
.
ÑÑ# $
SaveChangesAsync
ÑÑ$ 4
(
ÑÑ4 5
)
ÑÑ5 6
;
ÑÑ6 7
var
ÜÜ 
temporaryPassword
ÜÜ !
=
ÜÜ" #'
GenerateTemporaryPassword
áá )
(
áá) *
)
áá* +
;
áá+ ,
var
ââ 
user
ââ 
=
ââ 
new
ââ 
User
ââ 
{
ää 
Email
ãã 
=
ãã 
doctor
ãã 
.
ãã 
Email
ãã $
,
ãã$ %
PasswordHash
åå 
=
åå 
HashPassword
çç  
(
çç  !
temporaryPassword
çç! 2
)
çç2 3
,
çç3 4
Role
éé 
=
éé 
UserRole
éé 
.
éé  
Doctor
éé  &
,
éé& '
ReferenceId
èè 
=
èè 
doctor
èè $
.
èè$ %
DoctorId
èè% -
,
èè- .
CreatedDate
êê 
=
êê 
DateTime
êê &
.
êê& '
UtcNow
êê' -
}
ëë 
;
ëë 
await
ìì 
_userService
ìì 
.
ìì 
CreateAsync
ìì *
(
ìì* +
user
ìì+ /
)
ìì/ 0
;
ìì0 1
await
îî 
_userService
îî 
.
îî 
SaveChangesAsync
îî /
(
îî/ 0
)
îî0 1
;
îî1 2
return
ññ 
new
ññ %
DoctorCreationResultDto
ññ .
{
óó 
DoctorId
òò 
=
òò 
doctor
òò !
.
òò! "
DoctorId
òò" *
,
òò* +
FullName
ôô 
=
ôô 
doctor
ôô !
.
ôô! "
FullName
ôô" *
,
ôô* +
Email
öö 
=
öö 
doctor
öö 
.
öö 
Email
öö $
,
öö$ %
TemporaryPassword
õõ !
=
õõ" #
temporaryPassword
õõ$ 5
}
úú 
;
úú 
}
ùù 	
public
üü 
async
üü 
Task
üü 
ActivateAsync
üü '
(
üü' (
int
üü( +
id
üü, .
)
üü. /
{
†† 	
var
°° 
doctor
°° 
=
°° 
await
°° 
_doctorRepository
°° 0
.
°°0 1
GetByIdAsync
°°1 =
(
°°= >
id
°°> @
)
°°@ A
;
°°A B
if
££ 
(
££ 
doctor
££ 
==
££ 
null
££ 
)
££ 
throw
§§ 
new
§§ "
KeyNotFoundException
§§ .
(
§§. /
$"
§§/ 1
$str
§§1 @
{
§§@ A
id
§§A C
}
§§C D
$str
§§D O
"
§§O P
)
§§P Q
;
§§Q R
doctor
¶¶ 
.
¶¶ 
IsActive
¶¶ 
=
¶¶ 
true
¶¶ "
;
¶¶" #
await
®® 
_doctorRepository
®® #
.
®®# $
UpdateAsync
®®$ /
(
®®/ 0
doctor
®®0 6
)
®®6 7
;
®®7 8
await
©© 
_doctorRepository
©© #
.
©©# $
SaveChangesAsync
©©$ 4
(
©©4 5
)
©©5 6
;
©©6 7
}
™™ 	
public
¨¨ 
async
¨¨ 
Task
¨¨ 
DeactivateAsync
¨¨ )
(
¨¨) *
int
¨¨* -
id
¨¨. 0
)
¨¨0 1
{
≠≠ 	
var
ÆÆ 
doctor
ÆÆ 
=
ÆÆ 
await
ÆÆ 
_doctorRepository
ÆÆ 0
.
ÆÆ0 1
GetByIdAsync
ÆÆ1 =
(
ÆÆ= >
id
ÆÆ> @
)
ÆÆ@ A
;
ÆÆA B
if
∞∞ 
(
∞∞ 
doctor
∞∞ 
==
∞∞ 
null
∞∞ 
)
∞∞ 
throw
±± 
new
±± "
KeyNotFoundException
±± .
(
±±. /
$"
±±/ 1
$str
±±1 @
{
±±@ A
id
±±A C
}
±±C D
$str
±±D O
"
±±O P
)
±±P Q
;
±±Q R
doctor
≥≥ 
.
≥≥ 
IsActive
≥≥ 
=
≥≥ 
false
≥≥ #
;
≥≥# $
await
µµ 
_doctorRepository
µµ #
.
µµ# $
UpdateAsync
µµ$ /
(
µµ/ 0
doctor
µµ0 6
)
µµ6 7
;
µµ7 8
await
∂∂ 
_doctorRepository
∂∂ #
.
∂∂# $
SaveChangesAsync
∂∂$ 4
(
∂∂4 5
)
∂∂5 6
;
∂∂6 7
}
∑∑ 	
private
ππ 
static
ππ 
void
ππ 
ValidateDoctor
ππ *
(
ππ* +
CreateDoctorDto
ππ+ :
dto
ππ; >
)
ππ> ?
{
∫∫ 	
if
ªª 
(
ªª 
string
ªª 
.
ªª  
IsNullOrWhiteSpace
ªª )
(
ªª) *
dto
ªª* -
.
ªª- .
FullName
ªª. 6
)
ªª6 7
)
ªª7 8
throw
ºº 
new
ºº 
ArgumentException
ºº +
(
ºº+ ,
$str
ºº, F
)
ººF G
;
ººG H
if
ææ 
(
ææ 
!
ææ 
Enum
ææ 
.
ææ 
	IsDefined
ææ 
(
ææ  
typeof
ææ  &
(
ææ& '"
DoctorSpecialisation
ææ' ;
)
ææ; <
,
ææ< =
dto
ææ> A
.
ææA B
Specialisation
ææB P
)
ææP Q
)
ææQ R
throw
øø 
new
øø 
ArgumentException
øø +
(
øø+ ,
$str
øø, L
)
øøL M
;
øøM N
if
¡¡ 
(
¡¡ 
dto
¡¡ 
.
¡¡ 
YearsOfExperience
¡¡ %
<
¡¡& '
$num
¡¡( )
||
¡¡* ,
dto
¡¡- 0
.
¡¡0 1
YearsOfExperience
¡¡1 B
>
¡¡C D
$num
¡¡E G
)
¡¡G H
throw
¬¬ 
new
¬¬ 
ArgumentException
¬¬ +
(
¬¬+ ,
$str
¬¬, X
)
¬¬X Y
;
¬¬Y Z
if
ƒƒ 
(
ƒƒ 
dto
ƒƒ 
.
ƒƒ 
ConsultationFee
ƒƒ #
<=
ƒƒ$ &
$num
ƒƒ' (
)
ƒƒ( )
throw
≈≈ 
new
≈≈ 
ArgumentException
≈≈ +
(
≈≈+ ,
$str
≈≈, Y
)
≈≈Y Z
;
≈≈Z [
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
∆∆- .
Email
∆∆. 3
)
∆∆3 4
)
∆∆4 5
{
«« 
throw
»» 
new
»» 
ArgumentException
»» +
(
»»+ ,
$str
»», @
)
»»@ A
;
»»A B
}
…… 
}
   	
private
ÃÃ 
static
ÃÃ 
void
ÃÃ 
ValidateDoctor
ÃÃ *
(
ÃÃ* +
UpdateDoctorDto
ÃÃ+ :
dto
ÃÃ; >
)
ÃÃ> ?
{
ÕÕ 	
if
ŒŒ 
(
ŒŒ 
string
ŒŒ 
.
ŒŒ  
IsNullOrWhiteSpace
ŒŒ )
(
ŒŒ) *
dto
ŒŒ* -
.
ŒŒ- .
FullName
ŒŒ. 6
)
ŒŒ6 7
)
ŒŒ7 8
throw
œœ 
new
œœ 
ArgumentException
œœ +
(
œœ+ ,
$str
œœ, F
)
œœF G
;
œœG H
if
—— 
(
—— 
!
—— 
Enum
—— 
.
—— 
	IsDefined
—— 
(
——  
typeof
——  &
(
——& '"
DoctorSpecialisation
——' ;
)
——; <
,
——< =
dto
——> A
.
——A B
Specialisation
——B P
)
——P Q
)
——Q R
throw
““ 
new
““ 
ArgumentException
““ +
(
““+ ,
$str
““, L
)
““L M
;
““M N
if
‘‘ 
(
‘‘ 
dto
‘‘ 
.
‘‘ 
YearsOfExperience
‘‘ %
<
‘‘& '
$num
‘‘( )
||
‘‘* ,
dto
‘‘- 0
.
‘‘0 1
YearsOfExperience
‘‘1 B
>
‘‘C D
$num
‘‘E G
)
‘‘G H
throw
’’ 
new
’’ 
ArgumentException
’’ +
(
’’+ ,
$str
’’, X
)
’’X Y
;
’’Y Z
if
◊◊ 
(
◊◊ 
dto
◊◊ 
.
◊◊ 
ConsultationFee
◊◊ #
<=
◊◊$ &
$num
◊◊' (
)
◊◊( )
throw
ÿÿ 
new
ÿÿ 
ArgumentException
ÿÿ +
(
ÿÿ+ ,
$str
ÿÿ, Y
)
ÿÿY Z
;
ÿÿZ [
}
ŸŸ 	
private
€€ 
static
€€ 
string
€€ '
GenerateTemporaryPassword
€€ 7
(
€€7 8
)
€€8 9
{
‹‹ 	
return
›› 
$"
›› 
$str
›› 
{
›› 
Random
››  
.
››  !
Shared
››! '
.
››' (
Next
››( ,
(
››, -
$num
››- 3
,
››3 4
$num
››5 ;
)
››; <
}
››< =
"
››= >
;
››> ?
}
ﬁﬁ 	
private
‡‡ 
static
‡‡ 
string
‡‡ 
HashPassword
‡‡ *
(
‡‡* +
string
·· 

password
·· 
)
·· 
{
‚‚ 	
using
„„ 
var
„„ 
sha256
„„ 
=
„„ 
System
‰‰ 
.
‰‰ 
Security
‰‰ 
.
‰‰  
Cryptography
‰‰  ,
.
‰‰, -
SHA256
‰‰- 3
.
‰‰3 4
Create
‰‰4 :
(
‰‰: ;
)
‰‰; <
;
‰‰< =
var
ÊÊ 
bytes
ÊÊ 
=
ÊÊ 
System
ÁÁ 
.
ÁÁ 
Text
ÁÁ 
.
ÁÁ 
Encoding
ÁÁ $
.
ÁÁ$ %
UTF8
ÁÁ% )
.
ÁÁ) *
GetBytes
ÁÁ* 2
(
ÁÁ2 3
password
ÁÁ3 ;
)
ÁÁ; <
;
ÁÁ< =
var
ÈÈ 
hash
ÈÈ 
=
ÈÈ 
sha256
ÍÍ 
.
ÍÍ 
ComputeHash
ÍÍ "
(
ÍÍ" #
bytes
ÍÍ# (
)
ÍÍ( )
;
ÍÍ) *
return
ÏÏ 
Convert
ÏÏ 
.
ÏÏ 
ToBase64String
ÏÏ )
(
ÏÏ) *
hash
ÏÏ* .
)
ÏÏ. /
;
ÏÏ/ 0
}
ÌÌ 	
private
ÔÔ 
static
ÔÔ 
	DoctorDto
ÔÔ  
MapToDoctorDto
ÔÔ! /
(
ÔÔ/ 0
Doctor
ÔÔ0 6
doctor
ÔÔ7 =
)
ÔÔ= >
{
 	
return
ÒÒ 
new
ÒÒ 
	DoctorDto
ÒÒ  
{
ÚÚ 
DoctorId
ÛÛ 
=
ÛÛ 
doctor
ÛÛ !
.
ÛÛ! "
DoctorId
ÛÛ" *
,
ÛÛ* +
FullName
ÙÙ 
=
ÙÙ 
doctor
ÙÙ !
.
ÙÙ! "
FullName
ÙÙ" *
,
ÙÙ* +
Specialisation
ıı 
=
ıı  
(
ıı! "
int
ıı" %
)
ıı% &
doctor
ıı& ,
.
ıı, -
Specialisation
ıı- ;
,
ıı; <
YearsOfExperience
ˆˆ !
=
ˆˆ" #
doctor
ˆˆ$ *
.
ˆˆ* +
YearsOfExperience
ˆˆ+ <
,
ˆˆ< =
ConsultationFee
˜˜ 
=
˜˜  !
doctor
˜˜" (
.
˜˜( )
ConsultationFee
˜˜) 8
,
˜˜8 9
IsActive
¯¯ 
=
¯¯ 
doctor
¯¯ !
.
¯¯! "
IsActive
¯¯" *
}
˘˘ 
;
˘˘ 
}
˙˙ 	
}
˚˚ 
}¸¸ ≠
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
}îî Ïõ
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\AppointmentService.cs
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
AppointmentService		 #
:		$ %
IAppointmentService		& 9
{

 
private 
readonly "
IAppointmentRepository /"
_appointmentRepository0 F
;F G
private 
readonly 
IPatientRepository +
_patientRepository, >
;> ?
private 
readonly 
IDoctorRepository *
_doctorRepository+ <
;< =
public 
AppointmentService !
(! ""
IAppointmentRepository "!
appointmentRepository# 8
,8 9
IPatientRepository 
patientRepository 0
,0 1
IDoctorRepository 
doctorRepository .
). /
{ 	"
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
_patientRepository 
=  
patientRepository! 2
;2 3
_doctorRepository 
= 
doctorRepository  0
;0 1
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
AppointmentDto& 4
>4 5
>5 6
GetAllAsync7 B
(B C
)C D
{ 	
var 
appointments 
= 
await $"
_appointmentRepository% ;
.; <
GetAllAsync< G
(G H
)H I
;I J
return 
appointments 
.  
Select  &
(& '
MapToAppointmentDto' :
): ;
;; <
} 	
public   
async   
Task   
<   !
AppointmentDetailsDto   /
?  / 0
>  0 1
GetByIdAsync  2 >
(  > ?
int  ? B
id  C E
)  E F
{!! 	
var"" 
appointment"" 
="" 
await"" #"
_appointmentRepository""$ :
."": ;
GetByIdAsync""; G
(""G H
id""H J
)""J K
;""K L
if$$ 
($$ 
appointment$$ 
==$$ 
null$$ #
)$$# $
return%% 
null%% 
;%% 
return'' 
new'' !
AppointmentDetailsDto'' ,
{(( 
AppointmentId)) 
=)) 
appointment))  +
.))+ ,
AppointmentId)), 9
,))9 :
	PatientId** 
=** 
appointment** '
.**' (
	PatientId**( 1
,**1 2
PatientName++ 
=++ 
appointment++ )
.++) *
Patient++* 1
.++1 2
FullName++2 :
,++: ;
DoctorId,, 
=,, 
appointment,, &
.,,& '
DoctorId,,' /
,,,/ 0

DoctorName-- 
=-- 
appointment-- (
.--( )
Doctor--) /
.--/ 0
FullName--0 8
,--8 9
ScheduledDate.. 
=.. 
appointment..  +
...+ ,
ScheduledDate.., 9
,..9 :
TimeSlot// 
=// 
(// 
int// 
)//  
appointment//  +
.//+ ,
TimeSlot//, 4
,//4 5
Status00 
=00 
(00 
int00 
)00 
appointment00 )
.00) *
Status00* 0
,000 1
CancellationReason11 "
=11# $
appointment11% 0
.110 1
CancellationReason111 C
}22 
;22 
}33 	
public55 
async55 
Task55 
<55 
IEnumerable55 %
<55% &(
PatientAppointmentHistoryDto55& B
>55B C
>55C D"
GetPatientHistoryAsync66 "
(66" #
int66# &
	patientId66' 0
)660 1
{77 	
var88 
appointments88 
=88 
await99 "
_appointmentRepository99 ,
.99, -
GetByPatientIdAsync99- @
(99@ A
	patientId99A J
)99J K
;99K L
return;; 
appointments;; 
.;;  
Select;;  &
(;;& '
a;;' (
=>;;) +
new<< (
PatientAppointmentHistoryDto<< 0
{== 
AppointmentId>> !
=>>" #
a>>$ %
.>>% &
AppointmentId>>& 3
,>>3 4
ScheduledDate?? !
=??" #
a??$ %
.??% &
ScheduledDate??& 3
,??3 4
TimeSlot@@ 
=@@ 
(@@  
int@@  #
)@@# $
a@@$ %
.@@% &
TimeSlot@@& .
,@@. /
DoctorIdAA 
=AA 
aAA  
.AA  !
DoctorIdAA! )
,AA) *

DoctorNameBB 
=BB  
aBB! "
.BB" #
DoctorBB# )
.BB) *
FullNameBB* 2
,BB2 3
StatusCC 
=CC 
(CC 
intCC !
)CC! "
aCC" #
.CC# $
StatusCC$ *
}DD 
)DD 
;DD 
}EE 	
publicGG 
asyncGG 
TaskGG 
<GG 
IEnumerableGG %
<GG% &!
DoctorScheduleItemDtoGG& ;
>GG; <
>GG< ='
GetDoctorTodayScheduleAsyncHH '
(HH' (
intHH( +
doctorIdHH, 4
)HH4 5
{II 	
varJJ 
appointmentsJJ 
=JJ 
awaitKK "
_appointmentRepositoryKK ,
.KK, -'
GetDoctorTodayScheduleAsyncKK- H
(KKH I
doctorIdLL 
,LL 
DateOnlyMM 
.MM 
FromDateTimeMM )
(MM) *
DateTimeMM* 2
.MM2 3
TodayMM3 8
)MM8 9
)MM9 :
;MM: ;
returnOO 
appointmentsOO 
.OO  
SelectOO  &
(OO& '!
MapDoctorScheduleItemOO' <
)OO< =
;OO= >
}PP 	
publicRR 
asyncRR 
TaskRR 
<RR 
IEnumerableRR %
<RR% &!
DoctorScheduleItemDtoRR& ;
>RR; <
>RR< =&
GetDoctorWeekScheduleAsyncSS &
(SS& '
intTT 
doctorIdTT 
,TT 
DateOnlyUU 
	startDateUU "
,UU" #
DateOnlyVV 
endDateVV  
)VV  !
{WW 	
varXX 
appointmentsXX 
=XX 
awaitYY "
_appointmentRepositoryYY ,
.YY, -&
GetDoctorWeekScheduleAsyncYY- G
(YYG H
doctorIdZZ 
,ZZ 
	startDate[[ 
,[[ 
endDate\\ 
)\\ 
;\\ 
return^^ 
appointments^^ 
.^^  
Select^^  &
(^^& '!
MapDoctorScheduleItem^^' <
)^^< =
;^^= >
}__ 	
publicaa 
asyncaa 
Taskaa 
<aa 
AppointmentDtoaa (
>aa( )
CreateAsyncaa* 5
(aa5 6 
CreateAppointmentDtoaa6 J
dtoaaK N
)aaN O
{bb 	
awaitcc  
ValidateBookingAsynccc &
(cc& '
dtodd 
.dd 
	PatientIddd 
,dd 
dtoee 
.ee 
DoctorIdee 
,ee 
dtoff 
.ff 
ScheduledDateff !
,ff! "
dtogg 
.gg 
TimeSlotgg 
)gg 
;gg 
varii 
appointmentii 
=ii 
newii !
Appointmentii" -
{jj 
	PatientIdkk 
=kk 
dtokk 
.kk  
	PatientIdkk  )
,kk) *
DoctorIdll 
=ll 
dtoll 
.ll 
DoctorIdll '
,ll' (
ScheduledDatemm 
=mm 
dtomm  #
.mm# $
ScheduledDatemm$ 1
,mm1 2
TimeSlotnn 
=nn 
(nn 
AppointmentTimeSlotnn /
)nn/ 0
dtonn0 3
.nn3 4
TimeSlotnn4 <
,nn< =
Statusoo 
=oo 
AppointmentStatusoo *
.oo* +
Pendingoo+ 2
}pp 
;pp 
awaitrr "
_appointmentRepositoryrr (
.rr( )
AddAsyncrr) 1
(rr1 2
appointmentrr2 =
)rr= >
;rr> ?
awaitss "
_appointmentRepositoryss (
.ss( )
SaveChangesAsyncss) 9
(ss9 :
)ss: ;
;ss; <
returnuu 
MapToAppointmentDtouu &
(uu& '
appointmentuu' 2
)uu2 3
;uu3 4
}vv 	
publicxx 
asyncxx 
Taskxx 
UpdateAsyncxx %
(xx% &
intyy 
idyy 
,yy  
UpdateAppointmentDtozz  
dtozz! $
)zz$ %
{{{ 	
var|| 
appointment|| 
=|| 
await}} "
_appointmentRepository}} ,
.}}, -
GetByIdAsync}}- 9
(}}9 :
id}}: <
)}}< =
;}}= >
if 
( 
appointment 
== 
null #
)# $
throw
ÄÄ 
new
ÄÄ "
KeyNotFoundException
ÄÄ .
(
ÄÄ. /
$"
ÅÅ 
$str
ÅÅ "
{
ÅÅ" #
id
ÅÅ# %
}
ÅÅ% &
$str
ÅÅ& 1
"
ÅÅ1 2
)
ÅÅ2 3
;
ÅÅ3 4
if
ÉÉ 
(
ÉÉ 
appointment
ÉÉ 
.
ÉÉ 
Status
ÉÉ "
==
ÉÉ# %
AppointmentStatus
ÉÉ& 7
.
ÉÉ7 8
	Completed
ÉÉ8 A
)
ÉÉA B
throw
ÑÑ 
new
ÑÑ '
InvalidOperationException
ÑÑ 3
(
ÑÑ3 4
$str
ÖÖ @
)
ÖÖ@ A
;
ÖÖA B
if
áá 
(
áá 
appointment
áá 
.
áá 
Status
áá "
==
áá# %
AppointmentStatus
áá& 7
.
áá7 8
	Cancelled
áá8 A
)
ááA B
throw
àà 
new
àà '
InvalidOperationException
àà 3
(
àà3 4
$str
ââ @
)
ââ@ A
;
ââA B
await
ãã (
ValidateUpdateBookingAsync
ãã ,
(
ãã, -
appointment
åå 
.
åå 
AppointmentId
åå )
,
åå) *
appointment
çç 
.
çç 
	PatientId
çç %
,
çç% &
dto
éé 
.
éé 
DoctorId
éé 
,
éé 
dto
èè 
.
èè 
ScheduledDate
èè !
,
èè! "
dto
êê 
.
êê 
TimeSlot
êê 
)
êê 
;
êê 
appointment
íí 
.
íí 
DoctorId
íí  
=
íí! "
dto
íí# &
.
íí& '
DoctorId
íí' /
;
íí/ 0
appointment
ìì 
.
ìì 
ScheduledDate
ìì %
=
ìì& '
dto
ìì( +
.
ìì+ ,
ScheduledDate
ìì, 9
;
ìì9 :
appointment
îî 
.
îî 
TimeSlot
îî  
=
îî! "
(
ïï !
AppointmentTimeSlot
ïï $
)
ïï$ %
dto
ïï% (
.
ïï( )
TimeSlot
ïï) 1
;
ïï1 2
await
óó $
_appointmentRepository
óó (
.
óó( )
UpdateAsync
óó) 4
(
óó4 5
appointment
óó5 @
)
óó@ A
;
óóA B
await
òò $
_appointmentRepository
òò (
.
òò( )
SaveChangesAsync
òò) 9
(
òò9 :
)
òò: ;
;
òò; <
}
ôô 	
public
õõ 
async
õõ 
Task
õõ 
UpdateStatusAsync
õõ +
(
õõ+ ,
int
úú 
id
úú 

,
úú
 (
UpdateAppointmentStatusDto
ùù 
dto
ùù "
)
ùù" #
{
ûû 	
var
üü 
appointment
üü 
=
üü 
await
†† $
_appointmentRepository
†† ,
.
††, -
GetByIdAsync
††- 9
(
††9 :
id
††: <
)
††< =
;
††= >
if
¢¢ 
(
¢¢ 
appointment
¢¢ 
==
¢¢ 
null
¢¢ #
)
¢¢# $
throw
££ 
new
££ "
KeyNotFoundException
££ .
(
££. /
$"
§§ 
$str
§§ "
{
§§" #
id
§§# %
}
§§% &
$str
§§& 1
"
§§1 2
)
§§2 3
;
§§3 4
if
¶¶ 
(
¶¶ 
!
¶¶ 
Enum
¶¶ 
.
¶¶ 
	IsDefined
¶¶ 
(
¶¶  
typeof
ßß 
(
ßß 
AppointmentStatus
ßß ,
)
ßß, -
,
ßß- .
dto
®® 
.
®® 
Status
®® 
)
®® 
)
®®  
{
©© 
throw
™™ 
new
™™ 
ArgumentException
™™ +
(
™™+ ,
$str
´´ 1
)
´´1 2
;
´´2 3
}
¨¨ 
var
ÆÆ 
	newStatus
ÆÆ 
=
ÆÆ 
(
ØØ 
AppointmentStatus
ØØ "
)
ØØ" #
dto
ØØ# &
.
ØØ& '
Status
ØØ' -
;
ØØ- .
if
±± 
(
±± 
appointment
±± 
.
±± 
Status
±± "
==
±±# %
AppointmentStatus
±±& 7
.
±±7 8
	Completed
±±8 A
)
±±A B
{
≤≤ 
throw
≥≥ 
new
≥≥ '
InvalidOperationException
≥≥ 3
(
≥≥3 4
$str
¥¥ @
)
¥¥@ A
;
¥¥A B
}
µµ 
if
∑∑ 
(
∑∑ 
appointment
∑∑ 
.
∑∑ 
Status
∑∑ "
==
∑∑# %
AppointmentStatus
∑∑& 7
.
∑∑7 8
	Cancelled
∑∑8 A
)
∑∑A B
{
∏∏ 
throw
ππ 
new
ππ '
InvalidOperationException
ππ 3
(
ππ3 4
$str
∫∫ @
)
∫∫@ A
;
∫∫A B
}
ªª 
switch
ΩΩ 
(
ΩΩ 
	newStatus
ΩΩ 
)
ΩΩ 
{
ææ 
case
øø 
AppointmentStatus
øø &
.
øø& '
Pending
øø' .
:
øø. /
throw
¿¿ 
new
¿¿ '
InvalidOperationException
¿¿ 7
(
¿¿7 8
$str
¡¡ M
)
¡¡M N
;
¡¡N O
case
√√ 
AppointmentStatus
√√ &
.
√√& '
	Confirmed
√√' 0
:
√√0 1
if
≈≈ 
(
≈≈ 
appointment
≈≈ #
.
≈≈# $
Status
≈≈$ *
!=
≈≈+ -
AppointmentStatus
≈≈. ?
.
≈≈? @
Pending
≈≈@ G
)
≈≈G H
{
∆∆ 
throw
«« 
new
«« !'
InvalidOperationException
««" ;
(
««; <
$str
»» I
)
»»I J
;
»»J K
}
…… 
appointment
ÀÀ 
.
ÀÀ  
Status
ÀÀ  &
=
ÀÀ' (
AppointmentStatus
ÃÃ )
.
ÃÃ) *
	Confirmed
ÃÃ* 3
;
ÃÃ3 4
break
ÕÕ 
;
ÕÕ 
case
œœ 
AppointmentStatus
œœ &
.
œœ& '
	Completed
œœ' 0
:
œœ0 1
if
—— 
(
—— 
appointment
—— #
.
——# $
Status
——$ *
!=
——+ -
AppointmentStatus
——. ?
.
——? @
	Confirmed
——@ I
)
——I J
{
““ 
throw
”” 
new
”” !'
InvalidOperationException
””" ;
(
””; <
$str
‘‘ K
)
‘‘K L
;
‘‘L M
}
’’ 
appointment
◊◊ 
.
◊◊  
Status
◊◊  &
=
◊◊' (
AppointmentStatus
ÿÿ )
.
ÿÿ) *
	Completed
ÿÿ* 3
;
ÿÿ3 4
break
ŸŸ 
;
ŸŸ 
case
€€ 
AppointmentStatus
€€ &
.
€€& '
	Cancelled
€€' 0
:
€€0 1
if
›› 
(
›› 
string
›› 
.
››  
IsNullOrWhiteSpace
›› 1
(
››1 2
dto
ﬁﬁ 
.
ﬁﬁ   
CancellationReason
ﬁﬁ  2
)
ﬁﬁ2 3
)
ﬁﬁ3 4
{
ﬂﬂ 
throw
‡‡ 
new
‡‡ !
ArgumentException
‡‡" 3
(
‡‡3 4
$str
·· >
)
··> ?
;
··? @
}
‚‚ 
appointment
‰‰ 
.
‰‰  
Status
‰‰  &
=
‰‰' (
AppointmentStatus
ÂÂ )
.
ÂÂ) *
	Cancelled
ÂÂ* 3
;
ÂÂ3 4
appointment
ÁÁ 
.
ÁÁ   
CancellationReason
ÁÁ  2
=
ÁÁ3 4
dto
ËË 
.
ËË  
CancellationReason
ËË .
.
ËË. /
Trim
ËË/ 3
(
ËË3 4
)
ËË4 5
;
ËË5 6
break
ÍÍ 
;
ÍÍ 
default
ÏÏ 
:
ÏÏ 
throw
ÌÌ 
new
ÌÌ 
ArgumentException
ÌÌ /
(
ÌÌ/ 0
$str
ÓÓ 5
)
ÓÓ5 6
;
ÓÓ6 7
}
ÔÔ 
await
ÒÒ $
_appointmentRepository
ÒÒ (
.
ÒÒ( )
UpdateAsync
ÒÒ) 4
(
ÒÒ4 5
appointment
ÚÚ 
)
ÚÚ 
;
ÚÚ 
await
ÙÙ $
_appointmentRepository
ÙÙ (
.
ÙÙ( )
SaveChangesAsync
ÙÙ) 9
(
ÙÙ9 :
)
ÙÙ: ;
;
ÙÙ; <
}
ıı 	
public
˜˜ 
async
˜˜ 
Task
˜˜ 
ConfirmAsync
˜˜ &
(
˜˜& '
int
˜˜' *
id
˜˜+ -
)
˜˜- .
{
¯¯ 	
var
˘˘ 
appointment
˘˘ 
=
˘˘ 
await
˙˙ $
_appointmentRepository
˙˙ ,
.
˙˙, -
GetByIdAsync
˙˙- 9
(
˙˙9 :
id
˙˙: <
)
˙˙< =
;
˙˙= >
if
¸¸ 
(
¸¸ 
appointment
¸¸ 
==
¸¸ 
null
¸¸ #
)
¸¸# $
throw
˝˝ 
new
˝˝ "
KeyNotFoundException
˝˝ .
(
˝˝. /
)
˝˝/ 0
;
˝˝0 1
if
ˇˇ 
(
ˇˇ 
appointment
ˇˇ 
.
ˇˇ 
Status
ˇˇ "
!=
ˇˇ# %
AppointmentStatus
ˇˇ& 7
.
ˇˇ7 8
Pending
ˇˇ8 ?
)
ˇˇ? @
throw
ÄÄ 
new
ÄÄ '
InvalidOperationException
ÄÄ 3
(
ÄÄ3 4
$str
ÅÅ A
)
ÅÅA B
;
ÅÅB C
appointment
ÉÉ 
.
ÉÉ 
Status
ÉÉ 
=
ÉÉ  
AppointmentStatus
ÉÉ! 2
.
ÉÉ2 3
	Confirmed
ÉÉ3 <
;
ÉÉ< =
await
ÖÖ $
_appointmentRepository
ÖÖ (
.
ÖÖ( )
UpdateAsync
ÖÖ) 4
(
ÖÖ4 5
appointment
ÖÖ5 @
)
ÖÖ@ A
;
ÖÖA B
await
ÜÜ $
_appointmentRepository
ÜÜ (
.
ÜÜ( )
SaveChangesAsync
ÜÜ) 9
(
ÜÜ9 :
)
ÜÜ: ;
;
ÜÜ; <
}
áá 	
public
ââ 
async
ââ 
Task
ââ 
CompleteAsync
ââ '
(
ââ' (
int
ââ( +
id
ââ, .
)
ââ. /
{
ää 	
var
ãã 
appointment
ãã 
=
ãã 
await
åå $
_appointmentRepository
åå ,
.
åå, -
GetByIdAsync
åå- 9
(
åå9 :
id
åå: <
)
åå< =
;
åå= >
if
éé 
(
éé 
appointment
éé 
==
éé 
null
éé #
)
éé# $
throw
èè 
new
èè "
KeyNotFoundException
èè .
(
èè. /
)
èè/ 0
;
èè0 1
if
ëë 
(
ëë 
appointment
ëë 
.
ëë 
Status
ëë "
!=
ëë# %
AppointmentStatus
ëë& 7
.
ëë7 8
	Confirmed
ëë8 A
)
ëëA B
throw
íí 
new
íí '
InvalidOperationException
íí 3
(
íí3 4
$str
ìì C
)
ììC D
;
ììD E
appointment
ïï 
.
ïï 
Status
ïï 
=
ïï  
AppointmentStatus
ïï! 2
.
ïï2 3
	Completed
ïï3 <
;
ïï< =
await
óó $
_appointmentRepository
óó (
.
óó( )
UpdateAsync
óó) 4
(
óó4 5
appointment
óó5 @
)
óó@ A
;
óóA B
await
òò $
_appointmentRepository
òò (
.
òò( )
SaveChangesAsync
òò) 9
(
òò9 :
)
òò: ;
;
òò; <
}
ôô 	
public
õõ 
async
õõ 
Task
õõ 
CancelAsync
õõ %
(
õõ% &
int
úú 
id
úú 
,
úú "
CancelAppointmentDto
ùù  
dto
ùù! $
)
ùù$ %
{
ûû 	
var
üü 
appointment
üü 
=
üü 
await
†† $
_appointmentRepository
†† ,
.
††, -
GetByIdAsync
††- 9
(
††9 :
id
††: <
)
††< =
;
††= >
if
¢¢ 
(
¢¢ 
appointment
¢¢ 
==
¢¢ 
null
¢¢ #
)
¢¢# $
throw
££ 
new
££ "
KeyNotFoundException
££ .
(
££. /
)
££/ 0
;
££0 1
if
•• 
(
•• 
appointment
•• 
.
•• 
Status
•• "
==
••# %
AppointmentStatus
••& 7
.
••7 8
	Completed
••8 A
)
••A B
throw
¶¶ 
new
¶¶ '
InvalidOperationException
¶¶ 3
(
¶¶3 4
$str
ßß A
)
ßßA B
;
ßßB C
if
©© 
(
©© 
appointment
©© 
.
©© 
Status
©© "
==
©©# %
AppointmentStatus
©©& 7
.
©©7 8
	Cancelled
©©8 A
)
©©A B
throw
™™ 
new
™™ '
InvalidOperationException
™™ 3
(
™™3 4
$str
´´ 4
)
´´4 5
;
´´5 6
if
≠≠ 
(
≠≠ 
string
≠≠ 
.
≠≠  
IsNullOrWhiteSpace
≠≠ )
(
≠≠) *
dto
≠≠* -
.
≠≠- . 
CancellationReason
≠≠. @
)
≠≠@ A
)
≠≠A B
throw
ÆÆ 
new
ÆÆ 
ArgumentException
ÆÆ +
(
ÆÆ+ ,
$str
ØØ 6
)
ØØ6 7
;
ØØ7 8
appointment
±± 
.
±± 
Status
±± 
=
±±  
AppointmentStatus
±±! 2
.
±±2 3
	Cancelled
±±3 <
;
±±< =
appointment
≤≤ 
.
≤≤  
CancellationReason
≤≤ *
=
≤≤+ ,
dto
≥≥ 
.
≥≥  
CancellationReason
≥≥ &
.
≥≥& '
Trim
≥≥' +
(
≥≥+ ,
)
≥≥, -
;
≥≥- .
await
µµ $
_appointmentRepository
µµ (
.
µµ( )
UpdateAsync
µµ) 4
(
µµ4 5
appointment
µµ5 @
)
µµ@ A
;
µµA B
await
∂∂ $
_appointmentRepository
∂∂ (
.
∂∂( )
SaveChangesAsync
∂∂) 9
(
∂∂9 :
)
∂∂: ;
;
∂∂; <
}
∑∑ 	
public
ππ 
async
ππ 
Task
ππ 
<
ππ 
IEnumerable
ππ %
<
ππ% &#
DoctorScheduleItemDto
ππ& ;
>
ππ; <
>
ππ< =,
GetDoctorUpcomingScheduleAsync
ππ> \
(
ππ\ ]
int
ππ] `
doctorId
ππa i
)
ππi j
{
∫∫ 	
var
ªª 
	startDate
ªª 
=
ªª 
DateOnly
ºº 
.
ºº 
FromDateTime
ºº %
(
ºº% &
DateTime
ºº& .
.
ºº. /
Today
ºº/ 4
)
ºº4 5
;
ºº5 6
var
ææ 
endDate
ææ 
=
ææ 
	startDate
øø 
.
øø 
AddDays
øø !
(
øø! "
$num
øø" #
)
øø# $
;
øø$ %
var
¡¡ 
appointments
¡¡ 
=
¡¡ 
await
¬¬ $
_appointmentRepository
¬¬ ,
.
√√ (
GetDoctorWeekScheduleAsync
√√ /
(
√√/ 0
doctorId
ƒƒ  
,
ƒƒ  !
	startDate
≈≈ !
,
≈≈! "
endDate
∆∆ 
)
∆∆  
;
∆∆  !
return
»» 
appointments
»» 
.
»»  
Select
»»  &
(
»»& '#
MapDoctorScheduleItem
»»' <
)
»»< =
;
»»= >
}
…… 	
private
ÀÀ 
async
ÀÀ 
Task
ÀÀ "
ValidateBookingAsync
ÀÀ /
(
ÀÀ/ 0
int
ÃÃ 
	patientId
ÃÃ 
,
ÃÃ 
int
ÕÕ 
doctorId
ÕÕ 
,
ÕÕ 
DateOnly
ŒŒ 
date
ŒŒ 
,
ŒŒ 
int
œœ 
timeSlot
œœ 
)
œœ 
{
–– 	
var
—— 
patient
—— 
=
—— 
await
““  
_patientRepository
““ (
.
““( )
GetByIdAsync
““) 5
(
““5 6
	patientId
““6 ?
)
““? @
;
““@ A
if
‘‘ 
(
‘‘ 
patient
‘‘ 
==
‘‘ 
null
‘‘ 
)
‘‘  
throw
’’ 
new
’’ "
KeyNotFoundException
’’ .
(
’’. /
$str
÷÷ (
)
÷÷( )
;
÷÷) *
if
ÿÿ 
(
ÿÿ 
!
ÿÿ 
patient
ÿÿ 
.
ÿÿ 
IsActive
ÿÿ !
)
ÿÿ! "
throw
ŸŸ 
new
ŸŸ '
InvalidOperationException
ŸŸ 3
(
ŸŸ3 4
$str
⁄⁄ A
)
⁄⁄A B
;
⁄⁄B C
var
‹‹ 
doctor
‹‹ 
=
‹‹ 
await
›› 
_doctorRepository
›› '
.
››' (
GetByIdAsync
››( 4
(
››4 5
doctorId
››5 =
)
››= >
;
››> ?
if
ﬂﬂ 
(
ﬂﬂ 
doctor
ﬂﬂ 
==
ﬂﬂ 
null
ﬂﬂ 
)
ﬂﬂ 
throw
‡‡ 
new
‡‡ "
KeyNotFoundException
‡‡ .
(
‡‡. /
$str
·· '
)
··' (
;
··( )
if
„„ 
(
„„ 
!
„„ 
doctor
„„ 
.
„„ 
IsActive
„„  
)
„„  !
throw
‰‰ 
new
‰‰ '
InvalidOperationException
‰‰ 3
(
‰‰3 4
$str
ÂÂ &
)
ÂÂ& '
;
ÂÂ' (
if
ÁÁ 
(
ÁÁ 
date
ÁÁ 
<
ÁÁ 
DateOnly
ÁÁ 
.
ÁÁ  
FromDateTime
ÁÁ  ,
(
ÁÁ, -
DateTime
ÁÁ- 5
.
ÁÁ5 6
Today
ÁÁ6 ;
)
ÁÁ; <
)
ÁÁ< =
throw
ËË 
new
ËË 
ArgumentException
ËË +
(
ËË+ ,
$str
ÈÈ =
)
ÈÈ= >
;
ÈÈ> ?
if
ÎÎ 
(
ÎÎ 
!
ÎÎ 
Enum
ÎÎ 
.
ÎÎ 
	IsDefined
ÎÎ 
(
ÎÎ  
typeof
ÏÏ 
(
ÏÏ !
AppointmentTimeSlot
ÏÏ .
)
ÏÏ. /
,
ÏÏ/ 0
timeSlot
ÌÌ 
)
ÌÌ 
)
ÌÌ 
{
ÓÓ 
throw
ÔÔ 
new
ÔÔ 
ArgumentException
ÔÔ +
(
ÔÔ+ ,
$str
 /
)
/ 0
;
0 1
}
ÒÒ 
if
ÛÛ 
(
ÛÛ 
await
ÛÛ $
_appointmentRepository
ÛÛ ,
.
ÙÙ 6
(ExistsSamePatientSameDoctorSameDateAsync
ÙÙ 9
(
ÙÙ9 :
	patientId
ıı 
,
ıı 
doctorId
ˆˆ 
,
ˆˆ 
date
˜˜ 
)
˜˜ 
)
˜˜ 
{
¯¯ 
throw
˘˘ 
new
˘˘ '
InvalidOperationException
˘˘ 3
(
˘˘3 4
$str
˙˙ _
)
˙˙_ `
;
˙˙` a
}
˚˚ 
if
˝˝ 
(
˝˝ 
await
˝˝ $
_appointmentRepository
˝˝ ,
.
˛˛ 4
&ExistsSamePatientSameSlotSameDateAsync
˛˛ 7
(
˛˛7 8
	patientId
ˇˇ 
,
ˇˇ 
date
ÄÄ 
,
ÄÄ 
timeSlot
ÅÅ 
)
ÅÅ 
)
ÅÅ 
{
ÇÇ 
throw
ÉÉ 
new
ÉÉ '
InvalidOperationException
ÉÉ 3
(
ÉÉ3 4
$str
ÑÑ P
)
ÑÑP Q
;
ÑÑQ R
}
ÖÖ 
if
áá 
(
áá 
await
áá $
_appointmentRepository
áá ,
.
àà 3
%ExistsSameDoctorSameSlotSameDateAsync
àà 6
(
àà6 7
doctorId
ââ 
,
ââ 
date
ää 
,
ää 
timeSlot
ãã 
)
ãã 
)
ãã 
{
åå 
throw
çç 
new
çç '
InvalidOperationException
çç 3
(
çç3 4
$str
éé B
)
ééB C
;
ééC D
}
èè 
}
êê 	
private
íí 
async
íí 
Task
íí (
ValidateUpdateBookingAsync
íí 5
(
íí5 6
int
íí6 9
appointmentId
íí: G
,
ííG H
int
ííH K
	patientId
ííL U
,
ííU V
int
ííV Y
doctorId
ííZ b
,
ííb c
DateOnly
ííc k
date
ííl p
,
ííp q
int
ííq t
timeSlot
ííu }
)
íí} ~
{
ìì 	
var
îî 
patient
îî 
=
îî 
await
ïï  
_patientRepository
ïï (
.
ïï( )
GetByIdAsync
ïï) 5
(
ïï5 6
	patientId
ïï6 ?
)
ïï? @
;
ïï@ A
if
óó 
(
óó 
patient
óó 
==
óó 
null
óó 
)
óó  
throw
òò 
new
òò "
KeyNotFoundException
òò .
(
òò. /
$str
ôô (
)
ôô( )
;
ôô) *
if
õõ 
(
õõ 
!
õõ 
patient
õõ 
.
õõ 
IsActive
õõ !
)
õõ! "
throw
úú 
new
úú '
InvalidOperationException
úú 3
(
úú3 4
$str
ùù A
)
ùùA B
;
ùùB C
var
üü 
doctor
üü 
=
üü 
await
†† 
_doctorRepository
†† '
.
††' (
GetByIdAsync
††( 4
(
††4 5
doctorId
††5 =
)
††= >
;
††> ?
if
¢¢ 
(
¢¢ 
doctor
¢¢ 
==
¢¢ 
null
¢¢ 
)
¢¢ 
throw
££ 
new
££ "
KeyNotFoundException
££ .
(
££. /
$str
§§ '
)
§§' (
;
§§( )
if
¶¶ 
(
¶¶ 
!
¶¶ 
doctor
¶¶ 
.
¶¶ 
IsActive
¶¶  
)
¶¶  !
throw
ßß 
new
ßß '
InvalidOperationException
ßß 3
(
ßß3 4
$str
®® &
)
®®& '
;
®®' (
if
™™ 
(
™™ 
date
™™ 
<
™™ 
DateOnly
™™ 
.
™™  
FromDateTime
™™  ,
(
™™, -
DateTime
™™- 5
.
™™5 6
Today
™™6 ;
)
™™; <
)
™™< =
throw
´´ 
new
´´ 
ArgumentException
´´ +
(
´´+ ,
$str
¨¨ =
)
¨¨= >
;
¨¨> ?
if
ÆÆ 
(
ÆÆ 
!
ÆÆ 
Enum
ÆÆ 
.
ÆÆ 
	IsDefined
ÆÆ 
(
ÆÆ  
typeof
ØØ 
(
ØØ !
AppointmentTimeSlot
ØØ .
)
ØØ. /
,
ØØ/ 0
timeSlot
∞∞ 
)
∞∞ 
)
∞∞ 
{
±± 
throw
≤≤ 
new
≤≤ 
ArgumentException
≤≤ +
(
≤≤+ ,
$str
≥≥ /
)
≥≥/ 0
;
≥≥0 1
}
¥¥ 
if
∂∂ 
(
∂∂ 
await
∂∂ $
_appointmentRepository
∂∂ ,
.
∑∑ 6
(ExistsSamePatientSameDoctorSameDateAsync
∑∑ 9
(
∑∑9 :
	patientId
∏∏ 
,
∏∏ 
doctorId
ππ 
,
ππ 
date
∫∫ 
,
∫∫ 
appointmentId
ªª !
)
ªª! "
)
ªª" #
{
ºº 
throw
ΩΩ 
new
ΩΩ '
InvalidOperationException
ΩΩ 3
(
ΩΩ3 4
$str
ææ _
)
ææ_ `
;
ææ` a
}
øø 
if
¡¡ 
(
¡¡ 
await
¡¡ $
_appointmentRepository
¡¡ ,
.
¬¬ 4
&ExistsSamePatientSameSlotSameDateAsync
¬¬ 7
(
¬¬7 8
	patientId
√√ 
,
√√ 
date
ƒƒ 
,
ƒƒ 
timeSlot
≈≈ 
,
≈≈ 
appointmentId
∆∆ !
)
∆∆! "
)
∆∆" #
{
«« 
throw
»» 
new
»» '
InvalidOperationException
»» 3
(
»»3 4
$str
…… P
)
……P Q
;
……Q R
}
   
if
ÃÃ 
(
ÃÃ 
await
ÃÃ $
_appointmentRepository
ÃÃ ,
.
ÕÕ 3
%ExistsSameDoctorSameSlotSameDateAsync
ÕÕ 6
(
ÕÕ6 7
doctorId
ŒŒ 
,
ŒŒ 
date
œœ 
,
œœ 
timeSlot
–– 
,
–– 
appointmentId
—— !
)
——! "
)
——" #
{
““ 
throw
”” 
new
”” '
InvalidOperationException
”” 3
(
””3 4
$str
‘‘ B
)
‘‘B C
;
‘‘C D
}
’’ 
}
÷÷ 	
private
ÿÿ 
static
ÿÿ 
AppointmentDto
ÿÿ %!
MapToAppointmentDto
ÿÿ& 9
(
ÿÿ9 :
Appointment
ŸŸ 
appointment
ŸŸ #
)
ŸŸ# $
{
⁄⁄ 	
return
€€ 
new
€€ 
AppointmentDto
€€ %
{
‹‹ 
AppointmentId
›› 
=
›› 
appointment
››  +
.
››+ ,
AppointmentId
››, 9
,
››9 :
	PatientId
ﬁﬁ 
=
ﬁﬁ 
appointment
ﬁﬁ '
.
ﬁﬁ' (
	PatientId
ﬁﬁ( 1
,
ﬁﬁ1 2
DoctorId
ﬂﬂ 
=
ﬂﬂ 
appointment
ﬂﬂ &
.
ﬂﬂ& '
DoctorId
ﬂﬂ' /
,
ﬂﬂ/ 0
ScheduledDate
‡‡ 
=
‡‡ 
appointment
‡‡  +
.
‡‡+ ,
ScheduledDate
‡‡, 9
,
‡‡9 :
TimeSlot
·· 
=
·· 
(
·· 
int
·· 
)
··  
appointment
··  +
.
··+ ,
TimeSlot
··, 4
,
··4 5
Status
‚‚ 
=
‚‚ 
(
‚‚ 
int
‚‚ 
)
‚‚ 
appointment
‚‚ )
.
‚‚) *
Status
‚‚* 0
,
‚‚0 1 
CancellationReason
„„ "
=
„„# $
appointment
„„% 0
.
„„0 1 
CancellationReason
„„1 C
}
‰‰ 
;
‰‰ 
}
ÂÂ 	
private
ÁÁ 
static
ÁÁ #
DoctorScheduleItemDto
ÁÁ ,#
MapDoctorScheduleItem
ÁÁ- B
(
ÁÁB C
Appointment
ËË 
appointment
ËË #
)
ËË# $
{
ÈÈ 	
return
ÍÍ 
new
ÍÍ #
DoctorScheduleItemDto
ÍÍ ,
{
ÎÎ 
AppointmentId
ÏÏ 
=
ÏÏ 
appointment
ÏÏ  +
.
ÏÏ+ ,
AppointmentId
ÏÏ, 9
,
ÏÏ9 :
ScheduledDate
ÌÌ 
=
ÌÌ 
appointment
ÌÌ  +
.
ÌÌ+ ,
ScheduledDate
ÌÌ, 9
,
ÌÌ9 :
TimeSlot
ÓÓ 
=
ÓÓ 
(
ÓÓ 
int
ÓÓ 
)
ÓÓ  
appointment
ÓÓ  +
.
ÓÓ+ ,
TimeSlot
ÓÓ, 4
,
ÓÓ4 5
	PatientId
ÔÔ 
=
ÔÔ 
appointment
ÔÔ '
.
ÔÔ' (
	PatientId
ÔÔ( 1
,
ÔÔ1 2
PatientName
 
=
 
appointment
 )
.
) *
Patient
* 1
.
1 2
FullName
2 :
,
: ;
Status
ÒÒ 
=
ÒÒ 
(
ÒÒ 
int
ÒÒ 
)
ÒÒ 
appointment
ÒÒ )
.
ÒÒ) *
Status
ÒÒ* 0
}
ÚÚ 
;
ÚÚ 
}
ÛÛ 	
}
ÙÙ 
}ıı ¬,
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
IAdminRepository 

repository '
)' (
{ 	
_repository 
= 

repository $
;$ %
} 	
public 
async 
Task 
< 
AdminDashboardDto +
>+ ,
GetDashboardAsync 
( 
) 
{ 	
return 
new 
AdminDashboardDto (
{ 
TotalPatients 
= 
await 
_repository %
.% &
CountPatientsAsync& 8
(8 9
)9 :
,: ;
ActivePatients 
=  
await 
_repository %
.% &$
CountActivePatientsAsync& >
(> ?
)? @
,@ A
TotalDoctors 
= 
await 
_repository %
.% &
CountDoctorsAsync& 7
(7 8
)8 9
,9 :
ActiveDoctors 
= 
await   
_repository   %
.  % &#
CountActiveDoctorsAsync  & =
(  = >
)  > ?
,  ? @
TodayAppointments"" !
=""" #
await## 
_repository## %
.##% &'
CountTodayAppointmentsAsync##& A
(##A B
)##B C
,##C D
PendingAppointments%% #
=%%$ %
await&& 
_repository&& %
.&&% &)
CountPendingAppointmentsAsync&&& C
(&&C D
)&&D E
,&&E F!
CompletedAppointments(( %
=((& '
await)) 
_repository)) %
.))% &+
CountCompletedAppointmentsAsync))& E
())E F
)))F G
}** 
;** 
}++ 	
public-- 
async-- 
Task-- 
<-- 
AdminStatisticsDto-- ,
>--, -
GetStatisticsAsync.. 
(.. 
)..  
{// 	
return00 
new00 
AdminStatisticsDto00 )
{11 
Patients22 
=22 
await33 
_repository33 %
.33% &
CountPatientsAsync33& 8
(338 9
)339 :
,33: ;
Doctors55 
=55 
await66 
_repository66 %
.66% &
CountDoctorsAsync66& 7
(667 8
)668 9
,669 :
Appointments88 
=88 
await99 
_repository99 %
.99% &'
CountTodayAppointmentsAsync99& A
(99A B
)99B C
,99C D
HealthRecords;; 
=;; 
await<< 
_repository<< %
.<<% &#
CountHealthRecordsAsync<<& =
(<<= >
)<<> ?
}== 
;== 
}>> 	
public@@ 
async@@ 
Task@@ 
<@@ 
IEnumerable@@ %
<@@% &
UserManagementDto@@& 7
>@@7 8
>@@8 9
GetUsersAsyncAA 
(AA 
)AA 
{BB 	
varCC 
usersCC 
=CC 
awaitDD 
_repositoryDD !
.DD! "
GetUsersAsyncDD" /
(DD/ 0
)DD0 1
;DD1 2
returnFF 
usersFF 
.FF 
SelectFF 
(FF  
uFF  !
=>FF" $
newGG 
UserManagementDtoGG %
{HH 
UserIdII 
=II 
uII 
.II 
UserIdII %
,II% &
EmailJJ 
=JJ 
uJJ 
.JJ 
EmailJJ #
,JJ# $
RoleKK 
=KK 
uKK 
.KK 
RoleKK !
.KK! "
ToStringKK" *
(KK* +
)KK+ ,
,KK, -
ReferenceIdLL 
=LL  !
uLL" #
.LL# $
ReferenceIdLL$ /
}MM 
)MM 
;MM 
}NN 	
publicPP 
asyncPP 
TaskPP 
<PP 
UserManagementDtoPP +
?PP+ ,
>PP, -
GetUserByIdAsyncQQ 
(QQ 
intQQ  
idQQ! #
)QQ# $
{RR 	
varSS 
userSS 
=SS 
awaitTT 
_repositoryTT !
.TT! "
GetUserByIdAsyncTT" 2
(TT2 3
idTT3 5
)TT5 6
;TT6 7
ifVV 
(VV 
userVV 
==VV 
nullVV 
)VV 
returnWW 
nullWW 
;WW 
returnYY 
newYY 
UserManagementDtoYY (
{ZZ 
UserId[[ 
=[[ 
user[[ 
.[[ 
UserId[[ $
,[[$ %
Email\\ 
=\\ 
user\\ 
.\\ 
Email\\ "
,\\" #
Role]] 
=]] 
user]] 
.]] 
Role]]  
.]]  !
ToString]]! )
(]]) *
)]]* +
,]]+ ,
ReferenceId^^ 
=^^ 
user^^ "
.^^" #
ReferenceId^^# .
}__ 
;__ 
}`` 	
}aa 
}bb ¢
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
} á
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
Task 
< 
User 
? 
> 
GetUserByIdAsync $
($ %
int% (
id) +
)+ ,
;, -
} 
} ò$
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\UserRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public		 

class		 
UserRepository		 
:		  !
IUserRepository		" 1
{

 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
UserRepository 
( 
HealthAxisDbContext 1
context2 9
)9 :
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByIdAsync! -
(- .
int. 1
id2 4
)4 5
{ 	
return 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
UserId, 2
==3 5
id6 8
)8 9
;9 :
} 	
public 
async 
Task 
< 
User 
? 
>  
GetByEmailAsync! 0
(0 1
string1 7
email8 =
)= >
{ 	
return 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
Email, 1
==2 4
email5 :
): ;
;; <
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
User& *
>* +
>+ ,
GetByRoleAsync- ;
(; <
UserRole< D
roleE I
)I J
{ 	
return   
await   
_context   !
.  ! "
Users  " '
.!! 
Where!! 
(!! 
u!! 
=>!! 
u!! 
.!! 
Role!! "
==!!# %
role!!& *
)!!* +
."" 
ToListAsync"" 
("" 
)"" 
;"" 
}## 	
public%% 
async%% 
Task%% 
AddAsync%% "
(%%" #
User%%# '
user%%( ,
)%%, -
{&& 	
await'' 
_context'' 
.'' 
Users''  
.''  !
AddAsync''! )
('') *
user''* .
)''. /
;''/ 0
}(( 	
public** 
Task** 
UpdateAsync** 
(**  
User**  $
user**% )
)**) *
{++ 	
_context,, 
.,, 
Users,, 
.,, 
Update,, !
(,,! "
user,," &
),,& '
;,,' (
return-- 
Task-- 
.-- 
CompletedTask-- %
;--% &
}.. 	
public00 
async00 
Task00 
<00 
bool00 
>00 
EmailExistsAsync00  0
(000 1
string001 7
email008 =
)00= >
{11 	
return22 
await22 
_context22 !
.22! "
Users22" '
.33 
AnyAsync33 
(33 
u33 
=>33 
u33  
.33  !
Email33! &
==33' )
email33* /
)33/ 0
;330 1
}44 	
public66 
async66 
Task66 
SaveChangesAsync66 *
(66* +
)66+ ,
{77 	
await88 
_context88 
.88 
SaveChangesAsync88 +
(88+ ,
)88, -
;88- .
}99 	
public;; 
async;; 
Task;; 
<;; 
User;; 
?;; 
>;;  "
GetByRefreshTokenAsync;;! 7
(;;7 8
string;;8 >
refreshToken;;? K
);;K L
{<< 	
return== 
await== 
_context== !
.==! "
Users==" '
.>> 
FirstOrDefaultAsync>> $
(>>$ %
u>>% &
=>>>' )
u>>* +
.>>+ ,
RefreshToken>>, 8
==>>9 ;
refreshToken>>< H
)>>H I
;>>I J
}?? 	
}@@ 
}AA È%
lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\PatientRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public 

class 
PatientRepository "
:# $
IPatientRepository% 7
{		 
private

 
readonly

 
HealthAxisDbContext

 ,
_context

- 5
;

5 6
public 
PatientRepository  
(  !
HealthAxisDbContext! 4
context5 <
)< =
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Patient& -
>- .
>. /
GetAllAsync0 ;
(; <
)< =
{ 	
return 
await 
_context !
.! "
Patients" *
. 
OrderBy 
( 
p 
=> 
p 
.  
	PatientId  )
)) *
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByIdAsync$ 0
(0 1
int1 4
id5 7
)7 8
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
	PatientId, 5
==6 8
id9 ;
); <
;< =
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Patient& -
>- .
>. /
SearchByNameAsync0 A
(A B
stringB H
nameI M
)M N
{ 	
if   
(   
string   
.   
IsNullOrWhiteSpace   )
(  ) *
name  * .
)  . /
)  / 0
return!! 
new!! 
List!! 
<!!  
Patient!!  '
>!!' (
(!!( )
)!!) *
;!!* +
name## 
=## 
name## 
.## 
Trim## 
(## 
)## 
;## 
return%% 
await%% 
_context%% !
.%%! "
Patients%%" *
.&& 
Where&& 
(&& 
p&& 
=>&& 
p&& 
.&& 
IsActive&& &
&&&&' )
p&&* +
.&&+ ,
FullName&&, 4
.&&4 5
Contains&&5 =
(&&= >
name&&> B
)&&B C
)&&C D
.'' 
OrderBy'' 
('' 
p'' 
=>'' 
p'' 
.''  
FullName''  (
)''( )
.(( 
ToListAsync(( 
((( 
)(( 
;(( 
})) 	
public++ 
async++ 
Task++ 
AddAsync++ "
(++" #
Patient++# *
patient+++ 2
)++2 3
{,, 	
await-- 
_context-- 
.-- 
Patients-- #
.--# $
AddAsync--$ ,
(--, -
patient--- 4
)--4 5
;--5 6
}.. 	
public00 
Task00 
UpdateAsync00 
(00  
Patient00  '
patient00( /
)00/ 0
{11 	
_context22 
.22 
Patients22 
.22 
Update22 $
(22$ %
patient22% ,
)22, -
;22- .
return33 
Task33 
.33 
CompletedTask33 %
;33% &
}44 	
public66 
async66 
Task66 
<66 
bool66 
>66 
ExistsAsync66  +
(66+ ,
int66, /
id660 2
)662 3
{77 	
return88 
await88 
_context88 !
.88! "
Patients88" *
.88* +
AnyAsync88+ 3
(883 4
p884 5
=>886 8
p889 :
.88: ;
	PatientId88; D
==88E G
id88H J
)88J K
;88K L
}99 	
public;; 
async;; 
Task;; 
SaveChangesAsync;; *
(;;* +
);;+ ,
{<< 	
await== 
_context== 
.== 
SaveChangesAsync== +
(==+ ,
)==, -
;==- .
}>> 	
}?? 
}@@ ”
gC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\HealthRecord.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public 

class "
HealthRecordRepository '
:( )#
IHealthRecordRepository* A
{		 
private

 
readonly

 
HealthAxisDbContext

 ,
_context

- 5
;

5 6
public "
HealthRecordRepository %
(% &
HealthAxisDbContext& 9
context: A
)A B
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
HealthRecord &
?& '
>' (
GetByIdAsync) 5
(5 6
int6 9
id: <
)< =
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
. 
Include 
( 
hr 
=> 
hr !
.! "
Doctor" (
)( )
. 
Include 
( 
hr 
=> 
hr !
.! "
Appointment" -
)- .
. 
FirstOrDefaultAsync $
($ %
hr% '
=>( *
hr+ -
.- .
HealthRecordId. <
=== ?
id@ B
)B C
;C D
} 	
public 
async 
Task 
< 
HealthRecord &
?& '
>' (#
GetByAppointmentIdAsync) @
(@ A
intA D
appointmentIdE R
)R S
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
. 
Include 
( 
hr 
=> 
hr !
.! "
Doctor" (
)( )
. 
Include 
( 
hr 
=> 
hr !
.! "
Appointment" -
)- .
.   
FirstOrDefaultAsync   $
(  $ %
hr  % '
=>  ( *
hr  + -
.  - .
AppointmentId  . ;
==  < >
appointmentId  ? L
)  L M
;  M N
}!! 	
public## 
async## 
Task## 
AddAsync## "
(##" #
HealthRecord### /
record##0 6
)##6 7
{$$ 	
await%% 
_context%% 
.%% 
HealthRecords%% (
.%%( )
AddAsync%%) 1
(%%1 2
record%%2 8
)%%8 9
;%%9 :
}&& 	
public(( 
Task(( 
UpdateAsync(( 
(((  
HealthRecord((  ,
record((- 3
)((3 4
{)) 	
_context** 
.** 
HealthRecords** "
.**" #
Update**# )
(**) *
record*** 0
)**0 1
;**1 2
return++ 
Task++ 
.++ 
CompletedTask++ %
;++% &
},, 	
public.. 
async.. 
Task.. 
SaveChangesAsync.. *
(..* +
)..+ ,
{// 	
await00 
_context00 
.00 
SaveChangesAsync00 +
(00+ ,
)00, -
;00- .
}11 	
}22 
}33 ﬁ 
lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\GenericRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public 

class 
GenericRepository "
<" #
T# $
>$ %
:& '
IGenericRepository( :
<: ;
T; <
>< =
where 
T 
: 
class 
{		 
	protected

 
readonly

 
HealthAxisDbContext

 .
_context

/ 7
;

7 8
	protected 
readonly 
DbSet  
<  !
T! "
>" #
_dbSet$ *
;* +
public 
GenericRepository  
(  !
HealthAxisDbContext! 4
context5 <
)< =
{ 	
_context 
= 
context 
; 
_dbSet 
= 
context 
. 
Set  
<  !
T! "
>" #
(# $
)$ %
;% &
} 	
public 
virtual 
async 
Task !
<! "
IEnumerable" -
<- .
T. /
>/ 0
>0 1
GetAllAsync2 =
(= >
)> ?
{ 	
return 
await 
_dbSet 
.  
ToListAsync  +
(+ ,
), -
;- .
} 	
public 
virtual 
async 
Task !
<! "
T" #
?# $
>$ %
GetByIdAsync& 2
(2 3
int3 6
id7 9
)9 :
{ 	
return 
await 
_dbSet 
.  
	FindAsync  )
() *
id* ,
), -
;- .
} 	
public 
virtual 
async 
Task !
AddAsync" *
(* +
T+ ,
entity- 3
)3 4
{ 	
await 
_dbSet 
. 
AddAsync !
(! "
entity" (
)( )
;) *
await   
_context   
.   
SaveChangesAsync   +
(  + ,
)  , -
;  - .
}!! 	
public## 
virtual## 
async## 
Task## !
UpdateAsync##" -
(##- .
T##. /
entity##0 6
)##6 7
{$$ 	
_dbSet%% 
.%% 
Update%% 
(%% 
entity%%  
)%%  !
;%%! "
await&& 
_context&& 
.&& 
SaveChangesAsync&& +
(&&+ ,
)&&, -
;&&- .
}'' 	
public)) 
virtual)) 
async)) 
Task)) !
DeleteAsync))" -
())- .
int)). 1
id))2 4
)))4 5
{** 	
var++ 
entity++ 
=++ 
await++ 
_dbSet++ %
.++% &
	FindAsync++& /
(++/ 0
id++0 2
)++2 3
;++3 4
if-- 
(-- 
entity-- 
!=-- 
null-- 
)-- 
{.. 
_dbSet// 
.// 
Remove// 
(// 
entity// $
)//$ %
;//% &
await00 
_context00 
.00 
SaveChangesAsync00 /
(00/ 0
)000 1
;001 2
}11 
}22 	
public44 
virtual44 
async44 
Task44 !
<44! "
bool44" &
>44& '
ExistsAsync44( 3
(443 4
int444 7
id448 :
)44: ;
{55 	
return66 
await66 
_dbSet66 
.66  
	FindAsync66  )
(66) *
id66* ,
)66, -
!=66. 0
null661 5
;665 6
}77 	
}88 
}99 ∏@
kC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\DoctorRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public		 

class		 
DoctorRepository		 !
:		" #
IDoctorRepository		$ 5
{

 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
DoctorRepository 
(  
HealthAxisDbContext  3
context4 ;
); <
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Doctor& ,
>, -
>- .
GetAllAsync/ :
(: ;
string; A
?A B
sortByC I
,I J
intK N
?N O
specialisationP ^
)^ _
{ 	

IQueryable 
< 
Doctor 
> 
query $
=% &
_context' /
./ 0
Doctors0 7
;7 8
if 
( 
specialisation 
. 
HasValue '
)' (
{ 
if 
( 
! 
Enum 
. 
	IsDefined #
(# $
typeof$ *
(* + 
DoctorSpecialisation+ ?
)? @
,@ A
specialisationB P
.P Q
ValueQ V
)V W
)W X
throw 
new 
ArgumentException /
(/ 0
$str0 P
)P Q
;Q R
var  
doctorSpecialisation (
=) *
(+ , 
DoctorSpecialisation, @
)@ A
specialisationA O
.O P
ValueP U
;U V
query 
= 
query 
. 
Where #
(# $
d$ %
=>& (
d) *
.* +
Specialisation+ 9
==: < 
doctorSpecialisation= Q
)Q R
;R S
} 
query 
= 
sortBy 
? 
. 
ToLower #
(# $
)$ %
switch& ,
{   
$str!! 
=>!! 
query!! 
.!!  
OrderBy!!  '
(!!' (
d!!( )
=>!!* ,
d!!- .
.!!. /
FullName!!/ 7
)!!7 8
,!!8 9
$str"" 
=>"" 
query"" $
.""$ %
OrderByDescending""% 6
(""6 7
d""7 8
=>""9 ;
d""< =
.""= >
FullName""> F
)""F G
,""G H
$str## 
=>## 
query## 
.## 
OrderBy## %
(##% &
d##& '
=>##( *
d##+ ,
.##, -
DoctorId##- 5
)##5 6
,##6 7
_$$ 
=>$$ 
query$$ 
.$$ 
OrderBy$$ "
($$" #
d$$# $
=>$$% '
d$$( )
.$$) *
DoctorId$$* 2
)$$2 3
}%% 
;%% 
return'' 
await'' 
query'' 
.'' 
ToListAsync'' *
(''* +
)''+ ,
;'', -
}(( 	
public** 
async** 
Task** 
<** 
IEnumerable** %
<**% &
Doctor**& ,
>**, -
>**- .*
GetActiveBySpecialisationAsync**/ M
(**M N
int**N Q
specialisation**R `
)**` a
{++ 	
if,, 
(,, 
!,, 
Enum,, 
.,, 
	IsDefined,, 
(,,  
typeof,,  &
(,,& ' 
DoctorSpecialisation,,' ;
),,; <
,,,< =
specialisation,,> L
),,L M
),,M N
throw-- 
new-- 
ArgumentException-- +
(--+ ,
$str--, L
)--L M
;--M N
var//  
doctorSpecialisation// $
=//% &
(//' ( 
DoctorSpecialisation//( <
)//< =
specialisation//= K
;//K L
return11 
await11 
_context11 !
.11! "
Doctors11" )
.22 
Where22 
(22 
d22 
=>22 
d22 
.22 
IsActive22 &
&&22' )
d22* +
.22+ ,
Specialisation22, :
==22; = 
doctorSpecialisation22> R
)22R S
.33 
OrderBy33 
(33 
d33 
=>33 
d33 
.33  
FullName33  (
)33( )
.44 
ToListAsync44 
(44 
)44 
;44 
}55 	
public77 
async77 
Task77 
<77 
Doctor77  
?77  !
>77! "
GetByIdAsync77# /
(77/ 0
int770 3
id774 6
)776 7
{88 	
return99 
await99 
_context99 !
.99! "
Doctors99" )
.99) *
	FindAsync99* 3
(993 4
id994 6
)996 7
;997 8
}:: 	
public<< 
async<< 
Task<< 
AddAsync<< "
(<<" #
Doctor<<# )
doctor<<* 0
)<<0 1
{== 	
await>> 
_context>> 
.>> 
Doctors>> "
.>>" #
AddAsync>># +
(>>+ ,
doctor>>, 2
)>>2 3
;>>3 4
}?? 	
publicAA 
TaskAA 
UpdateAsyncAA 
(AA  
DoctorAA  &
doctorAA' -
)AA- .
{BB 	
_contextCC 
.CC 
DoctorsCC 
.CC 
UpdateCC #
(CC# $
doctorCC$ *
)CC* +
;CC+ ,
returnDD 
TaskDD 
.DD 
CompletedTaskDD %
;DD% &
}EE 	
publicGG 
asyncGG 
TaskGG 
<GG 
IEnumerableGG %
<GG% &
intGG& )
>GG) *
>GG* +
GetBookedSlotsAsyncGG, ?
(GG? @
intGG@ C
doctorIdGGD L
,GGL M
DateOnlyGGM U
dateGGV Z
)GGZ [
{HH 	
returnII 
awaitII 
_contextII !
.II! "
AppointmentsII" .
.JJ 
WhereJJ 
(JJ 
aJJ 
=>JJ 
aKK 
.KK 
DoctorIdKK 
==KK !
doctorIdKK" *
&&KK+ -
aLL 
.LL 
ScheduledDateLL #
==LL$ &
dateLL' +
&&LL, .
aMM 
.MM 
StatusMM 
!=MM 
AppointmentStatusMM  1
.MM1 2
	CancelledMM2 ;
)MM; <
.NN 
SelectNN 
(NN 
aNN 
=>NN 
(NN 
intNN !
)NN! "
aNN" #
.NN# $
TimeSlotNN$ ,
)NN, -
.OO 
ToListAsyncOO 
(OO 
)OO 
;OO 
}PP 	
publicRR 
asyncRR 
TaskRR 
<RR 
boolRR 
>RR 
ExistsAsyncRR  +
(RR+ ,
intRR, /
idRR0 2
)RR2 3
{SS 	
returnTT 
awaitTT 
_contextTT !
.TT! "
DoctorsTT" )
.TT) *
AnyAsyncTT* 2
(TT2 3
dTT3 4
=>TT5 7
dTT8 9
.TT9 :
DoctorIdTT: B
==TTC E
idTTF H
)TTH I
;TTI J
}UU 	
publicWW 
asyncWW 
TaskWW 
SaveChangesAsyncWW *
(WW* +
)WW+ ,
{XX 	
awaitYY 
_contextYY 
.YY 
SaveChangesAsyncYY +
(YY+ ,
)YY, -
;YY- .
}ZZ 	
}[[ 
}\\ ˙~
pC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\AppointmentRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{		 
public

 

class

 !
AppointmentRepository

 &
:

' ("
IAppointmentRepository

) ?
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public !
AppointmentRepository $
($ %
HealthAxisDbContext% 8
context9 @
)@ A
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
<% &
Appointment& 1
>1 2
>2 3
GetAllAsync4 ?
(? @
)@ A
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
. 
Include 
( 
a 
=> 
a 
.  
Doctor  &
)& '
. 
OrderByDescending "
(" #
a# $
=>% '
a( )
.) *
ScheduledDate* 7
)7 8
. 
ThenBy 
( 
a 
=> 
a 
. 
TimeSlot '
)' (
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
Appointment %
?% &
>& '
GetByIdAsync( 4
(4 5
int5 8
id9 ;
); <
{ 	
return 
await 
_context !
.! "
Appointments" .
.   
Include   
(   
a   
=>   
a   
.    
Patient    '
)  ' (
.!! 
Include!! 
(!! 
a!! 
=>!! 
a!! 
.!!  
Doctor!!  &
)!!& '
."" 
FirstOrDefaultAsync"" $
(""$ %
a""% &
=>""' )
a""* +
.""+ ,
AppointmentId"", 9
=="": <
id""= ?
)""? @
;""@ A
}## 	
public%% 
async%% 
Task%% 
<%% 
IEnumerable%% %
<%%% &
Appointment%%& 1
>%%1 2
>%%2 3
GetByPatientIdAsync%%4 G
(%%G H
int%%H K
	patientId%%L U
)%%U V
{&& 	
return'' 
await'' 
_context'' !
.''! "
Appointments''" .
.(( 
Include(( 
((( 
a(( 
=>(( 
a(( 
.((  
Doctor((  &
)((& '
.)) 
Where)) 
()) 
a)) 
=>)) 
a)) 
.)) 
	PatientId)) '
==))( *
	patientId))+ 4
)))4 5
.** 
OrderByDescending** "
(**" #
a**# $
=>**% '
a**( )
.**) *
ScheduledDate*** 7
)**7 8
.++ 
ThenBy++ 
(++ 
a++ 
=>++ 
a++ 
.++ 
TimeSlot++ '
)++' (
.,, 
ToListAsync,, 
(,, 
),, 
;,, 
}-- 	
public// 
async// 
Task// 
<// 
IEnumerable// %
<//% &
Appointment//& 1
>//1 2
>//2 3'
GetDoctorTodayScheduleAsync//4 O
(//O P
int//P S
doctorId//T \
,//\ ]
DateOnly//^ f
today//g l
)//l m
{00 	
return11 
await11 
_context11 !
.11! "
Appointments11" .
.22 
Include22 
(22 
a22 
=>22 
a22 
.22  
Patient22  '
)22' (
.33 
Where33 
(33 
a33 
=>33 
a33 
.33 
DoctorId33 &
==33' )
doctorId33* 2
&&333 5
a336 7
.337 8
ScheduledDate338 E
==33F H
today33I N
)33N O
.44 
OrderBy44 
(44 
a44 
=>44 
a44 
.44  
TimeSlot44  (
)44( )
.55 
ToListAsync55 
(55 
)55 
;55 
}66 	
public88 
async88 
Task88 
<88 
IEnumerable88 %
<88% &
Appointment88& 1
>881 2
>882 3&
GetDoctorWeekScheduleAsync884 N
(88N O
int88O R
doctorId88S [
,88[ \
DateOnly88] e
	startDate88f o
,88o p
DateOnly88q y
endDate	88z Å
)
88Å Ç
{99 	
return:: 
await:: 
_context:: !
.::! "
Appointments::" .
.;; 
Include;; 
(;; 
a;; 
=>;; 
a;; 
.;;  
Patient;;  '
);;' (
.<< 
Where<< 
(<< 
a<< 
=><< 
a<< 
.<< 
DoctorId<< &
==<<' )
doctorId<<* 2
&&<<3 5
a== 
.== 
ScheduledDate== +
>===, .
	startDate==/ 8
&&==9 ;
a>> 
.>> 
ScheduledDate>> +
<=>>, .
endDate>>/ 6
)>>6 7
.?? 
OrderBy?? 
(?? 
a?? 
=>?? 
a?? 
.??  
ScheduledDate??  -
)??- .
.@@ 
ThenBy@@ 
(@@ 
a@@ 
=>@@ 
a@@ 
.@@ 
TimeSlot@@ '
)@@' (
.AA 
ToListAsyncAA 
(AA 
)AA 
;AA 
}BB 	
publicDD 
asyncDD 
TaskDD 
<DD 
boolDD 
>DD 4
(ExistsSamePatientSameDoctorSameDateAsyncDD  H
(DDH I
intDDI L
	patientIdDDM V
,DDV W
intDDX [
doctorIdDD\ d
,DDd e
DateOnlyDDf n
dateDDo s
)DDs t
{EE 	
returnFF 
awaitFF 
_contextFF !
.FF! "
AppointmentsFF" .
.FF. /
AnyAsyncFF/ 7
(FF7 8
aFF8 9
=>FF: <
aGG 
.GG 
	PatientIdGG 
==GG 
	patientIdGG (
&&GG) +
aHH 
.HH 
DoctorIdHH 
==HH 
doctorIdHH &
&&HH' )
aII 
.II 
ScheduledDateII 
==II  "
dateII# '
&&II( *
aJJ 
.JJ 
StatusJJ 
!=JJ 
AppointmentStatusJJ -
.JJ- .
	CancelledJJ. 7
)JJ7 8
;JJ8 9
}KK 	
publicMM 
asyncMM 
TaskMM 
<MM 
boolMM 
>MM 2
&ExistsSamePatientSameSlotSameDateAsyncMM  F
(MMF G
intMMG J
	patientIdMMK T
,MMT U
DateOnlyMMV ^
dateMM_ c
,MMc d
intMMe h
timeSlotMMi q
)MMq r
{NN 	
ifOO 
(OO 
!OO 
EnumOO 
.OO 
	IsDefinedOO 
(OO  
typeofOO  &
(OO& '
AppointmentTimeSlotOO' :
)OO: ;
,OO; <
timeSlotOO= E
)OOE F
)OOF G
throwPP 
newPP 
ArgumentExceptionPP +
(PP+ ,
$strPP, L
)PPL M
;PPM N
varRR 
slotEnumRR 
=RR 
(RR 
AppointmentTimeSlotRR /
)RR/ 0
timeSlotRR0 8
;RR8 9
returnTT 
awaitTT 
_contextTT !
.TT! "
AppointmentsTT" .
.TT. /
AnyAsyncTT/ 7
(TT7 8
aTT8 9
=>TT: <
aUU 
.UU 
	PatientIdUU 
==UU 
	patientIdUU (
&&UU) +
aVV 
.VV 
ScheduledDateVV 
==VV  "
dateVV# '
&&VV( *
aWW 
.WW 
TimeSlotWW 
==WW 
slotEnumWW &
&&WW' )
aXX 
.XX 
StatusXX 
!=XX 
AppointmentStatusXX -
.XX- .
	CancelledXX. 7
)XX7 8
;XX8 9
}YY 	
public[[ 
async[[ 
Task[[ 
<[[ 
bool[[ 
>[[ 1
%ExistsSameDoctorSameSlotSameDateAsync[[  E
([[E F
int[[F I
doctorId[[J R
,[[R S
DateOnly[[T \
date[[] a
,[[a b
int[[c f
timeSlot[[g o
)[[o p
{\\ 	
if]] 
(]] 
!]] 
Enum]] 
.]] 
	IsDefined]] 
(]]  
typeof]]  &
(]]& '
AppointmentTimeSlot]]' :
)]]: ;
,]]; <
timeSlot]]= E
)]]E F
)]]F G
throw^^ 
new^^ 
ArgumentException^^ +
(^^+ ,
$str^^, L
)^^L M
;^^M N
var`` 
slotEnum`` 
=`` 
(`` 
AppointmentTimeSlot`` /
)``/ 0
timeSlot``0 8
;``8 9
returnbb 
awaitbb 
_contextbb !
.bb! "
Appointmentsbb" .
.bb. /
AnyAsyncbb/ 7
(bb7 8
abb8 9
=>bb: <
acc 
.cc 
DoctorIdcc 
==cc 
doctorIdcc &
&&cc' )
add 
.dd 
ScheduledDatedd 
==dd  "
datedd# '
&&dd( *
aee 
.ee 
TimeSlotee 
==ee 
slotEnumee &
&&ee' )
aff 
.ff 
Statusff 
!=ff 
AppointmentStatusff -
.ff- .
	Cancelledff. 7
)ff7 8
;ff8 9
}gg 	
publicii 
asyncii 
Taskii 
<ii 
boolii 
>ii 4
(ExistsSamePatientSameDoctorSameDateAsyncii  H
(iiH I
intiiI L
	patientIdiiM V
,iiV W
intiiX [
doctorIdii\ d
,iid e
DateOnlyiif n
dateiio s
,iis t
intiiu x
appointmentId	iiy Ü
)
iiÜ á
{jj 	
returnkk 
awaitkk 
_contextkk !
.kk! "
Appointmentskk" .
.kk. /
AnyAsynckk/ 7
(kk7 8
akk8 9
=>kk: <
all 
.ll 
AppointmentIdll 
!=ll  "
appointmentIdll# 0
&&ll1 3
amm 
.mm 
	PatientIdmm 
==mm 
	patientIdmm (
&&mm) +
ann 
.nn 
DoctorIdnn 
==nn 
doctorIdnn &
&&nn' )
aoo 
.oo 
ScheduledDateoo 
==oo  "
dateoo# '
&&oo( *
app 
.pp 
Statuspp 
!=pp 
AppointmentStatuspp -
.pp- .
	Cancelledpp. 7
)pp7 8
;pp8 9
}qq 	
publicss 
asyncss 
Taskss 
<ss 
boolss 
>ss 2
&ExistsSamePatientSameSlotSameDateAsyncss  F
(ssF G
intssG J
	patientIdssK T
,ssT U
DateOnlyssV ^
datess_ c
,ssc d
intsse h
timeSlotssi q
,ssq r
intsss v
appointmentId	ssw Ñ
)
ssÑ Ö
{tt 	
returnuu 
awaituu 
_contextuu !
.uu! "
Appointmentsuu" .
.uu. /
AnyAsyncuu/ 7
(uu7 8
auu8 9
=>uu: <
avv 
.vv 
AppointmentIdvv 
!=vv  "
appointmentIdvv# 0
&&vv1 3
aww 
.ww 
	PatientIdww 
==ww 
	patientIdww (
&&ww) +
axx 
.xx 
ScheduledDatexx 
==xx  "
datexx# '
&&xx( *
(yy 
intyy 
)yy 
ayy 
.yy 
TimeSlotyy 
==yy  "
timeSlotyy# +
&&yy, .
azz 
.zz 
Statuszz 
!=zz 
AppointmentStatuszz -
.zz- .
	Cancelledzz. 7
)zz7 8
;zz8 9
}{{ 	
public}} 
async}} 
Task}} 
<}} 
bool}} 
>}} 1
%ExistsSameDoctorSameSlotSameDateAsync}}  E
(}}E F
int}}F I
doctorId}}J R
,}}R S
DateOnly}}T \
date}}] a
,}}a b
int}}c f
timeSlot}}g o
,}}o p
int}}q t
appointmentId	}}u Ç
)
}}Ç É
{~~ 	
return 
await 
_context !
.! "
Appointments" .
.. /
AnyAsync/ 7
(7 8
a8 9
=>: <
a
ÄÄ 
.
ÄÄ 
AppointmentId
ÄÄ 
!=
ÄÄ  "
appointmentId
ÄÄ# 0
&&
ÄÄ1 3
a
ÅÅ 
.
ÅÅ 
DoctorId
ÅÅ 
==
ÅÅ 
doctorId
ÅÅ &
&&
ÅÅ' )
a
ÇÇ 
.
ÇÇ 
ScheduledDate
ÇÇ 
==
ÇÇ  "
date
ÇÇ# '
&&
ÇÇ( *
(
ÉÉ 
int
ÉÉ 
)
ÉÉ 
a
ÉÉ 
.
ÉÉ 
TimeSlot
ÉÉ 
==
ÉÉ  "
timeSlot
ÉÉ# +
&&
ÉÉ, .
a
ÑÑ 
.
ÑÑ 
Status
ÑÑ 
!=
ÑÑ 
AppointmentStatus
ÑÑ -
.
ÑÑ- .
	Cancelled
ÑÑ. 7
)
ÑÑ7 8
;
ÑÑ8 9
}
ÖÖ 	
public
áá 
async
áá 
Task
áá 
AddAsync
áá "
(
áá" #
Appointment
áá# .
appointment
áá/ :
)
áá: ;
{
àà 	
await
ââ 
_context
ââ 
.
ââ 
Appointments
ââ '
.
ââ' (
AddAsync
ââ( 0
(
ââ0 1
appointment
ââ1 <
)
ââ< =
;
ââ= >
}
ää 	
public
åå 
Task
åå 
UpdateAsync
åå 
(
åå  
Appointment
åå  +
appointment
åå, 7
)
åå7 8
{
çç 	
_context
éé 
.
éé 
Appointments
éé !
.
éé! "
Update
éé" (
(
éé( )
appointment
éé) 4
)
éé4 5
;
éé5 6
return
èè 
Task
èè 
.
èè 
CompletedTask
èè %
;
èè% &
}
êê 	
public
íí 
async
íí 
Task
íí 
<
íí 
bool
íí 
>
íí 
ExistsAsync
íí  +
(
íí+ ,
int
íí, /
id
íí0 2
)
íí2 3
{
ìì 	
return
îî 
await
îî 
_context
îî !
.
îî! "
Appointments
îî" .
.
îî. /
AnyAsync
îî/ 7
(
îî7 8
a
îî8 9
=>
îî: <
a
îî= >
.
îî> ?
AppointmentId
îî? L
==
îîM O
id
îîP R
)
îîR S
;
îîS T
}
ïï 	
public
óó 
async
óó 
Task
óó 
SaveChangesAsync
óó *
(
óó* +
)
óó+ ,
{
òò 	
await
ôô 
_context
ôô 
.
ôô 
SaveChangesAsync
ôô +
(
ôô+ ,
)
ôô, -
;
ôô- .
}
öö 	
}
õõ 
}úú ë(
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Repositories\Implementation\AdminRepository.cs
	namespace 	
S3_HealthAxisApi
 
. 

Repository %
.% &
Implementation& 4
{ 
public		 

class		 
AdminRepository		  
:		! "
IAdminRepository		# 3
{

 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
AdminRepository 
( 
HealthAxisDbContext 
context  '
)' (
{ 	
_context 
= 
context 
; 
} 	
public 
Task 
< 
int 
> 
CountPatientsAsync +
(+ ,
), -
=> 
_context 
. 
Patients  
.  !

CountAsync! +
(+ ,
), -
;- .
public 
Task 
< 
int 
> $
CountActivePatientsAsync 1
(1 2
)2 3
=> 
_context 
. 
Patients  
. 

CountAsync 
( 
p 
=>  
p! "
." #
IsActive# +
)+ ,
;, -
public 
Task 
< 
int 
> 
CountDoctorsAsync *
(* +
)+ ,
=> 
_context 
. 
Doctors 
.  

CountAsync  *
(* +
)+ ,
;, -
public 
Task 
< 
int 
> #
CountActiveDoctorsAsync 0
(0 1
)1 2
=> 
_context 
. 
Doctors 
. 

CountAsync 
( 
d 
=>  
d! "
." #
IsActive# +
)+ ,
;, -
public!! 
Task!! 
<!! 
int!! 
>!! '
CountTodayAppointmentsAsync!! 4
(!!4 5
)!!5 6
=>"" 
_context"" 
."" 
Appointments"" $
.## 

CountAsync## 
(## 
a## 
=>##  
a$$ 
.$$ 
ScheduledDate$$ #
==$$$ &
DateOnly%% 
.%% 
FromDateTime%% )
(%%) *
DateTime%%* 2
.%%2 3
Today%%3 8
)%%8 9
)%%9 :
;%%: ;
public'' 
Task'' 
<'' 
int'' 
>'' )
CountPendingAppointmentsAsync'' 6
(''6 7
)''7 8
=>(( 
_context(( 
.(( 
Appointments(( $
.)) 

CountAsync)) 
()) 
a)) 
=>))  
a** 
.** 
Status** 
==** 
AppointmentStatus**  1
.**1 2
Pending**2 9
)**9 :
;**: ;
public,, 
Task,, 
<,, 
int,, 
>,, +
CountCompletedAppointmentsAsync,, 8
(,,8 9
),,9 :
=>-- 
_context-- 
.-- 
Appointments-- $
... 

CountAsync.. 
(.. 
a.. 
=>..  
a// 
.// 
Status// 
==// 
AppointmentStatus//  1
.//1 2
	Completed//2 ;
)//; <
;//< =
public11 
Task11 
<11 
int11 
>11 #
CountHealthRecordsAsync11 0
(110 1
)111 2
=>22 
_context22 
.22 
HealthRecords22 %
.22% &

CountAsync22& 0
(220 1
)221 2
;222 3
public44 
async44 
Task44 
<44 
IEnumerable44 %
<44% &
User44& *
>44* +
>44+ ,
GetUsersAsync55 
(55 
)55 
{66 	
return77 
await77 
_context77 !
.77! "
Users77" '
.88 
OrderBy88 
(88 
u88 
=>88 
u88 
.88  
UserId88  &
)88& '
.99 
ToListAsync99 
(99 
)99 
;99 
}:: 	
public<< 
async<< 
Task<< 
<<< 
User<< 
?<< 
><<  
GetUserByIdAsync== 
(== 
int==  
id==! #
)==# $
{>> 	
return?? 
await?? 
_context?? !
.??! "
Users??" '
.@@ 
FirstOrDefaultAsync@@ $
(@@$ %
uAA 
=>AA 
uAA 
.AA 
UserIdAA !
==AA" $
idAA% '
)AA' (
;AA( )
}BB 	
}CC 
}DD ÉO
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
.ll 
	AddScopedll 
<ll 
IPatientRepositoryll -
,ll- .
PatientRepositoryll/ @
>ll@ A
(llA B
)llB C
;llC D
buildermm 
.mm 
Servicesmm 
.mm 
	AddScopedmm 
<mm 
IDoctorRepositorymm ,
,mm, -
DoctorRepositorymm. >
>mm> ?
(mm? @
)mm@ A
;mmA B
buildernn 
.nn 
Servicesnn 
.nn 
	AddScopednn 
<nn "
IAppointmentRepositorynn 1
,nn1 2!
AppointmentRepositorynn3 H
>nnH I
(nnI J
)nnJ K
;nnK L
builderoo 
.oo 
Servicesoo 
.oo 
	AddScopedoo 
<oo #
IHealthRecordRepositoryoo 2
,oo2 3"
HealthRecordRepositoryoo4 J
>ooJ K
(ooK L
)ooL M
;ooM N
builderpp 
.pp 
Servicespp 
.pp 
	AddScopedpp 
<pp 
IUserRepositorypp *
,pp* +
UserRepositorypp, :
>pp: ;
(pp; <
)pp< =
;pp= >
builderqq 
.qq 
Servicesqq 
.qq 
	AddScopedqq 
<qq 
IAdminRepositoryqq +
,qq+ ,
AdminRepositoryqq- <
>qq< =
(qq= >
)qq> ?
;qq? @
builderww 
.ww 
Servicesww 
.ww 
	AddScopedww 
<ww 
IPatientServiceww *
,ww* +
PatientServiceww, :
>ww: ;
(ww; <
)ww< =
;ww= >
builderxx 
.xx 
Servicesxx 
.xx 
	AddScopedxx 
<xx 
IDoctorServicexx )
,xx) *
DoctorServicexx+ 8
>xx8 9
(xx9 :
)xx: ;
;xx; <
builderyy 
.yy 
Servicesyy 
.yy 
	AddScopedyy 
<yy 
IAppointmentServiceyy .
,yy. /
AppointmentServiceyy0 B
>yyB C
(yyC D
)yyD E
;yyE F
builderzz 
.zz 
Serviceszz 
.zz 
	AddScopedzz 
<zz  
IHealthRecordServicezz /
,zz/ 0
HealthRecordServicezz1 D
>zzD E
(zzE F
)zzF G
;zzG H
builder{{ 
.{{ 
Services{{ 
.{{ 
	AddScoped{{ 
<{{ 
IAuthService{{ '
,{{' (
AuthService{{) 4
>{{4 5
({{5 6
){{6 7
;{{7 8
builder|| 
.|| 
Services|| 
.|| 
	AddScoped|| 
<|| 
IAdminService|| (
,||( )
AdminService||* 6
>||6 7
(||7 8
)||8 9
;||9 :
builder}} 
.}} 
Services}} 
.}} 
	AddScoped}} 
<}} 
IUserService}} '
,}}' (
UserService}}) 4
>}}4 5
(}}5 6
)}}6 7
;}}7 8
varÅÅ 
app
ÅÅ 
=
ÅÅ 	
builder
ÅÅ
 
.
ÅÅ 
Build
ÅÅ 
(
ÅÅ 
)
ÅÅ 
;
ÅÅ 
ifÖÖ 
(
ÖÖ 
app
ÖÖ 
.
ÖÖ 
Environment
ÖÖ 
.
ÖÖ 
IsDevelopment
ÖÖ !
(
ÖÖ! "
)
ÖÖ" #
)
ÖÖ# $
{ÜÜ 
app
áá 
.
áá 

UseSwagger
áá 
(
áá 
)
áá 
;
áá 
app
ââ 
.
ââ 
UseSwaggerUI
ââ 
(
ââ 
options
ââ 
=>
ââ 
{
ää 
options
ãã 
.
ãã 
SwaggerEndpoint
ãã 
(
ãã  
$str
åå &
,
åå& '
$str
çç 
)
çç  
;
çç  !
options
èè 
.
èè 
RoutePrefix
èè 
=
èè 
string
èè $
.
èè$ %
Empty
èè% *
;
èè* +
}
êê 
)
êê 
;
êê 
}ëë 
appìì 
.
ìì 
UseMiddleware
ìì 
<
ìì !
ExceptionMiddleware
ìì %
>
ìì% &
(
ìì& '
)
ìì' (
;
ìì( )
appïï 
.
ïï !
UseHttpsRedirection
ïï 
(
ïï 
)
ïï 
;
ïï 
appóó 
.
óó 
UseAuthentication
óó 
(
óó 
)
óó 
;
óó 
appôô 
.
ôô 
UseAuthorization
ôô 
(
ôô 
)
ôô 
;
ôô 
appõõ 
.
õõ 
MapControllers
õõ 
(
õõ 
)
õõ 
;
õõ 
appüü 
.
üü 
Run
üü 
(
üü 
)
üü 	
;
üü	 
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
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Patient 
{ 
[ 	
Key	 
] 
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
$str! <
)< =
]= >
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) T
)T U
]U V
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 6
)6 7
]7 8
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
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
$str <
)< =
]= >
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 5
)5 6
]6 7
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% =
)= >
]> ?
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public   
InsuranceStatus   
InsuranceStatus   .
{  / 0
get  1 4
;  4 5
set  6 9
;  9 :
}  ; <
["" 	
StringLength""	 
("" 
$num"" 
)"" 
]"" 
public## 
string## 
?## 
InsuranceNumber## &
{##' (
get##) ,
;##, -
set##. 1
;##1 2
}##3 4
public%% 
bool%% 
IsActive%% 
{%% 
get%% "
;%%" #
set%%$ '
;%%' (
}%%) *
=%%+ ,
true%%- 1
;%%1 2
public(( 
ICollection(( 
<(( 
Appointment(( &
>((& '
Appointments((( 4
{((5 6
get((7 :
;((: ;
set((< ?
;((? @
}((A B
=((C D
new((E H
List((I M
<((M N
Appointment((N Y
>((Y Z
(((Z [
)(([ \
;((\ ]
public** 
ICollection** 
<** 
HealthRecord** '
>**' (
HealthRecords**) 6
{**7 8
get**9 <
;**< =
set**> A
;**A B
}**C D
=**E F
new**G J
List**K O
<**O P
HealthRecord**P \
>**\ ]
(**] ^
)**^ _
;**_ `
}++ 
},,  
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
}$$ ¸
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
}TT ’
MC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\UserRole.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
UserRole 
{ 
Admin 
= 
$num 
, 
Doctor 
= 
$num 
, 
Patient 
= 
$num 
} 
}		 ú
MC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\TimeSlot.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
AppointmentTimeSlot #
{ 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
TenAM 
= 
$num 
, 
[		 	
Display			 
(		 
Name		 
=		 
$str		 -
)		- .
]		. /
TenThirtyAM

 
=

 
$num

 
,

 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
ElevenAM 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
ElevenThirtyAM 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
TwelvePM 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
TwelveThirtyPM 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
OnePM 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
OneThirtyPM 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str -
)- .
]. /
TwoPM 
= 
$num 
, 
[!! 	
Display!!	 
(!! 
Name!! 
=!! 
$str!! -
)!!- .
]!!. /
TwoThirtyPM"" 
="" 
$num"" 
,"" 
[$$ 	
Display$$	 
($$ 
Name$$ 
=$$ 
$str$$ -
)$$- .
]$$. /
ThreePM%% 
=%% 
$num%% 
,%% 
['' 	
Display''	 
('' 
Name'' 
='' 
$str'' -
)''- .
]''. /
ThreeThirtyPM(( 
=(( 
$num(( 
})) 
}** Ä
TC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\InsuranceStatus.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
InsuranceStatus 
{ 

NotInsured 
= 
$num 
, 
PendingVerification 
= 
$num 
,  
Active 
= 
$num 
, 
Expired 
= 
$num 
, 
	Suspended		 
=		 
$num		 
}

 
} û
KC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\Gender.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
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
= 
$num 
, 
	NonBinary 
= 
$num 
, 
PreferNotToSay 
= 
$num 
}		 
}

 ∫
YC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\DoctorSpecialization.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum  
DoctorSpecialisation $
{ 
[ 	
Display	 
( 
Name 
= 
$str .
). /
]/ 0
GeneralPractitioner 
= 
$num 
,  
[		 	
Display			 
(		 
Name		 
=		 
$str		 &
)		& '
]		' (
Cardiologist

 
=

 
$num

 
,

 
[ 	
Display	 
( 
Name 
= 
$str '
)' (
]( )
Dermatologist 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str %
)% &
]& '
Neurologist 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
Pediatrician 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
Psychiatrist 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str ,
), -
]- .
OrthopedicSurgeon 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
Gynecologist 
= 
$num 
, 
[ 	
Display	 
( 
Name 
= 
$str $
)$ %
]% &

Oncologist 
= 
$num 
, 
[!! 	
Display!!	 
(!! 
Name!! 
=!! 
$str!! )
)!!) *
]!!* +
Endocrinologist"" 
="" 
$num"" 
}## 
}$$ µ
VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Enums\AppointmentStatus.cs
	namespace 	
S3_HealthAxisApi
 
. 
Enums  
{ 
public 

enum 
AppointmentStatus !
{ 
Pending 
= 
$num 
, 
	Confirmed 
= 
$num 
, 
	Cancelled 
= 
$num 
, 
	Completed 
= 
$num 
}		 
}

 º
\C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\UpdatePatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 
UpdatePatientDto !
{ 
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
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
] 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
] 
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
EmailAddress	 
] 
public 
string 
? 
Email 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
? 
InsuranceNumber &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} ﬂ
]C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\PatientSummaryDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 
PatientSummaryDto "
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
;; <
} 
}		 É
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\PatientSearchResultDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class "
PatientSearchResultDto '
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
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
}		 Ô
VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\PatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 

PatientDto 
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public		 
DateOnly		 
DateOfBirth		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		0 1
public

 
Gender

 
Gender

 
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
) *
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
string 
? 
Email 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ´
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\DeactivatePatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class "
UpdatePatientStatusDto '
{ 
[ 	
Required	 
] 
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
}		 
}

 º
\C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Patient\CreatePatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Patient  '
{ 
public 

class 
CreatePatientDto !
{ 
[ 	
Required	 
] 
[		 	
StringLength			 
(		 
$num		 
)		 
]		 
public

 
string

 
FullName

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
=

- .
string

/ 5
.

5 6
Empty

6 ;
;

; <
[ 	
Required	 
] 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
] 
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
EmailAddress	 
] 
public 
string 
? 
Email 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
? 
InsuranceNumber &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} Í
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\HealthRecord\UpdateHealthRecordDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
HealthRecord  ,
{ 
public 

class !
UpdateHealthRecordDto &
{ 
public 
string 
? 
	Diagnosis  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
? 
Prescription #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
} 
}		 Œ
`C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\HealthRecord\HealthRecordDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
HealthRecord  ,
{ 
public 

class 
HealthRecordDto  
{ 
public 
int 
HealthRecordId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public		 
string		 
?		 
	Diagnosis		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
public

 
string

 
?

 
Prescription

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

0 1
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ª

fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\HealthRecord\CreateHealthRecordDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
HealthRecord  ,
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
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
? 
	Diagnosis  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
string		 
?		 
Prescription		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		0 1
public

 
string

 
?

 
Notes

 
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
} 
} §
ZC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\UpdateDoctorDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
{ 
public 

class 
UpdateDoctorDto  
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
Specialisation !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
}		 
}

 ∏
TC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\DoctorDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
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
public 
int 
Specialisation !
{" #
get$ '
;' (
set) ,
;, -
}. /
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
} Œ	
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\DoctorCreationResultDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
{ 
public 

class #
DoctorCreationResultDto (
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
string 
TemporaryPassword '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
=6 7
string8 >
.> ?
Empty? D
;D E
} 
} ë

ZC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Doctor\CreateDoctorDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Doctor  &
{ 
public 

class 
CreateDoctorDto  
{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public		 
int		 
Specialisation		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} ø

PC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\UserDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
UserDto 
{ 
public 
int 
UserId 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
int 
Role 
{ 
get 
; 
set "
;" #
}$ %
public 
int 
? 
	PatientId 
{ 
get  #
;# $
set% (
;( )
}* +
public		 
int		 
?		 
DoctorId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
bool

 
IsActive

 
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
} 
} ≤
[C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\RegisterPatientDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
RegisterPatientDto #
{ 
[ 	
Required	 
] 
[		 	
StringLength			 
(		 
$num		 
)		 
]		 
public

 
string

 
FullName

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
=

- .
string

/ 5
.

5 6
Empty

6 ;
;

; <
[ 	
Required	 
] 
public 
DateOnly 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
] 
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Phone	 
] 
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
] 
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
public   
string   
?   
InsuranceNumber   &
{  ' (
get  ) ,
;  , -
set  . 1
;  1 2
}  3 4
}!! 
}"" ‰
TC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\RegisterDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
RegisterDto 
{ 
[ 	
Required	 
] 
[		 	
EmailAddress			 
]		 
public

 
string

 
Email

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
=

* +
string

, 2
.

2 3
Empty

3 8
;

8 9
[ 	
Required	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
] 
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
[ 	
Required	 
] 
public 
UserRole 
Role 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
? 
ReferenceId 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ö
QC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\LoginDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
LoginDto 
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
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
} 
} ø
XC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\RefreshTokenDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
RefreshTokenDto  
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
} ä

VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\CreateUserDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
CreateUserDto 
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
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
Role 
{ 
get 
; 
set "
;" #
}$ %
public 
int 
? 
	PatientId 
{ 
get  #
;# $
set% (
;( )
}* +
public		 
int		 
?		 
DoctorId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
}

 
} ã

XC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Auth\AuthResponseDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Auth  $
{ 
public 

class 
AuthResponseDto  
{ 
public 
string 
AccessToken !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
string 
RefreshToken "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
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
;7 8
} 
} ª
jC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\UpdateAppointmentStatusDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class &
UpdateAppointmentStatusDto +
{ 
public 
int 
Status 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
}		 ∏
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\UpdateAppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class  
UpdateAppointmentDto %
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
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
} 
}

 Ô

lC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\PatientAppointmentHistoryDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class (
PatientAppointmentHistoryDto -
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
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public		 
string		 

DoctorName		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
=		/ 0
string		1 7
.		7 8
Empty		8 =
;		= >
public

 
int

 
Status

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
} 
} „

eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\DoctorScheduleItemDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class !
DoctorScheduleItemDto &
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
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
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
public

 
int

 
Status

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
} 
} “
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\CreateAppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class  
CreateAppointmentDto %
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
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
}		 
} ›
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\CancelAppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class  
CancelAppointmentDto %
{ 
public 
string 
CancellationReason (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
=7 8
string9 ?
.? @
Empty@ E
;E F
} 
} ∞
^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\AppointmentDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
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
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public		 
int		 
TimeSlot		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public

 
int

 
Status

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} £
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Appointment\AppointmentDetailsDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Appointment  +
{ 
public 

class !
AppointmentDetailsDto &
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
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PatientName !
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
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public		 
string		 

DoctorName		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
=		/ 0
string		1 7
.		7 8
Empty		8 =
;		= >
public

 
DateOnly

 
ScheduledDate

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
Status 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} Ï
[C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Admin\UserManagementDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Admin  %
{ 
public 

class 
UserManagementDto "
{ 
public 
int 
UserId 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public		 
string		 
Role		 
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
public 
int 
? 
ReferenceId 
{  !
get" %
;% &
set' *
;* +
}, -
} 
} ø
\C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Admin\AdminStatisticsDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Admin  %
{ 
public 

class 
AdminStatisticsDto #
{ 
public 
int 
Patients 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
Doctors 
{ 
get  
;  !
set" %
;% &
}' (
public		 
int		 
Appointments		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public 
int 
HealthRecords  
{! "
get# &
;& '
set( +
;+ ,
}- .
} 
} µ
[C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\DTOs\Admin\AdminDashboardDto.cs
	namespace 	
S3_HealthAxisApi
 
. 
DTOs 
.  
Admin  %
{ 
public 

class 
AdminDashboardDto "
{ 
public 
int 
TotalPatients  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
ActivePatients !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
int		 
TotalDoctors		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public 
int 
ActiveDoctors  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
TodayAppointments $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
int 
PendingAppointments &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
int !
CompletedAppointments (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} ·]
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
public 
DbSet 
< 
User 
> 
Users  
=>! #
Set$ '
<' (
User( ,
>, -
(- .
). /
;/ 0
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
modelBuilder 
. 
Entity 
<  
Appointment  +
>+ ,
(, -
)- .
. 
HasOne 
( 
a 
=> 
a 
. 
Patient &
)& '
. 
WithMany 
( 
p 
=> 
p  
.  !
Appointments! -
)- .
. 
HasForeignKey 
( 
a  
=>! #
a$ %
.% &
	PatientId& /
)/ 0
.   
OnDelete   
(   
DeleteBehavior   (
.  ( )
Restrict  ) 1
)  1 2
;  2 3
modelBuilder"" 
."" 
Entity"" 
<""  
Appointment""  +
>""+ ,
("", -
)""- .
.## 
HasOne## 
(## 
a## 
=>## 
a## 
.## 
Doctor## %
)##% &
.$$ 
WithMany$$ 
($$ 
d$$ 
=>$$ 
d$$  
.$$  !
Appointments$$! -
)$$- .
.%% 
HasForeignKey%% 
(%% 
a%%  
=>%%! #
a%%$ %
.%%% &
DoctorId%%& .
)%%. /
.&& 
OnDelete&& 
(&& 
DeleteBehavior&& (
.&&( )
Restrict&&) 1
)&&1 2
;&&2 3
modelBuilder)) 
.)) 
Entity)) 
<))  
Appointment))  +
>))+ ,
()), -
)))- .
.** 
HasIndex** 
(** 
a** 
=>** 
new** "
{++ 
a,, 
.,, 
DoctorId,, 
,,, 
a-- 
.-- 
ScheduledDate-- #
,--# $
a.. 
... 
TimeSlot.. 
}// 
)// 
.00 
IsUnique00 
(00 
)00 
;00 
modelBuilder55 
.55 
Entity55 
<55  
HealthRecord55  ,
>55, -
(55- .
)55. /
.66 
HasOne66 
(66 
hr66 
=>66 
hr66  
.66  !
Appointment66! ,
)66, -
.77 
WithOne77 
(77 
a77 
=>77 
a77 
.77  
HealthRecord77  ,
)77, -
.88 
HasForeignKey88 
<88 
HealthRecord88 +
>88+ ,
(88, -
hr88- /
=>880 2
hr883 5
.885 6
AppointmentId886 C
)88C D
.99 
OnDelete99 
(99 
DeleteBehavior99 (
.99( )
Restrict99) 1
)991 2
;992 3
modelBuilder;; 
.;; 
Entity;; 
<;;  
HealthRecord;;  ,
>;;, -
(;;- .
);;. /
.<< 
HasOne<< 
(<< 
hr<< 
=><< 
hr<<  
.<<  !
Patient<<! (
)<<( )
.== 
WithMany== 
(== 
p== 
=>== 
p==  
.==  !
HealthRecords==! .
)==. /
.>> 
HasForeignKey>> 
(>> 
hr>> !
=>>>" $
hr>>% '
.>>' (
	PatientId>>( 1
)>>1 2
.?? 
OnDelete?? 
(?? 
DeleteBehavior?? (
.??( )
Restrict??) 1
)??1 2
;??2 3
modelBuilderAA 
.AA 
EntityAA 
<AA  
HealthRecordAA  ,
>AA, -
(AA- .
)AA. /
.BB 
HasOneBB 
(BB 
hrBB 
=>BB 
hrBB  
.BB  !
DoctorBB! '
)BB' (
.CC 
WithManyCC 
(CC 
dCC 
=>CC 
dCC  
.CC  !
HealthRecordsCC! .
)CC. /
.DD 
HasForeignKeyDD 
(DD 
hrDD !
=>DD" $
hrDD% '
.DD' (
DoctorIdDD( 0
)DD0 1
.EE 
OnDeleteEE 
(EE 
DeleteBehaviorEE (
.EE( )
RestrictEE) 1
)EE1 2
;EE2 3
modelBuilderHH 
.HH 
EntityHH 
<HH  
HealthRecordHH  ,
>HH, -
(HH- .
)HH. /
.II 
HasIndexII 
(II 
hrII 
=>II 
hrII  "
.II" #
AppointmentIdII# 0
)II0 1
.JJ 
IsUniqueJJ 
(JJ 
)JJ 
;JJ 
modelBuilderNN 
.NN 
EntityNN 
<NN  
DoctorNN  &
>NN& '
(NN' (
)NN( )
.OO 
PropertyOO 
(OO 
dOO 
=>OO 
dOO  
.OO  !
ConsultationFeeOO! 0
)OO0 1
.PP 
HasPrecisionPP 
(PP 
$numPP  
,PP  !
$numPP" #
)PP# $
;PP$ %
modelBuilderTT 
.TT 
EntityTT 
<TT  
PatientTT  '
>TT' (
(TT( )
)TT) *
.TT* +
HasDataTT+ 2
(TT2 3
newUU 
PatientUU 
{VV 
	PatientIdWW 
=WW 
$numWW  !
,WW! "
FullNameXX 
=XX 
$strXX -
,XX- .
DateOfBirthYY 
=YY  !
newYY" %
DateOnlyYY& .
(YY. /
$numYY/ 3
,YY3 4
$numYY5 6
,YY6 7
$numYY8 :
)YY: ;
,YY; <
GenderZZ 
=ZZ 
GenderZZ #
.ZZ# $
MaleZZ$ (
,ZZ( )
PhoneNumber[[ 
=[[  !
$str[[" .
,[[. /
Email\\ 
=\\ 
$str\\ /
,\\/ 0
InsuranceStatus]] #
=]]$ %
InsuranceStatus]]& 5
.]]5 6
Active]]6 <
,]]< =
InsuranceNumber^^ #
=^^$ %
$str^^& /
,^^/ 0
IsActive__ 
=__ 
true__ #
}`` 
,`` 
newaa 
Patientaa 
{bb 
	PatientIdcc 
=cc 
$numcc  !
,cc! "
FullNamedd 
=dd 
$strdd ,
,dd, -
DateOfBirthee 
=ee  !
newee" %
DateOnlyee& .
(ee. /
$numee/ 3
,ee3 4
$numee5 7
,ee7 8
$numee9 :
)ee: ;
,ee; <
Genderff 
=ff 
Genderff #
.ff# $
Femaleff$ *
,ff* +
PhoneNumbergg 
=gg  !
$strgg" .
,gg. /
Emailhh 
=hh 
$strhh .
,hh. /
InsuranceStatusii #
=ii$ %
InsuranceStatusii& 5
.ii5 6
Activeii6 <
,ii< =
InsuranceNumberjj #
=jj$ %
$strjj& /
,jj/ 0
IsActivekk 
=kk 
truekk #
}ll 
)mm 
;mm 
modelBuilderqq 
.qq 
Entityqq 
<qq  
Doctorqq  &
>qq& '
(qq' (
)qq( )
.qq) *
HasDataqq* 1
(qq1 2
newrr 
Doctorrr 
{ss 
DoctorIdtt 
=tt 
$numtt  
,tt  !
FullNameuu 
=uu 
$struu *
,uu* +
Specialisationvv "
=vv# $ 
DoctorSpecialisationvv% 9
.vv9 :
GeneralPractitionervv: M
,vvM N
YearsOfExperienceww %
=ww& '
$numww( )
,ww) *
ConsultationFeexx #
=xx$ %
$numxx& -
,xx- .
IsActiveyy 
=yy 
trueyy #
}zz 
,zz 
new{{ 
Doctor{{ 
{|| 
DoctorId}} 
=}} 
$num}}  
,}}  !
FullName~~ 
=~~ 
$str~~ ,
,~~, -
Specialisation "
=# $ 
DoctorSpecialisation% 9
.9 :
Cardiologist: F
,F G
YearsOfExperience
ÄÄ %
=
ÄÄ& '
$num
ÄÄ( *
,
ÄÄ* +
ConsultationFee
ÅÅ #
=
ÅÅ$ %
$num
ÅÅ& .
,
ÅÅ. /
IsActive
ÇÇ 
=
ÇÇ 
true
ÇÇ #
}
ÉÉ 
)
ÑÑ 
;
ÑÑ 
modelBuilder
ää 
.
ää 
Entity
ää 
<
ää  
Appointment
ää  +
>
ää+ ,
(
ää, -
)
ää- .
.
ää. /
HasData
ää/ 6
(
ää6 7
new
ãã 
Appointment
ãã 
{
åå 
AppointmentId
çç !
=
çç" #
$num
çç$ %
,
çç% &
	PatientId
éé 
=
éé 
$num
éé  !
,
éé! "
DoctorId
èè 
=
èè 
$num
èè  
,
èè  !
ScheduledDate
êê !
=
êê" #
new
êê$ '
DateOnly
êê( 0
(
êê0 1
$num
êê1 5
,
êê5 6
$num
êê7 8
,
êê8 9
$num
êê: <
)
êê< =
,
êê= >
TimeSlot
ëë 
=
ëë !
AppointmentTimeSlot
ëë 2
.
ëë2 3
TenAM
ëë3 8
,
ëë8 9
Status
íí 
=
íí 
AppointmentStatus
íí .
.
íí. /
Pending
íí/ 6
}
ìì 
)
îî 
;
îî 
}
ïï 	
}
ññ 
}óó ß>
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
}JJ Òv
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
< 
IActionResult '
>' (
GetAll) /
(/ 0
)0 1
{ 	
var 
appointments 
= 
await 
_appointmentService )
.) *
GetAllAsync* 5
(5 6
)6 7
;7 8
return 
Ok 
( 
appointments "
)" #
;# $
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (
GetById  ) 0
(  0 1
int  1 4
id  5 7
)  7 8
{!! 	
var"" 
appointment"" 
="" 
await## 
_appointmentService## )
.##) *
GetByIdAsync##* 6
(##6 7
id##7 9
)##9 :
;##: ;
if%% 
(%% 
appointment%% 
==%% 
null%% #
)%%# $
return&& 
NotFound&& 
(&&  
$"'' 
$str'' "
{''" #
id''# %
}''% &
$str''& 1
"''1 2
)''2 3
;''3 4
return)) 
Ok)) 
()) 
appointment)) !
)))! "
;))" #
}** 	
[,, 	
HttpGet,,	 
(,, 
$str,, *
),,* +
],,+ ,
public-- 
async-- 
Task-- 
<-- 
IActionResult-- '
>--' (
GetPatientHistory--) :
(--: ;
int.. 
	patientId.. 
).. 
{// 	
var00 
appointments00 
=00 
await11 
_appointmentService11 )
.22 "
GetPatientHistoryAsync22 +
(22+ ,
	patientId22, 5
)225 6
;226 7
return44 
Ok44 
(44 
appointments44 "
)44" #
;44# $
}55 	
[77 	
HttpGet77	 
(77 
$str77 .
)77. /
]77/ 0
[88 	
	Authorize88	 
(88 
Roles88 
=88 
$str88 )
)88) *
]88* +
public99 
async99 
Task99 
<99 
IActionResult99 '
>99' ("
GetDoctorTodaySchedule99) ?
(99? @
int:: 
doctorId:: 
):: 
{;; 	
var<< 
schedule<< 
=<< 
await== 
_appointmentService== )
.>> '
GetDoctorTodayScheduleAsync>> 0
(>>0 1
doctorId>>1 9
)>>9 :
;>>: ;
return@@ 
Ok@@ 
(@@ 
schedule@@ 
)@@ 
;@@  
}AA 	
[CC 	
HttpGetCC	 
(CC 
$strCC -
)CC- .
]CC. /
[DD 	
	AuthorizeDD	 
(DD 
RolesDD 
=DD 
$strDD )
)DD) *
]DD* +
publicEE 
asyncEE 
TaskEE 
<EE 
IActionResultEE '
>EE' (!
GetDoctorWeekScheduleEE) >
(EE> ?
intFF 
doctorIdFF 
,FF 
[GG 
	FromQueryGG 
]GG 
DateOnlyGG  
	startDateGG! *
,GG* +
[HH 
	FromQueryHH 
]HH 
DateOnlyHH  
endDateHH! (
)HH( )
{II 	
varJJ 
scheduleJJ 
=JJ 
awaitKK 
_appointmentServiceKK )
.LL &
GetDoctorWeekScheduleAsyncLL /
(LL/ 0
doctorIdMM  
,MM  !
	startDateNN !
,NN! "
endDateOO 
)OO  
;OO  !
returnQQ 
OkQQ 
(QQ 
scheduleQQ 
)QQ 
;QQ  
}RR 	
[TT 	
HttpGetTT	 
(TT 
$strTT -
)TT- .
]TT. /
[UU 	
	AuthorizeUU	 
(UU 
RolesUU 
=UU 
$strUU )
)UU) *
]UU* +
publicVV 
asyncVV 
TaskVV 
<VV 
IActionResultVV '
>VV' (%
GetDoctorUpcomingScheduleVV) B
(VVB C
intVVC F
doctorIdVVG O
)VVO P
{WW 	
varXX 
resultXX 
=XX 
awaitYY 
_appointmentServiceYY )
.ZZ *
GetDoctorUpcomingScheduleAsyncZZ 3
(ZZ3 4
doctorId[[  
)[[  !
;[[! "
return]] 
Ok]] 
(]] 
result]] 
)]] 
;]] 
}^^ 	
[`` 	
HttpPost``	 
]`` 
[aa 	
	Authorizeaa	 
(aa 
Rolesaa 
=aa 
$straa *
)aa* +
]aa+ ,
publicbb 
asyncbb 
Taskbb 
<bb 
IActionResultbb '
>bb' (
Createbb) /
(bb/ 0
[cc 
FromBodycc 
]cc  
CreateAppointmentDtocc +
dtocc, /
)cc/ 0
{dd 	
tryee 
{ff 
vargg 
appointmentgg 
=gg  !
awaithh 
_appointmentServicehh -
.hh- .
CreateAsynchh. 9
(hh9 :
dtohh: =
)hh= >
;hh> ?
returnjj 
CreatedAtActionjj &
(jj& '
nameofkk 
(kk 
GetByIdkk "
)kk" #
,kk# $
newll 
{ll 
idll 
=ll 
appointmentll *
.ll* +
AppointmentIdll+ 8
}ll9 :
,ll: ;
appointmentmm 
)mm  
;mm  !
}nn 
catchoo 
(oo 
ArgumentExceptionpp !
expp" $
)pp$ %
{qq 
returnrr 

BadRequestrr !
(rr! "
exrr" $
.rr$ %
Messagerr% ,
)rr, -
;rr- .
}ss 
catchtt 
(tt %
InvalidOperationExceptionuu )
exuu* ,
)uu, -
{vv 
returnww 

BadRequestww !
(ww! "
exww" $
.ww$ %
Messageww% ,
)ww, -
;ww- .
}xx 
catchyy 
(yy  
KeyNotFoundExceptionzz $
exzz% '
)zz' (
{{{ 
return|| 
NotFound|| 
(||  
ex||  "
.||" #
Message||# *
)||* +
;||+ ,
}}} 
}~~ 	
[
ÄÄ 	
HttpPut
ÄÄ	 
(
ÄÄ 
$str
ÄÄ 
)
ÄÄ 
]
ÄÄ 
[
ÅÅ 	
	Authorize
ÅÅ	 
(
ÅÅ 
Roles
ÅÅ 
=
ÅÅ 
$str
ÅÅ *
)
ÅÅ* +
]
ÅÅ+ ,
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
IActionResult
ÇÇ '
>
ÇÇ' (
Update
ÇÇ) /
(
ÇÇ/ 0
int
ÉÉ 
id
ÉÉ 
,
ÉÉ 
[
ÑÑ 
FromBody
ÑÑ 
]
ÑÑ "
UpdateAppointmentDto
ÑÑ +
dto
ÑÑ, /
)
ÑÑ/ 0
{
ÖÖ 	
try
ÜÜ 
{
áá 
await
àà !
_appointmentService
àà )
.
ââ 
UpdateAsync
ââ  
(
ââ  !
id
ââ! #
,
ââ# $
dto
ââ% (
)
ââ( )
;
ââ) *
return
ãã 
	NoContent
ãã  
(
ãã  !
)
ãã! "
;
ãã" #
}
åå 
catch
çç 
(
çç "
KeyNotFoundException
éé $
ex
éé% '
)
éé' (
{
èè 
return
êê 
NotFound
êê 
(
êê  
ex
êê  "
.
êê" #
Message
êê# *
)
êê* +
;
êê+ ,
}
ëë 
catch
íí 
(
íí 
ArgumentException
ìì !
ex
ìì" $
)
ìì$ %
{
îî 
return
ïï 

BadRequest
ïï !
(
ïï! "
ex
ïï" $
.
ïï$ %
Message
ïï% ,
)
ïï, -
;
ïï- .
}
ññ 
catch
óó 
(
óó '
InvalidOperationException
òò )
ex
òò* ,
)
òò, -
{
ôô 
return
öö 

BadRequest
öö !
(
öö! "
ex
öö" $
.
öö$ %
Message
öö% ,
)
öö, -
;
öö- .
}
õõ 
}
úú 	
[
ûû 	
HttpPut
ûû	 
(
ûû 
$str
ûû #
)
ûû# $
]
ûû$ %
[
üü 	
	Authorize
üü	 
(
üü 
Roles
üü 
=
üü 
$str
üü )
)
üü) *
]
üü* +
public
†† 
async
†† 
Task
†† 
<
†† 
IActionResult
†† '
>
††' (
Confirm
††) 0
(
††0 1
int
††1 4
id
††5 7
)
††7 8
{
°° 	
try
¢¢ 
{
££ 
await
§§ !
_appointmentService
§§ )
.
•• 
ConfirmAsync
•• !
(
••! "
id
••" $
)
••$ %
;
••% &
return
ßß 
	NoContent
ßß  
(
ßß  !
)
ßß! "
;
ßß" #
}
®® 
catch
©© 
(
©© "
KeyNotFoundException
™™ $
)
™™$ %
{
´´ 
return
¨¨ 
NotFound
¨¨ 
(
¨¨  
)
¨¨  !
;
¨¨! "
}
≠≠ 
catch
ÆÆ 
(
ÆÆ '
InvalidOperationException
ØØ )
ex
ØØ* ,
)
ØØ, -
{
∞∞ 
return
±± 

BadRequest
±± !
(
±±! "
ex
±±" $
.
±±$ %
Message
±±% ,
)
±±, -
;
±±- .
}
≤≤ 
}
≥≥ 	
[
µµ 	
HttpPut
µµ	 
(
µµ 
$str
µµ $
)
µµ$ %
]
µµ% &
[
∂∂ 	
	Authorize
∂∂	 
(
∂∂ 
Roles
∂∂ 
=
∂∂ 
$str
∂∂ )
)
∂∂) *
]
∂∂* +
public
∑∑ 
async
∑∑ 
Task
∑∑ 
<
∑∑ 
IActionResult
∑∑ '
>
∑∑' (
Complete
∑∑) 1
(
∑∑1 2
int
∑∑2 5
id
∑∑6 8
)
∑∑8 9
{
∏∏ 	
try
ππ 
{
∫∫ 
await
ªª !
_appointmentService
ªª )
.
ºº 
CompleteAsync
ºº "
(
ºº" #
id
ºº# %
)
ºº% &
;
ºº& '
return
ææ 
	NoContent
ææ  
(
ææ  !
)
ææ! "
;
ææ" #
}
øø 
catch
¿¿ 
(
¿¿ "
KeyNotFoundException
¡¡ $
)
¡¡$ %
{
¬¬ 
return
√√ 
NotFound
√√ 
(
√√  
)
√√  !
;
√√! "
}
ƒƒ 
catch
≈≈ 
(
≈≈ '
InvalidOperationException
∆∆ )
ex
∆∆* ,
)
∆∆, -
{
«« 
return
»» 

BadRequest
»» !
(
»»! "
ex
»»" $
.
»»$ %
Message
»»% ,
)
»», -
;
»»- .
}
…… 
}
   	
[
ÃÃ 	
HttpPut
ÃÃ	 
(
ÃÃ 
$str
ÃÃ 
)
ÃÃ 
]
ÃÃ  
[
ÕÕ 	
	Authorize
ÕÕ	 
]
ÕÕ 
public
ŒŒ 
async
ŒŒ 
Task
ŒŒ 
<
ŒŒ 
IActionResult
ŒŒ '
>
ŒŒ' (
UpdateStatus
ŒŒ) 5
(
ŒŒ5 6
int
ŒŒ6 9
id
ŒŒ: <
,
ŒŒ< =(
UpdateAppointmentStatusDto
ŒŒ> X
dto
ŒŒY \
)
ŒŒ\ ]
{
œœ 	
await
–– !
_appointmentService
–– %
.
—— 
UpdateStatusAsync
—— "
(
——" #
id
——# %
,
——% &
dto
——' *
)
——* +
;
——+ ,
return
”” 
	NoContent
”” 
(
”” 
)
”” 
;
”” 
}
‘‘ 	
[
÷÷ 	
HttpPut
÷÷	 
(
÷÷ 
$str
÷÷ "
)
÷÷" #
]
÷÷# $
[
◊◊ 	
	Authorize
◊◊	 
(
◊◊ 
Roles
◊◊ 
=
◊◊ 
$str
◊◊ *
)
◊◊* +
]
◊◊+ ,
public
ÿÿ 
async
ÿÿ 
Task
ÿÿ 
<
ÿÿ 
IActionResult
ÿÿ '
>
ÿÿ' (
Cancel
ÿÿ) /
(
ÿÿ/ 0
int
ŸŸ 
id
ŸŸ 
,
ŸŸ 
[
⁄⁄ 
FromBody
⁄⁄ 
]
⁄⁄ "
CancelAppointmentDto
⁄⁄ +
dto
⁄⁄, /
)
⁄⁄/ 0
{
€€ 	
try
‹‹ 
{
›› 
await
ﬁﬁ !
_appointmentService
ﬁﬁ )
.
ﬂﬂ 
CancelAsync
ﬂﬂ  
(
ﬂﬂ  !
id
ﬂﬂ! #
,
ﬂﬂ# $
dto
ﬂﬂ% (
)
ﬂﬂ( )
;
ﬂﬂ) *
return
·· 
	NoContent
··  
(
··  !
)
··! "
;
··" #
}
‚‚ 
catch
„„ 
(
„„ "
KeyNotFoundException
‰‰ $
)
‰‰$ %
{
ÂÂ 
return
ÊÊ 
NotFound
ÊÊ 
(
ÊÊ  
)
ÊÊ  !
;
ÊÊ! "
}
ÁÁ 
catch
ËË 
(
ËË 
ArgumentException
ÈÈ !
ex
ÈÈ" $
)
ÈÈ$ %
{
ÍÍ 
return
ÎÎ 

BadRequest
ÎÎ !
(
ÎÎ! "
ex
ÎÎ" $
.
ÎÎ$ %
Message
ÎÎ% ,
)
ÎÎ, -
;
ÎÎ- .
}
ÏÏ 
catch
ÌÌ 
(
ÌÌ '
InvalidOperationException
ÓÓ )
ex
ÓÓ* ,
)
ÓÓ, -
{
ÔÔ 
return
 

BadRequest
 !
(
! "
ex
" $
.
$ %
Message
% ,
)
, -
;
- .
}
ÒÒ 
}
ÚÚ 	
}
ÛÛ 
}ÙÙ Ì
ZC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\AdminController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[ 
	Authorize 
( 
Roles 
= 
$str 
) 
]  
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
]		 
public

 

class

 
AdminController

  
:

! "
ControllerBase

# 1
{ 
private 
readonly 
IAdminService &
_adminService' 4
;4 5
public 
AdminController 
( 
IAdminService 
adminService &
)& '
{ 	
_adminService 
= 
adminService (
;( )
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetDashboard 
( 
) 
{ 	
return 
Ok 
( 
await 
_adminService #
. 
GetDashboardAsync &
(& '
)' (
)( )
;) *
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetStatistics 
( 
) 
{   	
return!! 
Ok!! 
(!! 
await"" 
_adminService"" #
.## 
GetStatisticsAsync## '
(##' (
)##( )
)##) *
;##* +
}$$ 	
[&& 	
HttpGet&&	 
(&& 
$str&& 
)&& 
]&& 
public'' 
async'' 
Task'' 
<'' 
IActionResult'' '
>''' (
GetUsers(( 
((( 
)(( 
{)) 	
return** 
Ok** 
(** 
await++ 
_adminService++ #
.,, 
GetUsersAsync,, "
(,," #
),,# $
),,$ %
;,,% &
}-- 	
[// 	
HttpGet//	 
(// 
$str// 
)// 
]// 
public00 
async00 
Task00 
<00 
IActionResult00 '
>00' (
GetUser11 
(11 
int11 
id11 
)11 
{22 	
var33 
user33 
=33 
await44 
_adminService44 #
.55 
GetUserByIdAsync55 %
(55% &
id55& (
)55( )
;55) *
if77 
(77 
user77 
==77 
null77 
)77 
return88 
NotFound88 
(88  
)88  !
;88! "
return:: 
Ok:: 
(:: 
user:: 
):: 
;:: 
};; 	
}<< 
}== 