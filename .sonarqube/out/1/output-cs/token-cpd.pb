˘	
aC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IUserService.cs
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
} —
dC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IPatientService.cs
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
} â
iC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IHealthRecordService.cs
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
< 
IEnumerable 
< 
HealthRecordDto (
>( )
>) *
GetByPatientIdAsync+ >
(> ?
int? B
	patientIdC L
)L M
;M N
Task 
< 
HealthRecordDto 
> 
CreateAsync )
() *!
CreateHealthRecordDto* ?
dto@ C
)C D
;D E
Task 
UpdateAsync 
( 
int 
id 
,  !
UpdateHealthRecordDto! 6
dto7 :
): ;
;; <
} 
} “
cC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IDoctorService.cs
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
} ì
aC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IAuthService.cs
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
Task		 
<		 
(		 
bool		 
Success		 
,		 
string		 "
Message		# *
,		* +
AuthResponseDto		, ;
?		; <
Data		= A
)		A B
>		B C 
RegisterPatientAsync		D X
(		X Y
RegisterPatientDto		Y k
request		l s
)		s t
;		t u
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
AuthResponseDto, ;
?; <
Data= A
)A B
>B C

LoginAsyncD N
(N O
LoginDtoO W
requestX _
)_ `
;` a
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
AuthResponseDto, ;
?; <
Data= A
)A B
>B C
RefreshTokenAsyncD U
(U V
RefreshTokenDtoV e
requestf m
)m n
;n o
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
)* +
>+ ,
ChangePasswordAsync- @
(@ A
stringA G
emailH M
,M N
ChangePasswordDtoO `
requesta h
)h i
;i j
} 
} ë
hC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IAppointmentService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
	Interface$ -
{ 
public 

	interface 
IAppointmentService (
{ 
Task 
< 
IEnumerable 
< !
AppointmentDetailsDto .
>. /
>/ 0
GetAllAsync1 <
(< =
)= >
;> ?
Task

 
<

 !
AppointmentDetailsDto

 "
?

" #
>

# $
GetByIdAsync

% 1
(

1 2
int

2 5
id

6 8
)

8 9
;

9 :
Task 
< 
IEnumerable 
< (
PatientAppointmentHistoryDto 5
>5 6
>6 7"
GetPatientHistoryAsync8 N
(N O
intO R
	patientIdS \
)\ ]
;] ^
Task 
< 
IEnumerable 
< !
DoctorScheduleItemDto .
>. /
>/ 0'
GetDoctorTodayScheduleAsync1 L
(L M
intM P
doctorIdQ Y
)Y Z
;Z [
Task 
< 
IEnumerable 
< !
DoctorScheduleItemDto .
>. /
>/ 0&
GetDoctorWeekScheduleAsync1 K
(K L
int 
doctorId 
, 
DateOnly 
	startDate 
, 
DateOnly 
endDate 
) 
; 
Task 
< 
AppointmentDto 
> 
CreateAsync (
(( ) 
CreateAppointmentDto) =
dto> A
)A B
;B C
Task 
< 
IEnumerable 
< !
DoctorScheduleItemDto .
>. /
>/ 0*
GetDoctorUpcomingScheduleAsync1 O
(O P
intP S
doctorIdT \
)\ ]
;] ^
Task 
< 
IEnumerable 
< 
DoctorPatientDto )
>) *
>* +"
GetDoctorPatientsAsync, B
(B C
intC F
doctorIdG O
)O P
;P Q
Task 
UpdateAsync 
( 
int 
id 
,   
UpdateAppointmentDto! 5
dto6 9
)9 :
;: ;
Task 
UpdateStatusAsync 
( 
int "
id# %
,% &&
UpdateAppointmentStatusDto' A
dtoB E
)E F
;F G
Task 
ConfirmAsync 
( 
int 
id  
)  !
;! "
Task 
CompleteAsync 
( 
int 
id !
)! "
;" #
Task!! 
CancelAsync!! 
(!! 
int!! 
id!! 
,!!   
CancelAppointmentDto!!! 5
dto!!6 9
)!!9 :
;!!: ;
}"" 
}## ∞
bC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Interface\IAdminService.cs
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
} ´
eC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\UserService.cs
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
}:: ±r
hC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\PatientService.cs
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
}ùù ”É
mC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\HealthRecordService.cs
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
<// 
IEnumerable// %
<//% &
HealthRecordDto//& 5
>//5 6
>//6 7
GetByPatientIdAsync//8 K
(//K L
int//L O
	patientId//P Y
)//Y Z
{00 	
var11 
patient11 
=11 
await11 
_patientRepository11  2
.112 3
GetByIdAsync113 ?
(11? @
	patientId11@ I
)11I J
;11J K
if33 
(33 
patient33 
==33 
null33 
)33  
{44 
throw55 
new55  
KeyNotFoundException55 .
(55. /
$"66 
$str66 &
{66& '
	patientId66' 0
}660 1
$str661 <
"66< =
)66= >
;66> ?
}77 
var99 
records99 
=99 
await:: #
_healthRecordRepository:: -
.::- .
GetByPatientIdAsync::. A
(::A B
	patientId::B K
)::K L
;::L M
return<< 
records<< 
.<< 
Select<< !
(<<! "
MapToDto<<" *
)<<* +
;<<+ ,
}== 	
public?? 
async?? 
Task?? 
<?? 
HealthRecordDto?? )
>??) *
CreateAsync??+ 6
(??6 7!
CreateHealthRecordDto??7 L
dto??M P
)??P Q
{@@ 	
ValidateCreateDtoAA 
(AA 
dtoAA !
)AA! "
;AA" #
varCC 
appointmentCC 
=CC 
awaitDD "
_appointmentRepositoryDD ,
.DD, -
GetByIdAsyncDD- 9
(DD9 :
dtoDD: =
.DD= >
AppointmentIdDD> K
)DDK L
;DDL M
ifFF 
(FF 
appointmentFF 
==FF 
nullFF #
)FF# $
{GG 
throwHH 
newHH  
KeyNotFoundExceptionHH .
(HH. /
$strHH/ G
)HHG H
;HHH I
}II 
ifKK 
(KK 
appointmentKK 
.KK 
StatusKK "
!=KK# %
AppointmentStatusKK& 7
.KK7 8
	CompletedKK8 A
)KKA B
{LL 
throwMM 
newMM %
InvalidOperationExceptionMM 3
(MM3 4
$strNN S
)NNS T
;NNT U
}OO 
varQQ 
existingRecordQQ 
=QQ  
awaitRR #
_healthRecordRepositoryRR -
.RR- .#
GetByAppointmentIdAsyncRR. E
(RRE F
dtoSS 
.SS 
AppointmentIdSS %
)SS% &
;SS& '
ifUU 
(UU 
existingRecordUU 
!=UU !
nullUU" &
)UU& '
{VV 
throwWW 
newWW %
InvalidOperationExceptionWW 3
(WW3 4
$strXX J
)XXJ K
;XXK L
}YY 
var[[ 
patient[[ 
=[[ 
await\\ 
_patientRepository\\ (
.\\( )
GetByIdAsync\\) 5
(\\5 6
dto\\6 9
.\\9 :
	PatientId\\: C
)\\C D
;\\D E
if^^ 
(^^ 
patient^^ 
==^^ 
null^^ 
)^^  
{__ 
throw`` 
new``  
KeyNotFoundException`` .
(``. /
$str``/ C
)``C D
;``D E
}aa 
varcc 
doctorcc 
=cc 
awaitdd 
_doctorRepositorydd '
.dd' (
GetByIdAsyncdd( 4
(dd4 5
dtodd5 8
.dd8 9
DoctorIddd9 A
)ddA B
;ddB C
ifff 
(ff 
doctorff 
==ff 
nullff 
)ff 
{gg 
throwhh 
newhh  
KeyNotFoundExceptionhh .
(hh. /
$strhh/ B
)hhB C
;hhC D
}ii 
ifkk 
(kk 
appointmentkk 
.kk 
	PatientIdkk %
!=kk& (
dtokk) ,
.kk, -
	PatientIdkk- 6
)kk6 7
{ll 
throwmm 
newmm %
InvalidOperationExceptionmm 3
(mm3 4
$strnn 9
)nn9 :
;nn: ;
}oo 
ifqq 
(qq 
appointmentqq 
.qq 
DoctorIdqq $
!=qq% '
dtoqq( +
.qq+ ,
DoctorIdqq, 4
)qq4 5
{rr 
throwss 
newss %
InvalidOperationExceptionss 3
(ss3 4
$strtt 8
)tt8 9
;tt9 :
}uu 
varww 
recordww 
=ww 
newww 
HealthRecordww )
{xx 
AppointmentIdyy 
=yy 
dtoyy  #
.yy# $
AppointmentIdyy$ 1
,yy1 2
	PatientIdzz 
=zz 
dtozz 
.zz  
	PatientIdzz  )
,zz) *
DoctorId{{ 
={{ 
dto{{ 
.{{ 
DoctorId{{ '
,{{' (
	Diagnosis|| 
=|| 
dto|| 
.||  
	Diagnosis||  )
!||) *
.||* +
Trim||+ /
(||/ 0
)||0 1
,||1 2
Prescription}} 
=}} 
dto}} "
.}}" #
Prescription}}# /
!}}/ 0
.}}0 1
Trim}}1 5
(}}5 6
)}}6 7
,}}7 8
Notes~~ 
=~~ 
dto~~ 
.~~ 
Notes~~ !
?~~! "
.~~" #
Trim~~# '
(~~' (
)~~( )
,~~) *
	CreatedOn 
= 
DateTime $
.$ %
UtcNow% +
}
ÄÄ 
;
ÄÄ 
await
ÇÇ %
_healthRecordRepository
ÇÇ )
.
ÇÇ) *
AddAsync
ÇÇ* 2
(
ÇÇ2 3
record
ÇÇ3 9
)
ÇÇ9 :
;
ÇÇ: ;
await
ÉÉ %
_healthRecordRepository
ÉÉ )
.
ÉÉ) *
SaveChangesAsync
ÉÉ* :
(
ÉÉ: ;
)
ÉÉ; <
;
ÉÉ< =
var
ÖÖ 
createdRecord
ÖÖ 
=
ÖÖ 
await
ÜÜ %
_healthRecordRepository
ÜÜ -
.
ÜÜ- .
GetByIdAsync
ÜÜ. :
(
ÜÜ: ;
record
ÜÜ; A
.
ÜÜA B
HealthRecordId
ÜÜB P
)
ÜÜP Q
;
ÜÜQ R
return
àà 
MapToDto
àà 
(
àà 
createdRecord
àà )
??
àà* ,
record
àà- 3
)
àà3 4
;
àà4 5
}
ââ 	
public
ãã 
async
ãã 
Task
ãã 
UpdateAsync
ãã %
(
ãã% &
int
åå 
id
åå 
,
åå #
UpdateHealthRecordDto
çç !
dto
çç" %
)
çç% &
{
éé 	
ValidateUpdateDto
èè 
(
èè 
dto
èè !
)
èè! "
;
èè" #
var
ëë 
record
ëë 
=
ëë 
await
íí %
_healthRecordRepository
íí -
.
íí- .
GetByIdAsync
íí. :
(
íí: ;
id
íí; =
)
íí= >
;
íí> ?
if
îî 
(
îî 
record
îî 
==
îî 
null
îî 
)
îî 
{
ïï 
throw
ññ 
new
ññ "
KeyNotFoundException
ññ .
(
ññ. /
$"
óó 
$str
óó $
{
óó$ %
id
óó% '
}
óó' (
$str
óó( 3
"
óó3 4
)
óó4 5
;
óó5 6
}
òò 
record
öö 
.
öö 
	Diagnosis
öö 
=
öö 
dto
öö "
.
öö" #
	Diagnosis
öö# ,
!
öö, -
.
öö- .
Trim
öö. 2
(
öö2 3
)
öö3 4
;
öö4 5
record
õõ 
.
õõ 
Prescription
õõ 
=
õõ  !
dto
õõ" %
.
õõ% &
Prescription
õõ& 2
!
õõ2 3
.
õõ3 4
Trim
õõ4 8
(
õõ8 9
)
õõ9 :
;
õõ: ;
record
úú 
.
úú 
Notes
úú 
=
úú 
dto
úú 
.
úú 
Notes
úú $
?
úú$ %
.
úú% &
Trim
úú& *
(
úú* +
)
úú+ ,
;
úú, -
await
ûû %
_healthRecordRepository
ûû )
.
ûû) *
UpdateAsync
ûû* 5
(
ûû5 6
record
ûû6 <
)
ûû< =
;
ûû= >
await
üü %
_healthRecordRepository
üü )
.
üü) *
SaveChangesAsync
üü* :
(
üü: ;
)
üü; <
;
üü< =
}
†† 	
private
¢¢ 
static
¢¢ 
void
¢¢ 
ValidateCreateDto
¢¢ -
(
¢¢- .#
CreateHealthRecordDto
££ !
dto
££" %
)
££% &
{
§§ 	
if
•• 
(
•• 
dto
•• 
.
•• 
AppointmentId
•• !
<=
••" $
$num
••% &
)
••& '
{
¶¶ 
throw
ßß 
new
ßß 
ArgumentException
ßß +
(
ßß+ ,
$str
®® 0
)
®®0 1
;
®®1 2
}
©© 
if
´´ 
(
´´ 
dto
´´ 
.
´´ 
	PatientId
´´ 
<=
´´  
$num
´´! "
)
´´" #
{
¨¨ 
throw
≠≠ 
new
≠≠ 
ArgumentException
≠≠ +
(
≠≠+ ,
$str
ÆÆ ,
)
ÆÆ, -
;
ÆÆ- .
}
ØØ 
if
±± 
(
±± 
dto
±± 
.
±± 
DoctorId
±± 
<=
±± 
$num
±±  !
)
±±! "
{
≤≤ 
throw
≥≥ 
new
≥≥ 
ArgumentException
≥≥ +
(
≥≥+ ,
$str
¥¥ +
)
¥¥+ ,
;
¥¥, -
}
µµ 
if
∑∑ 
(
∑∑ 
string
∑∑ 
.
∑∑  
IsNullOrWhiteSpace
∑∑ )
(
∑∑) *
dto
∑∑* -
.
∑∑- .
	Diagnosis
∑∑. 7
)
∑∑7 8
)
∑∑8 9
{
∏∏ 
throw
ππ 
new
ππ 
ArgumentException
ππ +
(
ππ+ ,
$str
∫∫ ,
)
∫∫, -
;
∫∫- .
}
ªª 
if
ΩΩ 
(
ΩΩ 
string
ΩΩ 
.
ΩΩ  
IsNullOrWhiteSpace
ΩΩ )
(
ΩΩ) *
dto
ΩΩ* -
.
ΩΩ- .
Prescription
ΩΩ. :
)
ΩΩ: ;
)
ΩΩ; <
{
ææ 
throw
øø 
new
øø 
ArgumentException
øø +
(
øø+ ,
$str
¿¿ /
)
¿¿/ 0
;
¿¿0 1
}
¡¡ 
}
¬¬ 	
private
ƒƒ 
static
ƒƒ 
void
ƒƒ 
ValidateUpdateDto
ƒƒ -
(
ƒƒ- .#
UpdateHealthRecordDto
≈≈ !
dto
≈≈" %
)
≈≈% &
{
∆∆ 	
if
«« 
(
«« 
string
«« 
.
««  
IsNullOrWhiteSpace
«« )
(
««) *
dto
««* -
.
««- .
	Diagnosis
««. 7
)
««7 8
)
««8 9
{
»» 
throw
…… 
new
…… 
ArgumentException
…… +
(
……+ ,
$str
   ,
)
  , -
;
  - .
}
ÀÀ 
if
ÕÕ 
(
ÕÕ 
string
ÕÕ 
.
ÕÕ  
IsNullOrWhiteSpace
ÕÕ )
(
ÕÕ) *
dto
ÕÕ* -
.
ÕÕ- .
Prescription
ÕÕ. :
)
ÕÕ: ;
)
ÕÕ; <
{
ŒŒ 
throw
œœ 
new
œœ 
ArgumentException
œœ +
(
œœ+ ,
$str
–– /
)
––/ 0
;
––0 1
}
—— 
}
““ 	
private
‘‘ 
static
‘‘ 
HealthRecordDto
‘‘ &
MapToDto
‘‘' /
(
‘‘/ 0
HealthRecord
’’ 
record
’’ 
)
’’  
{
÷÷ 	
return
◊◊ 
new
◊◊ 
HealthRecordDto
◊◊ &
{
ÿÿ 
HealthRecordId
ŸŸ 
=
ŸŸ  
record
ŸŸ! '
.
ŸŸ' (
HealthRecordId
ŸŸ( 6
,
ŸŸ6 7
AppointmentId
⁄⁄ 
=
⁄⁄ 
record
⁄⁄  &
.
⁄⁄& '
AppointmentId
⁄⁄' 4
,
⁄⁄4 5
	PatientId
€€ 
=
€€ 
record
€€ "
.
€€" #
	PatientId
€€# ,
,
€€, -
DoctorId
‹‹ 
=
‹‹ 
record
‹‹ !
.
‹‹! "
DoctorId
‹‹" *
,
‹‹* +

DoctorName
›› 
=
›› 
record
›› #
.
››# $
Doctor
››$ *
?
››* +
.
››+ ,
FullName
››, 4
??
››5 7
string
››8 >
.
››> ?
Empty
››? D
,
››D E"
DoctorSpecialisation
ﬁﬁ $
=
ﬁﬁ% &
record
ﬁﬁ' -
.
ﬁﬁ- .
Doctor
ﬁﬁ. 4
==
ﬁﬁ5 7
null
ﬁﬁ8 <
?
ﬂﬂ 
$num
ﬂﬂ 
:
‡‡ 
(
‡‡ 
int
‡‡ 
)
‡‡ 
record
‡‡ !
.
‡‡! "
Doctor
‡‡" (
.
‡‡( )
Specialisation
‡‡) 7
,
‡‡7 8
	CreatedOn
·· 
=
·· 
record
·· "
.
··" #
	CreatedOn
··# ,
,
··, -
	Diagnosis
‚‚ 
=
‚‚ 
record
‚‚ "
.
‚‚" #
	Diagnosis
‚‚# ,
,
‚‚, -
Prescription
„„ 
=
„„ 
record
„„ %
.
„„% &
Prescription
„„& 2
,
„„2 3
Notes
‰‰ 
=
‰‰ 
record
‰‰ 
.
‰‰ 
Notes
‰‰ $
}
ÂÂ 
;
ÂÂ 
}
ÊÊ 	
}
ÁÁ 
}ËË Ê±
gC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\DoctorService.cs
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
string 
? 
sortBy 
, 
int 
? 
specialisation 
)  
{ 	
var 
doctors 
= 
await 
_doctorRepository '
.' (
GetAllAsync( 3
(3 4
sortBy 
, 
specialisation "
)" #
;# $
return 
doctors 
. 
Select !
(! "
MapToDoctorDto" 0
)0 1
;1 2
}   	
public"" 
async"" 
Task"" 
<"" 
IEnumerable"" %
<""% &
	DoctorDto""& /
>""/ 0
>""0 1*
GetActiveBySpecialisationAsync""2 P
(""P Q
int## 
specialisation## 
)## 
{$$ 	
if%% 
(%% 
!%% 
Enum%% 
.%% 
	IsDefined%% 
(%%  
typeof%%  &
(%%& ' 
DoctorSpecialisation%%' ;
)%%; <
,%%< =
specialisation%%> L
)%%L M
)%%M N
{&& 
throw'' 
new'' 
ArgumentException'' +
(''+ ,
$str'', L
)''L M
;''M N
}(( 
var** 
doctors** 
=** 
await++ 
_doctorRepository++ '
.,, *
GetActiveBySpecialisationAsync,, 3
(,,3 4
specialisation-- &
)--& '
;--' (
return// 
doctors// 
.// 
Select// !
(//! "
MapToDoctorDto//" 0
)//0 1
;//1 2
}00 	
public22 
async22 
Task22 
<22 
	DoctorDto22 #
?22# $
>22$ %
GetByIdAsync22& 2
(222 3
int223 6
id227 9
)229 :
{33 	
var44 
doctor44 
=44 
await55 
_doctorRepository55 '
.55' (
GetByIdAsync55( 4
(554 5
id555 7
)557 8
;558 9
return77 
doctor77 
==77 
null77 !
?88 
null88 
:99 
MapToDoctorDto99  
(99  !
doctor99! '
)99' (
;99( )
}:: 	
public<< 
async<< 
Task<< 
<<< 
	DoctorDto<< #
><<# $
CreateAsync<<% 0
(<<0 1
CreateDoctorDto<<1 @
dto<<A D
)<<D E
{== 	
ValidateDoctor>> 
(>> 
dto>> 
)>> 
;>>  
var@@ 
doctor@@ 
=@@ 
new@@ 
Doctor@@ #
{AA 
FullNameBB 
=BB 
dtoBB 
.BB 
FullNameBB '
.BB' (
TrimBB( ,
(BB, -
)BB- .
,BB. /
EmailCC 
=CC 
dtoCC 
.CC 
EmailCC !
.CC! "
TrimCC" &
(CC& '
)CC' (
.CC( )
ToLowerCC) 0
(CC0 1
)CC1 2
,CC2 3
SpecialisationDD 
=DD  
(DD! " 
DoctorSpecialisationDD" 6
)DD6 7
dtoDD7 :
.DD: ;
SpecialisationDD; I
,DDI J
YearsOfExperienceEE !
=EE" #
dtoEE$ '
.EE' (
YearsOfExperienceEE( 9
,EE9 :
ConsultationFeeFF 
=FF  !
dtoFF" %
.FF% &
ConsultationFeeFF& 5
,FF5 6
IsActiveGG 
=GG 
trueGG 
}HH 
;HH 
awaitJJ 
_doctorRepositoryJJ #
.JJ# $
AddAsyncJJ$ ,
(JJ, -
doctorJJ- 3
)JJ3 4
;JJ4 5
awaitKK 
_doctorRepositoryKK #
.KK# $
SaveChangesAsyncKK$ 4
(KK4 5
)KK5 6
;KK6 7
returnMM 
MapToDoctorDtoMM !
(MM! "
doctorMM" (
)MM( )
;MM) *
}NN 	
publicPP 
asyncPP 
TaskPP 
UpdateAsyncPP %
(PP% &
intQQ 
idQQ 
,QQ 
UpdateDoctorDtoRR 
dtoRR 
)RR  
{SS 	
ValidateDoctorTT 
(TT 
dtoTT 
)TT 
;TT  
varVV 
doctorVV 
=VV 
awaitWW 
_doctorRepositoryWW '
.WW' (
GetByIdAsyncWW( 4
(WW4 5
idWW5 7
)WW7 8
;WW8 9
ifYY 
(YY 
doctorYY 
==YY 
nullYY 
)YY 
{ZZ 
throw[[ 
new[[  
KeyNotFoundException[[ .
([[. /
$"\\ 
$str\\ %
{\\% &
id\\& (
}\\( )
$str\\) 4
"\\4 5
)\\5 6
;\\6 7
}]] 
doctor__ 
.__ 
FullName__ 
=__ 
dto__ !
.__! "
FullName__" *
.__* +
Trim__+ /
(__/ 0
)__0 1
;__1 2
doctor`` 
.`` 
Specialisation`` !
=``" #
(``$ % 
DoctorSpecialisation``% 9
)``9 :
dto``: =
.``= >
Specialisation``> L
;``L M
doctoraa 
.aa 
YearsOfExperienceaa $
=aa% &
dtoaa' *
.aa* +
YearsOfExperienceaa+ <
;aa< =
doctorbb 
.bb 
ConsultationFeebb "
=bb# $
dtobb% (
.bb( )
ConsultationFeebb) 8
;bb8 9
awaitdd 
_doctorRepositorydd #
.dd# $
UpdateAsyncdd$ /
(dd/ 0
doctordd0 6
)dd6 7
;dd7 8
awaitee 
_doctorRepositoryee #
.ee# $
SaveChangesAsyncee$ 4
(ee4 5
)ee5 6
;ee6 7
}ff 	
publichh 
asynchh 
Taskhh 
<hh 
IEnumerablehh %
<hh% &
inthh& )
>hh) *
>hh* + 
GetAvailabilityAsynchh, @
(hh@ A
intii 
doctorIdii 
,ii 
DateOnlyjj 
datejj 
)jj 
{kk 	
varll 
doctorll 
=ll 
awaitmm 
_doctorRepositorymm '
.mm' (
GetByIdAsyncmm( 4
(mm4 5
doctorIdmm5 =
)mm= >
;mm> ?
ifoo 
(oo 
doctoroo 
==oo 
nulloo 
)oo 
{pp 
throwqq 
newqq  
KeyNotFoundExceptionqq .
(qq. /
$strqq/ B
)qqB C
;qqC D
}rr 
vartt 
bookedSlotstt 
=tt 
awaituu 
_doctorRepositoryuu '
.uu' (
GetBookedSlotsAsyncuu( ;
(uu; <
doctorIdvv 
,vv 
dateww 
)ww 
;ww 
varyy 
allSlotsyy 
=yy 
Enumzz 
.zz 
	GetValueszz 
<zz 
AppointmentTimeSlotzz 2
>zz2 3
(zz3 4
)zz4 5
.{{ 
Select{{ 
({{ 
slot{{  
=>{{! #
({{$ %
int{{% (
){{( )
slot{{) -
){{- .
;{{. /
return}} 
allSlots}} 
.}} 
Except}} "
(}}" #
bookedSlots}}# .
)}}. /
;}}/ 0
}~~ 	
public
ÄÄ 
async
ÄÄ 
Task
ÄÄ 
<
ÄÄ %
DoctorCreationResultDto
ÄÄ 1
>
ÄÄ1 2*
CreateDoctorWithAccountAsync
ÄÄ3 O
(
ÄÄO P
CreateDoctorDto
ÅÅ 
dto
ÅÅ 
)
ÅÅ  
{
ÇÇ 	
ValidateDoctor
ÉÉ 
(
ÉÉ 
dto
ÉÉ 
)
ÉÉ 
;
ÉÉ  
if
ÖÖ 
(
ÖÖ 
await
ÖÖ 
_userService
ÖÖ "
.
ÖÖ" #
EmailExistsAsync
ÖÖ# 3
(
ÖÖ3 4
dto
ÖÖ4 7
.
ÖÖ7 8
Email
ÖÖ8 =
)
ÖÖ= >
)
ÖÖ> ?
{
ÜÜ 
throw
áá 
new
áá 
ArgumentException
áá +
(
áá+ ,
$str
áá, C
)
ááC D
;
ááD E
}
àà 
var
ää 
doctor
ää 
=
ää 
new
ää 
Doctor
ää #
{
ãã 
FullName
åå 
=
åå 
dto
åå 
.
åå 
FullName
åå '
.
åå' (
Trim
åå( ,
(
åå, -
)
åå- .
,
åå. /
Email
çç 
=
çç 
dto
çç 
.
çç 
Email
çç !
.
çç! "
Trim
çç" &
(
çç& '
)
çç' (
.
çç( )
ToLower
çç) 0
(
çç0 1
)
çç1 2
,
çç2 3
Specialisation
éé 
=
éé  
(
éé! ""
DoctorSpecialisation
éé" 6
)
éé6 7
dto
éé7 :
.
éé: ;
Specialisation
éé; I
,
ééI J
YearsOfExperience
èè !
=
èè" #
dto
èè$ '
.
èè' (
YearsOfExperience
èè( 9
,
èè9 :
ConsultationFee
êê 
=
êê  !
dto
êê" %
.
êê% &
ConsultationFee
êê& 5
,
êê5 6
IsActive
ëë 
=
ëë 
true
ëë 
}
íí 
;
íí 
await
îî 
_doctorRepository
îî #
.
îî# $
AddAsync
îî$ ,
(
îî, -
doctor
îî- 3
)
îî3 4
;
îî4 5
await
ïï 
_doctorRepository
ïï #
.
ïï# $
SaveChangesAsync
ïï$ 4
(
ïï4 5
)
ïï5 6
;
ïï6 7
var
óó 
temporaryPassword
óó !
=
óó" #'
GenerateTemporaryPassword
òò )
(
òò) *
)
òò* +
;
òò+ ,
var
öö 
user
öö 
=
öö 
new
öö 
User
öö 
{
õõ 
Email
úú 
=
úú 
doctor
úú 
.
úú 
Email
úú $
,
úú$ %
PasswordHash
ùù 
=
ùù 
HashPassword
ùù +
(
ùù+ ,
temporaryPassword
ùù, =
)
ùù= >
,
ùù> ?
Role
ûû 
=
ûû 
UserRole
ûû 
.
ûû  
Doctor
ûû  &
,
ûû& '
ReferenceId
üü 
=
üü 
doctor
üü $
.
üü$ %
DoctorId
üü% -
,
üü- .
CreatedDate
†† 
=
†† 
DateTime
†† &
.
††& '
UtcNow
††' -
,
††- . 
MustChangePassword
°° "
=
°°# $
true
°°% )
}
¢¢ 
;
¢¢ 
await
§§ 
_userService
§§ 
.
§§ 
CreateAsync
§§ *
(
§§* +
user
§§+ /
)
§§/ 0
;
§§0 1
await
•• 
_userService
•• 
.
•• 
SaveChangesAsync
•• /
(
••/ 0
)
••0 1
;
••1 2
return
ßß 
new
ßß %
DoctorCreationResultDto
ßß .
{
®® 
DoctorId
©© 
=
©© 
doctor
©© !
.
©©! "
DoctorId
©©" *
,
©©* +
FullName
™™ 
=
™™ 
doctor
™™ !
.
™™! "
FullName
™™" *
,
™™* +
Email
´´ 
=
´´ 
doctor
´´ 
.
´´ 
Email
´´ $
,
´´$ %
TemporaryPassword
¨¨ !
=
¨¨" #
temporaryPassword
¨¨$ 5
}
≠≠ 
;
≠≠ 
}
ÆÆ 	
public
∞∞ 
async
∞∞ 
Task
∞∞ 
ActivateAsync
∞∞ '
(
∞∞' (
int
∞∞( +
id
∞∞, .
)
∞∞. /
{
±± 	
var
≤≤ 
doctor
≤≤ 
=
≤≤ 
await
≥≥ 
_doctorRepository
≥≥ '
.
≥≥' (
GetByIdAsync
≥≥( 4
(
≥≥4 5
id
≥≥5 7
)
≥≥7 8
;
≥≥8 9
if
µµ 
(
µµ 
doctor
µµ 
==
µµ 
null
µµ 
)
µµ 
{
∂∂ 
throw
∑∑ 
new
∑∑ "
KeyNotFoundException
∑∑ .
(
∑∑. /
$"
∏∏ 
$str
∏∏ %
{
∏∏% &
id
∏∏& (
}
∏∏( )
$str
∏∏) 4
"
∏∏4 5
)
∏∏5 6
;
∏∏6 7
}
ππ 
doctor
ªª 
.
ªª 
IsActive
ªª 
=
ªª 
true
ªª "
;
ªª" #
await
ΩΩ 
_doctorRepository
ΩΩ #
.
ΩΩ# $
UpdateAsync
ΩΩ$ /
(
ΩΩ/ 0
doctor
ΩΩ0 6
)
ΩΩ6 7
;
ΩΩ7 8
await
ææ 
_doctorRepository
ææ #
.
ææ# $
SaveChangesAsync
ææ$ 4
(
ææ4 5
)
ææ5 6
;
ææ6 7
}
øø 	
public
¡¡ 
async
¡¡ 
Task
¡¡ 
DeactivateAsync
¡¡ )
(
¡¡) *
int
¡¡* -
id
¡¡. 0
)
¡¡0 1
{
¬¬ 	
var
√√ 
doctor
√√ 
=
√√ 
await
ƒƒ 
_doctorRepository
ƒƒ '
.
ƒƒ' (
GetByIdAsync
ƒƒ( 4
(
ƒƒ4 5
id
ƒƒ5 7
)
ƒƒ7 8
;
ƒƒ8 9
if
∆∆ 
(
∆∆ 
doctor
∆∆ 
==
∆∆ 
null
∆∆ 
)
∆∆ 
{
«« 
throw
»» 
new
»» "
KeyNotFoundException
»» .
(
»». /
$"
…… 
$str
…… %
{
……% &
id
……& (
}
……( )
$str
……) 4
"
……4 5
)
……5 6
;
……6 7
}
   
doctor
ÃÃ 
.
ÃÃ 
IsActive
ÃÃ 
=
ÃÃ 
false
ÃÃ #
;
ÃÃ# $
await
ŒŒ 
_doctorRepository
ŒŒ #
.
ŒŒ# $
UpdateAsync
ŒŒ$ /
(
ŒŒ/ 0
doctor
ŒŒ0 6
)
ŒŒ6 7
;
ŒŒ7 8
await
œœ 
_doctorRepository
œœ #
.
œœ# $
SaveChangesAsync
œœ$ 4
(
œœ4 5
)
œœ5 6
;
œœ6 7
}
–– 	
private
““ 
static
““ 
void
““ 
ValidateDoctor
““ *
(
““* +
CreateDoctorDto
““+ :
dto
““; >
)
““> ?
{
”” 	
if
‘‘ 
(
‘‘ 
string
‘‘ 
.
‘‘  
IsNullOrWhiteSpace
‘‘ )
(
‘‘) *
dto
‘‘* -
.
‘‘- .
FullName
‘‘. 6
)
‘‘6 7
)
‘‘7 8
{
’’ 
throw
÷÷ 
new
÷÷ 
ArgumentException
÷÷ +
(
÷÷+ ,
$str
÷÷, F
)
÷÷F G
;
÷÷G H
}
◊◊ 
if
ŸŸ 
(
ŸŸ 
string
ŸŸ 
.
ŸŸ  
IsNullOrWhiteSpace
ŸŸ )
(
ŸŸ) *
dto
ŸŸ* -
.
ŸŸ- .
Email
ŸŸ. 3
)
ŸŸ3 4
)
ŸŸ4 5
{
⁄⁄ 
throw
€€ 
new
€€ 
ArgumentException
€€ +
(
€€+ ,
$str
€€, @
)
€€@ A
;
€€A B
}
‹‹ 
if
ﬁﬁ 
(
ﬁﬁ 
!
ﬁﬁ 
Enum
ﬁﬁ 
.
ﬁﬁ 
	IsDefined
ﬁﬁ 
(
ﬁﬁ  
typeof
ﬁﬁ  &
(
ﬁﬁ& '"
DoctorSpecialisation
ﬁﬁ' ;
)
ﬁﬁ; <
,
ﬁﬁ< =
dto
ﬁﬁ> A
.
ﬁﬁA B
Specialisation
ﬁﬁB P
)
ﬁﬁP Q
)
ﬁﬁQ R
{
ﬂﬂ 
throw
‡‡ 
new
‡‡ 
ArgumentException
‡‡ +
(
‡‡+ ,
$str
‡‡, L
)
‡‡L M
;
‡‡M N
}
·· 
if
„„ 
(
„„ 
dto
„„ 
.
„„ 
YearsOfExperience
„„ %
<
„„& '
$num
„„( )
||
„„* ,
dto
„„- 0
.
„„0 1
YearsOfExperience
„„1 B
>
„„C D
$num
„„E G
)
„„G H
{
‰‰ 
throw
ÂÂ 
new
ÂÂ 
ArgumentException
ÂÂ +
(
ÂÂ+ ,
$str
ÊÊ @
)
ÊÊ@ A
;
ÊÊA B
}
ÁÁ 
if
ÈÈ 
(
ÈÈ 
dto
ÈÈ 
.
ÈÈ 
ConsultationFee
ÈÈ #
<=
ÈÈ$ &
$num
ÈÈ' (
)
ÈÈ( )
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ 
ArgumentException
ÎÎ +
(
ÎÎ+ ,
$str
ÏÏ A
)
ÏÏA B
;
ÏÏB C
}
ÌÌ 
}
ÓÓ 	
private
 
static
 
void
 
ValidateDoctor
 *
(
* +
UpdateDoctorDto
+ :
dto
; >
)
> ?
{
ÒÒ 	
if
ÚÚ 
(
ÚÚ 
string
ÚÚ 
.
ÚÚ  
IsNullOrWhiteSpace
ÚÚ )
(
ÚÚ) *
dto
ÚÚ* -
.
ÚÚ- .
FullName
ÚÚ. 6
)
ÚÚ6 7
)
ÚÚ7 8
{
ÛÛ 
throw
ÙÙ 
new
ÙÙ 
ArgumentException
ÙÙ +
(
ÙÙ+ ,
$str
ÙÙ, F
)
ÙÙF G
;
ÙÙG H
}
ıı 
if
˜˜ 
(
˜˜ 
!
˜˜ 
Enum
˜˜ 
.
˜˜ 
	IsDefined
˜˜ 
(
˜˜  
typeof
˜˜  &
(
˜˜& '"
DoctorSpecialisation
˜˜' ;
)
˜˜; <
,
˜˜< =
dto
˜˜> A
.
˜˜A B
Specialisation
˜˜B P
)
˜˜P Q
)
˜˜Q R
{
¯¯ 
throw
˘˘ 
new
˘˘ 
ArgumentException
˘˘ +
(
˘˘+ ,
$str
˘˘, L
)
˘˘L M
;
˘˘M N
}
˙˙ 
if
¸¸ 
(
¸¸ 
dto
¸¸ 
.
¸¸ 
YearsOfExperience
¸¸ %
<
¸¸& '
$num
¸¸( )
||
¸¸* ,
dto
¸¸- 0
.
¸¸0 1
YearsOfExperience
¸¸1 B
>
¸¸C D
$num
¸¸E G
)
¸¸G H
{
˝˝ 
throw
˛˛ 
new
˛˛ 
ArgumentException
˛˛ +
(
˛˛+ ,
$str
ˇˇ @
)
ˇˇ@ A
;
ˇˇA B
}
ÄÄ 
if
ÇÇ 
(
ÇÇ 
dto
ÇÇ 
.
ÇÇ 
ConsultationFee
ÇÇ #
<=
ÇÇ$ &
$num
ÇÇ' (
)
ÇÇ( )
{
ÉÉ 
throw
ÑÑ 
new
ÑÑ 
ArgumentException
ÑÑ +
(
ÑÑ+ ,
$str
ÖÖ A
)
ÖÖA B
;
ÖÖB C
}
ÜÜ 
}
áá 	
private
ââ 
static
ââ 
string
ââ '
GenerateTemporaryPassword
ââ 7
(
ââ7 8
)
ââ8 9
{
ää 	
return
ãã 
$"
ãã 
$str
ãã 
{
ãã 
Random
ãã  
.
ãã  !
Shared
ãã! '
.
ãã' (
Next
ãã( ,
(
ãã, -
$num
ãã- 3
,
ãã3 4
$num
ãã5 ;
)
ãã; <
}
ãã< =
"
ãã= >
;
ãã> ?
}
åå 	
private
éé 
static
éé 
string
éé 
HashPassword
éé *
(
éé* +
string
éé+ 1
password
éé2 :
)
éé: ;
{
èè 	
using
êê 
var
êê 
sha256
êê 
=
êê 
System
ëë 
.
ëë 
Security
ëë 
.
ëë  
Cryptography
ëë  ,
.
ëë, -
SHA256
ëë- 3
.
ëë3 4
Create
ëë4 :
(
ëë: ;
)
ëë; <
;
ëë< =
var
ìì 
bytes
ìì 
=
ìì 
System
îî 
.
îî 
Text
îî 
.
îî 
Encoding
îî $
.
îî$ %
UTF8
îî% )
.
îî) *
GetBytes
îî* 2
(
îî2 3
password
îî3 ;
)
îî; <
;
îî< =
var
ññ 
hash
ññ 
=
ññ 
sha256
óó 
.
óó 
ComputeHash
óó "
(
óó" #
bytes
óó# (
)
óó( )
;
óó) *
return
ôô 
Convert
ôô 
.
ôô 
ToBase64String
ôô )
(
ôô) *
hash
ôô* .
)
ôô. /
;
ôô/ 0
}
öö 	
private
úú 
static
úú 
	DoctorDto
úú  
MapToDoctorDto
úú! /
(
úú/ 0
Doctor
úú0 6
doctor
úú7 =
)
úú= >
{
ùù 	
return
ûû 
new
ûû 
	DoctorDto
ûû  
{
üü 
DoctorId
†† 
=
†† 
doctor
†† !
.
††! "
DoctorId
††" *
,
††* +
FullName
°° 
=
°° 
doctor
°° !
.
°°! "
FullName
°°" *
,
°°* +
Email
¢¢ 
=
¢¢ 
doctor
¢¢ 
.
¢¢ 
Email
¢¢ $
,
¢¢$ %
Specialisation
££ 
=
££  
(
££! "
int
££" %
)
££% &
doctor
££& ,
.
££, -
Specialisation
££- ;
,
££; <
YearsOfExperience
§§ !
=
§§" #
doctor
§§$ *
.
§§* +
YearsOfExperience
§§+ <
,
§§< =
ConsultationFee
•• 
=
••  !
doctor
••" (
.
••( )
ConsultationFee
••) 8
,
••8 9
IsActive
¶¶ 
=
¶¶ 
doctor
¶¶ !
.
¶¶! "
IsActive
¶¶" *
}
ßß 
;
ßß 
}
®® 	
}
©© 
}™™ çÙ
eC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\AuthService.cs
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
RegisterDto 
request 
)  
{   	
if!! 
(!! 
request!! 
.!! 
Password!!  
!=!!! #
request!!$ +
.!!+ ,
ConfirmPassword!!, ;
)!!; <
{"" 
return## 
(## 
false## 
,## 
$str## 8
,##8 9
null##: >
)##> ?
;##? @
}$$ 
var&& 
email&& 
=&& 
request&& 
.&&  
Email&&  %
.&&% &
Trim&&& *
(&&* +
)&&+ ,
.&&, -
ToLower&&- 4
(&&4 5
)&&5 6
;&&6 7
if(( 
((( 
await(( 
_userRepository(( %
.((% &
EmailExistsAsync((& 6
(((6 7
email((7 <
)((< =
)((= >
{)) 
return** 
(** 
false** 
,** 
$str** 6
,**6 7
null**8 <
)**< =
;**= >
}++ 
var-- 
user-- 
=-- 
new-- 
User-- 
{.. 
Email// 
=// 
email// 
,// 
PasswordHash00 
=00 
HashPassword00 +
(00+ ,
request00, 3
.003 4
Password004 <
)00< =
,00= >
Role11 
=11 
request11 
.11 
Role11 #
,11# $
CreatedDate22 
=22 
DateTime22 &
.22& '
UtcNow22' -
,22- .
MustChangePassword33 "
=33# $
false33% *
}44 
;44 
var66 
refreshToken66 
=66  
GenerateRefreshToken66 3
(663 4
)664 5
;665 6
user88 
.88 
RefreshToken88 
=88 
refreshToken88  ,
;88, -
user99 
.99 "
RefreshTokenExpiryTime99 '
=99( )
DateTime99* 2
.992 3
UtcNow993 9
.999 :
AddDays99: A
(99A B
$num99B C
)99C D
;99D E
await;; 
_userRepository;; !
.;;! "
AddAsync;;" *
(;;* +
user;;+ /
);;/ 0
;;;0 1
await<< 
_userRepository<< !
.<<! "
SaveChangesAsync<<" 2
(<<2 3
)<<3 4
;<<4 5
var>> 
accessToken>> 
=>> 
GenerateToken>> +
(>>+ ,
user>>, 0
)>>0 1
;>>1 2
return@@ 
(@@ 
trueAA 
,AA 
$strBB /
,BB/ 0
newCC 
AuthResponseDtoCC #
{DD 
AccessTokenEE 
=EE  !
accessTokenEE" -
,EE- .
RefreshTokenFF  
=FF! "
refreshTokenFF# /
,FF/ 0
EmailGG 
=GG 
userGG  
.GG  !
EmailGG! &
,GG& '
RoleHH 
=HH 
userHH 
.HH  
RoleHH  $
.HH$ %
ToStringHH% -
(HH- .
)HH. /
,HH/ 0
ReferenceIdII 
=II  !
userII" &
.II& '
ReferenceIdII' 2
,II2 3
MustChangePasswordJJ &
=JJ' (
userJJ) -
.JJ- .
MustChangePasswordJJ. @
}KK 
)KK 
;KK 
}LL 	
publicNN 
asyncNN 
TaskNN 
<NN 
(NN 
boolNN 
SuccessNN  '
,NN' (
stringNN) /
MessageNN0 7
,NN7 8
AuthResponseDtoNN9 H
?NNH I
DataNNJ N
)NNN O
>NNO P 
RegisterPatientAsyncNNQ e
(NNe f
RegisterPatientDtoOO 
requestOO &
)OO& '
{PP 	
ifQQ 
(QQ 
requestQQ 
.QQ 
PasswordQQ  
!=QQ! #
requestQQ$ +
.QQ+ ,
ConfirmPasswordQQ, ;
)QQ; <
{RR 
returnSS 
(SS 
falseSS 
,SS 
$strSS 8
,SS8 9
nullSS: >
)SS> ?
;SS? @
}TT 
varVV 
emailVV 
=VV 
requestVV 
.VV  
EmailVV  %
.VV% &
TrimVV& *
(VV* +
)VV+ ,
.VV, -
ToLowerVV- 4
(VV4 5
)VV5 6
;VV6 7
ifXX 
(XX 
awaitXX 
_userRepositoryXX %
.XX% &
EmailExistsAsyncXX& 6
(XX6 7
emailXX7 <
)XX< =
)XX= >
{YY 
returnZZ 
(ZZ 
falseZZ 
,ZZ 
$strZZ 6
,ZZ6 7
nullZZ8 <
)ZZ< =
;ZZ= >
}[[ 
var]] 
patient]] 
=]] 
new]] 
Patient]] %
{^^ 
FullName__ 
=__ 
request__ "
.__" #
FullName__# +
.__+ ,
Trim__, 0
(__0 1
)__1 2
,__2 3
DateOfBirth`` 
=`` 
request`` %
.``% &
DateOfBirth``& 1
,``1 2
Genderaa 
=aa 
requestaa  
.aa  !
Genderaa! '
,aa' (
PhoneNumberbb 
=bb 
requestbb %
.bb% &
PhoneNumberbb& 1
.bb1 2
Trimbb2 6
(bb6 7
)bb7 8
,bb8 9
Emailcc 
=cc 
emailcc 
,cc 
InsuranceNumberdd 
=dd  !
requestdd" )
.dd) *
InsuranceNumberdd* 9
,dd9 :
IsActiveee 
=ee 
trueee 
}ff 
;ff 
awaithh 
_patientRepositoryhh $
.hh$ %
AddAsynchh% -
(hh- .
patienthh. 5
)hh5 6
;hh6 7
awaitii 
_patientRepositoryii $
.ii$ %
SaveChangesAsyncii% 5
(ii5 6
)ii6 7
;ii7 8
varkk 
userkk 
=kk 
newkk 
Userkk 
{ll 
Emailmm 
=mm 
emailmm 
,mm 
PasswordHashnn 
=nn 
HashPasswordnn +
(nn+ ,
requestnn, 3
.nn3 4
Passwordnn4 <
)nn< =
,nn= >
Roleoo 
=oo 
UserRoleoo 
.oo  
Patientoo  '
,oo' (
ReferenceIdpp 
=pp 
patientpp %
.pp% &
	PatientIdpp& /
,pp/ 0
CreatedDateqq 
=qq 
DateTimeqq &
.qq& '
UtcNowqq' -
,qq- .
MustChangePasswordrr "
=rr# $
falserr% *
}ss 
;ss 
varuu 
refreshTokenuu 
=uu  
GenerateRefreshTokenuu 3
(uu3 4
)uu4 5
;uu5 6
userww 
.ww 
RefreshTokenww 
=ww 
refreshTokenww  ,
;ww, -
userxx 
.xx "
RefreshTokenExpiryTimexx '
=xx( )
DateTimexx* 2
.xx2 3
UtcNowxx3 9
.xx9 :
AddDaysxx: A
(xxA B
$numxxB C
)xxC D
;xxD E
awaitzz 
_userRepositoryzz !
.zz! "
AddAsynczz" *
(zz* +
userzz+ /
)zz/ 0
;zz0 1
await{{ 
_userRepository{{ !
.{{! "
SaveChangesAsync{{" 2
({{2 3
){{3 4
;{{4 5
var}} 
accessToken}} 
=}} 
GenerateToken}} +
(}}+ ,
user}}, 0
)}}0 1
;}}1 2
return 
( 
true
ÄÄ 
,
ÄÄ 
$str
ÅÅ 2
,
ÅÅ2 3
new
ÇÇ 
AuthResponseDto
ÇÇ #
{
ÉÉ 
AccessToken
ÑÑ 
=
ÑÑ  !
accessToken
ÑÑ" -
,
ÑÑ- .
RefreshToken
ÖÖ  
=
ÖÖ! "
refreshToken
ÖÖ# /
,
ÖÖ/ 0
Email
ÜÜ 
=
ÜÜ 
user
ÜÜ  
.
ÜÜ  !
Email
ÜÜ! &
,
ÜÜ& '
Role
áá 
=
áá 
user
áá 
.
áá  
Role
áá  $
.
áá$ %
ToString
áá% -
(
áá- .
)
áá. /
,
áá/ 0
ReferenceId
àà 
=
àà  !
user
àà" &
.
àà& '
ReferenceId
àà' 2
,
àà2 3 
MustChangePassword
ââ &
=
ââ' (
user
ââ) -
.
ââ- . 
MustChangePassword
ââ. @
}
ää 
)
ää 
;
ää 
}
ãã 	
public
çç 
async
çç 
Task
çç 
<
çç 
(
çç 
bool
çç 
Success
çç  '
,
çç' (
string
çç) /
Message
çç0 7
,
çç7 8
AuthResponseDto
çç9 H
?
ççH I
Data
ççJ N
)
ççN O
>
ççO P

LoginAsync
ççQ [
(
çç[ \
LoginDto
éé 
request
éé 
)
éé 
{
èè 	
var
êê 
email
êê 
=
êê 
request
êê 
.
êê  
Email
êê  %
.
êê% &
Trim
êê& *
(
êê* +
)
êê+ ,
.
êê, -
ToLower
êê- 4
(
êê4 5
)
êê5 6
;
êê6 7
var
íí 
user
íí 
=
íí 
await
íí 
_userRepository
íí ,
.
íí, -
GetByEmailAsync
íí- <
(
íí< =
email
íí= B
)
ííB C
;
ííC D
if
îî 
(
îî 
user
îî 
==
îî 
null
îî 
)
îî 
{
ïï 
return
ññ 
(
ññ 
false
ññ 
,
ññ 
$str
ññ ;
,
ññ; <
null
ññ= A
)
ññA B
;
ññB C
}
óó 
var
ôô 
hashedPassword
ôô 
=
ôô  
HashPassword
ôô! -
(
ôô- .
request
ôô. 5
.
ôô5 6
Password
ôô6 >
)
ôô> ?
;
ôô? @
if
õõ 
(
õõ 
user
õõ 
.
õõ 
PasswordHash
õõ !
!=
õõ" $
hashedPassword
õõ% 3
)
õõ3 4
{
úú 
return
ùù 
(
ùù 
false
ùù 
,
ùù 
$str
ùù ;
,
ùù; <
null
ùù= A
)
ùùA B
;
ùùB C
}
ûû 
var
†† 
accessToken
†† 
=
†† 
GenerateToken
†† +
(
††+ ,
user
††, 0
)
††0 1
;
††1 2
var
°° 
refreshToken
°° 
=
°° "
GenerateRefreshToken
°° 3
(
°°3 4
)
°°4 5
;
°°5 6
user
££ 
.
££ 
RefreshToken
££ 
=
££ 
refreshToken
££  ,
;
££, -
user
§§ 
.
§§ $
RefreshTokenExpiryTime
§§ '
=
§§( )
DateTime
§§* 2
.
§§2 3
UtcNow
§§3 9
.
§§9 :
AddDays
§§: A
(
§§A B
$num
§§B C
)
§§C D
;
§§D E
await
¶¶ 
_userRepository
¶¶ !
.
¶¶! "
UpdateAsync
¶¶" -
(
¶¶- .
user
¶¶. 2
)
¶¶2 3
;
¶¶3 4
await
ßß 
_userRepository
ßß !
.
ßß! "
SaveChangesAsync
ßß" 2
(
ßß2 3
)
ßß3 4
;
ßß4 5
return
©© 
(
©© 
true
™™ 
,
™™ 
$str
´´ #
,
´´# $
new
¨¨ 
AuthResponseDto
¨¨ #
{
≠≠ 
AccessToken
ÆÆ 
=
ÆÆ  !
accessToken
ÆÆ" -
,
ÆÆ- .
RefreshToken
ØØ  
=
ØØ! "
refreshToken
ØØ# /
,
ØØ/ 0
Email
∞∞ 
=
∞∞ 
user
∞∞  
.
∞∞  !
Email
∞∞! &
,
∞∞& '
Role
±± 
=
±± 
user
±± 
.
±±  
Role
±±  $
.
±±$ %
ToString
±±% -
(
±±- .
)
±±. /
,
±±/ 0
ReferenceId
≤≤ 
=
≤≤  !
user
≤≤" &
.
≤≤& '
ReferenceId
≤≤' 2
,
≤≤2 3 
MustChangePassword
≥≥ &
=
≥≥' (
user
≥≥) -
.
≥≥- . 
MustChangePassword
≥≥. @
}
¥¥ 
)
¥¥ 
;
¥¥ 
}
µµ 	
public
∑∑ 
async
∑∑ 
Task
∑∑ 
<
∑∑ 
(
∑∑ 
bool
∑∑ 
Success
∑∑  '
,
∑∑' (
string
∑∑) /
Message
∑∑0 7
,
∑∑7 8
AuthResponseDto
∑∑9 H
?
∑∑H I
Data
∑∑J N
)
∑∑N O
>
∑∑O P
RefreshTokenAsync
∑∑Q b
(
∑∑b c
RefreshTokenDto
∏∏ 
request
∏∏ #
)
∏∏# $
{
ππ 	
var
∫∫ 
user
∫∫ 
=
∫∫ 
await
ªª 
_userRepository
ªª %
.
ªª% &$
GetByRefreshTokenAsync
ªª& <
(
ªª< =
request
ºº 
.
ºº 
RefreshToken
ºº (
)
ºº( )
;
ºº) *
if
ææ 
(
ææ 
user
ææ 
==
ææ 
null
ææ 
)
ææ 
{
øø 
return
¿¿ 
(
¿¿ 
false
¿¿ 
,
¿¿ 
$str
¿¿ 7
,
¿¿7 8
null
¿¿9 =
)
¿¿= >
;
¿¿> ?
}
¡¡ 
if
√√ 
(
√√ 
!
√√ 
user
√√ 
.
√√ $
RefreshTokenExpiryTime
√√ ,
.
√√, -
HasValue
√√- 5
||
√√6 8
user
ƒƒ 
.
ƒƒ $
RefreshTokenExpiryTime
ƒƒ +
.
ƒƒ+ ,
Value
ƒƒ, 1
<=
ƒƒ2 4
DateTime
ƒƒ5 =
.
ƒƒ= >
UtcNow
ƒƒ> D
)
ƒƒD E
{
≈≈ 
return
∆∆ 
(
∆∆ 
false
∆∆ 
,
∆∆ 
$str
∆∆ ;
,
∆∆; <
null
∆∆= A
)
∆∆A B
;
∆∆B C
}
«« 
var
…… 
newAccessToken
…… 
=
……  
GenerateToken
……! .
(
……. /
user
……/ 3
)
……3 4
;
……4 5
var
   
newRefreshToken
   
=
    !"
GenerateRefreshToken
  " 6
(
  6 7
)
  7 8
;
  8 9
user
ÃÃ 
.
ÃÃ 
RefreshToken
ÃÃ 
=
ÃÃ 
newRefreshToken
ÃÃ  /
;
ÃÃ/ 0
user
ÕÕ 
.
ÕÕ $
RefreshTokenExpiryTime
ÕÕ '
=
ÕÕ( )
DateTime
ÕÕ* 2
.
ÕÕ2 3
UtcNow
ÕÕ3 9
.
ÕÕ9 :
AddDays
ÕÕ: A
(
ÕÕA B
$num
ÕÕB C
)
ÕÕC D
;
ÕÕD E
await
œœ 
_userRepository
œœ !
.
œœ! "
UpdateAsync
œœ" -
(
œœ- .
user
œœ. 2
)
œœ2 3
;
œœ3 4
await
–– 
_userRepository
–– !
.
––! "
SaveChangesAsync
––" 2
(
––2 3
)
––3 4
;
––4 5
return
““ 
(
““ 
true
”” 
,
”” 
$str
‘‘ /
,
‘‘/ 0
new
’’ 
AuthResponseDto
’’ #
{
÷÷ 
AccessToken
◊◊ 
=
◊◊  !
newAccessToken
◊◊" 0
,
◊◊0 1
RefreshToken
ÿÿ  
=
ÿÿ! "
newRefreshToken
ÿÿ# 2
,
ÿÿ2 3
Email
ŸŸ 
=
ŸŸ 
user
ŸŸ  
.
ŸŸ  !
Email
ŸŸ! &
,
ŸŸ& '
Role
⁄⁄ 
=
⁄⁄ 
user
⁄⁄ 
.
⁄⁄  
Role
⁄⁄  $
.
⁄⁄$ %
ToString
⁄⁄% -
(
⁄⁄- .
)
⁄⁄. /
,
⁄⁄/ 0
ReferenceId
€€ 
=
€€  !
user
€€" &
.
€€& '
ReferenceId
€€' 2
,
€€2 3 
MustChangePassword
‹‹ &
=
‹‹' (
user
‹‹) -
.
‹‹- . 
MustChangePassword
‹‹. @
}
›› 
)
›› 
;
›› 
}
ﬁﬁ 	
public
‡‡ 
async
‡‡ 
Task
‡‡ 
<
‡‡ 
(
‡‡ 
bool
‡‡ 
Success
‡‡  '
,
‡‡' (
string
‡‡) /
Message
‡‡0 7
)
‡‡7 8
>
‡‡8 9!
ChangePasswordAsync
‡‡: M
(
‡‡M N
string
·· 

email
·· 
,
·· 
ChangePasswordDto
‚‚ 
request
‚‚ 
)
‚‚ 
{
„„ 	
if
‰‰ 
(
‰‰ 
string
‰‰ 
.
‰‰  
IsNullOrWhiteSpace
‰‰ )
(
‰‰) *
email
‰‰* /
)
‰‰/ 0
)
‰‰0 1
{
ÂÂ 
return
ÊÊ 
(
ÊÊ 
false
ÊÊ 
,
ÊÊ 
$str
ÊÊ <
)
ÊÊ< =
;
ÊÊ= >
}
ÁÁ 
if
ÈÈ 
(
ÈÈ 
string
ÈÈ 
.
ÈÈ  
IsNullOrWhiteSpace
ÈÈ )
(
ÈÈ) *
request
ÈÈ* 1
.
ÈÈ1 2
CurrentPassword
ÈÈ2 A
)
ÈÈA B
)
ÈÈB C
{
ÍÍ 
return
ÎÎ 
(
ÎÎ 
false
ÎÎ 
,
ÎÎ 
$str
ÎÎ >
)
ÎÎ> ?
;
ÎÎ? @
}
ÏÏ 
if
ÓÓ 
(
ÓÓ 
string
ÓÓ 
.
ÓÓ  
IsNullOrWhiteSpace
ÓÓ )
(
ÓÓ) *
request
ÓÓ* 1
.
ÓÓ1 2
NewPassword
ÓÓ2 =
)
ÓÓ= >
)
ÓÓ> ?
{
ÔÔ 
return
 
(
 
false
 
,
 
$str
 :
)
: ;
;
; <
}
ÒÒ 
if
ÛÛ 
(
ÛÛ 
string
ÛÛ 
.
ÛÛ  
IsNullOrWhiteSpace
ÛÛ )
(
ÛÛ) *
request
ÛÛ* 1
.
ÛÛ1 2 
ConfirmNewPassword
ÛÛ2 D
)
ÛÛD E
)
ÛÛE F
{
ÙÙ 
return
ıı 
(
ıı 
false
ıı 
,
ıı 
$str
ıı >
)
ıı> ?
;
ıı? @
}
ˆˆ 
if
¯¯ 
(
¯¯ 
request
¯¯ 
.
¯¯ 
NewPassword
¯¯ #
!=
¯¯$ &
request
¯¯' .
.
¯¯. / 
ConfirmNewPassword
¯¯/ A
)
¯¯A B
{
˘˘ 
return
˙˙ 
(
˙˙ 
false
˙˙ 
,
˙˙ 
$str
˙˙ P
)
˙˙P Q
;
˙˙Q R
}
˚˚ 
if
˝˝ 
(
˝˝ 
request
˝˝ 
.
˝˝ 
CurrentPassword
˝˝ '
==
˝˝( *
request
˝˝+ 2
.
˝˝2 3
NewPassword
˝˝3 >
)
˝˝> ?
{
˛˛ 
return
ˇˇ 
(
ˇˇ 
false
ˇˇ 
,
ˇˇ 
$str
ˇˇ U
)
ˇˇU V
;
ˇˇV W
}
ÄÄ 
var
ÇÇ '
passwordValidationMessage
ÇÇ )
=
ÇÇ* +&
ValidatePasswordStrength
ÇÇ, D
(
ÇÇD E
request
ÇÇE L
.
ÇÇL M
NewPassword
ÇÇM X
)
ÇÇX Y
;
ÇÇY Z
if
ÑÑ 
(
ÑÑ 
!
ÑÑ 
string
ÑÑ 
.
ÑÑ  
IsNullOrWhiteSpace
ÑÑ *
(
ÑÑ* +'
passwordValidationMessage
ÑÑ+ D
)
ÑÑD E
)
ÑÑE F
{
ÖÖ 
return
ÜÜ 
(
ÜÜ 
false
ÜÜ 
,
ÜÜ '
passwordValidationMessage
ÜÜ 8
)
ÜÜ8 9
;
ÜÜ9 :
}
áá 
var
ââ 
user
ââ 
=
ââ 
await
ää 
_userRepository
ää %
.
ää% &
GetByEmailAsync
ää& 5
(
ää5 6
email
ãã 
.
ãã 
Trim
ãã 
(
ãã 
)
ãã  
.
ãã  !
ToLower
ãã! (
(
ãã( )
)
ãã) *
)
ãã* +
;
ãã+ ,
if
çç 
(
çç 
user
çç 
==
çç 
null
çç 
)
çç 
{
éé 
return
èè 
(
èè 
false
èè 
,
èè 
$str
èè 8
)
èè8 9
;
èè9 :
}
êê 
var
íí !
currentPasswordHash
íí #
=
íí$ %
HashPassword
íí& 2
(
íí2 3
request
íí3 :
.
íí: ;
CurrentPassword
íí; J
)
ííJ K
;
ííK L
if
îî 
(
îî 
user
îî 
.
îî 
PasswordHash
îî !
!=
îî" $!
currentPasswordHash
îî% 8
)
îî8 9
{
ïï 
return
ññ 
(
ññ 
false
ññ 
,
ññ 
$str
ññ ?
)
ññ? @
;
ññ@ A
}
óó 
user
ôô 
.
ôô 
PasswordHash
ôô 
=
ôô 
HashPassword
ôô  ,
(
ôô, -
request
ôô- 4
.
ôô4 5
NewPassword
ôô5 @
)
ôô@ A
;
ôôA B
user
õõ 
.
õõ  
MustChangePassword
õõ #
=
õõ$ %
false
õõ& +
;
õõ+ ,
await
ùù 
_userRepository
ùù !
.
ùù! "
UpdateAsync
ùù" -
(
ùù- .
user
ùù. 2
)
ùù2 3
;
ùù3 4
await
ûû 
_userRepository
ûû !
.
ûû! "
SaveChangesAsync
ûû" 2
(
ûû2 3
)
ûû3 4
;
ûû4 5
return
†† 
(
†† 
true
†† 
,
†† 
$str
†† :
)
††: ;
;
††; <
}
°° 	
private
££ 
static
££ 
string
££ 
?
££ &
ValidatePasswordStrength
££ 7
(
££7 8
string
££8 >
password
££? G
)
££G H
{
§§ 	
if
•• 
(
•• 
password
•• 
.
•• 
Length
•• 
<
••  !
$num
••" #
)
••# $
{
¶¶ 
return
ßß 
$str
ßß E
;
ßßE F
}
®® 
if
™™ 
(
™™ 
!
™™ 
password
™™ 
.
™™ 
Any
™™ 
(
™™ 
char
™™ "
.
™™" #
IsUpper
™™# *
)
™™* +
)
™™+ ,
{
´´ 
return
¨¨ 
$str
¨¨ M
;
¨¨M N
}
≠≠ 
if
ØØ 
(
ØØ 
!
ØØ 
password
ØØ 
.
ØØ 
Any
ØØ 
(
ØØ 
char
ØØ "
.
ØØ" #
IsLower
ØØ# *
)
ØØ* +
)
ØØ+ ,
{
∞∞ 
return
±± 
$str
±± M
;
±±M N
}
≤≤ 
if
¥¥ 
(
¥¥ 
!
¥¥ 
password
¥¥ 
.
¥¥ 
Any
¥¥ 
(
¥¥ 
char
¥¥ "
.
¥¥" #
IsDigit
¥¥# *
)
¥¥* +
)
¥¥+ ,
{
µµ 
return
∂∂ 
$str
∂∂ C
;
∂∂C D
}
∑∑ 
if
ππ 
(
ππ 
!
ππ 
password
ππ 
.
ππ 
Any
ππ 
(
ππ 
ch
ππ  
=>
ππ! #
!
ππ$ %
char
ππ% )
.
ππ) *
IsLetterOrDigit
ππ* 9
(
ππ9 :
ch
ππ: <
)
ππ< =
)
ππ= >
)
ππ> ?
{
∫∫ 
return
ªª 
$str
ªª N
;
ªªN O
}
ºº 
return
ææ 
null
ææ 
;
ææ 
}
øø 	
private
¡¡ 
string
¡¡ 
GenerateToken
¡¡ $
(
¡¡$ %
User
¡¡% )
user
¡¡* .
)
¡¡. /
{
¬¬ 	
var
√√ 
jwtSettings
√√ 
=
√√ 
_configuration
√√ ,
.
√√, -

GetSection
√√- 7
(
√√7 8
$str
√√8 =
)
√√= >
;
√√> ?
var
≈≈ 
key
≈≈ 
=
≈≈ 
new
≈≈ "
SymmetricSecurityKey
≈≈ .
(
≈≈. /
Encoding
∆∆ 
.
∆∆ 
UTF8
∆∆ 
.
∆∆ 
GetBytes
∆∆ &
(
∆∆& '
jwtSettings
∆∆' 2
[
∆∆2 3
$str
∆∆3 8
]
∆∆8 9
!
∆∆9 :
)
∆∆: ;
)
∆∆; <
;
∆∆< =
var
»» 
credentials
»» 
=
»» 
new
»» ! 
SigningCredentials
»»" 4
(
»»4 5
key
…… 
,
……  
SecurityAlgorithms
   "
.
  " #

HmacSha256
  # -
)
  - .
;
  . /
var
ÃÃ 
claims
ÃÃ 
=
ÃÃ 
new
ÃÃ 
List
ÃÃ !
<
ÃÃ! "
Claim
ÃÃ" '
>
ÃÃ' (
{
ÕÕ 
new
ŒŒ 
Claim
ŒŒ 
(
ŒŒ %
JwtRegisteredClaimNames
œœ +
.
œœ+ ,
Sub
œœ, /
,
œœ/ 0
user
–– 
.
–– 
UserId
–– 
.
––  
ToString
––  (
(
––( )
)
––) *
)
––* +
,
––+ ,
new
““ 
Claim
““ 
(
““ %
JwtRegisteredClaimNames
”” +
.
””+ ,
Email
””, 1
,
””1 2
user
‘‘ 
.
‘‘ 
Email
‘‘ 
)
‘‘ 
,
‘‘  
new
÷÷ 
Claim
÷÷ 
(
÷÷ %
JwtRegisteredClaimNames
◊◊ +
.
◊◊+ ,
Jti
◊◊, /
,
◊◊/ 0
Guid
ÿÿ 
.
ÿÿ 
NewGuid
ÿÿ  
(
ÿÿ  !
)
ÿÿ! "
.
ÿÿ" #
ToString
ÿÿ# +
(
ÿÿ+ ,
)
ÿÿ, -
)
ÿÿ- .
,
ÿÿ. /
new
⁄⁄ 
Claim
⁄⁄ 
(
⁄⁄ 

ClaimTypes
€€ 
.
€€ 
NameIdentifier
€€ -
,
€€- .
user
‹‹ 
.
‹‹ 
UserId
‹‹ 
.
‹‹  
ToString
‹‹  (
(
‹‹( )
)
‹‹) *
)
‹‹* +
,
‹‹+ ,
new
ﬁﬁ 
Claim
ﬁﬁ 
(
ﬁﬁ 

ClaimTypes
ﬂﬂ 
.
ﬂﬂ 
Role
ﬂﬂ #
,
ﬂﬂ# $
user
‡‡ 
.
‡‡ 
Role
‡‡ 
.
‡‡ 
ToString
‡‡ &
(
‡‡& '
)
‡‡' (
)
‡‡( )
,
‡‡) *
new
‚‚ 
Claim
‚‚ 
(
‚‚ 
$str
„„ !
,
„„! "
user
‰‰ 
.
‰‰ 
ReferenceId
‰‰ $
.
‰‰$ %
ToString
‰‰% -
(
‰‰- .
)
‰‰. /
)
‰‰/ 0
,
‰‰0 1
new
ÊÊ 
Claim
ÊÊ 
(
ÊÊ 
$str
ÁÁ (
,
ÁÁ( )
user
ËË 
.
ËË  
MustChangePassword
ËË +
.
ËË+ ,
ToString
ËË, 4
(
ËË4 5
)
ËË5 6
)
ËË6 7
}
ÈÈ 
;
ÈÈ 
var
ÎÎ 
token
ÎÎ 
=
ÎÎ 
new
ÎÎ 
JwtSecurityToken
ÎÎ ,
(
ÎÎ, -
issuer
ÏÏ 
:
ÏÏ 
jwtSettings
ÏÏ #
[
ÏÏ# $
$str
ÏÏ$ ,
]
ÏÏ, -
,
ÏÏ- .
audience
ÌÌ 
:
ÌÌ 
jwtSettings
ÌÌ %
[
ÌÌ% &
$str
ÌÌ& 0
]
ÌÌ0 1
,
ÌÌ1 2
claims
ÓÓ 
:
ÓÓ 
claims
ÓÓ 
,
ÓÓ 
expires
ÔÔ 
:
ÔÔ 
DateTime
ÔÔ !
.
ÔÔ! "
UtcNow
ÔÔ" (
.
ÔÔ( )

AddMinutes
ÔÔ) 3
(
ÔÔ3 4
int
 
.
 
Parse
 
(
 
jwtSettings
 )
[
) *
$str
* H
]
H I
!
I J
)
J K
)
K L
,
L M 
signingCredentials
ÒÒ "
:
ÒÒ" #
credentials
ÒÒ$ /
)
ÒÒ/ 0
;
ÒÒ0 1
return
ÛÛ 
new
ÛÛ %
JwtSecurityTokenHandler
ÛÛ .
(
ÛÛ. /
)
ÛÛ/ 0
.
ÙÙ 

WriteToken
ÙÙ 
(
ÙÙ 
token
ÙÙ !
)
ÙÙ! "
;
ÙÙ" #
}
ıı 	
private
˜˜ 
static
˜˜ 
string
˜˜ "
GenerateRefreshToken
˜˜ 2
(
˜˜2 3
)
˜˜3 4
{
¯¯ 	
return
˘˘ 
Convert
˘˘ 
.
˘˘ 
ToBase64String
˘˘ )
(
˘˘) *#
RandomNumberGenerator
˙˙ %
.
˙˙% &
GetBytes
˙˙& .
(
˙˙. /
$num
˙˙/ 1
)
˙˙1 2
)
˙˙2 3
;
˙˙3 4
}
˚˚ 	
private
˝˝ 
static
˝˝ 
string
˝˝ 
HashPassword
˝˝ *
(
˝˝* +
string
˝˝+ 1
password
˝˝2 :
)
˝˝: ;
{
˛˛ 	
using
ˇˇ 
var
ˇˇ 
sha256
ˇˇ 
=
ˇˇ 
SHA256
ˇˇ %
.
ˇˇ% &
Create
ˇˇ& ,
(
ˇˇ, -
)
ˇˇ- .
;
ˇˇ. /
var
ÅÅ 
bytes
ÅÅ 
=
ÅÅ 
Encoding
ÅÅ  
.
ÅÅ  !
UTF8
ÅÅ! %
.
ÅÅ% &
GetBytes
ÅÅ& .
(
ÅÅ. /
password
ÅÅ/ 7
)
ÅÅ7 8
;
ÅÅ8 9
var
ÇÇ 
hash
ÇÇ 
=
ÇÇ 
sha256
ÇÇ 
.
ÇÇ 
ComputeHash
ÇÇ )
(
ÇÇ) *
bytes
ÇÇ* /
)
ÇÇ/ 0
;
ÇÇ0 1
return
ÑÑ 
Convert
ÑÑ 
.
ÑÑ 
ToBase64String
ÑÑ )
(
ÑÑ) *
hash
ÑÑ* .
)
ÑÑ. /
;
ÑÑ/ 0
}
ÖÖ 	
}
ÜÜ 
}áá ®¿
lC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\AppointmentService.cs
	namespace		 	
S3_HealthAxisApi		
 
.		 
Services		 #
.		# $
Implementation		$ 2
{

 
public 

class 
AppointmentService #
:$ %
IAppointmentService& 9
{ 
private 
readonly "
IAppointmentRepository /"
_appointmentRepository0 F
;F G
private 
readonly 
IPatientRepository +
_patientRepository, >
;> ?
private 
readonly 
IDoctorRepository *
_doctorRepository+ <
;< =
public 
AppointmentService !
(! ""
IAppointmentRepository "!
appointmentRepository# 8
,8 9
IPatientRepository 
patientRepository 0
,0 1
IDoctorRepository 
doctorRepository .
). /
{ 	"
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
_patientRepository 
=  
patientRepository! 2
;2 3
_doctorRepository 
= 
doctorRepository  0
;0 1
} 	
public 
async 
Task 
< 
IEnumerable %
<% &!
AppointmentDetailsDto& ;
>; <
>< =
GetAllAsync> I
(I J
)J K
{ 	
var 
appointments 
= 
await $"
_appointmentRepository% ;
.; <
GetAllAsync< G
(G H
)H I
;I J
return   
appointments   
.    
Select    &
(  & '&
MapToAppointmentDetailsDto  ' A
)  A B
;  B C
}!! 	
public## 
async## 
Task## 
<## !
AppointmentDetailsDto## /
?##/ 0
>##0 1
GetByIdAsync##2 >
(##> ?
int##? B
id##C E
)##E F
{$$ 	
var%% 
appointment%% 
=%% 
await%% #"
_appointmentRepository%%$ :
.%%: ;
GetByIdAsync%%; G
(%%G H
id%%H J
)%%J K
;%%K L
if'' 
('' 
appointment'' 
=='' 
null'' #
)''# $
return(( 
null(( 
;(( 
return** &
MapToAppointmentDetailsDto** -
(**- .
appointment**. 9
)**9 :
;**: ;
}++ 	
public-- 
async-- 
Task-- 
<-- 
IEnumerable-- %
<--% &(
PatientAppointmentHistoryDto--& B
>--B C
>--C D"
GetPatientHistoryAsync--E [
(--[ \
int--\ _
	patientId--` i
)--i j
{.. 	
var// 
appointments// 
=// 
await// $"
_appointmentRepository//% ;
.//; <
GetByPatientIdAsync//< O
(//O P
	patientId//P Y
)//Y Z
;//Z [
return11 
appointments11 
.11  
Select11  &
(11& '
a11' (
=>11) +
new11, /(
PatientAppointmentHistoryDto110 L
{22 
AppointmentId33 
=33 
a33  !
.33! "
AppointmentId33" /
,33/ 0
ScheduledDate44 
=44 
a44  !
.44! "
ScheduledDate44" /
,44/ 0
TimeSlot55 
=55 
(55 
int55 
)55  
a55  !
.55! "
TimeSlot55" *
,55* +
DoctorId66 
=66 
a66 
.66 
DoctorId66 %
,66% &

DoctorName77 
=77 
a77 
.77 
Doctor77 %
.77% &
FullName77& .
,77. /
Status88 
=88 
(88 
int88 
)88 
a88 
.88  
Status88  &
}99 
)99 
;99 
}:: 	
public<< 
async<< 
Task<< 
<<< 
IEnumerable<< %
<<<% &!
DoctorScheduleItemDto<<& ;
><<; <
><<< ='
GetDoctorTodayScheduleAsync<<> Y
(<<Y Z
int<<Z ]
doctorId<<^ f
)<<f g
{== 	
var>> 
appointments>> 
=>> 
await?? "
_appointmentRepository?? ,
.??, -'
GetDoctorTodayScheduleAsync??- H
(??H I
doctorId@@ 
,@@ 
DateOnlyAA 
.AA 
FromDateTimeAA )
(AA) *
DateTimeAA* 2
.AA2 3
TodayAA3 8
)AA8 9
)AA9 :
;AA: ;
returnCC 
appointmentsCC 
.CC  
SelectCC  &
(CC& '!
MapDoctorScheduleItemCC' <
)CC< =
;CC= >
}DD 	
publicFF 
asyncFF 
TaskFF 
<FF 
IEnumerableFF %
<FF% &!
DoctorScheduleItemDtoFF& ;
>FF; <
>FF< =&
GetDoctorWeekScheduleAsyncFF> X
(FFX Y
intGG 
doctorIdGG 
,GG 
DateOnlyHH 
	startDateHH 
,HH 
DateOnlyII 
endDateII 
)II 
{JJ 	
varKK 
appointmentsKK 
=KK 
awaitLL "
_appointmentRepositoryLL ,
.LL, -&
GetDoctorWeekScheduleAsyncLL- G
(LLG H
doctorIdMM 
,MM 
	startDateNN 
,NN 
endDateOO 
)OO 
;OO 
returnQQ 
appointmentsQQ 
.QQ  
SelectQQ  &
(QQ& '!
MapDoctorScheduleItemQQ' <
)QQ< =
;QQ= >
}RR 	
publicTT 
asyncTT 
TaskTT 
<TT 
AppointmentDtoTT (
>TT( )
CreateAsyncTT* 5
(TT5 6 
CreateAppointmentDtoTT6 J
dtoTTK N
)TTN O
{UU 	
awaitVV  
ValidateBookingAsyncVV &
(VV& '
dtoWW 
.WW 
	PatientIdWW 
,WW 
dtoXX 
.XX 
DoctorIdXX 
,XX 
dtoYY 
.YY 
ScheduledDateYY !
,YY! "
dtoZZ 
.ZZ 
TimeSlotZZ 
)ZZ 
;ZZ 
var\\ 
appointment\\ 
=\\ 
new\\ !
Appointment\\" -
{]] 
	PatientId^^ 
=^^ 
dto^^ 
.^^  
	PatientId^^  )
,^^) *
DoctorId__ 
=__ 
dto__ 
.__ 
DoctorId__ '
,__' (
ScheduledDate`` 
=`` 
dto``  #
.``# $
ScheduledDate``$ 1
,``1 2
TimeSlotaa 
=aa 
(aa 
AppointmentTimeSlotaa /
)aa/ 0
dtoaa0 3
.aa3 4
TimeSlotaa4 <
,aa< =
Statusbb 
=bb 
AppointmentStatusbb *
.bb* +
Pendingbb+ 2
}cc 
;cc 
awaitee "
_appointmentRepositoryee (
.ee( )
AddAsyncee) 1
(ee1 2
appointmentee2 =
)ee= >
;ee> ?
awaitff "
_appointmentRepositoryff (
.ff( )
SaveChangesAsyncff) 9
(ff9 :
)ff: ;
;ff; <
returnhh 
MapToAppointmentDtohh &
(hh& '
appointmenthh' 2
)hh2 3
;hh3 4
}ii 	
publickk 
asynckk 
Taskkk 
UpdateAsynckk %
(kk% &
intkk& )
idkk* ,
,kk, - 
UpdateAppointmentDtokk. B
dtokkC F
)kkF G
{ll 	
varmm 
appointmentmm 
=mm 
awaitmm #"
_appointmentRepositorymm$ :
.mm: ;
GetByIdAsyncmm; G
(mmG H
idmmH J
)mmJ K
;mmK L
ifoo 
(oo 
appointmentoo 
==oo 
nulloo #
)oo# $
throwpp 
newpp  
KeyNotFoundExceptionpp .
(pp. /
$"pp/ 1
$strpp1 =
{pp= >
idpp> @
}pp@ A
$strppA L
"ppL M
)ppM N
;ppN O
ifrr 
(rr 
appointmentrr 
.rr 
Statusrr "
==rr# %
AppointmentStatusrr& 7
.rr7 8
	Completedrr8 A
)rrA B
throwss 
newss %
InvalidOperationExceptionss 3
(ss3 4
$strss4 `
)ss` a
;ssa b
ifuu 
(uu 
appointmentuu 
.uu 
Statusuu "
==uu# %
AppointmentStatusuu& 7
.uu7 8
	Cancelleduu8 A
)uuA B
throwvv 
newvv %
InvalidOperationExceptionvv 3
(vv3 4
$strvv4 `
)vv` a
;vva b
awaitxx &
ValidateUpdateBookingAsyncxx ,
(xx, -
appointmentyy 
.yy 
AppointmentIdyy )
,yy) *
appointmentzz 
.zz 
	PatientIdzz %
,zz% &
dto{{ 
.{{ 
DoctorId{{ 
,{{ 
dto|| 
.|| 
ScheduledDate|| !
,||! "
dto}} 
.}} 
TimeSlot}} 
)}} 
;}} 
appointment 
. 
DoctorId  
=! "
dto# &
.& '
DoctorId' /
;/ 0
appointment
ÄÄ 
.
ÄÄ 
ScheduledDate
ÄÄ %
=
ÄÄ& '
dto
ÄÄ( +
.
ÄÄ+ ,
ScheduledDate
ÄÄ, 9
;
ÄÄ9 :
appointment
ÅÅ 
.
ÅÅ 
TimeSlot
ÅÅ  
=
ÅÅ! "
(
ÅÅ# $!
AppointmentTimeSlot
ÅÅ$ 7
)
ÅÅ7 8
dto
ÅÅ8 ;
.
ÅÅ; <
TimeSlot
ÅÅ< D
;
ÅÅD E
await
ÉÉ $
_appointmentRepository
ÉÉ (
.
ÉÉ( )
UpdateAsync
ÉÉ) 4
(
ÉÉ4 5
appointment
ÉÉ5 @
)
ÉÉ@ A
;
ÉÉA B
await
ÑÑ $
_appointmentRepository
ÑÑ (
.
ÑÑ( )
SaveChangesAsync
ÑÑ) 9
(
ÑÑ9 :
)
ÑÑ: ;
;
ÑÑ; <
}
ÖÖ 	
public
áá 
async
áá 
Task
áá 
UpdateStatusAsync
áá +
(
áá+ ,
int
áá, /
id
áá0 2
,
áá2 3(
UpdateAppointmentStatusDto
áá4 N
dto
ááO R
)
ááR S
{
àà 	
var
ââ 
appointment
ââ 
=
ââ 
await
ââ #$
_appointmentRepository
ââ$ :
.
ââ: ;
GetByIdAsync
ââ; G
(
ââG H
id
ââH J
)
ââJ K
;
ââK L
if
ãã 
(
ãã 
appointment
ãã 
==
ãã 
null
ãã #
)
ãã# $
throw
åå 
new
åå "
KeyNotFoundException
åå .
(
åå. /
$"
åå/ 1
$str
åå1 =
{
åå= >
id
åå> @
}
åå@ A
$str
ååA L
"
ååL M
)
ååM N
;
ååN O
if
éé 
(
éé 
!
éé 
Enum
éé 
.
éé 
	IsDefined
éé 
(
éé  
typeof
éé  &
(
éé& '
AppointmentStatus
éé' 8
)
éé8 9
,
éé9 :
dto
éé; >
.
éé> ?
Status
éé? E
)
ééE F
)
ééF G
throw
èè 
new
èè 
ArgumentException
èè +
(
èè+ ,
$str
èè, I
)
èèI J
;
èèJ K
var
ëë 
	newStatus
ëë 
=
ëë 
(
ëë 
AppointmentStatus
ëë .
)
ëë. /
dto
ëë/ 2
.
ëë2 3
Status
ëë3 9
;
ëë9 :
if
ìì 
(
ìì 
appointment
ìì 
.
ìì 
Status
ìì "
==
ìì# %
AppointmentStatus
ìì& 7
.
ìì7 8
	Completed
ìì8 A
)
ììA B
throw
îî 
new
îî '
InvalidOperationException
îî 3
(
îî3 4
$str
îî4 `
)
îî` a
;
îîa b
if
ññ 
(
ññ 
appointment
ññ 
.
ññ 
Status
ññ "
==
ññ# %
AppointmentStatus
ññ& 7
.
ññ7 8
	Cancelled
ññ8 A
)
ññA B
throw
óó 
new
óó '
InvalidOperationException
óó 3
(
óó3 4
$str
óó4 `
)
óó` a
;
óóa b
switch
ôô 
(
ôô 
	newStatus
ôô 
)
ôô 
{
öö 
case
õõ 
AppointmentStatus
õõ &
.
õõ& '
Pending
õõ' .
:
õõ. /
throw
úú 
new
úú '
InvalidOperationException
úú 7
(
úú7 8
$str
úú8 m
)
úúm n
;
úún o
case
ûû 
AppointmentStatus
ûû &
.
ûû& '
	Confirmed
ûû' 0
:
ûû0 1
if
üü 
(
üü 
appointment
üü #
.
üü# $
Status
üü$ *
!=
üü+ -
AppointmentStatus
üü. ?
.
üü? @
Pending
üü@ G
)
üüG H
throw
†† 
new
†† !'
InvalidOperationException
††" ;
(
††; <
$str
††< i
)
††i j
;
††j k
appointment
¢¢ 
.
¢¢  
Status
¢¢  &
=
¢¢' (
AppointmentStatus
¢¢) :
.
¢¢: ;
	Confirmed
¢¢; D
;
¢¢D E
break
££ 
;
££ 
case
•• 
AppointmentStatus
•• &
.
••& '
	Completed
••' 0
:
••0 1
if
¶¶ 
(
¶¶ 
appointment
¶¶ #
.
¶¶# $
Status
¶¶$ *
!=
¶¶+ -
AppointmentStatus
¶¶. ?
.
¶¶? @
	Confirmed
¶¶@ I
)
¶¶I J
throw
ßß 
new
ßß !'
InvalidOperationException
ßß" ;
(
ßß; <
$str
ßß< k
)
ßßk l
;
ßßl m
appointment
©© 
.
©©  
Status
©©  &
=
©©' (
AppointmentStatus
©©) :
.
©©: ;
	Completed
©©; D
;
©©D E
break
™™ 
;
™™ 
case
¨¨ 
AppointmentStatus
¨¨ &
.
¨¨& '
	Cancelled
¨¨' 0
:
¨¨0 1
if
≠≠ 
(
≠≠ 
string
≠≠ 
.
≠≠  
IsNullOrWhiteSpace
≠≠ 1
(
≠≠1 2
dto
≠≠2 5
.
≠≠5 6 
CancellationReason
≠≠6 H
)
≠≠H I
)
≠≠I J
throw
ÆÆ 
new
ÆÆ !
ArgumentException
ÆÆ" 3
(
ÆÆ3 4
$str
ÆÆ4 V
)
ÆÆV W
;
ÆÆW X
appointment
∞∞ 
.
∞∞  
Status
∞∞  &
=
∞∞' (
AppointmentStatus
∞∞) :
.
∞∞: ;
	Cancelled
∞∞; D
;
∞∞D E
appointment
±± 
.
±±   
CancellationReason
±±  2
=
±±3 4
dto
±±5 8
.
±±8 9 
CancellationReason
±±9 K
.
±±K L
Trim
±±L P
(
±±P Q
)
±±Q R
;
±±R S
break
≤≤ 
;
≤≤ 
default
¥¥ 
:
¥¥ 
throw
µµ 
new
µµ 
ArgumentException
µµ /
(
µµ/ 0
$str
µµ0 M
)
µµM N
;
µµN O
}
∂∂ 
await
∏∏ $
_appointmentRepository
∏∏ (
.
∏∏( )
UpdateAsync
∏∏) 4
(
∏∏4 5
appointment
∏∏5 @
)
∏∏@ A
;
∏∏A B
await
ππ $
_appointmentRepository
ππ (
.
ππ( )
SaveChangesAsync
ππ) 9
(
ππ9 :
)
ππ: ;
;
ππ; <
}
∫∫ 	
public
ºº 
async
ºº 
Task
ºº 
ConfirmAsync
ºº &
(
ºº& '
int
ºº' *
id
ºº+ -
)
ºº- .
{
ΩΩ 	
var
ææ 
appointment
ææ 
=
ææ 
await
ææ #$
_appointmentRepository
ææ$ :
.
ææ: ;
GetByIdAsync
ææ; G
(
ææG H
id
ææH J
)
ææJ K
;
ææK L
if
¿¿ 
(
¿¿ 
appointment
¿¿ 
==
¿¿ 
null
¿¿ #
)
¿¿# $
throw
¡¡ 
new
¡¡ "
KeyNotFoundException
¡¡ .
(
¡¡. /
)
¡¡/ 0
;
¡¡0 1
if
√√ 
(
√√ 
appointment
√√ 
.
√√ 
Status
√√ "
!=
√√# %
AppointmentStatus
√√& 7
.
√√7 8
Pending
√√8 ?
)
√√? @
throw
ƒƒ 
new
ƒƒ '
InvalidOperationException
ƒƒ 3
(
ƒƒ3 4
$str
ƒƒ4 a
)
ƒƒa b
;
ƒƒb c
appointment
∆∆ 
.
∆∆ 
Status
∆∆ 
=
∆∆  
AppointmentStatus
∆∆! 2
.
∆∆2 3
	Confirmed
∆∆3 <
;
∆∆< =
await
»» $
_appointmentRepository
»» (
.
»»( )
UpdateAsync
»») 4
(
»»4 5
appointment
»»5 @
)
»»@ A
;
»»A B
await
…… $
_appointmentRepository
…… (
.
……( )
SaveChangesAsync
……) 9
(
……9 :
)
……: ;
;
……; <
}
   	
public
ÃÃ 
async
ÃÃ 
Task
ÃÃ 
CompleteAsync
ÃÃ '
(
ÃÃ' (
int
ÃÃ( +
id
ÃÃ, .
)
ÃÃ. /
{
ÕÕ 	
var
ŒŒ 
appointment
ŒŒ 
=
ŒŒ 
await
ŒŒ #$
_appointmentRepository
ŒŒ$ :
.
ŒŒ: ;
GetByIdAsync
ŒŒ; G
(
ŒŒG H
id
ŒŒH J
)
ŒŒJ K
;
ŒŒK L
if
–– 
(
–– 
appointment
–– 
==
–– 
null
–– #
)
––# $
throw
—— 
new
—— "
KeyNotFoundException
—— .
(
——. /
)
——/ 0
;
——0 1
if
”” 
(
”” 
appointment
”” 
.
”” 
Status
”” "
!=
””# %
AppointmentStatus
””& 7
.
””7 8
	Confirmed
””8 A
)
””A B
throw
‘‘ 
new
‘‘ '
InvalidOperationException
‘‘ 3
(
‘‘3 4
$str
‘‘4 c
)
‘‘c d
;
‘‘d e
appointment
÷÷ 
.
÷÷ 
Status
÷÷ 
=
÷÷  
AppointmentStatus
÷÷! 2
.
÷÷2 3
	Completed
÷÷3 <
;
÷÷< =
await
ÿÿ $
_appointmentRepository
ÿÿ (
.
ÿÿ( )
UpdateAsync
ÿÿ) 4
(
ÿÿ4 5
appointment
ÿÿ5 @
)
ÿÿ@ A
;
ÿÿA B
await
ŸŸ $
_appointmentRepository
ŸŸ (
.
ŸŸ( )
SaveChangesAsync
ŸŸ) 9
(
ŸŸ9 :
)
ŸŸ: ;
;
ŸŸ; <
}
⁄⁄ 	
public
‹‹ 
async
‹‹ 
Task
‹‹ 
CancelAsync
‹‹ %
(
‹‹% &
int
‹‹& )
id
‹‹* ,
,
‹‹, -"
CancelAppointmentDto
‹‹. B
dto
‹‹C F
)
‹‹F G
{
›› 	
var
ﬁﬁ 
appointment
ﬁﬁ 
=
ﬁﬁ 
await
ﬁﬁ #$
_appointmentRepository
ﬁﬁ$ :
.
ﬁﬁ: ;
GetByIdAsync
ﬁﬁ; G
(
ﬁﬁG H
id
ﬁﬁH J
)
ﬁﬁJ K
;
ﬁﬁK L
if
‡‡ 
(
‡‡ 
appointment
‡‡ 
==
‡‡ 
null
‡‡ #
)
‡‡# $
throw
·· 
new
·· "
KeyNotFoundException
·· .
(
··. /
)
··/ 0
;
··0 1
if
„„ 
(
„„ 
appointment
„„ 
.
„„ 
Status
„„ "
==
„„# %
AppointmentStatus
„„& 7
.
„„7 8
	Completed
„„8 A
)
„„A B
throw
‰‰ 
new
‰‰ '
InvalidOperationException
‰‰ 3
(
‰‰3 4
$str
‰‰4 a
)
‰‰a b
;
‰‰b c
if
ÊÊ 
(
ÊÊ 
appointment
ÊÊ 
.
ÊÊ 
Status
ÊÊ "
==
ÊÊ# %
AppointmentStatus
ÊÊ& 7
.
ÊÊ7 8
	Cancelled
ÊÊ8 A
)
ÊÊA B
throw
ÁÁ 
new
ÁÁ '
InvalidOperationException
ÁÁ 3
(
ÁÁ3 4
$str
ÁÁ4 T
)
ÁÁT U
;
ÁÁU V
if
ÈÈ 
(
ÈÈ 
string
ÈÈ 
.
ÈÈ  
IsNullOrWhiteSpace
ÈÈ )
(
ÈÈ) *
dto
ÈÈ* -
.
ÈÈ- . 
CancellationReason
ÈÈ. @
)
ÈÈ@ A
)
ÈÈA B
throw
ÍÍ 
new
ÍÍ 
ArgumentException
ÍÍ +
(
ÍÍ+ ,
$str
ÍÍ, N
)
ÍÍN O
;
ÍÍO P
appointment
ÏÏ 
.
ÏÏ 
Status
ÏÏ 
=
ÏÏ  
AppointmentStatus
ÏÏ! 2
.
ÏÏ2 3
	Cancelled
ÏÏ3 <
;
ÏÏ< =
appointment
ÌÌ 
.
ÌÌ  
CancellationReason
ÌÌ *
=
ÌÌ+ ,
dto
ÌÌ- 0
.
ÌÌ0 1 
CancellationReason
ÌÌ1 C
.
ÌÌC D
Trim
ÌÌD H
(
ÌÌH I
)
ÌÌI J
;
ÌÌJ K
await
ÔÔ $
_appointmentRepository
ÔÔ (
.
ÔÔ( )
UpdateAsync
ÔÔ) 4
(
ÔÔ4 5
appointment
ÔÔ5 @
)
ÔÔ@ A
;
ÔÔA B
await
 $
_appointmentRepository
 (
.
( )
SaveChangesAsync
) 9
(
9 :
)
: ;
;
; <
}
ÒÒ 	
public
ÛÛ 
async
ÛÛ 
Task
ÛÛ 
<
ÛÛ 
IEnumerable
ÛÛ %
<
ÛÛ% &#
DoctorScheduleItemDto
ÛÛ& ;
>
ÛÛ; <
>
ÛÛ< =,
GetDoctorUpcomingScheduleAsync
ÛÛ> \
(
ÛÛ\ ]
int
ÛÛ] `
doctorId
ÛÛa i
)
ÛÛi j
{
ÙÙ 	
var
ıı 
	startDate
ıı 
=
ıı 
DateOnly
ıı $
.
ıı$ %
FromDateTime
ıı% 1
(
ıı1 2
DateTime
ıı2 :
.
ıı: ;
Today
ıı; @
)
ıı@ A
;
ııA B
var
ˆˆ 
endDate
ˆˆ 
=
ˆˆ 
	startDate
ˆˆ #
.
ˆˆ# $
AddDays
ˆˆ$ +
(
ˆˆ+ ,
$num
ˆˆ, -
)
ˆˆ- .
;
ˆˆ. /
var
¯¯ 
appointments
¯¯ 
=
¯¯ 
await
˘˘ $
_appointmentRepository
˘˘ ,
.
˘˘, -(
GetDoctorWeekScheduleAsync
˘˘- G
(
˘˘G H
doctorId
˙˙ 
,
˙˙ 
	startDate
˚˚ 
,
˚˚ 
endDate
¸¸ 
)
¸¸ 
;
¸¸ 
return
˛˛ 
appointments
˛˛ 
.
˛˛  
Select
˛˛  &
(
˛˛& '#
MapDoctorScheduleItem
˛˛' <
)
˛˛< =
;
˛˛= >
}
ˇˇ 	
private
ÅÅ 
async
ÅÅ 
Task
ÅÅ "
ValidateBookingAsync
ÅÅ /
(
ÅÅ/ 0
int
ÅÅ0 3
	patientId
ÅÅ4 =
,
ÅÅ= >
int
ÅÅ? B
doctorId
ÅÅC K
,
ÅÅK L
DateOnly
ÅÅM U
date
ÅÅV Z
,
ÅÅZ [
int
ÅÅ\ _
timeSlot
ÅÅ` h
)
ÅÅh i
{
ÇÇ 	
var
ÉÉ 
patient
ÉÉ 
=
ÉÉ 
await
ÉÉ  
_patientRepository
ÉÉ  2
.
ÉÉ2 3
GetByIdAsync
ÉÉ3 ?
(
ÉÉ? @
	patientId
ÉÉ@ I
)
ÉÉI J
;
ÉÉJ K
if
ÖÖ 
(
ÖÖ 
patient
ÖÖ 
==
ÖÖ 
null
ÖÖ 
)
ÖÖ  
throw
ÜÜ 
new
ÜÜ "
KeyNotFoundException
ÜÜ .
(
ÜÜ. /
$str
ÜÜ/ C
)
ÜÜC D
;
ÜÜD E
if
àà 
(
àà 
!
àà 
patient
àà 
.
àà 
IsActive
àà !
)
àà! "
throw
ââ 
new
ââ '
InvalidOperationException
ââ 3
(
ââ3 4
$str
ââ4 a
)
ââa b
;
ââb c
var
ãã 
doctor
ãã 
=
ãã 
await
ãã 
_doctorRepository
ãã 0
.
ãã0 1
GetByIdAsync
ãã1 =
(
ãã= >
doctorId
ãã> F
)
ããF G
;
ããG H
if
çç 
(
çç 
doctor
çç 
==
çç 
null
çç 
)
çç 
throw
éé 
new
éé "
KeyNotFoundException
éé .
(
éé. /
$str
éé/ B
)
ééB C
;
ééC D
if
êê 
(
êê 
!
êê 
doctor
êê 
.
êê 
IsActive
êê  
)
êê  !
throw
ëë 
new
ëë '
InvalidOperationException
ëë 3
(
ëë3 4
$str
ëë4 F
)
ëëF G
;
ëëG H
if
ìì 
(
ìì 
date
ìì 
<
ìì 
DateOnly
ìì 
.
ìì  
FromDateTime
ìì  ,
(
ìì, -
DateTime
ìì- 5
.
ìì5 6
Today
ìì6 ;
)
ìì; <
)
ìì< =
throw
îî 
new
îî 
ArgumentException
îî +
(
îî+ ,
$str
îî, U
)
îîU V
;
îîV W
if
ññ 
(
ññ 
!
ññ 
Enum
ññ 
.
ññ 
	IsDefined
ññ 
(
ññ  
typeof
ññ  &
(
ññ& '!
AppointmentTimeSlot
ññ' :
)
ññ: ;
,
ññ; <
timeSlot
ññ= E
)
ññE F
)
ññF G
throw
óó 
new
óó 
ArgumentException
óó +
(
óó+ ,
$str
óó, G
)
óóG H
;
óóH I
if
ôô 
(
ôô 
await
ôô $
_appointmentRepository
ôô ,
.
ôô, -6
(ExistsSamePatientSameDoctorSameDateAsync
ôô- U
(
ôôU V
	patientId
ôôV _
,
ôô_ `
doctorId
ôôa i
,
ôôi j
date
ôôk o
)
ôôo p
)
ôôp q
throw
öö 
new
öö '
InvalidOperationException
öö 3
(
öö3 4
$str
öö4 
)öö Ä
;ööÄ Å
if
úú 
(
úú 
await
úú $
_appointmentRepository
úú ,
.
úú, -4
&ExistsSamePatientSameSlotSameDateAsync
úú- S
(
úúS T
	patientId
úúT ]
,
úú] ^
date
úú_ c
,
úúc d
timeSlot
úúe m
)
úúm n
)
úún o
throw
ùù 
new
ùù '
InvalidOperationException
ùù 3
(
ùù3 4
$str
ùù4 p
)
ùùp q
;
ùùq r
if
üü 
(
üü 
await
üü $
_appointmentRepository
üü ,
.
üü, -3
%ExistsSameDoctorSameSlotSameDateAsync
üü- R
(
üüR S
doctorId
üüS [
,
üü[ \
date
üü] a
,
üüa b
timeSlot
üüc k
)
üük l
)
üül m
throw
†† 
new
†† '
InvalidOperationException
†† 3
(
††3 4
$str
††4 b
)
††b c
;
††c d
}
°° 	
private
££ 
async
££ 
Task
££ (
ValidateUpdateBookingAsync
££ 5
(
££5 6
int
££6 9
appointmentId
££: G
,
££G H
int
££I L
	patientId
££M V
,
££V W
int
££X [
doctorId
££\ d
,
££d e
DateOnly
££f n
date
££o s
,
££s t
int
££u x
timeSlot££y Å
)££Å Ç
{
§§ 	
var
•• 
patient
•• 
=
•• 
await
••  
_patientRepository
••  2
.
••2 3
GetByIdAsync
••3 ?
(
••? @
	patientId
••@ I
)
••I J
;
••J K
if
ßß 
(
ßß 
patient
ßß 
==
ßß 
null
ßß 
)
ßß  
throw
®® 
new
®® "
KeyNotFoundException
®® .
(
®®. /
$str
®®/ C
)
®®C D
;
®®D E
if
™™ 
(
™™ 
!
™™ 
patient
™™ 
.
™™ 
IsActive
™™ !
)
™™! "
throw
´´ 
new
´´ '
InvalidOperationException
´´ 3
(
´´3 4
$str
´´4 a
)
´´a b
;
´´b c
var
≠≠ 
doctor
≠≠ 
=
≠≠ 
await
≠≠ 
_doctorRepository
≠≠ 0
.
≠≠0 1
GetByIdAsync
≠≠1 =
(
≠≠= >
doctorId
≠≠> F
)
≠≠F G
;
≠≠G H
if
ØØ 
(
ØØ 
doctor
ØØ 
==
ØØ 
null
ØØ 
)
ØØ 
throw
∞∞ 
new
∞∞ "
KeyNotFoundException
∞∞ .
(
∞∞. /
$str
∞∞/ B
)
∞∞B C
;
∞∞C D
if
≤≤ 
(
≤≤ 
!
≤≤ 
doctor
≤≤ 
.
≤≤ 
IsActive
≤≤  
)
≤≤  !
throw
≥≥ 
new
≥≥ '
InvalidOperationException
≥≥ 3
(
≥≥3 4
$str
≥≥4 F
)
≥≥F G
;
≥≥G H
if
µµ 
(
µµ 
date
µµ 
<
µµ 
DateOnly
µµ 
.
µµ  
FromDateTime
µµ  ,
(
µµ, -
DateTime
µµ- 5
.
µµ5 6
Today
µµ6 ;
)
µµ; <
)
µµ< =
throw
∂∂ 
new
∂∂ 
ArgumentException
∂∂ +
(
∂∂+ ,
$str
∂∂, U
)
∂∂U V
;
∂∂V W
if
∏∏ 
(
∏∏ 
!
∏∏ 
Enum
∏∏ 
.
∏∏ 
	IsDefined
∏∏ 
(
∏∏  
typeof
∏∏  &
(
∏∏& '!
AppointmentTimeSlot
∏∏' :
)
∏∏: ;
,
∏∏; <
timeSlot
∏∏= E
)
∏∏E F
)
∏∏F G
throw
ππ 
new
ππ 
ArgumentException
ππ +
(
ππ+ ,
$str
ππ, G
)
ππG H
;
ππH I
if
ªª 
(
ªª 
await
ªª $
_appointmentRepository
ªª ,
.
ªª, -6
(ExistsSamePatientSameDoctorSameDateAsync
ªª- U
(
ªªU V
	patientId
ªªV _
,
ªª_ `
doctorId
ªªa i
,
ªªi j
date
ªªk o
,
ªªo p
appointmentId
ªªq ~
)
ªª~ 
)ªª Ä
throw
ºº 
new
ºº '
InvalidOperationException
ºº 3
(
ºº3 4
$str
ºº4 
)ºº Ä
;ººÄ Å
if
ææ 
(
ææ 
await
ææ $
_appointmentRepository
ææ ,
.
ææ, -4
&ExistsSamePatientSameSlotSameDateAsync
ææ- S
(
ææS T
	patientId
ææT ]
,
ææ] ^
date
ææ_ c
,
ææc d
timeSlot
ææe m
,
ææm n
appointmentId
ææo |
)
ææ| }
)
ææ} ~
throw
øø 
new
øø '
InvalidOperationException
øø 3
(
øø3 4
$str
øø4 p
)
øøp q
;
øøq r
if
¡¡ 
(
¡¡ 
await
¡¡ $
_appointmentRepository
¡¡ ,
.
¡¡, -3
%ExistsSameDoctorSameSlotSameDateAsync
¡¡- R
(
¡¡R S
doctorId
¡¡S [
,
¡¡[ \
date
¡¡] a
,
¡¡a b
timeSlot
¡¡c k
,
¡¡k l
appointmentId
¡¡m z
)
¡¡z {
)
¡¡{ |
throw
¬¬ 
new
¬¬ '
InvalidOperationException
¬¬ 3
(
¬¬3 4
$str
¬¬4 b
)
¬¬b c
;
¬¬c d
}
√√ 	
private
«« 
static
«« 
AppointmentDto
«« %!
MapToAppointmentDto
««& 9
(
««9 :
Appointment
««: E
appointment
««F Q
)
««Q R
{
»» 	
return
…… 
new
…… 
AppointmentDto
…… %
{
   
AppointmentId
ÀÀ 
=
ÀÀ 
appointment
ÀÀ  +
.
ÀÀ+ ,
AppointmentId
ÀÀ, 9
,
ÀÀ9 :
	PatientId
ÃÃ 
=
ÃÃ 
appointment
ÃÃ '
.
ÃÃ' (
	PatientId
ÃÃ( 1
,
ÃÃ1 2
DoctorId
ÕÕ 
=
ÕÕ 
appointment
ÕÕ &
.
ÕÕ& '
DoctorId
ÕÕ' /
,
ÕÕ/ 0
ScheduledDate
ŒŒ 
=
ŒŒ 
appointment
ŒŒ  +
.
ŒŒ+ ,
ScheduledDate
ŒŒ, 9
,
ŒŒ9 :
TimeSlot
œœ 
=
œœ 
(
œœ 
int
œœ 
)
œœ  
appointment
œœ  +
.
œœ+ ,
TimeSlot
œœ, 4
,
œœ4 5
Status
–– 
=
–– 
(
–– 
int
–– 
)
–– 
appointment
–– )
.
––) *
Status
––* 0
,
––0 1 
CancellationReason
—— "
=
——# $
appointment
——% 0
.
——0 1 
CancellationReason
——1 C
}
““ 
;
““ 
}
”” 	
private
÷÷ 
static
÷÷ #
AppointmentDetailsDto
÷÷ ,(
MapToAppointmentDetailsDto
÷÷- G
(
÷÷G H
Appointment
÷÷H S
appointment
÷÷T _
)
÷÷_ `
{
◊◊ 	
return
ÿÿ 
new
ÿÿ #
AppointmentDetailsDto
ÿÿ ,
{
ŸŸ 
AppointmentId
⁄⁄ 
=
⁄⁄ 
appointment
⁄⁄  +
.
⁄⁄+ ,
AppointmentId
⁄⁄, 9
,
⁄⁄9 :
	PatientId
€€ 
=
€€ 
appointment
€€ '
.
€€' (
	PatientId
€€( 1
,
€€1 2
PatientName
‹‹ 
=
‹‹ 
appointment
‹‹ )
.
‹‹) *
Patient
‹‹* 1
?
‹‹1 2
.
‹‹2 3
FullName
‹‹3 ;
??
‹‹< >
string
‹‹? E
.
‹‹E F
Empty
‹‹F K
,
‹‹K L
DoctorId
›› 
=
›› 
appointment
›› &
.
››& '
DoctorId
››' /
,
››/ 0

DoctorName
ﬁﬁ 
=
ﬁﬁ 
appointment
ﬁﬁ (
.
ﬁﬁ( )
Doctor
ﬁﬁ) /
?
ﬁﬁ/ 0
.
ﬁﬁ0 1
FullName
ﬁﬁ1 9
??
ﬁﬁ: <
string
ﬁﬁ= C
.
ﬁﬁC D
Empty
ﬁﬁD I
,
ﬁﬁI J
ScheduledDate
ﬂﬂ 
=
ﬂﬂ 
appointment
ﬂﬂ  +
.
ﬂﬂ+ ,
ScheduledDate
ﬂﬂ, 9
,
ﬂﬂ9 :
TimeSlot
‡‡ 
=
‡‡ 
(
‡‡ 
int
‡‡ 
)
‡‡  
appointment
‡‡  +
.
‡‡+ ,
TimeSlot
‡‡, 4
,
‡‡4 5
Status
·· 
=
·· 
(
·· 
int
·· 
)
·· 
appointment
·· )
.
··) *
Status
··* 0
,
··0 1 
CancellationReason
‚‚ "
=
‚‚# $
appointment
‚‚% 0
.
‚‚0 1 
CancellationReason
‚‚1 C
}
„„ 
;
„„ 
}
‰‰ 	
public
ÂÂ 
async
ÂÂ 
Task
ÂÂ 
<
ÂÂ 
IEnumerable
ÂÂ %
<
ÂÂ% &
DoctorPatientDto
ÂÂ& 6
>
ÂÂ6 7
>
ÂÂ7 8$
GetDoctorPatientsAsync
ÂÂ9 O
(
ÂÂO P
int
ÂÂP S
doctorId
ÂÂT \
)
ÂÂ\ ]
{
ÊÊ 	
var
ÁÁ 
doctor
ÁÁ 
=
ÁÁ 
await
ÁÁ 
_doctorRepository
ÁÁ 0
.
ÁÁ0 1
GetByIdAsync
ÁÁ1 =
(
ÁÁ= >
doctorId
ÁÁ> F
)
ÁÁF G
;
ÁÁG H
if
ÈÈ 
(
ÈÈ 
doctor
ÈÈ 
==
ÈÈ 
null
ÈÈ 
)
ÈÈ 
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ "
KeyNotFoundException
ÎÎ .
(
ÎÎ. /
$"
ÎÎ/ 1
$str
ÎÎ1 @
{
ÎÎ@ A
doctorId
ÎÎA I
}
ÎÎI J
$str
ÎÎJ U
"
ÎÎU V
)
ÎÎV W
;
ÎÎW X
}
ÏÏ 
var
ÓÓ 
appointments
ÓÓ 
=
ÓÓ 
await
ÔÔ $
_appointmentRepository
ÔÔ ,
.
ÔÔ, -/
!GetDoctorPatientAppointmentsAsync
ÔÔ- N
(
ÔÔN O
doctorId
ÔÔO W
)
ÔÔW X
;
ÔÔX Y
var
ÒÒ 
patients
ÒÒ 
=
ÒÒ 
appointments
ÒÒ '
.
ÚÚ 
Where
ÚÚ 
(
ÚÚ 
a
ÚÚ 
=>
ÚÚ 
a
ÚÚ 
.
ÚÚ 
Patient
ÚÚ %
!=
ÚÚ& (
null
ÚÚ) -
)
ÚÚ- .
.
ÛÛ 
GroupBy
ÛÛ 
(
ÛÛ 
a
ÛÛ 
=>
ÛÛ 
a
ÛÛ 
.
ÛÛ  
	PatientId
ÛÛ  )
)
ÛÛ) *
.
ÙÙ 
Select
ÙÙ 
(
ÙÙ 
group
ÙÙ 
=>
ÙÙ  
{
ıı 
var
ˆˆ 
latestAppointment
ˆˆ )
=
ˆˆ* +
group
ˆˆ, 1
.
˜˜ 
OrderByDescending
˜˜ *
(
˜˜* +
a
˜˜+ ,
=>
˜˜- /
a
˜˜0 1
.
˜˜1 2
ScheduledDate
˜˜2 ?
)
˜˜? @
.
¯¯ 
ThenByDescending
¯¯ )
(
¯¯) *
a
¯¯* +
=>
¯¯, .
a
¯¯/ 0
.
¯¯0 1
TimeSlot
¯¯1 9
)
¯¯9 :
.
˘˘ 
First
˘˘ 
(
˘˘ 
)
˘˘  
;
˘˘  !
var
˚˚ 
patient
˚˚ 
=
˚˚  !
latestAppointment
˚˚" 3
.
˚˚3 4
Patient
˚˚4 ;
;
˚˚; <
return
˝˝ 
new
˝˝ 
DoctorPatientDto
˝˝ /
{
˛˛ 
	PatientId
ˇˇ !
=
ˇˇ" #
patient
ˇˇ$ +
.
ˇˇ+ ,
	PatientId
ˇˇ, 5
,
ˇˇ5 6
FullName
ÄÄ  
=
ÄÄ! "
patient
ÄÄ# *
.
ÄÄ* +
FullName
ÄÄ+ 3
,
ÄÄ3 4
DateOfBirth
ÅÅ #
=
ÅÅ$ %
patient
ÅÅ& -
.
ÅÅ- .
DateOfBirth
ÅÅ. 9
,
ÅÅ9 :
Gender
ÇÇ 
=
ÇÇ  
patient
ÇÇ! (
.
ÇÇ( )
Gender
ÇÇ) /
,
ÇÇ/ 0
PhoneNumber
ÉÉ #
=
ÉÉ$ %
patient
ÉÉ& -
.
ÉÉ- .
PhoneNumber
ÉÉ. 9
,
ÉÉ9 :
Email
ÑÑ 
=
ÑÑ 
patient
ÑÑ  '
.
ÑÑ' (
Email
ÑÑ( -
,
ÑÑ- .
InsuranceId
ÖÖ #
=
ÖÖ$ %
patient
ÖÖ& -
.
ÖÖ- .
InsuranceNumber
ÖÖ. =
,
ÖÖ= >
IsActive
ÜÜ  
=
ÜÜ! "
patient
ÜÜ# *
.
ÜÜ* +
IsActive
ÜÜ+ 3
,
ÜÜ3 4
TotalAppointments
áá )
=
áá* +
group
áá, 1
.
áá1 2
Count
áá2 7
(
áá7 8
)
áá8 9
,
áá9 :
LastVisitDate
àà %
=
àà& '
latestAppointment
àà( 9
.
àà9 :
ScheduledDate
àà: G
}
ââ 
;
ââ 
}
ää 
)
ää 
.
ãã 
OrderBy
ãã 
(
ãã 
p
ãã 
=>
ãã 
p
ãã 
.
ãã  
FullName
ãã  (
)
ãã( )
.
åå 
ToList
åå 
(
åå 
)
åå 
;
åå 
return
éé 
patients
éé 
;
éé 
}
èè 	
private
ëë 
static
ëë #
DoctorScheduleItemDto
ëë ,#
MapDoctorScheduleItem
ëë- B
(
ëëB C
Appointment
ëëC N
appointment
ëëO Z
)
ëëZ [
{
íí 	
return
ìì 
new
ìì #
DoctorScheduleItemDto
ìì ,
{
îî 
AppointmentId
ïï 
=
ïï 
appointment
ïï  +
.
ïï+ ,
AppointmentId
ïï, 9
,
ïï9 :
ScheduledDate
ññ 
=
ññ 
appointment
ññ  +
.
ññ+ ,
ScheduledDate
ññ, 9
,
ññ9 :
TimeSlot
óó 
=
óó 
(
óó 
int
óó 
)
óó  
appointment
óó  +
.
óó+ ,
TimeSlot
óó, 4
,
óó4 5
	PatientId
òò 
=
òò 
appointment
òò '
.
òò' (
	PatientId
òò( 1
,
òò1 2
PatientName
ôô 
=
ôô 
appointment
ôô )
.
ôô) *
Patient
ôô* 1
.
ôô1 2
FullName
ôô2 :
,
ôô: ;
Status
öö 
=
öö 
(
öö 
int
öö 
)
öö 
appointment
öö )
.
öö) *
Status
öö* 0
,
öö0 1 
CancellationReason
õõ "
=
õõ# $
appointment
õõ% 0
.
õõ0 1 
CancellationReason
õõ1 C
,
õõC D
HasHealthRecord
úú 
=
úú  !
appointment
úú" -
.
úú- .
HealthRecord
úú. :
!=
úú; =
null
úú> B
}
ùù 
;
ùù 
}
ûû 	
}
üü 
}†† ê3
fC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Services\Implementation\AdminService.cs
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
}QQ •
hC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IUserRepository.cs
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
} „
kC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IPatientRepository.cs
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
} Ô

pC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IHealthRecordRepository.cs
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
Task		 
<		 
HealthRecord		 
?		 
>		 #
GetByAppointmentIdAsync		 3
(		3 4
int		4 7
appointmentId		8 E
)		E F
;		F G
Task 
< 
IEnumerable 
< 
HealthRecord %
>% &
>& '
GetByPatientIdAsync( ;
(; <
int< ?
	patientId@ I
)I J
;J K
Task 
AddAsync 
( 
HealthRecord "
record# )
)) *
;* +
Task 
UpdateAsync 
( 
HealthRecord %
record& ,
), -
;- .
Task 
SaveChangesAsync 
( 
) 
;  
} 
} Ë

kC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IGenericRepository.cs
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
} î
jC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IDoctorRepository.cs
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
} ∂#
oC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IAppointmentRepository.cs
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
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &-
!GetDoctorPatientAppointmentsAsync' H
(H I
intI L
doctorIdM U
)U V
;V W
Task 
AddAsync 
( 
Appointment !
appointment" -
)- .
;. /
Task 
UpdateAsync 
( 
Appointment $
appointment% 0
)0 1
;1 2
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ˇ
iC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Interface\IAdminRepository.cs
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
bool 
> (
ResolveUserActiveStatusAsync /
(/ 0
string0 6
email7 <
,< =
string> D
roleE I
)I J
;J K
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
} Û$
lC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\UserRepository.cs
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
.! "
AppUsers" *
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
.! "
AppUsers" *
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
.""! "
AppUsers""" *
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
.)) 
AppUsers)) #
.))# $
AddAsync))$ ,
()), -
user))- 1
)))1 2
;))2 3
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
... 
AppUsers.. 
... 
Update.. $
(..$ %
user..% )
)..) *
;..* +
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
.44! "
AppUsers44" *
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
.??! "
AppUsers??" *
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
}CC Ø&
oC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\PatientRepository.cs
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
}BB ñ)
tC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\HealthRecordRepository.cs
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
( 
r 
=> 
r 
.  
Doctor  &
)& '
. 
Include 
( 
r 
=> 
r 
.  
Patient  '
)' (
. 
Include 
( 
r 
=> 
r 
.  
Appointment  +
)+ ,
. 
FirstOrDefaultAsync $
($ %
r% &
=>' )
r* +
.+ ,
HealthRecordId, :
==; =
id> @
)@ A
;A B
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
( 
r 
=> 
r 
.  
Doctor  &
)& '
.   
Include   
(   
r   
=>   
r   
.    
Patient    '
)  ' (
.!! 
Include!! 
(!! 
r!! 
=>!! 
r!! 
.!!  
Appointment!!  +
)!!+ ,
."" 
FirstOrDefaultAsync"" $
(""$ %
r""% &
=>""' )
r""* +
.""+ ,
AppointmentId"", 9
=="": <
appointmentId""= J
)""J K
;""K L
}## 	
public%% 
async%% 
Task%% 
<%% 
IEnumerable%% %
<%%% &
HealthRecord%%& 2
>%%2 3
>%%3 4
GetByPatientIdAsync%%5 H
(%%H I
int%%I L
	patientId%%M V
)%%V W
{&& 	
return'' 
await'' 
_context'' !
.''! "
HealthRecords''" /
.(( 
Include(( 
((( 
r(( 
=>(( 
r(( 
.((  
Doctor((  &
)((& '
.)) 
Include)) 
()) 
r)) 
=>)) 
r)) 
.))  
Patient))  '
)))' (
.** 
Include** 
(** 
r** 
=>** 
r** 
.**  
Appointment**  +
)**+ ,
.++ 
Where++ 
(++ 
r++ 
=>++ 
r++ 
.++ 
	PatientId++ '
==++( *
	patientId+++ 4
)++4 5
.,, 
OrderByDescending,, "
(,," #
r,,# $
=>,,% '
r,,( )
.,,) *
	CreatedOn,,* 3
),,3 4
.-- 
ToListAsync-- 
(-- 
)-- 
;-- 
}.. 	
public00 
async00 
Task00 
AddAsync00 "
(00" #
HealthRecord00# /
record000 6
)006 7
{11 	
await22 
_context22 
.22 
HealthRecords22 (
.22( )
AddAsync22) 1
(221 2
record222 8
)228 9
;229 :
}33 	
public55 
Task55 
UpdateAsync55 
(55  
HealthRecord55  ,
record55- 3
)553 4
{66 	
_context77 
.77 
HealthRecords77 "
.77" #
Update77# )
(77) *
record77* 0
)770 1
;771 2
return88 
Task88 
.88 
CompletedTask88 %
;88% &
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
}@@ §!
oC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\GenericRepository.cs
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
};; ˛@
nC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\DoctorRepository.cs
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
}^^ ‚í
sC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\AppointmentRepository.cs
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
class !
AppointmentRepository &
:' ("
IAppointmentRepository) ?
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public !
AppointmentRepository $
($ %
HealthAxisDbContext% 8
context9 @
)@ A
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
<% &
Appointment& 1
>1 2
>2 3
GetAllAsync4 ?
(? @
)@ A
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
. 
Include 
( 
a 
=> 
a 
.  
Doctor  &
)& '
. 
Include 
( 
a 
=> 
a 
.  
HealthRecord  ,
), -
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
.$$ 
Include$$ 
($$ 
a$$ 
=>$$ 
a$$ 
.$$  
HealthRecord$$  ,
)$$, -
.%% 
FirstOrDefaultAsync%% $
(%%$ %
a%%% &
=>%%' )
a%%* +
.%%+ ,
AppointmentId%%, 9
==%%: <
id%%= ?
)%%? @
;%%@ A
}&& 	
public(( 
async(( 
Task(( 
<(( 
IEnumerable(( %
<((% &
Appointment((& 1
>((1 2
>((2 3
GetByPatientIdAsync((4 G
(((G H
int((H K
	patientId((L U
)((U V
{)) 	
return** 
await** 
_context** !
.**! "
Appointments**" .
.++ 
Include++ 
(++ 
a++ 
=>++ 
a++ 
.++  
Doctor++  &
)++& '
.,, 
Include,, 
(,, 
a,, 
=>,, 
a,, 
.,,  
HealthRecord,,  ,
),,, -
.-- 
Where-- 
(-- 
a-- 
=>-- 
a-- 
.-- 
	PatientId-- '
==--( *
	patientId--+ 4
)--4 5
... 
OrderByDescending.. "
(.." #
a..# $
=>..% '
a..( )
...) *
ScheduledDate..* 7
)..7 8
.// 
ThenBy// 
(// 
a// 
=>// 
a// 
.// 
TimeSlot// '
)//' (
.00 
ToListAsync00 
(00 
)00 
;00 
}11 	
public33 
async33 
Task33 
<33 
IEnumerable33 %
<33% &
Appointment33& 1
>331 2
>332 3'
GetDoctorTodayScheduleAsync334 O
(33O P
int33P S
doctorId33T \
,33\ ]
DateOnly33^ f
today33g l
)33l m
{44 	
return55 
await55 
_context55 !
.55! "
Appointments55" .
.66 
Include66 
(66 
a66 
=>66 
a66 
.66  
Patient66  '
)66' (
.77 
Include77 
(77 
a77 
=>77 
a77 
.77  
HealthRecord77  ,
)77, -
.88 
Where88 
(88 
a88 
=>88 
a88 
.88 
DoctorId88 &
==88' )
doctorId88* 2
&&883 5
a886 7
.887 8
ScheduledDate888 E
==88F H
today88I N
)88N O
.99 
OrderBy99 
(99 
a99 
=>99 
a99 
.99  
TimeSlot99  (
)99( )
.:: 
ToListAsync:: 
(:: 
):: 
;:: 
};; 	
public== 
async== 
Task== 
<== 
IEnumerable== %
<==% &
Appointment==& 1
>==1 2
>==2 3&
GetDoctorWeekScheduleAsync==4 N
(==N O
int==O R
doctorId==S [
,==[ \
DateOnly==] e
	startDate==f o
,==o p
DateOnly==q y
endDate	==z Å
)
==Å Ç
{>> 	
return?? 
await?? 
_context?? !
.??! "
Appointments??" .
.@@ 
Include@@ 
(@@ 
a@@ 
=>@@ 
a@@ 
.@@  
Patient@@  '
)@@' (
.AA 
IncludeAA 
(AA 
aAA 
=>AA 
aAA 
.AA  
HealthRecordAA  ,
)AA, -
.BB 
WhereBB 
(BB 
aBB 
=>BB 
aCC 
.CC 
DoctorIdCC 
==CC !
doctorIdCC" *
&&CC+ -
aDD 
.DD 
ScheduledDateDD #
>=DD$ &
	startDateDD' 0
&&DD1 3
aEE 
.EE 
ScheduledDateEE #
<=EE$ &
endDateEE' .
)EE. /
.FF 
OrderByFF 
(FF 
aFF 
=>FF 
aFF 
.FF  
ScheduledDateFF  -
)FF- .
.GG 
ThenByGG 
(GG 
aGG 
=>GG 
aGG 
.GG 
TimeSlotGG '
)GG' (
.HH 
ToListAsyncHH 
(HH 
)HH 
;HH 
}II 	
publicKK 
asyncKK 
TaskKK 
<KK 
boolKK 
>KK 4
(ExistsSamePatientSameDoctorSameDateAsyncKK  H
(KKH I
intLL 
	patientIdLL 
,LL 
intMM 
doctorIdMM 
,MM 
DateOnlyNN 
dateNN 
)NN 
{OO 	
returnPP 
awaitPP 
_contextPP !
.PP! "
AppointmentsPP" .
.PP. /
AnyAsyncPP/ 7
(PP7 8
aPP8 9
=>PP: <
aQQ 
.QQ 
	PatientIdQQ 
==QQ 
	patientIdQQ (
&&QQ) +
aRR 
.RR 
DoctorIdRR 
==RR 
doctorIdRR &
&&RR' )
aSS 
.SS 
ScheduledDateSS 
==SS  "
dateSS# '
&&SS( *
aTT 
.TT 
StatusTT 
!=TT 
AppointmentStatusTT -
.TT- .
	CancelledTT. 7
)TT7 8
;TT8 9
}UU 	
publicWW 
asyncWW 
TaskWW 
<WW 
boolWW 
>WW 2
&ExistsSamePatientSameSlotSameDateAsyncWW  F
(WWF G
intXX 
	patientIdXX 
,XX 
DateOnlyYY 
dateYY 
,YY 
intZZ 
timeSlotZZ 
)ZZ 
{[[ 	
if\\ 
(\\ 
!\\ 
Enum\\ 
.\\ 
	IsDefined\\ 
(\\  
typeof\\  &
(\\& '
AppointmentTimeSlot\\' :
)\\: ;
,\\; <
timeSlot\\= E
)\\E F
)\\F G
throw]] 
new]] 
ArgumentException]] +
(]]+ ,
$str]], L
)]]L M
;]]M N
var__ 
slotEnum__ 
=__ 
(__ 
AppointmentTimeSlot__ /
)__/ 0
timeSlot__0 8
;__8 9
returnaa 
awaitaa 
_contextaa !
.aa! "
Appointmentsaa" .
.aa. /
AnyAsyncaa/ 7
(aa7 8
aaa8 9
=>aa: <
abb 
.bb 
	PatientIdbb 
==bb 
	patientIdbb (
&&bb) +
acc 
.cc 
ScheduledDatecc 
==cc  "
datecc# '
&&cc( *
add 
.dd 
TimeSlotdd 
==dd 
slotEnumdd &
&&dd' )
aee 
.ee 
Statusee 
!=ee 
AppointmentStatusee -
.ee- .
	Cancelledee. 7
)ee7 8
;ee8 9
}ff 	
publichh 
asynchh 
Taskhh 
<hh 
boolhh 
>hh 1
%ExistsSameDoctorSameSlotSameDateAsynchh  E
(hhE F
intii 
doctorIdii 
,ii 
DateOnlyjj 
datejj 
,jj 
intkk 
timeSlotkk 
)kk 
{ll 	
ifmm 
(mm 
!mm 
Enummm 
.mm 
	IsDefinedmm 
(mm  
typeofmm  &
(mm& '
AppointmentTimeSlotmm' :
)mm: ;
,mm; <
timeSlotmm= E
)mmE F
)mmF G
thrownn 
newnn 
ArgumentExceptionnn +
(nn+ ,
$strnn, L
)nnL M
;nnM N
varpp 
slotEnumpp 
=pp 
(pp 
AppointmentTimeSlotpp /
)pp/ 0
timeSlotpp0 8
;pp8 9
returnrr 
awaitrr 
_contextrr !
.rr! "
Appointmentsrr" .
.rr. /
AnyAsyncrr/ 7
(rr7 8
arr8 9
=>rr: <
ass 
.ss 
DoctorIdss 
==ss 
doctorIdss &
&&ss' )
att 
.tt 
ScheduledDatett 
==tt  "
datett# '
&&tt( *
auu 
.uu 
TimeSlotuu 
==uu 
slotEnumuu &
&&uu' )
avv 
.vv 
Statusvv 
!=vv 
AppointmentStatusvv -
.vv- .
	Cancelledvv. 7
)vv7 8
;vv8 9
}ww 	
publicyy 
asyncyy 
Taskyy 
<yy 
boolyy 
>yy 4
(ExistsSamePatientSameDoctorSameDateAsyncyy  H
(yyH I
intzz 
	patientIdzz 
,zz 
int{{ 
doctorId{{ 
,{{ 
DateOnly|| 
date|| 
,|| 
int}} 
appointmentId}} 
)}} 
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
ÅÅ 
	PatientId
ÅÅ 
==
ÅÅ 
	patientId
ÅÅ (
&&
ÅÅ) +
a
ÇÇ 
.
ÇÇ 
DoctorId
ÇÇ 
==
ÇÇ 
doctorId
ÇÇ &
&&
ÇÇ' )
a
ÉÉ 
.
ÉÉ 
ScheduledDate
ÉÉ 
==
ÉÉ  "
date
ÉÉ# '
&&
ÉÉ( *
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
áá 
<
áá 
bool
áá 
>
áá 4
&ExistsSamePatientSameSlotSameDateAsync
áá  F
(
ááF G
int
àà 
	patientId
àà 
,
àà 
DateOnly
ââ 
date
ââ 
,
ââ 
int
ää 
timeSlot
ää 
,
ää 
int
ãã 
appointmentId
ãã 
)
ãã 
{
åå 	
return
çç 
await
çç 
_context
çç !
.
çç! "
Appointments
çç" .
.
çç. /
AnyAsync
çç/ 7
(
çç7 8
a
çç8 9
=>
çç: <
a
éé 
.
éé 
AppointmentId
éé 
!=
éé  "
appointmentId
éé# 0
&&
éé1 3
a
èè 
.
èè 
	PatientId
èè 
==
èè 
	patientId
èè (
&&
èè) +
a
êê 
.
êê 
ScheduledDate
êê 
==
êê  "
date
êê# '
&&
êê( *
(
ëë 
int
ëë 
)
ëë 
a
ëë 
.
ëë 
TimeSlot
ëë 
==
ëë  "
timeSlot
ëë# +
&&
ëë, .
a
íí 
.
íí 
Status
íí 
!=
íí 
AppointmentStatus
íí -
.
íí- .
	Cancelled
íí. 7
)
íí7 8
;
íí8 9
}
ìì 	
public
ïï 
async
ïï 
Task
ïï 
<
ïï 
bool
ïï 
>
ïï 3
%ExistsSameDoctorSameSlotSameDateAsync
ïï  E
(
ïïE F
int
ññ 
doctorId
ññ 
,
ññ 
DateOnly
óó 
date
óó 
,
óó 
int
òò 
timeSlot
òò 
,
òò 
int
ôô 
appointmentId
ôô 
)
ôô 
{
öö 	
return
õõ 
await
õõ 
_context
õõ !
.
õõ! "
Appointments
õõ" .
.
õõ. /
AnyAsync
õõ/ 7
(
õõ7 8
a
õõ8 9
=>
õõ: <
a
úú 
.
úú 
AppointmentId
úú 
!=
úú  "
appointmentId
úú# 0
&&
úú1 3
a
ùù 
.
ùù 
DoctorId
ùù 
==
ùù 
doctorId
ùù &
&&
ùù' )
a
ûû 
.
ûû 
ScheduledDate
ûû 
==
ûû  "
date
ûû# '
&&
ûû( *
(
üü 
int
üü 
)
üü 
a
üü 
.
üü 
TimeSlot
üü 
==
üü  "
timeSlot
üü# +
&&
üü, .
a
†† 
.
†† 
Status
†† 
!=
†† 
AppointmentStatus
†† -
.
††- .
	Cancelled
††. 7
)
††7 8
;
††8 9
}
°° 	
public
££ 
async
££ 
Task
££ 
AddAsync
££ "
(
££" #
Appointment
££# .
appointment
££/ :
)
££: ;
{
§§ 	
await
•• 
_context
•• 
.
•• 
Appointments
•• '
.
••' (
AddAsync
••( 0
(
••0 1
appointment
••1 <
)
••< =
;
••= >
}
¶¶ 	
public
®® 
Task
®® 
UpdateAsync
®® 
(
®®  
Appointment
®®  +
appointment
®®, 7
)
®®7 8
{
©© 	
_context
™™ 
.
™™ 
Appointments
™™ !
.
™™! "
Update
™™" (
(
™™( )
appointment
™™) 4
)
™™4 5
;
™™5 6
return
´´ 
Task
´´ 
.
´´ 
CompletedTask
´´ %
;
´´% &
}
¨¨ 	
public
ÆÆ 
async
ÆÆ 
Task
ÆÆ 
<
ÆÆ 
bool
ÆÆ 
>
ÆÆ 
ExistsAsync
ÆÆ  +
(
ÆÆ+ ,
int
ÆÆ, /
id
ÆÆ0 2
)
ÆÆ2 3
{
ØØ 	
return
∞∞ 
await
∞∞ 
_context
∞∞ !
.
∞∞! "
Appointments
∞∞" .
.
∞∞. /
AnyAsync
∞∞/ 7
(
∞∞7 8
a
∞∞8 9
=>
∞∞: <
a
±± 
.
±± 
AppointmentId
±± 
==
±±  "
id
±±# %
)
±±% &
;
±±& '
}
≤≤ 	
public
¥¥ 
async
¥¥ 
Task
¥¥ 
<
¥¥ 
IEnumerable
¥¥ %
<
¥¥% &
Appointment
¥¥& 1
>
¥¥1 2
>
¥¥2 3/
!GetDoctorPatientAppointmentsAsync
¥¥4 U
(
¥¥U V
int
µµ 
doctorId
µµ 
)
µµ 
{
∂∂ 	
return
∑∑ 
await
∑∑ 
_context
∑∑ !
.
∑∑! "
Appointments
∑∑" .
.
∏∏ 
Include
∏∏ 
(
∏∏ 
a
∏∏ 
=>
∏∏ 
a
∏∏ 
.
∏∏  
Patient
∏∏  '
)
∏∏' (
.
ππ 
Include
ππ 
(
ππ 
a
ππ 
=>
ππ 
a
ππ 
.
ππ  
HealthRecord
ππ  ,
)
ππ, -
.
∫∫ 
Where
∫∫ 
(
∫∫ 
a
∫∫ 
=>
∫∫ 
a
∫∫ 
.
∫∫ 
DoctorId
∫∫ &
==
∫∫' )
doctorId
∫∫* 2
)
∫∫2 3
.
ªª 
OrderByDescending
ªª "
(
ªª" #
a
ªª# $
=>
ªª% '
a
ªª( )
.
ªª) *
ScheduledDate
ªª* 7
)
ªª7 8
.
ºº 
ThenBy
ºº 
(
ºº 
a
ºº 
=>
ºº 
a
ºº 
.
ºº 
TimeSlot
ºº '
)
ºº' (
.
ΩΩ 
ToListAsync
ΩΩ 
(
ΩΩ 
)
ΩΩ 
;
ΩΩ 
}
ææ 	
public
¿¿ 
async
¿¿ 
Task
¿¿ 
SaveChangesAsync
¿¿ *
(
¿¿* +
)
¿¿+ ,
{
¡¡ 	
await
¬¬ 
_context
¬¬ 
.
¬¬ 
SaveChangesAsync
¬¬ +
(
¬¬+ ,
)
¬¬, -
;
¬¬- .
}
√√ 	
}
ƒƒ 
}≈≈ ‹<
mC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Repositories\Implementation\AdminRepository.cs
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
HealthAxisDbContext 2
context3 :
): ;
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
int 
> 
CountPatientsAsync 1
(1 2
)2 3
{ 	
return 
await 
_context !
.! "
Patients" *
.* +

CountAsync+ 5
(5 6
)6 7
;7 8
} 	
public 
async 
Task 
< 
int 
> $
CountActivePatientsAsync 7
(7 8
)8 9
{ 	
return 
await 
_context !
.! "
Patients" *
.* +

CountAsync+ 5
(5 6
p6 7
=>8 :
p; <
.< =
IsActive= E
)E F
;F G
} 	
public 
async 
Task 
< 
int 
> 
CountDoctorsAsync 0
(0 1
)1 2
{ 	
return   
await   
_context   !
.  ! "
Doctors  " )
.  ) *

CountAsync  * 4
(  4 5
)  5 6
;  6 7
}!! 	
public## 
async## 
Task## 
<## 
int## 
>## #
CountActiveDoctorsAsync## 6
(##6 7
)##7 8
{$$ 	
return%% 
await%% 
_context%% !
.%%! "
Doctors%%" )
.%%) *

CountAsync%%* 4
(%%4 5
d%%5 6
=>%%7 9
d%%: ;
.%%; <
IsActive%%< D
)%%D E
;%%E F
}&& 	
public(( 
async(( 
Task(( 
<(( 
int(( 
>(( '
CountTodayAppointmentsAsync(( :
(((: ;
)((; <
{)) 	
var** 
today** 
=** 
DateOnly**  
.**  !
FromDateTime**! -
(**- .
DateTime**. 6
.**6 7
Today**7 <
)**< =
;**= >
return,, 
await,, 
_context,, !
.,,! "
Appointments,," .
.,,. /

CountAsync,,/ 9
(,,9 :
a,,: ;
=>,,< >
a,,? @
.,,@ A
ScheduledDate,,A N
==,,O Q
today,,R W
),,W X
;,,X Y
}-- 	
public// 
async// 
Task// 
<// 
int// 
>// )
CountPendingAppointmentsAsync// <
(//< =
)//= >
{00 	
return11 
await11 
_context11 !
.11! "
Appointments11" .
.11. /

CountAsync11/ 9
(119 :
a11: ;
=>11< >
a11? @
.11@ A
Status11A G
==11H J
AppointmentStatus11K \
.11\ ]
Pending11] d
)11d e
;11e f
}22 	
public44 
async44 
Task44 
<44 
int44 
>44 +
CountCompletedAppointmentsAsync44 >
(44> ?
)44? @
{55 	
return66 
await66 
_context66 !
.66! "
Appointments66" .
.66. /

CountAsync66/ 9
(669 :
a66: ;
=>66< >
a66? @
.66@ A
Status66A G
==66H J
AppointmentStatus66K \
.66\ ]
	Completed66] f
)66f g
;66g h
}77 	
public99 
async99 
Task99 
<99 
int99 
>99 #
CountHealthRecordsAsync99 6
(996 7
)997 8
{:: 	
return;; 
await;; 
_context;; !
.;;! "
HealthRecords;;" /
.;;/ 0

CountAsync;;0 :
(;;: ;
);;; <
;;;< =
}<< 	
public>> 
async>> 
Task>> 
<>> 
IEnumerable>> %
<>>% &
User>>& *
>>>* +
>>>+ ,
GetUsersAsync>>- :
(>>: ;
)>>; <
{?? 	
return@@ 
await@@ 
_context@@ !
.@@! "
AppUsers@@" *
.@@* +
ToListAsync@@+ 6
(@@6 7
)@@7 8
;@@8 9
}AA 	
publicCC 
asyncCC 
TaskCC 
<CC 
UserCC 
?CC 
>CC  
GetUserByIdAsyncCC! 1
(CC1 2
intCC2 5
idCC6 8
)CC8 9
{DD 	
returnEE 
awaitEE 
_contextEE !
.EE! "
AppUsersEE" *
.EE* +
FirstOrDefaultAsyncEE+ >
(EE> ?
uEE? @
=>EEA C
uEED E
.EEE F
UserIdEEF L
==EEM O
idEEP R
)EER S
;EES T
}FF 	
publicHH 
asyncHH 
TaskHH 
<HH 
boolHH 
>HH (
ResolveUserActiveStatusAsyncHH  <
(HH< =
stringHH= C
emailHHD I
,HHI J
stringHHK Q
roleHHR V
)HHV W
{II 	
ifJJ 
(JJ 
stringJJ 
.JJ 
IsNullOrWhiteSpaceJJ )
(JJ) *
emailJJ* /
)JJ/ 0
||JJ1 3
stringJJ4 :
.JJ: ;
IsNullOrWhiteSpaceJJ; M
(JJM N
roleJJN R
)JJR S
)JJS T
returnKK 
falseKK 
;KK 
switchMM 
(MM 
roleMM 
.MM 
TrimMM 
(MM 
)MM 
.MM  
ToLowerInvariantMM  0
(MM0 1
)MM1 2
)MM2 3
{NN 
caseOO 
$strOO 
:OO 
returnPP 
truePP 
;PP  
caseRR 
$strRR 
:RR 
returnSS 
awaitSS  
_contextSS! )
.SS) *
DoctorsSS* 1
.TT 
AsNoTrackingTT %
(TT% &
)TT& '
.UU 
AnyAsyncUU !
(UU! "
dUU" #
=>UU$ &
dUU' (
.UU( )
EmailUU) .
==UU/ 1
emailUU2 7
&&UU8 :
dUU; <
.UU< =
IsActiveUU= E
)UUE F
;UUF G
caseWW 
$strWW 
:WW 
returnXX 
awaitXX  
_contextXX! )
.XX) *
PatientsXX* 2
.YY 
AsNoTrackingYY %
(YY% &
)YY& '
.ZZ 
AnyAsyncZZ !
(ZZ! "
pZZ" #
=>ZZ$ &
pZZ' (
.ZZ( )
EmailZZ) .
==ZZ/ 1
emailZZ2 7
&&ZZ8 :
pZZ; <
.ZZ< =
IsActiveZZ= E
)ZZE F
;ZZF G
default\\ 
:\\ 
return]] 
false]]  
;]]  !
}^^ 
}__ 	
}`` 
}aa ôX
IC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Program.cs
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
$strtt ,
,tt, -
$struu +
)vv 
.ww 
AllowAnyHeaderww 
(ww  
)ww  !
.xx 
AllowAnyMethodxx 
(xx  
)xx  !
;xx! "
}yy 	
)yy	 

;yy
 
}zz 
)zz 
;zz 
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
ÄÄ  
IPatientRepository
ÄÄ -
,
ÄÄ- .
PatientRepository
ÄÄ/ @
>
ÄÄ@ A
(
ÄÄA B
)
ÄÄB C
;
ÄÄC D
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
IDoctorRepository
ÅÅ ,
,
ÅÅ, -
DoctorRepository
ÅÅ. >
>
ÅÅ> ?
(
ÅÅ? @
)
ÅÅ@ A
;
ÅÅA B
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
ÇÇ $
IAppointmentRepository
ÇÇ 1
,
ÇÇ1 2#
AppointmentRepository
ÇÇ3 H
>
ÇÇH I
(
ÇÇI J
)
ÇÇJ K
;
ÇÇK L
builderÉÉ 
.
ÉÉ 
Services
ÉÉ 
.
ÉÉ 
	AddScoped
ÉÉ 
<
ÉÉ %
IHealthRecordRepository
ÉÉ 2
,
ÉÉ2 3$
HealthRecordRepository
ÉÉ4 J
>
ÉÉJ K
(
ÉÉK L
)
ÉÉL M
;
ÉÉM N
builderÑÑ 
.
ÑÑ 
Services
ÑÑ 
.
ÑÑ 
	AddScoped
ÑÑ 
<
ÑÑ 
IUserRepository
ÑÑ *
,
ÑÑ* +
UserRepository
ÑÑ, :
>
ÑÑ: ;
(
ÑÑ; <
)
ÑÑ< =
;
ÑÑ= >
builderÖÖ 
.
ÖÖ 
Services
ÖÖ 
.
ÖÖ 
	AddScoped
ÖÖ 
<
ÖÖ 
IAdminRepository
ÖÖ +
,
ÖÖ+ ,
AdminRepository
ÖÖ- <
>
ÖÖ< =
(
ÖÖ= >
)
ÖÖ> ?
;
ÖÖ? @
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
ãã 
IPatientService
ãã *
,
ãã* +
PatientService
ãã, :
>
ãã: ;
(
ãã; <
)
ãã< =
;
ãã= >
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
åå 
IDoctorService
åå )
,
åå) *
DoctorService
åå+ 8
>
åå8 9
(
åå9 :
)
åå: ;
;
åå; <
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
çç !
IAppointmentService
çç .
,
çç. / 
AppointmentService
çç0 B
>
ççB C
(
ççC D
)
ççD E
;
ççE F
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
éé "
IHealthRecordService
éé /
,
éé/ 0!
HealthRecordService
éé1 D
>
ééD E
(
ééE F
)
ééF G
;
ééG H
builderèè 
.
èè 
Services
èè 
.
èè 
	AddScoped
èè 
<
èè 
IAuthService
èè '
,
èè' (
AuthService
èè) 4
>
èè4 5
(
èè5 6
)
èè6 7
;
èè7 8
builderêê 
.
êê 
Services
êê 
.
êê 
	AddScoped
êê 
<
êê 
IAdminService
êê (
,
êê( )
AdminService
êê* 6
>
êê6 7
(
êê7 8
)
êê8 9
;
êê9 :
builderëë 
.
ëë 
Services
ëë 
.
ëë 
	AddScoped
ëë 
<
ëë 
IUserService
ëë '
,
ëë' (
UserService
ëë) 4
>
ëë4 5
(
ëë5 6
)
ëë6 7
;
ëë7 8
varïï 
app
ïï 
=
ïï 	
builder
ïï
 
.
ïï 
Build
ïï 
(
ïï 
)
ïï 
;
ïï 
ifôô 
(
ôô 
app
ôô 
.
ôô 
Environment
ôô 
.
ôô 
IsDevelopment
ôô !
(
ôô! "
)
ôô" #
)
ôô# $
{öö 
app
õõ 
.
õõ 

UseSwagger
õõ 
(
õõ 
)
õõ 
;
õõ 
app
ùù 
.
ùù 
UseSwaggerUI
ùù 
(
ùù 
options
ùù 
=>
ùù 
{
ûû 
options
üü 
.
üü 
SwaggerEndpoint
üü 
(
üü  
$str
†† &
,
††& '
$str
°° 
)
°°  
;
°°  !
options
££ 
.
££ 
RoutePrefix
££ 
=
££ 
string
££ $
.
££$ %
Empty
££% *
;
££* +
}
§§ 
)
§§ 
;
§§ 
}•• 
appßß 
.
ßß 
UseMiddleware
ßß 
<
ßß !
ExceptionMiddleware
ßß %
>
ßß% &
(
ßß& '
)
ßß' (
;
ßß( )
app©© 
.
©© !
UseHttpsRedirection
©© 
(
©© 
)
©© 
;
©© 
app´´ 
.
´´ 
UseCors
´´ 
(
´´ 
$str
´´ 
)
´´ 
;
´´ 
app≠≠ 
.
≠≠ 
UseAuthentication
≠≠ 
(
≠≠ 
)
≠≠ 
;
≠≠ 
appØØ 
.
ØØ 
UseAuthorization
ØØ 
(
ØØ 
)
ØØ 
;
ØØ 
app±± 
.
±± 
MapControllers
±± 
(
±± 
)
±± 
;
±± 
appµµ 
.
µµ 
Run
µµ 
(
µµ 
)
µµ 	
;
µµ	 
Ú
MC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Models\User.cs
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
public 
bool 
MustChangePassword &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
false7 <
;< =
public 
DateTime 
? "
RefreshTokenExpiryTime /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
} 
} ˘!
PC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Models\Patient.cs
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
}++ Õ
UC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Models\HealthRecord.cs
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
}&& í
OC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Models\Doctor.cs
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
}++ Ã
TC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Models\Appointment.cs
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
}$$ ›
iC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260630180404_PasswordChang.cs
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
PasswordChang &
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
migrationBuilder 
. 
	AddColumn &
<& '
bool' +
>+ ,
(, -
name 
: 
$str *
,* +
table 
: 
$str 
, 
type 
: 
$str 
, 
nullable 
: 
false 
,  
defaultValue 
: 
false #
)# $
;$ %
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 

DropColumn '
(' (
name 
: 
$str *
,* +
table 
: 
$str 
) 
;  
} 	
} 
} ä
iC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260622111729_DoctorUpdates.cs
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
} Ñ
fC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260622111123_emailadded.cs
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
} ˇ
pC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260619044738_AddUniqueDoctorEmail.cs
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
}66 ·≈
cC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260617093733_testing.cs
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
}‡‡ ¶
mC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260616160604_UpdatedUsersMOdel.cs
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
}'' Ö
fC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260616051459_UsersAdded.cs
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
}'' Ñ∑
iC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Migrations\20260615042816_InitialCreate.cs
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
}ªª É
eC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Middleware\RequestLoggingMiddleware.cs
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
} ™#
`C:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Middleware\ExceptionMiddleware.cs
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
}TT õ_
SC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Data\AppDbContext.cs
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
public 
DbSet 
< 
User 
> 
AppUsers #
=>$ &
Set' *
<* +
User+ /
>/ 0
(0 1
)1 2
;2 3
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
builder= D
)D E
{ 	
base 
. 
OnModelCreating  
(  !
builder! (
)( )
;) *
builder 
. 
Entity 
< 
User 
>  
(  !
)! "
." #
ToTable# *
(* +
$str+ 2
)2 3
;3 4
builder"" 
."" 
Entity"" 
<"" 
Appointment"" &
>""& '
(""' (
)""( )
.## 
HasOne## 
(## 
a## 
=>## 
a## 
.## 
Patient## &
)##& '
.$$ 
WithMany$$ 
($$ 
p$$ 
=>$$ 
p$$  
.$$  !
Appointments$$! -
)$$- .
.%% 
HasForeignKey%% 
(%% 
a%%  
=>%%! #
a%%$ %
.%%% &
	PatientId%%& /
)%%/ 0
.&& 
OnDelete&& 
(&& 
DeleteBehavior&& (
.&&( )
Restrict&&) 1
)&&1 2
;&&2 3
builder(( 
.(( 
Entity(( 
<(( 
Appointment(( &
>((& '
(((' (
)((( )
.)) 
HasOne)) 
()) 
a)) 
=>)) 
a)) 
.)) 
Doctor)) %
)))% &
.** 
WithMany** 
(** 
d** 
=>** 
d**  
.**  !
Appointments**! -
)**- .
.++ 
HasForeignKey++ 
(++ 
a++  
=>++! #
a++$ %
.++% &
DoctorId++& .
)++. /
.,, 
OnDelete,, 
(,, 
DeleteBehavior,, (
.,,( )
Restrict,,) 1
),,1 2
;,,2 3
builder// 
.// 
Entity// 
<// 
Appointment// &
>//& '
(//' (
)//( )
.00 
HasIndex00 
(00 
a00 
=>00 
new00 "
{11 
a22 
.22 
DoctorId22 
,22 
a33 
.33 
ScheduledDate33 #
,33# $
a44 
.44 
TimeSlot44 
}55 
)55 
.66 
IsUnique66 
(66 
)66 
;66 
builder99 
.99 
Entity99 
<99 
HealthRecord99 '
>99' (
(99( )
)99) *
.:: 
HasOne:: 
(:: 
hr:: 
=>:: 
hr::  
.::  !
Appointment::! ,
)::, -
.;; 
WithOne;; 
(;; 
a;; 
=>;; 
a;; 
.;;  
HealthRecord;;  ,
);;, -
.<< 
HasForeignKey<< 
<<< 
HealthRecord<< +
><<+ ,
(<<, -
hr<<- /
=><<0 2
hr<<3 5
.<<5 6
AppointmentId<<6 C
)<<C D
.== 
OnDelete== 
(== 
DeleteBehavior== (
.==( )
Restrict==) 1
)==1 2
;==2 3
builder?? 
.?? 
Entity?? 
<?? 
HealthRecord?? '
>??' (
(??( )
)??) *
.@@ 
HasOne@@ 
(@@ 
hr@@ 
=>@@ 
hr@@  
.@@  !
Patient@@! (
)@@( )
.AA 
WithManyAA 
(AA 
pAA 
=>AA 
pAA  
.AA  !
HealthRecordsAA! .
)AA. /
.BB 
HasForeignKeyBB 
(BB 
hrBB !
=>BB" $
hrBB% '
.BB' (
	PatientIdBB( 1
)BB1 2
.CC 
OnDeleteCC 
(CC 
DeleteBehaviorCC (
.CC( )
RestrictCC) 1
)CC1 2
;CC2 3
builderEE 
.EE 
EntityEE 
<EE 
HealthRecordEE '
>EE' (
(EE( )
)EE) *
.FF 
HasOneFF 
(FF 
hrFF 
=>FF 
hrFF  
.FF  !
DoctorFF! '
)FF' (
.GG 
WithManyGG 
(GG 
dGG 
=>GG 
dGG  
.GG  !
HealthRecordsGG! .
)GG. /
.HH 
HasForeignKeyHH 
(HH 
hrHH !
=>HH" $
hrHH% '
.HH' (
DoctorIdHH( 0
)HH0 1
.II 
OnDeleteII 
(II 
DeleteBehaviorII (
.II( )
RestrictII) 1
)II1 2
;II2 3
builderLL 
.LL 
EntityLL 
<LL 
HealthRecordLL '
>LL' (
(LL( )
)LL) *
.MM 
HasIndexMM 
(MM 
hrMM 
=>MM 
hrMM  "
.MM" #
AppointmentIdMM# 0
)MM0 1
.NN 
IsUniqueNN 
(NN 
)NN 
;NN 
builderQQ 
.QQ 
EntityQQ 
<QQ 
DoctorQQ !
>QQ! "
(QQ" #
)QQ# $
.RR 
PropertyRR 
(RR 
dRR 
=>RR 
dRR  
.RR  !
ConsultationFeeRR! 0
)RR0 1
.SS 
HasPrecisionSS 
(SS 
$numSS  
,SS  !
$numSS" #
)SS# $
;SS$ %
builderVV 
.VV 
EntityVV 
<VV 
PatientVV "
>VV" #
(VV# $
)VV$ %
.VV% &
HasDataVV& -
(VV- .
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
;oo 
builderrr 
.rr 
Entityrr 
<rr 
Doctorrr !
>rr! "
(rr" #
)rr# $
.rr$ %
HasDatarr% ,
(rr, -
newss 
Doctorss 
{tt 
DoctorIduu 
=uu 
$numuu  
,uu  !
FullNamevv 
=vv 
$strvv *
,vv* +
Specialisationww "
=ww# $ 
DoctorSpecialisationww% 9
.ww9 :
GeneralPractitionerww: M
,wwM N
YearsOfExperiencexx %
=xx& '
$numxx( )
,xx) *
ConsultationFeeyy #
=yy$ %
$numyy& -
,yy- .
IsActivezz 
=zz 
truezz #
}{{ 
,{{ 
new|| 
Doctor|| 
{}} 
DoctorId~~ 
=~~ 
$num~~  
,~~  !
FullName 
= 
$str ,
,, -
Specialisation
ÄÄ "
=
ÄÄ# $"
DoctorSpecialisation
ÄÄ% 9
.
ÄÄ9 :
Cardiologist
ÄÄ: F
,
ÄÄF G
YearsOfExperience
ÅÅ %
=
ÅÅ& '
$num
ÅÅ( *
,
ÅÅ* +
ConsultationFee
ÇÇ #
=
ÇÇ$ %
$num
ÇÇ& .
,
ÇÇ. /
IsActive
ÉÉ 
=
ÉÉ 
true
ÉÉ #
}
ÑÑ 
)
ÖÖ 
;
ÖÖ 
builder
àà 
.
àà 
Entity
àà 
<
àà 
Appointment
àà &
>
àà& '
(
àà' (
)
àà( )
.
àà) *
HasData
àà* 1
(
àà1 2
new
ââ 
Appointment
ââ 
{
ää 
AppointmentId
ãã !
=
ãã" #
$num
ãã$ %
,
ãã% &
	PatientId
åå 
=
åå 
$num
åå  !
,
åå! "
DoctorId
çç 
=
çç 
$num
çç  
,
çç  !
ScheduledDate
éé !
=
éé" #
new
éé$ '
DateOnly
éé( 0
(
éé0 1
$num
éé1 5
,
éé5 6
$num
éé7 8
,
éé8 9
$num
éé: <
)
éé< =
,
éé= >
TimeSlot
èè 
=
èè !
AppointmentTimeSlot
èè 2
.
èè2 3
TenAM
èè3 8
,
èè8 9
Status
êê 
=
êê 
AppointmentStatus
êê .
.
êê. /
Pending
êê/ 6
}
ëë 
)
íí 
;
íí 
}
ìì 	
}
îî 
}ïï ác
_C:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Controllers\PatientController.cs
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
;8 9
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public 
PatientsController !
(! "
IPatientService 
patientService *
,* + 
IHealthRecordService  
healthRecordService! 4
)4 5
{ 	
_patientService 
= 
patientService ,
;, - 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
)0 1
{ 	
var 
patients 
= 
await  
_patientService! 0
.0 1
GetAllAsync1 <
(< =
)= >
;> ?
return 
Ok 
( 
patients 
) 
;  
} 	
[!! 	
HttpGet!!	 
(!! 
$str!! 
)!! 
]!! 
["" 	
	Authorize""	 
("" 
Roles"" 
="" 
$str"" 1
)""1 2
]""2 3
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
GetById##) 0
(##0 1
int##1 4
id##5 7
)##7 8
{$$ 	
if%% 
(%% 
User%% 
.%% 
IsInRole%% 
(%% 
$str%% '
)%%' (
)%%( )
{&& 
var'' 
patientIdFromToken'' &
=''' (*
GetPatientReferenceIdFromToken'') G
(''G H
)''H I
;''I J
if)) 
()) 
!)) 
patientIdFromToken)) '
.))' (
HasValue))( 0
||))1 3
patientIdFromToken** &
.**& '
Value**' ,
!=**- /
id**0 2
)**2 3
{++ 
return,, 
Forbid,, !
(,,! "
),," #
;,,# $
}-- 
}.. 
var00 
patient00 
=00 
await00 
_patientService00  /
.00/ 0
GetByIdAsync000 <
(00< =
id00= ?
)00? @
;00@ A
if22 
(22 
patient22 
==22 
null22 
)22  
{33 
return44 
NotFound44 
(44  
$"44  "
$str44" 2
{442 3
id443 5
}445 6
$str446 A
"44A B
)44B C
;44C D
}55 
return77 
Ok77 
(77 
patient77 
)77 
;77 
}88 	
[:: 	
HttpGet::	 
(:: 
$str:: *
)::* +
]::+ ,
[;; 	
	Authorize;;	 
(;; 
Roles;; 
=;; 
$str;; 1
);;1 2
];;2 3
public<< 
async<< 
Task<< 
<<< 
IActionResult<< '
><<' (
GetHealthRecords<<) 9
(<<9 :
int<<: =
id<<> @
)<<@ A
{== 	
if>> 
(>> 
User>> 
.>> 
IsInRole>> 
(>> 
$str>> '
)>>' (
)>>( )
{?? 
var@@ 
patientIdFromToken@@ &
=@@' (*
GetPatientReferenceIdFromToken@@) G
(@@G H
)@@H I
;@@I J
ifBB 
(BB 
!BB 
patientIdFromTokenBB '
.BB' (
HasValueBB( 0
||BB1 3
patientIdFromTokenCC &
.CC& '
ValueCC' ,
!=CC- /
idCC0 2
)CC2 3
{DD 
returnEE 
ForbidEE !
(EE! "
)EE" #
;EE# $
}FF 
}GG 
tryII 
{JJ 
varKK 
recordsKK 
=KK 
awaitLL  
_healthRecordServiceLL .
.LL. /
GetByPatientIdAsyncLL/ B
(LLB C
idLLC E
)LLE F
;LLF G
returnNN 
OkNN 
(NN 
recordsNN !
)NN! "
;NN" #
}OO 
catchPP 
(PP  
KeyNotFoundExceptionPP '
exPP( *
)PP* +
{QQ 
returnRR 
NotFoundRR 
(RR  
exRR  "
.RR" #
MessageRR# *
)RR* +
;RR+ ,
}SS 
}TT 	
[VV 	
HttpGetVV	 
(VV 
$strVV 
)VV 
]VV 
[WW 	
	AuthorizeWW	 
(WW 
RolesWW 
=WW 
$strWW )
)WW) *
]WW* +
publicXX 
asyncXX 
TaskXX 
<XX 
IActionResultXX '
>XX' (
SearchXX) /
(XX/ 0
[YY 
	FromQueryYY 
]YY 
stringYY 
nameYY #
)YY# $
{ZZ 	
var[[ 
patients[[ 
=[[ 
await\\ 
_patientService\\ %
.\\% &
SearchByNameAsync\\& 7
(\\7 8
name\\8 <
)\\< =
;\\= >
return^^ 
Ok^^ 
(^^ 
patients^^ 
)^^ 
;^^  
}__ 	
[aa 	
HttpPostaa	 
]aa 
publicbb 
asyncbb 
Taskbb 
<bb 
IActionResultbb '
>bb' (
Createbb) /
(bb/ 0
CreatePatientDtocc 
dtocc  
)cc  !
{dd 	
tryee 
{ff 
vargg 
patientgg 
=gg 
awaithh 
_patientServicehh )
.hh) *
CreateAsynchh* 5
(hh5 6
dtohh6 9
)hh9 :
;hh: ;
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
=ll 
patientll &
.ll& '
	PatientIdll' 0
}ll1 2
,ll2 3
patientmm 
)mm 
;mm 
}nn 
catchoo 
(oo 
ArgumentExceptionoo $
exoo% '
)oo' (
{pp 
returnqq 

BadRequestqq !
(qq! "
exqq" $
.qq$ %
Messageqq% ,
)qq, -
;qq- .
}rr 
}ss 	
[uu 	
HttpPutuu	 
(uu 
$struu 
)uu 
]uu 
publicvv 
asyncvv 
Taskvv 
<vv 
IActionResultvv '
>vv' (
Updatevv) /
(vv/ 0
intww 
idww 
,ww 
UpdatePatientDtoxx 
dtoxx  
)xx  !
{yy 	
tryzz 
{{{ 
if|| 
(|| 
User|| 
.|| 
IsInRole|| !
(||! "
$str||" +
)||+ ,
)||, -
{}} 
var~~ 
patientIdFromToken~~ *
=~~+ ,*
GetPatientReferenceIdFromToken~~- K
(~~K L
)~~L M
;~~M N
if
ÄÄ 
(
ÄÄ 
!
ÄÄ  
patientIdFromToken
ÄÄ +
.
ÄÄ+ ,
HasValue
ÄÄ, 4
||
ÄÄ5 7 
patientIdFromToken
ÅÅ *
.
ÅÅ* +
Value
ÅÅ+ 0
!=
ÅÅ1 3
id
ÅÅ4 6
)
ÅÅ6 7
{
ÇÇ 
return
ÉÉ 
Forbid
ÉÉ %
(
ÉÉ% &
)
ÉÉ& '
;
ÉÉ' (
}
ÑÑ 
}
ÖÖ 
await
áá 
_patientService
áá %
.
áá% &
UpdateAsync
áá& 1
(
áá1 2
id
áá2 4
,
áá4 5
dto
áá6 9
)
áá9 :
;
áá: ;
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
ãã '
ex
ãã( *
)
ãã* +
{
åå 
return
çç 
NotFound
çç 
(
çç  
ex
çç  "
.
çç" #
Message
çç# *
)
çç* +
;
çç+ ,
}
éé 
catch
èè 
(
èè 
ArgumentException
èè $
ex
èè% '
)
èè' (
{
êê 
return
ëë 

BadRequest
ëë !
(
ëë! "
ex
ëë" $
.
ëë$ %
Message
ëë% ,
)
ëë, -
;
ëë- .
}
íí 
}
ìì 	
[
ïï 	
HttpPut
ïï	 
(
ïï 
$str
ïï $
)
ïï$ %
]
ïï% &
[
ññ 	
	Authorize
ññ	 
(
ññ 
Roles
ññ 
=
ññ 
$str
ññ "
)
ññ" #
]
ññ# $
public
óó 
async
óó 
Task
óó 
<
óó 
IActionResult
óó '
>
óó' (
Activate
óó) 1
(
óó1 2
int
óó2 5
id
óó6 8
)
óó8 9
{
òò 	
try
ôô 
{
öö 
await
õõ 
_patientService
õõ %
.
õõ% &
ActivateAsync
õõ& 3
(
õõ3 4
id
õõ4 6
)
õõ6 7
;
õõ7 8
return
ùù 
	NoContent
ùù  
(
ùù  !
)
ùù! "
;
ùù" #
}
ûû 
catch
üü 
(
üü "
KeyNotFoundException
üü '
ex
üü( *
)
üü* +
{
†† 
return
°° 
NotFound
°° 
(
°°  
ex
°°  "
.
°°" #
Message
°°# *
)
°°* +
;
°°+ ,
}
¢¢ 
}
££ 	
[
•• 	
HttpPut
••	 
(
•• 
$str
•• &
)
••& '
]
••' (
[
¶¶ 	
	Authorize
¶¶	 
(
¶¶ 
Roles
¶¶ 
=
¶¶ 
$str
¶¶ "
)
¶¶" #
]
¶¶# $
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
IActionResult
ßß '
>
ßß' (

Deactivate
ßß) 3
(
ßß3 4
int
ßß4 7
id
ßß8 :
)
ßß: ;
{
®® 	
try
©© 
{
™™ 
await
´´ 
_patientService
´´ %
.
´´% &
DeactivateAsync
´´& 5
(
´´5 6
id
´´6 8
)
´´8 9
;
´´9 :
return
≠≠ 
	NoContent
≠≠  
(
≠≠  !
)
≠≠! "
;
≠≠" #
}
ÆÆ 
catch
ØØ 
(
ØØ "
KeyNotFoundException
ØØ '
ex
ØØ( *
)
ØØ* +
{
∞∞ 
return
±± 
NotFound
±± 
(
±±  
ex
±±  "
.
±±" #
Message
±±# *
)
±±* +
;
±±+ ,
}
≤≤ 
}
≥≥ 	
private
µµ 
int
µµ 
?
µµ ,
GetPatientReferenceIdFromToken
µµ 3
(
µµ3 4
)
µµ4 5
{
∂∂ 	
var
∑∑ 
referenceIdValue
∑∑  
=
∑∑! "
User
∑∑# '
.
∑∑' (
	FindFirst
∑∑( 1
(
∑∑1 2
$str
∑∑2 ?
)
∑∑? @
?
∑∑@ A
.
∑∑A B
Value
∑∑B G
;
∑∑G H
if
ππ 
(
ππ 
int
ππ 
.
ππ 
TryParse
ππ 
(
ππ 
referenceIdValue
ππ -
,
ππ- .
out
ππ/ 2
var
ππ3 6
referenceId
ππ7 B
)
ππB C
)
ππC D
{
∫∫ 
return
ªª 
referenceId
ªª "
;
ªª" #
}
ºº 
return
ææ 
null
ææ 
;
ææ 
}
øø 	
}
¿¿ 
}¡¡ —C
dC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Controllers\HealthRecordController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
[

 
ApiController

 
]

 
[ 
	Authorize 
] 
public 

class #
HealthRecordsController (
:) *
ControllerBase+ 9
{ 
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
private 
readonly 
IAppointmentService ,
_appointmentService- @
;@ A
public #
HealthRecordsController &
(& ' 
IHealthRecordService  
healthRecordService! 4
,4 5
IAppointmentService 
appointmentService  2
)2 3
{ 	 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
int1 4
id5 7
)7 8
{ 	
var 
record 
= 
await  
_healthRecordService *
.* +
GetByIdAsync+ 7
(7 8
id8 :
): ;
;; <
if 
( 
record 
== 
null 
) 
{   
return!! 
NotFound!! 
(!!  
$""" 
$str"" $
{""$ %
id""% '
}""' (
$str""( 3
"""3 4
)""4 5
;""5 6
}## 
return%% 
Ok%% 
(%% 
record%% 
)%% 
;%% 
}&& 	
[(( 	
HttpGet((	 
((( 
$str(( 2
)((2 3
]((3 4
public)) 
async)) 
Task)) 
<)) 
IActionResult)) '
>))' (
GetByAppointment))) 9
())9 :
int** 
appointmentId** 
)** 
{++ 	
var,, 
appointment,, 
=,, 
await-- 
_appointmentService-- )
.--) *
GetByIdAsync--* 6
(--6 7
appointmentId--7 D
)--D E
;--E F
if// 
(// 
appointment// 
==// 
null// #
)//# $
{00 
return11 
NotFound11 
(11  
$"22 
$str22 "
{22" #
appointmentId22# 0
}220 1
$str221 <
"22< =
)22= >
;22> ?
}33 
if55 
(55 
User55 
.55 
IsInRole55 
(55 
$str55 '
)55' (
)55( )
{66 
var77 
patientIdFromToken77 &
=77' (*
GetPatientReferenceIdFromToken77) G
(77G H
)77H I
;77I J
if99 
(99 
!99 
patientIdFromToken99 '
.99' (
HasValue99( 0
||991 3
appointment:: 
.::  
	PatientId::  )
!=::* ,
patientIdFromToken::- ?
.::? @
Value::@ E
)::E F
{;; 
return<< 
Forbid<< !
(<<! "
)<<" #
;<<# $
}== 
}>> 
var@@ 
record@@ 
=@@ 
awaitAA  
_healthRecordServiceAA *
.BB #
GetByAppointmentIdAsyncBB ,
(BB, -
appointmentIdBB- :
)BB: ;
;BB; <
ifDD 
(DD 
recordDD 
==DD 
nullDD 
)DD 
{EE 
returnFF 
NotFoundFF 
(FF  
$strGG C
)GGC D
;GGD E
}HH 
returnJJ 
OkJJ 
(JJ 
recordJJ 
)JJ 
;JJ 
}KK 	
[MM 	
HttpPostMM	 
]MM 
[NN 	
	AuthorizeNN	 
(NN 
RolesNN 
=NN 
$strNN )
)NN) *
]NN* +
publicOO 
asyncOO 
TaskOO 
<OO 
IActionResultOO '
>OO' (
CreateOO) /
(OO/ 0
[PP 
FromBodyPP 
]PP !
CreateHealthRecordDtoPP ,
dtoPP- 0
)PP0 1
{QQ 	
tryRR 
{SS 
varTT 
recordTT 
=TT 
awaitUU  
_healthRecordServiceUU .
.UU. /
CreateAsyncUU/ :
(UU: ;
dtoUU; >
)UU> ?
;UU? @
returnWW 
CreatedAtActionWW &
(WW& '
nameofXX 
(XX 
GetByIdXX "
)XX" #
,XX# $
newYY 
{YY 
idYY 
=YY 
recordYY %
.YY% &
HealthRecordIdYY& 4
}YY5 6
,YY6 7
recordZZ 
)ZZ 
;ZZ 
}[[ 
catch\\ 
(\\  
KeyNotFoundException\\ '
ex\\( *
)\\* +
{]] 
return^^ 
NotFound^^ 
(^^  
ex^^  "
.^^" #
Message^^# *
)^^* +
;^^+ ,
}__ 
catch`` 
(`` 
ArgumentException`` $
ex``% '
)``' (
{aa 
returnbb 

BadRequestbb !
(bb! "
exbb" $
.bb$ %
Messagebb% ,
)bb, -
;bb- .
}cc 
catchdd 
(dd %
InvalidOperationExceptiondd ,
exdd- /
)dd/ 0
{ee 
returnff 

BadRequestff !
(ff! "
exff" $
.ff$ %
Messageff% ,
)ff, -
;ff- .
}gg 
}hh 	
[jj 	
HttpPutjj	 
(jj 
$strjj 
)jj 
]jj 
[kk 	
	Authorizekk	 
(kk 
Roleskk 
=kk 
$strkk )
)kk) *
]kk* +
publicll 
asyncll 
Taskll 
<ll 
IActionResultll '
>ll' (
Updatell) /
(ll/ 0
intmm 
idmm 
,mm 
[nn 
FromBodynn 
]nn !
UpdateHealthRecordDtonn ,
dtonn- 0
)nn0 1
{oo 	
trypp 
{qq 
awaitrr  
_healthRecordServicerr *
.ss 
UpdateAsyncss  
(ss  !
idss! #
,ss# $
dtoss% (
)ss( )
;ss) *
returnuu 
	NoContentuu  
(uu  !
)uu! "
;uu" #
}vv 
catchww 
(ww  
KeyNotFoundExceptionww '
exww( *
)ww* +
{xx 
returnyy 
NotFoundyy 
(yy  
exyy  "
.yy" #
Messageyy# *
)yy* +
;yy+ ,
}zz 
catch{{ 
({{ 
ArgumentException{{ $
ex{{% '
){{' (
{|| 
return}} 

BadRequest}} !
(}}! "
ex}}" $
.}}$ %
Message}}% ,
)}}, -
;}}- .
}~~ 
} 	
private
ÅÅ 
int
ÅÅ 
?
ÅÅ ,
GetPatientReferenceIdFromToken
ÅÅ 3
(
ÅÅ3 4
)
ÅÅ4 5
{
ÇÇ 	
var
ÉÉ 
referenceIdValue
ÉÉ  
=
ÉÉ! "
User
ÉÉ# '
.
ÉÉ' (
	FindFirst
ÉÉ( 1
(
ÉÉ1 2
$str
ÉÉ2 ?
)
ÉÉ? @
?
ÉÉ@ A
.
ÉÉA B
Value
ÉÉB G
;
ÉÉG H
if
ÖÖ 
(
ÖÖ 
int
ÖÖ 
.
ÖÖ 
TryParse
ÖÖ 
(
ÖÖ 
referenceIdValue
ÖÖ -
,
ÖÖ- .
out
ÖÖ/ 2
var
ÖÖ3 6
referenceId
ÖÖ7 B
)
ÖÖB C
)
ÖÖC D
{
ÜÜ 
return
áá 
referenceId
áá "
;
áá" #
}
àà 
return
ää 
null
ää 
;
ää 
}
ãã 	
}
åå 
}çç ÌR
^C:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Controllers\DoctorController.cs
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
;6 7
private 
readonly 
IAppointmentService ,
_appointmentService- @
;@ A
public 
DoctorsController  
(  !
IDoctorService 
doctorService (
,( )
IAppointmentService 
appointmentService  2
)2 3
{ 	
_doctorService 
= 
doctorService *
;* +
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
[ 
	FromQuery 
] 
string 
? 
sortBy  &
,& '
[ 
	FromQuery 
] 
int 
? 
specialisation +
)+ ,
{ 	
var 
doctors 
= 
await 
_doctorService $
.$ %
GetAllAsync% 0
(0 1
sortBy 
, 
specialisation   "
)  " #
;  # $
return"" 
Ok"" 
("" 
doctors"" 
)"" 
;"" 
}## 	
[%% 	
HttpGet%%	 
(%% 
$str%% 
)%% 
]%% 
public&& 
async&& 
Task&& 
<&& 
IActionResult&& '
>&&' (
GetById&&) 0
(&&0 1
int'' 
id'' 
)'' 
{(( 	
var)) 
doctor)) 
=)) 
await** 
_doctorService** $
.**$ %
GetByIdAsync**% 1
(**1 2
id**2 4
)**4 5
;**5 6
if,, 
(,, 
doctor,, 
==,, 
null,, 
),, 
{-- 
return.. 
NotFound.. 
(..  
$"// 
$str// %
{//% &
id//& (
}//( )
$str//) 4
"//4 5
)//5 6
;//6 7
}00 
return22 
Ok22 
(22 
doctor22 
)22 
;22 
}33 	
[55 	
HttpGet55	 
(55 
$str55 6
)556 7
]557 8
public66 
async66 
Task66 
<66 
IActionResult66 '
>66' (
GetBySpecialisation77 
(77  
int88 
specialisation88 "
)88" #
{99 	
var:: 
doctors:: 
=:: 
await;; 
_doctorService;; $
.<< *
GetActiveBySpecialisationAsync<< 3
(<<3 4
specialisation== &
)==& '
;==' (
return?? 
Ok?? 
(?? 
doctors?? 
)?? 
;?? 
}@@ 	
[BB 	
HttpPostBB	 
]BB 
[CC 	
	AuthorizeCC	 
(CC 
RolesCC 
=CC 
$strCC "
)CC" #
]CC# $
publicDD 
asyncDD 
TaskDD 
<DD 
IActionResultDD '
>DD' (
CreateDD) /
(DD/ 0
[EE 
FromBodyEE 
]EE 
CreateDoctorDtoEE &
dtoEE' *
)EE* +
{FF 	
varGG 
doctorGG 
=GG 
awaitHH 
_doctorServiceHH $
.II (
CreateDoctorWithAccountAsyncII 1
(II1 2
dtoII2 5
)II5 6
;II6 7
returnKK 
CreatedAtActionKK "
(KK" #
nameofLL 
(LL 
GetByIdLL 
)LL 
,LL  
newMM 
{MM 
idMM 
=MM 
doctorMM !
.MM! "
DoctorIdMM" *
}MM+ ,
,MM, -
doctorNN 
)NN 
;NN 
}OO 	
[QQ 	
HttpPutQQ	 
(QQ 
$strQQ 
)QQ 
]QQ 
[RR 	
	AuthorizeRR	 
(RR 
RolesRR 
=RR 
$strRR "
)RR" #
]RR# $
publicSS 
asyncSS 
TaskSS 
<SS 
IActionResultSS '
>SS' (
UpdateSS) /
(SS/ 0
intTT 
idTT 
,TT 
[UU 
FromBodyUU 
]UU 
UpdateDoctorDtoUU &
dtoUU' *
)UU* +
{VV 	
awaitWW 
_doctorServiceWW  
.WW  !
UpdateAsyncWW! ,
(WW, -
idXX 
,XX 
dtoYY 
)YY 
;YY 
return[[ 
	NoContent[[ 
([[ 
)[[ 
;[[ 
}\\ 	
[^^ 	
HttpGet^^	 
(^^ 
$str^^ (
)^^( )
]^^) *
public__ 
async__ 
Task__ 
<__ 
IActionResult__ '
>__' (
GetAvailability`` 
(`` 
intaa 
idaa 
,aa 
[bb 
	FromQuerybb 
]bb 
DateOnlybb $
datebb% )
)bb) *
{cc 	
vardd 
slotsdd 
=dd 
awaitee 
_doctorServiceee $
.ff  
GetAvailabilityAsyncff )
(ff) *
idgg 
,gg 
datehh 
)hh 
;hh 
returnjj 
Okjj 
(jj 
slotsjj 
)jj 
;jj 
}kk 	
[mm 	
HttpPutmm	 
(mm 
$strmm $
)mm$ %
]mm% &
[nn 	
	Authorizenn	 
(nn 
Rolesnn 
=nn 
$strnn "
)nn" #
]nn# $
publicoo 
asyncoo 
Taskoo 
<oo 
IActionResultoo '
>oo' (
Activateoo) 1
(oo1 2
intpp 
idpp 
)pp 
{qq 	
awaitrr 
_doctorServicerr  
.rr  !
ActivateAsyncrr! .
(rr. /
idrr/ 1
)rr1 2
;rr2 3
returntt 
	NoContenttt 
(tt 
)tt 
;tt 
}uu 	
[ww 	
HttpGetww	 
(ww 
$strww $
)ww$ %
]ww% &
[xx 	
	Authorizexx	 
(xx 
Rolesxx 
=xx 
$strxx )
)xx) *
]xx* +
publicyy 
asyncyy 
Taskyy 
<yy 
IActionResultyy '
>yy' (
GetDoctorPatientsyy) :
(yy: ;
intyy; >
idyy? A
)yyA B
{zz 	
try{{ 
{|| 
if}} 
(}} 
User}} 
.}} 
IsInRole}} !
(}}! "
$str}}" *
)}}* +
)}}+ ,
{~~ 
var 
doctorIdFromToken )
=* +#
GetReferenceIdFromToken, C
(C D
)D E
;E F
if
ÅÅ 
(
ÅÅ 
!
ÅÅ 
doctorIdFromToken
ÅÅ *
.
ÅÅ* +
HasValue
ÅÅ+ 3
||
ÅÅ4 6
doctorIdFromToken
ÇÇ )
.
ÇÇ) *
Value
ÇÇ* /
!=
ÇÇ0 2
id
ÇÇ3 5
)
ÇÇ5 6
{
ÉÉ 
return
ÑÑ 
Forbid
ÑÑ %
(
ÑÑ% &
)
ÑÑ& '
;
ÑÑ' (
}
ÖÖ 
}
ÜÜ 
var
àà 
patients
àà 
=
àà 
await
ââ !
_appointmentService
ââ -
.
ââ- .$
GetDoctorPatientsAsync
ââ. D
(
ââD E
id
ââE G
)
ââG H
;
ââH I
return
ãã 
Ok
ãã 
(
ãã 
patients
ãã "
)
ãã" #
;
ãã# $
}
åå 
catch
çç 
(
çç "
KeyNotFoundException
çç '
ex
çç( *
)
çç* +
{
éé 
return
èè 
NotFound
èè 
(
èè  
ex
èè  "
.
èè" #
Message
èè# *
)
èè* +
;
èè+ ,
}
êê 
}
ëë 	
[
ìì 	
HttpPut
ìì	 
(
ìì 
$str
ìì &
)
ìì& '
]
ìì' (
[
îî 	
	Authorize
îî	 
(
îî 
Roles
îî 
=
îî 
$str
îî "
)
îî" #
]
îî# $
public
ïï 
async
ïï 
Task
ïï 
<
ïï 
IActionResult
ïï '
>
ïï' (

Deactivate
ïï) 3
(
ïï3 4
int
ññ 
id
ññ 
)
ññ 
{
óó 	
await
òò 
_doctorService
òò  
.
òò  !
DeactivateAsync
òò! 0
(
òò0 1
id
òò1 3
)
òò3 4
;
òò4 5
return
öö 
	NoContent
öö 
(
öö 
)
öö 
;
öö 
}
õõ 	
private
úú 
int
úú 
?
úú %
GetReferenceIdFromToken
úú ,
(
úú, -
)
úú- .
{
ùù 	
var
ûû 
referenceIdValue
ûû  
=
ûû! "
User
üü 
.
üü 
	FindFirst
üü 
(
üü 
$str
üü ,
)
üü, -
?
üü- .
.
üü. /
Value
üü/ 4
;
üü4 5
if
°° 
(
°° 
int
°° 
.
°° 
TryParse
°° 
(
°° 
referenceIdValue
°° -
,
°°- .
out
°°/ 2
var
°°3 6
referenceId
°°7 B
)
°°B C
)
°°C D
{
¢¢ 
return
££ 
referenceId
££ "
;
££" #
}
§§ 
return
¶¶ 
null
¶¶ 
;
¶¶ 
}
ßß 	
}
©© 
}´´ ‘4
\C:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Controllers\AuthController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{		 
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
class 
AuthController 
:  !
ControllerBase" 0
{ 
private 
readonly 
IAuthService %
_authService& 2
;2 3
public 
AuthController 
( 
IAuthService *
authService+ 6
)6 7
{ 	
_authService 
= 
authService &
;& '
} 	
[ 	
HttpPost	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Register) 1
(1 2
RegisterDto2 =
dto> A
)A B
{ 	
var 
result 
= 
await 
_authService +
.+ ,
RegisterAsync, 9
(9 :
dto: =
)= >
;> ?
if 
( 
! 
result 
. 
Success 
)  
{ 
return 

BadRequest !
(! "
result" (
.( )
Message) 0
)0 1
;1 2
} 
return 
Ok 
( 
result 
. 
Data !
)! "
;" #
}   	
["" 	
HttpPost""	 
("" 
$str"" $
)""$ %
]""% &
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
RegisterPatient##) 8
(##8 9
RegisterPatientDto##9 K
dto##L O
)##O P
{$$ 	
var%% 
result%% 
=%% 
await&& 
_authService&& "
.&&" # 
RegisterPatientAsync&&# 7
(&&7 8
dto&&8 ;
)&&; <
;&&< =
if(( 
((( 
!(( 
result(( 
.(( 
Success(( 
)((  
{)) 
return** 

BadRequest** !
(**! "
result**" (
.**( )
Message**) 0
)**0 1
;**1 2
}++ 
return-- 
Ok-- 
(-- 
result-- 
.-- 
Data-- !
)--! "
;--" #
}.. 	
[00 	
HttpPost00	 
(00 
$str00 
)00 
]00 
public11 
async11 
Task11 
<11 
IActionResult11 '
>11' (
Login11) .
(11. /
LoginDto11/ 7
dto118 ;
)11; <
{22 	
var33 
result33 
=33 
await33 
_authService33 +
.33+ ,

LoginAsync33, 6
(336 7
dto337 :
)33: ;
;33; <
if55 
(55 
!55 
result55 
.55 
Success55 
)55  
{66 
return77 
Unauthorized77 #
(77# $
result77$ *
.77* +
Message77+ 2
)772 3
;773 4
}88 
return:: 
Ok:: 
(:: 
result:: 
.:: 
Data:: !
)::! "
;::" #
};; 	
[== 	
HttpPut==	 
(== 
$str== "
)==" #
]==# $
[>> 	
	Authorize>>	 
]>> 
public?? 
async?? 
Task?? 
<?? 
IActionResult?? '
>??' (
ChangePassword??) 7
(??7 8
[@@ 
FromBody@@ 
]@@ 
ChangePasswordDto@@  
request@@! (
)@@( )
{AA 	
varBB 
emailBB 
=BB 
UserCC 
.CC 
	FindFirstCC 
(CC #
JwtRegisteredClaimNamesCC 6
.CC6 7
EmailCC7 <
)CC< =
?CC= >
.CC> ?
ValueCC? D
??CCE G
UserDD 
.DD 
	FindFirstDD 
(DD 

ClaimTypesDD )
.DD) *
EmailDD* /
)DD/ 0
?DD0 1
.DD1 2
ValueDD2 7
;DD7 8
ifFF 
(FF 
stringFF 
.FF 
IsNullOrWhiteSpaceFF )
(FF) *
emailFF* /
)FF/ 0
)FF0 1
{GG 
returnHH 
UnauthorizedHH #
(HH# $
$strHH$ L
)HHL M
;HHM N
}II 
varKK 
resultKK 
=KK 
awaitLL 
_authServiceLL "
.LL" #
ChangePasswordAsyncLL# 6
(LL6 7
emailLL7 <
,LL< =
requestLL> E
)LLE F
;LLF G
ifNN 
(NN 
!NN 
resultNN 
.NN 
SuccessNN 
)NN  
{OO 
returnPP 

BadRequestPP !
(PP! "
resultPP" (
.PP( )
MessagePP) 0
)PP0 1
;PP1 2
}QQ 
returnSS 
OkSS 
(SS 
newSS 
{TT 
messageUU 
=UU 
resultUU  
.UU  !
MessageUU! (
}VV 
)VV 
;VV 
}WW 	
[YY 	
HttpPostYY	 
(YY 
$strYY !
)YY! "
]YY" #
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
IActionResultZZ '
>ZZ' (
RefreshTokenZZ) 5
(ZZ5 6
RefreshTokenDtoZZ6 E
dtoZZF I
)ZZI J
{[[ 	
var\\ 
result\\ 
=\\ 
await]] 
_authService]] "
.]]" #
RefreshTokenAsync]]# 4
(]]4 5
dto]]5 8
)]]8 9
;]]9 :
if__ 
(__ 
!__ 
result__ 
.__ 
Success__ 
)__  
{`` 
returnaa 
Unauthorizedaa #
(aa# $
resultaa$ *
.aa* +
Messageaa+ 2
)aa2 3
;aa3 4
}bb 
returndd 
Okdd 
(dd 
resultdd 
.dd 
Datadd !
)dd! "
;dd" #
}ee 	
}ii 
}jj ¨ø
cC:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Controllers\AppointmentController.cs
	namespace 	
S3_HealthAxisApi
 
. 
Controllers &
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
[

 
ApiController

 
]

 
[ 
	Authorize 
] 
public 

class "
AppointmentsController '
:( )
ControllerBase* 8
{ 
private 
readonly 
IAppointmentService ,
_appointmentService- @
;@ A
public "
AppointmentsController %
(% &
IAppointmentService 
appointmentService  2
)2 3
{ 	
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
ActionResult &
<& '
IEnumerable' 2
<2 3!
AppointmentDetailsDto3 H
>H I
>I J
>J K
GetAllL R
(R S
)S T
{ 	
var 
appointments 
= 
await $
_appointmentService% 8
.8 9
GetAllAsync9 D
(D E
)E F
;F G
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
)%%# $
{&& 
return'' 
NotFound'' 
(''  
$"''  "
$str''" .
{''. /
id''/ 1
}''1 2
$str''2 =
"''= >
)''> ?
;''? @
}(( 
if** 
(** 
User** 
.** 
IsInRole** 
(** 
$str** '
)**' (
)**( )
{++ 
var,, 
patientIdFromToken,, &
=,,' (*
GetPatientReferenceIdFromToken,,) G
(,,G H
),,H I
;,,I J
if.. 
(.. 
!.. 
patientIdFromToken.. '
...' (
HasValue..( 0
||..1 3
appointment// 
.//  
	PatientId//  )
!=//* ,
patientIdFromToken//- ?
.//? @
Value//@ E
)//E F
{00 
return11 
Forbid11 !
(11! "
)11" #
;11# $
}22 
}33 
return55 
Ok55 
(55 
appointment55 !
)55! "
;55" #
}66 	
[88 	
HttpGet88	 
(88 
$str88 *
)88* +
]88+ ,
[99 	
	Authorize99	 
(99 
Roles99 
=99 
$str99 1
)991 2
]992 3
public:: 
async:: 
Task:: 
<:: 
IActionResult:: '
>::' (
GetPatientHistory::) :
(::: ;
int;; 
	patientId;; 
);; 
{<< 	
if== 
(== 
User== 
.== 
IsInRole== 
(== 
$str== '
)==' (
)==( )
{>> 
var?? 
patientIdFromToken?? &
=??' (*
GetPatientReferenceIdFromToken??) G
(??G H
)??H I
;??I J
ifAA 
(AA 
!AA 
patientIdFromTokenAA '
.AA' (
HasValueAA( 0
||AA1 3
patientIdFromTokenBB &
.BB& '
ValueBB' ,
!=BB- /
	patientIdBB0 9
)BB9 :
{CC 
returnDD 
ForbidDD !
(DD! "
)DD" #
;DD# $
}EE 
}FF 
varHH 
appointmentsHH 
=HH 
awaitII 
_appointmentServiceII )
.JJ "
GetPatientHistoryAsyncJJ +
(JJ+ ,
	patientIdJJ, 5
)JJ5 6
;JJ6 7
returnLL 
OkLL 
(LL 
appointmentsLL "
)LL" #
;LL# $
}MM 	
[OO 	
HttpGetOO	 
(OO 
$strOO .
)OO. /
]OO/ 0
[PP 	
	AuthorizePP	 
(PP 
RolesPP 
=PP 
$strPP )
)PP) *
]PP* +
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
IActionResultQQ '
>QQ' ("
GetDoctorTodayScheduleQQ) ?
(QQ? @
intRR 
doctorIdRR 
)RR 
{SS 	
varTT 
scheduleTT 
=TT 
awaitUU 
_appointmentServiceUU )
.VV '
GetDoctorTodayScheduleAsyncVV 0
(VV0 1
doctorIdVV1 9
)VV9 :
;VV: ;
returnXX 
OkXX 
(XX 
scheduleXX 
)XX 
;XX  
}YY 	
[[[ 	
HttpGet[[	 
([[ 
$str[[ -
)[[- .
][[. /
[\\ 	
	Authorize\\	 
(\\ 
Roles\\ 
=\\ 
$str\\ )
)\\) *
]\\* +
public]] 
async]] 
Task]] 
<]] 
IActionResult]] '
>]]' (!
GetDoctorWeekSchedule]]) >
(]]> ?
int^^ 
doctorId^^ 
,^^ 
[__ 
	FromQuery__ 
]__ 
DateOnly__  
	startDate__! *
,__* +
[`` 
	FromQuery`` 
]`` 
DateOnly``  
endDate``! (
)``( )
{aa 	
varbb 
schedulebb 
=bb 
awaitcc 
_appointmentServicecc )
.dd &
GetDoctorWeekScheduleAsyncdd /
(dd/ 0
doctorIdee  
,ee  !
	startDateff !
,ff! "
endDategg 
)gg  
;gg  !
returnii 
Okii 
(ii 
scheduleii 
)ii 
;ii  
}jj 	
[ll 	
HttpGetll	 
(ll 
$strll 1
)ll1 2
]ll2 3
[mm 	
	Authorizemm	 
(mm 
Rolesmm 
=mm 
$strmm )
)mm) *
]mm* +
publicnn 
asyncnn 
Tasknn 
<nn 
IActionResultnn '
>nn' (%
GetDoctorUpcomingSchedulenn) B
(nnB C
intnnC F
doctorIdnnG O
)nnO P
{oo 	
varpp 
resultpp 
=pp 
awaitqq 
_appointmentServiceqq )
.rr *
GetDoctorUpcomingScheduleAsyncrr 3
(rr3 4
doctorIdss  
)ss  !
;ss! "
returnuu 
Okuu 
(uu 
resultuu 
)uu 
;uu 
}vv 	
[xx 	
HttpPostxx	 
]xx 
[yy 	
	Authorizeyy	 
(yy 
Rolesyy 
=yy 
$stryy *
)yy* +
]yy+ ,
publiczz 
asynczz 
Taskzz 
<zz 
IActionResultzz '
>zz' (
Createzz) /
(zz/ 0
[{{ 
FromBody{{ 
]{{  
CreateAppointmentDto{{ +
dto{{, /
){{/ 0
{|| 	
try}} 
{~~ 
if 
( 
User 
. 
IsInRole !
(! "
$str" +
)+ ,
), -
{
ÄÄ 
var
ÅÅ  
patientIdFromToken
ÅÅ *
=
ÅÅ+ ,,
GetPatientReferenceIdFromToken
ÅÅ- K
(
ÅÅK L
)
ÅÅL M
;
ÅÅM N
if
ÉÉ 
(
ÉÉ 
!
ÉÉ  
patientIdFromToken
ÉÉ +
.
ÉÉ+ ,
HasValue
ÉÉ, 4
||
ÉÉ5 7
dto
ÑÑ 
.
ÑÑ 
	PatientId
ÑÑ %
!=
ÑÑ& ( 
patientIdFromToken
ÑÑ) ;
.
ÑÑ; <
Value
ÑÑ< A
)
ÑÑA B
{
ÖÖ 
return
ÜÜ 
Forbid
ÜÜ %
(
ÜÜ% &
)
ÜÜ& '
;
ÜÜ' (
}
áá 
}
àà 
var
ää 
appointment
ää 
=
ää  !
await
ãã !
_appointmentService
ãã -
.
ãã- .
CreateAsync
ãã. 9
(
ãã9 :
dto
ãã: =
)
ãã= >
;
ãã> ?
return
çç 
CreatedAtAction
çç &
(
çç& '
nameof
éé 
(
éé 
GetById
éé "
)
éé" #
,
éé# $
new
èè 
{
èè 
id
èè 
=
èè 
appointment
èè *
.
èè* +
AppointmentId
èè+ 8
}
èè9 :
,
èè: ;
appointment
êê 
)
êê  
;
êê  !
}
ëë 
catch
íí 
(
íí 
ArgumentException
íí $
ex
íí% '
)
íí' (
{
ìì 
return
îî 

BadRequest
îî !
(
îî! "
ex
îî" $
.
îî$ %
Message
îî% ,
)
îî, -
;
îî- .
}
ïï 
catch
ññ 
(
ññ '
InvalidOperationException
ññ ,
ex
ññ- /
)
ññ/ 0
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
ôô 
catch
öö 
(
öö "
KeyNotFoundException
öö '
ex
öö( *
)
öö* +
{
õõ 
return
úú 
NotFound
úú 
(
úú  
ex
úú  "
.
úú" #
Message
úú# *
)
úú* +
;
úú+ ,
}
ùù 
}
ûû 	
[
†† 	
HttpPut
††	 
(
†† 
$str
†† 
)
†† 
]
†† 
[
°° 	
	Authorize
°°	 
(
°° 
Roles
°° 
=
°° 
$str
°° *
)
°°* +
]
°°+ ,
public
¢¢ 
async
¢¢ 
Task
¢¢ 
<
¢¢ 
IActionResult
¢¢ '
>
¢¢' (
Update
¢¢) /
(
¢¢/ 0
int
££ 
id
££ 
,
££ 
[
§§ 
FromBody
§§ 
]
§§ "
UpdateAppointmentDto
§§ +
dto
§§, /
)
§§/ 0
{
•• 	
try
¶¶ 
{
ßß 
var
®® !
existingAppointment
®® '
=
®®( )
await
©© !
_appointmentService
©© -
.
©©- .
GetByIdAsync
©©. :
(
©©: ;
id
©©; =
)
©©= >
;
©©> ?
if
´´ 
(
´´ !
existingAppointment
´´ '
==
´´( *
null
´´+ /
)
´´/ 0
{
¨¨ 
return
≠≠ 
NotFound
≠≠ #
(
≠≠# $
$"
≠≠$ &
$str
≠≠& 2
{
≠≠2 3
id
≠≠3 5
}
≠≠5 6
$str
≠≠6 A
"
≠≠A B
)
≠≠B C
;
≠≠C D
}
ÆÆ 
if
∞∞ 
(
∞∞ 
User
∞∞ 
.
∞∞ 
IsInRole
∞∞ !
(
∞∞! "
$str
∞∞" +
)
∞∞+ ,
)
∞∞, -
{
±± 
var
≤≤  
patientIdFromToken
≤≤ *
=
≤≤+ ,,
GetPatientReferenceIdFromToken
≤≤- K
(
≤≤K L
)
≤≤L M
;
≤≤M N
if
¥¥ 
(
¥¥ 
!
¥¥  
patientIdFromToken
¥¥ +
.
¥¥+ ,
HasValue
¥¥, 4
||
¥¥5 7!
existingAppointment
µµ +
.
µµ+ ,
	PatientId
µµ, 5
!=
µµ6 8 
patientIdFromToken
µµ9 K
.
µµK L
Value
µµL Q
)
µµQ R
{
∂∂ 
return
∑∑ 
Forbid
∑∑ %
(
∑∑% &
)
∑∑& '
;
∑∑' (
}
∏∏ 
}
ππ 
await
ªª !
_appointmentService
ªª )
.
ºº 
UpdateAsync
ºº  
(
ºº  !
id
ºº! #
,
ºº# $
dto
ºº% (
)
ºº( )
;
ºº) *
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
¿¿ '
ex
¿¿( *
)
¿¿* +
{
¡¡ 
return
¬¬ 
NotFound
¬¬ 
(
¬¬  
ex
¬¬  "
.
¬¬" #
Message
¬¬# *
)
¬¬* +
;
¬¬+ ,
}
√√ 
catch
ƒƒ 
(
ƒƒ 
ArgumentException
ƒƒ $
ex
ƒƒ% '
)
ƒƒ' (
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
«« 
catch
»» 
(
»» '
InvalidOperationException
»» ,
ex
»»- /
)
»»/ 0
{
…… 
return
   

BadRequest
   !
(
  ! "
ex
  " $
.
  $ %
Message
  % ,
)
  , -
;
  - .
}
ÀÀ 
}
ÃÃ 	
[
ŒŒ 	
HttpPut
ŒŒ	 
(
ŒŒ 
$str
ŒŒ #
)
ŒŒ# $
]
ŒŒ$ %
[
œœ 	
	Authorize
œœ	 
(
œœ 
Roles
œœ 
=
œœ 
$str
œœ )
)
œœ) *
]
œœ* +
public
–– 
async
–– 
Task
–– 
<
–– 
IActionResult
–– '
>
––' (
Confirm
––) 0
(
––0 1
int
––1 4
id
––5 7
)
––7 8
{
—— 	
try
““ 
{
”” 
await
‘‘ !
_appointmentService
‘‘ )
.
’’ 
ConfirmAsync
’’ !
(
’’! "
id
’’" $
)
’’$ %
;
’’% &
return
◊◊ 
	NoContent
◊◊  
(
◊◊  !
)
◊◊! "
;
◊◊" #
}
ÿÿ 
catch
ŸŸ 
(
ŸŸ "
KeyNotFoundException
ŸŸ '
)
ŸŸ' (
{
⁄⁄ 
return
€€ 
NotFound
€€ 
(
€€  
)
€€  !
;
€€! "
}
‹‹ 
catch
›› 
(
›› '
InvalidOperationException
›› ,
ex
››- /
)
››/ 0
{
ﬁﬁ 
return
ﬂﬂ 

BadRequest
ﬂﬂ !
(
ﬂﬂ! "
ex
ﬂﬂ" $
.
ﬂﬂ$ %
Message
ﬂﬂ% ,
)
ﬂﬂ, -
;
ﬂﬂ- .
}
‡‡ 
}
·· 	
[
„„ 	
HttpPut
„„	 
(
„„ 
$str
„„ $
)
„„$ %
]
„„% &
[
‰‰ 	
	Authorize
‰‰	 
(
‰‰ 
Roles
‰‰ 
=
‰‰ 
$str
‰‰ )
)
‰‰) *
]
‰‰* +
public
ÂÂ 
async
ÂÂ 
Task
ÂÂ 
<
ÂÂ 
IActionResult
ÂÂ '
>
ÂÂ' (
Complete
ÂÂ) 1
(
ÂÂ1 2
int
ÂÂ2 5
id
ÂÂ6 8
)
ÂÂ8 9
{
ÊÊ 	
try
ÁÁ 
{
ËË 
await
ÈÈ !
_appointmentService
ÈÈ )
.
ÍÍ 
CompleteAsync
ÍÍ "
(
ÍÍ" #
id
ÍÍ# %
)
ÍÍ% &
;
ÍÍ& '
return
ÏÏ 
	NoContent
ÏÏ  
(
ÏÏ  !
)
ÏÏ! "
;
ÏÏ" #
}
ÌÌ 
catch
ÓÓ 
(
ÓÓ "
KeyNotFoundException
ÓÓ '
)
ÓÓ' (
{
ÔÔ 
return
 
NotFound
 
(
  
)
  !
;
! "
}
ÒÒ 
catch
ÚÚ 
(
ÚÚ '
InvalidOperationException
ÚÚ ,
ex
ÚÚ- /
)
ÚÚ/ 0
{
ÛÛ 
return
ÙÙ 

BadRequest
ÙÙ !
(
ÙÙ! "
ex
ÙÙ" $
.
ÙÙ$ %
Message
ÙÙ% ,
)
ÙÙ, -
;
ÙÙ- .
}
ıı 
}
ˆˆ 	
[
¯¯ 	
HttpPut
¯¯	 
(
¯¯ 
$str
¯¯ "
)
¯¯" #
]
¯¯# $
[
˘˘ 	
	Authorize
˘˘	 
]
˘˘ 
public
˙˙ 
async
˙˙ 
Task
˙˙ 
<
˙˙ 
IActionResult
˙˙ '
>
˙˙' (
UpdateStatus
˙˙) 5
(
˙˙5 6
int
˚˚ 
id
˚˚ 
,
˚˚ 
[
¸¸ 
FromBody
¸¸ 
]
¸¸ (
UpdateAppointmentStatusDto
¸¸ 1
dto
¸¸2 5
)
¸¸5 6
{
˝˝ 	
var
˛˛ !
existingAppointment
˛˛ #
=
˛˛$ %
await
ˇˇ !
_appointmentService
ˇˇ )
.
ˇˇ) *
GetByIdAsync
ˇˇ* 6
(
ˇˇ6 7
id
ˇˇ7 9
)
ˇˇ9 :
;
ˇˇ: ;
if
ÅÅ 
(
ÅÅ !
existingAppointment
ÅÅ #
==
ÅÅ$ &
null
ÅÅ' +
)
ÅÅ+ ,
{
ÇÇ 
return
ÉÉ 
NotFound
ÉÉ 
(
ÉÉ  
$"
ÉÉ  "
$str
ÉÉ" .
{
ÉÉ. /
id
ÉÉ/ 1
}
ÉÉ1 2
$str
ÉÉ2 =
"
ÉÉ= >
)
ÉÉ> ?
;
ÉÉ? @
}
ÑÑ 
if
ÜÜ 
(
ÜÜ 
User
ÜÜ 
.
ÜÜ 
IsInRole
ÜÜ 
(
ÜÜ 
$str
ÜÜ '
)
ÜÜ' (
)
ÜÜ( )
{
áá 
var
àà  
patientIdFromToken
àà &
=
àà' (,
GetPatientReferenceIdFromToken
àà) G
(
ààG H
)
ààH I
;
ààI J
if
ää 
(
ää 
!
ää  
patientIdFromToken
ää '
.
ää' (
HasValue
ää( 0
||
ää1 3!
existingAppointment
ãã '
.
ãã' (
	PatientId
ãã( 1
!=
ãã2 4 
patientIdFromToken
ãã5 G
.
ããG H
Value
ããH M
)
ããM N
{
åå 
return
çç 
Forbid
çç !
(
çç! "
)
çç" #
;
çç# $
}
éé 
}
èè 
await
ëë !
_appointmentService
ëë %
.
íí 
UpdateStatusAsync
íí "
(
íí" #
id
íí# %
,
íí% &
dto
íí' *
)
íí* +
;
íí+ ,
return
îî 
	NoContent
îî 
(
îî 
)
îî 
;
îî 
}
ïï 	
[
óó 	
HttpPut
óó	 
(
óó 
$str
óó "
)
óó" #
]
óó# $
[
òò 	
	Authorize
òò	 
(
òò 
Roles
òò 
=
òò 
$str
òò 1
)
òò1 2
]
òò2 3
public
ôô 
async
ôô 
Task
ôô 
<
ôô 
IActionResult
ôô '
>
ôô' (
Cancel
ôô) /
(
ôô/ 0
int
öö 
id
öö 
,
öö 
[
õõ 
FromBody
õõ 
]
õõ "
CancelAppointmentDto
õõ +
dto
õõ, /
)
õõ/ 0
{
úú 	
try
ùù 
{
ûû 
var
üü !
existingAppointment
üü '
=
üü( )
await
†† !
_appointmentService
†† -
.
††- .
GetByIdAsync
††. :
(
††: ;
id
††; =
)
††= >
;
††> ?
if
¢¢ 
(
¢¢ !
existingAppointment
¢¢ '
==
¢¢( *
null
¢¢+ /
)
¢¢/ 0
{
££ 
return
§§ 
NotFound
§§ #
(
§§# $
$"
§§$ &
$str
§§& 2
{
§§2 3
id
§§3 5
}
§§5 6
$str
§§6 A
"
§§A B
)
§§B C
;
§§C D
}
•• 
if
ßß 
(
ßß 
User
ßß 
.
ßß 
IsInRole
ßß !
(
ßß! "
$str
ßß" +
)
ßß+ ,
)
ßß, -
{
®® 
var
©©  
patientIdFromToken
©© *
=
©©+ ,,
GetPatientReferenceIdFromToken
©©- K
(
©©K L
)
©©L M
;
©©M N
if
´´ 
(
´´ 
!
´´  
patientIdFromToken
´´ +
.
´´+ ,
HasValue
´´, 4
||
´´5 7!
existingAppointment
¨¨ +
.
¨¨+ ,
	PatientId
¨¨, 5
!=
¨¨6 8 
patientIdFromToken
¨¨9 K
.
¨¨K L
Value
¨¨L Q
)
¨¨Q R
{
≠≠ 
return
ÆÆ 
Forbid
ÆÆ %
(
ÆÆ% &
)
ÆÆ& '
;
ÆÆ' (
}
ØØ 
dto
±± 
.
±±  
CancellationReason
±± *
=
±±+ ,
$"
≤≤ 
$str
≤≤ 0
{
≤≤0 1
dto
≤≤1 4
.
≤≤4 5 
CancellationReason
≤≤5 G
}
≤≤G H
"
≤≤H I
;
≤≤I J
}
≥≥ 
if
µµ 
(
µµ 
User
µµ 
.
µµ 
IsInRole
µµ !
(
µµ! "
$str
µµ" *
)
µµ* +
)
µµ+ ,
{
∂∂ 
var
∑∑ 
doctorIdFromToken
∑∑ )
=
∑∑* +,
GetPatientReferenceIdFromToken
∑∑, J
(
∑∑J K
)
∑∑K L
;
∑∑L M
if
ππ 
(
ππ 
!
ππ 
doctorIdFromToken
ππ *
.
ππ* +
HasValue
ππ+ 3
||
ππ4 6!
existingAppointment
∫∫ +
.
∫∫+ ,
DoctorId
∫∫, 4
!=
∫∫5 7
doctorIdFromToken
∫∫8 I
.
∫∫I J
Value
∫∫J O
)
∫∫O P
{
ªª 
return
ºº 
Forbid
ºº %
(
ºº% &
)
ºº& '
;
ºº' (
}
ΩΩ 
dto
øø 
.
øø  
CancellationReason
øø *
=
øø+ ,
$"
¿¿ 
$str
¿¿ /
{
¿¿/ 0
dto
¿¿0 3
.
¿¿3 4 
CancellationReason
¿¿4 F
}
¿¿F G
"
¿¿G H
;
¿¿H I
}
¡¡ 
await
√√ !
_appointmentService
√√ )
.
√√) *
CancelAsync
√√* 5
(
√√5 6
id
√√6 8
,
√√8 9
dto
√√: =
)
√√= >
;
√√> ?
return
≈≈ 
	NoContent
≈≈  
(
≈≈  !
)
≈≈! "
;
≈≈" #
}
∆∆ 
catch
«« 
(
«« "
KeyNotFoundException
«« '
)
««' (
{
»» 
return
…… 
NotFound
…… 
(
……  
)
……  !
;
……! "
}
   
catch
ÀÀ 
(
ÀÀ 
ArgumentException
ÀÀ $
ex
ÀÀ% '
)
ÀÀ' (
{
ÃÃ 
return
ÕÕ 

BadRequest
ÕÕ !
(
ÕÕ! "
ex
ÕÕ" $
.
ÕÕ$ %
Message
ÕÕ% ,
)
ÕÕ, -
;
ÕÕ- .
}
ŒŒ 
catch
œœ 
(
œœ '
InvalidOperationException
œœ ,
ex
œœ- /
)
œœ/ 0
{
–– 
return
—— 

BadRequest
—— !
(
——! "
ex
——" $
.
——$ %
Message
——% ,
)
——, -
;
——- .
}
““ 
}
”” 	
private
’’ 
int
’’ 
?
’’ ,
GetPatientReferenceIdFromToken
’’ 3
(
’’3 4
)
’’4 5
{
÷÷ 	
var
◊◊ 
referenceIdValue
◊◊  
=
◊◊! "
User
◊◊# '
.
◊◊' (
	FindFirst
◊◊( 1
(
◊◊1 2
$str
◊◊2 ?
)
◊◊? @
?
◊◊@ A
.
◊◊A B
Value
◊◊B G
;
◊◊G H
if
ŸŸ 
(
ŸŸ 
int
ŸŸ 
.
ŸŸ 
TryParse
ŸŸ 
(
ŸŸ 
referenceIdValue
ŸŸ -
,
ŸŸ- .
out
ŸŸ/ 2
var
ŸŸ3 6
referenceId
ŸŸ7 B
)
ŸŸB C
)
ŸŸC D
{
⁄⁄ 
return
€€ 
referenceId
€€ "
;
€€" #
}
‹‹ 
return
ﬁﬁ 
null
ﬁﬁ 
;
ﬁﬁ 
}
ﬂﬂ 	
}
‡‡ 
}·· 
]C:\Users\287766\source\repos\S3_HealthAxisWeb\S3_HealthAxisApi\Controllers\AdminController.cs
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