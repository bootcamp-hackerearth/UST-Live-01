˚
uC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IPatientService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IPatientService $
{ 
Task		 
<		 
PagedResponse		 
<		 

PatientDto		 %
>		% &
>		& '
GetPagedAsync		( 5
(		5 6
PaginationParams		6 F
paginationParams		G W
)		W X
;		X Y
Task 
< 
PagedResponse 
< 

PatientDto %
>% &
>& '"
GetDoctorPatientsAsync( >
(> ?
string 
doctorUserId 
,  
PaginationParams 
paginationParams -
)- .
;. /
Task 
< 
bool 
> 
IsPatientOwnerAsync &
(& '
int' *
	patientId+ 4
,4 5
string6 <
userId= C
)C D
;D E
Task 
< 

PatientDto 
? 
> 
GetByIdAsync &
(& '
int' *
id+ -
)- .
;. /
Task 
< 

PatientDto 
> 
AddAsync !
(! "
CreatePatientDto" 2
dto3 6
)6 7
;7 8
Task 
< 

PatientDto 
> 
UpdateAsync $
($ %
int% (
id) +
,+ ,
UpdatePatientDto- =
dto> A
)A B
;B C
Task 
< 
IEnumerable 
< 
HealthRecordDto (
>( )
>) *!
GetHealthRecordsAsync+ @
(@ A
intA D
	patientIdE N
)N O
;O P
Task 
< 

PatientDto 
? 
> 
GetByUserIdAsync *
(* +
string+ 1
userId2 8
)8 9
;9 :
} 
} ç
zC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IHealthRecordService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface  
IHealthRecordService )
{ 
Task 
< 
IEnumerable 
< 
HealthRecordDto (
>( )
>) *
GetByPatientIdAsync+ >
(> ?
int? B
	patientIdC L
)L M
;M N
Task		 
<		 
HealthRecordDto		 
>		 
GetByIdAsync		 *
(		* +
int		+ .
id		/ 1
)		1 2
;		2 3
Task 
< 
HealthRecordDto 
> 
AddAsync &
(& '!
CreateHealthRecordDto' <
dto= @
)@ A
;A B
} 
} ⁄
tC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IDoctorService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "

Interfaces" ,
{ 
public 

	interface 
IDoctorService #
{ 
Task 
< 
IEnumerable 
< 
	DoctorDto "
>" #
># $
GetAllAsync% 0
(0 1
)1 2
;2 3
Task

 
<

 
PagedResponse

 
<

 
	DoctorDto

 $
>

$ %
>

% &
GetAllAsync

' 2
(

2 3
PaginationParams 
paginationParams -
,- .
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< 
	DoctorDto 
? 
> 
GetByIdAsync %
(% &
int 
id 
, 
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< 
	DoctorDto 
? 
> 
GetByUserIdAsync )
() *
string 
userId 
, 
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< 
object 
>  
GetAvailabilityAsync )
() *
int 
id 
, 
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< !
DoctorAvailabilityDto "
>" # 
GetAvailabilityAsync$ 8
(8 9
int 
id 
, 
DateTime 
date 
, 
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< 
	DoctorDto 
> 
AddAsync  
(  !
CreateDoctorDto   
dto   
,    
CancellationToken!! 
ct!!  
=!!! "
default!!# *
)!!* +
;!!+ ,
Task## 
<## 
	DoctorDto## 
>## 
UpdateAsync## #
(### $
int$$ 
id$$ 
,$$ 
UpdateDoctorDto%% 
dto%% 
,%%  
CancellationToken&& 
ct&&  
=&&! "
default&&# *
)&&* +
;&&+ ,
}'' 
}(( â
rC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IAuthService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "

Interfaces" ,
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
,* +
string, 2
UserId3 9
)9 :
>: ;
Register< D
(D E
RegisterDtoE P
requestQ X
)X Y
;Y Z
Task		 
<		 
(		 
bool		 
Success		 
,		 
string		 "
Message		# *
,		* +
string		, 2
AccessToken		3 >
,		> ?
string		@ F
RefreshToken		G S
,		S T
int		U X
	ExpiresIn		Y b
,		b c
bool		d h"
RequiresPasswordChange		i 
)			 Ä
>
		Ä Å
Login
		Ç á
(
		á à
LoginDto
		à ê
request
		ë ò
)
		ò ô
;
		ô ö
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
)* +
>+ ,
ChangePassword- ;
(; <
ChangePasswordDto< M
requestN U
)U V
;V W
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
string, 2
AccessToken3 >
,> ?
string@ F
RefreshTokenG S
,S T
intU X
	ExpiresInY b
)b c
>c d
RefreshTokene q
(q r#
RefreshTokenRequestDto	r à
request
â ê
)
ê ë
;
ë í
} 
} Õ	
yC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IAppointmentService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "

Interfaces" ,
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
<		 
AppointmentDto		 
>		 
AddAsync		 %
(		% & 
CreateAppointmentDto		& :
dto		; >
)		> ?
;		? @
Task 
< 
AppointmentDto 
> 
UpdateStatusAsync .
(. /
int/ 2
id3 5
,5 6&
UpdateAppointmentStatusDto7 Q
dtoR U
)U V
;V W
Task 
< 
bool 
> 
DeleteAsync 
( 
int "
id# %
)% &
;& '
} 
} ˝
sC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IAdminService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "

Interfaces" ,
;, -
public 
	interface 
IAdminService 
{ 
Task 
< 	
List	 
< 
	DoctorDto 
> 
> 
GetDoctorsAsync )
() *
)* +
;+ ,
Task		 
<		 	
	DoctorDto			 
>		 
CreateDoctorAsync		 %
(		% &
CreateDoctorDto		& 5
dto		6 9
)		9 :
;		: ;
Task 
< 	
	DoctorDto	 
? 
> 
UpdateDoctorAsync &
(& '
int 
id 
, 
UpdateDoctorDto 
dto 
) 
; 
} áæ
yC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\PatientService.cs
	namespace

 	

HealthAxis


 
.

 
API

 
.

 
Services

 !
.

! "
Implementations

" 1
{ 
public 

class 
PatientService 
:  !
IPatientService" 1
{ 
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_patientRepository. @
;@ A
private 
readonly 
IRepository $
<$ %
Doctor% +
>+ ,
_doctorRepository- >
;> ?
private 
readonly 
IRepository $
<$ %
Appointment% 0
>0 1"
_appointmentRepository2 H
;H I
private 
readonly 
IRepository $
<$ %
HealthRecord% 1
>1 2#
_healthRecordRepository3 J
;J K
private 
readonly 
IMapper  
_mapper! (
;( )
public 
PatientService 
( 
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
IRepository 
< 
Doctor 
> 
doctorRepository  0
,0 1
IRepository 
< 
Appointment #
># $!
appointmentRepository% :
,: ;
IRepository 
< 
HealthRecord $
>$ %"
healthRecordRepository& <
,< =
IMapper 
mapper 
) 
{ 	
_patientRepository 
=  
patientRepository! 2
;2 3
_doctorRepository 
= 
doctorRepository  0
;0 1"
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;#
_healthRecordRepository #
=$ %"
healthRecordRepository& <
;< =
_mapper 
= 
mapper 
; 
}   	
public"" 
async"" 
Task"" 
<"" 
PagedResponse"" '
<""' (

PatientDto""( 2
>""2 3
>""3 4
GetPagedAsync""5 B
(""B C
PaginationParams""C S
paginationParams""T d
)""d e
{## 	
var$$ 
patients$$ 
=$$ 
await$$  
_patientRepository$$! 3
.$$3 4
GetAllAsync$$4 ?
($$? @
)$$@ A
;$$A B
var&& 

pageNumber&& 
=&& 
paginationParams&& -
.&&- .

PageNumber&&. 8
<=&&9 ;
$num&&< =
?&&> ?
$num&&@ A
:&&B C
paginationParams&&D T
.&&T U

PageNumber&&U _
;&&_ `
var'' 
pageSize'' 
='' 
paginationParams'' +
.''+ ,
PageSize'', 4
<=''5 7
$num''8 9
?'': ;
$num''< >
:''? @
paginationParams''A Q
.''Q R
PageSize''R Z
;''Z [
var)) 
totalRecords)) 
=)) 
patients)) '
.))' (
Count))( -
;))- .
var++ 
pagedPatients++ 
=++ 
patients++  (
.,, 
Skip,, 
(,, 
(,, 

pageNumber,, !
-,," #
$num,,$ %
),,% &
*,,' (
pageSize,,) 1
),,1 2
.-- 
Take-- 
(-- 
pageSize-- 
)-- 
;--  
var// 
patientDtos// 
=// 
_mapper// %
.//% &
Map//& )
<//) *
IEnumerable//* 5
<//5 6

PatientDto//6 @
>//@ A
>//A B
(//B C
pagedPatients//C P
)//P Q
;//Q R
return11 
new11 
PagedResponse11 $
<11$ %

PatientDto11% /
>11/ 0
{22 
Items33 
=33 
patientDtos33 #
.33# $
ToList33$ *
(33* +
)33+ ,
,33, -

PageNumber44 
=44 

pageNumber44 '
,44' (
PageSize55 
=55 
pageSize55 #
,55# $
TotalRecords66 
=66 
totalRecords66 +
,66+ ,

TotalPages77 
=77 
(77 
int77 !
)77! "
Math77" &
.77& '
Ceiling77' .
(77. /
totalRecords77/ ;
/77< =
(77> ?
double77? E
)77E F
pageSize77F N
)77N O
}88 
;88 
}99 	
public;; 
async;; 
Task;; 
<;; 
PagedResponse;; '
<;;' (

PatientDto;;( 2
>;;2 3
>;;3 4"
GetDoctorPatientsAsync;;5 K
(;;K L
string<< 
doctorUserId<< 
,<<  
PaginationParams== 
paginationParams== -
)==- .
{>> 	
var?? 
doctors?? 
=?? 
await?? 
_doctorRepository??  1
.??1 2
GetAllAsync??2 =
(??= >
)??> ?
;??? @
varAA 
doctorAA 
=AA 
doctorsAA  
.AA  !
FirstOrDefaultAA! /
(AA/ 0
dAA0 1
=>AA2 4
dAA5 6
.AA6 7
UserIdAA7 =
==AA> @
doctorUserIdAAA M
)AAM N
;AAN O
ifCC 
(CC 
doctorCC 
==CC 
nullCC 
)CC 
{DD 
throwEE 
newEE 
NotFoundExceptionEE +
(EE+ ,
$strEE, F
)EEF G
;EEG H
}FF 
varHH 
appointmentsHH 
=HH 
awaitHH $"
_appointmentRepositoryHH% ;
.HH; <
GetAllAsyncHH< G
(HHG H
)HHH I
;HHI J
varJJ 

patientIdsJJ 
=JJ 
appointmentsJJ )
.KK 
WhereKK 
(KK 
aKK 
=>KK 
aKK 
.KK 
DoctorIdKK &
==KK' )
doctorKK* 0
.KK0 1
DoctorIdKK1 9
)KK9 :
.LL 
SelectLL 
(LL 
aLL 
=>LL 
aLL 
.LL 
	PatientIdLL (
)LL( )
.MM 
DistinctMM 
(MM 
)MM 
.NN 
ToListNN 
(NN 
)NN 
;NN 
varPP 
patientsPP 
=PP 
awaitPP  
_patientRepositoryPP! 3
.PP3 4
GetAllAsyncPP4 ?
(PP? @
)PP@ A
;PPA B
varRR 
doctorPatientsRR 
=RR  
patientsRR! )
.SS 
WhereSS 
(SS 
pSS 
=>SS 

patientIdsSS &
.SS& '
ContainsSS' /
(SS/ 0
pSS0 1
.SS1 2
	PatientIdSS2 ;
)SS; <
)SS< =
.TT 
ToListTT 
(TT 
)TT 
;TT 
varVV 

pageNumberVV 
=VV 
paginationParamsVV -
.VV- .

PageNumberVV. 8
<=VV9 ;
$numVV< =
?VV> ?
$numVV@ A
:VVB C
paginationParamsVVD T
.VVT U

PageNumberVVU _
;VV_ `
varWW 
pageSizeWW 
=WW 
paginationParamsWW +
.WW+ ,
PageSizeWW, 4
<=WW5 7
$numWW8 9
?WW: ;
$numWW< >
:WW? @
paginationParamsWWA Q
.WWQ R
PageSizeWWR Z
;WWZ [
varYY 
totalRecordsYY 
=YY 
doctorPatientsYY -
.YY- .
CountYY. 3
;YY3 4
var[[ 
pagedPatients[[ 
=[[ 
doctorPatients[[  .
.\\ 
Skip\\ 
(\\ 
(\\ 

pageNumber\\ !
-\\" #
$num\\$ %
)\\% &
*\\' (
pageSize\\) 1
)\\1 2
.]] 
Take]] 
(]] 
pageSize]] 
)]] 
;]]  
var__ 
patientDtos__ 
=__ 
_mapper__ %
.__% &
Map__& )
<__) *
IEnumerable__* 5
<__5 6

PatientDto__6 @
>__@ A
>__A B
(__B C
pagedPatients__C P
)__P Q
;__Q R
returnaa 
newaa 
PagedResponseaa $
<aa$ %

PatientDtoaa% /
>aa/ 0
{bb 
Itemscc 
=cc 
patientDtoscc #
.cc# $
ToListcc$ *
(cc* +
)cc+ ,
,cc, -

PageNumberdd 
=dd 

pageNumberdd '
,dd' (
PageSizeee 
=ee 
pageSizeee #
,ee# $
TotalRecordsff 
=ff 
totalRecordsff +
,ff+ ,

TotalPagesgg 
=gg 
(gg 
intgg !
)gg! "
Mathgg" &
.gg& '
Ceilinggg' .
(gg. /
totalRecordsgg/ ;
/gg< =
(gg> ?
doublegg? E
)ggE F
pageSizeggF N
)ggN O
}hh 
;hh 
}ii 	
publickk 
asynckk 
Taskkk 
<kk 
boolkk 
>kk 
IsPatientOwnerAsynckk  3
(kk3 4
intkk4 7
	patientIdkk8 A
,kkA B
stringkkC I
userIdkkJ P
)kkP Q
{ll 	
varmm 
patientmm 
=mm 
awaitmm 
_patientRepositorymm  2
.mm2 3
GetByIdAsyncmm3 ?
(mm? @
	patientIdmm@ I
)mmI J
;mmJ K
ifoo 
(oo 
patientoo 
==oo 
nulloo 
)oo  
{pp 
throwqq 
newqq 
NotFoundExceptionqq +
(qq+ ,
$strqq, ?
)qq? @
;qq@ A
}rr 
returntt 
patienttt 
.tt 
UserIdtt !
==tt" $
userIdtt% +
;tt+ ,
}uu 	
publicww 
asyncww 
Taskww 
<ww 

PatientDtoww $
?ww$ %
>ww% &
GetByIdAsyncww' 3
(ww3 4
intww4 7
idww8 :
)ww: ;
{xx 	
varyy 
patientyy 
=yy 
awaityy 
_patientRepositoryyy  2
.yy2 3
GetByIdAsyncyy3 ?
(yy? @
idyy@ B
)yyB C
;yyC D
if{{ 
({{ 
patient{{ 
=={{ 
null{{ 
){{  
{|| 
throw}} 
new}} 
NotFoundException}} +
(}}+ ,
$str}}, ?
)}}? @
;}}@ A
}~~ 
return
ÄÄ 
_mapper
ÄÄ 
.
ÄÄ 
Map
ÄÄ 
<
ÄÄ 

PatientDto
ÄÄ )
>
ÄÄ) *
(
ÄÄ* +
patient
ÄÄ+ 2
)
ÄÄ2 3
;
ÄÄ3 4
}
ÅÅ 	
public
ÉÉ 
async
ÉÉ 
Task
ÉÉ 
<
ÉÉ 

PatientDto
ÉÉ $
>
ÉÉ$ %
AddAsync
ÉÉ& .
(
ÉÉ. /
CreatePatientDto
ÉÉ/ ?
dto
ÉÉ@ C
)
ÉÉC D
{
ÑÑ 	
if
ÖÖ 
(
ÖÖ 
dto
ÖÖ 
.
ÖÖ 
DateOfBirth
ÖÖ 
.
ÖÖ  
Date
ÖÖ  $
>
ÖÖ% &
DateTime
ÖÖ' /
.
ÖÖ/ 0
Today
ÖÖ0 5
)
ÖÖ5 6
{
ÜÜ 
throw
áá 
new
áá !
ValidationException
áá -
(
áá- .
$str
áá. V
)
ááV W
;
ááW X
}
àà 
if
ää 
(
ää 
dto
ää 
.
ää 
DateOfBirth
ää 
.
ää  
Date
ää  $
<
ää% &
new
ää' *
DateTime
ää+ 3
(
ää3 4
$num
ää4 8
,
ää8 9
$num
ää: ;
,
ää; <
$num
ää= >
,
ää> ?
$num
ää@ A
,
ääA B
$num
ääC D
,
ääD E
$num
ääF G
,
ääG H
DateTimeKind
ääI U
.
ääU V
Unspecified
ääV a
)
ääa b
)
ääb c
{
ãã 
throw
åå 
new
åå !
ValidationException
åå -
(
åå- .
$str
åå. X
)
ååX Y
;
ååY Z
}
çç 
var
èè 
existingPatients
èè  
=
èè! "
await
èè# ( 
_patientRepository
èè) ;
.
èè; <
GetAllAsync
èè< G
(
èèG H
)
èèH I
;
èèI J
var
ëë 
emailExists
ëë 
=
ëë 
existingPatients
ëë .
.
ëë. /
Any
ëë/ 2
(
ëë2 3
patient
ëë3 :
=>
ëë; =
patient
íí 
.
íí 
Email
íí 
.
íí 
Equals
íí $
(
íí$ %
dto
íí% (
.
íí( )
Email
íí) .
,
íí. /
StringComparison
íí0 @
.
íí@ A
OrdinalIgnoreCase
ííA R
)
ííR S
)
ííS T
;
ííT U
if
îî 
(
îî 
emailExists
îî 
)
îî 
{
ïï 
throw
ññ 
new
ññ !
ValidationException
ññ -
(
ññ- .
$str
ññ. M
)
ññM N
;
ññN O
}
óó 
var
ôô 
phoneExists
ôô 
=
ôô 
existingPatients
ôô .
.
ôô. /
Any
ôô/ 2
(
ôô2 3
patient
ôô3 :
=>
ôô; =
patient
öö 
.
öö 
PhoneNumber
öö #
==
öö$ &
dto
öö' *
.
öö* +
PhoneNumber
öö+ 6
)
öö6 7
;
öö7 8
if
úú 
(
úú 
phoneExists
úú 
)
úú 
{
ùù 
throw
ûû 
new
ûû !
ValidationException
ûû -
(
ûû- .
$str
ûû. T
)
ûûT U
;
ûûU V
}
üü 
var
°° 
patient
°° 
=
°° 
new
°° 
Patient
°° %
{
¢¢ 
UserId
££ 
=
££ 
null
££ 
,
££ 
FullName
§§ 
=
§§ 
dto
§§ 
.
§§ 
FullName
§§ '
,
§§' (
DateOfBirth
•• 
=
•• 
dto
•• !
.
••! "
DateOfBirth
••" -
,
••- .
Gender
¶¶ 
=
¶¶ 
dto
¶¶ 
.
¶¶ 
Gender
¶¶ #
,
¶¶# $
PhoneNumber
ßß 
=
ßß 
dto
ßß !
.
ßß! "
PhoneNumber
ßß" -
,
ßß- .
Email
®® 
=
®® 
dto
®® 
.
®® 
Email
®® !
,
®®! "
InsuranceId
©© 
=
©© 
null
©© "
,
©©" #
CreatedDate
™™ 
=
™™ 
DateTime
™™ &
.
™™& '
Today
™™' ,
}
´´ 
;
´´ 
var
≠≠ 
savedPatient
≠≠ 
=
≠≠ 
await
≠≠ $ 
_patientRepository
≠≠% 7
.
≠≠7 8
AddAsync
≠≠8 @
(
≠≠@ A
patient
≠≠A H
)
≠≠H I
;
≠≠I J
return
ØØ 
_mapper
ØØ 
.
ØØ 
Map
ØØ 
<
ØØ 

PatientDto
ØØ )
>
ØØ) *
(
ØØ* +
savedPatient
ØØ+ 7
)
ØØ7 8
;
ØØ8 9
}
∞∞ 	
public
≤≤ 
async
≤≤ 
Task
≤≤ 
<
≤≤ 

PatientDto
≤≤ $
>
≤≤$ %
UpdateAsync
≤≤& 1
(
≤≤1 2
int
≤≤2 5
id
≤≤6 8
,
≤≤8 9
UpdatePatientDto
≤≤: J
dto
≤≤K N
)
≤≤N O
{
≥≥ 	
if
¥¥ 
(
¥¥ 
dto
¥¥ 
.
¥¥ 
DateOfBirth
¥¥ 
.
¥¥  
Date
¥¥  $
>
¥¥% &
DateTime
¥¥' /
.
¥¥/ 0
Today
¥¥0 5
)
¥¥5 6
{
µµ 
throw
∂∂ 
new
∂∂ !
ValidationException
∂∂ -
(
∂∂- .
$str
∂∂. V
)
∂∂V W
;
∂∂W X
}
∑∑ 
if
ππ 
(
ππ 
dto
ππ 
.
ππ 
DateOfBirth
ππ 
.
ππ  
Date
ππ  $
<
ππ% &
new
ππ' *
DateTime
ππ+ 3
(
ππ3 4
$num
ππ4 8
,
ππ8 9
$num
ππ: ;
,
ππ; <
$num
ππ= >
,
ππ> ?
$num
ππ? @
,
ππ@ A
$num
ππA B
,
ππB C
$num
ππC D
,
ππD E
DateTimeKind
ππE Q
.
ππQ R
Unspecified
ππR ]
)
ππ] ^
)
ππ^ _
{
∫∫ 
throw
ªª 
new
ªª !
ValidationException
ªª -
(
ªª- .
$str
ªª. X
)
ªªX Y
;
ªªY Z
}
ºº 
var
ææ 
patient
ææ 
=
ææ 
await
ææ  
_patientRepository
ææ  2
.
ææ2 3
GetByIdAsync
ææ3 ?
(
ææ? @
id
ææ@ B
)
ææB C
;
ææC D
if
¿¿ 
(
¿¿ 
patient
¿¿ 
==
¿¿ 
null
¿¿ 
)
¿¿  
{
¡¡ 
throw
¬¬ 
new
¬¬ 
NotFoundException
¬¬ +
(
¬¬+ ,
$str
¬¬, ?
)
¬¬? @
;
¬¬@ A
}
√√ 
var
≈≈ 
existingPatients
≈≈  
=
≈≈! "
await
≈≈# ( 
_patientRepository
≈≈) ;
.
≈≈; <
GetAllAsync
≈≈< G
(
≈≈G H
)
≈≈H I
;
≈≈I J
var
«« 
emailExists
«« 
=
«« 
existingPatients
«« .
.
««. /
Any
««/ 2
(
««2 3
existingPatient
««3 B
=>
««C E
existingPatient
»» 
.
»»  
	PatientId
»»  )
!=
»»* ,
id
»»- /
&&
»»0 2
existingPatient
…… 
.
……  
Email
……  %
.
……% &
Equals
……& ,
(
……, -
dto
……- 0
.
……0 1
Email
……1 6
,
……6 7
StringComparison
……8 H
.
……H I
OrdinalIgnoreCase
……I Z
)
……Z [
)
……[ \
;
……\ ]
if
ÀÀ 
(
ÀÀ 
emailExists
ÀÀ 
)
ÀÀ 
{
ÃÃ 
throw
ÕÕ 
new
ÕÕ !
ValidationException
ÕÕ -
(
ÕÕ- .
$str
ÕÕ. M
)
ÕÕM N
;
ÕÕN O
}
ŒŒ 
var
–– 
phoneExists
–– 
=
–– 
existingPatients
–– .
.
––. /
Any
––/ 2
(
––2 3
existingPatient
––3 B
=>
––C E
existingPatient
—— 
.
——  
	PatientId
——  )
!=
——* ,
id
——- /
&&
——0 2
existingPatient
““ 
.
““  
PhoneNumber
““  +
==
““, .
dto
““/ 2
.
““2 3
PhoneNumber
““3 >
)
““> ?
;
““? @
if
‘‘ 
(
‘‘ 
phoneExists
‘‘ 
)
‘‘ 
{
’’ 
throw
÷÷ 
new
÷÷ !
ValidationException
÷÷ -
(
÷÷- .
$str
÷÷. T
)
÷÷T U
;
÷÷U V
}
◊◊ 
patient
ŸŸ 
.
ŸŸ 
FullName
ŸŸ 
=
ŸŸ 
dto
ŸŸ "
.
ŸŸ" #
FullName
ŸŸ# +
;
ŸŸ+ ,
patient
⁄⁄ 
.
⁄⁄ 
DateOfBirth
⁄⁄ 
=
⁄⁄  !
dto
⁄⁄" %
.
⁄⁄% &
DateOfBirth
⁄⁄& 1
;
⁄⁄1 2
patient
€€ 
.
€€ 
Gender
€€ 
=
€€ 
dto
€€  
.
€€  !
Gender
€€! '
;
€€' (
patient
‹‹ 
.
‹‹ 
PhoneNumber
‹‹ 
=
‹‹  !
dto
‹‹" %
.
‹‹% &
PhoneNumber
‹‹& 1
;
‹‹1 2
patient
›› 
.
›› 
Email
›› 
=
›› 
dto
›› 
.
››  
Email
››  %
;
››% &
patient
ﬁﬁ 
.
ﬁﬁ 
InsuranceId
ﬁﬁ 
=
ﬁﬁ  !
null
ﬁﬁ" &
;
ﬁﬁ& '
await
‡‡  
_patientRepository
‡‡ $
.
‡‡$ %
UpdateAsync
‡‡% 0
(
‡‡0 1
id
‡‡1 3
,
‡‡3 4
patient
‡‡5 <
,
‡‡< =
CancellationToken
‡‡> O
.
‡‡O P
None
‡‡P T
)
‡‡T U
;
‡‡U V
return
‚‚ 
_mapper
‚‚ 
.
‚‚ 
Map
‚‚ 
<
‚‚ 

PatientDto
‚‚ )
>
‚‚) *
(
‚‚* +
patient
‚‚+ 2
)
‚‚2 3
;
‚‚3 4
}
„„ 	
public
ÂÂ 
async
ÂÂ 
Task
ÂÂ 
<
ÂÂ 
IEnumerable
ÂÂ %
<
ÂÂ% &
HealthRecordDto
ÂÂ& 5
>
ÂÂ5 6
>
ÂÂ6 7#
GetHealthRecordsAsync
ÂÂ8 M
(
ÂÂM N
int
ÂÂN Q
	patientId
ÂÂR [
)
ÂÂ[ \
{
ÊÊ 	
var
ÁÁ 
patient
ÁÁ 
=
ÁÁ 
await
ÁÁ  
_patientRepository
ÁÁ  2
.
ÁÁ2 3
GetByIdAsync
ÁÁ3 ?
(
ÁÁ? @
	patientId
ÁÁ@ I
)
ÁÁI J
;
ÁÁJ K
if
ÈÈ 
(
ÈÈ 
patient
ÈÈ 
==
ÈÈ 
null
ÈÈ 
)
ÈÈ  
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ 
NotFoundException
ÎÎ +
(
ÎÎ+ ,
$str
ÎÎ, ?
)
ÎÎ? @
;
ÎÎ@ A
}
ÏÏ 
var
ÓÓ 
records
ÓÓ 
=
ÓÓ 
await
ÓÓ %
_healthRecordRepository
ÓÓ  7
.
ÓÓ7 8
GetAllAsync
ÓÓ8 C
(
ÓÓC D
)
ÓÓD E
;
ÓÓE F
var
 
patientRecords
 
=
  
records
! (
.
ÒÒ 
Where
ÒÒ 
(
ÒÒ 
r
ÒÒ 
=>
ÒÒ 
r
ÒÒ 
.
ÒÒ 
	PatientId
ÒÒ '
==
ÒÒ( *
	patientId
ÒÒ+ 4
)
ÒÒ4 5
;
ÒÒ5 6
return
ÛÛ 
_mapper
ÛÛ 
.
ÛÛ 
Map
ÛÛ 
<
ÛÛ 
IEnumerable
ÛÛ *
<
ÛÛ* +
HealthRecordDto
ÛÛ+ :
>
ÛÛ: ;
>
ÛÛ; <
(
ÛÛ< =
patientRecords
ÛÛ= K
)
ÛÛK L
;
ÛÛL M
}
ÙÙ 	
public
ıı 
async
ıı 
Task
ıı 
<
ıı 

PatientDto
ıı $
?
ıı$ %
>
ıı% &
GetByUserIdAsync
ıı' 7
(
ıı7 8
string
ıı8 >
userId
ıı? E
)
ııE F
{
ˆˆ 	
var
˜˜ 
patients
˜˜ 
=
˜˜ 
await
˜˜   
_patientRepository
˜˜! 3
.
˜˜3 4
GetAllAsync
˜˜4 ?
(
˜˜? @
)
˜˜@ A
;
˜˜A B
var
˘˘ 
patient
˘˘ 
=
˘˘ 
patients
˘˘ "
.
˘˘" #
FirstOrDefault
˘˘# 1
(
˘˘1 2
p
˘˘2 3
=>
˘˘4 6
p
˘˘7 8
.
˘˘8 9
UserId
˘˘9 ?
==
˘˘@ B
userId
˘˘C I
)
˘˘I J
;
˘˘J K
if
˚˚ 
(
˚˚ 
patient
˚˚ 
==
˚˚ 
null
˚˚ 
)
˚˚  
{
¸¸ 
throw
˝˝ 
new
˝˝ 
NotFoundException
˝˝ +
(
˝˝+ ,
$str
˝˝, H
)
˝˝H I
;
˝˝I J
}
˛˛ 
return
ÄÄ 
_mapper
ÄÄ 
.
ÄÄ 
Map
ÄÄ 
<
ÄÄ 

PatientDto
ÄÄ )
>
ÄÄ) *
(
ÄÄ* +
patient
ÄÄ+ 2
)
ÄÄ2 3
;
ÄÄ3 4
}
ÅÅ 	
}
ÇÇ 
}ÉÉ ú7
~C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\HealthRecordService.cs
	namespace

 	

HealthAxis


 
.

 
API

 
.

 
Services

 !
.

! "
Implementations

" 1
{ 
public 

class 
HealthRecordService $
:% & 
IHealthRecordService' ;
{ 
private 
readonly #
IHealthRecordRepository 0#
_healthRecordRepository1 H
;H I
private 
readonly 
IRepository $
<$ %
Appointment% 0
>0 1"
_appointmentRepository2 H
;H I
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_patientRepository. @
;@ A
private 
readonly 
IMapper  
_mapper! (
;( )
public 
HealthRecordService "
(" ##
IHealthRecordRepository #"
healthRecordRepository$ :
,: ;
IRepository 
< 
Appointment #
># $!
appointmentRepository% :
,: ;
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
IMapper 
mapper 
) 
{ 	#
_healthRecordRepository #
=$ %"
healthRecordRepository& <
;< ="
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
_patientRepository 
=  
patientRepository! 2
;2 3
_mapper 
= 
mapper 
; 
} 	
public   
async   
Task   
<   
IEnumerable   %
<  % &
HealthRecordDto  & 5
>  5 6
>  6 7
GetByPatientIdAsync  8 K
(  K L
int  L O
	patientId  P Y
)  Y Z
{!! 	
var"" 
patient"" 
="" 
await"" 
_patientRepository""  2
.""2 3
GetByIdAsync""3 ?
(""? @
	patientId""@ I
)""I J
;""J K
if$$ 
($$ 
patient$$ 
==$$ 
null$$ 
)$$  
{%% 
throw&& 
new&& 
NotFoundException&& +
(&&+ ,
$str&&, @
)&&@ A
;&&A B
}'' 
var)) 
records)) 
=)) 
await)) #
_healthRecordRepository))  7
.))7 8
GetByPatientIdAsync))8 K
())K L
	patientId))L U
)))U V
;))V W
return++ 
_mapper++ 
.++ 
Map++ 
<++ 
IEnumerable++ *
<++* +
HealthRecordDto+++ :
>++: ;
>++; <
(++< =
records++= D
)++D E
;++E F
},, 	
public// 
async// 
Task// 
<// 
HealthRecordDto// )
>//) *
GetByIdAsync//+ 7
(//7 8
int//8 ;
id//< >
)//> ?
{00 	
var11 
healthRecord11 
=11 
await11 $#
_healthRecordRepository11% <
.11< =
GetByIdAsync11= I
(11I J
id11J L
)11L M
;11M N
if33 
(33 
healthRecord33 
==33 
null33  $
)33$ %
{44 
throw55 
new55 
NotFoundException55 +
(55+ ,
$str55, F
)55F G
;55G H
}66 
return88 
_mapper88 
.88 
Map88 
<88 
HealthRecordDto88 .
>88. /
(88/ 0
healthRecord880 <
)88< =
;88= >
}99 	
public<< 
async<< 
Task<< 
<<< 
HealthRecordDto<< )
><<) *
AddAsync<<+ 3
(<<3 4!
CreateHealthRecordDto<<4 I
dto<<J M
)<<M N
{== 	
var>> 
appointment>> 
=>> 
await>> #"
_appointmentRepository>>$ :
.>>: ;
GetByIdAsync>>; G
(>>G H
dto>>H K
.>>K L
AppointmentId>>L Y
)>>Y Z
;>>Z [
if@@ 
(@@ 
appointment@@ 
==@@ 
null@@ #
)@@# $
{AA 
throwBB 
newBB 
NotFoundExceptionBB +
(BB+ ,
$strBB, D
)BBD E
;BBE F
}CC 
ifEE 
(EE 
appointmentEE 
.EE 
StatusEE "
!=EE# %
AppointmentStatusEE& 7
.EE7 8
	CompletedEE8 A
)EEA B
{FF 
throwGG 
newGG %
CustomValidationExceptionGG 3
(GG3 4
$strHH T
)HHT U
;HHU V
}II 
varKK 
existingRecordsKK 
=KK  !
awaitLL #
_healthRecordRepositoryLL -
.LL- .#
GetByAppointmentIdAsyncLL. E
(LLE F
dtoLLF I
.LLI J
AppointmentIdLLJ W
)LLW X
;LLX Y
ifNN 
(NN 
existingRecordsNN 
.NN  
AnyNN  #
(NN# $
)NN$ %
)NN% &
{OO 
throwPP 
newPP %
CustomValidationExceptionPP 3
(PP3 4
$strQQ H
)QQH I
;QQI J
}RR 
varTT 
healthRecordTT 
=TT 
newTT "
HealthRecordTT# /
{UU 
AppointmentIdVV 
=VV 
dtoVV  #
.VV# $
AppointmentIdVV$ 1
,VV1 2
DoctorIdWW 
=WW 
appointmentWW &
.WW& '
DoctorIdWW' /
,WW/ 0
	PatientIdXX 
=XX 
appointmentXX '
.XX' (
	PatientIdXX( 1
,XX1 2
	VisitDateYY 
=YY 
appointmentYY '
.YY' (
ScheduledDateYY( 5
,YY5 6
	DiagnosisZZ 
=ZZ 
dtoZZ 
.ZZ  
	DiagnosisZZ  )
,ZZ) *
Prescription[[ 
=[[ 
dto[[ "
.[[" #
Prescription[[# /
,[[/ 0
Notes\\ 
=\\ 
dto\\ 
.\\ 
Notes\\ !
}]] 
;]] 
await__ #
_healthRecordRepository__ )
.__) *
AddAsync__* 2
(__2 3
healthRecord__3 ?
)__? @
;__@ A
returnaa 
_mapperaa 
.aa 
Mapaa 
<aa 
HealthRecordDtoaa .
>aa. /
(aa/ 0
healthRecordaa0 <
)aa< =
;aa= >
}bb 	
}cc 
}dd åã
xC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\DoctorService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
DoctorService 
:  
IDoctorService! /
{ 
private 
readonly 
HealthAxisDbContext ,
?, -
_context. 6
;6 7
private 
readonly 
IDoctorRepository *
?* +
_doctorRepository, =
;= >
private 
readonly 
UserManager $
<$ %
ApplicationUser% 4
>4 5
?5 6
_userManager7 C
;C D
private 
readonly 
IMapper  
?  !
_mapper" )
;) *
private 
readonly 
IDistributedCache *
?* +
_cache, 2
;2 3
private 
readonly 
ILogger  
<  !
DoctorService! .
>. /
?/ 0
_logger1 8
;8 9
private 
static 
readonly (
DistributedCacheEntryOptions  <$
AvailabilityCacheOptions= U
=V W
newX [
([ \
)\ ]
{ 	+
AbsoluteExpirationRelativeToNow +
=, -
TimeSpan. 6
.6 7
FromMinutes7 B
(B C
$numC D
)D E
}   	
;  	 

private"" 
static"" 
readonly"" 
List""  $
<""$ %
string""% +
>""+ ,
AllSlots""- 5
=""6 7
new""8 ;
(""; <
)""< =
{## 	
$str$$ 
,$$ 
$str%% 
,%% 
$str&& 
,&& 
$str'' 
,'' 
$str(( 
,(( 
$str)) 
,)) 
$str** 
,** 
$str++ 
,++ 
$str,, 
,,, 
$str-- 
,-- 
$str.. 
,.. 
$str// 
}00 	
;00	 

[22 	)
ActivatorUtilitiesConstructor22	 &
]22& '
public33 
DoctorService33 
(33 
HealthAxisDbContext44 
context44  '
,44' (
UserManager55 
<55 
ApplicationUser55 '
>55' (
userManager55) 4
,554 5
IDistributedCache66 
cache66 #
,66# $
ILogger77 
<77 
DoctorService77 !
>77! "
logger77# )
)77) *
{88 	
_context99 
=99 
context99 
;99 
_userManager:: 
=:: 
userManager:: &
;::& '
_cache;; 
=;; 
cache;; 
;;; 
_logger<< 
=<< 
logger<< 
;<< 
}== 	
public?? 
DoctorService?? 
(?? 
HealthAxisDbContext?? 0
context??1 8
)??8 9
{@@ 	
_contextAA 
=AA 
contextAA 
;AA 
}BB 	
publicDD 
DoctorServiceDD 
(DD 
IDoctorRepositoryEE 
doctorRepositoryEE .
,EE. /
UserManagerFF 
<FF 
ApplicationUserFF '
>FF' (
userManagerFF) 4
,FF4 5
IMapperGG 
mapperGG 
)GG 
{HH 	
_doctorRepositoryII 
=II 
doctorRepositoryII  0
;II0 1
_userManagerJJ 
=JJ 
userManagerJJ &
;JJ& '
_mapperKK 
=KK 
mapperKK 
;KK 
}LL 	
publicNN 
asyncNN 
TaskNN 
<NN 
IEnumerableNN %
<NN% &
	DoctorDtoNN& /
>NN/ 0
>NN0 1
GetAllAsyncNN2 =
(NN= >
)NN> ?
{OO 	
ifPP 
(PP 
_doctorRepositoryPP !
!=PP" $
nullPP% )
&&PP* ,
_mapperPP- 4
!=PP5 7
nullPP8 <
)PP< =
{QQ 
varRR 
doctorsRR 
=RR 
awaitRR #
_doctorRepositoryRR$ 5
.RR5 6
GetAllAsyncRR6 A
(RRA B
)RRB C
;RRC D
returnSS 
_mapperSS 
.SS 
MapSS "
<SS" #
IEnumerableSS# .
<SS. /
	DoctorDtoSS/ 8
>SS8 9
>SS9 :
(SS: ;
doctorsSS; B
)SSB C
;SSC D
}TT 
ifVV 
(VV 
_contextVV 
==VV 
nullVV  
)VV  !
{WW 
returnXX 

EnumerableXX !
.XX! "
EmptyXX" '
<XX' (
	DoctorDtoXX( 1
>XX1 2
(XX2 3
)XX3 4
;XX4 5
}YY 
var[[ 
	dbDoctors[[ 
=[[ 
await[[ !
_context[[" *
.[[* +
Doctors[[+ 2
.\\ 
Include\\ 
(\\ 
d\\ 
=>\\ 
d\\ 
.\\  
Appointments\\  ,
)\\, -
.]] 
AsNoTracking]] 
(]] 
)]] 
.^^ 
ToListAsync^^ 
(^^ 
)^^ 
;^^ 
return`` 
	dbDoctors`` 
.`` 
Select`` #
(``# $
MapToDto``$ ,
)``, -
;``- .
}aa 	
publiccc 
asynccc 
Taskcc 
<cc 
PagedResponsecc '
<cc' (
	DoctorDtocc( 1
>cc1 2
>cc2 3
GetAllAsynccc4 ?
(cc? @
PaginationParamsdd 
paginationParamsdd -
,dd- .
CancellationTokenee 
ctee  
=ee! "
defaultee# *
)ee* +
{ff 	
vargg 

pageNumbergg 
=gg 
paginationParamsgg -
.gg- .

PageNumbergg. 8
<=gg9 ;
$numgg< =
?hh 
$numhh 
:ii 
paginationParamsii "
.ii" #

PageNumberii# -
;ii- .
varkk 
pageSizekk 
=kk 
paginationParamskk +
.kk+ ,
PageSizekk, 4
<=kk5 7
$numkk8 9
?ll 
$numll 
:mm 
paginationParamsmm "
.mm" #
PageSizemm# +
;mm+ ,
ifoo 
(oo 
_contextoo 
==oo 
nulloo  
)oo  !
{pp 
varqq 
doctorsqq 
=qq 
awaitqq #
GetAllAsyncqq$ /
(qq/ 0
)qq0 1
;qq1 2
varss 
totalRecordsss  
=ss! "
doctorsss# *
.ss* +
Countss+ 0
(ss0 1
)ss1 2
;ss2 3
varuu 
pagedDoctorsuu  
=uu! "
doctorsuu# *
.vv 
Skipvv 
(vv 
(vv 

pageNumbervv %
-vv& '
$numvv( )
)vv) *
*vv+ ,
pageSizevv- 5
)vv5 6
.ww 
Takeww 
(ww 
pageSizeww "
)ww" #
.xx 
ToListxx 
(xx 
)xx 
;xx 
returnzz 
newzz 
PagedResponsezz (
<zz( )
	DoctorDtozz) 2
>zz2 3
{{{ 
Items|| 
=|| 
pagedDoctors|| (
,||( )

PageNumber}} 
=}}  

pageNumber}}! +
,}}+ ,
PageSize~~ 
=~~ 
pageSize~~ '
,~~' (
TotalRecords  
=! "
totalRecords# /
,/ 0

TotalPages
ÄÄ 
=
ÄÄ  
(
ÄÄ! "
int
ÄÄ" %
)
ÄÄ% &
Math
ÄÄ& *
.
ÄÄ* +
Ceiling
ÄÄ+ 2
(
ÄÄ2 3
totalRecords
ÄÄ3 ?
/
ÄÄ@ A
(
ÄÄB C
double
ÄÄC I
)
ÄÄI J
pageSize
ÄÄJ R
)
ÄÄR S
}
ÅÅ 
;
ÅÅ 
}
ÇÇ 
var
ÑÑ 
query
ÑÑ 
=
ÑÑ 
_context
ÑÑ  
.
ÑÑ  !
Doctors
ÑÑ! (
.
ÖÖ 
Include
ÖÖ 
(
ÖÖ 
d
ÖÖ 
=>
ÖÖ 
d
ÖÖ 
.
ÖÖ  
Appointments
ÖÖ  ,
)
ÖÖ, -
.
ÜÜ 
AsNoTracking
ÜÜ 
(
ÜÜ 
)
ÜÜ 
.
áá 
AsQueryable
áá 
(
áá 
)
áá 
;
áá 
var
ââ 
total
ââ 
=
ââ 
await
ââ 
query
ââ #
.
ââ# $

CountAsync
ââ$ .
(
ââ. /
ct
ââ/ 1
)
ââ1 2
;
ââ2 3
var
ãã 
doctorsList
ãã 
=
ãã 
await
ãã #
query
ãã$ )
.
åå 
OrderBy
åå 
(
åå 
d
åå 
=>
åå 
d
åå 
.
åå  
DoctorId
åå  (
)
åå( )
.
çç 
Skip
çç 
(
çç 
(
çç 

pageNumber
çç !
-
çç" #
$num
çç$ %
)
çç% &
*
çç' (
pageSize
çç) 1
)
çç1 2
.
éé 
Take
éé 
(
éé 
pageSize
éé 
)
éé 
.
èè 
Select
èè 
(
èè 
d
èè 
=>
èè 
new
èè  
	DoctorDto
èè! *
{
êê 
DoctorId
ëë 
=
ëë 
d
ëë  
.
ëë  !
DoctorId
ëë! )
,
ëë) *
FullName
íí 
=
íí 
d
íí  
.
íí  !
FullName
íí! )
,
íí) *
Specialisation
ìì "
=
ìì# $
d
ìì% &
.
ìì& '
Specialisation
ìì' 5
,
ìì5 6
YearsOfExperience
îî %
=
îî& '
d
îî( )
.
îî) *
YearsOfExperience
îî* ;
,
îî; <
ConsultationFee
ïï #
=
ïï$ %
d
ïï& '
.
ïï' (
ConsultationFee
ïï( 7
,
ïï7 8
IsActive
ññ 
=
ññ 
d
ññ  
.
ññ  !
IsActive
ññ! )
,
ññ) *&
UpcomingAppointmentCount
óó ,
=
óó- .
d
óó/ 0
.
óó0 1
Appointments
óó1 =
.
óó= >
Count
óó> C
(
óóC D
a
óóD E
=>
óóF H
a
òò 
.
òò 
ScheduledDate
òò '
.
òò' (
Date
òò( ,
>=
òò- /
DateTime
òò0 8
.
òò8 9
Today
òò9 >
&&
òò? A
a
ôô 
.
ôô 
Status
ôô  
!=
ôô! #
AppointmentStatus
ôô$ 5
.
ôô5 6
	Cancelled
ôô6 ?
)
ôô? @
}
öö 
)
öö 
.
õõ 
ToListAsync
õõ 
(
õõ 
ct
õõ 
)
õõ  
;
õõ  !
return
ùù 
new
ùù 
PagedResponse
ùù $
<
ùù$ %
	DoctorDto
ùù% .
>
ùù. /
{
ûû 
Items
üü 
=
üü 
doctorsList
üü #
,
üü# $

PageNumber
†† 
=
†† 

pageNumber
†† '
,
††' (
PageSize
°° 
=
°° 
pageSize
°° #
,
°°# $
TotalRecords
¢¢ 
=
¢¢ 
total
¢¢ $
,
¢¢$ %

TotalPages
££ 
=
££ 
(
££ 
int
££ !
)
££! "
Math
££" &
.
££& '
Ceiling
££' .
(
££. /
total
££/ 4
/
££5 6
(
££7 8
double
££8 >
)
££> ?
pageSize
££? G
)
££G H
}
§§ 
;
§§ 
}
•• 	
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
	DoctorDto
ßß #
?
ßß# $
>
ßß$ %
GetByIdAsync
ßß& 2
(
ßß2 3
int
®® 
id
®® 
,
®® 
CancellationToken
©© 
ct
©©  
=
©©! "
default
©©# *
)
©©* +
{
™™ 	
if
´´ 
(
´´ 
_doctorRepository
´´ !
!=
´´" $
null
´´% )
&&
´´* ,
_mapper
´´- 4
!=
´´5 7
null
´´8 <
)
´´< =
{
¨¨ 
var
≠≠ 
doctorFromRepo
≠≠ "
=
≠≠# $
await
≠≠% *
_doctorRepository
≠≠+ <
.
≠≠< =
GetByIdAsync
≠≠= I
(
≠≠I J
id
≠≠J L
)
≠≠L M
;
≠≠M N
if
ØØ 
(
ØØ 
doctorFromRepo
ØØ "
==
ØØ# %
null
ØØ& *
)
ØØ* +
{
∞∞ 
throw
±± 
new
±± 
NotFoundException
±± /
(
±±/ 0
$str
±±0 B
)
±±B C
;
±±C D
}
≤≤ 
return
¥¥ 
_mapper
¥¥ 
.
¥¥ 
Map
¥¥ "
<
¥¥" #
	DoctorDto
¥¥# ,
>
¥¥, -
(
¥¥- .
doctorFromRepo
¥¥. <
)
¥¥< =
;
¥¥= >
}
µµ 
if
∑∑ 
(
∑∑ 
_context
∑∑ 
==
∑∑ 
null
∑∑  
)
∑∑  !
{
∏∏ 
throw
ππ 
new
ππ 
NotFoundException
ππ +
(
ππ+ ,
$str
ππ, >
)
ππ> ?
;
ππ? @
}
∫∫ 
var
ºº 
doctor
ºº 
=
ºº 
await
ºº 
_context
ºº '
.
ºº' (
Doctors
ºº( /
.
ΩΩ 
Include
ΩΩ 
(
ΩΩ 
d
ΩΩ 
=>
ΩΩ 
d
ΩΩ 
.
ΩΩ  
Appointments
ΩΩ  ,
)
ΩΩ, -
.
ææ 
AsNoTracking
ææ 
(
ææ 
)
ææ 
.
øø !
FirstOrDefaultAsync
øø $
(
øø$ %
d
øø% &
=>
øø' )
d
øø* +
.
øø+ ,
DoctorId
øø, 4
==
øø5 7
id
øø8 :
,
øø: ;
ct
øø< >
)
øø> ?
;
øø? @
if
¡¡ 
(
¡¡ 
doctor
¡¡ 
==
¡¡ 
null
¡¡ 
)
¡¡ 
{
¬¬ 
throw
√√ 
new
√√ 
NotFoundException
√√ +
(
√√+ ,
$str
√√, >
)
√√> ?
;
√√? @
}
ƒƒ 
return
∆∆ 
MapToDto
∆∆ 
(
∆∆ 
doctor
∆∆ "
)
∆∆" #
;
∆∆# $
}
«« 	
public
…… 
async
…… 
Task
…… 
<
…… 
	DoctorDto
…… #
?
……# $
>
……$ %
GetByUserIdAsync
……& 6
(
……6 7
string
   
userId
   
,
   
CancellationToken
ÀÀ 
ct
ÀÀ  
=
ÀÀ! "
default
ÀÀ# *
)
ÀÀ* +
{
ÃÃ 	
if
ÕÕ 
(
ÕÕ 
_context
ÕÕ 
!=
ÕÕ 
null
ÕÕ  
)
ÕÕ  !
{
ŒŒ 
var
œœ 
doctor
œœ 
=
œœ 
await
œœ "
_context
œœ# +
.
œœ+ ,
Doctors
œœ, 3
.
–– 
Include
–– 
(
–– 
d
–– 
=>
–– !
d
––" #
.
––# $
Appointments
––$ 0
)
––0 1
.
—— 
AsNoTracking
—— !
(
——! "
)
——" #
.
““ !
FirstOrDefaultAsync
““ (
(
““( )
d
““) *
=>
““+ -
d
““. /
.
““/ 0
UserId
““0 6
==
““7 9
userId
““: @
,
““@ A
ct
““B D
)
““D E
;
““E F
if
‘‘ 
(
‘‘ 
doctor
‘‘ 
==
‘‘ 
null
‘‘ "
)
‘‘" #
{
’’ 
throw
÷÷ 
new
÷÷ 
NotFoundException
÷÷ /
(
÷÷/ 0
$str
÷÷0 K
)
÷÷K L
;
÷÷L M
}
◊◊ 
return
ŸŸ 
MapToDto
ŸŸ 
(
ŸŸ  
doctor
ŸŸ  &
)
ŸŸ& '
;
ŸŸ' (
}
⁄⁄ 
if
‹‹ 
(
‹‹ 
_doctorRepository
‹‹ !
!=
‹‹" $
null
‹‹% )
&&
‹‹* ,
_mapper
‹‹- 4
!=
‹‹5 7
null
‹‹8 <
)
‹‹< =
{
›› 
var
ﬁﬁ 
doctors
ﬁﬁ 
=
ﬁﬁ 
await
ﬁﬁ #
_doctorRepository
ﬁﬁ$ 5
.
ﬁﬁ5 6
GetAllAsync
ﬁﬁ6 A
(
ﬁﬁA B
)
ﬁﬁB C
;
ﬁﬁC D
var
‡‡ 
doctor
‡‡ 
=
‡‡ 
doctors
‡‡ $
.
‡‡$ %
FirstOrDefault
‡‡% 3
(
‡‡3 4
d
‡‡4 5
=>
‡‡6 8
d
‡‡9 :
.
‡‡: ;
UserId
‡‡; A
==
‡‡B D
userId
‡‡E K
)
‡‡K L
;
‡‡L M
if
‚‚ 
(
‚‚ 
doctor
‚‚ 
==
‚‚ 
null
‚‚ "
)
‚‚" #
{
„„ 
throw
‰‰ 
new
‰‰ 
NotFoundException
‰‰ /
(
‰‰/ 0
$str
‰‰0 K
)
‰‰K L
;
‰‰L M
}
ÂÂ 
return
ÁÁ 
_mapper
ÁÁ 
.
ÁÁ 
Map
ÁÁ "
<
ÁÁ" #
	DoctorDto
ÁÁ# ,
>
ÁÁ, -
(
ÁÁ- .
doctor
ÁÁ. 4
)
ÁÁ4 5
;
ÁÁ5 6
}
ËË 
throw
ÍÍ 
new
ÍÍ 
NotFoundException
ÍÍ '
(
ÍÍ' (
$str
ÍÍ( C
)
ÍÍC D
;
ÍÍD E
}
ÎÎ 	
public
ÌÌ 
async
ÌÌ 
Task
ÌÌ 
<
ÌÌ 
object
ÌÌ  
>
ÌÌ  !"
GetAvailabilityAsync
ÌÌ" 6
(
ÌÌ6 7
int
ÓÓ 
id
ÓÓ 
,
ÓÓ 
CancellationToken
ÔÔ 
ct
ÔÔ  
=
ÔÔ! "
default
ÔÔ# *
)
ÔÔ* +
{
 	
return
ÒÒ 
await
ÒÒ "
GetAvailabilityAsync
ÒÒ -
(
ÒÒ- .
id
ÒÒ. 0
,
ÒÒ0 1
DateTime
ÒÒ2 :
.
ÒÒ: ;
Today
ÒÒ; @
,
ÒÒ@ A
ct
ÒÒB D
)
ÒÒD E
;
ÒÒE F
}
ÚÚ 	
public
ÙÙ 
async
ÙÙ 
Task
ÙÙ 
<
ÙÙ #
DoctorAvailabilityDto
ÙÙ /
>
ÙÙ/ 0"
GetAvailabilityAsync
ÙÙ1 E
(
ÙÙE F
int
ıı 
id
ıı 
,
ıı 
DateTime
ˆˆ 
date
ˆˆ 
,
ˆˆ 
CancellationToken
˜˜ 
ct
˜˜  
=
˜˜! "
default
˜˜# *
)
˜˜* +
{
¯¯ 	
var
˘˘ 
availabilityDate
˘˘  
=
˘˘! "
date
˘˘# '
.
˘˘' (
Date
˘˘( ,
;
˘˘, -
var
˙˙ 
cacheKey
˙˙ 
=
˙˙ 
$"
˙˙ 
$str
˙˙ %
{
˙˙% &
id
˙˙& (
}
˙˙( )
$str
˙˙) 7
{
˙˙7 8
availabilityDate
˙˙8 H
:
˙˙H I
$str
˙˙I S
}
˙˙S T
"
˙˙T U
;
˙˙U V
if
¸¸ 
(
¸¸ 
_cache
¸¸ 
!=
¸¸ 
null
¸¸ 
)
¸¸ 
{
˝˝ 
var
˛˛  
cachedAvailability
˛˛ &
=
˛˛' (
await
˛˛) .
_cache
˛˛/ 5
.
˛˛5 6
GetStringAsync
˛˛6 D
(
˛˛D E
cacheKey
˛˛E M
,
˛˛M N
ct
˛˛O Q
)
˛˛Q R
;
˛˛R S
if
ÄÄ 
(
ÄÄ 
!
ÄÄ 
string
ÄÄ 
.
ÄÄ  
IsNullOrWhiteSpace
ÄÄ .
(
ÄÄ. / 
cachedAvailability
ÄÄ/ A
)
ÄÄA B
)
ÄÄB C
{
ÅÅ 
var
ÇÇ 
	cachedDto
ÇÇ !
=
ÇÇ" #
JsonSerializer
ÇÇ$ 2
.
ÇÇ2 3
Deserialize
ÇÇ3 >
<
ÇÇ> ?#
DoctorAvailabilityDto
ÇÇ? T
>
ÇÇT U
(
ÇÇU V 
cachedAvailability
ÇÇV h
)
ÇÇh i
;
ÇÇi j
if
ÑÑ 
(
ÑÑ 
	cachedDto
ÑÑ !
!=
ÑÑ" $
null
ÑÑ% )
)
ÑÑ) *
{
ÖÖ 
_logger
ÜÜ 
?
ÜÜ  
.
ÜÜ  !
LogInformation
ÜÜ! /
(
ÜÜ/ 0
$str
áá /
,
áá/ 0"
BuildCacheHitMessage
àà 0
(
àà0 1
id
àà1 3
,
àà3 4
availabilityDate
àà5 E
,
ààE F
cacheKey
ààG O
)
ààO P
)
ààP Q
;
ààQ R
return
ää 
	cachedDto
ää (
;
ää( )
}
ãã 
}
åå 
_logger
éé 
?
éé 
.
éé 
LogInformation
éé '
(
éé' (
$str
èè (
,
èè( )#
BuildCacheMissMessage
êê )
(
êê) *
id
êê* ,
,
êê, -
availabilityDate
êê. >
,
êê> ?
cacheKey
êê@ H
)
êêH I
)
êêI J
;
êêJ K
}
ëë 
Doctor
ìì 
?
ìì 
doctor
ìì 
;
ìì 
if
ïï 
(
ïï 
_doctorRepository
ïï !
!=
ïï" $
null
ïï% )
)
ïï) *
{
ññ 
doctor
óó 
=
óó 
await
óó 
_doctorRepository
óó 0
.
óó0 1
GetByIdAsync
óó1 =
(
óó= >
id
óó> @
)
óó@ A
;
óóA B
}
òò 
else
ôô 
if
ôô 
(
ôô 
_context
ôô 
!=
ôô  
null
ôô! %
)
ôô% &
{
öö 
doctor
õõ 
=
õõ 
await
õõ 
_context
õõ '
.
õõ' (
Doctors
õõ( /
.
úú 
AsNoTracking
úú !
(
úú! "
)
úú" #
.
ùù !
FirstOrDefaultAsync
ùù (
(
ùù( )
d
ùù) *
=>
ùù+ -
d
ùù. /
.
ùù/ 0
DoctorId
ùù0 8
==
ùù9 ;
id
ùù< >
,
ùù> ?
ct
ùù@ B
)
ùùB C
;
ùùC D
}
ûû 
else
üü 
{
†† 
doctor
°° 
=
°° 
null
°° 
;
°° 
}
¢¢ 
if
§§ 
(
§§ 
doctor
§§ 
==
§§ 
null
§§ 
)
§§ 
{
•• 
throw
¶¶ 
new
¶¶ 
NotFoundException
¶¶ +
(
¶¶+ ,
$str
¶¶, >
)
¶¶> ?
;
¶¶? @
}
ßß 
var
©© 
bookedSlots
©© 
=
©© 
new
©© !
List
©©" &
<
©©& '
string
©©' -
>
©©- .
(
©©. /
)
©©/ 0
;
©©0 1
if
´´ 
(
´´ 
_context
´´ 
!=
´´ 
null
´´  
)
´´  !
{
¨¨ 
bookedSlots
≠≠ 
=
≠≠ 
await
≠≠ #
_context
≠≠$ ,
.
≠≠, -
Appointments
≠≠- 9
.
ÆÆ 
AsNoTracking
ÆÆ !
(
ÆÆ! "
)
ÆÆ" #
.
ØØ 
Where
ØØ 
(
ØØ 
a
ØØ 
=>
ØØ 
a
∞∞ 
.
∞∞ 
DoctorId
∞∞ "
==
∞∞# %
id
∞∞& (
&&
∞∞) +
a
±± 
.
±± 
ScheduledDate
±± '
.
±±' (
Date
±±( ,
==
±±- /
availabilityDate
±±0 @
&&
±±A C
a
≤≤ 
.
≤≤ 
Status
≤≤  
!=
≤≤! #
AppointmentStatus
≤≤$ 5
.
≤≤5 6
	Cancelled
≤≤6 ?
)
≤≤? @
.
≥≥ 
Select
≥≥ 
(
≥≥ 
a
≥≥ 
=>
≥≥  
a
≥≥! "
.
≥≥" #
TimeSlot
≥≥# +
)
≥≥+ ,
.
¥¥ 
ToListAsync
¥¥  
(
¥¥  !
ct
¥¥! #
)
¥¥# $
;
¥¥$ %
}
µµ 
var
∑∑ 
availableSlots
∑∑ 
=
∑∑  
doctor
∑∑! '
.
∑∑' (
IsActive
∑∑( 0
?
∏∏ 
AllSlots
∏∏ 
.
∏∏ 
Except
∏∏ !
(
∏∏! "
bookedSlots
∏∏" -
)
∏∏- .
.
∏∏. /
ToList
∏∏/ 5
(
∏∏5 6
)
∏∏6 7
:
ππ 
new
ππ 
List
ππ 
<
ππ 
string
ππ !
>
ππ! "
(
ππ" #
)
ππ# $
;
ππ$ %
var
ªª 
availability
ªª 
=
ªª 
new
ªª "#
DoctorAvailabilityDto
ªª# 8
{
ºº 
DoctorId
ΩΩ 
=
ΩΩ 
doctor
ΩΩ !
.
ΩΩ! "
DoctorId
ΩΩ" *
,
ΩΩ* +
FullName
ææ 
=
ææ 
doctor
ææ !
.
ææ! "
FullName
ææ" *
,
ææ* +
IsActive
øø 
=
øø 
doctor
øø !
.
øø! "
IsActive
øø" *
,
øø* +
Date
¿¿ 
=
¿¿ 
availabilityDate
¿¿ '
,
¿¿' (
AvailableSlots
¡¡ 
=
¡¡  
availableSlots
¡¡! /
}
¬¬ 
;
¬¬ 
if
ƒƒ 
(
ƒƒ 
_cache
ƒƒ 
!=
ƒƒ 
null
ƒƒ 
)
ƒƒ 
{
≈≈ 
await
∆∆ 
_cache
∆∆ 
.
∆∆ 
SetStringAsync
∆∆ +
(
∆∆+ ,
cacheKey
«« 
,
«« 
JsonSerializer
»» "
.
»»" #
	Serialize
»»# ,
(
»», -
availability
»»- 9
)
»»9 :
,
»»: ;&
AvailabilityCacheOptions
…… ,
,
……, -
ct
   
)
   
;
   
_logger
ÃÃ 
?
ÃÃ 
.
ÃÃ 
LogInformation
ÃÃ '
(
ÃÃ' (
$str
ÕÕ *
,
ÕÕ* +%
BuildCacheStoredMessage
ŒŒ +
(
ŒŒ+ ,
id
œœ 
,
œœ 
availabilityDate
–– (
,
––( )
cacheKey
——  
,
——  !
availableSlots
““ &
.
““& '
Count
““' ,
)
““, -
)
““- .
;
““. /
}
”” 
return
’’ 
availability
’’ 
;
’’  
}
÷÷ 	
public
ÿÿ 
async
ÿÿ 
Task
ÿÿ 
<
ÿÿ 
	DoctorDto
ÿÿ #
>
ÿÿ# $
AddAsync
ÿÿ% -
(
ÿÿ- .
CreateDoctorDto
ŸŸ 
dto
ŸŸ 
,
ŸŸ  
CancellationToken
⁄⁄ 
ct
⁄⁄  
=
⁄⁄! "
default
⁄⁄# *
)
⁄⁄* +
{
€€ 	
if
‹‹ 
(
‹‹ 
_doctorRepository
‹‹ !
!=
‹‹" $
null
‹‹% )
&&
‹‹* ,
_userManager
‹‹- 9
!=
‹‹: <
null
‹‹= A
&&
‹‹B D
_mapper
‹‹E L
!=
‹‹M O
null
‹‹P T
)
‹‹T U
{
›› 
var
ﬁﬁ 
existingUser
ﬁﬁ  
=
ﬁﬁ! "
await
ﬁﬁ# (
_userManager
ﬁﬁ) 5
.
ﬁﬁ5 6
FindByEmailAsync
ﬁﬁ6 F
(
ﬁﬁF G
dto
ﬁﬁG J
.
ﬁﬁJ K
Email
ﬁﬁK P
)
ﬁﬁP Q
;
ﬁﬁQ R
if
‡‡ 
(
‡‡ 
existingUser
‡‡  
!=
‡‡! #
null
‡‡$ (
)
‡‡( )
{
·· 
throw
‚‚ 
new
‚‚ #
BusinessRuleException
‚‚ 3
(
‚‚3 4
$str
‚‚4 b
)
‚‚b c
;
‚‚c d
}
„„ 
var
ÂÂ 
user
ÂÂ 
=
ÂÂ 
new
ÂÂ 
ApplicationUser
ÂÂ .
{
ÊÊ 
UserName
ÁÁ 
=
ÁÁ 
dto
ÁÁ "
.
ÁÁ" #
Email
ÁÁ# (
,
ÁÁ( )
Email
ËË 
=
ËË 
dto
ËË 
.
ËË  
Email
ËË  %
,
ËË% & 
MustChangePassword
ÈÈ &
=
ÈÈ' (
true
ÈÈ) -
}
ÍÍ 
;
ÍÍ 
var
ÏÏ 
createResult
ÏÏ  
=
ÏÏ! "
await
ÏÏ# (
_userManager
ÏÏ) 5
.
ÏÏ5 6
CreateAsync
ÏÏ6 A
(
ÏÏA B
user
ÏÏB F
,
ÏÏF G
dto
ÏÏH K
.
ÏÏK L
TemporaryPassword
ÏÏL ]
)
ÏÏ] ^
;
ÏÏ^ _
if
ÓÓ 
(
ÓÓ 
!
ÓÓ 
createResult
ÓÓ !
.
ÓÓ! "
	Succeeded
ÓÓ" +
)
ÓÓ+ ,
{
ÔÔ 
var
 
errorMessage
 $
=
% &
createResult
' 3
.
3 4
Errors
4 :
.
: ;
FirstOrDefault
; I
(
I J
)
J K
?
K L
.
L M
Description
M X
??
ÒÒ 
$str
ÒÒ :
;
ÒÒ: ;
throw
ÛÛ 
new
ÛÛ !
ValidationException
ÛÛ 1
(
ÛÛ1 2
errorMessage
ÛÛ2 >
)
ÛÛ> ?
;
ÛÛ? @
}
ÙÙ 
var
ˆˆ 

roleResult
ˆˆ 
=
ˆˆ  
await
ˆˆ! &
_userManager
ˆˆ' 3
.
ˆˆ3 4
AddToRoleAsync
ˆˆ4 B
(
ˆˆB C
user
ˆˆC G
,
ˆˆG H
$str
ˆˆI Q
)
ˆˆQ R
;
ˆˆR S
if
¯¯ 
(
¯¯ 
!
¯¯ 

roleResult
¯¯ 
.
¯¯  
	Succeeded
¯¯  )
)
¯¯) *
{
˘˘ 
var
˙˙ 
errorMessage
˙˙ $
=
˙˙% &

roleResult
˙˙' 1
.
˙˙1 2
Errors
˙˙2 8
.
˙˙8 9
FirstOrDefault
˙˙9 G
(
˙˙G H
)
˙˙H I
?
˙˙I J
.
˙˙J K
Description
˙˙K V
??
˚˚ 
$str
˚˚ ;
;
˚˚; <
throw
˝˝ 
new
˝˝ !
ValidationException
˝˝ 1
(
˝˝1 2
errorMessage
˝˝2 >
)
˝˝> ?
;
˝˝? @
}
˛˛ 
var
ÄÄ 
doctor
ÄÄ 
=
ÄÄ 
new
ÄÄ  
Doctor
ÄÄ! '
{
ÅÅ 
UserId
ÇÇ 
=
ÇÇ 
user
ÇÇ !
.
ÇÇ! "
Id
ÇÇ" $
,
ÇÇ$ %
FullName
ÉÉ 
=
ÉÉ 
dto
ÉÉ "
.
ÉÉ" #
FullName
ÉÉ# +
,
ÉÉ+ ,
Specialisation
ÑÑ "
=
ÑÑ# $
dto
ÑÑ% (
.
ÑÑ( )
Specialisation
ÑÑ) 7
,
ÑÑ7 8
YearsOfExperience
ÖÖ %
=
ÖÖ& '
dto
ÖÖ( +
.
ÖÖ+ ,
YearsOfExperience
ÖÖ, =
,
ÖÖ= >
ConsultationFee
ÜÜ #
=
ÜÜ$ %
dto
ÜÜ& )
.
ÜÜ) *
ConsultationFee
ÜÜ* 9
,
ÜÜ9 :
IsActive
áá 
=
áá 
true
áá #
}
àà 
;
àà 
var
ää 
savedDoctor
ää 
=
ää  !
await
ää" '
_doctorRepository
ää( 9
.
ää9 :
AddAsync
ää: B
(
ääB C
doctor
ääC I
)
ääI J
;
ääJ K
return
åå 
_mapper
åå 
.
åå 
Map
åå "
<
åå" #
	DoctorDto
åå# ,
>
åå, -
(
åå- .
savedDoctor
åå. 9
)
åå9 :
;
åå: ;
}
çç 
if
èè 
(
èè 
_context
èè 
==
èè 
null
èè  
||
èè! #
_userManager
èè$ 0
==
èè1 3
null
èè4 8
)
èè8 9
{
êê 
throw
ëë 
new
ëë '
InvalidOperationException
ëë 3
(
ëë3 4
$str
ëë4 h
)
ëëh i
;
ëëi j
}
íí 
var
îî 
existingDbUser
îî 
=
îî  
await
îî! &
_userManager
îî' 3
.
îî3 4
FindByEmailAsync
îî4 D
(
îîD E
dto
îîE H
.
îîH I
Email
îîI N
)
îîN O
;
îîO P
if
ññ 
(
ññ 
existingDbUser
ññ 
!=
ññ !
null
ññ" &
)
ññ& '
{
óó 
throw
òò 
new
òò #
BusinessRuleException
òò /
(
òò/ 0
$str
òò0 ^
)
òò^ _
;
òò_ `
}
ôô 
var
õõ 
dbUser
õõ 
=
õõ 
new
õõ 
ApplicationUser
õõ ,
{
úú 
UserName
ùù 
=
ùù 
dto
ùù 
.
ùù 
Email
ùù $
,
ùù$ %
Email
ûû 
=
ûû 
dto
ûû 
.
ûû 
Email
ûû !
,
ûû! " 
MustChangePassword
üü "
=
üü# $
true
üü% )
}
†† 
;
†† 
var
¢¢ 
dbCreateResult
¢¢ 
=
¢¢  
await
¢¢! &
_userManager
¢¢' 3
.
¢¢3 4
CreateAsync
¢¢4 ?
(
¢¢? @
dbUser
¢¢@ F
,
¢¢F G
dto
¢¢H K
.
¢¢K L
TemporaryPassword
¢¢L ]
)
¢¢] ^
;
¢¢^ _
if
§§ 
(
§§ 
!
§§ 
dbCreateResult
§§ 
.
§§  
	Succeeded
§§  )
)
§§) *
{
•• 
var
¶¶ 
errorMessage
¶¶  
=
¶¶! "
dbCreateResult
¶¶# 1
.
¶¶1 2
Errors
¶¶2 8
.
¶¶8 9
FirstOrDefault
¶¶9 G
(
¶¶G H
)
¶¶H I
?
¶¶I J
.
¶¶J K
Description
¶¶K V
??
ßß 
$str
ßß 6
;
ßß6 7
throw
©© 
new
©© !
ValidationException
©© -
(
©©- .
errorMessage
©©. :
)
©©: ;
;
©©; <
}
™™ 
var
¨¨ 
dbRoleResult
¨¨ 
=
¨¨ 
await
¨¨ $
_userManager
¨¨% 1
.
¨¨1 2
AddToRoleAsync
¨¨2 @
(
¨¨@ A
dbUser
¨¨A G
,
¨¨G H
$str
¨¨I Q
)
¨¨Q R
;
¨¨R S
if
ÆÆ 
(
ÆÆ 
!
ÆÆ 
dbRoleResult
ÆÆ 
.
ÆÆ 
	Succeeded
ÆÆ '
)
ÆÆ' (
{
ØØ 
var
∞∞ 
errorMessage
∞∞  
=
∞∞! "
dbRoleResult
∞∞# /
.
∞∞/ 0
Errors
∞∞0 6
.
∞∞6 7
FirstOrDefault
∞∞7 E
(
∞∞E F
)
∞∞F G
?
∞∞G H
.
∞∞H I
Description
∞∞I T
??
±± 
$str
±± 7
;
±±7 8
throw
≥≥ 
new
≥≥ !
ValidationException
≥≥ -
(
≥≥- .
errorMessage
≥≥. :
)
≥≥: ;
;
≥≥; <
}
¥¥ 
var
∂∂ 
dbDoctor
∂∂ 
=
∂∂ 
new
∂∂ 
Doctor
∂∂ %
{
∑∑ 
UserId
∏∏ 
=
∏∏ 
dbUser
∏∏ 
.
∏∏  
Id
∏∏  "
,
∏∏" #
FullName
ππ 
=
ππ 
dto
ππ 
.
ππ 
FullName
ππ '
,
ππ' (
Specialisation
∫∫ 
=
∫∫  
dto
∫∫! $
.
∫∫$ %
Specialisation
∫∫% 3
,
∫∫3 4
YearsOfExperience
ªª !
=
ªª" #
dto
ªª$ '
.
ªª' (
YearsOfExperience
ªª( 9
,
ªª9 :
ConsultationFee
ºº 
=
ºº  !
dto
ºº" %
.
ºº% &
ConsultationFee
ºº& 5
,
ºº5 6
IsActive
ΩΩ 
=
ΩΩ 
true
ΩΩ 
}
ææ 
;
ææ 
_context
¿¿ 
.
¿¿ 
Doctors
¿¿ 
.
¿¿ 
Add
¿¿  
(
¿¿  !
dbDoctor
¿¿! )
)
¿¿) *
;
¿¿* +
await
¡¡ 
_context
¡¡ 
.
¡¡ 
SaveChangesAsync
¡¡ +
(
¡¡+ ,
ct
¡¡, .
)
¡¡. /
;
¡¡/ 0
return
√√ 
MapToDto
√√ 
(
√√ 
dbDoctor
√√ $
)
√√$ %
;
√√% &
}
ƒƒ 	
public
∆∆ 
async
∆∆ 
Task
∆∆ 
<
∆∆ 
	DoctorDto
∆∆ #
>
∆∆# $
UpdateAsync
∆∆% 0
(
∆∆0 1
int
«« 
id
«« 
,
«« 
UpdateDoctorDto
»» 
dto
»» 
,
»»  
CancellationToken
…… 
ct
……  
=
……! "
default
……# *
)
……* +
{
   	
if
ÀÀ 
(
ÀÀ 
_doctorRepository
ÀÀ !
!=
ÀÀ" $
null
ÀÀ% )
&&
ÀÀ* ,
_mapper
ÀÀ- 4
!=
ÀÀ5 7
null
ÀÀ8 <
)
ÀÀ< =
{
ÃÃ 
var
ÕÕ 
existingDoctor
ÕÕ "
=
ÕÕ# $
await
ÕÕ% *
_doctorRepository
ÕÕ+ <
.
ÕÕ< =
GetByIdAsync
ÕÕ= I
(
ÕÕI J
id
ÕÕJ L
)
ÕÕL M
;
ÕÕM N
if
œœ 
(
œœ 
existingDoctor
œœ "
==
œœ# %
null
œœ& *
)
œœ* +
{
–– 
throw
—— 
new
—— 
NotFoundException
—— /
(
——/ 0
$str
——0 B
)
——B C
;
——C D
}
““ 
_mapper
‘‘ 
.
‘‘ 
Map
‘‘ 
(
‘‘ 
dto
‘‘ 
,
‘‘  
existingDoctor
‘‘! /
)
‘‘/ 0
;
‘‘0 1
var
÷÷ 
updatedDoctor
÷÷ !
=
÷÷" #
await
÷÷$ )
_doctorRepository
÷÷* ;
.
÷÷; <
UpdateAsync
÷÷< G
(
÷÷G H
id
÷÷H J
,
÷÷J K
existingDoctor
÷÷L Z
,
÷÷Z [
ct
÷÷\ ^
)
÷÷^ _
;
÷÷_ `
await
ÿÿ *
RemoveAvailabilityCacheAsync
ÿÿ 2
(
ÿÿ2 3
id
ÿÿ3 5
,
ÿÿ5 6
DateTime
ÿÿ7 ?
.
ÿÿ? @
Today
ÿÿ@ E
,
ÿÿE F
ct
ÿÿG I
)
ÿÿI J
;
ÿÿJ K
return
⁄⁄ 
_mapper
⁄⁄ 
.
⁄⁄ 
Map
⁄⁄ "
<
⁄⁄" #
	DoctorDto
⁄⁄# ,
>
⁄⁄, -
(
⁄⁄- .
updatedDoctor
⁄⁄. ;
)
⁄⁄; <
;
⁄⁄< =
}
€€ 
if
›› 
(
›› 
_context
›› 
==
›› 
null
››  
)
››  !
{
ﬁﬁ 
throw
ﬂﬂ 
new
ﬂﬂ '
InvalidOperationException
ﬂﬂ 3
(
ﬂﬂ3 4
$str
ﬂﬂ4 X
)
ﬂﬂX Y
;
ﬂﬂY Z
}
‡‡ 
var
‚‚ 
doctor
‚‚ 
=
‚‚ 
await
‚‚ 
_context
‚‚ '
.
‚‚' (
Doctors
‚‚( /
.
„„ 
Include
„„ 
(
„„ 
d
„„ 
=>
„„ 
d
„„ 
.
„„  
Appointments
„„  ,
)
„„, -
.
‰‰ !
FirstOrDefaultAsync
‰‰ $
(
‰‰$ %
d
‰‰% &
=>
‰‰' )
d
‰‰* +
.
‰‰+ ,
DoctorId
‰‰, 4
==
‰‰5 7
id
‰‰8 :
,
‰‰: ;
ct
‰‰< >
)
‰‰> ?
;
‰‰? @
if
ÊÊ 
(
ÊÊ 
doctor
ÊÊ 
==
ÊÊ 
null
ÊÊ 
)
ÊÊ 
{
ÁÁ 
throw
ËË 
new
ËË 
NotFoundException
ËË +
(
ËË+ ,
$str
ËË, >
)
ËË> ?
;
ËË? @
}
ÈÈ 
doctor
ÎÎ 
.
ÎÎ 
FullName
ÎÎ 
=
ÎÎ 
dto
ÎÎ !
.
ÎÎ! "
FullName
ÎÎ" *
;
ÎÎ* +
doctor
ÏÏ 
.
ÏÏ 
Specialisation
ÏÏ !
=
ÏÏ" #
dto
ÏÏ$ '
.
ÏÏ' (
Specialisation
ÏÏ( 6
;
ÏÏ6 7
doctor
ÌÌ 
.
ÌÌ 
YearsOfExperience
ÌÌ $
=
ÌÌ% &
dto
ÌÌ' *
.
ÌÌ* +
YearsOfExperience
ÌÌ+ <
;
ÌÌ< =
doctor
ÓÓ 
.
ÓÓ 
ConsultationFee
ÓÓ "
=
ÓÓ# $
dto
ÓÓ% (
.
ÓÓ( )
ConsultationFee
ÓÓ) 8
;
ÓÓ8 9
doctor
ÔÔ 
.
ÔÔ 
IsActive
ÔÔ 
=
ÔÔ 
dto
ÔÔ !
.
ÔÔ! "
IsActive
ÔÔ" *
;
ÔÔ* +
await
ÒÒ 
_context
ÒÒ 
.
ÒÒ 
SaveChangesAsync
ÒÒ +
(
ÒÒ+ ,
ct
ÒÒ, .
)
ÒÒ. /
;
ÒÒ/ 0
await
ÛÛ *
RemoveAvailabilityCacheAsync
ÛÛ .
(
ÛÛ. /
id
ÛÛ/ 1
,
ÛÛ1 2
DateTime
ÛÛ3 ;
.
ÛÛ; <
Today
ÛÛ< A
,
ÛÛA B
ct
ÛÛC E
)
ÛÛE F
;
ÛÛF G
return
ıı 
MapToDto
ıı 
(
ıı 
doctor
ıı "
)
ıı" #
;
ıı# $
}
ˆˆ 	
private
¯¯ 
async
¯¯ 
Task
¯¯ *
RemoveAvailabilityCacheAsync
¯¯ 7
(
¯¯7 8
int
˘˘ 
doctorId
˘˘ 
,
˘˘ 
DateTime
˙˙ 
date
˙˙ 
,
˙˙ 
CancellationToken
˚˚ 
ct
˚˚  
)
˚˚  !
{
¸¸ 	
if
˝˝ 
(
˝˝ 
_cache
˝˝ 
==
˝˝ 
null
˝˝ 
)
˝˝ 
{
˛˛ 
return
ˇˇ 
;
ˇˇ 
}
ÄÄ 
var
ÇÇ 
cacheKey
ÇÇ 
=
ÇÇ 
$"
ÇÇ 
$str
ÇÇ %
{
ÇÇ% &
doctorId
ÇÇ& .
}
ÇÇ. /
$str
ÇÇ/ =
{
ÇÇ= >
date
ÇÇ> B
:
ÇÇB C
$str
ÇÇC M
}
ÇÇM N
"
ÇÇN O
;
ÇÇO P
await
ÑÑ 
_cache
ÑÑ 
.
ÑÑ 
RemoveAsync
ÑÑ $
(
ÑÑ$ %
cacheKey
ÑÑ% -
,
ÑÑ- .
ct
ÑÑ/ 1
)
ÑÑ1 2
;
ÑÑ2 3
_logger
ÜÜ 
?
ÜÜ 
.
ÜÜ 
LogInformation
ÜÜ #
(
ÜÜ# $
$str
áá '
,
áá' (&
BuildCacheRemovedMessage
àà (
(
àà( )
doctorId
àà) 1
,
àà1 2
date
àà3 7
,
àà7 8
cacheKey
àà9 A
)
ààA B
)
ààB C
;
ààC D
}
ââ 	
private
ãã 
static
ãã 
string
ãã "
BuildCacheHitMessage
ãã 2
(
ãã2 3
int
åå 
doctorId
åå 
,
åå 
DateTime
çç 
date
çç 
,
çç 
string
éé 
cacheKey
éé 
)
éé 
{
èè 	
return
êê 
Environment
ëë 
.
ëë 
NewLine
ëë #
+
ëë$ %
$str
íí :
+
íí; <
Environment
íí= H
.
ííH I
NewLine
ííI P
+
ííQ R
$str
ìì 1
+
ìì2 3
Environment
ìì4 ?
.
ìì? @
NewLine
ìì@ G
+
ììH I
$str
îî :
+
îî; <
Environment
îî= H
.
îîH I
NewLine
îîI P
+
îîQ R
$"
ïï 
$str
ïï 
{
ïï 
doctorId
ïï '
}
ïï' (
"
ïï( )
+
ïï* +
Environment
ïï, 7
.
ïï7 8
NewLine
ïï8 ?
+
ïï@ A
$"
ññ 
$str
ññ 
{
ññ 
date
ññ #
:
ññ# $
$str
ññ$ .
}
ññ. /
"
ññ/ 0
+
ññ1 2
Environment
ññ3 >
.
ññ> ?
NewLine
ññ? F
+
ññG H
$"
óó 
$str
óó 
{
óó 
cacheKey
óó '
}
óó' (
"
óó( )
+
óó* +
Environment
óó, 7
.
óó7 8
NewLine
óó8 ?
+
óó@ A
$str
òò *
+
òò+ ,
Environment
òò- 8
.
òò8 9
NewLine
òò9 @
+
òòA B
$str
ôô :
;
ôô: ;
}
öö 	
private
úú 
static
úú 
string
úú #
BuildCacheMissMessage
úú 3
(
úú3 4
int
ùù 
doctorId
ùù 
,
ùù 
DateTime
ûû 
date
ûû 
,
ûû 
string
üü 
cacheKey
üü 
)
üü 
{
†† 	
return
°° 
Environment
¢¢ 
.
¢¢ 
NewLine
¢¢ #
+
¢¢$ %
$str
££ :
+
££; <
Environment
££= H
.
££H I
NewLine
££I P
+
££Q R
$str
§§ 2
+
§§3 4
Environment
§§5 @
.
§§@ A
NewLine
§§A H
+
§§I J
$str
•• :
+
••; <
Environment
••= H
.
••H I
NewLine
••I P
+
••Q R
$"
¶¶ 
$str
¶¶ 
{
¶¶ 
doctorId
¶¶ '
}
¶¶' (
"
¶¶( )
+
¶¶* +
Environment
¶¶, 7
.
¶¶7 8
NewLine
¶¶8 ?
+
¶¶@ A
$"
ßß 
$str
ßß 
{
ßß 
date
ßß #
:
ßß# $
$str
ßß$ .
}
ßß. /
"
ßß/ 0
+
ßß1 2
Environment
ßß3 >
.
ßß> ?
NewLine
ßß? F
+
ßßG H
$"
®® 
$str
®® 
{
®® 
cacheKey
®® '
}
®®' (
"
®®( )
+
®®* +
Environment
®®, 7
.
®®7 8
NewLine
®®8 ?
+
®®@ A
$str
©© 1
+
©©2 3
Environment
©©4 ?
.
©©? @
NewLine
©©@ G
+
©©H I
$str
™™ :
;
™™: ;
}
´´ 	
private
≠≠ 
static
≠≠ 
string
≠≠ %
BuildCacheStoredMessage
≠≠ 5
(
≠≠5 6
int
ÆÆ 
doctorId
ÆÆ 
,
ÆÆ 
DateTime
ØØ 
date
ØØ 
,
ØØ 
string
∞∞ 
cacheKey
∞∞ 
,
∞∞ 
int
±± 
	slotCount
±± 
)
±± 
{
≤≤ 	
return
≥≥ 
Environment
¥¥ 
.
¥¥ 
NewLine
¥¥ #
+
¥¥$ %
$str
µµ :
+
µµ; <
Environment
µµ= H
.
µµH I
NewLine
µµI P
+
µµQ R
$str
∂∂ (
+
∂∂) *
Environment
∂∂+ 6
.
∂∂6 7
NewLine
∂∂7 >
+
∂∂? @
$str
∑∑ :
+
∑∑; <
Environment
∑∑= H
.
∑∑H I
NewLine
∑∑I P
+
∑∑Q R
$"
∏∏ 
$str
∏∏ 
{
∏∏ 
doctorId
∏∏ '
}
∏∏' (
"
∏∏( )
+
∏∏* +
Environment
∏∏, 7
.
∏∏7 8
NewLine
∏∏8 ?
+
∏∏@ A
$"
ππ 
$str
ππ 
{
ππ 
date
ππ #
:
ππ# $
$str
ππ$ .
}
ππ. /
"
ππ/ 0
+
ππ1 2
Environment
ππ3 >
.
ππ> ?
NewLine
ππ? F
+
ππG H
$"
∫∫ 
$str
∫∫ 
{
∫∫ 
cacheKey
∫∫ '
}
∫∫' (
"
∫∫( )
+
∫∫* +
Environment
∫∫, 7
.
∫∫7 8
NewLine
∫∫8 ?
+
∫∫@ A
$str
ªª '
+
ªª( )
Environment
ªª* 5
.
ªª5 6
NewLine
ªª6 =
+
ªª> ?
$"
ºº 
$str
ºº 
{
ºº 
	slotCount
ºº (
}
ºº( )
"
ºº) *
+
ºº+ ,
Environment
ºº- 8
.
ºº8 9
NewLine
ºº9 @
+
ººA B
$str
ΩΩ :
;
ΩΩ: ;
}
ææ 	
private
¿¿ 
static
¿¿ 
string
¿¿ &
BuildCacheRemovedMessage
¿¿ 6
(
¿¿6 7
int
¡¡ 
doctorId
¡¡ 
,
¡¡ 
DateTime
¬¬ 
date
¬¬ 
,
¬¬ 
string
√√ 
cacheKey
√√ 
)
√√ 
{
ƒƒ 	
return
≈≈ 
Environment
∆∆ 
.
∆∆ 
NewLine
∆∆ #
+
∆∆$ %
$str
«« :
+
««; <
Environment
««= H
.
««H I
NewLine
««I P
+
««Q R
$str
»» 5
+
»»6 7
Environment
»»8 C
.
»»C D
NewLine
»»D K
+
»»L M
$str
…… :
+
……; <
Environment
……= H
.
……H I
NewLine
……I P
+
……Q R
$"
   
$str
   
{
   
doctorId
   '
}
  ' (
"
  ( )
+
  * +
Environment
  , 7
.
  7 8
NewLine
  8 ?
+
  @ A
$"
ÀÀ 
$str
ÀÀ 
{
ÀÀ 
date
ÀÀ #
:
ÀÀ# $
$str
ÀÀ$ .
}
ÀÀ. /
"
ÀÀ/ 0
+
ÀÀ1 2
Environment
ÀÀ3 >
.
ÀÀ> ?
NewLine
ÀÀ? F
+
ÀÀG H
$"
ÃÃ 
$str
ÃÃ 
{
ÃÃ 
cacheKey
ÃÃ '
}
ÃÃ' (
"
ÃÃ( )
+
ÃÃ* +
Environment
ÃÃ, 7
.
ÃÃ7 8
NewLine
ÃÃ8 ?
+
ÃÃ@ A
$str
ÕÕ <
+
ÕÕ= >
Environment
ÕÕ? J
.
ÕÕJ K
NewLine
ÕÕK R
+
ÕÕS T
$str
ŒŒ :
;
ŒŒ: ;
}
œœ 	
private
—— 
static
—— 
	DoctorDto
——  
MapToDto
——! )
(
——) *
Doctor
——* 0
doctor
——1 7
)
——7 8
{
““ 	
return
”” 
new
”” 
	DoctorDto
””  
{
‘‘ 
DoctorId
’’ 
=
’’ 
doctor
’’ !
.
’’! "
DoctorId
’’" *
,
’’* +
FullName
÷÷ 
=
÷÷ 
doctor
÷÷ !
.
÷÷! "
FullName
÷÷" *
,
÷÷* +
Specialisation
◊◊ 
=
◊◊  
doctor
◊◊! '
.
◊◊' (
Specialisation
◊◊( 6
,
◊◊6 7
YearsOfExperience
ÿÿ !
=
ÿÿ" #
doctor
ÿÿ$ *
.
ÿÿ* +
YearsOfExperience
ÿÿ+ <
,
ÿÿ< =
ConsultationFee
ŸŸ 
=
ŸŸ  !
doctor
ŸŸ" (
.
ŸŸ( )
ConsultationFee
ŸŸ) 8
,
ŸŸ8 9
IsActive
⁄⁄ 
=
⁄⁄ 
doctor
⁄⁄ !
.
⁄⁄! "
IsActive
⁄⁄" *
,
⁄⁄* +&
UpcomingAppointmentCount
€€ (
=
€€) *
doctor
€€+ 1
.
€€1 2
Appointments
€€2 >
?
€€> ?
.
€€? @
Count
€€@ E
(
€€E F
a
€€F G
=>
€€H J
a
‹‹ 
.
‹‹ 
ScheduledDate
‹‹ #
.
‹‹# $
Date
‹‹$ (
>=
‹‹) +
DateTime
‹‹, 4
.
‹‹4 5
Today
‹‹5 :
&&
‹‹; =
a
›› 
.
›› 
Status
›› 
!=
›› 
AppointmentStatus
››  1
.
››1 2
	Cancelled
››2 ;
)
››; <
??
››= ?
$num
››@ A
}
ﬁﬁ 
;
ﬁﬁ 
}
ﬂﬂ 	
}
‡‡ 
}·· öº
vC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\AuthService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
UserManager $
<$ %
ApplicationUser% 4
>4 5
_userManager6 B
;B C
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_patientRepository. @
;@ A
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
private 
readonly 
IConfiguration '
_config( /
;/ 0
public 
AuthService 
( 
UserManager 
< 
ApplicationUser '
>' (
userManager) 4
,4 5
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
HealthAxisDbContext 
context  '
,' (
IConfiguration 
config !
)! "
{ 	
_userManager 
= 
userManager &
;& '
_patientRepository 
=  
patientRepository! 2
;2 3
_context   
=   
context   
;   
_config!! 
=!! 
config!! 
;!! 
}"" 	
public$$ 
async$$ 
Task$$ 
<$$ 
($$ 
bool$$ 
Success$$  '
,$$' (
string$$) /
Message$$0 7
,$$7 8
string$$9 ?
UserId$$@ F
)$$F G
>$$G H
Register$$I Q
($$Q R
RegisterDto$$R ]
request$$^ e
)$$e f
{%% 	
if&& 
(&& 
request&& 
.&& 
Password&&  
!=&&! #
request&&$ +
.&&+ ,
ConfirmPassword&&, ;
)&&; <
{'' 
return(( 
((( 
false(( 
,(( 
$str(( 7
,((7 8
string((9 ?
.((? @
Empty((@ E
)((E F
;((F G
})) 
var++ 
user++ 
=++ 
new++ 
ApplicationUser++ *
{,, 
UserName-- 
=-- 
request-- "
.--" #
Email--# (
,--( )
Email.. 
=.. 
request.. 
...  
Email..  %
,..% &
MustChangePassword// "
=//# $
false//% *
}00 
;00 
var22 
createUserResult22  
=22! "
await22# (
_userManager22) 5
.225 6
CreateAsync226 A
(22A B
user22B F
,22F G
request22H O
.22O P
Password22P X
)22X Y
;22Y Z
if44 
(44 
!44 
createUserResult44 !
.44! "
	Succeeded44" +
)44+ ,
{55 
var66 
errors66 
=66 
string66 #
.66# $
Join66$ (
(66( )
$str66) -
,66- .
createUserResult66/ ?
.66? @
Errors66@ F
.66F G
Select66G M
(66M N
e66N O
=>66P R
e66S T
.66T U
Description66U `
)66` a
)66a b
;66b c
return77 
(77 
false77 
,77 
errors77 %
,77% &
string77' -
.77- .
Empty77. 3
)773 4
;774 5
}88 
var:: 
addRoleResult:: 
=:: 
await::  %
_userManager::& 2
.::2 3
AddToRoleAsync::3 A
(::A B
user::B F
,::F G
$str::H Q
)::Q R
;::R S
if<< 
(<< 
!<< 
addRoleResult<< 
.<< 
	Succeeded<< (
)<<( )
{== 
var>> 
errors>> 
=>> 
string>> #
.>># $
Join>>$ (
(>>( )
$str>>) -
,>>- .
addRoleResult>>/ <
.>>< =
Errors>>= C
.>>C D
Select>>D J
(>>J K
e>>K L
=>>>M O
e>>P Q
.>>Q R
Description>>R ]
)>>] ^
)>>^ _
;>>_ `
return?? 
(?? 
false?? 
,?? 
errors?? %
,??% &
string??' -
.??- .
Empty??. 3
)??3 4
;??4 5
}@@ 
varBB 
patientBB 
=BB 
newBB 
PatientBB %
{CC 
UserIdDD 
=DD 
userDD 
.DD 
IdDD  
,DD  !
FullNameEE 
=EE 
requestEE "
.EE" #
FullNameEE# +
,EE+ ,
DateOfBirthFF 
=FF 
requestFF %
.FF% &
DateOfBirthFF& 1
,FF1 2
GenderGG 
=GG 
requestGG  
.GG  !
GenderGG! '
,GG' (
PhoneNumberHH 
=HH 
requestHH %
.HH% &
PhoneNumberHH& 1
,HH1 2
EmailII 
=II 
requestII 
.II  
EmailII  %
,II% &
InsuranceIdJJ 
=JJ 
requestJJ %
.JJ% &
InsuranceIdJJ& 1
,JJ1 2
CreatedDateKK 
=KK 
DateTimeKK &
.KK& '
NowKK' *
}LL 
;LL 
awaitNN 
_patientRepositoryNN $
.NN$ %
AddAsyncNN% -
(NN- .
patientNN. 5
)NN5 6
;NN6 7
returnPP 
(PP 
truePP 
,PP 
$strPP ;
,PP; <
userPP= A
.PPA B
IdPPB D
)PPD E
;PPE F
}QQ 	
publicSS 
asyncSS 
TaskSS 
<SS 
(SS 
boolSS 
SuccessSS  '
,SS' (
stringSS) /
MessageSS0 7
,SS7 8
stringSS9 ?
AccessTokenSS@ K
,SSK L
stringSSM S
RefreshTokenSST `
,SS` a
intSSb e
	ExpiresInSSf o
,SSo p
boolSSq u#
RequiresPasswordChange	SSv å
)
SSå ç
>
SSç é
Login
SSè î
(
SSî ï
LoginDto
SSï ù
request
SSû •
)
SS• ¶
{TT 	
varUU 
userUU 
=UU 
awaitUU 
_userManagerUU )
.UU) *
FindByEmailAsyncUU* :
(UU: ;
requestUU; B
.UUB C
EmailUUC H
)UUH I
;UUI J
ifWW 
(WW 
userWW 
==WW 
nullWW 
)WW 
{XX 
returnYY 
(YY 
falseYY 
,YY 
$strYY 4
,YY4 5
stringYY6 <
.YY< =
EmptyYY= B
,YYB C
stringYYD J
.YYJ K
EmptyYYK P
,YYP Q
$numYYR S
,YYS T
falseYYU Z
)YYZ [
;YY[ \
}ZZ 
var\\ 
isPasswordValid\\ 
=\\  !
await\\" '
_userManager\\( 4
.\\4 5
CheckPasswordAsync\\5 G
(\\G H
user\\H L
,\\L M
request\\N U
.\\U V
Password\\V ^
)\\^ _
;\\_ `
if^^ 
(^^ 
!^^ 
isPasswordValid^^  
)^^  !
{__ 
return`` 
(`` 
false`` 
,`` 
$str`` 4
,``4 5
string``6 <
.``< =
Empty``= B
,``B C
string``D J
.``J K
Empty``K P
,``P Q
$num``R S
,``S T
false``U Z
)``Z [
;``[ \
}aa 
ifcc 
(cc 
usercc 
.cc 
MustChangePasswordcc '
)cc' (
{dd 
returnee 
(ee 
trueee 
,ee 
$stree 8
,ee8 9
stringee: @
.ee@ A
EmptyeeA F
,eeF G
stringeeH N
.eeN O
EmptyeeO T
,eeT U
$numeeV W
,eeW X
trueeeY ]
)ee] ^
;ee^ _
}ff 
varhh 
accessTokenhh 
=hh 
awaithh #
GenerateAccessTokenhh$ 7
(hh7 8
userhh8 <
)hh< =
;hh= >
varii 
refreshTokenii 
=ii  
GenerateRefreshTokenii 3
(ii3 4
)ii4 5
;ii5 6
awaitkk 
SaveRefreshTokenkk "
(kk" #
userkk# '
.kk' (
Idkk( *
,kk* +
refreshTokenkk, 8
)kk8 9
;kk9 :
varmm 
	expiresInmm 
=mm 
intmm 
.mm  
Parsemm  %
(mm% &
_confignn 
.nn 

GetSectionnn "
(nn" #
$strnn# (
)nn( )
[nn) *
$strnn* H
]nnH I
!nnI J
)nnJ K
;nnK L
returnpp 
(pp 
truepp 
,pp 
$strpp ,
,pp, -
accessTokenpp. 9
,pp9 :
refreshTokenpp; G
,ppG H
	expiresInppI R
,ppR S
falseppT Y
)ppY Z
;ppZ [
}qq 	
publicss 
asyncss 
Taskss 
<ss 
(ss 
boolss 
Successss  '
,ss' (
stringss) /
Messagess0 7
)ss7 8
>ss8 9
ChangePasswordss: H
(ssH I
ChangePasswordDtossI Z
requestss[ b
)ssb c
{tt 	
ifuu 
(uu 
requestuu 
.uu 
NewPassworduu #
!=uu$ &
requestuu' .
.uu. /
ConfirmPassworduu/ >
)uu> ?
{vv 
returnww 
(ww 
falseww 
,ww 
$strww O
)wwO P
;wwP Q
}xx 
varzz 
userzz 
=zz 
awaitzz 
_userManagerzz )
.zz) *
FindByEmailAsynczz* :
(zz: ;
requestzz; B
.zzB C
EmailzzC H
)zzH I
;zzI J
if|| 
(|| 
user|| 
==|| 
null|| 
)|| 
{}} 
return~~ 
(~~ 
false~~ 
,~~ 
$str~~ /
)~~/ 0
;~~0 1
} 
var
ÅÅ 
result
ÅÅ 
=
ÅÅ 
await
ÅÅ 
_userManager
ÅÅ +
.
ÅÅ+ ,!
ChangePasswordAsync
ÅÅ, ?
(
ÅÅ? @
user
ÇÇ 
,
ÇÇ 
request
ÉÉ 
.
ÉÉ 
OldPassword
ÉÉ #
,
ÉÉ# $
request
ÑÑ 
.
ÑÑ 
NewPassword
ÑÑ #
)
ÑÑ# $
;
ÑÑ$ %
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
result
ÜÜ 
.
ÜÜ 
	Succeeded
ÜÜ !
)
ÜÜ! "
{
áá 
var
àà 
errors
àà 
=
àà 
string
àà #
.
àà# $
Join
àà$ (
(
àà( )
$str
àà) -
,
àà- .
result
àà/ 5
.
àà5 6
Errors
àà6 <
.
àà< =
Select
àà= C
(
ààC D
e
ààD E
=>
ààF H
e
ààI J
.
ààJ K
Description
ààK V
)
ààV W
)
ààW X
;
ààX Y
return
ââ 
(
ââ 
false
ââ 
,
ââ 
errors
ââ %
)
ââ% &
;
ââ& '
}
ää 
user
åå 
.
åå  
MustChangePassword
åå #
=
åå$ %
false
åå& +
;
åå+ ,
await
éé 
_userManager
éé 
.
éé 
UpdateAsync
éé *
(
éé* +
user
éé+ /
)
éé/ 0
;
éé0 1
return
êê 
(
êê 
true
êê 
,
êê 
$str
êê 9
)
êê9 :
;
êê: ;
}
ëë 	
public
ìì 
async
ìì 
Task
ìì 
<
ìì 
(
ìì 
bool
ìì 
Success
ìì  '
,
ìì' (
string
ìì) /
Message
ìì0 7
,
ìì7 8
string
ìì9 ?
AccessToken
ìì@ K
,
ììK L
string
ììM S
RefreshToken
ììT `
,
ìì` a
int
ììb e
	ExpiresIn
ììf o
)
ììo p
>
ììp q
RefreshToken
ììr ~
(
ìì~ %
RefreshTokenRequestDtoìì ï
requestììñ ù
)ììù û
{
îî 	
var
ïï 
storedToken
ïï 
=
ïï 
await
ïï #
_context
ïï$ ,
.
ïï, -
RefreshTokens
ïï- :
.
ññ !
FirstOrDefaultAsync
ññ $
(
ññ$ %
x
ññ% &
=>
ññ' )
x
ññ* +
.
ññ+ ,
Token
ññ, 1
==
ññ2 4
request
ññ5 <
.
ññ< =
RefreshToken
ññ= I
)
ññI J
;
ññJ K
if
òò 
(
òò 
storedToken
òò 
==
òò 
null
òò #
)
òò# $
{
ôô 
return
öö 
(
öö 
false
öö 
,
öö 
$str
öö 6
,
öö6 7
string
öö8 >
.
öö> ?
Empty
öö? D
,
ööD E
string
ööF L
.
ööL M
Empty
ööM R
,
ööR S
$num
ööT U
)
ööU V
;
ööV W
}
õõ 
if
ùù 
(
ùù 
storedToken
ùù 
.
ùù 
	IsRevoked
ùù %
)
ùù% &
{
ûû 
return
üü 
(
üü 
false
üü 
,
üü 
$str
üü 9
,
üü9 :
string
üü; A
.
üüA B
Empty
üüB G
,
üüG H
string
üüI O
.
üüO P
Empty
üüP U
,
üüU V
$num
üüW X
)
üüX Y
;
üüY Z
}
†† 
if
¢¢ 
(
¢¢ 
storedToken
¢¢ 
.
¢¢ 
	ExpiresAt
¢¢ %
<
¢¢& '
DateTime
¢¢( 0
.
¢¢0 1
UtcNow
¢¢1 7
)
¢¢7 8
{
££ 
return
§§ 
(
§§ 
false
§§ 
,
§§ 
$str
§§ 6
,
§§6 7
string
§§8 >
.
§§> ?
Empty
§§? D
,
§§D E
string
§§F L
.
§§L M
Empty
§§M R
,
§§R S
$num
§§T U
)
§§U V
;
§§V W
}
•• 
var
ßß 
user
ßß 
=
ßß 
await
ßß 
_userManager
ßß )
.
ßß) *
FindByIdAsync
ßß* 7
(
ßß7 8
storedToken
ßß8 C
.
ßßC D
UserId
ßßD J
)
ßßJ K
;
ßßK L
if
©© 
(
©© 
user
©© 
==
©© 
null
©© 
)
©© 
{
™™ 
return
´´ 
(
´´ 
false
´´ 
,
´´ 
$str
´´ /
,
´´/ 0
string
´´1 7
.
´´7 8
Empty
´´8 =
,
´´= >
string
´´? E
.
´´E F
Empty
´´F K
,
´´K L
$num
´´M N
)
´´N O
;
´´O P
}
¨¨ 
storedToken
ÆÆ 
.
ÆÆ 
	IsRevoked
ÆÆ !
=
ÆÆ" #
true
ÆÆ$ (
;
ÆÆ( )
var
∞∞ 
newAccessToken
∞∞ 
=
∞∞  
await
∞∞! &!
GenerateAccessToken
∞∞' :
(
∞∞: ;
user
∞∞; ?
)
∞∞? @
;
∞∞@ A
var
±± 
newRefreshToken
±± 
=
±±  !"
GenerateRefreshToken
±±" 6
(
±±6 7
)
±±7 8
;
±±8 9
await
≥≥ 
SaveRefreshToken
≥≥ "
(
≥≥" #
user
≥≥# '
.
≥≥' (
Id
≥≥( *
,
≥≥* +
newRefreshToken
≥≥, ;
)
≥≥; <
;
≥≥< =
await
µµ 
_context
µµ 
.
µµ 
SaveChangesAsync
µµ +
(
µµ+ ,
)
µµ, -
;
µµ- .
var
∑∑ 
	expiresIn
∑∑ 
=
∑∑ 
int
∑∑ 
.
∑∑  
Parse
∑∑  %
(
∑∑% &
_config
∏∏ 
.
∏∏ 

GetSection
∏∏ "
(
∏∏" #
$str
∏∏# (
)
∏∏( )
[
∏∏) *
$str
∏∏* H
]
∏∏H I
!
∏∏I J
)
∏∏J K
;
∏∏K L
return
∫∫ 
(
∫∫ 
true
∫∫ 
,
∫∫ 
$str
∫∫ 8
,
∫∫8 9
newAccessToken
∫∫: H
,
∫∫H I
newRefreshToken
∫∫J Y
,
∫∫Y Z
	expiresIn
∫∫[ d
)
∫∫d e
;
∫∫e f
}
ªª 	
private
ΩΩ 
async
ΩΩ 
Task
ΩΩ 
<
ΩΩ 
string
ΩΩ !
>
ΩΩ! "!
GenerateAccessToken
ΩΩ# 6
(
ΩΩ6 7
ApplicationUser
ΩΩ7 F
user
ΩΩG K
)
ΩΩK L
{
ææ 	
var
øø 
jwt
øø 
=
øø 
_config
øø 
.
øø 

GetSection
øø (
(
øø( )
$str
øø) .
)
øø. /
;
øø/ 0
var
¡¡ 
key
¡¡ 
=
¡¡ 
new
¡¡ "
SymmetricSecurityKey
¡¡ .
(
¡¡. /
Encoding
¬¬ 
.
¬¬ 
UTF8
¬¬ 
.
¬¬ 
GetBytes
¬¬ &
(
¬¬& '
jwt
¬¬' *
[
¬¬* +
$str
¬¬+ 0
]
¬¬0 1
!
¬¬1 2
)
¬¬2 3
)
¬¬3 4
;
¬¬4 5
var
ƒƒ 
credentials
ƒƒ 
=
ƒƒ 
new
ƒƒ ! 
SigningCredentials
ƒƒ" 4
(
ƒƒ4 5
key
≈≈ 
,
≈≈  
SecurityAlgorithms
∆∆ "
.
∆∆" #

HmacSha256
∆∆# -
)
∆∆- .
;
∆∆. /
var
»» 
roles
»» 
=
»» 
await
»» 
_userManager
»» *
.
»»* +
GetRolesAsync
»»+ 8
(
»»8 9
user
»»9 =
)
»»= >
;
»»> ?
var
   
claims
   
=
   
new
   
List
   !
<
  ! "
Claim
  " '
>
  ' (
{
ÀÀ 
new
ÃÃ 
Claim
ÃÃ 
(
ÃÃ 

ClaimTypes
ÃÃ $
.
ÃÃ$ %
NameIdentifier
ÃÃ% 3
,
ÃÃ3 4
user
ÃÃ5 9
.
ÃÃ9 :
Id
ÃÃ: <
)
ÃÃ< =
,
ÃÃ= >
new
ÕÕ 
Claim
ÕÕ 
(
ÕÕ 

ClaimTypes
ÕÕ $
.
ÕÕ$ %
Email
ÕÕ% *
,
ÕÕ* +
user
ÕÕ, 0
.
ÕÕ0 1
Email
ÕÕ1 6
!
ÕÕ6 7
)
ÕÕ7 8
}
ŒŒ 
;
ŒŒ 
foreach
–– 
(
–– 
var
–– 
role
–– 
in
––  
roles
––! &
)
––& '
{
—— 
claims
““ 
.
““ 
Add
““ 
(
““ 
new
““ 
Claim
““ $
(
““$ %

ClaimTypes
““% /
.
““/ 0
Role
““0 4
,
““4 5
role
““6 :
)
““: ;
)
““; <
;
““< =
claims
”” 
.
”” 
Add
”” 
(
”” 
new
”” 
Claim
”” $
(
””$ %
$str
””% +
,
””+ ,
role
””- 1
)
””1 2
)
””2 3
;
””3 4
}
‘‘ 
var
÷÷ 
expirationMinutes
÷÷ !
=
÷÷" #
int
÷÷$ '
.
÷÷' (
Parse
÷÷( -
(
÷÷- .
jwt
◊◊ 
[
◊◊ 
$str
◊◊ 2
]
◊◊2 3
!
◊◊3 4
)
◊◊4 5
;
◊◊5 6
var
ŸŸ 
token
ŸŸ 
=
ŸŸ 
new
ŸŸ 
JwtSecurityToken
ŸŸ ,
(
ŸŸ, -
issuer
⁄⁄ 
:
⁄⁄ 
jwt
⁄⁄ 
[
⁄⁄ 
$str
⁄⁄ $
]
⁄⁄$ %
,
⁄⁄% &
audience
€€ 
:
€€ 
jwt
€€ 
[
€€ 
$str
€€ (
]
€€( )
,
€€) *
claims
‹‹ 
:
‹‹ 
claims
‹‹ 
,
‹‹ 
expires
›› 
:
›› 
DateTime
›› !
.
››! "
UtcNow
››" (
.
››( )

AddMinutes
››) 3
(
››3 4
expirationMinutes
››4 E
)
››E F
,
››F G 
signingCredentials
ﬁﬁ "
:
ﬁﬁ" #
credentials
ﬁﬁ$ /
)
ﬁﬁ/ 0
;
ﬁﬁ0 1
return
‡‡ 
new
‡‡ %
JwtSecurityTokenHandler
‡‡ .
(
‡‡. /
)
‡‡/ 0
.
‡‡0 1

WriteToken
‡‡1 ;
(
‡‡; <
token
‡‡< A
)
‡‡A B
;
‡‡B C
}
·· 	
private
„„ 
static
„„ 
string
„„ "
GenerateRefreshToken
„„ 2
(
„„2 3
)
„„3 4
{
‰‰ 	
var
ÂÂ 
randomBytes
ÂÂ 
=
ÂÂ 
new
ÂÂ !
byte
ÂÂ" &
[
ÂÂ& '
$num
ÂÂ' )
]
ÂÂ) *
;
ÂÂ* +
using
ÁÁ 
var
ÁÁ 
rng
ÁÁ 
=
ÁÁ #
RandomNumberGenerator
ÁÁ 1
.
ÁÁ1 2
Create
ÁÁ2 8
(
ÁÁ8 9
)
ÁÁ9 :
;
ÁÁ: ;
rng
ÈÈ 
.
ÈÈ 
GetBytes
ÈÈ 
(
ÈÈ 
randomBytes
ÈÈ $
)
ÈÈ$ %
;
ÈÈ% &
return
ÎÎ 
Convert
ÎÎ 
.
ÎÎ 
ToBase64String
ÎÎ )
(
ÎÎ) *
randomBytes
ÎÎ* 5
)
ÎÎ5 6
;
ÎÎ6 7
}
ÏÏ 	
private
ÓÓ 
async
ÓÓ 
Task
ÓÓ 
SaveRefreshToken
ÓÓ +
(
ÓÓ+ ,
string
ÓÓ, 2
userId
ÓÓ3 9
,
ÓÓ9 :
string
ÓÓ; A
token
ÓÓB G
)
ÓÓG H
{
ÔÔ 	
var
 (
refreshTokenExpirationDays
 *
=
+ ,
int
ÒÒ 
.
ÒÒ 
TryParse
ÒÒ 
(
ÒÒ 
_config
ÚÚ 
.
ÚÚ 

GetSection
ÚÚ &
(
ÚÚ& '
$str
ÚÚ' ,
)
ÚÚ, -
[
ÚÚ- .
$str
ÚÚ. J
]
ÚÚJ K
,
ÚÚK L
out
ÛÛ 
var
ÛÛ 
days
ÛÛ  
)
ÛÛ  !
?
ÙÙ 
days
ÙÙ 
:
ıı 
$num
ıı 
;
ıı 
var
˜˜ 
refreshToken
˜˜ 
=
˜˜ 
new
˜˜ "
RefreshToken
˜˜# /
{
¯¯ 
UserId
˘˘ 
=
˘˘ 
userId
˘˘ 
,
˘˘  
Token
˙˙ 
=
˙˙ 
token
˙˙ 
,
˙˙ 
	CreatedAt
˚˚ 
=
˚˚ 
DateTime
˚˚ $
.
˚˚$ %
UtcNow
˚˚% +
,
˚˚+ ,
	ExpiresAt
¸¸ 
=
¸¸ 
DateTime
¸¸ $
.
¸¸$ %
UtcNow
¸¸% +
.
¸¸+ ,
AddDays
¸¸, 3
(
¸¸3 4(
refreshTokenExpirationDays
¸¸4 N
)
¸¸N O
,
¸¸O P
	IsRevoked
˝˝ 
=
˝˝ 
false
˝˝ !
}
˛˛ 
;
˛˛ 
await
ÄÄ 
_context
ÄÄ 
.
ÄÄ 
RefreshTokens
ÄÄ (
.
ÄÄ( )
AddAsync
ÄÄ) 1
(
ÄÄ1 2
refreshToken
ÄÄ2 >
)
ÄÄ> ?
;
ÄÄ? @
await
ÅÅ 
_context
ÅÅ 
.
ÅÅ 
SaveChangesAsync
ÅÅ +
(
ÅÅ+ ,
)
ÅÅ, -
;
ÅÅ- .
}
ÇÇ 	
}
ÉÉ 
}ÑÑ ˙¶
}C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\AppointmentService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
AppointmentService #
:$ %
IAppointmentService& 9
{ 
private 
readonly 
IRepository $
<$ %
Appointment% 0
>0 1"
_appointmentRepository2 H
;H I
private 
readonly 
IRepository $
<$ %
Doctor% +
>+ ,
_doctorRepository- >
;> ?
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_patientRepository. @
;@ A
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
IPublishEndpoint )
?) *
_publishEndpoint+ ;
;; <
private 
readonly 
ILogger  
<  !
AppointmentService! 3
>3 4
?4 5
_logger6 =
;= >
private 
readonly 
IDistributedCache *
?* +
_cache, 2
;2 3
public 
AppointmentService !
(! "
IRepository 
< 
Appointment #
># $!
appointmentRepository% :
,: ;
IRepository 
< 
Doctor 
> 
doctorRepository  0
,0 1
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
IMapper 
mapper 
, 
IPublishEndpoint 
? 
publishEndpoint -
=. /
null0 4
,4 5
ILogger 
< 
AppointmentService &
>& '
?' (
logger) /
=0 1
null2 6
,6 7
IDistributedCache   
?   
cache   $
=  % &
null  ' +
)  + ,
{!! 	"
_appointmentRepository"" "
=""# $!
appointmentRepository""% :
;"": ;
_doctorRepository## 
=## 
doctorRepository##  0
;##0 1
_patientRepository$$ 
=$$  
patientRepository$$! 2
;$$2 3
_mapper%% 
=%% 
mapper%% 
;%% 
_publishEndpoint&& 
=&& 
publishEndpoint&& .
;&&. /
_logger'' 
='' 
logger'' 
;'' 
_cache(( 
=(( 
cache(( 
;(( 
})) 	
public++ 
async++ 
Task++ 
<++ 
IEnumerable++ %
<++% &
AppointmentDto++& 4
>++4 5
>++5 6
GetAllAsync++7 B
(++B C
)++C D
{,, 	
var-- 
appointments-- 
=-- 
await-- $"
_appointmentRepository--% ;
.--; <
GetAllAsync--< G
(--G H
)--H I
;--I J
return// 
_mapper// 
.// 
Map// 
<// 
IEnumerable// *
<//* +
AppointmentDto//+ 9
>//9 :
>//: ;
(//; <
appointments//< H
)//H I
;//I J
}00 	
public22 
async22 
Task22 
<22 
AppointmentDto22 (
>22( )
AddAsync22* 2
(222 3 
CreateAppointmentDto223 G
dto22H K
)22K L
{33 	
if44 
(44 
dto44 
.44 
ScheduledDate44 !
.44! "
Date44" &
<44' (
DateTime44) 1
.441 2
Today442 7
)447 8
{55 
throw66 
new66 %
CustomValidationException66 3
(663 4
$str664 c
)66c d
;66d e
}77 
if99 
(99 
dto99 
.99 
ScheduledDate99 !
.99! "
Date99" &
>99' (
DateTime99) 1
.991 2
Today992 7
.997 8
	AddMonths998 A
(99A B
$num99B C
)99C D
)99D E
{:: 
throw;; 
new;; %
CustomValidationException;; 3
(;;3 4
$str;;4 p
);;p q
;;;q r
}<< 
if>> 
(>> 
string>> 
.>> 
IsNullOrWhiteSpace>> )
(>>) *
dto>>* -
.>>- .
TimeSlot>>. 6
)>>6 7
)>>7 8
{?? 
throw@@ 
new@@ %
CustomValidationException@@ 3
(@@3 4
$str@@4 L
)@@L M
;@@M N
}AA 
varCC 
patientCC 
=CC 
awaitCC 
_patientRepositoryCC  2
.CC2 3
GetByIdAsyncCC3 ?
(CC? @
dtoCC@ C
.CCC D
	PatientIdCCD M
)CCM N
;CCN O
ifEE 
(EE 
patientEE 
==EE 
nullEE 
)EE  
{FF 
throwGG 
newGG 
NotFoundExceptionGG +
(GG+ ,
$strGG, @
)GG@ A
;GGA B
}HH 
varJJ 
doctorJJ 
=JJ 
awaitJJ 
_doctorRepositoryJJ 0
.JJ0 1
GetByIdAsyncJJ1 =
(JJ= >
dtoJJ> A
.JJA B
DoctorIdJJB J
)JJJ K
;JJK L
ifLL 
(LL 
doctorLL 
==LL 
nullLL 
)LL 
{MM 
throwNN 
newNN 
NotFoundExceptionNN +
(NN+ ,
$strNN, ?
)NN? @
;NN@ A
}OO 
ifQQ 
(QQ 
!QQ 
doctorQQ 
.QQ 
IsActiveQQ  
)QQ  !
{RR 
throwSS 
newSS %
CustomValidationExceptionSS 3
(SS3 4
$strSS4 j
)SSj k
;SSk l
}TT 
varVV  
existingAppointmentsVV $
=VV% &
awaitVV' ,"
_appointmentRepositoryVV- C
.VVC D
GetAllAsyncVVD O
(VVO P
)VVP Q
;VVQ R
varXX 
isSlotBookedXX 
=XX  
existingAppointmentsXX 3
.XX3 4
AnyXX4 7
(XX7 8
aXX8 9
=>XX: <
aYY 
.YY 
DoctorIdYY 
==YY 
dtoYY !
.YY! "
DoctorIdYY" *
&&YY+ -
aZZ 
.ZZ 
ScheduledDateZZ 
.ZZ  
DateZZ  $
==ZZ% '
dtoZZ( +
.ZZ+ ,
ScheduledDateZZ, 9
.ZZ9 :
DateZZ: >
&&ZZ? A
a[[ 
.[[ 
TimeSlot[[ 
==[[ 
dto[[ !
.[[! "
TimeSlot[[" *
&&[[+ -
a\\ 
.\\ 
Status\\ 
!=\\ 
AppointmentStatus\\ -
.\\- .
	Cancelled\\. 7
)\\7 8
;\\8 9
if^^ 
(^^ 
isSlotBooked^^ 
)^^ 
{__ 
throw`` 
new`` %
CustomValidationException`` 3
(``3 4
$str``4 [
)``[ \
;``\ ]
}aa 
varcc 
appointmentcc 
=cc 
_mappercc %
.cc% &
Mapcc& )
<cc) *
Appointmentcc* 5
>cc5 6
(cc6 7
dtocc7 :
)cc: ;
;cc; <
appointmentdd 
.dd 
Statusdd 
=dd  
AppointmentStatusdd! 2
.dd2 3
Pendingdd3 :
;dd: ;
awaitff "
_appointmentRepositoryff (
.ff( )
AddAsyncff) 1
(ff1 2
appointmentff2 =
)ff= >
;ff> ?
awaithh .
"RemoveDoctorAvailabilityCacheAsynchh 4
(hh4 5
doctorii 
.ii 
DoctorIdii 
,ii  
appointmentjj 
.jj 
ScheduledDatejj )
,jj) *
$strkk $
)kk$ %
;kk% &
ifmm 
(mm 
_publishEndpointmm  
!=mm! #
nullmm$ (
)mm( )
{nn 
tryoo 
{pp 
awaitqq 
_publishEndpointqq *
.qq* +
Publishqq+ 2
(qq2 3
newqq3 6"
AppointmentBookedEventqq7 M
{rr 
	EventTypess !
=ss" #
$strss$ 7
,ss7 8
AppointmentIdtt %
=tt& '
appointmenttt( 3
.tt3 4
AppointmentIdtt4 A
,ttA B
	PatientIduu !
=uu" #
patientuu$ +
.uu+ ,
	PatientIduu, 5
,uu5 6
PatientNamevv #
=vv$ %
patientvv& -
.vv- .
FullNamevv. 6
,vv6 7
DoctorIdww  
=ww! "
doctorww# )
.ww) *
DoctorIdww* 2
,ww2 3
ScheduledDatexx %
=xx& '
appointmentxx( 3
.xx3 4
ScheduledDatexx4 A
,xxA B
TimeSlotyy  
=yy! "
appointmentyy# .
.yy. /
TimeSlotyy/ 7
,yy7 8

OccurredAtzz "
=zz# $
DateTimezz% -
.zz- .
UtcNowzz. 4
}{{ 
){{ 
;{{ 
_logger}} 
?}} 
.}} 
LogInformation}} +
(}}+ ,
$str	~~ É
,
~~É Ñ
appointment #
.# $
AppointmentId$ 1
,1 2
doctor
ÄÄ 
.
ÄÄ 
DoctorId
ÄÄ '
)
ÄÄ' (
;
ÄÄ( )
}
ÅÅ 
catch
ÇÇ 
(
ÇÇ 
	Exception
ÇÇ  
	exception
ÇÇ! *
)
ÇÇ* +
{
ÉÉ 
_logger
ÑÑ 
?
ÑÑ 
.
ÑÑ 
LogError
ÑÑ %
(
ÑÑ% &
	exception
ÖÖ !
,
ÖÖ! "
$strÜÜ Ä
,ÜÜÄ Å
appointment
áá #
.
áá# $
AppointmentId
áá$ 1
)
áá1 2
;
áá2 3
}
àà 
}
ââ 
return
ãã 
_mapper
ãã 
.
ãã 
Map
ãã 
<
ãã 
AppointmentDto
ãã -
>
ãã- .
(
ãã. /
appointment
ãã/ :
)
ãã: ;
;
ãã; <
}
åå 	
public
éé 
async
éé 
Task
éé 
<
éé 
AppointmentDto
éé (
>
éé( )
UpdateStatusAsync
éé* ;
(
éé; <
int
éé< ?
id
éé@ B
,
ééB C(
UpdateAppointmentStatusDto
ééD ^
dto
éé_ b
)
ééb c
{
èè 	
var
êê 
appointment
êê 
=
êê 
await
êê #$
_appointmentRepository
êê$ :
.
êê: ;
GetByIdAsync
êê; G
(
êêG H
id
êêH J
)
êêJ K
;
êêK L
if
íí 
(
íí 
appointment
íí 
==
íí 
null
íí #
)
íí# $
{
ìì 
throw
îî 
new
îî 
NotFoundException
îî +
(
îî+ ,
$str
îî, D
)
îîD E
;
îîE F
}
ïï 
if
óó 
(
óó 
appointment
óó 
.
óó 
Status
óó "
==
óó# %
AppointmentStatus
óó& 7
.
óó7 8
	Cancelled
óó8 A
)
óóA B
{
òò 
throw
ôô 
new
ôô '
CustomValidationException
ôô 3
(
ôô3 4
$str
ôô4 `
)
ôô` a
;
ôôa b
}
öö 
if
úú 
(
úú 
appointment
úú 
.
úú 
Status
úú "
==
úú# %
AppointmentStatus
úú& 7
.
úú7 8
	Completed
úú8 A
)
úúA B
{
ùù 
throw
ûû 
new
ûû '
CustomValidationException
ûû 3
(
ûû3 4
$str
ûû4 `
)
ûû` a
;
ûûa b
}
üü 
if
°° 
(
°° 
appointment
°° 
.
°° 
Status
°° "
==
°°# %
dto
°°& )
.
°°) *
Status
°°* 0
)
°°0 1
{
¢¢ 
throw
££ 
new
££ '
CustomValidationException
££ 3
(
££3 4
$"
££4 6
$str
££6 M
{
££M N
dto
££N Q
.
££Q R
Status
££R X
}
££X Y
$str
££Y Z
"
££Z [
)
££[ \
;
££\ ]
}
§§ 
if
¶¶ 
(
¶¶ 
appointment
¶¶ 
.
¶¶ 
Status
¶¶ "
==
¶¶# %
AppointmentStatus
¶¶& 7
.
¶¶7 8
Pending
¶¶8 ?
&&
¶¶@ B
dto
ßß 
.
ßß 
Status
ßß 
==
ßß 
AppointmentStatus
ßß /
.
ßß/ 0
	Completed
ßß0 9
)
ßß9 :
{
®® 
throw
©© 
new
©© '
CustomValidationException
©© 3
(
©©3 4
$str
©©4 o
)
©©o p
;
©©p q
}
™™ 
switch
¨¨ 
(
¨¨ 
dto
¨¨ 
.
¨¨ 
Status
¨¨ 
)
¨¨ 
{
≠≠ 
case
ÆÆ 
AppointmentStatus
ÆÆ &
.
ÆÆ& '
	Confirmed
ÆÆ' 0
:
ÆÆ0 1
appointment
ØØ 
.
ØØ  
Confirm
ØØ  '
(
ØØ' (
)
ØØ( )
;
ØØ) *
break
∞∞ 
;
∞∞ 
case
≤≤ 
AppointmentStatus
≤≤ &
.
≤≤& '
	Cancelled
≤≤' 0
:
≤≤0 1
appointment
≥≥ 
.
≥≥  
Cancel
≥≥  &
(
≥≥& '
dto
≥≥' *
.
≥≥* + 
CancellationReason
≥≥+ =
??
≥≥> @
string
≥≥A G
.
≥≥G H
Empty
≥≥H M
)
≥≥M N
;
≥≥N O
break
¥¥ 
;
¥¥ 
case
∂∂ 
AppointmentStatus
∂∂ &
.
∂∂& '
	Completed
∂∂' 0
:
∂∂0 1
appointment
∑∑ 
.
∑∑  
Complete
∑∑  (
(
∑∑( )
)
∑∑) *
;
∑∑* +
break
∏∏ 
;
∏∏ 
case
∫∫ 
AppointmentStatus
∫∫ &
.
∫∫& '
Pending
∫∫' .
:
∫∫. /
throw
ªª 
new
ªª '
CustomValidationException
ªª 7
(
ªª7 8
$str
ªª8 _
)
ªª_ `
;
ªª` a
}
ºº 
await
ææ $
_appointmentRepository
ææ (
.
ææ( )
UpdateAsync
ææ) 4
(
ææ4 5
id
ææ5 7
,
ææ7 8
appointment
ææ9 D
,
ææD E
CancellationToken
ææF W
.
ææW X
None
ææX \
)
ææ\ ]
;
ææ] ^
await
¿¿ 0
"RemoveDoctorAvailabilityCacheAsync
¿¿ 4
(
¿¿4 5
appointment
¡¡ 
.
¡¡ 
DoctorId
¡¡ $
,
¡¡$ %
appointment
¬¬ 
.
¬¬ 
ScheduledDate
¬¬ )
,
¬¬) *
$"
√√ 
$str
√√ 0
{
√√0 1
appointment
√√1 <
.
√√< =
Status
√√= C
}
√√C D
"
√√D E
)
√√E F
;
√√F G
return
≈≈ 
_mapper
≈≈ 
.
≈≈ 
Map
≈≈ 
<
≈≈ 
AppointmentDto
≈≈ -
>
≈≈- .
(
≈≈. /
appointment
≈≈/ :
)
≈≈: ;
;
≈≈; <
}
∆∆ 	
public
»» 
async
»» 
Task
»» 
<
»» 
bool
»» 
>
»» 
DeleteAsync
»»  +
(
»»+ ,
int
»», /
id
»»0 2
)
»»2 3
{
…… 	
var
   
appointment
   
=
   
await
   #$
_appointmentRepository
  $ :
.
  : ;
GetByIdAsync
  ; G
(
  G H
id
  H J
)
  J K
;
  K L
if
ÃÃ 
(
ÃÃ 
appointment
ÃÃ 
==
ÃÃ 
null
ÃÃ #
)
ÃÃ# $
{
ÕÕ 
throw
ŒŒ 
new
ŒŒ 
NotFoundException
ŒŒ +
(
ŒŒ+ ,
$str
ŒŒ, D
)
ŒŒD E
;
ŒŒE F
}
œœ 
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
==
——# %
AppointmentStatus
——& 7
.
——7 8
	Completed
——8 A
)
——A B
{
““ 
throw
”” 
new
”” '
CustomValidationException
”” 3
(
””3 4
$str
””4 _
)
””_ `
;
””` a
}
‘‘ 
if
÷÷ 
(
÷÷ 
appointment
÷÷ 
.
÷÷ 
Status
÷÷ "
==
÷÷# %
AppointmentStatus
÷÷& 7
.
÷÷7 8
	Confirmed
÷÷8 A
)
÷÷A B
{
◊◊ 
throw
ÿÿ 
new
ÿÿ '
CustomValidationException
ÿÿ 3
(
ÿÿ3 4
$str
ÿÿ4 _
)
ÿÿ_ `
;
ÿÿ` a
}
ŸŸ 
await
€€ $
_appointmentRepository
€€ (
.
€€( )
DeleteAsync
€€) 4
(
€€4 5
id
€€5 7
)
€€7 8
;
€€8 9
await
›› 0
"RemoveDoctorAvailabilityCacheAsync
›› 4
(
››4 5
appointment
ﬁﬁ 
.
ﬁﬁ 
DoctorId
ﬁﬁ $
,
ﬁﬁ$ %
appointment
ﬂﬂ 
.
ﬂﬂ 
ScheduledDate
ﬂﬂ )
,
ﬂﬂ) *
$str
‡‡ %
)
‡‡% &
;
‡‡& '
return
‚‚ 
true
‚‚ 
;
‚‚ 
}
„„ 	
private
ÂÂ 
async
ÂÂ 
Task
ÂÂ 0
"RemoveDoctorAvailabilityCacheAsync
ÂÂ =
(
ÂÂ= >
int
ÊÊ 
doctorId
ÊÊ 
,
ÊÊ 
DateTime
ÁÁ 
scheduledDate
ÁÁ "
,
ÁÁ" #
string
ËË 
reason
ËË 
,
ËË 
CancellationToken
ÈÈ 
ct
ÈÈ  
=
ÈÈ! "
default
ÈÈ# *
)
ÈÈ* +
{
ÍÍ 	
if
ÎÎ 
(
ÎÎ 
_cache
ÎÎ 
==
ÎÎ 
null
ÎÎ 
)
ÎÎ 
{
ÏÏ 
return
ÌÌ 
;
ÌÌ 
}
ÓÓ 
var
 
cacheKey
 
=
 
$"
 
$str
 %
{
% &
doctorId
& .
}
. /
$str
/ =
{
= >
scheduledDate
> K
:
K L
$str
L V
}
V W
"
W X
;
X Y
await
ÚÚ 
_cache
ÚÚ 
.
ÚÚ 
RemoveAsync
ÚÚ $
(
ÚÚ$ %
cacheKey
ÚÚ% -
,
ÚÚ- .
ct
ÚÚ/ 1
)
ÚÚ1 2
;
ÚÚ2 3
_logger
ÙÙ 
?
ÙÙ 
.
ÙÙ 
LogInformation
ÙÙ #
(
ÙÙ# $
$str
ıı '
,
ıı' (&
BuildCacheRemovedMessage
ˆˆ (
(
ˆˆ( )
doctorId
ˆˆ) 1
,
ˆˆ1 2
scheduledDate
ˆˆ3 @
,
ˆˆ@ A
cacheKey
ˆˆB J
,
ˆˆJ K
reason
ˆˆL R
)
ˆˆR S
)
ˆˆS T
;
ˆˆT U
}
˜˜ 	
private
˘˘ 
static
˘˘ 
string
˘˘ &
BuildCacheRemovedMessage
˘˘ 6
(
˘˘6 7
int
˙˙ 
doctorId
˙˙ 
,
˙˙ 
DateTime
˚˚ 
date
˚˚ 
,
˚˚ 
string
¸¸ 
cacheKey
¸¸ 
,
¸¸ 
string
˝˝ 
reason
˝˝ 
)
˝˝ 
{
˛˛ 	
return
ˇˇ 
Environment
ÄÄ 
.
ÄÄ 
NewLine
ÄÄ #
+
ÄÄ$ %
$str
ÅÅ :
+
ÅÅ; <
Environment
ÅÅ= H
.
ÅÅH I
NewLine
ÅÅI P
+
ÅÅQ R
$str
ÇÇ 5
+
ÇÇ6 7
Environment
ÇÇ8 C
.
ÇÇC D
NewLine
ÇÇD K
+
ÇÇL M
$str
ÉÉ :
+
ÉÉ; <
Environment
ÉÉ= H
.
ÉÉH I
NewLine
ÉÉI P
+
ÉÉQ R
$"
ÑÑ 
$str
ÑÑ 
{
ÑÑ 
doctorId
ÑÑ '
}
ÑÑ' (
"
ÑÑ( )
+
ÑÑ* +
Environment
ÑÑ, 7
.
ÑÑ7 8
NewLine
ÑÑ8 ?
+
ÑÑ@ A
$"
ÖÖ 
$str
ÖÖ 
{
ÖÖ 
date
ÖÖ #
:
ÖÖ# $
$str
ÖÖ$ .
}
ÖÖ. /
"
ÖÖ/ 0
+
ÖÖ1 2
Environment
ÖÖ3 >
.
ÖÖ> ?
NewLine
ÖÖ? F
+
ÖÖG H
$"
ÜÜ 
$str
ÜÜ 
{
ÜÜ 
cacheKey
ÜÜ '
}
ÜÜ' (
"
ÜÜ( )
+
ÜÜ* +
Environment
ÜÜ, 7
.
ÜÜ7 8
NewLine
ÜÜ8 ?
+
ÜÜ@ A
$"
áá 
$str
áá 
{
áá 
reason
áá %
}
áá% &
"
áá& '
+
áá( )
Environment
áá* 5
.
áá5 6
NewLine
áá6 =
+
áá> ?
$str
àà :
;
àà: ;
}
ââ 	
}
ää 
}ãã æ
wC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\AdminService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "
Impl" &
;& '
public		 
class		 
AdminService		 
:		 
IAdminService		 )
{

 
private 
readonly 
IDoctorRepository &
_doctorRepository' 8
;8 9
private 
readonly 
IMapper 
_mapper $
;$ %
public 

AdminService 
( 
IDoctorRepository 
doctorRepository *
,* +
IMapper 
mapper 
) 
{ 
_doctorRepository 
= 
doctorRepository ,
;, -
_mapper 
= 
mapper 
; 
} 
public 

async 
Task 
< 
List 
< 
	DoctorDto $
>$ %
>% &
GetDoctorsAsync' 6
(6 7
)7 8
{ 
var 
doctors 
= 
await 
_doctorRepository -
.- .
GetAllAsync. 9
(9 :
): ;
;; <
return 
_mapper 
. 
Map 
< 
List 
<  
	DoctorDto  )
>) *
>* +
(+ ,
doctors, 3
)3 4
;4 5
} 
public 

async 
Task 
< 
	DoctorDto 
>  
CreateDoctorAsync! 2
(2 3
CreateDoctorDto3 B
dtoC F
)F G
{ 
var 
doctor 
= 
_mapper 
. 
Map  
<  !
Doctor! '
>' (
(( )
dto) ,
), -
;- .
await!! 
_doctorRepository!! 
.!!  
AddAsync!!  (
(!!( )
doctor!!) /
)!!/ 0
;!!0 1
return## 
_mapper## 
.## 
Map## 
<## 
	DoctorDto## $
>##$ %
(##% &
doctor##& ,
)##, -
;##- .
}$$ 
public&& 

async&& 
Task&& 
<&& 
	DoctorDto&& 
?&&  
>&&  !
UpdateDoctorAsync&&" 3
(&&3 4
int'' 
id'' 

,''
 
UpdateDoctorDto(( 
dto(( 
)(( 
{)) 
var** 
doctor** 
=** 
await** 
_doctorRepository** ,
.**, -
GetByIdAsync**- 9
(**9 :
id**: <
)**< =
;**= >
if,, 

(,, 
doctor,, 
==,, 
null,, 
),, 
return-- 
null-- 
;-- 
_mapper// 
.// 
Map// 
(// 
dto// 
,// 
doctor// 
)//  
;//  !
await11 
_doctorRepository11 
.11  
UpdateAsync11  +
(11+ ,
id11, .
,11. /
doctor110 6
,116 7
CancellationToken118 I
.11I J
None11J N
)11N O
;11O P
return33 
_mapper33 
.33 
Map33 
<33 
	DoctorDto33 $
>33$ %
(33% &
doctor33& ,
)33, -
;33- .
}44 
}55 È

sC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Interfaces\IRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface 
IRepository  
<  !
T! "
>" #
{ 
Task 
< 
T 
? 
> 
GetByIdAsync 
( 
int !
id" $
)$ %
;% &
Task 
< 
List 
< 
T 
> 
> 
GetAllAsync !
(! "
)" #
;# $
Task 
< 
T 
> 
AddAsync 
( 
T 
entity !
)! "
;" #
Task 
< 
T 
> 
UpdateAsync 
( 
int 
id  "
," #
T$ %
entity& ,
,, -
CancellationToken. ?
cancellationToken@ Q
)Q R
;R S
Task		 
DeleteAsync		 
(		 
int		 
id		 
)		  
;		  !
}

 
} Ô
zC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Interfaces\IPatientRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface 
IPatientRepository '
:( )
IRepository* 5
<5 6
Patient6 =
>= >
{ 
Task 
< 
Patient 
? 
> 
GetByEmailAsync &
(& '
string' -
email. 3
)3 4
;4 5
Task		 
<		 
Patient		 
?		 
>		 
GetByPhoneAsync		 &
(		& '
string		' -
phone		. 3
)		3 4
;		4 5
Task 
< 
IEnumerable 
< 
Patient  
>  !
>! "
SearchByNameAsync# 4
(4 5
string5 ;
name< @
)@ A
;A B
} 
} Ò	
C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Interfaces\IHealthRecordRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface #
IHealthRecordRepository ,
:- .
IRepository/ :
<: ;
HealthRecord; G
>G H
{ 
Task 
< 
IEnumerable 
< 
HealthRecord %
>% &
>& '
GetByPatientIdAsync( ;
(; <
int< ?
	patientId@ I
)I J
;J K
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
 
HealthRecord

 %
>

% &
>

& '
GetByDoctorIdAsync

( :
(

: ;
int

; >
doctorId

? G
)

G H
;

H I
Task 
< 
IEnumerable 
< 
HealthRecord %
>% &
>& '#
GetByAppointmentIdAsync( ?
(? @
int@ C
appointmentIdD Q
)Q R
;R S
} 
} ≠	
yC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Interfaces\IDoctorRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface 
IDoctorRepository &
:' (
IRepository) 4
<4 5
Doctor5 ;
>; <
{ 
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !
SearchByNameAsync" 3
(3 4
string4 :
name; ?
)? @
;@ A
Task		 
<		 
IEnumerable		 
<		 
Doctor		 
>		  
>		  !$
GetBySpecialisationAsync		" :
(		: ;
string		; A
specialization		B P
)		P Q
;		Q R
Task 
< 
IEnumerable 
< 
Doctor 
>  
>  !$
GetAvailableDoctorsAsync" :
(: ;
); <
;< =
} 
} í

~C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Interfaces\IAppointmentRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &

Interfaces& 0
{ 
public 

	interface "
IAppointmentRepository +
:, -
IRepository. 9
<9 :
Appointment: E
>E F
{ 
Task 
< 
IEnumerable 
< 
Appointment $
>$ %
>% &
GetByPatientIdAsync' :
(: ;
int; >
	patientId? H
)H I
;I J
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
 
Appointment

 $
>

$ %
>

% &
GetByDoctorIdAsync

' 9
(

9 :
int

: =
doctorId

> F
)

F G
;

G H
Task 
< 
bool 
> 
IsSlotBookedAsync $
($ %
int% (
doctorId) 1
,1 2
DateTime3 ;
date< @
,@ A
stringB H
timeSlotI Q
)Q R
;R S
} 
}  
wC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Implementations\Repository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &
Implementations& 5
{ 
public 

class 

Repository 
< 
T 
> 
:  
IRepository! ,
<, -
T- .
>. /
where0 5
T6 7
:8 9
class: ?
{ 
	protected		 
readonly		 
HealthAxisDbContext		 .
_context		/ 7
;		7 8
public 

Repository 
( 
HealthAxisDbContext -
context. 5
)5 6
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
T 
? 
> 
GetByIdAsync *
(* +
int+ .
id/ 1
)1 2
{ 	
return 
await 
_context !
.! "
Set" %
<% &
T& '
>' (
(( )
)) *
.* +
	FindAsync+ 4
(4 5
id5 7
)7 8
;8 9
} 	
public 
async 
Task 
< 
List 
< 
T  
>  !
>! "
GetAllAsync# .
(. /
)/ 0
{ 	
return 
await 
_context !
.! "
Set" %
<% &
T& '
>' (
(( )
)) *
.* +
ToListAsync+ 6
(6 7
)7 8
;8 9
} 	
public 
async 
Task 
< 
T 
> 
AddAsync %
(% &
T& '
entity( .
). /
{ 	
await 
_context 
. 
Set 
< 
T  
>  !
(! "
)" #
.# $
AddAsync$ ,
(, -
entity- 3
)3 4
;4 5
await 
_context 
. 
SaveChangesAsync +
(+ ,
), -
;- .
return 
entity 
; 
} 	
public!! 
async!! 
Task!! 
<!! 
T!! 
>!! 
UpdateAsync!! (
(!!( )
int!!) ,
id!!- /
,!!/ 0
T!!1 2
entity!!3 9
,!!9 :
CancellationToken!!; L
cancellationToken!!M ^
)!!^ _
{"" 	
_context## 
.## 
Set## 
<## 
T## 
>## 
(## 
)## 
.## 
Update## $
(##$ %
entity##% +
)##+ ,
;##, -
await$$ 
_context$$ 
.$$ 
SaveChangesAsync$$ +
($$+ ,
cancellationToken$$, =
)$$= >
;$$> ?
return%% 
entity%% 
;%% 
}&& 	
public(( 
async(( 
Task(( 
DeleteAsync(( %
(((% &
int((& )
id((* ,
)((, -
{)) 	
var** 
entity** 
=** 
await** 
GetByIdAsync** +
(**+ ,
id**, .
)**. /
;**/ 0
if++ 
(++ 
entity++ 
!=++ 
null++ 
)++ 
{,, 
_context-- 
.-- 
Set-- 
<-- 
T-- 
>-- 
(--  
)--  !
.--! "
Remove--" (
(--( )
entity--) /
)--/ 0
;--0 1
await.. 
_context.. 
... 
SaveChangesAsync.. /
(../ 0
)..0 1
;..1 2
}// 
}00 	
}11 
}22 ≈
~C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Implementations\PatientRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &
Implementations& 5
{ 
public 

class 
PatientRepository "
:# $

Repository% /
</ 0
Patient0 7
>7 8
,8 9
IPatientRepository: L
{		 
public 
PatientRepository  
(  !
HealthAxisDbContext! 4
context5 <
)< =
:> ?
base@ D
(D E
contextE L
)L M
{ 	
} 	
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByEmailAsync$ 3
(3 4
string4 :
email; @
)@ A
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
Email, 1
==2 4
email5 :
): ;
;; <
} 	
public 
async 
Task 
< 
Patient !
?! "
>" #
GetByPhoneAsync$ 3
(3 4
string4 :
phone; @
)@ A
{ 	
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
PhoneNumber, 7
==8 :
phone; @
)@ A
;A B
} 	
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
{!! 	
return"" 
await"" 
_context"" !
.""! "
Patients""" *
.## 
Where## 
(## 
p## 
=>## 
p## 
.## 
FullName## &
.##& '
Contains##' /
(##/ 0
name##0 4
)##4 5
)##5 6
.$$ 
ToListAsync$$ 
($$ 
)$$ 
;$$ 
}%% 	
}&& 
}'' ¥
ÉC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Implementations\HealthRecordRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &
Implementations& 5
{ 
public 

class "
HealthRecordRepository '
:( )

Repository* 4
<4 5
HealthRecord5 A
>A B
,B C#
IHealthRecordRepositoryD [
{		 
public "
HealthRecordRepository %
(% &
HealthAxisDbContext& 9
context: A
)A B
:C D
baseE I
(I J
contextJ Q
)Q R
{ 	
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
HealthRecord& 2
>2 3
>3 4
GetByPatientIdAsync5 H
(H I
intI L
	patientIdM V
)V W
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Where 
( 
r 
=> 
r 
. 
	PatientId '
==( *
	patientId+ 4
)4 5
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
HealthRecord& 2
>2 3
>3 4
GetByDoctorIdAsync5 G
(G H
intH K
doctorIdL T
)T U
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Where 
( 
r 
=> 
r 
. 
DoctorId &
==' )
doctorId* 2
)2 3
. 
ToListAsync 
( 
) 
; 
} 	
public"" 
async"" 
Task"" 
<"" 
IEnumerable"" %
<""% &
HealthRecord""& 2
>""2 3
>""3 4#
GetByAppointmentIdAsync""5 L
(""L M
int""M P
appointmentId""Q ^
)""^ _
{## 	
return$$ 
await$$ 
_context$$ !
.$$! "
HealthRecords$$" /
.%% 
Where%% 
(%% 
r%% 
=>%% 
r%% 
.%% 
AppointmentId%% +
==%%, .
appointmentId%%/ <
)%%< =
.&& 
ToListAsync&& 
(&& 
)&& 
;&& 
}'' 	
}(( 
})) ù
}C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Implementations\DoctorRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &
Implementations& 5
{ 
public 

class 
DoctorRepository !
:" #

Repository$ .
<. /
Doctor/ 5
>5 6
,6 7
IDoctorRepository8 I
{		 
public 
DoctorRepository 
(  
HealthAxisDbContext  3
context4 ;
); <
:= >
base? C
(C D
contextD K
)K L
{ 	
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Doctor& ,
>, -
>- .
SearchByNameAsync/ @
(@ A
stringA G
nameH L
)L M
{ 	
return 
await 
_context !
.! "
Doctors" )
. 
Where 
( 
d 
=> 
d 
. 
FullName &
.& '
Contains' /
(/ 0
name0 4
)4 5
)5 6
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Doctor& ,
>, -
>- .$
GetBySpecialisationAsync/ G
(G H
stringH N
specializationO ]
)] ^
{ 	
return 
await 
_context !
.! "
Doctors" )
. 
Where 
( 
d 
=> 
d 
. 
Specialisation ,
., -
ToString- 5
(5 6
)6 7
==8 :
specialization; I
)I J
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
IEnumerable %
<% &
Doctor& ,
>, -
>- .$
GetAvailableDoctorsAsync/ G
(G H
)H I
{   	
return!! 
await!! 
_context!! !
.!!! "
Doctors!!" )
."" 
Where"" 
("" 
d"" 
=>"" 
d"" 
."" 
IsActive"" &
)""& '
.## 
ToListAsync## 
(## 
)## 
;## 
}$$ 	
}%% 
}&& ±
ÇC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Repository\Implementations\AppointmentRepository.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Repositories %
.% &
Implementations& 5
{ 
public		 

class		 !
AppointmentRepository		 &
:		' (

Repository		) 3
<		3 4
Appointment		4 ?
>		? @
,		@ A"
IAppointmentRepository		B X
{

 
public !
AppointmentRepository $
($ %
HealthAxisDbContext% 8
context9 @
)@ A
:B C
baseD H
(H I
contextI P
)P Q
{ 	
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Appointment& 1
>1 2
>2 3
GetByPatientIdAsync4 G
(G H
intH K
	patientIdL U
)U V
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Where 
( 
a 
=> 
a 
. 
	PatientId '
==( *
	patientId+ 4
)4 5
. 
ToListAsync 
( 
) 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Appointment& 1
>1 2
>2 3
GetByDoctorIdAsync4 F
(F G
intG J
doctorIdK S
)S T
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Where 
( 
a 
=> 
a 
. 
DoctorId &
==' )
doctorId* 2
)2 3
. 
ToListAsync 
( 
) 
; 
}   	
public## 
async## 
Task## 
<## 
bool## 
>## 
IsSlotBookedAsync##  1
(##1 2
int##2 5
doctorId##6 >
,##> ?
DateTime##@ H
date##I M
,##M N
string##O U
timeSlot##V ^
)##^ _
{$$ 	
return%% 
await%% 
_context%% !
.%%! "
Appointments%%" .
.&& 
AnyAsync&& 
(&& 
a&& 
=>&& 
a'' 
.'' 
DoctorId'' 
=='' !
doctorId''" *
&&''+ -
a(( 
.(( 
ScheduledDate(( #
.((# $
Date(($ (
==(() +
date((, 0
.((0 1
Date((1 5
&&((6 8
a)) 
.)) 
TimeSlot)) 
==)) !
timeSlot))" *
&&))+ -
a** 
.** 
Status** 
!=** 
AppointmentStatus**  1
.**1 2
	Cancelled**2 ;
)**; <
;**< =
}++ 	
},, 
}-- ˚
gC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Options\GarnetOptions.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Options  
;  !
public 
sealed 
class 
GarnetOptions !
{ 
public 

string 
ConnectionString "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
$str3 C
;C D
public 

string 
InstanceName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
$str/ <
;< =
} ˝à
YC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Program.cs
Log 
. 
Logger 

= 
new 
LoggerConfiguration $
($ %
)% &
. 
WriteTo 
. 
Console 
( 
) 
. !
CreateBootstrapLogger 
( 
) 
; 
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 

AddSerilog 
( 
( 
services %
,% &
configuration' 4
)4 5
=>6 8
configuration 
. 	
ReadFrom	 
. 
Configuration 
(  
builder  '
.' (
Configuration( 5
)5 6
. 	
ReadFrom	 
. 
Services 
( 
services #
)# $
.   	
Enrich  	 
.   
FromLogContext   
(   
)    
)    !
;  ! "
builder$$ 
.$$ 
Services$$ 
.$$ 
AddControllers$$ 
($$  
)$$  !
;$$! "
builder&& 
.&& 
Services&& 
.&& 
AddHostedService&& !
<&&! "
HeartbeatService&&" 2
>&&2 3
(&&3 4
)&&4 5
;&&5 6
builder'' 
.'' 
Services'' 
.'' 
AddHostedService'' !
<''! "&
NotificationCleanupService''" <
>''< =
(''= >
)''> ?
;''? @
builder,, 
.,, 
Services,, 
.,, 
AddExceptionHandler,, $
<,,$ %"
GlobalExceptionHandler,,% ;
>,,; <
(,,< =
),,= >
;,,> ?
builder-- 
.-- 
Services-- 
.-- 
AddProblemDetails-- "
(--" #
)--# $
;--$ %
builder00 
.00 
Services00 
.00 #
AddEndpointsApiExplorer00 (
(00( )
)00) *
;00* +
builder33 
.33 
Services33 
.33 
AddSwaggerGen33 
(33 
options33 &
=>33' )
{44 
options55 
.55 

SwaggerDoc55 
(55 
$str55 
,55 
new55  
OpenApiInfo55! ,
{66 
Title77 
=77 
$str77  
,77  !
Version88 
=88 
$str88 
}99 
)99 
;99 
options;; 
.;; !
AddSecurityDefinition;; !
(;;! "
$str;;" *
,;;* +
new;;, /!
OpenApiSecurityScheme;;0 E
{<< 
Type== 
=== 
SecuritySchemeType== !
.==! "
Http==" &
,==& '
Scheme>> 
=>> 
$str>> 
,>> 
BearerFormat?? 
=?? 
$str?? 
,?? 
Description@@ 
=@@ 
$str@@ ?
}AA 
)AA 
;AA 
optionsCC 
.CC "
AddSecurityRequirementCC "
(CC" #
documentCC# +
=>CC, .
newCC/ 2&
OpenApiSecurityRequirementCC3 M
{DD 
[EE 	
newEE	 *
OpenApiSecuritySchemeReferenceEE +
(EE+ ,
$strEE, 4
,EE4 5
documentEE6 >
)EE> ?
]EE? @
=EEA B
newEEC F
ListEEG K
<EEK L
stringEEL R
>EER S
(EES T
)EET U
}FF 
)FF 
;FF 
}GG 
)GG 
;GG 
builderJJ 
.JJ 
ServicesJJ 
.JJ 
AddCorsJJ 
(JJ 
optionsJJ  
=>JJ! #
{KK 
optionsLL 
.LL 
	AddPolicyLL 
(LL 
$strLL  
,LL  !
policyLL" (
=>LL) +
policyLL, 2
.LL2 3
AllowAnyOriginLL3 A
(LLA B
)LLB C
.LLC D
AllowAnyHeaderLLD R
(LLR S
)LLS T
.LLT U
AllowAnyMethodLLU c
(LLc d
)LLd e
)LLe f
;LLf g
}MM 
)MM 
;MM 
builderPP 
.PP 
ServicesPP 
.PP 
AddDbContextPP 
<PP 
HealthAxisDbContextPP 1
>PP1 2
(PP2 3
optionsPP3 :
=>PP; =
optionsQQ 
.QQ 
UseSqlServerQQ 
(QQ 
builderRR 
.RR 
ConfigurationRR 
.RR 
GetConnectionStringRR 1
(RR1 2
$strRR2 E
)RRE F
)RRF G
)RRG H
;RRH I
builderUU 
.UU 
ServicesUU 
.UU 
AddIdentityUU 
<UU 
ApplicationUserUU ,
,UU, -
IdentityRoleUU. :
>UU: ;
(UU; <
)UU< =
.VV $
AddEntityFrameworkStoresVV 
<VV 
HealthAxisDbContextVV 1
>VV1 2
(VV2 3
)VV3 4
.WW $
AddDefaultTokenProvidersWW 
(WW 
)WW 
;WW  
builderZZ 
.ZZ 
ServicesZZ 
.ZZ 
AddAuthenticationZZ "
(ZZ" #
JwtBearerDefaultsZZ# 4
.ZZ4 5 
AuthenticationSchemeZZ5 I
)ZZI J
.[[ 
AddJwtBearer[[ 
([[ 
options[[ 
=>[[ 
{\\ 
var]] 
jwt]] 
=]] 
builder]] 
.]] 
Configuration]] '
.]]' (

GetSection]]( 2
(]]2 3
$str]]3 8
)]]8 9
;]]9 :
options__ 
.__ %
TokenValidationParameters__ )
=__* +
new__, /%
TokenValidationParameters__0 I
{`` 	
ValidateIssueraa 
=aa 
trueaa !
,aa! "
ValidIssuerbb 
=bb 
jwtbb 
[bb 
$strbb &
]bb& '
,bb' (
ValidateAudiencedd 
=dd 
truedd #
,dd# $
ValidAudienceee 
=ee 
jwtee 
[ee  
$stree  *
]ee* +
,ee+ ,
ValidateLifetimegg 
=gg 
truegg #
,gg# $$
ValidateIssuerSigningKeyii $
=ii% &
trueii' +
,ii+ ,
IssuerSigningKeyjj 
=jj 
newjj " 
SymmetricSecurityKeyjj# 7
(jj7 8
Encodingkk 
.kk 
UTF8kk 
.kk 
GetByteskk &
(kk& '
jwtkk' *
[kk* +
$strkk+ 0
]kk0 1
!kk1 2
)kk2 3
)ll 
}mm 	
;mm	 

}nn 
)nn 
;nn 
builderpp 
.pp 
Servicespp 
.pp 
AddAuthorizationpp !
(pp! "
)pp" #
;pp# $
builderss 
.ss 
Servicesss 
.ss 
	AddScopedss 
(ss 
typeofss !
(ss! "
IRepositoryss" -
<ss- .
>ss. /
)ss/ 0
,ss0 1
typeofss2 8
(ss8 9

Repositoryss9 C
<ssC D
>ssD E
)ssE F
)ssF G
;ssG H
buildervv 
.vv 
Servicesvv 
.vv 
	AddScopedvv 
<vv 
IPatientRepositoryvv -
,vv- .
PatientRepositoryvv/ @
>vv@ A
(vvA B
)vvB C
;vvC D
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
builder{{ 
.{{ 
Services{{ 
.{{ 
	AddScoped{{ 
<{{ 
IDoctorService{{ )
,{{) *
DoctorService{{+ 8
>{{8 9
({{9 :
){{: ;
;{{; <
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
ÄÄ !
IAppointmentService
ÄÄ .
,
ÄÄ. / 
AppointmentService
ÄÄ0 B
>
ÄÄB C
(
ÄÄC D
)
ÄÄD E
;
ÄÄE F
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
ÑÑ "
IHealthRecordService
ÑÑ /
,
ÑÑ/ 0!
HealthRecordService
ÑÑ1 D
>
ÑÑD E
(
ÑÑE F
)
ÑÑF G
;
ÑÑG H
builderáá 
.
áá 
Services
áá 
.
áá 
	AddScoped
áá 
<
áá 
IAuthService
áá '
,
áá' (
AuthService
áá) 4
>
áá4 5
(
áá5 6
)
áá6 7
;
áá7 8
builderââ 
.
ââ 
Services
ââ 
.
ââ 
AddCors
ââ 
(
ââ 
p
ââ 
=>
ââ 
{ää 
p
ãã 
.
ãã 
	AddPolicy
ãã 
(
ãã 
$str
ãã 
,
ãã 
cfg
ãã !
=>
ãã" $
{
åå 
cfg
çç 
.
çç 
WithOrigins
çç 
(
çç 
$str
çç 0
)
çç0 1
.
èè 	
AllowAnyHeader
èè	 
(
èè 
)
èè 
.
èè 
AllowAnyMethod
èè (
(
èè( )
)
èè) *
;
èè* +
}
êê 
)
êê 
;
êê 
}ëë 
)
ëë 
;
ëë 
builderîî 
.
îî 
Services
îî 
.
îî 
AddAutoMapper
îî 
(
îî 
cfg
îî "
=>
îî# %
{
îî& '
}
îî( )
,
îî) *
	AppDomain
îî+ 4
.
îî4 5
CurrentDomain
îî5 B
.
îîB C
GetAssemblies
îîC P
(
îîP Q
)
îîQ R
)
îîR S
;
îîS T
builderóó 
.
óó 
Services
óó 
.
óó 
AddMassTransit
óó 
(
óó  
x
óó  !
=>
óó" $
{òò 
xôô 
.
ôô 
AddConsumer
ôô 
<
ôô '
AppointmentBookedConsumer
ôô '
>
ôô' (
(
ôô( )
)
ôô) *
;
ôô* +
xõõ 
.
õõ 
UsingRabbitMq
õõ 
(
õõ 
(
õõ 
context
õõ 
,
õõ 
cfg
õõ 
)
õõ 
=>
õõ !
{úú 
varùù 
rabbitConfig
ùù 
=
ùù 
builder
ùù 
.
ùù 
Configuration
ùù (
.
ùù( )

GetSection
ùù) 3
(
ùù3 4
$str
ùù4 >
)
ùù> ?
;
ùù? @
cfgüü 
.
üü 
Host
üü 
(
üü 	
rabbitConfig
†† 
[
†† 
$str
†† 
]
†† 
,
†† 
rabbitConfig
°° 
[
°° 
$str
°° 
]
°° 
,
°°  
h
¢¢ 
=>
¢¢ 
{
££ 
h
•• 	
.
••	 

Username
••
 
(
•• 
rabbitConfig
•• 
[
••  
$str
••  *
]
••* +
!
••+ ,
)
••, -
;
••- .
h
¶¶ 	
.
¶¶	 

Password
¶¶
 
(
¶¶ 
rabbitConfig
¶¶ 
[
¶¶  
$str
¶¶  *
]
¶¶* +
!
¶¶+ ,
)
¶¶, -
;
¶¶- .
}
ßß 
)
ßß 
;
ßß 
cfg
©© 
.
©© 
ReceiveEndpoint
©© 
(
©© 
rabbitConfig
©© $
[
©©$ %
$str
©©% 7
]
©©7 8
!
©©8 9
,
©©9 :
e
©©; <
=>
©©= ?
{
™™ 
e
´´ 	
.
´´	 

ConfigureConsumer
´´
 
<
´´ '
AppointmentBookedConsumer
´´ 5
>
´´5 6
(
´´6 7
context
´´7 >
)
´´> ?
;
´´? @
}
¨¨ 
)
¨¨ 
;
¨¨ 
}≠≠ 
)
≠≠ 
;
≠≠ 
}ÆÆ 
)
ÆÆ 
;
ÆÆ 
builderØØ 
.
ØØ 
Services
ØØ 
.
ØØ 
	Configure
ØØ 
<
ØØ 
GarnetOptions
ØØ (
>
ØØ( )
(
ØØ) *
builder
∞∞ 
.
∞∞ 
Configuration
∞∞ 
.
∞∞ 

GetSection
∞∞ $
(
∞∞$ %
$str
∞∞% -
)
∞∞- .
)
∞∞. /
;
∞∞/ 0
builder≤≤ 
.
≤≤ 
Services
≤≤ 
.
≤≤ (
AddStackExchangeRedisCache
≤≤ +
(
≤≤+ ,
options
≤≤, 3
=>
≤≤4 6
{≥≥ 
var
¥¥ 
garnetOptions
¥¥ 
=
¥¥ 
builder
¥¥ 
.
¥¥  
Configuration
¥¥  -
.
µµ 	

GetSection
µµ	 
(
µµ 
$str
µµ 
)
µµ 
.
∂∂ 	
Get
∂∂	 
<
∂∂ 
GarnetOptions
∂∂ 
>
∂∂ 
(
∂∂ 
)
∂∂ 
??
∂∂  
new
∂∂! $
GarnetOptions
∂∂% 2
(
∂∂2 3
)
∂∂3 4
;
∂∂4 5
options
∏∏ 
.
∏∏ 
Configuration
∏∏ 
=
∏∏ 
garnetOptions
∏∏ )
.
∏∏) *
ConnectionString
∏∏* :
;
∏∏: ;
options
ππ 
.
ππ 
InstanceName
ππ 
=
ππ 
garnetOptions
ππ (
.
ππ( )
InstanceName
ππ) 5
;
ππ5 6
}∫∫ 
)
∫∫ 
;
∫∫ 
varºº 
app
ºº 
=
ºº 	
builder
ºº
 
.
ºº 
Build
ºº 
(
ºº 
)
ºº 
;
ºº 
appΩΩ 
.
ΩΩ &
UseSerilogRequestLogging
ΩΩ 
(
ΩΩ 
)
ΩΩ 
;
ΩΩ 
app¿¿ 
.
¿¿ 
UseCors
¿¿ 
(
¿¿ 
$str
¿¿ 
)
¿¿ 
;
¿¿ 
if√√ 
(
√√ 
app
√√ 
.
√√ 
Environment
√√ 
.
√√ 
IsDevelopment
√√ !
(
√√! "
)
√√" #
)
√√# $
{ƒƒ 
app
≈≈ 
.
≈≈ 

UseSwagger
≈≈ 
(
≈≈ 
)
≈≈ 
;
≈≈ 
app
∆∆ 
.
∆∆ 
UseSwaggerUI
∆∆ 
(
∆∆ 
)
∆∆ 
;
∆∆ 
}«« 
app…… 
.
…… !
UseHttpsRedirection
…… 
(
…… 
)
…… 
;
…… 
appÃÃ 
.
ÃÃ !
UseExceptionHandler
ÃÃ 
(
ÃÃ 
)
ÃÃ 
;
ÃÃ 
appœœ 
.
œœ 
UseAuthentication
œœ 
(
œœ 
)
œœ 
;
œœ 
app–– 
.
–– 
UseAuthorization
–– 
(
–– 
)
–– 
;
–– 
app““ 
.
““ 
MapControllers
““ 
(
““ 
)
““ 
;
““ 
using’’ 
(
’’ 
var
’’ 

scope
’’ 
=
’’ 
app
’’ 
.
’’ 
Services
’’ 
.
’’  
CreateScope
’’  +
(
’’+ ,
)
’’, -
)
’’- .
{÷÷ 
var
◊◊ 
roleManager
◊◊ 
=
◊◊ 
scope
◊◊ 
.
◊◊ 
ServiceProvider
◊◊ +
.
◊◊+ , 
GetRequiredService
◊◊, >
<
◊◊> ?
RoleManager
◊◊? J
<
◊◊J K
IdentityRole
◊◊K W
>
◊◊W X
>
◊◊X Y
(
◊◊Y Z
)
◊◊Z [
;
◊◊[ \
var
ÿÿ 
userManager
ÿÿ 
=
ÿÿ 
scope
ÿÿ 
.
ÿÿ 
ServiceProvider
ÿÿ +
.
ÿÿ+ , 
GetRequiredService
ÿÿ, >
<
ÿÿ> ?
UserManager
ÿÿ? J
<
ÿÿJ K
ApplicationUser
ÿÿK Z
>
ÿÿZ [
>
ÿÿ[ \
(
ÿÿ\ ]
)
ÿÿ] ^
;
ÿÿ^ _
await
⁄⁄ 	

RoleSeeder
⁄⁄
 
.
⁄⁄ 
	SeedRoles
⁄⁄ 
(
⁄⁄ 
roleManager
⁄⁄ *
)
⁄⁄* +
;
⁄⁄+ ,
await
€€ 	
AdminSeeder
€€
 
.
€€ 
	SeedAdmin
€€ 
(
€€  
userManager
€€  +
)
€€+ ,
;
€€, -
}‹‹ 
appﬁﬁ 
.
ﬁﬁ 
Run
ﬁﬁ 
(
ﬁﬁ 
)
ﬁﬁ 	
;
ﬁﬁ	 
•
]C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\User.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
{ 
public 

class 
User 
{		 
[

 	
Key

	 
]

 
public 
int 
UserId 
{ 
get 
;  
set! $
;$ %
}& '
[ 	
Required	 
] 
[ 	
StringLength	 
( 
ValidationLimits &
.& '
EmailLength' 2
)2 3
]3 4
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
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
PasswordHash "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
[ 	
Required	 
] 
public 
Role 
Role 
{ 
get 
; 
set  #
;# $
}% &
[ 	
Required	 
] 
public 
int 
ReferenceId 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ã:
`C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Patient.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
{		 
public

 

class

 
Patient

 
{ 
[ 	
Key	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
FullNameRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits &
.& '
FullNameLength' 5
)5 6
]6 7
[ 	
RegularExpression	 
( 
RegexPatterns (
.( )
FullName) 1
,1 2
ErrorMessage3 ?
=@ A
ValidationMessagesB T
.T U!
InvalidFullNameFormatU j
)j k
]k l
public 
string 
? 
UserId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DateOfBirthRequired4 G
)G H
]H I
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
[ 	
CustomValidation	 
( 
typeof 
( 
Patient 
) 
, 
nameof 
( 
ValidateDateOfBirth &
)& '
)' (
]( )
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
GenderRequired4 B
)B C
]C D
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[   	
Required  	 
(   
ErrorMessage   
=    
ValidationMessages  ! 3
.  3 4
PhoneNumberRequired  4 G
)  G H
]  H I
[!! 	
StringLength!!	 
(!! 
ValidationLimits!! &
.!!& '
PhoneNumberLength!!' 8
)!!8 9
]!!9 :
["" 	
RegularExpression""	 
("" 
RegexPatterns"" (
.""( )
PhoneNumber"") 4
,""4 5
ErrorMessage""6 B
=""C D
ValidationMessages""E W
.""W X$
InvalidPhoneNumberFormat""X p
)""p q
]""q r
public## 
string## 
PhoneNumber## !
{##" #
get##$ '
;##' (
set##) ,
;##, -
}##. /
=##0 1
string##2 8
.##8 9
Empty##9 >
;##> ?
[%% 	
Required%%	 
(%% 
ErrorMessage%% 
=%%  
ValidationMessages%%! 3
.%%3 4
EmailRequired%%4 A
)%%A B
]%%B C
[&& 	
EmailAddress&&	 
(&& 
ErrorMessage'' 
='' 
ValidationMessages'' -
.''- .
InvalidEmailFormat''. @
)''@ A
]''A B
[(( 	
StringLength((	 
((( 
ValidationLimits(( &
.((& '
EmailLength((' 2
)((2 3
]((3 4
public)) 
string)) 
Email)) 
{)) 
get)) !
;))! "
set))# &
;))& '
}))( )
=))* +
string)), 2
.))2 3
Empty))3 8
;))8 9
public++ 
DateTime++ 
CreatedDate++ #
{++$ %
get++& )
;++) *
set+++ .
;++. /
}++0 1
=++2 3
DateTime++4 <
.++< =
Now++= @
;++@ A
public-- 
string-- 
?-- 
InsuranceId-- "
{--# $
get--% (
;--( )
set--* -
;--- .
}--/ 0
public00 
virtual00 
ICollection00 "
<00" #
Appointment00# .
>00. /
Appointments000 <
{00= >
get00? B
;00B C
set00D G
;00G H
}00I J
=11 
new11 
List11 
<11 
Appointment11 "
>11" #
(11# $
)11$ %
;11% &
public33 
virtual33 
ICollection33 "
<33" #
HealthRecord33# /
>33/ 0
HealthRecords331 >
{33? @
get33A D
;33D E
set33F I
;33I J
}33K L
=44 
new44 
List44 
<44 
HealthRecord44 #
>44# $
(44$ %
)44% &
;44& '
public66 
int66 
GetAge66 
(66 
)66 
{77 	
int88 
age88 
=88 
DateTime88 
.88 
Today88 $
.88$ %
Year88% )
-88* +
DateOfBirth88, 7
.887 8
Year888 <
;88< =
if:: 
(:: 
DateOfBirth:: 
>:: 
DateTime:: &
.::& '
Today::' ,
.::, -
AddYears::- 5
(::5 6
-::6 7
age::7 :
)::: ;
)::; <
{;; 
age<< 
--<< 
;<< 
}== 
return?? 
age?? 
;?? 
}@@ 	
publicBB 
staticBB 
ValidationResultBB &
?BB& '
ValidateDateOfBirthBB( ;
(BB; <
DateTimeCC 
dateCC 
,CC 
ValidationContextDD 
contextDD %
)DD% &
{EE 	
ifFF 
(FF 
dateFF 
.FF 
YearFF 
<FF 
$numFF  
)FF  !
{GG 
returnHH 
newHH 
ValidationResultHH +
(HH+ ,
ValidationMessagesII &
.II& ',
 DateOfBirthYearMustBe1900OrLaterII' G
)IIG H
;IIH I
}JJ 
ifLL 
(LL 
dateLL 
>LL 
DateTimeLL 
.LL  
TodayLL  %
)LL% &
{MM 
returnNN 
newNN 
ValidationResultNN +
(NN+ ,
ValidationMessagesOO &
.OO& '%
DateOfBirthCannotBeFutureOO' @
)OO@ A
;OOA B
}PP 
returnRR 
ValidationResultRR #
.RR# $
SuccessRR$ +
;RR+ ,
}SS 	
}TT 
}UU ¸	
eC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Notification.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
;  
public 
sealed 
class 
Notification  
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
string		 
Message		 
{		 
get		 
;		  
set		! $
;		$ %
}		& '
=		( )
string		* 0
.		0 1
Empty		1 6
;		6 7
public 

bool 
IsRead 
{ 
get 
; 
set !
;! "
}# $
public 

DateTime 
	CreatedAt 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
DateTime. 6
.6 7
UtcNow7 =
;= >
} ®&
eC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\HealthRecord.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
{ 
public 

class 
HealthRecord 
{ 
[		 	
Key			 
]		 
public

 
int

 
RecordId

 
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
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
AppointmentRequired4 G
)G H
]H I
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DoctorRequired4 B
)B C
]C D
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
PatientRequired4 C
)C D
]D E
public 
int 
	PatientId 
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
=  
ValidationMessages! 3
.3 4
VisitDateRequired4 E
)E F
]F G
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
	VisitDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DiagnosisRequired4 E
)E F
]F G
[ 	
StringLength	 
( 
ValidationLimits &
.& '
DiagnosisLength' 6
)6 7
]7 8
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
=  
ValidationMessages! 3
.3 4 
PrescriptionRequired4 H
)H I
]I J
[ 	
StringLength	 
( 
ValidationLimits &
.& '
PrescriptionLength' 9
)9 :
]: ;
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
(!! 
ValidationLimits!! &
.!!& '
NotesLength!!' 2
)!!2 3
]!!3 4
public"" 
string"" 
Notes"" 
{"" 
get"" !
;""! "
set""# &
;""& '
}""( )
=""* +
string"", 2
.""2 3
Empty""3 8
;""8 9
[&& 	

ForeignKey&&	 
(&& 
nameof&& 
(&& 
AppointmentId&& (
)&&( )
)&&) *
]&&* +
public'' 
virtual'' 
Appointment'' "
Appointment''# .
{''/ 0
get''1 4
;''4 5
set''6 9
;''9 :
}''; <
=''= >
null''? C
!''C D
;''D E
[)) 	

ForeignKey))	 
()) 
nameof)) 
()) 
DoctorId)) #
)))# $
)))$ %
]))% &
public** 
virtual** 
Doctor** 
Doctor** $
{**% &
get**' *
;*** +
set**, /
;**/ 0
}**1 2
=**3 4
null**5 9
!**9 :
;**: ;
[,, 	

ForeignKey,,	 
(,, 
nameof,, 
(,, 
	PatientId,, $
),,$ %
),,% &
],,& '
public-- 
virtual-- 
Patient-- 
Patient-- &
{--' (
get--) ,
;--, -
set--. 1
;--1 2
}--3 4
=--5 6
null--7 ;
!--; <
;--< =
}// 
}00 µ/
_C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Doctor.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
{ 
public 

class 
Doctor 
{		 
[

 	
Key

	 
]

 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
FullNameRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits &
.& '
FullNameLength' 5
)5 6
]6 7
[ 	
RegularExpression	 
( 
RegexPatterns 
. 
FullName "
," #
ErrorMessage 
= 
ValidationMessages -
.- .!
InvalidFullNameFormat. C
)C D
]D E
public 
string 
? 
UserId 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
FullName 
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
;; <
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
ValidationLimits 
. 
MinExperience *
,* +
ValidationLimits 
. 
MaxExperience *
,* +
ErrorMessage 
= 
ValidationMessages -
.- ."
InvalidExperienceRange. D
)D E
]E F
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Range	 
( 
typeof 
( 
decimal 
) 
, 
ValidationLimits   
.   
MinConsultationFee   /
,  / 0
ValidationLimits!! 
.!! 
MaxConsultationFee!! /
,!!/ 0
ErrorMessage"" 
="" 
ValidationMessages"" -
.""- ."
InvalidConsultationFee"". D
)""D E
]""E F
[## 	
	Precision##	 
(## 
$num## 
,## 
$num## 
)## 
]## 
public$$ 
decimal$$ 
ConsultationFee$$ &
{$$' (
get$$) ,
;$$, -
set$$. 1
;$$1 2
}$$3 4
public&& 
bool&& 
IsActive&& 
{&& 
get&& "
;&&" #
set&&$ '
;&&' (
}&&) *
=&&+ ,
true&&- 1
;&&1 2
public)) 
virtual)) 
ICollection)) "
<))" #
Appointment))# .
>)). /
Appointments))0 <
{))= >
get))? B
;))B C
set))D G
;))G H
}))I J
=** 
new** 
List** 
<** 
Appointment** "
>**" #
(**# $
)**$ %
;**% &
public,, 
virtual,, 
ICollection,, "
<,," #
HealthRecord,,# /
>,,/ 0
HealthRecords,,1 >
{,,? @
get,,A D
;,,D E
set,,F I
;,,I J
},,K L
=-- 
new-- 
List-- 
<-- 
HealthRecord-- #
>--# $
(--$ %
)--% &
;--& '
public// 
bool// 
IsAvailable// 
(//  
DateTime00 
scheduledDate00 "
,00" #
string11 
timeSlot11 
)11 
{22 	
return44 
!44 
Appointments44  
.44  !
Any44! $
(44$ %
a44% &
=>44' )
a55 
.55 
ScheduledDate55 
.55  
Date55  $
==55% '
scheduledDate55( 5
.555 6
Date556 :
&&55; =
a66 
.66 
TimeSlot66 
==66 
timeSlot66 &
&&66' )
a77 
.77 
Status77 
!=77 
AppointmentStatus77 -
.77- .
	Cancelled77. 7
)777 8
;778 9
}88 	
public:: 
int:: '
GetUpcomingAppointmentCount:: .
(::. /
)::/ 0
{;; 	
return== 
Appointments== 
.==  
Count==  %
(==% &
a==& '
=>==( *
a>> 
.>> 
ScheduledDate>> 
.>>  
Date>>  $
>=>>% '
DateTime>>( 0
.>>0 1
Today>>1 6
&&>>7 9
a?? 
.?? 
Status?? 
!=?? 
AppointmentStatus?? -
.??- .
	Cancelled??. 7
)??7 8
;??8 9
}@@ 	
publicBB 
voidBB 
ActivateBB 
(BB 
)BB 
{CC 	
IsActiveDD 
=DD 
trueDD 
;DD 
}EE 	
publicGG 
voidGG 

DeactivateGG 
(GG 
)GG  
{HH 	
IsActiveII 
=II 
falseII 
;II 
}JJ 	
}KK 
}LL ≈
jC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Auth\RefreshToken.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
.  
Auth  $
{ 
public 

class 
RefreshToken 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
public		 
string		 
Token		 
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
public 
DateTime 
	CreatedAt !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
DateTime2 :
.: ;
UtcNow; A
;A B
public 
DateTime 
	ExpiresAt !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
bool 
	IsRevoked 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
false. 3
;3 4
} 
} ˙
mC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Auth\ApplicationUser.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
.  
Auth  $
{ 
public 

class 
ApplicationUser  
:! "
IdentityUser# /
{ 
public 
bool 
MustChangePassword &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
false7 <
;< =
} 
}		 Ë7
dC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Appointment.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Models 
{ 
public 

class 
Appointment 
{ 
[

 	
Key

	 
]

 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
PatientRequired4 C
)C D
]D E
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DoctorRequired4 B
)B C
]C D
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4#
AppointmentDateRequired4 K
)K L
]L M
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
[ 	
CustomValidation	 
( 
typeof 
( 
Appointment 
) 
,  
nameof 
( !
ValidateScheduledDate (
)( )
)) *
]* +
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
TimeSlotRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits 
. 
TimeSlotLength +
,+ ,
ErrorMessage 
= 
ValidationMessages -
.- .
InvalidTimeSlot. =
)= >
]> ?
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[   	
Required  	 
(   
ErrorMessage!! 
=!! 
ValidationMessages!! -
.!!- .%
AppointmentStatusRequired!!. G
)!!G H
]!!H I
public"" 
AppointmentStatus""  
Status""! '
{""( )
get""* -
;""- .
set""/ 2
;""2 3
}""4 5
=## 
AppointmentStatus## 
.##  
Pending##  '
;##' (
[%% 	
StringLength%%	 
(%% 
ValidationLimits&& 
.&& $
CancellationReasonLength&& 5
)&&5 6
]&&6 7
public'' 
string'' 
?'' 
CancellationReason'' )
{''* +
get'', /
;''/ 0
set''1 4
;''4 5
}''6 7
[** 	

ForeignKey**	 
(** 
nameof** 
(** 
	PatientId** $
)**$ %
)**% &
]**& '
public++ 
virtual++ 
Patient++ 
Patient++ &
{++' (
get++) ,
;++, -
set++. 1
;++1 2
}++3 4
=++5 6
null++7 ;
!++; <
;++< =
[-- 	

ForeignKey--	 
(-- 
nameof-- 
(-- 
DoctorId-- #
)--# $
)--$ %
]--% &
public.. 
virtual.. 
Doctor.. 
Doctor.. $
{..% &
get..' *
;..* +
set.., /
;../ 0
}..1 2
=..3 4
null..5 9
!..9 :
;..: ;
public00 
virtual00 
HealthRecord00 #
?00# $
HealthRecord00% 1
{002 3
get004 7
;007 8
set009 <
;00< =
}00> ?
public44 
void44 
Confirm44 
(44 
)44 
{55 	
Status77 
=77 
AppointmentStatus77 &
.77& '
	Confirmed77' 0
;770 1
}88 	
public:: 
void:: 
Cancel:: 
(:: 
string:: !
reason::" (
)::( )
{;; 	
Status== 
=== 
AppointmentStatus== &
.==& '
	Cancelled==' 0
;==0 1
CancellationReason?? 
=??  
reason??! '
;??' (
}@@ 	
publicBB 
voidBB 
CompleteBB 
(BB 
)BB 
{CC 	
StatusEE 
=EE 
AppointmentStatusEE &
.EE& '
	CompletedEE' 0
;EE0 1
}FF 	
publicHH 
boolHH 

IsUpcomingHH 
(HH 
)HH  
{II 	
returnKK 
ScheduledDateKK  
.KK  !
DateKK! %
>=KK& (
DateTimeKK) 1
.KK1 2
TodayKK2 7
&&LL 
StatusMM 
!=MM 
AppointmentStatusMM .
.MM. /
	CancelledMM/ 8
;MM8 9
}NN 	
publicPP 
boolPP 
IsCancelledPP 
(PP  
)PP  !
{QQ 	
returnSS 
StatusSS 
==SS 
AppointmentStatusTT $
.TT$ %
	CancelledTT% .
;TT. /
}UU 	
publicWW 
boolWW 
IsCompletedWW 
(WW  
)WW  !
{XX 	
returnZZ 
StatusZZ 
==ZZ 
AppointmentStatus[[ $
.[[$ %
	Completed[[% .
;[[. /
}\\ 	
public`` 
static`` 
ValidationResult`` &
?``& '!
ValidateScheduledDate``( =
(``= >
DateTimeaa 
scheduledDateaa "
,aa" #
ValidationContextbb 
validationContextbb /
)bb/ 0
{cc 	
ifee 
(ee 
scheduledDateee 
.ee 
Dateee "
<ee# $
DateTimeee% -
.ee- .
Todayee. 3
)ee3 4
{ff 
returnhh 
newhh 
ValidationResulthh +
(hh+ ,
ValidationMessagesii &
.ii& '%
ScheduledDateCannotBePastii' @
)ii@ A
;iiA B
}jj 
returnll 
ValidationResultll #
.ll# $
Successll$ +
;ll+ ,
}mm 	
}oo 
}pp à<
ÅC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Migrations\20260707170554_AddNotificationsTable.cs
	namespace 	

Healthcare
 
. 
netcore 
. 

Migrations '
{ 
public		 

partial		 
class		 !
AddNotificationsTable		 .
:		/ 0
	Migration		1 :
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
$str %
,% &
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
<% &
int& )
>) *
(* +
type+ /
:/ 0
$str1 6
,6 7
nullable8 @
:@ A
falseB G
)G H
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
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
,N O
Message 
= 
table #
.# $
Column$ *
<* +
string+ 1
>1 2
(2 3
type3 7
:7 8
$str9 H
,H I
nullableJ R
:R S
falseT Y
)Y Z
,Z [
IsRead 
= 
table "
." #
Column# )
<) *
bool* .
>. /
(/ 0
type0 4
:4 5
$str6 ;
,; <
nullable= E
:E F
falseG L
)L M
,M N
	CreatedAt 
= 
table  %
.% &
Column& ,
<, -
DateTime- 5
>5 6
(6 7
type7 ;
:; <
$str= H
,H I
nullableJ R
:R S
falseT Y
)Y Z
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 7
,7 8
x9 :
=>; =
x> ?
.? @
Id@ B
)B C
;C D
} 
) 
; 
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn   
:   
$str   %
,  % &
keyValue!! 
:!! 
$num!! 
,!! 
column"" 
:"" 
$str"" (
,""( )
value## 
:## 
$num## 
)## 
;## 
migrationBuilder%% 
.%% 

UpdateData%% '
(%%' (
table&& 
:&& 
$str&&  
,&&  !
	keyColumn'' 
:'' 
$str'' %
,''% &
keyValue(( 
:(( 
$num(( 
,(( 
column)) 
:)) 
$str)) (
,))( )
value** 
:** 
$num** 
)** 
;** 
migrationBuilder,, 
.,, 

UpdateData,, '
(,,' (
table-- 
:-- 
$str--  
,--  !
	keyColumn.. 
:.. 
$str.. %
,..% &
keyValue// 
:// 
$num// 
,// 
column00 
:00 
$str00 (
,00( )
value11 
:11 
$num11 
)11 
;11 
migrationBuilder33 
.33 

UpdateData33 '
(33' (
table44 
:44 
$str44  
,44  !
	keyColumn55 
:55 
$str55 %
,55% &
keyValue66 
:66 
$num66 
,66 
column77 
:77 
$str77 (
,77( )
value88 
:88 
$num88 
)88 
;88 
migrationBuilder:: 
.:: 

UpdateData:: '
(::' (
table;; 
:;; 
$str;;  
,;;  !
	keyColumn<< 
:<< 
$str<< %
,<<% &
keyValue== 
:== 
$num== 
,== 
column>> 
:>> 
$str>> (
,>>( )
value?? 
:?? 
$num?? 
)?? 
;?? 
}@@ 	
	protectedCC 
overrideCC 
voidCC 
DownCC  $
(CC$ %
MigrationBuilderCC% 5
migrationBuilderCC6 F
)CCF G
{DD 	
migrationBuilderEE 
.EE 
	DropTableEE &
(EE& '
nameFF 
:FF 
$strFF %
)FF% &
;FF& '
migrationBuilderHH 
.HH 

UpdateDataHH '
(HH' (
tableII 
:II 
$strII  
,II  !
	keyColumnJJ 
:JJ 
$strJJ %
,JJ% &
keyValueKK 
:KK 
$numKK 
,KK 
columnLL 
:LL 
$strLL (
,LL( )
valueMM 
:MM 
$numMM 
)MM 
;MM 
migrationBuilderOO 
.OO 

UpdateDataOO '
(OO' (
tablePP 
:PP 
$strPP  
,PP  !
	keyColumnQQ 
:QQ 
$strQQ %
,QQ% &
keyValueRR 
:RR 
$numRR 
,RR 
columnSS 
:SS 
$strSS (
,SS( )
valueTT 
:TT 
$numTT 
)TT 
;TT 
migrationBuilderVV 
.VV 

UpdateDataVV '
(VV' (
tableWW 
:WW 
$strWW  
,WW  !
	keyColumnXX 
:XX 
$strXX %
,XX% &
keyValueYY 
:YY 
$numYY 
,YY 
columnZZ 
:ZZ 
$strZZ (
,ZZ( )
value[[ 
:[[ 
$num[[ 
)[[ 
;[[ 
migrationBuilder]] 
.]] 

UpdateData]] '
(]]' (
table^^ 
:^^ 
$str^^  
,^^  !
	keyColumn__ 
:__ 
$str__ %
,__% &
keyValue`` 
:`` 
$num`` 
,`` 
columnaa 
:aa 
$straa (
,aa( )
valuebb 
:bb 
$numbb 
)bb 
;bb 
migrationBuilderdd 
.dd 

UpdateDatadd '
(dd' (
tableee 
:ee 
$stree  
,ee  !
	keyColumnff 
:ff 
$strff %
,ff% &
keyValuegg 
:gg 
$numgg 
,gg 
columnhh 
:hh 
$strhh (
,hh( )
valueii 
:ii 
$numii 
)ii 
;ii 
}jj 	
}kk 
}ll Á
áC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Migrations\20260620070046_AddUserIdToPatientAndDoctor.cs
	namespace 	

Healthcare
 
. 
netcore 
. 

Migrations '
{ 
public 

partial 
class '
AddUserIdToPatientAndDoctor 4
:5 6
	Migration7 @
{		 
	protected

 
override

 
void

 
Up

  "
(

" #
MigrationBuilder

# 3
migrationBuilder

4 D
)

D E
{ 	
migrationBuilder 
. 
	AddColumn &
<& '
string' -
>- .
(. /
name 
: 
$str 
, 
table 
: 
$str !
,! "
type 
: 
$str %
,% &
nullable 
: 
true 
) 
;  
migrationBuilder 
. 
	AddColumn &
<& '
string' -
>- .
(. /
name 
: 
$str 
, 
table 
: 
$str  
,  !
type 
: 
$str %
,% &
nullable 
: 
true 
) 
;  
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 

DropColumn '
(' (
name 
: 
$str 
, 
table 
: 
$str !
)! "
;" #
migrationBuilder 
. 

DropColumn '
(' (
name   
:   
$str   
,   
table!! 
:!! 
$str!!  
)!!  !
;!!! "
}"" 	
}## 
}$$ Ú
|C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Migrations\20260619035959_AddRefreshTokens.cs
	namespace 	

Healthcare
 
. 
netcore 
. 

Migrations '
{ 
public		 

partial		 
class		 
AddRefreshTokens		 )
:		* +
	Migration		, 5
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
$str %
,% &
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
<% &
int& )
>) *
(* +
type+ /
:/ 0
$str1 6
,6 7
nullable8 @
:@ A
falseB G
)G H
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
UserId 
= 
table "
." #
Column# )
<) *
string* 0
>0 1
(1 2
type2 6
:6 7
$str8 G
,G H
nullableI Q
:Q R
falseS X
)X Y
,Y Z
Token 
= 
table !
.! "
Column" (
<( )
string) /
>/ 0
(0 1
type1 5
:5 6
$str7 F
,F G
nullableH P
:P Q
falseR W
)W X
,X Y
	CreatedAt 
= 
table  %
.% &
Column& ,
<, -
DateTime- 5
>5 6
(6 7
type7 ;
:; <
$str= H
,H I
nullableJ R
:R S
falseT Y
)Y Z
,Z [
	ExpiresAt 
= 
table  %
.% &
Column& ,
<, -
DateTime- 5
>5 6
(6 7
type7 ;
:; <
$str= H
,H I
nullableJ R
:R S
falseT Y
)Y Z
,Z [
	IsRevoked 
= 
table  %
.% &
Column& ,
<, -
bool- 1
>1 2
(2 3
type3 7
:7 8
$str9 >
,> ?
nullable@ H
:H I
falseJ O
)O P
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
$str% 7
,7 8
x9 :
=>; =
x> ?
.? @
Id@ B
)B C
;C D
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
$str$$ %
)$$% &
;$$& '
}%% 	
}&& 
}'' æ
íC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Migrations\20260618102105_AddMustChangePasswordToApplicationUser.cs
	namespace 	

Healthcare
 
. 
netcore 
. 

Migrations '
{ 
public 

partial 
class 2
&AddMustChangePasswordToApplicationUser ?
:@ A
	MigrationB K
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
$str $
,$ %
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
$str $
)$ %
;% &
} 	
} 
} ©∆
}C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Migrations\20260616142141_AddIdentityTables.cs
	namespace 	

Healthcare
 
. 
netcore 
. 

Migrations '
{ 
public		 

partial		 
class		 
AddIdentityTables		 *
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
.GG3 4
RestrictGG4 <
)GG< =
;GG= >
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
.\\3 4
Restrict\\4 <
)\\< =
;\\= >
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
.pp3 4
Restrictpp4 <
)pp< =
;pp= >
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
ÇÇ3 4
Restrict
ÇÇ4 <
)
ÇÇ< =
;
ÇÇ= >
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
àà3 4
Restrict
àà4 <
)
àà< =
;
àà= >
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
úú3 4
Restrict
úú4 <
)
úú< =
;
úú= >
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
}‡‡ ãπ
xC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Migrations\20260616123056_SolvedIssues.cs
	namespace 	

Healthcare
 
. 
netcore 
. 

Migrations '
{		 
public 

partial 
class 
SolvedIssues %
:& '
	Migration( 1
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
$str'': I
,''I J
	maxLength''K T
:''T U
$num''V Y
,''Y Z
nullable''[ c
:''c d
false''e j
)''j k
,''k l
DateOfBirth(( 
=((  !
table((" '
.((' (
Column((( .
<((. /
DateTime((/ 7
>((7 8
(((8 9
type((9 =
:((= >
$str((? J
,((J K
nullable((L T
:((T U
false((V [
)(([ \
,((\ ]
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
,++h i
CreatedDate,, 
=,,  !
table,," '
.,,' (
Column,,( .
<,,. /
DateTime,,/ 7
>,,7 8
(,,8 9
type,,9 =
:,,= >
$str,,? J
,,,J K
nullable,,L T
:,,T U
false,,V [
),,[ \
,,,\ ]
InsuranceId-- 
=--  !
table--" '
.--' (
Column--( .
<--. /
string--/ 5
>--5 6
(--6 7
type--7 ;
:--; <
$str--= L
,--L M
nullable--N V
:--V W
true--X \
)--\ ]
}.. 
,.. 
constraints// 
:// 
table// "
=>//# %
{00 
table11 
.11 

PrimaryKey11 $
(11$ %
$str11% 2
,112 3
x114 5
=>116 8
x119 :
.11: ;
	PatientId11; D
)11D E
;11E F
}22 
)22 
;22 
migrationBuilder44 
.44 
CreateTable44 (
(44( )
name55 
:55 
$str55 $
,55$ %
columns66 
:66 
table66 
=>66 !
new66" %
{77 
AppointmentId88 !
=88" #
table88$ )
.88) *
Column88* 0
<880 1
int881 4
>884 5
(885 6
type886 :
:88: ;
$str88< A
,88A B
nullable88C K
:88K L
false88M R
)88R S
.99 

Annotation99 #
(99# $
$str99$ 8
,998 9
$str99: @
)99@ A
,99A B
	PatientId:: 
=:: 
table::  %
.::% &
Column::& ,
<::, -
int::- 0
>::0 1
(::1 2
type::2 6
:::6 7
$str::8 =
,::= >
nullable::? G
:::G H
false::I N
)::N O
,::O P
DoctorId;; 
=;; 
table;; $
.;;$ %
Column;;% +
<;;+ ,
int;;, /
>;;/ 0
(;;0 1
type;;1 5
:;;5 6
$str;;7 <
,;;< =
nullable;;> F
:;;F G
false;;H M
);;M N
,;;N O
ScheduledDate<< !
=<<" #
table<<$ )
.<<) *
Column<<* 0
<<<0 1
DateTime<<1 9
><<9 :
(<<: ;
type<<; ?
:<<? @
$str<<A L
,<<L M
nullable<<N V
:<<V W
false<<X ]
)<<] ^
,<<^ _
TimeSlot== 
=== 
table== $
.==$ %
Column==% +
<==+ ,
string==, 2
>==2 3
(==3 4
type==4 8
:==8 9
$str==: H
,==H I
	maxLength==J S
:==S T
$num==U W
,==W X
nullable==Y a
:==a b
false==c h
)==h i
,==i j
Status>> 
=>> 
table>> "
.>>" #
Column>># )
<>>) *
int>>* -
>>>- .
(>>. /
type>>/ 3
:>>3 4
$str>>5 :
,>>: ;
nullable>>< D
:>>D E
false>>F K
)>>K L
,>>L M
CancellationReason?? &
=??' (
table??) .
.??. /
Column??/ 5
<??5 6
string??6 <
>??< =
(??= >
type??> B
:??B C
$str??D S
,??S T
	maxLength??U ^
:??^ _
$num??` c
,??c d
nullable??e m
:??m n
true??o s
)??s t
}@@ 
,@@ 
constraintsAA 
:AA 
tableAA "
=>AA# %
{BB 
tableCC 
.CC 

PrimaryKeyCC $
(CC$ %
$strCC% 6
,CC6 7
xCC8 9
=>CC: <
xCC= >
.CC> ?
AppointmentIdCC? L
)CCL M
;CCM N
tableDD 
.DD 

ForeignKeyDD $
(DD$ %
nameEE 
:EE 
$strEE @
,EE@ A
columnFF 
:FF 
xFF  !
=>FF" $
xFF% &
.FF& '
DoctorIdFF' /
,FF/ 0
principalTableGG &
:GG& '
$strGG( 1
,GG1 2
principalColumnHH '
:HH' (
$strHH) 3
,HH3 4
onDeleteII  
:II  !
ReferentialActionII" 3
.II3 4
RestrictII4 <
)II< =
;II= >
tableJJ 
.JJ 

ForeignKeyJJ $
(JJ$ %
nameKK 
:KK 
$strKK B
,KKB C
columnLL 
:LL 
xLL  !
=>LL" $
xLL% &
.LL& '
	PatientIdLL' 0
,LL0 1
principalTableMM &
:MM& '
$strMM( 2
,MM2 3
principalColumnNN '
:NN' (
$strNN) 4
,NN4 5
onDeleteOO  
:OO  !
ReferentialActionOO" 3
.OO3 4
RestrictOO4 <
)OO< =
;OO= >
}PP 
)PP 
;PP 
migrationBuilderRR 
.RR 
CreateTableRR (
(RR( )
nameSS 
:SS 
$strSS %
,SS% &
columnsTT 
:TT 
tableTT 
=>TT !
newTT" %
{UU 
RecordIdVV 
=VV 
tableVV $
.VV$ %
ColumnVV% +
<VV+ ,
intVV, /
>VV/ 0
(VV0 1
typeVV1 5
:VV5 6
$strVV7 <
,VV< =
nullableVV> F
:VVF G
falseVVH M
)VVM N
.WW 

AnnotationWW #
(WW# $
$strWW$ 8
,WW8 9
$strWW: @
)WW@ A
,WWA B
AppointmentIdXX !
=XX" #
tableXX$ )
.XX) *
ColumnXX* 0
<XX0 1
intXX1 4
>XX4 5
(XX5 6
typeXX6 :
:XX: ;
$strXX< A
,XXA B
nullableXXC K
:XXK L
falseXXM R
)XXR S
,XXS T
DoctorIdYY 
=YY 
tableYY $
.YY$ %
ColumnYY% +
<YY+ ,
intYY, /
>YY/ 0
(YY0 1
typeYY1 5
:YY5 6
$strYY7 <
,YY< =
nullableYY> F
:YYF G
falseYYH M
)YYM N
,YYN O
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
,ZZO P
	VisitDate[[ 
=[[ 
table[[  %
.[[% &
Column[[& ,
<[[, -
DateTime[[- 5
>[[5 6
([[6 7
type[[7 ;
:[[; <
$str[[= H
,[[H I
nullable[[J R
:[[R S
false[[T Y
)[[Y Z
,[[Z [
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
$str\\; J
,\\J K
	maxLength\\L U
:\\U V
$num\\W Z
,\\Z [
nullable\\\ d
:\\d e
false\\f k
)\\k l
,\\l m
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
$str]]> M
,]]M N
	maxLength]]O X
:]]X Y
$num]]Z ]
,]]] ^
nullable]]_ g
:]]g h
false]]i n
)]]n o
,]]o p
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
:^^b c
false^^d i
)^^i j
}__ 
,__ 
constraints`` 
:`` 
table`` "
=>``# %
{aa 
tablebb 
.bb 

PrimaryKeybb $
(bb$ %
$strbb% 7
,bb7 8
xbb9 :
=>bb; =
xbb> ?
.bb? @
RecordIdbb@ H
)bbH I
;bbI J
tablecc 
.cc 

ForeignKeycc $
(cc$ %
namedd 
:dd 
$strdd K
,ddK L
columnee 
:ee 
xee  !
=>ee" $
xee% &
.ee& '
AppointmentIdee' 4
,ee4 5
principalTableff &
:ff& '
$strff( 6
,ff6 7
principalColumngg '
:gg' (
$strgg) 8
,gg8 9
onDeletehh  
:hh  !
ReferentialActionhh" 3
.hh3 4
Restricthh4 <
)hh< =
;hh= >
tableii 
.ii 

ForeignKeyii $
(ii$ %
namejj 
:jj 
$strjj A
,jjA B
columnkk 
:kk 
xkk  !
=>kk" $
xkk% &
.kk& '
DoctorIdkk' /
,kk/ 0
principalTablell &
:ll& '
$strll( 1
,ll1 2
principalColumnmm '
:mm' (
$strmm) 3
,mm3 4
onDeletenn  
:nn  !
ReferentialActionnn" 3
.nn3 4
Restrictnn4 <
)nn< =
;nn= >
tableoo 
.oo 

ForeignKeyoo $
(oo$ %
namepp 
:pp 
$strpp C
,ppC D
columnqq 
:qq 
xqq  !
=>qq" $
xqq% &
.qq& '
	PatientIdqq' 0
,qq0 1
principalTablerr &
:rr& '
$strrr( 2
,rr2 3
principalColumnss '
:ss' (
$strss) 4
,ss4 5
onDeletett  
:tt  !
ReferentialActiontt" 3
.tt3 4
Restricttt4 <
)tt< =
;tt= >
}uu 
)uu 
;uu 
migrationBuilderww 
.ww 

InsertDataww '
(ww' (
tablexx 
:xx 
$strxx  
,xx  !
columnsyy 
:yy 
newyy 
[yy 
]yy 
{yy  
$stryy! +
,yy+ ,
$stryy- >
,yy> ?
$stryy@ J
,yyJ K
$stryyL V
,yyV W
$stryyX h
,yyh i
$stryyj }
}yy~ 
,	yy Ä
valueszz 
:zz 
newzz 
objectzz "
[zz" #
,zz# $
]zz$ %
{{{ 
{|| 
$num|| 
,|| 
$num|| 
,|| 
$str|| *
,||* +
true||, 0
,||0 1
$num||2 3
,||3 4
$num||5 7
}||8 9
,||9 :
{}} 
$num}} 
,}} 
$num}} 
,}} 
$str}} )
,}}) *
true}}+ /
,}}/ 0
$num}}1 2
,}}2 3
$num}}4 5
}}}6 7
,}}7 8
{~~ 
$num~~ 
,~~ 
$num~~ 
,~~ 
$str~~  *
,~~* +
true~~, 0
,~~0 1
$num~~2 3
,~~3 4
$num~~5 7
}~~8 9
,~~9 :
{ 
$num 
, 
$num 
, 
$str +
,+ ,
true- 1
,1 2
$num3 4
,4 5
$num6 7
}8 9
,9 :
{
ÄÄ 
$num
ÄÄ 
,
ÄÄ 
$num
ÄÄ 
,
ÄÄ 
$str
ÄÄ )
,
ÄÄ) *
true
ÄÄ+ /
,
ÄÄ/ 0
$num
ÄÄ1 2
,
ÄÄ2 3
$num
ÄÄ4 6
}
ÄÄ7 8
}
ÅÅ 
)
ÅÅ 
;
ÅÅ 
migrationBuilder
ÉÉ 
.
ÉÉ 

InsertData
ÉÉ '
(
ÉÉ' (
table
ÑÑ 
:
ÑÑ 
$str
ÑÑ !
,
ÑÑ! "
columns
ÖÖ 
:
ÖÖ 
new
ÖÖ 
[
ÖÖ 
]
ÖÖ 
{
ÖÖ  
$str
ÖÖ! ,
,
ÖÖ, -
$str
ÖÖ. ;
,
ÖÖ; <
$str
ÖÖ= J
,
ÖÖJ K
$str
ÖÖL S
,
ÖÖS T
$str
ÖÖU _
,
ÖÖ_ `
$str
ÖÖa i
,
ÖÖi j
$str
ÖÖk x
,
ÖÖx y
$strÖÖz á
}ÖÖà â
,ÖÖâ ä
values
ÜÜ 
:
ÜÜ 
new
ÜÜ 
object
ÜÜ "
[
ÜÜ" #
,
ÜÜ# $
]
ÜÜ$ %
{
áá 
{
àà 
$num
àà 
,
àà 
new
àà 
DateTime
àà %
(
àà% &
$num
àà& *
,
àà* +
$num
àà, -
,
àà- .
$num
àà/ 0
,
àà0 1
$num
àà2 3
,
àà3 4
$num
àà5 6
,
àà6 7
$num
àà8 9
,
àà9 :
$num
àà; <
,
àà< =
DateTimeKind
àà> J
.
ààJ K
Unspecified
ààK V
)
ààV W
,
ààW X
new
ààY \
DateTime
àà] e
(
ààe f
$num
ààf j
,
ààj k
$num
ààl m
,
ààm n
$num
àào q
,
ààq r
$num
ààs t
,
ààt u
$num
ààv w
,
ààw x
$num
àày z
,
ààz {
$num
àà| }
,
àà} ~
DateTimeKindàà ã
.ààã å
Unspecifiedààå ó
)ààó ò
,ààò ô
$strààö Æ
,ààÆ Ø
$stràà∞ ∂
,àà∂ ∑
$numàà∏ π
,ààπ ∫
$strààª ƒ
,ààƒ ≈
$stràà∆ “
}àà” ‘
,àà‘ ’
{
ââ 
$num
ââ 
,
ââ 
new
ââ 
DateTime
ââ %
(
ââ% &
$num
ââ& *
,
ââ* +
$num
ââ, -
,
ââ- .
$num
ââ/ 0
,
ââ0 1
$num
ââ2 3
,
ââ3 4
$num
ââ5 6
,
ââ6 7
$num
ââ8 9
,
ââ9 :
$num
ââ; <
,
ââ< =
DateTimeKind
ââ> J
.
ââJ K
Unspecified
ââK V
)
ââV W
,
ââW X
new
ââY \
DateTime
ââ] e
(
ââe f
$num
ââf j
,
ââj k
$num
ââl m
,
ââm n
$num
ââo q
,
ââq r
$num
ââs t
,
âât u
$num
ââv w
,
ââw x
$num
âây z
,
ââz {
$num
ââ| }
,
ââ} ~
DateTimeKindââ ã
.ââã å
Unspecifiedââå ó
)ââó ò
,ââò ô
$strââö ≠
,ââ≠ Æ
$strââØ ∂
,ââ∂ ∑
$numââ∏ π
,ââπ ∫
$strââª ƒ
,ââƒ ≈
$strââ∆ “
}ââ” ‘
}
ää 
)
ää 
;
ää 
migrationBuilder
åå 
.
åå 
CreateIndex
åå (
(
åå( )
name
çç 
:
çç 
$str
çç 0
,
çç0 1
table
éé 
:
éé 
$str
éé %
,
éé% &
column
èè 
:
èè 
$str
èè "
)
èè" #
;
èè# $
migrationBuilder
ëë 
.
ëë 
CreateIndex
ëë (
(
ëë( )
name
íí 
:
íí 
$str
íí 1
,
íí1 2
table
ìì 
:
ìì 
$str
ìì %
,
ìì% &
column
îî 
:
îî 
$str
îî #
)
îî# $
;
îî$ %
migrationBuilder
ññ 
.
ññ 
CreateIndex
ññ (
(
ññ( )
name
óó 
:
óó 
$str
óó 6
,
óó6 7
table
òò 
:
òò 
$str
òò &
,
òò& '
column
ôô 
:
ôô 
$str
ôô '
,
ôô' (
unique
öö 
:
öö 
true
öö 
)
öö 
;
öö 
migrationBuilder
úú 
.
úú 
CreateIndex
úú (
(
úú( )
name
ùù 
:
ùù 
$str
ùù 1
,
ùù1 2
table
ûû 
:
ûû 
$str
ûû &
,
ûû& '
column
üü 
:
üü 
$str
üü "
)
üü" #
;
üü# $
migrationBuilder
°° 
.
°° 
CreateIndex
°° (
(
°°( )
name
¢¢ 
:
¢¢ 
$str
¢¢ 2
,
¢¢2 3
table
££ 
:
££ 
$str
££ &
,
££& '
column
§§ 
:
§§ 
$str
§§ #
)
§§# $
;
§§$ %
}
•• 	
	protected
®® 
override
®® 
void
®® 
Down
®®  $
(
®®$ %
MigrationBuilder
®®% 5
migrationBuilder
®®6 F
)
®®F G
{
©© 	
migrationBuilder
™™ 
.
™™ 
	DropTable
™™ &
(
™™& '
name
´´ 
:
´´ 
$str
´´ %
)
´´% &
;
´´& '
migrationBuilder
≠≠ 
.
≠≠ 
	DropTable
≠≠ &
(
≠≠& '
name
ÆÆ 
:
ÆÆ 
$str
ÆÆ $
)
ÆÆ$ %
;
ÆÆ% &
migrationBuilder
∞∞ 
.
∞∞ 
	DropTable
∞∞ &
(
∞∞& '
name
±± 
:
±± 
$str
±± 
)
±±  
;
±±  !
migrationBuilder
≥≥ 
.
≥≥ 
	DropTable
≥≥ &
(
≥≥& '
name
¥¥ 
:
¥¥ 
$str
¥¥  
)
¥¥  !
;
¥¥! "
}
µµ 	
}
∂∂ 
}∑∑ Ò 
vC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Middleware\GlobalExceptionMiddleware.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Middleware #
{ 
public 

class "
GlobalExceptionHandler '
:( )
IExceptionHandler* ;
{ 
private		 
readonly		 
ILogger		  
<		  !"
GlobalExceptionHandler		! 7
>		7 8
_logger		9 @
;		@ A
public "
GlobalExceptionHandler %
(% &
ILogger& -
<- ."
GlobalExceptionHandler. D
>D E
loggerF L
)L M
{ 	
_logger 
= 
logger 
; 
} 	
public 
async 
	ValueTask 
< 
bool #
># $
TryHandleAsync% 3
(3 4
HttpContext 
httpContext #
,# $
	Exception 
	exception 
,  
CancellationToken 
cancellationToken /
)/ 0
{ 	
_logger 
. 
LogError 
( 
	exception 
, 
$str G
,G H
	exception 
. 
Message !
)! "
;" #
var 
( 

statusCode 
, 
message $
)$ %
=& '
	exception( 1
switch2 8
{ 
NotFoundException !
=>" $
( 
StatusCodes  
.  !
Status404NotFound! 2
,2 3
	exception4 =
.= >
Message> E
)E F
,F G
ValidationException #
=>$ &
(   
StatusCodes    
.    !
Status400BadRequest  ! 4
,  4 5
	exception  6 ?
.  ? @
Message  @ G
)  G H
,  H I!
BusinessRuleException"" %
=>""& (
(## 
StatusCodes##  
.##  !
Status409Conflict##! 2
,##2 3
	exception##4 =
.##= >
Message##> E
)##E F
,##F G
AppException%% 
=>%% 
(&& 
StatusCodes&&  
.&&  !
Status400BadRequest&&! 4
,&&4 5
	exception&&6 ?
.&&? @
Message&&@ G
)&&G H
,&&H I'
UnauthorizedAccessException(( +
=>((, .
()) 
StatusCodes))  
.))  !!
Status401Unauthorized))! 6
,))6 7
	exception))8 A
.))A B
Message))B I
)))I J
,))J K
_++ 
=>++ 
(,, 
StatusCodes,,  
.,,  !(
Status500InternalServerError,,! =
,,,= >
$str,,? V
),,V W
}-- 
;-- 
httpContext// 
.// 
Response//  
.//  !

StatusCode//! +
=//, -

statusCode//. 8
;//8 9
httpContext00 
.00 
Response00  
.00  !
ContentType00! ,
=00- .
$str00/ A
;00A B
var22 
response22 
=22 
new22 
ErrorResponse22 ,
{33 

StatusCode44 
=44 

statusCode44 '
,44' (
Message55 
=55 
message55 !
,55! "
	TimeStamp66 
=66 
DateTime66 $
.66$ %
UtcNow66% +
,66+ ,
Path77 
=77 
httpContext77 "
.77" #
Request77# *
.77* +
Path77+ /
}88 
;88 
await:: 
httpContext:: 
.:: 
Response:: &
.::& '
WriteAsJsonAsync::' 7
(::7 8
response;; 
,;; 
cancellationToken<< !
)<<! "
;<<" #
return>> 
true>> 
;>> 
}?? 	
}@@ 
}AA §
iC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Mappings\MappingProfile.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Profiles !
{		 
public

 

class

 
MappingProfile

 
:

  !
Profile

" )
{ 
public 
MappingProfile 
( 
) 
{ 	
	CreateMap 
< 
CreatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
UpdatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
Patient 
, 

PatientDto )
>) *
(* +
)+ ,
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
Age! $
,$ %
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
GetAge2 8
(8 9
)9 :
): ;
); <
;< =
	CreateMap 
< 
CreateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
UpdateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
Doctor 
, 
	DoctorDto '
>' (
(( )
)) *
. 
	ForMember 
( 
dest   
=>   
dest    
.    !$
UpcomingAppointmentCount  ! 9
,  9 :
opt!! 
=>!! 
opt!! 
.!! 
MapFrom!! &
(!!& '
src"" 
=>"" 
src"" "
.""" #'
GetUpcomingAppointmentCount""# >
(""> ?
)""? @
)""@ A
)""A B
;""B C
	CreateMap%% 
<%%  
CreateAppointmentDto%% *
,%%* +
Appointment%%, 7
>%%7 8
(%%8 9
)%%9 :
;%%: ;
	CreateMap'' 
<'' &
UpdateAppointmentStatusDto'' 0
,''0 1
Appointment''2 =
>''= >
(''> ?
)''? @
;''@ A
	CreateMap)) 
<)) 
Appointment)) !
,))! "
AppointmentDto))# 1
>))1 2
())2 3
)))3 4
;))4 5
	CreateMap,, 
<,, !
CreateHealthRecordDto,, +
,,,+ ,
HealthRecord-- 
>-- 
(-- 
)-- 
;--  
	CreateMap// 
<// 
HealthRecord// "
,//" #
HealthRecordDto00 
>00  
(00  !
)00! "
;00" #
	CreateMap22 
<22 
HealthRecord22 "
,22" #
HealthRecordDto22$ 3
>223 4
(224 5
)225 6
;226 7
	CreateMap44 
<44 !
CreateHealthRecordDto44 +
,44+ ,
HealthRecord44- 9
>449 :
(44: ;
)44; <
;44< =
}55 	
}66 
}77 `
^C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\GlobalUsings.cs˝
pC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Exceptions\ValidationException.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Exceptions #
{ 
public 

class 
ValidationException $
:% &
AppException' 3
{ 
public 
ValidationException "
(" #
string# )
message* 1
)1 2
: 
base 
( 
message 
) 
{ 	
}		 	
}

 
} ˜
nC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Exceptions\NotFoundException.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Exceptions #
{ 
public 

class 
NotFoundException "
:# $
AppException% 1
{ 
public 
NotFoundException  
(  !
string! '
message( /
)/ 0
: 
base 
( 
message 
) 
{ 	
} 	
}		 
}

 É
rC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Exceptions\BusinessRuleException.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Exceptions #
{ 
public 

class !
BusinessRuleException &
:' (
AppException) 5
{ 
public !
BusinessRuleException $
($ %
string% +
message, 3
)3 4
: 
base 
( 
message 
) 
{ 	
} 	
}		 
}

 ë
aC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Data\RoleSeeder.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Data 
{ 
public 

static 
class 

RoleSeeder "
{ 
public 
static 
async 
Task  
	SeedRoles! *
(* +
RoleManager+ 6
<6 7
IdentityRole7 C
>C D
roleManagerE P
)P Q
{ 	
string		 
[		 
]		 
roles		 
=		 
{		 
$str		 &
,		& '
$str		( 0
,		0 1
$str		2 ;
}		< =
;		= >
foreach 
( 
var 
role 
in  
roles! &
)& '
{ 
if 
( 
! 
await 
roleManager &
.& '
RoleExistsAsync' 6
(6 7
role7 ;
); <
)< =
{ 
await 
roleManager %
.% &
CreateAsync& 1
(1 2
new2 5
IdentityRole6 B
(B C
roleC G
)G H
)H I
;I J
} 
} 
} 	
} 
} ˛
iC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Exceptions\AppException.cs
	namespace 	

HealthAxis
 
. 
API 
. 

Exceptions #
{ 
public 

abstract 
class 
AppException &
:' (
	Exception) 2
{ 
	protected 
AppException 
( 
string %
message& -
)- .
: 
base 
( 
message 
) 
{ 	
} 	
}		 
}

 Ç
oC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Events\AppointmentBookedEvent.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Events 
;  
public 
sealed 
class "
AppointmentBookedEvent *
{ 
public 

string 
	EventType 
{ 
get !
;! "
set# &
;& '
}( )
=* +
$str, ?
;? @
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
int		 
	PatientId		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
public 

string 
PatientName 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
public 

int 
DoctorId 
{ 
get 
; 
set "
;" #
}$ %
public 

DateTime 
ScheduledDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 

string 
TimeSlot 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
public 

DateTime 

OccurredAt 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
DateTime/ 7
.7 8
UtcNow8 >
;> ?
} øQ
jC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Data\HealthAxisDbContext.cs
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
<		8 9
ApplicationUser		9 H
>		H I
{

 
public 
HealthAxisDbContext "
(" #
DbContextOptions 
< 
HealthAxisDbContext 0
>0 1
options2 9
)9 :
: 
base 
( 
options 
) 
{ 
} 
public 
DbSet 
< 
Patient 
> 
Patients &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
DbSet 
< 
Doctor 
> 
Doctors $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
DbSet 
< 
Notification !
>! "
Notifications# 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
DbSet 
< 
RefreshToken !
>! "
RefreshTokens# 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
builder= D
)D E
{ 	
base 
. 
OnModelCreating  
(  !
builder! (
)( )
;) *
builder 
. 
Entity 
< 
Patient "
>" #
(# $
)$ %
. 
Property 
( 
p 
=> 
p  
.  !
UserId! '
)' (
.   

IsRequired   
(   
false   !
)  ! "
;  " #
builder"" 
."" 
Entity"" 
<"" 
Doctor"" !
>""! "
(""" #
)""# $
.## 
Property## 
(## 
d## 
=>## 
d##  
.##  !
UserId##! '
)##' (
.$$ 

IsRequired$$ 
($$ 
false$$ !
)$$! "
;$$" #
foreach&& 
(&& 
var&& 
relationship&& %
in&&& (
builder&&) 0
.&&0 1
Model&&1 6
.'' 
GetEntityTypes'' 
(''  
)''  !
.(( 

SelectMany(( 
((( 
e(( 
=>((  
e((! "
.((" #
GetForeignKeys((# 1
(((1 2
)((2 3
)((3 4
)((4 5
{)) 
relationship** 
.** 
DeleteBehavior** +
=**, -
DeleteBehavior**. <
.**< =
Restrict**= E
;**E F
}++ 
SeedData-- 
(-- 
builder-- 
)-- 
;-- 
}.. 	
private00 
static00 
void00 
SeedData00 $
(00$ %
ModelBuilder00% 1
modelBuilder002 >
)00> ?
{11 	
modelBuilder22 
.22 
Entity22 
<22  
Doctor22  &
>22& '
(22' (
)22( )
.22) *
HasData22* 1
(221 2
new33 
Doctor33 
{44 
DoctorId55 
=55 
$num55  
,55  !
UserId66 
=66 
null66 !
,66! "
FullName77 
=77 
$str77 *
,77* +
Specialisation88 "
=88# $
Specialisation88% 3
.883 4

Cardiology884 >
,88> ?
YearsOfExperience99 %
=99& '
$num99( *
,99* +
ConsultationFee:: #
=::$ %
$num::& *
,::* +
IsActive;; 
=;; 
true;; #
}<< 
,<< 
new== 
Doctor== 
{>> 
DoctorId?? 
=?? 
$num??  
,??  !
UserId@@ 
=@@ 
null@@ !
,@@! "
FullNameAA 
=AA 
$strAA )
,AA) *
SpecialisationBB "
=BB# $
SpecialisationBB% 3
.BB3 4
DermatologyBB4 ?
,BB? @
YearsOfExperienceCC %
=CC& '
$numCC( )
,CC) *
ConsultationFeeDD #
=DD$ %
$numDD& *
,DD* +
IsActiveEE 
=EE 
trueEE #
}FF 
,FF 
newGG 
DoctorGG 
{HH 
DoctorIdII 
=II 
$numII  
,II  !
UserIdJJ 
=JJ 
nullJJ !
,JJ! "
FullNameKK 
=KK 
$strKK )
,KK) *
SpecialisationLL "
=LL# $
SpecialisationLL% 3
.LL3 4
	NeurologyLL4 =
,LL= >
YearsOfExperienceMM %
=MM& '
$numMM( *
,MM* +
ConsultationFeeNN #
=NN$ %
$numNN& +
,NN+ ,
IsActiveOO 
=OO 
trueOO #
}PP 
,PP 
newQQ 
DoctorQQ 
{RR 
DoctorIdSS 
=SS 
$numSS  
,SS  !
UserIdTT 
=TT 
nullTT !
,TT! "
FullNameUU 
=UU 
$strUU +
,UU+ ,
SpecialisationVV "
=VV# $
SpecialisationVV% 3
.VV3 4

PediatricsVV4 >
,VV> ?
YearsOfExperienceWW %
=WW& '
$numWW( )
,WW) *
ConsultationFeeXX #
=XX$ %
$numXX& *
,XX* +
IsActiveYY 
=YY 
trueYY #
}ZZ 
,ZZ 
new[[ 
Doctor[[ 
{\\ 
DoctorId]] 
=]] 
$num]]  
,]]  !
UserId^^ 
=^^ 
null^^ !
,^^! "
FullName__ 
=__ 
$str__ )
,__) *
Specialisation`` "
=``# $
Specialisation``% 3
.``3 4
Orthopedics``4 ?
,``? @
YearsOfExperienceaa %
=aa& '
$numaa( *
,aa* +
ConsultationFeebb #
=bb$ %
$numbb& *
,bb* +
IsActivecc 
=cc 
truecc #
}dd 
)ee 
;ee 
modelBuildergg 
.gg 
Entitygg 
<gg  
Patientgg  '
>gg' (
(gg( )
)gg) *
.gg* +
HasDatagg+ 2
(gg2 3
newhh 
Patienthh 
{ii 
	PatientIdjj 
=jj 
$numjj  !
,jj! "
UserIdkk 
=kk 
nullkk !
,kk! "
FullNamell 
=ll 
$strll %
,ll% &
DateOfBirthnn 
=nn  !
newnn" %
DateTimenn& .
(nn. /
$numnn/ 3
,nn3 4
$numnn4 5
,nn5 6
$numnn6 8
,nn8 9
$numnn9 :
,nn: ;
$numnn; <
,nn< =
$numnn= >
,nn> ?
DateTimeKindnn? K
.nnK L
UnspecifiednnL W
)nnW X
,nnX Y
Genderpp 
=pp 
Genderpp #
.pp# $
Femalepp$ *
,pp* +
PhoneNumberqq 
=qq  !
$strqq" .
,qq. /
Emailrr 
=rr 
$strrr 0
,rr0 1
InsuranceIdss 
=ss  !
$strss" +
,ss+ ,
CreatedDateuu 
=uu  !
newuu" %
DateTimeuu& .
(uu. /
$numuu/ 3
,uu3 4
$numuu4 5
,uu5 6
$numuu6 7
,uu7 8
$numuu8 9
,uu9 :
$numuu: ;
,uu; <
$numuu< =
,uu= >
DateTimeKindvv 
.vv 
Utcvv 
)vv 
}xx 
,xx 
newyy 
Patientyy 
{zz 
	PatientId{{ 
={{ 
$num{{  !
,{{! "
UserId|| 
=|| 
null|| !
,||! "
FullName}} 
=}} 
$str}} &
,}}& '
DateOfBirth 
=  !
new" %
DateTime& .
(. /
$num
ÄÄ 
,
ÄÄ 
$num
ÅÅ 
,
ÅÅ 
$num
ÇÇ 
,
ÇÇ 
$num
ÉÉ 
,
ÉÉ 
$num
ÑÑ 
,
ÑÑ 
$num
ÖÖ 
,
ÖÖ 
DateTimeKind
ÜÜ 
.
ÜÜ 
Unspecified
ÜÜ $
)
ÜÜ$ %
,
ÜÜ% &
Gender
àà 
=
àà 
Gender
àà #
.
àà# $
Male
àà$ (
,
àà( )
PhoneNumber
ââ 
=
ââ  !
$str
ââ" .
,
ââ. /
Email
ää 
=
ää 
$str
ää /
,
ää/ 0
InsuranceId
ãã 
=
ãã  !
$str
ãã" +
,
ãã+ ,
CreatedDate
çç 
=
çç  !
new
çç" %
DateTime
çç& .
(
çç. /
$num
éé 
,
éé 
$num
èè 
,
èè 
$num
êê 
,
êê 
$num
ëë 
,
ëë 
$num
íí 
,
íí 
$num
ìì 
,
ìì 
DateTimeKind
îî 
.
îî 
Utc
îî 
)
îî 
}
ññ 
)
óó 
;
óó 
}
òò 	
}
ôô 
}öö æ
bC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Data\AdminSeeder.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Data 
{ 
public 

static 
class 
AdminSeeder #
{ 
public 
static 
async 
Task  
	SeedAdmin! *
(* +
UserManager+ 6
<6 7
ApplicationUser7 F
>F G
userManagerH S
)S T
{		 	
const

 
string

 

adminEmail

 #
=

$ %
$str

& <
;

< =
const 
string 
adminPassword &
=' (
$str) 4
;4 5
var 
existingAdmin 
= 
await  %
userManager& 1
.1 2
FindByEmailAsync2 B
(B C

adminEmailC M
)M N
;N O
if 
( 
existingAdmin 
!=  
null! %
)% &
{ 
return 
; 
} 
var 
	adminUser 
= 
new 
ApplicationUser  /
{ 
UserName 
= 

adminEmail %
,% &
Email 
= 

adminEmail "
," #
EmailConfirmed 
=  
true! %
,% &
MustChangePassword "
=# $
false% *
} 
; 
var 
result 
= 
await 
userManager *
.* +
CreateAsync+ 6
(6 7
	adminUser7 @
,@ A
adminPasswordB O
)O P
;P Q
if 
( 
result 
. 
	Succeeded  
)  !
{ 
await   
userManager   !
.  ! "
AddToRoleAsync  " 0
(  0 1
	adminUser  1 :
,  : ;
$str  < C
)  C D
;  D E
}!! 
}"" 	
}## 
}$$ ŸX
oC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\PatientController.cs
	namespace		 	

HealthAxis		
 
.		 
API		 
.		 
Controllers		 $
{

 
[ 
	Authorize 
( !
AuthenticationSchemes $
=% &
JwtBearerDefaults' 8
.8 9 
AuthenticationScheme9 M
)M N
]N O
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
PatientController "
:# $
ControllerBase% 3
{ 
private 
readonly 
IPatientService (
_service) 1
;1 2
public 
PatientController  
(  !
IPatientService! 0
service1 8
)8 9
{ 	
_service 
= 
service 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
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
[0 1
	FromQuery1 :
]: ;
PaginationParams< L
paginationParamsM ]
)] ^
{ 	
var 
result 
= 
await 
_service '
.' (
GetPagedAsync( 5
(5 6
paginationParams6 F
)F G
;G H
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str #
)# $
]$ %
[   	
HttpGet  	 
(   
$str   
)   
]   
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' (
GetDoctorPatients!!) :
(!!: ;
[!!; <
	FromQuery!!< E
]!!E F
PaginationParams!!G W
paginationParams!!X h
)!!h i
{"" 	
var## 
doctorUserId## 
=## 
User## #
.### $
	FindFirst##$ -
(##- .

ClaimTypes##. 8
.##8 9
NameIdentifier##9 G
)##G H
?##H I
.##I J
Value##J O
;##O P
if%% 
(%% 
string%% 
.%% 
IsNullOrEmpty%% $
(%%$ %
doctorUserId%%% 1
)%%1 2
)%%2 3
{&& 
return'' 
Unauthorized'' #
(''# $
)''$ %
;''% &
}(( 
var** 
result** 
=** 
await** 
_service** '
.**' ("
GetDoctorPatientsAsync**( >
(**> ?
doctorUserId**? K
,**K L
paginationParams**M ]
)**] ^
;**^ _
return++ 
Ok++ 
(++ 
result++ 
)++ 
;++ 
},, 	
[-- 	
	Authorize--	 
(-- 
Roles-- 
=-- 
$str-- $
)--$ %
]--% &
[.. 	
HttpGet..	 
(.. 
$str.. 
).. 
].. 
public// 
async// 
Task// 
<// 
IActionResult// '
>//' (
GetCurrentPatient//) :
(//: ;
)//; <
{00 	
var11 
userId11 
=11 
User11 
.11 
	FindFirst11 '
(11' (

ClaimTypes11( 2
.112 3
NameIdentifier113 A
)11A B
?11B C
.11C D
Value11D I
;11I J
if33 
(33 
string33 
.33 
IsNullOrEmpty33 $
(33$ %
userId33% +
)33+ ,
)33, -
{44 
return55 
Unauthorized55 #
(55# $
)55$ %
;55% &
}66 
var88 
patient88 
=88 
await88 
_service88  (
.88( )
GetByUserIdAsync88) 9
(889 :
userId88: @
)88@ A
;88A B
return:: 
Ok:: 
(:: 
patient:: 
):: 
;:: 
};; 	
[== 	
	Authorize==	 
(== 
Roles== 
=== 
$str== $
)==$ %
]==% &
[>> 	
HttpGet>>	 
(>> 
$str>> 
)>> 
]>> 
public?? 
async?? 
Task?? 
<?? 
IActionResult?? '
>??' (
Get??) ,
(??, -
int??- 0
id??1 3
)??3 4
{@@ 	
varAA 
userIdAA 
=AA 
UserAA 
.AA 
	FindFirstAA '
(AA' (

ClaimTypesAA( 2
.AA2 3
NameIdentifierAA3 A
)AAA B
?AAB C
.AAC D
ValueAAD I
;AAI J
ifCC 
(CC 
stringCC 
.CC 
IsNullOrEmptyCC $
(CC$ %
userIdCC% +
)CC+ ,
)CC, -
{DD 
returnEE 
UnauthorizedEE #
(EE# $
)EE$ %
;EE% &
}FF 
varHH 
isOwnerHH 
=HH 
awaitHH 
_serviceHH  (
.HH( )
IsPatientOwnerAsyncHH) <
(HH< =
idHH= ?
,HH? @
userIdHHA G
)HHG H
;HHH I
ifJJ 
(JJ 
!JJ 
isOwnerJJ 
)JJ 
{KK 
returnLL 
ForbidLL 
(LL 
)LL 
;LL  
}MM 
varOO 
patientOO 
=OO 
awaitOO 
_serviceOO  (
.OO( )
GetByIdAsyncOO) 5
(OO5 6
idOO6 8
)OO8 9
;OO9 :
returnPP 
OkPP 
(PP 
patientPP 
)PP 
;PP 
}QQ 	
[TT 	
	AuthorizeTT	 
(TT 
RolesTT 
=TT 
$strTT "
)TT" #
]TT# $
[UU 	
HttpPostUU	 
]UU 
publicVV 
asyncVV 
TaskVV 
<VV 
IActionResultVV '
>VV' (
CreateVV) /
(VV/ 0
CreatePatientDtoVV0 @
dtoVVA D
)VVD E
{WW 	
varXX 
resultXX 
=XX 
awaitXX 
_serviceXX '
.XX' (
AddAsyncXX( 0
(XX0 1
dtoXX1 4
)XX4 5
;XX5 6
returnYY 
OkYY 
(YY 
resultYY 
)YY 
;YY 
}ZZ 	
[]] 	
	Authorize]]	 
(]] 
Roles]] 
=]] 
$str]] *
)]]* +
]]]+ ,
[^^ 	
HttpPut^^	 
(^^ 
$str^^ 
)^^ 
]^^ 
public__ 
async__ 
Task__ 
<__ 
IActionResult__ '
>__' (
Update__) /
(__/ 0
int__0 3
id__4 6
,__6 7
UpdatePatientDto__8 H
dto__I L
)__L M
{`` 	
ifaa 
(aa 
Useraa 
.aa 
IsInRoleaa 
(aa 
$straa %
)aa% &
)aa& '
{bb 
varcc 
adminResultcc 
=cc  !
awaitcc" '
_servicecc( 0
.cc0 1
UpdateAsynccc1 <
(cc< =
idcc= ?
,cc? @
dtoccA D
)ccD E
;ccE F
returndd 
Okdd 
(dd 
adminResultdd %
)dd% &
;dd& '
}ee 
vargg 
userIdgg 
=gg 
Usergg 
.gg 
	FindFirstgg '
(gg' (

ClaimTypesgg( 2
.gg2 3
NameIdentifiergg3 A
)ggA B
?ggB C
.ggC D
ValueggD I
;ggI J
ifii 
(ii 
stringii 
.ii 
IsNullOrEmptyii $
(ii$ %
userIdii% +
)ii+ ,
)ii, -
{jj 
returnkk 
Unauthorizedkk #
(kk# $
)kk$ %
;kk% &
}ll 
varnn 
isOwnernn 
=nn 
awaitnn 
_servicenn  (
.nn( )
IsPatientOwnerAsyncnn) <
(nn< =
idnn= ?
,nn? @
userIdnnA G
)nnG H
;nnH I
ifpp 
(pp 
!pp 
isOwnerpp 
)pp 
{qq 
returnrr 
Forbidrr 
(rr 
)rr 
;rr  
}ss 
varuu 
resultuu 
=uu 
awaituu 
_serviceuu '
.uu' (
UpdateAsyncuu( 3
(uu3 4
iduu4 6
,uu6 7
dtouu8 ;
)uu; <
;uu< =
returnvv 
Okvv 
(vv 
resultvv 
)vv 
;vv 
}ww 	
[yy 	
	Authorizeyy	 
(yy 
Rolesyy 
=yy 
$stryy $
)yy$ %
]yy% &
[zz 	
HttpGetzz	 
(zz 
$strzz &
)zz& '
]zz' (
public{{ 
async{{ 
Task{{ 
<{{ 
IActionResult{{ '
>{{' (
GetHealthRecords{{) 9
({{9 :
int{{: =
id{{> @
){{@ A
{|| 	
var}} 
userId}} 
=}} 
User}} 
.}} 
	FindFirst}} '
(}}' (

ClaimTypes}}( 2
.}}2 3
NameIdentifier}}3 A
)}}A B
?}}B C
.}}C D
Value}}D I
;}}I J
if 
( 
string 
. 
IsNullOrEmpty $
($ %
userId% +
)+ ,
), -
{
ÄÄ 
return
ÅÅ 
Unauthorized
ÅÅ #
(
ÅÅ# $
)
ÅÅ$ %
;
ÅÅ% &
}
ÇÇ 
var
ÑÑ 
isOwner
ÑÑ 
=
ÑÑ 
await
ÑÑ 
_service
ÑÑ  (
.
ÑÑ( )!
IsPatientOwnerAsync
ÑÑ) <
(
ÑÑ< =
id
ÑÑ= ?
,
ÑÑ? @
userId
ÑÑA G
)
ÑÑG H
;
ÑÑH I
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
isOwner
ÜÜ 
)
ÜÜ 
{
áá 
return
àà 
Forbid
àà 
(
àà 
)
àà 
;
àà  
}
ââ 
var
ãã 
records
ãã 
=
ãã 
await
ãã 
_service
ãã  (
.
ãã( )#
GetHealthRecordsAsync
ãã) >
(
ãã> ?
id
ãã? A
)
ããA B
;
ããB C
return
åå 
Ok
åå 
(
åå 
records
åå 
)
åå 
;
åå 
}
çç 	
}
éé 
}èè  
tC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\HealthRecordController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[		 
ApiController		 
]		 
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
 
)

  
]

  !
[ 
	Authorize 
( !
AuthenticationSchemes $
=% &
JwtBearerDefaults' 8
.8 9 
AuthenticationScheme9 M
)M N
]N O
public 

class "
HealthRecordController '
:( )
ControllerBase* 8
{ 
private 
readonly  
IHealthRecordService -
_service. 6
;6 7
public "
HealthRecordController %
(% & 
IHealthRecordService& :
service; B
)B C
{ 	
_service 
= 
service 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str +
)+ ,
], -
[ 	
HttpGet	 
( 
$str &
)& '
]' (
public 
async 
Task 
< 
IActionResult '
>' (
GetByPatient) 5
(5 6
int6 9
	patientId: C
)C D
{ 	
var 
result 
= 
await 
_service '
.' (
GetByPatientIdAsync( ;
(; <
	patientId< E
)E F
;F G
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str +
)+ ,
], -
[   	
HttpGet  	 
(   
$str   
)   
]   
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' (
GetById!!) 0
(!!0 1
int!!1 4
id!!5 7
)!!7 8
{"" 	
var## 
result## 
=## 
await## 
_service## '
.##' (
GetByIdAsync##( 4
(##4 5
id##5 7
)##7 8
;##8 9
return$$ 
Ok$$ 
($$ 
result$$ 
)$$ 
;$$ 
}%% 	
[(( 	
	Authorize((	 
((( 
Roles(( 
=(( 
$str(( #
)((# $
](($ %
[)) 	
HttpPost))	 
])) 
public** 
async** 
Task** 
<** 
IActionResult** '
>**' (
Create**) /
(**/ 0!
CreateHealthRecordDto**0 E
dto**F I
)**I J
{++ 	
var,, 
result,, 
=,, 
await,, 
_service,, '
.,,' (
AddAsync,,( 0
(,,0 1
dto,,1 4
),,4 5
;,,5 6
return-- 
Ok-- 
(-- 
result-- 
)-- 
;-- 
}.. 	
}// 
}00 ﬂ>
nC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\DoctorController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{		 
[

 
	Authorize

 
(

 !
AuthenticationSchemes

 $
=

% &
JwtBearerDefaults

' 8
.

8 9 
AuthenticationScheme

9 M
,

M N
Roles

O T
=

U V
$str

W m
)

m n
]

n o
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
DoctorController !
:" #
ControllerBase$ 2
{ 
private 
readonly 
IDoctorService '
_service( 0
;0 1
public 
DoctorController 
(  
IDoctorService  .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
[ 
	FromQuery 
] 
PaginationParams (
paginationParams) 9
,9 :
CancellationToken 
ct  
)  !
{ 	
var 
doctors 
= 
await 
_service  (
.( )
GetAllAsync) 4
(4 5
paginationParams5 E
,E F
ctG I
)I J
;J K
return 
Ok 
( 
doctors 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str #
)# $
]$ %
[ 	
HttpPut	 
( 
$str 
) 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (%
UpdateCurrentDoctorStatus  ) B
(  B C!
UpdateDoctorStatusDto!! 
dto!! 
,!! 
CancellationToken"" 
ct"" 
)"" 
{## 	
var$$ 
userId$$ 
=$$ 
User$$ 
.$$ 
	FindFirst$$ '
($$' (

ClaimTypes$$( 2
.$$2 3
NameIdentifier$$3 A
)$$A B
?$$B C
.$$C D
Value$$D I
;$$I J
if&& 
(&& 
string&& 
.&& 
IsNullOrEmpty&& $
(&&$ %
userId&&% +
)&&+ ,
)&&, -
{'' 
return(( 
Unauthorized(( #
(((# $
)(($ %
;((% &
})) 
var++ 
doctor++ 
=++ 
await++ 
_service++ '
.++' (
GetByUserIdAsync++( 8
(++8 9
userId++9 ?
,++? @
ct++A C
)++C D
;++D E
if-- 
(-- 
doctor-- 
==-- 
null-- 
)-- 
{.. 
return// 
NotFound// 
(//  
$str//  ;
)//; <
;//< =
}00 
var22 
	updateDto22 
=22 
new22 
UpdateDoctorDto22  /
{33 
FullName44 
=44 
doctor44 !
.44! "
FullName44" *
,44* +
Specialisation55 
=55  
doctor55! '
.55' (
Specialisation55( 6
,556 7
YearsOfExperience66 !
=66" #
doctor66$ *
.66* +
YearsOfExperience66+ <
,66< =
ConsultationFee77 
=77  !
doctor77" (
.77( )
ConsultationFee77) 8
,778 9
IsActive88 
=88 
dto88 
.88 
IsActive88 '
}99 
;99 
var;; 
result;; 
=;; 
await;; 
_service;; '
.;;' (
UpdateAsync;;( 3
(;;3 4
doctor;;4 :
.;;: ;
DoctorId;;; C
,;;C D
	updateDto;;E N
,;;N O
ct;;P R
);;R S
;;;S T
return== 
Ok== 
(== 
result== 
)== 
;== 
}>> 	
[?? 	
	Authorize??	 
(?? 
Roles?? 
=?? 
$str?? #
)??# $
]??$ %
[@@ 	
HttpGet@@	 
(@@ 
$str@@ 
)@@ 
]@@ 
publicAA 
asyncAA 
TaskAA 
<AA 
IActionResultAA '
>AA' (
GetCurrentDoctorAA) 9
(AA9 :
CancellationTokenAA: K
ctAAL N
)AAN O
{BB 	
varCC 
userIdCC 
=CC 
UserCC 
.CC 
	FindFirstCC '
(CC' (

ClaimTypesCC( 2
.CC2 3
NameIdentifierCC3 A
)CCA B
?CCB C
.CCC D
ValueCCD I
;CCI J
ifEE 
(EE 
stringEE 
.EE 
IsNullOrEmptyEE $
(EE$ %
userIdEE% +
)EE+ ,
)EE, -
{FF 
returnGG 
UnauthorizedGG #
(GG# $
)GG$ %
;GG% &
}HH 
varJJ 
doctorJJ 
=JJ 
awaitJJ 
_serviceJJ '
.JJ' (
GetByUserIdAsyncJJ( 8
(JJ8 9
userIdJJ9 ?
,JJ? @
ctJJA C
)JJC D
;JJD E
returnLL 
OkLL 
(LL 
doctorLL 
)LL 
;LL 
}MM 	
[PP 	
HttpGetPP	 
(PP 
$strPP 
)PP 
]PP 
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
IActionResultQQ '
>QQ' (
GetByIdQQ) 0
(QQ0 1
intQQ1 4
idQQ5 7
,QQ7 8
CancellationTokenQQ9 J
ctQQK M
)QQM N
{RR 	
varSS 
doctorSS 
=SS 
awaitSS 
_serviceSS '
.SS' (
GetByIdAsyncSS( 4
(SS4 5
idSS5 7
,SS7 8
ctSS9 ;
)SS; <
;SS< =
ifUU 
(UU 
doctorUU 
isUU 
nullUU 
)UU 
{VV 
returnWW 
NotFoundWW 
(WW  
$strWW  2
)WW2 3
;WW3 4
}XX 
returnZZ 
OkZZ 
(ZZ 
doctorZZ 
)ZZ 
;ZZ 
}[[ 	
[]] 	
HttpGet]]	 
(]] 
$str]] $
)]]$ %
]]]% &
public^^ 
async^^ 
Task^^ 
<^^ 
IActionResult^^ '
>^^' (
GetAvailability^^) 8
(^^8 9
int__ 
id__ 

,__
 
[`` 
	FromQuery`` 
]`` 
DateTime`` 
?`` 
date`` 
,`` 
CancellationTokenaa 
ctaa 
)aa 
{bb 	
varcc 
availabilityDatecc  
=cc! "
datecc# '
??cc( *
DateTimecc+ 3
.cc3 4
Todaycc4 9
;cc9 :
varee 
availabilityee 
=ee 
awaitee $
_serviceee% -
.ee- . 
GetAvailabilityAsyncee. B
(eeB C
ideeC E
,eeE F
availabilityDateeeG W
,eeW X
cteeY [
)ee[ \
;ee\ ]
returngg 
Okgg 
(gg 
availabilitygg "
)gg" #
;gg# $
}hh 	
}ii 
}jj =
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\AuthController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
AuthController 
:  !
ControllerBase" 0
{		 
private

 
readonly

 
IAuthService

 %
_service

& .
;

. /
public 
AuthController 
( 
IAuthService *
service+ 2
)2 3
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpPost	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Register) 1
(1 2
RegisterDto2 =
request> E
)E F
{ 	
var 
result 
= 
await 
_service '
.' (
Register( 0
(0 1
request1 8
)8 9
;9 :
if 
( 
! 
result 
. 
Success 
)  
{ 
return 

BadRequest !
(! "
new" %
{ 
success 
= 
false #
,# $
message 
= 
result $
.$ %
Message% ,
} 
) 
; 
} 
return 
Ok 
( 
new 
{   
success!! 
=!! 
true!! 
,!! 
message"" 
="" 
result""  
.""  !
Message""! (
,""( )
userId## 
=## 
result## 
.##  
UserId##  &
}$$ 
)$$ 
;$$ 
}%% 	
['' 	
HttpPost''	 
('' 
$str'' 
)'' 
]'' 
public(( 
async(( 
Task(( 
<(( 
IActionResult(( '
>((' (
Login(() .
(((. /
LoginDto((/ 7
request((8 ?
)((? @
{)) 	
var** 
result** 
=** 
await** 
_service** '
.**' (
Login**( -
(**- .
request**. 5
)**5 6
;**6 7
if,, 
(,, 
!,, 
result,, 
.,, 
Success,, 
),,  
{-- 
return.. 
Unauthorized.. #
(..# $
new..$ '
AuthResponse..( 4
{// 
Success00 
=00 
false00 #
,00# $
Message11 
=11 
result11 $
.11$ %
Message11% ,
,11, -
AccessToken22 
=22  !
string22" (
.22( )
Empty22) .
,22. /
RefreshToken33  
=33! "
string33# )
.33) *
Empty33* /
,33/ 0
	ExpiresIn44 
=44 
$num44  !
,44! ""
RequiresPasswordChange55 *
=55+ ,
result55- 3
.553 4"
RequiresPasswordChange554 J
}66 
)66 
;66 
}77 
return99 
Ok99 
(99 
new99 
AuthResponse99 &
{:: 
Success;; 
=;; 
true;; 
,;; 
Message<< 
=<< 
result<<  
.<<  !
Message<<! (
,<<( )
AccessToken== 
=== 
result== $
.==$ %
AccessToken==% 0
,==0 1
RefreshToken>> 
=>> 
result>> %
.>>% &
RefreshToken>>& 2
,>>2 3
	ExpiresIn?? 
=?? 
result?? "
.??" #
	ExpiresIn??# ,
,??, -"
RequiresPasswordChange@@ &
=@@' (
result@@) /
.@@/ 0"
RequiresPasswordChange@@0 F
}AA 
)AA 
;AA 
}BB 	
[DD 	
HttpPostDD	 
(DD 
$strDD #
)DD# $
]DD$ %
publicEE 
asyncEE 
TaskEE 
<EE 
IActionResultEE '
>EE' (
ChangePasswordEE) 7
(EE7 8
ChangePasswordDtoEE8 I
requestEEJ Q
)EEQ R
{FF 	
varGG 
resultGG 
=GG 
awaitGG 
_serviceGG '
.GG' (
ChangePasswordGG( 6
(GG6 7
requestGG7 >
)GG> ?
;GG? @
ifII 
(II 
!II 
resultII 
.II 
SuccessII 
)II  
{JJ 
returnKK 

BadRequestKK !
(KK! "
newKK" %
{LL 
successMM 
=MM 
falseMM #
,MM# $
messageNN 
=NN 
resultNN $
.NN$ %
MessageNN% ,
}OO 
)OO 
;OO 
}PP 
returnRR 
OkRR 
(RR 
newRR 
{SS 
successTT 
=TT 
trueTT 
,TT 
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
(ZZ5 6"
RefreshTokenRequestDtoZZ6 L
requestZZM T
)ZZT U
{[[ 	
var\\ 
result\\ 
=\\ 
await\\ 
_service\\ '
.\\' (
RefreshToken\\( 4
(\\4 5
request\\5 <
)\\< =
;\\= >
if^^ 
(^^ 
!^^ 
result^^ 
.^^ 
Success^^ 
)^^  
{__ 
return`` 
Unauthorized`` #
(``# $
new``$ '
AuthResponse``( 4
{aa 
Successbb 
=bb 
falsebb #
,bb# $
Messagecc 
=cc 
resultcc $
.cc$ %
Messagecc% ,
,cc, -
AccessTokendd 
=dd  !
stringdd" (
.dd( )
Emptydd) .
,dd. /
RefreshTokenee  
=ee! "
stringee# )
.ee) *
Emptyee* /
,ee/ 0
	ExpiresInff 
=ff 
$numff  !
,ff! ""
RequiresPasswordChangegg *
=gg+ ,
falsegg- 2
}hh 
)hh 
;hh 
}ii 
returnkk 
Okkk 
(kk 
newkk 
AuthResponsekk &
{ll 
Successmm 
=mm 
truemm 
,mm 
Messagenn 
=nn 
resultnn  
.nn  !
Messagenn! (
,nn( )
AccessTokenoo 
=oo 
resultoo $
.oo$ %
AccessTokenoo% 0
,oo0 1
RefreshTokenpp 
=pp 
resultpp %
.pp% &
RefreshTokenpp& 2
,pp2 3
	ExpiresInqq 
=qq 
resultqq "
.qq" #
	ExpiresInqq# ,
,qq, -"
RequiresPasswordChangerr &
=rr' (
falserr) .
}ss 
)ss 
;ss 
}tt 	
}uu 
}vv ú#
sC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\AppointmentController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[		 
ApiController		 
]		 
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
 
public 

class !
AppointmentController &
:' (
ControllerBase) 7
{ 
private 
readonly 
IAppointmentService ,
_service- 5
;5 6
public !
AppointmentController $
($ %
IAppointmentService% 8
service9 @
)@ A
{ 	
_service 
= 
service 
; 
} 	
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ q
)q r
]r s
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
)- .
{ 	
var 
result 
= 
await 
_service '
.' (
GetAllAsync( 3
(3 4
)4 5
;5 6
return 
Ok 
( 
result 
) 
; 
} 	
[!! 	
	Authorize!!	 
(!! !
AuthenticationSchemes!! (
=!!) *
JwtBearerDefaults!!+ <
.!!< = 
AuthenticationScheme!!= Q
,!!Q R
Roles!!S X
=!!Y Z
$str!![ d
)!!d e
]!!e f
["" 	
HttpPost""	 
]"" 
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
Create##) /
(##/ 0 
CreateAppointmentDto##0 D
dto##E H
)##H I
{$$ 	
var%% 
result%% 
=%% 
await%% 
_service%% '
.%%' (
AddAsync%%( 0
(%%0 1
dto%%1 4
)%%4 5
;%%5 6
return&& 
Ok&& 
(&& 
result&& 
)&& 
;&& 
}'' 	
[,, 	
	Authorize,,	 
(,, !
AuthenticationSchemes,, (
=,,) *
JwtBearerDefaults,,+ <
.,,< = 
AuthenticationScheme,,= Q
,,,Q R
Roles,,S X
=,,Y Z
$str,,[ q
),,q r
],,r s
[-- 	
HttpPut--	 
(-- 
$str-- 
)-- 
]--  
public.. 
async.. 
Task.. 
<.. 
IActionResult.. '
>..' (
UpdateStatus..) 5
(..5 6
int..6 9
id..: <
,..< =&
UpdateAppointmentStatusDto..> X
dto..Y \
)..\ ]
{// 	
var00 
result00 
=00 
await00 
_service00 '
.00' (
UpdateStatusAsync00( 9
(009 :
id00: <
,00< =
dto00> A
)00A B
;00B C
return11 
Ok11 
(11 
result11 
)11 
;11 
}22 	
[77 	
	Authorize77	 
(77 !
AuthenticationSchemes77 (
=77) *
JwtBearerDefaults77+ <
.77< = 
AuthenticationScheme77= Q
,77Q R
Roles77S X
=77Y Z
$str77[ d
)77d e
]77e f
[88 	

HttpDelete88	 
(88 
$str88 
)88 
]88 
public99 
async99 
Task99 
<99 
IActionResult99 '
>99' (
Delete99) /
(99/ 0
int990 3
id994 6
)996 7
{:: 	
var;; 
result;; 
=;; 
await;; 
_service;; '
.;;' (
DeleteAsync;;( 3
(;;3 4
id;;4 6
);;6 7
;;;7 8
return<< 
Ok<< 
(<< 
result<< 
)<< 
;<< 
}== 	
}>> 
}?? ¿"
mC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\AdminController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[		 
	Authorize		 
(		 !
AuthenticationSchemes		 $
=		% &
JwtBearerDefaults		' 8
.		8 9 
AuthenticationScheme		9 M
,		M N
Roles		O T
=		U V
$str		W ^
)		^ _
]		_ `
[

 
ApiController

 
]

 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
AdminController  
:! "
ControllerBase# 1
{ 
private 
readonly 
IDoctorService '
_doctorService( 6
;6 7
private 
readonly 
IAppointmentService ,
_appointmentService- @
;@ A
public 
AdminController 
( 
IDoctorService 
doctorService (
,( )
IAppointmentService 
appointmentService  2
)2 3
{ 	
_doctorService 
= 
doctorService *
;* +
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (

GetDoctors) 3
(3 4
[4 5
	FromQuery5 >
]> ?

HealthAxis@ J
.J K
SharedK Q
.Q R
DTOsR V
.V W
CommonW ]
.] ^
PaginationParams^ n
paginationParamso 
,	 Ä
CancellationToken
Å í
ct
ì ï
)
ï ñ
{ 	
var 
result 
= 
await 
_doctorService -
.- .
GetAllAsync. 9
(9 :
paginationParams: J
,J K
ctL N
)N O
;O P
return 
Ok 
( 
result 
) 
; 
} 	
[   	
HttpPost  	 
(   
$str   
)   
]   
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' (
CreateDoctor!!) 5
(!!5 6
CreateDoctorDto!!6 E
dto!!F I
)!!I J
{"" 	
var## 
result## 
=## 
await## 
_doctorService## -
.##- .
AddAsync##. 6
(##6 7
dto##7 :
)##: ;
;##; <
return$$ 
Ok$$ 
($$ 
result$$ 
)$$ 
;$$ 
}%% 	
['' 	
HttpPut''	 
('' 
$str'' 
)''  
]''  !
public(( 
async(( 
Task(( 
<(( 
IActionResult(( '
>((' (
UpdateDoctor(() 5
(((5 6
int((6 9
id((: <
,((< =
UpdateDoctorDto((> M
dto((N Q
)((Q R
{)) 	
var** 
result** 
=** 
await** 
_doctorService** -
.**- .
UpdateAsync**. 9
(**9 :
id**: <
,**< =
dto**> A
)**A B
;**B C
return++ 
Ok++ 
(++ 
result++ 
)++ 
;++ 
},, 	
[.. 	
HttpGet..	 
(.. 
$str.. '
)..' (
]..( )
public// 
async// 
Task// 
<// 
IActionResult// '
>//' (

GetReports//) 3
(//3 4
)//4 5
{00 	
var11 
result11 
=11 
await11 
_appointmentService11 2
.112 3
GetAllAsync113 >
(11> ?
)11? @
;11@ A
return22 
Ok22 
(22 
result22 
)22 
;22 
}33 	
}44 
}55 å,
uC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Consumers\AppointmentBookedConsumer.cs
	namespace 	

HealthAxis
 
. 
API 
. 
	Consumers "
;" #
public 
sealed 
class %
AppointmentBookedConsumer -
:. /
	IConsumer0 9
<9 :"
AppointmentBookedEvent: P
>P Q
{		 
private

 
readonly

 
HealthAxisDbContext

 (

_dbContext

) 3
;

3 4
private 
readonly 
ILogger 
< %
AppointmentBookedConsumer 6
>6 7
_logger8 ?
;? @
public 
%
AppointmentBookedConsumer $
($ %
HealthAxisDbContext 
	dbContext %
,% &
ILogger 
< %
AppointmentBookedConsumer )
>) *
logger+ 1
)1 2
{ 

_dbContext 
= 
	dbContext 
; 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
Consume 
( 
ConsumeContext ,
<, -"
AppointmentBookedEvent- C
>C D
contextE L
)L M
{ 
var 
appointmentEvent 
= 
context &
.& '
Message' .
;. /
var !
formattedEventMessage !
=" #
$str -
+. /
Environment0 ;
.; <
NewLine< C
+D E
$str (
+) *
Environment+ 6
.6 7
NewLine7 >
+? @
$str -
+. /
Environment0 ;
.; <
NewLine< C
+D E
$" 
$str 
{ 
appointmentEvent -
.- .
	EventType. 7
}7 8
"8 9
+: ;
Environment< G
.G H
NewLineH O
+P Q
$" 
$str 
{ 
appointmentEvent -
.- .
AppointmentId. ;
}; <
"< =
+> ?
Environment@ K
.K L
NewLineL S
+T U
$" 
$str 
{ 
appointmentEvent -
.- .
PatientName. 9
}9 :
": ;
+< =
Environment> I
.I J
NewLineJ Q
+R S
$"   
$str   
{   
appointmentEvent   -
.  - .
DoctorId  . 6
}  6 7
"  7 8
+  9 :
Environment  ; F
.  F G
NewLine  G N
+  O P
$"!! 
$str!! 
{!! 
appointmentEvent!! -
.!!- .
ScheduledDate!!. ;
:!!; <
$str!!< G
}!!G H
"!!H I
+!!J K
Environment!!L W
.!!W X
NewLine!!X _
+!!` a
$""" 
$str"" 
{"" 
appointmentEvent"" -
.""- .
TimeSlot"". 6
}""6 7
"""7 8
+""9 :
Environment""; F
.""F G
NewLine""G N
+""O P
$"## 
$str## 
{## 
appointmentEvent## -
.##- .

OccurredAt##. 8
:##8 9
$str##9 M
}##M N
"##N O
+##P Q
Environment##R ]
.##] ^
NewLine##^ e
+##f g
$str$$ -
;$$- .
_logger&& 
.&& 
LogInformation&& 
(&& 
$str&& :
,&&: ;!
formattedEventMessage&&< Q
)&&Q R
;&&R S
var(( 
notification(( 
=(( 
new(( 
Notification(( +
{)) 	
DoctorId** 
=** 
appointmentEvent** '
.**' (
DoctorId**( 0
,**0 1
Message++ 
=++ 
$"++ 
$str++ 2
{++2 3
appointmentEvent++3 C
.++C D
PatientName++D O
}++O P
$str++P T
{++T U
appointmentEvent++U e
.++e f
ScheduledDate++f s
:++s t
$str++t 
}	++ Ä
$str
++Ä Ñ
{
++Ñ Ö
appointmentEvent
++Ö ï
.
++ï ñ
TimeSlot
++ñ û
}
++û ü
$str
++ü †
"
++† °
,
++° ¢
IsRead,, 
=,, 
false,, 
,,, 
	CreatedAt-- 
=-- 
DateTime--  
.--  !
UtcNow--! '
}.. 	
;..	 

await00 

_dbContext00 
.00 
Notifications00 &
.00& '
AddAsync00' /
(00/ 0
notification000 <
,00< =
context00> E
.00E F
CancellationToken00F W
)00W X
;00X Y
await11 

_dbContext11 
.11 
SaveChangesAsync11 )
(11) *
context11* 1
.111 2
CancellationToken112 C
)11C D
;11D E
_logger33 
.33 
LogInformation33 
(33 
$str44 \
,44\ ]
appointmentEvent55 
.55 
DoctorId55 %
,55% &
appointmentEvent66 
.66 
AppointmentId66 *
)66* +
;66+ ,
}77 
}88 ∑!
C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\BackgroundServices\NotificationCleanupService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
BackgroundServices +
;+ ,
public 
sealed 
class &
NotificationCleanupService .
(. / 
IServiceScopeFactory 
scopeFactory %
,% &
ILogger 
< &
NotificationCleanupService &
>& '
logger( .
). /
:0 1
BackgroundService2 C
{		 
	protected

 
override

 
async

 
Task

 !
ExecuteAsync

" .
(

. /
CancellationToken

/ @
stoppingToken

A N
)

N O
{ 
logger 
. 
LogInformation 
( 
$str C
)C D
;D E
try 
{ 	
while 
( 
! 
stoppingToken !
.! "#
IsCancellationRequested" 9
)9 :
{ 
await (
CleanupOldNotificationsAsync 2
(2 3
stoppingToken3 @
)@ A
;A B
await 
Task 
. 
Delay  
(  !
TimeSpan! )
.) *
	FromHours* 3
(3 4
$num4 5
)5 6
,6 7
stoppingToken8 E
)E F
;F G
} 
} 	
catch 
( &
OperationCanceledException )
)) *
{ 	
logger 
. 
LogInformation !
(! "
$str" V
)V W
;W X
} 	
logger 
. 
LogInformation 
( 
$str C
)C D
;D E
} 
private 
async 
Task (
CleanupOldNotificationsAsync 3
(3 4
CancellationToken4 E
stoppingTokenF S
)S T
{   
using!! 
var!! 
scope!! 
=!! 
scopeFactory!! &
.!!& '
CreateScope!!' 2
(!!2 3
)!!3 4
;!!4 5
var## 
	dbContext## 
=## 
scope## 
.## 
ServiceProvider## -
.##- .
GetRequiredService##. @
<##@ A
HealthAxisDbContext##A T
>##T U
(##U V
)##V W
;##W X
var%% 

cutoffDate%% 
=%% 
DateTime%% !
.%%! "
UtcNow%%" (
.%%( )
AddDays%%) 0
(%%0 1
-%%1 2
$num%%2 4
)%%4 5
;%%5 6
var'' 
oldNotifications'' 
='' 
await'' $
	dbContext''% .
.''. /
Notifications''/ <
.(( 
Where(( 
((( 
notification(( 
=>((  "
notification((# /
.((/ 0
	CreatedAt((0 9
<((: ;

cutoffDate((< F
)((F G
.)) 
ToListAsync)) 
()) 
stoppingToken)) &
)))& '
;))' (
if++ 

(++ 
oldNotifications++ 
.++ 
Count++ "
==++# %
$num++& '
)++' (
{,, 	
logger-- 
.-- 
LogInformation-- !
(--! "
$str--" K
)--K L
;--L M
return.. 
;.. 
}// 	
	dbContext11 
.11 
Notifications11 
.11  
RemoveRange11  +
(11+ ,
oldNotifications11, <
)11< =
;11= >
await33 
	dbContext33 
.33 
SaveChangesAsync33 (
(33( )
stoppingToken33) 6
)336 7
;337 8
logger55 
.55 
LogInformation55 
(55 
$str66 N
,66N O
oldNotifications77 
.77 
Count77 "
,77" #

cutoffDate88 
)88 
;88 
}99 
}:: ®
uC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\BackgroundServices\HeartbeatService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
BackgroundServices +
;+ ,
public 
sealed 
class 
HeartbeatService $
($ %
ILogger% ,
<, -
HeartbeatService- =
>= >
logger? E
)E F
:G H
BackgroundServiceI Z
{ 
	protected 
override 
async 
Task !
ExecuteAsync" .
(. /
CancellationToken/ @
stoppingTokenA N
)N O
{ 
logger 
. 
LogInformation 
( 
$str 9
)9 :
;: ;
try		 
{

 	
while 
( 
! 
stoppingToken !
.! "#
IsCancellationRequested" 9
)9 :
{ 
logger 
. 
LogInformation %
(% &
$str& R
,R S
DateTimeOffsetT b
.b c
Nowc f
)f g
;g h
await 
Task 
. 
Delay  
(  !
TimeSpan! )
.) *
FromSeconds* 5
(5 6
$num6 8
)8 9
,9 :
stoppingToken; H
)H I
;I J
} 
} 	
catch 
( &
OperationCanceledException )
)) *
{ 	
logger 
. 
LogInformation !
(! "
$str" L
)L M
;M N
} 	
logger 
. 
LogInformation 
( 
$str 9
)9 :
;: ;
} 
} 