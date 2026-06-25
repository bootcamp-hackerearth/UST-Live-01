ˇ
iC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Toast\ToastType.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Toast- 2
{ 
public 

enum 
	ToastType 
{ 
Success 
= 
$num 
, 
Error 
= 
$num 
, 
Warning		 
=		 
$num		 
,		 
Info 
= 
$num 
} 
} ‘	
lC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Toast\ToastMessage.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Toast- 2
{ 
public 

class 
ToastMessage 
{ 
public 
string 
Title 
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
public		 
	ToastType		 
Type		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
public 
int "
DurationInMilliseconds )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
=8 9
$num: >
;> ?
} 
} ˛	
rC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IToastService.cs
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
	interface 
IToastService "
{ 
event 
Action 
< 
ToastMessage !
>! "
?" #
OnShow$ *
;* +
void		 
ShowSuccess		 
(		 
string		 
title		  %
,		% &
string		' -
message		. 5
)		5 6
;		6 7
void 
	ShowError 
( 
string 
title #
,# $
string% +
message, 3
)3 4
;4 5
void 
ShowWarning 
( 
string 
title  %
,% &
string' -
message. 5
)5 6
;6 7
void 
ShowInfo 
( 
string 
title "
," #
string$ *
message+ 2
)2 3
;3 4
} 
} ≠
yC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IPatientAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface  
IPatientAdminService )
{ 
Task 
< 
List 
< 

PatientDto 
> 
> 
GetAllPatientsAsync 2
(2 3
)3 4
;4 5
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
 

PatientDto

 %
>

% &
>

& '!
GetPatientsPagedAsync

( =
(

= >%
PatientPaginationQueryDto

> W
query

X ]
)

] ^
;

^ _
Task 
< 

PatientDto 
? 
> 
GetPatientByIdAsync -
(- .
int. 1
	patientId2 ;
); <
;< =
Task 
< 

PatientDto 
? 
> 
CreatePatientAsync ,
(, -
CreatePatientDto- =

patientDto> H
)H I
;I J
Task 
< 

PatientDto 
? 
> 
UpdatePatientAsync ,
(, -
int- 0
	patientId1 :
,: ;
UpdatePatientDto< L

patientDtoM W
)W X
;X Y
} 
} È
xC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IDoctorAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface 
IDoctorAdminService (
{ 
Task 
< 
List 
< 
	DoctorDto 
> 
> 
GetAllDoctorsAsync 0
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
% & 
GetDoctorsPagedAsync

' ;
(

; <$
DoctorPaginationQueryDto

< T
query

U Z
)

Z [
;

[ \
Task 
< 
	DoctorDto 
? 
> 
GetDoctorByIdAsync +
(+ ,
int, /
doctorId0 8
)8 9
;9 :
Task 
< $
DoctorCreatedResponseDto %
>% &
CreateDoctorAsync' 8
(8 9
CreateDoctorDto9 H
	doctorDtoI R
)R S
;S T
Task 
< 
	DoctorDto 
? 
> 
UpdateDoctorAsync *
(* +
int+ .
doctorId/ 7
,7 8
UpdateDoctorDto9 H
	doctorDtoI R
)R S
;S T
Task 
< 
	DoctorDto 
? 
> #
ToggleDoctorStatusAsync 0
(0 1
int1 4
doctorId5 =
)= >
;> ?
} 
} Ü	
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
} ©
~C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Interfaces\IAppointementAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -

Interfaces- 7
{ 
public 

	interface $
IAppointmentAdminService -
{ 
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "#
GetAllAppointmentsAsync# :
(: ;
); <
;< =
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
 
AppointmentDto

 )
>

) *
>

* +%
GetAppointmentsPagedAsync

, E
(

E F)
AppointmentPaginationQueryDto

F c
query

d i
)

i j
;

j k
Task 
< '
AppointmentFilterOptionsDto (
>( ),
 GetAppointmentFilterOptionsAsync* J
(J K
)K L
;L M
Task 
< ,
 AppointmentDailyStatusSummaryDto -
>- .&
GetDailyStatusSummaryAsync/ I
(I J
DateTimeJ R
dateS W
)W X
;X Y
Task 
< 
AppointmentDto 
? 
> #
GetAppointmentByIdAsync 5
(5 6
int6 9
appointmentId: G
)G H
;H I
Task 
< 
AppointmentDto 
> "
CreateAppointmentAsync 3
(3 4
BookAppointmentDto4 F
requestG N
)N O
;O P
Task 
< 
AppointmentDto 
? 
> "
UpdateAppointmentAsync 4
(4 5
int5 8
appointmentId9 F
,F G 
UpdateAppointmentDtoH \
request] d
)d e
;e f
Task 
< 
bool 
> "
DeleteAppointmentAsync )
() *
int* -
appointmentId. ;
); <
;< =
} 
} œ
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
}		 ¥
kC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\ToastService.cs
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
class 
ToastService 
: 
IToastService  -
{ 
public 
event 
Action 
< 
ToastMessage (
>( )
?) *
OnShow+ 1
;1 2
public

 
void

 
ShowSuccess

 
(

  
string

  &
title

' ,
,

, -
string

. 4
message

5 <
)

< =
{ 	
	ShowToast 
( 
title 
, 
message $
,$ %
	ToastType& /
./ 0
Success0 7
)7 8
;8 9
} 	
public 
void 
	ShowError 
( 
string $
title% *
,* +
string, 2
message3 :
): ;
{ 	
	ShowToast 
( 
title 
, 
message $
,$ %
	ToastType& /
./ 0
Error0 5
)5 6
;6 7
} 	
public 
void 
ShowWarning 
(  
string  &
title' ,
,, -
string. 4
message5 <
)< =
{ 	
	ShowToast 
( 
title 
, 
message $
,$ %
	ToastType& /
./ 0
Warning0 7
)7 8
;8 9
} 	
public 
void 
ShowInfo 
( 
string #
title$ )
,) *
string+ 1
message2 9
)9 :
{ 	
	ShowToast 
( 
title 
, 
message $
,$ %
	ToastType& /
./ 0
Info0 4
)4 5
;5 6
} 	
private 
void 
	ShowToast 
( 
string %
title& +
,+ ,
string- 3
message4 ;
,; <
	ToastType= F
typeG K
)K L
{ 	
var   
toastMessage   
=   
new   "
ToastMessage  # /
{!! 
Title"" 
="" 
title"" 
,"" 
Message$$ 
=$$ 
message$$ !
,$$! "
Type&& 
=&& 
type&& 
}'' 
;'' 
OnShow)) 
?)) 
.)) 
Invoke)) 
()) 
toastMessage)) '
)))' (
;))( )
}** 	
}++ 
},, ÷;
rC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\PatientAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{		 
public

 

class

 
PatientAdminService

 $
:

% &
BaseApiService

' 5
,

5 6 
IPatientAdminService

7 K
{ 
private 
const 
string 
PatientsEndpoint -
=. /
$str0 >
;> ?
public 
PatientAdminService "
(" #

HttpClient 

httpClient 
, 

IJSRuntime 
	jsRuntime 
, 
NavigationManager 
navigationManager *
)* +
: 
base	 
( 

httpClient 
, 
	jsRuntime #
,# $
navigationManager% 6
)6 7
{ 	
} 	
public 
async 
Task 
< 
List 
< 

PatientDto )
>) *
>* +
GetAllPatientsAsync, ?
(? @
)@ A
{ 	
var 
query 
= 
new %
PatientPaginationQueryDto 5
{ 

PageNumber 
= 
$num 
, 
PageSize 
= 
$num 
} 
; 
var 
response 
= 
await  !
GetPatientsPagedAsync! 6
(6 7
query7 <
)< =
;= >
return   
response   
.   
Items   !
;  ! "
}!! 	
public## 
async## 
Task## 
<## 
PagedResponse## '
<##' (

PatientDto##( 2
>##2 3
>##3 4!
GetPatientsPagedAsync##5 J
(##J K%
PatientPaginationQueryDto##K d
query##e j
)##j k
{$$ 	
string%% 
endpoint%% 
=%% !
BuildPatientsEndpoint%% 3
(%%3 4
query%%4 9
)%%9 :
;%%: ;
return'' 
await'' 
GetAuthorizedAsync'' +
<''+ ,
PagedResponse'', 9
<''9 :

PatientDto'': D
>''D E
>''E F
(''F G
endpoint(( 
,(( 
$str)) L
)))L M
;))M N
}** 	
public,, 
async,, 
Task,, 
<,, 

PatientDto,, $
?,,$ %
>,,% &
GetPatientByIdAsync,,' :
(,,: ;
int,,; >
	patientId,,? H
),,H I
{-- 	
if.. 
(.. 
	patientId.. 
<=.. 
$num.. 
).. 
{// 
return00 
null00 
;00 
}11 
return33 
await33 
GetAuthorizedAsync33 +
<33+ ,

PatientDto33, 6
>336 7
(337 8
$"44 
{44 
PatientsEndpoint44 #
}44# $
$str44$ %
{44% &
	patientId44& /
}44/ 0
"440 1
,441 2
$str55 O
)55O P
;55P Q
}66 	
public88 
async88 
Task88 
<88 

PatientDto88 $
?88$ %
>88% &
CreatePatientAsync88' 9
(889 :
CreatePatientDto88: J

patientDto88K U
)88U V
{99 	
return:: 
await:: 
PostAuthorizedAsync:: ,
<::, -
CreatePatientDto::- =
,::= >

PatientDto::? I
>::I J
(::J K
PatientsEndpoint;;  
,;;  !

patientDto<< 
,<< 
$str== R
)==R S
;==S T
}>> 	
public@@ 
async@@ 
Task@@ 
<@@ 

PatientDto@@ $
?@@$ %
>@@% &
UpdatePatientAsync@@' 9
(@@9 :
int@@: =
	patientId@@> G
,@@G H
UpdatePatientDto@@I Y

patientDto@@Z d
)@@d e
{AA 	
ifBB 
(BB 
	patientIdBB 
<=BB 
$numBB 
)BB 
{CC 
returnDD 
nullDD 
;DD 
}EE 
returnGG 
awaitGG 
PutAuthorizedAsyncGG +
<GG+ ,
UpdatePatientDtoGG, <
,GG< =

PatientDtoGG> H
>GGH I
(GGI J
$"HH 
{HH 
PatientsEndpointHH #
}HH# $
$strHH$ %
{HH% &
	patientIdHH& /
}HH/ 0
"HH0 1
,HH1 2

patientDtoII 
,II 
$strJJ R
)JJR S
;JJS T
}KK 	
privateMM 
staticMM 
stringMM !
BuildPatientsEndpointMM 3
(MM3 4%
PatientPaginationQueryDtoMM4 M
queryMMN S
)MMS T
{NN 	
varOO 
queryParametersOO 
=OO  !
newOO" %
ListOO& *
<OO* +
stringOO+ 1
>OO1 2
{PP 
$"QQ 
$strQQ 
{QQ 
queryQQ #
.QQ# $

PageNumberQQ$ .
}QQ. /
"QQ/ 0
,QQ0 1
$"RR 
$strRR 
{RR 
queryRR !
.RR! "
PageSizeRR" *
}RR* +
"RR+ ,
}SS 
;SS 
ifUU 
(UU 
!UU 
stringUU 
.UU 
IsNullOrWhiteSpaceUU *
(UU* +
queryUU+ 0
.UU0 1

SearchTermUU1 ;
)UU; <
)UU< =
{VV 
queryParametersWW 
.WW  
AddWW  #
(WW# $
$"WW$ &
$strWW& 1
{WW1 2
UriWW2 5
.WW5 6
EscapeDataStringWW6 F
(WWF G
queryWWG L
.WWL M

SearchTermWWM W
)WWW X
}WWX Y
"WWY Z
)WWZ [
;WW[ \
}XX 
ifZZ 
(ZZ 
queryZZ 
.ZZ 
GenderZZ 
isZZ 
notZZ  #
nullZZ$ (
)ZZ( )
{[[ 
queryParameters\\ 
.\\  
Add\\  #
(\\# $
$"\\$ &
$str\\& -
{\\- .
query\\. 3
.\\3 4
Gender\\4 :
}\\: ;
"\\; <
)\\< =
;\\= >
}]] 
if__ 
(__ 
query__ 
.__ 
HasInsurance__ "
is__# %
not__& )
null__* .
)__. /
{`` 
queryParametersaa 
.aa  
Addaa  #
(aa# $
$"aa$ &
$straa& 3
{aa3 4
queryaa4 9
.aa9 :
HasInsuranceaa: F
.aaF G
ValueaaG L
.aaL M
ToStringaaM U
(aaU V
)aaV W
.aaW X
ToLowerInvariantaaX h
(aah i
)aai j
}aaj k
"aak l
)aal m
;aam n
}bb 
returndd 
$"dd 
{dd 
PatientsEndpointdd &
}dd& '
$strdd' (
{dd( )
stringdd) /
.dd/ 0
Joindd0 4
(dd4 5
$strdd5 8
,dd8 9
queryParametersdd: I
)ddI J
}ddJ K
"ddK L
;ddL M
}ee 	
}ff 
}gg ‡N
qC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\DoctorAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public		 

class		 
DoctorAdminService		 #
:		$ %
BaseApiService		& 4
,		4 5
IDoctorAdminService		6 I
{

 
private 
const 
string  
AdminDoctorsEndpoint 1
=2 3
$str4 G
;G H
private 
const 
string 
DoctorsEndpoint ,
=- .
$str/ <
;< =
public 
DoctorAdminService !
(! "

HttpClient 

httpClient 
, 

IJSRuntime 
	jsRuntime 
, 
NavigationManager 
navigationManager )
)) *
: 
base 
( 

httpClient 
, 
	jsRuntime "
," #
navigationManager$ 5
)5 6
{ 	
} 	
public 
async 
Task 
< 
List 
< 
	DoctorDto (
>( )
>) *
GetAllDoctorsAsync+ =
(= >
)> ?
{ 	
var 
query 
= 
new $
DoctorPaginationQueryDto 4
{ 

PageNumber 
= 
$num 
, 
PageSize 
= 
$num 
} 
; 
var 
response 
= 
await   
GetDoctorsPagedAsync! 5
(5 6
query6 ;
); <
;< =
return 
response 
. 
Items !
;! "
}   	
public"" 
async"" 
Task"" 
<"" 
PagedResponse"" '
<""' (
	DoctorDto""( 1
>""1 2
>""2 3 
GetDoctorsPagedAsync""4 H
(""H I$
DoctorPaginationQueryDto""I a
query""b g
)""g h
{## 	
string$$ 
endpoint$$ 
=$$  
BuildDoctorsEndpoint$$ 2
($$2 3
query$$3 8
)$$8 9
;$$9 :
return&& 
await&& 
GetAuthorizedAsync&& +
<&&+ ,
PagedResponse&&, 9
<&&9 :
	DoctorDto&&: C
>&&C D
>&&D E
(&&E F
endpoint'' 
,'' 
$str(( K
)((K L
;((L M
})) 	
public++ 
async++ 
Task++ 
<++ 
	DoctorDto++ #
?++# $
>++$ %
GetDoctorByIdAsync++& 8
(++8 9
int++9 <
doctorId++= E
)++E F
{,, 	
if-- 
(-- 
doctorId-- 
<=-- 
$num-- 
)-- 
{.. 
return// 
null// 
;// 
}00 
return22 
await22 
GetAuthorizedAsync22 +
<22+ ,
	DoctorDto22, 5
>225 6
(226 7
$"33 
{33 
DoctorsEndpoint33 "
}33" #
$str33# $
{33$ %
doctorId33% -
}33- .
"33. /
,33/ 0
$str44 N
)44N O
;44O P
}55 	
public77 
async77 
Task77 
<77 $
DoctorCreatedResponseDto77 2
>772 3
CreateDoctorAsync774 E
(77E F
CreateDoctorDto77F U
	doctorDto77V _
)77_ `
{88 	
return99 
await99 
PostAuthorizedAsync99 ,
<99, -
CreateDoctorDto99- <
,99< =$
DoctorCreatedResponseDto99> V
>99V W
(99W X 
AdminDoctorsEndpoint:: $
,::$ %
	doctorDto;; 
,;; 
$str<< Q
)<<Q R
;<<R S
}== 	
public?? 
async?? 
Task?? 
<?? 
	DoctorDto?? #
???# $
>??$ %
UpdateDoctorAsync??& 7
(??7 8
int??8 ;
doctorId??< D
,??D E
UpdateDoctorDto??F U
	doctorDto??V _
)??_ `
{@@ 	
ifAA 
(AA 
doctorIdAA 
<=AA 
$numAA 
)AA 
{BB 
returnCC 
nullCC 
;CC 
}DD 
returnFF 
awaitFF 
PutAuthorizedAsyncFF +
<FF+ ,
UpdateDoctorDtoFF, ;
,FF; <
	DoctorDtoFF= F
>FFF G
(FFG H
$"GG 
{GG  
AdminDoctorsEndpointGG '
}GG' (
$strGG( )
{GG) *
doctorIdGG* 2
}GG2 3
"GG3 4
,GG4 5
	doctorDtoHH 
,HH 
$strII Q
)IIQ R
;IIR S
}JJ 	
publicLL 
asyncLL 
TaskLL 
<LL 
	DoctorDtoLL #
?LL# $
>LL$ %#
ToggleDoctorStatusAsyncLL& =
(LL= >
intLL> A
doctorIdLLB J
)LLJ K
{MM 	
varNN 
doctorNN 
=NN 
awaitNN 
GetDoctorByIdAsyncNN 1
(NN1 2
doctorIdNN2 :
)NN: ;
;NN; <
ifPP 
(PP 
doctorPP 
isPP 
nullPP 
)PP 
{QQ 
returnRR 
nullRR 
;RR 
}SS 
varUU 
updateDoctorDtoUU 
=UU  !
newUU" %
UpdateDoctorDtoUU& 5
{VV 
FullNameWW 
=WW 
doctorWW !
.WW! "
FullNameWW" *
,WW* +
SpecialisationXX 
=XX  
doctorXX! '
.XX' (
SpecialisationXX( 6
,XX6 7
PracticeStartDateYY !
=YY" #+
GetApproximatePracticeStartDateYY$ C
(YYC D
doctorYYD J
.YYJ K
YearsOfExperienceYYK \
)YY\ ]
,YY] ^
ConsultationFeeZZ 
=ZZ  !
doctorZZ" (
.ZZ( )
ConsultationFeeZZ) 8
,ZZ8 9
IsActive[[ 
=[[ 
![[ 
doctor[[ "
.[[" #
IsActive[[# +
}\\ 
;\\ 
return^^ 
await^^ 
UpdateDoctorAsync^^ *
(^^* +
doctorId^^+ 3
,^^3 4
updateDoctorDto^^5 D
)^^D E
;^^E F
}__ 	
privatecc 
staticcc 
stringcc  
BuildDoctorsEndpointcc 2
(cc2 3$
DoctorPaginationQueryDtocc3 K
queryccL Q
)ccQ R
{dd 	
varee 
queryParametersee 
=ee  !
newee" %
Listee& *
<ee* +
stringee+ 1
>ee1 2
{ff 
$"gg 
$strgg 
{gg 
querygg #
.gg# $

PageNumbergg$ .
}gg. /
"gg/ 0
,gg0 1
$"hh 
$strhh 
{hh 
queryhh !
.hh! "
PageSizehh" *
}hh* +
"hh+ ,
}ii 
;ii 
ifkk 
(kk 
!kk 
stringkk 
.kk 
IsNullOrWhiteSpacekk *
(kk* +
querykk+ 0
.kk0 1

SearchTermkk1 ;
)kk; <
)kk< =
{ll 
queryParametersmm 
.mm  
Addmm  #
(mm# $
$"mm$ &
$strmm& 1
{mm1 2
Urimm2 5
.mm5 6
EscapeDataStringmm6 F
(mmF G
querymmG L
.mmL M

SearchTermmmM W
)mmW X
}mmX Y
"mmY Z
)mmZ [
;mm[ \
}nn 
ifpp 
(pp 
querypp 
.pp 
Specialisationpp $
ispp% '
notpp( +
nullpp, 0
)pp0 1
{qq 
queryParametersrr 
.rr  
Addrr  #
(rr# $
$"rr$ &
$strrr& 5
{rr5 6
queryrr6 ;
.rr; <
Specialisationrr< J
}rrJ K
"rrK L
)rrL M
;rrM N
}ss 
ifuu 
(uu 
queryuu 
.uu 
IsActiveuu 
isuu !
notuu" %
nulluu& *
)uu* +
{vv 
queryParametersww 
.ww  
Addww  #
(ww# $
$"ww$ &
$strww& /
{ww/ 0
queryww0 5
.ww5 6
IsActiveww6 >
.ww> ?
Valueww? D
.wwD E
ToStringwwE M
(wwM N
)wwN O
.wwO P
ToLowerInvariantwwP `
(ww` a
)wwa b
}wwb c
"wwc d
)wwd e
;wwe f
}xx 
returnzz 
$"zz 
{zz  
AdminDoctorsEndpointzz *
}zz* +
$strzz+ ,
{zz, -
stringzz- 3
.zz3 4
Joinzz4 8
(zz8 9
$strzz9 <
,zz< =
queryParameterszz> M
)zzM N
}zzN O
"zzO P
;zzP Q
}{{ 	
private}} 
static}} 
DateTime}} +
GetApproximatePracticeStartDate}}  ?
(}}? @
int}}@ C
yearsOfExperience}}D U
)}}U V
{~~ 	
if 
( 
yearsOfExperience !
<=" $
$num% &
)& '
{
ÄÄ 
return
ÅÅ 
DateTime
ÅÅ 
.
ÅÅ  
Today
ÅÅ  %
;
ÅÅ% &
}
ÇÇ 
return
ÑÑ 
DateTime
ÑÑ 
.
ÑÑ 
Today
ÑÑ !
.
ÑÑ! "
AddYears
ÑÑ" *
(
ÑÑ* +
-
ÑÑ+ ,
yearsOfExperience
ÑÑ, =
)
ÑÑ= >
;
ÑÑ> ?
}
ÖÖ 	
}
ÜÜ 
}áá Ω[
mC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\BaseApiService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public		 

abstract		 
class		 
BaseApiService		 (
{

 
private 
const 
string 
TokenStorageKey ,
=- .
$str/ 6
;6 7
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
private 
readonly 

IJSRuntime #

_jsRuntime$ .
;. /
private 
readonly 
NavigationManager *
_navigationManager+ =
;= >
	protected 
BaseApiService  
(  !

HttpClient 

httpClient !
,! "

IJSRuntime 
	jsRuntime  
,  !
NavigationManager 
navigationManager /
)/ 0
{ 	
_httpClient 
= 

httpClient $
;$ %

_jsRuntime 
= 
	jsRuntime "
;" #
_navigationManager 
=  
navigationManager! 2
;2 3
} 	
	protected 
async 
Task 
< 
	TResponse &
>& '
GetAuthorizedAsync( :
<: ;
	TResponse; D
>D E
(E F
string   
endpoint   
,   
string!! 
unauthorizedMessage!! &
)!!& '
where"" 
	TResponse"" 
:"" 
new"" !
(""! "
)""" #
{## 	
using$$ 
var$$ 
response$$ 
=$$  
await$$! &
SendAuthorizedAsync$$' :
($$: ;

HttpMethod%% 
.%% 
Get%% 
,%% 
endpoint&& 
)&& 
;&& 
await(( )
EnsureAuthorizedResponseAsync(( /
(((/ 0
response((0 8
,((8 9
unauthorizedMessage((: M
)((M N
;((N O
response** 
.** #
EnsureSuccessStatusCode** ,
(**, -
)**- .
;**. /
var,, 
result,, 
=,, 
await,, 
response,, '
.,,' (
Content,,( /
.,,/ 0
ReadFromJsonAsync,,0 A
<,,A B
	TResponse,,B K
>,,K L
(,,L M
),,M N
;,,N O
return.. 
result.. 
??.. 
new..  
	TResponse..! *
(..* +
)..+ ,
;.., -
}// 	
	protected11 
async11 
Task11 
<11 
	TResponse11 &
>11& '
PostAuthorizedAsync11( ;
<11; <
TRequest11< D
,11D E
	TResponse11F O
>11O P
(11P Q
string22 
endpoint22 
,22 
TRequest33 
requestData33  
,33  !
string44 
unauthorizedMessage44 &
)44& '
where55 
	TResponse55 
:55 
new55 !
(55! "
)55" #
{66 	
using77 
var77 
response77 
=77  
await77! &
SendAuthorizedAsync77' :
(77: ;

HttpMethod88 
.88 
Post88 
,88  
endpoint99 
,99 
requestData:: 
):: 
;:: 
await<< )
EnsureAuthorizedResponseAsync<< /
(<</ 0
response<<0 8
,<<8 9
unauthorizedMessage<<: M
)<<M N
;<<N O
response>> 
.>> #
EnsureSuccessStatusCode>> ,
(>>, -
)>>- .
;>>. /
var@@ 
result@@ 
=@@ 
await@@ 
response@@ '
.@@' (
Content@@( /
.@@/ 0
ReadFromJsonAsync@@0 A
<@@A B
	TResponse@@B K
>@@K L
(@@L M
)@@M N
;@@N O
returnBB 
resultBB 
??BB 
newBB  
	TResponseBB! *
(BB* +
)BB+ ,
;BB, -
}CC 	
	protectedEE 
asyncEE 
TaskEE 
<EE 
	TResponseEE &
>EE& '
PutAuthorizedAsyncEE( :
<EE: ;
TRequestEE; C
,EEC D
	TResponseEEE N
>EEN O
(EEO P
stringFF 
endpointFF 
,FF 
TRequestGG 
requestDataGG  
,GG  !
stringHH 
unauthorizedMessageHH &
)HH& '
whereII 
	TResponseII 
:II 
newII !
(II! "
)II" #
{JJ 	
usingKK 
varKK 
responseKK 
=KK  
awaitKK! &
SendAuthorizedAsyncKK' :
(KK: ;

HttpMethodLL 
.LL 
PutLL 
,LL 
endpointMM 
,MM 
requestDataNN 
)NN 
;NN 
awaitPP )
EnsureAuthorizedResponseAsyncPP /
(PP/ 0
responsePP0 8
,PP8 9
unauthorizedMessagePP: M
)PPM N
;PPN O
responseRR 
.RR #
EnsureSuccessStatusCodeRR ,
(RR, -
)RR- .
;RR. /
varTT 
resultTT 
=TT 
awaitTT 
responseTT '
.TT' (
ContentTT( /
.TT/ 0
ReadFromJsonAsyncTT0 A
<TTA B
	TResponseTTB K
>TTK L
(TTL M
)TTM N
;TTN O
returnVV 
resultVV 
??VV 
newVV  
	TResponseVV! *
(VV* +
)VV+ ,
;VV, -
}WW 	
	protectedYY 
asyncYY 
TaskYY 
<YY 
	TResponseYY &
>YY& '!
DeleteAuthorizedAsyncYY( =
<YY= >
	TResponseYY> G
>YYG H
(YYH I
stringZZ 
endpointZZ 
,ZZ 
string[[ 
unauthorizedMessage[[ &
)[[& '
where\\ 
	TResponse\\ 
:\\ 
new\\ !
(\\! "
)\\" #
{]] 	
using^^ 
var^^ 
response^^ 
=^^  
await^^! &
SendAuthorizedAsync^^' :
(^^: ;

HttpMethod__ 
.__ 
Delete__ !
,__! "
endpoint`` 
)`` 
;`` 
awaitbb )
EnsureAuthorizedResponseAsyncbb /
(bb/ 0
responsebb0 8
,bb8 9
unauthorizedMessagebb: M
)bbM N
;bbN O
responsedd 
.dd #
EnsureSuccessStatusCodedd ,
(dd, -
)dd- .
;dd. /
varff 
resultff 
=ff 
awaitff 
responseff '
.ff' (
Contentff( /
.ff/ 0
ReadFromJsonAsyncff0 A
<ffA B
	TResponseffB K
>ffK L
(ffL M
)ffM N
;ffN O
returnhh 
resulthh 
??hh 
newhh  
	TResponsehh! *
(hh* +
)hh+ ,
;hh, -
}ii 	
privatekk 
asynckk 
Taskkk 
<kk 
HttpResponseMessagekk .
>kk. /
SendAuthorizedAsynckk0 C
(kkC D

HttpMethodll 
methodll 
,ll 
stringmm 
endpointmm 
,mm 
objectnn 
?nn 
requestDatann 
=nn  !
nullnn" &
)nn& '
{oo 	
varpp 
tokenpp 
=pp 
awaitpp 
GetTokenAsyncpp +
(pp+ ,
)pp, -
;pp- .
ifrr 
(rr 
stringrr 
.rr 
IsNullOrWhiteSpacerr )
(rr) *
tokenrr* /
)rr/ 0
)rr0 1
{ss 
awaittt  
RedirectToLoginAsynctt *
(tt* +
)tt+ ,
;tt, -
throwvv 
newvv '
UnauthorizedAccessExceptionvv 5
(vv5 6
$strww M
)wwM N
;wwN O
}xx 
usingzz 
varzz 
requestzz 
=zz 
newzz  #
HttpRequestMessagezz$ 6
(zz6 7
methodzz7 =
,zz= >
endpointzz? G
)zzG H
;zzH I
request|| 
.|| 
Headers|| 
.|| 
Authorization|| )
=||* +
new}} %
AuthenticationHeaderValue}} -
(}}- .
$str}}. 6
,}}6 7
token}}8 =
)}}= >
;}}> ?
if 
( 
requestData 
is 
not "
null# '
)' (
{
ÄÄ 
request
ÅÅ 
.
ÅÅ 
Content
ÅÅ 
=
ÅÅ  !
JsonContent
ÅÅ" -
.
ÅÅ- .
Create
ÅÅ. 4
(
ÅÅ4 5
requestData
ÅÅ5 @
)
ÅÅ@ A
;
ÅÅA B
}
ÇÇ 
return
ÑÑ 
await
ÑÑ 
_httpClient
ÑÑ $
.
ÑÑ$ %
	SendAsync
ÑÑ% .
(
ÑÑ. /
request
ÑÑ/ 6
)
ÑÑ6 7
;
ÑÑ7 8
}
ÖÖ 	
private
áá 
async
áá 
Task
áá 
<
áá 
string
áá !
?
áá! "
>
áá" #
GetTokenAsync
áá$ 1
(
áá1 2
)
áá2 3
{
àà 	
return
ââ 
await
ââ 

_jsRuntime
ââ #
.
ââ# $
InvokeAsync
ââ$ /
<
ââ/ 0
string
ââ0 6
?
ââ6 7
>
ââ7 8
(
ââ8 9
$str
ää &
,
ää& '
TokenStorageKey
ãã 
)
ãã  
;
ãã  !
}
åå 	
private
éé 
async
éé 
Task
éé +
EnsureAuthorizedResponseAsync
éé 8
(
éé8 9!
HttpResponseMessage
èè 
response
èè  (
,
èè( )
string
êê !
unauthorizedMessage
êê &
)
êê& '
{
ëë 	
if
íí 
(
íí 
response
íí 
.
íí 

StatusCode
íí #
==
íí$ &
HttpStatusCode
íí' 5
.
íí5 6
Unauthorized
íí6 B
||
ííC E
response
ìì 
.
ìì 

StatusCode
ìì #
==
ìì$ &
HttpStatusCode
ìì' 5
.
ìì5 6
	Forbidden
ìì6 ?
)
ìì? @
{
îî 
await
ïï "
RedirectToLoginAsync
ïï *
(
ïï* +
)
ïï+ ,
;
ïï, -
throw
óó 
new
óó )
UnauthorizedAccessException
óó 5
(
óó5 6!
unauthorizedMessage
óó6 I
)
óóI J
;
óóJ K
}
òò 
}
ôô 	
private
õõ 
async
õõ 
Task
õõ "
RedirectToLoginAsync
õõ /
(
õõ/ 0
)
õõ0 1
{
úú 	
await
ùù 

_jsRuntime
ùù 
.
ùù 
InvokeVoidAsync
ùù ,
(
ùù, -
$str
ûû )
,
ûû) *
TokenStorageKey
üü 
)
üü  
;
üü  ! 
_navigationManager
°° 
.
°° 

NavigateTo
°° )
(
°°) *
$str
¢¢ ,
,
¢¢, -
replace
££ 
:
££ 
true
££ 
)
££ 
;
££ 
}
§§ 	
}
•• 
}¶¶ —l
jC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\AuthService.cs
	namespace

 	
HealthCareApp


 
.

 
AdminBlazor

 #
.

# $
Services

$ ,
.

, -
Impl

- 1
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
const 
string 
TokenStorageKey ,
=- .
$str/ 6
;6 7
private 
const 
string 
AdminRoleName *
=+ ,
$str- 4
;4 5
private 
const 
string 
LoginEndpoint *
=+ ,
$str- =
;= >
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
private 
readonly 

IJSRuntime #

_jsRuntime$ .
;. /
private 
readonly -
!CustomAuthenticationStateProvider :
_authStateProvider; M
;M N
public 
AuthService 
( 

HttpClient 

httpClient !
,! "

IJSRuntime 
	jsRuntime  
,  !-
!CustomAuthenticationStateProvider -
authStateProvider. ?
)? @
{ 	
_httpClient 
= 

httpClient $
;$ %

_jsRuntime 
= 
	jsRuntime "
;" #
_authStateProvider 
=  
authStateProvider! 2
;2 3
} 	
public   
async   
Task   
<   
AuthResponseDto   )
>  ) *

LoginAsync  + 5
(  5 6
LoginDto  6 >
loginDto  ? G
)  G H
{!! 	
var"" 
response"" 
="" 
await""  
_httpClient""! ,
."", -
PostAsJsonAsync""- <
(""< =
LoginEndpoint""= J
,""J K
loginDto""L T
)""T U
;""U V
if$$ 
($$ 
!$$ 
response$$ 
.$$ 
IsSuccessStatusCode$$ -
)$$- .
{%% 
return&&  
CreateFailedResponse&& +
(&&+ ,
$str&&, H
)&&H I
;&&I J
}'' 
var)) 
authResponse)) 
=)) 
await)) $
response))% -
.))- .
Content)). 5
.))5 6
ReadFromJsonAsync))6 G
<))G H
AuthResponseDto))H W
>))W X
())X Y
)))Y Z
;))Z [
if++ 
(++ 
authResponse++ 
is++ 
null++  $
||++% '
string++( .
.++. /
IsNullOrWhiteSpace++/ A
(++A B
authResponse++B N
.++N O
AccessToken++O Z
)++Z [
)++[ \
{,, 
return--  
CreateFailedResponse-- +
(--+ ,
$str--, Q
)--Q R
;--R S
}.. 
var00 
role00 
=00 
GetRoleFromToken00 '
(00' (
authResponse00( 4
.004 5
AccessToken005 @
)00@ A
;00A B
if22 
(22 
!22 
string22 
.22 
Equals22 
(22 
role22 #
,22# $
AdminRoleName22% 2
,222 3
StringComparison224 D
.22D E
OrdinalIgnoreCase22E V
)22V W
)22W X
{33 
await44 
LogoutAsync44 !
(44! "
)44" #
;44# $
return66  
CreateFailedResponse66 +
(66+ ,
$str66, V
)66V W
;66W X
}77 
await99 

_jsRuntime99 
.99 
InvokeVoidAsync99 ,
(99, -
$str:: &
,::& '
TokenStorageKey;; 
,;;  
authResponse<< 
.<< 
AccessToken<< (
)<<( )
;<<) *
_authStateProvider>> 
.>> 
NotifyUserLoggedIn>> 1
(>>1 2
authResponse>>2 >
.>>> ?
AccessToken>>? J
)>>J K
;>>K L
return@@ 
authResponse@@ 
;@@  
}AA 	
publicCC 
asyncCC 
TaskCC 
LogoutAsyncCC %
(CC% &
)CC& '
{DD 	
awaitEE 

_jsRuntimeEE 
.EE 
InvokeVoidAsyncEE ,
(EE, -
$strFF )
,FF) *
TokenStorageKeyGG 
)GG  
;GG  !
_authStateProviderII 
.II 
NotifyUserLoggedOutII 2
(II2 3
)II3 4
;II4 5
}JJ 	
publicLL 
asyncLL 
TaskLL 
<LL 
boolLL 
>LL  
IsAuthenticatedAsyncLL  4
(LL4 5
)LL5 6
{MM 	
varNN 
tokenNN 
=NN 
awaitNN 
GetTokenAsyncNN +
(NN+ ,
)NN, -
;NN- .
returnPP 
!PP 
stringPP 
.PP 
IsNullOrWhiteSpacePP -
(PP- .
tokenPP. 3
)PP3 4
;PP4 5
}QQ 	
publicSS 
asyncSS 
TaskSS 
<SS 
stringSS  
?SS  !
>SS! "$
GetCurrentUserEmailAsyncSS# ;
(SS; <
)SS< =
{TT 	
varUU 
tokenUU 
=UU 
awaitUU 
GetTokenAsyncUU +
(UU+ ,
)UU, -
;UU- .
ifWW 
(WW 
stringWW 
.WW 
IsNullOrWhiteSpaceWW )
(WW) *
tokenWW* /
)WW/ 0
)WW0 1
{XX 
returnYY 
nullYY 
;YY 
}ZZ 
return\\ 
GetEmailFromToken\\ $
(\\$ %
token\\% *
)\\* +
;\\+ ,
}]] 	
public__ 
async__ 
Task__ 
<__ 
string__  
?__  !
>__! "#
GetCurrentUserRoleAsync__# :
(__: ;
)__; <
{`` 	
varaa 
tokenaa 
=aa 
awaitaa 
GetTokenAsyncaa +
(aa+ ,
)aa, -
;aa- .
ifcc 
(cc 
stringcc 
.cc 
IsNullOrWhiteSpacecc )
(cc) *
tokencc* /
)cc/ 0
)cc0 1
{dd 
returnee 
nullee 
;ee 
}ff 
returnhh 
GetRoleFromTokenhh #
(hh# $
tokenhh$ )
)hh) *
;hh* +
}ii 	
privatekk 
asynckk 
Taskkk 
<kk 
stringkk !
?kk! "
>kk" #
GetTokenAsynckk$ 1
(kk1 2
)kk2 3
{ll 	
returnmm 
awaitmm 

_jsRuntimemm #
.mm# $
InvokeAsyncmm$ /
<mm/ 0
stringmm0 6
?mm6 7
>mm7 8
(mm8 9
$strnn &
,nn& '
TokenStorageKeyoo 
)oo  
;oo  !
}pp 	
privaterr 
staticrr 
AuthResponseDtorr & 
CreateFailedResponserr' ;
(rr; <
stringrr< B
messagerrC J
)rrJ K
{ss 	
returntt 
newtt 
AuthResponseDtott &
{uu 
AccessTokenvv 
=vv 
stringvv $
.vv$ %
Emptyvv% *
,vv* +
Messageww 
=ww 
messageww !
,ww! "
	ExpiresInxx 
=xx 
$numxx 
}yy 
;yy 
}zz 	
private|| 
static|| 
string|| 
GetEmailFromToken|| /
(||/ 0
string||0 6
token||7 <
)||< =
{}} 	
var~~ 
payload~~ 
=~~ 
GetJwtPayload~~ '
(~~' (
token~~( -
)~~- .
;~~. /
if
ÄÄ 
(
ÄÄ 
payload
ÄÄ 
.
ÄÄ 
TryGetProperty
ÄÄ &
(
ÄÄ& '
$str
ÄÄ' .
,
ÄÄ. /
out
ÄÄ0 3
var
ÄÄ4 7

emailClaim
ÄÄ8 B
)
ÄÄB C
)
ÄÄC D
{
ÅÅ 
return
ÇÇ 

emailClaim
ÇÇ !
.
ÇÇ! "
	GetString
ÇÇ" +
(
ÇÇ+ ,
)
ÇÇ, -
??
ÇÇ. 0
string
ÇÇ1 7
.
ÇÇ7 8
Empty
ÇÇ8 =
;
ÇÇ= >
}
ÉÉ 
return
ÖÖ 
string
ÖÖ 
.
ÖÖ 
Empty
ÖÖ 
;
ÖÖ  
}
ÜÜ 	
private
àà 
static
àà 
string
àà 
GetRoleFromToken
àà .
(
àà. /
string
àà/ 5
token
àà6 ;
)
àà; <
{
ââ 	
var
ää 
payload
ää 
=
ää 
GetJwtPayload
ää '
(
ää' (
token
ää( -
)
ää- .
;
ää. /
const
åå 
string
åå 
roleClaimUri
åå %
=
åå& '
$str
çç N
;
ççN O
if
èè 
(
èè 
payload
èè 
.
èè 
TryGetProperty
èè &
(
èè& '
roleClaimUri
èè' 3
,
èè3 4
out
èè5 8
var
èè9 <
	roleClaim
èè= F
)
èèF G
)
èèG H
{
êê 
return
ëë 
	roleClaim
ëë  
.
ëë  !
	GetString
ëë! *
(
ëë* +
)
ëë+ ,
??
ëë- /
string
ëë0 6
.
ëë6 7
Empty
ëë7 <
;
ëë< =
}
íí 
if
îî 
(
îî 
payload
îî 
.
îî 
TryGetProperty
îî &
(
îî& '

ClaimTypes
îî' 1
.
îî1 2
Role
îî2 6
,
îî6 7
out
îî8 ;
var
îî< ?
claimTypesRole
îî@ N
)
îîN O
)
îîO P
{
ïï 
return
ññ 
claimTypesRole
ññ %
.
ññ% &
	GetString
ññ& /
(
ññ/ 0
)
ññ0 1
??
ññ2 4
string
ññ5 ;
.
ññ; <
Empty
ññ< A
;
ññA B
}
óó 
if
ôô 
(
ôô 
payload
ôô 
.
ôô 
TryGetProperty
ôô &
(
ôô& '
$str
ôô' -
,
ôô- .
out
ôô/ 2
var
ôô3 6
simpleRoleClaim
ôô7 F
)
ôôF G
)
ôôG H
{
öö 
return
õõ 
simpleRoleClaim
õõ &
.
õõ& '
	GetString
õõ' 0
(
õõ0 1
)
õõ1 2
??
õõ3 5
string
õõ6 <
.
õõ< =
Empty
õõ= B
;
õõB C
}
úú 
return
ûû 
string
ûû 
.
ûû 
Empty
ûû 
;
ûû  
}
üü 	
private
°° 
static
°° 
JsonElement
°° "
GetJwtPayload
°°# 0
(
°°0 1
string
°°1 7
token
°°8 =
)
°°= >
{
¢¢ 	
var
££ 

tokenParts
££ 
=
££ 
token
££ "
.
££" #
Split
££# (
(
££( )
$char
££) ,
)
££, -
;
££- .
if
•• 
(
•• 

tokenParts
•• 
.
•• 
Length
•• !
<
••" #
$num
••$ %
)
••% &
{
¶¶ 
return
ßß 
default
ßß 
;
ßß 
}
®® 
var
™™ 
payload
™™ 
=
™™ 

tokenParts
™™ $
[
™™$ %
$num
™™% &
]
™™& '
.
´´ 
Replace
´´ 
(
´´ 
$char
´´ 
,
´´ 
$char
´´ !
)
´´! "
.
¨¨ 
Replace
¨¨ 
(
¨¨ 
$char
¨¨ 
,
¨¨ 
$char
¨¨ !
)
¨¨! "
;
¨¨" #
payload
ÆÆ 
=
ÆÆ 
AddBase64Padding
ÆÆ &
(
ÆÆ& '
payload
ÆÆ' .
)
ÆÆ. /
;
ÆÆ/ 0
var
∞∞ 
	jsonBytes
∞∞ 
=
∞∞ 
Convert
∞∞ #
.
∞∞# $
FromBase64String
∞∞$ 4
(
∞∞4 5
payload
∞∞5 <
)
∞∞< =
;
∞∞= >
var
≤≤ 
json
≤≤ 
=
≤≤ 
Encoding
≤≤ 
.
≤≤  
UTF8
≤≤  $
.
≤≤$ %
	GetString
≤≤% .
(
≤≤. /
	jsonBytes
≤≤/ 8
)
≤≤8 9
;
≤≤9 :
return
¥¥ 
JsonSerializer
¥¥ !
.
¥¥! "
Deserialize
¥¥" -
<
¥¥- .
JsonElement
¥¥. 9
>
¥¥9 :
(
¥¥: ;
json
¥¥; ?
)
¥¥? @
;
¥¥@ A
}
µµ 	
private
∑∑ 
static
∑∑ 
string
∑∑ 
AddBase64Padding
∑∑ .
(
∑∑. /
string
∑∑/ 5
base64
∑∑6 <
)
∑∑< =
{
∏∏ 	
int
ππ 
	remainder
ππ 
=
ππ 
base64
ππ "
.
ππ" #
Length
ππ# )
%
ππ* +
$num
ππ, -
;
ππ- .
if
ªª 
(
ªª 
	remainder
ªª 
==
ªª 
$num
ªª 
)
ªª 
{
ºº 
return
ΩΩ 
base64
ΩΩ 
+
ΩΩ 
$str
ΩΩ  $
;
ΩΩ$ %
}
ææ 
if
¿¿ 
(
¿¿ 
	remainder
¿¿ 
==
¿¿ 
$num
¿¿ 
)
¿¿ 
{
¡¡ 
return
¬¬ 
base64
¬¬ 
+
¬¬ 
$str
¬¬  #
;
¬¬# $
}
√√ 
return
≈≈ 
base64
≈≈ 
;
≈≈ 
}
∆∆ 	
}
«« 
}»» Æõ
vC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\AppointmentAdminService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class #
AppointmentAdminService (
:) *$
IAppointmentAdminService+ C
{ 
private 
const 
string 
TokenStorageKey ,
=- .
$str/ 6
;6 7
private 
const 
string  
AppointmentsEndpoint 1
=2 3
$str4 F
;F G
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
private 
readonly 

IJSRuntime #

_jsRuntime$ .
;. /
private 
static 
readonly !
JsonSerializerOptions  5
JsonOptions6 A
=B C
newD G
(G H
)H I
{ 	'
PropertyNameCaseInsensitive '
=( )
true* .
} 	
;	 

public #
AppointmentAdminService &
(& '

HttpClient 

httpClient !
,! "

IJSRuntime 
	jsRuntime  
)  !
{ 	
_httpClient   
=   

httpClient   $
;  $ %

_jsRuntime"" 
="" 
	jsRuntime"" "
;""" #
}## 	
public%% 
async%% 
Task%% 
<%% 
List%% 
<%% 
AppointmentDto%% -
>%%- .
>%%. /#
GetAllAppointmentsAsync%%0 G
(%%G H
)%%H I
{&& 	
var'' 
query'' 
='' 
new'' )
AppointmentPaginationQueryDto'' 9
{(( 

PageNumber)) 
=)) 
$num)) 
,)) 
PageSize++ 
=++ 
$num++ 
},, 
;,, 
var.. 
response.. 
=.. 
await..  %
GetAppointmentsPagedAsync..! :
(..: ;
query..; @
)..@ A
;..A B
return00 
response00 
.00 
Items00 !
??00" $
new00% (
List00) -
<00- .
AppointmentDto00. <
>00< =
(00= >
)00> ?
;00? @
}11 	
public33 
async33 
Task33 
<33 
PagedResponse33 '
<33' (
AppointmentDto33( 6
>336 7
>337 8%
GetAppointmentsPagedAsync339 R
(33R S)
AppointmentPaginationQueryDto44 )
query44* /
)44/ 0
{55 	
string66 
endpoint66 
=66 %
BuildAppointmentsEndpoint66 7
(667 8
query668 =
)66= >
;66> ?
using88 
var88 
response88 
=88  
await88! &&
SendAuthorizedRequestAsync88' A
(88A B

HttpMethod99 
.99 
Get99 
,99 
endpoint:: 
):: 
;:: $
EnsureAuthorizedResponse<< $
(<<$ %
response== 
,== 
$str>> P
)>>P Q
;>>Q R
response@@ 
.@@ #
EnsureSuccessStatusCode@@ ,
(@@, -
)@@- .
;@@. /
varBB 
pagedResponseBB 
=BB 
awaitBB  %
responseBB& .
.BB. /
ContentBB/ 6
.CC 
ReadFromJsonAsyncCC "
<CC" #
PagedResponseCC# 0
<CC0 1
AppointmentDtoCC1 ?
>CC? @
>CC@ A
(CCA B
JsonOptionsCCB M
)CCM N
;CCN O
returnEE 
pagedResponseEE  
??EE! #
newEE$ '
PagedResponseEE( 5
<EE5 6
AppointmentDtoEE6 D
>EED E
{FF 
ItemsGG 
=GG 
newGG 
ListGG  
<GG  !
AppointmentDtoGG! /
>GG/ 0
(GG0 1
)GG1 2
,GG2 3

PageNumberII 
=II 
queryII "
.II" #

PageNumberII# -
,II- .
PageSizeKK 
=KK 
queryKK  
.KK  !
PageSizeKK! )
,KK) *
TotalRecordsMM 
=MM 
$numMM  
,MM  !

TotalPagesOO 
=OO 
$numOO 
}PP 
;PP 
}QQ 	
publicSS 
asyncSS 
TaskSS 
<SS '
AppointmentFilterOptionsDtoSS 5
>SS5 6,
 GetAppointmentFilterOptionsAsyncSS7 W
(SSW X
)SSX Y
{TT 	
usingUU 
varUU 
responseUU 
=UU  
awaitUU! &&
SendAuthorizedRequestAsyncUU' A
(UUA B

HttpMethodVV 
.VV 
GetVV 
,VV 
$"WW 
{WW  
AppointmentsEndpointWW '
}WW' (
$strWW( 7
"WW7 8
)WW8 9
;WW9 :$
EnsureAuthorizedResponseYY $
(YY$ %
responseZZ 
,ZZ 
$str[[ Z
)[[Z [
;[[[ \
response]] 
.]] #
EnsureSuccessStatusCode]] ,
(]], -
)]]- .
;]]. /
var__ 
filterOptions__ 
=__ 
await__  %
response__& .
.__. /
Content__/ 6
.`` 
ReadFromJsonAsync`` "
<``" #'
AppointmentFilterOptionsDto``# >
>``> ?
(``? @
JsonOptions``@ K
)``K L
;``L M
returnbb 
filterOptionsbb  
??bb! #
newbb$ ''
AppointmentFilterOptionsDtobb( C
(bbC D
)bbD E
;bbE F
}cc 	
publicee 
asyncee 
Taskee 
<ee ,
 AppointmentDailyStatusSummaryDtoee :
>ee: ;&
GetDailyStatusSummaryAsyncee< V
(eeV W
DateTimeeeW _
dateee` d
)eed e
{ff 	
stringgg 
endpointgg 
=gg 
$"hh 
{hh  
AppointmentsEndpointhh '
}hh' (
$strhh( C
{hhC D
UrihhD G
.hhG H
EscapeDataStringhhH X
(hhX Y
datehhY ]
.hh] ^
ToStringhh^ f
(hhf g
$strhhg s
)hhs t
)hht u
}hhu v
"hhv w
;hhw x
usingjj 
varjj 
responsejj 
=jj  
awaitjj! &&
SendAuthorizedRequestAsyncjj' A
(jjA B

HttpMethodkk 
.kk 
Getkk 
,kk 
endpointll 
)ll 
;ll $
EnsureAuthorizedResponsenn $
(nn$ %
responseoo 
,oo 
$strpp Y
)ppY Z
;ppZ [
responserr 
.rr #
EnsureSuccessStatusCoderr ,
(rr, -
)rr- .
;rr. /
vartt 
summarytt 
=tt 
awaittt 
responsett  (
.tt( )
Contenttt) 0
.uu 
ReadFromJsonAsyncuu "
<uu" #,
 AppointmentDailyStatusSummaryDtouu# C
>uuC D
(uuD E
JsonOptionsuuE P
)uuP Q
;uuQ R
returnww 
summaryww 
??ww 
newww !,
 AppointmentDailyStatusSummaryDtoww" B
{xx 
Dateyy 
=yy 
dateyy 
.yy 
ToStringyy $
(yy$ %
$stryy% 1
)yy1 2
}zz 
;zz 
}{{ 	
public|| 
async|| 
Task|| 
<|| 
AppointmentDto|| (
?||( )
>||) *#
GetAppointmentByIdAsync||+ B
(||B C
int||C F
appointmentId||G T
)||T U
{}} 	
if~~ 
(~~ 
appointmentId~~ 
<=~~  
$num~~! "
)~~" #
{ 
return
ÄÄ 
null
ÄÄ 
;
ÄÄ 
}
ÅÅ 
using
ÉÉ 
var
ÉÉ 
response
ÉÉ 
=
ÉÉ  
await
ÉÉ! &(
SendAuthorizedRequestAsync
ÉÉ' A
(
ÉÉA B

HttpMethod
ÑÑ 
.
ÑÑ 
Get
ÑÑ 
,
ÑÑ 
$"
ÖÖ 
{
ÖÖ "
AppointmentsEndpoint
ÖÖ '
}
ÖÖ' (
$str
ÖÖ( )
{
ÖÖ) *
appointmentId
ÖÖ* 7
}
ÖÖ7 8
"
ÖÖ8 9
)
ÖÖ9 :
;
ÖÖ: ;
if
áá 
(
áá 
response
áá 
.
áá 

StatusCode
áá #
==
áá$ &
HttpStatusCode
áá' 5
.
áá5 6
NotFound
áá6 >
)
áá> ?
{
àà 
return
ââ 
null
ââ 
;
ââ 
}
ää &
EnsureAuthorizedResponse
åå $
(
åå$ %
response
çç 
,
çç 
$str
éé S
)
ééS T
;
ééT U
response
êê 
.
êê %
EnsureSuccessStatusCode
êê ,
(
êê, -
)
êê- .
;
êê. /
return
íí 
await
íí 
response
íí !
.
íí! "
Content
íí" )
.
íí) *
ReadFromJsonAsync
íí* ;
<
íí; <
AppointmentDto
íí< J
>
ííJ K
(
ííK L
JsonOptions
ííL W
)
ííW X
;
ííX Y
}
ìì 	
public
ïï 
async
ïï 
Task
ïï 
<
ïï 
AppointmentDto
ïï (
>
ïï( )$
CreateAppointmentAsync
ïï* @
(
ïï@ A 
BookAppointmentDto
ïïA S
request
ïïT [
)
ïï[ \
{
ññ 	
using
óó 
var
óó 
response
óó 
=
óó  
await
óó! &(
SendAuthorizedRequestAsync
óó' A
(
óóA B

HttpMethod
òò 
.
òò 
Post
òò 
,
òò  "
AppointmentsEndpoint
ôô $
,
ôô$ %
request
öö 
)
öö 
;
öö &
EnsureAuthorizedResponse
úú $
(
úú$ %
response
ùù 
,
ùù 
$str
ûû N
)
ûûN O
;
ûûO P
response
†† 
.
†† %
EnsureSuccessStatusCode
†† ,
(
††, -
)
††- .
;
††. /
var
¢¢ 
appointment
¢¢ 
=
¢¢ 
await
¢¢ #
response
¢¢$ ,
.
¢¢, -
Content
¢¢- 4
.
¢¢4 5
ReadFromJsonAsync
¢¢5 F
<
¢¢F G
AppointmentDto
¢¢G U
>
¢¢U V
(
¢¢V W
JsonOptions
¢¢W b
)
¢¢b c
;
¢¢c d
return
§§ 
appointment
§§ 
??
§§ !
new
§§" %
AppointmentDto
§§& 4
(
§§4 5
)
§§5 6
;
§§6 7
}
•• 	
public
ßß 
Task
ßß 
<
ßß 
AppointmentDto
ßß "
?
ßß" #
>
ßß# $$
UpdateAppointmentAsync
ßß% ;
(
ßß; <
int
®® 
appointmentId
®® 
,
®® "
UpdateAppointmentDto
©©  
request
©©! (
)
©©( )
{
™™ 	
return
´´ 
Task
´´ 
.
´´ 

FromResult
´´ "
<
´´" #
AppointmentDto
´´# 1
?
´´1 2
>
´´2 3
(
´´3 4
null
´´4 8
)
´´8 9
;
´´9 :
}
¨¨ 	
public
ÆÆ 
Task
ÆÆ 
<
ÆÆ 
bool
ÆÆ 
>
ÆÆ $
DeleteAppointmentAsync
ÆÆ 0
(
ÆÆ0 1
int
ÆÆ1 4
appointmentId
ÆÆ5 B
)
ÆÆB C
{
ØØ 	
return
∞∞ 
Task
∞∞ 
.
∞∞ 

FromResult
∞∞ "
(
∞∞" #
false
∞∞# (
)
∞∞( )
;
∞∞) *
}
±± 	
private
≥≥ 
static
≥≥ 
string
≥≥ '
BuildAppointmentsEndpoint
≥≥ 7
(
≥≥7 8+
AppointmentPaginationQueryDto
≥≥8 U
query
≥≥V [
)
≥≥[ \
{
¥¥ 	
var
µµ 
queryParameters
µµ 
=
µµ  !
new
µµ" %
List
µµ& *
<
µµ* +
string
µµ+ 1
>
µµ1 2
{
∂∂ 
$"
∑∑ 
$str
∑∑ 
{
∑∑ 
query
∑∑ #
.
∑∑# $

PageNumber
∑∑$ .
}
∑∑. /
"
∑∑/ 0
,
∑∑0 1
$"
ππ 
$str
ππ 
{
ππ 
query
ππ !
.
ππ! "
PageSize
ππ" *
}
ππ* +
"
ππ+ ,
}
∫∫ 
;
∫∫ 
if
ºº 
(
ºº 
!
ºº 
string
ºº 
.
ºº  
IsNullOrWhiteSpace
ºº *
(
ºº* +
query
ºº+ 0
.
ºº0 1

SearchTerm
ºº1 ;
)
ºº; <
)
ºº< =
{
ΩΩ 
queryParameters
ææ 
.
ææ  
Add
ææ  #
(
ææ# $
$"
ææ$ &
$str
ææ& 1
{
ææ1 2
Uri
ææ2 5
.
ææ5 6
EscapeDataString
ææ6 F
(
ææF G
query
ææG L
.
ææL M

SearchTerm
ææM W
)
ææW X
}
ææX Y
"
ææY Z
)
ææZ [
;
ææ[ \
}
øø 
if
¡¡ 
(
¡¡ 
query
¡¡ 
.
¡¡ 
	PatientId
¡¡ 
is
¡¡  "
not
¡¡# &
null
¡¡' +
)
¡¡+ ,
{
¬¬ 
queryParameters
√√ 
.
√√  
Add
√√  #
(
√√# $
$"
√√$ &
$str
√√& 0
{
√√0 1
query
√√1 6
.
√√6 7
	PatientId
√√7 @
.
√√@ A
Value
√√A F
}
√√F G
"
√√G H
)
√√H I
;
√√I J
}
ƒƒ 
if
∆∆ 
(
∆∆ 
query
∆∆ 
.
∆∆ 
DoctorId
∆∆ 
is
∆∆ !
not
∆∆" %
null
∆∆& *
)
∆∆* +
{
«« 
queryParameters
»» 
.
»»  
Add
»»  #
(
»»# $
$"
»»$ &
$str
»»& /
{
»»/ 0
query
»»0 5
.
»»5 6
DoctorId
»»6 >
.
»»> ?
Value
»»? D
}
»»D E
"
»»E F
)
»»F G
;
»»G H
}
…… 
if
ÀÀ 
(
ÀÀ 
query
ÀÀ 
.
ÀÀ 
Status
ÀÀ 
is
ÀÀ 
not
ÀÀ  #
null
ÀÀ$ (
)
ÀÀ( )
{
ÃÃ 
queryParameters
ÕÕ 
.
ÕÕ  
Add
ÕÕ  #
(
ÕÕ# $
$"
ÕÕ$ &
$str
ÕÕ& -
{
ÕÕ- .
query
ÕÕ. 3
.
ÕÕ3 4
Status
ÕÕ4 :
.
ÕÕ: ;
Value
ÕÕ; @
}
ÕÕ@ A
"
ÕÕA B
)
ÕÕB C
;
ÕÕC D
}
ŒŒ 
if
–– 
(
–– 
query
–– 
.
–– 
ScheduledDate
–– #
is
––$ &
not
––' *
null
––+ /
)
––/ 0
{
—— 
queryParameters
““ 
.
““  
Add
““  #
(
““# $
$"
““$ &
$str
““& 4
{
““4 5
query
““5 :
.
““: ;
ScheduledDate
““; H
.
““H I
Value
““I N
:
““N O
$str
““O Y
}
““Y Z
"
““Z [
)
““[ \
;
““\ ]
}
”” 
if
’’ 
(
’’ 
query
’’ 
.
’’ 
UpcomingOnly
’’ "
is
’’# %
not
’’& )
null
’’* .
)
’’. /
{
÷÷ 
queryParameters
◊◊ 
.
◊◊  
Add
◊◊  #
(
◊◊# $
$"
◊◊$ &
$str
◊◊& 3
{
◊◊3 4
query
◊◊4 9
.
◊◊9 :
UpcomingOnly
◊◊: F
.
◊◊F G
Value
◊◊G L
.
◊◊L M
ToString
◊◊M U
(
◊◊U V
)
◊◊V W
.
◊◊W X
ToLowerInvariant
◊◊X h
(
◊◊h i
)
◊◊i j
}
◊◊j k
"
◊◊k l
)
◊◊l m
;
◊◊m n
}
ÿÿ 
return
⁄⁄ 
$"
⁄⁄ 
{
⁄⁄ "
AppointmentsEndpoint
⁄⁄ *
}
⁄⁄* +
$str
⁄⁄+ ,
{
⁄⁄, -
string
⁄⁄- 3
.
⁄⁄3 4
Join
⁄⁄4 8
(
⁄⁄8 9
$str
⁄⁄9 <
,
⁄⁄< =
queryParameters
⁄⁄> M
)
⁄⁄M N
}
⁄⁄N O
"
⁄⁄O P
;
⁄⁄P Q
}
€€ 	
private
›› 
async
›› 
Task
›› 
<
›› !
HttpResponseMessage
›› .
>
››. /(
SendAuthorizedRequestAsync
››0 J
(
››J K

HttpMethod
ﬁﬁ 
method
ﬁﬁ 
,
ﬁﬁ 
string
ﬂﬂ 
endpoint
ﬂﬂ 
,
ﬂﬂ 
object
‡‡ 
?
‡‡ 
requestData
‡‡ 
=
‡‡  !
null
‡‡" &
)
‡‡& '
{
·· 	
var
‚‚ 
token
‚‚ 
=
‚‚ 
await
‚‚ 

_jsRuntime
‚‚ (
.
‚‚( )
InvokeAsync
‚‚) 4
<
‚‚4 5
string
‚‚5 ;
?
‚‚; <
>
‚‚< =
(
‚‚= >
$str
„„ &
,
„„& '
TokenStorageKey
‰‰ 
)
‰‰  
;
‰‰  !
if
ÊÊ 
(
ÊÊ 
string
ÊÊ 
.
ÊÊ  
IsNullOrWhiteSpace
ÊÊ )
(
ÊÊ) *
token
ÊÊ* /
)
ÊÊ/ 0
)
ÊÊ0 1
{
ÁÁ 
throw
ËË 
new
ËË )
UnauthorizedAccessException
ËË 5
(
ËË5 6
$str
ÈÈ M
)
ÈÈM N
;
ÈÈN O
}
ÍÍ 
using
ÏÏ 
var
ÏÏ 
request
ÏÏ 
=
ÏÏ 
new
ÏÏ  # 
HttpRequestMessage
ÏÏ$ 6
(
ÏÏ6 7
method
ÏÏ7 =
,
ÏÏ= >
endpoint
ÏÏ? G
)
ÏÏG H
;
ÏÏH I
request
ÓÓ 
.
ÓÓ 
Headers
ÓÓ 
.
ÓÓ 
Authorization
ÓÓ )
=
ÓÓ* +
new
ÔÔ '
AuthenticationHeaderValue
ÔÔ -
(
ÔÔ- .
$str
ÔÔ. 6
,
ÔÔ6 7
token
ÔÔ8 =
)
ÔÔ= >
;
ÔÔ> ?
if
ÒÒ 
(
ÒÒ 
requestData
ÒÒ 
is
ÒÒ 
not
ÒÒ "
null
ÒÒ# '
)
ÒÒ' (
{
ÚÚ 
request
ÛÛ 
.
ÛÛ 
Content
ÛÛ 
=
ÛÛ  !
JsonContent
ÛÛ" -
.
ÛÛ- .
Create
ÛÛ. 4
(
ÛÛ4 5
requestData
ÛÛ5 @
)
ÛÛ@ A
;
ÛÛA B
}
ÙÙ 
return
ˆˆ 
await
ˆˆ 
_httpClient
ˆˆ $
.
ˆˆ$ %
	SendAsync
ˆˆ% .
(
ˆˆ. /
request
ˆˆ/ 6
)
ˆˆ6 7
;
ˆˆ7 8
}
˜˜ 	
private
˘˘ 
static
˘˘ 
void
˘˘ &
EnsureAuthorizedResponse
˘˘ 4
(
˘˘4 5!
HttpResponseMessage
˙˙ 
response
˙˙  (
,
˙˙( )
string
˚˚ !
unauthorizedMessage
˚˚ &
)
˚˚& '
{
¸¸ 	
if
˝˝ 
(
˝˝ 
response
˝˝ 
.
˝˝ 

StatusCode
˝˝ #
==
˝˝$ &
HttpStatusCode
˝˝' 5
.
˝˝5 6
Unauthorized
˝˝6 B
||
˝˝C E
response
˛˛ 
.
˛˛ 

StatusCode
˛˛ #
==
˛˛$ &
HttpStatusCode
˛˛' 5
.
˛˛5 6
	Forbidden
˛˛6 ?
)
˛˛? @
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ )
UnauthorizedAccessException
ÄÄ 5
(
ÄÄ5 6!
unauthorizedMessage
ÄÄ6 I
)
ÄÄI J
;
ÄÄJ K
}
ÅÅ 
}
ÇÇ 	
}
ÉÉ 
}ÑÑ ¨
tC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Services\Impl\AdminDashboardService.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Services$ ,
., -
Impl- 1
{ 
public 

class !
AdminDashboardService &
:' (
BaseApiService) 7
,7 8"
IAdminDashboardService9 O
{		 
private

 
const

 
string

 
DashboardEndpoint

 .
=

/ 0
$str

1 F
;

F G
public !
AdminDashboardService $
($ %

HttpClient 

httpClient 
, 

IJSRuntime 
	jsRuntime 
, 
NavigationManager 
navigationManager )
)) *
: 
base 
( 

httpClient 
, 
	jsRuntime "
," #
navigationManager$ 5
)5 6
{ 	
} 	
public 
async 
Task 
< #
AdminDashboardReportDto 1
>1 2#
GetDashboardReportAsync3 J
(J K
)K L
{ 	
return 
await 
GetAuthorizedAsync +
<+ ,#
AdminDashboardReportDto, C
>C D
(D E
DashboardEndpoint !
,! "
$str N
)N O
;O P
} 	
} 
} Ò
XC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Program.cs
var

 
builder

 
=

 "
WebAssemblyHostBuilder

 $
.

$ %
CreateDefault

% 2
(

2 3
args

3 7
)

7 8
;

8 9
builder 
. 
RootComponents 
. 
Add 
< 
App 
> 
(  
$str  &
)& '
;' (
builder 
. 
RootComponents 
. 
Add 
< 

HeadOutlet %
>% &
(& '
$str' 4
)4 5
;5 6
builder 
. 
Services 
. 
	AddScoped 
( 
sp 
=>  
new 

HttpClient 
{ 
BaseAddress 
= 
new 
Uri 
( 
$str 7
)7 8
} 
) 
; 
builder 
. 
Services 
.  
AddAuthorizationCore %
(% &
)& '
;' (
builder 
. 
Services 
. 
	AddScoped 
< -
!CustomAuthenticationStateProvider <
>< =
(= >
)> ?
;? @
builder 
. 
Services 
. 
	AddScoped 
< '
AuthenticationStateProvider 6
>6 7
(7 8
sp8 :
=>; =
{ 
return 

sp 
. 
GetRequiredService  
<  !-
!CustomAuthenticationStateProvider! B
>B C
(C D
)D E
;E F
} 
) 
; 
builder 
. 
Services 
. 
	AddScoped 
< 
IAuthService '
,' (
AuthService) 4
>4 5
(5 6
)6 7
;7 8
builder   
.   
Services   
.   
	AddScoped   
<   "
IAdminDashboardService   1
,  1 2!
AdminDashboardService  3 H
>  H I
(  I J
)  J K
;  K L
builder!! 
.!! 
Services!! 
.!! 
	AddScoped!! 
<!! 
IDoctorAdminService!! .
,!!. /
DoctorAdminService!!0 B
>!!B C
(!!C D
)!!D E
;!!E F
builder"" 
."" 
Services"" 
."" 
	AddScoped"" 
<""  
IPatientAdminService"" /
,""/ 0
PatientAdminService""1 D
>""D E
(""E F
)""F G
;""G H
builder## 
.## 
Services## 
.## 
	AddScoped## 
<## $
IAppointmentAdminService## 3
,##3 4#
AppointmentAdminService##5 L
>##L M
(##M N
)##N O
;##O P
builder$$ 
.$$ 
Services$$ 
.$$ 
	AddScoped$$ 
<$$ "
IAdminDashboardService$$ 1
,$$1 2!
AdminDashboardService$$3 H
>$$H I
($$I J
)$$J K
;$$K L
builder%% 
.%% 
Services%% 
.%% 
	AddScoped%% 
<%% 
IToastService%% (
,%%( )
ToastService%%* 6
>%%6 7
(%%7 8
)%%8 9
;%%9 :
await'' 
builder'' 
.'' 
Build'' 
('' 
)'' 
.'' 
RunAsync'' 
('' 
)''  
;''  !†Z
wC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp.AdminBlazor\Auth\CustomAuthenticationStateProvider.cs
	namespace 	
HealthCareApp
 
. 
AdminBlazor #
.# $
Auth$ (
{ 
public		 

class		 -
!CustomAuthenticationStateProvider		 2
:		3 4'
AuthenticationStateProvider		5 P
{

 
private 
const 
string 
TokenStorageKey ,
=- .
$str/ 6
;6 7
private 
const 
string 
AuthenticationType /
=0 1
$str2 7
;7 8
private 
readonly 

IJSRuntime #

_jsRuntime$ .
;. /
private 
ClaimsPrincipal 
_currentUser  ,
=- .
CreateAnonymousUser/ B
(B C
)C D
;D E
public -
!CustomAuthenticationStateProvider 0
(0 1

IJSRuntime1 ;
	jsRuntime< E
)E F
{ 	

_jsRuntime 
= 
	jsRuntime "
;" #
} 	
public 
override 
async 
Task "
<" #
AuthenticationState# 6
>6 7'
GetAuthenticationStateAsync8 S
(S T
)T U
{ 	
if 
( 
_currentUser 
. 
Identity %
?% &
.& '
IsAuthenticated' 6
==7 9
true: >
)> ?
{ 
return 
new 
AuthenticationState .
(. /
_currentUser/ ;
); <
;< =
} 
var 
token 
= 
await 

_jsRuntime (
.( )
InvokeAsync) 4
<4 5
string5 ;
?; <
>< =
(= >
$str &
,& '
TokenStorageKey   
)    
;    !
if"" 
("" 
string"" 
."" 
IsNullOrWhiteSpace"" )
("") *
token""* /
)""/ 0
)""0 1
{## 
return$$ .
"CreateAnonymousAuthenticationState$$ 9
($$9 :
)$$: ;
;$$; <
}%% 
var'' 
claims'' 
='' 
ParseClaimsFromJwt'' +
(''+ ,
token'', 1
)''1 2
;''2 3
if)) 
()) 
!)) 
claims)) 
.)) 
Any)) 
()) 
))) 
))) 
{** 
return++ .
"CreateAnonymousAuthenticationState++ 9
(++9 :
)++: ;
;++; <
},, 
_currentUser.. 
=.. #
CreateAuthenticatedUser.. 2
(..2 3
claims..3 9
)..9 :
;..: ;
return00 
new00 
AuthenticationState00 *
(00* +
_currentUser00+ 7
)007 8
;008 9
}11 	
public33 
void33 
NotifyUserLoggedIn33 &
(33& '
string33' -
token33. 3
)333 4
{44 	
var55 
claims55 
=55 
ParseClaimsFromJwt55 +
(55+ ,
token55, 1
)551 2
;552 3
_currentUser77 
=77 #
CreateAuthenticatedUser77 2
(772 3
claims773 9
)779 :
;77: ;,
 NotifyAuthenticationStateChanged99 ,
(99, -
Task:: 
.:: 

FromResult:: 
(::  
new::  #
AuthenticationState::$ 7
(::7 8
_currentUser::8 D
)::D E
)::E F
)::F G
;::G H
};; 	
public== 
void== 
NotifyUserLoggedOut== '
(==' (
)==( )
{>> 	
_currentUser?? 
=?? 
CreateAnonymousUser?? .
(??. /
)??/ 0
;??0 1,
 NotifyAuthenticationStateChangedAA ,
(AA, -
TaskBB 
.BB 

FromResultBB 
(BB  .
"CreateAnonymousAuthenticationStateBB  B
(BBB C
)BBC D
)BBD E
)BBE F
;BBF G
}CC 	
privateEE 
staticEE 
AuthenticationStateEE *.
"CreateAnonymousAuthenticationStateEE+ M
(EEM N
)EEN O
{FF 	
returnGG 
newGG 
AuthenticationStateGG *
(GG* +
CreateAnonymousUserGG+ >
(GG> ?
)GG? @
)GG@ A
;GGA B
}HH 	
privateJJ 
staticJJ 
ClaimsPrincipalJJ &
CreateAnonymousUserJJ' :
(JJ: ;
)JJ; <
{KK 	
returnLL 
newLL 
ClaimsPrincipalLL &
(LL& '
newLL' *
ClaimsIdentityLL+ 9
(LL9 :
)LL: ;
)LL; <
;LL< =
}MM 	
privateOO 
staticOO 
ClaimsPrincipalOO &#
CreateAuthenticatedUserOO' >
(OO> ?
IEnumerableOO? J
<OOJ K
ClaimOOK P
>OOP Q
claimsOOR X
)OOX Y
{PP 	
varQQ 
identityQQ 
=QQ 
newQQ 
ClaimsIdentityQQ -
(QQ- .
claimsQQ. 4
,QQ4 5
AuthenticationTypeQQ6 H
)QQH I
;QQI J
returnSS 
newSS 
ClaimsPrincipalSS &
(SS& '
identitySS' /
)SS/ 0
;SS0 1
}TT 	
privateVV 
staticVV 
IEnumerableVV "
<VV" #
ClaimVV# (
>VV( )
ParseClaimsFromJwtVV* <
(VV< =
stringVV= C
jwtVVD G
)VVG H
{WW 	
varXX 
claimsXX 
=XX 
newXX 
ListXX !
<XX! "
ClaimXX" '
>XX' (
(XX( )
)XX) *
;XX* +
varZZ 

tokenPartsZZ 
=ZZ 
jwtZZ  
.ZZ  !
SplitZZ! &
(ZZ& '
$charZZ' *
)ZZ* +
;ZZ+ ,
if\\ 
(\\ 

tokenParts\\ 
.\\ 
Length\\ !
<\\" #
$num\\$ %
)\\% &
{]] 
return^^ 
claims^^ 
;^^ 
}__ 
tryaa 
{bb 
varcc 
payloadBytescc  
=cc! "
DecodeJwtPayloadcc# 3
(cc3 4

tokenPartscc4 >
[cc> ?
$numcc? @
]cc@ A
)ccA B
;ccB C
varee 
keyValuePairsee !
=ee" #
JsonSerializerff "
.ff" #
Deserializeff# .
<ff. /

Dictionaryff/ 9
<ff9 :
stringff: @
,ff@ A
JsonElementffB M
>ffM N
>ffN O
(ffO P
payloadBytesffP \
)ff\ ]
;ff] ^
ifhh 
(hh 
keyValuePairshh !
ishh" $
nullhh% )
)hh) *
{ii 
returnjj 
claimsjj !
;jj! "
}kk 
foreachmm 
(mm 
varmm 
keyValuePairmm )
inmm* ,
keyValuePairsmm- :
)mm: ;
{nn 
	AddClaimsoo 
(oo 
claimsoo $
,oo$ %
keyValuePairoo& 2
.oo2 3
Keyoo3 6
,oo6 7
keyValuePairoo8 D
.ooD E
ValueooE J
)ooJ K
;ooK L
}pp 
}qq 
catchrr 
(rr 
FormatExceptionrr "
)rr" #
{ss 
returntt 
claimstt 
;tt 
}uu 
catchvv 
(vv 
JsonExceptionvv  
)vv  !
{ww 
returnxx 
claimsxx 
;xx 
}yy 
return{{ 
claims{{ 
;{{ 
}|| 	
private~~ 
static~~ 
byte~~ 
[~~ 
]~~ 
DecodeJwtPayload~~ .
(~~. /
string~~/ 5
payload~~6 =
)~~= >
{ 	
var
ÄÄ 
base64
ÄÄ 
=
ÄÄ 
payload
ÄÄ  
.
ÅÅ 
Replace
ÅÅ 
(
ÅÅ 
$char
ÅÅ 
,
ÅÅ 
$char
ÅÅ !
)
ÅÅ! "
.
ÇÇ 
Replace
ÇÇ 
(
ÇÇ 
$char
ÇÇ 
,
ÇÇ 
$char
ÇÇ !
)
ÇÇ! "
;
ÇÇ" #
base64
ÑÑ 
=
ÑÑ 
AddBase64Padding
ÑÑ %
(
ÑÑ% &
base64
ÑÑ& ,
)
ÑÑ, -
;
ÑÑ- .
return
ÜÜ 
Convert
ÜÜ 
.
ÜÜ 
FromBase64String
ÜÜ +
(
ÜÜ+ ,
base64
ÜÜ, 2
)
ÜÜ2 3
;
ÜÜ3 4
}
áá 	
private
àà 
static
àà 
string
àà 
AddBase64Padding
àà .
(
àà. /
string
àà/ 5
base64
àà6 <
)
àà< =
{
ââ 	
int
ää 
	remainder
ää 
=
ää 
base64
ää "
.
ää" #
Length
ää# )
%
ää* +
$num
ää, -
;
ää- .
if
åå 
(
åå 
	remainder
åå 
==
åå 
$num
åå 
)
åå 
{
çç 
return
éé 
base64
éé 
+
éé 
$str
éé  $
;
éé$ %
}
èè 
if
ëë 
(
ëë 
	remainder
ëë 
==
ëë 
$num
ëë 
)
ëë 
{
íí 
return
ìì 
base64
ìì 
+
ìì 
$str
ìì  #
;
ìì# $
}
îî 
return
ññ 
base64
ññ 
;
ññ 
}
óó 	
private
ôô 
static
ôô 
void
ôô 
	AddClaims
ôô %
(
ôô% &
List
öö 
<
öö 
Claim
öö 
>
öö 
claims
öö 
,
öö 
string
õõ 
	claimType
õõ 
,
õõ 
JsonElement
úú 

claimValue
úú "
)
úú" #
{
ùù 	
if
ûû 
(
ûû 

claimValue
ûû 
.
ûû 
	ValueKind
ûû $
==
ûû% '
JsonValueKind
ûû( 5
.
ûû5 6
Array
ûû6 ;
)
ûû; <
{
üü 
foreach
†† 
(
†† 
var
†† 
value
†† "
in
††# %

claimValue
††& 0
.
††0 1
EnumerateArray
††1 ?
(
††? @
)
††@ A
)
††A B
{
°° 
claims
¢¢ 
.
¢¢ 
Add
¢¢ 
(
¢¢ 
new
¢¢ "
Claim
¢¢# (
(
¢¢( )
	claimType
¢¢) 2
,
¢¢2 3
value
¢¢4 9
.
¢¢9 :
ToString
¢¢: B
(
¢¢B C
)
¢¢C D
)
¢¢D E
)
¢¢E F
;
¢¢F G
}
££ 
return
•• 
;
•• 
}
¶¶ 
claims
®® 
.
®® 
Add
®® 
(
®® 
new
®® 
Claim
®®  
(
®®  !
	claimType
®®! *
,
®®* +

claimValue
®®, 6
.
®®6 7
ToString
®®7 ?
(
®®? @
)
®®@ A
)
®®A B
)
®®B C
;
®®C D
}
©© 	
}
™™ 
}´´ 