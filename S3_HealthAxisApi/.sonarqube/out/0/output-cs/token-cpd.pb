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
} –
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
Task 
ActivateAsync 
( 
int 
id !
)! "
;" #
Task 
DeactivateAsync 
( 
int  
id! #
)# $
;$ %
} 
} π
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
Task 
ConfirmAsync 
( 
int 
id  
)  !
;! "
Task 
CompleteAsync 
( 
int 
id !
)! "
;" #
Task 
CancelAsync 
( 
int 
id 
,   
CancelAppointmentDto! 5
dto6 9
)9 :
;: ;
} 
} Ær
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
}∫∫ ÿh
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
TaskQQ 
ActivateAsyncQQ '
(QQ' (
intQQ( +
idQQ, .
)QQ. /
{RR 	
varSS 
doctorSS 
=SS 
awaitSS 
_doctorRepositorySS 0
.SS0 1
GetByIdAsyncSS1 =
(SS= >
idSS> @
)SS@ A
;SSA B
ifUU 
(UU 
doctorUU 
==UU 
nullUU 
)UU 
throwVV 
newVV  
KeyNotFoundExceptionVV .
(VV. /
$"VV/ 1
$strVV1 @
{VV@ A
idVVA C
}VVC D
$strVVD O
"VVO P
)VVP Q
;VVQ R
doctorXX 
.XX 
IsActiveXX 
=XX 
trueXX "
;XX" #
awaitZZ 
_doctorRepositoryZZ #
.ZZ# $
UpdateAsyncZZ$ /
(ZZ/ 0
doctorZZ0 6
)ZZ6 7
;ZZ7 8
await[[ 
_doctorRepository[[ #
.[[# $
SaveChangesAsync[[$ 4
([[4 5
)[[5 6
;[[6 7
}\\ 	
public^^ 
async^^ 
Task^^ 
DeactivateAsync^^ )
(^^) *
int^^* -
id^^. 0
)^^0 1
{__ 	
var`` 
doctor`` 
=`` 
await`` 
_doctorRepository`` 0
.``0 1
GetByIdAsync``1 =
(``= >
id``> @
)``@ A
;``A B
ifbb 
(bb 
doctorbb 
==bb 
nullbb 
)bb 
throwcc 
newcc  
KeyNotFoundExceptioncc .
(cc. /
$"cc/ 1
$strcc1 @
{cc@ A
idccA C
}ccC D
$strccD O
"ccO P
)ccP Q
;ccQ R
doctoree 
.ee 
IsActiveee 
=ee 
falseee #
;ee# $
awaitgg 
_doctorRepositorygg #
.gg# $
UpdateAsyncgg$ /
(gg/ 0
doctorgg0 6
)gg6 7
;gg7 8
awaithh 
_doctorRepositoryhh #
.hh# $
SaveChangesAsynchh$ 4
(hh4 5
)hh5 6
;hh6 7
}ii 	
privatekk 
statickk 
voidkk 
ValidateDoctorkk *
(kk* +
CreateDoctorDtokk+ :
dtokk; >
)kk> ?
{ll 	
ifmm 
(mm 
stringmm 
.mm 
IsNullOrWhiteSpacemm )
(mm) *
dtomm* -
.mm- .
FullNamemm. 6
)mm6 7
)mm7 8
thrownn 
newnn 
ArgumentExceptionnn +
(nn+ ,
$strnn, F
)nnF G
;nnG H
ifpp 
(pp 
!pp 
Enumpp 
.pp 
	IsDefinedpp 
(pp  
typeofpp  &
(pp& ' 
DoctorSpecialisationpp' ;
)pp; <
,pp< =
dtopp> A
.ppA B
SpecialisationppB P
)ppP Q
)ppQ R
throwqq 
newqq 
ArgumentExceptionqq +
(qq+ ,
$strqq, L
)qqL M
;qqM N
ifss 
(ss 
dtoss 
.ss 
YearsOfExperiencess %
<ss& '
$numss( )
||ss* ,
dtoss- 0
.ss0 1
YearsOfExperiencess1 B
>ssC D
$numssE G
)ssG H
throwtt 
newtt 
ArgumentExceptiontt +
(tt+ ,
$strtt, X
)ttX Y
;ttY Z
ifvv 
(vv 
dtovv 
.vv 
ConsultationFeevv #
<=vv$ &
$numvv' (
)vv( )
throwww 
newww 
ArgumentExceptionww +
(ww+ ,
$strww, Y
)wwY Z
;wwZ [
}xx 	
privatezz 
staticzz 
voidzz 
ValidateDoctorzz *
(zz* +
UpdateDoctorDtozz+ :
dtozz; >
)zz> ?
{{{ 	
if|| 
(|| 
string|| 
.|| 
IsNullOrWhiteSpace|| )
(||) *
dto||* -
.||- .
FullName||. 6
)||6 7
)||7 8
throw}} 
new}} 
ArgumentException}} +
(}}+ ,
$str}}, F
)}}F G
;}}G H
if 
( 
! 
Enum 
. 
	IsDefined 
(  
typeof  &
(& ' 
DoctorSpecialisation' ;
); <
,< =
dto> A
.A B
SpecialisationB P
)P Q
)Q R
throw
ÄÄ 
new
ÄÄ 
ArgumentException
ÄÄ +
(
ÄÄ+ ,
$str
ÄÄ, L
)
ÄÄL M
;
ÄÄM N
if
ÇÇ 
(
ÇÇ 
dto
ÇÇ 
.
ÇÇ 
YearsOfExperience
ÇÇ %
<
ÇÇ& '
$num
ÇÇ( )
||
ÇÇ* ,
dto
ÇÇ- 0
.
ÇÇ0 1
YearsOfExperience
ÇÇ1 B
>
ÇÇC D
$num
ÇÇE G
)
ÇÇG H
throw
ÉÉ 
new
ÉÉ 
ArgumentException
ÉÉ +
(
ÉÉ+ ,
$str
ÉÉ, X
)
ÉÉX Y
;
ÉÉY Z
if
ÖÖ 
(
ÖÖ 
dto
ÖÖ 
.
ÖÖ 
ConsultationFee
ÖÖ #
<=
ÖÖ$ &
$num
ÖÖ' (
)
ÖÖ( )
throw
ÜÜ 
new
ÜÜ 
ArgumentException
ÜÜ +
(
ÜÜ+ ,
$str
ÜÜ, Y
)
ÜÜY Z
;
ÜÜZ [
}
áá 	
private
ââ 
static
ââ 
	DoctorDto
ââ  
MapToDoctorDto
ââ! /
(
ââ/ 0
Doctor
ââ0 6
doctor
ââ7 =
)
ââ= >
{
ää 	
return
ãã 
new
ãã 
	DoctorDto
ãã  
{
åå 
DoctorId
çç 
=
çç 
doctor
çç !
.
çç! "
DoctorId
çç" *
,
çç* +
FullName
éé 
=
éé 
doctor
éé !
.
éé! "
FullName
éé" *
,
éé* +
Specialisation
èè 
=
èè  
(
èè! "
int
èè" %
)
èè% &
doctor
èè& ,
.
èè, -
Specialisation
èè- ;
,
èè; <
YearsOfExperience
êê !
=
êê" #
doctor
êê$ *
.
êê* +
YearsOfExperience
êê+ <
,
êê< =
ConsultationFee
ëë 
=
ëë  !
doctor
ëë" (
.
ëë( )
ConsultationFee
ëë) 8
,
ëë8 9
IsActive
íí 
=
íí 
doctor
íí !
.
íí! "
IsActive
íí" *
}
ìì 
;
ìì 
}
îî 	
}
ïï 
}ññ à
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
} €¿
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
ãã "
ValidateBookingAsync
ãã &
(
ãã& '
appointment
åå 
.
åå 
	PatientId
åå %
,
åå% &
dto
çç 
.
çç 
DoctorId
çç 
,
çç 
dto
éé 
.
éé 
ScheduledDate
éé !
,
éé! "
dto
èè 
.
èè 
TimeSlot
èè 
)
èè 
;
èè 
appointment
ëë 
.
ëë 
DoctorId
ëë  
=
ëë! "
dto
ëë# &
.
ëë& '
DoctorId
ëë' /
;
ëë/ 0
appointment
íí 
.
íí 
ScheduledDate
íí %
=
íí& '
dto
íí( +
.
íí+ ,
ScheduledDate
íí, 9
;
íí9 :
appointment
ìì 
.
ìì 
TimeSlot
ìì  
=
ìì! "
(
îî !
AppointmentTimeSlot
îî $
)
îî$ %
dto
îî% (
.
îî( )
TimeSlot
îî) 1
;
îî1 2
await
ññ $
_appointmentRepository
ññ (
.
ññ( )
UpdateAsync
ññ) 4
(
ññ4 5
appointment
ññ5 @
)
ññ@ A
;
ññA B
await
óó $
_appointmentRepository
óó (
.
óó( )
SaveChangesAsync
óó) 9
(
óó9 :
)
óó: ;
;
óó; <
}
òò 	
public
öö 
async
öö 
Task
öö 
ConfirmAsync
öö &
(
öö& '
int
öö' *
id
öö+ -
)
öö- .
{
õõ 	
var
úú 
appointment
úú 
=
úú 
await
ùù $
_appointmentRepository
ùù ,
.
ùù, -
GetByIdAsync
ùù- 9
(
ùù9 :
id
ùù: <
)
ùù< =
;
ùù= >
if
üü 
(
üü 
appointment
üü 
==
üü 
null
üü #
)
üü# $
throw
†† 
new
†† "
KeyNotFoundException
†† .
(
††. /
)
††/ 0
;
††0 1
if
¢¢ 
(
¢¢ 
appointment
¢¢ 
.
¢¢ 
Status
¢¢ "
!=
¢¢# %
AppointmentStatus
¢¢& 7
.
¢¢7 8
Pending
¢¢8 ?
)
¢¢? @
throw
££ 
new
££ '
InvalidOperationException
££ 3
(
££3 4
$str
§§ A
)
§§A B
;
§§B C
appointment
¶¶ 
.
¶¶ 
Status
¶¶ 
=
¶¶  
AppointmentStatus
¶¶! 2
.
¶¶2 3
	Confirmed
¶¶3 <
;
¶¶< =
await
®® $
_appointmentRepository
®® (
.
®®( )
UpdateAsync
®®) 4
(
®®4 5
appointment
®®5 @
)
®®@ A
;
®®A B
await
©© $
_appointmentRepository
©© (
.
©©( )
SaveChangesAsync
©©) 9
(
©©9 :
)
©©: ;
;
©©; <
}
™™ 	
public
¨¨ 
async
¨¨ 
Task
¨¨ 
CompleteAsync
¨¨ '
(
¨¨' (
int
¨¨( +
id
¨¨, .
)
¨¨. /
{
≠≠ 	
var
ÆÆ 
appointment
ÆÆ 
=
ÆÆ 
await
ØØ $
_appointmentRepository
ØØ ,
.
ØØ, -
GetByIdAsync
ØØ- 9
(
ØØ9 :
id
ØØ: <
)
ØØ< =
;
ØØ= >
if
±± 
(
±± 
appointment
±± 
==
±± 
null
±± #
)
±±# $
throw
≤≤ 
new
≤≤ "
KeyNotFoundException
≤≤ .
(
≤≤. /
)
≤≤/ 0
;
≤≤0 1
if
¥¥ 
(
¥¥ 
appointment
¥¥ 
.
¥¥ 
Status
¥¥ "
!=
¥¥# %
AppointmentStatus
¥¥& 7
.
¥¥7 8
	Confirmed
¥¥8 A
)
¥¥A B
throw
µµ 
new
µµ '
InvalidOperationException
µµ 3
(
µµ3 4
$str
∂∂ C
)
∂∂C D
;
∂∂D E
appointment
∏∏ 
.
∏∏ 
Status
∏∏ 
=
∏∏  
AppointmentStatus
∏∏! 2
.
∏∏2 3
	Completed
∏∏3 <
;
∏∏< =
await
∫∫ $
_appointmentRepository
∫∫ (
.
∫∫( )
UpdateAsync
∫∫) 4
(
∫∫4 5
appointment
∫∫5 @
)
∫∫@ A
;
∫∫A B
await
ªª $
_appointmentRepository
ªª (
.
ªª( )
SaveChangesAsync
ªª) 9
(
ªª9 :
)
ªª: ;
;
ªª; <
}
ºº 	
public
ææ 
async
ææ 
Task
ææ 
CancelAsync
ææ %
(
ææ% &
int
øø 
id
øø 
,
øø "
CancelAppointmentDto
¿¿  
dto
¿¿! $
)
¿¿$ %
{
¡¡ 	
var
¬¬ 
appointment
¬¬ 
=
¬¬ 
await
√√ $
_appointmentRepository
√√ ,
.
√√, -
GetByIdAsync
√√- 9
(
√√9 :
id
√√: <
)
√√< =
;
√√= >
if
≈≈ 
(
≈≈ 
appointment
≈≈ 
==
≈≈ 
null
≈≈ #
)
≈≈# $
throw
∆∆ 
new
∆∆ "
KeyNotFoundException
∆∆ .
(
∆∆. /
)
∆∆/ 0
;
∆∆0 1
if
»» 
(
»» 
appointment
»» 
.
»» 
Status
»» "
==
»»# %
AppointmentStatus
»»& 7
.
»»7 8
	Completed
»»8 A
)
»»A B
throw
…… 
new
…… '
InvalidOperationException
…… 3
(
……3 4
$str
   A
)
  A B
;
  B C
if
ÃÃ 
(
ÃÃ 
appointment
ÃÃ 
.
ÃÃ 
Status
ÃÃ "
==
ÃÃ# %
AppointmentStatus
ÃÃ& 7
.
ÃÃ7 8
	Cancelled
ÃÃ8 A
)
ÃÃA B
throw
ÕÕ 
new
ÕÕ '
InvalidOperationException
ÕÕ 3
(
ÕÕ3 4
$str
ŒŒ 4
)
ŒŒ4 5
;
ŒŒ5 6
if
–– 
(
–– 
string
–– 
.
––  
IsNullOrWhiteSpace
–– )
(
––) *
dto
––* -
.
––- . 
CancellationReason
––. @
)
––@ A
)
––A B
throw
—— 
new
—— 
ArgumentException
—— +
(
——+ ,
$str
““ 6
)
““6 7
;
““7 8
appointment
‘‘ 
.
‘‘ 
Status
‘‘ 
=
‘‘  
AppointmentStatus
‘‘! 2
.
‘‘2 3
	Cancelled
‘‘3 <
;
‘‘< =
appointment
’’ 
.
’’  
CancellationReason
’’ *
=
’’+ ,
dto
÷÷ 
.
÷÷  
CancellationReason
÷÷ &
.
÷÷& '
Trim
÷÷' +
(
÷÷+ ,
)
÷÷, -
;
÷÷- .
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
⁄⁄ 	
private
‹‹ 
async
‹‹ 
Task
‹‹ "
ValidateBookingAsync
‹‹ /
(
‹‹/ 0
int
›› 
	patientId
›› 
,
›› 
int
ﬁﬁ 
doctorId
ﬁﬁ 
,
ﬁﬁ 
DateOnly
ﬂﬂ 
date
ﬂﬂ 
,
ﬂﬂ 
int
‡‡ 
timeSlot
‡‡ 
)
‡‡ 
{
·· 	
var
‚‚ 
patient
‚‚ 
=
‚‚ 
await
„„  
_patientRepository
„„ (
.
„„( )
GetByIdAsync
„„) 5
(
„„5 6
	patientId
„„6 ?
)
„„? @
;
„„@ A
if
ÂÂ 
(
ÂÂ 
patient
ÂÂ 
==
ÂÂ 
null
ÂÂ 
)
ÂÂ  
throw
ÊÊ 
new
ÊÊ "
KeyNotFoundException
ÊÊ .
(
ÊÊ. /
$str
ÁÁ (
)
ÁÁ( )
;
ÁÁ) *
if
ÈÈ 
(
ÈÈ 
!
ÈÈ 
patient
ÈÈ 
.
ÈÈ 
IsActive
ÈÈ !
)
ÈÈ! "
throw
ÍÍ 
new
ÍÍ '
InvalidOperationException
ÍÍ 3
(
ÍÍ3 4
$str
ÎÎ A
)
ÎÎA B
;
ÎÎB C
var
ÌÌ 
doctor
ÌÌ 
=
ÌÌ 
await
ÓÓ 
_doctorRepository
ÓÓ '
.
ÓÓ' (
GetByIdAsync
ÓÓ( 4
(
ÓÓ4 5
doctorId
ÓÓ5 =
)
ÓÓ= >
;
ÓÓ> ?
if
 
(
 
doctor
 
==
 
null
 
)
 
throw
ÒÒ 
new
ÒÒ "
KeyNotFoundException
ÒÒ .
(
ÒÒ. /
$str
ÚÚ '
)
ÚÚ' (
;
ÚÚ( )
if
ÙÙ 
(
ÙÙ 
!
ÙÙ 
doctor
ÙÙ 
.
ÙÙ 
IsActive
ÙÙ  
)
ÙÙ  !
throw
ıı 
new
ıı '
InvalidOperationException
ıı 3
(
ıı3 4
$str
ˆˆ &
)
ˆˆ& '
;
ˆˆ' (
if
¯¯ 
(
¯¯ 
date
¯¯ 
<
¯¯ 
DateOnly
¯¯ 
.
¯¯  
FromDateTime
¯¯  ,
(
¯¯, -
DateTime
¯¯- 5
.
¯¯5 6
Today
¯¯6 ;
)
¯¯; <
)
¯¯< =
throw
˘˘ 
new
˘˘ 
ArgumentException
˘˘ +
(
˘˘+ ,
$str
˙˙ =
)
˙˙= >
;
˙˙> ?
if
¸¸ 
(
¸¸ 
!
¸¸ 
Enum
¸¸ 
.
¸¸ 
	IsDefined
¸¸ 
(
¸¸  
typeof
˝˝ 
(
˝˝ !
AppointmentTimeSlot
˝˝ .
)
˝˝. /
,
˝˝/ 0
timeSlot
˛˛ 
)
˛˛ 
)
˛˛ 
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ 
ArgumentException
ÄÄ +
(
ÄÄ+ ,
$str
ÅÅ /
)
ÅÅ/ 0
;
ÅÅ0 1
}
ÇÇ 
if
ÑÑ 
(
ÑÑ 
await
ÑÑ $
_appointmentRepository
ÑÑ ,
.
ÖÖ 6
(ExistsSamePatientSameDoctorSameDateAsync
ÖÖ 9
(
ÖÖ9 :
	patientId
ÜÜ 
,
ÜÜ 
doctorId
áá 
,
áá 
date
àà 
)
àà 
)
àà 
{
ââ 
throw
ää 
new
ää '
InvalidOperationException
ää 3
(
ää3 4
$str
ãã _
)
ãã_ `
;
ãã` a
}
åå 
if
éé 
(
éé 
await
éé $
_appointmentRepository
éé ,
.
èè 4
&ExistsSamePatientSameSlotSameDateAsync
èè 7
(
èè7 8
	patientId
êê 
,
êê 
date
ëë 
,
ëë 
timeSlot
íí 
)
íí 
)
íí 
{
ìì 
throw
îî 
new
îî '
InvalidOperationException
îî 3
(
îî3 4
$str
ïï P
)
ïïP Q
;
ïïQ R
}
ññ 
if
òò 
(
òò 
await
òò $
_appointmentRepository
òò ,
.
ôô 3
%ExistsSameDoctorSameSlotSameDateAsync
ôô 6
(
ôô6 7
doctorId
öö 
,
öö 
date
õõ 
,
õõ 
timeSlot
úú 
)
úú 
)
úú 
{
ùù 
throw
ûû 
new
ûû '
InvalidOperationException
ûû 3
(
ûû3 4
$str
üü B
)
üüB C
;
üüC D
}
†† 
}
°° 	
private
££ 
static
££ 
AppointmentDto
££ %!
MapToAppointmentDto
££& 9
(
££9 :
Appointment
§§ 
appointment
§§ #
)
§§# $
{
•• 	
return
¶¶ 
new
¶¶ 
AppointmentDto
¶¶ %
{
ßß 
AppointmentId
®® 
=
®® 
appointment
®®  +
.
®®+ ,
AppointmentId
®®, 9
,
®®9 :
	PatientId
©© 
=
©© 
appointment
©© '
.
©©' (
	PatientId
©©( 1
,
©©1 2
DoctorId
™™ 
=
™™ 
appointment
™™ &
.
™™& '
DoctorId
™™' /
,
™™/ 0
ScheduledDate
´´ 
=
´´ 
appointment
´´  +
.
´´+ ,
ScheduledDate
´´, 9
,
´´9 :
TimeSlot
¨¨ 
=
¨¨ 
(
¨¨ 
int
¨¨ 
)
¨¨  
appointment
¨¨  +
.
¨¨+ ,
TimeSlot
¨¨, 4
,
¨¨4 5
Status
≠≠ 
=
≠≠ 
(
≠≠ 
int
≠≠ 
)
≠≠ 
appointment
≠≠ )
.
≠≠) *
Status
≠≠* 0
,
≠≠0 1 
CancellationReason
ÆÆ "
=
ÆÆ# $
appointment
ÆÆ% 0
.
ÆÆ0 1 
CancellationReason
ÆÆ1 C
}
ØØ 
;
ØØ 
}
∞∞ 	
private
≤≤ 
static
≤≤ #
DoctorScheduleItemDto
≤≤ ,#
MapDoctorScheduleItem
≤≤- B
(
≤≤B C
Appointment
≥≥ 
appointment
≥≥ #
)
≥≥# $
{
¥¥ 	
return
µµ 
new
µµ #
DoctorScheduleItemDto
µµ ,
{
∂∂ 
AppointmentId
∑∑ 
=
∑∑ 
appointment
∑∑  +
.
∑∑+ ,
AppointmentId
∑∑, 9
,
∑∑9 :
ScheduledDate
∏∏ 
=
∏∏ 
appointment
∏∏  +
.
∏∏+ ,
ScheduledDate
∏∏, 9
,
∏∏9 :
TimeSlot
ππ 
=
ππ 
(
ππ 
int
ππ 
)
ππ  
appointment
ππ  +
.
ππ+ ,
TimeSlot
ππ, 4
,
ππ4 5
	PatientId
∫∫ 
=
∫∫ 
appointment
∫∫ '
.
∫∫' (
	PatientId
∫∫( 1
,
∫∫1 2
PatientName
ªª 
=
ªª 
appointment
ªª )
.
ªª) *
Patient
ªª* 1
.
ªª1 2
FullName
ªª2 :
,
ªª: ;
Status
ºº 
=
ºº 
(
ºº 
int
ºº 
)
ºº 
appointment
ºº )
.
ºº) *
Status
ºº* 0
}
ΩΩ 
;
ΩΩ 
}
ææ 	
}
øø 
}¿¿ “
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
;  
} 
} ‡
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
} Ì
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
SaveChangesAsync 
( 
) 
;  
} 
} ◊
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
Task 
AddAsync 
( 
Appointment !
appointment" -
)- .
;. /
Task 
UpdateAsync 
( 
Appointment $
appointment% 0
)0 1
;1 2
Task 
< 
bool 
> 
ExistsAsync 
( 
int "
id# %
)% &
;& '
Task 
SaveChangesAsync 
( 
) 
;  
} 
} ’
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
}99 	
}:: 
};; ”
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
}33 È%
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
}@@ ﬁ 
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
}99 •7
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
<GG 
boolGG 
>GG 
ExistsAsyncGG  +
(GG+ ,
intGG, /
idGG0 2
)GG2 3
{HH 	
returnII 
awaitII 
_contextII !
.II! "
DoctorsII" )
.II) *
AnyAsyncII* 2
(II2 3
dII3 4
=>II5 7
dII8 9
.II9 :
DoctorIdII: B
==IIC E
idIIF H
)IIH I
;III J
}JJ 	
publicLL 
asyncLL 
TaskLL 
SaveChangesAsyncLL *
(LL* +
)LL+ ,
{MM 	
awaitNN 
_contextNN 
.NN 
SaveChangesAsyncNN +
(NN+ ,
)NN, -
;NN- .
}OO 	
}PP 
}QQ „_
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
Taskii 
AddAsyncii "
(ii" #
Appointmentii# .
appointmentii/ :
)ii: ;
{jj 	
awaitkk 
_contextkk 
.kk 
Appointmentskk '
.kk' (
AddAsynckk( 0
(kk0 1
appointmentkk1 <
)kk< =
;kk= >
}ll 	
publicnn 
Tasknn 
UpdateAsyncnn 
(nn  
Appointmentnn  +
appointmentnn, 7
)nn7 8
{oo 	
_contextpp 
.pp 
Appointmentspp !
.pp! "
Updatepp" (
(pp( )
appointmentpp) 4
)pp4 5
;pp5 6
returnqq 
Taskqq 
.qq 
CompletedTaskqq %
;qq% &
}rr 	
publictt 
asynctt 
Tasktt 
<tt 
booltt 
>tt 
ExistsAsynctt  +
(tt+ ,
inttt, /
idtt0 2
)tt2 3
{uu 	
returnvv 
awaitvv 
_contextvv !
.vv! "
Appointmentsvv" .
.vv. /
AnyAsyncvv/ 7
(vv7 8
avv8 9
=>vv: <
avv= >
.vv> ?
AppointmentIdvv? L
==vvM O
idvvP R
)vvR S
;vvS T
}ww 	
publicyy 
asyncyy 
Taskyy 
SaveChangesAsyncyy *
(yy* +
)yy+ ,
{zz 	
await{{ 
_context{{ 
.{{ 
SaveChangesAsync{{ +
({{+ ,
){{, -
;{{- .
}|| 	
}}} 
}~~ ∞
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
;C D
} 
} ò#
FC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
)  
;  !
builder 
. 
Services 
. 
AddDbContext 
< 
HealthAxisDbContext 1
>1 2
(2 3
options3 :
=>; =
options 
. 
UseSqlServer 
( 
builder 
. 
Configuration 
. 
GetConnectionString 1
(1 2
$str2 E
)E F
)F G
)G H
;H I
builder 
. 
Services 
. 
	AddScoped 
< 
IPatientRepository -
,- .
PatientRepository/ @
>@ A
(A B
)B C
;C D
builder 
. 
Services 
. 
	AddScoped 
< 
IDoctorRepository ,
,, -
DoctorRepository. >
>> ?
(? @
)@ A
;A B
builder 
. 
Services 
. 
	AddScoped 
< "
IAppointmentRepository 1
,1 2!
AppointmentRepository3 H
>H I
(I J
)J K
;K L
builder 
. 
Services 
. 
	AddScoped 
< #
IHealthRecordRepository 2
,2 3"
HealthRecordRepository4 J
>J K
(K L
)L M
;M N
builder"" 
."" 
Services"" 
."" 
	AddScoped"" 
<"" 
IPatientService"" *
,""* +
PatientService"", :
>"": ;
(""; <
)""< =
;""= >
builder## 
.## 
Services## 
.## 
	AddScoped## 
<## 
IDoctorService## )
,##) *
DoctorService##+ 8
>##8 9
(##9 :
)##: ;
;##; <
builder$$ 
.$$ 
Services$$ 
.$$ 
	AddScoped$$ 
<$$ 
IAppointmentService$$ .
,$$. /
AppointmentService$$0 B
>$$B C
($$C D
)$$D E
;$$E F
builder%% 
.%% 
Services%% 
.%% 
	AddScoped%% 
<%%  
IHealthRecordService%% /
,%%/ 0
HealthRecordService%%1 D
>%%D E
(%%E F
)%%F G
;%%G H
var)) 
app)) 
=)) 	
builder))
 
.)) 
Build)) 
()) 
))) 
;)) 
if-- 
(-- 
app-- 
.-- 
Environment-- 
.-- 
IsDevelopment-- !
(--! "
)--" #
)--# $
{.. 
app// 
.// 

UseSwagger// 
(// 
)// 
;// 
app00 
.00 
UseSwaggerUI00 
(00 
options00 
=>00 
{11 
options22 
.22 
SwaggerEndpoint22 
(22  
$str22  :
,22: ;
$str22< O
)22O P
;22P Q
options33 
.33 
RoutePrefix33 
=33 
string33 $
.33$ %
Empty33% *
;33* +
}44 
)44 
;44 
}55 
app77 
.77 
UseHttpsRedirection77 
(77 
)77 
;77 
app99 
.99 
UseAuthorization99 
(99 
)99 
;99 
app;; 
.;; 
MapControllers;; 
(;; 
);; 
;;; 
app?? 
.?? 
Run?? 
(?? 
)?? 	
;??	 
ˆ!
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
}$$ Ç
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
} ö
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
} ∏
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
} ô]
PC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Data\AppDbContext.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Data 
{ 
public 

class 
HealthAxisDbContext $
:% &
	DbContext' 0
{ 
public		 
HealthAxisDbContext		 "
(		" #
DbContextOptions		# 3
<		3 4
HealthAxisDbContext		4 G
>		G H
options		I P
)		P Q
:

 
base

 
(

 
options

 
)

 
{ 	
} 	
public 
DbSet 
< 
Patient 
> 
Patients &
=>' )
Set* -
<- .
Patient. 5
>5 6
(6 7
)7 8
;8 9
public 
DbSet 
< 
Doctor 
> 
Doctors $
=>% '
Set( +
<+ ,
Doctor, 2
>2 3
(3 4
)4 5
;5 6
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
=>/ 1
Set2 5
<5 6
Appointment6 A
>A B
(B C
)C D
;D E
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
=>1 3
Set4 7
<7 8
HealthRecord8 D
>D E
(E F
)F G
;G H
public 
DbSet 
< 
User 
> 
Users  
=>! #
Set$ '
<' (
User( ,
>, -
(- .
). /
;/ 0
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
modelBuilder 
. 
Entity 
<  
Appointment  +
>+ ,
(, -
)- .
. 
HasOne 
( 
a 
=> 
a 
. 
Patient &
)& '
. 
WithMany 
( 
p 
=> 
p  
.  !
Appointments! -
)- .
. 
HasForeignKey 
( 
a  
=>! #
a$ %
.% &
	PatientId& /
)/ 0
. 
OnDelete 
( 
DeleteBehavior (
.( )
Restrict) 1
)1 2
;2 3
modelBuilder!! 
.!! 
Entity!! 
<!!  
Appointment!!  +
>!!+ ,
(!!, -
)!!- .
."" 
HasOne"" 
("" 
a"" 
=>"" 
a"" 
."" 
Doctor"" %
)""% &
.## 
WithMany## 
(## 
d## 
=>## 
d##  
.##  !
Appointments##! -
)##- .
.$$ 
HasForeignKey$$ 
($$ 
a$$  
=>$$! #
a$$$ %
.$$% &
DoctorId$$& .
)$$. /
.%% 
OnDelete%% 
(%% 
DeleteBehavior%% (
.%%( )
Restrict%%) 1
)%%1 2
;%%2 3
modelBuilder(( 
.(( 
Entity(( 
<((  
Appointment((  +
>((+ ,
(((, -
)((- .
.)) 
HasIndex)) 
()) 
a)) 
=>)) 
new)) "
{** 
a++ 
.++ 
DoctorId++ 
,++ 
a,, 
.,, 
ScheduledDate,, #
,,,# $
a-- 
.-- 
TimeSlot-- 
}.. 
).. 
.// 
IsUnique// 
(// 
)// 
;// 
modelBuilder44 
.44 
Entity44 
<44  
HealthRecord44  ,
>44, -
(44- .
)44. /
.55 
HasOne55 
(55 
hr55 
=>55 
hr55  
.55  !
Appointment55! ,
)55, -
.66 
WithOne66 
(66 
a66 
=>66 
a66 
.66  
HealthRecord66  ,
)66, -
.77 
HasForeignKey77 
<77 
HealthRecord77 +
>77+ ,
(77, -
hr77- /
=>770 2
hr773 5
.775 6
AppointmentId776 C
)77C D
.88 
OnDelete88 
(88 
DeleteBehavior88 (
.88( )
Restrict88) 1
)881 2
;882 3
modelBuilder:: 
.:: 
Entity:: 
<::  
HealthRecord::  ,
>::, -
(::- .
)::. /
.;; 
HasOne;; 
(;; 
hr;; 
=>;; 
hr;;  
.;;  !
Patient;;! (
);;( )
.<< 
WithMany<< 
(<< 
p<< 
=><< 
p<<  
.<<  !
HealthRecords<<! .
)<<. /
.== 
HasForeignKey== 
(== 
hr== !
=>==" $
hr==% '
.==' (
	PatientId==( 1
)==1 2
.>> 
OnDelete>> 
(>> 
DeleteBehavior>> (
.>>( )
Restrict>>) 1
)>>1 2
;>>2 3
modelBuilder@@ 
.@@ 
Entity@@ 
<@@  
HealthRecord@@  ,
>@@, -
(@@- .
)@@. /
.AA 
HasOneAA 
(AA 
hrAA 
=>AA 
hrAA  
.AA  !
DoctorAA! '
)AA' (
.BB 
WithManyBB 
(BB 
dBB 
=>BB 
dBB  
.BB  !
HealthRecordsBB! .
)BB. /
.CC 
HasForeignKeyCC 
(CC 
hrCC !
=>CC" $
hrCC% '
.CC' (
DoctorIdCC( 0
)CC0 1
.DD 
OnDeleteDD 
(DD 
DeleteBehaviorDD (
.DD( )
RestrictDD) 1
)DD1 2
;DD2 3
modelBuilderGG 
.GG 
EntityGG 
<GG  
HealthRecordGG  ,
>GG, -
(GG- .
)GG. /
.HH 
HasIndexHH 
(HH 
hrHH 
=>HH 
hrHH  "
.HH" #
AppointmentIdHH# 0
)HH0 1
.II 
IsUniqueII 
(II 
)II 
;II 
modelBuilderMM 
.MM 
EntityMM 
<MM  
DoctorMM  &
>MM& '
(MM' (
)MM( )
.NN 
PropertyNN 
(NN 
dNN 
=>NN 
dNN  
.NN  !
ConsultationFeeNN! 0
)NN0 1
.OO 
HasPrecisionOO 
(OO 
$numOO  
,OO  !
$numOO" #
)OO# $
;OO$ %
modelBuilderSS 
.SS 
EntitySS 
<SS  
PatientSS  '
>SS' (
(SS( )
)SS) *
.SS* +
HasDataSS+ 2
(SS2 3
newTT 
PatientTT 
{UU 
	PatientIdVV 
=VV 
$numVV  !
,VV! "
FullNameWW 
=WW 
$strWW -
,WW- .
DateOfBirthXX 
=XX  !
newXX" %
DateOnlyXX& .
(XX. /
$numXX/ 3
,XX3 4
$numXX5 6
,XX6 7
$numXX8 :
)XX: ;
,XX; <
GenderYY 
=YY 
GenderYY #
.YY# $
MaleYY$ (
,YY( )
PhoneNumberZZ 
=ZZ  !
$strZZ" .
,ZZ. /
Email[[ 
=[[ 
$str[[ /
,[[/ 0
InsuranceStatus\\ #
=\\$ %
InsuranceStatus\\& 5
.\\5 6
Active\\6 <
,\\< =
InsuranceNumber]] #
=]]$ %
$str]]& /
,]]/ 0
IsActive^^ 
=^^ 
true^^ #
}__ 
,__ 
new`` 
Patient`` 
{aa 
	PatientIdbb 
=bb 
$numbb  !
,bb! "
FullNamecc 
=cc 
$strcc ,
,cc, -
DateOfBirthdd 
=dd  !
newdd" %
DateOnlydd& .
(dd. /
$numdd/ 3
,dd3 4
$numdd5 7
,dd7 8
$numdd9 :
)dd: ;
,dd; <
Genderee 
=ee 
Genderee #
.ee# $
Femaleee$ *
,ee* +
PhoneNumberff 
=ff  !
$strff" .
,ff. /
Emailgg 
=gg 
$strgg .
,gg. /
InsuranceStatushh #
=hh$ %
InsuranceStatushh& 5
.hh5 6
Activehh6 <
,hh< =
InsuranceNumberii #
=ii$ %
$strii& /
,ii/ 0
IsActivejj 
=jj 
truejj #
}kk 
)ll 
;ll 
modelBuilderpp 
.pp 
Entitypp 
<pp  
Doctorpp  &
>pp& '
(pp' (
)pp( )
.pp) *
HasDatapp* 1
(pp1 2
newqq 
Doctorqq 
{rr 
DoctorIdss 
=ss 
$numss  
,ss  !
FullNamett 
=tt 
$strtt *
,tt* +
Specialisationuu "
=uu# $ 
DoctorSpecialisationuu% 9
.uu9 :
GeneralPractitioneruu: M
,uuM N
YearsOfExperiencevv %
=vv& '
$numvv( )
,vv) *
ConsultationFeeww #
=ww$ %
$numww& -
,ww- .
IsActivexx 
=xx 
truexx #
}yy 
,yy 
newzz 
Doctorzz 
{{{ 
DoctorId|| 
=|| 
$num||  
,||  !
FullName}} 
=}} 
$str}} ,
,}}, -
Specialisation~~ "
=~~# $ 
DoctorSpecialisation~~% 9
.~~9 :
Cardiologist~~: F
,~~F G
YearsOfExperience %
=& '
$num( *
,* +
ConsultationFee
ÄÄ #
=
ÄÄ$ %
$num
ÄÄ& .
,
ÄÄ. /
IsActive
ÅÅ 
=
ÅÅ 
true
ÅÅ #
}
ÇÇ 
)
ÉÉ 
;
ÉÉ 
modelBuilder
ââ 
.
ââ 
Entity
ââ 
<
ââ  
Appointment
ââ  +
>
ââ+ ,
(
ââ, -
)
ââ- .
.
ââ. /
HasData
ââ/ 6
(
ââ6 7
new
ää 
Appointment
ää 
{
ãã 
AppointmentId
åå !
=
åå" #
$num
åå$ %
,
åå% &
	PatientId
çç 
=
çç 
$num
çç  !
,
çç! "
DoctorId
éé 
=
éé 
$num
éé  
,
éé  !
ScheduledDate
èè !
=
èè" #
new
èè$ '
DateOnly
èè( 0
(
èè0 1
$num
èè1 5
,
èè5 6
$num
èè7 8
,
èè8 9
$num
èè: <
)
èè< =
,
èè= >
TimeSlot
êê 
=
êê !
AppointmentTimeSlot
êê 2
.
êê2 3
TenAM
êê3 8
,
êê8 9
Status
ëë 
=
ëë 
AppointmentStatus
ëë .
.
ëë. /
Pending
ëë/ 6
}
íí 
)
ìì 
;
ìì 
}
îî 	
}
ïï 
}ññ Ω
[C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxisApi\Controllers\HealthController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
HealthController !
:" #
ControllerBase$ 2
{ 
[		 	
HttpGet			 
]		 
public

 
IActionResult

 
Get

  
(

  !
)

! "
{ 	
return 
Ok 
( 
new 
{ 
status 
= 
$str "
," #
service 
= 
$str *
} 
) 
; 
} 	
} 
} 