ˆ,
WC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\PatientApiService.cs
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
Web

 
.

 
Services

 !
{ 
public 

class 
PatientApiService "
:# $
IPatientApiService% 7
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public 
PatientApiService  
(  !

HttpClient! +

httpClient, 6
)6 7
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
ApiResponseDto (
>( )
Register* 2
(2 3

PatientDto3 =
dto> A
)A B
{ 	
var 
content 
= 
new 
StringContent +
(+ ,
JsonConvert 
. 
SerializeObject +
(+ ,
dto, /
)/ 0
,0 1
Encoding 
. 
UTF8 
, 
$str "
) 
; 
var 
response 
= 
await  
_httpClient! ,
., -
	PostAsync- 6
(6 7
$str7 @
,@ A
contentB I
)I J
;J K
var 
json 
= 
await 
response %
.% &
Content& -
.- .
ReadAsStringAsync. ?
(? @
)@ A
;A B
return!! 
JsonConvert!! 
.!! 
DeserializeObject!! 0
<!!0 1
ApiResponseDto!!1 ?
>!!? @
(!!@ A
json!!A E
)!!E F
;!!F G
}"" 	
public$$ 
async$$ 
Task$$ 
<$$ 

PatientDto$$ $
>$$$ %
GetById$$& -
($$- .
int$$. 1
id$$2 4
)$$4 5
{%% 	
var&& 
res&& 
=&& 
await&& 
_httpClient&& '
.&&' (
GetAsync&&( 0
(&&0 1
$"&&1 3
$str&&3 ;
{&&; <
id&&< >
}&&> ?
"&&? @
)&&@ A
;&&A B
if'' 
('' 
!'' 
res'' 
.'' 
IsSuccessStatusCode'' (
)''( )
return''* 0
null''1 5
;''5 6
var)) 
json)) 
=)) 
await)) 
res))  
.))  !
Content))! (
.))( )
ReadAsStringAsync))) :
()): ;
))); <
;))< =
return** 
JsonConvert** 
.** 
DeserializeObject** 0
<**0 1

PatientDto**1 ;
>**; <
(**< =
json**= A
)**A B
;**B C
}++ 	
public-- 
async-- 
Task-- 
<-- 
ApiResponseDto-- (
>--( )
Update--* 0
(--0 1
int--1 4
id--5 7
,--7 8

PatientDto--9 C
dto--D G
)--G H
{.. 	
var// 
content// 
=// 
new// 
StringContent// +
(//+ ,
JsonConvert00 
.00 
SerializeObject00 +
(00+ ,
dto00, /
)00/ 0
,000 1
Encoding11 
.11 
UTF811 
,11 
$str22 "
)33 
;33 
var55 
response55 
=55 
await55  
_httpClient55! ,
.55, -
PutAsync55- 5
(555 6
$str556 @
+55A B
id55C E
,55E F
content55G N
)55N O
;55O P
var77 
json77 
=77 
await77 
response77 %
.77% &
Content77& -
.77- .
ReadAsStringAsync77. ?
(77? @
)77@ A
;77A B
return99 
JsonConvert99 
.99 
DeserializeObject99 0
<990 1
ApiResponseDto991 ?
>99? @
(99@ A
json99A E
)99E F
;99F G
}:: 	
public== 
async== 
Task== 
<== 
List== 
<== 
HealthRecordDto== .
>==. /
>==/ 0
GetHealthRecords==1 A
(==A B
int==B E
	patientId==F O
)==O P
{>> 	
var?? 
res?? 
=?? 
await?? 
_httpClient?? '
.??' (
GetAsync??( 0
(??0 1
$"??1 3
$str??3 H
{??H I
	patientId??I R
}??R S
"??S T
)??T U
;??U V
ifAA 
(AA 
!AA 
resAA 
.AA 
IsSuccessStatusCodeAA (
)AA( )
returnBB 
newBB 
ListBB 
<BB  
HealthRecordDtoBB  /
>BB/ 0
(BB0 1
)BB1 2
;BB2 3
varDD 
jsonDD 
=DD 
awaitDD 
resDD  
.DD  !
ContentDD! (
.DD( )
ReadAsStringAsyncDD) :
(DD: ;
)DD; <
;DD< =
returnEE 
JsonConvertEE 
.EE 
DeserializeObjectEE 0
<EE0 1
ListEE1 5
<EE5 6
HealthRecordDtoEE6 E
>EEE F
>EEF G
(EEG H
jsonEEH L
)EEL M
;EEM N
}FF 	
}GG 
}HH à	
XC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\IPatientApiService.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Services !
{ 
public 

	interface 
IPatientApiService '
{ 
Task		 
<		 
ApiResponseDto		 
>		 
Register		 %
(		% &

PatientDto		& 0
dto		1 4
)		4 5
;		5 6
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
 
GetById

  
(

  !
int

! $
id

% '
)

' (
;

( )
Task 
< 
ApiResponseDto 
> 
Update #
(# $
int$ '
id( *
,* +

PatientDto, 6
dto7 :
): ;
;; <
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #
GetHealthRecords$ 4
(4 5
int5 8
	patientId9 B
)B C
;C D
} 
} ø
]C:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\IHealthRecordApiService.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Services !
{ 
public 

	interface #
IHealthRecordApiService ,
{ 
Task		 
<		 
ApiResponseDto		 
>		 
AddHealthRecord		 ,
(		, -
HealthRecordDto		- <
dto		= @
)		@ A
;		A B
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #
GetByPatient$ 0
(0 1
int1 4
	patientId5 >
)> ?
;? @
Task 
< 
HealthRecordDto 
> 
GetById %
(% &
int& )
recordId* 2
)2 3
;3 4
} 
} ˇ
WC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\IDoctorApiService.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Services !
{ 
public 

	interface 
IDoctorApiService &
{ 
Task		 
<		 
List		 
<		 
	DoctorDto		 
>		 
>		 
GetAllDoctors		 +
(		+ ,
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
 
GetDoctorById

 %
(

% &
int

& )
id

* ,
)

, -
;

- .
Task 
< 
bool 
> 
	AddDoctor 
( 
	DoctorDto &
dto' *
)* +
;+ ,
Task 
< 
bool 
> 
UpdateDoctor 
(  
int  #
id$ &
,& '
	DoctorDto( 1
dto2 5
)5 6
;6 7
Task 
< 
List 
< 
	DoctorDto 
> 
> 
GetBySpecialisation 1
(1 2
Specialisation2 @
specA E
)E F
;F G
Task 
< 
List 
< 

PatientDto 
> 
> 
GetAllPatients -
(- .
). /
;/ 0
} 
} ì
\C:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\IAppointmentApiService.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Services !
{ 
public 

	interface "
IAppointmentApiService +
{ 
Task		 
<		 
List		 
<		 
AppointmentDto		  
>		  !
>		! "
GetByDoctor		# .
(		. /
int		/ 2
doctorId		3 ;
)		; <
;		< =
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "
GetByPatient# /
(/ 0
int0 3
	patientId4 =
)= >
;> ?
Task 
< 
AppointmentDto 
> 
GetById $
($ %
int% (
appointmentId) 6
)6 7
;7 8
Task 
< 
ApiResponseDto 
> 
Book !
(! "
BookAppointmentDto" 4
dto5 8
)8 9
;9 :
Task 
UpdateStatus 
( 
int 
id  
,  !
AppointmentStatus" 3
status4 :
): ;
;; <
Task 
Cancel 
( 
int 
id 
,  
CancelAppointmentDto 0
dto1 4
)4 5
;5 6
} 
} ä#
\C:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\HealthRecordApiService.cs
	namespace		 	

HealthAxis		
 
.		 
Web		 
.		 
Services		 !
{

 
public 

class "
HealthRecordApiService '
:( )#
IHealthRecordApiService* A
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public "
HealthRecordApiService %
(% &

HttpClient& 0

httpClient1 ;
); <
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
ApiResponseDto (
>( )
AddHealthRecord* 9
(9 :
HealthRecordDto: I
dtoJ M
)M N
{ 	
var 
content 
= 
new 
StringContent +
(+ ,
JsonConvert 
. 
SerializeObject +
(+ ,
dto, /
)/ 0
,0 1
Encoding 
. 
UTF8 
, 
$str "
) 
; 
var 
response 
= 
await  
_httpClient! ,
., -
	PostAsync- 6
(6 7
$str7 E
,E F
contentG N
)N O
;O P
var 
json 
= 
await 
response %
.% &
Content& -
.- .
ReadAsStringAsync. ?
(? @
)@ A
;A B
return!! 
JsonConvert!! 
.!! 
DeserializeObject!! 0
<!!0 1
ApiResponseDto!!1 ?
>!!? @
(!!@ A
json!!A E
)!!E F
;!!F G
}"" 	
public$$ 
async$$ 
Task$$ 
<$$ 
List$$ 
<$$ 
HealthRecordDto$$ .
>$$. /
>$$/ 0
GetByPatient$$1 =
($$= >
int$$> A
	patientId$$B K
)$$K L
{%% 	
var&& 
response&& 
=&& 
await&&  
_httpClient&&! ,
.&&, -
GetAsync&&- 5
(&&5 6
$"&&6 8
$str&&8 M
{&&M N
	patientId&&N W
}&&W X
"&&X Y
)&&Y Z
;&&Z [
if(( 
((( 
!(( 
response(( 
.(( 
IsSuccessStatusCode(( -
)((- .
return)) 
new)) 
List)) 
<))  
HealthRecordDto))  /
>))/ 0
())0 1
)))1 2
;))2 3
var++ 
json++ 
=++ 
await++ 
response++ %
.++% &
Content++& -
.++- .
ReadAsStringAsync++. ?
(++? @
)++@ A
;++A B
return-- 
JsonConvert-- 
.-- 
DeserializeObject-- 0
<--0 1
List--1 5
<--5 6
HealthRecordDto--6 E
>--E F
>--F G
(--G H
json--H L
)--L M
;--M N
}.. 	
public00 
async00 
Task00 
<00 
HealthRecordDto00 )
>00) *
GetById00+ 2
(002 3
int003 6
recordId007 ?
)00? @
{11 	
var22 
response22 
=22 
await22  
_httpClient22! ,
.22, -
GetAsync22- 5
(225 6
$"226 8
$str228 E
{22E F
recordId22F N
}22N O
"22O P
)22P Q
;22Q R
if44 
(44 
!44 
response44 
.44 
IsSuccessStatusCode44 -
)44- .
return55 
null55 
;55 
var77 
json77 
=77 
await77 
response77 %
.77% &
Content77& -
.77- .
ReadAsStringAsync77. ?
(77? @
)77@ A
;77A B
return99 
JsonConvert99 
.99 
DeserializeObject99 0
<990 1
HealthRecordDto991 @
>99@ A
(99A B
json99B F
)99F G
;99G H
}:: 	
};; 
}<< È3
VC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\DoctorApiService.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Services !
{		 
public

 

class

 
DoctorApiService

 !
:

" #
IDoctorApiService

$ 5
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public 
DoctorApiService 
(  

HttpClient  *

httpClient+ 5
)5 6
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
List 
< 
	DoctorDto (
>( )
>) *
GetAllDoctors+ 8
(8 9
)9 :
{ 	
var 
res 
= 
await 
_httpClient '
.' (
GetAsync( 0
(0 1
$str1 9
)9 :
;: ;
var 
json 
= 
await 
res  
.  !
Content! (
.( )
ReadAsStringAsync) :
(: ;
); <
;< =
return 
JsonConvert 
. 
DeserializeObject 0
<0 1
List1 5
<5 6
	DoctorDto6 ?
>? @
>@ A
(A B
jsonB F
)F G
;G H
} 	
public 
async 
Task 
< 
	DoctorDto #
># $
GetDoctorById% 2
(2 3
int3 6
id7 9
)9 :
{ 	
var 
res 
= 
await 
_httpClient '
.' (
GetAsync( 0
(0 1
$"1 3
$str3 :
{: ;
id; =
}= >
"> ?
)? @
;@ A
if 
( 
! 
res 
. 
IsSuccessStatusCode (
)( )
return* 0
null1 5
;5 6
var 
json 
= 
await 
res  
.  !
Content! (
.( )
ReadAsStringAsync) :
(: ;
); <
;< =
return   
JsonConvert   
.   
DeserializeObject   0
<  0 1
	DoctorDto  1 :
>  : ;
(  ; <
json  < @
)  @ A
;  A B
}!! 	
public## 
async## 
Task## 
<## 
bool## 
>## 
	AddDoctor##  )
(##) *
	DoctorDto##* 3
dto##4 7
)##7 8
{$$ 	
var%% 
content%% 
=%% 
new%% 
StringContent%% +
(%%+ ,
JsonConvert%%, 7
.%%7 8
SerializeObject%%8 G
(%%G H
dto%%H K
)%%K L
,%%L M
Encoding%%N V
.%%V W
UTF8%%W [
,%%[ \
$str%%] o
)%%o p
;%%p q
var&& 
res&& 
=&& 
await&& 
_httpClient&& '
.&&' (
	PostAsync&&( 1
(&&1 2
$str&&2 :
,&&: ;
content&&< C
)&&C D
;&&D E
return'' 
res'' 
.'' 
IsSuccessStatusCode'' *
;''* +
}(( 	
public** 
async** 
Task** 
<** 
bool** 
>** 
UpdateDoctor**  ,
(**, -
int**- 0
id**1 3
,**3 4
	DoctorDto**5 >
dto**? B
)**B C
{++ 	
var,, 
content,, 
=,, 
new,, 
StringContent,, +
(,,+ ,
JsonConvert,,, 7
.,,7 8
SerializeObject,,8 G
(,,G H
dto,,H K
),,K L
,,,L M
Encoding,,N V
.,,V W
UTF8,,W [
,,,[ \
$str,,] o
),,o p
;,,p q
var-- 
res-- 
=-- 
await-- 
_httpClient-- '
.--' (
PutAsync--( 0
(--0 1
$"--1 3
$str--3 :
{--: ;
id--; =
}--= >
"--> ?
,--? @
content--A H
)--H I
;--I J
return.. 
res.. 
... 
IsSuccessStatusCode.. *
;..* +
}// 	
public11 
async11 
Task11 
<11 
List11 
<11 
	DoctorDto11 (
>11( )
>11) *
GetBySpecialisation11+ >
(11> ?
Specialisation11? M
spec11N R
)11R S
{22 	
var33 
res33 
=33 
await33 
_httpClient33 '
.33' (
GetAsync33( 0
(330 1
$"331 3
$str333 I
{33I J
spec33J N
}33N O
"33O P
)33P Q
;33Q R
var44 
json44 
=44 
await44 
res44  
.44  !
Content44! (
.44( )
ReadAsStringAsync44) :
(44: ;
)44; <
;44< =
return55 
JsonConvert55 
.55 
DeserializeObject55 0
<550 1
List551 5
<555 6
	DoctorDto556 ?
>55? @
>55@ A
(55A B
json55B F
)55F G
;55G H
}66 	
public88 
async88 
Task88 
<88 
List88 
<88 

PatientDto88 )
>88) *
>88* +
GetAllPatients88, :
(88: ;
)88; <
{99 	
var:: 
res:: 
=:: 
await:: 
_httpClient:: '
.::' (
GetAsync::( 0
(::0 1
$str::1 :
)::: ;
;::; <
var;; 
json;; 
=;; 
await;; 
res;;  
.;;  !
Content;;! (
.;;( )
ReadAsStringAsync;;) :
(;;: ;
);;; <
;;;< =
return<< 
JsonConvert<< 
.<< 
DeserializeObject<< 0
<<<0 1
List<<1 5
<<<5 6

PatientDto<<6 @
><<@ A
><<A B
(<<B C
json<<C G
)<<G H
;<<H I
}== 	
}>> 
}?? å;
[C:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Services\AppointmentApiService.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Services !
{		 
public

 

class

 !
AppointmentApiService

 &
:

' ("
IAppointmentApiService

) ?
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public !
AppointmentApiService $
($ %

HttpClient% /

httpClient0 :
): ;
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
List 
< 
AppointmentDto -
>- .
>. /
GetByDoctor0 ;
(; <
int< ?
doctorId@ H
)H I
{ 	
var 
response 
= 
await  
_httpClient! ,
., -
GetAsync- 5
(5 6
$"6 8
$str8 K
{K L
doctorIdL T
}T U
"U V
)V W
;W X
if 
( 
! 
response 
. 
IsSuccessStatusCode -
)- .
return 
null 
; 
var 
json 
= 
await 
response %
.% &
Content& -
.- .
ReadAsStringAsync. ?
(? @
)@ A
;A B
return 
JsonConvert 
. 
DeserializeObject 0
<0 1
List1 5
<5 6
AppointmentDto6 D
>D E
>E F
(F G
jsonG K
)K L
;L M
} 	
public 
async 
Task 
< 
List 
< 
AppointmentDto -
>- .
>. /
GetByPatient0 <
(< =
int= @
	patientIdA J
)J K
{ 	
var   
response   
=   
await    
_httpClient  ! ,
.  , -
GetAsync  - 5
(  5 6
$"  6 8
$str  8 L
{  L M
	patientId  M V
}  V W
"  W X
)  X Y
;  Y Z
if"" 
("" 
!"" 
response"" 
."" 
IsSuccessStatusCode"" -
)""- .
return## 
null## 
;## 
var%% 
json%% 
=%% 
await%% 
response%% %
.%%% &
Content%%& -
.%%- .
ReadAsStringAsync%%. ?
(%%? @
)%%@ A
;%%A B
return&& 
JsonConvert&& 
.&& 
DeserializeObject&& 0
<&&0 1
List&&1 5
<&&5 6
AppointmentDto&&6 D
>&&D E
>&&E F
(&&F G
json&&G K
)&&K L
;&&L M
}'' 	
public)) 
async)) 
Task)) 
<)) 
AppointmentDto)) (
>))( )
GetById))* 1
())1 2
int))2 5
appointmentId))6 C
)))C D
{** 	
var++ 
response++ 
=++ 
await++  
_httpClient++! ,
.++, -
GetAsync++- 5
(++5 6
$"++6 8
$str++8 D
{++D E
appointmentId++E R
}++R S
"++S T
)++T U
;++U V
if-- 
(-- 
!-- 
response-- 
.-- 
IsSuccessStatusCode-- -
)--- .
return.. 
null.. 
;.. 
var00 
json00 
=00 
await00 
response00 %
.00% &
Content00& -
.00- .
ReadAsStringAsync00. ?
(00? @
)00@ A
;00A B
return11 
JsonConvert11 
.11 
DeserializeObject11 0
<110 1
AppointmentDto111 ?
>11? @
(11@ A
json11A E
)11E F
;11F G
}22 	
public44 
async44 
Task44 
<44 
ApiResponseDto44 (
>44( )
Book44* .
(44. /
BookAppointmentDto44/ A
dto44B E
)44E F
{55 	
var66 
content66 
=66 
new66 
StringContent66 +
(66+ ,
JsonConvert77 
.77 
SerializeObject77 +
(77+ ,
dto77, /
)77/ 0
,770 1
Encoding88 
.88 
UTF888 
,88 
$str99 "
):: 
;:: 
var<< 
response<< 
=<< 
await<<  
_httpClient<<! ,
.<<, -
	PostAsync<<- 6
(<<6 7
$str<<7 I
,<<I J
content<<K R
)<<R S
;<<S T
var== 
json== 
=== 
await== 
response== %
.==% &
Content==& -
.==- .
ReadAsStringAsync==. ?
(==? @
)==@ A
;==A B
return?? 
JsonConvert?? 
.?? 
DeserializeObject?? 0
<??0 1
ApiResponseDto??1 ?
>??? @
(??@ A
json??A E
)??E F
;??F G
}@@ 	
publicBB 
asyncBB 
TaskBB 
UpdateStatusBB &
(BB& '
intBB' *
idBB+ -
,BB- .
AppointmentStatusBB/ @
statusBBA G
)BBG H
{CC 	
varDD 
dtoDD 
=DD 
newDD &
UpdateAppointmentStatusDtoDD 4
{EE 
StatusFF 
=FF 
statusFF 
}GG 
;GG 
varII 
contentII 
=II 
newII 
StringContentII +
(II+ ,
JsonConvertJJ 
.JJ 
SerializeObjectJJ +
(JJ+ ,
dtoJJ, /
)JJ/ 0
,JJ0 1
EncodingKK 
.KK 
UTF8KK 
,KK 
$strLL "
)MM 
;MM 
awaitOO 
_httpClientOO 
.OO 
PutAsyncOO &
(OO& '
$"OO' )
$strOO) 5
{OO5 6
idOO6 8
}OO8 9
$strOO9 @
"OO@ A
,OOA B
contentOOC J
)OOJ K
;OOK L
}PP 	
publicRR 
asyncRR 
TaskRR 
CancelRR  
(RR  !
intRR! $
idRR% '
,RR' ( 
CancelAppointmentDtoRR) =
dtoRR> A
)RRA B
{SS 	
varTT 
contentTT 
=TT 
newTT 
StringContentTT +
(TT+ ,
JsonConvertUU 
.UU 
SerializeObjectUU +
(UU+ ,
dtoUU, /
)UU/ 0
,UU0 1
EncodingVV 
.VV 
UTF8VV 
,VV 
$strWW "
)XX 
;XX 
awaitZZ 
_httpClientZZ 
.ZZ 
PutAsyncZZ &
(ZZ& '
$"ZZ' )
$strZZ) 5
{ZZ5 6
idZZ6 8
}ZZ8 9
$strZZ9 @
"ZZ@ A
,ZZA B
contentZZC J
)ZZJ K
;ZZK L
}[[ 	
}\\ 
}]] ˇ
TC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str (
)( )
]) *
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str *
)* +
]+ ,
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *èt
ZC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Controllers\PatientController.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Controllers $
{ 
public		 

class		 
PatientController		 "
:		# $

Controller		% /
{

 
private 
readonly 
IPatientApiService +
_patientService, ;
;; <
private 
readonly 
IDoctorApiService *
_doctorService+ 9
;9 :
private 
readonly "
IAppointmentApiService /
_appointmentService0 C
;C D
private 
readonly #
IHealthRecordApiService 0
_healthService1 ?
;? @
public 
PatientController  
(  !
IPatientApiService 
patientService -
,- .
IDoctorApiService 
doctorService +
,+ ,"
IAppointmentApiService "
appointmentService# 5
,5 6#
IHealthRecordApiService #
healthService$ 1
)1 2
{ 	
_patientService 
= 
patientService ,
;, -
_doctorService 
= 
doctorService *
;* +
_appointmentService 
=  !
appointmentService" 4
;4 5
_healthService 
= 
healthService *
;* +
} 	
public 
ActionResult 
Index !
(! "
)" #
{ 	
return 
View 
( 
) 
; 
} 	
[!! 	
HttpPost!!	 
]!! 
public"" 
ActionResult"" 

GoToAction"" &
(""& '
string""' -

actionName"". 8
,""8 9
int"": =
	patientId""> G
)""G H
{## 	
return$$ 
RedirectToAction$$ #
($$# $

actionName$$$ .
,$$. /
new$$0 3
{$$4 5
id$$6 8
=$$9 :
	patientId$$; D
}$$E F
)$$F G
;$$G H
}%% 	
public'' 
ActionResult'' 
Register'' $
(''$ %
)''% &
{(( 	
return)) 
View)) 
()) 
))) 
;)) 
}** 	
[,, 	
HttpPost,,	 
],, 
public-- 
async-- 
Task-- 
<-- 
ActionResult-- &
>--& '
Register--( 0
(--0 1

PatientDto--1 ;
dto--< ?
)--? @
{.. 	
if// 
(// 
!// 

ModelState// 
.// 
IsValid// #
)//# $
return00 
View00 
(00 
dto00 
)00  
;00  !
var22 
result22 
=22 
await22 
_patientService22 .
.22. /
Register22/ 7
(227 8
dto228 ;
)22; <
;22< =
if44 
(44 
!44 
result44 
.44 
Success44 
)44  
{55 
ViewBag66 
.66 
Error66 
=66 
result66  &
.66& '
Message66' .
;66. /
return77 
View77 
(77 
dto77 
)77  
;77  !
}88 
TempData:: 
[:: 
$str:: 
]:: 
=::  !
$str::" D
;::D E
return;; 
RedirectToAction;; #
(;;# $
$str;;$ +
);;+ ,
;;;, -
}<< 	
public>> 
async>> 
Task>> 
<>> 
ActionResult>> &
>>>& '
Profile>>( /
(>>/ 0
int>>0 3
id>>4 6
)>>6 7
{?? 	
var@@ 
patient@@ 
=@@ 
await@@ 
_patientService@@  /
.@@/ 0
GetById@@0 7
(@@7 8
id@@8 :
)@@: ;
;@@; <
ifBB 
(BB 
patientBB 
==BB 
nullBB 
)BB  
{CC 
ViewBagDD 
.DD 
ErrorDD 
=DD 
$strDD  4
;DD4 5
returnEE 
ViewEE 
(EE 
$strEE ,
)EE, -
;EE- .
}FF 
returnHH 
ViewHH 
(HH 
patientHH 
)HH  
;HH  !
}II 	
publicKK 
asyncKK 
TaskKK 
<KK 
ActionResultKK &
>KK& '
EditKK( ,
(KK, -
intKK- 0
idKK1 3
)KK3 4
{LL 	
varMM 
patientMM 
=MM 
awaitMM 
_patientServiceMM  /
.MM/ 0
GetByIdMM0 7
(MM7 8
idMM8 :
)MM: ;
;MM; <
ifOO 
(OO 
patientOO 
==OO 
nullOO 
)OO  
{PP 
ViewBagQQ 
.QQ 
ErrorQQ 
=QQ 
$strQQ  4
;QQ4 5
returnRR 
ViewRR 
(RR 
$strRR ,
)RR, -
;RR- .
}SS 
returnUU 
ViewUU 
(UU 
patientUU 
)UU  
;UU  !
}VV 	
[XX 	
HttpPostXX	 
]XX 
publicYY 
asyncYY 
TaskYY 
<YY 
ActionResultYY &
>YY& '
EditYY( ,
(YY, -

PatientDtoYY- 7
dtoYY8 ;
)YY; <
{ZZ 	
if[[ 
([[ 
![[ 

ModelState[[ 
.[[ 
IsValid[[ #
)[[# $
return\\ 
View\\ 
(\\ 
dto\\ 
)\\  
;\\  !
var^^ 
result^^ 
=^^ 
await^^ 
_patientService^^ .
.^^. /
Update^^/ 5
(^^5 6
dto^^6 9
.^^9 :
	PatientId^^: C
,^^C D
dto^^E H
)^^H I
;^^I J
if`` 
(`` 
!`` 
result`` 
.`` 
Success`` 
)``  
{aa 
ViewBagbb 
.bb 
Errorbb 
=bb 
resultbb  &
.bb& '
Messagebb' .
;bb. /
returncc 
Viewcc 
(cc 
dtocc 
)cc  
;cc  !
}dd 
varff 
updatedPatientff 
=ff  
awaitff! &
_patientServiceff' 6
.ff6 7
GetByIdff7 >
(ff> ?
dtoff? B
.ffB C
	PatientIdffC L
)ffL M
;ffM N
TempDatahh 
[hh 
$strhh 
]hh 
=hh  !
$strhh" A
;hhA B
returnjj 
Viewjj 
(jj 
$strjj !
,jj! "
updatedPatientjj# 1
)jj1 2
;jj2 3
}kk 	
[mm 	
HttpGetmm	 
]mm 
publicnn 
ActionResultnn "
SearchBySpecialisationnn 2
(nn2 3
)nn3 4
{oo 	
returnpp 
Viewpp 
(pp 
)pp 
;pp 
}qq 	
publicss 
asyncss 
Taskss 
<ss 
ActionResultss &
>ss& '"
SearchBySpecialisationss( >
(ss> ?
Specialisationss? M
specssN R
)ssR S
{tt 	
varuu 
doctorsuu 
=uu 
awaituu 
_doctorServiceuu  .
.uu. /
GetBySpecialisationuu/ B
(uuB C
specuuC G
)uuG H
;uuH I
returnvv 
Viewvv 
(vv 
doctorsvv 
)vv  
;vv  !
}ww 	
publicyy 
ActionResultyy 
BookAppointmentyy +
(yy+ ,
)yy, -
{zz 	
return{{ 
View{{ 
({{ 
$str{{ (
){{( )
;{{) *
}|| 	
public~~ 
async~~ 
Task~~ 
<~~ 
ActionResult~~ &
>~~& '
Book~~( ,
(~~, -
int~~- 0
id~~1 3
)~~3 4
{ 	
var
ÄÄ 
patient
ÄÄ 
=
ÄÄ 
await
ÄÄ 
_patientService
ÄÄ  /
.
ÄÄ/ 0
GetById
ÄÄ0 7
(
ÄÄ7 8
id
ÄÄ8 :
)
ÄÄ: ;
;
ÄÄ; <
if
ÇÇ 
(
ÇÇ 
patient
ÇÇ 
==
ÇÇ 
null
ÇÇ 
||
ÇÇ  "
!
ÇÇ# $
patient
ÇÇ$ +
.
ÇÇ+ ,
IsActive
ÇÇ, 4
)
ÇÇ4 5
{
ÉÉ 
ViewBag
ÑÑ 
.
ÑÑ 
Error
ÑÑ 
=
ÑÑ 
$str
ÑÑ  =
;
ÑÑ= >
return
ÖÖ 
View
ÖÖ 
(
ÖÖ 
$str
ÖÖ ,
)
ÖÖ, -
;
ÖÖ- .
}
ÜÜ 
var
àà 
dto
àà 
=
àà 
new
àà  
BookAppointmentDto
àà ,
{
ââ 
	PatientId
ää 
=
ää 
id
ää 
}
ãã 
;
ãã 
return
çç 
View
çç 
(
çç 
dto
çç 
)
çç 
;
çç 
}
éé 	
[
êê 	
HttpPost
êê	 
]
êê 
public
ëë 
async
ëë 
Task
ëë 
<
ëë 
ActionResult
ëë &
>
ëë& '
Book
ëë( ,
(
ëë, - 
BookAppointmentDto
ëë- ?
dto
ëë@ C
)
ëëC D
{
íí 	
if
ìì 
(
ìì 
!
ìì 

ModelState
ìì 
.
ìì 
IsValid
ìì #
)
ìì# $
return
îî 
View
îî 
(
îî 
dto
îî 
)
îî  
;
îî  !
var
ññ 
result
ññ 
=
ññ 
await
ññ !
_appointmentService
ññ 2
.
ññ2 3
Book
ññ3 7
(
ññ7 8
dto
ññ8 ;
)
ññ; <
;
ññ< =
if
òò 
(
òò 
!
òò 
result
òò 
.
òò 
Success
òò 
)
òò  
{
ôô 
ViewBag
öö 
.
öö 
Error
öö 
=
öö 
result
öö  &
.
öö& '
Message
öö' .
;
öö. /
return
õõ 
View
õõ 
(
õõ 
dto
õõ 
)
õõ  
;
õõ  !
}
úú 
TempData
ûû 
[
ûû 
$str
ûû 
]
ûû 
=
ûû  !
$str
ûû" D
;
ûûD E
return
üü 
RedirectToAction
üü #
(
üü# $
$str
üü$ -
,
üü- .
new
üü/ 2
{
üü3 4
id
üü5 7
=
üü8 9
dto
üü: =
.
üü= >
	PatientId
üü> G
}
üüH I
)
üüI J
;
üüJ K
}
†† 	
public
¢¢ 
async
¢¢ 
Task
¢¢ 
<
¢¢ 
ActionResult
¢¢ &
>
¢¢& '
MyAppointments
¢¢( 6
(
¢¢6 7
int
¢¢7 :
id
¢¢; =
)
¢¢= >
{
££ 	
var
§§ 
appointments
§§ 
=
§§ 
await
§§ $!
_appointmentService
§§% 8
.
§§8 9
GetByPatient
§§9 E
(
§§E F
id
§§F H
)
§§H I
;
§§I J
if
¶¶ 
(
¶¶ 
appointments
¶¶ 
==
¶¶ 
null
¶¶  $
)
¶¶$ %
{
ßß 
ViewBag
®® 
.
®® 
Error
®® 
=
®® 
$str
®®  4
;
®®4 5
return
©© 
View
©© 
(
©© 
$str
©© ,
)
©©, -
;
©©- .
}
™™ 
return
¨¨ 
View
¨¨ 
(
¨¨ 
appointments
¨¨ $
)
¨¨$ %
;
¨¨% &
}
≠≠ 	
public
ØØ 
ActionResult
ØØ 
CancelAppointment
ØØ -
(
ØØ- .
int
ØØ. 1
id
ØØ2 4
)
ØØ4 5
{
∞∞ 	
var
±± 
dto
±± 
=
±± 
new
±± "
CancelAppointmentDto
±± .
{
≤≤ 
AppointmentId
≥≥ 
=
≥≥ 
id
≥≥  "
}
¥¥ 
;
¥¥ 
return
∂∂ 
View
∂∂ 
(
∂∂ 
dto
∂∂ 
)
∂∂ 
;
∂∂ 
}
∑∑ 	
[
ππ 	
HttpPost
ππ	 
]
ππ 
public
∫∫ 
async
∫∫ 
Task
∫∫ 
<
∫∫ 
ActionResult
∫∫ &
>
∫∫& '
CancelAppointment
∫∫( 9
(
∫∫9 :"
CancelAppointmentDto
∫∫: N
dto
∫∫O R
)
∫∫R S
{
ªª 	
await
ºº !
_appointmentService
ºº %
.
ºº% &
Cancel
ºº& ,
(
ºº, -
dto
ºº- 0
.
ºº0 1
AppointmentId
ºº1 >
,
ºº> ?
dto
ºº@ C
)
ººC D
;
ººD E
return
ææ 
RedirectToAction
ææ #
(
ææ# $
$str
ææ$ 4
,
ææ4 5
new
ææ6 9
{
ææ: ;
id
ææ< >
=
ææ? @
dto
ææA D
.
ææD E
	PatientId
ææE N
}
ææO P
)
ææP Q
;
ææQ R
}
øø 	
public
¡¡ 
async
¡¡ 
Task
¡¡ 
<
¡¡ 
ActionResult
¡¡ &
>
¡¡& '
HealthRecords
¡¡( 5
(
¡¡5 6
int
¡¡6 9
id
¡¡: <
)
¡¡< =
{
¬¬ 	
var
√√ 
records
√√ 
=
√√ 
await
√√ 
_healthService
√√  .
.
√√. /
GetByPatient
√√/ ;
(
√√; <
id
√√< >
)
√√> ?
;
√√? @
if
≈≈ 
(
≈≈ 
records
≈≈ 
==
≈≈ 
null
≈≈ 
||
≈≈  "
records
≈≈# *
.
≈≈* +
Count
≈≈+ 0
==
≈≈1 3
$num
≈≈4 5
)
≈≈5 6
{
∆∆ 
ViewBag
«« 
.
«« 
Error
«« 
=
«« 
$str
««  9
;
««9 :
return
»» 
View
»» 
(
»» 
new
»» 
List
»»  $
<
»»$ %
HealthRecordDto
»»% 4
>
»»4 5
(
»»5 6
)
»»6 7
)
»»7 8
;
»»8 9
}
…… 
return
ÀÀ 
View
ÀÀ 
(
ÀÀ 
records
ÀÀ 
)
ÀÀ  
;
ÀÀ  !
}
ÃÃ 	
}
ÕÕ 
}ŒŒ ó	
WC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Controllers\HomeController.cs
	namespace 	
HealthAxisWeb
 
. 
Controllers #
{ 
public		 

class		 
HomeController		 
:		  !

Controller		" ,
{

 
public 
ActionResult 
Index !
(! "
)" #
{ 	
return 
View 
( 
) 
; 
} 	
public 
ActionResult 
About !
(! "
)" #
{ 	
ViewBag 
. 
Message 
= 
$str B
;B C
return 
View 
( 
) 
; 
} 	
public 
ActionResult 
Contact #
(# $
)$ %
{ 	
ViewBag 
. 
Message 
= 
$str 2
;2 3
return 
View 
( 
) 
; 
} 	
} 
} È	
HC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Global.asax.cs
	namespace		 	
HealthAxisWeb		
 
{

 
public 

class 
MvcApplication 
:  !
System" (
.( )
Web) ,
., -
HttpApplication- <
{ 
	protected 
void 
Application_Start (
(( )
)) *
{ 	
UnityConfig 
. 
RegisterComponents *
(* +
)+ ,
;, -
AreaRegistration 
. 
RegisterAllAreas -
(- .
). /
;/ 0
FilterConfig 
. !
RegisterGlobalFilters .
(. /
GlobalFilters/ <
.< =
Filters= D
)D E
;E F
RouteConfig 
. 
RegisterRoutes &
(& '

RouteTable' 1
.1 2
Routes2 8
)8 9
;9 :
BundleConfig 
. 
RegisterBundles (
(( )
BundleTable) 4
.4 5
Bundles5 <
)< =
;= >
} 	
} 
} äH
YC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Controllers\DoctorController.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Controllers $
{ 
public		 

class		 
DoctorController		 !
:		" #

Controller		$ .
{

 
private 
readonly 
IDoctorApiService *
_doctorService+ 9
;9 :
private 
readonly "
IAppointmentApiService /
_appointmentService0 C
;C D
public 
DoctorController 
(  
IDoctorApiService  1
doctorService2 ?
,? @"
IAppointmentApiService  6
appointmentService7 I
)I J
{ 	
_doctorService 
= 
doctorService *
;* +
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
public 
async 
Task 
< 
ActionResult &
>& '
Index( -
(- .
). /
{ 	
var 
doctors 
= 
await 
_doctorService  .
.. /
GetAllDoctors/ <
(< =
)= >
;> ?
return 
View 
( 
doctors 
)  
;  !
} 	
public 
ActionResult 
Create "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[   	
HttpPost  	 
]   
public!! 
async!! 
Task!! 
<!! 
ActionResult!! &
>!!& '
Create!!( .
(!!. /
	DoctorDto!!/ 8
dto!!9 <
)!!< =
{"" 	
if## 
(## 
!## 

ModelState## 
.## 
IsValid## #
)### $
return##% +
View##, 0
(##0 1
dto##1 4
)##4 5
;##5 6
await%% 
_doctorService%%  
.%%  !
	AddDoctor%%! *
(%%* +
dto%%+ .
)%%. /
;%%/ 0
TempData&& 
[&& 
$str&& 
]&& 
=&&  !
$str&&" >
;&&> ?
return'' 
RedirectToAction'' #
(''# $
$str''$ +
)''+ ,
;'', -
}(( 	
public** 
ActionResult** 
Edit**  
(**  !
int**! $
?**$ %
id**& (
)**( )
{++ 	
if,, 
(,, 
id,, 
==,, 
null,, 
),, 
return-- 
View-- 
(-- 
$str-- +
)--+ ,
;--, -
return// 
RedirectToAction// #
(//# $
$str//$ 0
,//0 1
new//2 5
{//6 7
id//8 :
}//; <
)//< =
;//= >
}00 	
public22 
async22 
Task22 
<22 
ActionResult22 &
>22& '

EditDoctor22( 2
(222 3
int223 6
id227 9
)229 :
{33 	
var44 
doctor44 
=44 
await44 
_doctorService44 -
.44- .
GetDoctorById44. ;
(44; <
id44< >
)44> ?
;44? @
if66 
(66 
doctor66 
==66 
null66 
)66 
{77 
ViewBag88 
.88 
Error88 
=88 
$str88  3
;883 4
return99 
View99 
(99 
$str99 +
)99+ ,
;99, -
}:: 
return<< 
View<< 
(<< 
$str<< 
,<< 
doctor<<  &
)<<& '
;<<' (
}== 	
[?? 	
HttpPost??	 
]?? 
public@@ 
async@@ 
Task@@ 
<@@ 
ActionResult@@ &
>@@& '

EditDoctor@@( 2
(@@2 3
int@@3 6
id@@7 9
,@@9 :
	DoctorDto@@; D
dto@@E H
)@@H I
{AA 	
ifBB 
(BB 
!BB 

ModelStateBB 
.BB 
IsValidBB #
)BB# $
returnBB% +
ViewBB, 0
(BB0 1
$strBB1 7
,BB7 8
dtoBB9 <
)BB< =
;BB= >
awaitDD 
_doctorServiceDD  
.DD  !
UpdateDoctorDD! -
(DD- .
idDD. 0
,DD0 1
dtoDD2 5
)DD5 6
;DD6 7
TempDataEE 
[EE 
$strEE 
]EE 
=EE  !
$strEE" @
;EE@ A
returnFF 
RedirectToActionFF #
(FF# $
$strFF$ +
)FF+ ,
;FF, -
}GG 	
[HH 	
HttpGetHH	 
]HH 
publicII 

JsonResultII 
ValidateDoctorII (
(II( )
intII) ,
idII- /
)II/ 0
{JJ 	
varKK 
doctorKK 
=KK 
_doctorServiceKK '
.KK' (
GetDoctorByIdKK( 5
(KK5 6
idKK6 8
)KK8 9
.KK9 :
ResultKK: @
;KK@ A
ifMM 
(MM 
doctorMM 
==MM 
nullMM 
)MM 
{NN 
returnOO 
JsonOO 
(OO 
newOO 
{OO  !
successOO" )
=OO* +
falseOO, 1
}OO2 3
,OO3 4
JsonRequestBehaviorOO5 H
.OOH I
AllowGetOOI Q
)OOQ R
;OOR S
}PP 
returnRR 
JsonRR 
(RR 
newRR 
{RR 
successRR %
=RR& '
trueRR( ,
}RR- .
,RR. /
JsonRequestBehaviorRR0 C
.RRC D
AllowGetRRD L
)RRL M
;RRM N
}SS 	
publicTT 
asyncTT 
TaskTT 
<TT 
ActionResultTT &
>TT& '
ConfirmTT( /
(TT/ 0
intTT0 3
idTT4 6
)TT6 7
{UU 	
awaitVV 
_appointmentServiceVV %
.VV% &
UpdateStatusVV& 2
(VV2 3
idVV3 5
,VV5 6
AppointmentStatusVV7 H
.VVH I
	ConfirmedVVI R
)VVR S
;VVS T
returnWW 
RedirectWW 
(WW 
RequestWW #
.WW# $
UrlReferrerWW$ /
.WW/ 0
ToStringWW0 8
(WW8 9
)WW9 :
)WW: ;
;WW; <
}XX 	
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
ActionResultZZ &
>ZZ& '
CompleteZZ( 0
(ZZ0 1
intZZ1 4
idZZ5 7
)ZZ7 8
{[[ 	
await\\ 
_appointmentService\\ %
.\\% &
UpdateStatus\\& 2
(\\2 3
id\\3 5
,\\5 6
AppointmentStatus\\7 H
.\\H I
	Completed\\I R
)\\R S
;\\S T
return]] 
Redirect]] 
(]] 
Request]] #
.]]# $
UrlReferrer]]$ /
.]]/ 0
ToString]]0 8
(]]8 9
)]]9 :
)]]: ;
;]]; <
}^^ 	
public`` 
ActionResult`` 
Cancel`` "
(``" #
int``# &
id``' )
)``) *
{aa 	
returnbb 
Viewbb 
(bb 
newbb  
CancelAppointmentDtobb 0
(bb0 1
)bb1 2
)bb2 3
;bb3 4
}cc 	
[ee 	
HttpPostee	 
]ee 
publicff 
asyncff 
Taskff 
<ff 
ActionResultff &
>ff& '
Cancelff( .
(ff. /
intff/ 2
idff3 5
,ff5 6 
CancelAppointmentDtoff7 K
dtoffL O
)ffO P
{gg 	
ifhh 
(hh 
!hh 

ModelStatehh 
.hh 
IsValidhh #
)hh# $
returnhh% +
Viewhh, 0
(hh0 1
dtohh1 4
)hh4 5
;hh5 6
awaitjj 
_appointmentServicejj %
.jj% &
Canceljj& ,
(jj, -
idjj- /
,jj/ 0
dtojj1 4
)jj4 5
;jj5 6
returnkk 
RedirectToActionkk #
(kk# $
$strkk$ +
)kk+ ,
;kk, -
}ll 	
publicnn 
asyncnn 
Tasknn 
<nn 
ActionResultnn &
>nn& '
ViewPatientsnn( 4
(nn4 5
)nn5 6
{oo 	
varpp 
patientspp 
=pp 
awaitpp  
_doctorServicepp! /
.pp/ 0
GetAllPatientspp0 >
(pp> ?
)pp? @
;pp@ A
returnqq 
Viewqq 
(qq 
patientsqq  
)qq  !
;qq! "
}rr 	
}ss 
}tt ˝/
_C:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Controllers\HealthRecordController.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Controllers $
{ 
public 

class "
HealthRecordController '
:( )

Controller* 4
{		 
private

 
readonly

 #
IHealthRecordApiService

 0
_healthService

1 ?
;

? @
private 
readonly "
IAppointmentApiService /
_appointmentService0 C
;C D
public "
HealthRecordController %
(% &#
IHealthRecordApiService #
healthService$ 1
,1 2"
IAppointmentApiService "
appointmentService# 5
)5 6
{ 	
_healthService 
= 
healthService *
;* +
_appointmentService 
=  !
appointmentService" 4
;4 5
} 	
public 
async 
Task 
< 
ActionResult &
>& '
Create( .
(. /
int/ 2
appointmentId3 @
)@ A
{ 	
var 
appointment 
= 
await #
_appointmentService$ 7
.7 8
GetById8 ?
(? @
appointmentId@ M
)M N
;N O
if 
( 
appointment 
== 
null #
)# $
{ 
TempData 
[ 
$str  
]  !
=" #
$str$ 9
;9 :
return 
RedirectToAction '
(' (
$str( /
,/ 0
$str1 9
)9 :
;: ;
} 
if 
( 
appointment 
. 
Status "
!=# %
AppointmentStatus& 7
.7 8
	Completed8 A
)A B
{   
TempData!! 
[!! 
$str!!  
]!!  !
=!!" #
$str!!$ d
;!!d e
return"" 
RedirectToAction"" '
(""' (
$str""( /
,""/ 0
$str""1 9
)""9 :
;"": ;
}## 
if%% 
(%% 
!%% 
appointment%% 
.%% 
CanAddHealthRecord%% /
)%%/ 0
{&& 
TempData'' 
['' 
$str''  
]''  !
=''" #
$str''$ W
;''W X
return(( 
RedirectToAction(( '
(((' (
$str((( /
,((/ 0
$str((1 9
)((9 :
;((: ;
})) 
var++ 
dto++ 
=++ 
new++ 
HealthRecordDto++ )
{,, 
AppointmentId-- 
=-- 
appointment--  +
.--+ ,
AppointmentId--, 9
,--9 :
	PatientId.. 
=.. 
appointment.. '
...' (
	PatientId..( 1
,..1 2
DoctorId// 
=// 
appointment// &
.//& '
DoctorId//' /
,/// 0
	VisitDate00 
=00 
System00 "
.00" #
DateTime00# +
.00+ ,
Now00, /
}11 
;11 
return33 
View33 
(33 
dto33 
)33 
;33 
}44 	
[66 	
HttpPost66	 
]66 
public77 
async77 
Task77 
<77 
ActionResult77 &
>77& '
Create77( .
(77. /
HealthRecordDto77/ >
dto77? B
)77B C
{88 	
if99 
(99 
!99 

ModelState99 
.99 
IsValid99 #
)99# $
return:: 
View:: 
(:: 
dto:: 
)::  
;::  !
var<< 
result<< 
=<< 
await<< 
_healthService<< -
.<<- .
AddHealthRecord<<. =
(<<= >
dto<<> A
)<<A B
;<<B C
if>> 
(>> 
!>> 
result>> 
.>> 
Success>> 
)>>  
{?? 
ViewBag@@ 
.@@ 
Error@@ 
=@@ 
result@@  &
.@@& '
Message@@' .
;@@. /
returnAA 
ViewAA 
(AA 
dtoAA 
)AA  
;AA  !
}BB 
TempDataDD 
[DD 
$strDD 
]DD 
=DD  !
$strDD" E
;DDE F
returnEE 
RedirectToActionEE #
(EE# $
$strEE$ +
,EE+ ,
$strEE- 5
)EE5 6
;EE6 7
}FF 	
publicHH 
asyncHH 
TaskHH 
<HH 
ActionResultHH &
>HH& '
GetByPatientHH( 4
(HH4 5
intHH5 8
	patientIdHH9 B
)HHB C
{II 	
varJJ 
recordsJJ 
=JJ 
awaitJJ 
_healthServiceJJ  .
.JJ. /
GetByPatientJJ/ ;
(JJ; <
	patientIdJJ< E
)JJE F
;JJF G
ifLL 
(LL 
recordsLL 
==LL 
nullLL 
||LL  "
recordsLL# *
.LL* +
CountLL+ 0
==LL1 3
$numLL4 5
)LL5 6
{MM 
ViewBagNN 
.NN 
ErrorNN 
=NN 
$strNN  9
;NN9 :
returnOO 
ViewOO 
(OO 
newOO 
SystemOO  &
.OO& '
CollectionsOO' 2
.OO2 3
GenericOO3 :
.OO: ;
ListOO; ?
<OO? @
HealthRecordDtoOO@ O
>OOO P
(OOP Q
)OOQ R
)OOR S
;OOS T
}PP 
returnRR 
ViewRR 
(RR 
recordsRR 
)RR  
;RR  !
}SS 	
}TT 
}UU ”E
^C:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\Controllers\AppointmentController.cs
	namespace 	

HealthAxis
 
. 
Web 
. 
Controllers $
{		 
public

 

class

 !
AppointmentController

 &
:

' (

Controller

) 3
{ 
private 
readonly "
IAppointmentApiService /
_service0 8
;8 9
public !
AppointmentController $
($ %"
IAppointmentApiService% ;
service< C
)C D
{ 	
_service 
= 
service 
; 
} 	
public 
async 
Task 
< 
ActionResult &
>& '
	ByPatient( 1
(1 2
int2 5
id6 8
)8 9
{ 	
var 
data 
= 
await 
_service %
.% &
GetByPatient& 2
(2 3
id3 5
)5 6
;6 7
if 
( 
data 
== 
null 
) 
{ 
ViewBag 
. 
Error 
= 
$str  4
;4 5
return 
View 
( 
$str ,
), -
;- .
} 
return 
View 
( 
data 
) 
; 
} 	
public 
async 
Task 
< 
ActionResult &
>& '
Confirm( /
(/ 0
int0 3
id4 6
)6 7
{   	
var!! 
appointment!! 
=!! 
await!! #
_service!!$ ,
.!!, -
GetById!!- 4
(!!4 5
id!!5 7
)!!7 8
;!!8 9
if## 
(## 
appointment## 
==## 
null## #
)### $
{$$ 
TempData%% 
[%% 
$str%%  
]%%  !
=%%" #
$str%%$ 9
;%%9 :
return&& 
RedirectToAction&& '
(&&' (
$str&&( /
,&&/ 0
$str&&1 9
)&&9 :
;&&: ;
}'' 
if)) 
()) 
appointment)) 
.)) 
Status)) "
!=))# %
AppointmentStatus))& 7
.))7 8
Pending))8 ?
)))? @
{** 
TempData++ 
[++ 
$str++  
]++  !
=++" #
$str++$ P
;++P Q
return,, 
Redirect,, 
(,,  
Request,,  '
.,,' (
UrlReferrer,,( 3
.,,3 4
ToString,,4 <
(,,< =
),,= >
),,> ?
;,,? @
}-- 
await// 
_service// 
.// 
UpdateStatus// '
(//' (
id//( *
,//* +
AppointmentStatus//, =
.//= >
	Confirmed//> G
)//G H
;//H I
return00 
Redirect00 
(00 
Request00 #
.00# $
UrlReferrer00$ /
.00/ 0
ToString000 8
(008 9
)009 :
)00: ;
;00; <
}11 	
public33 
async33 
Task33 
<33 
ActionResult33 &
>33& '
Complete33( 0
(330 1
int331 4
id335 7
)337 8
{44 	
var55 
appointment55 
=55 
await55 #
_service55$ ,
.55, -
GetById55- 4
(554 5
id555 7
)557 8
;558 9
if77 
(77 
appointment77 
==77 
null77 #
)77# $
{88 
TempData99 
[99 
$str99  
]99  !
=99" #
$str99$ 9
;999 :
return:: 
RedirectToAction:: '
(::' (
$str::( /
,::/ 0
$str::1 9
)::9 :
;::: ;
};; 
if== 
(== 
appointment== 
.== 
Status== "
!===# %
AppointmentStatus==& 7
.==7 8
	Confirmed==8 A
)==A B
{>> 
TempData?? 
[?? 
$str??  
]??  !
=??" #
$str??$ R
;??R S
return@@ 
Redirect@@ 
(@@  
Request@@  '
.@@' (
UrlReferrer@@( 3
.@@3 4
ToString@@4 <
(@@< =
)@@= >
)@@> ?
;@@? @
}AA 
awaitCC 
_serviceCC 
.CC 
UpdateStatusCC '
(CC' (
idCC( *
,CC* +
AppointmentStatusCC, =
.CC= >
	CompletedCC> G
)CCG H
;CCH I
returnDD 
RedirectDD 
(DD 
RequestDD #
.DD# $
UrlReferrerDD$ /
.DD/ 0
ToStringDD0 8
(DD8 9
)DD9 :
)DD: ;
;DD; <
}EE 	
publicFF 
asyncFF 
TaskFF 
<FF 
ActionResultFF &
>FF& '
ByDoctorFF( 0
(FF0 1
intFF1 4
idFF5 7
)FF7 8
{GG 	
varHH 
appointmentsHH 
=HH 
awaitHH $
_serviceHH% -
.HH- .
GetByDoctorHH. 9
(HH9 :
idHH: <
)HH< =
;HH= >
ifJJ 
(JJ 
appointmentsJJ 
==JJ 
nullJJ  $
||JJ% '
!JJ( )
appointmentsJJ) 5
.JJ5 6
AnyJJ6 9
(JJ9 :
)JJ: ;
)JJ; <
{KK 
TempDataLL 
[LL 
$strLL  
]LL  !
=LL" #
$strLL$ P
;LLP Q
returnMM 
RedirectToActionMM '
(MM' (
$strMM( /
,MM/ 0
$strMM1 9
)MM9 :
;MM: ;
}NN 
returnPP 
ViewPP 
(PP 
appointmentsPP $
)PP$ %
;PP% &
}QQ 	
publicRR 
ActionResultRR 
CancelRR "
(RR" #
intRR# &
idRR' )
)RR) *
{SS 	
returnTT 
ViewTT 
(TT 
newTT  
CancelAppointmentDtoTT 0
(TT0 1
)TT1 2
)TT2 3
;TT3 4
}UU 	
[WW 	
HttpPostWW	 
]WW 
publicXX 
asyncXX 
TaskXX 
<XX 
ActionResultXX &
>XX& '
CancelXX( .
(XX. /
intXX/ 2
idXX3 5
,XX5 6 
CancelAppointmentDtoXX7 K
dtoXXL O
)XXO P
{YY 	
ifZZ 
(ZZ 
!ZZ 

ModelStateZZ 
.ZZ 
IsValidZZ #
)ZZ# $
return[[ 
View[[ 
([[ 
dto[[ 
)[[  
;[[  !
var]] 
appointment]] 
=]] 
await]] #
_service]]$ ,
.]], -
GetById]]- 4
(]]4 5
id]]5 7
)]]7 8
;]]8 9
if__ 
(__ 
appointment__ 
==__ 
null__ #
)__# $
{`` 
TempDataaa 
[aa 
$straa  
]aa  !
=aa" #
$straa$ 9
;aa9 :
returnbb 
RedirectToActionbb '
(bb' (
$strbb( /
,bb/ 0
$strbb1 9
)bb9 :
;bb: ;
}cc 
ifee 
(ee 
appointmentee 
.ee 
Statusee "
!=ee# %
AppointmentStatusee& 7
.ee7 8
Pendingee8 ?
&&ee@ B
appointmentff 
.ff 
Statusff "
!=ff# %
AppointmentStatusff& 7
.ff7 8
	Confirmedff8 A
)ffA B
{gg 
TempDatahh 
[hh 
$strhh  
]hh  !
=hh" #
$strhh$ ]
;hh] ^
returnii 
Redirectii 
(ii  
Requestii  '
.ii' (
UrlReferrerii( 3
.ii3 4
ToStringii4 <
(ii< =
)ii= >
)ii> ?
;ii? @
}jj 
awaitll 
_servicell 
.ll 
Cancelll !
(ll! "
idll" $
,ll$ %
dtoll& )
)ll) *
;ll* +
TempDatann 
[nn 
$strnn 
]nn 
=nn  !
$strnn" F
;nnF G
returnoo 
RedirectToActionoo #
(oo# $
$stroo$ +
,oo+ ,
$stroo- 5
)oo5 6
;oo6 7
}pp 	
}qq 
}rr “
RC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\App_Start\UnityConfig.cs
	namespace 	
HealthAxisWeb
 
{		 
public

 

static

 
class

 
UnityConfig

 #
{ 
public 
static 
void 
RegisterComponents -
(- .
). /
{ 	
var 
	container 
= 
new 
UnityContainer %
(% &
)& '
;' (
var 

httpClient 
= 
new  

HttpClient! +
{ 
BaseAddress 
= 
new !
Uri" %
(% &
$str& D
)D E
} 
; 
	container 
. 
RegisterInstance &
<& '

HttpClient' 1
>1 2
(2 3

httpClient3 =
)= >
;> ?
	container 
. 
RegisterType "
<" #
IDoctorApiService# 4
,4 5
DoctorApiService6 F
>F G
(G H
)H I
;I J
	container 
. 
RegisterType "
<" #
IPatientApiService# 5
,5 6
PatientApiService7 H
>H I
(I J
)J K
;K L
	container 
. 
RegisterType "
<" #"
IAppointmentApiService# 9
,9 :!
AppointmentApiService; P
>P Q
(Q R
)R S
;S T
	container 
. 
RegisterType "
<" ##
IHealthRecordApiService# :
,: ;"
HealthRecordApiService< R
>R S
(S T
)T U
;U V
DependencyResolver 
. 
SetResolver *
(* +
new+ .#
UnityDependencyResolver/ F
(F G
	containerG P
)P Q
)Q R
;R S
} 	
} 
} ·
RC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\App_Start\RouteConfig.cs
	namespace 	
HealthAxisWeb
 
{		 
public

 

class

 
RouteConfig

 
{ 
public 
static 
void 
RegisterRoutes )
() *
RouteCollection* 9
routes: @
)@ A
{ 	
routes 
. 
IgnoreRoute 
( 
$str ;
); <
;< =
routes 
. 
MapRoute 
( 
name 
: 
$str 
,  
url 
: 
$str 1
,1 2
defaults 
: 
new 
{ 

controller  *
=+ ,
$str- 3
,3 4
action5 ;
=< =
$str> E
,E F
idG I
=J K
UrlParameterL X
.X Y
OptionalY a
}b c
) 
; 
} 	
} 
} É
SC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\App_Start\FilterConfig.cs
	namespace 	
HealthAxisWeb
 
{ 
public 

class 
FilterConfig 
{ 
public 
static 
void !
RegisterGlobalFilters 0
(0 1"
GlobalFilterCollection1 G
filtersH O
)O P
{		 	
filters

 
.

 
Add

 
(

 
new

  
HandleErrorAttribute

 0
(

0 1
)

1 2
)

2 3
;

3 4
} 	
} 
} ≥
SC:\Users\310053\source\repos\HealthAxis_Web\HealthAxisWeb\App_Start\BundleConfig.cs
	namespace 	
HealthAxisWeb
 
{ 
public 

class 
BundleConfig 
{ 
public		 
static		 
void		 
RegisterBundles		 *
(		* +
BundleCollection		+ ;
bundles		< C
)		C D
{

 	
bundles 
. 
Add 
( 
new 
ScriptBundle (
(( )
$str) ;
); <
.< =
Include= D
(D E
$str 7
)7 8
)8 9
;9 :
bundles 
. 
Add 
( 
new 
ScriptBundle (
(( )
$str) >
)> ?
.? @
Include@ G
(G H
$str 4
)4 5
)5 6
;6 7
bundles 
. 
Add 
( 
new 
ScriptBundle (
(( )
$str) >
)> ?
.? @
Include@ G
(G H
$str /
)/ 0
)0 1
;1 2
bundles 
. 
Add 
( 
new 
Bundle "
(" #
$str# 8
)8 9
.9 :
Include: A
(A B
$str .
). /
)/ 0
;0 1
bundles 
. 
Add 
( 
new 
StyleBundle '
(' (
$str( 7
)7 8
.8 9
Include9 @
(@ A
$str /
,/ 0
$str *
)* +
)+ ,
;, -
} 	
} 
} 