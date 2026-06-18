€
^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Interface\IUserService.cs
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
	interface 
IUserService !
{ 
} 
} Œ
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
} ı
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
Task 
ActivateAsync 
( 
int 
id !
)! "
;" #
Task 
DeactivateAsync 
( 
int  
id! #
)# $
;$ %
} 
} ≠
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
RegisterAsync 
( 
RegisterDto %
request& -
)- .
;. /
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

LoginAsync 
( 
LoginDto 
request  '
)' (
;( )
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
RefreshTokenAsync 
( 
RefreshTokenDto -
request. 5
)5 6
;6 7
} 
} Ä
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
Task 
UpdateAsync 
( 
int 
id 
,   
UpdateAppointmentDto! 5
dto6 9
)9 :
;: ;
Task 
UpdateStatusAsync 
( 
int "
id# %
,% &&
UpdateAppointmentStatusDto' A
dtoB E
)E F
;F G
Task 
ConfirmAsync 
( 
int 
id  
)  !
;! "
Task 
CompleteAsync 
( 
int 
id !
)! "
;" #
Task 
CancelAsync 
( 
int 
id 
,   
CancelAppointmentDto! 5
dto6 9
)9 :
;: ;
} 
} à
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\UserService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public 

class 
UserService 
: 
IUserService +
{ 
} 
} Ær
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
}∫∫ Äw
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
;< =
public 
DoctorService 
( 
IDoctorRepository .
doctorRepository/ ?
)? @
{ 	
_doctorRepository 
= 
doctorRepository  0
;0 1
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
	DoctorDto& /
>/ 0
>0 1
GetAllAsync2 =
(= >
string> D
?D E
sortByF L
,L M
intN Q
?Q R
specialisationS a
)a b
{ 	
var 
doctors 
= 
await 
_doctorRepository  1
.1 2
GetAllAsync2 =
(= >
sortBy> D
,D E
specialisationF T
)T U
;U V
return 
doctors 
. 
Select !
(! "
MapToDoctorDto" 0
)0 1
;1 2
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
	DoctorDto& /
>/ 0
>0 1*
GetActiveBySpecialisationAsync2 P
(P Q
intQ T
specialisationU c
)c d
{ 	
if 
( 
! 
Enum 
. 
	IsDefined 
(  
typeof  &
(& ' 
DoctorSpecialisation' ;
); <
,< =
specialisation> L
)L M
)M N
throw 
new 
ArgumentException +
(+ ,
$str, L
)L M
;M N
var 
doctors 
= 
await 
_doctorRepository  1
.1 2*
GetActiveBySpecialisationAsync2 P
(P Q
specialisationQ _
)_ `
;` a
return   
doctors   
.   
Select   !
(  ! "
MapToDoctorDto  " 0
)  0 1
;  1 2
}!! 	
public## 
async## 
Task## 
<## 
	DoctorDto## #
?### $
>##$ %
GetByIdAsync##& 2
(##2 3
int##3 6
id##7 9
)##9 :
{$$ 	
var%% 
doctor%% 
=%% 
await%% 
_doctorRepository%% 0
.%%0 1
GetByIdAsync%%1 =
(%%= >
id%%> @
)%%@ A
;%%A B
return'' 
doctor'' 
=='' 
null'' !
?(( 
null(( 
:)) 
MapToDoctorDto))  
())  !
doctor))! '
)))' (
;))( )
}** 	
public,, 
async,, 
Task,, 
<,, 
	DoctorDto,, #
>,,# $
CreateAsync,,% 0
(,,0 1
CreateDoctorDto,,1 @
dto,,A D
),,D E
{-- 	
ValidateDoctor.. 
(.. 
dto.. 
).. 
;..  
var00 
doctor00 
=00 
new00 
Doctor00 #
{11 
FullName22 
=22 
dto22 
.22 
FullName22 '
.22' (
Trim22( ,
(22, -
)22- .
,22. /
Specialisation33 
=33  
(33! " 
DoctorSpecialisation33" 6
)336 7
dto337 :
.33: ;
Specialisation33; I
,33I J
YearsOfExperience44 !
=44" #
dto44$ '
.44' (
YearsOfExperience44( 9
,449 :
ConsultationFee55 
=55  !
dto55" %
.55% &
ConsultationFee55& 5
,555 6
IsActive66 
=66 
true66 
}77 
;77 
await99 
_doctorRepository99 #
.99# $
AddAsync99$ ,
(99, -
doctor99- 3
)993 4
;994 5
await:: 
_doctorRepository:: #
.::# $
SaveChangesAsync::$ 4
(::4 5
)::5 6
;::6 7
return<< 
MapToDoctorDto<< !
(<<! "
doctor<<" (
)<<( )
;<<) *
}== 	
public?? 
async?? 
Task?? 
UpdateAsync?? %
(??% &
int??& )
id??* ,
,??, -
UpdateDoctorDto??. =
dto??> A
)??A B
{@@ 	
ValidateDoctorAA 
(AA 
dtoAA 
)AA 
;AA  
varCC 
doctorCC 
=CC 
awaitCC 
_doctorRepositoryCC 0
.CC0 1
GetByIdAsyncCC1 =
(CC= >
idCC> @
)CC@ A
;CCA B
ifEE 
(EE 
doctorEE 
==EE 
nullEE 
)EE 
throwFF 
newFF  
KeyNotFoundExceptionFF .
(FF. /
$"FF/ 1
$strFF1 @
{FF@ A
idFFA C
}FFC D
$strFFD O
"FFO P
)FFP Q
;FFQ R
doctorHH 
.HH 
FullNameHH 
=HH 
dtoHH !
.HH! "
FullNameHH" *
.HH* +
TrimHH+ /
(HH/ 0
)HH0 1
;HH1 2
doctorII 
.II 
SpecialisationII !
=II" #
(II$ % 
DoctorSpecialisationII% 9
)II9 :
dtoII: =
.II= >
SpecialisationII> L
;IIL M
doctorJJ 
.JJ 
YearsOfExperienceJJ $
=JJ% &
dtoJJ' *
.JJ* +
YearsOfExperienceJJ+ <
;JJ< =
doctorKK 
.KK 
ConsultationFeeKK "
=KK# $
dtoKK% (
.KK( )
ConsultationFeeKK) 8
;KK8 9
awaitMM 
_doctorRepositoryMM #
.MM# $
UpdateAsyncMM$ /
(MM/ 0
doctorMM0 6
)MM6 7
;MM7 8
awaitNN 
_doctorRepositoryNN #
.NN# $
SaveChangesAsyncNN$ 4
(NN4 5
)NN5 6
;NN6 7
}OO 	
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
IEnumerableQQ %
<QQ% &
intQQ& )
>QQ) *
>QQ* + 
GetAvailabilityAsyncQQ, @
(QQ@ A
intQQA D
doctorIdQQE M
,QQM N
DateOnlyQQN V
dateQQW [
)QQ[ \
{RR 	
varSS 
doctorSS 
=SS 
awaitTT 
_doctorRepositoryTT '
.TT' (
GetByIdAsyncTT( 4
(TT4 5
doctorIdTT5 =
)TT= >
;TT> ?
ifVV 
(VV 
doctorVV 
==VV 
nullVV 
)VV 
throwWW 
newWW  
KeyNotFoundExceptionWW .
(WW. /
$strXX '
)XX' (
;XX( )
varZZ 
bookedSlotsZZ 
=ZZ 
await[[ 
_doctorRepository[[ '
.[[' (
GetBookedSlotsAsync[[( ;
([[; <
doctorId\\ 
,\\ 
date]] 
)]] 
;]] 
var__ 
allSlots__ 
=__ 
Enum`` 
.`` 
	GetValues`` 
<`` 
AppointmentTimeSlot`` 2
>``2 3
(``3 4
)``4 5
.aa 
Selectaa 
(aa 
xaa 
=>aa  
(aa! "
intaa" %
)aa% &
xaa& '
)aa' (
;aa( )
returncc 
allSlotscc 
.cc 
Exceptcc "
(cc" #
bookedSlotscc# .
)cc. /
;cc/ 0
}dd 	
publicff 
asyncff 
Taskff 
ActivateAsyncff '
(ff' (
intff( +
idff, .
)ff. /
{gg 	
varhh 
doctorhh 
=hh 
awaithh 
_doctorRepositoryhh 0
.hh0 1
GetByIdAsynchh1 =
(hh= >
idhh> @
)hh@ A
;hhA B
ifjj 
(jj 
doctorjj 
==jj 
nulljj 
)jj 
throwkk 
newkk  
KeyNotFoundExceptionkk .
(kk. /
$"kk/ 1
$strkk1 @
{kk@ A
idkkA C
}kkC D
$strkkD O
"kkO P
)kkP Q
;kkQ R
doctormm 
.mm 
IsActivemm 
=mm 
truemm "
;mm" #
awaitoo 
_doctorRepositoryoo #
.oo# $
UpdateAsyncoo$ /
(oo/ 0
doctoroo0 6
)oo6 7
;oo7 8
awaitpp 
_doctorRepositorypp #
.pp# $
SaveChangesAsyncpp$ 4
(pp4 5
)pp5 6
;pp6 7
}qq 	
publicss 
asyncss 
Taskss 
DeactivateAsyncss )
(ss) *
intss* -
idss. 0
)ss0 1
{tt 	
varuu 
doctoruu 
=uu 
awaituu 
_doctorRepositoryuu 0
.uu0 1
GetByIdAsyncuu1 =
(uu= >
iduu> @
)uu@ A
;uuA B
ifww 
(ww 
doctorww 
==ww 
nullww 
)ww 
throwxx 
newxx  
KeyNotFoundExceptionxx .
(xx. /
$"xx/ 1
$strxx1 @
{xx@ A
idxxA C
}xxC D
$strxxD O
"xxO P
)xxP Q
;xxQ R
doctorzz 
.zz 
IsActivezz 
=zz 
falsezz #
;zz# $
await|| 
_doctorRepository|| #
.||# $
UpdateAsync||$ /
(||/ 0
doctor||0 6
)||6 7
;||7 8
await}} 
_doctorRepository}} #
.}}# $
SaveChangesAsync}}$ 4
(}}4 5
)}}5 6
;}}6 7
}~~ 	
private
ÄÄ 
static
ÄÄ 
void
ÄÄ 
ValidateDoctor
ÄÄ *
(
ÄÄ* +
CreateDoctorDto
ÄÄ+ :
dto
ÄÄ; >
)
ÄÄ> ?
{
ÅÅ 	
if
ÇÇ 
(
ÇÇ 
string
ÇÇ 
.
ÇÇ  
IsNullOrWhiteSpace
ÇÇ )
(
ÇÇ) *
dto
ÇÇ* -
.
ÇÇ- .
FullName
ÇÇ. 6
)
ÇÇ6 7
)
ÇÇ7 8
throw
ÉÉ 
new
ÉÉ 
ArgumentException
ÉÉ +
(
ÉÉ+ ,
$str
ÉÉ, F
)
ÉÉF G
;
ÉÉG H
if
ÖÖ 
(
ÖÖ 
!
ÖÖ 
Enum
ÖÖ 
.
ÖÖ 
	IsDefined
ÖÖ 
(
ÖÖ  
typeof
ÖÖ  &
(
ÖÖ& '"
DoctorSpecialisation
ÖÖ' ;
)
ÖÖ; <
,
ÖÖ< =
dto
ÖÖ> A
.
ÖÖA B
Specialisation
ÖÖB P
)
ÖÖP Q
)
ÖÖQ R
throw
ÜÜ 
new
ÜÜ 
ArgumentException
ÜÜ +
(
ÜÜ+ ,
$str
ÜÜ, L
)
ÜÜL M
;
ÜÜM N
if
àà 
(
àà 
dto
àà 
.
àà 
YearsOfExperience
àà %
<
àà& '
$num
àà( )
||
àà* ,
dto
àà- 0
.
àà0 1
YearsOfExperience
àà1 B
>
ààC D
$num
ààE G
)
ààG H
throw
ââ 
new
ââ 
ArgumentException
ââ +
(
ââ+ ,
$str
ââ, X
)
ââX Y
;
ââY Z
if
ãã 
(
ãã 
dto
ãã 
.
ãã 
ConsultationFee
ãã #
<=
ãã$ &
$num
ãã' (
)
ãã( )
throw
åå 
new
åå 
ArgumentException
åå +
(
åå+ ,
$str
åå, Y
)
ååY Z
;
ååZ [
}
çç 	
private
èè 
static
èè 
void
èè 
ValidateDoctor
èè *
(
èè* +
UpdateDoctorDto
èè+ :
dto
èè; >
)
èè> ?
{
êê 	
if
ëë 
(
ëë 
string
ëë 
.
ëë  
IsNullOrWhiteSpace
ëë )
(
ëë) *
dto
ëë* -
.
ëë- .
FullName
ëë. 6
)
ëë6 7
)
ëë7 8
throw
íí 
new
íí 
ArgumentException
íí +
(
íí+ ,
$str
íí, F
)
ííF G
;
ííG H
if
îî 
(
îî 
!
îî 
Enum
îî 
.
îî 
	IsDefined
îî 
(
îî  
typeof
îî  &
(
îî& '"
DoctorSpecialisation
îî' ;
)
îî; <
,
îî< =
dto
îî> A
.
îîA B
Specialisation
îîB P
)
îîP Q
)
îîQ R
throw
ïï 
new
ïï 
ArgumentException
ïï +
(
ïï+ ,
$str
ïï, L
)
ïïL M
;
ïïM N
if
óó 
(
óó 
dto
óó 
.
óó 
YearsOfExperience
óó %
<
óó& '
$num
óó( )
||
óó* ,
dto
óó- 0
.
óó0 1
YearsOfExperience
óó1 B
>
óóC D
$num
óóE G
)
óóG H
throw
òò 
new
òò 
ArgumentException
òò +
(
òò+ ,
$str
òò, X
)
òòX Y
;
òòY Z
if
öö 
(
öö 
dto
öö 
.
öö 
ConsultationFee
öö #
<=
öö$ &
$num
öö' (
)
öö( )
throw
õõ 
new
õõ 
ArgumentException
õõ +
(
õõ+ ,
$str
õõ, Y
)
õõY Z
;
õõZ [
}
úú 	
private
ûû 
static
ûû 
	DoctorDto
ûû  
MapToDoctorDto
ûû! /
(
ûû/ 0
Doctor
ûû0 6
doctor
ûû7 =
)
ûû= >
{
üü 	
return
†† 
new
†† 
	DoctorDto
††  
{
°° 
DoctorId
¢¢ 
=
¢¢ 
doctor
¢¢ !
.
¢¢! "
DoctorId
¢¢" *
,
¢¢* +
FullName
££ 
=
££ 
doctor
££ !
.
££! "
FullName
££" *
,
££* +
Specialisation
§§ 
=
§§  
(
§§! "
int
§§" %
)
§§% &
doctor
§§& ,
.
§§, -
Specialisation
§§- ;
,
§§; <
YearsOfExperience
•• !
=
••" #
doctor
••$ *
.
••* +
YearsOfExperience
••+ <
,
••< =
ConsultationFee
¶¶ 
=
¶¶  !
doctor
¶¶" (
.
¶¶( )
ConsultationFee
¶¶) 8
,
¶¶8 9
IsActive
ßß 
=
ßß 
doctor
ßß !
.
ßß! "
IsActive
ßß" *
}
®® 
;
®® 
}
©© 	
}
™™ 
}´´ Ÿ
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Services\Implementation\AuthService.cs
	namespace 	
S3_HealthAxisApi
 
. 
Services #
.# $
Implementation$ 2
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
IUserRepository (
_userRepository) 8
;8 9
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
public 
AuthService 
( 
IUserRepository 
userRepository *
,* +
IConfiguration 
configuration (
)( )
{ 	
_userRepository 
= 
userRepository ,
;, -
_configuration 
= 
configuration *
;* +
} 	
public 
async 
Task 
< 
( 
bool 
Success  '
,' (
string) /
Message0 7
,7 8
AuthResponseDto9 H
?H I
DataJ N
)N O
>O P
RegisterAsync 
( 
RegisterDto %
request& -
)- .
{ 	
if 
( 
request 
. 
Password  
!=! #
request$ +
.+ ,
ConfirmPassword, ;
); <
{ 
return 
( 
false 
, 
$str 8
,8 9
null: >
)> ?
;? @
}   
if"" 
("" 
await"" 
_userRepository"" %
.""% &
EmailExistsAsync""& 6
(""6 7
request""7 >
.""> ?
Email""? D
)""D E
)""E F
{## 
return$$ 
($$ 
false$$ 
,$$ 
$str$$ 6
,$$6 7
null$$8 <
)$$< =
;$$= >
}%% 
var'' 
user'' 
='' 
new'' 
User'' 
{(( 
Email)) 
=)) 
request)) 
.))  
Email))  %
.))% &
Trim))& *
())* +
)))+ ,
.)), -
ToLower))- 4
())4 5
)))5 6
,))6 7
PasswordHash** 
=** 
HashPassword** +
(**+ ,
request**, 3
.**3 4
Password**4 <
)**< =
,**= >
Role++ 
=++ 
request++ 
.++ 
Role++ #
,++# $
CreatedDate,, 
=,, 
DateTime,, &
.,,& '
UtcNow,,' -
}-- 
;-- 
var// 
refreshToken// 
=//  
GenerateRefreshToken// 3
(//3 4
)//4 5
;//5 6
user11 
.11 
RefreshToken11 
=11 
refreshToken11  ,
;11, -
user22 
.22 "
RefreshTokenExpiryTime22 '
=22( )
DateTime22* 2
.222 3
UtcNow223 9
.229 :
AddDays22: A
(22A B
$num22B C
)22C D
;22D E
await44 
_userRepository44 !
.44! "
AddAsync44" *
(44* +
user44+ /
)44/ 0
;440 1
await55 
_userRepository55 !
.55! "
SaveChangesAsync55" 2
(552 3
)553 4
;554 5
var77 
accessToken77 
=77 
GenerateToken77 +
(77+ ,
user77, 0
)770 1
;771 2
return99 
(99 
true:: 
,:: 
$str;; /
,;;/ 0
new<< 
AuthResponseDto<< #
{== 
AccessToken>> 
=>>  !
accessToken>>" -
,>>- .
RefreshToken??  
=??! "
refreshToken??# /
,??/ 0
Email@@ 
=@@ 
user@@  
.@@  !
Email@@! &
,@@& '
RoleAA 
=AA 
userAA 
.AA  
RoleAA  $
.AA$ %
ToStringAA% -
(AA- .
)AA. /
}BB 
)BB 
;BB 
}CC 	
publicEE 
asyncEE 
TaskEE 
<EE 
(EE 
boolEE 
SuccessEE  '
,EE' (
stringEE) /
MessageEE0 7
,EE7 8
AuthResponseDtoEE9 H
?EEH I
DataEEJ N
)EEN O
>EEO P

LoginAsyncFF 
(FF 
LoginDtoFF 
requestFF  '
)FF' (
{GG 	
varHH 
userHH 
=HH 
awaitHH 
_userRepositoryHH ,
.HH, -
GetByEmailAsyncHH- <
(HH< =
requestII 
.II 
EmailII 
.II 
TrimII "
(II" #
)II# $
.II$ %
ToLowerII% ,
(II, -
)II- .
)II. /
;II/ 0
ifKK 
(KK 
userKK 
==KK 
nullKK 
)KK 
{LL 
returnMM 
(MM 
falseMM 
,MM 
$strMM ;
,MM; <
nullMM= A
)MMA B
;MMB C
}NN 
varPP 
hashedPasswordPP 
=PP  
HashPasswordPP! -
(PP- .
requestPP. 5
.PP5 6
PasswordPP6 >
)PP> ?
;PP? @
ifRR 
(RR 
userRR 
.RR 
PasswordHashRR !
!=RR" $
hashedPasswordRR% 3
)RR3 4
{SS 
returnTT 
(TT 
falseTT 
,TT 
$strTT ;
,TT; <
nullTT= A
)TTA B
;TTB C
}UU 
varWW 
accessTokenWW 
=WW 
GenerateTokenWW +
(WW+ ,
userWW, 0
)WW0 1
;WW1 2
varYY 
refreshTokenYY 
=YY  
GenerateRefreshTokenYY 3
(YY3 4
)YY4 5
;YY5 6
user[[ 
.[[ 
RefreshToken[[ 
=[[ 
refreshToken[[  ,
;[[, -
user\\ 
.\\ "
RefreshTokenExpiryTime\\ '
=\\( )
DateTime\\* 2
.\\2 3
UtcNow\\3 9
.\\9 :
AddDays\\: A
(\\A B
$num\\B C
)\\C D
;\\D E
await^^ 
_userRepository^^ !
.^^! "
UpdateAsync^^" -
(^^- .
user^^. 2
)^^2 3
;^^3 4
await__ 
_userRepository__ !
.__! "
SaveChangesAsync__" 2
(__2 3
)__3 4
;__4 5
returnaa 
(aa 
truebb 
,bb 
$strcc #
,cc# $
newdd 
AuthResponseDtodd #
{ee 
AccessTokenff 
=ff  !
accessTokenff" -
,ff- .
RefreshTokengg  
=gg! "
refreshTokengg# /
,gg/ 0
Emailhh 
=hh 
userhh  
.hh  !
Emailhh! &
,hh& '
Roleii 
=ii 
userii 
.ii  
Roleii  $
.ii$ %
ToStringii% -
(ii- .
)ii. /
}jj 
)jj 
;jj 
}kk 	
publicmm 
asyncmm 
Taskmm 
<mm 
(mm 
boolmm 
Successmm  '
,mm' (
stringmm) /
Messagemm0 7
,mm7 8
AuthResponseDtomm9 H
?mmH I
DatammJ N
)mmN O
>mmO P
RefreshTokenAsyncnn 
(nn 
RefreshTokenDtonn -
requestnn. 5
)nn5 6
{oo 	
varpp 
userpp 
=pp 
awaitpp 
_userRepositorypp ,
.qq "
GetByRefreshTokenAsyncqq '
(qq' (
requestqq( /
.qq/ 0
RefreshTokenqq0 <
)qq< =
;qq= >
ifss 
(ss 
userss 
==ss 
nullss 
)ss 
{tt 
returnuu 
(uu 
falseuu 
,uu 
$struu 7
,uu7 8
nulluu9 =
)uu= >
;uu> ?
}vv 
ifxx 
(xx 
!xx 
userxx 
.xx "
RefreshTokenExpiryTimexx ,
.xx, -
HasValuexx- 5
||xx6 8
useryy 
.yy "
RefreshTokenExpiryTimeyy +
.yy+ ,
Valueyy, 1
<=yy2 4
DateTimeyy5 =
.yy= >
UtcNowyy> D
)yyD E
{zz 
return{{ 
({{ 
false{{ 
,{{ 
$str{{ ;
,{{; <
null{{= A
){{A B
;{{B C
}|| 
var~~ 
newAccessToken~~ 
=~~  
GenerateToken~~! .
(~~. /
user~~/ 3
)~~3 4
;~~4 5
var
ÄÄ 
newRefreshToken
ÄÄ 
=
ÄÄ  !"
GenerateRefreshToken
ÄÄ" 6
(
ÄÄ6 7
)
ÄÄ7 8
;
ÄÄ8 9
user
ÇÇ 
.
ÇÇ 
RefreshToken
ÇÇ 
=
ÇÇ 
newRefreshToken
ÇÇ  /
;
ÇÇ/ 0
user
ÉÉ 
.
ÉÉ $
RefreshTokenExpiryTime
ÉÉ '
=
ÉÉ( )
DateTime
ÉÉ* 2
.
ÉÉ2 3
UtcNow
ÉÉ3 9
.
ÉÉ9 :
AddDays
ÉÉ: A
(
ÉÉA B
$num
ÉÉB C
)
ÉÉC D
;
ÉÉD E
await
ÖÖ 
_userRepository
ÖÖ !
.
ÖÖ! "
UpdateAsync
ÖÖ" -
(
ÖÖ- .
user
ÖÖ. 2
)
ÖÖ2 3
;
ÖÖ3 4
await
ÜÜ 
_userRepository
ÜÜ !
.
ÜÜ! "
SaveChangesAsync
ÜÜ" 2
(
ÜÜ2 3
)
ÜÜ3 4
;
ÜÜ4 5
return
àà 
(
àà 
true
ââ 
,
ââ 
$str
ää /
,
ää/ 0
new
ãã 
AuthResponseDto
ãã #
{
åå 
AccessToken
çç 
=
çç  !
newAccessToken
çç" 0
,
çç0 1
RefreshToken
éé  
=
éé! "
newRefreshToken
éé# 2
,
éé2 3
Email
èè 
=
èè 
user
èè  
.
èè  !
Email
èè! &
,
èè& '
Role
êê 
=
êê 
user
êê 
.
êê  
Role
êê  $
.
êê$ %
ToString
êê% -
(
êê- .
)
êê. /
}
ëë 
)
ëë 
;
ëë 
}
íí 	
private
îî 
string
îî 
GenerateToken
îî $
(
îî$ %
User
îî% )
user
îî* .
)
îî. /
{
ïï 	
var
ññ 
jwtSettings
ññ 
=
ññ 
_configuration
ññ ,
.
ññ, -

GetSection
ññ- 7
(
ññ7 8
$str
ññ8 =
)
ññ= >
;
ññ> ?
var
òò 
key
òò 
=
òò 
new
òò "
SymmetricSecurityKey
òò .
(
òò. /
Encoding
ôô 
.
ôô 
UTF8
ôô 
.
ôô 
GetBytes
ôô &
(
ôô& '
jwtSettings
ôô' 2
[
ôô2 3
$str
ôô3 8
]
ôô8 9
!
ôô9 :
)
ôô: ;
)
ôô; <
;
ôô< =
var
õõ 
credentials
õõ 
=
õõ 
new
õõ ! 
SigningCredentials
õõ" 4
(
õõ4 5
key
úú 
,
úú  
SecurityAlgorithms
ùù "
.
ùù" #

HmacSha256
ùù# -
)
ùù- .
;
ùù. /
var
üü 
claims
üü 
=
üü 
new
üü 
List
üü !
<
üü! "
Claim
üü" '
>
üü' (
{
†† 
new
°° 
Claim
°° 
(
°° %
JwtRegisteredClaimNames
¢¢ +
.
¢¢+ ,
Sub
¢¢, /
,
¢¢/ 0
user
££ 
.
££ 
UserId
££ 
.
££  
ToString
££  (
(
££( )
)
££) *
)
££* +
,
££+ ,
new
•• 
Claim
•• 
(
•• %
JwtRegisteredClaimNames
¶¶ +
.
¶¶+ ,
Email
¶¶, 1
,
¶¶1 2
user
ßß 
.
ßß 
Email
ßß 
)
ßß 
,
ßß  
new
©© 
Claim
©© 
(
©© %
JwtRegisteredClaimNames
™™ +
.
™™+ ,
Jti
™™, /
,
™™/ 0
Guid
´´ 
.
´´ 
NewGuid
´´  
(
´´  !
)
´´! "
.
´´" #
ToString
´´# +
(
´´+ ,
)
´´, -
)
´´- .
,
´´. /
new
≠≠ 
Claim
≠≠ 
(
≠≠ 

ClaimTypes
ÆÆ 
.
ÆÆ 
NameIdentifier
ÆÆ -
,
ÆÆ- .
user
ØØ 
.
ØØ 
UserId
ØØ 
.
ØØ  
ToString
ØØ  (
(
ØØ( )
)
ØØ) *
)
ØØ* +
,
ØØ+ ,
new
±± 
Claim
±± 
(
±± 

ClaimTypes
≤≤ 
.
≤≤ 
Role
≤≤ #
,
≤≤# $
user
≥≥ 
.
≥≥ 
Role
≥≥ 
.
≥≥ 
ToString
≥≥ &
(
≥≥& '
)
≥≥' (
)
≥≥( )
}
¥¥ 
;
¥¥ 
var
∂∂ 
token
∂∂ 
=
∂∂ 
new
∂∂ 
JwtSecurityToken
∂∂ ,
(
∂∂, -
issuer
∑∑ 
:
∑∑ 
jwtSettings
∑∑ #
[
∑∑# $
$str
∑∑$ ,
]
∑∑, -
,
∑∑- .
audience
∏∏ 
:
∏∏ 
jwtSettings
∏∏ %
[
∏∏% &
$str
∏∏& 0
]
∏∏0 1
,
∏∏1 2
claims
ππ 
:
ππ 
claims
ππ 
,
ππ 
expires
∫∫ 
:
∫∫ 
DateTime
∫∫ !
.
∫∫! "
UtcNow
∫∫" (
.
∫∫( )

AddMinutes
∫∫) 3
(
∫∫3 4
int
ªª 
.
ªª 
Parse
ªª 
(
ªª 
jwtSettings
ªª )
[
ªª) *
$str
ªª* H
]
ªªH I
!
ªªI J
)
ªªJ K
)
ªªK L
,
ªªL M 
signingCredentials
ºº "
:
ºº" #
credentials
ºº$ /
)
ºº/ 0
;
ºº0 1
return
ææ 
new
ææ %
JwtSecurityTokenHandler
ææ .
(
ææ. /
)
ææ/ 0
.
øø 

WriteToken
øø 
(
øø 
token
øø !
)
øø! "
;
øø" #
}
¿¿ 	
private
¬¬ 
static
¬¬ 
string
¬¬ "
GenerateRefreshToken
¬¬ 2
(
¬¬2 3
)
¬¬3 4
{
√√ 	
return
ƒƒ 
Convert
ƒƒ 
.
ƒƒ 
ToBase64String
ƒƒ )
(
ƒƒ) *#
RandomNumberGenerator
≈≈ %
.
≈≈% &
GetBytes
≈≈& .
(
≈≈. /
$num
≈≈/ 1
)
≈≈1 2
)
≈≈2 3
;
≈≈3 4
}
∆∆ 	
private
»» 
static
»» 
string
»» 
HashPassword
»» *
(
»»* +
string
»»+ 1
password
»»2 :
)
»»: ;
{
…… 	
using
   
var
   
sha256
   
=
   
SHA256
   %
.
  % &
Create
  & ,
(
  , -
)
  - .
;
  . /
byte
ÃÃ 
[
ÃÃ 
]
ÃÃ 
bytes
ÃÃ 
=
ÃÃ 
Encoding
ÃÃ #
.
ÃÃ# $
UTF8
ÃÃ$ (
.
ÃÃ( )
GetBytes
ÃÃ) 1
(
ÃÃ1 2
password
ÃÃ2 :
)
ÃÃ: ;
;
ÃÃ; <
byte
ŒŒ 
[
ŒŒ 
]
ŒŒ 
hash
ŒŒ 
=
ŒŒ 
sha256
ŒŒ  
.
ŒŒ  !
ComputeHash
ŒŒ! ,
(
ŒŒ, -
bytes
ŒŒ- 2
)
ŒŒ2 3
;
ŒŒ3 4
return
–– 
Convert
–– 
.
–– 
ToBase64String
–– )
(
––) *
hash
––* .
)
––. /
;
––/ 0
}
—— 	
}
““ 
}”” œÚ
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
õõ, /
id
õõ0 2
,
õõ2 3(
UpdateAppointmentStatusDto
õõ3 M
dto
õõN Q
)
õõQ R
{
úú 	
switch
ùù 
(
ùù 
(
ùù 
AppointmentStatus
ùù &
)
ùù& '
dto
ùù' *
.
ùù* +
Status
ùù+ 1
)
ùù1 2
{
ûû 
case
üü 
AppointmentStatus
üü &
.
üü& '
	Confirmed
üü' 0
:
üü0 1
await
†† 
ConfirmAsync
†† &
(
††& '
id
††' )
)
††) *
;
††* +
break
°° 
;
°° 
case
££ 
AppointmentStatus
££ &
.
££& '
	Completed
££' 0
:
££0 1
await
§§ 
CompleteAsync
§§ '
(
§§' (
id
§§( *
)
§§* +
;
§§+ ,
break
•• 
;
•• 
case
ßß 
AppointmentStatus
ßß &
.
ßß& '
	Cancelled
ßß' 0
:
ßß0 1
await
®® 
CancelAsync
®® %
(
®®% &
id
©© 
,
©© 
new
™™ "
CancelAppointmentDto
™™ 0
{
´´  
CancellationReason
¨¨ .
=
¨¨/ 0
dto
≠≠  #
.
≠≠# $ 
CancellationReason
≠≠$ 6
??
≠≠7 9
$str
≠≠: E
}
ÆÆ 
)
ÆÆ 
;
ÆÆ 
break
ØØ 
;
ØØ 
default
±± 
:
±± 
throw
≤≤ 
new
≤≤ 
ArgumentException
≤≤ /
(
≤≤/ 0
$str
≥≥ 5
)
≥≥5 6
;
≥≥6 7
}
¥¥ 
}
µµ 	
public
∑∑ 
async
∑∑ 
Task
∑∑ 
ConfirmAsync
∑∑ &
(
∑∑& '
int
∑∑' *
id
∑∑+ -
)
∑∑- .
{
∏∏ 	
var
ππ 
appointment
ππ 
=
ππ 
await
∫∫ $
_appointmentRepository
∫∫ ,
.
∫∫, -
GetByIdAsync
∫∫- 9
(
∫∫9 :
id
∫∫: <
)
∫∫< =
;
∫∫= >
if
ºº 
(
ºº 
appointment
ºº 
==
ºº 
null
ºº #
)
ºº# $
throw
ΩΩ 
new
ΩΩ "
KeyNotFoundException
ΩΩ .
(
ΩΩ. /
)
ΩΩ/ 0
;
ΩΩ0 1
if
øø 
(
øø 
appointment
øø 
.
øø 
Status
øø "
!=
øø# %
AppointmentStatus
øø& 7
.
øø7 8
Pending
øø8 ?
)
øø? @
throw
¿¿ 
new
¿¿ '
InvalidOperationException
¿¿ 3
(
¿¿3 4
$str
¡¡ A
)
¡¡A B
;
¡¡B C
appointment
√√ 
.
√√ 
Status
√√ 
=
√√  
AppointmentStatus
√√! 2
.
√√2 3
	Confirmed
√√3 <
;
√√< =
await
≈≈ $
_appointmentRepository
≈≈ (
.
≈≈( )
UpdateAsync
≈≈) 4
(
≈≈4 5
appointment
≈≈5 @
)
≈≈@ A
;
≈≈A B
await
∆∆ $
_appointmentRepository
∆∆ (
.
∆∆( )
SaveChangesAsync
∆∆) 9
(
∆∆9 :
)
∆∆: ;
;
∆∆; <
}
«« 	
public
…… 
async
…… 
Task
…… 
CompleteAsync
…… '
(
……' (
int
……( +
id
……, .
)
……. /
{
   	
var
ÀÀ 
appointment
ÀÀ 
=
ÀÀ 
await
ÃÃ $
_appointmentRepository
ÃÃ ,
.
ÃÃ, -
GetByIdAsync
ÃÃ- 9
(
ÃÃ9 :
id
ÃÃ: <
)
ÃÃ< =
;
ÃÃ= >
if
ŒŒ 
(
ŒŒ 
appointment
ŒŒ 
==
ŒŒ 
null
ŒŒ #
)
ŒŒ# $
throw
œœ 
new
œœ "
KeyNotFoundException
œœ .
(
œœ. /
)
œœ/ 0
;
œœ0 1
if
—— 
(
—— 
appointment
—— 
.
—— 
Status
—— "
!=
——# %
AppointmentStatus
——& 7
.
——7 8
	Confirmed
——8 A
)
——A B
throw
““ 
new
““ '
InvalidOperationException
““ 3
(
““3 4
$str
”” C
)
””C D
;
””D E
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
‹‹ 
id
‹‹ 
,
‹‹ "
CancelAppointmentDto
››  
dto
››! $
)
››$ %
{
ﬁﬁ 	
var
ﬂﬂ 
appointment
ﬂﬂ 
=
ﬂﬂ 
await
‡‡ $
_appointmentRepository
‡‡ ,
.
‡‡, -
GetByIdAsync
‡‡- 9
(
‡‡9 :
id
‡‡: <
)
‡‡< =
;
‡‡= >
if
‚‚ 
(
‚‚ 
appointment
‚‚ 
==
‚‚ 
null
‚‚ #
)
‚‚# $
throw
„„ 
new
„„ "
KeyNotFoundException
„„ .
(
„„. /
)
„„/ 0
;
„„0 1
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
	Completed
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
ÁÁ A
)
ÁÁA B
;
ÁÁB C
if
ÈÈ 
(
ÈÈ 
appointment
ÈÈ 
.
ÈÈ 
Status
ÈÈ "
==
ÈÈ# %
AppointmentStatus
ÈÈ& 7
.
ÈÈ7 8
	Cancelled
ÈÈ8 A
)
ÈÈA B
throw
ÍÍ 
new
ÍÍ '
InvalidOperationException
ÍÍ 3
(
ÍÍ3 4
$str
ÎÎ 4
)
ÎÎ4 5
;
ÎÎ5 6
if
ÌÌ 
(
ÌÌ 
string
ÌÌ 
.
ÌÌ  
IsNullOrWhiteSpace
ÌÌ )
(
ÌÌ) *
dto
ÌÌ* -
.
ÌÌ- . 
CancellationReason
ÌÌ. @
)
ÌÌ@ A
)
ÌÌA B
throw
ÓÓ 
new
ÓÓ 
ArgumentException
ÓÓ +
(
ÓÓ+ ,
$str
ÔÔ 6
)
ÔÔ6 7
;
ÔÔ7 8
appointment
ÒÒ 
.
ÒÒ 
Status
ÒÒ 
=
ÒÒ  
AppointmentStatus
ÒÒ! 2
.
ÒÒ2 3
	Cancelled
ÒÒ3 <
;
ÒÒ< =
appointment
ÚÚ 
.
ÚÚ  
CancellationReason
ÚÚ *
=
ÚÚ+ ,
dto
ÛÛ 
.
ÛÛ  
CancellationReason
ÛÛ &
.
ÛÛ& '
Trim
ÛÛ' +
(
ÛÛ+ ,
)
ÛÛ, -
;
ÛÛ- .
await
ıı $
_appointmentRepository
ıı (
.
ıı( )
UpdateAsync
ıı) 4
(
ıı4 5
appointment
ıı5 @
)
ıı@ A
;
ııA B
await
ˆˆ $
_appointmentRepository
ˆˆ (
.
ˆˆ( )
SaveChangesAsync
ˆˆ) 9
(
ˆˆ9 :
)
ˆˆ: ;
;
ˆˆ; <
}
˜˜ 	
private
˘˘ 
async
˘˘ 
Task
˘˘ "
ValidateBookingAsync
˘˘ /
(
˘˘/ 0
int
˙˙ 
	patientId
˙˙ 
,
˙˙ 
int
˚˚ 
doctorId
˚˚ 
,
˚˚ 
DateOnly
¸¸ 
date
¸¸ 
,
¸¸ 
int
˝˝ 
timeSlot
˝˝ 
)
˝˝ 
{
˛˛ 	
var
ˇˇ 
patient
ˇˇ 
=
ˇˇ 
await
ÄÄ  
_patientRepository
ÄÄ (
.
ÄÄ( )
GetByIdAsync
ÄÄ) 5
(
ÄÄ5 6
	patientId
ÄÄ6 ?
)
ÄÄ? @
;
ÄÄ@ A
if
ÇÇ 
(
ÇÇ 
patient
ÇÇ 
==
ÇÇ 
null
ÇÇ 
)
ÇÇ  
throw
ÉÉ 
new
ÉÉ "
KeyNotFoundException
ÉÉ .
(
ÉÉ. /
$str
ÑÑ (
)
ÑÑ( )
;
ÑÑ) *
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
patient
ÜÜ 
.
ÜÜ 
IsActive
ÜÜ !
)
ÜÜ! "
throw
áá 
new
áá '
InvalidOperationException
áá 3
(
áá3 4
$str
àà A
)
ààA B
;
ààB C
var
ää 
doctor
ää 
=
ää 
await
ãã 
_doctorRepository
ãã '
.
ãã' (
GetByIdAsync
ãã( 4
(
ãã4 5
doctorId
ãã5 =
)
ãã= >
;
ãã> ?
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
èè '
)
èè' (
;
èè( )
if
ëë 
(
ëë 
!
ëë 
doctor
ëë 
.
ëë 
IsActive
ëë  
)
ëë  !
throw
íí 
new
íí '
InvalidOperationException
íí 3
(
íí3 4
$str
ìì &
)
ìì& '
;
ìì' (
if
ïï 
(
ïï 
date
ïï 
<
ïï 
DateOnly
ïï 
.
ïï  
FromDateTime
ïï  ,
(
ïï, -
DateTime
ïï- 5
.
ïï5 6
Today
ïï6 ;
)
ïï; <
)
ïï< =
throw
ññ 
new
ññ 
ArgumentException
ññ +
(
ññ+ ,
$str
óó =
)
óó= >
;
óó> ?
if
ôô 
(
ôô 
!
ôô 
Enum
ôô 
.
ôô 
	IsDefined
ôô 
(
ôô  
typeof
öö 
(
öö !
AppointmentTimeSlot
öö .
)
öö. /
,
öö/ 0
timeSlot
õõ 
)
õõ 
)
õõ 
{
úú 
throw
ùù 
new
ùù 
ArgumentException
ùù +
(
ùù+ ,
$str
ûû /
)
ûû/ 0
;
ûû0 1
}
üü 
if
°° 
(
°° 
await
°° $
_appointmentRepository
°° ,
.
¢¢ 6
(ExistsSamePatientSameDoctorSameDateAsync
¢¢ 9
(
¢¢9 :
	patientId
££ 
,
££ 
doctorId
§§ 
,
§§ 
date
•• 
)
•• 
)
•• 
{
¶¶ 
throw
ßß 
new
ßß '
InvalidOperationException
ßß 3
(
ßß3 4
$str
®® _
)
®®_ `
;
®®` a
}
©© 
if
´´ 
(
´´ 
await
´´ $
_appointmentRepository
´´ ,
.
¨¨ 4
&ExistsSamePatientSameSlotSameDateAsync
¨¨ 7
(
¨¨7 8
	patientId
≠≠ 
,
≠≠ 
date
ÆÆ 
,
ÆÆ 
timeSlot
ØØ 
)
ØØ 
)
ØØ 
{
∞∞ 
throw
±± 
new
±± '
InvalidOperationException
±± 3
(
±±3 4
$str
≤≤ P
)
≤≤P Q
;
≤≤Q R
}
≥≥ 
if
µµ 
(
µµ 
await
µµ $
_appointmentRepository
µµ ,
.
∂∂ 3
%ExistsSameDoctorSameSlotSameDateAsync
∂∂ 6
(
∂∂6 7
doctorId
∑∑ 
,
∑∑ 
date
∏∏ 
,
∏∏ 
timeSlot
ππ 
)
ππ 
)
ππ 
{
∫∫ 
throw
ªª 
new
ªª '
InvalidOperationException
ªª 3
(
ªª3 4
$str
ºº B
)
ººB C
;
ººC D
}
ΩΩ 
}
ææ 	
private
¿¿ 
async
¿¿ 
Task
¿¿ (
ValidateUpdateBookingAsync
¿¿ 5
(
¿¿5 6
int
¿¿6 9
appointmentId
¿¿: G
,
¿¿G H
int
¿¿H K
	patientId
¿¿L U
,
¿¿U V
int
¿¿V Y
doctorId
¿¿Z b
,
¿¿b c
DateOnly
¿¿c k
date
¿¿l p
,
¿¿p q
int
¿¿q t
timeSlot
¿¿u }
)
¿¿} ~
{
¡¡ 	
var
¬¬ 
patient
¬¬ 
=
¬¬ 
await
√√  
_patientRepository
√√ (
.
√√( )
GetByIdAsync
√√) 5
(
√√5 6
	patientId
√√6 ?
)
√√? @
;
√√@ A
if
≈≈ 
(
≈≈ 
patient
≈≈ 
==
≈≈ 
null
≈≈ 
)
≈≈  
throw
∆∆ 
new
∆∆ "
KeyNotFoundException
∆∆ .
(
∆∆. /
$str
«« (
)
««( )
;
««) *
if
…… 
(
…… 
!
…… 
patient
…… 
.
…… 
IsActive
…… !
)
……! "
throw
   
new
   '
InvalidOperationException
   3
(
  3 4
$str
ÀÀ A
)
ÀÀA B
;
ÀÀB C
var
ÕÕ 
doctor
ÕÕ 
=
ÕÕ 
await
ŒŒ 
_doctorRepository
ŒŒ '
.
ŒŒ' (
GetByIdAsync
ŒŒ( 4
(
ŒŒ4 5
doctorId
ŒŒ5 =
)
ŒŒ= >
;
ŒŒ> ?
if
–– 
(
–– 
doctor
–– 
==
–– 
null
–– 
)
–– 
throw
—— 
new
—— "
KeyNotFoundException
—— .
(
——. /
$str
““ '
)
““' (
;
““( )
if
‘‘ 
(
‘‘ 
!
‘‘ 
doctor
‘‘ 
.
‘‘ 
IsActive
‘‘  
)
‘‘  !
throw
’’ 
new
’’ '
InvalidOperationException
’’ 3
(
’’3 4
$str
÷÷ &
)
÷÷& '
;
÷÷' (
if
ÿÿ 
(
ÿÿ 
date
ÿÿ 
<
ÿÿ 
DateOnly
ÿÿ 
.
ÿÿ  
FromDateTime
ÿÿ  ,
(
ÿÿ, -
DateTime
ÿÿ- 5
.
ÿÿ5 6
Today
ÿÿ6 ;
)
ÿÿ; <
)
ÿÿ< =
throw
ŸŸ 
new
ŸŸ 
ArgumentException
ŸŸ +
(
ŸŸ+ ,
$str
⁄⁄ =
)
⁄⁄= >
;
⁄⁄> ?
if
‹‹ 
(
‹‹ 
!
‹‹ 
Enum
‹‹ 
.
‹‹ 
	IsDefined
‹‹ 
(
‹‹  
typeof
›› 
(
›› !
AppointmentTimeSlot
›› .
)
››. /
,
››/ 0
timeSlot
ﬁﬁ 
)
ﬁﬁ 
)
ﬁﬁ 
{
ﬂﬂ 
throw
‡‡ 
new
‡‡ 
ArgumentException
‡‡ +
(
‡‡+ ,
$str
·· /
)
··/ 0
;
··0 1
}
‚‚ 
if
‰‰ 
(
‰‰ 
await
‰‰ $
_appointmentRepository
‰‰ ,
.
ÂÂ 6
(ExistsSamePatientSameDoctorSameDateAsync
ÂÂ 9
(
ÂÂ9 :
	patientId
ÊÊ 
,
ÊÊ 
doctorId
ÁÁ 
,
ÁÁ 
date
ËË 
,
ËË 
appointmentId
ÈÈ !
)
ÈÈ! "
)
ÈÈ" #
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ '
InvalidOperationException
ÎÎ 3
(
ÎÎ3 4
$str
ÏÏ _
)
ÏÏ_ `
;
ÏÏ` a
}
ÌÌ 
if
ÔÔ 
(
ÔÔ 
await
ÔÔ $
_appointmentRepository
ÔÔ ,
.
 4
&ExistsSamePatientSameSlotSameDateAsync
 7
(
7 8
	patientId
ÒÒ 
,
ÒÒ 
date
ÚÚ 
,
ÚÚ 
timeSlot
ÛÛ 
,
ÛÛ 
appointmentId
ÙÙ !
)
ÙÙ! "
)
ÙÙ" #
{
ıı 
throw
ˆˆ 
new
ˆˆ '
InvalidOperationException
ˆˆ 3
(
ˆˆ3 4
$str
˜˜ P
)
˜˜P Q
;
˜˜Q R
}
¯¯ 
if
˙˙ 
(
˙˙ 
await
˙˙ $
_appointmentRepository
˙˙ ,
.
˚˚ 3
%ExistsSameDoctorSameSlotSameDateAsync
˚˚ 6
(
˚˚6 7
doctorId
¸¸ 
,
¸¸ 
date
˝˝ 
,
˝˝ 
timeSlot
˛˛ 
,
˛˛ 
appointmentId
ˇˇ !
)
ˇˇ! "
)
ˇˇ" #
{
ÄÄ 
throw
ÅÅ 
new
ÅÅ '
InvalidOperationException
ÅÅ 3
(
ÅÅ3 4
$str
ÇÇ B
)
ÇÇB C
;
ÇÇC D
}
ÉÉ 
}
ÑÑ 	
private
ÜÜ 
static
ÜÜ 
AppointmentDto
ÜÜ %!
MapToAppointmentDto
ÜÜ& 9
(
ÜÜ9 :
Appointment
áá 
appointment
áá #
)
áá# $
{
àà 	
return
ââ 
new
ââ 
AppointmentDto
ââ %
{
ää 
AppointmentId
ãã 
=
ãã 
appointment
ãã  +
.
ãã+ ,
AppointmentId
ãã, 9
,
ãã9 :
	PatientId
åå 
=
åå 
appointment
åå '
.
åå' (
	PatientId
åå( 1
,
åå1 2
DoctorId
çç 
=
çç 
appointment
çç &
.
çç& '
DoctorId
çç' /
,
çç/ 0
ScheduledDate
éé 
=
éé 
appointment
éé  +
.
éé+ ,
ScheduledDate
éé, 9
,
éé9 :
TimeSlot
èè 
=
èè 
(
èè 
int
èè 
)
èè  
appointment
èè  +
.
èè+ ,
TimeSlot
èè, 4
,
èè4 5
Status
êê 
=
êê 
(
êê 
int
êê 
)
êê 
appointment
êê )
.
êê) *
Status
êê* 0
,
êê0 1 
CancellationReason
ëë "
=
ëë# $
appointment
ëë% 0
.
ëë0 1 
CancellationReason
ëë1 C
}
íí 
;
íí 
}
ìì 	
private
ïï 
static
ïï #
DoctorScheduleItemDto
ïï ,#
MapDoctorScheduleItem
ïï- B
(
ïïB C
Appointment
ññ 
appointment
ññ #
)
ññ# $
{
óó 	
return
òò 
new
òò #
DoctorScheduleItemDto
òò ,
{
ôô 
AppointmentId
öö 
=
öö 
appointment
öö  +
.
öö+ ,
AppointmentId
öö, 9
,
öö9 :
ScheduledDate
õõ 
=
õõ 
appointment
õõ  +
.
õõ+ ,
ScheduledDate
õõ, 9
,
õõ9 :
TimeSlot
úú 
=
úú 
(
úú 
int
úú 
)
úú  
appointment
úú  +
.
úú+ ,
TimeSlot
úú, 4
,
úú4 5
	PatientId
ùù 
=
ùù 
appointment
ùù '
.
ùù' (
	PatientId
ùù( 1
,
ùù1 2
PatientName
ûû 
=
ûû 
appointment
ûû )
.
ûû) *
Patient
ûû* 1
.
ûû1 2
FullName
ûû2 :
,
ûû: ;
Status
üü 
=
üü 
(
üü 
int
üü 
)
üü 
appointment
üü )
.
üü) *
Status
üü* 0
}
†† 
;
†† 
}
°° 	
}
¢¢ 
}££ ¢
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
} ò$
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
}úú ÔG
FC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
. 
AddJsonOptions 
( 
options 
=> 
{ 
options 
. !
JsonSerializerOptions %
.% & 
PropertyNamingPolicy& :
=; <
JsonNamingPolicy 
. 
	CamelCase &
;& '
} 
) 
; 
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
options &
=>' )
{ 
options 
. 

SwaggerDoc 
( 
$str   
,   
new!! 
OpenApiInfo!! 
{"" 	
Title## 
=## 
$str## $
,##$ %
Version$$ 
=$$ 
$str$$ 
,$$ 
Description%% 
=%% 
$str%% A
}&& 	
)&&	 

;&&
 
options'' 
.'' !
AddSecurityDefinition'' !
(''! "
$str(( 
,(( 
new)) !
OpenApiSecurityScheme)) !
{** 	
Name++ 
=++ 
$str++ "
,++" #
In,, 
=,, 
ParameterLocation,, "
.,," #
Header,,# )
,,,) *
Type-- 
=-- 
SecuritySchemeType-- %
.--% &
Http--& *
,--* +
Scheme.. 
=.. 
$str.. 
,.. 
BearerFormat// 
=// 
$str//  
,//  !
Description00 
=00 
$str00 W
}11 	
)11	 

;11
 
options22 
.22 "
AddSecurityRequirement22 "
(22" #
document22# +
=>22, .
new33 &
OpenApiSecurityRequirement33 &
{44 	
[55 
new55 *
OpenApiSecuritySchemeReference55 /
(55/ 0
$str66 
,66 
document77 
)77 
]77 
=77 
[77 
]77 
}88 	
)88	 

;88
 
}99 
)99 
;99 
builder@@ 
.@@ 
Services@@ 
.@@ 
AddDbContext@@ 
<@@ 
HealthAxisDbContext@@ 1
>@@1 2
(@@2 3
options@@3 :
=>@@; =
{AA 
optionsBB 
.BB 
UseSqlServerBB 
(BB 
builderCC 
.CC 
ConfigurationCC 
.CC 
GetConnectionStringCC 1
(CC1 2
$strCC2 E
)CCE F
)CCF G
;CCG H
}DD 
)DD 
;DD 
builderJJ 
.JJ 
ServicesJJ 
.JJ 
AddAuthenticationJJ "
(JJ" #
JwtBearerDefaultsKK 
.KK  
AuthenticationSchemeKK *
)KK* +
.LL 
AddJwtBearerLL 
(LL 
optionsLL 
=>LL 
{MM 
varNN 
jwtNN 
=NN 
builderNN 
.NN 
ConfigurationNN '
.NN' (

GetSectionNN( 2
(NN2 3
$strNN3 8
)NN8 9
;NN9 :
optionsPP 
.PP %
TokenValidationParametersPP )
=PP* +
newQQ %
TokenValidationParametersQQ )
{RR 
ValidateIssuerSS 
=SS  
trueSS! %
,SS% &
ValidIssuerTT 
=TT 
jwtTT !
[TT! "
$strTT" *
]TT* +
,TT+ ,
ValidateAudienceVV  
=VV! "
trueVV# '
,VV' (
ValidAudienceWW 
=WW 
jwtWW  #
[WW# $
$strWW$ .
]WW. /
,WW/ 0
ValidateLifetimeYY  
=YY! "
trueYY# '
,YY' ($
ValidateIssuerSigningKey[[ (
=[[) *
true[[+ /
,[[/ 0
IssuerSigningKey]]  
=]]! "
new^^  
SymmetricSecurityKey^^ ,
(^^, -
Encoding__  
.__  !
UTF8__! %
.__% &
GetBytes__& .
(__. /
jwt__/ 2
[__2 3
$str__3 8
]__8 9
!__9 :
)__: ;
)__; <
,__< =
	ClockSkewaa 
=aa 
TimeSpanaa $
.aa$ %
Zeroaa% )
}bb 
;bb 
}cc 
)cc 
;cc 
builderee 
.ee 
Servicesee 
.ee 
AddAuthorizationee !
(ee! "
)ee" #
;ee# $
builderkk 
.kk 
Serviceskk 
.kk 
	AddScopedkk 
<kk 
IPatientRepositorykk -
,kk- .
PatientRepositorykk/ @
>kk@ A
(kkA B
)kkB C
;kkC D
builderll 
.ll 
Servicesll 
.ll 
	AddScopedll 
<ll 
IDoctorRepositoryll ,
,ll, -
DoctorRepositoryll. >
>ll> ?
(ll? @
)ll@ A
;llA B
buildermm 
.mm 
Servicesmm 
.mm 
	AddScopedmm 
<mm "
IAppointmentRepositorymm 1
,mm1 2!
AppointmentRepositorymm3 H
>mmH I
(mmI J
)mmJ K
;mmK L
buildernn 
.nn 
Servicesnn 
.nn 
	AddScopednn 
<nn #
IHealthRecordRepositorynn 2
,nn2 3"
HealthRecordRepositorynn4 J
>nnJ K
(nnK L
)nnL M
;nnM N
builderoo 
.oo 
Servicesoo 
.oo 
	AddScopedoo 
<oo 
IUserRepositoryoo *
,oo* +
UserRepositoryoo, :
>oo: ;
(oo; <
)oo< =
;oo= >
builderuu 
.uu 
Servicesuu 
.uu 
	AddScopeduu 
<uu 
IPatientServiceuu *
,uu* +
PatientServiceuu, :
>uu: ;
(uu; <
)uu< =
;uu= >
buildervv 
.vv 
Servicesvv 
.vv 
	AddScopedvv 
<vv 
IDoctorServicevv )
,vv) *
DoctorServicevv+ 8
>vv8 9
(vv9 :
)vv: ;
;vv; <
builderww 
.ww 
Servicesww 
.ww 
	AddScopedww 
<ww 
IAppointmentServiceww .
,ww. /
AppointmentServiceww0 B
>wwB C
(wwC D
)wwD E
;wwE F
builderxx 
.xx 
Servicesxx 
.xx 
	AddScopedxx 
<xx  
IHealthRecordServicexx /
,xx/ 0
HealthRecordServicexx1 D
>xxD E
(xxE F
)xxF G
;xxG H
builderyy 
.yy 
Servicesyy 
.yy 
	AddScopedyy 
<yy 
IAuthServiceyy '
,yy' (
AuthServiceyy) 4
>yy4 5
(yy5 6
)yy6 7
;yy7 8
var}} 
app}} 
=}} 	
builder}}
 
.}} 
Build}} 
(}} 
)}} 
;}} 
ifÅÅ 
(
ÅÅ 
app
ÅÅ 
.
ÅÅ 
Environment
ÅÅ 
.
ÅÅ 
IsDevelopment
ÅÅ !
(
ÅÅ! "
)
ÅÅ" #
)
ÅÅ# $
{ÇÇ 
app
ÉÉ 
.
ÉÉ 

UseSwagger
ÉÉ 
(
ÉÉ 
)
ÉÉ 
;
ÉÉ 
app
ÖÖ 
.
ÖÖ 
UseSwaggerUI
ÖÖ 
(
ÖÖ 
options
ÖÖ 
=>
ÖÖ 
{
ÜÜ 
options
áá 
.
áá 
SwaggerEndpoint
áá 
(
áá  
$str
àà &
,
àà& '
$str
ââ 
)
ââ  
;
ââ  !
options
ãã 
.
ãã 
RoutePrefix
ãã 
=
ãã 
string
ãã $
.
ãã$ %
Empty
ãã% *
;
ãã* +
}
åå 
)
åå 
;
åå 
}çç 
appèè 
.
èè !
UseHttpsRedirection
èè 
(
èè 
)
èè 
;
èè 
appëë 
.
ëë 
UseAuthentication
ëë 
(
ëë 
)
ëë 
;
ëë 
appìì 
.
ìì 
UseAuthorization
ìì 
(
ìì 
)
ìì 
;
ìì 
appïï 
.
ïï 
MapControllers
ïï 
(
ïï 
)
ïï 
;
ïï 
appôô 
.
ôô 
Run
ôô 
(
ôô 
)
ôô 	
;
ôô	 
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
}&& Ù
LC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Models\Doctor.cs
	namespace 	
S3_HealthAxisApi
 
. 
Models !
{ 
public 

class 
Doctor 
{ 
[ 	
Key	 
] 
public		 
int		 
DoctorId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ;
); <
]< =
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* U
)U V
]V W
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
$str! >
)> ?
]? @
public  
DoctorSpecialisation #
Specialisation$ 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
[ 	
Required	 
( 
ErrorMessage 
=  
$str! C
)C D
]D E
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% Q
)Q R
]R S
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
( 
ErrorMessage 
=  
$str! @
)@ A
]A B
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage )
=* +
$str, Y
)Y Z
]Z [
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
true- 1
;1 2
public 
ICollection 
< 
Appointment &
>& '
Appointments( 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
=C D
newE H
ListI M
<M N
AppointmentN Y
>Y Z
(Z [
)[ \
;\ ]
public 
ICollection 
< 
HealthRecord '
>' (
HealthRecords) 6
{7 8
get9 <
;< =
set> A
;A B
}C D
=E F
newG J
ListK O
<O P
HealthRecordP \
>\ ]
(] ^
)^ _
;_ `
}   
}!! …
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
}$$ ﬁ≈
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
} ’
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
 À

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
public 
int 
Specialisation !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public		 
decimal		 
ConsultationFee		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		3 4
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
} §
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
} ø

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
} ‰
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
} ø
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
} ö
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
} ä

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
} ·]
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
}ii «I
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
IDoctorService! /
doctorService0 =
)= >
{ 	
_doctorService 
= 
doctorService *
;* +
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
[ 
	FromQuery 
] 
string 
? 
sortBy  &
,& '
[ 
	FromQuery 
] 
int 
? 
specialisation +
)+ ,
{ 	
var 
doctors 
= 
await 
_doctorService  .
. 
GetAllAsync 
( 
sortBy #
,# $
specialisation% 3
)3 4
;4 5
return 
Ok 
( 
doctors 
) 
; 
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
var"" 
doctor"" 
="" 
await"" 
_doctorService"" -
.""- .
GetByIdAsync"". :
("": ;
id""; =
)""= >
;""> ?
if$$ 
($$ 
doctor$$ 
==$$ 
null$$ 
)$$ 
{%% 
return&& 
NotFound&& 
(&&  
$"'' 
$str'' %
{''% &
id''& (
}''( )
$str'') 4
"''4 5
)''5 6
;''6 7
}(( 
return** 
Ok** 
(** 
doctor** 
)** 
;** 
}++ 	
[-- 	
HttpGet--	 
(-- 
$str-- 6
)--6 7
]--7 8
public.. 
async.. 
Task.. 
<.. 
IActionResult.. '
>..' (
GetBySpecialisation..) <
(..< =
int// 
specialisation// 
)// 
{00 	
try11 
{22 
var33 
doctors33 
=33 
await44 
_doctorService44 (
.55 *
GetActiveBySpecialisationAsync55 7
(557 8
specialisation66 *
)66* +
;66+ ,
return88 
Ok88 
(88 
doctors88 !
)88! "
;88" #
}99 
catch:: 
(:: 
ArgumentException:: $
ex::% '
)::' (
{;; 
return<< 

BadRequest<< !
(<<! "
ex<<" $
.<<$ %
Message<<% ,
)<<, -
;<<- .
}== 
}>> 	
[@@ 	
HttpPost@@	 
]@@ 
[AA 	
	AuthorizeAA	 
(AA 
RolesAA 
=AA 
$strAA "
)AA" #
]AA# $
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
CreateBB) /
(BB/ 0
CreateDoctorDtoCC 
dtoCC 
)CC  
{DD 	
tryEE 
{FF 
varGG 
doctorGG 
=GG 
awaitHH 
_doctorServiceHH (
.HH( )
CreateAsyncHH) 4
(HH4 5
dtoHH5 8
)HH8 9
;HH9 :
returnJJ 
CreatedAtActionJJ &
(JJ& '
nameofKK 
(KK 
GetByIdKK "
)KK" #
,KK# $
newLL 
{LL 
idLL 
=LL 
doctorLL %
.LL% &
DoctorIdLL& .
}LL/ 0
,LL0 1
doctorMM 
)MM 
;MM 
}NN 
catchOO 
(OO 
ArgumentExceptionOO $
exOO% '
)OO' (
{PP 
returnQQ 

BadRequestQQ !
(QQ! "
exQQ" $
.QQ$ %
MessageQQ% ,
)QQ, -
;QQ- .
}RR 
}SS 	
[UU 	
HttpPutUU	 
(UU 
$strUU 
)UU 
]UU 
[VV 	
	AuthorizeVV	 
(VV 
RolesVV 
=VV 
$strVV "
)VV" #
]VV# $
publicWW 
asyncWW 
TaskWW 
<WW 
IActionResultWW '
>WW' (
UpdateWW) /
(WW/ 0
intXX 
idXX 
,XX 
UpdateDoctorDtoYY 
dtoYY 
)YY  
{ZZ 	
try[[ 
{\\ 
await]] 
_doctorService]] $
.]]$ %
UpdateAsync]]% 0
(]]0 1
id]]1 3
,]]3 4
dto]]5 8
)]]8 9
;]]9 :
return__ 
	NoContent__  
(__  !
)__! "
;__" #
}`` 
catchaa 
(aa  
KeyNotFoundExceptionaa '
exaa( *
)aa* +
{bb 
returncc 
NotFoundcc 
(cc  
excc  "
.cc" #
Messagecc# *
)cc* +
;cc+ ,
}dd 
catchee 
(ee 
ArgumentExceptionee $
exee% '
)ee' (
{ff 
returngg 

BadRequestgg !
(gg! "
exgg" $
.gg$ %
Messagegg% ,
)gg, -
;gg- .
}hh 
}ii 	
[kk 	
HttpGetkk	 
(kk 
$strkk $
)kk$ %
]kk% &
[ll 	
	Authorizell	 
]ll 
publicmm 
asyncmm 
Taskmm 
<mm 
IActionResultmm '
>mm' (
GetAvailabilitymm) 8
(mm8 9
intmm9 <
idmm= ?
,mm? @
[mmA B
	FromQuerymmB K
]mmK L
DateOnlymmM U
datemmV Z
)mmZ [
{nn 	
varoo 
slotsoo 
=oo 
awaitpp 
_doctorServicepp $
.pp$ % 
GetAvailabilityAsyncpp% 9
(pp9 :
idqq 
,qq 
daterr 
)rr 
;rr 
returntt 
Oktt 
(tt 
slotstt 
)tt 
;tt 
}uu 	
[ww 	
HttpPutww	 
(ww 
$strww $
)ww$ %
]ww% &
[xx 	
	Authorizexx	 
(xx 
Rolesxx 
=xx 
$strxx "
)xx" #
]xx# $
publicyy 
asyncyy 
Taskyy 
<yy 
IActionResultyy '
>yy' (
Activateyy) 1
(yy1 2
intzz 
idzz 
)zz 
{{{ 	
try|| 
{}} 
await~~ 
_doctorService~~ $
.~~$ %
ActivateAsync~~% 2
(~~2 3
id~~3 5
)~~5 6
;~~6 7
return
ÄÄ 
	NoContent
ÄÄ  
(
ÄÄ  !
)
ÄÄ! "
;
ÄÄ" #
}
ÅÅ 
catch
ÇÇ 
(
ÇÇ "
KeyNotFoundException
ÇÇ '
ex
ÇÇ( *
)
ÇÇ* +
{
ÉÉ 
return
ÑÑ 
NotFound
ÑÑ 
(
ÑÑ  
ex
ÑÑ  "
.
ÑÑ" #
Message
ÑÑ# *
)
ÑÑ* +
;
ÑÑ+ ,
}
ÖÖ 
}
ÜÜ 	
[
àà 	
HttpPut
àà	 
(
àà 
$str
àà &
)
àà& '
]
àà' (
[
ââ 	
	Authorize
ââ	 
(
ââ 
Roles
ââ 
=
ââ 
$str
ââ "
)
ââ" #
]
ââ# $
public
ää 
async
ää 
Task
ää 
<
ää 
IActionResult
ää '
>
ää' (

Deactivate
ää) 3
(
ää3 4
int
ãã 
id
ãã 
)
ãã 
{
åå 	
try
çç 
{
éé 
await
èè 
_doctorService
èè $
.
èè$ %
DeactivateAsync
èè% 4
(
èè4 5
id
èè5 7
)
èè7 8
;
èè8 9
return
ëë 
	NoContent
ëë  
(
ëë  !
)
ëë! "
;
ëë" #
}
íí 
catch
ìì 
(
ìì "
KeyNotFoundException
ìì '
ex
ìì( *
)
ìì* +
{
îî 
return
ïï 
NotFound
ïï 
(
ïï  
ex
ïï  "
.
ïï" #
Message
ïï# *
)
ïï* +
;
ïï+ ,
}
ññ 
}
óó 	
}
òò 
}ôô ◊
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
$str 
) 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (
Login  ) .
(  . /
LoginDto  / 7
dto  8 ;
)  ; <
{!! 	
var"" 
result"" 
="" 
await"" 
_authService"" +
.""+ ,

LoginAsync"", 6
(""6 7
dto""7 :
)"": ;
;""; <
if$$ 
($$ 
!$$ 
result$$ 
.$$ 
Success$$ 
)$$  
{%% 
return&& 
Unauthorized&& #
(&&# $
result&&$ *
.&&* +
Message&&+ 2
)&&2 3
;&&3 4
}'' 
return)) 
Ok)) 
()) 
result)) 
.)) 
Data)) !
)))! "
;))" #
}** 	
},, 
}-- ﬁo
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
[TT 	
HttpPostTT	 
]TT 
[UU 	
	AuthorizeUU	 
(UU 
RolesUU 
=UU 
$strUU *
)UU* +
]UU+ ,
publicVV 
asyncVV 
TaskVV 
<VV 
IActionResultVV '
>VV' (
CreateVV) /
(VV/ 0
[WW 
FromBodyWW 
]WW  
CreateAppointmentDtoWW +
dtoWW, /
)WW/ 0
{XX 	
tryYY 
{ZZ 
var[[ 
appointment[[ 
=[[  !
await\\ 
_appointmentService\\ -
.\\- .
CreateAsync\\. 9
(\\9 :
dto\\: =
)\\= >
;\\> ?
return^^ 
CreatedAtAction^^ &
(^^& '
nameof__ 
(__ 
GetById__ "
)__" #
,__# $
new`` 
{`` 
id`` 
=`` 
appointment`` *
.``* +
AppointmentId``+ 8
}``9 :
,``: ;
appointmentaa 
)aa  
;aa  !
}bb 
catchcc 
(cc 
ArgumentExceptiondd !
exdd" $
)dd$ %
{ee 
returnff 

BadRequestff !
(ff! "
exff" $
.ff$ %
Messageff% ,
)ff, -
;ff- .
}gg 
catchhh 
(hh %
InvalidOperationExceptionii )
exii* ,
)ii, -
{jj 
returnkk 

BadRequestkk !
(kk! "
exkk" $
.kk$ %
Messagekk% ,
)kk, -
;kk- .
}ll 
catchmm 
(mm  
KeyNotFoundExceptionnn $
exnn% '
)nn' (
{oo 
returnpp 
NotFoundpp 
(pp  
expp  "
.pp" #
Messagepp# *
)pp* +
;pp+ ,
}qq 
}rr 	
[tt 	
HttpPuttt	 
(tt 
$strtt 
)tt 
]tt 
[uu 	
	Authorizeuu	 
(uu 
Rolesuu 
=uu 
$struu *
)uu* +
]uu+ ,
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
[xx 
FromBodyxx 
]xx  
UpdateAppointmentDtoxx +
dtoxx, /
)xx/ 0
{yy 	
tryzz 
{{{ 
await|| 
_appointmentService|| )
.}} 
UpdateAsync}}  
(}}  !
id}}! #
,}}# $
dto}}% (
)}}( )
;}}) *
return 
	NoContent  
(  !
)! "
;" #
}
ÄÄ 
catch
ÅÅ 
(
ÅÅ "
KeyNotFoundException
ÇÇ $
ex
ÇÇ% '
)
ÇÇ' (
{
ÉÉ 
return
ÑÑ 
NotFound
ÑÑ 
(
ÑÑ  
ex
ÑÑ  "
.
ÑÑ" #
Message
ÑÑ# *
)
ÑÑ* +
;
ÑÑ+ ,
}
ÖÖ 
catch
ÜÜ 
(
ÜÜ 
ArgumentException
áá !
ex
áá" $
)
áá$ %
{
àà 
return
ââ 

BadRequest
ââ !
(
ââ! "
ex
ââ" $
.
ââ$ %
Message
ââ% ,
)
ââ, -
;
ââ- .
}
ää 
catch
ãã 
(
ãã '
InvalidOperationException
åå )
ex
åå* ,
)
åå, -
{
çç 
return
éé 

BadRequest
éé !
(
éé! "
ex
éé" $
.
éé$ %
Message
éé% ,
)
éé, -
;
éé- .
}
èè 
}
êê 	
[
íí 	
HttpPut
íí	 
(
íí 
$str
íí #
)
íí# $
]
íí$ %
[
ìì 	
	Authorize
ìì	 
(
ìì 
Roles
ìì 
=
ìì 
$str
ìì )
)
ìì) *
]
ìì* +
public
îî 
async
îî 
Task
îî 
<
îî 
IActionResult
îî '
>
îî' (
Confirm
îî) 0
(
îî0 1
int
îî1 4
id
îî5 7
)
îî7 8
{
ïï 	
try
ññ 
{
óó 
await
òò !
_appointmentService
òò )
.
ôô 
ConfirmAsync
ôô !
(
ôô! "
id
ôô" $
)
ôô$ %
;
ôô% &
return
õõ 
	NoContent
õõ  
(
õõ  !
)
õõ! "
;
õõ" #
}
úú 
catch
ùù 
(
ùù "
KeyNotFoundException
ûû $
)
ûû$ %
{
üü 
return
†† 
NotFound
†† 
(
††  
)
††  !
;
††! "
}
°° 
catch
¢¢ 
(
¢¢ '
InvalidOperationException
££ )
ex
££* ,
)
££, -
{
§§ 
return
•• 

BadRequest
•• !
(
••! "
ex
••" $
.
••$ %
Message
••% ,
)
••, -
;
••- .
}
¶¶ 
}
ßß 	
[
©© 	
HttpPut
©©	 
(
©© 
$str
©© $
)
©©$ %
]
©©% &
[
™™ 	
	Authorize
™™	 
(
™™ 
Roles
™™ 
=
™™ 
$str
™™ )
)
™™) *
]
™™* +
public
´´ 
async
´´ 
Task
´´ 
<
´´ 
IActionResult
´´ '
>
´´' (
Complete
´´) 1
(
´´1 2
int
´´2 5
id
´´6 8
)
´´8 9
{
¨¨ 	
try
≠≠ 
{
ÆÆ 
await
ØØ !
_appointmentService
ØØ )
.
∞∞ 
CompleteAsync
∞∞ "
(
∞∞" #
id
∞∞# %
)
∞∞% &
;
∞∞& '
return
≤≤ 
	NoContent
≤≤  
(
≤≤  !
)
≤≤! "
;
≤≤" #
}
≥≥ 
catch
¥¥ 
(
¥¥ "
KeyNotFoundException
µµ $
)
µµ$ %
{
∂∂ 
return
∑∑ 
NotFound
∑∑ 
(
∑∑  
)
∑∑  !
;
∑∑! "
}
∏∏ 
catch
ππ 
(
ππ '
InvalidOperationException
∫∫ )
ex
∫∫* ,
)
∫∫, -
{
ªª 
return
ºº 

BadRequest
ºº !
(
ºº! "
ex
ºº" $
.
ºº$ %
Message
ºº% ,
)
ºº, -
;
ºº- .
}
ΩΩ 
}
ææ 	
[
¿¿ 	
HttpPut
¿¿	 
(
¿¿ 
$str
¿¿ 
)
¿¿ 
]
¿¿  
[
¡¡ 	
	Authorize
¡¡	 
]
¡¡ 
public
¬¬ 
async
¬¬ 
Task
¬¬ 
<
¬¬ 
IActionResult
¬¬ '
>
¬¬' (
UpdateStatus
¬¬) 5
(
¬¬5 6
int
¬¬6 9
id
¬¬: <
,
¬¬< =(
UpdateAppointmentStatusDto
¬¬> X
dto
¬¬Y \
)
¬¬\ ]
{
√√ 	
await
ƒƒ !
_appointmentService
ƒƒ %
.
≈≈ 
UpdateStatusAsync
≈≈ "
(
≈≈" #
id
≈≈# %
,
≈≈% &
dto
≈≈' *
)
≈≈* +
;
≈≈+ ,
return
«« 
	NoContent
«« 
(
«« 
)
«« 
;
«« 
}
»» 	
[
   	
HttpPut
  	 
(
   
$str
   "
)
  " #
]
  # $
[
ÀÀ 	
	Authorize
ÀÀ	 
(
ÀÀ 
Roles
ÀÀ 
=
ÀÀ 
$str
ÀÀ *
)
ÀÀ* +
]
ÀÀ+ ,
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
ÃÃ' (
Cancel
ÃÃ) /
(
ÃÃ/ 0
int
ÕÕ 
id
ÕÕ 
,
ÕÕ 
[
ŒŒ 
FromBody
ŒŒ 
]
ŒŒ "
CancelAppointmentDto
ŒŒ +
dto
ŒŒ, /
)
ŒŒ/ 0
{
œœ 	
try
–– 
{
—— 
await
““ !
_appointmentService
““ )
.
”” 
CancelAsync
””  
(
””  !
id
””! #
,
””# $
dto
””% (
)
””( )
;
””) *
return
’’ 
	NoContent
’’  
(
’’  !
)
’’! "
;
’’" #
}
÷÷ 
catch
◊◊ 
(
◊◊ "
KeyNotFoundException
ÿÿ $
)
ÿÿ$ %
{
ŸŸ 
return
⁄⁄ 
NotFound
⁄⁄ 
(
⁄⁄  
)
⁄⁄  !
;
⁄⁄! "
}
€€ 
catch
‹‹ 
(
‹‹ 
ArgumentException
›› !
ex
››" $
)
››$ %
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
‡‡ 
catch
·· 
(
·· '
InvalidOperationException
‚‚ )
ex
‚‚* ,
)
‚‚, -
{
„„ 
return
‰‰ 

BadRequest
‰‰ !
(
‰‰! "
ex
‰‰" $
.
‰‰$ %
Message
‰‰% ,
)
‰‰, -
;
‰‰- .
}
ÂÂ 
}
ÊÊ 	
}
ÁÁ 
}ËË 