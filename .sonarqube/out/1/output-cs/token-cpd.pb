ù
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\TokenAuthenticationHandler.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class &
TokenAuthenticationHandler +
:, -
DelegatingHandler. ?
{ 
private 
readonly 

IJSRuntime #

_jsRuntime$ .
;. /
public

 &
TokenAuthenticationHandler

 )
(

) *

IJSRuntime

* 4
	jsRuntime

5 >
)

> ?
{ 	

_jsRuntime 
= 
	jsRuntime "
;" #
} 	
	protected 
override 
async  
Task! %
<% &
HttpResponseMessage& 9
>9 :
	SendAsync; D
(D E
HttpRequestMessageE W
requestX _
,_ `
CancellationTokena r
cancellationToken	s Ñ
)
Ñ Ö
{ 	
var 
token 
= 
await 

_jsRuntime (
.( )
InvokeAsync) 4
<4 5
string5 ;
>; <
(< =
$str= S
,S T
$strU `
)` a
;a b
if 
( 
! 
string 
. 
IsNullOrWhiteSpace *
(* +
token+ 0
)0 1
)1 2
{ 
request 
. 
Headers 
.  
Authorization  -
=. /
new0 3%
AuthenticationHeaderValue4 M
(M N
$strN V
,V W
tokenX ]
)] ^
;^ _
} 
return 
await 
base 
. 
	SendAsync '
(' (
request( /
,/ 0
cancellationToken1 B
)B C
;C D
} 	
} 
} Í
UC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\JwtParser.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

static 
class 
	JwtParser !
{ 
public 
static 
IEnumerable !
<! "
Claim" '
>' (
ParseClaimsFromJwt) ;
(; <
string< B
jwtC F
)F G
{		 	
var

 
claims

 
=

 
new

 
List

 !
<

! "
Claim

" '
>

' (
(

( )
)

) *
;

* +
var 
payload 
= 
jwt 
. 
Split #
(# $
$char$ '
)' (
[( )
$num) *
]* +
;+ ,
var 
	jsonBytes 
= %
ParseBase64WithoutPadding 5
(5 6
payload6 =
)= >
;> ?
var 
keyValuePairs 
= 
JsonSerializer  .
.. /
Deserialize/ :
<: ;

Dictionary; E
<E F
stringF L
,L M
objectN T
>T U
>U V
(V W
	jsonBytesW `
)` a
;a b
if 
( 
keyValuePairs 
!=  
null! %
)% &
{ 
claims 
. 
AddRange 
(  
keyValuePairs  -
.- .
Select. 4
(4 5
kvp5 8
=>9 ;
new< ?
Claim@ E
(E F
kvpF I
.I J
KeyJ M
,M N
kvpO R
.R S
ValueS X
?X Y
.Y Z
ToStringZ b
(b c
)c d
??e g
$strh j
)j k
)k l
)l m
;m n
} 
return 
claims 
; 
} 	
private 
static 
byte 
[ 
] %
ParseBase64WithoutPadding 7
(7 8
string8 >
base64? E
)E F
{ 	
switch 
( 
base64 
. 
Length !
%" #
$num$ %
)% &
{ 
case 
$num 
: 
base64 
+= !
$str" &
;& '
break( -
;- .
case 
$num 
: 
base64 
+= !
$str" %
;% &
break' ,
;, -
} 
return 
Convert 
. 
FromBase64String +
(+ ,
base64, 2
)2 3
;3 4
} 	
} 
}   ˚
eC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Interface\IPatientService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

	interface 
IPatientService $
{ 
Task 
< 
List 
< 

PatientDto 
> 
? 
> 
GetAllAsync  +
(+ ,
), -
;- .
Task 
< 

PatientDto 
? 
> 
GetByIdAsync &
(& '
int' *
id+ -
)- .
;. /
Task		 
<		 
List		 
<		 "
PatientSearchResultDto		 (
>		( )
?		) *
>		* +
SearchByNameAsync		, =
(		= >
string		> D
name		E I
)		I J
;		J K
Task

 
<

 

PatientDto

 
?

 
>

 
CreateAsync

 %
(

% &
CreatePatientDto

& 6
dto

7 :
)

: ;
;

; <
Task 
< 
bool 
> 
UpdateAsync 
( 
int "
id# %
,% &
UpdatePatientDto' 7
dto8 ;
); <
;< =
Task 
< 
bool 
> 
ActivateAsync  
(  !
int! $
id% '
)' (
;( )
Task 
< 
bool 
> 
DeactivateAsync "
(" #
int# &
id' )
)) *
;* +
} 
} ¡
dC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Interface\IDoctorService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

	interface 
IDoctorService #
{ 
Task 
< 
List 
< 
	DoctorDto 
> 
? 
> 
GetAllAsync *
(* +
string+ 1
?1 2
sortBy3 9
=: ;
null< @
,@ A
intB E
?E F
specialisationG U
=V W
nullX \
)\ ]
;] ^
Task 
< 
	DoctorDto 
? 
> 
GetByIdAsync %
(% &
int& )
id* ,
), -
;- .
Task		 
<		 #
DoctorCreationResultDto		 $
?		$ %
>		% &
CreateAsync		' 2
(		2 3
CreateDoctorDto		3 B
dto		C F
)		F G
;		G H
Task

 
<

 
bool

 
>

 
UpdateAsync

 
(

 
int

 "
id

# %
,

% &
UpdateDoctorDto

' 6
dto

7 :
)

: ;
;

; <
Task 
< 
bool 
> 
ActivateAsync  
(  !
int! $
id% '
)' (
;( )
Task 
< 
bool 
> 
DeactivateAsync "
(" #
int# &
id' )
)) *
;* +
} 
} „
bC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Interface\IAuthService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

	interface 
IAuthService !
{ 
Task 
< 
bool 
> 

LoginAsync 
( 
LoginDto &
request' .
). /
;/ 0
Task 
LogoutAsync 
( 
) 
; 
}		 
}

 ∂
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Interface\IAppointmentService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

	interface 
IAppointmentService (
{ 
Task 
< 
List 
< !
AppointmentDetailsDto '
>' (
?( )
>) *
GetAllAsync+ 6
(6 7
)7 8
;8 9
Task 
< !
AppointmentDetailsDto "
?" #
># $
GetByIdAsync% 1
(1 2
int2 5
id6 8
)8 9
;9 :
Task		 
<		 
List		 
<		 (
PatientAppointmentHistoryDto		 .
>		. /
?		/ 0
>		0 1"
GetPatientHistoryAsync		2 H
(		H I
int		I L
	patientId		M V
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
 !
DoctorScheduleItemDto

 '
>

' (
?

( )
>

) *'
GetDoctorTodayScheduleAsync

+ F
(

F G
int

G J
doctorId

K S
)

S T
;

T U
Task 
< 
List 
< !
DoctorScheduleItemDto '
>' (
?( )
>) *&
GetDoctorWeekScheduleAsync+ E
(E F
intF I
doctorIdJ R
,R S
DateOnlyT \
	startDate] f
,f g
DateOnlyh p
endDateq x
)x y
;y z
Task 
< 
List 
< !
DoctorScheduleItemDto '
>' (
?( )
>) **
GetDoctorUpcomingScheduleAsync+ I
(I J
intJ M
doctorIdN V
)V W
;W X
Task 
< 
AppointmentDto 
? 
> 
CreateAsync )
() * 
CreateAppointmentDto* >
dto? B
)B C
;C D
Task 
< 
bool 
> 
UpdateAsync 
( 
int "
id# %
,% & 
UpdateAppointmentDto' ;
dto< ?
)? @
;@ A
Task 
< 
bool 
> 
UpdateStatusAsync $
($ %
int% (
id) +
,+ ,&
UpdateAppointmentStatusDto- G
dtoH K
)K L
;L M
Task 
< 
bool 
> 
ConfirmAsync 
(  
int  #
id$ &
)& '
;' (
Task 
< 
bool 
> 
CompleteAsync  
(  !
int! $
id% '
)' (
;( )
Task 
< 
bool 
> 
CancelAsync 
( 
int "
id# %
,% & 
CancelAppointmentDto' ;
dto< ?
)? @
;@ A
} 
} ˝
cC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Interface\IAdminService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

	interface 
IAdminService "
{ 
Task 
< 
DashboardDto 
? 
> 
GetDashboardAsync -
(- .
). /
;/ 0
Task

 
<

 
StatisticsDto

 
?

 
>

 
GetStatisticsAsync

 /
(

/ 0
)

0 1
;

1 2
Task 
< 
List 
< 
UserManagementDto #
># $
?$ %
>% &
GetUsersAsync' 4
(4 5
)5 6
;6 7
} 
} ı.
iC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Implementation\PatientService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class 
PatientService 
:  !
IPatientService" 1
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public

 
PatientService

 
(

 

HttpClient

 (

httpClient

) 3
)

3 4
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
List 
< 

PatientDto )
>) *
?* +
>+ ,
GetAllAsync- 8
(8 9
)9 :
{ 	
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;

PatientDto; E
>E F
>F G
(G H
$strH V
)V W
;W X
} 	
public 
async 
Task 
< 

PatientDto $
?$ %
>% &
GetByIdAsync' 3
(3 4
int4 7
id8 :
): ;
{ 	
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6

PatientDto6 @
>@ A
(A B
$"B D
$strD Q
{Q R
idR T
}T U
"U V
)V W
;W X
} 	
public 
async 
Task 
< 
List 
< "
PatientSearchResultDto 5
>5 6
?6 7
>7 8
SearchByNameAsync9 J
(J K
stringK Q
nameR V
)V W
{ 	
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;"
PatientSearchResultDto; Q
>Q R
>R S
(S T
$"T V
$strV o
{o p
namep t
}t u
"u v
)v w
;w x
} 	
public 
async 
Task 
< 

PatientDto $
?$ %
>% &
CreateAsync' 2
(2 3
CreatePatientDto3 C
dtoD G
)G H
{ 	
var   
response   
=   
await    
_httpClient  ! ,
.  , -
PostAsJsonAsync  - <
(  < =
$str  = K
,  K L
dto  M P
)  P Q
;  Q R
if!! 
(!! 
response!! 
.!! 
IsSuccessStatusCode!! ,
)!!, -
{"" 
return## 
await## 
response## %
.##% &
Content##& -
.##- .
ReadFromJsonAsync##. ?
<##? @

PatientDto##@ J
>##J K
(##K L
)##L M
;##M N
}$$ 
throw&& 
new&& 
	Exception&& 
(&&  
await&&  %
response&&& .
.&&. /
Content&&/ 6
.&&6 7
ReadAsStringAsync&&7 H
(&&H I
)&&I J
)&&J K
;&&K L
}'' 	
public)) 
async)) 
Task)) 
<)) 
bool)) 
>)) 
UpdateAsync))  +
())+ ,
int)), /
id))0 2
,))2 3
UpdatePatientDto))4 D
dto))E H
)))H I
{** 	
var++ 
response++ 
=++ 
await++  
_httpClient++! ,
.++, -
PutAsJsonAsync++- ;
(++; <
$"++< >
$str++> K
{++K L
id++L N
}++N O
"++O P
,++P Q
dto++R U
)++U V
;++V W
if,, 
(,, 
response,, 
.,, 
IsSuccessStatusCode,, ,
),,, -
{-- 
return.. 
true.. 
;.. 
}// 
throw00 
new00 
	Exception00 
(00  
await00  %
response00& .
.00. /
Content00/ 6
.006 7
ReadAsStringAsync007 H
(00H I
)00I J
)00J K
;00K L
}11 	
public33 
async33 
Task33 
<33 
bool33 
>33 
ActivateAsync33  -
(33- .
int33. 1
id332 4
)334 5
{44 	
var55 
response55 
=55 
await55  
_httpClient55! ,
.55, -
PutAsync55- 5
(555 6
$"556 8
$str558 E
{55E F
id55F H
}55H I
$str55I R
"55R S
,55S T
null55U Y
)55Y Z
;55Z [
return66 
response66 
.66 
IsSuccessStatusCode66 /
;66/ 0
}77 	
public99 
async99 
Task99 
<99 
bool99 
>99 
DeactivateAsync99  /
(99/ 0
int990 3
id994 6
)996 7
{:: 	
var;; 
response;; 
=;; 
await;;  
_httpClient;;! ,
.;;, -
PutAsync;;- 5
(;;5 6
$";;6 8
$str;;8 E
{;;E F
id;;F H
};;H I
$str;;I T
";;T U
,;;U V
null;;W [
);;[ \
;;;\ ]
return<< 
response<< 
.<< 
IsSuccessStatusCode<< /
;<</ 0
}== 	
}>> 
}?? §1
hC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Implementation\DoctorService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class 
DoctorService 
:  
IDoctorService! /
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public

 
DoctorService

 
(

 

HttpClient

 '

httpClient

( 2
)

2 3
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
List 
< 
	DoctorDto (
>( )
?) *
>* +
GetAllAsync, 7
(7 8
string8 >
?> ?
sortBy@ F
=G H
nullI M
,M N
intO R
?R S
specialisationT b
=c d
nulle i
)i j
{ 	
var 
url 
= 
$str #
;# $
var 
query 
= 
new 
List  
<  !
string! '
>' (
(( )
)) *
;* +
if 
( 
! 
string 
. 
IsNullOrWhiteSpace *
(* +
sortBy+ 1
)1 2
)2 3
query 
. 
Add 
( 
$" 
$str #
{# $
sortBy$ *
}* +
"+ ,
), -
;- .
if 
( 
specialisation 
. 
HasValue '
)' (
query 
. 
Add 
( 
$" 
$str +
{+ ,
specialisation, :
.: ;
Value; @
}@ A
"A B
)B C
;C D
if 
( 
query 
. 
Any 
( 
) 
) 
url 
+= 
$str 
+ 
string #
.# $
Join$ (
(( )
$str) ,
,, -
query. 3
)3 4
;4 5
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;
	DoctorDto; D
>D E
>E F
(F G
urlG J
)J K
;K L
} 	
public   
async   
Task   
<   
	DoctorDto   #
?  # $
>  $ %
GetByIdAsync  & 2
(  2 3
int  3 6
id  7 9
)  9 :
{!! 	
return"" 
await"" 
_httpClient"" $
.""$ %
GetFromJsonAsync""% 5
<""5 6
	DoctorDto""6 ?
>""? @
(""@ A
$"""A C
$str""C O
{""O P
id""P R
}""R S
"""S T
)""T U
;""U V
}## 	
public%% 
async%% 
Task%% 
<%% #
DoctorCreationResultDto%% 1
?%%1 2
>%%2 3
CreateAsync%%4 ?
(%%? @
CreateDoctorDto%%@ O
dto%%P S
)%%S T
{&& 	
var'' 
response'' 
='' 
await''  
_httpClient''! ,
.'', -
PostAsJsonAsync''- <
(''< =
$str''= J
,''J K
dto''L O
)''O P
;''P Q
if(( 
((( 
response(( 
.(( 
IsSuccessStatusCode(( ,
)((, -
{)) 
return** 
await** 
response** %
.**% &
Content**& -
.**- .
ReadFromJsonAsync**. ?
<**? @#
DoctorCreationResultDto**@ W
>**W X
(**X Y
)**Y Z
;**Z [
}++ 
return,, 
null,, 
;,, 
}-- 	
public// 
async// 
Task// 
<// 
bool// 
>// 
UpdateAsync//  +
(//+ ,
int//, /
id//0 2
,//2 3
UpdateDoctorDto//4 C
dto//D G
)//G H
{00 	
var11 
response11 
=11 
await11  
_httpClient11! ,
.11, -
PutAsJsonAsync11- ;
(11; <
$"11< >
$str11> J
{11J K
id11K M
}11M N
"11N O
,11O P
dto11Q T
)11T U
;11U V
return22 
response22 
.22 
IsSuccessStatusCode22 /
;22/ 0
}33 	
public55 
async55 
Task55 
<55 
bool55 
>55 
ActivateAsync55  -
(55- .
int55. 1
id552 4
)554 5
{66 	
var77 
response77 
=77 
await77  
_httpClient77! ,
.77, -
PutAsync77- 5
(775 6
$"776 8
$str778 D
{77D E
id77E G
}77G H
$str77H Q
"77Q R
,77R S
null77T X
)77X Y
;77Y Z
return88 
response88 
.88 
IsSuccessStatusCode88 /
;88/ 0
}99 	
public;; 
async;; 
Task;; 
<;; 
bool;; 
>;; 
DeactivateAsync;;  /
(;;/ 0
int;;0 3
id;;4 6
);;6 7
{<< 	
var== 
response== 
=== 
await==  
_httpClient==! ,
.==, -
PutAsync==- 5
(==5 6
$"==6 8
$str==8 D
{==D E
id==E G
}==G H
$str==H S
"==S T
,==T U
null==V Z
)==Z [
;==[ \
return>> 
response>> 
.>> 
IsSuccessStatusCode>> /
;>>/ 0
}?? 	
}@@ 
}AA ª
fC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Implementation\AuthService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class 
AuthService 
: 
IAuthService +
{		 
private

 
readonly

 

HttpClient

 #
_httpClient

$ /
;

/ 0
private 
readonly 

IJSRuntime #

_jsRuntime$ .
;. /
private 
readonly '
AuthenticationStateProvider 4
_authStateProvider5 G
;G H
public 
AuthService 
( 

HttpClient %

httpClient& 0
,0 1

IJSRuntime2 <
	jsRuntime= F
,F G'
AuthenticationStateProviderH c
authStateProviderd u
)u v
{ 	
_httpClient 
= 

httpClient $
;$ %

_jsRuntime 
= 
	jsRuntime "
;" #
_authStateProvider 
=  
authStateProvider! 2
;2 3
} 	
public 
async 
Task 
< 
bool 
> 

LoginAsync  *
(* +
LoginDto+ 3
request4 ;
); <
{ 	
var 
response 
= 
await  
_httpClient! ,
., -
PostAsJsonAsync- <
(< =
$str= M
,M N
requestO V
)V W
;W X
if 
( 
! 
response 
. 
IsSuccessStatusCode -
)- .
{ 
return 
false 
; 
} 
var 
authResponse 
= 
await $
response% -
.- .
Content. 5
.5 6
ReadFromJsonAsync6 G
<G H
AuthResponseDtoH W
>W X
(X Y
)Y Z
;Z [
if   
(   
authResponse   
==   
null    $
||  % '
string  ( .
.  . /
IsNullOrEmpty  / <
(  < =
authResponse  = I
.  I J
AccessToken  J U
)  U V
)  V W
{!! 
return"" 
false"" 
;"" 
}## 
await&& 

_jsRuntime&& 
.&& 
InvokeVoidAsync&& ,
(&&, -
$str&&- C
,&&C D
$str&&E P
,&&P Q
authResponse&&R ^
.&&^ _
AccessToken&&_ j
)&&j k
;&&k l
((( 
((( #
CustomAuthStateProvider(( %
)((% &
_authStateProvider((& 8
)((8 9
.((9 :$
NotifyUserAuthentication((: R
(((R S
authResponse((S _
.((_ `
AccessToken((` k
)((k l
;((l m
return** 
true** 
;** 
}++ 	
public-- 
async-- 
Task-- 
LogoutAsync-- %
(--% &
)--& '
{.. 	
await00 

_jsRuntime00 
.00 
InvokeVoidAsync00 ,
(00, -
$str00- F
,00F G
$str00H S
)00S T
;00T U
(11 
(11 #
CustomAuthStateProvider11 %
)11% &
_authStateProvider11& 8
)118 9
.119 :
NotifyUserLogout11: J
(11J K
)11K L
;11L M
}22 	
}33 
}44 ≥V
mC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Implementation\AppointmentService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class 
AppointmentService #
:$ %
IAppointmentService& 9
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
public

 
AppointmentService

 !
(

! "

HttpClient

" ,

httpClient

- 7
)

7 8
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
List 
< !
AppointmentDetailsDto 4
>4 5
?5 6
>6 7
GetAllAsync8 C
(C D
)D E
=>F H
await 
_httpClient 
. 
GetFromJsonAsync .
<. /
List/ 3
<3 4!
AppointmentDetailsDto4 I
>I J
>J K
(K L
$strL ^
)^ _
;_ `
public 
async 
Task 
< !
AppointmentDetailsDto /
?/ 0
>0 1
GetByIdAsync2 >
(> ?
int? B
idC E
)E F
=>G I
await 
_httpClient 
. 
GetFromJsonAsync .
<. /!
AppointmentDetailsDto/ D
>D E
(E F
$"F H
$strH Y
{Y Z
idZ \
}\ ]
"] ^
)^ _
;_ `
public 
async 
Task 
< 
List 
< (
PatientAppointmentHistoryDto ;
>; <
?< =
>= >"
GetPatientHistoryAsync? U
(U V
intV Y
	patientIdZ c
)c d
=>e g
await 
_httpClient 
. 
GetFromJsonAsync .
<. /
List/ 3
<3 4(
PatientAppointmentHistoryDto4 P
>P Q
>Q R
(R S
$"S U
$strU n
{n o
	patientIdo x
}x y
"y z
)z {
;{ |
public 
async 
Task 
< 
List 
< !
DoctorScheduleItemDto 4
>4 5
?5 6
>6 7'
GetDoctorTodayScheduleAsync8 S
(S T
intT W
doctorIdX `
)` a
=>b d
await 
_httpClient 
. 
GetFromJsonAsync .
<. /
List/ 3
<3 4!
DoctorScheduleItemDto4 I
>I J
>J K
(K L
$"L N
$strN f
{f g
doctorIdg o
}o p
$strp v
"v w
)w x
;x y
public 
async 
Task 
< 
List 
< !
DoctorScheduleItemDto 4
>4 5
?5 6
>6 7&
GetDoctorWeekScheduleAsync8 R
(R S
intS V
doctorIdW _
,_ `
DateOnlya i
	startDatej s
,s t
DateOnlyu }
endDate	~ Ö
)
Ö Ü
=>
á â
await 
_httpClient 
. 
GetFromJsonAsync .
<. /
List/ 3
<3 4!
DoctorScheduleItemDto4 I
>I J
>J K
(K L
$"L N
$strN f
{f g
doctorIdg o
}o p
$str	p Ä
{
Ä Å
	startDate
Å ä
:
ä ã
$str
ã ï
}
ï ñ
$str
ñ ü
{
ü †
endDate
† ß
:
ß ®
$str
® ≤
}
≤ ≥
"
≥ ¥
)
¥ µ
;
µ ∂
public 
async 
Task 
< 
List 
< !
DoctorScheduleItemDto 4
>4 5
?5 6
>6 7*
GetDoctorUpcomingScheduleAsync8 V
(V W
intW Z
doctorId[ c
)c d
=>e g
await 
_httpClient 
. 
GetFromJsonAsync .
<. /
List/ 3
<3 4!
DoctorScheduleItemDto4 I
>I J
>J K
(K L
$"L N
$strN f
{f g
doctorIdg o
}o p
$strp y
"y z
)z {
;{ |
public!! 
async!! 
Task!! 
<!! 
AppointmentDto!! (
?!!( )
>!!) *
CreateAsync!!+ 6
(!!6 7 
CreateAppointmentDto!!7 K
dto!!L O
)!!O P
{"" 	
var## 
response## 
=## 
await##  
_httpClient##! ,
.##, -
PostAsJsonAsync##- <
(##< =
$str##= O
,##O P
dto##Q T
)##T U
;##U V
if$$ 
($$ 
response$$ 
.$$ 
IsSuccessStatusCode$$ ,
)$$, -
return%% 
await%% 
response%% %
.%%% &
Content%%& -
.%%- .
ReadFromJsonAsync%%. ?
<%%? @
AppointmentDto%%@ N
>%%N O
(%%O P
)%%P Q
;%%Q R
throw'' 
new'' 
	Exception'' 
(''  
await''  %
response''& .
.''. /
Content''/ 6
.''6 7
ReadAsStringAsync''7 H
(''H I
)''I J
)''J K
;''K L
}(( 	
public** 
async** 
Task** 
<** 
bool** 
>** 
UpdateAsync**  +
(**+ ,
int**, /
id**0 2
,**2 3 
UpdateAppointmentDto**4 H
dto**I L
)**L M
{++ 	
var,, 
response,, 
=,, 
await,,  
_httpClient,,! ,
.,,, -
PutAsJsonAsync,,- ;
(,,; <
$",,< >
$str,,> O
{,,O P
id,,P R
},,R S
",,S T
,,,T U
dto,,V Y
),,Y Z
;,,Z [
if-- 
(-- 
response-- 
.-- 
IsSuccessStatusCode-- ,
)--, -
return--. 4
true--5 9
;--9 :
throw.. 
new.. 
	Exception.. 
(..  
await..  %
response..& .
.... /
Content../ 6
...6 7
ReadAsStringAsync..7 H
(..H I
)..I J
)..J K
;..K L
}// 	
public11 
async11 
Task11 
<11 
bool11 
>11 
UpdateStatusAsync11  1
(111 2
int112 5
id116 8
,118 9&
UpdateAppointmentStatusDto11: T
dto11U X
)11X Y
{22 	
var33 
response33 
=33 
await33  
_httpClient33! ,
.33, -
PutAsJsonAsync33- ;
(33; <
$"33< >
$str33> O
{33O P
id33P R
}33R S
$str33S Z
"33Z [
,33[ \
dto33] `
)33` a
;33a b
if44 
(44 
response44 
.44 
IsSuccessStatusCode44 ,
)44, -
return44. 4
true445 9
;449 :
throw55 
new55 
	Exception55 
(55  
await55  %
response55& .
.55. /
Content55/ 6
.556 7
ReadAsStringAsync557 H
(55H I
)55I J
)55J K
;55K L
}66 	
public88 
async88 
Task88 
<88 
bool88 
>88 
ConfirmAsync88  ,
(88, -
int88- 0
id881 3
)883 4
{99 	
var:: 
response:: 
=:: 
await::  
_httpClient::! ,
.::, -
PutAsync::- 5
(::5 6
$"::6 8
$str::8 I
{::I J
id::J L
}::L M
$str::M U
"::U V
,::V W
null::X \
)::\ ]
;::] ^
if;; 
(;; 
response;; 
.;; 
IsSuccessStatusCode;; ,
);;, -
return;;. 4
true;;5 9
;;;9 :
throw<< 
new<< 
	Exception<< 
(<<  
await<<  %
response<<& .
.<<. /
Content<</ 6
.<<6 7
ReadAsStringAsync<<7 H
(<<H I
)<<I J
)<<J K
;<<K L
}== 	
public?? 
async?? 
Task?? 
<?? 
bool?? 
>?? 
CompleteAsync??  -
(??- .
int??. 1
id??2 4
)??4 5
{@@ 	
varAA 
responseAA 
=AA 
awaitAA  
_httpClientAA! ,
.AA, -
PutAsyncAA- 5
(AA5 6
$"AA6 8
$strAA8 I
{AAI J
idAAJ L
}AAL M
$strAAM V
"AAV W
,AAW X
nullAAY ]
)AA] ^
;AA^ _
ifBB 
(BB 
responseBB 
.BB 
IsSuccessStatusCodeBB ,
)BB, -
returnBB. 4
trueBB5 9
;BB9 :
throwCC 
newCC 
	ExceptionCC 
(CC  
awaitCC  %
responseCC& .
.CC. /
ContentCC/ 6
.CC6 7
ReadAsStringAsyncCC7 H
(CCH I
)CCI J
)CCJ K
;CCK L
}DD 	
publicFF 
asyncFF 
TaskFF 
<FF 
boolFF 
>FF 
CancelAsyncFF  +
(FF+ ,
intFF, /
idFF0 2
,FF2 3 
CancelAppointmentDtoFF4 H
dtoFFI L
)FFL M
{GG 	
varHH 
responseHH 
=HH 
awaitHH  
_httpClientHH! ,
.HH, -
PutAsJsonAsyncHH- ;
(HH; <
$"HH< >
$strHH> O
{HHO P
idHHP R
}HHR S
$strHHS Z
"HHZ [
,HH[ \
dtoHH] `
)HH` a
;HHa b
ifII 
(II 
responseII 
.II 
IsSuccessStatusCodeII ,
)II, -
returnII. 4
trueII5 9
;II9 :
throwJJ 
newJJ 
	ExceptionJJ 
(JJ  
awaitJJ  %
responseJJ& .
.JJ. /
ContentJJ/ 6
.JJ6 7
ReadAsStringAsyncJJ7 H
(JJH I
)JJI J
)JJJ K
;JJK L
}KK 	
}LL 
}MM ˛
gC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\Implementation\AdminService.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class 
AdminService 
: 
IAdminService  -
{ 
private		 
readonly		 

HttpClient		 #
_httpClient		$ /
;		/ 0
public 
AdminService 
( 

HttpClient &

httpClient' 1
)1 2
{ 	
_httpClient 
= 

httpClient $
;$ %
} 	
public 
async 
Task 
< 
DashboardDto &
?& '
>' (
GetDashboardAsync) :
(: ;
); <
{ 	
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6
DashboardDto6 B
>B C
(C D
$strD Y
)Y Z
;Z [
} 	
public 
async 
Task 
< 
StatisticsDto '
?' (
>( )
GetStatisticsAsync* <
(< =
)= >
{ 	
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6
StatisticsDto6 C
>C D
(D E
$strE [
)[ \
;\ ]
} 	
public 
async 
Task 
< 
List 
< 
UserManagementDto 0
>0 1
?1 2
>2 3
GetUsersAsync4 A
(A B
)B C
{ 	
return 
await 
_httpClient $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;
UserManagementDto; L
>L M
>M N
(N O
$strO `
)` a
;a b
} 	
} 
} ∂#
cC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Services\CustomAuthStateProvider.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Services '
{ 
public 

class #
CustomAuthStateProvider (
:) *'
AuthenticationStateProvider+ F
{ 
private		 
readonly		 

IJSRuntime		 #

_jsRuntime		$ .
;		. /
public #
CustomAuthStateProvider &
(& '

IJSRuntime' 1
	jsRuntime2 ;
); <
{ 	

_jsRuntime 
= 
	jsRuntime "
;" #
} 	
public 
override 
async 
Task "
<" #
AuthenticationState# 6
>6 7'
GetAuthenticationStateAsync8 S
(S T
)T U
{ 	
try 
{ 
var 
token 
= 
await !

_jsRuntime" ,
., -
InvokeAsync- 8
<8 9
string9 ?
>? @
(@ A
$strA W
,W X
$strY d
)d e
;e f
if 
( 
string 
. 
IsNullOrWhiteSpace -
(- .
token. 3
)3 4
)4 5
{ 
return 
new 
AuthenticationState 2
(2 3
new3 6
ClaimsPrincipal7 F
(F G
newG J
ClaimsIdentityK Y
(Y Z
)Z [
)[ \
)\ ]
;] ^
} 
var 
claims 
= 
	JwtParser &
.& '
ParseClaimsFromJwt' 9
(9 :
token: ?
)? @
;@ A
var 
identity 
= 
new "
ClaimsIdentity# 1
(1 2
claims2 8
,8 9
$str: ?
)? @
;@ A
var 
user 
= 
new 
ClaimsPrincipal .
(. /
identity/ 7
)7 8
;8 9
return   
new   
AuthenticationState   .
(  . /
user  / 3
)  3 4
;  4 5
}!! 
catch"" 
{## 
return$$ 
new$$ 
AuthenticationState$$ .
($$. /
new$$/ 2
ClaimsPrincipal$$3 B
($$B C
new$$C F
ClaimsIdentity$$G U
($$U V
)$$V W
)$$W X
)$$X Y
;$$Y Z
}%% 
}&& 	
public(( 
void(( $
NotifyUserAuthentication(( ,
(((, -
string((- 3
token((4 9
)((9 :
{)) 	
var** 
claims** 
=** 
	JwtParser** "
.**" #
ParseClaimsFromJwt**# 5
(**5 6
token**6 ;
)**; <
;**< =
var++ 
authenticatedUser++ !
=++" #
new++$ '
ClaimsPrincipal++( 7
(++7 8
new++8 ;
ClaimsIdentity++< J
(++J K
claims++K Q
,++Q R
$str++S X
)++X Y
)++Y Z
;++Z [
var,, 
	authState,, 
=,, 
Task,,  
.,,  !

FromResult,,! +
(,,+ ,
new,,, /
AuthenticationState,,0 C
(,,C D
authenticatedUser,,D U
),,U V
),,V W
;,,W X,
 NotifyAuthenticationStateChanged-- ,
(--, -
	authState--- 6
)--6 7
;--7 8
}.. 	
public00 
void00 
NotifyUserLogout00 $
(00$ %
)00% &
{11 	
var22 
anonymousUser22 
=22 
new22  #
ClaimsPrincipal22$ 3
(223 4
new224 7
ClaimsIdentity228 F
(22F G
)22G H
)22H I
;22I J
var33 
	authState33 
=33 
Task33  
.33  !

FromResult33! +
(33+ ,
new33, /
AuthenticationState330 C
(33C D
anonymousUser33D Q
)33Q R
)33R S
;33S T,
 NotifyAuthenticationStateChanged44 ,
(44, -
	authState44- 6
)446 7
;447 8
}55 	
}66 
}77 -
JC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Program.cs
var 
builder 
= "
WebAssemblyHostBuilder $
.$ %
CreateDefault% 2
(2 3
args3 7
)7 8
;8 9
builder		 
.		 
RootComponents		 
.		 
Add		 
<		 
App		 
>		 
(		  
$str		  &
)		& '
;		' (
builder

 
.

 
RootComponents

 
.

 
Add

 
<

 

HeadOutlet

 %
>

% &
(

& '
$str

' 4
)

4 5
;

5 6
builder 
. 
Services 
.  
AddAuthorizationCore %
(% &
)& '
;' (
builder 
. 
Services 
. 
	AddScoped 
< '
AuthenticationStateProvider 6
,6 7#
CustomAuthStateProvider8 O
>O P
(P Q
)Q R
;R S
builder 
. 
Services 
. 
AddTransient 
< &
TokenAuthenticationHandler 8
>8 9
(9 :
): ;
;; <
builder 
. 
Services 
. 
	AddScoped 
( 
sp 
=>  
new! $

HttpClient% /
{ 
BaseAddress 
= 
new 
Uri 
( 
$str 3
)3 4
} 
) 
; 
builder 
. 
Services 
. 
AddHttpClient 
( 
$str .
,. /
client0 6
=>7 9
{ 
client 

.
 
BaseAddress 
= 
new 
Uri  
(  !
$str! :
): ;
;; <
} 
) 
. !
AddHttpMessageHandler 
< &
TokenAuthenticationHandler 3
>3 4
(4 5
)5 6
;6 7
builder 
. 
Services 
. 
	AddScoped 
< 
IAuthService '
,' (
AuthService) 4
>4 5
(5 6
)6 7
;7 8
builder 
. 
Services 
. 
	AddScoped 
< 
IAdminService (
>( )
() *
sp* ,
=>- /
{ 
var 
httpClientFactory 
= 
sp 
. 
GetRequiredService 1
<1 2
IHttpClientFactory2 D
>D E
(E F
)F G
;G H
var   
client   
=   
httpClientFactory   "
.  " #
CreateClient  # /
(  / 0
$str  0 ?
)  ? @
;  @ A
return!! 

new!! 
AdminService!! 
(!! 
client!! "
)!!" #
;!!# $
}"" 
)"" 
;"" 
builder$$ 
.$$ 
Services$$ 
.$$ 
	AddScoped$$ 
<$$ 
IDoctorService$$ )
>$$) *
($$* +
sp$$+ -
=>$$. 0
{%% 
var&& 
httpClientFactory&& 
=&& 
sp&& 
.&& 
GetRequiredService&& 1
<&&1 2
IHttpClientFactory&&2 D
>&&D E
(&&E F
)&&F G
;&&G H
var'' 
client'' 
='' 
httpClientFactory'' "
.''" #
CreateClient''# /
(''/ 0
$str''0 ?
)''? @
;''@ A
return(( 

new(( 
DoctorService(( 
((( 
client(( #
)((# $
;(($ %
})) 
))) 
;)) 
builder,, 
.,, 
Services,, 
.,, 
	AddScoped,, 
<,, 
IPatientService,, *
>,,* +
(,,+ ,
sp,,, .
=>,,/ 1
{-- 
var.. 
httpClientFactory.. 
=.. 
sp.. 
... 
GetRequiredService.. 1
<..1 2
IHttpClientFactory..2 D
>..D E
(..E F
)..F G
;..G H
var// 
client// 
=// 
httpClientFactory// "
.//" #
CreateClient//# /
(/// 0
$str//0 ?
)//? @
;//@ A
return00 

new00 
PatientService00 
(00 
client00 $
)00$ %
;00% &
}11 
)11 
;11 
builder44 
.44 
Services44 
.44 
	AddScoped44 
<44 
IAppointmentService44 .
>44. /
(44/ 0
sp440 2
=>443 5
{55 
var66 
httpClientFactory66 
=66 
sp66 
.66 
GetRequiredService66 1
<661 2
IHttpClientFactory662 D
>66D E
(66E F
)66F G
;66G H
var77 
client77 
=77 
httpClientFactory77 "
.77" #
CreateClient77# /
(77/ 0
$str770 ?
)77? @
;77@ A
return88 

new88 
AppointmentService88 !
(88! "
client88" (
)88( )
;88) *
}99 
)99 
;99 
await;; 
builder;; 
.;; 
Build;; 
(;; 
);; 
.;; 
RunAsync;; 
(;; 
);;  
;;;  !Œ
QC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\UserDto.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
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
public 
DateTime 
CreatedDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} ﬂ
WC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\StatisticsDto.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
{ 
public 

class 
StatisticsDto 
{ 
public 
int !
CompletedAppointments (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
int 
PendingAppointments &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public		 
int		 !
ConfirmedAppointments		 (
{		) *
get		+ .
;		. /
set		0 3
;		3 4
}		5 6
public 
int !
CancelledAppointments (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} ˇ
TC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\PatientDto.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
{ 
public 

class 

PatientDto 
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
;; <
public		 
DateOnly		 
DateOfBirth		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		0 1
public 
int 
Gender 
{ 
get 
;  
set! $
;$ %
}& '
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
} §
VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\LoginRequest.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
{ 
public 

class 
LoginRequest 
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
string 
Password 
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
}		 ∂
SC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\DoctorDto.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
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
} ÷
^C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\DoctorCreationResult.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
{ 
public 

class  
DoctorCreationResult %
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
string		 
TemporaryPassword		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		4 5
=		6 7
string		8 >
.		> ?
Empty		? D
;		D E
}

 
} ´
VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\DashboardDto.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
{ 
public 

class 
DashboardDto 
{ 
public 
int 
TotalDoctors 
{  !
get" %
;% &
set' *
;* +
}, -
public 
int 
ActiveDoctors  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
TotalPatients  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
int		 
ActivePatients		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
public 
int 
TodayAppointments $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
int 
PendingAppointments &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
int !
CompletedAppointments (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} ó

]C:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\CreateDoctorRequest.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
{ 
public 

class 
CreateDoctorRequest $
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
} á

VC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\AuthResponse.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
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
} §
XC:\Users\287766\source\repos\S3_HealthAxis\S3_HealthAxis.Blazor\Models\AppointmentDto.cs
	namespace 	
S3_HealthAxis
 
. 
Blazor 
. 
Models %
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
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
public		 
int		 
DoctorId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public 
DateOnly 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 
TimeSlot 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
Status 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} 