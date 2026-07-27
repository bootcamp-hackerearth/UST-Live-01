Œ
UC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\IPatientService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
{ 
public 

	interface 
IPatientService $
{ 
Task 
< 
List 
< 

PatientDto 
> 
> 
GetAllAsync *
(* +
)+ ,
;, -
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
Task

 
<

 

PatientDto

 
>

 
CreateAsync

 $
(

$ %
CreatePatientDto

% 5
entity

6 <
)

< =
;

= >
Task 
< 

PatientDto 
? 
> 
UpdateAsync %
(% &
int& )
id* ,
,, -
UpdatePatientDto. >
entity? E
)E F
;F G
Task 
< 
List 
< 

PatientDto 
> 
> $
SearchByPatientNameAsync 7
(7 8
string8 >
name? C
)C D
;D E
Task 
< 

PatientDto 
? 
> $
SearchByPhoneNumberAsync 2
(2 3
string3 9
phoneNumber: E
)E F
;F G
Task 
< 

PatientDto 
? 
> 
SearchByEmailAsync ,
(, -
string- 3
email4 9
)9 :
;: ;
Task 
< 

PatientDto 
> "
DeactivatePatientAsync /
(/ 0
int0 3
id4 6
)6 7
;7 8
Task 
< 
List 
< 

PatientDto 
> 
> 
SearchAsync *
(* +
string+ 1
?1 2
name3 7
,7 8
string9 ?
?? @
phoneA F
)F G
;G H
Task 
< 
Patient 
? 
> 
GetByUserIdAsync '
(' (
string( .
userId/ 5
)5 6
;6 7
Task 
< 
PatientDetailsDto 
? 
>  +
GetPatientDetailsForDoctorAsync! @
(@ A
intA D
doctorIdE M
,M N
intN Q
	patientIdR [
)[ \
;\ ]
} 
} ã]
YC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\PatientService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
.$ %
Impl% )
{ 
public		 

class		 
PatientService		 
(		  
IPatientRepository		  2

repository		3 =
,		= >#
IHealthRecordRepository		? V"
healthRecordRepository		W m
,		m n
IMapper		o v
mapper		w }
)		} ~
:			 Ä
IPatientService
		Å ê
{

 
public 
async 
Task 
< 

PatientDto $
>$ %
CreateAsync& 1
(1 2
CreatePatientDto2 B
entityC I
)I J
{ 	
var 
patient 
= 
mapper  
.  !
Map! $
<$ %
Patient% ,
>, -
(- .
entity. 4
)4 5
;5 6
var 
savedEntity 
= 
await #

repository$ .
.. /
CreateAsync/ :
(: ;
patient; B
)B C
;C D
return 
mapper 
. 
Map 
< 

PatientDto (
>( )
() *
savedEntity* 5
)5 6
;6 7
} 	
public 
async 
Task 
< 

PatientDto $
>$ %"
DeactivatePatientAsync& <
(< =
int= @
idA C
)C D
{ 	
var 
existing 
= 
await  

repository! +
.+ ,
GetByIdAsync, 8
(8 9
id9 ;
); <
;< =
if 
( 
existing 
== 
null  
)  !
throw 
new 
	Exception #
(# $
$str$ 7
)7 8
;8 9
existing 
. 
IsActive 
= 
!  !
existing! )
.) *
IsActive* 2
;2 3
var 
updated 
= 
await 

repository  *
.* +
UpdateAsync+ 6
(6 7
id7 9
,9 :
existing; C
)C D
;D E
return 
mapper 
. 
Map 
< 

PatientDto (
>( )
() *
updated* 1
)1 2
;2 3
}!! 	
public## 
async## 
Task## 
<## 
List## 
<## 

PatientDto## )
>##) *
>##* +
GetAllAsync##, 7
(##7 8
)##8 9
{$$ 	
var%% 
patients%% 
=%% 
await%%  

repository%%! +
.%%+ ,
GetAllAsync%%, 7
(%%7 8
)%%8 9
;%%9 :
return&& 
mapper&& 
.&& 
Map&& 
<&& 
List&& "
<&&" #

PatientDto&&# -
>&&- .
>&&. /
(&&/ 0
patients&&0 8
)&&8 9
;&&9 :
}'' 	
public)) 
async)) 
Task)) 
<)) 

PatientDto)) $
?))$ %
>))% &
GetByIdAsync))' 3
())3 4
int))4 7
id))8 :
))): ;
{** 	
var++ 
patient++ 
=++ 
await++ 

repository++  *
.++* +
GetByIdAsync+++ 7
(++7 8
id++8 :
)++: ;
;++; <
return,, 
mapper,, 
.,, 
Map,, 
<,, 

PatientDto,, (
>,,( )
(,,) *
patient,,* 1
),,1 2
;,,2 3
}-- 	
public// 
async// 
Task// 
<// 

PatientDto// $
?//$ %
>//% &
SearchByEmailAsync//' 9
(//9 :
string//: @
email//A F
)//F G
{00 	
var11 
patient11 
=11 
await11 

repository11  *
.11* +
SearchByEmailAsync11+ =
(11= >
email11> C
)11C D
;11D E
return22 
mapper22 
.22 
Map22 
<22 

PatientDto22 (
>22( )
(22) *
patient22* 1
)221 2
;222 3
}33 	
public55 
async55 
Task55 
<55 
List55 
<55 

PatientDto55 )
>55) *
>55* +$
SearchByPatientNameAsync55, D
(55D E
string55E K
name55L P
)55P Q
{66 	
var77 
patient77 
=77 
await77 

repository77  *
.77* +
SearchByNameAsync77+ <
(77< =
name77= A
)77A B
;77B C
return88 
mapper88 
.88 
Map88 
<88 
List88 "
<88" #

PatientDto88# -
>88- .
>88. /
(88/ 0
patient880 7
)887 8
;888 9
}99 	
public;; 
async;; 
Task;; 
<;; 

PatientDto;; $
?;;$ %
>;;% &$
SearchByPhoneNumberAsync;;' ?
(;;? @
string;;@ F
phoneNumber;;G R
);;R S
{<< 	
var== 
patient== 
=== 
await== 

repository==  *
.==* +$
SearchByPhoneNumberAsync==+ C
(==C D
phoneNumber==D O
)==O P
;==P Q
return>> 
mapper>> 
.>> 
Map>> 
<>> 

PatientDto>> (
>>>( )
(>>) *
patient>>* 1
)>>1 2
;>>2 3
}?? 	
publicAA 
asyncAA 
TaskAA 
<AA 

PatientDtoAA $
?AA$ %
>AA% &
UpdateAsyncAA' 2
(AA2 3
intAA3 6
idAA7 9
,AA9 :
UpdatePatientDtoAA; K
entityAAL R
)AAR S
{BB 	
ifDD 
(DD 
entityDD 
.DD 
DateOfBirthDD "
>DD# $
DateTimeDD% -
.DD- .
TodayDD. 3
)DD3 4
{EE 
throwFF 
newFF 
	ExceptionFF #
(FF# $
$strFF$ ;
)FF; <
;FF< =
}GG 
varII 
existingEmailII 
=II 
awaitII  %

repositoryII& 0
.II0 1
SearchByEmailAsyncII1 C
(IIC D
entityIID J
.IIJ K
EmailIIK P
)IIP Q
;IIQ R
ifKK 
(KK 
existingEmailKK 
!=KK  
nullKK! %
&&KK& (
existingEmailKK) 6
.KK6 7
	PatientIdKK7 @
!=KKA C
idKKD F
)KKF G
{LL 
throwMM 
newMM 
	ExceptionMM #
(MM# $
$strMM$ :
)MM: ;
;MM; <
}NN 
varQQ 
existingQQ 
=QQ 
awaitQQ  

repositoryQQ! +
.QQ+ ,
GetByIdAsyncQQ, 8
(QQ8 9
idQQ9 ;
)QQ; <
;QQ< =
ifRR 
(RR 
existingRR 
==RR 
nullRR  
)RR  !
throwSS 
newSS 
	ExceptionSS #
(SS# $
$strSS$ 7
)SS7 8
;SS8 9
existingUU 
.UU 
PatientNameUU  
=UU! "
entityUU# )
.UU) *
PatientNameUU* 5
;UU5 6
existingVV 
.VV 
DateOfBirthVV  
=VV! "
entityVV# )
.VV) *
DateOfBirthVV* 5
;VV5 6
existingWW 
.WW 
GenderWW 
=WW 
entityWW $
.WW$ %
GenderWW% +
;WW+ ,
existingXX 
.XX 
EmailXX 
=XX 
entityXX #
.XX# $
EmailXX$ )
;XX) *
existingYY 
.YY 
PhoneNoYY 
=YY 
entityYY %
.YY% &
PhoneNoYY& -
;YY- .
existingZZ 
.ZZ 
InsuranceIDZZ  
=ZZ! "
entityZZ# )
.ZZ) *
InsuranceIDZZ* 5
;ZZ5 6
var\\ 
updated\\ 
=\\ 
await\\ 

repository\\  *
.\\* +
UpdateAsync\\+ 6
(\\6 7
id\\7 9
,\\9 :
existing\\; C
)\\C D
;\\D E
return^^ 
mapper^^ 
.^^ 
Map^^ 
<^^ 

PatientDto^^ (
>^^( )
(^^) *
updated^^* 1
)^^1 2
;^^2 3
}`` 	
publicbb 
asyncbb 
Taskbb 
<bb 
Listbb 
<bb 

PatientDtobb )
>bb) *
>bb* +
SearchAsyncbb, 7
(bb7 8
stringbb8 >
?bb> ?
namebb@ D
,bbD E
stringbbF L
?bbL M
phonebbN S
)bbS T
{cc 	
vardd 
patientsdd 
=dd 
awaitdd  

repositorydd! +
.dd+ ,
SearchAsyncdd, 7
(dd7 8
namedd8 <
,dd< =
phonedd> C
)ddC D
;ddD E
returnee 
mapperee 
.ee 
Mapee 
<ee 
Listee "
<ee" #

PatientDtoee# -
>ee- .
>ee. /
(ee/ 0
patientsee0 8
)ee8 9
;ee9 :
}ff 	
publichh 
asynchh 
Taskhh 
<hh 
Patienthh !
?hh! "
>hh" #
GetByUserIdAsynchh$ 4
(hh4 5
stringhh5 ;
userIdhh< B
)hhB C
{ii 	
returnjj 
awaitjj 

repositoryjj #
.jj# $
GetByUserIdAsyncjj$ 4
(jj4 5
userIdjj5 ;
)jj; <
;jj< =
}kk 	
publicmm 
asyncmm 
Taskmm 
<mm 
PatientDetailsDtomm +
?mm+ ,
>mm, -+
GetPatientDetailsForDoctorAsyncmm. M
(mmM N
intmmN Q
doctorIdmmR Z
,mmZ [
intmm[ ^
	patientIdmm_ h
)mmh i
{nn 	
varoo 
patientoo 
=oo 
awaitoo 

repositoryoo  *
.oo* +
GetByIdAsyncoo+ 7
(oo7 8
	patientIdoo8 A
)ooA B
;ooB C
ifqq 
(qq 
patientqq 
==qq 
nullqq 
)qq  
returnrr 
nullrr 
;rr 
vartt 
recordstt 
=tt 
awaituu "
healthRecordRepositoryuu ,
.vv +
GetRecordsForDoctorPatientAsyncvv 4
(vv4 5
doctorIdww  
,ww  !
	patientIdxx !
)xx! "
;xx" #
returnzz 
newzz 
PatientDetailsDtozz (
{{{ 
Patient|| 
=|| 
mapper||  
.||  !
Map||! $
<||$ %

PatientDto||% /
>||/ 0
(||0 1
patient||1 8
)||8 9
,||9 :
HealthRecords~~ 
=~~ 
mapper 
. 
Map 
< 
List #
<# $
HealthRecordDto$ 3
>3 4
>4 5
(5 6
records6 =
)= >
}
ÄÄ 
;
ÄÄ 
}
ÅÅ 	
}
ÉÉ 
}ÑÑ Ç
[C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\HeartbeatService.cs
public 
class 
HeartbeatService 
: 
BackgroundService  1
{ 
private 
readonly 
ILogger 
< 
HeartbeatService -
>- .
_logger/ 6
;6 7
public 

HeartbeatService 
( 
ILogger #
<# $
HeartbeatService$ 4
>4 5
logger6 <
)< =
{ 
_logger 
= 
logger 
; 
_logger 
. 
LogInformation 
( 
$str D
)D E
;E F
}		 
	protected 
override 
async 
Task !
ExecuteAsync" .
(. /
CancellationToken/ @
stoppingTokenA N
)N O
{ 
_logger 
. 
LogInformation 
( 
$str 9
)9 :
;: ;
while 
( 
! 
stoppingToken 
. #
IsCancellationRequested 5
)5 6
{ 	
_logger 
. 
LogInformation "
(" #
$str# 8
,8 9
DateTime: B
.B C
UtcNowC I
)I J
;J K
await 
Task 
. 
Delay 
( 
TimeSpan %
.% &
FromSeconds& 1
(1 2
$num2 5
)5 6
,6 7
stoppingToken8 E
)E F
;F G
} 	
} 
} ‹L
^C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\HealthRecordService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
.$ %
Impl% )
{ 
public		 

class		 
HealthRecordService		 $
(		$ %#
IHealthRecordRepository		% <

repository		= G
,		G H"
IAppointmentRepository		I _!
appointmentRepository		` u
,		u v
IMapper		w ~
mapper			 Ö
)
		Ö Ü
:
		á à"
IHealthRecordService
		â ù
{

 
public 
async 
Task 
< 
HealthRecordDto )
?) *
>* +
UpdateAsync, 7
(7 8
int8 ;
id< >
,> ?!
UpdateHealthRecordDto@ U
dtoV Y
)Y Z
{ 	
var 
record 
= 
await 

repository )
.) *
GetByIdAsync* 6
(6 7
id7 9
)9 :
;: ;
if 
( 
record 
== 
null 
) 
return 
null 
; 
var 
appointment 
= 
await #!
appointmentRepository$ 9
.9 :
GetByIdAsync: F
(F G
recordG M
.M N
AppointmentIdN [
)[ \
;\ ]
if 
( 
appointment 
? 
. 
Status #
!=$ &
$str' 2
)2 3
throw 
new 
	Exception #
(# $
$str$ Z
)Z [
;[ \
if 
( 
string 
. 
IsNullOrWhiteSpace )
() *
dto* -
.- .
	Diagnosis. 7
)7 8
)8 9
throw 
new 
	Exception #
(# $
$str$ ;
); <
;< =
if 
( 
string 
. 
IsNullOrWhiteSpace )
() *
dto* -
.- .
Prescription. :
): ;
); <
throw 
new 
	Exception #
(# $
$str$ >
)> ?
;? @
record 
. 
	Diagnosis 
= 
dto "
." #
	Diagnosis# ,
;, -
record 
. 
Prescription 
=  !
dto" %
.% &
Prescription& 2
;2 3
record   
.   
Notes   
=   
dto   
.   
Notes   $
;  $ %
var"" 
updated"" 
="" 
await"" 

repository""  *
.""* +
UpdateAsync""+ 6
(""6 7
id""7 9
,""9 :
record""; A
)""A B
;""B C
return$$ 
mapper$$ 
.$$ 
Map$$ 
<$$ 
HealthRecordDto$$ -
>$$- .
($$. /
updated$$/ 6
)$$6 7
;$$7 8
}%% 	
public(( 
async(( 
Task(( 
<(( 
List(( 
<(( 
HealthRecordDto(( .
>((. /
>((/ 0
GetAllAsync((1 <
(((< =
)((= >
{)) 	
return** 
mapper** 
.** 
Map** 
<** 
List** "
<**" #
HealthRecordDto**# 2
>**2 3
>**3 4
(**4 5
await**5 :

repository**; E
.**E F
GetAllAsync**F Q
(**Q R
)**R S
)**S T
;**T U
}++ 	
public-- 
async-- 
Task-- 
<-- 
HealthRecordDto-- )
?--) *
>--* +
GetByIdAsync--, 8
(--8 9
int--9 <
id--= ?
)--? @
{.. 	
var// 
healthRecord// 
=// 
await// $

repository//% /
./// 0
GetByIdAsync//0 <
(//< =
id//= ?
)//? @
;//@ A
return00 
mapper00 
.00 
Map00 
<00 
HealthRecordDto00 -
?00- .
>00. /
(00/ 0
healthRecord000 <
)00< =
;00= >
}11 	
public33 
async33 
Task33 
<33 
List33 
<33 
HealthRecordDto33 .
>33. /
>33/ 0&
GetRecordsByPatientIdAsync331 K
(33K L
int33L O
	patientId33P Y
)33Y Z
{44 	
return55 
mapper55 
.55 
Map55 
<55 
List55 "
<55" #
HealthRecordDto55# 2
>552 3
>553 4
(554 5
await555 :

repository55; E
.55E F&
GetRecordsByPatientIdAsync55F `
(55` a
	patientId55a j
)55j k
)55k l
;55l m
}66 	
public88 
async88 
Task88 
<88 
List88 
<88 
HealthRecordDto88 .
>88. /
>88/ 0%
GetRecordsByDoctorIdAsync881 J
(88J K
int88K N
doctorId88O W
)88W X
{99 	
return:: 
mapper:: 
.:: 
Map:: 
<:: 
List:: "
<::" #
HealthRecordDto::# 2
>::2 3
>::3 4
(::4 5
await::5 :

repository::; E
.::E F%
GetRecordsByDoctorIdAsync::F _
(::_ `
doctorId::` h
)::h i
)::i j
;::j k
};; 	
public== 
async== 
Task== 
<== 
List== 
<== 
HealthRecordDto== .
>==. /
>==/ 0'
GetRecordsByDoctorNameAsync==1 L
(==L M
string==M S

doctorName==T ^
)==^ _
{>> 	
return?? 
mapper?? 
.?? 
Map?? 
<?? 
List?? "
<??" #
HealthRecordDto??# 2
>??2 3
>??3 4
(??4 5
await??5 :

repository??; E
.??E F'
GetRecordsByDoctorNameAsync??F a
(??a b

doctorName??b l
)??l m
)??m n
;??n o
}@@ 	
publicBB 
asyncBB 
TaskBB 
<BB 
ListBB 
<BB 
HealthRecordDtoBB .
>BB. /
>BB/ 0(
GetRecordsByPatientNameAsyncBB1 M
(BBM N
stringBBN T
patientNameBBU `
)BB` a
{CC 	
returnDD 
mapperDD 
.DD 
MapDD 
<DD 
ListDD "
<DD" #
HealthRecordDtoDD# 2
>DD2 3
>DD3 4
(DD4 5
awaitDD5 :

repositoryDD; E
.DDE F(
GetRecordsByPatientNameAsyncDDF b
(DDb c
patientNameDDc n
)DDn o
)DDo p
;DDp q
}EE 	
publicGG 
asyncGG 
TaskGG !
CreateFromAppointmentGG /
(GG/ 0
AppointmentGG0 ;
appointmentGG< G
)GGG H
{HH 	
varII 
existsII 
=II 
awaitII 

repositoryII )
.II) *%
ExistsForAppointmentAsyncII* C
(IIC D
appointmentIID O
.IIO P
AppointmentIdIIP ]
)II] ^
;II^ _
ifKK 
(KK 
existsKK 
)KK 
returnLL 
;LL 
varNN 
recordNN 
=NN 
newNN 
HealthRecordNN )
{OO 
	PatientIdPP 
=PP 
appointmentPP '
.PP' (
	PatientIdPP( 1
,PP1 2
DoctorIdQQ 
=QQ 
appointmentQQ &
.QQ& '
DoctorIdQQ' /
,QQ/ 0
AppointmentIdRR 
=RR 
appointmentRR  +
.RR+ ,
AppointmentIdRR, 9
,RR9 :
	VisitDateSS 
=SS 
appointmentSS '
.SS' (
ScheduledDateSS( 5
}TT 
;TT 
awaitVV 

repositoryVV 
.VV 
CreateAsyncVV (
(VV( )
recordVV) /
)VV/ 0
;VV0 1
}WW 	
publicYY 
asyncYY 
TaskYY 
<YY 
HealthRecordDtoYY )
?YY) *
>YY* +#
GetByAppointmentIdAsyncYY, C
(YYC D
intYYD G
appointmentIdYYH U
)YYU V
{ZZ 	
var[[ 
record[[ 
=[[ 
await[[ 

repository[[ )
.[[) *#
GetByAppointmentIdAsync[[* A
([[A B
appointmentId[[B O
)[[O P
;[[P Q
return]] 
mapper]] 
.]] 
Map]] 
<]] 
HealthRecordDto]] -
?]]- .
>]]. /
(]]/ 0
record]]0 6
)]]6 7
;]]7 8
}^^ 	
public`` 
async`` 
Task`` 
<`` 
List`` 
<``  
DoctorPatientListDto`` 3
>``3 4
>``4 5"
GetDoctorPatientsAsync``6 L
(``L M
int``M P
doctorId``Q Y
)``Y Z
{aa 	
returnbb 
awaitbb 

repositorybb #
.bb# $"
GetDoctorPatientsAsyncbb$ :
(bb: ;
doctorIdbb; C
)bbC D
;bbD E
}cc 	
}dd 
}ee ¶
^C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\GarnetHostedService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
.$ %
Impl% )
{ 
public 

class 
GarnetHostedService $
($ %
ILogger% ,
<, -
GarnetHostedService- @
>@ A
loggerB H
)H I
:J K
IHostedServiceL Z
,Z [
IDisposable\ g
{ 
private 
GarnetServer 
? 
_server %
;% &
private		 
readonly		 
ILogger		  
<		  !
GarnetHostedService		! 4
>		4 5
_logger		6 =
=		> ?
logger		@ F
;		F G
public 
Task 

StartAsync 
( 
CancellationToken 0
cancellationToken1 B
)B C
{ 	
try 
{ 
_server 
= 
new 
GarnetServer *
(* +
[+ ,
$str, 9
]9 :
): ;
;; <
_server 
. 
Start 
( 
) 
;  
_logger 
. 
LogInformation &
(& '
$str' T
)T U
;U V
} 
catch 
( 
	Exception 
ex 
)  
{ 
_logger 
. 
LogError  
(  !
ex! #
,# $
$str% M
)M N
;N O
} 
return 
Task 
. 
CompletedTask %
;% &
} 	
public 
Task 
	StopAsync 
( 
CancellationToken /
cancellationToken0 A
)A B
{   	
_server!! 
?!! 
.!! 
Dispose!! 
(!! 
)!! 
;!! 
_logger## 
.## 
LogInformation## "
(##" #
$str### C
)##C D
;##D E
return%% 
Task%% 
.%% 
CompletedTask%% %
;%%% &
}'' 	
public(( 
void(( 
Dispose(( 
((( 
)(( 
=>((  
_server((! (
?((( )
.(() *
Dispose((* 1
(((1 2
)((2 3
;((3 4
})) 
}** ≥n
XC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\DoctorService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
.$ %
Impl% )
{ 
public 

class 
DoctorService 
( 
IDoctorRepository 0

repository1 ;
,; <
IMapper= D
mapperE K
,K L"
IConnectionMultiplexerM c
redisd i
)i j
:k l
IDoctorServicem {
{ 
public 
async 
Task 
< 
	DoctorDto #
># $
CreateAsync% 0
(0 1
CreateDoctorDto1 @
entityA G
)G H
{ 	
var 
doctor 
= 
mapper 
.  
Map  #
<# $
Doctor$ *
>* +
(+ ,
entity, 2
)2 3
;3 4
var 
savedEntity 
= 
await #

repository$ .
.. /
CreateAsync/ :
(: ;
doctor; A
)A B
;B C
await (
InvalidateActiveDoctorsCache .
(. /
)/ 0
;0 1
return 
mapper 
. 
Map 
< 
	DoctorDto '
>' (
(( )
savedEntity) 4
)4 5
;5 6
} 	
public 
async 
Task 
< 
	DoctorDto #
># $!
DeactivateDoctorAsync% :
(: ;
int; >
id? A
)A B
{ 	
var 
existing 
= 
await  

repository! +
.+ ,
GetByIdAsync, 8
(8 9
id9 ;
); <
;< =
if 
( 
existing 
== 
null  
)  !
throw 
new 
	Exception #
(# $
$str$ 6
)6 7
;7 8
existing 
. 
IsActive 
= 
false  %
;% &
var   
updated   
=   
await   

repository    *
.  * +
UpdateAsync  + 6
(  6 7
id  7 9
,  9 :
existing  ; C
)  C D
;  D E
await!! (
InvalidateActiveDoctorsCache!! .
(!!. /
)!!/ 0
;!!0 1
return## 
mapper## 
.## 
Map## 
<## 
	DoctorDto## '
>##' (
(##( )
updated##) 0
)##0 1
;##1 2
}%% 	
public'' 
async'' 
Task'' 
<'' 
List'' 
<'' 
	DoctorDto'' (
>''( )
>'') *
GetAllAsync''+ 6
(''6 7
)''7 8
{(( 	
return)) 
mapper)) 
.)) 
Map)) 
<)) 
List)) "
<))" #
	DoctorDto))# ,
>)), -
>))- .
()). /
await))/ 4

repository))5 ?
.))? @
GetAllAsync))@ K
())K L
)))L M
)))M N
;))N O
}** 	
public,, 
async,, 
Task,, 
<,, 
List,, 
<,, 
	DoctorDto,, (
>,,( )
>,,) *!
GetActiveDoctorsAsync,,+ @
(,,@ A
),,A B
{-- 	
Log.. 
... 
Information.. 
(.. 
$str.. 0
)..0 1
;..1 2
var00 
db00 
=00 
redis00 
.00 
GetDatabase00 &
(00& '
)00' (
;00( )
const22 
string22 
cacheKey22 !
=22" #
$str22$ 4
;224 5
var44 
cachedDoctors44 
=44 
await55 
db55 
.55 
StringGetAsync55 '
(55' (
cacheKey55( 0
)550 1
;551 2
if77 
(77 
!77 
cachedDoctors77 
.77 
IsNullOrEmpty77 ,
)77, -
{88 
Log99 
.99 
Information99 
(99  
$str99  .
)99. /
;99/ 0
return;; 
JsonSerializer;; %
.;;% &
Deserialize;;& 1
<;;1 2
List;;2 6
<;;6 7
	DoctorDto;;7 @
>;;@ A
>;;A B
(;;B C
cachedDoctors<< !
.<<! "
ToString<<" *
(<<* +
)<<+ ,
)<<, -
!<<- .
;<<. /
}== 
Log?? 
.?? 
Information?? 
(?? 
$str?? +
)??+ ,
;??, -
varAA 
doctorsAA 
=AA 
mapperAA  
.AA  !
MapAA! $
<AA$ %
ListAA% )
<AA) *
	DoctorDtoAA* 3
>AA3 4
>AA4 5
(AA5 6
awaitBB 

repositoryBB  
.BB  !!
GetActiveDoctorsAsyncBB! 6
(BB6 7
)BB7 8
)BB8 9
;BB9 :
awaitDD 
dbDD 
.DD 
StringSetAsyncDD #
(DD# $
cacheKeyEE 
,EE 
JsonSerializerFF 
.FF 
	SerializeFF (
(FF( )
doctorsFF) 0
)FF0 1
,FF1 2
TimeSpanGG 
.GG 
FromMinutesGG $
(GG$ %
$numGG% &
)GG& '
)GG' (
;GG( )
returnII 
doctorsII 
;II 
}JJ 	
publicLL 
asyncLL 
TaskLL 
<LL 
	DoctorDtoLL #
?LL# $
>LL$ %
GetByIdAsyncLL& 2
(LL2 3
intLL3 6
idLL7 9
)LL9 :
{MM 	
varNN 
doctorNN 
=NN 
awaitNN 

repositoryNN )
.NN) *
GetByIdAsyncNN* 6
(NN6 7
idNN7 9
)NN9 :
;NN: ;
returnOO 
mapperOO 
.OO 
MapOO 
<OO 
	DoctorDtoOO '
?OO' (
>OO( )
(OO) *
doctorOO* 0
)OO0 1
;OO1 2
}PP 	
publicRR 
asyncRR 
TaskRR 
<RR 
ListRR 
<RR 
	DoctorDtoRR (
>RR( )
>RR) *'
SearchBySpecialisationAsyncRR+ F
(RRF G
stringRRG M
specialisationRRN \
)RR\ ]
{SS 	
varTT 
doctorsTT 
=TT 
awaitTT 

repositoryTT  *
.TT* +'
SearchBySpecialisationAsyncTT+ F
(TTF G
specialisationTTG U
)TTU V
;TTV W
returnUU 
mapperUU 
.UU 
MapUU 
<UU 
ListUU "
<UU" #
	DoctorDtoUU# ,
>UU, -
>UU- .
(UU. /
doctorsUU/ 6
)UU6 7
;UU7 8
}VV 	
publicXX 
asyncXX 
TaskXX 
<XX 
ListXX 
<XX 
	DoctorDtoXX (
>XX( )
>XX) *
SearchByNameAsyncXX+ <
(XX< =
stringXX= C
nameXXD H
)XXH I
{YY 	
varZZ 
doctorsZZ 
=ZZ 
awaitZZ 

repositoryZZ  *
.ZZ* +
SearchByNameAsyncZZ+ <
(ZZ< =
nameZZ= A
)ZZA B
;ZZB C
return[[ 
mapper[[ 
.[[ 
Map[[ 
<[[ 
List[[ "
<[[" #
	DoctorDto[[# ,
>[[, -
>[[- .
([[. /
doctors[[/ 6
)[[6 7
;[[7 8
}\\ 	
public^^ 
async^^ 
Task^^ 
<^^ 
	DoctorDto^^ #
?^^# $
>^^$ %
UpdateAsync^^& 1
(^^1 2
int^^2 5
id^^6 8
,^^8 9
UpdateDoctorDto^^: I
entity^^J P
)^^P Q
{__ 	
varaa 
existingaa 
=aa 
awaitaa  

repositoryaa! +
.aa+ ,
GetByIdAsyncaa, 8
(aa8 9
idaa9 ;
)aa; <
;aa< =
ifbb 
(bb 
existingbb 
==bb 
nullbb  
)bb  !
throwcc 
newcc 
	Exceptioncc #
(cc# $
$strcc$ 6
)cc6 7
;cc7 8
existingee 
.ee 

DoctorNameee 
=ee  !
entityee" (
.ee( )

DoctorNameee) 3
;ee3 4
existingff 
.ff 
Specialisationff #
=ff$ %
entityff& ,
.ff, -
Specialisationff- ;
;ff; <
existinggg 
.gg 
Emailgg 
=gg 
entitygg #
.gg# $
Emailgg$ )
;gg) *
existinghh 
.hh 
YearsOfExperiencehh &
=hh' (
entityhh) /
.hh/ 0
YearsOfExperiencehh0 A
;hhA B
existingii 
.ii 
ConsultationFeeii $
=ii% &
entityii' -
.ii- .
ConsultationFeeii. =
;ii= >
existingjj 
.jj 
IsActivejj 
=jj 
entityjj  &
.jj& '
IsActivejj' /
;jj/ 0
varll 
updatedll 
=ll 
awaitll 

repositoryll  *
.ll* +
UpdateAsyncll+ 6
(ll6 7
idll7 9
,ll9 :
existingll; C
)llC D
;llD E
awaitmm (
InvalidateActiveDoctorsCachemm .
(mm. /
)mm/ 0
;mm0 1
returnoo 
mapperoo 
.oo 
Mapoo 
<oo 
	DoctorDtooo '
?oo' (
>oo( )
(oo) *
updatedoo* 1
)oo1 2
;oo2 3
}qq 	
publicss 
asyncss 
Taskss 
<ss 
boolss 
>ss 
ToggleActiveAsyncss  1
(ss1 2
intss2 5
idss6 8
)ss8 9
{tt 	
varuu 
doctoruu 
=uu 
awaituu 

repositoryuu )
.uu) *
GetByIdAsyncuu* 6
(uu6 7
iduu7 9
)uu9 :
;uu: ;
ifww 
(ww 
doctorww 
==ww 
nullww 
)ww 
returnxx 
falsexx 
;xx 
doctorzz 
.zz 
IsActivezz 
=zz 
!zz 
doctorzz %
.zz% &
IsActivezz& .
;zz. /
await|| 

repository|| 
.|| 
UpdateAsync|| (
(||( )
id||) +
,||+ ,
doctor||- 3
)||3 4
;||4 5
await}} (
InvalidateActiveDoctorsCache}} .
(}}. /
)}}/ 0
;}}0 1
return 
true 
; 
}
ÄÄ 	
public
ÅÅ 
async
ÅÅ 
Task
ÅÅ 
<
ÅÅ 
List
ÅÅ 
<
ÅÅ 
	DoctorDto
ÅÅ (
>
ÅÅ( )
>
ÅÅ) *
SearchAsync
ÅÅ+ 6
(
ÅÅ6 7
string
ÅÅ7 =
query
ÅÅ> C
)
ÅÅC D
{
ÇÇ 	
var
ÉÉ 
doctors
ÉÉ 
=
ÉÉ 
await
ÉÉ 

repository
ÉÉ  *
.
ÉÉ* +
SearchAsync
ÉÉ+ 6
(
ÉÉ6 7
query
ÉÉ7 <
)
ÉÉ< =
;
ÉÉ= >
return
ÑÑ 
mapper
ÑÑ 
.
ÑÑ 
Map
ÑÑ 
<
ÑÑ 
List
ÑÑ "
<
ÑÑ" #
	DoctorDto
ÑÑ# ,
>
ÑÑ, -
>
ÑÑ- .
(
ÑÑ. /
doctors
ÑÑ/ 6
)
ÑÑ6 7
;
ÑÑ7 8
}
ÖÖ 	
public
áá 
async
áá 
Task
áá 
<
áá 
Doctor
áá  
?
áá  !
>
áá! "
GetByUserIdAsync
áá# 3
(
áá3 4
string
áá4 :
userId
áá; A
)
ááA B
{
àà 	
return
ââ 
await
ââ 

repository
ââ #
.
ââ# $
GetByUserIdAsync
ââ$ 4
(
ââ4 5
userId
ââ5 ;
)
ââ; <
;
ââ< =
}
ää 	
public
ãã 
async
ãã 
Task
ãã 
<
ãã 
List
ãã 
<
ãã 
	DoctorDto
ãã (
>
ãã( )
>
ãã) *
FilterAsync
ãã+ 6
(
ãã6 7
string
ãã7 =
?
ãã= >
name
ãã? C
,
ããC D
string
ããE K
?
ããK L
specialization
ããM [
)
ãã[ \
{
åå 	
var
çç 
doctors
çç 
=
çç 
await
çç 

repository
çç  *
.
çç* +
FilterAsync
çç+ 6
(
çç6 7
name
çç7 ;
,
çç; <
specialization
çç= K
)
ççK L
;
ççL M
return
éé 
mapper
éé 
.
éé 
Map
éé 
<
éé 
List
éé "
<
éé" #
	DoctorDto
éé# ,
>
éé, -
>
éé- .
(
éé. /
doctors
éé/ 6
)
éé6 7
;
éé7 8
}
èè 	
private
ëë 
async
ëë 
Task
ëë *
InvalidateActiveDoctorsCache
ëë 7
(
ëë7 8
)
ëë8 9
{
íí 	
var
ìì 
db
ìì 
=
ìì 
redis
ìì 
.
ìì 
GetDatabase
ìì &
(
ìì& '
)
ìì' (
;
ìì( )
await
ïï 
db
ïï 
.
ïï 
KeyDeleteAsync
ïï #
(
ïï# $
$str
ïï$ 4
)
ïï4 5
;
ïï5 6
Log
óó 
.
óó 
Information
óó 
(
óó 
$str
óó >
)
óó> ?
;
óó? @
}
òò 	
}
õõ 
}úú ≤ù
VC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\AuthService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
.$ %
Impl% )
{ 
public 

class 
AuthService 
( 
UserManager (
<( )
ApplicationUser) 8
>8 9
userManager: E
,E F
IConfigurationG U
configurationV c
,c d
AppDbContexte q
	dbContextr {
){ |
:} ~
IAuthService	 ã
{ 
public 
async 
Task 
< 
AuthResponse &
>& '

LoginAsync( 2
(2 3
LoginDto3 ;
request< C
)C D
{ 	
var 
user 
= 
await 
userManager (
.( )
FindByEmailAsync) 9
(9 :
request: A
.A B
EmailB G
)G H
;H I
if 
( 
user 
is 
null 
) 
{ 
return 
new 
AuthResponse '
{ 
AccessToken 
=  !
$str" $
,$ %
RefreshToken  
=! "
$str# %
,% &
	ExpiresIn 
= 
$num  !
,! "
Message 
= 
$str 3
,3 4
IsFirstLogin  
=! "
false# (
} 
; 
} 
var 
isPasswordValid 
=  !
await" '
userManager( 3
.3 4
CheckPasswordAsync4 F
(F G
userG K
,K L
requestM T
.T U
PasswordU ]
)] ^
;^ _
if!! 
(!! 
!!! 
isPasswordValid!!  
)!!  !
{"" 
return## 
new## 
AuthResponse## '
{$$ 
AccessToken%% 
=%%  !
$str%%" $
,%%$ %
RefreshToken&&  
=&&! "
$str&&# %
,&&% &
	ExpiresIn'' 
='' 
$num''  !
,''! "
Message(( 
=(( 
$str(( 0
,((0 1
IsFirstLogin))  
=))! "
false))# (
}** 
;** 
}++ 
var-- 
token-- 
=-- 
await-- 
GenerateJwtToken-- .
(--. /
user--/ 3
)--3 4
;--4 5
var// 
refreshToken// 
=// 
Convert// &
.//& '
ToBase64String//' 5
(//5 6
Guid//6 :
.//: ;
NewGuid//; B
(//B C
)//C D
.//D E
ToByteArray//E P
(//P Q
)//Q R
)//R S
;//S T
var11 
refreshTokenEntity11 "
=11# $
new11% (
RefreshToken11) 5
{22 
Token33 
=33 
refreshToken33 $
,33$ %
Expires44 
=44 
DateTime44 "
.44" #
UtcNow44# )
.44) *
AddDays44* 1
(441 2
$num442 3
)443 4
,444 5
	IsRevoked55 
=55 
false55 !
,55! "
UserId66 
=66 
user66 
.66 
Id66  
}77 
;77 
await99 
	dbContext99 
.99 
RefreshTokens99 )
.99) *
AddAsync99* 2
(992 3
refreshTokenEntity993 E
)99E F
;99F G
await:: 
	dbContext:: 
.:: 
SaveChangesAsync:: ,
(::, -
)::- .
;::. /
var<< 
expiry<< 
=<< 
int<< 
.<< 
Parse<< "
(<<" #
configuration<<# 0
[<<0 1
$str<<1 S
]<<S T
!<<T U
)<<U V
;<<V W
return>> 
new>> 
AuthResponse>> #
{?? 
AccessToken@@ 
=@@ 
token@@ #
,@@# $
RefreshTokenAA 
=AA 
refreshTokenAA +
,AA+ ,
	ExpiresInBB 
=BB 
expiryBB "
,BB" #
MessageCC 
=CC 
$strCC ,
,CC, -
IsFirstLoginDD 
=DD 
userDD #
.DD# $
IsFirstLoginDD$ 0
}EE 
;EE 
}FF 	
publicKK 
asyncKK 
TaskKK 
<KK  
AuthRegisterResponseKK .
>KK. /
RegisterAsyncKK0 =
(KK= >
RegisterDtoKK> I
requestKKJ Q
)KKQ R
{LL 	
ifNN 
(NN 
requestNN 
.NN 
PasswordNN  
!=NN! #
requestNN$ +
.NN+ ,
ConfirmPasswordNN, ;
)NN; <
{OO 
returnPP 
newPP  
AuthRegisterResponsePP /
{QQ 
SuccessRR 
=RR 
falseRR #
,RR# $
MessageSS 
=SS 
$strSS 6
,SS6 7
UserIdTT 
=TT 
$strTT 
}UU 
;UU 
}VV 
ifYY 
(YY 
requestYY 
.YY 
RoleYY 
!=YY 
$strYY  '
&&YY( *
requestYY+ 2
.YY2 3
RoleYY3 7
!=YY8 :
$strYY; D
&&YYE G
requestYYH O
.YYO P
RoleYYP T
!=YYU W
$strYYX `
)YY` a
{ZZ 
return[[ 
new[[  
AuthRegisterResponse[[ /
{\\ 
Success]] 
=]] 
false]] #
,]]# $
Message^^ 
=^^ 
$str^^ \
,^^\ ]
UserId__ 
=__ 
$str__ 
}`` 
;`` 
}aa 
varcc 
usercc 
=cc 
newcc 
ApplicationUsercc *
{dd 
UserNameee 
=ee 
requestee "
.ee" #
Emailee# (
,ee( )
Emailff 
=ff 
requestff 
.ff  
Emailff  %
}gg 
;gg 
varii 
resultii 
=ii 
awaitii 
userManagerii *
.ii* +
CreateAsyncii+ 6
(ii6 7
userii7 ;
,ii; <
requestii= D
.iiD E
PasswordiiE M
)iiM N
;iiN O
ifkk 
(kk 
!kk 
resultkk 
.kk 
	Succeededkk !
)kk! "
{ll 
varmm 
errorsmm 
=mm 
stringmm #
.mm# $
Joinmm$ (
(mm( )
$strmm) ,
,mm, -
resultmm. 4
.mm4 5
Errorsmm5 ;
.mm; <
Selectmm< B
(mmB C
emmC D
=>mmE G
emmH I
.mmI J
DescriptionmmJ U
)mmU V
)mmV W
;mmW X
returnoo 
newoo  
AuthRegisterResponseoo /
{pp 
Successqq 
=qq 
falseqq #
,qq# $
Messagerr 
=rr 
errorsrr $
,rr$ %
UserIdss 
=ss 
$strss 
}tt 
;tt 
}uu 
awaitww 
userManagerww 
.ww 
AddToRoleAsyncww ,
(ww, -
userww- 1
,ww1 2
requestww3 :
.ww: ;
Roleww; ?
)ww? @
;ww@ A
ifzz 
(zz 
requestzz 
.zz 
Rolezz 
==zz 
$strzz  )
)zz) *
user{{ 
.{{ 
IsFirstLogin{{ !
={{" #
false{{$ )
;{{) *
else|| 
user}} 
.}} 
IsFirstLogin}} !
=}}" #
true}}$ (
;}}( )
await 
userManager 
. 
UpdateAsync )
() *
user* .
). /
;/ 0
if
ÇÇ 
(
ÇÇ 
request
ÇÇ 
.
ÇÇ 
Role
ÇÇ 
==
ÇÇ 
$str
ÇÇ  )
)
ÇÇ) *
{
ÉÉ 
if
ÑÑ 
(
ÑÑ 
string
ÑÑ 
.
ÑÑ 
IsNullOrEmpty
ÑÑ (
(
ÑÑ( )
request
ÑÑ) 0
.
ÑÑ0 1
Name
ÑÑ1 5
)
ÑÑ5 6
||
ÑÑ7 9
request
ÖÖ 
.
ÖÖ 
DateOfBirth
ÖÖ '
==
ÖÖ( *
null
ÖÖ+ /
||
ÖÖ0 2
string
ÜÜ 
.
ÜÜ 
IsNullOrEmpty
ÜÜ (
(
ÜÜ( )
request
ÜÜ) 0
.
ÜÜ0 1
Gender
ÜÜ1 7
)
ÜÜ7 8
||
ÜÜ9 ;
string
áá 
.
áá 
IsNullOrEmpty
áá (
(
áá( )
request
áá) 0
.
áá0 1
PhoneNo
áá1 8
)
áá8 9
)
áá9 :
{
àà 
return
ââ 
new
ââ "
AuthRegisterResponse
ââ 3
{
ää 
Success
ãã 
=
ãã  !
false
ãã" '
,
ãã' (
Message
åå 
=
åå  !
$str
åå" ;
,
åå; <
UserId
çç 
=
çç  
$str
çç! #
}
éé 
;
éé 
}
èè 
var
ëë 
patient
ëë 
=
ëë 
new
ëë !
Patient
ëë" )
{
íí 
PatientName
ìì 
=
ìì  !
request
ìì" )
.
ìì) *
Name
ìì* .
,
ìì. /
Email
îî 
=
îî 
request
îî #
.
îî# $
Email
îî$ )
,
îî) *
PhoneNo
ïï 
=
ïï 
request
ïï %
.
ïï% &
PhoneNo
ïï& -
,
ïï- .
DateOfBirth
ññ 
=
ññ  !
request
ññ" )
.
ññ) *
DateOfBirth
ññ* 5
.
ññ5 6
Value
ññ6 ;
,
ññ; <
Gender
óó 
=
óó 
request
óó $
.
óó$ %
Gender
óó% +
,
óó+ ,
InsuranceID
òò 
=
òò  !
request
òò" )
.
òò) *
InsuranceID
òò* 5
,
òò5 6
IsActive
ôô 
=
ôô 
true
ôô #
,
ôô# $
UserId
öö 
=
öö 
user
öö !
.
öö! "
Id
öö" $
}
õõ 
;
õõ 
await
ùù 
	dbContext
ùù 
.
ùù  
Patients
ùù  (
.
ùù( )
AddAsync
ùù) 1
(
ùù1 2
patient
ùù2 9
)
ùù9 :
;
ùù: ;
await
ûû 
	dbContext
ûû 
.
ûû  
SaveChangesAsync
ûû  0
(
ûû0 1
)
ûû1 2
;
ûû2 3
}
üü 
return
§§ 
new
§§ "
AuthRegisterResponse
§§ +
{
•• 
Success
¶¶ 
=
¶¶ 
true
¶¶ 
,
¶¶ 
Message
ßß 
=
ßß 
$str
ßß 8
,
ßß8 9
UserId
®® 
=
®® 
user
®® 
.
®® 
Id
®®  
}
©© 
;
©© 
}
™™ 	
private
≠≠ 
async
≠≠ 
Task
≠≠ 
<
≠≠ 
string
≠≠ !
>
≠≠! "
GenerateJwtToken
≠≠# 3
(
≠≠3 4
ApplicationUser
≠≠4 C
user
≠≠D H
)
≠≠H I
{
ÆÆ 	
var
ØØ 

jwtSetting
ØØ 
=
ØØ 
configuration
ØØ *
.
ØØ* +

GetSection
ØØ+ 5
(
ØØ5 6
$str
ØØ6 ;
)
ØØ; <
;
ØØ< =
var
±± 
key
±± 
=
±± 
new
±± "
SymmetricSecurityKey
±± .
(
±±. /
Encoding
±±/ 7
.
±±7 8
UTF8
±±8 <
.
±±< =
GetBytes
±±= E
(
±±E F

jwtSetting
±±F P
[
±±P Q
$str
±±Q V
]
±±V W
!
±±W X
)
±±X Y
)
±±Y Z
;
±±Z [
var
≤≤ 
credentials
≤≤ 
=
≤≤ 
new
≤≤ ! 
SigningCredentials
≤≤" 4
(
≤≤4 5
key
≤≤5 8
,
≤≤8 9 
SecurityAlgorithms
≤≤: L
.
≤≤L M

HmacSha256
≤≤M W
)
≤≤W X
;
≤≤X Y
var
¥¥ 
roles
¥¥ 
=
¥¥ 
await
¥¥ 
userManager
¥¥ )
.
¥¥) *
GetRolesAsync
¥¥* 7
(
¥¥7 8
user
¥¥8 <
)
¥¥< =
;
¥¥= >
var
∂∂ 
claim
∂∂ 
=
∂∂ 
new
∂∂ 
List
∂∂  
<
∂∂  !
Claim
∂∂! &
>
∂∂& '
{
∑∑ 
new
∏∏ 
Claim
∏∏ 
(
∏∏ %
JwtRegisteredClaimNames
∏∏ )
.
∏∏) *
Sub
∏∏* -
,
∏∏- .
user
∏∏/ 3
.
∏∏3 4
Id
∏∏4 6
)
∏∏6 7
,
∏∏7 8
new
ππ 
Claim
ππ 
(
ππ %
JwtRegisteredClaimNames
ππ )
.
ππ) *
Email
ππ* /
,
ππ/ 0
user
ππ1 5
.
ππ5 6
Email
ππ6 ;
!
ππ; <
)
ππ< =
,
ππ= >
new
∫∫ 
Claim
∫∫ 
(
∫∫ %
JwtRegisteredClaimNames
∫∫ )
.
∫∫) *
Jti
∫∫* -
,
∫∫- .
Guid
∫∫/ 3
.
∫∫3 4
NewGuid
∫∫4 ;
(
∫∫; <
)
∫∫< =
.
∫∫= >
ToString
∫∫> F
(
∫∫F G
)
∫∫G H
)
∫∫H I
,
∫∫I J
new
ªª 
Claim
ªª 
(
ªª 

ClaimTypes
ªª 
.
ªª 
NameIdentifier
ªª +
,
ªª+ ,
user
ªª- 1
.
ªª1 2
Id
ªª2 4
)
ªª4 5
}
ºº 
;
ºº 
foreach
ææ 
(
ææ 
var
ææ 
role
ææ 
in
ææ  
roles
ææ! &
)
ææ& '
{
øø 
claim
¿¿ 
.
¿¿ 
Add
¿¿ 
(
¿¿ 
new
¿¿ 
Claim
¿¿ #
(
¿¿# $

ClaimTypes
¿¿$ .
.
¿¿. /
Role
¿¿/ 3
,
¿¿3 4
role
¿¿5 9
)
¿¿9 :
)
¿¿: ;
;
¿¿; <
}
¡¡ 
var
ƒƒ 
patient
ƒƒ 
=
ƒƒ 
await
ƒƒ 
	dbContext
ƒƒ  )
.
ƒƒ) *
Patients
ƒƒ* 2
.
≈≈ !
FirstOrDefaultAsync
≈≈ $
(
≈≈$ %
p
≈≈% &
=>
≈≈' )
p
≈≈* +
.
≈≈+ ,
UserId
≈≈, 2
==
≈≈3 5
user
≈≈6 :
.
≈≈: ;
Id
≈≈; =
)
≈≈= >
;
≈≈> ?
if
«« 
(
«« 
patient
«« 
!=
«« 
null
«« 
)
««  
{
»» 
claim
…… 
.
…… 
Add
…… 
(
…… 
new
…… 
Claim
…… #
(
……# $
$str
……$ /
,
……/ 0
patient
……1 8
.
……8 9
	PatientId
……9 B
.
……B C
ToString
……C K
(
……K L
)
……L M
)
……M N
)
……N O
;
……O P
}
   
var
ÕÕ 
doctor
ÕÕ 
=
ÕÕ 
await
ÕÕ 
	dbContext
ÕÕ (
.
ÕÕ( )
Doctors
ÕÕ) 0
.
ŒŒ !
FirstOrDefaultAsync
ŒŒ (
(
ŒŒ( )
d
ŒŒ) *
=>
ŒŒ+ -
d
ŒŒ. /
.
ŒŒ/ 0
UserId
ŒŒ0 6
==
ŒŒ7 9
user
ŒŒ: >
.
ŒŒ> ?
Id
ŒŒ? A
)
ŒŒA B
;
ŒŒB C
if
–– 
(
–– 
doctor
–– 
!=
–– 
null
–– 
)
–– 
{
—— 
claim
““ 
.
““ 
Add
““ 
(
““ 
new
““ 
Claim
““ #
(
““# $
$str
““$ .
,
““. /
doctor
““0 6
.
““6 7
DoctorId
““7 ?
.
““? @
ToString
““@ H
(
““H I
)
““I J
)
““J K
)
““K L
;
““L M
}
”” 
var
’’ 
expirationMinutes
’’ !
=
’’" #
int
’’$ '
.
’’' (
Parse
’’( -
(
’’- .

jwtSetting
’’. 8
[
’’8 9
$str
’’9 W
]
’’W X
)
’’X Y
;
’’Y Z
var
◊◊ 
token
◊◊ 
=
◊◊ 
new
◊◊ 
JwtSecurityToken
◊◊ ,
(
◊◊, -
issuer
ÿÿ 
:
ÿÿ 

jwtSetting
ÿÿ "
[
ÿÿ" #
$str
ÿÿ# +
]
ÿÿ+ ,
,
ÿÿ, -
audience
ŸŸ 
:
ŸŸ 

jwtSetting
ŸŸ $
[
ŸŸ$ %
$str
ŸŸ% /
]
ŸŸ/ 0
,
ŸŸ0 1
claims
⁄⁄ 
:
⁄⁄ 
claim
⁄⁄ 
,
⁄⁄ 
expires
€€ 
:
€€ 
DateTime
€€ !
.
€€! "
UtcNow
€€" (
.
€€( )

AddMinutes
€€) 3
(
€€3 4
expirationMinutes
€€4 E
)
€€E F
,
€€F G 
signingCredentials
‹‹ "
:
‹‹" #
credentials
‹‹$ /
)
›› 
;
›› 
return
ﬂﬂ 
new
ﬂﬂ %
JwtSecurityTokenHandler
ﬂﬂ .
(
ﬂﬂ. /
)
ﬂﬂ/ 0
.
ﬂﬂ0 1

WriteToken
ﬂﬂ1 ;
(
ﬂﬂ; <
token
ﬂﬂ< A
)
ﬂﬂA B
;
ﬂﬂB C
}
‡‡ 	
public
‰‰ 
async
‰‰ 
Task
‰‰ 
<
‰‰ 
AuthResponse
‰‰ &
?
‰‰& '
>
‰‰' (
RefreshAsync
‰‰) 5
(
‰‰5 6
string
‰‰6 <
refreshToken
‰‰= I
)
‰‰I J
{
ÂÂ 	
var
ÁÁ 
storedToken
ÁÁ 
=
ÁÁ 
await
ÁÁ #
	dbContext
ÁÁ$ -
.
ÁÁ- .
RefreshTokens
ÁÁ. ;
.
ËË !
FirstOrDefaultAsync
ËË $
(
ËË$ %
rt
ËË% '
=>
ËË( *
rt
ËË+ -
.
ËË- .
Token
ËË. 3
==
ËË4 6
refreshToken
ËË7 C
)
ËËC D
;
ËËD E
if
ÎÎ 
(
ÎÎ 
storedToken
ÎÎ 
==
ÎÎ 
null
ÎÎ #
||
ÎÎ$ &
storedToken
ÎÎ' 2
.
ÎÎ2 3
	IsRevoked
ÎÎ3 <
||
ÎÎ= ?
storedToken
ÎÎ@ K
.
ÎÎK L
Expires
ÎÎL S
<
ÎÎT U
DateTime
ÎÎV ^
.
ÎÎ^ _
UtcNow
ÎÎ_ e
)
ÎÎe f
{
ÏÏ 
return
ÌÌ 
null
ÌÌ 
;
ÌÌ 
}
ÓÓ 
var
ÒÒ 
user
ÒÒ 
=
ÒÒ 
await
ÒÒ 
userManager
ÒÒ (
.
ÒÒ( )
FindByIdAsync
ÒÒ) 6
(
ÒÒ6 7
storedToken
ÒÒ7 B
.
ÒÒB C
UserId
ÒÒC I
)
ÒÒI J
;
ÒÒJ K
if
ÚÚ 
(
ÚÚ 
user
ÚÚ 
==
ÚÚ 
null
ÚÚ 
)
ÚÚ 
return
ÛÛ 
null
ÛÛ 
;
ÛÛ 
var
ˆˆ 
newAccessToken
ˆˆ 
=
ˆˆ  
await
ˆˆ! &
GenerateJwtToken
ˆˆ' 7
(
ˆˆ7 8
user
ˆˆ8 <
)
ˆˆ< =
;
ˆˆ= >
var
¯¯ 
expiry
¯¯ 
=
¯¯ 
int
¯¯ 
.
¯¯ 
Parse
¯¯ "
(
¯¯" #
configuration
¯¯# 0
[
¯¯0 1
$str
¯¯1 S
]
¯¯S T
!
¯¯T U
)
¯¯U V
;
¯¯V W
return
˙˙ 
new
˙˙ 
AuthResponse
˙˙ #
{
˚˚ 
AccessToken
¸¸ 
=
¸¸ 
newAccessToken
¸¸ ,
,
¸¸, -
RefreshToken
˝˝ 
=
˝˝ 
refreshToken
˝˝ +
,
˝˝+ ,
	ExpiresIn
˛˛ 
=
˛˛ 
expiry
˛˛ "
,
˛˛" #
Message
ˇˇ 
=
ˇˇ 
$str
ˇˇ 8
}
ÄÄ 
;
ÄÄ 
}
ÅÅ 	
}
ÉÉ 
}ÑÑ ≤î
]C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\Impl\AppointmentService.cs
	namespace		 	
HealthAxisApplicn		
 
.		 
Services		 $
.		$ %
Impl		% )
{

 
public 

class 
AppointmentService #
(# $"
IAppointmentRepository 

repository )
,) * 
IHealthRecordService 
healthRecordService 0
,0 1
IMapper 
mapper 
, 
IPublishEndpoint 
publishEndpoint (
)( )
: 	
IAppointmentService
 
{ 
private 
const 
string 
Pending $
=% &
$str' 0
;0 1
private 
const 
string 
	Confirmed &
=' (
$str) 4
;4 5
private 
const 
string 
	Cancelled &
=' (
$str) 4
;4 5
private 
const 
string 
	Completed &
=' (
$str) 4
;4 5
private 
const 
string 
PatientRole (
=) *
$str+ 4
;4 5
private 
const 
string 

DoctorRole '
=( )
$str* 2
;2 3
public 
async 
Task 
< 
AppointmentDto (
>( )
CreateAsync* 5
(5 6 
CreateAppointmentDto6 J
entityK Q
,Q R
intS V
	patientIdW `
)` a
{ 	
if 
( 
entity 
. 
ScheduledDate $
.$ %
Date% )
<* +
DateTime, 4
.4 5
UtcNow5 ;
.; <
Date< @
)@ A
throw 
new %
InvalidOperationException 3
(3 4
$str4 Y
)Y Z
;Z [
if!! 
(!! 
!!! 
TimeSpan!! 
.!! 
TryParse!! "
(!!" #
entity"" 
."" 
TimeSlot"" #
,""# $
CultureInfo## 
.##  
InvariantCulture##  0
,##0 1
out$$ 
_$$ 
)$$ 
)$$ 
{%% 
throw&& 
new&& 
ArgumentException&& +
(&&+ ,
$str&&, L
)&&L M
;&&M N
}'' 
var++ 
doctorConflict++ 
=++  
await++! &

repository++' 1
.++1 2"
DoctorHasConflictAsync++2 H
(++H I
entity,, 
.,, 
DoctorId,, 
,,,  
entity-- 
.-- 
ScheduledDate-- $
,--$ %
entity.. 
... 
TimeSlot.. 
)// 
;// 
if11 
(11 
doctorConflict11 
)11 
throw22 
new22 %
InvalidOperationException22 3
(223 4
$str224 ^
)22^ _
;22_ `
var55 
patientConflict55 
=55  !
await55" '

repository55( 2
.552 3#
PatientHasConflictAsync553 J
(55J K
	patientId66 
,66 
entity77 
.77 
ScheduledDate77 $
,77$ %
entity88 
.88 
TimeSlot88 
)99 
;99 
if;; 
(;; 
patientConflict;; 
);;  
throw<< 
new<< %
InvalidOperationException<< 3
(<<3 4
$str<<4 b
)<<b c
;<<c d
var?? 

dailyLimit?? 
=?? 
await?? "

repository??# -
.??- .,
 PatientHasAppointmentOnDateAsync??. N
(??N O
	patientId@@ 
,@@ 
entityAA 
.AA 
ScheduledDateAA $
)BB 
;BB 
ifDD 
(DD 

dailyLimitDD 
)DD 
throwEE 
newEE %
InvalidOperationExceptionEE 3
(EE3 4
$strEE4 b
)EEb c
;EEc d
varHH 
appointmentHH 
=HH 
mapperHH $
.HH$ %
MapHH% (
<HH( )
AppointmentHH) 4
>HH4 5
(HH5 6
entityHH6 <
)HH< =
;HH= >
appointmentJJ 
.JJ 
	PatientIdJJ !
=JJ" #
	patientIdJJ$ -
;JJ- .
appointmentMM 
.MM 
StatusMM 
=MM  
PendingMM! (
;MM( )
varOO 
savedEntityOO 
=OO 
awaitOO #

repositoryOO$ .
.OO. /
CreateAsyncOO/ :
(OO: ;
appointmentOO; F
)OOF G
;OOG H
awaitQQ 
publishEndpointQQ !
.QQ! "
PublishQQ" )
(QQ) *
newRR  
BookAppointmentEventRR (
{SS 
EventIdTT 
=TT 
GuidTT "
.TT" #
NewGuidTT# *
(TT* +
)TT+ ,
,TT, -
	EventTypeVV 
=VV 
$strVV  3
,VV3 4

OccurredAtXX 
=XX  
DateTimeXX! )
.XX) *
UtcNowXX* 0
,XX0 1
SourceZZ 
=ZZ 
$strZZ -
,ZZ- .
AppointmentId\\ !
=\\" #
savedEntity\\$ /
.\\/ 0
AppointmentId\\0 =
,\\= >
	PatientId^^ 
=^^ 
savedEntity^^  +
.^^+ ,
	PatientId^^, 5
,^^5 6
DoctorId`` 
=`` 
savedEntity`` *
.``* +
DoctorId``+ 3
,``3 4
ScheduledDatebb !
=bb" #
savedEntitybb$ /
.bb/ 0
ScheduledDatebb0 =
,bb= >
TimeSlotdd 
=dd 
savedEntitydd *
.dd* +
TimeSlotdd+ 3
}ee 
)ee 
;ee 
returngg 
mappergg 
.gg 
Mapgg 
<gg 
AppointmentDtogg ,
>gg, -
(gg- .
savedEntitygg. 9
)gg9 :
;gg: ;
}hh 	
publickk 
asynckk 
Taskkk 
<kk 
boolkk 
>kk "
DeleteAppointmentAsynckk  6
(kk6 7
intkk7 :
appointmentIdkk; H
)kkH I
{ll 	
varmm 
deletedmm 
=mm 
awaitmm 

repositorymm  *
.mm* +
DeleteAsyncmm+ 6
(mm6 7
appointmentIdmm7 D
)mmD E
;mmE F
returnnn 
deletednn 
;nn 
}oo 	
publicqq 
asyncqq 
Taskqq 
<qq 
Listqq 
<qq 
AppointmentDtoqq -
>qq- .
>qq. /
GetAllAsyncqq0 ;
(qq; <
intqq< ?
pageqq@ D
,qqD E
intqqE H
pageSizeqqI Q
)qqQ R
{rr 	
returnss 
mapperss 
.ss 
Mapss 
<ss 
Listss "
<ss" #
AppointmentDtoss# 1
>ss1 2
>ss2 3
(ss3 4
awaittt 

repositorytt  
.tt  !#
GetAllAppointmentsAsynctt! 8
(tt8 9
pageuu 
,uu 
pageSizevv 
)vv 
)vv 
;vv 
}ww 	
publicyy 
asyncyy 
Taskyy 
<yy 
Listyy 
<yy 
AppointmentDtoyy -
>yy- .
>yy. /*
GetAppointmentsByDoctorIdAsyncyy0 N
(yyN O
intyyO R
doctorIdyyS [
,yy[ \
intyy\ _
pageyy` d
,yyd e
intyye h
pageSizeyyi q
)yyq r
{zz 	
return{{ 
mapper{{ 
.{{ 
Map{{ 
<{{ 
List{{ "
<{{" #
AppointmentDto{{# 1
>{{1 2
>{{2 3
({{3 4
await|| 

repository||  
.||  !2
&GetUpcomingAppointmentsByDoctorIdAsync||! G
(||G H
doctorId}} 
,}} 
page~~ 
,~~ 
pageSize 
) 
) 
; 
}
ÄÄ 	
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
List
ÇÇ 
<
ÇÇ 
AppointmentDto
ÇÇ -
>
ÇÇ- .
>
ÇÇ. /-
GetAppointmentsByPatientIdAsync
ÇÇ0 O
(
ÇÇO P
int
ÇÇP S
	patientId
ÇÇT ]
,
ÇÇ] ^
int
ÇÇ_ b
page
ÇÇc g
,
ÇÇg h
int
ÇÇi l
pageSize
ÇÇm u
)
ÇÇu v
{
ÉÉ 	
return
ÑÑ 
mapper
ÑÑ 
.
ÑÑ 
Map
ÑÑ 
<
ÑÑ 
List
ÑÑ "
<
ÑÑ" #
AppointmentDto
ÑÑ# 1
>
ÑÑ1 2
>
ÑÑ2 3
(
ÑÑ3 4
await
ÖÖ 

repository
ÖÖ  
.
ÖÖ  !-
GetAppointmentsByPatientIdAsync
ÖÖ! @
(
ÖÖ@ A
	patientId
ÜÜ 
,
ÜÜ 
page
áá 
,
áá 
pageSize
àà 
)
àà 
)
àà 
;
àà 
}
ââ 	
public
ää 
async
ää 
Task
ää 
<
ää 
List
ää 
<
ää 
AppointmentDto
ää -
>
ää- .
>
ää. /.
 GetAppointmentsByDoctorNameAsync
ää0 P
(
ääP Q
string
ääQ W

doctorName
ääX b
)
ääb c
{
ãã 	
return
åå 
mapper
åå 
.
åå 
Map
åå 
<
åå 
List
åå "
<
åå" #
AppointmentDto
åå# 1
>
åå1 2
>
åå2 3
(
åå3 4
await
åå4 9

repository
åå: D
.
ååD E.
 GetAppointmentsByDoctorNameAsync
ååE e
(
ååe f

doctorName
ååf p
)
ååp q
)
ååq r
;
åår s
}
çç 	
public
èè 
async
èè 
Task
èè 
<
èè 
List
èè 
<
èè 
AppointmentDto
èè -
>
èè- .
>
èè. //
!GetAppointmentsByPatientNameAsync
èè0 Q
(
èèQ R
string
èèR X
patientName
èèY d
)
èèd e
{
êê 	
return
ëë 
mapper
ëë 
.
ëë 
Map
ëë 
<
ëë 
List
ëë "
<
ëë" #
AppointmentDto
ëë# 1
>
ëë1 2
>
ëë2 3
(
ëë3 4
await
ëë4 9

repository
ëë: D
.
ëëD E/
!GetAppointmentsByPatientNameAsync
ëëE f
(
ëëf g
patientName
ëëg r
)
ëër s
)
ëës t
;
ëët u
}
íí 	
public
îî 
async
îî 
Task
îî 
<
îî 
AppointmentDto
îî (
?
îî( )
>
îî) *
GetByIdAsync
îî+ 7
(
îî7 8
int
îî8 ;
id
îî< >
)
îî> ?
{
ïï 	
var
ññ 
appointment
ññ 
=
ññ 
await
ññ #

repository
ññ$ .
.
ññ. /
GetByIdAsync
ññ/ ;
(
ññ; <
id
ññ< >
)
ññ> ?
;
ññ? @
return
óó 
mapper
óó 
.
óó 
Map
óó 
<
óó 
AppointmentDto
óó ,
?
óó, -
>
óó- .
(
óó. /
appointment
óó/ :
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
öö 
<
öö 
AppointmentDto
öö (
?
öö( )
>
öö) *
UpdateAsync
öö+ 6
(
öö6 7
int
öö7 :
id
öö; =
,
öö= >(
UpdateAppointmentStatusDto
öö? Y
entity
ööZ `
,
öö` a
string
ööb h
role
ööi m
)
ööm n
{
õõ 	
var
úú 
existing
úú 
=
úú 
await
úú  

repository
úú! +
.
úú+ ,
GetByIdAsync
úú, 8
(
úú8 9
id
úú9 ;
)
úú; <
;
úú< =
if
ûû 
(
ûû 
existing
ûû 
==
ûû 
null
ûû  
)
ûû  !
return
üü 
null
üü 
;
üü 
var
°° 
previousStatus
°° 
=
°°  
existing
°°! )
.
°°) *
Status
°°* 0
;
°°0 1
if
§§ 
(
§§ 
previousStatus
§§ 
==
§§ !
	Completed
§§" +
)
§§+ ,
throw
•• 
new
•• '
InvalidOperationException
•• 3
(
••3 4
$str
••4 ^
)
••^ _
;
••_ `
if
ßß 
(
ßß 
previousStatus
ßß 
==
ßß !
	Cancelled
ßß" +
)
ßß+ ,
throw
®® 
new
®® '
InvalidOperationException
®® 3
(
®®3 4
$str
®®4 ^
)
®®^ _
;
®®_ `
if
´´ 
(
´´ 
!
´´ 
new
´´ 
[
´´ 
]
´´ 
{
´´ 
Pending
´´  
,
´´  !
	Confirmed
´´" +
,
´´+ ,
	Cancelled
´´- 6
,
´´6 7
	Completed
´´8 A
}
´´B C
.
¨¨ 
Contains
¨¨ 
(
¨¨ 
entity
¨¨  
.
¨¨  !
Status
¨¨! '
)
¨¨' (
)
¨¨( )
{
≠≠ 
throw
ÆÆ 
new
ÆÆ '
InvalidOperationException
ÆÆ 3
(
ÆÆ3 4
$str
ÆÆ4 J
)
ÆÆJ K
;
ÆÆK L
}
ØØ 
if
≤≤ 
(
≤≤ 
role
≤≤ 
==
≤≤ 
PatientRole
≤≤ #
)
≤≤# $
{
≥≥ 
if
¥¥ 
(
¥¥ 
entity
¥¥ 
.
¥¥ 
Status
¥¥ !
!=
¥¥" $
	Cancelled
¥¥% .
)
¥¥. /
throw
µµ 
new
µµ '
InvalidOperationException
µµ 7
(
µµ7 8
$str
µµ8 ^
)
µµ^ _
;
µµ_ `
if
∑∑ 
(
∑∑ 
previousStatus
∑∑ "
!=
∑∑# %
Pending
∑∑& -
)
∑∑- .
throw
∏∏ 
new
∏∏ '
InvalidOperationException
∏∏ 7
(
∏∏7 8
$str
∏∏8 f
)
∏∏f g
;
∏∏g h
}
ππ 
if
ºº 
(
ºº 
role
ºº 
==
ºº 

DoctorRole
ºº "
)
ºº" #
{
ΩΩ 
if
ææ 
(
ææ 
previousStatus
ææ "
==
ææ# %
Pending
ææ& -
&&
ææ. 0
entity
ææ1 7
.
ææ7 8
Status
ææ8 >
!=
ææ? A
	Confirmed
ææB K
&&
ææL N
entity
ææO U
.
ææU V
Status
ææV \
!=
ææ] _
	Cancelled
ææ` i
)
ææi j
{
øø 
throw
¿¿ 
new
¿¿ '
InvalidOperationException
¿¿ 7
(
¿¿7 8
$str
¡¡ P
)
¡¡P Q
;
¡¡Q R
}
¬¬ 
if
ƒƒ 
(
ƒƒ 
previousStatus
ƒƒ "
==
ƒƒ# %
	Confirmed
ƒƒ& /
&&
ƒƒ0 2
entity
ƒƒ3 9
.
ƒƒ9 :
Status
ƒƒ: @
!=
ƒƒA C
	Completed
ƒƒD M
&&
ƒƒN P
entity
ƒƒQ W
.
ƒƒW X
Status
ƒƒX ^
!=
ƒƒ_ a
	Cancelled
ƒƒb k
)
ƒƒk l
{
≈≈ 
throw
∆∆ 
new
∆∆ '
InvalidOperationException
∆∆ 7
(
∆∆7 8
$str
«« R
)
««R S
;
««S T
}
»» 
}
…… 
if
ÃÃ 
(
ÃÃ 
entity
ÃÃ 
.
ÃÃ 
Status
ÃÃ 
==
ÃÃ  
	Cancelled
ÃÃ! *
)
ÃÃ* +
{
ÕÕ 
if
ŒŒ 
(
ŒŒ 
string
ŒŒ 
.
ŒŒ  
IsNullOrWhiteSpace
ŒŒ -
(
ŒŒ- .
entity
ŒŒ. 4
.
ŒŒ4 5 
CancellationReason
ŒŒ5 G
)
ŒŒG H
)
ŒŒH I
throw
œœ 
new
œœ '
InvalidOperationException
œœ 7
(
œœ7 8
$str
œœ8 Y
)
œœY Z
;
œœZ [
existing
—— 
.
——  
CancellationReason
—— +
=
——, -
entity
——. 4
.
——4 5 
CancellationReason
——5 G
;
——G H
}
““ 
else
”” 
{
‘‘ 
existing
’’ 
.
’’  
CancellationReason
’’ +
=
’’, -
null
’’. 2
;
’’2 3
}
÷÷ 
existing
ŸŸ 
.
ŸŸ 
Status
ŸŸ 
=
ŸŸ 
entity
ŸŸ $
.
ŸŸ$ %
Status
ŸŸ% +
;
ŸŸ+ ,
var
€€ 
updated
€€ 
=
€€ 
await
€€ 

repository
€€  *
.
€€* +
UpdateAsync
€€+ 6
(
€€6 7
id
€€7 9
,
€€9 :
existing
€€; C
)
€€C D
;
€€D E
if
ﬁﬁ 
(
ﬁﬁ 
previousStatus
ﬁﬁ 
!=
ﬁﬁ !
	Completed
ﬁﬁ" +
&&
ﬁﬁ, .
entity
ﬁﬁ/ 5
.
ﬁﬁ5 6
Status
ﬁﬁ6 <
==
ﬁﬁ= ?
	Completed
ﬁﬁ@ I
)
ﬁﬁI J
{
ﬂﬂ 
await
‡‡ !
healthRecordService
‡‡ )
.
‡‡) *#
CreateFromAppointment
‡‡* ?
(
‡‡? @
existing
‡‡@ H
)
‡‡H I
;
‡‡I J
}
·· 
return
„„ 
mapper
„„ 
.
„„ 
Map
„„ 
<
„„ 
AppointmentDto
„„ ,
>
„„, -
(
„„- .
updated
„„. 5
)
„„5 6
;
„„6 7
}
‰‰ 	
public
ÊÊ 
async
ÊÊ 
Task
ÊÊ 
<
ÊÊ 
List
ÊÊ 
<
ÊÊ 
AppointmentDto
ÊÊ -
>
ÊÊ- .
>
ÊÊ. /'
GetTodayAppointmentsAsync
ÊÊ0 I
(
ÊÊI J
int
ÊÊJ M
doctorId
ÊÊN V
,
ÊÊV W
int
ÊÊW Z
page
ÊÊ[ _
,
ÊÊ_ `
int
ÊÊ` c
pageSize
ÊÊd l
)
ÊÊl m
{
ÁÁ 	
var
ËË 
appointments
ËË 
=
ËË 
await
ÈÈ 

repository
ÈÈ  
.
ÈÈ  !'
GetTodayAppointmentsAsync
ÈÈ! :
(
ÈÈ: ;
doctorId
ÍÍ 
,
ÍÍ 
page
ÎÎ 
,
ÎÎ 
pageSize
ÏÏ 
)
ÏÏ 
;
ÏÏ 
return
ÓÓ 
mapper
ÓÓ 
.
ÓÓ 
Map
ÓÓ 
<
ÓÓ 
List
ÓÓ "
<
ÓÓ" #
AppointmentDto
ÓÓ# 1
>
ÓÓ1 2
>
ÓÓ2 3
(
ÓÓ3 4
appointments
ÓÓ4 @
)
ÓÓ@ A
;
ÓÓA B
}
ÔÔ 	
}
ÒÒ 
}ÚÚ °
ZC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\IHealthRecordService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
{ 
public 

	interface  
IHealthRecordService )
{ 
Task		 
<		 
List		 
<		 
HealthRecordDto		 !
>		! "
>		" #
GetAllAsync		$ /
(		/ 0
)		0 1
;		1 2
Task

 
<

 
HealthRecordDto

 
?

 
>

 
GetByIdAsync

 +
(

+ ,
int

, /
id

0 2
)

2 3
;

3 4
Task 
< 
HealthRecordDto 
? 
> 
UpdateAsync *
(* +
int+ .
id/ 1
,1 2!
UpdateHealthRecordDto3 H
dtoI L
)L M
;M N
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #&
GetRecordsByPatientIdAsync$ >
(> ?
int? B
	patientIdC L
)L M
;M N
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #%
GetRecordsByDoctorIdAsync$ =
(= >
int> A
doctorIdB J
)J K
;K L
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #(
GetRecordsByPatientNameAsync$ @
(@ A
stringA G
patientNameH S
)S T
;T U
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #'
GetRecordsByDoctorNameAsync$ ?
(? @
string@ F

doctorNameG Q
)Q R
;R S
Task !
CreateFromAppointment "
(" #
Appointment# .
appointment/ :
): ;
;; <
Task 
< 
HealthRecordDto 
? 
> #
GetByAppointmentIdAsync 6
(6 7
int7 :
appointmentId; H
)H I
;I J
Task 
< 
List 
<  
DoctorPatientListDto &
>& '
>' ("
GetDoctorPatientsAsync) ?
(? @
int@ C
doctorIdD L
)L M
;M N
} 
} ‚
TC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\IDoctorService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
{ 
public 

	interface 
IDoctorService #
{ 
Task 
< 
List 
< 
	DoctorDto 
> 
> 
GetAllAsync )
() *
)* +
;+ ,
Task		 
<		 
	DoctorDto		 
?		 
>		 
GetByIdAsync		 %
(		% &
int		& )
id		* ,
)		, -
;		- .
Task

 
<

 
	DoctorDto

 
>

 
CreateAsync

 #
(

# $
CreateDoctorDto

$ 3
entity

4 :
)

: ;
;

; <
Task 
< 
	DoctorDto 
? 
> 
UpdateAsync $
($ %
int% (
id) +
,+ ,
UpdateDoctorDto- <
entity= C
)C D
;D E
Task 
< 
List 
< 
	DoctorDto 
> 
> 
SearchByNameAsync /
(/ 0
string0 6
name7 ;
); <
;< =
Task 
< 
List 
< 
	DoctorDto 
> 
> !
GetActiveDoctorsAsync 3
(3 4
)4 5
;5 6
Task 
< 
List 
< 
	DoctorDto 
> 
> '
SearchBySpecialisationAsync 9
(9 :
string: @
specialisationA O
)O P
;P Q
Task 
< 
	DoctorDto 
> !
DeactivateDoctorAsync -
(- .
int. 1
id2 4
)4 5
;5 6
Task 
< 
bool 
> 
ToggleActiveAsync $
($ %
int% (
id) +
)+ ,
;, -
Task 
< 
List 
< 
	DoctorDto 
> 
> 
SearchAsync )
() *
string* 0
query1 6
)6 7
;7 8
Task 
< 
Doctor 
? 
> 
GetByUserIdAsync &
(& '
string' -
userId. 4
)4 5
;5 6
Task 
< 
List 
< 
	DoctorDto 
> 
> 
FilterAsync )
() *
string* 0
?0 1
name2 6
,6 7
string8 >
?> ?
specialization@ N
)N O
;O P
} 
} ˙
RC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\IAuthService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
{ 
public 

	interface 
IAuthService !
{ 
Task 
<  
AuthRegisterResponse !
>! "
RegisterAsync# 0
(0 1
RegisterDto1 <
request= D
)D E
;E F
Task 
< 
AuthResponse 
> 

LoginAsync %
(% &
LoginDto& .
request/ 6
)6 7
;7 8
Task		 
<		 
AuthResponse		 
?		 
>		 
RefreshAsync		 (
(		( )
string		) /
refreshToken		0 <
)		< =
;		= >
}

 
} «
YC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Services\IAppointmentService.cs
	namespace 	
HealthAxisApplicn
 
. 
Services $
{ 
public 

	interface 
IAppointmentService (
{ 
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "
GetAllAsync# .
(. /
int/ 2
page3 7
,7 8
int8 ;
pageSize< D
)D E
;E F
Task		 
<		 
AppointmentDto		 
?		 
>		 
GetByIdAsync		 *
(		* +
int		+ .
id		/ 1
)		1 2
;		2 3
Task

 
<

 
AppointmentDto

 
>

 
CreateAsync

 (
(

( ) 
CreateAppointmentDto

) =
entity

> D
,

D E
int

F I
	patientId

J S
)

S T
;

T U
Task 
< 
AppointmentDto 
? 
> 
UpdateAsync )
() *
int* -
id. 0
,0 1&
UpdateAppointmentStatusDto2 L
entityM S
,S T
stringU [
role\ `
)` a
;a b
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "+
GetAppointmentsByPatientIdAsync# B
(B C
intC F
	patientIdG P
,P Q
intR U
pageV Z
,Z [
int\ _
pageSize` h
)h i
;i j
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "*
GetAppointmentsByDoctorIdAsync# A
(A B
intB E
doctorIdF N
,N O
intO R
pageS W
,W X
intX [
pageSize\ d
)d e
;e f
Task 
< 
bool 
> "
DeleteAppointmentAsync )
() *
int* -
appointmentId. ;
); <
;< =
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "-
!GetAppointmentsByPatientNameAsync# D
(D E
stringE K
patientNameL W
)W X
;X Y
Task 
< 
List 
< 
AppointmentDto  
>  !
>! ",
 GetAppointmentsByDoctorNameAsync# C
(C D
stringD J

doctorNameK U
)U V
;V W
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "%
GetTodayAppointmentsAsync# <
(< =
int= @
doctorIdA I
,I J
intK N
pageO S
,S T
intU X
pageSizeY a
)a b
;b c
} 
} ü
UC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\IRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
{ 
public 

	interface 
IRepository  
<  !
T! "
>" #
where$ )
T* +
:+ ,
class, 1
{ 
Task 
< 
List 
< 
T 
> 
> 
GetAllAsync !
(! "
CancellationToken" 3
ct4 6
=7 8
default9 @
)@ A
;A B
Task 
< 
T 
? 
> 
GetByIdAsync 
( 
int !
id" $
,$ %
CancellationToken& 7
ct8 :
=; <
default= D
)D E
;E F
Task 
< 
T 
> 
CreateAsync 
( 
T 
entity $
,$ %
CancellationToken& 7
ct8 :
=; <
default= D
)D E
;E F
Task 
< 
T 
? 
> 
UpdateAsync 
( 
int  
id! #
,# $
T% &
entity' -
,- .
CancellationToken/ @
ctA C
=D E
defaultF M
)M N
;N O
Task		 
<		 
bool		 
>		 
DeleteAsync		 
(		 
int		 "
id		# %
,		% &
CancellationToken		' 8
ct		9 ;
=		< =
default		> E
)		E F
;		F G
}

 
} †
\C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\IPatientRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
{ 
public 

	interface 
IPatientRepository '
:' (
IRepository) 4
<4 5
Patient5 <
>< =
{ 
Task 
< 
List 
< 
Patient 
> 
> 
SearchByNameAsync -
(- .
string. 4
name5 9
,9 :
CancellationToken; L
ctM O
=P Q
defaultR Y
)Y Z
;Z [
Task 
< 
Patient 
? 
> $
SearchByPhoneNumberAsync /
(/ 0
string0 6
phoneNumber7 B
,B C
CancellationTokenD U
ctV X
=Y Z
default[ b
)b c
;c d
Task		 
<		 
Patient		 
?		 
>		 
SearchByEmailAsync		 )
(		) *
string		* 0
email		1 6
,		6 7
CancellationToken		8 I
ct		J L
=		M N
default		O V
)		V W
;		W X
Task

 
<

 
List

 
<

 
Patient

 
>

 
>

 
SearchAsync

 '
(

' (
string

( .
?

. /
name

0 4
,

4 5
string

6 <
?

< =
phone

> C
,

C D
CancellationToken

E V
ct

W Y
=

Z [
default

\ c
)

c d
;

d e
Task 
< 
Patient 
? 
> 
GetByUserIdAsync '
(' (
string( .
userId/ 5
)5 6
;6 7
} 
} Ã-
YC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\Impl\Repository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
.( )
Impl) -
{ 
public 

class 

Repository 
< 
T 
> 
:  
IRepository! ,
<, -
T- .
>. /
where0 5
T6 7
:8 9
class: ?
{ 
private 
readonly 
	DbContext "
_context# +
;+ ,
public

 

Repository

 
(

 
	DbContext

 #
context

$ +
)

+ ,
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
T 
> 
CreateAsync (
(( )
T) *
entity+ 1
,1 2
CancellationToken3 D
ctE G
=H I
defaultJ Q
)Q R
{ 	
await 
_context 
. 
Set 
< 
T  
>  !
(! "
)" #
.# $
AddAsync$ ,
(, -
entity- 3
,3 4
ct5 7
)7 8
;8 9
await 
_context 
. 
SaveChangesAsync +
(+ ,
ct, .
). /
;/ 0
return 
entity 
; 
} 	
public 
async 
Task 
< 
bool 
> 
DeleteAsync  +
(+ ,
int, /
id0 2
,2 3
CancellationToken4 E
ctF H
=I J
defaultK R
)R S
{ 	
var 
existing 
= 
await  
_context! )
.) *
Set* -
<- .
T. /
>/ 0
(0 1
)1 2
.2 3
	FindAsync3 <
(< =
[= >
id> @
]@ A
,A B
ctC E
)E F
;F G
if 
( 
existing 
is 
null  
)  !
return" (
false) .
;. /
_context 
. 
Set 
< 
T 
> 
( 
) 
. 
Remove $
($ %
existing% -
)- .
;. /
await 
_context 
. 
SaveChangesAsync +
(+ ,
ct, .
). /
;/ 0
return 
true 
; 
} 	
public 
async 
Task 
< 
List 
< 
T  
>  !
>! "
GetAllAsync# .
(. /
CancellationToken/ @
ctA C
=D E
defaultF M
)M N
{ 	
return   
await   
_context   !
.  ! "
Set  " %
<  % &
T  & '
>  ' (
(  ( )
)  ) *
.  * +
ToListAsync  + 6
(  6 7
ct  7 9
)  9 :
;  : ;
}!! 	
public## 
async## 
Task## 
<## 
T## 
?## 
>## 
GetByIdAsync## *
(##* +
int##+ .
id##/ 1
,##1 2
CancellationToken##3 D
ct##E G
=##H I
default##J Q
)##Q R
{$$ 	
var%% 
existing%% 
=%% 
await%%  
_context%%! )
.%%) *
Set%%* -
<%%- .
T%%. /
>%%/ 0
(%%0 1
)%%1 2
.%%2 3
	FindAsync%%3 <
(%%< =
[%%= >
id%%> @
]%%@ A
,%%A B
ct%%C E
)%%E F
;%%F G
return&& 
existing&& 
;&& 
}'' 	
public)) 
async)) 
Task)) 
<)) 
T)) 
?)) 
>)) 
UpdateAsync)) )
())) *
int))* -
id)). 0
,))0 1
T))2 3
entity))4 :
,)): ;
CancellationToken))< M
ct))N P
=))Q R
default))S Z
)))Z [
{** 	
var++ 
existing++ 
=++ 
await++  
_context++! )
.++) *
Set++* -
<++- .
T++. /
>++/ 0
(++0 1
)++1 2
.++2 3
	FindAsync++3 <
(++< =
[++= >
id++> @
]++@ A
,++A B
ct++C E
)++E F
;++F G
if,, 
(,, 
existing,, 
is,, 
null,,  
),,  !
return,," (
null,,) -
;,,- .
_context-- 
.-- 
Entry-- 
(-- 
existing-- #
)--# $
.--$ %
State--% *
=--+ ,
EntityState--- 8
.--8 9
Detached--9 A
;--A B
_context.. 
... 
Set.. 
<.. 
T.. 
>.. 
(.. 
).. 
... 
Update.. $
(..$ %
entity..% +
)..+ ,
;.., -
await// 
_context// 
.// 
SaveChangesAsync// +
(//+ ,
ct//, .
)//. /
;/// 0
return00 
entity00 
;00 
}22 	
}33 
}44 Â4
`C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\Impl\PatientRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
.( )
Impl) -
{ 
public 

class 
PatientRepository "
:# $

Repository% /
</ 0
Patient0 7
>7 8
,8 9
IPatientRepository: L
{ 
private		 
readonly		 
AppDbContext		 %
_context		& .
;		. /
public

 
PatientRepository

  
(

  !
AppDbContext

! -
context

. 5
)

5 6
:

6 7
base

8 <
(

< =
context

= D
)

D E
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
Patient !
?! "
>" #
SearchByEmailAsync$ 6
(6 7
string7 =
email> C
,C D
CancellationTokenE V
ctW Y
=Z [
default\ c
)c d
{ 	
var 

cleanEmail 
= 
email "
." #
Trim# '
(' (
)( )
.) *
ToLower* 1
(1 2
)2 3
;3 4
return 
await 
_context !
.! "
Patients" *
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
Email, 1
.1 2
ToLower2 9
(9 :
): ;
==< >

cleanEmail? I
,I J
ctK M
)M N
;N O
} 	
public 
async 
Task 
< 
List 
< 
Patient &
>& '
>' (
SearchByNameAsync) :
(: ;
string; A
nameB F
,F G
CancellationTokenH Y
ctZ \
=] ^
default_ f
)f g
{ 	
var 
	cleanName 
= 
name  
.  !
Trim! %
(% &
)& '
.' (
ToLower( /
(/ 0
)0 1
;1 2
return 
await 
_context !
.! "
Patients" *
. 
Where 
( 
p 
=> 
p 
. 
PatientName )
.) *
ToLower* 1
(1 2
)2 3
.3 4
Contains4 <
(< =
	cleanName= F
)F G
)G H
. 
ToListAsync 
( 
ct 
)  
;  !
} 	
public   
async   
Task   
<   
Patient   !
?  ! "
>  " #$
SearchByPhoneNumberAsync  $ <
(  < =
string  = C
phoneNumber  D O
,  O P
CancellationToken  Q b
ct  c e
=  f g
default  h o
)  o p
{!! 	
var"" 
existing"" 
="" 
await""  
_context""! )
."") *
Set""* -
<""- .
Patient"". 5
>""5 6
(""6 7
)""7 8
.""8 9
FirstOrDefaultAsync""9 L
(""L M
p""M N
=>""O Q
p""R S
.""S T
PhoneNo""T [
.""[ \
Contains""\ d
(""d e
phoneNumber""e p
)""p q
,""q r
ct""s u
)""u v
;""v w
return## 
existing## 
;## 
}$$ 	
public&& 
async&& 
Task&& 
<&& 
List&& 
<&& 
Patient&& &
>&&& '
>&&' (
SearchAsync&&) 4
(&&4 5
string&&5 ;
?&&; <
name&&= A
,&&A B
string&&C I
?&&I J
phone&&K P
,&&P Q
CancellationToken&&R c
ct&&d f
=&&g h
default&&i p
)&&p q
{'' 	
var(( 
query(( 
=(( 
_context((  
.((  !
Patients((! )
.(() *
AsQueryable((* 5
(((5 6
)((6 7
;((7 8
if++ 
(++ 
!++ 
string++ 
.++ 
IsNullOrWhiteSpace++ *
(++* +
name+++ /
)++/ 0
)++0 1
{,, 
var-- 
	lowerName-- 
=-- 
name--  $
.--$ %
Trim--% )
(--) *
)--* +
.--+ ,
ToLower--, 3
(--3 4
)--4 5
;--5 6
query// 
=// 
query// 
.// 
Where// #
(//# $
p//$ %
=>//& (
p00 
.00 
PatientName00 !
.00! "
ToLower00" )
(00) *
)00* +
.00+ ,
Contains00, 4
(004 5
	lowerName005 >
)00> ?
)00? @
;00@ A
}11 
if44 
(44 
!44 
string44 
.44 
IsNullOrWhiteSpace44 *
(44* +
phone44+ 0
)440 1
)441 2
{55 
var66 

cleanPhone66 
=66  
phone66! &
.66& '
Trim66' +
(66+ ,
)66, -
;66- .
query88 
=88 
query88 
.88 
Where88 #
(88# $
p88$ %
=>88& (
p99 
.99 
PhoneNo99 
.99 
Contains99 &
(99& '

cleanPhone99' 1
)991 2
)992 3
;993 4
}:: 
return<< 
await<< 
query<< 
.<< 
ToListAsync<< *
(<<* +
ct<<+ -
)<<- .
;<<. /
}== 	
public?? 
async?? 
Task?? 
<?? 
Patient?? !
???! "
>??" #
GetByUserIdAsync??$ 4
(??4 5
string??5 ;
userId??< B
)??B C
{@@ 	
returnAA 
awaitAA 
_contextAA !
.AA! "
PatientsAA" *
.BB 
FirstOrDefaultAsyncBB $
(BB$ %
pBB% &
=>BB' )
pBB* +
.BB+ ,
UserIdBB, 2
==BB3 5
userIdBB6 <
)BB< =
;BB= >
}CC 	
}EE 
}FF E
eC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\Impl\HealthRecordRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
.( )
Impl) -
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
 
AppDbContext

 %
_context

& .
;

. /
public "
HealthRecordRepository %
(% &
AppDbContext& 2
context3 :
): ;
:< =
base> B
(B C
contextC J
)J K
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -&
GetRecordsByPatientIdAsync. H
(H I
intI L
	patientIdM V
,V W
CancellationTokenX i
ctj l
=m n
defaulto v
)v w
{ 	
return 
await 
_context !
.! "
Set" %
<% &
HealthRecord& 2
>2 3
(3 4
)4 5
. 
Include 
( 
h 
=> 
h 
.  
Doctor  &
)& '
. 
Where 
( 
h 
=> 
h 
. 
	PatientId '
==( *
	patientId+ 4
)4 5
. 
ToListAsync 
( 
ct 
)  
;  !
} 	
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -%
GetRecordsByDoctorIdAsync. G
(G H
intH K
doctorIdL T
,T U
CancellationTokenV g
cth j
=k l
defaultm t
)t u
{ 	
var 
recordsByDoctorID !
=" #
await$ )
_context* 2
.2 3
Set3 6
<6 7
HealthRecord7 C
>C D
(D E
)E F
.F G
WhereG L
(L M
hM N
=>O Q
hR S
.S T
DoctorIdT \
==] _
doctorId` h
)h i
.i j
ToListAsyncj u
(u v
ctv x
)x y
;y z
return 
recordsByDoctorID $
;$ %
} 	
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -'
GetRecordsByDoctorNameAsync. I
(I J
stringJ P

doctorNameQ [
,[ \
CancellationToken] n
cto q
=r s
defaultt {
){ |
{ 	
return 
await 
_context !
.! "
Set" %
<% &
HealthRecord& 2
>2 3
(3 4
)4 5
.   
Include   
(   
h   
=>   
h   
.    
Doctor    &
)  & '
.!! 
Where!! 
(!! 
h!! 
=>!! 
h!! 
.!! 
Doctor!! $
.!!$ %

DoctorName!!% /
==!!0 2

doctorName!!3 =
)!!= >
."" 
ToListAsync"" 
("" 
ct"" 
)""  
;""  !
}## 	
public%% 
async%% 
Task%% 
<%% 
List%% 
<%% 
HealthRecord%% +
>%%+ ,
>%%, -(
GetRecordsByPatientNameAsync%%. J
(%%J K
string%%K Q
patientName%%R ]
,%%] ^
CancellationToken%%_ p
ct%%q s
=%%t u
default%%v }
)%%} ~
{&& 	
return'' 
await'' 
_context'' !
.''! "
Set''" %
<''% &
HealthRecord''& 2
>''2 3
(''3 4
)''4 5
.(( 
Include(( 
((( 
h(( 
=>(( 
h(( 
.((  
Patient((  '
)((' (
.)) 
Where)) 
()) 
h)) 
=>)) 
h)) 
.)) 
Patient)) %
.))% &
PatientName))& 1
==))2 4
patientName))5 @
)))@ A
.))A B
ToListAsync))B M
())M N
ct))N P
)))P Q
;))Q R
}** 	
public,, 
async,, 
Task,, 
<,, 
HealthRecord,, &
?,,& '
>,,' (#
GetByAppointmentIdAsync,,) @
(,,@ A
int,,A D
appointmentId,,E R
,,,R S
CancellationToken,,T e
ct,,f h
=,,i j
default,,k r
),,r s
{-- 	
return.. 
await.. 
_context.. !
...! "
Set.." %
<..% &
HealthRecord..& 2
>..2 3
(..3 4
)..4 5
.// 
AsNoTracking// 
(// 
)// 
.00 
FirstOrDefaultAsync00 $
(00$ %
h00% &
=>00' )
h00* +
.00+ ,
AppointmentId00, 9
==00: <
appointmentId00= J
,00J K
ct00L N
)00N O
;00O P
}11 	
public33 
async33 
Task33 
<33 
bool33 
>33 %
ExistsForAppointmentAsync33  9
(339 :
int33: =
appointmentId33> K
,33K L
CancellationToken33M ^
ct33_ a
=33b c
default33d k
)33k l
{44 	
return55 
await55 
_context55 !
.55! "
Set55" %
<55% &
HealthRecord55& 2
>552 3
(553 4
)554 5
.66 
AnyAsync66 
(66 
h66 
=>66 
h66  
.66  !
AppointmentId66! .
==66/ 1
appointmentId662 ?
,66? @
ct66A C
)66C D
;66D E
}77 	
public99 
async99 
Task99 
<99 
List99 
<99  
DoctorPatientListDto99 3
>993 4
>994 5"
GetDoctorPatientsAsync996 L
(99L M
int99M P
doctorId99Q Y
)99Y Z
{:: 	
return;; 
await;; 
_context;; !
.;;! "
HealthRecords;;" /
.<< 
Where<< 
(<< 
h<< 
=><< 
h<< 
.<< 
DoctorId<< &
==<<' )
doctorId<<* 2
)<<2 3
.== 
Select== 
(== 
h== 
=>== 
h== 
.== 
Patient== &
)==& '
.>> 
Distinct>> 
(>> 
)>> 
.?? 
Select?? 
(?? 
p?? 
=>?? 
new??   
DoctorPatientListDto??! 5
{@@ 
	PatientIdAA 
=AA 
pAA  !
.AA! "
	PatientIdAA" +
,AA+ ,
PatientNameBB 
=BB  !
pBB" #
.BB# $
PatientNameBB$ /
}CC 
)CC 
.DD 
ToListAsyncDD 
(DD 
)DD 
;DD 
}EE 	
publicFF 
asyncFF 
TaskFF 
<FF 
ListFF 
<FF 
HealthRecordFF +
>FF+ ,
>FF, -+
GetRecordsForDoctorPatientAsyncFF. M
(FFM N
intFFN Q
doctorIdFFR Z
,FFZ [
intFF\ _
	patientIdFF` i
)FFi j
{GG 	
returnHH 
awaitHH 
_contextHH !
.HH! "
HealthRecordsHH" /
.II 
WhereII 
(II 
hII 
=>II 
hJJ 
.JJ 
DoctorIdJJ 
==JJ !
doctorIdJJ" *
&&JJ+ -
hKK 
.KK 
	PatientIdKK 
==KK  "
	patientIdKK# ,
)KK, -
.LL 
OrderByDescendingLL "
(LL" #
hLL# $
=>LL% '
hLL( )
.LL) *
	VisitDateLL* 3
)LL3 4
.MM 
ToListAsyncMM 
(MM 
)MM 
;MM 
}NN 	
}OO 
}PP ‚<
_C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\Impl\DoctorRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
.( )
Impl) -
{ 
public 

class 
DoctorRepository !
:" #

Repository$ .
<. /
Doctor/ 5
>5 6
,6 7
IDoctorRepository8 I
{ 
private		 
readonly		 
AppDbContext		 %
_context		& .
;		. /
public

 
DoctorRepository

 
(

  
AppDbContext

  ,
context

- 4
)

4 5
:

5 6
base

7 ;
(

; <
context

< C
)

C D
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
List 
< 
Doctor %
>% &
>& '!
GetActiveDoctorsAsync( =
(= >
CancellationToken> O
ctP R
=S T
defaultU \
)\ ]
{ 	
var 
availableDoctors  
=! "
await# (
_context) 1
.1 2
Set2 5
<5 6
Doctor6 <
>< =
(= >
)> ?
.? @
Where@ E
(E F
dF G
=>H J
dK L
.L M
IsActiveM U
)U V
.V W
ToListAsyncW b
(b c
ctc e
)e f
;f g
return 
availableDoctors #
;# $
} 	
public 
async 
Task 
< 
List 
< 
Doctor %
>% &
>& ''
SearchBySpecialisationAsync( C
(C D
stringD J
specialisationK Y
,Y Z
CancellationToken[ l
ctm o
=p q
defaultr y
)y z
{ 	
specialisation 
= 
specialisation +
.+ ,
Trim, 0
(0 1
)1 2
;2 3
var 
existing 
= 
await  
_context! )
.) *
Set* -
<- .
Doctor. 4
>4 5
(5 6
)6 7
. 
Where 
( 
d 
=> 
d 
. 
Specialisation ,
==- /
specialisation0 >
)> ?
. 
ToListAsync 
( 
ct 
)  
;  !
return 
existing 
; 
} 	
public 
async 
Task 
< 
List 
< 
Doctor %
>% &
>& '
SearchByNameAsync( 9
(9 :
string: @
nameA E
,E F
CancellationTokenG X
ctY [
=\ ]
default^ e
)e f
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
=>"" 
EF"" 
."" 
	Functions"" (
.""( )
Like"") -
(""- .
d"". /
.""/ 0

DoctorName""0 :
,"": ;
$"""< >
$str""> ?
{""? @
name""@ D
}""D E
$str""E F
"""F G
)""G H
)""H I
.## 
ToListAsync## 
(## 
ct## 
)##  
;##  !
}$$ 	
public(( 
async(( 
Task(( 
<(( 
List(( 
<(( 
Doctor(( %
>((% &
>((& '
SearchAsync((( 3
(((3 4
string((4 :
query((; @
,((@ A
CancellationToken((B S
ct((T V
=((W X
default((Y `
)((` a
{)) 	
return** 
await** 
_context** !
.**! "
Doctors**" )
.++ 
Where++ 
(++ 
d++ 
=>++ 
EF,, 
.,, 
	Functions,,  
.,,  !
Like,,! %
(,,% &
d,,& '
.,,' (

DoctorName,,( 2
,,,2 3
$",,4 6
$str,,6 7
{,,7 8
query,,8 =
},,= >
$str,,> ?
",,? @
),,@ A
||,,B D
EF-- 
.-- 
	Functions--  
.--  !
Like--! %
(--% &
d--& '
.--' (
Specialisation--( 6
,--6 7
$"--8 :
$str--: ;
{--; <
query--< A
}--A B
$str--B C
"--C D
)--D E
)--E F
... 
ToListAsync.. 
(.. 
ct.. 
)..  
;..  !
}// 	
public22 
async22 
Task22 
<22 
Doctor22  
?22  !
>22! "
GetByUserIdAsync22# 3
(223 4
string224 :
userId22; A
)22A B
{33 	
return44 
await44 
_context44 !
.44! "
Doctors44" )
.55 
FirstOrDefaultAsync55 $
(55$ %
d55% &
=>55' )
d55* +
.55+ ,
UserId55, 2
==553 5
userId556 <
)55< =
;55= >
}66 	
public88 
async88 
Task88 
<88 
List88 
<88 
Doctor88 %
>88% &
>88& '
FilterAsync88( 3
(883 4
string884 :
?88: ;
name88< @
,88@ A
string88B H
?88H I
specialization88J X
)88X Y
{99 	
var;; 
query;; 
=;; 
_context;;  
.;;  !
Doctors;;! (
.;;( )
AsQueryable;;) 4
(;;4 5
);;5 6
;;;6 7
if== 
(== 
!== 
string== 
.== 
IsNullOrWhiteSpace== *
(==* +
name==+ /
)==/ 0
)==0 1
{>> 
query?? 
=?? 
query?? 
.?? 
Where?? #
(??# $
d??$ %
=>??& (
EF@@ 
.@@ 
	Functions@@  
.@@  !
Like@@! %
(@@% &
d@@& '
.@@' (

DoctorName@@( 2
,@@2 3
$"@@4 6
$str@@6 7
{@@7 8
name@@8 <
}@@< =
$str@@= >
"@@> ?
)@@? @
)@@@ A
;@@A B
}AA 
ifCC 
(CC 
!CC 
stringCC 
.CC 
IsNullOrWhiteSpaceCC *
(CC* +
specializationCC+ 9
)CC9 :
)CC: ;
{DD 
queryEE 
=EE 
queryEE 
.EE 
WhereEE #
(EE# $
dEE$ %
=>EE& (
EFFF 
.FF 
	FunctionsFF  
.FF  !
LikeFF! %
(FF% &
dFF& '
.FF' (
SpecialisationFF( 6
,FF6 7
$"FF8 :
$strFF: ;
{FF; <
specializationFF< J
}FFJ K
$strFFK L
"FFL M
)FFM N
)FFN O
;FFO P
}GG 
returnII 
awaitII 
queryII 
.II 
ToListAsyncII *
(II* +
)II+ ,
;II, -
}KK 	
}LL 
}MM ‚e
dC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\Impl\AppointmentRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
.( )
Impl) -
{ 
public 

class !
AppointmentRepository &
:' (

Repository) 3
<3 4
Appointment4 ?
>? @
,@ A"
IAppointmentRepositoryB X
{ 
private		 
readonly		 
AppDbContext		 %
_context		& .
;		. /
public

 !
AppointmentRepository

 $
(

$ %
AppDbContext

% 1
context

2 9
)

9 :
:

: ;
base

< @
(

@ A
context

A H
)

H I
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
List 
< 
Appointment *
>* +
>+ ,2
&GetUpcomingAppointmentsByDoctorIdAsync- S
(S T
intT W
doctorIdX `
,` a
inta d
pagee i
,i j
intj m
pageSizen v
,v w
CancellationToken	w à
ct
â ã
=
å ç
default
é ï
)
ï ñ
{ 	
return 
await 
_context !
.! "
Set" %
<% &
Appointment& 1
>1 2
(2 3
)3 4
. 
AsNoTracking 
( 
) 
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
. 
Where 
( 
a 
=> 
a 
. 
DoctorId 
== !
doctorId" *
&&+ -
a 
. 
ScheduledDate #
.# $
Date$ (
>=) +
DateTime, 4
.4 5
UtcNow5 ;
.; <
Date< @
&&A C
( 
a 
. 
Status 
==  
$str! *
||+ -
a. /
./ 0
Status0 6
==7 9
$str: E
)E F
) 
. 
OrderBy 
( 
a 
=> 
a 
.  
ScheduledDate  -
)- .
. 
Skip 
( 
( 
page 
- 
$num 
)  
*! "
pageSize# +
)+ ,
. 
Take 
( 
pageSize 
) 
. 
ToListAsync 
( 
ct 
)  
;  !
} 	
public"" 
async"" 
Task"" 
<"" 
List"" 
<"" 
Appointment"" *
>""* +
>""+ ,+
GetAppointmentsByPatientIdAsync""- L
(""L M
int""M P
	patientId""Q Z
,""Z [
int""\ _
page""` d
,""d e
int""f i
pageSize""j r
,""r s
CancellationToken	""t Ö
ct
""Ü à
=
""â ä
default
""ã í
)
""í ì
{## 	
return$$ 
await$$ 
_context$$ !
.$$! "
Set$$" %
<$$% &
Appointment$$& 1
>$$1 2
($$2 3
)$$3 4
.%% 
AsNoTracking%% 
(%% 
)%% 
.&& 
Include&& 
(&& 
a&& 
=>&& 
a&& 
.&&  
Doctor&&  &
)&&& '
.'' 
Where'' 
('' 
a'' 
=>'' 
a'' 
.'' 
	PatientId'' '
==''( *
	patientId''+ 4
)''4 5
.(( 
OrderByDescending(( "
(((" #
a((# $
=>((% '
a((( )
.(() *
ScheduledDate((* 7
)((7 8
.)) 
Skip)) 
()) 
()) 
page)) 
-)) 
$num)) 
)))  
*))! "
pageSize))# +
)))+ ,
.** 
Take** 
(** 
pageSize** 
)** 
.++ 
ToListAsync++ 
(++ 
ct++ 
)++  
;++  !
},, 	
public// 
async// 
Task// 
<// 
List// 
<// 
Appointment// *
>//* +
>//+ ,,
 GetAppointmentsByDoctorNameAsync//- M
(//M N
string//N T

doctorName//U _
,//_ `
CancellationToken//a r
ct//s u
=//v w
default//x 
)	// Ä
{00 	
return11 
await11 
_context11 !
.11! "
Set11" %
<11% &
Appointment11& 1
>111 2
(112 3
)113 4
.22 
Include22 
(22 
a22 
=>22 
a22 
.22  
Doctor22  &
)22& '
.33 
Where33 
(33 
a33 
=>33 
a33 
.33 
Doctor33 $
.33$ %

DoctorName33% /
==330 2

doctorName333 =
)33= >
.44 
ToListAsync44 
(44 
ct44 
)44  
;44  !
}55 	
public77 
async77 
Task77 
<77 
List77 
<77 
Appointment77 *
>77* +
>77+ ,-
!GetAppointmentsByPatientNameAsync77- N
(77N O
string77O U
patientName77V a
,77a b
CancellationToken77c t
ct77u w
=77x y
default	77z Å
)
77Å Ç
{88 	
return99 
await99 
_context99 !
.99! "
Set99" %
<99% &
Appointment99& 1
>991 2
(992 3
)993 4
.:: 
Include:: 
(:: 
a:: 
=>:: 
a:: 
.::  
Patient::  '
)::' (
.;; 
Where;; 
(;; 
a;; 
=>;; 
a;; 
.;; 
Patient;; %
.;;% &
PatientName;;& 1
==;;2 4
patientName;;5 @
);;@ A
.<< 
ToListAsync<< 
(<< 
ct<< 
)<<  
;<<  !
}== 	
public?? 
async?? 
Task?? 
<?? 
bool?? 
>?? "
DoctorHasConflictAsync??  6
(??6 7
int??7 :
doctorId??; C
,??C D
DateTime??E M
date??N R
,??R S
string??T Z
timeSlot??[ c
,??c d
CancellationToken??e v
ct??w y
=??z {
default	??| É
)
??É Ñ
{@@ 	
returnAA 
awaitAA 
_contextAA !
.AA! "
SetAA" %
<AA% &
AppointmentAA& 1
>AA1 2
(AA2 3
)AA3 4
.BB 
AnyAsyncBB 
(BB 
aBB 
=>BB 
aCC 
.CC 
DoctorIdCC 
==CC !
doctorIdCC" *
&&CC+ -
aDD 
.DD 
ScheduledDateDD #
.DD# $
DateDD$ (
==DD) +
dateDD, 0
.DD0 1
DateDD1 5
&&DD6 8
aEE 
.EE 
TimeSlotEE 
==EE !
timeSlotEE" *
&&EE+ -
aFF 
.FF 
StatusFF 
!=FF 
$strFF )
,FF) *
ctFF+ -
)FF- .
;FF. /
}GG 	
publicII 
asyncII 
TaskII 
<II 
boolII 
>II #
PatientHasConflictAsyncII  7
(II7 8
intII8 ;
	patientIdII< E
,IIE F
DateTimeIIG O
dateIIP T
,IIT U
stringIIV \
timeSlotII] e
,IIe f
CancellationTokenIIg x
ctIIy {
=II| }
default	II~ Ö
)
IIÖ Ü
{JJ 	
returnKK 
awaitKK 
_contextKK !
.KK! "
SetKK" %
<KK% &
AppointmentKK& 1
>KK1 2
(KK2 3
)KK3 4
.LL 
AnyAsyncLL 
(LL 
aLL 
=>LL 
aMM 
.MM 
	PatientIdMM 
==MM  "
	patientIdMM# ,
&&MM- /
aNN 
.NN 
ScheduledDateNN #
.NN# $
DateNN$ (
==NN) +
dateNN, 0
.NN0 1
DateNN1 5
&&NN6 8
aOO 
.OO 
TimeSlotOO 
==OO !
timeSlotOO" *
,OO* +
ctOO, .
)OO. /
;OO/ 0
}PP 	
publicRR 
asyncRR 
TaskRR 
<RR 
boolRR 
>RR ,
 PatientHasAppointmentOnDateAsyncRR  @
(RR@ A
intRRA D
	patientIdRRE N
,RRN O
DateTimeRRP X
dateRRY ]
,RR] ^
CancellationTokenRR_ p
ctRRq s
=RRt u
defaultRRv }
)RR} ~
{SS 	
returnTT 
awaitTT 
_contextTT !
.TT! "
SetTT" %
<TT% &
AppointmentTT& 1
>TT1 2
(TT2 3
)TT3 4
.UU 
AnyAsyncUU 
(UU 
aUU 
=>UU 
aVV 
.VV 
	PatientIdVV 
==VV  "
	patientIdVV# ,
&&VV- /
aWW 
.WW 
ScheduledDateWW #
.WW# $
DateWW$ (
==WW) +
dateWW, 0
.WW0 1
DateWW1 5
,WW5 6
ctWW7 9
)WW9 :
;WW: ;
}XX 	
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
ListZZ 
<ZZ 
AppointmentZZ *
>ZZ* +
>ZZ+ ,%
GetTodayAppointmentsAsyncZZ- F
(ZZF G
intZZG J
doctorIdZZK S
,ZZS T
intZZT W
pageZZX \
,ZZ\ ]
intZZ] `
pageSizeZZa i
,ZZi j
CancellationTokenZZj {
ctZZ| ~
=	ZZ Ä
default
ZZÅ à
)
ZZà â
{[[ 	
return\\ 
await\\ 
_context\\ !
.\\! "
Appointments\\" .
.]] 
Include]] 
(]] 
a]] 
=>]] 
a]] 
.]]  
Patient]]  '
)]]' (
.^^ 
Where^^ 
(^^ 
a^^ 
=>^^ 
a__ 
.__ 
DoctorId__ 
==__ !
doctorId__" *
&&__+ -
a`` 
.`` 
ScheduledDate`` #
.``# $
Date``$ (
==``) +
DateTime``, 4
.``4 5
Today``5 :
)``: ;
.aa 
OrderByaa 
(aa 
aaa 
=>aa 
aaa 
.aa  
TimeSlotaa  (
)aa( )
.bb 
Skipbb 
(bb 
(bb 
pagebb 
-bb 
$numbb 
)bb  
*bb! "
pageSizebb# +
)bb+ ,
.cc 
Takecc 
(cc 
pageSizecc 
)cc 
.dd 
ToListAsyncdd 
(dd 
ctdd 
)dd  
;dd  !
}ee 	
publichh 
asynchh 
Taskhh 
<hh 
Listhh 
<hh 
Appointmenthh *
>hh* +
>hh+ ,#
GetAllAppointmentsAsynchh- D
(hhD E
inthhE H
pagehhI M
,hhM N
inthhN Q
pageSizehhR Z
)hhZ [
{ii 	
returnjj 
awaitjj 
_contextjj !
.jj! "
Appointmentsjj" .
.kk 
Includekk 
(kk 
akk 
=>kk 
akk 
.kk  
Patientkk  '
)kk' (
.ll 
Includell 
(ll 
all 
=>ll 
all 
.ll  
Doctorll  &
)ll& '
.mm 
OrderByDescendingmm "
(mm" #
amm# $
=>mm% '
amm( )
.mm) *
AppointmentIdmm* 7
)mm7 8
.nn 
Skipnn 
(nn 
(nn 
pagenn 
-nn 
$numnn 
)nn  
*nn! "
pageSizenn# +
)nn+ ,
.oo 
Takeoo 
(oo 
pageSizeoo 
)oo 
.pp 
ToListAsyncpp 
(pp 
)pp 
;pp 
}qq 	
}tt 
}uu ü
aC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\IHealthRecordRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
{ 
public 

	interface #
IHealthRecordRepository ,
:, -
IRepository. 9
<9 :
HealthRecord: F
>F G
{ 
Task 
< 
List 
< 
HealthRecord 
> 
>  &
GetRecordsByPatientIdAsync! ;
(; <
int< ?
	patientId@ I
,I J
CancellationTokenK \
ct] _
=` a
defaultb i
)i j
;j k
Task		 
<		 
List		 
<		 
HealthRecord		 
>		 
>		  %
GetRecordsByDoctorIdAsync		! :
(		: ;
int		; >
doctorId		? G
,		G H
CancellationToken		I Z
ct		[ ]
=		^ _
default		` g
)		g h
;		h i
Task

 
<

 
List

 
<

 
HealthRecord

 
>

 
>

  (
GetRecordsByPatientNameAsync

! =
(

= >
string

> D
patientName

E P
,

P Q
CancellationToken

R c
ct

d f
=

g h
default

i p
)

p q
;

q r
Task 
< 
List 
< 
HealthRecord 
> 
>  '
GetRecordsByDoctorNameAsync! <
(< =
string= C

doctorNameD N
,N O
CancellationTokenP a
ctb d
=e f
defaultg n
)n o
;o p
Task 
< 
HealthRecord 
? 
> #
GetByAppointmentIdAsync 3
(3 4
int4 7
appointmentId8 E
,E F
CancellationTokenG X
ctY [
=\ ]
default^ e
)e f
;f g
Task 
< 
bool 
> %
ExistsForAppointmentAsync ,
(, -
int- 0
appointmentId1 >
,> ?
CancellationToken@ Q
ctR T
=U V
defaultW ^
)^ _
;_ `
Task 
< 
List 
<  
DoctorPatientListDto &
>& '
>' ("
GetDoctorPatientsAsync) ?
(? @
int@ C
doctorIdD L
)L M
;M N
Task 
< 
List 
< 
HealthRecord 
> 
>  +
GetRecordsForDoctorPatientAsync! @
(@ A
intB E
doctorIdF N
,N O
intO R
	patientIdS \
)\ ]
;] ^
} 
} ó
[C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\IDoctorRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
{ 
public 

	interface 
IDoctorRepository &
:& '
IRepository( 3
<3 4
Doctor4 :
>: ;
{ 
Task 
< 
List 
< 
Doctor 
> 
> 
SearchByNameAsync ,
(, -
string- 3
name4 8
,8 9
CancellationToken: K
ctL N
=O P
defaultQ X
)X Y
;Y Z
Task 
< 
List 
< 
Doctor 
> 
> !
GetActiveDoctorsAsync 0
(0 1
CancellationToken1 B
ctC E
=F G
defaultH O
)O P
;P Q
Task		 
<		 
List		 
<		 
Doctor		 
>		 
>		 '
SearchBySpecialisationAsync		 6
(		6 7
string		7 =
specialisation		> L
,		L M
CancellationToken		N _
ct		` b
=		c d
default		e l
)		l m
;		m n
Task

 
<

 
List

 
<

 
Doctor

 
>

 
>

 
SearchAsync

 &
(

& '
string

' -
query

. 3
,

3 4
CancellationToken

5 F
ct

G I
=

J K
default

L S
)

S T
;

T U
Task 
< 
Doctor 
? 
> 
GetByUserIdAsync &
(& '
string' -
userId. 4
)4 5
;5 6
Task 
< 
List 
< 
Doctor 
> 
> 
FilterAsync &
(& '
string' -
?- .
name/ 3
,3 4
string5 ;
?; <
specialization= K
)K L
;L M
} 
} ‰
`C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Repositories\IAppointmentRepository.cs
	namespace 	
HealthAxisApplicn
 
. 
Repositories (
{ 
public 

	interface "
IAppointmentRepository +
:+ ,
IRepository- 8
<8 9
Appointment9 D
>D E
{ 
Task 
< 
List 
< 
Appointment 
> 
> +
GetAppointmentsByPatientIdAsync  ?
(? @
int@ C
	patientIdD M
,M N
intO R
pageS W
,W X
intY \
pageSize] e
,e f
CancellationTokeng x
cty {
=| }
default	~ Ö
)
Ö Ü
;
Ü á
Task		 
<		 
List		 
<		 
Appointment		 
>		 
>		 2
&GetUpcomingAppointmentsByDoctorIdAsync		  F
(		F G
int		G J
doctorId		K S
,		S T
int		U X
page		Y ]
,		] ^
int		_ b
pageSize		c k
,		k l
CancellationToken		m ~
ct			 Å
=
		Ç É
default
		Ñ ã
)
		ã å
;
		å ç
Task

 
<

 
List

 
<

 
Appointment

 
>

 
>

 -
!GetAppointmentsByPatientNameAsync

  A
(

A B
string

B H
patientName

I T
,

T U
CancellationToken

V g
ct

h j
=

k l
default

m t
)

t u
;

u v
Task 
< 
List 
< 
Appointment 
> 
> ,
 GetAppointmentsByDoctorNameAsync  @
(@ A
stringA G

doctorNameH R
,R S
CancellationTokenT e
ctf h
=i j
defaultk r
)r s
;s t
Task 
< 
bool 
> "
DoctorHasConflictAsync )
() *
int* -
doctorId. 6
,6 7
DateTime8 @
dateA E
,E F
stringG M
timeSlotN V
,V W
CancellationTokenX i
ctj l
=m n
defaulto v
)v w
;w x
Task 
< 
bool 
> #
PatientHasConflictAsync *
(* +
int+ .
	patientId/ 8
,8 9
DateTime: B
dateC G
,G H
stringI O
timeSlotP X
,X Y
CancellationTokenZ k
ctl n
=o p
defaultq x
)x y
;y z
Task 
< 
bool 
> ,
 PatientHasAppointmentOnDateAsync 3
(3 4
int4 7
	patientId8 A
,A B
DateTimeC K
dateL P
,P Q
CancellationTokenR c
ctd f
=g h
defaulti p
)p q
;q r
Task 
< 
List 
< 
Appointment 
> 
> %
GetTodayAppointmentsAsync  9
(9 :
int: =
doctorId> F
,F G
intH K
pageL P
,P Q
intR U
pageSizeV ^
,^ _
CancellationToken` q
ctr t
=u v
defaultw ~
)~ 
;	 Ä
Task 
< 
List 
< 
Appointment 
> 
> #
GetAllAppointmentsAsync  7
(7 8
int8 ;
page< @
,@ A
intB E
pageSizeF N
)N O
;O P
} 
} Î∞
DC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Program.cs
Log 
. 
Logger 

= 
new 
LoggerConfiguration $
($ %
)% &
. 
WriteTo 
. 
Console 
( 
) 
. 
WriteTo 
. 
File 
( 
$str !
,! "
rollingInterval# 2
:2 3
RollingInterval4 C
.C D
DayD G
)G H
. 
CreateLogger 
( 
) 
; 
SelfLog   
.   
Enable   
(   
msg   
=>   
Console   
.   
	WriteLine   '
(  ' (
msg  ( +
)  + ,
)  , -
;  - .
var!! 
builder!! 
=!! 
WebApplication!! 
.!! 
CreateBuilder!! *
(!!* +
args!!+ /
)!!/ 0
;!!0 1
builder"" 
."" 
Host"" 
."" 

UseSerilog"" 
("" 
("" 
context""  
,""  !
services""" *
,""* +
configuration"", 9
)""9 :
=>""; =
{## 
configuration$$ 
.$$ 
ReadFrom$$ 
.$$ 
Configuration$$ (
($$( )
context$$) 0
.$$0 1
Configuration$$1 >
)$$> ?
.&& 	
ReadFrom&&	 
.&& 
Services&& 
(&& 
services&& #
)&&# $
.(( 	
Enrich((	 
.(( 
FromLogContext(( 
((( 
)((  
.** 	
WriteTo**	 
.** 
Console** 
(** 
)** 
.,, 	
WriteTo,,	 
.,, 
File,, 
(,, 
$str-- "
,--" #
rollingInterval.. 
:.. 
RollingInterval.. ,
..., -
Day..- 0
,..0 1"
retainedFileCountLimit// "
://" #
$num//$ %
)//% &
.11 	
WriteTo11	 
.11 
Elasticsearch11 
(11 
new22 $
ElasticsearchSinkOptions22 $
(22$ %
new33 
Uri33 
(33 
$str33 '
)33' (
)33( )
{44  
AutoRegisterTemplate55 
=55 
true55 #
,55# $
IndexFormat66 
=66 
$str66 3
}77 
)77 
;77 
}88 
)88 
;88 !
JwtSecurityTokenHandler:: 
.:: &
DefaultInboundClaimTypeMap:: 2
.::2 3
Clear::3 8
(::8 9
)::9 :
;::: ;
builder>> 
.>> 
Services>> 
.>> 
AddControllers>> 
(>>  
)>>  !
.?? 
AddJsonOptions?? 
(?? 
options?? 
=>?? 
{@@ 
optionsAA 
.AA !
JsonSerializerOptionsAA %
.AA% & 
PropertyNamingPolicyAA& :
=AA; <
JsonNamingPolicyAA= M
.AAM N
	CamelCaseAAN W
;AAW X
}BB 
)BB 
;BB 
builderCC 
.CC 
ServicesCC 
.CC 
AddExceptionHandlerCC $
<CC$ %"
GlobalExceptionHandlerCC% ;
>CC; <
(CC< =
)CC= >
;CC> ?
builderDD 
.DD 
ServicesDD 
.DD 
AddProblemDetailsDD "
(DD" #
)DD# $
;DD$ %
builderFF 
.FF 
ServicesFF 
.FF 
AddDbContextFF 
<FF 
AppDbContextFF *
>FF* +
(FF+ ,
optionFF, 2
=>FF3 5
{GG 
optionHH 

.HH
 
UseSqlServerHH 
(HH 
builderHH 
.HH  
ConfigurationHH  -
.HH- .
GetConnectionStringHH. A
(HHA B
$strHHB I
)HHI J
)HHJ K
;HHK L
}II 
)II 
;II 
builderKK 
.KK 
ServicesKK 
.KK 
AddIdentityKK 
<KK 
ApplicationUserKK ,
,KK, -
IdentityRoleKK. :
>KK: ;
(KK; <
optionsKK< C
=>KKD F
{LL 
optionsMM 
.MM 
UserMM 
.MM 
RequireUniqueEmailMM #
=MM$ %
trueMM& *
;MM* +
optionsNN 
.NN 
PasswordNN 
.NN 
RequireDigitNN !
=NN" #
trueNN$ (
;NN( )
optionsOO 
.OO 
PasswordOO 
.OO 
RequireUppercaseOO %
=OO& '
trueOO( ,
;OO, -
optionsPP 
.PP 
PasswordPP 
.PP 
RequiredLengthPP #
=PP$ %
$numPP& '
;PP' (
optionsQQ 
.QQ 
PasswordQQ 
.QQ "
RequireNonAlphanumericQQ +
=QQ, -
trueQQ. 2
;QQ2 3
optionsRR 
.RR 
PasswordRR 
.RR 
RequireLowercaseRR %
=RR& '
trueRR( ,
;RR, -
}TT 
)TT 
.TT $
AddEntityFrameworkStoresTT 
<TT 
AppDbContextTT (
>TT( )
(TT) *
)TT* +
.TT+ ,$
AddDefaultTokenProvidersTT, D
(TTD E
)TTE F
;TTF G
builderVV 
.VV 
ServicesVV 
.VV &
ConfigureApplicationCookieVV +
(VV+ ,
optionsVV, 3
=>VV4 6
{WW 
optionsXX 
.XX 
EventsXX 
.XX 
OnRedirectToLoginXX $
=XX% &
contextXX' .
=>XX/ 1
{YY 
contextZZ 
.ZZ 
ResponseZZ 
.ZZ 

StatusCodeZZ #
=ZZ$ %
$numZZ& )
;ZZ) *
return[[ 
Task[[ 
.[[ 
CompletedTask[[ !
;[[! "
}\\ 
;\\ 
options^^ 
.^^ 
Events^^ 
.^^ $
OnRedirectToAccessDenied^^ +
=^^, -
context^^. 5
=>^^6 8
{__ 
context`` 
.`` 
Response`` 
.`` 

StatusCode`` #
=``$ %
$num``& )
;``) *
returnaa 
Taskaa 
.aa 
CompletedTaskaa !
;aa! "
}bb 
;bb 
}cc 
)cc 
;cc 
builderee 
.ee 
Servicesee 
.ee 
AddAuthenticationee "
(ee" #
optionsee# *
=>ee+ -
{ff 
optionsgg 
.gg %
DefaultAuthenticateSchemegg %
=gg& '
JwtBearerDefaultsgg( 9
.gg9 : 
AuthenticationSchemegg: N
;ggN O
optionshh 
.hh "
DefaultChallengeSchemehh "
=hh# $
JwtBearerDefaultshh% 6
.hh6 7 
AuthenticationSchemehh7 K
;hhK L
}ii 
)ii 
.jj 
AddJwtBearerjj 
(jj 
optionjj 
=>jj 
{kk 
varll 
jwtll 
=ll 
builderll 
.ll 
Configurationll #
.ll# $

GetSectionll$ .
(ll. /
$strll/ 4
)ll4 5
;ll5 6
optionnn 

.nn
 %
TokenValidationParametersnn $
=nn% &
newnn' *%
TokenValidationParametersnn+ D
{oo 
ValidateIssuerpp 
=pp 
truepp 
,pp 
ValidIssuerqq 
=qq 
jwtqq 
[qq 
$strqq "
]qq" #
,qq# $
ValidateAudiencerr 
=rr 
truerr 
,rr  
ValidAudiencess 
=ss 
jwtss 
[ss 
$strss &
]ss& '
,ss' (
ValidateLifetimett 
=tt 
truett 
,tt  $
ValidateIssuerSigningKeyuu  
=uu! "
trueuu# '
,uu' (
IssuerSigningKeyvv 
=vv 
newvv  
SymmetricSecurityKeyvv 3
(vv3 4
Encodingww 
.ww 
UTF8ww 
.ww 
GetBytesww "
(ww" #
jwtww# &
[ww& '
$strww' ,
]ww, -
!ww- .
)ww. /
)xx 	
,xx	 

	ClockSkewyy 
=yy 
TimeSpanyy 
.yy 
Zeroyy !
,yy! "
RoleClaimType{{ 
={{ 

ClaimTypes{{ "
.{{" #
Role{{# '
}|| 
;|| 
}}} 
)}} 
;}} 
builder~~ 
.~~ 
Services~~ 
.~~ 
AddAuthorization~~ !
(~~! "
options~~" )
=>~~* ,
{ 
options
ÄÄ 
.
ÄÄ 
	AddPolicy
ÄÄ 
(
ÄÄ 
$str
ÄÄ &
,
ÄÄ& '
policy
ÄÄ( .
=>
ÄÄ/ 1
policy
ÅÅ 
.
ÅÅ 
RequireRole
ÅÅ 
(
ÅÅ 
$str
ÅÅ "
,
ÅÅ" #
$str
ÅÅ$ -
)
ÅÅ- .
)
ÅÅ. /
;
ÅÅ/ 0
}ÇÇ 
)
ÇÇ 
;
ÇÇ 
builderÖÖ 
.
ÖÖ 
Services
ÖÖ 
.
ÖÖ 
AddSwaggerGen
ÖÖ 
(
ÖÖ 
options
ÖÖ &
=>
ÖÖ' )
{ÜÜ 
options
áá 
.
áá 

SwaggerDoc
áá 
(
áá 
$str
áá 
,
áá 
new
áá  
OpenApiInfo
áá! ,
{
àà 
Title
ââ 
=
ââ 
$str
ââ  
,
ââ  !
Version
ää 
=
ää 
$str
ää 
}
ãã 
)
ãã 
;
ãã 
options
çç 
.
çç #
AddSecurityDefinition
çç !
(
çç! "
$str
çç" *
,
çç* +
new
çç, /#
OpenApiSecurityScheme
çç0 E
{
éé 
Name
èè 
=
èè 
$str
èè 
,
èè 
Type
êê 
=
êê  
SecuritySchemeType
êê !
.
êê! "
Http
êê" &
,
êê& '
Scheme
ëë 
=
ëë 
$str
ëë 
,
ëë 
BearerFormat
íí 
=
íí 
$str
íí 
,
íí 
In
ìì 

=
ìì 
ParameterLocation
ìì 
.
ìì 
Header
ìì %
,
ìì% &
Description
îî 
=
îî 
$str
îî .
}
ïï 
)
ïï 
;
ïï 
options
òò 
.
òò $
AddSecurityRequirement
òò "
(
òò" #
document
òò# +
=>
òò, .
new
ôô (
OpenApiSecurityRequirement
ôô	 #
{
öö 
[
õõ	 

new
õõ
 ,
OpenApiSecuritySchemeReference
õõ ,
(
õõ, -
$str
õõ- 5
,
õõ5 6
document
ùù 
)
ùù 
]
ùù 
=
ùù 
new
ùù 
List
ùù "
<
ùù" #
string
ùù# )
>
ùù) *
(
ùù* +
)
ùù+ ,
}
ûû 
)
ûû 
;
ûû 
}†† 
)
†† 
;
†† 
builder§§ 
.
§§ 
Services
§§ 
.
§§ 
	AddScoped
§§ 
<
§§  
IPatientRepository
§§ -
,
§§- .
PatientRepository
§§/ @
>
§§@ A
(
§§A B
)
§§B C
;
§§C D
builder•• 
.
•• 
Services
•• 
.
•• 
	AddScoped
•• 
<
•• 
IPatientService
•• *
,
••* +
PatientService
••, :
>
••: ;
(
••; <
)
••< =
;
••= >
builder¶¶ 
.
¶¶ 
Services
¶¶ 
.
¶¶ 
	AddScoped
¶¶ 
<
¶¶ 
IDoctorRepository
¶¶ ,
,
¶¶, -
DoctorRepository
¶¶. >
>
¶¶> ?
(
¶¶? @
)
¶¶@ A
;
¶¶A B
builderßß 
.
ßß 
Services
ßß 
.
ßß 
	AddScoped
ßß 
<
ßß 
IDoctorService
ßß )
,
ßß) *
DoctorService
ßß+ 8
>
ßß8 9
(
ßß9 :
)
ßß: ;
;
ßß; <
builder®® 
.
®® 
Services
®® 
.
®® 
	AddScoped
®® 
<
®® $
IAppointmentRepository
®® 1
,
®®1 2#
AppointmentRepository
®®3 H
>
®®H I
(
®®I J
)
®®J K
;
®®K L
builder©© 
.
©© 
Services
©© 
.
©© 
	AddScoped
©© 
<
©© !
IAppointmentService
©© .
,
©©. / 
AppointmentService
©©0 B
>
©©B C
(
©©C D
)
©©D E
;
©©E F
builder™™ 
.
™™ 
Services
™™ 
.
™™ 
	AddScoped
™™ 
<
™™ %
IHealthRecordRepository
™™ 2
,
™™2 3$
HealthRecordRepository
™™4 J
>
™™J K
(
™™K L
)
™™L M
;
™™M N
builder´´ 
.
´´ 
Services
´´ 
.
´´ 
	AddScoped
´´ 
<
´´ "
IHealthRecordService
´´ /
,
´´/ 0!
HealthRecordService
´´1 D
>
´´D E
(
´´E F
)
´´F G
;
´´G H
builder¨¨ 
.
¨¨ 
Services
¨¨ 
.
¨¨ 
	AddScoped
¨¨ 
<
¨¨ 
IAuthService
¨¨ '
,
¨¨' (
AuthService
¨¨) 4
>
¨¨4 5
(
¨¨5 6
)
¨¨6 7
;
¨¨7 8
builder≠≠ 
.
≠≠ 
Services
≠≠ 
.
≠≠ 
AddHostedService
≠≠ !
<
≠≠! "
HeartbeatService
≠≠" 2
>
≠≠2 3
(
≠≠3 4
)
≠≠4 5
;
≠≠5 6
builderÆÆ 
.
ÆÆ 
Services
ÆÆ 
.
ÆÆ 
AddSingleton
ÆÆ 
<
ÆÆ !
GarnetHostedService
ÆÆ 1
>
ÆÆ1 2
(
ÆÆ2 3
)
ÆÆ3 4
;
ÆÆ4 5
builderØØ 
.
ØØ 
Services
ØØ 
.
ØØ 
AddHostedService
ØØ !
(
ØØ! "
sp
ØØ" $
=>
ØØ% '
sp
ØØ( *
.
ØØ* + 
GetRequiredService
ØØ+ =
<
ØØ= >!
GarnetHostedService
ØØ> Q
>
ØØQ R
(
ØØR S
)
ØØS T
)
ØØT U
;
ØØU V
builder∞∞ 
.
∞∞ 
Services
∞∞ 
.
∞∞ (
AddStackExchangeRedisCache
∞∞ +
(
∞∞+ ,
options
∞∞, 3
=>
∞∞4 6
options
±± 
.
±± 
Configuration
±± 
=
±± 
$str
±± ,
)
±±, -
;
±±- .
builder≥≥ 
.
≥≥ 
Services
≥≥ 
.
≥≥ 
AddAutoMapper
≥≥ 
(
≥≥ 
cfg
≥≥ "
=>
≥≥# %
{¥¥ 
cfg
µµ 
.
µµ 

AddProfile
µµ 
<
µµ 
MappingProfile
µµ !
>
µµ! "
(
µµ" #
)
µµ# $
;
µµ$ %
}∂∂ 
)
∂∂ 
;
∂∂ 
builderππ 
.
ππ 
Services
ππ 
.
ππ 
AddCors
ππ 
(
ππ 
options
ππ  
=>
ππ! #
{∫∫ 
options
ªª 
.
ªª 
	AddPolicy
ªª 
(
ªª 
$str
ªª %
,
ªª% &
policy
ºº 
=>
ºº 
{
ΩΩ 	
policy
ææ 
.
ææ 
WithOrigins
ææ 
(
ææ 
$str
øø ,
,
øø, -
$str
¿¿ ,
,
¿¿, -
$str
¡¡ +
)
¬¬ 
.
√√ 
AllowAnyHeader
√√ 
(
√√  
)
√√  !
.
ƒƒ 
AllowAnyMethod
ƒƒ 
(
ƒƒ  
)
ƒƒ  !
;
ƒƒ! "
}
≈≈ 	
)
≈≈	 

;
≈≈
 
}∆∆ 
)
∆∆ 
;
∆∆ 
var»» 
rabbitMqSettings
»» 
=
»» 
builder
…… 
.
…… 
Configuration
…… 
.
…… 

GetSection
…… $
(
……$ %
$str
……% /
)
……/ 0
;
……0 1
varÀÀ 
host
ÀÀ 
=
ÀÀ	 

rabbitMqSettings
ÃÃ 
[
ÃÃ 
$str
ÃÃ 
]
ÃÃ 
;
ÃÃ 
varŒŒ 
virtualHost
ŒŒ 
=
ŒŒ 
rabbitMqSettings
œœ 
[
œœ 
$str
œœ "
]
œœ" #
;
œœ# $
var—— 
username
—— 
=
—— 
rabbitMqSettings
““ 
[
““ 
$str
““ 
]
““  
;
““  !
var‘‘ 
password
‘‘ 
=
‘‘ 
rabbitMqSettings
’’ 
[
’’ 
$str
’’ 
]
’’  
;
’’  !
var◊◊ 
	queueName
◊◊ 
=
◊◊ 
rabbitMqSettings
ÿÿ 
[
ÿÿ 
$str
ÿÿ  
]
ÿÿ  !
;
ÿÿ! "
builder⁄⁄ 
.
⁄⁄ 
Services
⁄⁄ 
.
⁄⁄ 
AddMassTransit
⁄⁄ 
(
⁄⁄  
x
⁄⁄  !
=>
⁄⁄" $
{€€ 
x
‹‹ 
.
‹‹ 
AddConsumer
‹‹ 
<
‹‹ %
BookAppointmentConsumer
‹‹ )
>
‹‹) *
(
‹‹* +
)
‹‹+ ,
;
‹‹, -
x
ﬁﬁ 
.
ﬁﬁ 
UsingRabbitMq
ﬁﬁ 
(
ﬁﬁ 
(
ﬁﬁ 
context
ﬁﬁ 
,
ﬁﬁ 
cfg
ﬁﬁ !
)
ﬁﬁ! "
=>
ﬁﬁ# %
{
ﬂﬂ 
cfg
‡‡ 
.
‡‡ 
Host
‡‡ 
(
‡‡ 
host
‡‡ 
!
‡‡ 
,
‡‡ 
virtualHost
‡‡ #
!
‡‡# $
,
‡‡$ %
h
‡‡& '
=>
‡‡( *
{
·· 	
h
‚‚ 
.
‚‚ 
Username
‚‚ 
(
‚‚ 
username
‚‚ 
!
‚‚  
)
‚‚  !
;
‚‚! "
h
„„ 
.
„„ 
Password
„„ 
(
„„ 
password
„„ 
!
„„  
)
„„  !
;
„„! "
}
‰‰ 	
)
‰‰	 

;
‰‰
 
cfg
ÊÊ 
.
ÊÊ 
ReceiveEndpoint
ÊÊ 
(
ÊÊ 
	queueName
ÊÊ %
!
ÊÊ% &
,
ÊÊ& '
e
ÊÊ( )
=>
ÊÊ* ,
{
ÁÁ 	
e
ËË 
.
ËË 
ConfigureConsumer
ËË 
<
ËË  %
BookAppointmentConsumer
ËË  7
>
ËË7 8
(
ËË8 9
context
ÈÈ 
)
ÈÈ 
;
ÈÈ 
}
ÍÍ 	
)
ÍÍ	 

;
ÍÍ
 
}
ÎÎ 
)
ÎÎ 
;
ÎÎ 
}ÏÏ 
)
ÏÏ 
;
ÏÏ 
builderÓÓ 
.
ÓÓ 
Services
ÓÓ 
.
ÓÓ 
AddSingleton
ÓÓ 
<
ÓÓ $
IConnectionMultiplexer
ÓÓ 4
>
ÓÓ4 5
(
ÓÓ5 6
sp
ÓÓ6 8
=>
ÓÓ9 ;
{ÔÔ 
return
 
#
ConnectionMultiplexer
  
.
  !
Connect
! (
(
( )
$str
) 9
)
9 :
;
: ;
}ÒÒ 
)
ÒÒ 
;
ÒÒ 
varÛÛ 
app
ÛÛ 
=
ÛÛ 	
builder
ÛÛ
 
.
ÛÛ 
Build
ÛÛ 
(
ÛÛ 
)
ÛÛ 
;
ÛÛ 
usingÙÙ 
(
ÙÙ 
var
ÙÙ 

scope
ÙÙ 
=
ÙÙ 
app
ÙÙ 
.
ÙÙ 
Services
ÙÙ 
.
ÙÙ  
CreateScope
ÙÙ  +
(
ÙÙ+ ,
)
ÙÙ, -
)
ÙÙ- .
{ıı 
var
ˆˆ 
services
ˆˆ 
=
ˆˆ 
scope
ˆˆ 
.
ˆˆ 
ServiceProvider
ˆˆ (
;
ˆˆ( )
var
¯¯ 
roleManager
¯¯ 
=
¯¯ 
services
¯¯ 
.
¯¯  
GetRequiredService
¯¯ 1
<
¯¯1 2
RoleManager
¯¯2 =
<
¯¯= >
IdentityRole
¯¯> J
>
¯¯J K
>
¯¯K L
(
¯¯L M
)
¯¯M N
;
¯¯N O
var
˘˘ 
userManager
˘˘ 
=
˘˘ 
services
˘˘ 
.
˘˘  
GetRequiredService
˘˘ 1
<
˘˘1 2
UserManager
˘˘2 =
<
˘˘= >
ApplicationUser
˘˘> M
>
˘˘M N
>
˘˘N O
(
˘˘O P
)
˘˘P Q
;
˘˘Q R
await
˙˙ 	

RoleSeeder
˙˙
 
.
˙˙ 
SeedRoleAsync
˙˙ "
(
˙˙" #
roleManager
˙˙# .
)
˙˙. /
;
˙˙/ 0
await
˚˚ 	

RoleSeeder
˚˚
 
.
˚˚ 
SeedAdminAsync
˚˚ #
(
˚˚# $
userManager
˚˚$ /
,
˚˚/ 0
roleManager
˚˚1 <
)
˚˚< =
;
˚˚= >
}¸¸ 
ifÄÄ 
(
ÄÄ 
app
ÄÄ 
.
ÄÄ 
Environment
ÄÄ 
.
ÄÄ 
IsDevelopment
ÄÄ !
(
ÄÄ! "
)
ÄÄ" #
)
ÄÄ# $
{ÅÅ 
app
ÇÇ 
.
ÇÇ 

UseSwagger
ÇÇ 
(
ÇÇ 
)
ÇÇ 
;
ÇÇ 
app
ÉÉ 
.
ÉÉ 
UseSwaggerUI
ÉÉ 
(
ÉÉ 
)
ÉÉ 
;
ÉÉ 
}ÑÑ 
appÜÜ 
.
ÜÜ !
UseExceptionHandler
ÜÜ 
(
ÜÜ 
)
ÜÜ 
;
ÜÜ 
appàà 
.
àà &
UseSerilogRequestLogging
àà 
(
àà 
)
àà 
;
àà 
appää 
.
ää !
UseHttpsRedirection
ää 
(
ää 
)
ää 
;
ää 
appåå 
.
åå 
UseCors
åå 
(
åå 
$str
åå 
)
åå 
;
åå 
appçç 
.
çç 
UseAuthentication
çç 
(
çç 
)
çç 
;
çç 
appéé 
.
éé 
UseAuthorization
éé 
(
éé 
)
éé 
;
éé 
appêê 
.
êê 
MapControllers
êê 
(
êê 
)
êê 
;
êê 
appíí 
.
íí 
Run
íí 
(
íí 
)
íí 	
;
íí	 
®
PC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Models\RefreshToken.cs
	namespace 	
HealthAxisApplicn
 
. 
Models "
{ 
public 

class 
RefreshToken 
{		 
public

 
int

 
Id

 
{

 
get

 
;

 
set

  
;

  !
}

" #
public 
string 
Token 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
DateTime 
Expires 
{  !
get" %
;% &
set' *
;* +
}, -
public 
bool 
	IsRevoked 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
public 
ApplicationUser 
User #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
null4 8
!8 9
;9 :
} 
} ﬂ
KC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Models\Patient.cs
	namespace 	
HealthAxisApplicn
 
. 
Models "
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
[

 	
Required

	 
]

 
[ 	
RegularExpression	 
( 
$str 0
,0 1
ErrorMessage2 >
=? @
$strA p
)p q
]q r
[ 	
	MinLength	 
( 
$num 
) 
] 
public 
string 
PatientName !
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
;> ?
[ 	
Required	 
] 
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
] 
[ 	
RegularExpression	 
( 
$str ?
,? @
ErrorMessageA M
=N O
$strP `
)` a
]a b
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
[ 	
RegularExpression	 
( 
$str +
,+ ,
ErrorMessage- 9
=: ;
$str< w
)w x
]x y
public 
string 
PhoneNo 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
[ 	
RegularExpression	 
( 
$str +
)+ ,
], -
public 
string 
? 
InsuranceID "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
Required	 
] 
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
[   	
Required  	 
]   
public!! 
string!! 
UserId!! 
{!! 
get!! "
;!!" #
set!!$ '
;!!' (
}!!) *
=!!+ ,
string!!- 3
.!!3 4
Empty!!4 9
;!!9 :
["" 	

ForeignKey""	 
("" 
$str"" 
)"" 
]"" 
public## 
ApplicationUser## 
User## #
{##$ %
get##& )
;##) *
set##+ .
;##. /
}##0 1
=##2 3
null##4 8
!##8 9
;##9 :
}%% 
}&& ¸
PC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Models\HealthRecord.cs
	namespace 	
HealthAxisApplicn
 
. 
Models "
{ 
public 

class 
HealthRecord 
{ 
[ 	
Key	 
] 
public		 
int		 
HealthRecordId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
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
[ 	

ForeignKey	 
( 
nameof 
( 
	PatientId $
)$ %
)% &
]& '
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	

ForeignKey	 
( 
nameof 
( 
DoctorId #
)# $
)$ %
]% &
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	

ForeignKey	 
( 
nameof 
( 
AppointmentId (
)( )
)) *
]* +
public 
Appointment 
Appointment &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
null7 ;
!; <
;< =
public 
DateTime 
	VisitDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
? 
	Diagnosis  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
string1 7
.7 8
Empty8 =
;= >
public 
string 
? 
Prescription #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
string4 :
.: ;
Empty; @
;@ A
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ¢
JC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Models\Doctor.cs
	namespace 	
HealthAxisApplicn
 
. 
Models "
{ 
public 

class 
Doctor 
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
DoctorId
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
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ;
); <
]< =
[ 	
RegularExpression	 
( 
$str .
,. /
ErrorMessage0 <
== >
$str? c
)c d
]d e
[ 	
	MinLength	 
( 
$num 
) 
] 
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* U
)U V
]V W
public 
string 

DoctorName  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
string1 7
.7 8
Empty8 =
;= >
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
] 
[ 	
RegularExpression	 
( 
$str	 ≠
)
≠ Æ
]
Æ Ø
public 
string 
Specialisation $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
string5 ;
.; <
Empty< A
;A B
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% Q
)Q R
]R S
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) N
)N O
]O P
[ 	
	Precision	 
( 
$num 
, 
$num 
) 
] 
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
[ 	
Required	 
] 
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
[   	
Required  	 
]   
public!! 
string!! 
UserId!! 
{!! 
get!! "
;!!" #
set!!$ '
;!!' (
}!!) *
=!!+ ,
string!!- 3
.!!3 4
Empty!!4 9
;!!9 :
[## 	

ForeignKey##	 
(## 
$str## 
)## 
]## 
public$$ 
ApplicationUser$$ 
User$$ #
{$$$ %
get$$& )
;$$) *
set$$+ .
;$$. /
}$$0 1
=$$2 3
null$$4 8
!$$8 9
;$$9 :
}&& 
}'' ¡
OC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Models\Appointment.cs
	namespace 	
HealthAxisApplicn
 
. 
Models "
{ 
public 

class 
Appointment 
{ 
[ 	
Key	 
] 
public		 
int		 
AppointmentId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
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
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	

ForeignKey	 
( 
nameof 
( 
	PatientId $
)$ %
)% &
]& '
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	

ForeignKey	 
( 
nameof 
( 
DoctorId #
)# $
)$ %
]% &
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
[ 	
Required	 
] 
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
RegularExpression	 
( 
$str D
)D E
]E F
public 
string 
Status 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
$str- 6
;6 7
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} Û
SC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Models\ApplicationUser.cs
	namespace 	
HealthAxisApplicn
 
. 
Models "
{ 
public 

class 
ApplicationUser  
:  !
IdentityUser" .
{ 
public 
bool 
IsFirstLogin  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
true1 5
;5 6
public 
Patient 
? 
Patient 
{  !
get" %
;% &
set' *
;* +
}, -
public		 
Doctor		 
?		 
Doctor		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
}

 
} ‰
wC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260703052316_NullableDiagnosisAndPrescription.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class ,
 NullableDiagnosisAndPrescription 9
:: ;
	Migration< E
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
. 
AlterColumn (
<( )
string) /
>/ 0
(0 1
name 
: 
$str $
,$ %
table 
: 
$str &
,& '
type 
: 
$str %
,% &
nullable 
: 
true 
, 

oldClrType 
: 
typeof "
(" #
string# )
)) *
,* +
oldType 
: 
$str (
)( )
;) *
migrationBuilder 
. 
AlterColumn (
<( )
string) /
>/ 0
(0 1
name 
: 
$str !
,! "
table 
: 
$str &
,& '
type 
: 
$str %
,% &
nullable 
: 
true 
, 

oldClrType 
: 
typeof "
(" #
string# )
)) *
,* +
oldType 
: 
$str (
)( )
;) *
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{   	
migrationBuilder!! 
.!! 
AlterColumn!! (
<!!( )
string!!) /
>!!/ 0
(!!0 1
name"" 
:"" 
$str"" $
,""$ %
table## 
:## 
$str## &
,##& '
type$$ 
:$$ 
$str$$ %
,$$% &
nullable%% 
:%% 
false%% 
,%%  
defaultValue&& 
:&& 
$str&&  
,&&  !

oldClrType'' 
:'' 
typeof'' "
(''" #
string''# )
)'') *
,''* +
oldType(( 
:(( 
$str(( (
,((( )
oldNullable)) 
:)) 
true)) !
)))! "
;))" #
migrationBuilder++ 
.++ 
AlterColumn++ (
<++( )
string++) /
>++/ 0
(++0 1
name,, 
:,, 
$str,, !
,,,! "
table-- 
:-- 
$str-- &
,--& '
type.. 
:.. 
$str.. %
,..% &
nullable// 
:// 
false// 
,//  
defaultValue00 
:00 
$str00  
,00  !

oldClrType11 
:11 
typeof11 "
(11" #
string11# )
)11) *
,11* +
oldType22 
:22 
$str22 (
,22( )
oldNullable33 
:33 
true33 !
)33! "
;33" #
}44 	
}55 
}66 Ô
yC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260630100659_IsFirstLoginAddedToApplicationUser.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class .
"IsFirstLoginAddedToApplicationUser ;
:< =
	Migration> G
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
	DropIndex &
(& '
name 
: 
$str 6
,6 7
table 
: 
$str &
)& '
;' (
migrationBuilder 
. 
	AddColumn &
<& '
bool' +
>+ ,
(, -
name 
: 
$str $
,$ %
table 
: 
$str $
,$ %
type 
: 
$str 
, 
nullable 
: 
false 
,  
defaultValue 
: 
false #
)# $
;$ %
migrationBuilder 
. 
CreateIndex (
(( )
name 
: 
$str 6
,6 7
table 
: 
$str &
,& '
column 
: 
$str '
,' (
unique 
: 
true 
) 
; 
} 	
	protected   
override   
void   
Down    $
(  $ %
MigrationBuilder  % 5
migrationBuilder  6 F
)  F G
{!! 	
migrationBuilder"" 
."" 
	DropIndex"" &
(""& '
name## 
:## 
$str## 6
,##6 7
table$$ 
:$$ 
$str$$ &
)$$& '
;$$' (
migrationBuilder&& 
.&& 

DropColumn&& '
(&&' (
name'' 
:'' 
$str'' $
,''$ %
table(( 
:(( 
$str(( $
)(($ %
;((% &
migrationBuilder** 
.** 
CreateIndex** (
(**( )
name++ 
:++ 
$str++ 6
,++6 7
table,, 
:,, 
$str,, &
,,,& '
column-- 
:-- 
$str-- '
)--' (
;--( )
}.. 	
}// 
}00 ∂
tC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260629094137_HealthRecordLinkToAppointment.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class )
HealthRecordLinkToAppointment 6
:7 8
	Migration9 B
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
<& '
int' *
>* +
(+ ,
name 
: 
$str %
,% &
table 
: 
$str &
,& '
type 
: 
$str 
, 
nullable 
: 
false 
,  
defaultValue 
: 
$num 
)  
;  !
migrationBuilder 
. 
CreateIndex (
(( )
name 
: 
$str 6
,6 7
table 
: 
$str &
,& '
column 
: 
$str '
)' (
;( )
migrationBuilder 
. 
AddForeignKey *
(* +
name 
: 
$str C
,C D
table 
: 
$str &
,& '
column 
: 
$str '
,' (
principalTable 
: 
$str  .
,. /
principalColumn 
:  
$str! 0
,0 1
onDelete 
: 
ReferentialAction +
.+ ,
Cascade, 3
)3 4
;4 5
}   	
	protected## 
override## 
void## 
Down##  $
(##$ %
MigrationBuilder##% 5
migrationBuilder##6 F
)##F G
{$$ 	
migrationBuilder%% 
.%% 
DropForeignKey%% +
(%%+ ,
name&& 
:&& 
$str&& C
,&&C D
table'' 
:'' 
$str'' &
)''& '
;''' (
migrationBuilder)) 
.)) 
	DropIndex)) &
())& '
name** 
:** 
$str** 6
,**6 7
table++ 
:++ 
$str++ &
)++& '
;++' (
migrationBuilder-- 
.-- 

DropColumn-- '
(--' (
name.. 
:.. 
$str.. %
,..% &
table// 
:// 
$str// &
)//& '
;//' (
}00 	
}11 
}22 ÎU
eC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260629000343_FixCascadeIssu.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{		 
public 

partial 
class 
FixCascadeIssu '
:( )
	Migration* 3
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
. 

DeleteData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
) 
; 
migrationBuilder 
. 

DeleteData '
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
) 
; 
migrationBuilder 
. 

DeleteData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
) 
; 
migrationBuilder 
. 

DeleteData '
(' (
table   
:   
$str   !
,  ! "
	keyColumn!! 
:!! 
$str!! &
,!!& '
keyValue"" 
:"" 
$num"" 
)"" 
;"" 
migrationBuilder$$ 
.$$ 

DeleteData$$ '
($$' (
table%% 
:%% 
$str%% !
,%%! "
	keyColumn&& 
:&& 
$str&& &
,&&& '
keyValue'' 
:'' 
$num'' 
)'' 
;'' 
migrationBuilder)) 
.)) 

DeleteData)) '
())' (
table** 
:** 
$str** !
,**! "
	keyColumn++ 
:++ 
$str++ &
,++& '
keyValue,, 
:,, 
$num,, 
),, 
;,, 
migrationBuilder.. 
... 
	AddColumn.. &
<..& '
string..' -
>..- .
(... /
name// 
:// 
$str// 
,// 
table00 
:00 
$str00 !
,00! "
type11 
:11 
$str11 %
,11% &
nullable22 
:22 
false22 
,22  
defaultValue33 
:33 
$str33  
)33  !
;33! "
migrationBuilder55 
.55 
	AddColumn55 &
<55& '
string55' -
>55- .
(55. /
name66 
:66 
$str66 
,66 
table77 
:77 
$str77  
,77  !
type88 
:88 
$str88 %
,88% &
nullable99 
:99 
false99 
,99  
defaultValue:: 
::: 
$str::  
)::  !
;::! "
migrationBuilder<< 
.<< 
CreateIndex<< (
(<<( )
name== 
:== 
$str== *
,==* +
table>> 
:>> 
$str>> !
,>>! "
column?? 
:?? 
$str??  
,??  !
unique@@ 
:@@ 
true@@ 
)@@ 
;@@ 
migrationBuilderBB 
.BB 
CreateIndexBB (
(BB( )
nameCC 
:CC 
$strCC )
,CC) *
tableDD 
:DD 
$strDD  
,DD  !
columnEE 
:EE 
$strEE  
,EE  !
uniqueFF 
:FF 
trueFF 
)FF 
;FF 
migrationBuilderHH 
.HH 
AddForeignKeyHH *
(HH* +
nameII 
:II 
$strII 5
,II5 6
tableJJ 
:JJ 
$strJJ  
,JJ  !
columnKK 
:KK 
$strKK  
,KK  !
principalTableLL 
:LL 
$strLL  -
,LL- .
principalColumnMM 
:MM  
$strMM! %
)MM% &
;MM& '
migrationBuilderOO 
.OO 
AddForeignKeyOO *
(OO* +
namePP 
:PP 
$strPP 6
,PP6 7
tableQQ 
:QQ 
$strQQ !
,QQ! "
columnRR 
:RR 
$strRR  
,RR  !
principalTableSS 
:SS 
$strSS  -
,SS- .
principalColumnTT 
:TT  
$strTT! %
)TT% &
;TT& '
}UU 	
	protectedXX 
overrideXX 
voidXX 
DownXX  $
(XX$ %
MigrationBuilderXX% 5
migrationBuilderXX6 F
)XXF G
{YY 	
migrationBuilderZZ 
.ZZ 
DropForeignKeyZZ +
(ZZ+ ,
name[[ 
:[[ 
$str[[ 5
,[[5 6
table\\ 
:\\ 
$str\\  
)\\  !
;\\! "
migrationBuilder^^ 
.^^ 
DropForeignKey^^ +
(^^+ ,
name__ 
:__ 
$str__ 6
,__6 7
table`` 
:`` 
$str`` !
)``! "
;``" #
migrationBuilderbb 
.bb 
	DropIndexbb &
(bb& '
namecc 
:cc 
$strcc *
,cc* +
tabledd 
:dd 
$strdd !
)dd! "
;dd" #
migrationBuilderff 
.ff 
	DropIndexff &
(ff& '
namegg 
:gg 
$strgg )
,gg) *
tablehh 
:hh 
$strhh  
)hh  !
;hh! "
migrationBuilderjj 
.jj 

DropColumnjj '
(jj' (
namekk 
:kk 
$strkk 
,kk 
tablell 
:ll 
$strll !
)ll! "
;ll" #
migrationBuildernn 
.nn 

DropColumnnn '
(nn' (
nameoo 
:oo 
$stroo 
,oo 
tablepp 
:pp 
$strpp  
)pp  !
;pp! "
migrationBuilderrr 
.rr 

InsertDatarr '
(rr' (
tabless 
:ss 
$strss  
,ss  !
columnstt 
:tt 
newtt 
[tt 
]tt 
{tt  
$strtt! +
,tt+ ,
$strtt- >
,tt> ?
$strtt@ L
,ttL M
$strttN U
,ttU V
$strttW a
,tta b
$strttc s
,tts t
$str	ttu à
}
ttâ ä
,
ttä ã
valuesuu 
:uu 
newuu 
objectuu "
[uu" #
,uu# $
]uu$ %
{vv 
{ww 
$numww 
,ww 
$numww 
,ww 
$strww  0
,ww0 1
$strww2 N
,wwN O
truewwP T
,wwT U
$strwwV d
,wwd e
$numwwf g
}wwh i
,wwi j
{xx 
$numxx 
,xx 
$numxx 
,xx 
$strxx  /
,xx/ 0
$strxx1 L
,xxL M
truexxN R
,xxR S
$strxxT e
,xxe f
$numxxg h
}xxi j
,xxj k
{yy 
$numyy 
,yy 
$numyy 
,yy 
$stryy  ,
,yy, -
$stryy. F
,yyF G
trueyyH L
,yyL M
$stryyN ]
,yy] ^
$numyy_ `
}yya b
}zz 
)zz 
;zz 
migrationBuilder|| 
.|| 

InsertData|| '
(||' (
table}} 
:}} 
$str}} !
,}}! "
columns~~ 
:~~ 
new~~ 
[~~ 
]~~ 
{~~  
$str~~! ,
,~~, -
$str~~. ;
,~~; <
$str~~= D
,~~D E
$str~~F N
,~~N O
$str~~P ]
,~~] ^
$str~~_ i
,~~i j
$str~~k x
,~~x y
$str	~~z É
}
~~Ñ Ö
,
~~Ö Ü
values 
: 
new 
object "
[" #
,# $
]$ %
{
ÄÄ 
{
ÅÅ 
$num
ÅÅ 
,
ÅÅ 
new
ÅÅ 
DateTime
ÅÅ %
(
ÅÅ% &
$num
ÅÅ& *
,
ÅÅ* +
$num
ÅÅ, .
,
ÅÅ. /
$num
ÅÅ0 1
,
ÅÅ1 2
$num
ÅÅ3 4
,
ÅÅ4 5
$num
ÅÅ6 7
,
ÅÅ7 8
$num
ÅÅ9 :
,
ÅÅ: ;
$num
ÅÅ< =
,
ÅÅ= >
DateTimeKind
ÅÅ? K
.
ÅÅK L
Unspecified
ÅÅL W
)
ÅÅW X
,
ÅÅX Y
$str
ÅÅZ k
,
ÅÅk l
$str
ÅÅm s
,
ÅÅs t
null
ÅÅu y
,
ÅÅy z
true
ÅÅ{ 
,ÅÅ Ä
$strÅÅÅ à
,ÅÅà â
$strÅÅä ñ
}ÅÅó ò
,ÅÅò ô
{
ÇÇ 
$num
ÇÇ 
,
ÇÇ 
new
ÇÇ 
DateTime
ÇÇ %
(
ÇÇ% &
$num
ÇÇ& *
,
ÇÇ* +
$num
ÇÇ, -
,
ÇÇ- .
$num
ÇÇ/ 0
,
ÇÇ0 1
$num
ÇÇ2 3
,
ÇÇ3 4
$num
ÇÇ5 6
,
ÇÇ6 7
$num
ÇÇ8 9
,
ÇÇ9 :
$num
ÇÇ; <
,
ÇÇ< =
DateTimeKind
ÇÇ> J
.
ÇÇJ K
Unspecified
ÇÇK V
)
ÇÇV W
,
ÇÇW X
$str
ÇÇY k
,
ÇÇk l
$str
ÇÇm s
,
ÇÇs t
null
ÇÇu y
,
ÇÇy z
true
ÇÇ{ 
,ÇÇ Ä
$strÇÇÅ â
,ÇÇâ ä
$strÇÇã ó
}ÇÇò ô
,ÇÇô ö
{
ÉÉ 
$num
ÉÉ 
,
ÉÉ 
new
ÉÉ 
DateTime
ÉÉ %
(
ÉÉ% &
$num
ÉÉ& *
,
ÉÉ* +
$num
ÉÉ, -
,
ÉÉ- .
$num
ÉÉ/ 0
,
ÉÉ0 1
$num
ÉÉ2 3
,
ÉÉ3 4
$num
ÉÉ5 6
,
ÉÉ6 7
$num
ÉÉ8 9
,
ÉÉ9 :
$num
ÉÉ; <
,
ÉÉ< =
DateTimeKind
ÉÉ> J
.
ÉÉJ K
Unspecified
ÉÉK V
)
ÉÉV W
,
ÉÉW X
$str
ÉÉY k
,
ÉÉk l
$str
ÉÉm s
,
ÉÉs t
null
ÉÉu y
,
ÉÉy z
true
ÉÉ{ 
,ÉÉ Ä
$strÉÉÅ â
,ÉÉâ ä
$strÉÉã ó
}ÉÉò ô
}
ÑÑ 
)
ÑÑ 
;
ÑÑ 
}
ÖÖ 	
}
ÜÜ 
}áá È 
gC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260625200256_DoctorEmailAdded.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class 
DoctorEmailAdded )
:* +
	Migration, 5
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
. 
AlterColumn (
<( )
string) /
>/ 0
(0 1
name 
: 
$str "
," #
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
,  

oldClrType 
: 
typeof "
(" #
string# )
)) *
,* +
oldType 
: 
$str (
)( )
;) *
migrationBuilder 
. 
	AddColumn &
<& '
string' -
>- .
(. /
name 
: 
$str 
, 
table 
: 
$str  
,  !
type 
: 
$str %
,% &
nullable 
: 
false 
,  
defaultValue 
: 
$str  
)  !
;! "
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue   
:   
$num   
,   
column!! 
:!! 
$str!! 
,!!  
value"" 
:"" 
$str"" 3
)""3 4
;""4 5
migrationBuilder$$ 
.$$ 

UpdateData$$ '
($$' (
table%% 
:%% 
$str%%  
,%%  !
	keyColumn&& 
:&& 
$str&& %
,&&% &
keyValue'' 
:'' 
$num'' 
,'' 
column(( 
:(( 
$str(( 
,((  
value)) 
:)) 
$str)) 2
)))2 3
;))3 4
migrationBuilder++ 
.++ 

UpdateData++ '
(++' (
table,, 
:,, 
$str,,  
,,,  !
	keyColumn-- 
:-- 
$str-- %
,--% &
keyValue.. 
:.. 
$num.. 
,.. 
column// 
:// 
$str// 
,//  
value00 
:00 
$str00 /
)00/ 0
;000 1
}11 	
	protected44 
override44 
void44 
Down44  $
(44$ %
MigrationBuilder44% 5
migrationBuilder446 F
)44F G
{55 	
migrationBuilder66 
.66 

DropColumn66 '
(66' (
name77 
:77 
$str77 
,77 
table88 
:88 
$str88  
)88  !
;88! "
migrationBuilder:: 
.:: 
AlterColumn:: (
<::( )
string::) /
>::/ 0
(::0 1
name;; 
:;; 
$str;; "
,;;" #
table<< 
:<< 
$str<<  
,<<  !
type== 
:== 
$str== %
,==% &
nullable>> 
:>> 
false>> 
,>>  

oldClrType?? 
:?? 
typeof?? "
(??" #
string??# )
)??) *
,??* +
oldType@@ 
:@@ 
$str@@ (
,@@( )
oldMaxLengthAA 
:AA 
$numAA !
)AA! "
;AA" #
}BB 	
}CC 
}DD ú
oC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260622113124_RemovedAuthResponseTable.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class $
RemovedAuthResponseTable 1
:2 3
	Migration4 =
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
} „
hC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260622101616_refreshTokenTable.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public		 

partial		 
class		 
refreshTokenTable		 *
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
,A B
Token 
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
,F G
nullableH P
:P Q
falseR W
)W X
,X Y
Expires 
= 
table #
.# $
Column$ *
<* +
DateTime+ 3
>3 4
(4 5
type5 9
:9 :
$str; F
,F G
nullableH P
:P Q
falseR W
)W X
,X Y
	IsRevoked 
= 
table  %
.% &
Column& ,
<, -
bool- 1
>1 2
(2 3
type3 7
:7 8
$str9 >
,> ?
nullable@ H
:H I
falseJ O
)O P
,P Q
UserId 
= 
table "
." #
Column# )
<) *
string* 0
>0 1
(1 2
type2 6
:6 7
$str8 G
,G H
nullableI Q
:Q R
falseS X
)X Y
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
;C D
table 
. 

ForeignKey $
($ %
name 
: 
$str C
,C D
column 
: 
x  !
=>" $
x% &
.& '
UserId' -
,- .
principalTable &
:& '
$str( 5
,5 6
principalColumn   '
:  ' (
$str  ) -
,  - .
onDelete!!  
:!!  !
ReferentialAction!!" 3
.!!3 4
Cascade!!4 ;
)!!; <
;!!< =
}"" 
)"" 
;"" 
migrationBuilder$$ 
.$$ 
CreateIndex$$ (
($$( )
name%% 
:%% 
$str%% /
,%%/ 0
table&& 
:&& 
$str&& &
,&&& '
column'' 
:'' 
$str''  
)''  !
;''! "
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
	DropTable-- &
(--& '
name.. 
:.. 
$str.. %
)..% &
;..& '
}// 	
}00 
}11 ä
fC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260622101235_refreshTokenNew.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class 
refreshTokenNew (
:) *
	Migration+ 4
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
cC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260622100810_refreshToken.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class 
refreshToken %
:& '
	Migration( 1
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
} ﬁV
fC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260618103308_UpdatedEntities.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class 
UpdatedEntities (
:) *
	Migration+ 4
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
. 
DropForeignKey +
(+ ,
name 
: 
$str 8
,8 9
table 
: 
$str %
)% &
;& '
migrationBuilder 
. 
DropForeignKey +
(+ ,
name 
: 
$str :
,: ;
table 
: 
$str %
)% &
;& '
migrationBuilder 
. 
DropForeignKey +
(+ ,
name 
: 
$str C
,C D
table 
: 
$str &
)& '
;' (
migrationBuilder 
. 
	DropIndex &
(& '
name 
: 
$str 6
,6 7
table 
: 
$str &
)& '
;' (
migrationBuilder 
. 

DropColumn '
(' (
name 
: 
$str %
,% &
table 
: 
$str &
)& '
;' (
migrationBuilder!! 
.!! 
RenameColumn!! )
(!!) *
name"" 
:"" 
$str"" !
,""! "
table## 
:## 
$str## %
,##% &
newName$$ 
:$$ 
$str$$ $
)$$$ %
;$$% &
migrationBuilder&& 
.&& 
RenameColumn&& )
(&&) *
name'' 
:'' 
$str''  
,''  !
table(( 
:(( 
$str(( %
,((% &
newName)) 
:)) 
$str)) #
)))# $
;))$ %
migrationBuilder++ 
.++ 
RenameColumn++ )
(++) *
name,, 
:,, 
$str,, %
,,,% &
table-- 
:-- 
$str-- %
,--% &
newName.. 
:.. 
$str.. (
)..( )
;..) *
migrationBuilder00 
.00 
RenameIndex00 (
(00( )
name11 
:11 
$str11 1
,111 2
table22 
:22 
$str22 %
,22% &
newName33 
:33 
$str33 4
)334 5
;335 6
migrationBuilder55 
.55 
RenameIndex55 (
(55( )
name66 
:66 
$str66 0
,660 1
table77 
:77 
$str77 %
,77% &
newName88 
:88 
$str88 3
)883 4
;884 5
migrationBuilder:: 
.:: 
AlterColumn:: (
<::( )
string::) /
>::/ 0
(::0 1
name;; 
:;; 
$str;; *
,;;* +
table<< 
:<< 
$str<< %
,<<% &
type== 
:== 
$str== %
,==% &
	maxLength>> 
:>> 
$num>> 
,>> 
nullable?? 
:?? 
true?? 
,?? 

oldClrType@@ 
:@@ 
typeof@@ "
(@@" #
string@@# )
)@@) *
,@@* +
oldTypeAA 
:AA 
$strAA (
,AA( )
oldMaxLengthBB 
:BB 
$numBB !
)BB! "
;BB" #
migrationBuilderDD 
.DD 
AddForeignKeyDD *
(DD* +
nameEE 
:EE 
$strEE 8
,EE8 9
tableFF 
:FF 
$strFF %
,FF% &
columnGG 
:GG 
$strGG "
,GG" #
principalTableHH 
:HH 
$strHH  )
,HH) *
principalColumnII 
:II  
$strII! +
,II+ ,
onDeleteJJ 
:JJ 
ReferentialActionJJ +
.JJ+ ,
CascadeJJ, 3
)JJ3 4
;JJ4 5
migrationBuilderLL 
.LL 
AddForeignKeyLL *
(LL* +
nameMM 
:MM 
$strMM :
,MM: ;
tableNN 
:NN 
$strNN %
,NN% &
columnOO 
:OO 
$strOO #
,OO# $
principalTablePP 
:PP 
$strPP  *
,PP* +
principalColumnQQ 
:QQ  
$strQQ! ,
,QQ, -
onDeleteRR 
:RR 
ReferentialActionRR +
.RR+ ,
CascadeRR, 3
)RR3 4
;RR4 5
}SS 	
	protectedVV 
overrideVV 
voidVV 
DownVV  $
(VV$ %
MigrationBuilderVV% 5
migrationBuilderVV6 F
)VVF G
{WW 	
migrationBuilderXX 
.XX 
DropForeignKeyXX +
(XX+ ,
nameYY 
:YY 
$strYY 8
,YY8 9
tableZZ 
:ZZ 
$strZZ %
)ZZ% &
;ZZ& '
migrationBuilder\\ 
.\\ 
DropForeignKey\\ +
(\\+ ,
name]] 
:]] 
$str]] :
,]]: ;
table^^ 
:^^ 
$str^^ %
)^^% &
;^^& '
migrationBuilder`` 
.`` 
RenameColumn`` )
(``) *
nameaa 
:aa 
$straa !
,aa! "
tablebb 
:bb 
$strbb %
,bb% &
newNamecc 
:cc 
$strcc $
)cc$ %
;cc% &
migrationBuilderee 
.ee 
RenameColumnee )
(ee) *
nameff 
:ff 
$strff  
,ff  !
tablegg 
:gg 
$strgg %
,gg% &
newNamehh 
:hh 
$strhh #
)hh# $
;hh$ %
migrationBuilderjj 
.jj 
RenameColumnjj )
(jj) *
namekk 
:kk 
$strkk %
,kk% &
tablell 
:ll 
$strll %
,ll% &
newNamemm 
:mm 
$strmm (
)mm( )
;mm) *
migrationBuilderoo 
.oo 
RenameIndexoo (
(oo( )
namepp 
:pp 
$strpp 1
,pp1 2
tableqq 
:qq 
$strqq %
,qq% &
newNamerr 
:rr 
$strrr 4
)rr4 5
;rr5 6
migrationBuildertt 
.tt 
RenameIndextt (
(tt( )
nameuu 
:uu 
$struu 0
,uu0 1
tablevv 
:vv 
$strvv %
,vv% &
newNameww 
:ww 
$strww 3
)ww3 4
;ww4 5
migrationBuilderyy 
.yy 
	AddColumnyy &
<yy& '
intyy' *
>yy* +
(yy+ ,
namezz 
:zz 
$strzz %
,zz% &
table{{ 
:{{ 
$str{{ &
,{{& '
type|| 
:|| 
$str|| 
,|| 
nullable}} 
:}} 
false}} 
,}}  
defaultValue~~ 
:~~ 
$num~~ 
)~~  
;~~  !
migrationBuilder
ÄÄ 
.
ÄÄ 
AlterColumn
ÄÄ (
<
ÄÄ( )
string
ÄÄ) /
>
ÄÄ/ 0
(
ÄÄ0 1
name
ÅÅ 
:
ÅÅ 
$str
ÅÅ *
,
ÅÅ* +
table
ÇÇ 
:
ÇÇ 
$str
ÇÇ %
,
ÇÇ% &
type
ÉÉ 
:
ÉÉ 
$str
ÉÉ %
,
ÉÉ% &
	maxLength
ÑÑ 
:
ÑÑ 
$num
ÑÑ 
,
ÑÑ 
nullable
ÖÖ 
:
ÖÖ 
false
ÖÖ 
,
ÖÖ  
defaultValue
ÜÜ 
:
ÜÜ 
$str
ÜÜ  
,
ÜÜ  !

oldClrType
áá 
:
áá 
typeof
áá "
(
áá" #
string
áá# )
)
áá) *
,
áá* +
oldType
àà 
:
àà 
$str
àà (
,
àà( )
oldMaxLength
ââ 
:
ââ 
$num
ââ !
,
ââ! "
oldNullable
ää 
:
ää 
true
ää !
)
ää! "
;
ää" #
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
çç 6
,
çç6 7
table
éé 
:
éé 
$str
éé &
,
éé& '
column
èè 
:
èè 
$str
èè '
)
èè' (
;
èè( )
migrationBuilder
ëë 
.
ëë 
AddForeignKey
ëë *
(
ëë* +
name
íí 
:
íí 
$str
íí 8
,
íí8 9
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
îî "
,
îî" #
principalTable
ïï 
:
ïï 
$str
ïï  )
,
ïï) *
principalColumn
ññ 
:
ññ  
$str
ññ! +
,
ññ+ ,
onDelete
óó 
:
óó 
ReferentialAction
óó +
.
óó+ ,
Cascade
óó, 3
)
óó3 4
;
óó4 5
migrationBuilder
ôô 
.
ôô 
AddForeignKey
ôô *
(
ôô* +
name
öö 
:
öö 
$str
öö :
,
öö: ;
table
õõ 
:
õõ 
$str
õõ %
,
õõ% &
column
úú 
:
úú 
$str
úú #
,
úú# $
principalTable
ùù 
:
ùù 
$str
ùù  *
,
ùù* +
principalColumn
ûû 
:
ûû  
$str
ûû! ,
,
ûû, -
onDelete
üü 
:
üü 
ReferentialAction
üü +
.
üü+ ,
Cascade
üü, 3
)
üü3 4
;
üü4 5
migrationBuilder
°° 
.
°° 
AddForeignKey
°° *
(
°°* +
name
¢¢ 
:
¢¢ 
$str
¢¢ C
,
¢¢C D
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
§§ '
,
§§' (
principalTable
•• 
:
•• 
$str
••  .
,
••. /
principalColumn
¶¶ 
:
¶¶  
$str
¶¶! 0
,
¶¶0 1
onDelete
ßß 
:
ßß 
ReferentialAction
ßß +
.
ßß+ ,
Cascade
ßß, 3
)
ßß3 4
;
ßß4 5
}
®® 	
}
©© 
}™™ ˛
`C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260618091053_NewModels.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class 
	NewModels "
:# $
	Migration% .
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
} ˆ‡
wC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260616094607_removedUserTableandidentityadded.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public		 

partial		 
class		 ,
 removedUserTableandidentityadded		 9
:		: ;
	Migration		< E
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
	DropTable &
(& '
name 
: 
$str 
) 
; 
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str #
,# $
columns 
: 
table 
=> !
new" %
{ 
Id 
= 
table 
. 
Column %
<% &
string& ,
>, -
(- .
type. 2
:2 3
$str4 C
,C D
nullableE M
:M N
falseO T
)T U
,U V
Name 
= 
table  
.  !
Column! '
<' (
string( .
>. /
(/ 0
type0 4
:4 5
$str6 E
,E F
	maxLengthG P
:P Q
$numR U
,U V
nullableW _
:_ `
truea e
)e f
,f g
NormalizedName "
=# $
table% *
.* +
Column+ 1
<1 2
string2 8
>8 9
(9 :
type: >
:> ?
$str@ O
,O P
	maxLengthQ Z
:Z [
$num\ _
,_ `
nullablea i
:i j
truek o
)o p
,p q
ConcurrencyStamp $
=% &
table' ,
., -
Column- 3
<3 4
string4 :
>: ;
(; <
type< @
:@ A
$strB Q
,Q R
nullableS [
:[ \
true] a
)a b
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
$str% 5
,5 6
x7 8
=>9 ;
x< =
.= >
Id> @
)@ A
;A B
} 
) 
; 
migrationBuilder 
. 
CreateTable (
(( )
name   
:   
$str   #
,  # $
columns!! 
:!! 
table!! 
=>!! !
new!!" %
{"" 
Id## 
=## 
table## 
.## 
Column## %
<##% &
string##& ,
>##, -
(##- .
type##. 2
:##2 3
$str##4 C
,##C D
nullable##E M
:##M N
false##O T
)##T U
,##U V
UserName$$ 
=$$ 
table$$ $
.$$$ %
Column$$% +
<$$+ ,
string$$, 2
>$$2 3
($$3 4
type$$4 8
:$$8 9
$str$$: I
,$$I J
	maxLength$$K T
:$$T U
$num$$V Y
,$$Y Z
nullable$$[ c
:$$c d
true$$e i
)$$i j
,$$j k
NormalizedUserName%% &
=%%' (
table%%) .
.%%. /
Column%%/ 5
<%%5 6
string%%6 <
>%%< =
(%%= >
type%%> B
:%%B C
$str%%D S
,%%S T
	maxLength%%U ^
:%%^ _
$num%%` c
,%%c d
nullable%%e m
:%%m n
true%%o s
)%%s t
,%%t u
Email&& 
=&& 
table&& !
.&&! "
Column&&" (
<&&( )
string&&) /
>&&/ 0
(&&0 1
type&&1 5
:&&5 6
$str&&7 F
,&&F G
	maxLength&&H Q
:&&Q R
$num&&S V
,&&V W
nullable&&X `
:&&` a
true&&b f
)&&f g
,&&g h
NormalizedEmail'' #
=''$ %
table''& +
.''+ ,
Column'', 2
<''2 3
string''3 9
>''9 :
('': ;
type''; ?
:''? @
$str''A P
,''P Q
	maxLength''R [
:''[ \
$num''] `
,''` a
nullable''b j
:''j k
true''l p
)''p q
,''q r
EmailConfirmed(( "
=((# $
table((% *
.((* +
Column((+ 1
<((1 2
bool((2 6
>((6 7
(((7 8
type((8 <
:((< =
$str((> C
,((C D
nullable((E M
:((M N
false((O T
)((T U
,((U V
PasswordHash))  
=))! "
table))# (
.))( )
Column))) /
<))/ 0
string))0 6
>))6 7
())7 8
type))8 <
:))< =
$str))> M
,))M N
nullable))O W
:))W X
true))Y ]
)))] ^
,))^ _
SecurityStamp** !
=**" #
table**$ )
.**) *
Column*** 0
<**0 1
string**1 7
>**7 8
(**8 9
type**9 =
:**= >
$str**? N
,**N O
nullable**P X
:**X Y
true**Z ^
)**^ _
,**_ `
ConcurrencyStamp++ $
=++% &
table++' ,
.++, -
Column++- 3
<++3 4
string++4 :
>++: ;
(++; <
type++< @
:++@ A
$str++B Q
,++Q R
nullable++S [
:++[ \
true++] a
)++a b
,++b c
PhoneNumber,, 
=,,  !
table,," '
.,,' (
Column,,( .
<,,. /
string,,/ 5
>,,5 6
(,,6 7
type,,7 ;
:,,; <
$str,,= L
,,,L M
nullable,,N V
:,,V W
true,,X \
),,\ ]
,,,] ^ 
PhoneNumberConfirmed-- (
=--) *
table--+ 0
.--0 1
Column--1 7
<--7 8
bool--8 <
>--< =
(--= >
type--> B
:--B C
$str--D I
,--I J
nullable--K S
:--S T
false--U Z
)--Z [
,--[ \
TwoFactorEnabled.. $
=..% &
table..' ,
..., -
Column..- 3
<..3 4
bool..4 8
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
,..W X

LockoutEnd// 
=//  
table//! &
.//& '
Column//' -
<//- .
DateTimeOffset//. <
>//< =
(//= >
type//> B
://B C
$str//D T
,//T U
nullable//V ^
://^ _
true//` d
)//d e
,//e f
LockoutEnabled00 "
=00# $
table00% *
.00* +
Column00+ 1
<001 2
bool002 6
>006 7
(007 8
type008 <
:00< =
$str00> C
,00C D
nullable00E M
:00M N
false00O T
)00T U
,00U V
AccessFailedCount11 %
=11& '
table11( -
.11- .
Column11. 4
<114 5
int115 8
>118 9
(119 :
type11: >
:11> ?
$str11@ E
,11E F
nullable11G O
:11O P
false11Q V
)11V W
}22 
,22 
constraints33 
:33 
table33 "
=>33# %
{44 
table55 
.55 

PrimaryKey55 $
(55$ %
$str55% 5
,555 6
x557 8
=>559 ;
x55< =
.55= >
Id55> @
)55@ A
;55A B
}66 
)66 
;66 
migrationBuilder88 
.88 
CreateTable88 (
(88( )
name99 
:99 
$str99 (
,99( )
columns:: 
::: 
table:: 
=>:: !
new::" %
{;; 
Id<< 
=<< 
table<< 
.<< 
Column<< %
<<<% &
int<<& )
><<) *
(<<* +
type<<+ /
:<</ 0
$str<<1 6
,<<6 7
nullable<<8 @
:<<@ A
false<<B G
)<<G H
.== 

Annotation== #
(==# $
$str==$ 8
,==8 9
$str==: @
)==@ A
,==A B
RoleId>> 
=>> 
table>> "
.>>" #
Column>># )
<>>) *
string>>* 0
>>>0 1
(>>1 2
type>>2 6
:>>6 7
$str>>8 G
,>>G H
nullable>>I Q
:>>Q R
false>>S X
)>>X Y
,>>Y Z
	ClaimType?? 
=?? 
table??  %
.??% &
Column??& ,
<??, -
string??- 3
>??3 4
(??4 5
type??5 9
:??9 :
$str??; J
,??J K
nullable??L T
:??T U
true??V Z
)??Z [
,??[ \

ClaimValue@@ 
=@@  
table@@! &
.@@& '
Column@@' -
<@@- .
string@@. 4
>@@4 5
(@@5 6
type@@6 :
:@@: ;
$str@@< K
,@@K L
nullable@@M U
:@@U V
true@@W [
)@@[ \
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
$strDD% :
,DD: ;
xDD< =
=>DD> @
xDDA B
.DDB C
IdDDC E
)DDE F
;DDF G
tableEE 
.EE 

ForeignKeyEE $
(EE$ %
nameFF 
:FF 
$strFF F
,FFF G
columnGG 
:GG 
xGG  !
=>GG" $
xGG% &
.GG& '
RoleIdGG' -
,GG- .
principalTableHH &
:HH& '
$strHH( 5
,HH5 6
principalColumnII '
:II' (
$strII) -
,II- .
onDeleteJJ  
:JJ  !
ReferentialActionJJ" 3
.JJ3 4
CascadeJJ4 ;
)JJ; <
;JJ< =
}KK 
)KK 
;KK 
migrationBuilderMM 
.MM 
CreateTableMM (
(MM( )
nameNN 
:NN 
$strNN (
,NN( )
columnsOO 
:OO 
tableOO 
=>OO !
newOO" %
{PP 
IdQQ 
=QQ 
tableQQ 
.QQ 
ColumnQQ %
<QQ% &
intQQ& )
>QQ) *
(QQ* +
typeQQ+ /
:QQ/ 0
$strQQ1 6
,QQ6 7
nullableQQ8 @
:QQ@ A
falseQQB G
)QQG H
.RR 

AnnotationRR #
(RR# $
$strRR$ 8
,RR8 9
$strRR: @
)RR@ A
,RRA B
UserIdSS 
=SS 
tableSS "
.SS" #
ColumnSS# )
<SS) *
stringSS* 0
>SS0 1
(SS1 2
typeSS2 6
:SS6 7
$strSS8 G
,SSG H
nullableSSI Q
:SSQ R
falseSSS X
)SSX Y
,SSY Z
	ClaimTypeTT 
=TT 
tableTT  %
.TT% &
ColumnTT& ,
<TT, -
stringTT- 3
>TT3 4
(TT4 5
typeTT5 9
:TT9 :
$strTT; J
,TTJ K
nullableTTL T
:TTT U
trueTTV Z
)TTZ [
,TT[ \

ClaimValueUU 
=UU  
tableUU! &
.UU& '
ColumnUU' -
<UU- .
stringUU. 4
>UU4 5
(UU5 6
typeUU6 :
:UU: ;
$strUU< K
,UUK L
nullableUUM U
:UUU V
trueUUW [
)UU[ \
}VV 
,VV 
constraintsWW 
:WW 
tableWW "
=>WW# %
{XX 
tableYY 
.YY 

PrimaryKeyYY $
(YY$ %
$strYY% :
,YY: ;
xYY< =
=>YY> @
xYYA B
.YYB C
IdYYC E
)YYE F
;YYF G
tableZZ 
.ZZ 

ForeignKeyZZ $
(ZZ$ %
name[[ 
:[[ 
$str[[ F
,[[F G
column\\ 
:\\ 
x\\  !
=>\\" $
x\\% &
.\\& '
UserId\\' -
,\\- .
principalTable]] &
:]]& '
$str]]( 5
,]]5 6
principalColumn^^ '
:^^' (
$str^^) -
,^^- .
onDelete__  
:__  !
ReferentialAction__" 3
.__3 4
Cascade__4 ;
)__; <
;__< =
}`` 
)`` 
;`` 
migrationBuilderbb 
.bb 
CreateTablebb (
(bb( )
namecc 
:cc 
$strcc (
,cc( )
columnsdd 
:dd 
tabledd 
=>dd !
newdd" %
{ee 
LoginProviderff !
=ff" #
tableff$ )
.ff) *
Columnff* 0
<ff0 1
stringff1 7
>ff7 8
(ff8 9
typeff9 =
:ff= >
$strff? N
,ffN O
nullableffP X
:ffX Y
falseffZ _
)ff_ `
,ff` a
ProviderKeygg 
=gg  !
tablegg" '
.gg' (
Columngg( .
<gg. /
stringgg/ 5
>gg5 6
(gg6 7
typegg7 ;
:gg; <
$strgg= L
,ggL M
nullableggN V
:ggV W
falseggX ]
)gg] ^
,gg^ _
ProviderDisplayNamehh '
=hh( )
tablehh* /
.hh/ 0
Columnhh0 6
<hh6 7
stringhh7 =
>hh= >
(hh> ?
typehh? C
:hhC D
$strhhE T
,hhT U
nullablehhV ^
:hh^ _
truehh` d
)hhd e
,hhe f
UserIdii 
=ii 
tableii "
.ii" #
Columnii# )
<ii) *
stringii* 0
>ii0 1
(ii1 2
typeii2 6
:ii6 7
$strii8 G
,iiG H
nullableiiI Q
:iiQ R
falseiiS X
)iiX Y
}jj 
,jj 
constraintskk 
:kk 
tablekk "
=>kk# %
{ll 
tablemm 
.mm 

PrimaryKeymm $
(mm$ %
$strmm% :
,mm: ;
xmm< =
=>mm> @
newmmA D
{mmE F
xmmG H
.mmH I
LoginProvidermmI V
,mmV W
xmmX Y
.mmY Z
ProviderKeymmZ e
}mmf g
)mmg h
;mmh i
tablenn 
.nn 

ForeignKeynn $
(nn$ %
nameoo 
:oo 
$stroo F
,ooF G
columnpp 
:pp 
xpp  !
=>pp" $
xpp% &
.pp& '
UserIdpp' -
,pp- .
principalTableqq &
:qq& '
$strqq( 5
,qq5 6
principalColumnrr '
:rr' (
$strrr) -
,rr- .
onDeletess  
:ss  !
ReferentialActionss" 3
.ss3 4
Cascadess4 ;
)ss; <
;ss< =
}tt 
)tt 
;tt 
migrationBuildervv 
.vv 
CreateTablevv (
(vv( )
nameww 
:ww 
$strww '
,ww' (
columnsxx 
:xx 
tablexx 
=>xx !
newxx" %
{yy 
UserIdzz 
=zz 
tablezz "
.zz" #
Columnzz# )
<zz) *
stringzz* 0
>zz0 1
(zz1 2
typezz2 6
:zz6 7
$strzz8 G
,zzG H
nullablezzI Q
:zzQ R
falsezzS X
)zzX Y
,zzY Z
RoleId{{ 
={{ 
table{{ "
.{{" #
Column{{# )
<{{) *
string{{* 0
>{{0 1
({{1 2
type{{2 6
:{{6 7
$str{{8 G
,{{G H
nullable{{I Q
:{{Q R
false{{S X
){{X Y
}|| 
,|| 
constraints}} 
:}} 
table}} "
=>}}# %
{~~ 
table 
. 

PrimaryKey $
($ %
$str% 9
,9 :
x; <
=>= ?
new@ C
{D E
xF G
.G H
UserIdH N
,N O
xP Q
.Q R
RoleIdR X
}Y Z
)Z [
;[ \
table
ÄÄ 
.
ÄÄ 

ForeignKey
ÄÄ $
(
ÄÄ$ %
name
ÅÅ 
:
ÅÅ 
$str
ÅÅ E
,
ÅÅE F
column
ÇÇ 
:
ÇÇ 
x
ÇÇ  !
=>
ÇÇ" $
x
ÇÇ% &
.
ÇÇ& '
RoleId
ÇÇ' -
,
ÇÇ- .
principalTable
ÉÉ &
:
ÉÉ& '
$str
ÉÉ( 5
,
ÉÉ5 6
principalColumn
ÑÑ '
:
ÑÑ' (
$str
ÑÑ) -
,
ÑÑ- .
onDelete
ÖÖ  
:
ÖÖ  !
ReferentialAction
ÖÖ" 3
.
ÖÖ3 4
Cascade
ÖÖ4 ;
)
ÖÖ; <
;
ÖÖ< =
table
ÜÜ 
.
ÜÜ 

ForeignKey
ÜÜ $
(
ÜÜ$ %
name
áá 
:
áá 
$str
áá E
,
ááE F
column
àà 
:
àà 
x
àà  !
=>
àà" $
x
àà% &
.
àà& '
UserId
àà' -
,
àà- .
principalTable
ââ &
:
ââ& '
$str
ââ( 5
,
ââ5 6
principalColumn
ää '
:
ää' (
$str
ää) -
,
ää- .
onDelete
ãã  
:
ãã  !
ReferentialAction
ãã" 3
.
ãã3 4
Cascade
ãã4 ;
)
ãã; <
;
ãã< =
}
åå 
)
åå 
;
åå 
migrationBuilder
éé 
.
éé 
CreateTable
éé (
(
éé( )
name
èè 
:
èè 
$str
èè (
,
èè( )
columns
êê 
:
êê 
table
êê 
=>
êê !
new
êê" %
{
ëë 
UserId
íí 
=
íí 
table
íí "
.
íí" #
Column
íí# )
<
íí) *
string
íí* 0
>
íí0 1
(
íí1 2
type
íí2 6
:
íí6 7
$str
íí8 G
,
ííG H
nullable
ííI Q
:
ííQ R
false
ííS X
)
ííX Y
,
ííY Z
LoginProvider
ìì !
=
ìì" #
table
ìì$ )
.
ìì) *
Column
ìì* 0
<
ìì0 1
string
ìì1 7
>
ìì7 8
(
ìì8 9
type
ìì9 =
:
ìì= >
$str
ìì? N
,
ììN O
nullable
ììP X
:
ììX Y
false
ììZ _
)
ìì_ `
,
ìì` a
Name
îî 
=
îî 
table
îî  
.
îî  !
Column
îî! '
<
îî' (
string
îî( .
>
îî. /
(
îî/ 0
type
îî0 4
:
îî4 5
$str
îî6 E
,
îîE F
nullable
îîG O
:
îîO P
false
îîQ V
)
îîV W
,
îîW X
Value
ïï 
=
ïï 
table
ïï !
.
ïï! "
Column
ïï" (
<
ïï( )
string
ïï) /
>
ïï/ 0
(
ïï0 1
type
ïï1 5
:
ïï5 6
$str
ïï7 F
,
ïïF G
nullable
ïïH P
:
ïïP Q
true
ïïR V
)
ïïV W
}
ññ 
,
ññ 
constraints
óó 
:
óó 
table
óó "
=>
óó# %
{
òò 
table
ôô 
.
ôô 

PrimaryKey
ôô $
(
ôô$ %
$str
ôô% :
,
ôô: ;
x
ôô< =
=>
ôô> @
new
ôôA D
{
ôôE F
x
ôôG H
.
ôôH I
UserId
ôôI O
,
ôôO P
x
ôôQ R
.
ôôR S
LoginProvider
ôôS `
,
ôô` a
x
ôôb c
.
ôôc d
Name
ôôd h
}
ôôi j
)
ôôj k
;
ôôk l
table
öö 
.
öö 

ForeignKey
öö $
(
öö$ %
name
õõ 
:
õõ 
$str
õõ F
,
õõF G
column
úú 
:
úú 
x
úú  !
=>
úú" $
x
úú% &
.
úú& '
UserId
úú' -
,
úú- .
principalTable
ùù &
:
ùù& '
$str
ùù( 5
,
ùù5 6
principalColumn
ûû '
:
ûû' (
$str
ûû) -
,
ûû- .
onDelete
üü  
:
üü  !
ReferentialAction
üü" 3
.
üü3 4
Cascade
üü4 ;
)
üü; <
;
üü< =
}
†† 
)
†† 
;
†† 
migrationBuilder
¢¢ 
.
¢¢ 
CreateIndex
¢¢ (
(
¢¢( )
name
££ 
:
££ 
$str
££ 2
,
££2 3
table
§§ 
:
§§ 
$str
§§ )
,
§§) *
column
•• 
:
•• 
$str
••  
)
••  !
;
••! "
migrationBuilder
ßß 
.
ßß 
CreateIndex
ßß (
(
ßß( )
name
®® 
:
®® 
$str
®® %
,
®®% &
table
©© 
:
©© 
$str
©© $
,
©©$ %
column
™™ 
:
™™ 
$str
™™ (
,
™™( )
unique
´´ 
:
´´ 
true
´´ 
,
´´ 
filter
¨¨ 
:
¨¨ 
$str
¨¨ 6
)
¨¨6 7
;
¨¨7 8
migrationBuilder
ÆÆ 
.
ÆÆ 
CreateIndex
ÆÆ (
(
ÆÆ( )
name
ØØ 
:
ØØ 
$str
ØØ 2
,
ØØ2 3
table
∞∞ 
:
∞∞ 
$str
∞∞ )
,
∞∞) *
column
±± 
:
±± 
$str
±±  
)
±±  !
;
±±! "
migrationBuilder
≥≥ 
.
≥≥ 
CreateIndex
≥≥ (
(
≥≥( )
name
¥¥ 
:
¥¥ 
$str
¥¥ 2
,
¥¥2 3
table
µµ 
:
µµ 
$str
µµ )
,
µµ) *
column
∂∂ 
:
∂∂ 
$str
∂∂  
)
∂∂  !
;
∂∂! "
migrationBuilder
∏∏ 
.
∏∏ 
CreateIndex
∏∏ (
(
∏∏( )
name
ππ 
:
ππ 
$str
ππ 1
,
ππ1 2
table
∫∫ 
:
∫∫ 
$str
∫∫ (
,
∫∫( )
column
ªª 
:
ªª 
$str
ªª  
)
ªª  !
;
ªª! "
migrationBuilder
ΩΩ 
.
ΩΩ 
CreateIndex
ΩΩ (
(
ΩΩ( )
name
ææ 
:
ææ 
$str
ææ "
,
ææ" #
table
øø 
:
øø 
$str
øø $
,
øø$ %
column
¿¿ 
:
¿¿ 
$str
¿¿ )
)
¿¿) *
;
¿¿* +
migrationBuilder
¬¬ 
.
¬¬ 
CreateIndex
¬¬ (
(
¬¬( )
name
√√ 
:
√√ 
$str
√√ %
,
√√% &
table
ƒƒ 
:
ƒƒ 
$str
ƒƒ $
,
ƒƒ$ %
column
≈≈ 
:
≈≈ 
$str
≈≈ ,
,
≈≈, -
unique
∆∆ 
:
∆∆ 
true
∆∆ 
,
∆∆ 
filter
«« 
:
«« 
$str
«« :
)
««: ;
;
««; <
}
»» 	
	protected
ÀÀ 
override
ÀÀ 
void
ÀÀ 
Down
ÀÀ  $
(
ÀÀ$ %
MigrationBuilder
ÀÀ% 5
migrationBuilder
ÀÀ6 F
)
ÀÀF G
{
ÃÃ 	
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
‘‘ (
)
‘‘( )
;
‘‘) *
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
◊◊ '
)
◊◊' (
;
◊◊( )
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
⁄⁄ (
)
⁄⁄( )
;
⁄⁄) *
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
››$ %
migrationBuilder
ﬂﬂ 
.
ﬂﬂ 
	DropTable
ﬂﬂ &
(
ﬂﬂ& '
name
‡‡ 
:
‡‡ 
$str
‡‡ #
)
‡‡# $
;
‡‡$ %
migrationBuilder
‚‚ 
.
‚‚ 
CreateTable
‚‚ (
(
‚‚( )
name
„„ 
:
„„ 
$str
„„ 
,
„„ 
columns
‰‰ 
:
‰‰ 
table
‰‰ 
=>
‰‰ !
new
‰‰" %
{
ÂÂ 
UserId
ÊÊ 
=
ÊÊ 
table
ÊÊ "
.
ÊÊ" #
Column
ÊÊ# )
<
ÊÊ) *
int
ÊÊ* -
>
ÊÊ- .
(
ÊÊ. /
type
ÊÊ/ 3
:
ÊÊ3 4
$str
ÊÊ5 :
,
ÊÊ: ;
nullable
ÊÊ< D
:
ÊÊD E
false
ÊÊF K
)
ÊÊK L
.
ÁÁ 

Annotation
ÁÁ #
(
ÁÁ# $
$str
ÁÁ$ 8
,
ÁÁ8 9
$str
ÁÁ: @
)
ÁÁ@ A
,
ÁÁA B
CreatedDate
ËË 
=
ËË  !
table
ËË" '
.
ËË' (
Column
ËË( .
<
ËË. /
DateTime
ËË/ 7
>
ËË7 8
(
ËË8 9
type
ËË9 =
:
ËË= >
$str
ËË? J
,
ËËJ K
nullable
ËËL T
:
ËËT U
false
ËËV [
)
ËË[ \
,
ËË\ ]
Email
ÈÈ 
=
ÈÈ 
table
ÈÈ !
.
ÈÈ! "
Column
ÈÈ" (
<
ÈÈ( )
string
ÈÈ) /
>
ÈÈ/ 0
(
ÈÈ0 1
type
ÈÈ1 5
:
ÈÈ5 6
$str
ÈÈ7 F
,
ÈÈF G
nullable
ÈÈH P
:
ÈÈP Q
false
ÈÈR W
)
ÈÈW X
,
ÈÈX Y
PasswordHash
ÍÍ  
=
ÍÍ! "
table
ÍÍ# (
.
ÍÍ( )
Column
ÍÍ) /
<
ÍÍ/ 0
string
ÍÍ0 6
>
ÍÍ6 7
(
ÍÍ7 8
type
ÍÍ8 <
:
ÍÍ< =
$str
ÍÍ> M
,
ÍÍM N
nullable
ÍÍO W
:
ÍÍW X
false
ÍÍY ^
)
ÍÍ^ _
,
ÍÍ_ `
PasswordSalt
ÎÎ  
=
ÎÎ! "
table
ÎÎ# (
.
ÎÎ( )
Column
ÎÎ) /
<
ÎÎ/ 0
string
ÎÎ0 6
>
ÎÎ6 7
(
ÎÎ7 8
type
ÎÎ8 <
:
ÎÎ< =
$str
ÎÎ> M
,
ÎÎM N
nullable
ÎÎO W
:
ÎÎW X
false
ÎÎY ^
)
ÎÎ^ _
,
ÎÎ_ `
ReferenceId
ÏÏ 
=
ÏÏ  !
table
ÏÏ" '
.
ÏÏ' (
Column
ÏÏ( .
<
ÏÏ. /
int
ÏÏ/ 2
>
ÏÏ2 3
(
ÏÏ3 4
type
ÏÏ4 8
:
ÏÏ8 9
$str
ÏÏ: ?
,
ÏÏ? @
nullable
ÏÏA I
:
ÏÏI J
true
ÏÏK O
)
ÏÏO P
,
ÏÏP Q
Role
ÌÌ 
=
ÌÌ 
table
ÌÌ  
.
ÌÌ  !
Column
ÌÌ! '
<
ÌÌ' (
string
ÌÌ( .
>
ÌÌ. /
(
ÌÌ/ 0
type
ÌÌ0 4
:
ÌÌ4 5
$str
ÌÌ6 E
,
ÌÌE F
nullable
ÌÌG O
:
ÌÌO P
false
ÌÌQ V
)
ÌÌV W
}
ÓÓ 
,
ÓÓ 
constraints
ÔÔ 
:
ÔÔ 
table
ÔÔ "
=>
ÔÔ# %
{
 
table
ÒÒ 
.
ÒÒ 

PrimaryKey
ÒÒ $
(
ÒÒ$ %
$str
ÒÒ% /
,
ÒÒ/ 0
x
ÒÒ1 2
=>
ÒÒ3 5
x
ÒÒ6 7
.
ÒÒ7 8
UserId
ÒÒ8 >
)
ÒÒ> ?
;
ÒÒ? @
}
ÚÚ 
)
ÚÚ 
;
ÚÚ 
}
ÛÛ 	
}
ÙÙ 
}ıı õ
nC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260615041903_AddingIsActiveToPatient.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public 

partial 
class #
AddingIsActiveToPatient 0
:1 2
	Migration3 <
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
$str  
,  !
table 
: 
$str !
,! "
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
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str !
,! "
	keyColumn 
: 
$str &
,& '
keyValue 
: 
$num 
, 
column 
: 
$str "
," #
value 
: 
true 
) 
; 
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str !
,! "
	keyColumn 
: 
$str &
,& '
keyValue 
: 
$num 
, 
column 
: 
$str "
," #
value   
:   
true   
)   
;   
migrationBuilder"" 
."" 

UpdateData"" '
(""' (
table## 
:## 
$str## !
,##! "
	keyColumn$$ 
:$$ 
$str$$ &
,$$& '
keyValue%% 
:%% 
$num%% 
,%% 
column&& 
:&& 
$str&& "
,&&" #
value'' 
:'' 
true'' 
)'' 
;'' 
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
.-- 

DropColumn-- '
(--' (
name.. 
:.. 
$str..  
,..  !
table// 
:// 
$str// !
)//! "
;//" #
}00 	
}11 
}22 ©2
fC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260614210306_SeedInitialData.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{		 
public 

partial 
class 
SeedInitialData (
:) *
	Migration+ 4
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
. 

InsertData '
(' (
table 
: 
$str  
,  !
columns 
: 
new 
[ 
] 
{  
$str! +
,+ ,
$str- >
,> ?
$str@ L
,L M
$strN X
,X Y
$strZ j
,j k
$strl 
}
Ä Å
,
Å Ç
values 
: 
new 
object "
[" #
,# $
]$ %
{ 
{ 
$num 
, 
$num 
, 
$str  0
,0 1
true2 6
,6 7
$str8 F
,F G
$numH I
}J K
,K L
{ 
$num 
, 
$num 
, 
$str  /
,/ 0
true1 5
,5 6
$str7 H
,H I
$numJ K
}L M
,M N
{ 
$num 
, 
$num 
, 
$str  ,
,, -
true. 2
,2 3
$str4 C
,C D
$numE F
}G H
} 
) 
; 
migrationBuilder 
. 

InsertData '
(' (
table 
: 
$str !
,! "
columns 
: 
new 
[ 
] 
{  
$str! ,
,, -
$str. ;
,; <
$str= D
,D E
$strF N
,N O
$strP ]
,] ^
$str_ l
,l m
$strn w
}x y
,y z
values 
: 
new 
object "
[" #
,# $
]$ %
{ 
{ 
$num 
, 
new 
DateTime %
(% &
$num& *
,* +
$num, .
,. /
$num0 1
,1 2
$num3 4
,4 5
$num6 7
,7 8
$num9 :
,: ;
$num< =
,= >
DateTimeKind? K
.K L
UnspecifiedL W
)W X
,X Y
$strZ k
,k l
$strm s
,s t
nullu y
,y z
$str	{ Ç
,
Ç É
$str
Ñ ê
}
ë í
,
í ì
{   
$num   
,   
new   
DateTime   %
(  % &
$num  & *
,  * +
$num  , -
,  - .
$num  / 0
,  0 1
$num  2 3
,  3 4
$num  5 6
,  6 7
$num  8 9
,  9 :
$num  ; <
,  < =
DateTimeKind  > J
.  J K
Unspecified  K V
)  V W
,  W X
$str  Y k
,  k l
$str  m s
,  s t
null  u y
,  y z
$str	  { É
,
  É Ñ
$str
  Ö ë
}
  í ì
,
  ì î
{!! 
$num!! 
,!! 
new!! 
DateTime!! %
(!!% &
$num!!& *
,!!* +
$num!!, -
,!!- .
$num!!/ 0
,!!0 1
$num!!2 3
,!!3 4
$num!!5 6
,!!6 7
$num!!8 9
,!!9 :
$num!!; <
,!!< =
DateTimeKind!!> J
.!!J K
Unspecified!!K V
)!!V W
,!!W X
$str!!Y k
,!!k l
$str!!m s
,!!s t
null!!u y
,!!y z
$str	!!{ É
,
!!É Ñ
$str
!!Ö ë
}
!!í ì
}"" 
)"" 
;"" 
}## 	
	protected&& 
override&& 
void&& 
Down&&  $
(&&$ %
MigrationBuilder&&% 5
migrationBuilder&&6 F
)&&F G
{'' 	
migrationBuilder(( 
.(( 

DeleteData(( '
(((' (
table)) 
:)) 
$str))  
,))  !
	keyColumn** 
:** 
$str** %
,**% &
keyValue++ 
:++ 
$num++ 
)++ 
;++ 
migrationBuilder-- 
.-- 

DeleteData-- '
(--' (
table.. 
:.. 
$str..  
,..  !
	keyColumn// 
:// 
$str// %
,//% &
keyValue00 
:00 
$num00 
)00 
;00 
migrationBuilder22 
.22 

DeleteData22 '
(22' (
table33 
:33 
$str33  
,33  !
	keyColumn44 
:44 
$str44 %
,44% &
keyValue55 
:55 
$num55 
)55 
;55 
migrationBuilder77 
.77 

DeleteData77 '
(77' (
table88 
:88 
$str88 !
,88! "
	keyColumn99 
:99 
$str99 &
,99& '
keyValue:: 
::: 
$num:: 
):: 
;:: 
migrationBuilder<< 
.<< 

DeleteData<< '
(<<' (
table== 
:== 
$str== !
,==! "
	keyColumn>> 
:>> 
$str>> &
,>>& '
keyValue?? 
:?? 
$num?? 
)?? 
;?? 
migrationBuilderAA 
.AA 

DeleteDataAA '
(AA' (
tableBB 
:BB 
$strBB !
,BB! "
	keyColumnCC 
:CC 
$strCC &
,CC& '
keyValueDD 
:DD 
$numDD 
)DD 
;DD 
}EE 	
}FF 
}GG °
cC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Migrations\20260614203038_FixedCascade.cs
	namespace 	
HealthAxisApplicn
 
. 

Migrations &
{ 
public		 

partial		 
class		 
FixedCascade		 %
:		& '
	Migration		( 1
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
$str 
,  
columns 
: 
table 
=> !
new" %
{ 
DoctorId 
= 
table $
.$ %
Column% +
<+ ,
int, /
>/ 0
(0 1
type1 5
:5 6
$str7 <
,< =
nullable> F
:F G
falseH M
)M N
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B

DoctorName 
=  
table! &
.& '
Column' -
<- .
string. 4
>4 5
(5 6
type6 :
:: ;
$str< K
,K L
nullableM U
:U V
falseW \
)\ ]
,] ^
Specialisation "
=# $
table% *
.* +
Column+ 1
<1 2
string2 8
>8 9
(9 :
type: >
:> ?
$str@ O
,O P
nullableQ Y
:Y Z
false[ `
)` a
,a b
YearsOfExperience %
=& '
table( -
.- .
Column. 4
<4 5
int5 8
>8 9
(9 :
type: >
:> ?
$str@ E
,E F
nullableG O
:O P
falseQ V
)V W
,W X
ConsultationFee #
=$ %
table& +
.+ ,
Column, 2
<2 3
decimal3 :
>: ;
(; <
type< @
:@ A
$strB P
,P Q
	precisionR [
:[ \
$num] ^
,^ _
scale` e
:e f
$numg h
,h i
nullablej r
:r s
falset y
)y z
,z {
IsActive 
= 
table $
.$ %
Column% +
<+ ,
bool, 0
>0 1
(1 2
type2 6
:6 7
$str8 =
,= >
nullable? G
:G H
falseI N
)N O
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
$str% 1
,1 2
x3 4
=>5 7
x8 9
.9 :
DoctorId: B
)B C
;C D
} 
) 
; 
migrationBuilder 
. 
CreateTable (
(( )
name   
:   
$str    
,    !
columns!! 
:!! 
table!! 
=>!! !
new!!" %
{"" 
	PatientId## 
=## 
table##  %
.##% &
Column##& ,
<##, -
int##- 0
>##0 1
(##1 2
type##2 6
:##6 7
$str##8 =
,##= >
nullable##? G
:##G H
false##I N
)##N O
.$$ 

Annotation$$ #
($$# $
$str$$$ 8
,$$8 9
$str$$: @
)$$@ A
,$$A B
PatientName%% 
=%%  !
table%%" '
.%%' (
Column%%( .
<%%. /
string%%/ 5
>%%5 6
(%%6 7
type%%7 ;
:%%; <
$str%%= L
,%%L M
nullable%%N V
:%%V W
false%%X ]
)%%] ^
,%%^ _
DateOfBirth&& 
=&&  !
table&&" '
.&&' (
Column&&( .
<&&. /
DateTime&&/ 7
>&&7 8
(&&8 9
type&&9 =
:&&= >
$str&&? J
,&&J K
nullable&&L T
:&&T U
false&&V [
)&&[ \
,&&\ ]
Gender'' 
='' 
table'' "
.''" #
Column''# )
<'') *
string''* 0
>''0 1
(''1 2
type''2 6
:''6 7
$str''8 G
,''G H
nullable''I Q
:''Q R
false''S X
)''X Y
,''Y Z
Email(( 
=(( 
table(( !
.((! "
Column((" (
<((( )
string(() /
>((/ 0
(((0 1
type((1 5
:((5 6
$str((7 F
,((F G
nullable((H P
:((P Q
false((R W
)((W X
,((X Y
PhoneNo)) 
=)) 
table)) #
.))# $
Column))$ *
<))* +
string))+ 1
>))1 2
())2 3
type))3 7
:))7 8
$str))9 H
,))H I
nullable))J R
:))R S
false))T Y
)))Y Z
,))Z [
InsuranceID** 
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
$str**= L
,**L M
nullable**N V
:**V W
true**X \
)**\ ]
}++ 
,++ 
constraints,, 
:,, 
table,, "
=>,,# %
{-- 
table.. 
... 

PrimaryKey.. $
(..$ %
$str..% 2
,..2 3
x..4 5
=>..6 8
x..9 :
...: ;
	PatientId..; D
)..D E
;..E F
}// 
)// 
;// 
migrationBuilder11 
.11 
CreateTable11 (
(11( )
name22 
:22 
$str22 
,22 
columns33 
:33 
table33 
=>33 !
new33" %
{44 
UserId55 
=55 
table55 "
.55" #
Column55# )
<55) *
int55* -
>55- .
(55. /
type55/ 3
:553 4
$str555 :
,55: ;
nullable55< D
:55D E
false55F K
)55K L
.66 

Annotation66 #
(66# $
$str66$ 8
,668 9
$str66: @
)66@ A
,66A B
Email77 
=77 
table77 !
.77! "
Column77" (
<77( )
string77) /
>77/ 0
(770 1
type771 5
:775 6
$str777 F
,77F G
nullable77H P
:77P Q
false77R W
)77W X
,77X Y
PasswordHash88  
=88! "
table88# (
.88( )
Column88) /
<88/ 0
string880 6
>886 7
(887 8
type888 <
:88< =
$str88> M
,88M N
nullable88O W
:88W X
false88Y ^
)88^ _
,88_ `
PasswordSalt99  
=99! "
table99# (
.99( )
Column99) /
<99/ 0
string990 6
>996 7
(997 8
type998 <
:99< =
$str99> M
,99M N
nullable99O W
:99W X
false99Y ^
)99^ _
,99_ `
Role:: 
=:: 
table::  
.::  !
Column::! '
<::' (
string::( .
>::. /
(::/ 0
type::0 4
:::4 5
$str::6 E
,::E F
nullable::G O
:::O P
false::Q V
)::V W
,::W X
ReferenceId;; 
=;;  !
table;;" '
.;;' (
Column;;( .
<;;. /
int;;/ 2
>;;2 3
(;;3 4
type;;4 8
:;;8 9
$str;;: ?
,;;? @
nullable;;A I
:;;I J
true;;K O
);;O P
,;;P Q
CreatedDate<< 
=<<  !
table<<" '
.<<' (
Column<<( .
<<<. /
DateTime<</ 7
><<7 8
(<<8 9
type<<9 =
:<<= >
$str<<? J
,<<J K
nullable<<L T
:<<T U
false<<V [
)<<[ \
}== 
,== 
constraints>> 
:>> 
table>> "
=>>># %
{?? 
table@@ 
.@@ 

PrimaryKey@@ $
(@@$ %
$str@@% /
,@@/ 0
x@@1 2
=>@@3 5
x@@6 7
.@@7 8
UserId@@8 >
)@@> ?
;@@? @
}AA 
)AA 
;AA 
migrationBuilderCC 
.CC 
CreateTableCC (
(CC( )
nameDD 
:DD 
$strDD $
,DD$ %
columnsEE 
:EE 
tableEE 
=>EE !
newEE" %
{FF 
AppointmentIDGG !
=GG" #
tableGG$ )
.GG) *
ColumnGG* 0
<GG0 1
intGG1 4
>GG4 5
(GG5 6
typeGG6 :
:GG: ;
$strGG< A
,GGA B
nullableGGC K
:GGK L
falseGGM R
)GGR S
.HH 

AnnotationHH #
(HH# $
$strHH$ 8
,HH8 9
$strHH: @
)HH@ A
,HHA B
	PatientIDII 
=II 
tableII  %
.II% &
ColumnII& ,
<II, -
intII- 0
>II0 1
(II1 2
typeII2 6
:II6 7
$strII8 =
,II= >
nullableII? G
:IIG H
falseIII N
)IIN O
,IIO P
DoctorIDJJ 
=JJ 
tableJJ $
.JJ$ %
ColumnJJ% +
<JJ+ ,
intJJ, /
>JJ/ 0
(JJ0 1
typeJJ1 5
:JJ5 6
$strJJ7 <
,JJ< =
nullableJJ> F
:JJF G
falseJJH M
)JJM N
,JJN O
ScheduledDateKK !
=KK" #
tableKK$ )
.KK) *
ColumnKK* 0
<KK0 1
DateTimeKK1 9
>KK9 :
(KK: ;
typeKK; ?
:KK? @
$strKKA L
,KKL M
nullableKKN V
:KKV W
falseKKX ]
)KK] ^
,KK^ _
TimeSlotLL 
=LL 
tableLL $
.LL$ %
ColumnLL% +
<LL+ ,
stringLL, 2
>LL2 3
(LL3 4
typeLL4 8
:LL8 9
$strLL: I
,LLI J
nullableLLK S
:LLS T
falseLLU Z
)LLZ [
,LL[ \
StatusMM 
=MM 
tableMM "
.MM" #
ColumnMM# )
<MM) *
stringMM* 0
>MM0 1
(MM1 2
typeMM2 6
:MM6 7
$strMM8 G
,MMG H
nullableMMI Q
:MMQ R
falseMMS X
)MMX Y
,MMY Z
CancellationReasonNN &
=NN' (
tableNN) .
.NN. /
ColumnNN/ 5
<NN5 6
stringNN6 <
>NN< =
(NN= >
typeNN> B
:NNB C
$strNND S
,NNS T
	maxLengthNNU ^
:NN^ _
$numNN` c
,NNc d
nullableNNe m
:NNm n
falseNNo t
)NNt u
}OO 
,OO 
constraintsPP 
:PP 
tablePP "
=>PP# %
{QQ 
tableRR 
.RR 

PrimaryKeyRR $
(RR$ %
$strRR% 6
,RR6 7
xRR8 9
=>RR: <
xRR= >
.RR> ?
AppointmentIDRR? L
)RRL M
;RRM N
tableSS 
.SS 

ForeignKeySS $
(SS$ %
nameTT 
:TT 
$strTT @
,TT@ A
columnUU 
:UU 
xUU  !
=>UU" $
xUU% &
.UU& '
DoctorIDUU' /
,UU/ 0
principalTableVV &
:VV& '
$strVV( 1
,VV1 2
principalColumnWW '
:WW' (
$strWW) 3
,WW3 4
onDeleteXX  
:XX  !
ReferentialActionXX" 3
.XX3 4
CascadeXX4 ;
)XX; <
;XX< =
tableYY 
.YY 

ForeignKeyYY $
(YY$ %
nameZZ 
:ZZ 
$strZZ B
,ZZB C
column[[ 
:[[ 
x[[  !
=>[[" $
x[[% &
.[[& '
	PatientID[[' 0
,[[0 1
principalTable\\ &
:\\& '
$str\\( 2
,\\2 3
principalColumn]] '
:]]' (
$str]]) 4
,]]4 5
onDelete^^  
:^^  !
ReferentialAction^^" 3
.^^3 4
Cascade^^4 ;
)^^; <
;^^< =
}__ 
)__ 
;__ 
migrationBuilderaa 
.aa 
CreateTableaa (
(aa( )
namebb 
:bb 
$strbb %
,bb% &
columnscc 
:cc 
tablecc 
=>cc !
newcc" %
{dd 
HealthRecordIdee "
=ee# $
tableee% *
.ee* +
Columnee+ 1
<ee1 2
intee2 5
>ee5 6
(ee6 7
typeee7 ;
:ee; <
$stree= B
,eeB C
nullableeeD L
:eeL M
falseeeN S
)eeS T
.ff 

Annotationff #
(ff# $
$strff$ 8
,ff8 9
$strff: @
)ff@ A
,ffA B
	PatientIdgg 
=gg 
tablegg  %
.gg% &
Columngg& ,
<gg, -
intgg- 0
>gg0 1
(gg1 2
typegg2 6
:gg6 7
$strgg8 =
,gg= >
nullablegg? G
:ggG H
falseggI N
)ggN O
,ggO P
DoctorIdhh 
=hh 
tablehh $
.hh$ %
Columnhh% +
<hh+ ,
inthh, /
>hh/ 0
(hh0 1
typehh1 5
:hh5 6
$strhh7 <
,hh< =
nullablehh> F
:hhF G
falsehhH M
)hhM N
,hhN O
AppointmentIdii !
=ii" #
tableii$ )
.ii) *
Columnii* 0
<ii0 1
intii1 4
>ii4 5
(ii5 6
typeii6 :
:ii: ;
$strii< A
,iiA B
nullableiiC K
:iiK L
falseiiM R
)iiR S
,iiS T
	VisitDatejj 
=jj 
tablejj  %
.jj% &
Columnjj& ,
<jj, -
DateTimejj- 5
>jj5 6
(jj6 7
typejj7 ;
:jj; <
$strjj= H
,jjH I
nullablejjJ R
:jjR S
falsejjT Y
)jjY Z
,jjZ [
	Diagnosiskk 
=kk 
tablekk  %
.kk% &
Columnkk& ,
<kk, -
stringkk- 3
>kk3 4
(kk4 5
typekk5 9
:kk9 :
$strkk; J
,kkJ K
nullablekkL T
:kkT U
falsekkV [
)kk[ \
,kk\ ]
Prescriptionll  
=ll! "
tablell# (
.ll( )
Columnll) /
<ll/ 0
stringll0 6
>ll6 7
(ll7 8
typell8 <
:ll< =
$strll> M
,llM N
nullablellO W
:llW X
falsellY ^
)ll^ _
,ll_ `
Notesmm 
=mm 
tablemm !
.mm! "
Columnmm" (
<mm( )
stringmm) /
>mm/ 0
(mm0 1
typemm1 5
:mm5 6
$strmm7 F
,mmF G
nullablemmH P
:mmP Q
truemmR V
)mmV W
}nn 
,nn 
constraintsoo 
:oo 
tableoo "
=>oo# %
{pp 
tableqq 
.qq 

PrimaryKeyqq $
(qq$ %
$strqq% 7
,qq7 8
xqq9 :
=>qq; =
xqq> ?
.qq? @
HealthRecordIdqq@ N
)qqN O
;qqO P
tablerr 
.rr 

ForeignKeyrr $
(rr$ %
namess 
:ss 
$strss K
,ssK L
columntt 
:tt 
xtt  !
=>tt" $
xtt% &
.tt& '
AppointmentIdtt' 4
,tt4 5
principalTableuu &
:uu& '
$struu( 6
,uu6 7
principalColumnvv '
:vv' (
$strvv) 8
,vv8 9
onDeleteww  
:ww  !
ReferentialActionww" 3
.ww3 4
Cascadeww4 ;
)ww; <
;ww< =
tablexx 
.xx 

ForeignKeyxx $
(xx$ %
nameyy 
:yy 
$stryy A
,yyA B
columnzz 
:zz 
xzz  !
=>zz" $
xzz% &
.zz& '
DoctorIdzz' /
,zz/ 0
principalTable{{ &
:{{& '
$str{{( 1
,{{1 2
principalColumn|| '
:||' (
$str||) 3
)||3 4
;||4 5
table}} 
.}} 

ForeignKey}} $
(}}$ %
name~~ 
:~~ 
$str~~ C
,~~C D
column 
: 
x  !
=>" $
x% &
.& '
	PatientId' 0
,0 1
principalTable
ÄÄ &
:
ÄÄ& '
$str
ÄÄ( 2
,
ÄÄ2 3
principalColumn
ÅÅ '
:
ÅÅ' (
$str
ÅÅ) 4
)
ÅÅ4 5
;
ÅÅ5 6
}
ÇÇ 
)
ÇÇ 
;
ÇÇ 
migrationBuilder
ÑÑ 
.
ÑÑ 
CreateIndex
ÑÑ (
(
ÑÑ( )
name
ÖÖ 
:
ÖÖ 
$str
ÖÖ 0
,
ÖÖ0 1
table
ÜÜ 
:
ÜÜ 
$str
ÜÜ %
,
ÜÜ% &
column
áá 
:
áá 
$str
áá "
)
áá" #
;
áá# $
migrationBuilder
ââ 
.
ââ 
CreateIndex
ââ (
(
ââ( )
name
ää 
:
ää 
$str
ää 1
,
ää1 2
table
ãã 
:
ãã 
$str
ãã %
,
ãã% &
column
åå 
:
åå 
$str
åå #
)
åå# $
;
åå$ %
migrationBuilder
éé 
.
éé 
CreateIndex
éé (
(
éé( )
name
èè 
:
èè 
$str
èè 6
,
èè6 7
table
êê 
:
êê 
$str
êê &
,
êê& '
column
ëë 
:
ëë 
$str
ëë '
)
ëë' (
;
ëë( )
migrationBuilder
ìì 
.
ìì 
CreateIndex
ìì (
(
ìì( )
name
îî 
:
îî 
$str
îî 1
,
îî1 2
table
ïï 
:
ïï 
$str
ïï &
,
ïï& '
column
ññ 
:
ññ 
$str
ññ "
)
ññ" #
;
ññ# $
migrationBuilder
òò 
.
òò 
CreateIndex
òò (
(
òò( )
name
ôô 
:
ôô 
$str
ôô 2
,
ôô2 3
table
öö 
:
öö 
$str
öö &
,
öö& '
column
õõ 
:
õõ 
$str
õõ #
)
õõ# $
;
õõ$ %
}
úú 	
	protected
üü 
override
üü 
void
üü 
Down
üü  $
(
üü$ %
MigrationBuilder
üü% 5
migrationBuilder
üü6 F
)
üüF G
{
†† 	
migrationBuilder
°° 
.
°° 
	DropTable
°° &
(
°°& '
name
¢¢ 
:
¢¢ 
$str
¢¢ %
)
¢¢% &
;
¢¢& '
migrationBuilder
§§ 
.
§§ 
	DropTable
§§ &
(
§§& '
name
•• 
:
•• 
$str
•• 
)
•• 
;
•• 
migrationBuilder
ßß 
.
ßß 
	DropTable
ßß &
(
ßß& '
name
®® 
:
®® 
$str
®® $
)
®®$ %
;
®®% &
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
´´ 
)
´´  
;
´´  !
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
ÆÆ  
)
ÆÆ  !
;
ÆÆ! "
}
ØØ 	
}
∞∞ 
}±± ù
^C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\MiddleWare\GlobalExceptionHandler.cs
	namespace 	
HealthAxisApplicn
 
. 

MiddleWare &
{ 
public 

class "
GlobalExceptionHandler '
:' (
IExceptionHandler) :
{		 
private

 
readonly

 
ILogger

  
<

  !"
GlobalExceptionHandler

! 7
>

7 8
_logger

9 @
;

@ A
public "
GlobalExceptionHandler %
(% &
ILogger& -
<- ."
GlobalExceptionHandler. D
>D E
loggerF L
)L M
{ 	
_logger 
= 
logger 
; 
} 	
public 
async 
	ValueTask 
< 
bool #
># $
TryHandleAsync% 3
(3 4
HttpContext 
httpContext #
,# $
	Exception 
	exception 
,  
CancellationToken 
cancellationToken /
)/ 0
{ 	
_logger 
. 
LogError 
( 
	exception 
, 
$str b
,b c
httpContext 
. 
Request #
.# $
Path$ (
,( )
httpContext 
. 
Request #
.# $
Method$ *
,* +
	exception 
. 
Message !
)! "
;" #
var 
( 

statusCode 
, 
message $
)$ %
=& '
	exception( 1
switch2 8
{ 
NotFoundException !
=>" $
(% &
StatusCodes& 1
.1 2
Status404NotFound2 C
,C D
	exceptionE N
.N O
MessageO V
)V W
,W X
BadRequestException!! #
=>!!$ &
(!!' (
StatusCodes!!( 3
.!!3 4
Status400BadRequest!!4 G
,!!G H
	exception!!I R
.!!R S
Message!!S Z
)!!Z [
,!![ \
ForbiddenException## "
=>### %
(##& '
StatusCodes##' 2
.##2 3
Status403Forbidden##3 E
,##E F
	exception##G P
.##P Q
Message##Q X
)##X Y
,##Y Z'
UnauthorizedAccessException%% +
=>%%, .
(%%/ 0
StatusCodes%%0 ;
.%%; <!
Status401Unauthorized%%< Q
,%%Q R
	exception%%S \
.%%\ ]
Message%%] d
)%%d e
,%%e f
_'' 
=>'' 
('' 
StatusCodes'' !
.''! "(
Status500InternalServerError''" >
,''> ?
$str''@ _
)''_ `
}(( 
;(( 
var** 
response** 
=** 
new** 
ErrorResponseDto** /
{++ 

StatusCode,, 
=,, 

statusCode,, '
,,,' (
Message-- 
=-- 
message-- !
,--! "
	Timestamp.. 
=.. 
DateTime.. $
...$ %
UtcNow..% +
,..+ ,
Path// 
=// 
httpContext// "
.//" #
Request//# *
.//* +
Path//+ /
}00 
;00 
httpContext22 
.22 
Response22  
.22  !

StatusCode22! +
=22, -

statusCode22. 8
;228 9
await44 
httpContext44 
.44 
Response44 &
.44& '
WriteAsJsonAsync44' 7
(447 8
response55 
,55 
cancellationToken66 !
)66! "
;66" #
return88 
true88 
;88 
}99 	
}:: 
};; ‰
eC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Messaging\Contracts\BookAppointmentEvent.cs
	namespace 	
HealthAxisApplicn
 
. 
	Messaging %
.% &
	Contracts& /
{ 
public 

class  
BookAppointmentEvent %
{ 
public 
Guid 
EventId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
	EventType 
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
public		 
DateTime		 

OccurredAt		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		/ 0
public 
string 
Source 
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
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
} 
} Å
hC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Messaging\Consumers\BookAppointmentConsumer.cs
	namespace 	
HealthAxisApplicn
 
. 
	Messaging %
.% &
	Consumers& /
{ 
public 

class #
BookAppointmentConsumer (
: 	
	IConsumer
 
<  
BookAppointmentEvent (
>( )
{ 
public 
Task 
Consume 
( 
ConsumeContext		 
<		  
BookAppointmentEvent		 /
>		/ 0
context		1 8
)		8 9
{

 	
Console 
. 
	WriteLine 
( 
$str >
)> ?
;? @
Console 
. 
	WriteLine 
( 
$" 
$str 
{  
context  '
.' (
Message( /
./ 0
	EventType0 9
}9 :
": ;
); <
;< =
Console 
. 
	WriteLine 
( 
$" 
$str  
{  !
context! (
.( )
Message) 0
.0 1

OccurredAt1 ;
}; <
"< =
)= >
;> ?
Console 
. 
	WriteLine 
( 
$" 
$str #
{# $
context$ +
.+ ,
Message, 3
.3 4
AppointmentId4 A
}A B
"B C
)C D
;D E
Console 
. 
	WriteLine 
( 
$" 
$str 
{  
context  '
.' (
Message( /
./ 0
	PatientId0 9
}9 :
": ;
); <
;< =
Console 
. 
	WriteLine 
( 
$" 
$str 
{ 
context &
.& '
Message' .
.. /
DoctorId/ 7
}7 8
"8 9
)9 :
;: ;
Console 
. 
	WriteLine 
( 
$str >
)> ?
;? @
return 
Task 
. 
CompletedTask %
;% &
} 	
}   
}"" ·
TC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Mappings\MappingProfile.cs
	namespace 	
HealthAxisApplicn
 
. 
Mappings $
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
	CreateMap 
< 
CreatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
UpdatePatientDto &
,& '
Patient( /
>/ 0
(0 1
)1 2
;2 3
	CreateMap 
< 
Patient 
, 

PatientDto )
>) *
(* +
)+ ,
., -

ReverseMap- 7
(7 8
)8 9
;9 :
	CreateMap 
< 
Doctor 
, 
	DoctorDto '
>' (
(( )
)) *
.* +

ReverseMap+ 5
(5 6
)6 7
;7 8
	CreateMap 
< 
CreateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
UpdateDoctorDto %
,% &
Doctor' -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
Appointment !
,! "
AppointmentDto# 1
>1 2
(2 3
)3 4
. 
	ForMember 
( 
dest 
=> 
dest 
. 

DoctorName '
,' (
opt 
=> 
opt 
. 
MapFrom "
(" #
src# &
=>' )
src* -
.- .
Doctor. 4
.4 5

DoctorName5 ?
)? @
) 
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
PatientName! ,
,, -
opt   
=>   
opt   
.   
MapFrom   &
(  & '
src  ' *
=>  + -
src  . 1
.  1 2
Patient  2 9
.  9 :
PatientName  : E
)  E F
)!! 
;!! 
	CreateMap$$ 
<$$ 
AppointmentDto$$ $
,$$$ %
Appointment$$& 1
>$$1 2
($$2 3
)$$3 4
;$$4 5
	CreateMap%% 
<%%  
CreateAppointmentDto%% *
,%%* +
Appointment%%, 7
>%%7 8
(%%8 9
)%%9 :
;%%: ;
	CreateMap&& 
<&& &
UpdateAppointmentStatusDto&& 0
,&&0 1
Appointment&&2 =
>&&= >
(&&> ?
)&&? @
;&&@ A
	CreateMap)) 
<)) 
HealthRecord)) "
,))" #
HealthRecordDto))$ 3
>))3 4
())4 5
)))5 6
.** 
	ForMember** 
(** 
dest++ 
=>++ 
dest++ 
.++ 

DoctorName++ '
,++' (
opt,, 
=>,, 
opt,, 
.,, 
MapFrom,, "
(,," #
src,,# &
=>,,' )
src,,* -
.,,- .
Doctor,,. 4
.,,4 5

DoctorName,,5 ?
),,? @
)-- 
;-- 
	CreateMap// 
<// 
HealthRecordDto// %
,//% &
HealthRecord//' 3
>//3 4
(//4 5
)//5 6
;//6 7
}00 	
}11 
}22 ∆
YC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Exceptions\NotFoundException.cs
	namespace 	
HealthAxisApplicn
 
. 

Exceptions &
{ 
public 

class 
NotFoundException "
:" #
	Exception$ -
{ 
public 
NotFoundException  
(  !
string! '
message( /
)/ 0
:1 2
base3 7
(7 8
message8 ?
)? @
{ 	
} 	
}		 
}

 …
ZC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Exceptions\ForbiddenException.cs
	namespace 	
HealthAxisApplicn
 
. 

Exceptions &
{ 
public 

class 
ForbiddenException #
:# $
	Exception% .
{ 
public 
ForbiddenException !
(! "
string" (
message) 0
)0 1
:2 3
base4 8
(8 9
message9 @
)@ A
{ 	
} 	
}		 
}

 Ã
[C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Exceptions\BadRequestException.cs
	namespace 	
HealthAxisApplicn
 
. 

Exceptions &
{ 
public 

class 
BadRequestException $
:$ %
	Exception& /
{ 
public 
BadRequestException "
(" #
string# )
message* 1
)1 2
:3 4
base5 9
(9 :
message: A
)A B
{ 	
} 	
}		 
}

 Œ!
LC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Data\RoleSeeder.cs
	namespace 	
HealthAxisApplicn
 
. 
Data  
{ 
public 

static 
class 

RoleSeeder "
{ 
public 
static 
async 
Task  
SeedRoleAsync! .
(. /
RoleManager/ :
<: ;
IdentityRole; G
>G H
roleManagerI T
)T U
{		 	
string

 
[

 
]

 
roles

 
=

 
{

 
$str

 &
,

& '
$str

( 0
,

0 1
$str

2 ;
}

< =
;

= >
foreach 
( 
var 
role 
in  
roles! &
)& '
{ 
if 
( 
! 
await 
roleManager &
.& '
RoleExistsAsync' 6
(6 7
role7 ;
); <
)< =
{ 
await 
roleManager %
.% &
CreateAsync& 1
(1 2
new2 5
IdentityRole6 B
(B C
roleC G
)G H
)H I
;I J
} 
} 
} 	
public 
static 
async 
Task  
SeedAdminAsync! /
(/ 0
UserManager 
< 
ApplicationUser '
>' (
userManager) 4
,4 5
RoleManager 
< 
IdentityRole $
>$ %
roleManager& 1
)1 2
{ 	
string 

adminEmail 
= 
$str  9
;9 :
string 
adminPassword  
=! "
$str# .
;. /
var 
existingAdmin 
= 
await  %
userManager& 1
.1 2
FindByEmailAsync2 B
(B C

adminEmailC M
)M N
;N O
if 
( 
existingAdmin 
!=  
null! %
)% &
return 
; 
var!! 
	adminUser!! 
=!! 
new!! 
ApplicationUser!!  /
{"" 
UserName## 
=## 

adminEmail## %
,##% &
Email$$ 
=$$ 

adminEmail$$ "
,$$" #
EmailConfirmed%% 
=%%  
true%%! %
}&& 
;&& 
var(( 
result(( 
=(( 
await(( 
userManager(( *
.((* +
CreateAsync((+ 6
(((6 7
	adminUser((7 @
,((@ A
adminPassword((B O
)((O P
;((P Q
if** 
(** 
!** 
result** 
.** 
	Succeeded** !
)**! "
{++ 
var,, 
errors,, 
=,, 
string,, #
.,,# $
Join,,$ (
(,,( )
$str,,) -
,,,- .
result,,/ 5
.,,5 6
Errors,,6 <
.,,< =
Select,,= C
(,,C D
e,,D E
=>,,F H
e,,I J
.,,J K
Description,,K V
),,V W
),,W X
;,,X Y
throw-- 
new-- 
	Exception-- #
(--# $
$"--$ &
$str--& =
{--= >
errors--> D
}--D E
"--E F
)--F G
;--G H
}.. 
if00 
(00 
!00 
await00 
roleManager00 "
.00" #
RoleExistsAsync00# 2
(002 3
$str003 :
)00: ;
)00; <
{11 
await22 
roleManager22 !
.22! "
CreateAsync22" -
(22- .
new22. 1
IdentityRole222 >
(22> ?
$str22? F
)22F G
)22G H
;22H I
}33 
await55 
userManager55 
.55 
AddToRoleAsync55 ,
(55, -
	adminUser55- 6
,556 7
$str558 ?
)55? @
;55@ A
}66 	
}77 
}88 ‰+
NC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Data\AppDbContext.cs
	namespace 
HealthAxisApplicn 
.  
Data  $
{ 
public 
class 
AppDbContext !
:" #
IdentityDbContext$ 5
<5 6
ApplicationUser6 E
>E F
{		 	
public

 
AppDbContext

 
(

  
DbContextOptions

  0
<

0 1
AppDbContext

1 =
>

= >
options

? F
)

F G
:

H I
base

J N
(

N O
options

O V
)

V W
{ 
} 
public 
DbSet 
< 
Patient  
>  !
Patients" *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
DbSet 
< 
Doctor 
>  
Doctors! (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
DbSet 
< 
Appointment $
>$ %
Appointments& 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
public 
DbSet 
< 
HealthRecord %
>% &
HealthRecords' 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 
DbSet 
< 
RefreshToken %
>% &
RefreshTokens' 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
	protected 
override 
void #
OnModelCreating$ 3
(3 4
ModelBuilder4 @
modelBuilderA M
)M N
{ 
modelBuilder 
. 
Entity #
<# $
HealthRecord$ 0
>0 1
(1 2
)2 3
. 
HasOne 
( 
hr 
=> !
hr" $
.$ %
Doctor% +
)+ ,
. 
WithMany 
( 
) 
. 
HasForeignKey "
(" #
hr# %
=>& (
hr) +
.+ ,
DoctorId, 4
)4 5
. 
OnDelete 
( 
DeleteBehavior ,
., -
NoAction- 5
)5 6
;6 7
modelBuilder 
. 
Entity #
<# $
HealthRecord$ 0
>0 1
(1 2
)2 3
. 
HasOne 
( 
hr 
=> !
hr" $
.$ %
Patient% ,
), -
. 
WithMany 
( 
) 
.   
HasForeignKey   "
(  " #
hr  # %
=>  & (
hr  ) +
.  + ,
	PatientId  , 5
)  5 6
.!! 
OnDelete!! 
(!! 
DeleteBehavior!! ,
.!!, -
NoAction!!- 5
)!!5 6
;!!6 7
modelBuilder## 
.## 
Entity## #
<### $
HealthRecord##$ 0
>##0 1
(##1 2
)##2 3
.$$ 
HasOne$$ 
($$ 
hr$$ 
=>$$ !
hr$$" $
.$$$ %
Appointment$$% 0
)$$0 1
.%% 
WithOne%% 
(%% 
)%% 
.&& 
HasForeignKey&& "
<&&" #
HealthRecord&&# /
>&&/ 0
(&&0 1
hr&&1 3
=>&&4 6
hr&&7 9
.&&9 :
AppointmentId&&: G
)&&G H
;&&H I
base(( 
.(( 
OnModelCreating(( $
((($ %
modelBuilder((% 1
)((1 2
;((2 3
modelBuilder)) 
.)) 
Entity)) #
<))# $
ApplicationUser))$ 3
>))3 4
())4 5
)))5 6
.** 
HasOne** 
(** 
u** 
=>**  
u**! "
.**" #
Patient**# *
)*** +
.++ 
WithOne++ 
(++ 
p++ 
=>++ !
p++" #
.++# $
User++$ (
)++( )
.,, 
HasForeignKey,, "
<,," #
Patient,,# *
>,,* +
(,,+ ,
p,,, -
=>,,. 0
p,,1 2
.,,2 3
UserId,,3 9
),,9 :
.-- 
OnDelete-- 
(-- 
DeleteBehavior-- ,
.--, -
NoAction--- 5
)--5 6
;--6 7
modelBuilder// 
.// 
Entity// #
<//# $
ApplicationUser//$ 3
>//3 4
(//4 5
)//5 6
.00 
HasOne00 
(00 
u00 
=>00  
u00! "
.00" #
Doctor00# )
)00) *
.11 
WithOne11 
(11 
d11 
=>11 !
d11" #
.11# $
User11$ (
)11( )
.22 
HasForeignKey22 "
<22" #
Doctor22# )
>22) *
(22* +
d22+ ,
=>22- /
d220 1
.221 2
UserId222 8
)228 9
.33 
OnDelete33 
(33 
DeleteBehavior33 ,
.33, -
NoAction33- 5
)335 6
;336 7
}66 
}77 	
}88 Ôf
ZC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Controllers\PatientController.cs
	namespace		 	
HealthAxisApplicn		
 
.		 
Controllers		 '
{

 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class 
PatientController "
(" #
IPatientService# 2
service3 :
): ;
:< =
ControllerBase> L
{ 
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
)0 1
{ 	
var 
result 
= 
await 
service &
.& '
GetAllAsync' 2
(2 3
)3 4
;4 5
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
[1 2
	FromRoute2 ;
]; <
int= @
idA C
)C D
{ 	
var 
result 
= 
await 
service &
.& '
GetByIdAsync' 3
(3 4
id4 6
)6 7
;7 8
if 
( 
result 
is 
null 
) 
return  &
NotFound' /
(/ 0
)0 1
;1 2
return 
Ok 
( 
result 
) 
; 
} 	
[!! 	
HttpPost!!	 
]!! 
["" 	
AllowAnonymous""	 
]"" 
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
Create##) /
(##/ 0
[##0 1
FromBody##1 9
]##9 :
CreatePatientDto##; K
entity##L R
)##R S
{$$ 	
if%% 
(%% 
!%% 

ModelState%% 
.%% 
IsValid%% "
)%%" #
{&& 
return'' 

BadRequest'' !
(''! "
)''" #
;''# $
}(( 
var)) 
result)) 
=)) 
await)) 
service)) &
.))& '
CreateAsync))' 2
())2 3
entity))3 9
)))9 :
;)): ;
if** 
(** 
result** 
is** 
null** 
)** 
return**  &
NotFound**' /
(**/ 0
)**0 1
;**1 2
return++ 
CreatedAtAction++ "
(++" #
$str++# ,
,++, -
new++. 1
{++2 3
id++3 5
=++6 7
result++8 >
.++> ?
	PatientId++? H
}++H I
,++I J
result++K Q
)++Q R
;++R S
},, 	
[// 	
HttpPut//	 
(// 
$str// 
)// 
]// 
[00 	
	Authorize00	 
(00 !
AuthenticationSchemes00 (
=00) *
JwtBearerDefaults00+ <
.00< = 
AuthenticationScheme00= Q
,00Q R
Roles00S X
=00Y Z
$str00[ b
)00b c
]00c d
public11 
async11 
Task11 
<11 
IActionResult11 '
>11' (
Update11) /
(11/ 0
int110 3
id114 6
,116 7
[118 9
FromBody119 A
]11A B
UpdatePatientDto11C S
entity11T Z
)11Z [
{22 	
if33 
(33 
!33 

ModelState33 
.33 
IsValid33 "
)33" #
{44 
return55 

BadRequest55 !
(55! "

ModelState55" ,
)55, -
;55- .
}66 
var77 
result77 
=77 
await77 
service77 &
.77& '
UpdateAsync77' 2
(772 3
id773 5
,775 6
entity777 =
)77= >
;77> ?
if88 
(88 
result88 
is88 
null88 
)88 
return88  &
NotFound88' /
(88/ 0
)880 1
;881 2
return99 
Ok99 
(99 
result99 
)99 
;99 
}:: 	
[<< 	
HttpPut<<	 
(<< 
$str<< "
)<<" #
]<<# $
[== 	
	Authorize==	 
(== !
AuthenticationSchemes== (
===) *
JwtBearerDefaults==+ <
.==< = 
AuthenticationScheme=== Q
,==Q R
Roles==S X
===Y Z
$str==[ b
)==b c
]==c d
public>> 
async>> 
Task>> 
<>> 
IActionResult>> '
>>>' (
Toggle>>) /
(>>/ 0
int>>0 3
id>>4 6
)>>6 7
{?? 	
var@@ 
result@@ 
=@@ 
await@@ 
service@@ &
.@@& '"
DeactivatePatientAsync@@' =
(@@= >
id@@> @
)@@@ A
;@@A B
returnAA 
OkAA 
(AA 
resultAA 
)AA 
;AA 
}BB 	
[DD 	
HttpGetDD	 
(DD 
$strDD 
)DD 
]DD  
[EE 	
	AuthorizeEE	 
(EE !
AuthenticationSchemesEE (
=EE) *
JwtBearerDefaultsEE+ <
.EE< = 
AuthenticationSchemeEE= Q
,EEQ R
RolesEES X
=EEY Z
$strEE[ b
)EEb c
]EEc d
publicFF 
asyncFF 
TaskFF 
<FF 
IActionResultFF '
>FF' (
GetByPatientNameFF) 9
(FF9 :
stringFF: @
nameFFA E
)FFE F
{GG 	
varHH 
resultHH 
=HH 
awaitHH 
serviceHH &
.HH& '$
SearchByPatientNameAsyncHH' ?
(HH? @
nameHH@ D
)HHD E
;HHE F
ifII 
(II 
resultII 
.II 
CountII 
==II 
$numII  !
)II! "
returnII# )
NotFoundII* 2
(II2 3
)II3 4
;II4 5
returnJJ 
OkJJ 
(JJ 
resultJJ 
)JJ 
;JJ 
}KK 	
[MM 	
HttpGetMM	 
(MM 
$strMM 
)MM 
]MM 
[NN 	
	AuthorizeNN	 
(NN !
AuthenticationSchemesNN (
=NN) *
JwtBearerDefaultsNN+ <
.NN< = 
AuthenticationSchemeNN= Q
,NNQ R
RolesNNS X
=NNY Z
$strNN[ b
)NNb c
]NNc d
publicOO 
asyncOO 
TaskOO 
<OO 
IActionResultOO '
>OO' (
SearchOO) /
(OO/ 0
[PP 	
	FromQueryPP	 
]PP 
stringPP 
?PP 
namePP  
,PP  !
[QQ 	
	FromQueryQQ	 
]QQ 
stringQQ 
?QQ 
phoneQQ !
)QQ! "
{RR 	
varSS 
resultSS 
=SS 
awaitSS 
serviceSS "
.SS" #
SearchAsyncSS# .
(SS. /
nameSS/ 3
,SS3 4
phoneSS5 :
)SS: ;
;SS; <
returnTT 
OkTT 
(TT 
resultTT 
)TT 
;TT 
}UU 	
[WW 	
HttpGetWW	 
(WW 
$strWW 
)WW 
]WW 
[XX 	
	AuthorizeXX	 
(XX 
RolesXX 
=XX 
$strXX $
)XX$ %
]XX% &
publicYY 
asyncYY 
TaskYY 
<YY 
IActionResultYY '
>YY' (
GetCurrentPatientYY) :
(YY: ;
)YY; <
{ZZ 	
var[[ 
userId[[ 
=[[ 
User[[ 
.[[ 
	FindFirst[[ '
([[' (

ClaimTypes[[( 2
.[[2 3
NameIdentifier[[3 A
)[[A B
?[[B C
.[[C D
Value[[D I
;[[I J
if]] 
(]] 
string]] 
.]] 
IsNullOrEmpty]] $
(]]$ %
userId]]% +
)]]+ ,
)]], -
return^^ 
Unauthorized^^ #
(^^# $
)^^$ %
;^^% &
var`` 
patient`` 
=`` 
await`` 
service``  '
.``' (
GetByUserIdAsync``( 8
(``8 9
userId``9 ?
)``? @
;``@ A
ifbb 
(bb 
patientbb 
==bb 
nullbb 
)bb  
returncc 
NotFoundcc 
(cc  
)cc  !
;cc! "
returnee 
Okee 
(ee 
newee 
{ff 
patientNamegg 
=gg 
patientgg %
.gg% &
PatientNamegg& 1
}hh 
)hh 
;hh 
}ii 	
[kk 	
HttpGetkk	 
(kk 
$strkk 
)kk 
]kk 
[ll 	
	Authorizell	 
(ll 
Rolesll 
=ll 
$strll $
)ll$ %
]ll% &
publicmm 
asyncmm 
Taskmm 
<mm 
IActionResultmm '
>mm' (

GetProfilemm) 3
(mm3 4
)mm4 5
{nn 	
varoo 
userIdoo 
=oo 
Useroo 
.oo 
	FindFirstoo '
(oo' (
Systempp 
.pp 
Securitypp 
.pp  
Claimspp  &
.pp& '

ClaimTypespp' 1
.pp1 2
NameIdentifierpp2 @
)pp@ A
?ppA B
.ppB C
ValueppC H
;ppH I
ifrr 
(rr 
stringrr 
.rr 
IsNullOrEmptyrr $
(rr$ %
userIdrr% +
)rr+ ,
)rr, -
returnss 
Unauthorizedss #
(ss# $
)ss$ %
;ss% &
varuu 
patientuu 
=uu 
awaituu 
serviceuu  '
.uu' (
GetByUserIdAsyncuu( 8
(uu8 9
userIduu9 ?
)uu? @
;uu@ A
ifww 
(ww 
patientww 
==ww 
nullww 
)ww  
returnxx 
NotFoundxx 
(xx  
)xx  !
;xx! "
returnzz 
Okzz 
(zz 
patientzz 
)zz 
;zz 
}{{ 	
[}} 	
HttpGet}}	 
(}} 
$str}} 1
)}}1 2
]}}2 3
[~~ 	
	Authorize~~	 
(~~ 
Roles~~ 
=~~ 
$str~~ #
)~~# $
]~~$ %
public 
async 
Task 
< 
IActionResult '
>' (
GetPatientDetails) :
(: ;
int; >
	patientId? H
)H I
{
ÄÄ 	
var
ÅÅ 
doctorId
ÅÅ 
=
ÅÅ 
int
ÇÇ 
.
ÇÇ 
Parse
ÇÇ 
(
ÇÇ 
User
ÇÇ 
.
ÇÇ 
	FindFirst
ÇÇ (
(
ÇÇ( )
$str
ÇÇ) 3
)
ÇÇ3 4
!
ÇÇ4 5
.
ÇÇ5 6
Value
ÇÇ6 ;
)
ÇÇ; <
;
ÇÇ< =
var
ÑÑ 
result
ÑÑ 
=
ÑÑ 
await
ÖÖ 
service
ÖÖ 
.
ÖÖ -
GetPatientDetailsForDoctorAsync
ÖÖ =
(
ÖÖ= >
doctorId
ÜÜ 
,
ÜÜ 
	patientId
áá 
)
áá 
;
áá 
if
ââ 
(
ââ 
result
ââ 
==
ââ 
null
ââ 
)
ââ 
return
ää 
NotFound
ää 
(
ää  
)
ää  !
;
ää! "
return
åå 
Ok
åå 
(
åå 
result
åå 
)
åå 
;
åå 
}
çç 	
}
èè 
}êê Á5
_C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Controllers\HealthRecordController.cs
	namespace 	
HealthAxisApplicn
 
. 
Controllers '
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		  
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
HealthRecordController '
(' ( 
IHealthRecordService( <
service= D
)D E
:F G
ControllerBaseH V
{ 
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str $
)$ %
]% &
public 
async 
Task 
< 
IActionResult '
>' (
GetMyRecords) 5
(5 6
)6 7
{ 	
var 
	patientId 
= 
int 
.  
Parse  %
(% &
User& *
.* +
	FindFirst+ 4
(4 5
$str5 @
)@ A
!A B
.B C
ValueC H
)H I
;I J
var 
result 
= 
await 
service &
.& '&
GetRecordsByPatientIdAsync' A
(A B
	patientIdB K
)K L
;L M
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str #
)# $
]$ %
public 
async 
Task 
< 
IActionResult '
>' (
GetDoctorRecords) 9
(9 :
): ;
{ 	
var 
doctorId 
= 
int 
. 
Parse $
($ %
User% )
.) *
	FindFirst* 3
(3 4
$str4 >
)> ?
!? @
.@ A
ValueA F
)F G
;G H
var!! 
result!! 
=!! 
await!! 
service!! &
.!!& '%
GetRecordsByDoctorIdAsync!!' @
(!!@ A
doctorId!!A I
)!!I J
;!!J K
return## 
Ok## 
(## 
result## 
)## 
;## 
}$$ 	
['' 	
HttpGet''	 
('' 
$str'' 
)'' 
]'' 
public(( 
async(( 
Task(( 
<(( 
IActionResult(( '
>((' (
GetById(() 0
(((0 1
int((1 4
id((5 7
)((7 8
{)) 	
var** 
result** 
=** 
await** 
service** &
.**& '
GetByIdAsync**' 3
(**3 4
id**4 6
)**6 7
;**7 8
if,, 
(,, 
result,, 
is,, 
null,, 
),, 
return-- 
NotFound-- 
(--  
)--  !
;--! "
return// 
Ok// 
(// 
result// 
)// 
;// 
}00 	
[33 	
HttpPut33	 
(33 
$str33 
)33 
]33 
[44 	
	Authorize44	 
(44 
Roles44 
=44 
$str44 #
)44# $
]44$ %
public55 
async55 
Task55 
<55 
IActionResult55 '
>55' (
Update55) /
(55/ 0
int550 3
id554 6
,556 7!
UpdateHealthRecordDto558 M
dto55N Q
)55Q R
{66 	
if77 
(77 
!77 

ModelState77 
.77 
IsValid77 #
)77# $
return88 

BadRequest88 !
(88! "

ModelState88" ,
)88, -
;88- .
var:: 
result:: 
=:: 
await:: 
service:: &
.::& '
UpdateAsync::' 2
(::2 3
id::3 5
,::5 6
dto::7 :
)::: ;
;::; <
if<< 
(<< 
result<< 
is<< 
null<< 
)<< 
return== 
NotFound== 
(==  
)==  !
;==! "
return?? 
Ok?? 
(?? 
result?? 
)?? 
;?? 
}@@ 	
[CC 	
HttpGetCC	 
(CC 
$strCC 2
)CC2 3
]CC3 4
[DD 	
	AuthorizeDD	 
(DD 
RolesDD 
=DD 
$strDD #
)DD# $
]DD$ %
publicEE 
asyncEE 
TaskEE 
<EE 
IActionResultEE '
>EE' (
GetByAppointmentEE) 9
(EE9 :
intEE: =
appointmentIdEE> K
)EEK L
{FF 	
varGG 
resultGG 
=GG 
awaitGG 
serviceGG &
.GG& '#
GetByAppointmentIdAsyncGG' >
(GG> ?
appointmentIdGG? L
)GGL M
;GGM N
ifII 
(II 
resultII 
==II 
nullII 
)II 
returnJJ 
NotFoundJJ 
(JJ  
)JJ  !
;JJ! "
returnLL 
OkLL 
(LL 
resultLL 
)LL 
;LL 
}MM 	
[OO 	
HttpGetOO	 
(OO 
$strOO "
)OO" #
]OO# $
[PP 	
	AuthorizePP	 
(PP 
RolesPP 
=PP 
$strPP #
)PP# $
]PP$ %
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
IActionResultQQ '
>QQ' (
GetDoctorPatientsQQ) :
(QQ: ;
)QQ; <
{RR 	
varSS 
doctorIdSS 
=SS 
intSS 
.SS 
ParseSS $
(SS$ %
UserTT 
.TT 
	FindFirstTT 
(TT 
$strTT )
)TT) *
!TT* +
.TT+ ,
ValueTT, 1
)UU 
;UU 
varWW 
resultWW 
=WW 
awaitXX 
serviceXX 
.XX "
GetDoctorPatientsAsyncXX 4
(XX4 5
doctorIdXX5 =
)XX= >
;XX> ?
returnZZ 
OkZZ 
(ZZ 
resultZZ 
)ZZ 
;ZZ 
}[[ 	
}]] 
}^^ “k
YC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Controllers\DoctorController.cs
	namespace 	
HealthAxisApplicn
 
. 
Controllers '
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
 
)

 
]

 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class 
DoctorController !
(! "
IDoctorService" 0
service1 8
)8 9
:: ;
ControllerBase< J
{ 
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes (
=) *
JwtBearerDefaults+ <
.< = 
AuthenticationScheme= Q
,Q R
RolesS X
=Y Z
$str[ b
)b c
]c d
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
)0 1
{ 	
var 
result 
= 
await 
service &
.& '
GetAllAsync' 2
(2 3
)3 4
;4 5
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
[1 2
	FromRoute2 ;
]; <
int= @
idA C
)C D
{ 	
var 
result 
= 
await 
service &
.& '
GetByIdAsync' 3
(3 4
id4 6
)6 7
;7 8
if 
( 
result 
is 
null 
) 
return  &
NotFound' /
(/ 0
)0 1
;1 2
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
HttpPut	 
( 
$str 
) 
]  
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
$str  [ b
)  b c
]  c d
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' (
ToggleActive!!) 5
(!!5 6
int!!6 9
id!!: <
)!!< =
{"" 	
var## 
result## 
=## 
await## 
service## &
.##& '
ToggleActiveAsync##' 8
(##8 9
id##9 ;
)##; <
;##< =
if%% 
(%% 
!%% 
result%% 
)%% 
return&& 
NotFound&& 
(&&  
)&&  !
;&&! "
return(( 
	NoContent(( 
((( 
)(( 
;(( 
})) 	
[,, 	
HttpPost,,	 
],, 
[-- 	
	Authorize--	 
(-- !
AuthenticationSchemes-- (
=--) *
JwtBearerDefaults--+ <
.--< = 
AuthenticationScheme--= Q
,--Q R
Roles--S X
=--Y Z
$str--[ b
)--b c
]--c d
public.. 
async.. 
Task.. 
<.. 
IActionResult.. '
>..' (
Create..) /
(../ 0
[..0 1
FromBody..1 9
]..9 :
CreateDoctorDto..; J
entity..K Q
)..Q R
{// 	
if00 
(00 
!00 

ModelState00 
.00 
IsValid00 #
)00# $
{11 
return22 

BadRequest22 !
(22! "

ModelState22" ,
)22, -
;22- .
}33 
var55 
result55 
=55 
await55 
service55 &
.55& '
CreateAsync55' 2
(552 3
entity553 9
)559 :
;55: ;
return77 
Ok77 
(77 
result77 
)77 
;77 
}88 
[;; 	
HttpPut;;	 
(;; 
$str;; 
);; 
];; 
[<< 	
	Authorize<<	 
(<< !
AuthenticationSchemes<< (
=<<) *
JwtBearerDefaults<<+ <
.<<< = 
AuthenticationScheme<<= Q
,<<Q R
Roles<<S X
=<<Y Z
$str<<[ b
)<<b c
]<<c d
public== 
async== 
Task== 
<== 
IActionResult== '
>==' (
Update==) /
(==/ 0
int==0 3
id==4 6
,==6 7
[==8 9
FromBody==9 A
]==A B
UpdateDoctorDto==C R
entity==S Y
)==Y Z
{>> 	
if?? 
(?? 
!?? 

ModelState?? 
.?? 
IsValid?? #
)??# $
{@@ 
returnAA 

BadRequestAA !
(AA! "

ModelStateAA" ,
)AA, -
;AA- .
}BB 
varCC 
resultCC 
=CC 
awaitCC 
serviceCC &
.CC& '
UpdateAsyncCC' 2
(CC2 3
idCC3 5
,CC5 6
entityCC7 =
)CC= >
;CC> ?
ifDD 
(DD 
resultDD 
isDD 
nullDD 
)DD 
returnDD  &
NotFoundDD' /
(DD/ 0
)DD0 1
;DD1 2
returnEE 
OkEE 
(EE 
resultEE 
)EE 
;EE 
}FF 	
[HH 	
HttpGetHH	 
(HH 
$strHH 
)HH 
]HH 
publicII 
asyncII 
TaskII 
<II 
IActionResultII '
>II' (
GetActiveDoctorsII) 9
(II9 :
)II: ;
{JJ 	
returnKK 
OkKK 
(KK 
awaitKK 
serviceKK #
.KK# $!
GetActiveDoctorsAsyncKK$ 9
(KK9 :
)KK: ;
)KK; <
;KK< =
}LL 	
[NN 	
HttpGetNN	 
(NN 
$strNN 
)NN 
]NN  
publicOO 
asyncOO 
TaskOO 
<OO 
IActionResultOO '
>OO' (
SearchByNameOO) 5
(OO5 6
stringOO6 <
nameOO= A
)OOA B
{PP 	
varQQ 
resultQQ 
=QQ 
awaitQQ 
serviceQQ &
.QQ& '
SearchByNameAsyncQQ' 8
(QQ8 9
nameQQ9 =
)QQ= >
;QQ> ?
returnRR 
resultRR 
.RR 
CountRR 
==RR  "
$numRR# $
?RR% &
NotFoundRR' /
(RR/ 0
)RR0 1
:RR2 3
OkRR4 6
(RR6 7
resultRR7 =
)RR= >
;RR> ?
}SS 	
[UU 	
HttpGetUU	 
(UU 
$strUU 2
)UU2 3
]UU3 4
publicVV 
asyncVV 
TaskVV 
<VV 
IActionResultVV '
>VV' ("
SearchBySpecialisationVV) ?
(VV? @
stringVV@ F
specialisationVVG U
)VVU V
{WW 	
varXX 
resultXX 
=XX 
awaitXX 
serviceXX &
.XX& ''
SearchBySpecialisationAsyncXX' B
(XXB C
specialisationXXC Q
)XXQ R
;XXR S
returnYY 
resultYY 
.YY 
CountYY 
==YY  "
$numYY# $
?YY% &
NotFoundYY' /
(YY/ 0
)YY0 1
:YY2 3
OkYY4 6
(YY6 7
resultYY7 =
)YY= >
;YY> ?
}ZZ 	
[]] 	
HttpGet]]	 
(]] 
$str]] 
)]] 
]]] 
[^^ 	
	Authorize^^	 
(^^ !
AuthenticationSchemes^^ (
=^^) *
JwtBearerDefaults^^+ <
.^^< = 
AuthenticationScheme^^= Q
,^^Q R
Policy^^S Y
=^^Z [
$str^^\ l
)^^l m
]^^m n
public__ 
async__ 
Task__ 
<__ 
IActionResult__ '
>__' (
Search__) /
(__/ 0
[__0 1
	FromQuery__1 :
]__: ;
string__< B
query__C H
)__H I
{`` 	
varaa 
doctorsaa 
=aa 
awaitaa 
serviceaa  '
.aa' (
SearchAsyncaa( 3
(aa3 4
queryaa4 9
)aa9 :
;aa: ;
returnbb 
Okbb 
(bb 
doctorsbb 
)bb 
;bb 
}cc 	
[ee 	
HttpGetee	 
(ee 
$stree 
)ee 
]ee 
[ff 	
	Authorizeff	 
(ff 
Rolesff 
=ff 
$strff #
)ff# $
]ff$ %
publicgg 
asyncgg 
Taskgg 
<gg 
IActionResultgg '
>gg' (
GetCurrentDoctorgg) 9
(gg9 :
)gg: ;
{hh 	
varii 
userIdii 
=ii 
Userii 
.ii 
	FindFirstii '
(ii' (
Systemii( .
.ii. /
Securityii/ 7
.ii7 8
Claimsii8 >
.ii> ?

ClaimTypesii? I
.iiI J
NameIdentifieriiJ X
)iiX Y
?iiY Z
.iiZ [
Valueii[ `
;ii` a
ifkk 
(kk 
stringkk 
.kk 
IsNullOrEmptykk $
(kk$ %
userIdkk% +
)kk+ ,
)kk, -
returnll 
Unauthorizedll #
(ll# $
)ll$ %
;ll% &
varnn 
doctornn 
=nn 
awaitnn 
servicenn &
.nn& '
GetByUserIdAsyncnn' 7
(nn7 8
userIdnn8 >
)nn> ?
;nn? @
ifpp 
(pp 
doctorpp 
==pp 
nullpp 
)pp 
returnqq 
NotFoundqq 
(qq  
)qq  !
;qq! "
returnss 
Okss 
(ss 
newss 
{tt 

doctorNameuu 
=uu 
doctoruu #
.uu# $

DoctorNameuu$ .
}vv 
)vv 
;vv 
}ww 	
[yy 	
HttpGetyy	 
(yy 
$stryy 
)yy 
]yy 
[zz 	
	Authorizezz	 
(zz 
Roleszz 
=zz 
$strzz #
)zz# $
]zz$ %
public{{ 
async{{ 
Task{{ 
<{{ 
IActionResult{{ '
>{{' (

GetProfile{{) 3
({{3 4
){{4 5
{|| 	
var}} 
userId}} 
=}} 
User}} 
.}} 
	FindFirst}} '
(}}' (
System~~ 
.~~ 
Security~~ 
.~~  
Claims~~  &
.~~& '

ClaimTypes~~' 1
.~~1 2
NameIdentifier~~2 @
)~~@ A
?~~A B
.~~B C
Value~~C H
;~~H I
if
ÄÄ 
(
ÄÄ 
string
ÄÄ 
.
ÄÄ 
IsNullOrEmpty
ÄÄ $
(
ÄÄ$ %
userId
ÄÄ% +
)
ÄÄ+ ,
)
ÄÄ, -
return
ÅÅ 
Unauthorized
ÅÅ #
(
ÅÅ# $
)
ÅÅ$ %
;
ÅÅ% &
var
ÉÉ 
doctor
ÉÉ 
=
ÉÉ 
await
ÉÉ 
service
ÉÉ &
.
ÉÉ& '
GetByUserIdAsync
ÉÉ' 7
(
ÉÉ7 8
userId
ÉÉ8 >
)
ÉÉ> ?
;
ÉÉ? @
if
ÖÖ 
(
ÖÖ 
doctor
ÖÖ 
==
ÖÖ 
null
ÖÖ 
)
ÖÖ 
return
ÜÜ 
NotFound
ÜÜ 
(
ÜÜ  
)
ÜÜ  !
;
ÜÜ! "
return
àà 
Ok
àà 
(
àà 
doctor
àà 
)
àà 
;
àà 
}
ââ 	
[
åå 	
HttpGet
åå	 
(
åå 
$str
åå 
)
åå 
]
åå 
[
çç 	
	Authorize
çç	 
(
çç #
AuthenticationSchemes
çç (
=
çç) *
JwtBearerDefaults
çç+ <
.
çç< ="
AuthenticationScheme
çç= Q
,
ççQ R
Policy
ççS Y
=
ççZ [
$str
çç\ l
)
ççl m
]
ççm n
public
éé 
async
éé 
Task
éé 
<
éé 
IActionResult
éé '
>
éé' (
Filter
éé) /
(
éé/ 0
[
èè 
	FromQuery
èè 
]
èè 
string
èè 
?
èè 
name
èè 
,
èè 
[
êê 
	FromQuery
êê 
]
êê 
string
êê 
?
êê 
specialization
êê &
)
êê& '
{
ëë 	
var
íí 
doctors
íí 
=
íí 
await
íí 
service
íí  '
.
íí' (
FilterAsync
íí( 3
(
íí3 4
name
íí4 8
,
íí8 9
specialization
íí: H
)
ííH I
;
ííI J
return
ìì 
Ok
ìì 
(
ìì 
doctors
ìì 
)
ìì 
;
ìì 
}
îî 	
}
ññ 
}óó û.
WC:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Controllers\AuthController.cs
	namespace 	
HealthAxisApplicn
 
. 
Controllers '
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
 
)

 
]

 
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
IAuthService %
authService& 1
;1 2
private 
readonly 
UserManager $
<$ %
ApplicationUser% 4
>4 5
userManager6 A
;A B
public 
AuthController 
( 
IAuthService *
authService+ 6
,6 7
UserManager )
<) *
ApplicationUser* 9
>9 :
userManager; F
)F G
{ 	
this 
. 
authService 
= 
authService *
;* +
this 
. 
userManager 
= 
userManager *
;* +
} 	
[ 	
HttpPost	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Register) 1
(1 2
RegisterDto2 =
request> E
)E F
{ 	
var 
result 
= 
await 
authService *
.* +
RegisterAsync+ 8
(8 9
request9 @
)@ A
;A B
if 
( 
! 
result 
. 
Success 
)  
return 

BadRequest !
(! "
result" (
.( )
Message) 0
)0 1
;1 2
return   
Ok   
(   
result   
)   
;   
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
await&& 
authService&& *
.&&* +

LoginAsync&&+ 5
(&&5 6
request&&6 =
)&&= >
;&&> ?
if(( 
((( 
string(( 
.(( 
IsNullOrEmpty(( $
((($ %
result((% +
.((+ ,
AccessToken((, 7
)((7 8
)((8 9
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
(-- 
result-- 
)-- 
;-- 
}.. 	
[00 	
HttpPost00	 
(00 
$str00 
)00 
]00 
public11 
async11 
Task11 
<11 
IActionResult11 '
>11' (
Refresh11) 0
(110 1
[111 2
FromBody112 :
]11: ;
string11< B
refreshToken11C O
)11O P
{22 	
var33 
response33 
=33 
await33  
authService33! ,
.33, -
RefreshAsync33- 9
(339 :
refreshToken33: F
)33F G
;33G H
if55 
(55 
response55 
==55 
null55  
)55  !
{66 
return77 
Unauthorized77 #
(77# $
new77$ '
{77( )
message77* 1
=772 3
$str774 V
}77W X
)77X Y
;77Y Z
}88 
return:: 
Ok:: 
(:: 
response:: 
):: 
;::  
};; 	
[== 	
	Authorize==	 
]== 
[>> 	
HttpPost>>	 
(>> 
$str>> #
)>># $
]>>$ %
public?? 
async?? 
Task?? 
<?? 
IActionResult?? '
>??' (
ChangePassword??) 7
(??7 8
[??8 9
FromBody??9 A
]??A B
ChangePasswordDto??C T
dto??U X
)??X Y
{@@ 	
varAA 
userAA 
=AA 
awaitAA 
userManagerAA (
.AA( )
GetUserAsyncAA) 5
(AA5 6
UserAA6 :
)AA: ;
;AA; <
ifCC 
(CC 
userCC 
==CC 
nullCC 
)CC 
returnDD 
UnauthorizedDD #
(DD# $
)DD$ %
;DD% &
userFF 
.FF 
PasswordHashFF 
=FF 
userManagerFF  +
.FF+ ,
PasswordHasherFF, :
.FF: ;
HashPasswordFF; G
(FFG H
userFFH L
,FFL M
dtoFFN Q
.FFQ R
NewPasswordFFR ]
)FF] ^
;FF^ _
userGG 
.GG 
IsFirstLoginGG 
=GG 
falseGG  %
;GG% &
awaitII 
userManagerII 
.II 
UpdateAsyncII )
(II) *
userII* .
)II. /
;II/ 0
returnKK 
OkKK 
(KK 
newKK 
{KK 
messageKK #
=KK$ %
$strKK& :
}KK; <
)KK< =
;KK= >
}LL 	
}MM 
}NN ôj
^C:\Users\310033\source\repos\HealthAxis\HealthAxisApplicn\Controllers\AppointmentController.cs
	namespace		 	
HealthAxisApplicn		
 
.		 
Controllers		 '
{

 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class !
AppointmentController &
(& '
IAppointmentService' :
service; B
)B C
:D E
ControllerBaseF T
{ 
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
int0 3
page4 8
=9 :
$num; <
,< =
int= @
pageSizeA I
=J K
$numL N
)N O
{ 	
var 
result 
= 
await 
service 
. 
GetAllAsync )
() *
page 
, 
pageSize 
) 
; 
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
[1 2
	FromRoute2 ;
]; <
int= @
idA C
)C D
{ 	
var 
result 
= 
await 
service &
.& '
GetByIdAsync' 3
(3 4
id4 6
)6 7
;7 8
if   
(   
result   
is   
null   
)   
return    &
NotFound  ' /
(  / 0
)  0 1
;  1 2
return!! 
Ok!! 
(!! 
result!! 
)!! 
;!! 
}"" 	
[$$ 	
HttpPost$$	 
]$$ 
[%% 	
	Authorize%%	 
(%% 
Roles%% 
=%% 
$str%% $
)%%$ %
]%%% &
public&& 
async&& 
Task&& 
<&& 
IActionResult&& '
>&&' (
Create&&) /
(&&/ 0
[&&0 1
FromBody&&1 9
]&&9 : 
CreateAppointmentDto&&; O
entity&&P V
)&&V W
{'' 	
try(( 
{)) 
if** 
(** 
!** 

ModelState** 
.**  
IsValid**  '
)**' (
{++ 
return,, 

BadRequest,, %
(,,% &

ModelState,,& 0
),,0 1
;,,1 2
}-- 
var00 
	patientId00 
=00 
int00  #
.00# $
Parse00$ )
(00) *
User00* .
.00. /
	FindFirst00/ 8
(008 9
$str009 D
)00D E
!00E F
.00F G
Value00G L
)00L M
;00M N
var33 
result33 
=33 
await33 "
service33# *
.33* +
CreateAsync33+ 6
(336 7
entity337 =
,33= >
	patientId33? H
)33H I
;33I J
return55 
CreatedAtAction55 &
(55& '
$str55' 0
,550 1
new552 5
{556 7
id558 :
=55; <
result55= C
.55C D
AppointmentId55D Q
}55R S
,55S T
result55U [
)55[ \
;55\ ]
}66 
catch77 
(77 
	Exception77 
ex77 
)77  
{88 
return:: 

BadRequest:: !
(::! "
new::" %
{::& '
message::( /
=::0 1
ex::2 4
.::4 5
Message::5 <
}::= >
)::> ?
;::? @
};; 
}<< 	
[?? 	
HttpGet??	 
(?? 
$str?? 
)?? 
]?? 
[@@ 	
	Authorize@@	 
(@@ 
Roles@@ 
=@@ 
$str@@ $
)@@$ %
]@@% &
publicAA 
asyncAA 
TaskAA 
<AA 
IActionResultAA '
>AA' (
GetMyAppointmentsAA) :
(AA: ;
intAA; >
pageAA? C
=AAD E
$numAAF G
,AAG H
intAAH K
pageSizeAAL T
=AAU V
$numAAW Y
)AAY Z
{BB 	
varCC 
	patientIdCC 
=CC 
intCC 
.CC  
ParseCC  %
(CC% &
UserDD 
.DD 
	FindFirstDD 
(DD 
$strDD *
)DD* +
!DD+ ,
.DD, -
ValueDD- 2
)DD2 3
;DD3 4
varFF 
resultFF 
=FF 
awaitGG 
serviceGG 
.GG +
GetAppointmentsByPatientIdAsyncGG =
(GG= >
	patientIdHH 
,HH 
pageII 
,II 
pageSizeJJ 
)JJ 
;JJ 
returnLL 
OkLL 
(LL 
resultLL 
)LL 
;LL 
}MM 	
[OO 	
HttpGetOO	 
(OO 
$strOO 
)OO 
]OO 
[PP 	
	AuthorizePP	 
(PP 
RolesPP 
=PP 
$strPP #
)PP# $
]PP$ %
publicQQ 
asyncQQ 
TaskQQ 
<QQ 
IActionResultQQ '
>QQ' (!
GetDoctorAppointmentsQQ) >
(QQ> ?
intQQ? B
pageQQC G
=QQH I
$numQQJ K
,QQK L
intQQL O
pageSizeQQP X
=QQY Z
$numQQ[ ]
)QQ] ^
{RR 	
varSS 
doctorIdSS 
=SS 
intSS 
.SS 
ParseSS $
(SS$ %
UserTT 
.TT 
	FindFirstTT 
(TT 
$strTT )
)TT) *
!TT* +
.TT+ ,
ValueTT, 1
)TT1 2
;TT2 3
varVV 
resultVV 
=VV 
awaitWW 
serviceWW 
.WW *
GetAppointmentsByDoctorIdAsyncWW <
(WW< =
doctorIdXX 
,XX 
pageYY 
,YY 
pageSizeZZ 
)ZZ 
;ZZ 
return\\ 
Ok\\ 
(\\ 
result\\ 
)\\ 
;\\ 
}]] 	
[`` 	
HttpPut``	 
(`` 
$str`` 
)`` 
]`` 
[aa 	
	Authorizeaa	 
(aa 
Rolesaa 
=aa 
$straa +
)aa+ ,
]aa, -
publicbb 
asyncbb 
Taskbb 
<bb 
IActionResultbb '
>bb' (
Updatebb) /
(bb/ 0
intbb0 3
idbb4 6
,bb6 7
[bb8 9
FromBodybb9 A
]bbA B&
UpdateAppointmentStatusDtobbC ]
entitybb^ d
)bbd e
{cc 	
ifdd 
(dd 
!dd 

ModelStatedd 
.dd 
IsValiddd #
)dd# $
returnee 

BadRequestee !
(ee! "

ModelStateee" ,
)ee, -
;ee- .
varhh 
rolehh 
=hh 
Userhh 
.hh 
	FindFirsthh %
(hh% &

ClaimTypeshh& 0
.hh0 1
Rolehh1 5
)hh5 6
?hh6 7
.hh7 8
Valuehh8 =
;hh= >
varjj 
resultjj 
=jj 
awaitjj 
servicejj &
.jj& '
UpdateAsyncjj' 2
(jj2 3
idjj3 5
,jj5 6
entityjj7 =
,jj= >
rolejj? C
!jjC D
)jjD E
;jjE F
ifll 
(ll 
resultll 
isll 
nullll 
)ll 
returnmm 
NotFoundmm 
(mm  
)mm  !
;mm! "
returnoo 
Okoo 
(oo 
resultoo 
)oo 
;oo 
}pp 	
[rr 	
HttpGetrr	 
(rr 
$strrr *
)rr* +
]rr+ ,
[ss 	
	Authorizess	 
(ss 
Rolesss 
=ss 
$strss "
)ss" #
]ss# $
publictt 
asynctt 
Tasktt 
<tt 
IActionResulttt '
>tt' (
GetByPatienttt) 5
(tt5 6
inttt6 9
	patientIdtt: C
,ttC D
intttD G
pagettH L
=ttM N
$numttO P
,ttP Q
intttQ T
pageSizettU ]
=tt^ _
$numtt` b
)ttb c
{uu 	
varvv 
resultvv 
=vv 
awaitww 
serviceww 
.ww +
GetAppointmentsByPatientIdAsyncww =
(ww= >
	patientIdxx 
,xx 
pageyy 
,yy 
pageSizezz 
)zz 
;zz 
return|| 
result|| 
.|| 
Count|| 
==||  "
$num||# $
?}} 
NotFound}} 
(}} 
)}} 
:~~ 
Ok~~ 
(~~ 
result~~ 
)~~ 
;~~ 
} 	
[
ÅÅ 	
HttpGet
ÅÅ	 
(
ÅÅ 
$str
ÅÅ (
)
ÅÅ( )
]
ÅÅ) *
[
ÇÇ 	
	Authorize
ÇÇ	 
(
ÇÇ 
Roles
ÇÇ 
=
ÇÇ 
$str
ÇÇ "
)
ÇÇ" #
]
ÇÇ# $
public
ÉÉ 
async
ÉÉ 
Task
ÉÉ 
<
ÉÉ 
IActionResult
ÉÉ '
>
ÉÉ' (
GetByDoctor
ÉÉ) 4
(
ÉÉ4 5
int
ÉÉ5 8
doctorId
ÉÉ9 A
,
ÉÉA B
int
ÉÉB E
page
ÉÉF J
=
ÉÉK L
$num
ÉÉM N
,
ÉÉN O
int
ÉÉO R
pageSize
ÉÉS [
=
ÉÉ\ ]
$num
ÉÉ^ `
)
ÉÉ` a
{
ÑÑ 	
var
ÖÖ 
result
ÖÖ 
=
ÖÖ 
await
ÜÜ 
service
ÜÜ 
.
ÜÜ ,
GetAppointmentsByDoctorIdAsync
ÜÜ <
(
ÜÜ< =
doctorId
áá 
,
áá 
page
àà 
,
àà 
pageSize
ââ 
)
ââ 
;
ââ 
return
ãã 
result
ãã 
.
ãã 
Count
ãã 
==
ãã  "
$num
ãã# $
?
åå 
NotFound
åå 
(
åå 
)
åå 
:
çç 
Ok
çç 
(
çç 
result
çç 
)
çç 
;
çç 
}
éé 	
[
êê 	

HttpDelete
êê	 
(
êê 
$str
êê 
)
êê 
]
êê  
[
ëë 	
	Authorize
ëë	 
(
ëë 
Roles
ëë 
=
ëë 
$str
ëë "
)
ëë" #
]
ëë# $
public
íí 
async
íí 
Task
íí 
<
íí 
IActionResult
íí '
>
íí' (
Delete
íí) /
(
íí/ 0
int
íí0 3
id
íí4 6
)
íí6 7
{
ìì 	
var
îî 
deleted
îî 
=
îî 
await
îî 
service
îî  '
.
îî' ($
DeleteAppointmentAsync
îî( >
(
îî> ?
id
îî? A
)
îîA B
;
îîB C
if
ññ 
(
ññ 
!
ññ 
deleted
ññ 
)
ññ 
return
óó 
NotFound
óó 
(
óó  
)
óó  !
;
óó! "
return
ôô 
	NoContent
ôô 
(
ôô 
)
ôô 
;
ôô 
}
õõ 	
[
ùù 	
HttpGet
ùù	 
(
ùù 
$str
ùù 
)
ùù  
]
ùù  !
[
ûû 	
	Authorize
ûû	 
(
ûû 
Roles
ûû 
=
ûû 
$str
ûû #
)
ûû# $
]
ûû$ %
public
üü 
async
üü 
Task
üü 
<
üü 
IActionResult
üü '
>
üü' ("
GetTodayAppointments
üü) =
(
üü= >
int
üü> A
page
üüB F
=
üüG H
$num
üüI J
,
üüJ K
int
üüL O
pageSize
üüP X
=
üüY Z
$num
üü[ ]
)
üü] ^
{
†† 	
var
°° 
doctorId
°° 
=
°° 
int
°° 
.
°° 
Parse
°° $
(
°°$ %
User
¢¢ 
.
¢¢ 
	FindFirst
¢¢ 
(
¢¢ 
$str
¢¢ )
)
¢¢) *
!
¢¢* +
.
¢¢+ ,
Value
¢¢, 1
)
¢¢1 2
;
¢¢2 3
var
§§ 
result
§§ 
=
§§ 
await
•• 
service
•• 
.
•• '
GetTodayAppointmentsAsync
•• 7
(
••7 8
doctorId
¶¶ 
,
¶¶ 
page
ßß 
,
ßß 
pageSize
®® 
)
®® 
;
®® 
return
™™ 
Ok
™™ 
(
™™ 
result
™™ 
)
™™ 
;
™™ 
}
´´ 	
}
≠≠ 
}ÆÆ 