¸
oC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IPatientApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 

	interface 
IPatientApiService '
{ 
Task 
< 
List 
< 
PatientResponseDto $
>$ %
>% &
GetPatientsAsync' 7
(7 8
)8 9
;9 :
Task

 
<

 
PatientResponseDto

 
?

  
>

  !
GetPatientByIdAsync

" 5
(

5 6
int

6 9
id

: <
)

< =
;

= >
Task 
< 
List 
< 
PatientResponseDto $
>$ %
>% &
SearchPatientsAsync' :
(: ;
string; A
?A B
nameC G
,G H
stringI O
?O P
emailQ V
)V W
;W X
Task 
< 
PagedResponseDto 
< 
PatientResponseDto 0
>0 1
>1 2!
GetPatientsPagedAsync3 H
(H I
int 

pageNumber 
, 
int 
pageSize 
, 
string 
? 
search 
, 
string 
? 
gender 
) 	
;	 

Task 
< 
bool 
> 
CreatePatientAsync %
(% &
CreatePatientDto& 6
dto7 :
): ;
;; <
Task 
< 
bool 
> 
UpdatePatientAsync %
(% &
int& )
id* ,
,, -
UpdatePatientDto. >
dto? B
)B C
;C D
Task 
< 
bool 
> 
DeletePatientAsync %
(% &
int& )
id* ,
), -
;- .
} 
} å
tC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IHealthRecordApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 

	interface #
IHealthRecordApiService ,
{ 
Task 
< 
List 
< #
HealthRecordResponseDto )
>) *
>* +!
GetHealthRecordsAsync, A
(A B
)B C
;C D
Task		 
<		 
bool		 
>		 #
DeleteHealthRecordAsync		 *
(		* +
int		+ .
id		/ 1
)		1 2
;		2 3
}

 
} Ç
nC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IDoctorApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 

	interface 
IDoctorApiService &
{ 
Task		 
<		 
List		 
<		 
DoctorResponseDto		 #
>		# $
>		$ %
GetDoctorsAsync		& 5
(		5 6
)		6 7
;		7 8
Task 
< 
DoctorResponseDto 
? 
>  
GetDoctorByIdAsync! 3
(3 4
int4 7
id8 :
): ;
;; <
Task 
< 
PagedResponseDto 
< 
DoctorResponseDto /
>/ 0
>0 1 
GetDoctorsPagedAsync2 F
(F G
int 

pageNumber 
, 
int 
pageSize 
, 
string 
? 
search 
, 
string 
? 
specialisation "
," #
string 
? 
status 
) 	
;	 

Task 
< !
CreateDoctorResultDto "
?" #
># $
CreateDoctorAsync% 6
(6 7
CreateDoctorDto7 F
dtoG J
)J K
;K L
Task 
< 
bool 
> 
UpdateDoctorAsync $
($ %
int% (
id) +
,+ ,
CreateDoctorDto- <
dto= @
)@ A
;A B
Task 
< 
bool 
> 
DeleteDoctorAsync $
($ %
int% (
id) +
)+ ,
;, -
Task 
< 
bool 
>  
SetDoctorStatusAsync '
(' (
int( +
doctorId, 4
,4 5
bool6 :
status; A
)A B
;B C
} 
} ë
nC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IDashboardService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 

	interface 
IDashboardService &
{ 
Task 
< 
AdminDashboardDto 
> 
GetDashboardAsync  1
(1 2
)2 3
;3 4
} 
}		 ¬
oC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IAuthStaterService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 

	interface 
IAuthStateService &
{ 
string		 

?		
 
Token		 
{		 
get		 
;		 
}		 
string

 

?


 
Email

 
{

 
get

 
;

 
}

 
string 

?
 
Role 
{ 
get 
; 
} 
int 
? 
ReferenceId	 
{ 
get 
; 
} 
bool 
IsFirstLogin	 
{ 
get 
; 
} 
bool 

IsLoggedIn	 
{ 
get 
; 
} 
bool 
IsAdmin	 
{ 
get 
; 
} 
Task 
SetLoginAsync	 
( 
AuthResponseDto &
response' /
)/ 0
;0 1
Task  
LoadFromStorageAsync	 
( 
) 
;  
Task 
LogoutAsync	 
( 
) 
; 
} 
} ¯

iC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IAuthService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 
	interface 
IAuthService %
{ 	
Task 
< 
AuthResponseDto  
?  !
>! "

LoginAsync# -
(- .
LoginDto. 6
LoginDto7 ?
)? @
;@ A
Task

 
LogoutAsync

 
(

 
)

 
;

 
Task 
< 
string 
? 
> 
GetTokenAsync '
(' (
)( )
;) *
Task 
< 
string 
? 
> 
GetEmailAsync '
(' (
)( )
;) *
Task 
< 
string 
? 
> 
GetRoleAsync &
(& '
)' (
;( )
Task 
< 
bool 
> 
IsLoggedInAsync &
(& '
)' (
;( )
Task 
< 
bool 
> 
IsAdminAsync #
(# $
)$ %
;% &
} 	
} ‰
sC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Interfaces\IAppointmentApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )

Interfaces) 3
{ 
public 

	interface "
IAppointmentApiService +
{ 
Task		 
<		 
List		 
<		 "
AppointmentResponseDto		 (
>		( )
>		) * 
GetAppointmentsAsync		+ ?
(		? @
)		@ A
;		A B
Task 
< 
List 
< "
AppointmentResponseDto (
>( )
>) *(
GetAppointmentsByDoctorAsync+ G
(G H
intH K
doctorIdL T
)T U
;U V
Task 
< 
PagedResponseDto 
< "
AppointmentResponseDto 4
>4 5
>5 6%
GetAppointmentsPagedAsync7 P
(P Q
int 

pageNumber 
, 
int 
pageSize 
, 
string 
? 
search 
, 
AppointmentStatus 
? 
status %
,% &
DateTime 
? 
	startDate 
,  
DateTime 
? 
endDate 
) 	
;	 

Task 
< 
List 
< "
AppointmentResponseDto (
>( )
>) *#
FilterAppointmentsAsync+ B
(B C
AppointmentStatus 
? 
status %
,% &
DateTime 
? 
	startDate 
,  
DateTime 
? 
endDate 
) 	
;	 

Task 
< 
bool 
> #
ConfirmAppointmentAsync *
(* +
int+ .
appointmentId/ <
)< =
;= >
Task 
< 
bool 
> "
CancelAppointmentAsync )
() *
int* -
appointmentId. ;
,; <
string= C
reasonD J
)J K
;K L
Task   
<   
bool   
>   "
DeleteAppointmentAsync   )
(  ) *
int  * -
appointmentId  . ;
)  ; <
;  < =
}!! 
}"" ˜G
rC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\PatientApiService.cs
	namespace

 	!
HealthAxisAdminLayout


 
.

  
Services

  (
.

( )
Implementations

) 8
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

HttpClient #
_http$ )
;) *
public 
PatientApiService  
(  !

HttpClient! +
http, 0
)0 1
{ 	
_http 
= 
http 
; 
} 	
public 
async 
Task 
< 
List 
< 
PatientResponseDto 1
>1 2
>2 3
GetPatientsAsync4 D
(D E
)E F
{ 	
var 
result 
= 
await 
_http $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;
PatientResponseDto; M
>M N
>N O
(O P
$str 
) 
; 
return 
result 
?? 
new  
List! %
<% &
PatientResponseDto& 8
>8 9
(9 :
): ;
;; <
} 	
public 
async 
Task 
< 
PatientResponseDto ,
?, -
>- .
GetPatientByIdAsync/ B
(B C
intC F
idG I
)I J
{ 	
return   
await   
_http   
.   
GetFromJsonAsync   /
<  / 0
PatientResponseDto  0 B
>  B C
(  C D
$"!! 
$str!! 
{!! 
id!! !
}!!! "
"!!" #
)"" 
;"" 
}## 	
public%% 
async%% 
Task%% 
<%% 
List%% 
<%% 
PatientResponseDto%% 1
>%%1 2
>%%2 3
SearchPatientsAsync%%4 G
(%%G H
string%%H N
?%%N O
name%%P T
,%%T U
string%%V \
?%%\ ]
email%%^ c
)%%c d
{&& 	
var'' 
query'' 
='' 
new'' 
List''  
<''  !
string''! '
>''' (
(''( )
)'') *
;''* +
if)) 
()) 
!)) 
string)) 
.)) 
IsNullOrWhiteSpace)) *
())* +
name))+ /
)))/ 0
)))0 1
{** 
query++ 
.++ 
Add++ 
(++ 
$"++ 
$str++ !
{++! "
Uri++" %
.++% &
EscapeDataString++& 6
(++6 7
name++7 ;
)++; <
}++< =
"++= >
)++> ?
;++? @
},, 
if.. 
(.. 
!.. 
string.. 
... 
IsNullOrWhiteSpace.. *
(..* +
email..+ 0
)..0 1
)..1 2
{// 
query00 
.00 
Add00 
(00 
$"00 
$str00 "
{00" #
Uri00# &
.00& '
EscapeDataString00' 7
(007 8
email008 =
)00= >
}00> ?
"00? @
)00@ A
;00A B
}11 
var33 
url33 
=33 
query33 
.33 
Count33 !
==33" $
$num33% &
?44 
$str44 &
:55 
$"55 
$str55 '
{55' (
string55( .
.55. /
Join55/ 3
(553 4
$str554 7
,557 8
query559 >
)55> ?
}55? @
"55@ A
;55A B
var77 
result77 
=77 
await77 
_http77 $
.77$ %
GetFromJsonAsync77% 5
<775 6
List776 :
<77: ;
PatientResponseDto77; M
>77M N
>77N O
(77O P
url77P S
)77S T
;77T U
return99 
result99 
??99 
new99  
List99! %
<99% &
PatientResponseDto99& 8
>998 9
(999 :
)99: ;
;99; <
}:: 	
public<< 
async<< 
Task<< 
<<< 
PagedResponseDto<< *
<<<* +
PatientResponseDto<<+ =
><<= >
><<> ?!
GetPatientsPagedAsync<<@ U
(<<U V
int== 

pageNumber== 
,== 
int>> 
pageSize>> 
,>> 
string?? 
??? 
search?? 
,?? 
string@@ 
?@@ 
gender@@ 
)@@ 
{AA 	
varBB 
queryBB 
=BB 
newBB 
ListBB  
<BB  !
stringBB! '
>BB' (
{CC 
$"DD 
$strDD 
{DD 

pageNumberDD (
}DD( )
"DD) *
,DD* +
$"EE 
$strEE 
{EE 
pageSizeEE $
}EE$ %
"EE% &
}FF 
;FF 
ifHH 
(HH 
!HH 
stringHH 
.HH 
IsNullOrWhiteSpaceHH *
(HH* +
searchHH+ 1
)HH1 2
)HH2 3
{II 
queryJJ 
.JJ 
AddJJ 
(JJ 
$"JJ 
$strJJ #
{JJ# $
UriJJ$ '
.JJ' (
EscapeDataStringJJ( 8
(JJ8 9
searchJJ9 ?
)JJ? @
}JJ@ A
"JJA B
)JJB C
;JJC D
}KK 
ifMM 
(MM 
!MM 
stringMM 
.MM 
IsNullOrWhiteSpaceMM *
(MM* +
genderMM+ 1
)MM1 2
&&MM3 5
genderMM6 <
!=MM= ?
$strMM@ E
)MME F
{NN 
queryOO 
.OO 
AddOO 
(OO 
$"OO 
$strOO #
{OO# $
UriOO$ '
.OO' (
EscapeDataStringOO( 8
(OO8 9
genderOO9 ?
)OO? @
}OO@ A
"OOA B
)OOB C
;OOC D
}PP 
varRR 
urlRR 
=RR 
$"RR 
$strRR *
{RR* +
stringRR+ 1
.RR1 2
JoinRR2 6
(RR6 7
$strRR7 :
,RR: ;
queryRR< A
)RRA B
}RRB C
"RRC D
;RRD E
varTT 
resultTT 
=TT 
awaitTT 
_httpTT $
.TT$ %
GetFromJsonAsyncTT% 5
<TT5 6
PagedResponseDtoTT6 F
<TTF G
PatientResponseDtoTTG Y
>TTY Z
>TTZ [
(TT[ \
urlTT\ _
)TT_ `
;TT` a
returnVV 
resultVV 
??VV 
newVV  
PagedResponseDtoVV! 1
<VV1 2
PatientResponseDtoVV2 D
>VVD E
(VVE F
)VVF G
;VVG H
}WW 	
publicYY 
asyncYY 
TaskYY 
<YY 
boolYY 
>YY 
CreatePatientAsyncYY  2
(YY2 3
CreatePatientDtoYY3 C
dtoYYD G
)YYG H
{ZZ 	
var[[ 
response[[ 
=[[ 
await[[  
_http[[! &
.[[& '
PostAsJsonAsync[[' 6
([[6 7
$str\\ 
,\\ 
dto]] 
)^^ 
;^^ 
return`` 
response`` 
.`` 
IsSuccessStatusCode`` /
;``/ 0
}aa 	
publiccc 
asynccc 
Taskcc 
<cc 
boolcc 
>cc 
UpdatePatientAsynccc  2
(cc2 3
intcc3 6
idcc7 9
,cc9 :
UpdatePatientDtocc; K
dtoccL O
)ccO P
{dd 	
varee 
responseee 
=ee 
awaitee  
_httpee! &
.ee& '
PutAsJsonAsyncee' 5
(ee5 6
$"ff 
$strff 
{ff 
idff !
}ff! "
"ff" #
,ff# $
dtogg 
)hh 
;hh 
returnjj 
responsejj 
.jj 
IsSuccessStatusCodejj /
;jj/ 0
}kk 	
publicmm 
asyncmm 
Taskmm 
<mm 
boolmm 
>mm 
DeletePatientAsyncmm  2
(mm2 3
intmm3 6
idmm7 9
)mm9 :
{nn 	
varoo 
responseoo 
=oo 
awaitoo  
_httpoo! &
.oo& '
DeleteAsyncoo' 2
(oo2 3
$"pp 
$strpp 
{pp 
idpp !
}pp! "
"pp" #
)qq 
;qq 
returnss 
responsess 
.ss 
IsSuccessStatusCodess /
;ss/ 0
}tt 	
}uu 
}vv Ú
wC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\HealthRecordApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )
Implementations) 8
{ 
public 

class "
HealthRecordApiService '
:( )#
IHealthRecordApiService* A
{ 
private		 
readonly		 

HttpClient		 #
_http		$ )
;		) *
public "
HealthRecordApiService %
(% &

HttpClient& 0
http1 5
)5 6
{ 	
_http 
= 
http 
; 
} 	
public 
async 
Task 
< 
List 
< #
HealthRecordResponseDto 6
>6 7
>7 8!
GetHealthRecordsAsync9 N
(N O
)O P
{ 	
var 
result 
= 
await 
_http $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;#
HealthRecordResponseDto; R
>R S
>S T
(T U
$str "
) 
; 
return 
result 
?? 
new  
List! %
<% &#
HealthRecordResponseDto& =
>= >
(> ?
)? @
;@ A
} 	
public 
async 
Task 
< 
bool 
> #
DeleteHealthRecordAsync  7
(7 8
int8 ;
id< >
)> ?
{ 	
var 
response 
= 
await  
_http! &
.& '
DeleteAsync' 2
(2 3
$" 
$str #
{# $
id$ &
}& '
"' (
) 
; 
return 
response 
. 
IsSuccessStatusCode /
;/ 0
}   	
}!! 
}"" ƒC
qC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\DoctorApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )
Implementations) 8
{ 
public		 

class		 
DoctorApiService		 !
:		" #
IDoctorApiService		$ 5
{

 
private 
readonly 

HttpClient #
_http$ )
;) *
public 
DoctorApiService 
(  

HttpClient  *
http+ /
)/ 0
{ 	
_http 
= 
http 
; 
} 	
public 
async 
Task 
< 
List 
< 
DoctorResponseDto 0
>0 1
>1 2
GetDoctorsAsync3 B
(B C
)C D
{ 	
var 
result 
= 
await 
_http $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;
DoctorResponseDto; L
>L M
>M N
(N O
$strO [
)[ \
;\ ]
return 
result 
?? 
new  
List! %
<% &
DoctorResponseDto& 7
>7 8
(8 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
PagedResponseDto *
<* +
DoctorResponseDto+ <
>< =
>= > 
GetDoctorsPagedAsync? S
(S T
int 

pageNumber 
, 
int 
pageSize 
, 
string 
? 
search 
, 
string 
? 
specialisation "
," #
string 
? 
status 
) 
{ 	
var   
query   
=   
new   
List    
<    !
string  ! '
>  ' (
{!! 
$""" 
$str"" 
{"" 

pageNumber"" (
}""( )
""") *
,""* +
$"## 
$str## 
{## 
pageSize## $
}##$ %
"##% &
}$$ 
;$$ 
if&& 
(&& 
!&& 
string&& 
.&& 
IsNullOrWhiteSpace&& *
(&&* +
search&&+ 1
)&&1 2
)&&2 3
{'' 
query(( 
.(( 
Add(( 
((( 
$"(( 
$str(( #
{((# $
Uri(($ '
.((' (
EscapeDataString((( 8
(((8 9
search((9 ?
)((? @
}((@ A
"((A B
)((B C
;((C D
})) 
if++ 
(++ 
!++ 
string++ 
.++ 
IsNullOrWhiteSpace++ *
(++* +
specialisation+++ 9
)++9 :
&&++; =
specialisation++> L
!=++M O
$str++P U
)++U V
{,, 
query-- 
.-- 
Add-- 
(-- 
$"-- 
$str-- +
{--+ ,
Uri--, /
.--/ 0
EscapeDataString--0 @
(--@ A
specialisation--A O
)--O P
}--P Q
"--Q R
)--R S
;--S T
}.. 
if00 
(00 
!00 
string00 
.00 
IsNullOrWhiteSpace00 *
(00* +
status00+ 1
)001 2
&&003 5
status006 <
!=00= ?
$str00@ E
)00E F
{11 
query22 
.22 
Add22 
(22 
$"22 
$str22 #
{22# $
Uri22$ '
.22' (
EscapeDataString22( 8
(228 9
status229 ?
)22? @
}22@ A
"22A B
)22B C
;22C D
}33 
var55 
url55 
=55 
$"55 
$str55 )
{55) *
string55* 0
.550 1
Join551 5
(555 6
$str556 9
,559 :
query55; @
)55@ A
}55A B
"55B C
;55C D
var77 
result77 
=77 
await77 
_http77 $
.77$ %
GetFromJsonAsync77% 5
<775 6
PagedResponseDto776 F
<77F G
DoctorResponseDto77G X
>77X Y
>77Y Z
(77Z [
url77[ ^
)77^ _
;77_ `
return99 
result99 
??99 
new99  
PagedResponseDto99! 1
<991 2
DoctorResponseDto992 C
>99C D
(99D E
)99E F
;99F G
}:: 	
public<< 
async<< 
Task<< 
<<< 
DoctorResponseDto<< +
?<<+ ,
><<, -
GetDoctorByIdAsync<<. @
(<<@ A
int<<A D
id<<E G
)<<G H
{== 	
return>> 
await>> 
_http>> 
.>> 
GetFromJsonAsync>> /
<>>/ 0
DoctorResponseDto>>0 A
>>>A B
(>>B C
$">>C E
$str>>E P
{>>P Q
id>>Q S
}>>S T
">>T U
)>>U V
;>>V W
}?? 	
publicAA 
asyncAA 
TaskAA 
<AA !
CreateDoctorResultDtoAA /
?AA/ 0
>AA0 1
CreateDoctorAsyncAA2 C
(AAC D
CreateDoctorDtoAAD S
dtoAAT W
)AAW X
{BB 	
varCC 
responseCC 
=CC 
awaitCC  
_httpCC! &
.CC& '
PostAsJsonAsyncCC' 6
(CC6 7
$strCC7 C
,CCC D
dtoCCE H
)CCH I
;CCI J
ifEE 
(EE 
!EE 
responseEE 
.EE 
IsSuccessStatusCodeEE -
)EE- .
{FF 
returnGG 
nullGG 
;GG 
}HH 
varJJ 
resultJJ 
=JJ 
awaitJJ 
responseJJ '
.JJ' (
ContentJJ( /
.JJ/ 0
ReadFromJsonAsyncJJ0 A
<JJA B!
CreateDoctorResultDtoJJB W
>JJW X
(JJX Y
)JJY Z
;JJZ [
returnLL 
resultLL 
;LL 
}MM 	
publicOO 
asyncOO 
TaskOO 
<OO 
boolOO 
>OO 
UpdateDoctorAsyncOO  1
(OO1 2
intOO2 5
idOO6 8
,OO8 9
CreateDoctorDtoOO: I
dtoOOJ M
)OOM N
{PP 	
varQQ 
responseQQ 
=QQ 
awaitQQ  
_httpQQ! &
.QQ& '
PutAsJsonAsyncQQ' 5
(QQ5 6
$"QQ6 8
$strQQ8 C
{QQC D
idQQD F
}QQF G
"QQG H
,QQH I
dtoQQJ M
)QQM N
;QQN O
returnSS 
responseSS 
.SS 
IsSuccessStatusCodeSS /
;SS/ 0
}TT 	
publicVV 
asyncVV 
TaskVV 
<VV 
boolVV 
>VV 
DeleteDoctorAsyncVV  1
(VV1 2
intVV2 5
idVV6 8
)VV8 9
{WW 	
varXX 
responseXX 
=XX 
awaitXX  
_httpXX! &
.XX& '
DeleteAsyncXX' 2
(XX2 3
$"XX3 5
$strXX5 @
{XX@ A
idXXA C
}XXC D
"XXD E
)XXE F
;XXF G
returnZZ 
responseZZ 
.ZZ 
IsSuccessStatusCodeZZ /
;ZZ/ 0
}[[ 	
public]] 
async]] 
Task]] 
<]] 
bool]] 
>]]  
SetDoctorStatusAsync]]  4
(]]4 5
int]]5 8
doctorId]]9 A
,]]A B
bool]]C G
status]]H N
)]]N O
{^^ 	
var__ 
response__ 
=__ 
await__  
_http__! &
.__& '

PatchAsync__' 1
(__1 2
$"`` 
$str`` $
{``$ %
doctorId``% -
}``- .
$str``. 6
{``6 7
status``7 =
}``= >
"``> ?
,``? @
nullaa 
)bb 
;bb 
returndd 
responsedd 
.dd 
IsSuccessStatusCodedd /
;dd/ 0
}ee 	
}ff 
}gg Õ"
qC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\DashboardService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )
Implementations) 8
{ 
public 

class 
DashboardService !
:" #
IDashboardService$ 5
{ 
private		 
readonly		 
IDoctorApiService		 *
_doctorService		+ 9
;		9 :
private

 
readonly

 
IPatientApiService

 +
_patientService

, ;
;

; <
private 
readonly "
IAppointmentApiService /
_appointmentService0 C
;C D
private 
readonly #
IHealthRecordApiService 0 
_healthRecordService1 E
;E F
public 
DashboardService 
(  
IDoctorApiService 
doctorService +
,+ ,
IPatientApiService 
patientService -
,- ."
IAppointmentApiService "
appointmentService# 5
,5 6#
IHealthRecordApiService #
healthRecordService$ 7
)7 8
{ 	
_doctorService 
= 
doctorService *
;* +
_patientService 
= 
patientService ,
;, -
_appointmentService 
=  !
appointmentService" 4
;4 5 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
public 
async 
Task 
< 
AdminDashboardDto +
>+ ,
GetDashboardAsync- >
(> ?
)? @
{ 	
var 
doctors 
= 
await 
_doctorService  .
.. /
GetDoctorsAsync/ >
(> ?
)? @
;@ A
var 
patients 
= 
await  
_patientService! 0
.0 1
GetPatientsAsync1 A
(A B
)B C
;C D
var 
appointments 
= 
await $
_appointmentService% 8
.8 9 
GetAppointmentsAsync9 M
(M N
)N O
;O P
var 
records 
= 
await  
_healthRecordService  4
.4 5!
GetHealthRecordsAsync5 J
(J K
)K L
;L M
return!! 
new!! 
AdminDashboardDto!! (
{"" 
TotalDoctors## 
=## 
doctors## &
.##& '
Count##' ,
,##, -
ActiveDoctors$$ 
=$$ 
doctors$$  '
.$$' (
Count$$( -
($$- .
d$$. /
=>$$0 2
d$$3 4
.$$4 5
IsActive$$5 =
)$$= >
,$$> ?
TotalPatients%% 
=%% 
patients%%  (
.%%( )
Count%%) .
,%%. /
TotalAppointments&& !
=&&" #
appointments&&$ 0
.&&0 1
Count&&1 6
,&&6 7
PendingAppointments(( #
=(($ %
appointments((& 2
.((2 3
Count((3 8
(((8 9
a((9 :
=>((; =
a((> ?
.((? @
Status((@ F
==((G I
AppointmentStatus((J [
.(([ \
Pending((\ c
)((c d
,((d e!
ConfirmedAppointments)) %
=))& '
appointments))( 4
.))4 5
Count))5 :
()): ;
a)); <
=>))= ?
a))@ A
.))A B
Status))B H
==))I K
AppointmentStatus))L ]
.))] ^
	Confirmed))^ g
)))g h
,))h i!
CancelledAppointments** %
=**& '
appointments**( 4
.**4 5
Count**5 :
(**: ;
a**; <
=>**= ?
a**@ A
.**A B
Status**B H
==**I K
AppointmentStatus**L ]
.**] ^
	Cancelled**^ g
)**g h
,**h i
TotalHealthRecords,, "
=,,# $
records,,% ,
.,,, -
Count,,- 2
}-- 
;-- 
}.. 	
}// 
}00 ¿å
qC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\AuthStateService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )
Implementations) 8
{		 
public

 

sealed

 
class

 
AuthStateService

 (
:

) *
IAuthStateService

+ <
{ 
private 
const 
string 
SetItemFunction ,
=- .
$str &
;& '
private 
const 
string 
GetItemFunction ,
=- .
$str &
;& '
private 
const 
string 
RemoveItemFunction /
=0 1
$str )
;) *
private 
const 
string 
TokenKey %
=& '
$str 
; 
private 
const 
string 
EmailKey %
=& '
$str 
; 
private 
const 
string 
RoleKey $
=% &
$str 
; 
private 
const 
string 
ReferenceIdKey +
=, -
$str 
; 
private!! 
const!! 
string!! 
IsFirstLoginKey!! ,
=!!- .
$str"" 
;"" 
private$$ 
readonly$$ 

IJSRuntime$$ #
_js$$$ '
;$$' (
private&& 
readonly&& 
ILogger&&  
<&&  !
AuthStateService&&! 1
>&&1 2
_logger&&3 :
;&&: ;
private(( 
bool(( 
	_isLoaded(( 
;(( 
public** 
AuthStateService** 
(**  

IJSRuntime++ 
js++ 
,++ 
ILogger,, 
<,, 
AuthStateService,, $
>,,$ %
logger,,& ,
),,, -
{-- 	
_js.. 
=.. 
js.. 
;.. 
_logger// 
=// 
logger// 
;// 
}00 	
public22 
string22 
?22 
Token22 
{22 
get22 "
;22" #
private22$ +
set22, /
;22/ 0
}221 2
public44 
string44 
?44 
Email44 
{44 
get44 "
;44" #
private44$ +
set44, /
;44/ 0
}441 2
public66 
string66 
?66 
Role66 
{66 
get66 !
;66! "
private66# *
set66+ .
;66. /
}660 1
public88 
int88 
?88 
ReferenceId88 
{88  !
get88" %
;88% &
private88' .
set88/ 2
;882 3
}884 5
public:: 
bool:: 
IsFirstLogin::  
{::! "
get::# &
;::& '
private::( /
set::0 3
;::3 4
}::5 6
public<< 
bool<< 

IsLoggedIn<< 
=><< !
!== 
string== 
.== 
IsNullOrWhiteSpace== &
(==& '
Token==' ,
)==, -
;==- .
public?? 
bool?? 
IsAdmin?? 
=>?? 
Role@@ 
?@@ 
.@@ 
Equals@@ 
(@@ 
$strAA 
,AA 
StringComparisonBB  
.BB  !
OrdinalIgnoreCaseBB! 2
)BB2 3
==BB4 6
trueBB7 ;
;BB; <
publicDD 
asyncDD 
TaskDD 
SetLoginAsyncDD '
(DD' (
AuthResponseDtoEE 
responseEE $
)EE$ %
{FF 	!
ArgumentNullExceptionGG !
.GG! "
ThrowIfNullGG" -
(GG- .
responseGG. 6
)GG6 7
;GG7 8"
SetAuthenticationStateII "
(II" #
responseII# +
)II+ ,
;II, -
tryKK 
{LL 
awaitMM (
SaveAuthenticationStateAsyncMM 2
(MM2 3
)MM3 4
;MM4 5
	_isLoadedNN 
=NN 
trueNN  
;NN  !
}OO 
catchPP 
(PP 
JSExceptionPP 
	exceptionPP (
)PP( )
{QQ "
LogStorageWriteFailureRR &
(RR& '
	exceptionRR' 0
)RR0 1
;RR1 2
}SS 
catchTT 
(TT %
InvalidOperationExceptionTT ,
	exceptionTT- 6
)TT6 7
{UU !
LogInteropUnavailableVV %
(VV% &
	exceptionVV& /
)VV/ 0
;VV0 1
}WW 
}XX 	
publicZZ 
asyncZZ 
TaskZZ  
LoadFromStorageAsyncZZ .
(ZZ. /
)ZZ/ 0
{[[ 	
if\\ 
(\\ 
	_isLoaded\\ 
)\\ 
{]] 
return^^ 
;^^ 
}__ 
tryaa 
{bb 
awaitcc (
LoadAuthenticationStateAsynccc 2
(cc2 3
)cc3 4
;cc4 5
	_isLoadedee 
=ee 
trueee  
;ee  !
}ff 
catchgg 
(gg 
JSExceptiongg 
	exceptiongg (
)gg( )
{hh !
LogStorageReadFailureii %
(ii% &
	exceptionii& /
)ii/ 0
;ii0 1
}jj 
catchkk 
(kk %
InvalidOperationExceptionkk ,
	exceptionkk- 6
)kk6 7
{ll !
LogInteropUnavailablemm %
(mm% &
	exceptionmm& /
)mm/ 0
;mm0 1
}nn 
}oo 	
publicqq 
asyncqq 
Taskqq 
LogoutAsyncqq %
(qq% &
)qq& '
{rr 	$
ClearAuthenticationStatess $
(ss$ %
)ss% &
;ss& '
tryuu 
{vv 
awaitww *
RemoveAuthenticationStateAsyncww 4
(ww4 5
)ww5 6
;ww6 7
	_isLoadedyy 
=yy 
trueyy  
;yy  !
}zz 
catch{{ 
({{ 
JSException{{ 
	exception{{ (
){{( )
{|| $
LogStorageRemovalFailure}} (
(}}( )
	exception}}) 2
)}}2 3
;}}3 4
}~~ 
catch 
( %
InvalidOperationException ,
	exception- 6
)6 7
{
ÄÄ #
LogInteropUnavailable
ÅÅ %
(
ÅÅ% &
	exception
ÅÅ& /
)
ÅÅ/ 0
;
ÅÅ0 1
}
ÇÇ 
}
ÉÉ 	
private
ÖÖ 
void
ÖÖ $
SetAuthenticationState
ÖÖ +
(
ÖÖ+ ,
AuthResponseDto
ÜÜ 
response
ÜÜ $
)
ÜÜ$ %
{
áá 	
Token
àà 
=
àà 
response
àà 
.
àà 
Token
àà "
;
àà" #
Email
ââ 
=
ââ 
response
ââ 
.
ââ 
Email
ââ "
;
ââ" #
Role
ää 
=
ää 
response
ää 
.
ää 
Role
ää  
;
ää  !
ReferenceId
ãã 
=
ãã 
response
ãã "
.
ãã" #
ReferenceId
ãã# .
;
ãã. /
IsFirstLogin
åå 
=
åå 
response
åå #
.
åå# $
IsFirstLogin
åå$ 0
;
åå0 1
}
çç 	
private
èè 
async
èè 
Task
èè *
SaveAuthenticationStateAsync
èè 7
(
èè7 8
)
èè8 9
{
êê 	
await
ëë !
SetStorageItemAsync
ëë %
(
ëë% &
TokenKey
íí 
,
íí 
Token
ìì 
)
ìì 
;
ìì 
await
ïï !
SetStorageItemAsync
ïï %
(
ïï% &
EmailKey
ññ 
,
ññ 
Email
óó 
)
óó 
;
óó 
await
ôô !
SetStorageItemAsync
ôô %
(
ôô% &
RoleKey
öö 
,
öö 
Role
õõ 
)
õõ 
;
õõ 
await
ùù !
SetStorageItemAsync
ùù %
(
ùù% &
ReferenceIdKey
ûû 
,
ûû 
ReferenceId
üü 
?
üü 
.
üü 
ToString
üü %
(
üü% &
)
üü& '
)
üü' (
;
üü( )
await
°° !
SetStorageItemAsync
°° %
(
°°% &
IsFirstLoginKey
¢¢ 
,
¢¢  
IsFirstLogin
££ 
.
££ 
ToString
££ %
(
££% &
)
££& '
)
££' (
;
££( )
}
§§ 	
private
¶¶ 
async
¶¶ 
Task
¶¶ *
LoadAuthenticationStateAsync
¶¶ 7
(
¶¶7 8
)
¶¶8 9
{
ßß 	
Token
®® 
=
®® #
NormalizeStorageValue
®® )
(
®®) *
await
©© !
GetStorageItemAsync
©© )
(
©©) *
TokenKey
©©* 2
)
©©2 3
)
©©3 4
;
©©4 5
Email
´´ 
=
´´ #
NormalizeStorageValue
´´ )
(
´´) *
await
¨¨ !
GetStorageItemAsync
¨¨ )
(
¨¨) *
EmailKey
¨¨* 2
)
¨¨2 3
)
¨¨3 4
;
¨¨4 5
Role
ÆÆ 
=
ÆÆ #
NormalizeStorageValue
ÆÆ (
(
ÆÆ( )
await
ØØ !
GetStorageItemAsync
ØØ )
(
ØØ) *
RoleKey
ØØ* 1
)
ØØ1 2
)
ØØ2 3
;
ØØ3 4
var
±± 
referenceIdValue
±±  
=
±±! "
await
≤≤ !
GetStorageItemAsync
≤≤ )
(
≤≤) *
ReferenceIdKey
≥≥ "
)
≥≥" #
;
≥≥# $
ReferenceId
µµ 
=
µµ 
ParseReferenceId
∂∂  
(
∂∂  !
referenceIdValue
∂∂! 1
)
∂∂1 2
;
∂∂2 3
var
∏∏ 
firstLoginValue
∏∏ 
=
∏∏  !
await
ππ !
GetStorageItemAsync
ππ )
(
ππ) *
IsFirstLoginKey
∫∫ #
)
∫∫# $
;
∫∫$ %
IsFirstLogin
ºº 
=
ºº 
ParseFirstLogin
ΩΩ 
(
ΩΩ  
firstLoginValue
ΩΩ  /
)
ΩΩ/ 0
;
ΩΩ0 1
}
ææ 	
private
¿¿ 
async
¿¿ 
Task
¿¿ ,
RemoveAuthenticationStateAsync
¿¿ 9
(
¿¿9 :
)
¿¿: ;
{
¡¡ 	
await
¬¬ $
RemoveStorageItemAsync
¬¬ (
(
¬¬( )
TokenKey
¬¬) 1
)
¬¬1 2
;
¬¬2 3
await
√√ $
RemoveStorageItemAsync
√√ (
(
√√( )
EmailKey
√√) 1
)
√√1 2
;
√√2 3
await
ƒƒ $
RemoveStorageItemAsync
ƒƒ (
(
ƒƒ( )
RoleKey
ƒƒ) 0
)
ƒƒ0 1
;
ƒƒ1 2
await
≈≈ $
RemoveStorageItemAsync
≈≈ (
(
≈≈( )
ReferenceIdKey
≈≈) 7
)
≈≈7 8
;
≈≈8 9
await
∆∆ $
RemoveStorageItemAsync
∆∆ (
(
∆∆( )
IsFirstLoginKey
∆∆) 8
)
∆∆8 9
;
∆∆9 :
}
«« 	
private
…… 
async
…… 
Task
…… !
SetStorageItemAsync
…… .
(
……. /
string
   
key
   
,
   
string
ÀÀ 
?
ÀÀ 
value
ÀÀ 
)
ÀÀ 
{
ÃÃ 	
await
ÕÕ 
_js
ÕÕ 
.
ÕÕ 
InvokeVoidAsync
ÕÕ %
(
ÕÕ% &
SetItemFunction
ŒŒ 
,
ŒŒ  
key
œœ 
,
œœ 
value
–– 
??
–– 
string
–– 
.
––  
Empty
––  %
)
––% &
;
––& '
}
—— 	
private
”” 
async
”” 
Task
”” 
<
”” 
string
”” !
?
””! "
>
””" #!
GetStorageItemAsync
‘‘ 
(
‘‘  
string
‘‘  &
key
‘‘' *
)
‘‘* +
{
’’ 	
return
÷÷ 
await
÷÷ 
_js
÷÷ 
.
÷÷ 
InvokeAsync
÷÷ (
<
÷÷( )
string
÷÷) /
?
÷÷/ 0
>
÷÷0 1
(
÷÷1 2
GetItemFunction
◊◊ 
,
◊◊  
key
ÿÿ 
)
ÿÿ 
;
ÿÿ 
}
ŸŸ 	
private
€€ 
async
€€ 
Task
€€ $
RemoveStorageItemAsync
€€ 1
(
€€1 2
string
‹‹ 
key
‹‹ 
)
‹‹ 
{
›› 	
await
ﬁﬁ 
_js
ﬁﬁ 
.
ﬁﬁ 
InvokeVoidAsync
ﬁﬁ %
(
ﬁﬁ% & 
RemoveItemFunction
ﬂﬂ "
,
ﬂﬂ" #
key
‡‡ 
)
‡‡ 
;
‡‡ 
}
·· 	
private
„„ 
void
„„ &
ClearAuthenticationState
„„ -
(
„„- .
)
„„. /
{
‰‰ 	
Token
ÂÂ 
=
ÂÂ 
null
ÂÂ 
;
ÂÂ 
Email
ÊÊ 
=
ÊÊ 
null
ÊÊ 
;
ÊÊ 
Role
ÁÁ 
=
ÁÁ 
null
ÁÁ 
;
ÁÁ 
ReferenceId
ËË 
=
ËË 
null
ËË 
;
ËË 
IsFirstLogin
ÈÈ 
=
ÈÈ 
false
ÈÈ  
;
ÈÈ  !
}
ÍÍ 	
private
ÏÏ 
static
ÏÏ 
string
ÏÏ 
?
ÏÏ #
NormalizeStorageValue
ÏÏ 4
(
ÏÏ4 5
string
ÌÌ 
?
ÌÌ 
value
ÌÌ 
)
ÌÌ 
{
ÓÓ 	
return
ÔÔ 
string
ÔÔ 
.
ÔÔ  
IsNullOrWhiteSpace
ÔÔ ,
(
ÔÔ, -
value
ÔÔ- 2
)
ÔÔ2 3
?
 
null
 
:
ÒÒ 
value
ÒÒ 
;
ÒÒ 
}
ÚÚ 	
private
ÙÙ 
static
ÙÙ 
int
ÙÙ 
?
ÙÙ 
ParseReferenceId
ÙÙ ,
(
ÙÙ, -
string
ıı 
?
ıı 
referenceIdValue
ıı $
)
ıı$ %
{
ˆˆ 	
return
˜˜ 
int
˜˜ 
.
˜˜ 
TryParse
˜˜ 
(
˜˜  
referenceIdValue
¯¯  
,
¯¯  !
out
˘˘ 
var
˘˘ 
parsedReferenceId
˘˘ )
)
˘˘) *
?
˙˙ 
parsedReferenceId
˙˙ '
:
˚˚ 
null
˚˚ 
;
˚˚ 
}
¸¸ 	
private
˛˛ 
static
˛˛ 
bool
˛˛ 
ParseFirstLogin
˛˛ +
(
˛˛+ ,
string
ˇˇ 
?
ˇˇ 
firstLoginValue
ˇˇ #
)
ˇˇ# $
{
ÄÄ 	
return
ÅÅ 
bool
ÅÅ 
.
ÅÅ 
TryParse
ÅÅ  
(
ÅÅ  !
firstLoginValue
ÇÇ 
,
ÇÇ  
out
ÉÉ 
var
ÉÉ 
parsedFirstLogin
ÉÉ (
)
ÉÉ( )
&&
ÉÉ* ,
parsedFirstLogin
ÑÑ  
;
ÑÑ  !
}
ÖÖ 	
private
áá 
void
áá $
LogStorageWriteFailure
áá +
(
áá+ ,
JSException
àà 
	exception
àà !
)
àà! "
{
ââ 	
if
ää 
(
ää 
!
ää 
_logger
ää 
.
ää 
	IsEnabled
ää "
(
ää" #
LogLevel
ää# +
.
ää+ ,
Warning
ää, 3
)
ää3 4
)
ää4 5
{
ãã 
return
åå 
;
åå 
}
çç 
_logger
èè 
.
èè 

LogWarning
èè 
(
èè 
	exception
êê 
,
êê 
$str
ëë 6
+
ëë7 8
$str
íí +
)
íí+ ,
;
íí, -
}
ìì 	
private
ïï 
void
ïï #
LogStorageReadFailure
ïï *
(
ïï* +
JSException
ññ 
	exception
ññ !
)
ññ! "
{
óó 	
if
òò 
(
òò 
!
òò 
_logger
òò 
.
òò 
	IsEnabled
òò "
(
òò" #
LogLevel
òò# +
.
òò+ ,
Warning
òò, 3
)
òò3 4
)
òò4 5
{
ôô 
return
öö 
;
öö 
}
õõ 
_logger
ùù 
.
ùù 

LogWarning
ùù 
(
ùù 
	exception
ûû 
,
ûû 
$str
üü 6
+
üü7 8
$str
†† -
)
††- .
;
††. /
}
°° 	
private
££ 
void
££ &
LogStorageRemovalFailure
££ -
(
££- .
JSException
§§ 
	exception
§§ !
)
§§! "
{
•• 	
if
¶¶ 
(
¶¶ 
!
¶¶ 
_logger
¶¶ 
.
¶¶ 
	IsEnabled
¶¶ "
(
¶¶" #
LogLevel
¶¶# +
.
¶¶+ ,
Warning
¶¶, 3
)
¶¶3 4
)
¶¶4 5
{
ßß 
return
®® 
;
®® 
}
©© 
_logger
´´ 
.
´´ 

LogWarning
´´ 
(
´´ 
	exception
¨¨ 
,
¨¨ 
$str
≠≠ 8
+
≠≠9 :
$str
ÆÆ -
)
ÆÆ- .
;
ÆÆ. /
}
ØØ 	
private
±± 
void
±± #
LogInteropUnavailable
±± *
(
±±* +'
InvalidOperationException
≤≤ %
	exception
≤≤& /
)
≤≤/ 0
{
≥≥ 	
if
¥¥ 
(
¥¥ 
!
¥¥ 
_logger
¥¥ 
.
¥¥ 
	IsEnabled
¥¥ "
(
¥¥" #
LogLevel
¥¥# +
.
¥¥+ ,
Debug
¥¥, 1
)
¥¥1 2
)
¥¥2 3
{
µµ 
return
∂∂ 
;
∂∂ 
}
∑∑ 
_logger
ππ 
.
ππ 
LogDebug
ππ 
(
ππ 
	exception
∫∫ 
,
∫∫ 
$str
ªª B
+
ªªC D
$str
ºº <
)
ºº< =
;
ºº= >
}
ΩΩ 	
}
ææ 
}øø Œ$
lC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\AuthService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )
Implementations) 8
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
 
IAuthStateService

 *

_authState

+ 5
;

5 6
private 
readonly 

HttpClient #
_http$ )
;) *
public 
AuthService 
( 
IAuthStateService ,
	authState- 6
,6 7

HttpClient8 B
httpC G
)G H
{ 	

_authState 
= 
	authState "
;" #
_http 
= 
http 
; 
} 	
public 
async 
Task 
< 
AuthResponseDto )
?) *
>* +

LoginAsync, 6
(6 7
LoginDto7 ?
LoginDto@ H
)H I
{ 	
var 
response 
= 
await  
_http! &
.& '
PostAsJsonAsync' 6
(6 7
$str  
,  !
LoginDto 
) 
; 
if 
( 
! 
response 
. 
IsSuccessStatusCode -
)- .
{ 
var 
error 
= 
await !
response" *
.* +
Content+ 2
.2 3
ReadAsStringAsync3 D
(D E
)E F
;F G
throw 
new 
	Exception #
(# $
error$ )
)) *
;* +
} 
var   
result   
=   
await   
response   '
.  ' (
Content  ( /
.  / 0
ReadFromJsonAsync  0 A
<  A B
AuthResponseDto  B Q
>  Q R
(  R S
)  S T
;  T U
if"" 
("" 
result"" 
!="" 
null"" 
)"" 
{## 
await$$ 

_authState$$  
.$$  !
SetLoginAsync$$! .
($$. /
result$$/ 5
)$$5 6
;$$6 7
}%% 
return'' 
result'' 
;'' 
}(( 	
public** 
async** 
Task** 
LogoutAsync** %
(**% &
)**& '
{++ 	
await,, 

_authState,, 
.,, 
LogoutAsync,, (
(,,( )
),,) *
;,,* +
}-- 	
public// 
Task// 
<// 
string// 
?// 
>// 
GetTokenAsync// *
(//* +
)//+ ,
{00 	
return11 
Task11 
.11 

FromResult11 "
(11" #

_authState11# -
.11- .
Token11. 3
)113 4
;114 5
}22 	
public44 
Task44 
<44 
string44 
?44 
>44 
GetEmailAsync44 *
(44* +
)44+ ,
{55 	
return66 
Task66 
.66 

FromResult66 "
(66" #

_authState66# -
.66- .
Email66. 3
)663 4
;664 5
}77 	
public99 
Task99 
<99 
string99 
?99 
>99 
GetRoleAsync99 )
(99) *
)99* +
{:: 	
return;; 
Task;; 
.;; 

FromResult;; "
(;;" #

_authState;;# -
.;;- .
Role;;. 2
);;2 3
;;;3 4
}<< 	
public>> 
Task>> 
<>> 
bool>> 
>>> 
IsLoggedInAsync>> )
(>>) *
)>>* +
{?? 	
return@@ 
Task@@ 
.@@ 

FromResult@@ "
(@@" #

_authState@@# -
.@@- .

IsLoggedIn@@. 8
)@@8 9
;@@9 :
}AA 	
publicCC 
TaskCC 
<CC 
boolCC 
>CC 
IsAdminAsyncCC &
(CC& '
)CC' (
{DD 	
returnEE 
TaskEE 
.EE 

FromResultEE "
(EE" #

_authStateEE# -
.EE- .
IsAdminEE. 5
)EE5 6
;EE6 7
}FF 	
}GG 
}HH êg
vC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Services\Implmentations\AppointmentApiService.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Services  (
.( )
Implementations) 8
{ 
public 

class !
AppointmentApiService &
:' ("
IAppointmentApiService) ?
{ 
private 
readonly 

HttpClient #
_http$ )
;) *
public !
AppointmentApiService $
($ %

HttpClient% /
http0 4
)4 5
{ 	
_http 
= 
http 
; 
} 	
public 
async 
Task 
< 
List 
< "
AppointmentResponseDto 5
>5 6
>6 7 
GetAppointmentsAsync8 L
(L M
)M N
{ 	
var 
result 
= 
await 
_http $
.$ %
GetFromJsonAsync% 5
<5 6
List6 :
<: ;"
AppointmentResponseDto; Q
>Q R
>R S
(S T
$str !
) 
; 
return 
result 
?? 
new  
List! %
<% &"
AppointmentResponseDto& <
>< =
(= >
)> ?
;? @
} 	
public!! 
async!! 
Task!! 
<!! 
List!! 
<!! "
AppointmentResponseDto!! 5
>!!5 6
>!!6 7(
GetAppointmentsByDoctorAsync!!8 T
(!!T U
int!!U X
doctorId!!Y a
)!!a b
{"" 	
var## 
response## 
=## 
await##  
_http##! &
.##& '
GetAsync##' /
(##/ 0
$"$$ 
$str$$ )
{$$) *
doctorId$$* 2
}$$2 3
"$$3 4
)%% 
;%% 
if'' 
('' 
response'' 
.'' 
IsSuccessStatusCode'' ,
)'', -
{(( 
var)) 
result)) 
=)) 
await)) "
response))# +
.))+ ,
Content)), 3
.))3 4
ReadFromJsonAsync))4 E
<))E F
List))F J
<))J K"
AppointmentResponseDto))K a
>))a b
>))b c
())c d
)))d e
;))e f
return++ 
result++ 
??++  
new++! $
List++% )
<++) *"
AppointmentResponseDto++* @
>++@ A
(++A B
)++B C
;++C D
},, 
if.. 
(.. 
response.. 
... 

StatusCode.. #
==..$ &
HttpStatusCode..' 5
...5 6
NotFound..6 >
)..> ?
{// 
var00 
allAppointments00 #
=00$ %
await00& + 
GetAppointmentsAsync00, @
(00@ A
)00A B
;00B C
return22 
allAppointments22 &
.33 
Where33 
(33 
a33 
=>33 
a33  !
.33! "
DoctorId33" *
==33+ -
doctorId33. 6
)336 7
.44 
ToList44 
(44 
)44 
;44 
}55 
var77 
error77 
=77 
await77 
response77 &
.77& '
Content77' .
.77. /
ReadAsStringAsync77/ @
(77@ A
)77A B
;77B C
throw99 
new99  
HttpRequestException99 *
(99* +
$":: 
$str:: 9
{::9 :
doctorId::: B
}::B C
$str::C M
{::M N
(::N O
int::O R
)::R S
response::S [
.::[ \

StatusCode::\ f
}::f g
$str::g s
{::s t
error::t y
}::y z
"::z {
);; 
;;; 
}<< 	
public>> 
async>> 
Task>> 
<>> 
PagedResponseDto>> *
<>>* +"
AppointmentResponseDto>>+ A
>>>A B
>>>B C%
GetAppointmentsPagedAsync>>D ]
(>>] ^
int?? 

pageNumber?? 
,?? 
int@@ 
pageSize@@ 
,@@ 
stringAA 
?AA 
searchAA 
,AA 
AppointmentStatusBB 
?BB 
statusBB %
,BB% &
DateTimeCC 
?CC 
	startDateCC 
,CC  
DateTimeDD 
?DD 
endDateDD 
)DD 
{EE 	
varFF 
queryFF 
=FF 
newFF 
ListFF  
<FF  !
stringFF! '
>FF' (
{GG 
$"HH 
$strHH 
{HH 

pageNumberHH (
}HH( )
"HH) *
,HH* +
$"II 
$strII 
{II 
pageSizeII $
}II$ %
"II% &
}JJ 
;JJ 
ifLL 
(LL 
!LL 
stringLL 
.LL 
IsNullOrWhiteSpaceLL *
(LL* +
searchLL+ 1
)LL1 2
)LL2 3
{MM 
queryNN 
.NN 
AddNN 
(NN 
$"NN 
$strNN #
{NN# $
UriNN$ '
.NN' (
EscapeDataStringNN( 8
(NN8 9
searchNN9 ?
)NN? @
}NN@ A
"NNA B
)NNB C
;NNC D
}OO 
ifQQ 
(QQ 
statusQQ 
.QQ 
HasValueQQ 
)QQ  
{RR 
querySS 
.SS 
AddSS 
(SS 
$"SS 
$strSS #
{SS# $
UriSS$ '
.SS' (
EscapeDataStringSS( 8
(SS8 9
statusSS9 ?
.SS? @
ValueSS@ E
.SSE F
ToStringSSF N
(SSN O
)SSO P
)SSP Q
}SSQ R
"SSR S
)SSS T
;SST U
}TT 
ifVV 
(VV 
	startDateVV 
.VV 
HasValueVV "
)VV" #
{WW 
queryXX 
.XX 
AddXX 
(XX 
$"XX 
$strXX &
{XX& '
	startDateXX' 0
.XX0 1
ValueXX1 6
:XX6 7
$strXX7 A
}XXA B
"XXB C
)XXC D
;XXD E
}YY 
if[[ 
([[ 
endDate[[ 
.[[ 
HasValue[[  
)[[  !
{\\ 
query]] 
.]] 
Add]] 
(]] 
$"]] 
$str]] $
{]]$ %
endDate]]% ,
.]], -
Value]]- 2
:]]2 3
$str]]3 =
}]]= >
"]]> ?
)]]? @
;]]@ A
}^^ 
var`` 
url`` 
=`` 
$"`` 
$str`` .
{``. /
string``/ 5
.``5 6
Join``6 :
(``: ;
$str``; >
,``> ?
query``@ E
)``E F
}``F G
"``G H
;``H I
varbb 
resultbb 
=bb 
awaitbb 
_httpbb $
.bb$ %
GetFromJsonAsyncbb% 5
<bb5 6
PagedResponseDtobb6 F
<bbF G"
AppointmentResponseDtobbG ]
>bb] ^
>bb^ _
(bb_ `
urlbb` c
)bbc d
;bbd e
returndd 
resultdd 
??dd 
newdd  
PagedResponseDtodd! 1
<dd1 2"
AppointmentResponseDtodd2 H
>ddH I
(ddI J
)ddJ K
;ddK L
}ee 	
publicgg 
asyncgg 
Taskgg 
<gg 
Listgg 
<gg "
AppointmentResponseDtogg 5
>gg5 6
>gg6 7#
FilterAppointmentsAsyncgg8 O
(ggO P
AppointmentStatushh 
?hh 
statushh %
,hh% &
DateTimeii 
?ii 
	startDateii 
,ii  
DateTimejj 
?jj 
endDatejj 
)jj 
{kk 	
varll 
queryll 
=ll 
newll 
Listll  
<ll  !
stringll! '
>ll' (
(ll( )
)ll) *
;ll* +
ifnn 
(nn 
statusnn 
.nn 
HasValuenn 
)nn  
{oo 
querypp 
.pp 
Addpp 
(pp 
$"pp 
$strpp #
{pp# $
statuspp$ *
.pp* +
Valuepp+ 0
}pp0 1
"pp1 2
)pp2 3
;pp3 4
}qq 
ifss 
(ss 
	startDatess 
.ss 
HasValuess "
)ss" #
{tt 
queryuu 
.uu 
Adduu 
(uu 
$"uu 
$struu &
{uu& '
	startDateuu' 0
.uu0 1
Valueuu1 6
:uu6 7
$struu7 A
}uuA B
"uuB C
)uuC D
;uuD E
}vv 
ifxx 
(xx 
endDatexx 
.xx 
HasValuexx  
)xx  !
{yy 
queryzz 
.zz 
Addzz 
(zz 
$"zz 
$strzz $
{zz$ %
endDatezz% ,
.zz, -
Valuezz- 2
:zz2 3
$strzz3 =
}zz= >
"zz> ?
)zz? @
;zz@ A
}{{ 
var}} 
url}} 
=}} 
query}} 
.}} 
Count}} !
==}}" $
$num}}% &
?~~ 
$str~~ *
: 
$" 
$str +
{+ ,
string, 2
.2 3
Join3 7
(7 8
$str8 ;
,; <
query= B
)B C
}C D
"D E
;E F
var
ÅÅ 
result
ÅÅ 
=
ÅÅ 
await
ÅÅ 
_http
ÅÅ $
.
ÅÅ$ %
GetFromJsonAsync
ÅÅ% 5
<
ÅÅ5 6
List
ÅÅ6 :
<
ÅÅ: ;$
AppointmentResponseDto
ÅÅ; Q
>
ÅÅQ R
>
ÅÅR S
(
ÅÅS T
url
ÅÅT W
)
ÅÅW X
;
ÅÅX Y
return
ÉÉ 
result
ÉÉ 
??
ÉÉ 
new
ÉÉ  
List
ÉÉ! %
<
ÉÉ% &$
AppointmentResponseDto
ÉÉ& <
>
ÉÉ< =
(
ÉÉ= >
)
ÉÉ> ?
;
ÉÉ? @
}
ÑÑ 	
public
ÜÜ 
async
ÜÜ 
Task
ÜÜ 
<
ÜÜ 
bool
ÜÜ 
>
ÜÜ %
ConfirmAppointmentAsync
ÜÜ  7
(
ÜÜ7 8
int
ÜÜ8 ;
appointmentId
ÜÜ< I
)
ÜÜI J
{
áá 	
var
àà 
response
àà 
=
àà 
await
àà  
_http
àà! &
.
àà& '
	PostAsync
àà' 0
(
àà0 1
$"
ââ 
$str
ââ *
{
ââ* +
appointmentId
ââ+ 8
}
ââ8 9
"
ââ9 :
,
ââ: ;
null
ää 
)
ãã 
;
ãã 
return
çç 
response
çç 
.
çç !
IsSuccessStatusCode
çç /
;
çç/ 0
}
éé 	
public
êê 
async
êê 
Task
êê 
<
êê 
bool
êê 
>
êê $
CancelAppointmentAsync
êê  6
(
êê6 7
int
êê7 :
appointmentId
êê; H
,
êêH I
string
êêJ P
reason
êêQ W
)
êêW X
{
ëë 	
var
íí 
encodedReason
íí 
=
íí 
Uri
íí  #
.
íí# $
EscapeDataString
íí$ 4
(
íí4 5
reason
íí5 ;
)
íí; <
;
íí< =
var
îî 
response
îî 
=
îî 
await
îî  
_http
îî! &
.
îî& '
	PostAsync
îî' 0
(
îî0 1
$"
ïï 
$str
ïï )
{
ïï) *
appointmentId
ïï* 7
}
ïï7 8
$str
ïï8 @
{
ïï@ A
encodedReason
ïïA N
}
ïïN O
"
ïïO P
,
ïïP Q
null
ññ 
)
óó 
;
óó 
return
ôô 
response
ôô 
.
ôô !
IsSuccessStatusCode
ôô /
;
ôô/ 0
}
öö 	
public
úú 
async
úú 
Task
úú 
<
úú 
bool
úú 
>
úú $
DeleteAppointmentAsync
úú  6
(
úú6 7
int
úú7 :
appointmentId
úú; H
)
úúH I
{
ùù 	
var
ûû 
response
ûû 
=
ûû 
await
ûû  
_http
ûû! &
.
ûû& '
DeleteAsync
ûû' 2
(
ûû2 3
$"
üü 
$str
üü "
{
üü" #
appointmentId
üü# 0
}
üü0 1
"
üü1 2
)
†† 
;
†† 
return
¢¢ 
response
¢¢ 
.
¢¢ !
IsSuccessStatusCode
¢¢ /
;
¢¢/ 0
}
££ 	
}
§§ 
}•• ‹ 
PC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Program.cs
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
builder 
. 
RootComponents 
. 
Add 
< 

HeadOutlet %
>% &
(& '
$str' 4
)4 5
;5 6
builder 
. 
Services 
. 
	AddScoped 
< 
IAuthStateService ,
,, -
AuthStateService. >
>> ?
(? @
)@ A
;A B
builder 
. 
Services 
. 
	AddScoped 
< 
AuthHeaderHandler ,
>, -
(- .
). /
;/ 0
builder 
. 
Services 
. 
AddHttpClient 
( 
$str .
,. /
client0 6
=>7 9
{ 
client 

.
 
BaseAddress 
= 
new 
Uri  
(  !
$str! :
): ;
;; <
} 
) 
. !
AddHttpMessageHandler 
< 
AuthHeaderHandler (
>( )
() *
)* +
;+ ,
builder 
. 
Services 
. 
	AddScoped 
( 
sp 
=>  
sp 
. 
GetRequiredService 
< 
IHttpClientFactory ,
>, -
(- .
). /
. 
CreateClient 
( 
$str #
)# $
)$ %
;% &
builder 
. 
Services 
. 
	AddScoped 
< 
IAuthService '
,' (
AuthService) 4
>4 5
(5 6
)6 7
;7 8
builder 
. 
Services 
. 
	AddScoped 
< 
IDoctorApiService ,
,, -
DoctorApiService. >
>> ?
(? @
)@ A
;A B
builder 
. 
Services 
. 
	AddScoped 
< 
IPatientApiService -
,- .
PatientApiService/ @
>@ A
(A B
)B C
;C D
builder   
.   
Services   
.   
	AddScoped   
<   "
IAppointmentApiService   1
,  1 2!
AppointmentApiService  3 H
>  H I
(  I J
)  J K
;  K L
builder!! 
.!! 
Services!! 
.!! 
	AddScoped!! 
<!! #
IHealthRecordApiService!! 2
,!!2 3"
HealthRecordApiService!!4 J
>!!J K
(!!K L
)!!L M
;!!M N
builder"" 
."" 
Services"" 
."" 
	AddScoped"" 
<"" 
IDashboardService"" ,
,"", -
DashboardService"". >
>""> ?
(""? @
)""@ A
;""A B
builder$$ 
.$$ 
Services$$ 
.$$  
AddAuthorizationCore$$ %
($$% &
)$$& '
;$$' (
builder&& 
.&& 
Services&& 
.&& 
	AddScoped&& 
<&& '
AuthenticationStateProvider&& 6
,&&6 7-
!CustomAuthenticationStateProvider&&8 Y
>&&Y Z
(&&Z [
)&&[ \
;&&\ ]
await(( 
builder(( 
.(( 
Build(( 
((( 
)(( 
.(( 
RunAsync(( 
((( 
)((  
;((  !„
aC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Handlers\AuthHeadHandler.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Handlers  (
{ 
public 

class 
AuthHeaderHandler "
:# $
DelegatingHandler% 6
{ 
private 
readonly 
IAuthStateService *

_authState+ 5
;5 6
public

 
AuthHeaderHandler

  
(

  !
IAuthStateService

! 2
	authState

3 <
)

< =
{ 	

_authState 
= 
	authState "
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
HttpRequestMessage 
request &
,& '
CancellationToken 
cancellationToken /
)/ 0
{ 	
await 

_authState 
.  
LoadFromStorageAsync 1
(1 2
)2 3
;3 4
var 
token 
= 

_authState "
." #
Token# (
;( )
if 
( 
! 
string 
. 
IsNullOrWhiteSpace *
(* +
token+ 0
)0 1
)1 2
{ 
request 
. 
Headers 
.  
Authorization  -
=. /
new %
AuthenticationHeaderValue 1
(1 2
$str2 :
,: ;
token< A
)A B
;B C
} 
return 
await 
base 
. 
	SendAsync '
(' (
request( /
,/ 0
cancellationToken1 B
)B C
;C D
} 	
}   
}!! ¯
iC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\DTOs\Dashboard\AdminDashboardDTO.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
DTOs  $
.$ %
	Dashboard% .
{ 
public 
class 
AdminDashboardDto &
{ 	
public 
int 
TotalDoctors #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
int 
ActiveDoctors $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public		 
int		 
TotalPatients		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		1 2
public 
int 
TotalAppointments (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
int 
PendingAppointments *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
int !
ConfirmedAppointments ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
int !
CancelledAppointments ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
int 
TotalHealthRecords )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 	
} †
pC:\Users\287802\source\repos\Sprint4_HealthAxis\HealthAxisAdminLayout\Auth\CustomeAuthenticationStateProvider.cs
	namespace 	!
HealthAxisAdminLayout
 
.  
Auth  $
;$ %
public 
class -
!CustomAuthenticationStateProvider .
:/ 0'
AuthenticationStateProvider1 L
{ 
private		 
readonly		 
IAuthStateService		 &

_authState		' 1
;		1 2
public 
-
!CustomAuthenticationStateProvider ,
(, -
IAuthStateService- >
	authState? H
)H I
{ 

_authState 
= 
	authState 
; 
} 
public 

override 
async 
Task 
< 
AuthenticationState 2
>2 3'
GetAuthenticationStateAsync4 O
(O P
)P Q
{ 
try 
{ 	
await 

_authState 
.  
LoadFromStorageAsync 1
(1 2
)2 3
;3 4
} 	
catch 
{ 	
} 	
ClaimsIdentity 
identity 
;  
if 

( 
! 
string 
. 
IsNullOrWhiteSpace &
(& '

_authState' 1
.1 2
Token2 7
)7 8
)8 9
{ 	
var 
claims 
= 
new 
List !
<! "
Claim" '
>' (
{   
new!! 
Claim!! 
(!! 

ClaimTypes!! $
.!!$ %
Name!!% )
,!!) *

_authState!!+ 5
.!!5 6
Email!!6 ;
??!!< >
$str!!? A
)!!A B
,!!B C
new"" 
Claim"" 
("" 

ClaimTypes"" $
.""$ %
Role""% )
,"") *

_authState""+ 5
.""5 6
Role""6 :
??""; =
$str""> @
)""@ A
}## 
;## 
identity%% 
=%% 
new%% 
ClaimsIdentity%% )
(%%) *
claims%%* 0
,%%0 1
$str%%2 7
)%%7 8
;%%8 9
}&& 	
else'' 
{(( 	
identity)) 
=)) 
new)) 
ClaimsIdentity)) )
())) *
)))* +
;))+ ,
}** 	
var,, 
user,, 
=,, 
new,, 
ClaimsPrincipal,, &
(,,& '
identity,,' /
),,/ 0
;,,0 1
return.. 
new.. 
AuthenticationState.. &
(..& '
user..' +
)..+ ,
;.., -
}// 
}00 