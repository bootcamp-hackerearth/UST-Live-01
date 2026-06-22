à&
nC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Utilities\ValidationMessages.cs
	namespace 	

HealthAxis
 
. 
API 
. 
	Utilities "
{ 
public 

static 
class 
ValidationMessages *
{ 
public 
const 
string 
FullNameRequired ,
=- .
$str/ G
;G H
public 
const 
string !
InvalidFullNameFormat 1
=2 3
$str4 g
;g h
public

 
const

 
string

 
DateOfBirthRequired

 /
=

0 1
$str

2 N
;

N O
public 
const 
string %
DateOfBirthCannotBeFuture 5
=6 7
$str8 `
;` a
public 
const 
string ,
 DateOfBirthYearMustBe1900OrLater <
== >
$str? j
;j k
public 
const 
string 
GenderRequired *
=+ ,
$str- B
;B C
public 
const 
string 
PhoneNumberRequired /
=0 1
$str2 M
;M N
public 
const 
string $
InvalidPhoneNumberFormat 4
=5 6
$str7 e
;e f
public 
const 
string 
EmailRequired )
=* +
$str, @
;@ A
public 
const 
string 
InvalidEmailFormat .
=/ 0
$str1 V
;V W
public 
const 
string "
SpecialisationRequired 2
=3 4
$str5 R
;R S
public 
const 
string "
InvalidExperienceRange 2
=3 4
$str5 d
;d e
public 
const 
string "
InvalidConsultationFee 2
=3 4
$str5 d
;d e
public   
const   
string   
PatientRequired   +
=  , -
$str  . M
;  M N
public"" 
const"" 
string"" 
DoctorRequired"" *
=""+ ,
$str""- K
;""K L
public$$ 
const$$ 
string$$ !
ScheduledDateRequired$$ 1
=$$2 3
$str$$4 Q
;$$Q R
public&& 
const&& 
string&& 
TimeSlotRequired&& ,
=&&- .
$str&&/ G
;&&G H
public(( 
const(( 
string(( %
AppointmentStatusRequired(( 5
=((6 7
$str((8 Y
;((Y Z
public** 
const** 
string** 
VisitDateRequired** -
=**. /
$str**0 I
;**I J
public,, 
const,, 
string,, 
DiagnosisRequired,, -
=,,. /
$str,,0 H
;,,H I
public.. 
const.. 
string..  
PrescriptionRequired.. 0
=..1 2
$str..3 N
;..N O
public00 
const00 
string00  
ProviderNameRequired00 0
=001 2
$str003 O
;00O P
public22 
const22 
string22  
PolicyNumberRequired22 0
=221 2
$str223 O
;22O P
public44 
const44 
string44 
ExpiryDateRequired44 .
=44/ 0
$str441 K
;44K L
public66 
const66 
string66 #
InsuranceStatusRequired66 3
=664 5
$str666 U
;66U V
public88 
const88 
string88 
PasswordRequired88 ,
=88- .
$str88/ F
;88F G
public:: 
const:: 
string:: 
UserRoleRequired:: ,
=::- .
$str::/ G
;::G H
public<< 
const<< 
string<< 
ReferenceIdRequired<< /
=<<0 1
$str<<2 M
;<<M N
public>> 
const>> 
string>> %
ScheduledDateCannotBePast>> 5
=>>6 7
$str>>8 ^
;>>^ _
public@@ 
const@@ 
string@@ #
AppointmentDateRequired@@ 3
=@@4 5
$str@@6 U
;@@U V
publicBB 
constBB 
stringBB 
InvalidTimeSlotBB +
=BB, -
$strBB. g
;BBg h
publicDD 
constDD 
stringDD 
AppointmentRequiredDD /
=DD0 1
$strDD2 U
;DDU V
}FF 
}GG  
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Utilities\ValidationLimits.cs
	namespace 	

HealthAxis
 
. 
API 
. 
	Utilities "
{ 
public 

class 
ValidationLimits !
{ 
public 
const 
int 
FullNameLength '
=( )
$num* -
;- .
public 
const 
int 
EmailLength $
=% &
$num' *
;* +
public		 
const		 
int		 
PhoneNumberLength		 *
=		+ ,
$num		- /
;		/ 0
public 
const 
int 
PolicyNumberLength +
=, -
$num. 0
;0 1
public 
const 
int 
ProviderNameLength +
=, -
$num. 1
;1 2
public 
const 
int 
TimeSlotLength '
=( )
$num* ,
;, -
public 
const 
int $
CancellationReasonLength 1
=2 3
$num4 7
;7 8
public 
const 
int 
DiagnosisLength (
=) *
$num+ .
;. /
public 
const 
int 
PrescriptionLength +
=, -
$num. 1
;1 2
public 
const 
int 
NotesLength $
=% &
$num' +
;+ ,
public 
const 
int 
MinExperience &
=' (
$num) *
;* +
public 
const 
int 
MaxExperience &
=' (
$num) +
;+ ,
public 
const 
string 
MinCoverageAmount -
=. /
$str0 3
;3 4
public 
const 
string 
MaxCoverageAmount -
=. /
$str0 ;
;; <
public!! 
const!! 
string!! 
MinConsultationFee!! .
=!!/ 0
$str!!1 7
;!!7 8
public## 
const## 
string## 
MaxConsultationFee## .
=##/ 0
$str##1 8
;##8 9
}$$ 
}%% ›
iC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Utilities\RegexPatterns.cs
	namespace 	

HealthAxis
 
. 
API 
. 
	Utilities "
{ 
public 

class 
RegexPatterns 
{ 
public 
const 
string 
FullName $
=% &
$str' 7
;7 8
public 
const 
string 
PhoneNumber '
=( )
$str* 5
;5 6
} 
}		 ˛
uC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IPatientService.cs
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
< 

PatientDto 
> 
UpdateAsync $
($ %
int% (
id) +
,+ ,
UpdatePatientDto- =
dto> A
)A B
;B C
Task 
< 
IEnumerable 
< 
HealthRecordDto (
>( )
>) *!
GetHealthRecordsAsync+ @
(@ A
intA D
	patientIdE N
)N O
;O P
Task 
< 
IEnumerable 
< 

PatientDto #
># $
>$ %
SearchByNameAsync& 7
(7 8
string8 >
name? C
)C D
;D E
Task 
< 

PatientDto 
? 
> 
GetByEmailAsync )
() *
string* 0
email1 6
)6 7
;7 8
Task 
< 

PatientDto 
? 
> 
GetByPhoneAsync )
() *
string* 0
phone1 6
)6 7
;7 8
} 
} ç
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
} ú
tC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Interfaces\IDoctorService.cs
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
(0 1
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task

 
<

 
	DoctorDto

 
?

 
>

 
GetByIdAsync

 %
(

% &
int 
id 
, 
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< 
object 
>  
GetAvailabilityAsync )
() *
int* -
id. 0
)0 1
;1 2
Task 
< 
	DoctorDto 
> 
AddAsync  
(  !
CreateDoctorDto! 0
dto1 4
)4 5
;5 6
Task 
< 
	DoctorDto 
> 
UpdateAsync #
(# $
int$ '
id( *
,* +
UpdateDoctorDto, ;
dto< ?
)? @
;@ A
} 
} â
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
} ∆L
yC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\PatientService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "
Implementations" 1
{		 
public

 

class

 
PatientService

 
:

  !
IPatientService

" 1
{ 
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_patientRepository. @
;@ A
private 
readonly 
IRepository $
<$ %
Appointment% 0
>0 1"
_appointmentRepository2 H
;H I
private 
readonly 
IRepository $
<$ %
HealthRecord% 1
>1 2#
_healthRecordRepository3 J
;J K
private 
readonly 
IMapper  
_mapper! (
;( )
public 
PatientService 
( 
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
IRepository 
< 
Appointment #
># $!
appointmentRepository% :
,: ;
IRepository 
< 
HealthRecord $
>$ %"
healthRecordRepository& <
,< =
IMapper 
mapper 
) 
{ 	
_patientRepository 
=  
patientRepository! 2
;2 3"
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;#
_healthRecordRepository #
=$ %"
healthRecordRepository& <
;< =
_mapper 
= 
mapper 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &

PatientDto& 0
>0 1
>1 2
GetAllAsync3 >
(> ?
)? @
{ 	
var   
patients   
=   
await    
_patientRepository  ! 3
.  3 4
GetAllAsync  4 ?
(  ? @
)  @ A
;  A B
return!! 
_mapper!! 
.!! 
Map!! 
<!! 
IEnumerable!! *
<!!* +

PatientDto!!+ 5
>!!5 6
>!!6 7
(!!7 8
patients!!8 @
)!!@ A
;!!A B
}"" 	
public%% 
async%% 
Task%% 
<%% 

PatientDto%% $
?%%$ %
>%%% &
GetByIdAsync%%' 3
(%%3 4
int%%4 7
id%%8 :
)%%: ;
{&& 	
var'' 
patient'' 
='' 
await'' 
_patientRepository''  2
.''2 3
GetByIdAsync''3 ?
(''? @
id''@ B
)''B C
;''C D
if)) 
()) 
patient)) 
==)) 
null)) 
)))  
throw** 
new** 
NotFoundException** +
(**+ ,
$str**, ?
)**? @
;**@ A
return,, 
_mapper,, 
.,, 
Map,, 
<,, 

PatientDto,, )
>,,) *
(,,* +
patient,,+ 2
),,2 3
;,,3 4
}-- 	
public00 
async00 
Task00 
<00 

PatientDto00 $
>00$ %
UpdateAsync00& 1
(001 2
int002 5
id006 8
,008 9
UpdatePatientDto00: J
dto00K N
)00N O
{11 	
var22 
patient22 
=22 
await22 
_patientRepository22  2
.222 3
GetByIdAsync223 ?
(22? @
id22@ B
)22B C
;22C D
if44 
(44 
patient44 
==44 
null44 
)44  
throw55 
new55 
NotFoundException55 +
(55+ ,
$str55, ?
)55? @
;55@ A
_mapper77 
.77 
Map77 
(77 
dto77 
,77 
patient77 $
)77$ %
;77% &
await99 
_patientRepository99 $
.99$ %
UpdateAsync99% 0
(990 1
id991 3
,993 4
patient995 <
,99< =
CancellationToken99> O
.99O P
None99P T
)99T U
;99U V
return;; 
_mapper;; 
.;; 
Map;; 
<;; 

PatientDto;; )
>;;) *
(;;* +
patient;;+ 2
);;2 3
;;;3 4
}<< 	
public?? 
async?? 
Task?? 
<?? 
IEnumerable?? %
<??% &
HealthRecordDto??& 5
>??5 6
>??6 7!
GetHealthRecordsAsync??8 M
(??M N
int??N Q
	patientId??R [
)??[ \
{@@ 	
varAA 
patientAA 
=AA 
awaitAA 
_patientRepositoryAA  2
.AA2 3
GetByIdAsyncAA3 ?
(AA? @
	patientIdAA@ I
)AAI J
;AAJ K
ifCC 
(CC 
patientCC 
==CC 
nullCC 
)CC  
throwDD 
newDD 
NotFoundExceptionDD +
(DD+ ,
$strDD, ?
)DD? @
;DD@ A
varFF 
recordsFF 
=FF 
awaitFF #
_healthRecordRepositoryFF  7
.FF7 8
GetAllAsyncFF8 C
(FFC D
)FFD E
;FFE F
varHH 
patientRecordsHH 
=HH  
recordsHH! (
.II 
WhereII 
(II 
rII 
=>II 
rII 
.II 
	PatientIdII '
==II( *
	patientIdII+ 4
)II4 5
;II5 6
returnKK 
_mapperKK 
.KK 
MapKK 
<KK 
IEnumerableKK *
<KK* +
HealthRecordDtoKK+ :
>KK: ;
>KK; <
(KK< =
patientRecordsKK= K
)KKK L
;KKL M
}LL 	
publicOO 
asyncOO 
TaskOO 
<OO 
IEnumerableOO %
<OO% &

PatientDtoOO& 0
>OO0 1
>OO1 2
SearchByNameAsyncOO3 D
(OOD E
stringOOE K
nameOOL P
)OOP Q
{PP 	
varQQ 
patientsQQ 
=QQ 
awaitQQ  
_patientRepositoryQQ! 3
.QQ3 4
GetAllAsyncQQ4 ?
(QQ? @
)QQ@ A
;QQA B
varSS 
resultSS 
=SS 
patientsSS !
.TT 
WhereTT 
(TT 
pTT 
=>TT 
pTT 
.TT 
FullNameTT &
.TT& '
ContainsTT' /
(TT/ 0
nameTT0 4
,TT4 5
StringComparisonTT6 F
.TTF G
OrdinalIgnoreCaseTTG X
)TTX Y
)TTY Z
;TTZ [
returnVV 
_mapperVV 
.VV 
MapVV 
<VV 
IEnumerableVV *
<VV* +

PatientDtoVV+ 5
>VV5 6
>VV6 7
(VV7 8
resultVV8 >
)VV> ?
;VV? @
}WW 	
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 

PatientDtoZZ $
?ZZ$ %
>ZZ% &
GetByEmailAsyncZZ' 6
(ZZ6 7
stringZZ7 =
emailZZ> C
)ZZC D
{[[ 	
var\\ 
patients\\ 
=\\ 
await\\  
_patientRepository\\! 3
.\\3 4
GetAllAsync\\4 ?
(\\? @
)\\@ A
;\\A B
var^^ 
patient^^ 
=^^ 
patients^^ "
.__ 
FirstOrDefault__ 
(__  
p__  !
=>__" $
p__% &
.__& '
Email__' ,
==__- /
email__0 5
)__5 6
;__6 7
ifaa 
(aa 
patientaa 
==aa 
nullaa 
)aa  
throwbb 
newbb 
NotFoundExceptionbb +
(bb+ ,
$strbb, ?
)bb? @
;bb@ A
returndd 
_mapperdd 
.dd 
Mapdd 
<dd 

PatientDtodd )
>dd) *
(dd* +
patientdd+ 2
)dd2 3
;dd3 4
}ee 	
publichh 
asynchh 
Taskhh 
<hh 

PatientDtohh $
?hh$ %
>hh% &
GetByPhoneAsynchh' 6
(hh6 7
stringhh7 =
phonehh> C
)hhC D
{ii 	
varjj 
patientsjj 
=jj 
awaitjj  
_patientRepositoryjj! 3
.jj3 4
GetAllAsyncjj4 ?
(jj? @
)jj@ A
;jjA B
varll 
patientll 
=ll 
patientsll "
.mm 
FirstOrDefaultmm 
(mm  
pmm  !
=>mm" $
pmm% &
.mm& '
PhoneNumbermm' 2
==mm3 5
phonemm6 ;
)mm; <
;mm< =
ifoo 
(oo 
patientoo 
==oo 
nulloo 
)oo  
throwpp 
newpp 
NotFoundExceptionpp +
(pp+ ,
$strpp, ?
)pp? @
;pp@ A
returnrr 
_mapperrr 
.rr 
Maprr 
<rr 

PatientDtorr )
>rr) *
(rr* +
patientrr+ 2
)rr2 3
;rr3 4
}ss 	
}tt 
}uu ú7
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
}dd ∆Q
xC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\DoctorService.cs
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
class 
DoctorService 
:  
IDoctorService! /
{ 
private 
readonly 
IDoctorRepository *
_doctorRepository+ <
;< =
private 
readonly 
UserManager $
<$ %
ApplicationUser% 4
>4 5
_userManager6 B
;B C
private 
readonly 
IMapper  
_mapper! (
;( )
public 
DoctorService 
( 
IDoctorRepository 

repository (
,( )
UserManager 
< 
ApplicationUser '
>' (
userManager) 4
,4 5
IMapper 
mapper 
) 
{ 	
_doctorRepository 
= 

repository  *
;* +
_userManager 
= 
userManager &
;& '
_mapper 
= 
mapper 
; 
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
	DoctorDto& /
>/ 0
>0 1
GetAllAsync2 =
(= >
CancellationToken> O
ctP R
=S T
defaultU \
)\ ]
{ 	
var 
doctors 
= 
await 
_doctorRepository  1
.1 2
GetAllAsync2 =
(= >
)> ?
;? @
return   
_mapper   
.   
Map   
<   
IEnumerable   *
<  * +
	DoctorDto  + 4
>  4 5
>  5 6
(  6 7
doctors  7 >
)  > ?
;  ? @
}!! 	
public$$ 
async$$ 
Task$$ 
<$$ 
	DoctorDto$$ #
?$$# $
>$$$ %
GetByIdAsync$$& 2
($$2 3
int$$3 6
id$$7 9
,$$9 :
CancellationToken$$; L
ct$$M O
=$$P Q
default$$R Y
)$$Y Z
{%% 	
var&& 
doctor&& 
=&& 
await&& 
_doctorRepository&& 0
.&&0 1
GetByIdAsync&&1 =
(&&= >
id&&> @
)&&@ A
;&&A B
if(( 
((( 
doctor(( 
==(( 
null(( 
)(( 
{)) 
throw** 
new** 
NotFoundException** +
(**+ ,
$str**, >
)**> ?
;**? @
}++ 
return-- 
_mapper-- 
.-- 
Map-- 
<-- 
	DoctorDto-- (
>--( )
(--) *
doctor--* 0
)--0 1
;--1 2
}.. 	
public11 
async11 
Task11 
<11 
object11  
>11  ! 
GetAvailabilityAsync11" 6
(116 7
int117 :
id11; =
)11= >
{22 	
var33 
doctor33 
=33 
await33 
_doctorRepository33 0
.330 1
GetByIdAsync331 =
(33= >
id33> @
)33@ A
;33A B
if55 
(55 
doctor55 
==55 
null55 
)55 
{66 
throw77 
new77 
NotFoundException77 +
(77+ ,
$str77, >
)77> ?
;77? @
}88 
return:: 
new:: 
{;; 
doctorId<< 
=<< 
doctor<< !
.<<! "
DoctorId<<" *
,<<* +

doctorName== 
=== 
doctor== #
.==# $
FullName==$ ,
,==, -
isActive>> 
=>> 
doctor>> !
.>>! "
IsActive>>" *
,>>* +
availableSlots?? 
=??  
doctor??! '
.??' (
IsActive??( 0
?@@ 
new@@ 
List@@ 
<@@ 
string@@ %
>@@% &
{AA 
$strBB "
,BB" #
$strCC "
,CC" #
$strDD "
,DD" #
$strEE "
,EE" #
$strFF "
}GG 
:HH 
newHH 
ListHH 
<HH 
stringHH %
>HH% &
(HH& '
)HH' (
}II 
;II 
}JJ 	
publicMM 
asyncMM 
TaskMM 
<MM 
	DoctorDtoMM #
>MM# $
AddAsyncMM% -
(MM- .
CreateDoctorDtoMM. =
dtoMM> A
)MMA B
{NN 	
varOO 
existingUserOO 
=OO 
awaitOO $
_userManagerOO% 1
.OO1 2
FindByEmailAsyncOO2 B
(OOB C
dtoOOC F
.OOF G
EmailOOG L
)OOL M
;OOM N
ifQQ 
(QQ 
existingUserQQ 
!=QQ 
nullQQ  $
)QQ$ %
{RR 
throwSS 
newSS !
BusinessRuleExceptionSS /
(SS/ 0
$strSS0 ^
)SS^ _
;SS_ `
}TT 
varVV 

doctorUserVV 
=VV 
newVV  
ApplicationUserVV! 0
{WW 
UserNameXX 
=XX 
dtoXX 
.XX 
EmailXX $
,XX$ %
EmailYY 
=YY 
dtoYY 
.YY 
EmailYY !
,YY! "
EmailConfirmedZZ 
=ZZ  
trueZZ! %
,ZZ% &
MustChangePassword[[ "
=[[# $
true[[% )
}\\ 
;\\ 
var^^ 
createUserResult^^  
=^^! "
await^^# (
_userManager^^) 5
.^^5 6
CreateAsync^^6 A
(^^A B

doctorUser__ 
,__ 
dto`` 
.`` 
TemporaryPassword`` %
)``% &
;``& '
ifbb 
(bb 
!bb 
createUserResultbb !
.bb! "
	Succeededbb" +
)bb+ ,
{cc 
vardd 
errorsdd 
=dd 
stringdd #
.dd# $
Joindd$ (
(dd( )
$strdd) -
,dd- .
createUserResultdd/ ?
.dd? @
Errorsdd@ F
.ddF G
SelectddG M
(ddM N
eddN O
=>ddP R
eddS T
.ddT U
DescriptionddU `
)dd` a
)dda b
;ddb c
throwee 
newee 
ValidationExceptionee -
(ee- .
errorsee. 4
)ee4 5
;ee5 6
}ff 
varhh 

roleResulthh 
=hh 
awaithh "
_userManagerhh# /
.hh/ 0
AddToRoleAsynchh0 >
(hh> ?

doctorUserhh? I
,hhI J
$strhhK S
)hhS T
;hhT U
ifjj 
(jj 
!jj 

roleResultjj 
.jj 
	Succeededjj %
)jj% &
{kk 
varll 
errorsll 
=ll 
stringll #
.ll# $
Joinll$ (
(ll( )
$strll) -
,ll- .

roleResultll/ 9
.ll9 :
Errorsll: @
.ll@ A
SelectllA G
(llG H
ellH I
=>llJ L
ellM N
.llN O
DescriptionllO Z
)llZ [
)ll[ \
;ll\ ]
throwmm 
newmm 
ValidationExceptionmm -
(mm- .
errorsmm. 4
)mm4 5
;mm5 6
}nn 
varpp 
doctorpp 
=pp 
newpp 
Doctorpp #
{qq 
FullNamerr 
=rr 
dtorr 
.rr 
FullNamerr '
,rr' (
Specialisationss 
=ss  
dtoss! $
.ss$ %
Specialisationss% 3
,ss3 4
YearsOfExperiencett !
=tt" #
dtott$ '
.tt' (
YearsOfExperiencett( 9
,tt9 :
ConsultationFeeuu 
=uu  !
dtouu" %
.uu% &
ConsultationFeeuu& 5
,uu5 6
IsActivevv 
=vv 
truevv 
}ww 
;ww 
awaityy 
_doctorRepositoryyy #
.yy# $
AddAsyncyy$ ,
(yy, -
doctoryy- 3
)yy3 4
;yy4 5
return{{ 
_mapper{{ 
.{{ 
Map{{ 
<{{ 
	DoctorDto{{ (
>{{( )
({{) *
doctor{{* 0
){{0 1
;{{1 2
}|| 	
public 
async 
Task 
< 
	DoctorDto #
># $
UpdateAsync% 0
(0 1
int1 4
id5 7
,7 8
UpdateDoctorDto9 H
dtoI L
)L M
{
ÄÄ 	
var
ÅÅ 
doctor
ÅÅ 
=
ÅÅ 
await
ÅÅ 
_doctorRepository
ÅÅ 0
.
ÅÅ0 1
GetByIdAsync
ÅÅ1 =
(
ÅÅ= >
id
ÅÅ> @
)
ÅÅ@ A
;
ÅÅA B
if
ÉÉ 
(
ÉÉ 
doctor
ÉÉ 
==
ÉÉ 
null
ÉÉ 
)
ÉÉ 
{
ÑÑ 
throw
ÖÖ 
new
ÖÖ 
NotFoundException
ÖÖ +
(
ÖÖ+ ,
$str
ÖÖ, >
)
ÖÖ> ?
;
ÖÖ? @
}
ÜÜ 
_mapper
àà 
.
àà 
Map
àà 
(
àà 
dto
àà 
,
àà 
doctor
àà #
)
àà# $
;
àà$ %
await
ää 
_doctorRepository
ää #
.
ää# $
UpdateAsync
ää$ /
(
ää/ 0
id
ää0 2
,
ää2 3
doctor
ää4 :
,
ää: ;
CancellationToken
ää< M
.
ääM N
None
ääN R
)
ääR S
;
ääS T
return
åå 
_mapper
åå 
.
åå 
Map
åå 
<
åå 
	DoctorDto
åå (
>
åå( )
(
åå) *
doctor
åå* 0
)
åå0 1
;
åå1 2
}
çç 	
}
éé 
}èè ∑π
vC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\AuthService.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Services !
.! "
Implementations" 1
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
UserManager $
<$ %
ApplicationUser% 4
>4 5
_userManager6 B
;B C
private 
readonly 
IRepository $
<$ %
Patient% ,
>, -
_patientRepository. @
;@ A
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
private 
readonly 
IConfiguration '
_config( /
;/ 0
public 
AuthService 
( 
UserManager 
< 
ApplicationUser '
>' (
userManager) 4
,4 5
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
HealthAxisDbContext 
context  '
,' (
IConfiguration 
config !
)! "
{ 	
_userManager 
= 
userManager &
;& '
_patientRepository 
=  
patientRepository! 2
;2 3
_context 
= 
context 
; 
_config   
=   
config   
;   
}!! 	
public## 
async## 
Task## 
<## 
(## 
bool## 
Success##  '
,##' (
string##) /
Message##0 7
,##7 8
string##9 ?
UserId##@ F
)##F G
>##G H
Register##I Q
(##Q R
RegisterDto##R ]
request##^ e
)##e f
{$$ 	
if%% 
(%% 
request%% 
.%% 
Password%%  
!=%%! #
request%%$ +
.%%+ ,
ConfirmPassword%%, ;
)%%; <
{&& 
return'' 
('' 
false'' 
,'' 
$str'' 7
,''7 8
string''9 ?
.''? @
Empty''@ E
)''E F
;''F G
}(( 
var** 
user** 
=** 
new** 
ApplicationUser** *
{++ 
UserName,, 
=,, 
request,, "
.,," #
Email,,# (
,,,( )
Email-- 
=-- 
request-- 
.--  
Email--  %
,--% &
MustChangePassword.. "
=..# $
false..% *
}// 
;// 
var11 
createUserResult11  
=11! "
await11# (
_userManager11) 5
.115 6
CreateAsync116 A
(11A B
user11B F
,11F G
request11H O
.11O P
Password11P X
)11X Y
;11Y Z
if33 
(33 
!33 
createUserResult33 !
.33! "
	Succeeded33" +
)33+ ,
{44 
var55 
errors55 
=55 
string55 #
.55# $
Join55$ (
(55( )
$str55) -
,55- .
createUserResult55/ ?
.55? @
Errors55@ F
.55F G
Select55G M
(55M N
e55N O
=>55P R
e55S T
.55T U
Description55U `
)55` a
)55a b
;55b c
return66 
(66 
false66 
,66 
errors66 %
,66% &
string66' -
.66- .
Empty66. 3
)663 4
;664 5
}77 
var99 
addRoleResult99 
=99 
await99  %
_userManager99& 2
.992 3
AddToRoleAsync993 A
(99A B
user99B F
,99F G
$str99H Q
)99Q R
;99R S
if;; 
(;; 
!;; 
addRoleResult;; 
.;; 
	Succeeded;; (
);;( )
{<< 
var== 
errors== 
=== 
string== #
.==# $
Join==$ (
(==( )
$str==) -
,==- .
addRoleResult==/ <
.==< =
Errors=== C
.==C D
Select==D J
(==J K
e==K L
=>==M O
e==P Q
.==Q R
Description==R ]
)==] ^
)==^ _
;==_ `
return>> 
(>> 
false>> 
,>> 
errors>> %
,>>% &
string>>' -
.>>- .
Empty>>. 3
)>>3 4
;>>4 5
}?? 
varAA 
patientAA 
=AA 
newAA 
PatientAA %
{BB 
FullNameCC 
=CC 
requestCC "
.CC" #
FullNameCC# +
,CC+ ,
DateOfBirthDD 
=DD 
requestDD %
.DD% &
DateOfBirthDD& 1
,DD1 2
GenderEE 
=EE 
requestEE  
.EE  !
GenderEE! '
,EE' (
PhoneNumberFF 
=FF 
requestFF %
.FF% &
PhoneNumberFF& 1
,FF1 2
EmailGG 
=GG 
requestGG 
.GG  
EmailGG  %
,GG% &
InsuranceIdHH 
=HH 
requestHH %
.HH% &
InsuranceIdHH& 1
,HH1 2
CreatedDateII 
=II 
DateTimeII &
.II& '
NowII' *
}JJ 
;JJ 
awaitLL 
_patientRepositoryLL $
.LL$ %
AddAsyncLL% -
(LL- .
patientLL. 5
)LL5 6
;LL6 7
returnNN 
(NN 
trueNN 
,NN 
$strNN ;
,NN; <
userNN= A
.NNA B
IdNNB D
)NND E
;NNE F
}OO 	
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
(QQ 
boolQQ 
SuccessQQ  '
,QQ' (
stringQQ) /
MessageQQ0 7
,QQ7 8
stringQQ9 ?
AccessTokenQQ@ K
,QQK L
stringQQM S
RefreshTokenQQT `
,QQ` a
intQQb e
	ExpiresInQQf o
,QQo p
boolQQq u#
RequiresPasswordChange	QQv å
)
QQå ç
>
QQç é
Login
QQè î
(
QQî ï
LoginDto
QQï ù
request
QQû •
)
QQ• ¶
{RR 	
varSS 
userSS 
=SS 
awaitSS 
_userManagerSS )
.SS) *
FindByEmailAsyncSS* :
(SS: ;
requestSS; B
.SSB C
EmailSSC H
)SSH I
;SSI J
ifUU 
(UU 
userUU 
==UU 
nullUU 
)UU 
{VV 
returnWW 
(WW 
falseWW 
,WW 
$strWW 4
,WW4 5
stringWW6 <
.WW< =
EmptyWW= B
,WWB C
stringWWD J
.WWJ K
EmptyWWK P
,WWP Q
$numWWR S
,WWS T
falseWWU Z
)WWZ [
;WW[ \
}XX 
varZZ 
isPasswordValidZZ 
=ZZ  !
awaitZZ" '
_userManagerZZ( 4
.ZZ4 5
CheckPasswordAsyncZZ5 G
(ZZG H
userZZH L
,ZZL M
requestZZN U
.ZZU V
PasswordZZV ^
)ZZ^ _
;ZZ_ `
if\\ 
(\\ 
!\\ 
isPasswordValid\\  
)\\  !
{]] 
return^^ 
(^^ 
false^^ 
,^^ 
$str^^ 4
,^^4 5
string^^6 <
.^^< =
Empty^^= B
,^^B C
string^^D J
.^^J K
Empty^^K P
,^^P Q
$num^^R S
,^^S T
false^^U Z
)^^Z [
;^^[ \
}__ 
ifaa 
(aa 
useraa 
.aa 
MustChangePasswordaa '
)aa' (
{bb 
returncc 
(cc 
truecc 
,cc 
$strcc 8
,cc8 9
stringcc: @
.cc@ A
EmptyccA F
,ccF G
stringccH N
.ccN O
EmptyccO T
,ccT U
$numccV W
,ccW X
trueccY ]
)cc] ^
;cc^ _
}dd 
varff 
accessTokenff 
=ff 
awaitff #
GenerateAccessTokenff$ 7
(ff7 8
userff8 <
)ff< =
;ff= >
vargg 
refreshTokengg 
=gg  
GenerateRefreshTokengg 3
(gg3 4
)gg4 5
;gg5 6
awaitii 
SaveRefreshTokenii "
(ii" #
userii# '
.ii' (
Idii( *
,ii* +
refreshTokenii, 8
)ii8 9
;ii9 :
varkk 
	expiresInkk 
=kk 
intkk 
.kk  
Parsekk  %
(kk% &
_configll 
.ll 

GetSectionll "
(ll" #
$strll# (
)ll( )
[ll) *
$strll* H
]llH I
!llI J
)llJ K
;llK L
returnnn 
(nn 
truenn 
,nn 
$strnn ,
,nn, -
accessTokennn. 9
,nn9 :
refreshTokennn; G
,nnG H
	expiresInnnI R
,nnR S
falsennT Y
)nnY Z
;nnZ [
}oo 	
publicqq 
asyncqq 
Taskqq 
<qq 
(qq 
boolqq 
Successqq  '
,qq' (
stringqq) /
Messageqq0 7
)qq7 8
>qq8 9
ChangePasswordqq: H
(qqH I
ChangePasswordDtoqqI Z
requestqq[ b
)qqb c
{rr 	
ifss 
(ss 
requestss 
.ss 
NewPasswordss #
!=ss$ &
requestss' .
.ss. /
ConfirmPasswordss/ >
)ss> ?
{tt 
returnuu 
(uu 
falseuu 
,uu 
$struu O
)uuO P
;uuP Q
}vv 
varxx 
userxx 
=xx 
awaitxx 
_userManagerxx )
.xx) *
FindByEmailAsyncxx* :
(xx: ;
requestxx; B
.xxB C
EmailxxC H
)xxH I
;xxI J
ifzz 
(zz 
userzz 
==zz 
nullzz 
)zz 
{{{ 
return|| 
(|| 
false|| 
,|| 
$str|| /
)||/ 0
;||0 1
}}} 
var 
result 
= 
await 
_userManager +
.+ ,
ChangePasswordAsync, ?
(? @
user
ÄÄ 
,
ÄÄ 
request
ÅÅ 
.
ÅÅ 
OldPassword
ÅÅ #
,
ÅÅ# $
request
ÇÇ 
.
ÇÇ 
NewPassword
ÇÇ #
)
ÇÇ# $
;
ÇÇ$ %
if
ÑÑ 
(
ÑÑ 
!
ÑÑ 
result
ÑÑ 
.
ÑÑ 
	Succeeded
ÑÑ !
)
ÑÑ! "
{
ÖÖ 
var
ÜÜ 
errors
ÜÜ 
=
ÜÜ 
string
ÜÜ #
.
ÜÜ# $
Join
ÜÜ$ (
(
ÜÜ( )
$str
ÜÜ) -
,
ÜÜ- .
result
ÜÜ/ 5
.
ÜÜ5 6
Errors
ÜÜ6 <
.
ÜÜ< =
Select
ÜÜ= C
(
ÜÜC D
e
ÜÜD E
=>
ÜÜF H
e
ÜÜI J
.
ÜÜJ K
Description
ÜÜK V
)
ÜÜV W
)
ÜÜW X
;
ÜÜX Y
return
áá 
(
áá 
false
áá 
,
áá 
errors
áá %
)
áá% &
;
áá& '
}
àà 
user
ää 
.
ää  
MustChangePassword
ää #
=
ää$ %
false
ää& +
;
ää+ ,
await
åå 
_userManager
åå 
.
åå 
UpdateAsync
åå *
(
åå* +
user
åå+ /
)
åå/ 0
;
åå0 1
return
éé 
(
éé 
true
éé 
,
éé 
$str
éé 9
)
éé9 :
;
éé: ;
}
èè 	
public
ëë 
async
ëë 
Task
ëë 
<
ëë 
(
ëë 
bool
ëë 
Success
ëë  '
,
ëë' (
string
ëë) /
Message
ëë0 7
,
ëë7 8
string
ëë9 ?
AccessToken
ëë@ K
,
ëëK L
string
ëëM S
RefreshToken
ëëT `
,
ëë` a
int
ëëb e
	ExpiresIn
ëëf o
)
ëëo p
>
ëëp q
RefreshToken
ëër ~
(
ëë~ %
RefreshTokenRequestDtoëë ï
requestëëñ ù
)ëëù û
{
íí 	
var
ìì 
storedToken
ìì 
=
ìì 
await
ìì #
_context
ìì$ ,
.
ìì, -
RefreshTokens
ìì- :
.
îî !
FirstOrDefaultAsync
îî $
(
îî$ %
x
îî% &
=>
îî' )
x
îî* +
.
îî+ ,
Token
îî, 1
==
îî2 4
request
îî5 <
.
îî< =
RefreshToken
îî= I
)
îîI J
;
îîJ K
if
ññ 
(
ññ 
storedToken
ññ 
==
ññ 
null
ññ #
)
ññ# $
{
óó 
return
òò 
(
òò 
false
òò 
,
òò 
$str
òò 6
,
òò6 7
string
òò8 >
.
òò> ?
Empty
òò? D
,
òòD E
string
òòF L
.
òòL M
Empty
òòM R
,
òòR S
$num
òòT U
)
òòU V
;
òòV W
}
ôô 
if
õõ 
(
õõ 
storedToken
õõ 
.
õõ 
	IsRevoked
õõ %
)
õõ% &
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
ùù 9
,
ùù9 :
string
ùù; A
.
ùùA B
Empty
ùùB G
,
ùùG H
string
ùùI O
.
ùùO P
Empty
ùùP U
,
ùùU V
$num
ùùW X
)
ùùX Y
;
ùùY Z
}
ûû 
if
†† 
(
†† 
storedToken
†† 
.
†† 
	ExpiresAt
†† %
<
††& '
DateTime
††( 0
.
††0 1
UtcNow
††1 7
)
††7 8
{
°° 
return
¢¢ 
(
¢¢ 
false
¢¢ 
,
¢¢ 
$str
¢¢ 6
,
¢¢6 7
string
¢¢8 >
.
¢¢> ?
Empty
¢¢? D
,
¢¢D E
string
¢¢F L
.
¢¢L M
Empty
¢¢M R
,
¢¢R S
$num
¢¢T U
)
¢¢U V
;
¢¢V W
}
££ 
var
•• 
user
•• 
=
•• 
await
•• 
_userManager
•• )
.
••) *
FindByIdAsync
••* 7
(
••7 8
storedToken
••8 C
.
••C D
UserId
••D J
)
••J K
;
••K L
if
ßß 
(
ßß 
user
ßß 
==
ßß 
null
ßß 
)
ßß 
{
®® 
return
©© 
(
©© 
false
©© 
,
©© 
$str
©© /
,
©©/ 0
string
©©1 7
.
©©7 8
Empty
©©8 =
,
©©= >
string
©©? E
.
©©E F
Empty
©©F K
,
©©K L
$num
©©M N
)
©©N O
;
©©O P
}
™™ 
storedToken
¨¨ 
.
¨¨ 
	IsRevoked
¨¨ !
=
¨¨" #
true
¨¨$ (
;
¨¨( )
var
ÆÆ 
newAccessToken
ÆÆ 
=
ÆÆ  
await
ÆÆ! &!
GenerateAccessToken
ÆÆ' :
(
ÆÆ: ;
user
ÆÆ; ?
)
ÆÆ? @
;
ÆÆ@ A
var
ØØ 
newRefreshToken
ØØ 
=
ØØ  !"
GenerateRefreshToken
ØØ" 6
(
ØØ6 7
)
ØØ7 8
;
ØØ8 9
await
±± 
SaveRefreshToken
±± "
(
±±" #
user
±±# '
.
±±' (
Id
±±( *
,
±±* +
newRefreshToken
±±, ;
)
±±; <
;
±±< =
await
≥≥ 
_context
≥≥ 
.
≥≥ 
SaveChangesAsync
≥≥ +
(
≥≥+ ,
)
≥≥, -
;
≥≥- .
var
µµ 
	expiresIn
µµ 
=
µµ 
int
µµ 
.
µµ  
Parse
µµ  %
(
µµ% &
_config
∂∂ 
.
∂∂ 

GetSection
∂∂ "
(
∂∂" #
$str
∂∂# (
)
∂∂( )
[
∂∂) *
$str
∂∂* H
]
∂∂H I
!
∂∂I J
)
∂∂J K
;
∂∂K L
return
∏∏ 
(
∏∏ 
true
∏∏ 
,
∏∏ 
$str
∏∏ 8
,
∏∏8 9
newAccessToken
∏∏: H
,
∏∏H I
newRefreshToken
∏∏J Y
,
∏∏Y Z
	expiresIn
∏∏[ d
)
∏∏d e
;
∏∏e f
}
ππ 	
private
ªª 
async
ªª 
Task
ªª 
<
ªª 
string
ªª !
>
ªª! "!
GenerateAccessToken
ªª# 6
(
ªª6 7
ApplicationUser
ªª7 F
user
ªªG K
)
ªªK L
{
ºº 	
var
ΩΩ 
jwt
ΩΩ 
=
ΩΩ 
_config
ΩΩ 
.
ΩΩ 

GetSection
ΩΩ (
(
ΩΩ( )
$str
ΩΩ) .
)
ΩΩ. /
;
ΩΩ/ 0
var
øø 
key
øø 
=
øø 
new
øø "
SymmetricSecurityKey
øø .
(
øø. /
Encoding
¿¿ 
.
¿¿ 
UTF8
¿¿ 
.
¿¿ 
GetBytes
¿¿ &
(
¿¿& '
jwt
¿¿' *
[
¿¿* +
$str
¿¿+ 0
]
¿¿0 1
!
¿¿1 2
)
¿¿2 3
)
¿¿3 4
;
¿¿4 5
var
¬¬ 
credentials
¬¬ 
=
¬¬ 
new
¬¬ ! 
SigningCredentials
¬¬" 4
(
¬¬4 5
key
√√ 
,
√√  
SecurityAlgorithms
ƒƒ "
.
ƒƒ" #

HmacSha256
ƒƒ# -
)
ƒƒ- .
;
ƒƒ. /
var
∆∆ 
roles
∆∆ 
=
∆∆ 
await
∆∆ 
_userManager
∆∆ *
.
∆∆* +
GetRolesAsync
∆∆+ 8
(
∆∆8 9
user
∆∆9 =
)
∆∆= >
;
∆∆> ?
var
»» 
claims
»» 
=
»» 
new
»» 
List
»» !
<
»»! "
Claim
»»" '
>
»»' (
{
…… 
new
   
Claim
   
(
   

ClaimTypes
   $
.
  $ %
NameIdentifier
  % 3
,
  3 4
user
  5 9
.
  9 :
Id
  : <
)
  < =
,
  = >
new
ÀÀ 
Claim
ÀÀ 
(
ÀÀ 

ClaimTypes
ÀÀ $
.
ÀÀ$ %
Email
ÀÀ% *
,
ÀÀ* +
user
ÀÀ, 0
.
ÀÀ0 1
Email
ÀÀ1 6
!
ÀÀ6 7
)
ÀÀ7 8
}
ÃÃ 
;
ÃÃ 
foreach
ŒŒ 
(
ŒŒ 
var
ŒŒ 
role
ŒŒ 
in
ŒŒ  
roles
ŒŒ! &
)
ŒŒ& '
{
œœ 
claims
–– 
.
–– 
Add
–– 
(
–– 
new
–– 
Claim
–– $
(
––$ %

ClaimTypes
––% /
.
––/ 0
Role
––0 4
,
––4 5
role
––6 :
)
––: ;
)
––; <
;
––< =
}
—— 
var
”” 
expirationMinutes
”” !
=
””" #
int
””$ '
.
””' (
Parse
””( -
(
””- .
jwt
‘‘ 
[
‘‘ 
$str
‘‘ 2
]
‘‘2 3
!
‘‘3 4
)
‘‘4 5
;
‘‘5 6
var
÷÷ 
token
÷÷ 
=
÷÷ 
new
÷÷ 
JwtSecurityToken
÷÷ ,
(
÷÷, -
issuer
◊◊ 
:
◊◊ 
jwt
◊◊ 
[
◊◊ 
$str
◊◊ $
]
◊◊$ %
,
◊◊% &
audience
ÿÿ 
:
ÿÿ 
jwt
ÿÿ 
[
ÿÿ 
$str
ÿÿ (
]
ÿÿ( )
,
ÿÿ) *
claims
ŸŸ 
:
ŸŸ 
claims
ŸŸ 
,
ŸŸ 
expires
⁄⁄ 
:
⁄⁄ 
DateTime
⁄⁄ !
.
⁄⁄! "
UtcNow
⁄⁄" (
.
⁄⁄( )

AddMinutes
⁄⁄) 3
(
⁄⁄3 4
expirationMinutes
⁄⁄4 E
)
⁄⁄E F
,
⁄⁄F G 
signingCredentials
€€ "
:
€€" #
credentials
€€$ /
)
€€/ 0
;
€€0 1
return
›› 
new
›› %
JwtSecurityTokenHandler
›› .
(
››. /
)
››/ 0
.
››0 1

WriteToken
››1 ;
(
››; <
token
››< A
)
››A B
;
››B C
}
ﬁﬁ 	
private
‡‡ 
static
‡‡ 
string
‡‡ "
GenerateRefreshToken
‡‡ 2
(
‡‡2 3
)
‡‡3 4
{
·· 	
var
‚‚ 
randomBytes
‚‚ 
=
‚‚ 
new
‚‚ !
byte
‚‚" &
[
‚‚& '
$num
‚‚' )
]
‚‚) *
;
‚‚* +
using
‰‰ 
var
‰‰ 
rng
‰‰ 
=
‰‰ #
RandomNumberGenerator
‰‰ 1
.
‰‰1 2
Create
‰‰2 8
(
‰‰8 9
)
‰‰9 :
;
‰‰: ;
rng
ÊÊ 
.
ÊÊ 
GetBytes
ÊÊ 
(
ÊÊ 
randomBytes
ÊÊ $
)
ÊÊ$ %
;
ÊÊ% &
return
ËË 
Convert
ËË 
.
ËË 
ToBase64String
ËË )
(
ËË) *
randomBytes
ËË* 5
)
ËË5 6
;
ËË6 7
}
ÈÈ 	
private
ÎÎ 
async
ÎÎ 
Task
ÎÎ 
SaveRefreshToken
ÎÎ +
(
ÎÎ+ ,
string
ÎÎ, 2
userId
ÎÎ3 9
,
ÎÎ9 :
string
ÎÎ; A
token
ÎÎB G
)
ÎÎG H
{
ÏÏ 	
var
ÌÌ (
refreshTokenExpirationDays
ÌÌ *
=
ÌÌ+ ,
int
ÓÓ 
.
ÓÓ 
TryParse
ÓÓ 
(
ÓÓ 
_config
ÔÔ 
.
ÔÔ 

GetSection
ÔÔ &
(
ÔÔ& '
$str
ÔÔ' ,
)
ÔÔ, -
[
ÔÔ- .
$str
ÔÔ. J
]
ÔÔJ K
,
ÔÔK L
out
 
var
 
days
  
)
  !
?
ÒÒ 
days
ÒÒ 
:
ÚÚ 
$num
ÚÚ 
;
ÚÚ 
var
ÙÙ 
refreshToken
ÙÙ 
=
ÙÙ 
new
ÙÙ "
RefreshToken
ÙÙ# /
{
ıı 
UserId
ˆˆ 
=
ˆˆ 
userId
ˆˆ 
,
ˆˆ  
Token
˜˜ 
=
˜˜ 
token
˜˜ 
,
˜˜ 
	CreatedAt
¯¯ 
=
¯¯ 
DateTime
¯¯ $
.
¯¯$ %
UtcNow
¯¯% +
,
¯¯+ ,
	ExpiresAt
˘˘ 
=
˘˘ 
DateTime
˘˘ $
.
˘˘$ %
UtcNow
˘˘% +
.
˘˘+ ,
AddDays
˘˘, 3
(
˘˘3 4(
refreshTokenExpirationDays
˘˘4 N
)
˘˘N O
,
˘˘O P
	IsRevoked
˙˙ 
=
˙˙ 
false
˙˙ !
}
˚˚ 
;
˚˚ 
await
˝˝ 
_context
˝˝ 
.
˝˝ 
RefreshTokens
˝˝ (
.
˝˝( )
AddAsync
˝˝) 1
(
˝˝1 2
refreshToken
˝˝2 >
)
˝˝> ?
;
˝˝? @
await
˛˛ 
_context
˛˛ 
.
˛˛ 
SaveChangesAsync
˛˛ +
(
˛˛+ ,
)
˛˛, -
;
˛˛- .
}
ˇˇ 	
}
ÄÄ 
}ÅÅ ûj
}C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Services\Implementations\AppointmentService.cs
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
AppointmentService #
:$ %
IAppointmentService& 9
{ 
private 
readonly 
IRepository $
<$ %
Appointment% 0
>0 1"
_appointmentRepository2 H
;H I
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
AppointmentService !
(! "
IRepository 
< 
Appointment #
># $!
appointmentRepository% :
,: ;
IRepository 
< 
Doctor 
> 
doctorRepository  0
,0 1
IRepository 
< 
Patient 
>  
patientRepository! 2
,2 3
IMapper 
mapper 
) 
{ 	"
_appointmentRepository "
=# $!
appointmentRepository% :
;: ;
_doctorRepository 
= 
doctorRepository  0
;0 1
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
<  % &
AppointmentDto  & 4
>  4 5
>  5 6
GetAllAsync  7 B
(  B C
)  C D
{!! 	
var"" 
appointments"" 
="" 
await"" $"
_appointmentRepository""% ;
.""; <
GetAllAsync""< G
(""G H
)""H I
;""I J
return## 
_mapper## 
.## 
Map## 
<## 
IEnumerable## *
<##* +
AppointmentDto##+ 9
>##9 :
>##: ;
(##; <
appointments##< H
)##H I
;##I J
}$$ 	
public'' 
async'' 
Task'' 
<'' 
AppointmentDto'' (
>''( )
AddAsync''* 2
(''2 3 
CreateAppointmentDto''3 G
dto''H K
)''K L
{(( 	
if)) 
()) 
dto)) 
.)) 
ScheduledDate)) !
.))! "
Date))" &
<))' (
DateTime))) 1
.))1 2
Today))2 7
)))7 8
{** 
throw++ 
new++ %
CustomValidationException++ 3
(++3 4
$str++4 c
)++c d
;++d e
},, 
if.. 
(.. 
dto.. 
... 
ScheduledDate.. !
...! "
Date.." &
>..' (
DateTime..) 1
...1 2
Today..2 7
...7 8
	AddMonths..8 A
(..A B
$num..B C
)..C D
)..D E
{// 
throw00 
new00 %
CustomValidationException00 3
(003 4
$str004 p
)00p q
;00q r
}11 
if33 
(33 
string33 
.33 
IsNullOrWhiteSpace33 )
(33) *
dto33* -
.33- .
TimeSlot33. 6
)336 7
)337 8
{44 
throw55 
new55 %
CustomValidationException55 3
(553 4
$str554 L
)55L M
;55M N
}66 
var88 
patient88 
=88 
await88 
_patientRepository88  2
.882 3
GetByIdAsync883 ?
(88? @
dto88@ C
.88C D
	PatientId88D M
)88M N
;88N O
if:: 
(:: 
patient:: 
==:: 
null:: 
)::  
{;; 
throw<< 
new<< 
NotFoundException<< +
(<<+ ,
$str<<, @
)<<@ A
;<<A B
}== 
var?? 
doctor?? 
=?? 
await?? 
_doctorRepository?? 0
.??0 1
GetByIdAsync??1 =
(??= >
dto??> A
.??A B
DoctorId??B J
)??J K
;??K L
ifAA 
(AA 
doctorAA 
==AA 
nullAA 
)AA 
{BB 
throwCC 
newCC 
NotFoundExceptionCC +
(CC+ ,
$strCC, ?
)CC? @
;CC@ A
}DD 
ifFF 
(FF 
!FF 
doctorFF 
.FF 
IsActiveFF  
)FF  !
{GG 
throwHH 
newHH %
CustomValidationExceptionHH 3
(HH3 4
$strHH4 j
)HHj k
;HHk l
}II 
varKK  
existingAppointmentsKK $
=KK% &
awaitKK' ,"
_appointmentRepositoryKK- C
.KKC D
GetAllAsyncKKD O
(KKO P
)KKP Q
;KKQ R
varMM 
isSlotBookedMM 
=MM  
existingAppointmentsMM 3
.MM3 4
AnyMM4 7
(MM7 8
aMM8 9
=>MM: <
aNN 
.NN 
DoctorIdNN 
==NN 
dtoNN !
.NN! "
DoctorIdNN" *
&&NN+ -
aOO 
.OO 
ScheduledDateOO 
.OO  
DateOO  $
==OO% '
dtoOO( +
.OO+ ,
ScheduledDateOO, 9
.OO9 :
DateOO: >
&&OO? A
aPP 
.PP 
TimeSlotPP 
==PP 
dtoPP !
.PP! "
TimeSlotPP" *
&&PP+ -
aQQ 
.QQ 
StatusQQ 
!=QQ 
AppointmentStatusQQ -
.QQ- .
	CancelledQQ. 7
)QQ7 8
;QQ8 9
ifSS 
(SS 
isSlotBookedSS 
)SS 
{TT 
throwUU 
newUU %
CustomValidationExceptionUU 3
(UU3 4
$strUU4 [
)UU[ \
;UU\ ]
}VV 
varXX 
appointmentXX 
=XX 
_mapperXX %
.XX% &
MapXX& )
<XX) *
AppointmentXX* 5
>XX5 6
(XX6 7
dtoXX7 :
)XX: ;
;XX; <
appointmentYY 
.YY 
StatusYY 
=YY  
AppointmentStatusYY! 2
.YY2 3
PendingYY3 :
;YY: ;
await[[ "
_appointmentRepository[[ (
.[[( )
AddAsync[[) 1
([[1 2
appointment[[2 =
)[[= >
;[[> ?
return]] 
_mapper]] 
.]] 
Map]] 
<]] 
AppointmentDto]] -
>]]- .
(]]. /
appointment]]/ :
)]]: ;
;]]; <
}^^ 	
publicaa 
asyncaa 
Taskaa 
<aa 
AppointmentDtoaa (
>aa( )
UpdateStatusAsyncaa* ;
(aa; <
intaa< ?
idaa@ B
,aaB C&
UpdateAppointmentStatusDtoaaD ^
dtoaa_ b
)aab c
{bb 	
varcc 
appointmentcc 
=cc 
awaitcc #"
_appointmentRepositorycc$ :
.cc: ;
GetByIdAsynccc; G
(ccG H
idccH J
)ccJ K
;ccK L
ifee 
(ee 
appointmentee 
==ee 
nullee #
)ee# $
{ff 
throwgg 
newgg 
NotFoundExceptiongg +
(gg+ ,
$strgg, D
)ggD E
;ggE F
}hh 
ifjj 
(jj 
appointmentjj 
.jj 
Statusjj "
==jj# %
AppointmentStatusjj& 7
.jj7 8
	Cancelledjj8 A
)jjA B
{kk 
throwll 
newll %
CustomValidationExceptionll 3
(ll3 4
$strll4 `
)ll` a
;lla b
}mm 
ifoo 
(oo 
appointmentoo 
.oo 
Statusoo "
==oo# %
AppointmentStatusoo& 7
.oo7 8
	Completedoo8 A
)ooA B
{pp 
throwqq 
newqq %
CustomValidationExceptionqq 3
(qq3 4
$strqq4 `
)qq` a
;qqa b
}rr 
iftt 
(tt 
appointmenttt 
.tt 
Statustt "
==tt# %
dtott& )
.tt) *
Statustt* 0
)tt0 1
{uu 
throwvv 
newvv %
CustomValidationExceptionvv 3
(vv3 4
$"vv4 6
$strvv6 M
{vvM N
dtovvN Q
.vvQ R
StatusvvR X
}vvX Y
$strvvY Z
"vvZ [
)vv[ \
;vv\ ]
}ww 
ifyy 
(yy 
appointmentyy 
.yy 
Statusyy "
==yy# %
AppointmentStatusyy& 7
.yy7 8
Pendingyy8 ?
&&yy@ B
dtozz 
.zz 
Statuszz 
==zz 
AppointmentStatuszz /
.zz/ 0
	Completedzz0 9
)zz9 :
{{{ 
throw|| 
new|| %
CustomValidationException|| 3
(||3 4
$str||4 o
)||o p
;||p q
}}} 
switch 
( 
dto 
. 
Status 
) 
{
ÄÄ 
case
ÅÅ 
AppointmentStatus
ÅÅ &
.
ÅÅ& '
	Confirmed
ÅÅ' 0
:
ÅÅ0 1
appointment
ÇÇ 
.
ÇÇ  
Confirm
ÇÇ  '
(
ÇÇ' (
)
ÇÇ( )
;
ÇÇ) *
break
ÉÉ 
;
ÉÉ 
case
ÖÖ 
AppointmentStatus
ÖÖ &
.
ÖÖ& '
	Cancelled
ÖÖ' 0
:
ÖÖ0 1
appointment
ÜÜ 
.
ÜÜ  
Cancel
ÜÜ  &
(
ÜÜ& '
dto
ÜÜ' *
.
ÜÜ* + 
CancellationReason
ÜÜ+ =
??
ÜÜ> @
string
ÜÜA G
.
ÜÜG H
Empty
ÜÜH M
)
ÜÜM N
;
ÜÜN O
break
áá 
;
áá 
case
ââ 
AppointmentStatus
ââ &
.
ââ& '
	Completed
ââ' 0
:
ââ0 1
appointment
ää 
.
ää  
Complete
ää  (
(
ää( )
)
ää) *
;
ää* +
break
ãã 
;
ãã 
case
çç 
AppointmentStatus
çç &
.
çç& '
Pending
çç' .
:
çç. /
throw
éé 
new
éé '
CustomValidationException
éé 7
(
éé7 8
$str
éé8 _
)
éé_ `
;
éé` a
}
èè 
await
ëë $
_appointmentRepository
ëë (
.
ëë( )
UpdateAsync
ëë) 4
(
ëë4 5
id
ëë5 7
,
ëë7 8
appointment
ëë9 D
,
ëëD E
CancellationToken
ëëF W
.
ëëW X
None
ëëX \
)
ëë\ ]
;
ëë] ^
return
ìì 
_mapper
ìì 
.
ìì 
Map
ìì 
<
ìì 
AppointmentDto
ìì -
>
ìì- .
(
ìì. /
appointment
ìì/ :
)
ìì: ;
;
ìì; <
}
îî 	
public
óó 
async
óó 
Task
óó 
<
óó 
bool
óó 
>
óó 
DeleteAsync
óó  +
(
óó+ ,
int
óó, /
id
óó0 2
)
óó2 3
{
òò 	
var
ôô 
appointment
ôô 
=
ôô 
await
ôô #$
_appointmentRepository
ôô$ :
.
ôô: ;
GetByIdAsync
ôô; G
(
ôôG H
id
ôôH J
)
ôôJ K
;
ôôK L
if
õõ 
(
õõ 
appointment
õõ 
==
õõ 
null
õõ #
)
õõ# $
{
úú 
throw
ùù 
new
ùù 
NotFoundException
ùù +
(
ùù+ ,
$str
ùù, D
)
ùùD E
;
ùùE F
}
ûû 
if
†† 
(
†† 
appointment
†† 
.
†† 
Status
†† "
==
††# %
AppointmentStatus
††& 7
.
††7 8
	Completed
††8 A
)
††A B
{
°° 
throw
¢¢ 
new
¢¢ '
CustomValidationException
¢¢ 3
(
¢¢3 4
$str
¢¢4 _
)
¢¢_ `
;
¢¢` a
}
££ 
if
•• 
(
•• 
appointment
•• 
.
•• 
Status
•• "
==
••# %
AppointmentStatus
••& 7
.
••7 8
	Confirmed
••8 A
)
••A B
{
¶¶ 
throw
ßß 
new
ßß '
CustomValidationException
ßß 3
(
ßß3 4
$str
ßß4 _
)
ßß_ `
;
ßß` a
}
®® 
await
™™ $
_appointmentRepository
™™ (
.
™™( )
DeleteAsync
™™) 4
(
™™4 5
id
™™5 7
)
™™7 8
;
™™8 9
return
¨¨ 
true
¨¨ 
;
¨¨ 
}
≠≠ 	
}
ÆÆ 
}ØØ æ
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
}55 ⁄

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
> 
GetByIdAsync 
( 
int  
id! #
)# $
;$ %
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
}22 ˇ
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
:> ?
base@ D
(D E
contextE L
)L M
{ 	
_context 
= 
context 
; 
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
}'' Ó
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
:C D
baseE I
(I J
contextJ Q
)Q R
{ 	
_context 
= 
context 
; 
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
})) ◊
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
}&& Î
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
 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
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
{ 	
_context 
= 
context 
; 
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
}-- ¨T
YC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. 
AddExceptionHandler $
<$ %"
GlobalExceptionHandler% ;
>; <
(< =
)= >
;> ?
builder 
. 
Services 
. 
AddProblemDetails "
(" #
)# $
;$ %
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
options &
=>' )
{ 
options 
. 

SwaggerDoc 
( 
$str 
, 
new  
OpenApiInfo! ,
{ 
Title   
=   
$str    
,    !
Version!! 
=!! 
$str!! 
}"" 
)"" 
;"" 
options$$ 
.$$ !
AddSecurityDefinition$$ !
($$! "
$str$$" *
,$$* +
new$$, /!
OpenApiSecurityScheme$$0 E
{%% 
Type&& 
=&& 
SecuritySchemeType&& !
.&&! "
Http&&" &
,&&& '
Scheme'' 
='' 
$str'' 
,'' 
BearerFormat(( 
=(( 
$str(( 
,(( 
Description)) 
=)) 
$str)) ?
}** 
)** 
;** 
options,, 
.,, "
AddSecurityRequirement,, "
(,," #
document,,# +
=>,,, .
new,,/ 2&
OpenApiSecurityRequirement,,3 M
{-- 
[.. 	
new..	 *
OpenApiSecuritySchemeReference.. +
(..+ ,
$str.., 4
,..4 5
document..6 >
)..> ?
]..? @
=..A B
new..C F
List..G K
<..K L
string..L R
>..R S
(..S T
)..T U
}// 
)// 
;// 
}00 
)00 
;00 
builder33 
.33 
Services33 
.33 
AddDbContext33 
<33 
HealthAxisDbContext33 1
>331 2
(332 3
options333 :
=>33; =
options44 
.44 
UseSqlServer44 
(44 
builder55 
.55 
Configuration55 
.55 
GetConnectionString55 1
(551 2
$str552 E
)55E F
)55F G
)55G H
;55H I
builder88 
.88 
Services88 
.88 
AddIdentity88 
<88 
ApplicationUser88 ,
,88, -
IdentityRole88. :
>88: ;
(88; <
)88< =
.99 $
AddEntityFrameworkStores99 
<99 
HealthAxisDbContext99 1
>991 2
(992 3
)993 4
.:: $
AddDefaultTokenProviders:: 
(:: 
):: 
;::  
builder== 
.== 
Services== 
.== 
AddAuthentication== "
(==" #
JwtBearerDefaults==# 4
.==4 5 
AuthenticationScheme==5 I
)==I J
.>> 
AddJwtBearer>> 
(>> 
options>> 
=>>> 
{?? 
var@@ 
jwt@@ 
=@@ 
builder@@ 
.@@ 
Configuration@@ '
.@@' (

GetSection@@( 2
(@@2 3
$str@@3 8
)@@8 9
;@@9 :
optionsBB 
.BB %
TokenValidationParametersBB )
=BB* +
newBB, /%
TokenValidationParametersBB0 I
{CC 	
ValidateIssuerDD 
=DD 
trueDD !
,DD! "
ValidIssuerEE 
=EE 
jwtEE 
[EE 
$strEE &
]EE& '
,EE' (
ValidateAudienceGG 
=GG 
trueGG #
,GG# $
ValidAudienceHH 
=HH 
jwtHH 
[HH  
$strHH  *
]HH* +
,HH+ ,
ValidateLifetimeJJ 
=JJ 
trueJJ #
,JJ# $$
ValidateIssuerSigningKeyLL $
=LL% &
trueLL' +
,LL+ ,
IssuerSigningKeyMM 
=MM 
newMM " 
SymmetricSecurityKeyMM# 7
(MM7 8
EncodingNN 
.NN 
UTF8NN 
.NN 
GetBytesNN &
(NN& '
jwtNN' *
[NN* +
$strNN+ 0
]NN0 1
!NN1 2
)NN2 3
)OO 
}PP 	
;PP	 

}QQ 
)QQ 
;QQ 
builderSS 
.SS 
ServicesSS 
.SS 
AddAuthorizationSS !
(SS! "
)SS" #
;SS# $
builderVV 
.VV 
ServicesVV 
.VV 
	AddScopedVV 
(VV 
typeofVV !
(VV! "
IRepositoryVV" -
<VV- .
>VV. /
)VV/ 0
,VV0 1
typeofVV2 8
(VV8 9

RepositoryVV9 C
<VVC D
>VVD E
)VVE F
)VVF G
;VVG H
builderYY 
.YY 
ServicesYY 
.YY 
	AddScopedYY 
<YY 
IPatientRepositoryYY -
,YY- .
PatientRepositoryYY/ @
>YY@ A
(YYA B
)YYB C
;YYC D
builderZZ 
.ZZ 
ServicesZZ 
.ZZ 
	AddScopedZZ 
<ZZ 
IPatientServiceZZ *
,ZZ* +
PatientServiceZZ, :
>ZZ: ;
(ZZ; <
)ZZ< =
;ZZ= >
builder]] 
.]] 
Services]] 
.]] 
	AddScoped]] 
<]] 
IDoctorRepository]] ,
,]], -
DoctorRepository]]. >
>]]> ?
(]]? @
)]]@ A
;]]A B
builder^^ 
.^^ 
Services^^ 
.^^ 
	AddScoped^^ 
<^^ 
IDoctorService^^ )
,^^) *
DoctorService^^+ 8
>^^8 9
(^^9 :
)^^: ;
;^^; <
builderaa 
.aa 
Servicesaa 
.aa 
	AddScopedaa 
<aa "
IAppointmentRepositoryaa 1
,aa1 2!
AppointmentRepositoryaa3 H
>aaH I
(aaI J
)aaJ K
;aaK L
builderbb 
.bb 
Servicesbb 
.bb 
	AddScopedbb 
<bb 
IAppointmentServicebb .
,bb. /
AppointmentServicebb0 B
>bbB C
(bbC D
)bbD E
;bbE F
builderee 
.ee 
Servicesee 
.ee 
	AddScopedee 
<ee #
IHealthRecordRepositoryee 2
,ee2 3"
HealthRecordRepositoryee4 J
>eeJ K
(eeK L
)eeL M
;eeM N
builderff 
.ff 
Servicesff 
.ff 
	AddScopedff 
<ff  
IHealthRecordServiceff /
,ff/ 0
HealthRecordServiceff1 D
>ffD E
(ffE F
)ffF G
;ffG H
builderii 
.ii 
Servicesii 
.ii 
	AddScopedii 
<ii 
IAuthServiceii '
,ii' (
AuthServiceii) 4
>ii4 5
(ii5 6
)ii6 7
;ii7 8
builderll 
.ll 
Servicesll 
.ll 
AddAutoMapperll 
(ll 
cfgll "
=>ll# %
{ll& '
}ll( )
,ll) *
	AppDomainll+ 4
.ll4 5
CurrentDomainll5 B
.llB C
GetAssembliesllC P
(llP Q
)llQ R
)llR S
;llS T
varnn 
appnn 
=nn 	
buildernn
 
.nn 
Buildnn 
(nn 
)nn 
;nn 
ifqq 
(qq 
appqq 
.qq 
Environmentqq 
.qq 
IsDevelopmentqq !
(qq! "
)qq" #
)qq# $
{rr 
appss 
.ss 

UseSwaggerss 
(ss 
)ss 
;ss 
apptt 
.tt 
UseSwaggerUItt 
(tt 
)tt 
;tt 
}uu 
appww 
.ww 
UseHttpsRedirectionww 
(ww 
)ww 
;ww 
appzz 
.zz 
UseExceptionHandlerzz 
(zz 
)zz 
;zz 
app}} 
.}} 
UseAuthentication}} 
(}} 
)}} 
;}} 
app~~ 
.~~ 
UseAuthorization~~ 
(~~ 
)~~ 
;~~ 
appÄÄ 
.
ÄÄ 
MapControllers
ÄÄ 
(
ÄÄ 
)
ÄÄ 
;
ÄÄ 
usingÉÉ 
(
ÉÉ 
var
ÉÉ 

scope
ÉÉ 
=
ÉÉ 
app
ÉÉ 
.
ÉÉ 
Services
ÉÉ 
.
ÉÉ  
CreateScope
ÉÉ  +
(
ÉÉ+ ,
)
ÉÉ, -
)
ÉÉ- .
{ÑÑ 
var
ÖÖ 
roleManager
ÖÖ 
=
ÖÖ 
scope
ÖÖ 
.
ÖÖ 
ServiceProvider
ÖÖ +
.
ÖÖ+ , 
GetRequiredService
ÖÖ, >
<
ÖÖ> ?
RoleManager
ÖÖ? J
<
ÖÖJ K
IdentityRole
ÖÖK W
>
ÖÖW X
>
ÖÖX Y
(
ÖÖY Z
)
ÖÖZ [
;
ÖÖ[ \
var
ÜÜ 
userManager
ÜÜ 
=
ÜÜ 
scope
ÜÜ 
.
ÜÜ 
ServiceProvider
ÜÜ +
.
ÜÜ+ , 
GetRequiredService
ÜÜ, >
<
ÜÜ> ?
UserManager
ÜÜ? J
<
ÜÜJ K
ApplicationUser
ÜÜK Z
>
ÜÜZ [
>
ÜÜ[ \
(
ÜÜ\ ]
)
ÜÜ] ^
;
ÜÜ^ _
await
àà 	

RoleSeeder
àà
 
.
àà 
	SeedRoles
àà 
(
àà 
roleManager
àà *
)
àà* +
;
àà+ ,
await
ââ 	
AdminSeeder
ââ
 
.
ââ 
	SeedAdmin
ââ 
(
ââ  
userManager
ââ  +
)
ââ+ ,
;
ââ, -
}ää 
appåå 
.
åå 
Run
åå 
(
åå 
)
åå 	
;
åå	 
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
} ‚8
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
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
FullNameRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits &
.& '
FullNameLength' 5
)5 6
]6 7
[ 	
RegularExpression	 
( 
RegexPatterns (
.( )
FullName) 1
,1 2
ErrorMessage3 ?
=@ A
ValidationMessagesB T
.T U!
InvalidFullNameFormatU j
)j k
]k l
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DateOfBirthRequired4 G
)G H
]H I
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
[ 	
CustomValidation	 
( 
typeof 
( 
Patient 
) 
, 
nameof 
( 
ValidateDateOfBirth &
)& '
)' (
]( )
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
GenderRequired4 B
)B C
]C D
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
PhoneNumberRequired4 G
)G H
]H I
[ 	
StringLength	 
( 
ValidationLimits &
.& '
PhoneNumberLength' 8
)8 9
]9 :
[   	
RegularExpression  	 
(   
RegexPatterns   (
.  ( )
PhoneNumber  ) 4
,  4 5
ErrorMessage  6 B
=  C D
ValidationMessages  E W
.  W X$
InvalidPhoneNumberFormat  X p
)  p q
]  q r
public!! 
string!! 
PhoneNumber!! !
{!!" #
get!!$ '
;!!' (
set!!) ,
;!!, -
}!!. /
=!!0 1
string!!2 8
.!!8 9
Empty!!9 >
;!!> ?
[## 	
Required##	 
(## 
ErrorMessage## 
=##  
ValidationMessages##! 3
.##3 4
EmailRequired##4 A
)##A B
]##B C
[$$ 	
EmailAddress$$	 
($$ 
ErrorMessage%% 
=%% 
ValidationMessages%% -
.%%- .
InvalidEmailFormat%%. @
)%%@ A
]%%A B
[&& 	
StringLength&&	 
(&& 
ValidationLimits&& &
.&&& '
EmailLength&&' 2
)&&2 3
]&&3 4
public'' 
string'' 
Email'' 
{'' 
get'' !
;''! "
set''# &
;''& '
}''( )
=''* +
string'', 2
.''2 3
Empty''3 8
;''8 9
public)) 
DateTime)) 
CreatedDate)) #
{))$ %
get))& )
;))) *
set))+ .
;)). /
}))0 1
=))2 3
DateTime))4 <
.))< =
Now))= @
;))@ A
public++ 
string++ 
?++ 
InsuranceId++ "
{++# $
get++% (
;++( )
set++* -
;++- .
}++/ 0
public.. 
virtual.. 
ICollection.. "
<.." #
Appointment..# .
>... /
Appointments..0 <
{..= >
get..? B
;..B C
set..D G
;..G H
}..I J
=// 
new// 
List// 
<// 
Appointment// "
>//" #
(//# $
)//$ %
;//% &
public11 
virtual11 
ICollection11 "
<11" #
HealthRecord11# /
>11/ 0
HealthRecords111 >
{11? @
get11A D
;11D E
set11F I
;11I J
}11K L
=22 
new22 
List22 
<22 
HealthRecord22 #
>22# $
(22$ %
)22% &
;22& '
public44 
int44 
GetAge44 
(44 
)44 
{55 	
int66 
age66 
=66 
DateTime66 
.66 
Today66 $
.66$ %
Year66% )
-66* +
DateOfBirth66, 7
.667 8
Year668 <
;66< =
if88 
(88 
DateOfBirth88 
>88 
DateTime88 &
.88& '
Today88' ,
.88, -
AddYears88- 5
(885 6
-886 7
age887 :
)88: ;
)88; <
{99 
age:: 
--:: 
;:: 
};; 
return== 
age== 
;== 
}>> 	
public@@ 
static@@ 
ValidationResult@@ &
?@@& '
ValidateDateOfBirth@@( ;
(@@; <
DateTimeAA 
dateAA 
,AA 
ValidationContextBB 
contextBB %
)BB% &
{CC 	
ifDD 
(DD 
dateDD 
.DD 
YearDD 
<DD 
$numDD  
)DD  !
{EE 
returnFF 
newFF 
ValidationResultFF +
(FF+ ,
ValidationMessagesGG &
.GG& ',
 DateOfBirthYearMustBe1900OrLaterGG' G
)GGG H
;GGH I
}HH 
ifJJ 
(JJ 
dateJJ 
>JJ 
DateTimeJJ 
.JJ  
TodayJJ  %
)JJ% &
{KK 
returnLL 
newLL 
ValidationResultLL +
(LL+ ,
ValidationMessagesMM &
.MM& '%
DateOfBirthCannotBeFutureMM' @
)MM@ A
;MMA B
}NN 
returnPP 
ValidationResultPP #
.PP# $
SuccessPP$ +
;PP+ ,
}QQ 	
}RR 
}SS ®&
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
}00 å.
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
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4"
SpecialisationRequired4 J
)J K
]K L
public 
Specialisation 
Specialisation ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
[ 	
Range	 
( 
ValidationLimits 
. 
MinExperience *
,* +
ValidationLimits 
. 
MaxExperience *
,* +
ErrorMessage 
= 
ValidationMessages -
.- ."
InvalidExperienceRange. D
)D E
]E F
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Range	 
( 
typeof 
( 
decimal 
) 
, 
ValidationLimits 
. 
MinConsultationFee /
,/ 0
ValidationLimits   
.   
MaxConsultationFee   /
,  / 0
ErrorMessage!! 
=!! 
ValidationMessages!! -
.!!- ."
InvalidConsultationFee!!. D
)!!D E
]!!E F
["" 	
	Precision""	 
("" 
$num"" 
,"" 
$num"" 
)"" 
]"" 
public## 
decimal## 
ConsultationFee## &
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
public(( 
virtual(( 
ICollection(( "
<((" #
Appointment((# .
>((. /
Appointments((0 <
{((= >
get((? B
;((B C
set((D G
;((G H
}((I J
=)) 
new)) 
List)) 
<)) 
Appointment)) "
>))" #
())# $
)))$ %
;))% &
public++ 
virtual++ 
ICollection++ "
<++" #
HealthRecord++# /
>++/ 0
HealthRecords++1 >
{++? @
get++A D
;++D E
set++F I
;++I J
}++K L
=,, 
new,, 
List,, 
<,, 
HealthRecord,, #
>,,# $
(,,$ %
),,% &
;,,& '
public.. 
bool.. 
IsAvailable.. 
(..  
DateTime// 
scheduledDate// "
,//" #
string00 
timeSlot00 
)00 
{11 	
return33 
!33 
Appointments33  
.33  !
Any33! $
(33$ %
a33% &
=>33' )
a44 
.44 
ScheduledDate44 
.44  
Date44  $
==44% '
scheduledDate44( 5
.445 6
Date446 :
&&44; =
a55 
.55 
TimeSlot55 
==55 
timeSlot55 &
&&55' )
a66 
.66 
Status66 
!=66 
AppointmentStatus66 -
.66- .
	Cancelled66. 7
)667 8
;668 9
}77 	
public99 
int99 '
GetUpcomingAppointmentCount99 .
(99. /
)99/ 0
{:: 	
return<< 
Appointments<< 
.<<  
Count<<  %
(<<% &
a<<& '
=><<( *
a== 
.== 
ScheduledDate== 
.==  
Date==  $
>===% '
DateTime==( 0
.==0 1
Today==1 6
&&==7 9
a>> 
.>> 
Status>> 
!=>> 
AppointmentStatus>> -
.>>- .
	Cancelled>>. 7
)>>7 8
;>>8 9
}?? 	
publicAA 
voidAA 
ActivateAA 
(AA 
)AA 
{BB 	
IsActiveCC 
=CC 
trueCC 
;CC 
}DD 	
publicFF 
voidFF 

DeactivateFF 
(FF 
)FF  
{GG 	
IsActiveHH 
=HH 
falseHH 
;HH 
}II 	
}JJ 
}KK î
iC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Auth\RegisterDto.cs
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
} ˛
tC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Auth\RefreshTokenRequestDto.cs
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
} ≈
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
} £
fC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Auth\LoginDto.cs
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
}( )
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ø
jC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Models\Auth\AuthResponse.cs
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
AuthResponse 
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
string		 
Message		 
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
public 
int 
	ExpiresIn 
{ 
get "
;" #
set$ '
;' (
}) *
public 
bool "
RequiresPasswordChange *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
=9 :
false; @
;@ A
} 
} ˙
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
}pp Ú
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
	namespace 	

HealthAxis
 
. 
API 
. 
Profiles !
{ 
public		 

class		 
MappingProfile		 
:		  !
Profile		" )
{

 
public 
MappingProfile 
( 
) 
{ 	
	CreateMap 
< 
CreatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
UpdatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
Patient 
, 

PatientDto )
>) *
(* +
)+ ,
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
Age! $
,$ %
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
GetAge2 8
(8 9
)9 :
): ;
); <
;< =
	CreateMap 
< 
CreateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
UpdateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
Doctor 
, 
	DoctorDto '
>' (
(( )
)) *
. 
	ForMember 
( 
dest 
=> 
dest  
.  !$
UpcomingAppointmentCount! 9
,9 :
opt   
=>   
opt   
.   
MapFrom   &
(  & '
src!! 
=>!! 
src!! "
.!!" #'
GetUpcomingAppointmentCount!!# >
(!!> ?
)!!? @
)!!@ A
)!!A B
;!!B C
	CreateMap$$ 
<$$  
CreateAppointmentDto$$ *
,$$* +
Appointment$$, 7
>$$7 8
($$8 9
)$$9 :
;$$: ;
	CreateMap&& 
<&& &
UpdateAppointmentStatusDto&& 0
,&&0 1
Appointment&&2 =
>&&= >
(&&> ?
)&&? @
;&&@ A
	CreateMap(( 
<(( 
Appointment(( !
,((! "
AppointmentDto((# 1
>((1 2
(((2 3
)((3 4
;((4 5
	CreateMap++ 
<++ !
CreateHealthRecordDto++ +
,+++ ,
HealthRecord,, 
>,, 
(,, 
),, 
;,,  
	CreateMap.. 
<.. 
HealthRecord.. "
,.." #
HealthRecordDto// 
>//  
(//  !
)//! "
;//" #
	CreateMap11 
<11 
HealthRecord11 "
,11" #
HealthRecordDto11$ 3
>113 4
(114 5
)115 6
;116 7
	CreateMap33 
<33 !
CreateHealthRecordDto33 +
,33+ ,
HealthRecord33- 9
>339 :
(33: ;
)33; <
;33< =
}44 	
}55 
}66 ˝
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
 ˛
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
 º
`C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Enums\UserRole.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Enums 
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
}		 ˆ
gC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Enums\InsuranceStatus.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Enums 
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
 π
^C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Enums\Gender.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Enums 
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
}		 ä	
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Enums\DoctorSpecialisation.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Enums 
{ 
public 

enum 
Specialisation 
{ 

Cardiology 
= 
$num 
, 
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
} ‚
iC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Enums\AppointmentStatus.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Enums 
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
= 
$num 
, 
	Cancelled 
= 
$num 
, 
	Completed 
= 
$num 
}		 
}

 ß

^C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\UserDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
; 
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
} ˙
gC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\UpdatePatientDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
{ 
public 

class 
UpdatePatientDto !
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
RegexPatterns 
. 
FullName "
," #
ErrorMessage 
= 
ValidationMessages -
.- .!
InvalidFullNameFormat. C
)C D
]D E
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DateOfBirthRequired4 G
)G H
]H I
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
GenderRequired4 B
)B C
]C D
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
PhoneNumberRequired4 G
)G H
]H I
[ 	
StringLength	 
( 
ValidationLimits &
.& '
PhoneNumberLength' 8
)8 9
]9 :
[ 	
RegularExpression	 
( 
RegexPatterns 
. 
PhoneNumber %
,% &
ErrorMessage 
= 
ValidationMessages -
.- .$
InvalidPhoneNumberFormat. F
)F G
]G H
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
EmailRequired4 A
)A B
]B C
[ 	
EmailAddress	 
( 
ErrorMessage   
=   
ValidationMessages   -
.  - .
InvalidEmailFormat  . @
)  @ A
]  A B
[!! 	
StringLength!!	 
(!! 
ValidationLimits!! &
.!!& '
EmailLength!!' 2
)!!2 3
]!!3 4
public"" 
string"" 
Email"" 
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
;""8 9
public$$ 
string$$ 
?$$ 
InsuranceId$$ "
{$$# $
get$$% (
;$$( )
set$$* -
;$$- .
}$$/ 0
}%% 
}&& ¯
fC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\UpdateDoctorDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
} º
qC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\UpdateAppointmentStatusDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
} Œ
bC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\RegisterDto.cs
	namespace 	

Healthcare
 
. 
netcore 
. 
DTOs !
{ 
public 

class 
RegisterDto 
{ 
} 
} –
aC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\PatientDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
;		; <
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
int 
Age 
{ 
get 
; 
set !
;! "
}# $
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
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
;8 9
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
? 
InsuranceId "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} »
_C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\LoginDto.cs
	namespace 	

Healthcare
 
. 
netcore 
. 
DTOs !
{ 
public 

class 
LoginDto 
{ 
} 
} „
dC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\ErrorResponse.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
} ≠
fC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\HealthRecordDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
{ 
public 

class 
HealthRecordDto  
{ 
public 
int 
RecordId 
{ 
get !
;! "
set# &
;& '
}( )
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
int		 
DoctorId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
DateTime 
	VisitDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
	Diagnosis 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
string0 6
.6 7
Empty7 <
;< =
public 
string 
Prescription "
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
;? @
public 
string 
Notes 
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
} 
} Æ
`C:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\DoctorDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
{ 
public 

class 
	DoctorDto 
{ 
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
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public		 
Specialisation		 
Specialisation		 ,
{		- .
get		/ 2
;		2 3
set		4 7
;		7 8
}		9 :
public

 
int

 
YearsOfExperience

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

1 2
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int $
UpcomingAppointmentCount +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
} 
} ˙
gC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\CreatePatientDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
{ 
public 

class 
CreatePatientDto !
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
RegexPatterns 
. 
FullName "
," #
ErrorMessage 
= 
ValidationMessages -
.- .!
InvalidFullNameFormat. C
)C D
]D E
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
DateOfBirthRequired4 G
)G H
]H I
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
GenderRequired4 B
)B C
]C D
public 
Gender 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
PhoneNumberRequired4 G
)G H
]H I
[ 	
StringLength	 
( 
ValidationLimits &
.& '
PhoneNumberLength' 8
)8 9
]9 :
[ 	
RegularExpression	 
( 
RegexPatterns 
. 
PhoneNumber %
,% &
ErrorMessage 
= 
ValidationMessages -
.- .$
InvalidPhoneNumberFormat. F
)F G
]G H
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
EmailRequired4 A
)A B
]B C
[ 	
EmailAddress	 
( 
ErrorMessage   
=   
ValidationMessages   -
.  - .
InvalidEmailFormat  . @
)  @ A
]  A B
[!! 	
StringLength!!	 
(!! 
ValidationLimits!! &
.!!& '
EmailLength!!' 2
)!!2 3
]!!3 4
public"" 
string"" 
Email"" 
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
;""8 9
public$$ 
string$$ 
?$$ 
InsuranceId$$ "
{$$# $
get$$% (
;$$( )
set$$* -
;$$- .
}$$/ 0
}%% 
}&& Ú
kC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\CreateAppointmentDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
{ 
public 

class  
CreateAppointmentDto %
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
ValidationMessages! 3
.3 4
PatientRequired4 C
)C D
]D E
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
=  
ValidationMessages! 3
.3 4
DoctorRequired4 B
)B C
]C D
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
=  
ValidationMessages! 3
.3 4#
AppointmentDateRequired4 K
)K L
]L M
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
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
=  
ValidationMessages! 3
.3 4
TimeSlotRequired4 D
)D E
]E F
[ 	
StringLength	 
( 
ValidationLimits 
. 
TimeSlotLength +
,+ ,
ErrorMessage 
= 
ValidationMessages -
.- .
InvalidTimeSlot. =
)= >
]> ?
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
} 
} ‰
fC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\CreateDoctorDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
}   Œ	
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\CreateHealthRecordDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
} √

hC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\ChangePasswordDto.cs
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
} é
eC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\DTOs\AppointmentDto.cs
	namespace 	

HealthAxis
 
. 
API 
. 
DTOs 
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
} ë
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
} ‹?
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
ModelBuilder0 <
modelBuilder= I
)I J
{ 	
base 
. 
OnModelCreating  
(  !
modelBuilder! -
)- .
;. /
foreach 
( 
var 
relationship %
in& (
modelBuilder) 5
.5 6
Model6 ;
. 
GetEntityTypes 
(  
)  !
.   

SelectMany   
(   
e   
=>    
e  ! "
.  " #
GetForeignKeys  # 1
(  1 2
)  2 3
)  3 4
)  4 5
{!! 
relationship"" 
."" 
DeleteBehavior"" +
="", -
DeleteBehavior"". <
.""< =
Restrict""= E
;""E F
}## 
SeedData%% 
(%% 
modelBuilder%% !
)%%! "
;%%" #
}&& 	
private(( 
static(( 
void(( 
SeedData(( $
((($ %
ModelBuilder((% 1
modelBuilder((2 >
)((> ?
{)) 	
modelBuilder** 
.** 
Entity** 
<**  
Doctor**  &
>**& '
(**' (
)**( )
.**) *
HasData*** 1
(**1 2
new++ 
Doctor++ 
{,, 
DoctorId-- 
=-- 
$num--  
,--  !
FullName.. 
=.. 
$str.. *
,..* +
Specialisation// "
=//# $
Specialisation//% 3
.//3 4

Cardiology//4 >
,//> ?
YearsOfExperience00 %
=00& '
$num00( *
,00* +
ConsultationFee11 #
=11$ %
$num11& *
,11* +
IsActive22 
=22 
true22 #
}33 
,33 
new44 
Doctor44 
{55 
DoctorId66 
=66 
$num66  
,66  !
FullName77 
=77 
$str77 )
,77) *
Specialisation88 "
=88# $
Specialisation88% 3
.883 4
Dermatology884 ?
,88? @
YearsOfExperience99 %
=99& '
$num99( )
,99) *
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
,??  !
FullName@@ 
=@@ 
$str@@ )
,@@) *
SpecialisationAA "
=AA# $
SpecialisationAA% 3
.AA3 4
	NeurologyAA4 =
,AA= >
YearsOfExperienceBB %
=BB& '
$numBB( *
,BB* +
ConsultationFeeCC #
=CC$ %
$numCC& +
,CC+ ,
IsActiveDD 
=DD 
trueDD #
}EE 
,EE 
newFF 
DoctorFF 
{GG 
DoctorIdHH 
=HH 
$numHH  
,HH  !
FullNameII 
=II 
$strII +
,II+ ,
SpecialisationJJ "
=JJ# $
SpecialisationJJ% 3
.JJ3 4

PediatricsJJ4 >
,JJ> ?
YearsOfExperienceKK %
=KK& '
$numKK( )
,KK) *
ConsultationFeeLL #
=LL$ %
$numLL& *
,LL* +
IsActiveMM 
=MM 
trueMM #
}NN 
,NN 
newOO 
DoctorOO 
{PP 
DoctorIdQQ 
=QQ 
$numQQ  
,QQ  !
FullNameRR 
=RR 
$strRR )
,RR) *
SpecialisationSS "
=SS# $
SpecialisationSS% 3
.SS3 4
OrthopedicsSS4 ?
,SS? @
YearsOfExperienceTT %
=TT& '
$numTT( *
,TT* +
ConsultationFeeUU #
=UU$ %
$numUU& *
,UU* +
IsActiveVV 
=VV 
trueVV #
}WW 
)XX 
;XX 
modelBuilderZZ 
.ZZ 
EntityZZ 
<ZZ  
PatientZZ  '
>ZZ' (
(ZZ( )
)ZZ) *
.ZZ* +
HasDataZZ+ 2
(ZZ2 3
new[[ 
Patient[[ 
{\\ 
	PatientId]] 
=]] 
$num]]  !
,]]! "
FullName^^ 
=^^ 
$str^^ %
,^^% &
DateOfBirth__ 
=__  !
new__" %
DateTime__& .
(__. /
$num__/ 3
,__3 4
$num__5 6
,__6 7
$num__8 :
)__: ;
,__; <
Gender`` 
=`` 
Gender`` #
.``# $
Female``$ *
,``* +
PhoneNumberaa 
=aa  !
$straa" .
,aa. /
Emailbb 
=bb 
$strbb 0
,bb0 1
InsuranceIdcc 
=cc  !
$strcc" +
,cc+ ,
CreatedDatedd 
=dd  !
newdd" %
DateTimedd& .
(dd. /
$numdd/ 3
,dd3 4
$numdd5 6
,dd6 7
$numdd8 9
)dd9 :
}ee 
,ee 
newff 
Patientff 
{gg 
	PatientIdhh 
=hh 
$numhh  !
,hh! "
FullNameii 
=ii 
$strii &
,ii& '
DateOfBirthjj 
=jj  !
newjj" %
DateTimejj& .
(jj. /
$numjj/ 3
,jj3 4
$numjj5 6
,jj6 7
$numjj8 :
)jj: ;
,jj; <
Genderkk 
=kk 
Genderkk #
.kk# $
Malekk$ (
,kk( )
PhoneNumberll 
=ll  !
$strll" .
,ll. /
Emailmm 
=mm 
$strmm /
,mm/ 0
InsuranceIdnn 
=nn  !
$strnn" +
,nn+ ,
CreatedDateoo 
=oo  !
newoo" %
DateTimeoo& .
(oo. /
$numoo/ 3
,oo3 4
$numoo5 6
,oo6 7
$numoo8 9
)oo9 :
}pp 
)qq 
;qq 
}rr 	
}ss 
}tt æ
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
}$$ ”
oC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\PatientController.cs
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
$str		W `
)		` a
]		a b
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
$str 
) 
] 
public 

class 
PatientController "
:# $
ControllerBase% 3
{ 
private 
readonly 
IPatientService (
_service) 1
;1 2
public 
PatientController  
(  !
IPatientService! 0
service1 8
)8 9
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
int- 0
id1 3
)3 4
{ 	
var 
patient 
= 
await 
_service  (
.( )
GetByIdAsync) 5
(5 6
id6 8
)8 9
;9 :
return 
Ok 
( 
patient 
) 
; 
} 	
[ 	
HttpPut	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Update) /
(/ 0
int0 3
id4 6
,6 7
UpdatePatientDto8 H
dtoI L
)L M
{   	
var!! 
result!! 
=!! 
await!! 
_service!! '
.!!' (
UpdateAsync!!( 3
(!!3 4
id!!4 6
,!!6 7
dto!!8 ;
)!!; <
;!!< =
return"" 
Ok"" 
("" 
result"" 
)"" 
;"" 
}## 	
[&& 	
HttpGet&&	 
(&& 
$str&& &
)&&& '
]&&' (
public'' 
async'' 
Task'' 
<'' 
IActionResult'' '
>''' (
GetHealthRecords'') 9
(''9 :
int'': =
id''> @
)''@ A
{(( 	
var)) 
records)) 
=)) 
await)) 
_service))  (
.))( )!
GetHealthRecordsAsync))) >
())> ?
id))? A
)))A B
;))B C
return** 
Ok** 
(** 
records** 
)** 
;** 
}++ 	
},, 
}--  
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
}00 õ
nC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\DoctorController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[ 
	Authorize 
( !
AuthenticationSchemes $
=% &
JwtBearerDefaults' 8
.8 9 
AuthenticationScheme9 M
,M N
RolesO T
=U V
$strW m
)m n
]n o
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
 
)

 
]

 
public 

class 
DoctorController !
:" #
ControllerBase$ 2
{ 
private 
readonly 
IDoctorService '
_service( 0
;0 1
public 
DoctorController 
(  
IDoctorService  .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
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
)0 1
{ 	
var 
doctors 
= 
await 
_service  (
.( )
GetAllAsync) 4
(4 5
)5 6
;6 7
return 
Ok 
( 
doctors 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
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
var   
doctor   
=   
await   
_service   '
.  ' (
GetByIdAsync  ( 4
(  4 5
id  5 7
)  7 8
;  8 9
return!! 
Ok!! 
(!! 
doctor!! 
)!! 
;!! 
}"" 	
[%% 	
HttpGet%%	 
(%% 
$str%% $
)%%$ %
]%%% &
public&& 
async&& 
Task&& 
<&& 
IActionResult&& '
>&&' (
GetAvailability&&) 8
(&&8 9
int&&9 <
id&&= ?
)&&? @
{'' 	
var(( 
availability(( 
=(( 
await(( $
_service((% -
.((- . 
GetAvailabilityAsync((. B
(((B C
id((C E
)((E F
;((F G
return)) 
Ok)) 
()) 
availability)) "
)))" #
;))# $
}** 	
}++ 
},, Ó2
lC:\Users\310481\source\repos\UST-Live-01\Healthcare.netcore\Healthcare.netcore\Controllers\AuthController.cs
	namespace 	

HealthAxis
 
. 
API 
. 
Controllers $
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
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
IAuthService %
_service& .
;. /
public 
AuthController 
( 
IAuthService *
service+ 2
)2 3
{ 	
_service 
= 
service 
; 
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
RegisterDto2 =
request> E
)E F
{ 	
var 
result 
= 
await 
_service '
.' (
Register( 0
(0 1
request1 8
)8 9
;9 :
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
(! "
new" %
{& '
message( /
=0 1
result2 8
.8 9
Message9 @
}A B
)B C
;C D
} 
return 
Ok 
( 
new 
{ 
message 
= 
result  
.  !
Message! (
,( )
userId 
= 
result 
.  
UserId  &
}   
)   
;   
}!! 	
[## 	
HttpPost##	 
(## 
$str## 
)## 
]## 
public$$ 
async$$ 
Task$$ 
<$$ 
IActionResult$$ '
>$$' (
Login$$) .
($$. /
LoginDto$$/ 7
request$$8 ?
)$$? @
{%% 	
var&& 
result&& 
=&& 
await&& 
_service&& '
.&&' (
Login&&( -
(&&- .
request&&. 5
)&&5 6
;&&6 7
if(( 
((( 
!(( 
result(( 
.(( 
Success(( 
)((  
{)) 
return** 
Unauthorized** #
(**# $
new**$ '
{**( )
message*** 1
=**2 3
result**4 :
.**: ;
Message**; B
}**C D
)**D E
;**E F
}++ 
return-- 
Ok-- 
(-- 
new-- 
AuthResponse-- &
{.. 
AccessToken// 
=// 
result// $
.//$ %
AccessToken//% 0
,//0 1
RefreshToken00 
=00 
result00 %
.00% &
RefreshToken00& 2
,002 3
Message11 
=11 
result11  
.11  !
Message11! (
,11( )
	ExpiresIn22 
=22 
result22 "
.22" #
	ExpiresIn22# ,
,22, -"
RequiresPasswordChange33 &
=33' (
result33) /
.33/ 0"
RequiresPasswordChange330 F
}44 
)44 
;44 
}55 	
[77 	
HttpPost77	 
(77 
$str77 #
)77# $
]77$ %
public88 
async88 
Task88 
<88 
IActionResult88 '
>88' (
ChangePassword88) 7
(887 8
ChangePasswordDto888 I
request88J Q
)88Q R
{99 	
var:: 
result:: 
=:: 
await:: 
_service:: '
.::' (
ChangePassword::( 6
(::6 7
request::7 >
)::> ?
;::? @
if<< 
(<< 
!<< 
result<< 
.<< 
Success<< 
)<<  
{== 
return>> 

BadRequest>> !
(>>! "
new>>" %
{>>& '
message>>( /
=>>0 1
result>>2 8
.>>8 9
Message>>9 @
}>>A B
)>>B C
;>>C D
}?? 
returnAA 
OkAA 
(AA 
newAA 
{AA 
messageAA #
=AA$ %
resultAA& ,
.AA, -
MessageAA- 4
}AA5 6
)AA6 7
;AA7 8
}BB 	
[DD 	
HttpPostDD	 
(DD 
$strDD !
)DD! "
]DD" #
publicEE 
asyncEE 
TaskEE 
<EE 
IActionResultEE '
>EE' (
RefreshTokenEE) 5
(EE5 6"
RefreshTokenRequestDtoEE6 L
requestEEM T
)EET U
{FF 	
varGG 
resultGG 
=GG 
awaitGG 
_serviceGG '
.GG' (
RefreshTokenGG( 4
(GG4 5
requestGG5 <
)GG< =
;GG= >
ifII 
(II 
!II 
resultII 
.II 
SuccessII 
)II  
{JJ 
returnKK 
UnauthorizedKK #
(KK# $
newKK$ '
{KK( )
messageKK* 1
=KK2 3
resultKK4 :
.KK: ;
MessageKK; B
}KKC D
)KKD E
;KKE F
}LL 
returnNN 
OkNN 
(NN 
newNN 
AuthResponseNN &
{OO 
AccessTokenPP 
=PP 
resultPP $
.PP$ %
AccessTokenPP% 0
,PP0 1
RefreshTokenQQ 
=QQ 
resultQQ %
.QQ% &
RefreshTokenQQ& 2
,QQ2 3
MessageRR 
=RR 
resultRR  
.RR  !
MessageRR! (
,RR( )
	ExpiresInSS 
=SS 
resultSS "
.SS" #
	ExpiresInSS# ,
,SS, -"
RequiresPasswordChangeTT &
=TT' (
falseTT) .
}UU 
)UU 
;UU 
}VV 	
}WW 
}XX ú#
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
[   	
	Authorize  	 
(   !
AuthenticationSchemes   (
=  ) *
JwtBearerDefaults  + <
.  < = 
AuthenticationScheme  = Q
,  Q R
Roles  S X
=  Y Z
$str  [ d
)  d e
]  e f
[!! 	
HttpPost!!	 
]!! 
public"" 
async"" 
Task"" 
<"" 
IActionResult"" '
>""' (
Create"") /
(""/ 0 
CreateAppointmentDto""0 D
dto""E H
)""H I
{## 	
var$$ 
result$$ 
=$$ 
await$$ 
_service$$ '
.$$' (
AddAsync$$( 0
($$0 1
dto$$1 4
)$$4 5
;$$5 6
return%% 
Ok%% 
(%% 
result%% 
)%% 
;%% 
}&& 	
[** 	
	Authorize**	 
(** !
AuthenticationSchemes** (
=**) *
JwtBearerDefaults**+ <
.**< = 
AuthenticationScheme**= Q
,**Q R
Roles**S X
=**Y Z
$str**[ k
)**k l
]**l m
[++ 	
HttpPut++	 
(++ 
$str++ 
)++ 
]++  
public,, 
async,, 
Task,, 
<,, 
IActionResult,, '
>,,' (
UpdateStatus,,) 5
(,,5 6
int,,6 9
id,,: <
,,,< =&
UpdateAppointmentStatusDto,,> X
dto,,Y \
),,\ ]
{-- 	
var.. 
result.. 
=.. 
await.. 
_service.. '
...' (
UpdateStatusAsync..( 9
(..9 :
id..: <
,..< =
dto..> A
)..A B
;..B C
return// 
Ok// 
(// 
result// 
)// 
;// 
}00 	
[44 	
	Authorize44	 
(44 !
AuthenticationSchemes44 (
=44) *
JwtBearerDefaults44+ <
.44< = 
AuthenticationScheme44= Q
,44Q R
Roles44S X
=44Y Z
$str44[ d
)44d e
]44e f
[55 	

HttpDelete55	 
(55 
$str55 
)55 
]55 
public66 
async66 
Task66 
<66 
IActionResult66 '
>66' (
Delete66) /
(66/ 0
int660 3
id664 6
)666 7
{77 	
var88 
result88 
=88 
await88 
_service88 '
.88' (
DeleteAsync88( 3
(883 4
id884 6
)886 7
;887 8
return99 
Ok99 
(99 
result99 
)99 
;99 
}:: 	
};; 
}<< ø
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
)4 5
{ 	
var 
result 
= 
await 
_doctorService -
.- .
GetAllAsync. 9
(9 :
): ;
;; <
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
}55 