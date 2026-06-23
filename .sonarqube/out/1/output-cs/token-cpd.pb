⁄
gC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IPatientService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public 

	interface 
IPatientService $
{ 
Task		 
<		 
List		 
<		 

PatientDto		 
>		 
>		 
GetAllPatientsAsync		 2
(		2 3
)		3 4
;		4 5
Task 
< 
PagedResponse 
< 

PatientDto %
>% &
>& '$
GetAllPatientsPagedAsync( @
(@ A%
PatientPaginationQueryDtoA Z
query[ `
)` a
;a b
Task 
< 

PatientDto 
> 
GetPatientByIdAsync ,
(, -
int- 0
	patientId1 :
): ;
;; <
Task 
< 

PatientDto 
>  
RegisterPatientAsync -
(- .
CreatePatientDto. >
dto? B
)B C
;C D
Task 
< 

PatientDto 
> 
UpdatePatientAsync +
(+ ,
int, /
	patientId0 9
,9 :
UpdatePatientDto; K
dtoL O
)O P
;P Q
Task 
< 

PatientDto 
> 
GetMyProfileAsync *
(* +
string+ 1
identityUserId2 @
)@ A
;A B
Task 
< 

PatientDto 
>  
UpdateMyProfileAsync -
(- .
string. 4
identityUserId5 C
,C D
UpdatePatientDtoE U
dtoV Y
)Y Z
;Z [
} 
} ê!
lC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IHealthRecordService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public 

	interface  
IHealthRecordService )
{ 
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #$
GetAllHealthRecordsAsync$ <
(< =
)= >
;> ?
Task 
< 
HealthRecordDto 
> $
GetHealthRecordByIdAsync 6
(6 7
int7 :
healthRecordId; I
)I J
;J K
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
 
HealthRecordDto

 !
>

! "
>

" #,
 GetHealthRecordsByPatientIdAsync

$ D
(

D E
int

E H
	patientId

I R
)

R S
;

S T
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #+
GetHealthRecordsByDoctorIdAsync$ C
(C D
intD G
doctorIdH P
)P Q
;Q R
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #0
$GetHealthRecordsByAppointmentIdAsync$ H
(H I
intI L
appointmentIdM Z
)Z [
;[ \
Task 
< 
HealthRecordDto 
>  
AddHealthRecordAsync 2
(2 3
AddHealthRecordDto3 E
dtoF I
)I J
;J K
Task 
< 
HealthRecordDto 
> #
UpdateHealthRecordAsync 5
(5 6
int6 9
healthRecordId: H
,H I!
UpdateHealthRecordDtoJ _
dto` c
)c d
;d e
Task 
< 
HealthRecordDto 
> #
DeleteHealthRecordAsync 5
(5 6
int6 9
healthRecordId: H
)H I
;I J
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #-
!GetMyHealthRecordsForPatientAsync$ E
(E F
stringF L
identityUserIdM [
)[ \
;\ ]
Task 
< 
HealthRecordDto 
> .
"GetHealthRecordByIdForPatientAsync @
(@ A
intA D
healthRecordIdE S
,S T
stringT Z
identityUserId[ i
)i j
;j k
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #,
 GetMyHealthRecordsForDoctorAsync$ D
(D E
stringE K
identityUserIdL Z
)Z [
;[ \
Task 
< 
HealthRecordDto 
> -
!GetHealthRecordByIdForDoctorAsync ?
(? @
int@ C
healthRecordIdD R
,R S
stringS Y
identityUserIdZ h
)h i
;i j
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #9
-GetHealthRecordsByAppointmentIdForDoctorAsync$ Q
(Q R
intR U
appointmentIdV c
,c d
stringd j
identityUserIdk y
)y z
;z {
Task   
<   
HealthRecordDto   
>   )
AddHealthRecordForDoctorAsync   ;
(  ; <
AddHealthRecordDto  < N
dto  O R
,  R S
string  S Y
identityUserId  Z h
)  h i
;  i j
Task"" 
<"" 
HealthRecordDto"" 
>"" ,
 UpdateHealthRecordForDoctorAsync"" >
(""> ?
int""? B
healthRecordId""C Q
,""Q R!
UpdateHealthRecordDto""S h
dto""i l
,""l m
string""m s
identityUserId	""t Ç
)
""Ç É
;
""É Ñ
}## 
}$$ Î
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IDoctorService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public 

	interface 
IDoctorService #
{ 
Task		 
<		 
List		 
<		 
	DoctorDto		 
>		 
>		 
GetAllDoctorsAsync		 0
(		0 1
)		1 2
;		2 3
Task 
< 
List 
< 
	DoctorDto 
> 
> $
GetAllActiveDoctorsAsync 6
(6 7
)7 8
;8 9
Task 
< 
	DoctorDto 
> 
GetDoctorByIdAsync *
(* +
int+ .
doctorId/ 7
)7 8
;8 9
Task 
< 
List 
< 
	DoctorDto 
> 
> +
GetDoctorsBySpecialisationAsync =
(= >
SpecialisationType> P
specialisationQ _
)_ `
;` a
Task 
< 
List 
< 
	DoctorDto 
> 
> 1
%GetActiveDoctorsBySpecialisationAsync C
(C D
SpecialisationTypeD V
specialisationW e
)e f
;f g
Task 
< $
DoctorCreatedResponseDto %
>% &$
CreateDoctorByAdminAsync' ?
(? @
CreateDoctorDto@ O
dtoP S
)S T
;T U
Task 
< 
	DoctorDto 
> 
UpdateDoctorAsync )
() *
int* -
doctorId. 6
,6 7
UpdateDoctorDto8 G
dtoH K
)K L
;L M
Task 
< 
	DoctorDto 
> 
DeleteDoctorAsync )
() *
int* -
doctorId. 6
)6 7
;7 8
Task 
< 
List 
< 
string 
> 
> &
GetDoctorAvailabilityAsync 5
(5 6
int6 9
doctorId: B
)B C
;C D
Task 
< 
	DoctorDto 
> 
GetMyProfileAsync )
() *
string* 0
identityUserId1 ?
)? @
;@ A
Task 
< 
PagedResponse 
< 
	DoctorDto $
>$ %
>% &#
GetAllDoctorsPagedAsync' >
(> ?$
DoctorPaginationQueryDto? W
queryX ]
)] ^
;^ _
} 
} ±
dC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IAuthService.cs
	namespace 	
HealthCareApp
 
. 
Services  
.  !
	Interface! *
{ 
public 

	interface 
IAuthService !
{ 
Task		 
<		 
(		 
bool		 
Success		 
,		 
string		 "
Message		# *
,		* +
int		, /
	PatientId		0 9
)		9 :
>		: ; 
RegisterPatientAsync		< P
(		P Q
PatientRegisterDto		Q c
request		d k
)		k l
;		l m
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
string, 2
Token3 8
,8 9
int: =
	ExpiresIn> G
)G H
>H I
LoginJ O
(O P
LoginDtoP X
requestY `
)` a
;a b
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
)* +
>+ ,
ChangePasswordAsync- @
(@ A
stringA G
userIdH N
,N O
ChangePasswordDtoP a
requestb i
)i j
;j k
} 
} ’A
kC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IAppointmentService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public 

	interface 
IAppointmentService (
{		 
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
 
AppointmentDto

  
>

  !
>

! "#
GetAllAppointmentsAsync

# :
(

: ;
)

; <
;

< =
Task 
< 
AppointmentDto 
> #
GetAppointmentByIdAsync 4
(4 5
int5 8
appointmentId9 F
)F G
;G H
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "+
GetAppointmentsByPatientIdAsync# B
(B C
intC F
	patientIdG P
)P Q
;Q R
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "*
GetAppointmentsByDoctorIdAsync# A
(A B
intB E
doctorIdF N
)N O
;O P
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "(
GetAppointmentsByStatusAsync# ?
(? @
AppointmentStatus@ Q
statusR X
)X Y
;Y Z
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "(
GetUpcomingAppointmentsAsync# ?
(? @
)@ A
;A B
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "3
'GetUpcomingAppointmentsByPatientIdAsync# J
(J K
intK N
	patientIdO X
)X Y
;Y Z
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "2
&GetUpcomingAppointmentsByDoctorIdAsync# I
(I J
intJ M
doctorIdN V
)V W
;W X
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "2
&GetPendingAppointmentsByPatientIdAsync# I
(I J
intJ M
	patientIdN W
)W X
;X Y
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "1
%GetPendingAppointmentsByDoctorIdAsync# H
(H I
intI L
doctorIdM U
)U V
;V W
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "8
,GetTodayConfirmedAppointmentsByDoctorIdAsync# O
(O P
intP S
doctorIdT \
)\ ]
;] ^
Task   
<   
AppointmentDto   
>    
BookAppointmentAsync   1
(  1 2
BookAppointmentDto  2 D
dto  E H
)  H I
;  I J
Task"" 
<"" 
AppointmentDto"" 
>"" "
UpdateAppointmentAsync"" 3
(""3 4
int""4 7
appointmentId""8 E
,""E F 
UpdateAppointmentDto""G [
dto""\ _
)""_ `
;""` a
Task$$ 
<$$ 
AppointmentDto$$ 
>$$ #
ConfirmAppointmentAsync$$ 4
($$4 5
int$$5 8
appointmentId$$9 F
)$$F G
;$$G H
Task&& 
<&& 
AppointmentDto&& 
>&& $
CompleteAppointmentAsync&& 5
(&&5 6
int&&6 9
appointmentId&&: G
)&&G H
;&&H I
Task(( 
<(( 
AppointmentDto(( 
>(( "
CancelAppointmentAsync(( 3
(((3 4 
CancelAppointmentDto((4 H
dto((I L
)((L M
;((M N
Task** 
<** 
AppointmentDto** 
>** "
DeleteAppointmentAsync** 3
(**3 4
int**4 7
appointmentId**8 E
)**E F
;**F G
Task,, 
<,, 
List,, 
<,, 
AppointmentDto,,  
>,,  !
>,,! ",
 GetMyAppointmentsForPatientAsync,,# C
(,,C D
string,,D J
identityUserId,,K Y
),,Y Z
;,,Z [
Task.. 
<.. 
List.. 
<.. 
AppointmentDto..  
>..  !
>..! "4
(GetMyUpcomingAppointmentsForPatientAsync..# K
(..K L
string..L R
identityUserId..S a
)..a b
;..b c
Task00 
<00 
List00 
<00 
AppointmentDto00  
>00  !
>00! "3
'GetMyPendingAppointmentsForPatientAsync00# J
(00J K
string00K Q
identityUserId00R `
)00` a
;00a b
Task22 
<22 
AppointmentDto22 
>22 -
!GetAppointmentByIdForPatientAsync22 >
(22> ?
int22? B
appointmentId22C P
,22P Q
string22R X
identityUserId22Y g
)22g h
;22h i
Task44 
<44 
AppointmentDto44 
>44 *
BookAppointmentForPatientAsync44 ;
(44; <
BookAppointmentDto44< N
dto44O R
,44R S
string44T Z
identityUserId44[ i
)44i j
;44j k
Task66 
<66 
AppointmentDto66 
>66 ,
 CancelAppointmentForPatientAsync66 =
(66= > 
CancelAppointmentDto66> R
dto66S V
,66V W
string66X ^
identityUserId66_ m
)66m n
;66n o
Task88 
<88 
List88 
<88 
AppointmentDto88  
>88  !
>88! "+
GetMyAppointmentsForDoctorAsync88# B
(88B C
string88C I
identityUserId88J X
)88X Y
;88Y Z
Task:: 
<:: 
List:: 
<:: 
AppointmentDto::  
>::  !
>::! "3
'GetMyUpcomingAppointmentsForDoctorAsync::# J
(::J K
string::K Q
identityUserId::R `
)::` a
;::a b
Task<< 
<<< 
List<< 
<<< 
AppointmentDto<<  
><<  !
><<! "2
&GetMyPendingAppointmentsForDoctorAsync<<# I
(<<I J
string<<J P
identityUserId<<Q _
)<<_ `
;<<` a
Task>> 
<>> 
List>> 
<>> 
AppointmentDto>>  
>>>  !
>>>! "9
-GetMyTodayConfirmedAppointmentsForDoctorAsync>># P
(>>P Q
string>>Q W
identityUserId>>X f
)>>f g
;>>g h
Task@@ 
<@@ 
AppointmentDto@@ 
>@@ ,
 GetAppointmentByIdForDoctorAsync@@ =
(@@= >
int@@> A
appointmentId@@B O
,@@O P
string@@Q W
identityUserId@@X f
)@@f g
;@@g h
TaskBB 
<BB 
AppointmentDtoBB 
>BB ,
 ConfirmAppointmentForDoctorAsyncBB =
(BB= >
intBB> A
appointmentIdBBB O
,BBO P
stringBBQ W
identityUserIdBBX f
)BBf g
;BBg h
TaskDD 
<DD 
AppointmentDtoDD 
>DD -
!CompleteAppointmentForDoctorAsyncDD >
(DD> ?
intDD? B
appointmentIdDDC P
,DDP Q
stringDDR X
identityUserIdDDY g
)DDg h
;DDh i
TaskFF 
<FF 
AppointmentDtoFF 
>FF +
CancelAppointmentForDoctorAsyncFF <
(FF< = 
CancelAppointmentDtoFF= Q
dtoFFR U
,FFU V
stringFFV \
identityUserIdFF] k
)FFk l
;FFl m
TaskHH 
<HH 
PagedResponseHH 
<HH 
AppointmentDtoHH )
>HH) *
>HH* +(
GetAllAppointmentsPagedAsyncHH, H
(HHH I)
AppointmentPaginationQueryDtoHHI f
queryHHg l
)HHl m
;HHm n
}II 
}JJ ‰”
aC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\PatientService.cs
	namespace 	
HealthCareApp
 
. 
Services  
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
(

  
IPatientRepository

  2

repository

3 =
,

= >
IMapper

? F
mapper

G M
)

M N
:

O P
IPatientService

Q `
{ 
private 
const 
string 
PatientEntityName .
=/ 0
$str1 :
;: ;
private 
static 
readonly 
DateTime  (
MinimumDateOfBirth) ;
=< =
new> A
(A B
$num 
, 
$num 
, 
$num 
, 
$num 
, 
$num 
, 
$num 
, 
DateTimeKind 
. 
Unspecified $
)$ %
;% &
public 
async 
Task 
< 
List 
< 

PatientDto )
>) *
>* +
GetAllPatientsAsync, ?
(? @
)@ A
{ 	
var 
patients 
= 
await  

repository! +
.+ ,
GetAllAsync, 7
(7 8
)8 9
;9 :
return 
mapper 
. 
Map 
< 
List "
<" #

PatientDto# -
>- .
>. /
(/ 0
patients0 8
)8 9
;9 :
} 	
public 
async 
Task 
< 
PagedResponse '
<' (

PatientDto( 2
>2 3
>3 4$
GetAllPatientsPagedAsync5 M
(M N%
PatientPaginationQueryDtoN g
queryh m
)m n
{ 	
if   
(   
query   
is   
null   
)   
{!! 
query"" 
="" 
new"" %
PatientPaginationQueryDto"" 5
(""5 6
)""6 7
;""7 8
}## 
int%% 

pageNumber%% 
=%% 
query%% "
.%%" #

PageNumber%%# -
<=%%. 0
$num%%1 2
?%%3 4
$num%%5 6
:%%7 8
query%%9 >
.%%> ?

PageNumber%%? I
;%%I J
int'' 
pageSize'' 
='' 
query''  
.''  !
PageSize''! )
<=''* ,
$num''- .
?''/ 0
$num''1 3
:''4 5
query''6 ;
.''; <
PageSize''< D
;''D E
pageSize)) 
=)) 
pageSize)) 
>))  !
$num))" %
?))& '
$num))( +
:)), -
pageSize)). 6
;))6 7
var++ 
patients++ 
=++ 
await++  

repository++! +
.+++ ,
GetAllAsync++, 7
(++7 8
)++8 9
;++9 :
var-- 
filteredPatients--  
=--! "
patients--# +
.--+ ,
AsEnumerable--, 8
(--8 9
)--9 :
;--: ;
if// 
(// 
!// 
string// 
.// 
IsNullOrWhiteSpace// *
(//* +
query//+ 0
.//0 1

SearchTerm//1 ;
)//; <
)//< =
{00 
string11 

searchTerm11 !
=11" #
query11$ )
.11) *

SearchTerm11* 4
.114 5
Trim115 9
(119 :
)11: ;
;11; <
filteredPatients33  
=33! "
filteredPatients33# 3
.333 4
Where334 9
(339 :
p33: ;
=>33< >
p44 
.44 
PatientName44 !
.44! "
Contains44" *
(44* +

searchTerm44+ 5
,445 6
StringComparison447 G
.44G H
OrdinalIgnoreCase44H Y
)44Y Z
||44[ ]
p55 
.55 
Email55 
.55 
Contains55 $
(55$ %

searchTerm55% /
,55/ 0
StringComparison551 A
.55A B
OrdinalIgnoreCase55B S
)55S T
||55U W
p66 
.66 
PhoneNumber66 !
.66! "
Contains66" *
(66* +

searchTerm66+ 5
,665 6
StringComparison667 G
.66G H
OrdinalIgnoreCase66H Y
)66Y Z
||66[ ]
(77 
!77 
string77 
.77 
IsNullOrWhiteSpace77 /
(77/ 0
p770 1
.771 2
InsuranceID772 =
)77= >
&&77? A
p88 
.88 
InsuranceID88 "
.88" #
Contains88# +
(88+ ,

searchTerm88, 6
,886 7
StringComparison888 H
.88H I
OrdinalIgnoreCase88I Z
)88Z [
)88[ \
)88\ ]
;88] ^
}99 
if;; 
(;; 
query;; 
.;; 
Gender;; 
is;; 
not;;  #
null;;$ (
);;( )
{<< 
filteredPatients==  
===! "
filteredPatients==# 3
.==3 4
Where==4 9
(==9 :
p==: ;
=>==< >
p>> 
.>> 
Gender>> 
==>> 
query>>  %
.>>% &
Gender>>& ,
.>>, -
Value>>- 2
)>>2 3
;>>3 4
}?? 
ifAA 
(AA 
queryAA 
.AA 
HasInsuranceAA "
isAA# %
notAA& )
nullAA* .
)AA. /
{BB 
ifCC 
(CC 
queryCC 
.CC 
HasInsuranceCC &
.CC& '
ValueCC' ,
)CC, -
{DD 
filteredPatientsEE $
=EE% &
filteredPatientsEE' 7
.EE7 8
WhereEE8 =
(EE= >
pEE> ?
=>EE@ B
!FF 
stringFF 
.FF  
IsNullOrWhiteSpaceFF  2
(FF2 3
pFF3 4
.FF4 5
InsuranceIDFF5 @
)FF@ A
)FFA B
;FFB C
}GG 
elseHH 
{II 
filteredPatientsJJ $
=JJ% &
filteredPatientsJJ' 7
.JJ7 8
WhereJJ8 =
(JJ= >
pJJ> ?
=>JJ@ B
stringKK 
.KK 
IsNullOrWhiteSpaceKK 1
(KK1 2
pKK2 3
.KK3 4
InsuranceIDKK4 ?
)KK? @
)KK@ A
;KKA B
}LL 
}MM 
intOO 
totalRecordsOO 
=OO 
filteredPatientsOO /
.OO/ 0
CountOO0 5
(OO5 6
)OO6 7
;OO7 8
varQQ 
pagedPatientsQQ 
=QQ 
filteredPatientsQQ  0
.RR 
OrderByRR 
(RR 
pRR 
=>RR 
pRR 
.RR  
	PatientIdRR  )
)RR) *
.SS 
SkipSS 
(SS 
(SS 

pageNumberSS !
-SS" #
$numSS$ %
)SS% &
*SS' (
pageSizeSS) 1
)SS1 2
.TT 
TakeTT 
(TT 
pageSizeTT 
)TT 
.UU 
ToListUU 
(UU 
)UU 
;UU 
varWW 
mappedPatientsWW 
=WW  
mapperWW! '
.WW' (
MapWW( +
<WW+ ,
ListWW, 0
<WW0 1

PatientDtoWW1 ;
>WW; <
>WW< =
(WW= >
pagedPatientsWW> K
)WWK L
;WWL M
returnYY 
newYY 
PagedResponseYY $
<YY$ %

PatientDtoYY% /
>YY/ 0
{ZZ 
Items[[ 
=[[ 
mappedPatients[[ &
,[[& '

PageNumber\\ 
=\\ 

pageNumber\\ '
,\\' (
PageSize]] 
=]] 
pageSize]] #
,]]# $
TotalRecords^^ 
=^^ 
totalRecords^^ +
,^^+ ,

TotalPages__ 
=__ 
(__ 
int__ !
)__! "
Math__" &
.__& '
Ceiling__' .
(__. /
totalRecords__/ ;
/__< =
(__> ?
double__? E
)__E F
pageSize__F N
)__N O
}`` 
;`` 
}aa 	
publiccc 
asynccc 
Taskcc 
<cc 

PatientDtocc $
>cc$ %
GetPatientByIdAsynccc& 9
(cc9 :
intcc: =
	patientIdcc> G
)ccG H
{dd 	
ValidatePatientIdee 
(ee 
	patientIdee '
)ee' (
;ee( )
vargg 
patientgg 
=gg 
awaitgg 

repositorygg  *
.gg* +
GetByIdAsyncgg+ 7
(gg7 8
	patientIdgg8 A
)ggA B
;ggB C
ifii 
(ii 
patientii 
isii 
nullii 
)ii  
{jj 
throwkk 
newkk #
EntityNotFoundExceptionkk 1
(kk1 2
PatientEntityNamekk2 C
,kkC D
	patientIdkkE N
)kkN O
;kkO P
}ll 
returnnn 
mappernn 
.nn 
Mapnn 
<nn 

PatientDtonn (
>nn( )
(nn) *
patientnn* 1
)nn1 2
;nn2 3
}oo 	
publicqq 
asyncqq 
Taskqq 
<qq 

PatientDtoqq $
>qq$ % 
RegisterPatientAsyncqq& :
(qq: ;
CreatePatientDtoqq; K
dtoqqL O
)qqO P
{rr 	$
ValidateCreatePatientDtoss $
(ss$ %
dtoss% (
)ss( )
;ss) *
stringuu 
patientNameuu 
=uu  
dtouu! $
.uu$ %
FullNameuu% -
.uu- .
Trimuu. 2
(uu2 3
)uu3 4
.uu4 5
ToLoweruu5 <
(uu< =
)uu= >
;uu> ?
stringvv 
emailvv 
=vv 
dtovv 
.vv 
Emailvv $
.vv$ %
Trimvv% )
(vv) *
)vv* +
.vv+ ,
ToLowervv, 3
(vv3 4
)vv4 5
;vv5 6
stringww 
phoneNumberww 
=ww  
dtoww! $
.ww$ %
PhoneNumberww% 0
.ww0 1
Trimww1 5
(ww5 6
)ww6 7
;ww7 8
DateTimexx 
dateOfBirthxx  
=xx! "
dtoxx# &
.xx& '
DateOfBirthxx' 2
.xx2 3
Datexx3 7
;xx7 8
boolzz 
	duplicatezz 
=zz 
awaitzz "

repositoryzz# -
.zz- .#
IsDuplicatePatientAsynczz. E
(zzE F
patientName{{ 
,{{ 
email|| 
,|| 
phoneNumber}} 
,}} 
dateOfBirth~~ 
)~~ 
;~~ 
if
ÄÄ 
(
ÄÄ 
	duplicate
ÄÄ 
)
ÄÄ 
{
ÅÅ 
throw
ÇÇ 
new
ÇÇ 
ConflictException
ÇÇ +
(
ÇÇ+ ,
$str
ÇÇ, \
)
ÇÇ\ ]
;
ÇÇ] ^
}
ÉÉ 
var
ÖÖ 
patient
ÖÖ 
=
ÖÖ 
mapper
ÖÖ  
.
ÖÖ  !
Map
ÖÖ! $
<
ÖÖ$ %
Patient
ÖÖ% ,
>
ÖÖ, -
(
ÖÖ- .
dto
ÖÖ. 1
)
ÖÖ1 2
;
ÖÖ2 3
patient
áá 
.
áá 
DateOfBirth
áá 
=
áá  !
dateOfBirth
áá" -
;
áá- .
patient
àà 
.
àà 
CreatedDate
àà 
=
àà  !
DateTime
àà" *
.
àà* +
Now
àà+ .
;
àà. /
var
ää 
savedPatient
ää 
=
ää 
await
ää $

repository
ää% /
.
ää/ 0
CreateAsync
ää0 ;
(
ää; <
patient
ää< C
)
ääC D
;
ääD E
return
åå 
mapper
åå 
.
åå 
Map
åå 
<
åå 

PatientDto
åå (
>
åå( )
(
åå) *
savedPatient
åå* 6
)
åå6 7
;
åå7 8
}
çç 	
public
èè 
async
èè 
Task
èè 
<
èè 

PatientDto
èè $
>
èè$ % 
UpdatePatientAsync
èè& 8
(
èè8 9
int
èè9 <
	patientId
èè= F
,
èèF G
UpdatePatientDto
èèH X
dto
èèY \
)
èè\ ]
{
êê 	
ValidatePatientId
ëë 
(
ëë 
	patientId
ëë '
)
ëë' (
;
ëë( )&
ValidateUpdatePatientDto
ìì $
(
ìì$ %
dto
ìì% (
)
ìì( )
;
ìì) *
var
ïï 
existingPatient
ïï 
=
ïï  !
await
ïï" '

repository
ïï( 2
.
ïï2 3
GetByIdAsync
ïï3 ?
(
ïï? @
	patientId
ïï@ I
)
ïïI J
;
ïïJ K
if
óó 
(
óó 
existingPatient
óó 
is
óó  "
null
óó# '
)
óó' (
{
òò 
throw
ôô 
new
ôô %
EntityNotFoundException
ôô 1
(
ôô1 2
PatientEntityName
ôô2 C
,
ôôC D
	patientId
ôôE N
)
ôôN O
;
ôôO P
}
öö 
string
úú 
patientName
úú 
=
úú  
dto
úú! $
.
úú$ %
FullName
úú% -
.
úú- .
Trim
úú. 2
(
úú2 3
)
úú3 4
.
úú4 5
ToLower
úú5 <
(
úú< =
)
úú= >
;
úú> ?
string
ùù 
email
ùù 
=
ùù 
dto
ùù 
.
ùù 
Email
ùù $
.
ùù$ %
Trim
ùù% )
(
ùù) *
)
ùù* +
.
ùù+ ,
ToLower
ùù, 3
(
ùù3 4
)
ùù4 5
;
ùù5 6
string
ûû 
phoneNumber
ûû 
=
ûû  
dto
ûû! $
.
ûû$ %
PhoneNumber
ûû% 0
.
ûû0 1
Trim
ûû1 5
(
ûû5 6
)
ûû6 7
;
ûû7 8
DateTime
üü 
dateOfBirth
üü  
=
üü! "
dto
üü# &
.
üü& '
DateOfBirth
üü' 2
.
üü2 3
Date
üü3 7
;
üü7 8
bool
°° 
	duplicate
°° 
=
°° 
await
°° "

repository
°°# -
.
°°- .%
IsDuplicatePatientAsync
°°. E
(
°°E F
patientName
¢¢ 
,
¢¢ 
email
££ 
,
££ 
phoneNumber
§§ 
,
§§ 
dateOfBirth
•• 
,
•• 
	patientId
¶¶ 
)
¶¶ 
;
¶¶ 
if
®® 
(
®® 
	duplicate
®® 
)
®® 
{
©© 
throw
™™ 
new
™™ 
ConflictException
™™ +
(
™™+ ,
$str
™™, b
)
™™b c
;
™™c d
}
´´ 
var
≠≠ 
patient
≠≠ 
=
≠≠ 
mapper
≠≠  
.
≠≠  !
Map
≠≠! $
<
≠≠$ %
Patient
≠≠% ,
>
≠≠, -
(
≠≠- .
dto
≠≠. 1
)
≠≠1 2
;
≠≠2 3
patient
ØØ 
.
ØØ 
	PatientId
ØØ 
=
ØØ 
	patientId
ØØ  )
;
ØØ) *
patient
∞∞ 
.
∞∞ 
DateOfBirth
∞∞ 
=
∞∞  !
dateOfBirth
∞∞" -
;
∞∞- .
patient
±± 
.
±± 
CreatedDate
±± 
=
±±  !
existingPatient
±±" 1
.
±±1 2
CreatedDate
±±2 =
;
±±= >
var
≥≥ 
updatedPatient
≥≥ 
=
≥≥  
await
≥≥! &

repository
≥≥' 1
.
≥≥1 2
UpdateAsync
≥≥2 =
(
≥≥= >
	patientId
≥≥> G
,
≥≥G H
patient
≥≥I P
)
≥≥P Q
;
≥≥Q R
if
µµ 
(
µµ 
updatedPatient
µµ 
is
µµ !
null
µµ" &
)
µµ& '
{
∂∂ 
throw
∑∑ 
new
∑∑ %
EntityNotFoundException
∑∑ 1
(
∑∑1 2
PatientEntityName
∑∑2 C
,
∑∑C D
	patientId
∑∑E N
)
∑∑N O
;
∑∑O P
}
∏∏ 
return
∫∫ 
mapper
∫∫ 
.
∫∫ 
Map
∫∫ 
<
∫∫ 

PatientDto
∫∫ (
>
∫∫( )
(
∫∫) *
updatedPatient
∫∫* 8
)
∫∫8 9
;
∫∫9 :
}
ªª 	
public
ΩΩ 
async
ΩΩ 
Task
ΩΩ 
<
ΩΩ 

PatientDto
ΩΩ $
>
ΩΩ$ %
GetMyProfileAsync
ΩΩ& 7
(
ΩΩ7 8
string
ΩΩ8 >
identityUserId
ΩΩ? M
)
ΩΩM N
{
ææ 	
if
øø 
(
øø 
string
øø 
.
øø  
IsNullOrWhiteSpace
øø )
(
øø) *
identityUserId
øø* 8
)
øø8 9
)
øø9 :
{
¿¿ 
throw
¡¡ 
new
¡¡ #
BusinessRuleException
¡¡ /
(
¡¡/ 0
$str
¡¡0 I
)
¡¡I J
;
¡¡J K
}
¬¬ 
var
ƒƒ 
patient
ƒƒ 
=
ƒƒ 
await
ƒƒ 

repository
ƒƒ  *
.
ƒƒ* +&
GetByIdentityUserIdAsync
ƒƒ+ C
(
ƒƒC D
identityUserId
ƒƒD R
)
ƒƒR S
;
ƒƒS T
if
∆∆ 
(
∆∆ 
patient
∆∆ 
is
∆∆ 
null
∆∆ 
)
∆∆  
{
«« 
throw
»» 
new
»» %
EntityNotFoundException
»» 1
(
»»1 2
$str
»»2 V
,
»»V W
$num
»»X Y
)
»»Y Z
;
»»Z [
}
…… 
return
ÀÀ 
mapper
ÀÀ 
.
ÀÀ 
Map
ÀÀ 
<
ÀÀ 

PatientDto
ÀÀ (
>
ÀÀ( )
(
ÀÀ) *
patient
ÀÀ* 1
)
ÀÀ1 2
;
ÀÀ2 3
}
ÃÃ 	
public
ŒŒ 
async
ŒŒ 
Task
ŒŒ 
<
ŒŒ 

PatientDto
ŒŒ $
>
ŒŒ$ %"
UpdateMyProfileAsync
ŒŒ& :
(
ŒŒ: ;
string
ŒŒ; A
identityUserId
ŒŒB P
,
ŒŒP Q
UpdatePatientDto
ŒŒR b
dto
ŒŒc f
)
ŒŒf g
{
œœ 	
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
––) *
identityUserId
––* 8
)
––8 9
)
––9 :
{
—— 
throw
““ 
new
““ #
BusinessRuleException
““ /
(
““/ 0
$str
““0 I
)
““I J
;
““J K
}
”” 
if
’’ 
(
’’ 
dto
’’ 
is
’’ 
null
’’ 
)
’’ 
{
÷÷ 
throw
◊◊ 
new
◊◊ #
BusinessRuleException
◊◊ /
(
◊◊/ 0
$str
◊◊0 O
)
◊◊O P
;
◊◊P Q
}
ÿÿ 
if
⁄⁄ 
(
⁄⁄ 
dto
⁄⁄ 
.
⁄⁄ 
DateOfBirth
⁄⁄ 
.
⁄⁄  
Date
⁄⁄  $
<
⁄⁄% & 
MinimumDateOfBirth
⁄⁄' 9
)
⁄⁄9 :
{
€€ 
throw
‹‹ 
new
‹‹ #
BusinessRuleException
‹‹ /
(
‹‹/ 0
$str
‹‹0 ]
)
‹‹] ^
;
‹‹^ _
}
›› 
if
ﬂﬂ 
(
ﬂﬂ 
dto
ﬂﬂ 
.
ﬂﬂ 
DateOfBirth
ﬂﬂ 
.
ﬂﬂ  
Date
ﬂﬂ  $
>
ﬂﬂ% &
DateTime
ﬂﬂ' /
.
ﬂﬂ/ 0
Today
ﬂﬂ0 5
)
ﬂﬂ5 6
{
‡‡ 
throw
·· 
new
·· #
BusinessRuleException
·· /
(
··/ 0
$str
··0 X
)
··X Y
;
··Y Z
}
‚‚ 
var
‰‰ 
existingPatient
‰‰ 
=
‰‰  !
await
‰‰" '

repository
‰‰( 2
.
‰‰2 3&
GetByIdentityUserIdAsync
‰‰3 K
(
‰‰K L
identityUserId
‰‰L Z
)
‰‰Z [
;
‰‰[ \
if
ÊÊ 
(
ÊÊ 
existingPatient
ÊÊ 
is
ÊÊ  "
null
ÊÊ# '
)
ÊÊ' (
{
ÁÁ 
throw
ËË 
new
ËË %
EntityNotFoundException
ËË 1
(
ËË1 2
$str
ËË2 V
,
ËËV W
$num
ËËX Y
)
ËËY Z
;
ËËZ [
}
ÈÈ 
var
ÎÎ 
patient
ÎÎ 
=
ÎÎ 
mapper
ÎÎ  
.
ÎÎ  !
Map
ÎÎ! $
<
ÎÎ$ %
Patient
ÎÎ% ,
>
ÎÎ, -
(
ÎÎ- .
dto
ÎÎ. 1
)
ÎÎ1 2
;
ÎÎ2 3
patient
ÌÌ 
.
ÌÌ 
	PatientId
ÌÌ 
=
ÌÌ 
existingPatient
ÌÌ  /
.
ÌÌ/ 0
	PatientId
ÌÌ0 9
;
ÌÌ9 :
patient
ÓÓ 
.
ÓÓ 
IdentityUserId
ÓÓ "
=
ÓÓ# $
existingPatient
ÓÓ% 4
.
ÓÓ4 5
IdentityUserId
ÓÓ5 C
;
ÓÓC D
patient
ÔÔ 
.
ÔÔ 
Email
ÔÔ 
=
ÔÔ 
existingPatient
ÔÔ +
.
ÔÔ+ ,
Email
ÔÔ, 1
;
ÔÔ1 2
patient
 
.
 
CreatedDate
 
=
  !
existingPatient
" 1
.
1 2
CreatedDate
2 =
;
= >
var
ÚÚ 
updatedPatient
ÚÚ 
=
ÚÚ  
await
ÚÚ! &

repository
ÚÚ' 1
.
ÚÚ1 2
UpdateAsync
ÚÚ2 =
(
ÚÚ= >
existingPatient
ÚÚ> M
.
ÚÚM N
	PatientId
ÚÚN W
,
ÚÚW X
patient
ÚÚY `
)
ÚÚ` a
;
ÚÚa b
if
ÙÙ 
(
ÙÙ 
updatedPatient
ÙÙ 
is
ÙÙ !
null
ÙÙ" &
)
ÙÙ& '
{
ıı 
throw
ˆˆ 
new
ˆˆ %
EntityNotFoundException
ˆˆ 1
(
ˆˆ1 2
PatientEntityName
ˆˆ2 C
,
ˆˆC D
existingPatient
ˆˆE T
.
ˆˆT U
	PatientId
ˆˆU ^
)
ˆˆ^ _
;
ˆˆ_ `
}
˜˜ 
return
˘˘ 
mapper
˘˘ 
.
˘˘ 
Map
˘˘ 
<
˘˘ 

PatientDto
˘˘ (
>
˘˘( )
(
˘˘) *
updatedPatient
˘˘* 8
)
˘˘8 9
;
˘˘9 :
}
˙˙ 	
private
¸¸ 
static
¸¸ 
void
¸¸ 
ValidatePatientId
¸¸ -
(
¸¸- .
int
¸¸. 1
	patientId
¸¸2 ;
)
¸¸; <
{
˝˝ 	
if
˛˛ 
(
˛˛ 
	patientId
˛˛ 
<=
˛˛ 
$num
˛˛ 
)
˛˛ 
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ #
BusinessRuleException
ÄÄ /
(
ÄÄ/ 0
$str
ÄÄ0 [
)
ÄÄ[ \
;
ÄÄ\ ]
}
ÅÅ 
}
ÇÇ 	
private
ÑÑ 
static
ÑÑ 
void
ÑÑ &
ValidateCreatePatientDto
ÑÑ 4
(
ÑÑ4 5
CreatePatientDto
ÑÑ5 E
dto
ÑÑF I
)
ÑÑI J
{
ÖÖ 	
if
ÜÜ 
(
ÜÜ 
dto
ÜÜ 
is
ÜÜ 
null
ÜÜ 
)
ÜÜ 
{
áá 
throw
àà 
new
àà #
BusinessRuleException
àà /
(
àà/ 0
$str
àà0 O
)
ààO P
;
ààP Q
}
ââ )
ValidatePatientCommonFields
ãã '
(
ãã' (
dto
åå 
.
åå 
FullName
åå 
,
åå 
dto
çç 
.
çç 
DateOfBirth
çç 
,
çç  
dto
éé 
.
éé 
Email
éé 
,
éé 
dto
èè 
.
èè 
PhoneNumber
èè 
)
èè  
;
èè  !
}
êê 	
private
íí 
static
íí 
void
íí &
ValidateUpdatePatientDto
íí 4
(
íí4 5
UpdatePatientDto
íí5 E
dto
ííF I
)
ííI J
{
ìì 	
if
îî 
(
îî 
dto
îî 
is
îî 
null
îî 
)
îî 
{
ïï 
throw
ññ 
new
ññ #
BusinessRuleException
ññ /
(
ññ/ 0
$str
ññ0 O
)
ññO P
;
ññP Q
}
óó )
ValidatePatientCommonFields
ôô '
(
ôô' (
dto
öö 
.
öö 
FullName
öö 
,
öö 
dto
õõ 
.
õõ 
DateOfBirth
õõ 
,
õõ  
dto
úú 
.
úú 
Email
úú 
,
úú 
dto
ùù 
.
ùù 
PhoneNumber
ùù 
)
ùù  
;
ùù  !
}
ûû 	
private
†† 
static
†† 
void
†† )
ValidatePatientCommonFields
†† 7
(
††7 8
string
°° 
patientName
°° 
,
°° 
DateTime
¢¢ 
dateOfBirth
¢¢  
,
¢¢  !
string
££ 
email
££ 
,
££ 
string
§§ 
phoneNumber
§§ 
)
§§ 
{
•• 	
if
¶¶ 
(
¶¶ 
string
¶¶ 
.
¶¶  
IsNullOrWhiteSpace
¶¶ )
(
¶¶) *
patientName
¶¶* 5
)
¶¶5 6
)
¶¶6 7
{
ßß 
throw
®® 
new
®® #
BusinessRuleException
®® /
(
®®/ 0
$str
®®0 K
)
®®K L
;
®®L M
}
©© 
if
´´ 
(
´´ 
dateOfBirth
´´ 
.
´´ 
Date
´´  
<
´´! " 
MinimumDateOfBirth
´´# 5
)
´´5 6
{
¨¨ 
throw
≠≠ 
new
≠≠ #
BusinessRuleException
≠≠ /
(
≠≠/ 0
$str
≠≠0 ]
)
≠≠] ^
;
≠≠^ _
}
ÆÆ 
if
∞∞ 
(
∞∞ 
dateOfBirth
∞∞ 
.
∞∞ 
Date
∞∞  
>
∞∞! "
DateTime
∞∞# +
.
∞∞+ ,
Today
∞∞, 1
)
∞∞1 2
{
±± 
throw
≤≤ 
new
≤≤ #
BusinessRuleException
≤≤ /
(
≤≤/ 0
$str
≤≤0 X
)
≤≤X Y
;
≤≤Y Z
}
≥≥ 
if
µµ 
(
µµ 
string
µµ 
.
µµ  
IsNullOrWhiteSpace
µµ )
(
µµ) *
email
µµ* /
)
µµ/ 0
)
µµ0 1
{
∂∂ 
throw
∑∑ 
new
∑∑ #
BusinessRuleException
∑∑ /
(
∑∑/ 0
$str
∑∑0 L
)
∑∑L M
;
∑∑M N
}
∏∏ 
if
∫∫ 
(
∫∫ 
string
∫∫ 
.
∫∫  
IsNullOrWhiteSpace
∫∫ )
(
∫∫) *
phoneNumber
∫∫* 5
)
∫∫5 6
)
∫∫6 7
{
ªª 
throw
ºº 
new
ºº #
BusinessRuleException
ºº /
(
ºº/ 0
$str
ºº0 K
)
ººK L
;
ººL M
}
ΩΩ 
}
ææ 	
}
øø 
}¿¿ ›Ë
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\HealthRecordService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{		 
public

 

class

 
HealthRecordService

 $
(

$ %#
IHealthRecordRepository "
healthRecordRepository  6
,6 7
IPatientRepository 
patientRepository ,
,, -
IDoctorRepository 
doctorRepository *
,* +"
IAppointmentRepository !
appointmentRepository 4
,4 5
IMapper 
mapper 
) 
:  
IHealthRecordService .
{ 
private 
const 
string "
HealthRecordEntityName 3
=4 5
$str6 D
;D E
private 
const 
string 
PatientEntityName .
=/ 0
$str1 :
;: ;
private 
const 
string 
DoctorEntityName -
=. /
$str0 8
;8 9
private 
const 
string !
AppointmentEntityName 2
=3 4
$str5 B
;B C
private 
const 
string .
"HealthRecordDetailsRequiredMessage ?
=@ A
$strB g
;g h
public 
async 
Task 
< 
List 
< 
HealthRecordDto .
>. /
>/ 0$
GetAllHealthRecordsAsync1 I
(I J
)J K
{ 	
var 
healthRecords 
= 
await  %"
healthRecordRepository& <
.< =
GetAllAsync= H
(H I
)I J
;J K
return 
mapper 
. 
Map 
< 
List "
<" #
HealthRecordDto# 2
>2 3
>3 4
(4 5
healthRecords5 B
)B C
;C D
} 	
public 
async 
Task 
< 
HealthRecordDto )
>) *$
GetHealthRecordByIdAsync+ C
(C D
intD G
healthRecordIdH V
)V W
{ 	"
ValidateHealthRecordId   "
(  " #
healthRecordId  # 1
)  1 2
;  2 3
var"" 
healthRecord"" 
="" 
await"" $"
healthRecordRepository""% ;
.""; <
GetByIdAsync""< H
(""H I
healthRecordId""I W
)""W X
;""X Y
if$$ 
($$ 
healthRecord$$ 
is$$ 
null$$  $
)$$$ %
{%% 
throw&& 
new&& #
EntityNotFoundException&& 1
(&&1 2"
HealthRecordEntityName&&2 H
,&&H I
healthRecordId&&J X
)&&X Y
;&&Y Z
}'' 
return)) 
mapper)) 
.)) 
Map)) 
<)) 
HealthRecordDto)) -
>))- .
()). /
healthRecord))/ ;
))); <
;))< =
}** 	
public,, 
async,, 
Task,, 
<,, 
List,, 
<,, 
HealthRecordDto,, .
>,,. /
>,,/ 0,
 GetHealthRecordsByPatientIdAsync,,1 Q
(,,Q R
int,,R U
	patientId,,V _
),,_ `
{-- 	
await.. &
ValidatePatientExistsAsync.. ,
(.., -
	patientId..- 6
)..6 7
;..7 8
var00 
healthRecords00 
=00 
await00  %"
healthRecordRepository00& <
.00< =
GetByPatientIdAsync00= P
(00P Q
	patientId00Q Z
)00Z [
;00[ \
return22 
mapper22 
.22 
Map22 
<22 
List22 "
<22" #
HealthRecordDto22# 2
>222 3
>223 4
(224 5
healthRecords225 B
)22B C
;22C D
}33 	
public55 
async55 
Task55 
<55 
List55 
<55 
HealthRecordDto55 .
>55. /
>55/ 0+
GetHealthRecordsByDoctorIdAsync551 P
(55P Q
int55Q T
doctorId55U ]
)55] ^
{66 	
await77 %
ValidateDoctorExistsAsync77 +
(77+ ,
doctorId77, 4
)774 5
;775 6
var99 
healthRecords99 
=99 
await99  %"
healthRecordRepository99& <
.99< =
GetByDoctorIdAsync99= O
(99O P
doctorId99P X
)99X Y
;99Y Z
return;; 
mapper;; 
.;; 
Map;; 
<;; 
List;; "
<;;" #
HealthRecordDto;;# 2
>;;2 3
>;;3 4
(;;4 5
healthRecords;;5 B
);;B C
;;;C D
}<< 	
public>> 
async>> 
Task>> 
<>> 
List>> 
<>> 
HealthRecordDto>> .
>>>. /
>>>/ 00
$GetHealthRecordsByAppointmentIdAsync>>1 U
(>>U V
int>>V Y
appointmentId>>Z g
)>>g h
{?? 	
await@@ )
GetAppointmentEntityByIdAsync@@ /
(@@/ 0
appointmentId@@0 =
)@@= >
;@@> ?
varBB 
healthRecordsBB 
=BB 
awaitBB  %"
healthRecordRepositoryBB& <
.BB< =#
GetByAppointmentIdAsyncBB= T
(BBT U
appointmentIdBBU b
)BBb c
;BBc d
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
(DD4 5
healthRecordsDD5 B
)DDB C
;DDC D
}EE 	
publicGG 
asyncGG 
TaskGG 
<GG 
HealthRecordDtoGG )
>GG) * 
AddHealthRecordAsyncGG+ ?
(GG? @
AddHealthRecordDtoGG@ R
dtoGGS V
)GGV W
{HH 	
ifII 
(II 
dtoII 
isII 
nullII 
)II 
{JJ 
throwKK 
newKK %
HealthRecordRuleExceptionKK 3
(KK3 4.
"HealthRecordDetailsRequiredMessageKK4 V
)KKV W
;KKW X
}LL 
ValidatePatientIdNN 
(NN 
dtoNN !
.NN! "
	PatientIdNN" +
)NN+ ,
;NN, -!
ValidateAppointmentIdOO !
(OO! "
dtoOO" %
.OO% &
AppointmentIdOO& 3
)OO3 4
;OO4 5
varQQ 
appointmentQQ 
=QQ 
awaitQQ #9
-ValidateAndGetAppointmentForHealthRecordAsyncQQ$ Q
(QQQ R
dtoQQR U
)QQU V
;QQV W6
*ValidateAppointmentForHealthRecordCreationSS 6
(SS6 7
appointmentSS7 B
,SSB C
dtoSSD G
.SSG H
	VisitDateSSH Q
)SSQ R
;SSR S
varUU 
healthRecordExistsUU "
=UU# $
awaitUU% *"
healthRecordRepositoryUU+ A
.UUA B&
ExistsByAppointmentIdAsyncUUB \
(UU\ ]
dtoUU] `
.UU` a
AppointmentIdUUa n
)UUn o
;UUo p
ifWW 
(WW 
healthRecordExistsWW "
)WW" #
{XX 
throwYY 
newYY 
ConflictExceptionYY +
(YY+ ,
$strYY, `
)YY` a
;YYa b
}ZZ $
ValidateHealthRecordText\\ $
(\\$ %
dto\\% (
.\\( )
	Diagnosis\\) 2
,\\2 3
dto\\4 7
.\\7 8
Prescription\\8 D
,\\D E
dto\\F I
.\\I J
Notes\\J O
)\\O P
;\\P Q
var^^ 
healthRecord^^ 
=^^ 
mapper^^ %
.^^% &
Map^^& )
<^^) *
HealthRecord^^* 6
>^^6 7
(^^7 8
dto^^8 ;
)^^; <
;^^< =
healthRecord`` 
.`` 
	PatientId`` "
=``# $
appointment``% 0
.``0 1
	PatientId``1 :
;``: ;
healthRecordaa 
.aa 
DoctorIdaa !
=aa" #
appointmentaa$ /
.aa/ 0
DoctorIdaa0 8
;aa8 9
healthRecordbb 
.bb 
AppointmentIdbb &
=bb' (
appointmentbb) 4
.bb4 5
AppointmentIdbb5 B
;bbB C
healthRecordcc 
.cc 
CreatedDatecc $
=cc% &
DateTimecc' /
.cc/ 0
Nowcc0 3
;cc3 4
varee 
savedHealthRecordee !
=ee" #
awaitee$ )"
healthRecordRepositoryee* @
.ee@ A
CreateAsynceeA L
(eeL M
healthRecordeeM Y
)eeY Z
;eeZ [
appointmentgg 
.gg 
Statusgg 
=gg  
AppointmentStatusgg! 2
.gg2 3
	Completedgg3 <
;gg< =
awaitii !
appointmentRepositoryii '
.ii' (
UpdateAsyncii( 3
(ii3 4
appointmentii4 ?
.ii? @
AppointmentIdii@ M
,iiM N
appointmentiiO Z
)iiZ [
;ii[ \
returnkk 
mapperkk 
.kk 
Mapkk 
<kk 
HealthRecordDtokk -
>kk- .
(kk. /
savedHealthRecordkk/ @
)kk@ A
;kkA B
}ll 	
publicnn 
asyncnn 
Tasknn 
<nn 
HealthRecordDtonn )
>nn) *#
UpdateHealthRecordAsyncnn+ B
(nnB C
intnnC F
healthRecordIdnnG U
,nnU V!
UpdateHealthRecordDtonnW l
dtonnm p
)nnp q
{oo 	"
ValidateHealthRecordIdpp "
(pp" #
healthRecordIdpp# 1
)pp1 2
;pp2 3
ifrr 
(rr 
dtorr 
isrr 
nullrr 
)rr 
{ss 
throwtt 
newtt %
HealthRecordRuleExceptiontt 3
(tt3 4.
"HealthRecordDetailsRequiredMessagett4 V
)ttV W
;ttW X
}uu 
varww  
existingHealthRecordww $
=ww% &
awaitww' ,"
healthRecordRepositoryww- C
.wwC D
GetByIdAsyncwwD P
(wwP Q
healthRecordIdwwQ _
)ww_ `
;ww` a
ifyy 
(yy  
existingHealthRecordyy $
isyy% '
nullyy( ,
)yy, -
{zz 
throw{{ 
new{{ #
EntityNotFoundException{{ 1
({{1 2"
HealthRecordEntityName{{2 H
,{{H I
healthRecordId{{J X
){{X Y
;{{Y Z
}|| 
var~~ 
appointment~~ 
=~~ 
await~~ #!
appointmentRepository~~$ 9
.~~9 :
GetByIdAsync~~: F
(~~F G 
existingHealthRecord~~G [
.~~[ \
AppointmentId~~\ i
)~~i j
;~~j k
if
ÄÄ 
(
ÄÄ 
appointment
ÄÄ 
is
ÄÄ 
null
ÄÄ #
)
ÄÄ# $
{
ÅÅ 
throw
ÇÇ 
new
ÇÇ %
EntityNotFoundException
ÇÇ 1
(
ÇÇ1 2#
AppointmentEntityName
ÇÇ2 G
,
ÇÇG H"
existingHealthRecord
ÇÇI ]
.
ÇÇ] ^
AppointmentId
ÇÇ^ k
)
ÇÇk l
;
ÇÇl m
}
ÉÉ 
if
ÖÖ 
(
ÖÖ 
appointment
ÖÖ 
.
ÖÖ 
ScheduledDate
ÖÖ )
.
ÖÖ) *
Date
ÖÖ* .
>
ÖÖ/ 0
DateTime
ÖÖ1 9
.
ÖÖ9 :
Today
ÖÖ: ?
)
ÖÖ? @
{
ÜÜ 
throw
áá 
new
áá '
HealthRecordRuleException
áá 3
(
áá3 4
$str
áá4 r
)
áár s
;
áás t
}
àà 
if
ää 
(
ää 
dto
ää 
.
ää 
	VisitDate
ää 
.
ää 
Date
ää "
!=
ää# %
appointment
ää& 1
.
ää1 2
ScheduledDate
ää2 ?
.
ää? @
Date
ää@ D
)
ääD E
{
ãã 
throw
åå 
new
åå '
HealthRecordRuleException
åå 3
(
åå3 4
$str
åå4 k
)
ååk l
;
åål m
}
çç &
ValidateHealthRecordText
èè $
(
èè$ %
dto
èè% (
.
èè( )
	Diagnosis
èè) 2
,
èè2 3
dto
èè4 7
.
èè7 8
Prescription
èè8 D
,
èèD E
dto
èèF I
.
èèI J
Notes
èèJ O
)
èèO P
;
èèP Q
mapper
ëë 
.
ëë 
Map
ëë 
(
ëë 
dto
ëë 
,
ëë "
existingHealthRecord
ëë 0
)
ëë0 1
;
ëë1 2"
existingHealthRecord
ìì  
.
ìì  !
HealthRecordId
ìì! /
=
ìì0 1
healthRecordId
ìì2 @
;
ìì@ A
var
ïï !
updatedHealthRecord
ïï #
=
ïï$ %
await
ïï& +$
healthRecordRepository
ïï, B
.
ïïB C
UpdateAsync
ïïC N
(
ïïN O
healthRecordId
ññ 
,
ññ "
existingHealthRecord
óó $
)
óó$ %
;
óó% &
if
ôô 
(
ôô !
updatedHealthRecord
ôô #
is
ôô$ &
null
ôô' +
)
ôô+ ,
{
öö 
throw
õõ 
new
õõ %
EntityNotFoundException
õõ 1
(
õõ1 2$
HealthRecordEntityName
õõ2 H
,
õõH I
healthRecordId
õõJ X
)
õõX Y
;
õõY Z
}
úú 
return
ûû 
mapper
ûû 
.
ûû 
Map
ûû 
<
ûû 
HealthRecordDto
ûû -
>
ûû- .
(
ûû. /!
updatedHealthRecord
ûû/ B
)
ûûB C
;
ûûC D
}
üü 	
public
°° 
async
°° 
Task
°° 
<
°° 
HealthRecordDto
°° )
>
°°) *%
DeleteHealthRecordAsync
°°+ B
(
°°B C
int
°°C F
healthRecordId
°°G U
)
°°U V
{
¢¢ 	$
ValidateHealthRecordId
££ "
(
££" #
healthRecordId
££# 1
)
££1 2
;
££2 3
var
•• !
deletedHealthRecord
•• #
=
••$ %
await
••& +$
healthRecordRepository
••, B
.
••B C
DeleteAsync
••C N
(
••N O
healthRecordId
••O ]
)
••] ^
;
••^ _
if
ßß 
(
ßß !
deletedHealthRecord
ßß #
is
ßß$ &
null
ßß' +
)
ßß+ ,
{
®® 
throw
©© 
new
©© %
EntityNotFoundException
©© 1
(
©©1 2$
HealthRecordEntityName
©©2 H
,
©©H I
healthRecordId
©©J X
)
©©X Y
;
©©Y Z
}
™™ 
return
¨¨ 
mapper
¨¨ 
.
¨¨ 
Map
¨¨ 
<
¨¨ 
HealthRecordDto
¨¨ -
>
¨¨- .
(
¨¨. /!
deletedHealthRecord
¨¨/ B
)
¨¨B C
;
¨¨C D
}
≠≠ 	
public
ØØ 
async
ØØ 
Task
ØØ 
<
ØØ 
List
ØØ 
<
ØØ 
HealthRecordDto
ØØ .
>
ØØ. /
>
ØØ/ 0/
!GetMyHealthRecordsForPatientAsync
ØØ1 R
(
ØØR S
string
ØØS Y
identityUserId
ØØZ h
)
ØØh i
{
∞∞ 	
var
±± 
patient
±± 
=
±± 
await
±± %
GetLoggedInPatientAsync
±±  7
(
±±7 8
identityUserId
±±8 F
)
±±F G
;
±±G H
var
≥≥ 
healthRecords
≥≥ 
=
≥≥ 
await
≥≥  %$
healthRecordRepository
≥≥& <
.
≥≥< =!
GetByPatientIdAsync
≥≥= P
(
≥≥P Q
patient
≥≥Q X
.
≥≥X Y
	PatientId
≥≥Y b
)
≥≥b c
;
≥≥c d
return
µµ 
mapper
µµ 
.
µµ 
Map
µµ 
<
µµ 
List
µµ "
<
µµ" #
HealthRecordDto
µµ# 2
>
µµ2 3
>
µµ3 4
(
µµ4 5
healthRecords
µµ5 B
)
µµB C
;
µµC D
}
∂∂ 	
public
∏∏ 
async
∏∏ 
Task
∏∏ 
<
∏∏ 
HealthRecordDto
∏∏ )
>
∏∏) *0
"GetHealthRecordByIdForPatientAsync
∏∏+ M
(
∏∏M N
int
ππ 
healthRecordId
ππ 
,
ππ 
string
∫∫ 
identityUserId
∫∫ !
)
∫∫! "
{
ªª 	$
ValidateHealthRecordId
ºº "
(
ºº" #
healthRecordId
ºº# 1
)
ºº1 2
;
ºº2 3
var
ææ 
patient
ææ 
=
ææ 
await
ææ %
GetLoggedInPatientAsync
ææ  7
(
ææ7 8
identityUserId
ææ8 F
)
ææF G
;
ææG H
var
¿¿ 
healthRecord
¿¿ 
=
¿¿ 
await
¿¿ $$
healthRecordRepository
¿¿% ;
.
¿¿; <
GetByIdAsync
¿¿< H
(
¿¿H I
healthRecordId
¿¿I W
)
¿¿W X
;
¿¿X Y
if
¬¬ 
(
¬¬ 
healthRecord
¬¬ 
is
¬¬ 
null
¬¬  $
)
¬¬$ %
{
√√ 
throw
ƒƒ 
new
ƒƒ %
EntityNotFoundException
ƒƒ 1
(
ƒƒ1 2$
HealthRecordEntityName
ƒƒ2 H
,
ƒƒH I
healthRecordId
ƒƒJ X
)
ƒƒX Y
;
ƒƒY Z
}
≈≈ 
if
«« 
(
«« 
healthRecord
«« 
.
«« 
	PatientId
«« &
!=
««' )
patient
««* 1
.
««1 2
	PatientId
««2 ;
)
««; <
{
»» 
throw
…… 
new
…… &
ForbiddenAccessException
…… 2
(
……2 3
$str
……3 g
)
……g h
;
……h i
}
   
return
ÃÃ 
mapper
ÃÃ 
.
ÃÃ 
Map
ÃÃ 
<
ÃÃ 
HealthRecordDto
ÃÃ -
>
ÃÃ- .
(
ÃÃ. /
healthRecord
ÃÃ/ ;
)
ÃÃ; <
;
ÃÃ< =
}
ÕÕ 	
public
œœ 
async
œœ 
Task
œœ 
<
œœ 
List
œœ 
<
œœ 
HealthRecordDto
œœ .
>
œœ. /
>
œœ/ 0.
 GetMyHealthRecordsForDoctorAsync
œœ1 Q
(
œœQ R
string
œœR X
identityUserId
œœY g
)
œœg h
{
–– 	
var
—— 
doctor
—— 
=
—— 
await
—— $
GetLoggedInDoctorAsync
—— 5
(
——5 6
identityUserId
——6 D
)
——D E
;
——E F
var
”” 
healthRecords
”” 
=
”” 
await
””  %$
healthRecordRepository
””& <
.
””< = 
GetByDoctorIdAsync
””= O
(
””O P
doctor
””P V
.
””V W
DoctorId
””W _
)
””_ `
;
””` a
return
’’ 
mapper
’’ 
.
’’ 
Map
’’ 
<
’’ 
List
’’ "
<
’’" #
HealthRecordDto
’’# 2
>
’’2 3
>
’’3 4
(
’’4 5
healthRecords
’’5 B
)
’’B C
;
’’C D
}
÷÷ 	
public
ÿÿ 
async
ÿÿ 
Task
ÿÿ 
<
ÿÿ 
HealthRecordDto
ÿÿ )
>
ÿÿ) */
!GetHealthRecordByIdForDoctorAsync
ÿÿ+ L
(
ÿÿL M
int
ŸŸ 
healthRecordId
ŸŸ 
,
ŸŸ 
string
⁄⁄ 
identityUserId
⁄⁄ !
)
⁄⁄! "
{
€€ 	$
ValidateHealthRecordId
‹‹ "
(
‹‹" #
healthRecordId
‹‹# 1
)
‹‹1 2
;
‹‹2 3
var
ﬁﬁ 
doctor
ﬁﬁ 
=
ﬁﬁ 
await
ﬁﬁ $
GetLoggedInDoctorAsync
ﬁﬁ 5
(
ﬁﬁ5 6
identityUserId
ﬁﬁ6 D
)
ﬁﬁD E
;
ﬁﬁE F
var
‡‡ 
healthRecord
‡‡ 
=
‡‡ 
await
‡‡ $$
healthRecordRepository
‡‡% ;
.
‡‡; <
GetByIdAsync
‡‡< H
(
‡‡H I
healthRecordId
‡‡I W
)
‡‡W X
;
‡‡X Y
if
‚‚ 
(
‚‚ 
healthRecord
‚‚ 
is
‚‚ 
null
‚‚  $
)
‚‚$ %
{
„„ 
throw
‰‰ 
new
‰‰ %
EntityNotFoundException
‰‰ 1
(
‰‰1 2$
HealthRecordEntityName
‰‰2 H
,
‰‰H I
healthRecordId
‰‰J X
)
‰‰X Y
;
‰‰Y Z
}
ÂÂ 
if
ÁÁ 
(
ÁÁ 
healthRecord
ÁÁ 
.
ÁÁ 
DoctorId
ÁÁ %
!=
ÁÁ& (
doctor
ÁÁ) /
.
ÁÁ/ 0
DoctorId
ÁÁ0 8
)
ÁÁ8 9
{
ËË 
throw
ÈÈ 
new
ÈÈ &
ForbiddenAccessException
ÈÈ 2
(
ÈÈ2 3
$str
ÈÈ3 f
)
ÈÈf g
;
ÈÈg h
}
ÍÍ 
return
ÏÏ 
mapper
ÏÏ 
.
ÏÏ 
Map
ÏÏ 
<
ÏÏ 
HealthRecordDto
ÏÏ -
>
ÏÏ- .
(
ÏÏ. /
healthRecord
ÏÏ/ ;
)
ÏÏ; <
;
ÏÏ< =
}
ÌÌ 	
public
ÔÔ 
async
ÔÔ 
Task
ÔÔ 
<
ÔÔ 
List
ÔÔ 
<
ÔÔ 
HealthRecordDto
ÔÔ .
>
ÔÔ. /
>
ÔÔ/ 0;
-GetHealthRecordsByAppointmentIdForDoctorAsync
ÔÔ1 ^
(
ÔÔ^ _
int
 
appointmentId
 
,
 
string
ÒÒ 
identityUserId
ÒÒ !
)
ÒÒ! "
{
ÚÚ 	#
ValidateAppointmentId
ÛÛ !
(
ÛÛ! "
appointmentId
ÛÛ" /
)
ÛÛ/ 0
;
ÛÛ0 1
var
ıı 
doctor
ıı 
=
ıı 
await
ıı $
GetLoggedInDoctorAsync
ıı 5
(
ıı5 6
identityUserId
ıı6 D
)
ııD E
;
ııE F
var
˜˜ 
appointment
˜˜ 
=
˜˜ 
await
˜˜ ##
appointmentRepository
˜˜$ 9
.
˜˜9 :
GetByIdAsync
˜˜: F
(
˜˜F G
appointmentId
˜˜G T
)
˜˜T U
;
˜˜U V
if
˘˘ 
(
˘˘ 
appointment
˘˘ 
is
˘˘ 
null
˘˘ #
)
˘˘# $
{
˙˙ 
throw
˚˚ 
new
˚˚ %
EntityNotFoundException
˚˚ 1
(
˚˚1 2#
AppointmentEntityName
˚˚2 G
,
˚˚G H
appointmentId
˚˚I V
)
˚˚V W
;
˚˚W X
}
¸¸ 
if
˛˛ 
(
˛˛ 
appointment
˛˛ 
.
˛˛ 
DoctorId
˛˛ $
!=
˛˛% '
doctor
˛˛( .
.
˛˛. /
DoctorId
˛˛/ 7
)
˛˛7 8
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ &
ForbiddenAccessException
ÄÄ 2
(
ÄÄ2 3
$str
ÄÄ3 w
)
ÄÄw x
;
ÄÄx y
}
ÅÅ 
var
ÉÉ 
healthRecords
ÉÉ 
=
ÉÉ 
await
ÉÉ  %$
healthRecordRepository
ÉÉ& <
.
ÉÉ< =%
GetByAppointmentIdAsync
ÉÉ= T
(
ÉÉT U
appointmentId
ÉÉU b
)
ÉÉb c
;
ÉÉc d
return
ÖÖ 
mapper
ÖÖ 
.
ÖÖ 
Map
ÖÖ 
<
ÖÖ 
List
ÖÖ "
<
ÖÖ" #
HealthRecordDto
ÖÖ# 2
>
ÖÖ2 3
>
ÖÖ3 4
(
ÖÖ4 5
healthRecords
ÖÖ5 B
)
ÖÖB C
;
ÖÖC D
}
ÜÜ 	
public
àà 
async
àà 
Task
àà 
<
àà 
HealthRecordDto
àà )
>
àà) *+
AddHealthRecordForDoctorAsync
àà+ H
(
ààH I 
AddHealthRecordDto
ââ 
dto
ââ "
,
ââ" #
string
ää 
identityUserId
ää !
)
ää! "
{
ãã 	
if
åå 
(
åå 
dto
åå 
is
åå 
null
åå 
)
åå 
{
çç 
throw
éé 
new
éé '
HealthRecordRuleException
éé 3
(
éé3 40
"HealthRecordDetailsRequiredMessage
éé4 V
)
ééV W
;
ééW X
}
èè 
var
ëë 
doctor
ëë 
=
ëë 
await
ëë $
GetLoggedInDoctorAsync
ëë 5
(
ëë5 6
identityUserId
ëë6 D
)
ëëD E
;
ëëE F#
ValidateAppointmentId
ìì !
(
ìì! "
dto
ìì" %
.
ìì% &
AppointmentId
ìì& 3
)
ìì3 4
;
ìì4 5
var
ïï 
appointment
ïï 
=
ïï 
await
ïï ##
appointmentRepository
ïï$ 9
.
ïï9 :
GetByIdAsync
ïï: F
(
ïïF G
dto
ïïG J
.
ïïJ K
AppointmentId
ïïK X
)
ïïX Y
;
ïïY Z
if
óó 
(
óó 
appointment
óó 
is
óó 
null
óó #
)
óó# $
{
òò 
throw
ôô 
new
ôô %
EntityNotFoundException
ôô 1
(
ôô1 2#
AppointmentEntityName
ôô2 G
,
ôôG H
dto
ôôI L
.
ôôL M
AppointmentId
ôôM Z
)
ôôZ [
;
ôô[ \
}
öö 
if
úú 
(
úú 
appointment
úú 
.
úú 
DoctorId
úú $
!=
úú% '
doctor
úú( .
.
úú. /
DoctorId
úú/ 7
)
úú7 8
{
ùù 
throw
ûû 
new
ûû &
ForbiddenAccessException
ûû 2
(
ûû2 3
$str
ûû3 t
)
ûût u
;
ûûu v
}
üü 
if
°° 
(
°° 
dto
°° 
.
°° 
	PatientId
°° 
!=
°°  
appointment
°°! ,
.
°°, -
	PatientId
°°- 6
)
°°6 7
{
¢¢ 
throw
££ 
new
££ '
HealthRecordRuleException
££ 3
(
££3 4
$str
££4 o
)
££o p
;
££p q
}
§§ 
if
¶¶ 
(
¶¶ 
dto
¶¶ 
.
¶¶ 
DoctorId
¶¶ 
is
¶¶ 
not
¶¶  #
null
¶¶$ (
&&
¶¶) +
dto
¶¶, /
.
¶¶/ 0
DoctorId
¶¶0 8
.
¶¶8 9
Value
¶¶9 >
!=
¶¶? A
doctor
¶¶B H
.
¶¶H I
DoctorId
¶¶I Q
)
¶¶Q R
{
ßß 
throw
®® 
new
®® &
ForbiddenAccessException
®® 2
(
®®2 3
$str
®®3 j
)
®®j k
;
®®k l
}
©© 
dto
´´ 
.
´´ 
DoctorId
´´ 
=
´´ 
doctor
´´ !
.
´´! "
DoctorId
´´" *
;
´´* +
return
≠≠ 
await
≠≠ "
AddHealthRecordAsync
≠≠ -
(
≠≠- .
dto
≠≠. 1
)
≠≠1 2
;
≠≠2 3
}
ÆÆ 	
public
∞∞ 
async
∞∞ 
Task
∞∞ 
<
∞∞ 
HealthRecordDto
∞∞ )
>
∞∞) *.
 UpdateHealthRecordForDoctorAsync
∞∞+ K
(
∞∞K L
int
±± 
healthRecordId
±± 
,
±± #
UpdateHealthRecordDto
≤≤ !
dto
≤≤" %
,
≤≤% &
string
≥≥ 
identityUserId
≥≥ !
)
≥≥! "
{
¥¥ 	$
ValidateHealthRecordId
µµ "
(
µµ" #
healthRecordId
µµ# 1
)
µµ1 2
;
µµ2 3
if
∑∑ 
(
∑∑ 
dto
∑∑ 
is
∑∑ 
null
∑∑ 
)
∑∑ 
{
∏∏ 
throw
ππ 
new
ππ '
HealthRecordRuleException
ππ 3
(
ππ3 40
"HealthRecordDetailsRequiredMessage
ππ4 V
)
ππV W
;
ππW X
}
∫∫ 
var
ºº 
doctor
ºº 
=
ºº 
await
ºº $
GetLoggedInDoctorAsync
ºº 5
(
ºº5 6
identityUserId
ºº6 D
)
ººD E
;
ººE F
var
ææ 
healthRecord
ææ 
=
ææ 
await
ææ $$
healthRecordRepository
ææ% ;
.
ææ; <
GetByIdAsync
ææ< H
(
ææH I
healthRecordId
ææI W
)
ææW X
;
ææX Y
if
¿¿ 
(
¿¿ 
healthRecord
¿¿ 
is
¿¿ 
null
¿¿  $
)
¿¿$ %
{
¡¡ 
throw
¬¬ 
new
¬¬ %
EntityNotFoundException
¬¬ 1
(
¬¬1 2$
HealthRecordEntityName
¬¬2 H
,
¬¬H I
healthRecordId
¬¬J X
)
¬¬X Y
;
¬¬Y Z
}
√√ 
if
≈≈ 
(
≈≈ 
healthRecord
≈≈ 
.
≈≈ 
DoctorId
≈≈ %
!=
≈≈& (
doctor
≈≈) /
.
≈≈/ 0
DoctorId
≈≈0 8
)
≈≈8 9
{
∆∆ 
throw
«« 
new
«« &
ForbiddenAccessException
«« 2
(
««2 3
$str
««3 f
)
««f g
;
««g h
}
»» 
return
   
await
   %
UpdateHealthRecordAsync
   0
(
  0 1
healthRecordId
  1 ?
,
  ? @
dto
  A D
)
  D E
;
  E F
}
ÀÀ 	
private
ÕÕ 
async
ÕÕ 
Task
ÕÕ 
<
ÕÕ 
Appointment
ÕÕ &
>
ÕÕ& ';
-ValidateAndGetAppointmentForHealthRecordAsync
ÕÕ( U
(
ÕÕU V 
AddHealthRecordDto
ÕÕV h
dto
ÕÕi l
)
ÕÕl m
{
ŒŒ 	
var
œœ 
patient
œœ 
=
œœ 
await
œœ 
patientRepository
œœ  1
.
œœ1 2
GetByIdAsync
œœ2 >
(
œœ> ?
dto
œœ? B
.
œœB C
	PatientId
œœC L
)
œœL M
;
œœM N
if
—— 
(
—— 
patient
—— 
is
—— 
null
—— 
)
——  
{
““ 
throw
”” 
new
”” %
EntityNotFoundException
”” 1
(
””1 2
PatientEntityName
””2 C
,
””C D
dto
””E H
.
””H I
	PatientId
””I R
)
””R S
;
””S T
}
‘‘ 
var
÷÷ 
appointment
÷÷ 
=
÷÷ 
await
÷÷ ##
appointmentRepository
÷÷$ 9
.
÷÷9 :
GetByIdAsync
÷÷: F
(
÷÷F G
dto
÷÷G J
.
÷÷J K
AppointmentId
÷÷K X
)
÷÷X Y
;
÷÷Y Z
if
ÿÿ 
(
ÿÿ 
appointment
ÿÿ 
is
ÿÿ 
null
ÿÿ #
)
ÿÿ# $
{
ŸŸ 
throw
⁄⁄ 
new
⁄⁄ %
EntityNotFoundException
⁄⁄ 1
(
⁄⁄1 2#
AppointmentEntityName
⁄⁄2 G
,
⁄⁄G H
dto
⁄⁄I L
.
⁄⁄L M
AppointmentId
⁄⁄M Z
)
⁄⁄Z [
;
⁄⁄[ \
}
€€ 
if
›› 
(
›› 
appointment
›› 
.
›› 
	PatientId
›› %
!=
››& (
dto
››) ,
.
››, -
	PatientId
››- 6
)
››6 7
{
ﬁﬁ 
throw
ﬂﬂ 
new
ﬂﬂ '
HealthRecordRuleException
ﬂﬂ 3
(
ﬂﬂ3 4
$str
ﬂﬂ4 j
)
ﬂﬂj k
;
ﬂﬂk l
}
‡‡ 
if
‚‚ 
(
‚‚ 
dto
‚‚ 
.
‚‚ 
DoctorId
‚‚ 
is
‚‚ 
not
‚‚  #
null
‚‚$ (
)
‚‚( )
{
„„ 
await
‰‰ 0
"ValidateDoctorForHealthRecordAsync
‰‰ 8
(
‰‰8 9
dto
‰‰9 <
.
‰‰< =
DoctorId
‰‰= E
.
‰‰E F
Value
‰‰F K
,
‰‰K L
appointment
‰‰M X
)
‰‰X Y
;
‰‰Y Z
}
ÂÂ 
return
ÁÁ 
appointment
ÁÁ 
;
ÁÁ 
}
ËË 	
private
ÍÍ 
async
ÍÍ 
Task
ÍÍ 0
"ValidateDoctorForHealthRecordAsync
ÍÍ =
(
ÍÍ= >
int
ÍÍ> A
doctorId
ÍÍB J
,
ÍÍJ K
Appointment
ÍÍL W
appointment
ÍÍX c
)
ÍÍc d
{
ÎÎ 	
ValidateDoctorId
ÏÏ 
(
ÏÏ 
doctorId
ÏÏ %
)
ÏÏ% &
;
ÏÏ& '
var
ÓÓ 
doctor
ÓÓ 
=
ÓÓ 
await
ÓÓ 
doctorRepository
ÓÓ /
.
ÓÓ/ 0
GetByIdAsync
ÓÓ0 <
(
ÓÓ< =
doctorId
ÓÓ= E
)
ÓÓE F
;
ÓÓF G
if
 
(
 
doctor
 
is
 
null
 
)
 
{
ÒÒ 
throw
ÚÚ 
new
ÚÚ %
EntityNotFoundException
ÚÚ 1
(
ÚÚ1 2
DoctorEntityName
ÚÚ2 B
,
ÚÚB C
doctorId
ÚÚD L
)
ÚÚL M
;
ÚÚM N
}
ÛÛ 
if
ıı 
(
ıı 
appointment
ıı 
.
ıı 
DoctorId
ıı $
!=
ıı% '
doctorId
ıı( 0
)
ıı0 1
{
ˆˆ 
throw
˜˜ 
new
˜˜ '
HealthRecordRuleException
˜˜ 3
(
˜˜3 4
$str
˜˜4 i
)
˜˜i j
;
˜˜j k
}
¯¯ 
}
˘˘ 	
private
˚˚ 
static
˚˚ 
void
˚˚ 8
*ValidateAppointmentForHealthRecordCreation
˚˚ F
(
˚˚F G
Appointment
¸¸ 
appointment
¸¸ #
,
¸¸# $
DateTime
˝˝ 
	visitDate
˝˝ 
)
˝˝ 
{
˛˛ 	
if
ˇˇ 
(
ˇˇ 
appointment
ˇˇ 
.
ˇˇ 
Status
ˇˇ "
==
ˇˇ# %
AppointmentStatus
ˇˇ& 7
.
ˇˇ7 8
	Cancelled
ˇˇ8 A
)
ˇˇA B
{
ÄÄ 
throw
ÅÅ 
new
ÅÅ '
HealthRecordRuleException
ÅÅ 3
(
ÅÅ3 4
$str
ÅÅ4 p
)
ÅÅp q
;
ÅÅq r
}
ÇÇ 
if
ÑÑ 
(
ÑÑ 
appointment
ÑÑ 
.
ÑÑ 
Status
ÑÑ "
==
ÑÑ# %
AppointmentStatus
ÑÑ& 7
.
ÑÑ7 8
Pending
ÑÑ8 ?
)
ÑÑ? @
{
ÖÖ 
throw
ÜÜ 
new
ÜÜ '
HealthRecordRuleException
ÜÜ 3
(
ÜÜ3 4
$str
ÜÜ4 n
)
ÜÜn o
;
ÜÜo p
}
áá 
if
ââ 
(
ââ 
appointment
ââ 
.
ââ 
Status
ââ "
==
ââ# %
AppointmentStatus
ââ& 7
.
ââ7 8
	Completed
ââ8 A
)
ââA B
{
ää 
throw
ãã 
new
ãã '
HealthRecordRuleException
ãã 3
(
ãã3 4
$str
ãã4 w
)
ããw x
;
ããx y
}
åå 
if
éé 
(
éé 
appointment
éé 
.
éé 
Status
éé "
!=
éé# %
AppointmentStatus
éé& 7
.
éé7 8
	Confirmed
éé8 A
)
ééA B
{
èè 
throw
êê 
new
êê '
HealthRecordRuleException
êê 3
(
êê3 4
$str
êê4 q
)
êêq r
;
êêr s
}
ëë 
if
ìì 
(
ìì 
appointment
ìì 
.
ìì 
ScheduledDate
ìì )
.
ìì) *
Date
ìì* .
>
ìì/ 0
DateTime
ìì1 9
.
ìì9 :
Today
ìì: ?
)
ìì? @
{
îî 
throw
ïï 
new
ïï '
HealthRecordRuleException
ïï 3
(
ïï3 4
$str
ïï4 p
)
ïïp q
;
ïïq r
}
ññ 
if
òò 
(
òò 
	visitDate
òò 
.
òò 
Date
òò 
!=
òò !
appointment
òò" -
.
òò- .
ScheduledDate
òò. ;
.
òò; <
Date
òò< @
)
òò@ A
{
ôô 
throw
öö 
new
öö '
HealthRecordRuleException
öö 3
(
öö3 4
$str
öö4 k
)
öök l
;
ööl m
}
õõ 
}
úú 	
private
ûû 
async
ûû 
Task
ûû 
<
ûû 
Patient
ûû "
>
ûû" #%
GetLoggedInPatientAsync
ûû$ ;
(
ûû; <
string
ûû< B
identityUserId
ûûC Q
)
ûûQ R
{
üü 	
if
†† 
(
†† 
string
†† 
.
††  
IsNullOrWhiteSpace
†† )
(
††) *
identityUserId
††* 8
)
††8 9
)
††9 :
{
°° 
throw
¢¢ 
new
¢¢ #
BusinessRuleException
¢¢ /
(
¢¢/ 0
$str
¢¢0 I
)
¢¢I J
;
¢¢J K
}
££ 
var
•• 
patient
•• 
=
•• 
await
•• 
patientRepository
••  1
.
••1 2&
GetByIdentityUserIdAsync
••2 J
(
••J K
identityUserId
••K Y
)
••Y Z
;
••Z [
if
ßß 
(
ßß 
patient
ßß 
is
ßß 
null
ßß 
)
ßß  
{
®® 
throw
©© 
new
©© %
EntityNotFoundException
©© 1
(
©©1 2
$str
©©2 V
,
©©V W
$num
©©X Y
)
©©Y Z
;
©©Z [
}
™™ 
return
¨¨ 
patient
¨¨ 
;
¨¨ 
}
≠≠ 	
private
ØØ 
async
ØØ 
Task
ØØ 
<
ØØ 
Doctor
ØØ !
>
ØØ! "$
GetLoggedInDoctorAsync
ØØ# 9
(
ØØ9 :
string
ØØ: @
identityUserId
ØØA O
)
ØØO P
{
∞∞ 	
if
±± 
(
±± 
string
±± 
.
±±  
IsNullOrWhiteSpace
±± )
(
±±) *
identityUserId
±±* 8
)
±±8 9
)
±±9 :
{
≤≤ 
throw
≥≥ 
new
≥≥ #
BusinessRuleException
≥≥ /
(
≥≥/ 0
$str
≥≥0 I
)
≥≥I J
;
≥≥J K
}
¥¥ 
var
∂∂ 
doctor
∂∂ 
=
∂∂ 
await
∂∂ 
doctorRepository
∂∂ /
.
∂∂/ 0&
GetByIdentityUserIdAsync
∂∂0 H
(
∂∂H I
identityUserId
∂∂I W
)
∂∂W X
;
∂∂X Y
if
∏∏ 
(
∏∏ 
doctor
∏∏ 
is
∏∏ 
null
∏∏ 
)
∏∏ 
{
ππ 
throw
∫∫ 
new
∫∫ %
EntityNotFoundException
∫∫ 1
(
∫∫1 2
$str
∫∫2 U
,
∫∫U V
$num
∫∫W X
)
∫∫X Y
;
∫∫Y Z
}
ªª 
return
ΩΩ 
doctor
ΩΩ 
;
ΩΩ 
}
ææ 	
private
¿¿ 
async
¿¿ 
Task
¿¿ 
<
¿¿ 
Appointment
¿¿ &
>
¿¿& '+
GetAppointmentEntityByIdAsync
¿¿( E
(
¿¿E F
int
¿¿F I
appointmentId
¿¿J W
)
¿¿W X
{
¡¡ 	#
ValidateAppointmentId
¬¬ !
(
¬¬! "
appointmentId
¬¬" /
)
¬¬/ 0
;
¬¬0 1
var
ƒƒ 
appointment
ƒƒ 
=
ƒƒ 
await
ƒƒ ##
appointmentRepository
ƒƒ$ 9
.
ƒƒ9 :
GetByIdAsync
ƒƒ: F
(
ƒƒF G
appointmentId
ƒƒG T
)
ƒƒT U
;
ƒƒU V
if
∆∆ 
(
∆∆ 
appointment
∆∆ 
is
∆∆ 
null
∆∆ #
)
∆∆# $
{
«« 
throw
»» 
new
»» %
EntityNotFoundException
»» 1
(
»»1 2#
AppointmentEntityName
»»2 G
,
»»G H
appointmentId
»»I V
)
»»V W
;
»»W X
}
…… 
return
ÀÀ 
appointment
ÀÀ 
;
ÀÀ 
}
ÃÃ 	
private
ŒŒ 
async
ŒŒ 
Task
ŒŒ (
ValidatePatientExistsAsync
ŒŒ 5
(
ŒŒ5 6
int
ŒŒ6 9
	patientId
ŒŒ: C
)
ŒŒC D
{
œœ 	
ValidatePatientId
–– 
(
–– 
	patientId
–– '
)
––' (
;
––( )
var
““ 
patient
““ 
=
““ 
await
““ 
patientRepository
““  1
.
““1 2
GetByIdAsync
““2 >
(
““> ?
	patientId
““? H
)
““H I
;
““I J
if
‘‘ 
(
‘‘ 
patient
‘‘ 
is
‘‘ 
null
‘‘ 
)
‘‘  
{
’’ 
throw
÷÷ 
new
÷÷ %
EntityNotFoundException
÷÷ 1
(
÷÷1 2
PatientEntityName
÷÷2 C
,
÷÷C D
	patientId
÷÷E N
)
÷÷N O
;
÷÷O P
}
◊◊ 
}
ÿÿ 	
private
⁄⁄ 
async
⁄⁄ 
Task
⁄⁄ '
ValidateDoctorExistsAsync
⁄⁄ 4
(
⁄⁄4 5
int
⁄⁄5 8
doctorId
⁄⁄9 A
)
⁄⁄A B
{
€€ 	
ValidateDoctorId
‹‹ 
(
‹‹ 
doctorId
‹‹ %
)
‹‹% &
;
‹‹& '
var
ﬁﬁ 
doctor
ﬁﬁ 
=
ﬁﬁ 
await
ﬁﬁ 
doctorRepository
ﬁﬁ /
.
ﬁﬁ/ 0
GetByIdAsync
ﬁﬁ0 <
(
ﬁﬁ< =
doctorId
ﬁﬁ= E
)
ﬁﬁE F
;
ﬁﬁF G
if
‡‡ 
(
‡‡ 
doctor
‡‡ 
is
‡‡ 
null
‡‡ 
)
‡‡ 
{
·· 
throw
‚‚ 
new
‚‚ %
EntityNotFoundException
‚‚ 1
(
‚‚1 2
DoctorEntityName
‚‚2 B
,
‚‚B C
doctorId
‚‚D L
)
‚‚L M
;
‚‚M N
}
„„ 
}
‰‰ 	
private
ÊÊ 
static
ÊÊ 
void
ÊÊ $
ValidateHealthRecordId
ÊÊ 2
(
ÊÊ2 3
int
ÊÊ3 6
healthRecordId
ÊÊ7 E
)
ÊÊE F
{
ÁÁ 	
if
ËË 
(
ËË 
healthRecordId
ËË 
<=
ËË !
$num
ËË" #
)
ËË# $
{
ÈÈ 
throw
ÍÍ 
new
ÍÍ '
HealthRecordRuleException
ÍÍ 3
(
ÍÍ3 4
$str
ÍÍ4 e
)
ÍÍe f
;
ÍÍf g
}
ÎÎ 
}
ÏÏ 	
private
ÓÓ 
static
ÓÓ 
void
ÓÓ 
ValidatePatientId
ÓÓ -
(
ÓÓ- .
int
ÓÓ. 1
	patientId
ÓÓ2 ;
)
ÓÓ; <
{
ÔÔ 	
if
 
(
 
	patientId
 
<=
 
$num
 
)
 
{
ÒÒ 
throw
ÚÚ 
new
ÚÚ '
HealthRecordRuleException
ÚÚ 3
(
ÚÚ3 4
$str
ÚÚ4 _
)
ÚÚ_ `
;
ÚÚ` a
}
ÛÛ 
}
ÙÙ 	
private
ˆˆ 
static
ˆˆ 
void
ˆˆ 
ValidateDoctorId
ˆˆ ,
(
ˆˆ, -
int
ˆˆ- 0
doctorId
ˆˆ1 9
)
ˆˆ9 :
{
˜˜ 	
if
¯¯ 
(
¯¯ 
doctorId
¯¯ 
<=
¯¯ 
$num
¯¯ 
)
¯¯ 
{
˘˘ 
throw
˙˙ 
new
˙˙ '
HealthRecordRuleException
˙˙ 3
(
˙˙3 4
$str
˙˙4 ^
)
˙˙^ _
;
˙˙_ `
}
˚˚ 
}
¸¸ 	
private
˛˛ 
static
˛˛ 
void
˛˛ #
ValidateAppointmentId
˛˛ 1
(
˛˛1 2
int
˛˛2 5
appointmentId
˛˛6 C
)
˛˛C D
{
ˇˇ 	
if
ÄÄ 
(
ÄÄ 
appointmentId
ÄÄ 
<=
ÄÄ  
$num
ÄÄ! "
)
ÄÄ" #
{
ÅÅ 
throw
ÇÇ 
new
ÇÇ '
HealthRecordRuleException
ÇÇ 3
(
ÇÇ3 4
$str
ÇÇ4 c
)
ÇÇc d
;
ÇÇd e
}
ÉÉ 
}
ÑÑ 	
private
ÜÜ 
static
ÜÜ 
void
ÜÜ &
ValidateHealthRecordText
ÜÜ 4
(
ÜÜ4 5
string
áá 
	diagnosis
áá 
,
áá 
string
àà 
prescription
àà 
,
àà  
string
ââ 
?
ââ 
notes
ââ 
)
ââ 
{
ää 	
if
ãã 
(
ãã 
string
ãã 
.
ãã  
IsNullOrWhiteSpace
ãã )
(
ãã) *
	diagnosis
ãã* 3
)
ãã3 4
)
ãã4 5
{
åå 
throw
çç 
new
çç '
HealthRecordRuleException
çç 3
(
çç3 4
$str
çç4 U
)
ççU V
;
ççV W
}
éé 
if
êê 
(
êê 
	diagnosis
êê 
.
êê 
Trim
êê 
(
êê 
)
êê  
.
êê  !
Length
êê! '
>
êê( )
$num
êê* -
)
êê- .
{
ëë 
throw
íí 
new
íí '
HealthRecordRuleException
íí 3
(
íí3 4
$str
íí4 g
)
ííg h
;
ííh i
}
ìì 
if
ïï 
(
ïï 
string
ïï 
.
ïï  
IsNullOrWhiteSpace
ïï )
(
ïï) *
prescription
ïï* 6
)
ïï6 7
)
ïï7 8
{
ññ 
throw
óó 
new
óó '
HealthRecordRuleException
óó 3
(
óó3 4
$str
óó4 X
)
óóX Y
;
óóY Z
}
òò 
if
öö 
(
öö 
prescription
öö 
.
öö 
Trim
öö !
(
öö! "
)
öö" #
.
öö# $
Length
öö$ *
>
öö+ ,
$num
öö- 0
)
öö0 1
{
õõ 
throw
úú 
new
úú '
HealthRecordRuleException
úú 3
(
úú3 4
$str
úú4 j
)
úúj k
;
úúk l
}
ùù 
if
üü 
(
üü 
!
üü 
string
üü 
.
üü  
IsNullOrWhiteSpace
üü *
(
üü* +
notes
üü+ 0
)
üü0 1
&&
üü2 4
notes
üü5 :
.
üü: ;
Trim
üü; ?
(
üü? @
)
üü@ A
.
üüA B
Length
üüB H
>
üüI J
$num
üüK O
)
üüO P
{
†† 
throw
°° 
new
°° '
HealthRecordRuleException
°° 3
(
°°3 4
$str
°°4 g
)
°°g h
;
°°h i
}
¢¢ 
}
££ 	
}
§§ 
}•• π¸
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\DoctorService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public 

class 
DoctorService 
( 
IDoctorRepository 

repository $
,$ %
IMapper 
mapper 
, 
UserManager 
< 
IdentityUser  
>  !
userManager" -
,- .
RoleManager 
< 
IdentityRole  
>  !
roleManager" -
)- .
:/ 0
IDoctorService1 ?
{ 
private 
const 
string 
DoctorEntityName -
=. /
$str0 8
;8 9
private 
const 
string 
DoctorRoleName +
=, -
$str. 6
;6 7
private 
const 
string (
DoctorDetailsRequiredMessage 9
=: ;
$str< Z
;Z [
public 
async 
Task 
< 
List 
< 
	DoctorDto (
>( )
>) *
GetAllDoctorsAsync+ =
(= >
)> ?
{ 	
var 
doctors 
= 
await 

repository  *
.* +
GetAllAsync+ 6
(6 7
)7 8
;8 9
return 
mapper 
. 
Map 
< 
List "
<" #
	DoctorDto# ,
>, -
>- .
(. /
doctors/ 6
)6 7
;7 8
} 	
public 
async 
Task 
< 
PagedResponse '
<' (
	DoctorDto( 1
>1 2
>2 3#
GetAllDoctorsPagedAsync4 K
(K L$
DoctorPaginationQueryDtoL d
querye j
)j k
{ 	
if   
(   
query   
is   
null   
)   
{!! 
query"" 
="" 
new"" $
DoctorPaginationQueryDto"" 4
(""4 5
)""5 6
;""6 7
}## 
int%% 

pageNumber%% 
=%% 
query%% "
.%%" #

PageNumber%%# -
<=%%. 0
$num%%1 2
?%%3 4
$num%%5 6
:%%7 8
query%%9 >
.%%> ?

PageNumber%%? I
;%%I J
int'' 
pageSize'' 
='' 
query''  
.''  !
PageSize''! )
<=''* ,
$num''- .
?''/ 0
$num''1 3
:''4 5
query''6 ;
.''; <
PageSize''< D
;''D E
pageSize)) 
=)) 
pageSize)) 
>))  !
$num))" %
?))& '
$num))( +
:)), -
pageSize)). 6
;))6 7
var++ 
doctors++ 
=++ 
await++ 

repository++  *
.++* +
GetAllAsync+++ 6
(++6 7
)++7 8
;++8 9
var-- 
filteredDoctors-- 
=--  !
doctors--" )
.--) *
AsEnumerable--* 6
(--6 7
)--7 8
;--8 9
if// 
(// 
!// 
string// 
.// 
IsNullOrWhiteSpace// *
(//* +
query//+ 0
.//0 1

SearchTerm//1 ;
)//; <
)//< =
{00 
string11 

searchTerm11 !
=11" #
query11$ )
.11) *

SearchTerm11* 4
.114 5
Trim115 9
(119 :
)11: ;
;11; <
filteredDoctors33 
=33  !
filteredDoctors33" 1
.331 2
Where332 7
(337 8
d338 9
=>33: <
d44 
.44 

DoctorName44  
.44  !
Contains44! )
(44) *

searchTerm44* 4
,444 5
StringComparison446 F
.44F G
OrdinalIgnoreCase44G X
)44X Y
||44Z \
d55 
.55 
Email55 
.55 
Contains55 $
(55$ %

searchTerm55% /
,55/ 0
StringComparison551 A
.55A B
OrdinalIgnoreCase55B S
)55S T
)55T U
;55U V
}66 
if88 
(88 
query88 
.88 
Specialisation88 $
is88% '
not88( +
null88, 0
)880 1
{99 
filteredDoctors:: 
=::  !
filteredDoctors::" 1
.::1 2
Where::2 7
(::7 8
d::8 9
=>::: <
d;; 
.;; 
Specialisation;; $
==;;% '
query;;( -
.;;- .
Specialisation;;. <
.;;< =
Value;;= B
);;B C
;;;C D
}<< 
if>> 
(>> 
query>> 
.>> 
IsActive>> 
is>> !
not>>" %
null>>& *
)>>* +
{?? 
filteredDoctors@@ 
=@@  !
filteredDoctors@@" 1
.@@1 2
Where@@2 7
(@@7 8
d@@8 9
=>@@: <
dAA 
.AA 
IsActiveAA 
==AA !
queryAA" '
.AA' (
IsActiveAA( 0
.AA0 1
ValueAA1 6
)AA6 7
;AA7 8
}BB 
intDD 
totalRecordsDD 
=DD 
filteredDoctorsDD .
.DD. /
CountDD/ 4
(DD4 5
)DD5 6
;DD6 7
varFF 
pagedDoctorsFF 
=FF 
filteredDoctorsFF .
.GG 
OrderByGG 
(GG 
dGG 
=>GG 
dGG 
.GG  
DoctorIdGG  (
)GG( )
.HH 
SkipHH 
(HH 
(HH 

pageNumberHH !
-HH" #
$numHH$ %
)HH% &
*HH' (
pageSizeHH) 1
)HH1 2
.II 
TakeII 
(II 
pageSizeII 
)II 
.JJ 
ToListJJ 
(JJ 
)JJ 
;JJ 
varLL 
mappedDoctorsLL 
=LL 
mapperLL  &
.LL& '
MapLL' *
<LL* +
ListLL+ /
<LL/ 0
	DoctorDtoLL0 9
>LL9 :
>LL: ;
(LL; <
pagedDoctorsLL< H
)LLH I
;LLI J
returnNN 
newNN 
PagedResponseNN $
<NN$ %
	DoctorDtoNN% .
>NN. /
{OO 
ItemsPP 
=PP 
mappedDoctorsPP %
,PP% &

PageNumberQQ 
=QQ 

pageNumberQQ '
,QQ' (
PageSizeRR 
=RR 
pageSizeRR #
,RR# $
TotalRecordsSS 
=SS 
totalRecordsSS +
,SS+ ,

TotalPagesTT 
=TT 
(TT 
intTT !
)TT! "
MathTT" &
.TT& '
CeilingTT' .
(TT. /
totalRecordsTT/ ;
/TT< =
(TT> ?
doubleTT? E
)TTE F
pageSizeTTF N
)TTN O
}UU 
;UU 
}VV 	
publicXX 
asyncXX 
TaskXX 
<XX 
ListXX 
<XX 
	DoctorDtoXX (
>XX( )
>XX) *$
GetAllActiveDoctorsAsyncXX+ C
(XXC D
)XXD E
{YY 	
varZZ 
doctorsZZ 
=ZZ 
awaitZZ 

repositoryZZ  *
.ZZ* +
GetAllActiveAsyncZZ+ <
(ZZ< =
)ZZ= >
;ZZ> ?
return\\ 
mapper\\ 
.\\ 
Map\\ 
<\\ 
List\\ "
<\\" #
	DoctorDto\\# ,
>\\, -
>\\- .
(\\. /
doctors\\/ 6
)\\6 7
;\\7 8
}]] 	
public__ 
async__ 
Task__ 
<__ 
	DoctorDto__ #
>__# $
GetDoctorByIdAsync__% 7
(__7 8
int__8 ;
doctorId__< D
)__D E
{`` 	
ValidateDoctorIdaa 
(aa 
doctorIdaa %
)aa% &
;aa& '
varcc 
doctorcc 
=cc 
awaitcc 

repositorycc )
.cc) *
GetByIdAsynccc* 6
(cc6 7
doctorIdcc7 ?
)cc? @
;cc@ A
ifee 
(ee 
doctoree 
isee 
nullee 
)ee 
{ff 
throwgg 
newgg #
EntityNotFoundExceptiongg 1
(gg1 2
DoctorEntityNamegg2 B
,ggB C
doctorIdggD L
)ggL M
;ggM N
}hh 
returnjj 
mapperjj 
.jj 
Mapjj 
<jj 
	DoctorDtojj '
>jj' (
(jj( )
doctorjj) /
)jj/ 0
;jj0 1
}kk 	
publicmm 
asyncmm 
Taskmm 
<mm 
Listmm 
<mm 
	DoctorDtomm (
>mm( )
>mm) *+
GetDoctorsBySpecialisationAsyncmm+ J
(mmJ K
SpecialisationTypemmK ]
specialisationmm^ l
)mml m
{nn 	
varoo 
doctorsoo 
=oo 
awaitoo 

repositoryoo  *
.oo* +$
GetBySpecialisationAsyncoo+ C
(ooC D
specialisationooD R
)ooR S
;ooS T
returnqq 
mapperqq 
.qq 
Mapqq 
<qq 
Listqq "
<qq" #
	DoctorDtoqq# ,
>qq, -
>qq- .
(qq. /
doctorsqq/ 6
)qq6 7
;qq7 8
}rr 	
publictt 
asynctt 
Tasktt 
<tt 
Listtt 
<tt 
	DoctorDtott (
>tt( )
>tt) *1
%GetActiveDoctorsBySpecialisationAsynctt+ P
(ttP Q
SpecialisationTypettQ c
specialisationttd r
)ttr s
{uu 	
varvv 
doctorsvv 
=vv 
awaitvv 

repositoryvv  *
.vv* +*
GetActiveBySpecialisationAsyncvv+ I
(vvI J
specialisationvvJ X
)vvX Y
;vvY Z
returnxx 
mapperxx 
.xx 
Mapxx 
<xx 
Listxx "
<xx" #
	DoctorDtoxx# ,
>xx, -
>xx- .
(xx. /
doctorsxx/ 6
)xx6 7
;xx7 8
}yy 	
public{{ 
async{{ 
Task{{ 
<{{ $
DoctorCreatedResponseDto{{ 2
>{{2 3$
CreateDoctorByAdminAsync{{4 L
({{L M
CreateDoctorDto{{M \
dto{{] `
){{` a
{|| 	#
ValidateCreateDoctorDto}} #
(}}# $
dto}}$ '
)}}' (
;}}( )
string 
normalizedEmail "
=# $
dto% (
.( )
Email) .
.. /
Trim/ 3
(3 4
)4 5
.5 6
ToLower6 =
(= >
)> ?
;? @
bool
ÅÅ 
doctorEmailExists
ÅÅ "
=
ÅÅ# $
await
ÅÅ% *

repository
ÅÅ+ 5
.
ÅÅ5 6 
ExistsByEmailAsync
ÅÅ6 H
(
ÅÅH I
normalizedEmail
ÅÅI X
)
ÅÅX Y
;
ÅÅY Z
if
ÉÉ 
(
ÉÉ 
doctorEmailExists
ÉÉ !
)
ÉÉ! "
{
ÑÑ 
throw
ÖÖ 
new
ÖÖ 
ConflictException
ÖÖ +
(
ÖÖ+ ,
$str
ÖÖ, V
)
ÖÖV W
;
ÖÖW X
}
ÜÜ 
var
àà "
existingIdentityUser
àà $
=
àà% &
await
àà' ,
userManager
àà- 8
.
àà8 9
FindByEmailAsync
àà9 I
(
ààI J
normalizedEmail
ààJ Y
)
ààY Z
;
ààZ [
if
ää 
(
ää "
existingIdentityUser
ää $
is
ää% '
not
ää( +
null
ää, 0
)
ää0 1
{
ãã 
throw
åå 
new
åå 
ConflictException
åå +
(
åå+ ,
$str
åå, ]
)
åå] ^
;
åå^ _
}
çç 
string
èè 
temporaryPassword
èè $
=
èè% &'
GenerateTemporaryPassword
èè' @
(
èè@ A
dto
èèA D
.
èèD E
FullName
èèE M
)
èèM N
;
èèN O
var
ëë 
identityUser
ëë 
=
ëë 
new
ëë "
IdentityUser
ëë# /
{
íí 
UserName
ìì 
=
ìì 
normalizedEmail
ìì *
,
ìì* +
Email
îî 
=
îî 
normalizedEmail
îî '
,
îî' (
EmailConfirmed
ïï 
=
ïï  
true
ïï! %
}
ññ 
;
ññ 
var
òò 
createUserResult
òò  
=
òò! "
await
òò# (
userManager
òò) 4
.
òò4 5
CreateAsync
òò5 @
(
òò@ A
identityUser
òòA M
,
òòM N
temporaryPassword
òòO `
)
òò` a
;
òòa b
if
öö 
(
öö 
!
öö 
createUserResult
öö !
.
öö! "
	Succeeded
öö" +
)
öö+ ,
{
õõ 
var
úú 
errors
úú 
=
úú 
string
úú #
.
úú# $
Join
úú$ (
(
úú( )
$str
úú) ,
,
úú, -
createUserResult
úú. >
.
úú> ?
Errors
úú? E
.
úúE F
Select
úúF L
(
úúL M
e
úúM N
=>
úúO Q
e
úúR S
.
úúS T
Description
úúT _
)
úú_ `
)
úú` a
;
úúa b
throw
ùù 
new
ùù #
BusinessRuleException
ùù /
(
ùù/ 0
errors
ùù0 6
)
ùù6 7
;
ùù7 8
}
ûû 
if
†† 
(
†† 
!
†† 
await
†† 
roleManager
†† "
.
††" #
RoleExistsAsync
††# 2
(
††2 3
DoctorRoleName
††3 A
)
††A B
)
††B C
{
°° 
await
¢¢ 
roleManager
¢¢ !
.
¢¢! "
CreateAsync
¢¢" -
(
¢¢- .
new
¢¢. 1
IdentityRole
¢¢2 >
(
¢¢> ?
DoctorRoleName
¢¢? M
)
¢¢M N
)
¢¢N O
;
¢¢O P
}
££ 
var
•• 

roleResult
•• 
=
•• 
await
•• "
userManager
••# .
.
••. /
AddToRoleAsync
••/ =
(
••= >
identityUser
••> J
,
••J K
DoctorRoleName
••L Z
)
••Z [
;
••[ \
if
ßß 
(
ßß 
!
ßß 

roleResult
ßß 
.
ßß 
	Succeeded
ßß %
)
ßß% &
{
®® 
await
©© 
userManager
©© !
.
©©! "
DeleteAsync
©©" -
(
©©- .
identityUser
©©. :
)
©©: ;
;
©©; <
var
´´ 
errors
´´ 
=
´´ 
string
´´ #
.
´´# $
Join
´´$ (
(
´´( )
$str
´´) ,
,
´´, -

roleResult
´´. 8
.
´´8 9
Errors
´´9 ?
.
´´? @
Select
´´@ F
(
´´F G
e
´´G H
=>
´´I K
e
´´L M
.
´´M N
Description
´´N Y
)
´´Y Z
)
´´Z [
;
´´[ \
throw
¨¨ 
new
¨¨ #
BusinessRuleException
¨¨ /
(
¨¨/ 0
errors
¨¨0 6
)
¨¨6 7
;
¨¨7 8
}
≠≠ 
var
ØØ 
doctor
ØØ 
=
ØØ 
mapper
ØØ 
.
ØØ  
Map
ØØ  #
<
ØØ# $
Doctor
ØØ$ *
>
ØØ* +
(
ØØ+ ,
dto
ØØ, /
)
ØØ/ 0
;
ØØ0 1
doctor
±± 
.
±± 

DoctorName
±± 
=
±± 
dto
±±  #
.
±±# $
FullName
±±$ ,
.
±±, -
Trim
±±- 1
(
±±1 2
)
±±2 3
;
±±3 4
doctor
≤≤ 
.
≤≤ 
Email
≤≤ 
=
≤≤ 
normalizedEmail
≤≤ *
;
≤≤* +
doctor
≥≥ 
.
≥≥ 
YearsOfExperience
≥≥ $
=
≥≥% &(
CalculateYearsOfExperience
≥≥' A
(
≥≥A B
dto
≥≥B E
.
≥≥E F
PracticeStartDate
≥≥F W
)
≥≥W X
;
≥≥X Y
doctor
¥¥ 
.
¥¥ 
IsActive
¥¥ 
=
¥¥ 
true
¥¥ "
;
¥¥" #
doctor
µµ 
.
µµ 
IdentityUserId
µµ !
=
µµ" #
identityUser
µµ$ 0
.
µµ0 1
Id
µµ1 3
;
µµ3 4
doctor
∂∂ 
.
∂∂ 
CreatedDate
∂∂ 
=
∂∂  
DateTime
∂∂! )
.
∂∂) *
Now
∂∂* -
;
∂∂- .
var
∏∏ 
savedDoctor
∏∏ 
=
∏∏ 
await
∏∏ #

repository
∏∏$ .
.
∏∏. /
CreateAsync
∏∏/ :
(
∏∏: ;
doctor
∏∏; A
)
∏∏A B
;
∏∏B C
return
∫∫ 
new
∫∫ &
DoctorCreatedResponseDto
∫∫ /
{
ªª 
DoctorId
ºº 
=
ºº 
savedDoctor
ºº &
.
ºº& '
DoctorId
ºº' /
,
ºº/ 0

DoctorName
ΩΩ 
=
ΩΩ 
savedDoctor
ΩΩ (
.
ΩΩ( )

DoctorName
ΩΩ) 3
,
ΩΩ3 4
Email
ææ 
=
ææ 
savedDoctor
ææ #
.
ææ# $
Email
ææ$ )
,
ææ) *
TemporaryPassword
øø !
=
øø" #
temporaryPassword
øø$ 5
,
øø5 6
Message
¿¿ 
=
¿¿ 
$str
¿¿ @
}
¡¡ 
;
¡¡ 
}
¬¬ 	
public
ƒƒ 
async
ƒƒ 
Task
ƒƒ 
<
ƒƒ 
	DoctorDto
ƒƒ #
>
ƒƒ# $
UpdateDoctorAsync
ƒƒ% 6
(
ƒƒ6 7
int
ƒƒ7 :
doctorId
ƒƒ; C
,
ƒƒC D
UpdateDoctorDto
ƒƒE T
dto
ƒƒU X
)
ƒƒX Y
{
≈≈ 	
ValidateDoctorId
∆∆ 
(
∆∆ 
doctorId
∆∆ %
)
∆∆% &
;
∆∆& '%
ValidateUpdateDoctorDto
»» #
(
»»# $
dto
»»$ '
)
»»' (
;
»»( )
var
   
existingDoctor
   
=
    
await
  ! &

repository
  ' 1
.
  1 2
GetByIdAsync
  2 >
(
  > ?
doctorId
  ? G
)
  G H
;
  H I
if
ÃÃ 
(
ÃÃ 
existingDoctor
ÃÃ 
is
ÃÃ !
null
ÃÃ" &
)
ÃÃ& '
{
ÕÕ 
throw
ŒŒ 
new
ŒŒ %
EntityNotFoundException
ŒŒ 1
(
ŒŒ1 2
DoctorEntityName
ŒŒ2 B
,
ŒŒB C
doctorId
ŒŒD L
)
ŒŒL M
;
ŒŒM N
}
œœ 
var
—— 
doctor
—— 
=
—— 
mapper
—— 
.
——  
Map
——  #
<
——# $
Doctor
——$ *
>
——* +
(
——+ ,
dto
——, /
)
——/ 0
;
——0 1
doctor
”” 
.
”” 
DoctorId
”” 
=
”” 
doctorId
”” &
;
””& '
doctor
‘‘ 
.
‘‘ 
Email
‘‘ 
=
‘‘ 
existingDoctor
‘‘ )
.
‘‘) *
Email
‘‘* /
;
‘‘/ 0
doctor
’’ 
.
’’ 
IdentityUserId
’’ !
=
’’" #
existingDoctor
’’$ 2
.
’’2 3
IdentityUserId
’’3 A
;
’’A B
doctor
÷÷ 
.
÷÷ 
YearsOfExperience
÷÷ $
=
÷÷% &(
CalculateYearsOfExperience
÷÷' A
(
÷÷A B
dto
÷÷B E
.
÷÷E F
PracticeStartDate
÷÷F W
)
÷÷W X
;
÷÷X Y
doctor
◊◊ 
.
◊◊ 
CreatedDate
◊◊ 
=
◊◊  
existingDoctor
◊◊! /
.
◊◊/ 0
CreatedDate
◊◊0 ;
;
◊◊; <
var
ŸŸ 
updatedDoctor
ŸŸ 
=
ŸŸ 
await
ŸŸ  %

repository
ŸŸ& 0
.
ŸŸ0 1
UpdateAsync
ŸŸ1 <
(
ŸŸ< =
doctorId
ŸŸ= E
,
ŸŸE F
doctor
ŸŸG M
)
ŸŸM N
;
ŸŸN O
if
€€ 
(
€€ 
updatedDoctor
€€ 
is
€€  
null
€€! %
)
€€% &
{
‹‹ 
throw
›› 
new
›› %
EntityNotFoundException
›› 1
(
››1 2
DoctorEntityName
››2 B
,
››B C
doctorId
››D L
)
››L M
;
››M N
}
ﬁﬁ 
return
‡‡ 
mapper
‡‡ 
.
‡‡ 
Map
‡‡ 
<
‡‡ 
	DoctorDto
‡‡ '
>
‡‡' (
(
‡‡( )
updatedDoctor
‡‡) 6
)
‡‡6 7
;
‡‡7 8
}
·· 	
public
„„ 
async
„„ 
Task
„„ 
<
„„ 
	DoctorDto
„„ #
>
„„# $
DeleteDoctorAsync
„„% 6
(
„„6 7
int
„„7 :
doctorId
„„; C
)
„„C D
{
‰‰ 	
ValidateDoctorId
ÂÂ 
(
ÂÂ 
doctorId
ÂÂ %
)
ÂÂ% &
;
ÂÂ& '
var
ÁÁ 
deletedDoctor
ÁÁ 
=
ÁÁ 
await
ÁÁ  %

repository
ÁÁ& 0
.
ÁÁ0 1
DeleteAsync
ÁÁ1 <
(
ÁÁ< =
doctorId
ÁÁ= E
)
ÁÁE F
;
ÁÁF G
if
ÈÈ 
(
ÈÈ 
deletedDoctor
ÈÈ 
is
ÈÈ  
null
ÈÈ! %
)
ÈÈ% &
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ %
EntityNotFoundException
ÎÎ 1
(
ÎÎ1 2
DoctorEntityName
ÎÎ2 B
,
ÎÎB C
doctorId
ÎÎD L
)
ÎÎL M
;
ÎÎM N
}
ÏÏ 
return
ÓÓ 
mapper
ÓÓ 
.
ÓÓ 
Map
ÓÓ 
<
ÓÓ 
	DoctorDto
ÓÓ '
>
ÓÓ' (
(
ÓÓ( )
deletedDoctor
ÓÓ) 6
)
ÓÓ6 7
;
ÓÓ7 8
}
ÔÔ 	
public
ÒÒ 
async
ÒÒ 
Task
ÒÒ 
<
ÒÒ 
	DoctorDto
ÒÒ #
>
ÒÒ# $
GetMyProfileAsync
ÒÒ% 6
(
ÒÒ6 7
string
ÒÒ7 =
identityUserId
ÒÒ> L
)
ÒÒL M
{
ÚÚ 	
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
ÛÛ) *
identityUserId
ÛÛ* 8
)
ÛÛ8 9
)
ÛÛ9 :
{
ÙÙ 
throw
ıı 
new
ıı #
BusinessRuleException
ıı /
(
ıı/ 0
$str
ıı0 I
)
ııI J
;
ııJ K
}
ˆˆ 
var
¯¯ 
doctor
¯¯ 
=
¯¯ 
await
¯¯ 

repository
¯¯ )
.
¯¯) *&
GetByIdentityUserIdAsync
¯¯* B
(
¯¯B C
identityUserId
¯¯C Q
)
¯¯Q R
;
¯¯R S
if
˙˙ 
(
˙˙ 
doctor
˙˙ 
is
˙˙ 
null
˙˙ 
)
˙˙ 
{
˚˚ 
throw
¸¸ 
new
¸¸ %
EntityNotFoundException
¸¸ 1
(
¸¸1 2
$str
¸¸2 U
,
¸¸U V
$num
¸¸W X
)
¸¸X Y
;
¸¸Y Z
}
˝˝ 
return
ˇˇ 
mapper
ˇˇ 
.
ˇˇ 
Map
ˇˇ 
<
ˇˇ 
	DoctorDto
ˇˇ '
>
ˇˇ' (
(
ˇˇ( )
doctor
ˇˇ) /
)
ˇˇ/ 0
;
ˇˇ0 1
}
ÄÄ 	
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
List
ÇÇ 
<
ÇÇ 
string
ÇÇ %
>
ÇÇ% &
>
ÇÇ& '(
GetDoctorAvailabilityAsync
ÇÇ( B
(
ÇÇB C
int
ÇÇC F
doctorId
ÇÇG O
)
ÇÇO P
{
ÉÉ 	
ValidateDoctorId
ÑÑ 
(
ÑÑ 
doctorId
ÑÑ %
)
ÑÑ% &
;
ÑÑ& '
var
ÜÜ 
doctor
ÜÜ 
=
ÜÜ 
await
ÜÜ 

repository
ÜÜ )
.
ÜÜ) *
GetByIdAsync
ÜÜ* 6
(
ÜÜ6 7
doctorId
ÜÜ7 ?
)
ÜÜ? @
;
ÜÜ@ A
if
àà 
(
àà 
doctor
àà 
is
àà 
null
àà 
)
àà 
{
ââ 
throw
ää 
new
ää %
EntityNotFoundException
ää 1
(
ää1 2
DoctorEntityName
ää2 B
,
ääB C
doctorId
ääD L
)
ääL M
;
ääM N
}
ãã 
if
çç 
(
çç 
!
çç 
doctor
çç 
.
çç 
IsActive
çç  
)
çç  !
{
éé 
throw
èè 
new
èè #
BusinessRuleException
èè /
(
èè/ 0
$str
èè0 h
)
èèh i
;
èèi j
}
êê 
return
íí 
	TimeSlots
íí 
.
íí 
Slots
íí "
.
íí" #
ToList
íí# )
(
íí) *
)
íí* +
;
íí+ ,
}
ìì 	
private
ïï 
static
ïï 
void
ïï 
ValidateDoctorId
ïï ,
(
ïï, -
int
ïï- 0
doctorId
ïï1 9
)
ïï9 :
{
ññ 	
if
óó 
(
óó 
doctorId
óó 
<=
óó 
$num
óó 
)
óó 
{
òò 
throw
ôô 
new
ôô #
BusinessRuleException
ôô /
(
ôô/ 0
$str
ôô0 Z
)
ôôZ [
;
ôô[ \
}
öö 
}
õõ 	
private
ùù 
static
ùù 
void
ùù %
ValidateCreateDoctorDto
ùù 3
(
ùù3 4
CreateDoctorDto
ùù4 C
dto
ùùD G
)
ùùG H
{
ûû 	
if
üü 
(
üü 
dto
üü 
is
üü 
null
üü 
)
üü 
{
†† 
throw
°° 
new
°° #
BusinessRuleException
°° /
(
°°/ 0*
DoctorDetailsRequiredMessage
°°0 L
)
°°L M
;
°°M N
}
¢¢ (
ValidateDoctorCommonFields
§§ &
(
§§& '
dto
•• 
.
•• 
FullName
•• 
,
•• 
dto
¶¶ 
.
¶¶ 
Email
¶¶ 
,
¶¶ 
dto
ßß 
.
ßß 
PracticeStartDate
ßß %
,
ßß% &
dto
®® 
.
®® 
ConsultationFee
®® #
)
®®# $
;
®®$ %
}
©© 	
private
´´ 
static
´´ 
void
´´ %
ValidateUpdateDoctorDto
´´ 3
(
´´3 4
UpdateDoctorDto
´´4 C
dto
´´D G
)
´´G H
{
¨¨ 	
if
≠≠ 
(
≠≠ 
dto
≠≠ 
is
≠≠ 
null
≠≠ 
)
≠≠ 
{
ÆÆ 
throw
ØØ 
new
ØØ #
BusinessRuleException
ØØ /
(
ØØ/ 0*
DoctorDetailsRequiredMessage
ØØ0 L
)
ØØL M
;
ØØM N
}
∞∞ (
ValidateDoctorCommonFields
≤≤ &
(
≤≤& '
dto
≥≥ 
.
≥≥ 
FullName
≥≥ 
,
≥≥ 
null
¥¥ 
,
¥¥ 
dto
µµ 
.
µµ 
PracticeStartDate
µµ %
,
µµ% &
dto
∂∂ 
.
∂∂ 
ConsultationFee
∂∂ #
)
∂∂# $
;
∂∂$ %
}
∑∑ 	
private
ππ 
static
ππ 
void
ππ (
ValidateDoctorCommonFields
ππ 6
(
ππ6 7
string
∫∫ 
fullName
∫∫ 
,
∫∫ 
string
ªª 
?
ªª 
email
ªª 
,
ªª 
DateTime
ºº 
practiceStartDate
ºº &
,
ºº& '
decimal
ΩΩ 
consultationFee
ΩΩ #
)
ΩΩ# $
{
ææ 	
if
øø 
(
øø 
string
øø 
.
øø  
IsNullOrWhiteSpace
øø )
(
øø) *
fullName
øø* 2
)
øø2 3
)
øø3 4
{
¿¿ 
throw
¡¡ 
new
¡¡ #
BusinessRuleException
¡¡ /
(
¡¡/ 0
$str
¡¡0 O
)
¡¡O P
;
¡¡P Q
}
¬¬ 
if
ƒƒ 
(
ƒƒ 
email
ƒƒ 
is
ƒƒ 
not
ƒƒ 
null
ƒƒ !
&&
ƒƒ" $
string
ƒƒ% +
.
ƒƒ+ , 
IsNullOrWhiteSpace
ƒƒ, >
(
ƒƒ> ?
email
ƒƒ? D
)
ƒƒD E
)
ƒƒE F
{
≈≈ 
throw
∆∆ 
new
∆∆ #
BusinessRuleException
∆∆ /
(
∆∆/ 0
$str
∆∆0 K
)
∆∆K L
;
∆∆L M
}
«« 
if
…… 
(
…… 
practiceStartDate
…… !
.
……! "
Date
……" &
>
……' (
DateTime
……) 1
.
……1 2
Today
……2 7
)
……7 8
{
   
throw
ÀÀ 
new
ÀÀ #
BusinessRuleException
ÀÀ /
(
ÀÀ/ 0
$str
ÀÀ0 ^
)
ÀÀ^ _
;
ÀÀ_ `
}
ÃÃ 
if
ŒŒ 
(
ŒŒ 
consultationFee
ŒŒ 
<
ŒŒ  !
$num
ŒŒ" #
)
ŒŒ# $
{
œœ 
throw
–– 
new
–– #
BusinessRuleException
–– /
(
––/ 0
$str
––0 V
)
––V W
;
––W X
}
—— 
}
““ 	
private
‘‘ 
static
‘‘ 
int
‘‘ (
CalculateYearsOfExperience
‘‘ 5
(
‘‘5 6
DateTime
‘‘6 >
practiceStartDate
‘‘? P
)
‘‘P Q
{
’’ 	
int
÷÷ 
years
÷÷ 
=
÷÷ 
DateTime
÷÷  
.
÷÷  !
Today
÷÷! &
.
÷÷& '
Year
÷÷' +
-
÷÷, -
practiceStartDate
÷÷. ?
.
÷÷? @
Year
÷÷@ D
;
÷÷D E
if
ÿÿ 
(
ÿÿ 
practiceStartDate
ÿÿ !
.
ÿÿ! "
Date
ÿÿ" &
>
ÿÿ' (
DateTime
ÿÿ) 1
.
ÿÿ1 2
Today
ÿÿ2 7
.
ÿÿ7 8
AddYears
ÿÿ8 @
(
ÿÿ@ A
-
ÿÿA B
years
ÿÿB G
)
ÿÿG H
)
ÿÿH I
{
ŸŸ 
years
⁄⁄ 
--
⁄⁄ 
;
⁄⁄ 
}
€€ 
return
›› 
years
›› 
;
›› 
}
ﬁﬁ 	
private
‡‡ 
static
‡‡ 
string
‡‡ '
GenerateTemporaryPassword
‡‡ 7
(
‡‡7 8
string
‡‡8 >

doctorName
‡‡? I
)
‡‡I J
{
·· 	
string
‚‚ 
cleanedName
‚‚ 
=
‚‚  
new
‚‚! $
string
‚‚% +
(
‚‚+ ,

doctorName
„„ 
.
‰‰ 
Where
‰‰ 
(
‰‰ 
char
‰‰ 
.
‰‰  
IsLetter
‰‰  (
)
‰‰( )
.
ÂÂ 
Take
ÂÂ 
(
ÂÂ 
$num
ÂÂ 
)
ÂÂ 
.
ÊÊ 
ToArray
ÊÊ 
(
ÊÊ 
)
ÊÊ 
)
ÊÊ 
;
ÊÊ  
if
ËË 
(
ËË 
string
ËË 
.
ËË  
IsNullOrWhiteSpace
ËË )
(
ËË) *
cleanedName
ËË* 5
)
ËË5 6
)
ËË6 7
{
ÈÈ 
cleanedName
ÍÍ 
=
ÍÍ 
DoctorEntityName
ÍÍ .
;
ÍÍ. /
}
ÎÎ 
string
ÌÌ 
formattedName
ÌÌ  
=
ÌÌ! "
char
ÓÓ 
.
ÓÓ 
ToUpper
ÓÓ 
(
ÓÓ 
cleanedName
ÓÓ (
[
ÓÓ( )
$num
ÓÓ) *
]
ÓÓ* +
)
ÓÓ+ ,
+
ÓÓ- .
cleanedName
ÓÓ/ :
.
ÓÓ: ;
	Substring
ÓÓ; D
(
ÓÓD E
$num
ÓÓE F
)
ÓÓF G
.
ÓÓG H
ToLower
ÓÓH O
(
ÓÓO P
)
ÓÓP Q
;
ÓÓQ R
return
 
$"
 
{
 
formattedName
 #
}
# $
$str
$ %
{
% &
DateTime
& .
.
. /
Today
/ 4
.
4 5
Year
5 9
}
9 :
"
: ;
;
; <
}
ÒÒ 	
}
ÚÚ 
}ÛÛ ≥}
^C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\AuthService.cs
	namespace 	
HealthCareApp
 
. 
Services  
.  !
Impl! %
{ 
public 

class 
AuthService 
( 
UserManager 
< 
IdentityUser  
>  !
userManager" -
,- .
IPatientRepository 
patientRepository ,
,, -
IConfiguration 
config 
) 
:  
IAuthService! -
{ 
public 
async 
Task 
< 
( 
bool 
Success  '
,' (
string) /
Message0 7
,7 8
int9 <
	PatientId= F
)F G
>G H 
RegisterPatientAsyncI ]
(] ^
PatientRegisterDto^ p
requestq x
)x y
{ 	
if 
( 
request 
. 
Password  
!=! #
request$ +
.+ ,
ConfirmPassword, ;
); <
{ 
return 
( 
false 
, 
$str L
,L M
$numN O
)O P
;P Q
} 
if 
( 
request 
. 
DateOfBirth #
.# $
Date$ (
>) *
DateTime+ 3
.3 4
Today4 9
)9 :
{ 
return 
( 
false 
, 
$str G
,G H
$numI J
)J K
;K L
} 
var   
existingUser   
=   
await   $
userManager  % 0
.  0 1
FindByEmailAsync  1 A
(  A B
request  B I
.  I J
Email  J O
)  O P
;  P Q
if"" 
("" 
existingUser"" 
!="" 
null""  $
)""$ %
{## 
return$$ 
($$ 
false$$ 
,$$ 
$str$$ =
,$$= >
$num$$? @
)$$@ A
;$$A B
}%% 
var'' 
identityUser'' 
='' 
new'' "
IdentityUser''# /
{(( 
UserName)) 
=)) 
request)) "
.))" #
Email))# (
,))( )
Email** 
=** 
request** 
.**  
Email**  %
,**% &
EmailConfirmed++ 
=++  
true++! %
},, 
;,, 
var.. 
createUserResult..  
=..! "
await..# (
userManager..) 4
...4 5
CreateAsync..5 @
(..@ A
identityUser..A M
,..M N
request..O V
...V W
Password..W _
).._ `
;..` a
if00 
(00 
!00 
createUserResult00 !
.00! "
	Succeeded00" +
)00+ ,
{11 
var22 
errors22 
=22 
string22 #
.22# $
Join22$ (
(22( )
$str22) ,
,22, -
createUserResult22. >
.22> ?
Errors22? E
.22E F
Select22F L
(22L M
e22M N
=>22O Q
e22R S
.22S T
Description22T _
)22_ `
)22` a
;22a b
return33 
(33 
false33 
,33 
errors33 %
,33% &
$num33' (
)33( )
;33) *
}44 
var66 

roleResult66 
=66 
await66 "
userManager66# .
.66. /
AddToRoleAsync66/ =
(66= >
identityUser66> J
,66J K
$str66L U
)66U V
;66V W
if88 
(88 
!88 

roleResult88 
.88 
	Succeeded88 %
)88% &
{99 
await:: 
userManager:: !
.::! "
DeleteAsync::" -
(::- .
identityUser::. :
)::: ;
;::; <
var<< 
errors<< 
=<< 
string<< #
.<<# $
Join<<$ (
(<<( )
$str<<) ,
,<<, -

roleResult<<. 8
.<<8 9
Errors<<9 ?
.<<? @
Select<<@ F
(<<F G
e<<G H
=><<I K
e<<L M
.<<M N
Description<<N Y
)<<Y Z
)<<Z [
;<<[ \
return== 
(== 
false== 
,== 
errors== %
,==% &
$num==' (
)==( )
;==) *
}>> 
var@@ 
patient@@ 
=@@ 
new@@ 
Patient@@ %
{AA 
PatientNameBB 
=BB 
requestBB %
.BB% &
FullNameBB& .
,BB. /
DateOfBirthCC 
=CC 
requestCC %
.CC% &
DateOfBirthCC& 1
.CC1 2
DateCC2 6
,CC6 7
GenderDD 
=DD 
requestDD  
.DD  !
GenderDD! '
,DD' (
EmailEE 
=EE 
requestEE 
.EE  
EmailEE  %
,EE% &
PhoneNumberFF 
=FF 
requestFF %
.FF% &
PhoneNumberFF& 1
,FF1 2
InsuranceIDGG 
=GG 
requestGG %
.GG% &
InsuranceIdGG& 1
,GG1 2
IdentityUserIdHH 
=HH  
identityUserHH! -
.HH- .
IdHH. 0
,HH0 1
CreatedDateII 
=II 
DateTimeII &
.II& '
NowII' *
}JJ 
;JJ 
varLL 
savedPatientLL 
=LL 
awaitLL $
patientRepositoryLL% 6
.LL6 7
CreateAsyncLL7 B
(LLB C
patientLLC J
)LLJ K
;LLK L
returnNN 
(NN 
trueNN 
,NN 
$strNN <
,NN< =
savedPatientNN> J
.NNJ K
	PatientIdNNK T
)NNT U
;NNU V
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
stringQQ9 ?
TokenQQ@ E
,QQE F
intQQG J
	ExpiresInQQK T
)QQT U
>QQU V
LoginQQW \
(QQ\ ]
LoginDtoQQ] e
requestQQf m
)QQm n
{RR 	
varSS 
userSS 
=SS 
awaitSS 
userManagerSS (
.SS( )
FindByEmailAsyncSS) 9
(SS9 :
requestSS: A
.SSA B
EmailSSB G
)SSG H
;SSH I
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
,WWB C
$numWWD E
)WWE F
;WWF G
}XX 
varZZ 
isPasswordValidZZ 
=ZZ  !
awaitZZ" '
userManagerZZ( 3
.ZZ3 4
CheckPasswordAsyncZZ4 F
(ZZF G
userZZG K
,ZZK L
requestZZM T
.ZZT U
PasswordZZU ]
)ZZ] ^
;ZZ^ _
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
,^^B C
$num^^D E
)^^E F
;^^F G
}__ 
varaa 
tokenaa 
=aa 
awaitaa 
GenerateTokenaa +
(aa+ ,
useraa, 0
)aa0 1
;aa1 2
varcc 
expirycc 
=cc 
intcc 
.cc 
Parsecc "
(cc" #
configcc# )
.cc) *

GetSectioncc* 4
(cc4 5
$strcc5 :
)cc: ;
[cc; <
$strcc< Z
]ccZ [
!cc[ \
)cc\ ]
;cc] ^
returnee 
(ee 
trueee 
,ee 
$stree ,
,ee, -
tokenee. 3
,ee3 4
expiryee5 ;
)ee; <
;ee< =
}ff 	
publichh 
asynchh 
Taskhh 
<hh 
(hh 
boolhh 
Successhh  '
,hh' (
stringhh) /
Messagehh0 7
)hh7 8
>hh8 9
ChangePasswordAsynchh: M
(hhM N
stringhhN T
userIdhhU [
,hh[ \
ChangePasswordDtohh] n
requesthho v
)hhv w
{ii 	
ifjj 
(jj 
stringjj 
.jj 
Equalsjj 
(jj 
requestjj %
.jj% &
CurrentPasswordjj& 5
,jj5 6
requestjj7 >
.jj> ?
NewPasswordjj? J
,jjJ K
StringComparisonjjL \
.jj\ ]
Ordinaljj] d
)jjd e
)jje f
{kk 
returnll 
(ll 
falsell 
,ll 
$strll U
)llU V
;llV W
}mm 
ifoo 
(oo 
requestoo 
.oo 
NewPasswordoo #
!=oo$ &
requestoo' .
.oo. /
ConfirmNewPasswordoo/ A
)ooA B
{pp 
returnqq 
(qq 
falseqq 
,qq 
$strqq T
)qqT U
;qqU V
}rr 
vartt 
usertt 
=tt 
awaittt 
userManagertt (
.tt( )
FindByIdAsynctt) 6
(tt6 7
userIdtt7 =
)tt= >
;tt> ?
ifvv 
(vv 
uservv 
==vv 
nullvv 
)vv 
{ww 
returnxx 
(xx 
falsexx 
,xx 
$strxx 0
)xx0 1
;xx1 2
}yy 
var{{ 
result{{ 
={{ 
await{{ 
userManager{{ *
.{{* +
ChangePasswordAsync{{+ >
({{> ?
user|| 
,|| 
request}} 
.}} 
CurrentPassword}} '
,}}' (
request~~ 
.~~ 
NewPassword~~ #
)~~# $
;~~$ %
if
ÄÄ 
(
ÄÄ 
!
ÄÄ 
result
ÄÄ 
.
ÄÄ 
	Succeeded
ÄÄ !
)
ÄÄ! "
{
ÅÅ 
var
ÇÇ 
errors
ÇÇ 
=
ÇÇ 
string
ÇÇ #
.
ÇÇ# $
Join
ÇÇ$ (
(
ÇÇ( )
$str
ÇÇ) ,
,
ÇÇ, -
result
ÇÇ. 4
.
ÇÇ4 5
Errors
ÇÇ5 ;
.
ÇÇ; <
Select
ÇÇ< B
(
ÇÇB C
e
ÇÇC D
=>
ÇÇE G
e
ÇÇH I
.
ÇÇI J
Description
ÇÇJ U
)
ÇÇU V
)
ÇÇV W
;
ÇÇW X
return
ÉÉ 
(
ÉÉ 
false
ÉÉ 
,
ÉÉ 
errors
ÉÉ %
)
ÉÉ% &
;
ÉÉ& '
}
ÑÑ 
return
ÜÜ 
(
ÜÜ 
true
ÜÜ 
,
ÜÜ 
$str
ÜÜ :
)
ÜÜ: ;
;
ÜÜ; <
}
áá 	
private
ââ 
async
ââ 
Task
ââ 
<
ââ 
string
ââ !
>
ââ! "
GenerateToken
ââ# 0
(
ââ0 1
IdentityUser
ââ1 =
user
ââ> B
)
ââB C
{
ää 	
var
ãã 
jwtSettings
ãã 
=
ãã 
config
ãã $
.
ãã$ %

GetSection
ãã% /
(
ãã/ 0
$str
ãã0 5
)
ãã5 6
;
ãã6 7
var
çç 
key
çç 
=
çç 
new
çç "
SymmetricSecurityKey
çç .
(
çç. /
Encoding
éé 
.
éé 
UTF8
éé 
.
éé 
GetBytes
éé &
(
éé& '
jwtSettings
éé' 2
[
éé2 3
$str
éé3 8
]
éé8 9
!
éé9 :
)
éé: ;
)
èè 
;
èè 
var
ëë 
credentials
ëë 
=
ëë 
new
ëë ! 
SigningCredentials
ëë" 4
(
ëë4 5
key
íí 
,
íí  
SecurityAlgorithms
ìì "
.
ìì" #

HmacSha256
ìì# -
)
îî 
;
îî 
var
ññ 
roles
ññ 
=
ññ 
await
ññ 
userManager
ññ )
.
ññ) *
GetRolesAsync
ññ* 7
(
ññ7 8
user
ññ8 <
)
ññ< =
;
ññ= >
var
òò 
claims
òò 
=
òò 
new
òò 
List
òò !
<
òò! "
Claim
òò" '
>
òò' (
{
ôô 
new
öö 
Claim
öö 
(
öö %
JwtRegisteredClaimNames
öö 1
.
öö1 2
Sub
öö2 5
,
öö5 6
user
öö7 ;
.
öö; <
Id
öö< >
)
öö> ?
,
öö? @
new
õõ 
Claim
õõ 
(
õõ %
JwtRegisteredClaimNames
õõ 1
.
õõ1 2
Email
õõ2 7
,
õõ7 8
user
õõ9 =
.
õõ= >
Email
õõ> C
??
õõD F
string
õõG M
.
õõM N
Empty
õõN S
)
õõS T
,
õõT U
new
úú 
Claim
úú 
(
úú %
JwtRegisteredClaimNames
úú 1
.
úú1 2
Jti
úú2 5
,
úú5 6
Guid
úú7 ;
.
úú; <
NewGuid
úú< C
(
úúC D
)
úúD E
.
úúE F
ToString
úúF N
(
úúN O
)
úúO P
)
úúP Q
,
úúQ R
new
ùù 
Claim
ùù 
(
ùù 

ClaimTypes
ùù $
.
ùù$ %
NameIdentifier
ùù% 3
,
ùù3 4
user
ùù5 9
.
ùù9 :
Id
ùù: <
)
ùù< =
}
ûû 
;
ûû 
foreach
†† 
(
†† 
var
†† 
role
†† 
in
††  
roles
††! &
)
††& '
{
°° 
claims
¢¢ 
.
¢¢ 
Add
¢¢ 
(
¢¢ 
new
¢¢ 
Claim
¢¢ $
(
¢¢$ %

ClaimTypes
¢¢% /
.
¢¢/ 0
Role
¢¢0 4
,
¢¢4 5
role
¢¢6 :
)
¢¢: ;
)
¢¢; <
;
¢¢< =
}
££ 
var
•• 
expirationMinutes
•• !
=
••" #
int
••$ '
.
••' (
Parse
••( -
(
••- .
jwtSettings
••. 9
[
••9 :
$str
••: X
]
••X Y
!
••Y Z
)
••Z [
;
••[ \
var
ßß 
token
ßß 
=
ßß 
new
ßß 
JwtSecurityToken
ßß ,
(
ßß, -
issuer
®® 
:
®® 
jwtSettings
®® #
[
®®# $
$str
®®$ ,
]
®®, -
,
®®- .
audience
©© 
:
©© 
jwtSettings
©© %
[
©©% &
$str
©©& 0
]
©©0 1
,
©©1 2
claims
™™ 
:
™™ 
claims
™™ 
,
™™ 
expires
´´ 
:
´´ 
DateTime
´´ !
.
´´! "
UtcNow
´´" (
.
´´( )

AddMinutes
´´) 3
(
´´3 4
expirationMinutes
´´4 E
)
´´E F
,
´´F G 
signingCredentials
¨¨ "
:
¨¨" #
credentials
¨¨$ /
)
≠≠ 
;
≠≠ 
return
ØØ 
new
ØØ %
JwtSecurityTokenHandler
ØØ .
(
ØØ. /
)
ØØ/ 0
.
ØØ0 1

WriteToken
ØØ1 ;
(
ØØ; <
token
ØØ< A
)
ØØA B
;
ØØB C
}
∞∞ 	
}
±± 
}≤≤ Æ√
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\AppointmentService.cs
	namespace 	
HealthCareApp
 
. 
Services  
.  !
Impl! %
{ 
public 

class 
AppointmentService #
(# $"
IAppointmentRepository !
appointmentRepository 4
,4 5
IPatientRepository 
patientRepository ,
,, -
IDoctorRepository 
doctorRepository *
,* +#
IHealthRecordRepository "
healthRecordRepository  6
,6 7
IMapper 
mapper 
) 
: 
IAppointmentService -
{ 
private 
const 
string !
AppointmentEntityName 2
=3 4
$str5 B
;B C
private 
const 
string -
!AppointmentDetailsRequiredMessage >
=? @
$strA d
;d e
private 
const 
string .
"CancellationDetailsRequiredMessage ?
=@ A
$strB f
;f g
public 
async 
Task 
< 
List 
< 
AppointmentDto -
>- .
>. /#
GetAllAppointmentsAsync0 G
(G H
)H I
{ 	
var 
appointments 
= 
await $!
appointmentRepository% :
.: ;
GetAllAsync; F
(F G
)G H
;H I
return 
mapper 
. 
Map 
< 
List "
<" #
AppointmentDto# 1
>1 2
>2 3
(3 4
appointments4 @
)@ A
;A B
} 	
public 
async 
Task 
< 
PagedResponse '
<' (
AppointmentDto( 6
>6 7
>7 8(
GetAllAppointmentsPagedAsync9 U
(U V)
AppointmentPaginationQueryDtoV s
queryt y
)y z
{   
query!! 
??=!! 
new!! )
AppointmentPaginationQueryDto!! 7
(!!7 8
)!!8 9
;!!9 :
int## 

pageNumber## 
=## 
query## "
.##" #

PageNumber### -
<=##. 0
$num##1 2
?##3 4
$num##5 6
:##7 8
query##9 >
.##> ?

PageNumber##? I
;##I J
int%% 
pageSize%% 
=%% 
query%% 
.%% 
PageSize%% !
<=%%" $
$num%%% &
?%%' (
$num%%) +
:%%, -
query%%. 3
.%%3 4
PageSize%%4 <
;%%< =
pageSize'' 
='' 
pageSize'' 
>'' 
$num'' 
?'' 
$num''  #
:''$ %
pageSize''& .
;''. /
var)) 
appointments)) 
=)) 
await)) !
appointmentRepository)) 2
.))2 3
GetAllAsync))3 >
())> ?
)))? @
;))@ A
var++  
filteredAppointments++ 
=++ 
appointments++ +
.+++ ,
AsEnumerable++, 8
(++8 9
)++9 :
;++: ;
if-- 
(-- 
!-- 	
string--	 
.-- 
IsNullOrWhiteSpace-- "
(--" #
query--# (
.--( )

SearchTerm--) 3
)--3 4
)--4 5
{.. 
string// 

searchTerm// 
=// 
query// !
.//! "

SearchTerm//" ,
.//, -
Trim//- 1
(//1 2
)//2 3
;//3 4 
filteredAppointments11 
=11  
filteredAppointments11 3
.113 4
Where114 9
(119 :
a11: ;
=>11< >
(22 
a22 
.22 
Patient22 
!=22 
null22 
&&22 !
a33 
.33 
Patient33 
.33 
PatientName33 "
.33" #
Contains33# +
(33+ ,

searchTerm33, 6
,336 7
StringComparison338 H
.33H I
OrdinalIgnoreCase33I Z
)33Z [
)33[ \
||33] _
(44 
a44 
.44 
Doctor44 
!=44 
null44 
&&44  
a55 
.55 
Doctor55 
.55 

DoctorName55  
.55  !
Contains55! )
(55) *

searchTerm55* 4
,554 5
StringComparison556 F
.55F G
OrdinalIgnoreCase55G X
)55X Y
)55Y Z
||55[ ]
a66 
.66 
TimeSlot66 
.66 
Contains66 
(66  

searchTerm66  *
,66* +
StringComparison66, <
.66< =
OrdinalIgnoreCase66= N
)66N O
||66P R
(77 
!77 
string77 
.77 
IsNullOrWhiteSpace77 '
(77' (
a77( )
.77) *
CancellationReason77* <
)77< =
&&77> @
a88 
.88 
CancellationReason88 !
.88! "
Contains88" *
(88* +

searchTerm88+ 5
,885 6
StringComparison887 G
.88G H
OrdinalIgnoreCase88H Y
)88Y Z
)88Z [
)88[ \
;88\ ]
}99 
if;; 
(;; 
query;; 
.;; 
	PatientId;; 
is;; 
not;; 
null;; #
);;# $
{<<  
filteredAppointments== 
===  
filteredAppointments== 3
.==3 4
Where==4 9
(==9 :
a==: ;
=>==< >
a>> 
.>> 
	PatientId>> 
==>> 
query>>  
.>>  !
	PatientId>>! *
.>>* +
Value>>+ 0
)>>0 1
;>>1 2
}?? 
ifAA 
(AA 
queryAA 
.AA 
DoctorIdAA 
isAA 
notAA 
nullAA "
)AA" #
{BB  
filteredAppointmentsCC 
=CC  
filteredAppointmentsCC 3
.CC3 4
WhereCC4 9
(CC9 :
aCC: ;
=>CC< >
aDD 
.DD 
DoctorIdDD 
==DD 
queryDD 
.DD  
DoctorIdDD  (
.DD( )
ValueDD) .
)DD. /
;DD/ 0
}EE 
ifGG 
(GG 
queryGG 
.GG 
StatusGG 
isGG 
notGG 
nullGG  
)GG  !
{HH  
filteredAppointmentsII 
=II  
filteredAppointmentsII 3
.II3 4
WhereII4 9
(II9 :
aII: ;
=>II< >
aJJ 
.JJ 
StatusJJ 
==JJ 
queryJJ 
.JJ 
StatusJJ $
.JJ$ %
ValueJJ% *
)JJ* +
;JJ+ ,
}KK 
ifMM 
(MM 
queryMM 
.MM 
ScheduledDateMM 
isMM 
notMM "
nullMM# '
)MM' (
{NN  
filteredAppointmentsOO 
=OO  
filteredAppointmentsOO 3
.OO3 4
WhereOO4 9
(OO9 :
aOO: ;
=>OO< >
aPP 
.PP 
ScheduledDatePP 
.PP 
DatePP  
==PP! #
queryPP$ )
.PP) *
ScheduledDatePP* 7
.PP7 8
ValuePP8 =
.PP= >
DatePP> B
)PPB C
;PPC D
}QQ 
ifSS 
(SS 
querySS 
.SS 
UpcomingOnlySS 
isSS 
notSS !
nullSS" &
&&SS' )
querySS* /
.SS/ 0
UpcomingOnlySS0 <
.SS< =
ValueSS= B
)SSB C
{TT  
filteredAppointmentsUU 
=UU  
filteredAppointmentsUU 3
.UU3 4
WhereUU4 9
(UU9 :
aUU: ;
=>UU< >
aVV 
.VV 
ScheduledDateVV 
.VV 
DateVV  
>=VV! #
DateTimeVV$ ,
.VV, -
TodayVV- 2
&&VV3 5
aWW 
.WW 
StatusWW 
!=WW 
AppointmentStatusWW )
.WW) *
	CancelledWW* 3
&&WW4 6
aXX 
.XX 
StatusXX 
!=XX 
AppointmentStatusXX )
.XX) *
	CompletedXX* 3
)XX3 4
;XX4 5
}YY 
int[[ 
totalRecords[[ 
=[[  
filteredAppointments[[ +
.[[+ ,
Count[[, 1
([[1 2
)[[2 3
;[[3 4
var]] 
pagedAppointments]] 
=]]  
filteredAppointments]] 0
.^^ 	
OrderByDescending^^	 
(^^ 
a^^ 
=>^^ 
a^^  !
.^^! "
ScheduledDate^^" /
)^^/ 0
.__ 	
ThenBy__	 
(__ 
a__ 
=>__ 
a__ 
.__ 
TimeSlot__ 
)__  
.`` 	
Skip``	 
(`` 
(`` 

pageNumber`` 
-`` 
$num`` 
)`` 
*``  
pageSize``! )
)``) *
.aa 	
Takeaa	 
(aa 
pageSizeaa 
)aa 
.bb 	
ToListbb	 
(bb 
)bb 
;bb 
vardd 
mappedAppointmentsdd 
=dd 
mapperdd #
.dd# $
Mapdd$ '
<dd' (
Listdd( ,
<dd, -
AppointmentDtodd- ;
>dd; <
>dd< =
(dd= >
pagedAppointmentsdd> O
)ddO P
;ddP Q
returnff 

newff 
PagedResponseff 
<ff 
AppointmentDtoff +
>ff+ ,
{gg 
Itemshh 
=hh 
mappedAppointmentshh "
,hh" #

PageNumberii 
=ii 

pageNumberii 
,ii  
PageSizejj 
=jj 
pageSizejj 
,jj 
TotalRecordskk 
=kk 
totalRecordskk #
,kk# $

TotalPagesll 
=ll 
(ll 
intll 
)ll 
Mathll 
.ll 
Ceilingll &
(ll& '
totalRecordsll' 3
/ll4 5
(ll6 7
doublell7 =
)ll= >
pageSizell> F
)llF G
}mm 
;mm 
}nn 
publicpp 
asyncpp 
Taskpp 
<pp 
AppointmentDtopp (
>pp( )#
GetAppointmentByIdAsyncpp* A
(ppA B
intppB E
appointmentIdppF S
)ppS T
{qq 	!
ValidateAppointmentIdrr !
(rr! "
appointmentIdrr" /
)rr/ 0
;rr0 1
vartt 
appointmenttt 
=tt 
awaittt #!
appointmentRepositorytt$ 9
.tt9 :
GetByIdAsynctt: F
(ttF G
appointmentIdttG T
)ttT U
;ttU V
ifvv 
(vv 
appointmentvv 
isvv 
nullvv #
)vv# $
{ww 
throwxx 
newxx #
EntityNotFoundExceptionxx 1
(xx1 2!
AppointmentEntityNamexx2 G
,xxG H
appointmentIdxxI V
)xxV W
;xxW X
}yy 
return{{ 
mapper{{ 
.{{ 
Map{{ 
<{{ 
AppointmentDto{{ ,
>{{, -
({{- .
appointment{{. 9
){{9 :
;{{: ;
}|| 	
public~~ 
async~~ 
Task~~ 
<~~ 
List~~ 
<~~ 
AppointmentDto~~ -
>~~- .
>~~. /+
GetAppointmentsByPatientIdAsync~~0 O
(~~O P
int~~P S
	patientId~~T ]
)~~] ^
{ 	
await
ÄÄ (
ValidatePatientExistsAsync
ÄÄ ,
(
ÄÄ, -
	patientId
ÄÄ- 6
)
ÄÄ6 7
;
ÄÄ7 8
var
ÇÇ 
appointments
ÇÇ 
=
ÇÇ 
await
ÇÇ $#
appointmentRepository
ÇÇ% :
.
ÇÇ: ;!
GetByPatientIdAsync
ÇÇ; N
(
ÇÇN O
	patientId
ÇÇO X
)
ÇÇX Y
;
ÇÇY Z
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
ÑÑ3 4
appointments
ÑÑ4 @
)
ÑÑ@ A
;
ÑÑA B
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
List
áá 
<
áá 
AppointmentDto
áá -
>
áá- .
>
áá. /,
GetAppointmentsByDoctorIdAsync
áá0 N
(
ááN O
int
ááO R
doctorId
ááS [
)
áá[ \
{
àà 	
await
ââ '
ValidateDoctorExistsAsync
ââ +
(
ââ+ ,
doctorId
ââ, 4
)
ââ4 5
;
ââ5 6
var
ãã 
appointments
ãã 
=
ãã 
await
ãã $#
appointmentRepository
ãã% :
.
ãã: ; 
GetByDoctorIdAsync
ãã; M
(
ããM N
doctorId
ããN V
)
ããV W
;
ããW X
return
çç 
mapper
çç 
.
çç 
Map
çç 
<
çç 
List
çç "
<
çç" #
AppointmentDto
çç# 1
>
çç1 2
>
çç2 3
(
çç3 4
appointments
çç4 @
)
çç@ A
;
ççA B
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
List
êê 
<
êê 
AppointmentDto
êê -
>
êê- .
>
êê. /*
GetAppointmentsByStatusAsync
êê0 L
(
êêL M
AppointmentStatus
êêM ^
status
êê_ e
)
êêe f
{
ëë 	
var
íí 
appointments
íí 
=
íí 
await
íí $#
appointmentRepository
íí% :
.
íí: ;
GetByStatusAsync
íí; K
(
ííK L
status
ííL R
)
ííR S
;
ííS T
return
îî 
mapper
îî 
.
îî 
Map
îî 
<
îî 
List
îî "
<
îî" #
AppointmentDto
îî# 1
>
îî1 2
>
îî2 3
(
îî3 4
appointments
îî4 @
)
îî@ A
;
îîA B
}
ïï 	
public
óó 
async
óó 
Task
óó 
<
óó 
List
óó 
<
óó 
AppointmentDto
óó -
>
óó- .
>
óó. /*
GetUpcomingAppointmentsAsync
óó0 L
(
óóL M
)
óóM N
{
òò 	
var
ôô 
appointments
ôô 
=
ôô 
await
ôô $#
appointmentRepository
ôô% :
.
ôô: ;*
GetUpcomingAppointmentsAsync
ôô; W
(
ôôW X
)
ôôX Y
;
ôôY Z
return
õõ 
mapper
õõ 
.
õõ 
Map
õõ 
<
õõ 
List
õõ "
<
õõ" #
AppointmentDto
õõ# 1
>
õõ1 2
>
õõ2 3
(
õõ3 4
appointments
õõ4 @
)
õõ@ A
;
õõA B
}
úú 	
public
ûû 
async
ûû 
Task
ûû 
<
ûû 
List
ûû 
<
ûû 
AppointmentDto
ûû -
>
ûû- .
>
ûû. /5
'GetUpcomingAppointmentsByPatientIdAsync
ûû0 W
(
ûûW X
int
ûûX [
	patientId
ûû\ e
)
ûûe f
{
üü 	
await
†† (
ValidatePatientExistsAsync
†† ,
(
††, -
	patientId
††- 6
)
††6 7
;
††7 8
var
¢¢ 
appointments
¢¢ 
=
¢¢ 
await
¢¢ $#
appointmentRepository
¢¢% :
.
¢¢: ;5
'GetUpcomingAppointmentsByPatientIdAsync
¢¢; b
(
¢¢b c
	patientId
¢¢c l
)
¢¢l m
;
¢¢m n
return
§§ 
mapper
§§ 
.
§§ 
Map
§§ 
<
§§ 
List
§§ "
<
§§" #
AppointmentDto
§§# 1
>
§§1 2
>
§§2 3
(
§§3 4
appointments
§§4 @
)
§§@ A
;
§§A B
}
•• 	
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
List
ßß 
<
ßß 
AppointmentDto
ßß -
>
ßß- .
>
ßß. /4
&GetUpcomingAppointmentsByDoctorIdAsync
ßß0 V
(
ßßV W
int
ßßW Z
doctorId
ßß[ c
)
ßßc d
{
®® 	
await
©© '
ValidateDoctorExistsAsync
©© +
(
©©+ ,
doctorId
©©, 4
)
©©4 5
;
©©5 6
var
´´ 
appointments
´´ 
=
´´ 
await
´´ $#
appointmentRepository
´´% :
.
´´: ;4
&GetUpcomingAppointmentsByDoctorIdAsync
´´; a
(
´´a b
doctorId
´´b j
)
´´j k
;
´´k l
return
≠≠ 
mapper
≠≠ 
.
≠≠ 
Map
≠≠ 
<
≠≠ 
List
≠≠ "
<
≠≠" #
AppointmentDto
≠≠# 1
>
≠≠1 2
>
≠≠2 3
(
≠≠3 4
appointments
≠≠4 @
)
≠≠@ A
;
≠≠A B
}
ÆÆ 	
public
∞∞ 
async
∞∞ 
Task
∞∞ 
<
∞∞ 
List
∞∞ 
<
∞∞ 
AppointmentDto
∞∞ -
>
∞∞- .
>
∞∞. /4
&GetPendingAppointmentsByPatientIdAsync
∞∞0 V
(
∞∞V W
int
∞∞W Z
	patientId
∞∞[ d
)
∞∞d e
{
±± 	
await
≤≤ (
ValidatePatientExistsAsync
≤≤ ,
(
≤≤, -
	patientId
≤≤- 6
)
≤≤6 7
;
≤≤7 8
var
¥¥ 
appointments
¥¥ 
=
¥¥ 
await
¥¥ $#
appointmentRepository
¥¥% :
.
¥¥: ;4
&GetPendingAppointmentsByPatientIdAsync
¥¥; a
(
¥¥a b
	patientId
¥¥b k
)
¥¥k l
;
¥¥l m
return
∂∂ 
mapper
∂∂ 
.
∂∂ 
Map
∂∂ 
<
∂∂ 
List
∂∂ "
<
∂∂" #
AppointmentDto
∂∂# 1
>
∂∂1 2
>
∂∂2 3
(
∂∂3 4
appointments
∂∂4 @
)
∂∂@ A
;
∂∂A B
}
∑∑ 	
public
ππ 
async
ππ 
Task
ππ 
<
ππ 
List
ππ 
<
ππ 
AppointmentDto
ππ -
>
ππ- .
>
ππ. /3
%GetPendingAppointmentsByDoctorIdAsync
ππ0 U
(
ππU V
int
ππV Y
doctorId
ππZ b
)
ππb c
{
∫∫ 	
await
ªª '
ValidateDoctorExistsAsync
ªª +
(
ªª+ ,
doctorId
ªª, 4
)
ªª4 5
;
ªª5 6
var
ΩΩ 
appointments
ΩΩ 
=
ΩΩ 
await
ΩΩ $#
appointmentRepository
ΩΩ% :
.
ΩΩ: ;3
%GetPendingAppointmentsByDoctorIdAsync
ΩΩ; `
(
ΩΩ` a
doctorId
ΩΩa i
)
ΩΩi j
;
ΩΩj k
return
øø 
mapper
øø 
.
øø 
Map
øø 
<
øø 
List
øø "
<
øø" #
AppointmentDto
øø# 1
>
øø1 2
>
øø2 3
(
øø3 4
appointments
øø4 @
)
øø@ A
;
øøA B
}
¿¿ 	
public
¬¬ 
async
¬¬ 
Task
¬¬ 
<
¬¬ 
List
¬¬ 
<
¬¬ 
AppointmentDto
¬¬ -
>
¬¬- .
>
¬¬. /:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
¬¬0 \
(
¬¬\ ]
int
¬¬] `
doctorId
¬¬a i
)
¬¬i j
{
√√ 	
await
ƒƒ '
ValidateDoctorExistsAsync
ƒƒ +
(
ƒƒ+ ,
doctorId
ƒƒ, 4
)
ƒƒ4 5
;
ƒƒ5 6
var
∆∆ 
appointments
∆∆ 
=
∆∆ 
await
∆∆ $#
appointmentRepository
∆∆% :
.
∆∆: ;:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
∆∆; g
(
∆∆g h
doctorId
∆∆h p
)
∆∆p q
;
∆∆q r
return
»» 
mapper
»» 
.
»» 
Map
»» 
<
»» 
List
»» "
<
»»" #
AppointmentDto
»»# 1
>
»»1 2
>
»»2 3
(
»»3 4
appointments
»»4 @
)
»»@ A
;
»»A B
}
…… 	
public
ÀÀ 
async
ÀÀ 
Task
ÀÀ 
<
ÀÀ 
AppointmentDto
ÀÀ (
>
ÀÀ( )"
BookAppointmentAsync
ÀÀ* >
(
ÀÀ> ? 
BookAppointmentDto
ÀÀ? Q
dto
ÀÀR U
)
ÀÀU V
{
ÃÃ 	
if
ÕÕ 
(
ÕÕ 
dto
ÕÕ 
is
ÕÕ 
null
ÕÕ 
)
ÕÕ 
{
ŒŒ 
throw
œœ 
new
œœ &
AppointmentRuleException
œœ 2
(
œœ2 3/
!AppointmentDetailsRequiredMessage
œœ3 T
)
œœT U
;
œœU V
}
–– 
await
““ (
ValidatePatientExistsAsync
““ ,
(
““, -
dto
““- 0
.
““0 1
	PatientId
““1 :
)
““: ;
;
““; <
var
‘‘ 
doctor
‘‘ 
=
‘‘ 
await
‘‘ '
ValidateDoctorExistsAsync
‘‘ 8
(
‘‘8 9
dto
‘‘9 <
.
‘‘< =
DoctorId
‘‘= E
)
‘‘E F
;
‘‘F G(
ValidateDoctorAvailability
÷÷ &
(
÷÷& '
doctor
÷÷' -
)
÷÷- .
;
÷÷. /%
ValidateAppointmentDate
ÿÿ #
(
ÿÿ# $
dto
ÿÿ$ '
.
ÿÿ' (
ScheduledDate
ÿÿ( 5
)
ÿÿ5 6
;
ÿÿ6 7
ValidateTimeSlot
⁄⁄ 
(
⁄⁄ 
dto
⁄⁄  
.
⁄⁄  !
TimeSlot
⁄⁄! )
)
⁄⁄) *
;
⁄⁄* +
var
‹‹ 
isSlotBooked
‹‹ 
=
‹‹ 
await
‹‹ $#
appointmentRepository
‹‹% :
.
‹‹: ;
IsSlotBookedAsync
‹‹; L
(
‹‹L M
dto
›› 
.
›› 
DoctorId
›› 
,
›› 
dto
ﬁﬁ 
.
ﬁﬁ 
ScheduledDate
ﬁﬁ !
.
ﬁﬁ! "
Date
ﬁﬁ" &
,
ﬁﬁ& '
dto
ﬂﬂ 
.
ﬂﬂ 
TimeSlot
ﬂﬂ 
)
ﬂﬂ 
;
ﬂﬂ 
if
·· 
(
·· 
isSlotBooked
·· 
)
·· 
{
‚‚ 
throw
„„ 
new
„„ 
ConflictException
„„ +
(
„„+ ,
$str
„„, g
)
„„g h
;
„„h i
}
‰‰ 
var
ÊÊ  
patientHasSameSlot
ÊÊ "
=
ÊÊ# $
await
ÊÊ% *#
appointmentRepository
ÊÊ+ @
.
ÊÊ@ A;
-PatientHasActiveAppointmentOnDateAndSlotAsync
ÊÊA n
(
ÊÊn o
dto
ÁÁ 
.
ÁÁ 
	PatientId
ÁÁ 
,
ÁÁ 
dto
ËË 
.
ËË 
ScheduledDate
ËË !
.
ËË! "
Date
ËË" &
,
ËË& '
dto
ÈÈ 
.
ÈÈ 
TimeSlot
ÈÈ 
)
ÈÈ 
;
ÈÈ 
if
ÎÎ 
(
ÎÎ  
patientHasSameSlot
ÎÎ "
)
ÎÎ" #
{
ÏÏ 
throw
ÌÌ 
new
ÌÌ 
ConflictException
ÌÌ +
(
ÌÌ+ ,
$str
ÌÌ, j
)
ÌÌj k
;
ÌÌk l
}
ÓÓ 
var
 -
patientHasAppointmentWithDoctor
 /
=
0 1
await
2 7#
appointmentRepository
8 M
.
M N>
0PatientHasActiveAppointmentWithDoctorOnDateAsync
N ~
(
~ 
dto
ÒÒ 
.
ÒÒ 
	PatientId
ÒÒ 
,
ÒÒ 
dto
ÚÚ 
.
ÚÚ 
DoctorId
ÚÚ 
,
ÚÚ 
dto
ÛÛ 
.
ÛÛ 
ScheduledDate
ÛÛ !
.
ÛÛ! "
Date
ÛÛ" &
)
ÛÛ& '
;
ÛÛ' (
if
ıı 
(
ıı -
patientHasAppointmentWithDoctor
ıı /
)
ıı/ 0
{
ˆˆ 
throw
˜˜ 
new
˜˜ 
ConflictException
˜˜ +
(
˜˜+ ,
$str
˜˜, ~
)
˜˜~ 
;˜˜ Ä
}
¯¯ 
var
˙˙ 
appointment
˙˙ 
=
˙˙ 
mapper
˙˙ $
.
˙˙$ %
Map
˙˙% (
<
˙˙( )
Appointment
˙˙) 4
>
˙˙4 5
(
˙˙5 6
dto
˙˙6 9
)
˙˙9 :
;
˙˙: ;
appointment
¸¸ 
.
¸¸ 
ScheduledDate
¸¸ %
=
¸¸& '
dto
¸¸( +
.
¸¸+ ,
ScheduledDate
¸¸, 9
.
¸¸9 :
Date
¸¸: >
;
¸¸> ?
appointment
˝˝ 
.
˝˝ 
Status
˝˝ 
=
˝˝  
AppointmentStatus
˝˝! 2
.
˝˝2 3
Pending
˝˝3 :
;
˝˝: ;
appointment
˛˛ 
.
˛˛  
CancellationReason
˛˛ *
=
˛˛+ ,
null
˛˛- 1
;
˛˛1 2
appointment
ˇˇ 
.
ˇˇ 
CreatedDate
ˇˇ #
=
ˇˇ$ %
DateTime
ˇˇ& .
.
ˇˇ. /
Now
ˇˇ/ 2
;
ˇˇ2 3
var
ÅÅ 
savedAppointment
ÅÅ  
=
ÅÅ! "
await
ÅÅ# (#
appointmentRepository
ÅÅ) >
.
ÅÅ> ?
CreateAsync
ÅÅ? J
(
ÅÅJ K
appointment
ÅÅK V
)
ÅÅV W
;
ÅÅW X
return
ÉÉ 
mapper
ÉÉ 
.
ÉÉ 
Map
ÉÉ 
<
ÉÉ 
AppointmentDto
ÉÉ ,
>
ÉÉ, -
(
ÉÉ- .
savedAppointment
ÉÉ. >
)
ÉÉ> ?
;
ÉÉ? @
}
ÑÑ 	
public
ÜÜ 
async
ÜÜ 
Task
ÜÜ 
<
ÜÜ 
AppointmentDto
ÜÜ (
>
ÜÜ( )$
UpdateAppointmentAsync
ÜÜ* @
(
ÜÜ@ A
int
ÜÜA D
appointmentId
ÜÜE R
,
ÜÜR S"
UpdateAppointmentDto
ÜÜT h
dto
ÜÜi l
)
ÜÜl m
{
áá 	#
ValidateAppointmentId
àà !
(
àà! "
appointmentId
àà" /
)
àà/ 0
;
àà0 1
if
ää 
(
ää 
dto
ää 
is
ää 
null
ää 
)
ää 
{
ãã 
throw
åå 
new
åå &
AppointmentRuleException
åå 2
(
åå2 3/
!AppointmentDetailsRequiredMessage
åå3 T
)
ååT U
;
ååU V
}
çç 
var
èè !
existingAppointment
èè #
=
èè$ %
await
èè& +#
appointmentRepository
èè, A
.
èèA B
GetByIdAsync
èèB N
(
èèN O
appointmentId
èèO \
)
èè\ ]
;
èè] ^
if
ëë 
(
ëë !
existingAppointment
ëë #
is
ëë$ &
null
ëë' +
)
ëë+ ,
{
íí 
throw
ìì 
new
ìì %
EntityNotFoundException
ìì 1
(
ìì1 2#
AppointmentEntityName
ìì2 G
,
ììG H
appointmentId
ììI V
)
ììV W
;
ììW X
}
îî 
await
ññ (
ValidatePatientExistsAsync
ññ ,
(
ññ, -
dto
ññ- 0
.
ññ0 1
	PatientId
ññ1 :
)
ññ: ;
;
ññ; <
var
òò 
doctor
òò 
=
òò 
await
òò '
ValidateDoctorExistsAsync
òò 8
(
òò8 9
dto
òò9 <
.
òò< =
DoctorId
òò= E
)
òòE F
;
òòF G(
ValidateDoctorAvailability
öö &
(
öö& '
doctor
öö' -
)
öö- .
;
öö. /%
ValidateAppointmentDate
úú #
(
úú# $
dto
úú$ '
.
úú' (
ScheduledDate
úú( 5
)
úú5 6
;
úú6 7
ValidateTimeSlot
ûû 
(
ûû 
dto
ûû  
.
ûû  !
TimeSlot
ûû! )
)
ûû) *
;
ûû* +
bool
†† 
	slotTaken
†† 
=
†† 
await
†† "#
appointmentRepository
††# 8
.
††8 9
IsSlotBookedAsync
††9 J
(
††J K
dto
°° 
.
°° 
DoctorId
°° 
,
°° 
dto
¢¢ 
.
¢¢ 
ScheduledDate
¢¢ !
.
¢¢! "
Date
¢¢" &
,
¢¢& '
dto
££ 
.
££ 
TimeSlot
££ 
)
££ 
;
££ 
bool
•• 
sameExistingSlot
•• !
=
••" #!
existingAppointment
¶¶ #
.
¶¶# $
DoctorId
¶¶$ ,
==
¶¶- /
dto
¶¶0 3
.
¶¶3 4
DoctorId
¶¶4 <
&&
¶¶= ?!
existingAppointment
ßß #
.
ßß# $
ScheduledDate
ßß$ 1
.
ßß1 2
Date
ßß2 6
==
ßß7 9
dto
ßß: =
.
ßß= >
ScheduledDate
ßß> K
.
ßßK L
Date
ßßL P
&&
ßßQ S!
existingAppointment
®® #
.
®®# $
TimeSlot
®®$ ,
==
®®- /
dto
®®0 3
.
®®3 4
TimeSlot
®®4 <
;
®®< =
if
™™ 
(
™™ 
	slotTaken
™™ 
&&
™™ 
!
™™ 
sameExistingSlot
™™ .
)
™™. /
{
´´ 
throw
¨¨ 
new
¨¨ 
ConflictException
¨¨ +
(
¨¨+ ,
$str
¨¨, g
)
¨¨g h
;
¨¨h i
}
≠≠ 
mapper
ØØ 
.
ØØ 
Map
ØØ 
(
ØØ 
dto
ØØ 
,
ØØ !
existingAppointment
ØØ /
)
ØØ/ 0
;
ØØ0 1!
existingAppointment
±± 
.
±±  
AppointmentId
±±  -
=
±±. /
appointmentId
±±0 =
;
±±= >!
existingAppointment
≤≤ 
.
≤≤  
ScheduledDate
≤≤  -
=
≤≤. /
dto
≤≤0 3
.
≤≤3 4
ScheduledDate
≤≤4 A
.
≤≤A B
Date
≤≤B F
;
≤≤F G
var
¥¥  
updatedAppointment
¥¥ "
=
¥¥# $
await
¥¥% *#
appointmentRepository
¥¥+ @
.
¥¥@ A
UpdateAsync
¥¥A L
(
¥¥L M
appointmentId
µµ 
,
µµ !
existingAppointment
∂∂ #
)
∂∂# $
;
∂∂$ %
if
∏∏ 
(
∏∏  
updatedAppointment
∏∏ "
is
∏∏# %
null
∏∏& *
)
∏∏* +
{
ππ 
throw
∫∫ 
new
∫∫ %
EntityNotFoundException
∫∫ 1
(
∫∫1 2#
AppointmentEntityName
∫∫2 G
,
∫∫G H
appointmentId
∫∫I V
)
∫∫V W
;
∫∫W X
}
ªª 
return
ΩΩ 
mapper
ΩΩ 
.
ΩΩ 
Map
ΩΩ 
<
ΩΩ 
AppointmentDto
ΩΩ ,
>
ΩΩ, -
(
ΩΩ- . 
updatedAppointment
ΩΩ. @
)
ΩΩ@ A
;
ΩΩA B
}
ææ 	
public
¿¿ 
async
¿¿ 
Task
¿¿ 
<
¿¿ 
AppointmentDto
¿¿ (
>
¿¿( )%
ConfirmAppointmentAsync
¿¿* A
(
¿¿A B
int
¿¿B E
appointmentId
¿¿F S
)
¿¿S T
{
¡¡ 	#
ValidateAppointmentId
¬¬ !
(
¬¬! "
appointmentId
¬¬" /
)
¬¬/ 0
;
¬¬0 1
var
ƒƒ 
appointment
ƒƒ 
=
ƒƒ 
await
ƒƒ ##
appointmentRepository
ƒƒ$ 9
.
ƒƒ9 :
GetByIdAsync
ƒƒ: F
(
ƒƒF G
appointmentId
ƒƒG T
)
ƒƒT U
;
ƒƒU V
if
∆∆ 
(
∆∆ 
appointment
∆∆ 
is
∆∆ 
null
∆∆ #
)
∆∆# $
{
«« 
throw
»» 
new
»» %
EntityNotFoundException
»» 1
(
»»1 2#
AppointmentEntityName
»»2 G
,
»»G H
appointmentId
»»I V
)
»»V W
;
»»W X
}
…… 
if
ÀÀ 
(
ÀÀ 
appointment
ÀÀ 
.
ÀÀ 
Status
ÀÀ "
==
ÀÀ# %
AppointmentStatus
ÀÀ& 7
.
ÀÀ7 8
	Cancelled
ÀÀ8 A
)
ÀÀA B
{
ÃÃ 
throw
ÕÕ 
new
ÕÕ 
ConflictException
ÕÕ +
(
ÕÕ+ ,
$str
ÕÕ, X
)
ÕÕX Y
;
ÕÕY Z
}
ŒŒ 
if
–– 
(
–– 
appointment
–– 
.
–– 
Status
–– "
==
––# %
AppointmentStatus
––& 7
.
––7 8
	Completed
––8 A
)
––A B
{
—— 
throw
““ 
new
““ 
ConflictException
““ +
(
““+ ,
$str
““, ^
)
““^ _
;
““_ `
}
”” 
if
’’ 
(
’’ 
appointment
’’ 
.
’’ 
Status
’’ "
==
’’# %
AppointmentStatus
’’& 7
.
’’7 8
	Confirmed
’’8 A
)
’’A B
{
÷÷ 
throw
◊◊ 
new
◊◊ 
ConflictException
◊◊ +
(
◊◊+ ,
$str
◊◊, O
)
◊◊O P
;
◊◊P Q
}
ÿÿ 
appointment
⁄⁄ 
.
⁄⁄ 
Status
⁄⁄ 
=
⁄⁄  
AppointmentStatus
⁄⁄! 2
.
⁄⁄2 3
	Confirmed
⁄⁄3 <
;
⁄⁄< =
appointment
€€ 
.
€€  
CancellationReason
€€ *
=
€€+ ,
null
€€- 1
;
€€1 2
var
››  
updatedAppointment
›› "
=
››# $
await
››% *#
appointmentRepository
››+ @
.
››@ A
UpdateAsync
››A L
(
››L M
appointmentId
ﬁﬁ 
,
ﬁﬁ 
appointment
ﬂﬂ 
)
ﬂﬂ 
;
ﬂﬂ 
if
·· 
(
··  
updatedAppointment
·· "
is
··# %
null
··& *
)
··* +
{
‚‚ 
throw
„„ 
new
„„ %
EntityNotFoundException
„„ 1
(
„„1 2#
AppointmentEntityName
„„2 G
,
„„G H
appointmentId
„„I V
)
„„V W
;
„„W X
}
‰‰ 
return
ÊÊ 
mapper
ÊÊ 
.
ÊÊ 
Map
ÊÊ 
<
ÊÊ 
AppointmentDto
ÊÊ ,
>
ÊÊ, -
(
ÊÊ- . 
updatedAppointment
ÊÊ. @
)
ÊÊ@ A
;
ÊÊA B
}
ÁÁ 	
public
ÈÈ 
async
ÈÈ 
Task
ÈÈ 
<
ÈÈ 
AppointmentDto
ÈÈ (
>
ÈÈ( )&
CompleteAppointmentAsync
ÈÈ* B
(
ÈÈB C
int
ÈÈC F
appointmentId
ÈÈG T
)
ÈÈT U
{
ÍÍ 	#
ValidateAppointmentId
ÎÎ !
(
ÎÎ! "
appointmentId
ÎÎ" /
)
ÎÎ/ 0
;
ÎÎ0 1
var
ÌÌ 
appointment
ÌÌ 
=
ÌÌ 
await
ÌÌ ##
appointmentRepository
ÌÌ$ 9
.
ÌÌ9 :
GetByIdAsync
ÌÌ: F
(
ÌÌF G
appointmentId
ÌÌG T
)
ÌÌT U
;
ÌÌU V
if
ÔÔ 
(
ÔÔ 
appointment
ÔÔ 
is
ÔÔ 
null
ÔÔ #
)
ÔÔ# $
{
 
throw
ÒÒ 
new
ÒÒ %
EntityNotFoundException
ÒÒ 1
(
ÒÒ1 2#
AppointmentEntityName
ÒÒ2 G
,
ÒÒG H
appointmentId
ÒÒI V
)
ÒÒV W
;
ÒÒW X
}
ÚÚ 
if
ÙÙ 
(
ÙÙ 
appointment
ÙÙ 
.
ÙÙ 
Status
ÙÙ "
==
ÙÙ# %
AppointmentStatus
ÙÙ& 7
.
ÙÙ7 8
	Cancelled
ÙÙ8 A
)
ÙÙA B
{
ıı 
throw
ˆˆ 
new
ˆˆ 
ConflictException
ˆˆ +
(
ˆˆ+ ,
$str
ˆˆ, X
)
ˆˆX Y
;
ˆˆY Z
}
˜˜ 
if
˘˘ 
(
˘˘ 
appointment
˘˘ 
.
˘˘ 
Status
˘˘ "
==
˘˘# %
AppointmentStatus
˘˘& 7
.
˘˘7 8
	Completed
˘˘8 A
)
˘˘A B
{
˙˙ 
throw
˚˚ 
new
˚˚ 
ConflictException
˚˚ +
(
˚˚+ ,
$str
˚˚, O
)
˚˚O P
;
˚˚P Q
}
¸¸ 
if
˛˛ 
(
˛˛ 
appointment
˛˛ 
.
˛˛ 
Status
˛˛ "
!=
˛˛# %
AppointmentStatus
˛˛& 7
.
˛˛7 8
	Confirmed
˛˛8 A
)
˛˛A B
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ &
AppointmentRuleException
ÄÄ 2
(
ÄÄ2 3
$str
ÄÄ3 b
)
ÄÄb c
;
ÄÄc d
}
ÅÅ 
appointment
ÉÉ 
.
ÉÉ 
Status
ÉÉ 
=
ÉÉ  
AppointmentStatus
ÉÉ! 2
.
ÉÉ2 3
	Completed
ÉÉ3 <
;
ÉÉ< =
var
ÖÖ  
updatedAppointment
ÖÖ "
=
ÖÖ# $
await
ÖÖ% *#
appointmentRepository
ÖÖ+ @
.
ÖÖ@ A
UpdateAsync
ÖÖA L
(
ÖÖL M
appointmentId
ÜÜ 
,
ÜÜ 
appointment
áá 
)
áá 
;
áá 
if
ââ 
(
ââ  
updatedAppointment
ââ "
is
ââ# %
null
ââ& *
)
ââ* +
{
ää 
throw
ãã 
new
ãã %
EntityNotFoundException
ãã 1
(
ãã1 2#
AppointmentEntityName
ãã2 G
,
ããG H
appointmentId
ããI V
)
ããV W
;
ããW X
}
åå 
return
éé 
mapper
éé 
.
éé 
Map
éé 
<
éé 
AppointmentDto
éé ,
>
éé, -
(
éé- . 
updatedAppointment
éé. @
)
éé@ A
;
ééA B
}
èè 	
public
ëë 
async
ëë 
Task
ëë 
<
ëë 
AppointmentDto
ëë (
>
ëë( )$
CancelAppointmentAsync
ëë* @
(
ëë@ A"
CancelAppointmentDto
ëëA U
dto
ëëV Y
)
ëëY Z
{
íí 	
if
ìì 
(
ìì 
dto
ìì 
is
ìì 
null
ìì 
)
ìì 
{
îî 
throw
ïï 
new
ïï &
AppointmentRuleException
ïï 2
(
ïï2 30
"CancellationDetailsRequiredMessage
ïï3 U
)
ïïU V
;
ïïV W
}
ññ #
ValidateAppointmentId
òò !
(
òò! "
dto
òò" %
.
òò% &
AppointmentId
òò& 3
)
òò3 4
;
òò4 5(
ValidateCancellationReason
öö &
(
öö& '
dto
öö' *
.
öö* +
Reason
öö+ 1
)
öö1 2
;
öö2 3
var
úú 
appointment
úú 
=
úú 
await
úú ##
appointmentRepository
úú$ 9
.
úú9 :
GetByIdAsync
úú: F
(
úúF G
dto
úúG J
.
úúJ K
AppointmentId
úúK X
)
úúX Y
;
úúY Z
if
ûû 
(
ûû 
appointment
ûû 
is
ûû 
null
ûû #
)
ûû# $
{
üü 
throw
†† 
new
†† %
EntityNotFoundException
†† 1
(
††1 2#
AppointmentEntityName
††2 G
,
††G H
dto
††I L
.
††L M
AppointmentId
††M Z
)
††Z [
;
††[ \
}
°° 
if
££ 
(
££ 
appointment
££ 
.
££ 
Status
££ "
==
££# %
AppointmentStatus
££& 7
.
££7 8
	Completed
££8 A
)
££A B
{
§§ 
throw
•• 
new
•• 
ConflictException
•• +
(
••+ ,
$str
••, X
)
••X Y
;
••Y Z
}
¶¶ 
if
®® 
(
®® 
appointment
®® 
.
®® 
Status
®® "
==
®®# %
AppointmentStatus
®®& 7
.
®®7 8
	Cancelled
®®8 A
)
®®A B
{
©© 
throw
™™ 
new
™™ 
ConflictException
™™ +
(
™™+ ,
$str
™™, O
)
™™O P
;
™™P Q
}
´´ 
appointment
≠≠ 
.
≠≠ 
Status
≠≠ 
=
≠≠  
AppointmentStatus
≠≠! 2
.
≠≠2 3
	Cancelled
≠≠3 <
;
≠≠< =
appointment
ÆÆ 
.
ÆÆ  
CancellationReason
ÆÆ *
=
ÆÆ+ ,
dto
ÆÆ- 0
.
ÆÆ0 1
Reason
ÆÆ1 7
.
ÆÆ7 8
Trim
ÆÆ8 <
(
ÆÆ< =
)
ÆÆ= >
;
ÆÆ> ?
var
∞∞  
updatedAppointment
∞∞ "
=
∞∞# $
await
∞∞% *#
appointmentRepository
∞∞+ @
.
∞∞@ A
UpdateAsync
∞∞A L
(
∞∞L M
dto
±± 
.
±± 
AppointmentId
±± !
,
±±! "
appointment
≤≤ 
)
≤≤ 
;
≤≤ 
if
¥¥ 
(
¥¥  
updatedAppointment
¥¥ "
is
¥¥# %
null
¥¥& *
)
¥¥* +
{
µµ 
throw
∂∂ 
new
∂∂ %
EntityNotFoundException
∂∂ 1
(
∂∂1 2#
AppointmentEntityName
∂∂2 G
,
∂∂G H
dto
∂∂I L
.
∂∂L M
AppointmentId
∂∂M Z
)
∂∂Z [
;
∂∂[ \
}
∑∑ 
return
ππ 
mapper
ππ 
.
ππ 
Map
ππ 
<
ππ 
AppointmentDto
ππ ,
>
ππ, -
(
ππ- . 
updatedAppointment
ππ. @
)
ππ@ A
;
ππA B
}
∫∫ 	
public
ºº 
async
ºº 
Task
ºº 
<
ºº 
AppointmentDto
ºº (
>
ºº( )$
DeleteAppointmentAsync
ºº* @
(
ºº@ A
int
ººA D
appointmentId
ººE R
)
ººR S
{
ΩΩ 	#
ValidateAppointmentId
ææ !
(
ææ! "
appointmentId
ææ" /
)
ææ/ 0
;
ææ0 1
var
¿¿ 
appointment
¿¿ 
=
¿¿ 
await
¿¿ ##
appointmentRepository
¿¿$ 9
.
¿¿9 :
GetByIdAsync
¿¿: F
(
¿¿F G
appointmentId
¿¿G T
)
¿¿T U
;
¿¿U V
if
¬¬ 
(
¬¬ 
appointment
¬¬ 
is
¬¬ 
null
¬¬ #
)
¬¬# $
{
√√ 
throw
ƒƒ 
new
ƒƒ %
EntityNotFoundException
ƒƒ 1
(
ƒƒ1 2#
AppointmentEntityName
ƒƒ2 G
,
ƒƒG H
appointmentId
ƒƒI V
)
ƒƒV W
;
ƒƒW X
}
≈≈ 
var
«« 
hasHealthRecord
«« 
=
««  !
await
««" '$
healthRecordRepository
««( >
.
««> ?(
ExistsByAppointmentIdAsync
««? Y
(
««Y Z
appointmentId
««Z g
)
««g h
;
««h i
if
…… 
(
…… 
hasHealthRecord
…… 
)
……  
{
   
throw
ÀÀ 
new
ÀÀ 
ConflictException
ÀÀ +
(
ÀÀ+ ,
$str
ÀÀ, |
)
ÀÀ| }
;
ÀÀ} ~
}
ÃÃ 
var
ŒŒ  
deletedAppointment
ŒŒ "
=
ŒŒ# $
await
ŒŒ% *#
appointmentRepository
ŒŒ+ @
.
ŒŒ@ A
DeleteAsync
ŒŒA L
(
ŒŒL M
appointmentId
ŒŒM Z
)
ŒŒZ [
;
ŒŒ[ \
if
–– 
(
––  
deletedAppointment
–– "
is
––# %
null
––& *
)
––* +
{
—— 
throw
““ 
new
““ %
EntityNotFoundException
““ 1
(
““1 2#
AppointmentEntityName
““2 G
,
““G H
appointmentId
““I V
)
““V W
;
““W X
}
”” 
return
’’ 
mapper
’’ 
.
’’ 
Map
’’ 
<
’’ 
AppointmentDto
’’ ,
>
’’, -
(
’’- . 
deletedAppointment
’’. @
)
’’@ A
;
’’A B
}
÷÷ 	
public
ÿÿ 
async
ÿÿ 
Task
ÿÿ 
<
ÿÿ 
List
ÿÿ 
<
ÿÿ 
AppointmentDto
ÿÿ -
>
ÿÿ- .
>
ÿÿ. /.
 GetMyAppointmentsForPatientAsync
ÿÿ0 P
(
ÿÿP Q
string
ÿÿQ W
identityUserId
ÿÿX f
)
ÿÿf g
{
ŸŸ 	
var
⁄⁄ 
patient
⁄⁄ 
=
⁄⁄ 
await
⁄⁄ %
GetLoggedInPatientAsync
⁄⁄  7
(
⁄⁄7 8
identityUserId
⁄⁄8 F
)
⁄⁄F G
;
⁄⁄G H
var
‹‹ 
appointments
‹‹ 
=
‹‹ 
await
‹‹ $#
appointmentRepository
‹‹% :
.
‹‹: ;!
GetByPatientIdAsync
‹‹; N
(
‹‹N O
patient
‹‹O V
.
‹‹V W
	PatientId
‹‹W `
)
‹‹` a
;
‹‹a b
return
ﬁﬁ 
mapper
ﬁﬁ 
.
ﬁﬁ 
Map
ﬁﬁ 
<
ﬁﬁ 
List
ﬁﬁ "
<
ﬁﬁ" #
AppointmentDto
ﬁﬁ# 1
>
ﬁﬁ1 2
>
ﬁﬁ2 3
(
ﬁﬁ3 4
appointments
ﬁﬁ4 @
)
ﬁﬁ@ A
;
ﬁﬁA B
}
ﬂﬂ 	
public
·· 
async
·· 
Task
·· 
<
·· 
List
·· 
<
·· 
AppointmentDto
·· -
>
··- .
>
··. /6
(GetMyUpcomingAppointmentsForPatientAsync
··0 X
(
··X Y
string
··Y _
identityUserId
··` n
)
··n o
{
‚‚ 	
var
„„ 
patient
„„ 
=
„„ 
await
„„ %
GetLoggedInPatientAsync
„„  7
(
„„7 8
identityUserId
„„8 F
)
„„F G
;
„„G H
var
ÂÂ 
appointments
ÂÂ 
=
ÂÂ 
await
ÂÂ $#
appointmentRepository
ÂÂ% :
.
ÂÂ: ;5
'GetUpcomingAppointmentsByPatientIdAsync
ÂÂ; b
(
ÂÂb c
patient
ÂÂc j
.
ÂÂj k
	PatientId
ÂÂk t
)
ÂÂt u
;
ÂÂu v
return
ÁÁ 
mapper
ÁÁ 
.
ÁÁ 
Map
ÁÁ 
<
ÁÁ 
List
ÁÁ "
<
ÁÁ" #
AppointmentDto
ÁÁ# 1
>
ÁÁ1 2
>
ÁÁ2 3
(
ÁÁ3 4
appointments
ÁÁ4 @
)
ÁÁ@ A
;
ÁÁA B
}
ËË 	
public
ÍÍ 
async
ÍÍ 
Task
ÍÍ 
<
ÍÍ 
List
ÍÍ 
<
ÍÍ 
AppointmentDto
ÍÍ -
>
ÍÍ- .
>
ÍÍ. /5
'GetMyPendingAppointmentsForPatientAsync
ÍÍ0 W
(
ÍÍW X
string
ÍÍX ^
identityUserId
ÍÍ_ m
)
ÍÍm n
{
ÎÎ 	
var
ÏÏ 
patient
ÏÏ 
=
ÏÏ 
await
ÏÏ %
GetLoggedInPatientAsync
ÏÏ  7
(
ÏÏ7 8
identityUserId
ÏÏ8 F
)
ÏÏF G
;
ÏÏG H
var
ÓÓ 
appointments
ÓÓ 
=
ÓÓ 
await
ÓÓ $#
appointmentRepository
ÓÓ% :
.
ÓÓ: ;4
&GetPendingAppointmentsByPatientIdAsync
ÓÓ; a
(
ÓÓa b
patient
ÓÓb i
.
ÓÓi j
	PatientId
ÓÓj s
)
ÓÓs t
;
ÓÓt u
return
 
mapper
 
.
 
Map
 
<
 
List
 "
<
" #
AppointmentDto
# 1
>
1 2
>
2 3
(
3 4
appointments
4 @
)
@ A
;
A B
}
ÒÒ 	
public
ÛÛ 
async
ÛÛ 
Task
ÛÛ 
<
ÛÛ 
AppointmentDto
ÛÛ (
>
ÛÛ( )/
!GetAppointmentByIdForPatientAsync
ÛÛ* K
(
ÛÛK L
int
ÙÙ 
appointmentId
ÙÙ 
,
ÙÙ 
string
ıı 
identityUserId
ıı !
)
ıı! "
{
ˆˆ 	#
ValidateAppointmentId
˜˜ !
(
˜˜! "
appointmentId
˜˜" /
)
˜˜/ 0
;
˜˜0 1
var
˘˘ 
patient
˘˘ 
=
˘˘ 
await
˘˘ %
GetLoggedInPatientAsync
˘˘  7
(
˘˘7 8
identityUserId
˘˘8 F
)
˘˘F G
;
˘˘G H
var
˚˚ 
appointment
˚˚ 
=
˚˚ 
await
˚˚ ##
appointmentRepository
˚˚$ 9
.
˚˚9 :
GetByIdAsync
˚˚: F
(
˚˚F G
appointmentId
˚˚G T
)
˚˚T U
;
˚˚U V
if
˝˝ 
(
˝˝ 
appointment
˝˝ 
is
˝˝ 
null
˝˝ #
)
˝˝# $
{
˛˛ 
throw
ˇˇ 
new
ˇˇ %
EntityNotFoundException
ˇˇ 1
(
ˇˇ1 2#
AppointmentEntityName
ˇˇ2 G
,
ˇˇG H
appointmentId
ˇˇI V
)
ˇˇV W
;
ˇˇW X
}
ÄÄ 
if
ÇÇ 
(
ÇÇ 
appointment
ÇÇ 
.
ÇÇ 
	PatientId
ÇÇ %
!=
ÇÇ& (
patient
ÇÇ) 0
.
ÇÇ0 1
	PatientId
ÇÇ1 :
)
ÇÇ: ;
{
ÉÉ 
throw
ÑÑ 
new
ÑÑ &
ForbiddenAccessException
ÑÑ 2
(
ÑÑ2 3
$str
ÑÑ3 e
)
ÑÑe f
;
ÑÑf g
}
ÖÖ 
return
áá 
mapper
áá 
.
áá 
Map
áá 
<
áá 
AppointmentDto
áá ,
>
áá, -
(
áá- .
appointment
áá. 9
)
áá9 :
;
áá: ;
}
àà 	
public
ää 
async
ää 
Task
ää 
<
ää 
AppointmentDto
ää (
>
ää( ),
BookAppointmentForPatientAsync
ää* H
(
ääH I 
BookAppointmentDto
ãã 
dto
ãã "
,
ãã" #
string
åå 
identityUserId
åå !
)
åå! "
{
çç 	
if
éé 
(
éé 
dto
éé 
is
éé 
null
éé 
)
éé 
{
èè 
throw
êê 
new
êê &
AppointmentRuleException
êê 2
(
êê2 3/
!AppointmentDetailsRequiredMessage
êê3 T
)
êêT U
;
êêU V
}
ëë 
var
ìì 
patient
ìì 
=
ìì 
await
ìì %
GetLoggedInPatientAsync
ìì  7
(
ìì7 8
identityUserId
ìì8 F
)
ììF G
;
ììG H
dto
óó 
.
óó 
	PatientId
óó 
=
óó 
patient
óó #
.
óó# $
	PatientId
óó$ -
;
óó- .
return
ôô 
await
ôô "
BookAppointmentAsync
ôô -
(
ôô- .
dto
ôô. 1
)
ôô1 2
;
ôô2 3
}
öö 	
public
úú 
async
úú 
Task
úú 
<
úú 
AppointmentDto
úú (
>
úú( ).
 CancelAppointmentForPatientAsync
úú* J
(
úúJ K"
CancelAppointmentDto
ùù  
dto
ùù! $
,
ùù$ %
string
ûû 
identityUserId
ûû !
)
ûû! "
{
üü 	
if
†† 
(
†† 
dto
†† 
is
†† 
null
†† 
)
†† 
{
°° 
throw
¢¢ 
new
¢¢ &
AppointmentRuleException
¢¢ 2
(
¢¢2 30
"CancellationDetailsRequiredMessage
¢¢3 U
)
¢¢U V
;
¢¢V W
}
££ #
ValidateAppointmentId
•• !
(
••! "
dto
••" %
.
••% &
AppointmentId
••& 3
)
••3 4
;
••4 5
var
ßß 
patient
ßß 
=
ßß 
await
ßß %
GetLoggedInPatientAsync
ßß  7
(
ßß7 8
identityUserId
ßß8 F
)
ßßF G
;
ßßG H
var
©© 
appointment
©© 
=
©© 
await
©© ##
appointmentRepository
©©$ 9
.
©©9 :
GetByIdAsync
©©: F
(
©©F G
dto
©©G J
.
©©J K
AppointmentId
©©K X
)
©©X Y
;
©©Y Z
if
´´ 
(
´´ 
appointment
´´ 
is
´´ 
null
´´ #
)
´´# $
{
¨¨ 
throw
≠≠ 
new
≠≠ %
EntityNotFoundException
≠≠ 1
(
≠≠1 2#
AppointmentEntityName
≠≠2 G
,
≠≠G H
dto
≠≠I L
.
≠≠L M
AppointmentId
≠≠M Z
)
≠≠Z [
;
≠≠[ \
}
ÆÆ 
if
∞∞ 
(
∞∞ 
appointment
∞∞ 
.
∞∞ 
	PatientId
∞∞ %
!=
∞∞& (
patient
∞∞) 0
.
∞∞0 1
	PatientId
∞∞1 :
)
∞∞: ;
{
±± 
throw
≤≤ 
new
≤≤ &
ForbiddenAccessException
≤≤ 2
(
≤≤2 3
$str
≤≤3 e
)
≤≤e f
;
≤≤f g
}
≥≥ 
return
µµ 
await
µµ $
CancelAppointmentAsync
µµ /
(
µµ/ 0
dto
µµ0 3
)
µµ3 4
;
µµ4 5
}
∂∂ 	
public
∑∑ 
async
∑∑ 
Task
∑∑ 
<
∑∑ 
List
∑∑ 
<
∑∑ 
AppointmentDto
∑∑ -
>
∑∑- .
>
∑∑. /-
GetMyAppointmentsForDoctorAsync
∑∑0 O
(
∑∑O P
string
∑∑P V
identityUserId
∑∑W e
)
∑∑e f
{
∏∏ 	
var
ππ 
doctor
ππ 
=
ππ 
await
ππ $
GetLoggedInDoctorAsync
ππ 5
(
ππ5 6
identityUserId
ππ6 D
)
ππD E
;
ππE F
var
ªª 
appointments
ªª 
=
ªª 
await
ªª $#
appointmentRepository
ªª% :
.
ªª: ; 
GetByDoctorIdAsync
ªª; M
(
ªªM N
doctor
ªªN T
.
ªªT U
DoctorId
ªªU ]
)
ªª] ^
;
ªª^ _
return
ΩΩ 
mapper
ΩΩ 
.
ΩΩ 
Map
ΩΩ 
<
ΩΩ 
List
ΩΩ "
<
ΩΩ" #
AppointmentDto
ΩΩ# 1
>
ΩΩ1 2
>
ΩΩ2 3
(
ΩΩ3 4
appointments
ΩΩ4 @
)
ΩΩ@ A
;
ΩΩA B
}
ææ 	
public
øø 
async
øø 
Task
øø 
<
øø 
List
øø 
<
øø 
AppointmentDto
øø -
>
øø- .
>
øø. /5
'GetMyUpcomingAppointmentsForDoctorAsync
øø0 W
(
øøW X
string
øøX ^
identityUserId
øø_ m
)
øøm n
{
¿¿ 	
var
¡¡ 
doctor
¡¡ 
=
¡¡ 
await
¡¡ $
GetLoggedInDoctorAsync
¡¡ 5
(
¡¡5 6
identityUserId
¡¡6 D
)
¡¡D E
;
¡¡E F
var
√√ 
appointments
√√ 
=
√√ 
await
√√ $#
appointmentRepository
√√% :
.
√√: ;4
&GetUpcomingAppointmentsByDoctorIdAsync
√√; a
(
√√a b
doctor
√√b h
.
√√h i
DoctorId
√√i q
)
√√q r
;
√√r s
return
≈≈ 
mapper
≈≈ 
.
≈≈ 
Map
≈≈ 
<
≈≈ 
List
≈≈ "
<
≈≈" #
AppointmentDto
≈≈# 1
>
≈≈1 2
>
≈≈2 3
(
≈≈3 4
appointments
≈≈4 @
)
≈≈@ A
;
≈≈A B
}
∆∆ 	
public
»» 
async
»» 
Task
»» 
<
»» 
List
»» 
<
»» 
AppointmentDto
»» -
>
»»- .
>
»». /4
&GetMyPendingAppointmentsForDoctorAsync
»»0 V
(
»»V W
string
»»W ]
identityUserId
»»^ l
)
»»l m
{
…… 	
var
   
doctor
   
=
   
await
   $
GetLoggedInDoctorAsync
   5
(
  5 6
identityUserId
  6 D
)
  D E
;
  E F
var
ÃÃ 
appointments
ÃÃ 
=
ÃÃ 
await
ÃÃ $#
appointmentRepository
ÃÃ% :
.
ÃÃ: ;3
%GetPendingAppointmentsByDoctorIdAsync
ÃÃ; `
(
ÃÃ` a
doctor
ÃÃa g
.
ÃÃg h
DoctorId
ÃÃh p
)
ÃÃp q
;
ÃÃq r
return
ŒŒ 
mapper
ŒŒ 
.
ŒŒ 
Map
ŒŒ 
<
ŒŒ 
List
ŒŒ "
<
ŒŒ" #
AppointmentDto
ŒŒ# 1
>
ŒŒ1 2
>
ŒŒ2 3
(
ŒŒ3 4
appointments
ŒŒ4 @
)
ŒŒ@ A
;
ŒŒA B
}
œœ 	
public
—— 
async
—— 
Task
—— 
<
—— 
List
—— 
<
—— 
AppointmentDto
—— -
>
——- .
>
——. /;
-GetMyTodayConfirmedAppointmentsForDoctorAsync
——0 ]
(
——] ^
string
——^ d
identityUserId
——e s
)
——s t
{
““ 	
var
”” 
doctor
”” 
=
”” 
await
”” $
GetLoggedInDoctorAsync
”” 5
(
””5 6
identityUserId
””6 D
)
””D E
;
””E F
var
’’ 
appointments
’’ 
=
’’ 
await
’’ $#
appointmentRepository
’’% :
.
’’: ;:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
’’; g
(
’’g h
doctor
’’h n
.
’’n o
DoctorId
’’o w
)
’’w x
;
’’x y
return
◊◊ 
mapper
◊◊ 
.
◊◊ 
Map
◊◊ 
<
◊◊ 
List
◊◊ "
<
◊◊" #
AppointmentDto
◊◊# 1
>
◊◊1 2
>
◊◊2 3
(
◊◊3 4
appointments
◊◊4 @
)
◊◊@ A
;
◊◊A B
}
ÿÿ 	
public
⁄⁄ 
async
⁄⁄ 
Task
⁄⁄ 
<
⁄⁄ 
AppointmentDto
⁄⁄ (
>
⁄⁄( ).
 GetAppointmentByIdForDoctorAsync
⁄⁄* J
(
⁄⁄J K
int
€€ 
appointmentId
€€ 
,
€€ 
string
‹‹ 

identityUserId
‹‹ 
)
‹‹ 
{
›› 	#
ValidateAppointmentId
ﬁﬁ !
(
ﬁﬁ! "
appointmentId
ﬁﬁ" /
)
ﬁﬁ/ 0
;
ﬁﬁ0 1
var
‡‡ 
doctor
‡‡ 
=
‡‡ 
await
‡‡ $
GetLoggedInDoctorAsync
‡‡ 5
(
‡‡5 6
identityUserId
‡‡6 D
)
‡‡D E
;
‡‡E F
var
‚‚ 
appointment
‚‚ 
=
‚‚ 
await
‚‚ ##
appointmentRepository
‚‚$ 9
.
‚‚9 :
GetByIdAsync
‚‚: F
(
‚‚F G
appointmentId
‚‚G T
)
‚‚T U
;
‚‚U V
if
‰‰ 
(
‰‰ 
appointment
‰‰ 
is
‰‰ 
null
‰‰ #
)
‰‰# $
{
ÂÂ 
throw
ÊÊ 
new
ÊÊ %
EntityNotFoundException
ÊÊ 1
(
ÊÊ1 2#
AppointmentEntityName
ÊÊ2 G
,
ÊÊG H
appointmentId
ÊÊI V
)
ÊÊV W
;
ÊÊW X
}
ÁÁ 
if
ÈÈ 
(
ÈÈ 
appointment
ÈÈ 
.
ÈÈ 
DoctorId
ÈÈ $
!=
ÈÈ% '
doctor
ÈÈ( .
.
ÈÈ. /
DoctorId
ÈÈ/ 7
)
ÈÈ7 8
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ &
ForbiddenAccessException
ÎÎ 2
(
ÎÎ2 3
$str
ÎÎ3 d
)
ÎÎd e
;
ÎÎe f
}
ÏÏ 
return
ÓÓ 
mapper
ÓÓ 
.
ÓÓ 
Map
ÓÓ 
<
ÓÓ 
AppointmentDto
ÓÓ ,
>
ÓÓ, -
(
ÓÓ- .
appointment
ÓÓ. 9
)
ÓÓ9 :
;
ÓÓ: ;
}
ÔÔ 	
public
ÒÒ 
async
ÒÒ 
Task
ÒÒ 
<
ÒÒ 
AppointmentDto
ÒÒ (
>
ÒÒ( ).
 ConfirmAppointmentForDoctorAsync
ÒÒ* J
(
ÒÒJ K
int
ÚÚ 
appointmentId
ÚÚ 
,
ÚÚ 
string
ÛÛ 

identityUserId
ÛÛ 
)
ÛÛ 
{
ÙÙ 	#
ValidateAppointmentId
ıı !
(
ıı! "
appointmentId
ıı" /
)
ıı/ 0
;
ıı0 1
var
˜˜ 
doctor
˜˜ 
=
˜˜ 
await
˜˜ $
GetLoggedInDoctorAsync
˜˜ 5
(
˜˜5 6
identityUserId
˜˜6 D
)
˜˜D E
;
˜˜E F
var
˘˘ 
appointment
˘˘ 
=
˘˘ 
await
˘˘ ##
appointmentRepository
˘˘$ 9
.
˘˘9 :
GetByIdAsync
˘˘: F
(
˘˘F G
appointmentId
˘˘G T
)
˘˘T U
;
˘˘U V
if
˚˚ 
(
˚˚ 
appointment
˚˚ 
is
˚˚ 
null
˚˚ #
)
˚˚# $
{
¸¸ 
throw
˝˝ 
new
˝˝ %
EntityNotFoundException
˝˝ 1
(
˝˝1 2#
AppointmentEntityName
˝˝2 G
,
˝˝G H
appointmentId
˝˝I V
)
˝˝V W
;
˝˝W X
}
˛˛ 
if
ÄÄ 
(
ÄÄ 
appointment
ÄÄ 
.
ÄÄ 
DoctorId
ÄÄ $
!=
ÄÄ% '
doctor
ÄÄ( .
.
ÄÄ. /
DoctorId
ÄÄ/ 7
)
ÄÄ7 8
{
ÅÅ 
throw
ÇÇ 
new
ÇÇ &
ForbiddenAccessException
ÇÇ 2
(
ÇÇ2 3
$str
ÇÇ3 e
)
ÇÇe f
;
ÇÇf g
}
ÉÉ 
return
ÖÖ 
await
ÖÖ %
ConfirmAppointmentAsync
ÖÖ 0
(
ÖÖ0 1
appointmentId
ÖÖ1 >
)
ÖÖ> ?
;
ÖÖ? @
}
ÜÜ 	
public
àà 
async
àà 
Task
àà 
<
àà 
AppointmentDto
àà (
>
àà( )/
!CompleteAppointmentForDoctorAsync
àà* K
(
ààK L
int
ââ 
appointmentId
ââ 
,
ââ 
string
ää 

identityUserId
ää 
)
ää 
{
ãã 	#
ValidateAppointmentId
åå !
(
åå! "
appointmentId
åå" /
)
åå/ 0
;
åå0 1
var
éé 
doctor
éé 
=
éé 
await
éé $
GetLoggedInDoctorAsync
éé 5
(
éé5 6
identityUserId
éé6 D
)
ééD E
;
ééE F
var
êê 
appointment
êê 
=
êê 
await
êê ##
appointmentRepository
êê$ 9
.
êê9 :
GetByIdAsync
êê: F
(
êêF G
appointmentId
êêG T
)
êêT U
;
êêU V
if
íí 
(
íí 
appointment
íí 
is
íí 
null
íí #
)
íí# $
{
ìì 
throw
îî 
new
îî %
EntityNotFoundException
îî 1
(
îî1 2#
AppointmentEntityName
îî2 G
,
îîG H
appointmentId
îîI V
)
îîV W
;
îîW X
}
ïï 
if
óó 
(
óó 
appointment
óó 
.
óó 
DoctorId
óó $
!=
óó% '
doctor
óó( .
.
óó. /
DoctorId
óó/ 7
)
óó7 8
{
òò 
throw
ôô 
new
ôô &
ForbiddenAccessException
ôô 2
(
ôô2 3
$str
ôô3 f
)
ôôf g
;
ôôg h
}
öö 
return
úú 
await
úú &
CompleteAppointmentAsync
úú 1
(
úú1 2
appointmentId
úú2 ?
)
úú? @
;
úú@ A
}
ùù 	
public
üü 
async
üü 
Task
üü 
<
üü 
AppointmentDto
üü (
>
üü( )-
CancelAppointmentForDoctorAsync
üü* I
(
üüI J"
CancelAppointmentDto
†† 
dto
†† 
,
†† 
string
°° 

identityUserId
°° 
)
°° 
{
¢¢ 	
if
££ 
(
££ 
dto
££ 
is
££ 
null
££ 
)
££ 
{
§§ 
throw
•• 
new
•• &
AppointmentRuleException
•• 2
(
••2 30
"CancellationDetailsRequiredMessage
••3 U
)
••U V
;
••V W
}
¶¶ #
ValidateAppointmentId
®® !
(
®®! "
dto
®®" %
.
®®% &
AppointmentId
®®& 3
)
®®3 4
;
®®4 5
var
™™ 
doctor
™™ 
=
™™ 
await
™™ $
GetLoggedInDoctorAsync
™™ 5
(
™™5 6
identityUserId
™™6 D
)
™™D E
;
™™E F
var
¨¨ 
appointment
¨¨ 
=
¨¨ 
await
¨¨ ##
appointmentRepository
¨¨$ 9
.
¨¨9 :
GetByIdAsync
¨¨: F
(
¨¨F G
dto
¨¨G J
.
¨¨J K
AppointmentId
¨¨K X
)
¨¨X Y
;
¨¨Y Z
if
ÆÆ 
(
ÆÆ 
appointment
ÆÆ 
is
ÆÆ 
null
ÆÆ #
)
ÆÆ# $
{
ØØ 
throw
∞∞ 
new
∞∞ %
EntityNotFoundException
∞∞ 1
(
∞∞1 2#
AppointmentEntityName
∞∞2 G
,
∞∞G H
dto
∞∞I L
.
∞∞L M
AppointmentId
∞∞M Z
)
∞∞Z [
;
∞∞[ \
}
±± 
if
≥≥ 
(
≥≥ 
appointment
≥≥ 
.
≥≥ 
DoctorId
≥≥ $
!=
≥≥% '
doctor
≥≥( .
.
≥≥. /
DoctorId
≥≥/ 7
)
≥≥7 8
{
¥¥ 
throw
µµ 
new
µµ &
ForbiddenAccessException
µµ 2
(
µµ2 3
$str
µµ3 d
)
µµd e
;
µµe f
}
∂∂ 
return
∏∏ 
await
∏∏ $
CancelAppointmentAsync
∏∏ /
(
∏∏/ 0
dto
∏∏0 3
)
∏∏3 4
;
∏∏4 5
}
ππ 	
private
∫∫ 
async
∫∫ 
Task
∫∫ 
<
∫∫ 
Doctor
∫∫ !
>
∫∫! "$
GetLoggedInDoctorAsync
∫∫# 9
(
∫∫9 :
string
∫∫: @
identityUserId
∫∫A O
)
∫∫O P
{
ªª 	
if
ºº 
(
ºº 
string
ºº 
.
ºº  
IsNullOrWhiteSpace
ºº )
(
ºº) *
identityUserId
ºº* 8
)
ºº8 9
)
ºº9 :
{
ΩΩ 
throw
ææ 
new
ææ #
BusinessRuleException
ææ /
(
ææ/ 0
$str
ææ0 I
)
ææI J
;
ææJ K
}
øø 
var
¡¡ 
doctor
¡¡ 
=
¡¡ 
await
¡¡ 
doctorRepository
¡¡ /
.
¡¡/ 0&
GetByIdentityUserIdAsync
¡¡0 H
(
¡¡H I
identityUserId
¡¡I W
)
¡¡W X
;
¡¡X Y
if
√√ 
(
√√ 
doctor
√√ 
is
√√ 
null
√√ 
)
√√ 
{
ƒƒ 
throw
≈≈ 
new
≈≈ %
EntityNotFoundException
≈≈ 1
(
≈≈1 2
$str
≈≈2 U
,
≈≈U V
$num
≈≈W X
)
≈≈X Y
;
≈≈Y Z
}
∆∆ 
return
»» 
doctor
»» 
;
»» 
}
…… 	
private
   
async
   
Task
   
<
   
Patient
   "
>
  " #%
GetLoggedInPatientAsync
  $ ;
(
  ; <
string
  < B
identityUserId
  C Q
)
  Q R
{
ÀÀ 	
if
ÃÃ 
(
ÃÃ 
string
ÃÃ 
.
ÃÃ  
IsNullOrWhiteSpace
ÃÃ )
(
ÃÃ) *
identityUserId
ÃÃ* 8
)
ÃÃ8 9
)
ÃÃ9 :
{
ÕÕ 
throw
ŒŒ 
new
ŒŒ #
BusinessRuleException
ŒŒ /
(
ŒŒ/ 0
$str
ŒŒ0 I
)
ŒŒI J
;
ŒŒJ K
}
œœ 
var
—— 
patient
—— 
=
—— 
await
—— 
patientRepository
——  1
.
——1 2&
GetByIdentityUserIdAsync
——2 J
(
——J K
identityUserId
——K Y
)
——Y Z
;
——Z [
if
”” 
(
”” 
patient
”” 
is
”” 
null
”” 
)
””  
{
‘‘ 
throw
’’ 
new
’’ %
EntityNotFoundException
’’ 1
(
’’1 2
$str
’’2 V
,
’’V W
$num
’’X Y
)
’’Y Z
;
’’Z [
}
÷÷ 
return
ÿÿ 
patient
ÿÿ 
;
ÿÿ 
}
ŸŸ 	
private
⁄⁄ 
static
⁄⁄ 
void
⁄⁄ #
ValidateAppointmentId
⁄⁄ 1
(
⁄⁄1 2
int
⁄⁄2 5
appointmentId
⁄⁄6 C
)
⁄⁄C D
{
€€ 	
if
‹‹ 
(
‹‹ 
appointmentId
‹‹ 
<=
‹‹  
$num
‹‹! "
)
‹‹" #
{
›› 
throw
ﬁﬁ 
new
ﬁﬁ &
AppointmentRuleException
ﬁﬁ 2
(
ﬁﬁ2 3
$str
ﬁﬁ3 b
)
ﬁﬁb c
;
ﬁﬁc d
}
ﬂﬂ 
}
‡‡ 	
private
‚‚ 
static
‚‚ 
void
‚‚ 
ValidatePatientId
‚‚ -
(
‚‚- .
int
‚‚. 1
	patientId
‚‚2 ;
)
‚‚; <
{
„„ 	
if
‰‰ 
(
‰‰ 
	patientId
‰‰ 
<=
‰‰ 
$num
‰‰ 
)
‰‰ 
{
ÂÂ 
throw
ÊÊ 
new
ÊÊ &
AppointmentRuleException
ÊÊ 2
(
ÊÊ2 3
$str
ÊÊ3 ^
)
ÊÊ^ _
;
ÊÊ_ `
}
ÁÁ 
}
ËË 	
private
ÍÍ 
static
ÍÍ 
void
ÍÍ 
ValidateDoctorId
ÍÍ ,
(
ÍÍ, -
int
ÍÍ- 0
doctorId
ÍÍ1 9
)
ÍÍ9 :
{
ÎÎ 	
if
ÏÏ 
(
ÏÏ 
doctorId
ÏÏ 
<=
ÏÏ 
$num
ÏÏ 
)
ÏÏ 
{
ÌÌ 
throw
ÓÓ 
new
ÓÓ &
AppointmentRuleException
ÓÓ 2
(
ÓÓ2 3
$str
ÓÓ3 ]
)
ÓÓ] ^
;
ÓÓ^ _
}
ÔÔ 
}
 	
private
ÚÚ 
async
ÚÚ 
Task
ÚÚ (
ValidatePatientExistsAsync
ÚÚ 5
(
ÚÚ5 6
int
ÚÚ6 9
	patientId
ÚÚ: C
)
ÚÚC D
{
ÛÛ 	
ValidatePatientId
ÙÙ 
(
ÙÙ 
	patientId
ÙÙ '
)
ÙÙ' (
;
ÙÙ( )
var
ˆˆ 
patient
ˆˆ 
=
ˆˆ 
await
ˆˆ 
patientRepository
ˆˆ  1
.
ˆˆ1 2
GetByIdAsync
ˆˆ2 >
(
ˆˆ> ?
	patientId
ˆˆ? H
)
ˆˆH I
;
ˆˆI J
if
¯¯ 
(
¯¯ 
patient
¯¯ 
is
¯¯ 
null
¯¯ 
)
¯¯  
{
˘˘ 
throw
˙˙ 
new
˙˙ %
EntityNotFoundException
˙˙ 1
(
˙˙1 2
$str
˙˙2 ;
,
˙˙; <
	patientId
˙˙= F
)
˙˙F G
;
˙˙G H
}
˚˚ 
}
¸¸ 	
private
˛˛ 
async
˛˛ 
Task
˛˛ 
<
˛˛ 
Doctor
˛˛ !
>
˛˛! "'
ValidateDoctorExistsAsync
˛˛# <
(
˛˛< =
int
˛˛= @
doctorId
˛˛A I
)
˛˛I J
{
ˇˇ 	
ValidateDoctorId
ÄÄ 
(
ÄÄ 
doctorId
ÄÄ %
)
ÄÄ% &
;
ÄÄ& '
var
ÇÇ 
doctor
ÇÇ 
=
ÇÇ 
await
ÇÇ 
doctorRepository
ÇÇ /
.
ÇÇ/ 0
GetByIdAsync
ÇÇ0 <
(
ÇÇ< =
doctorId
ÇÇ= E
)
ÇÇE F
;
ÇÇF G
if
ÑÑ 
(
ÑÑ 
doctor
ÑÑ 
is
ÑÑ 
null
ÑÑ 
)
ÑÑ 
{
ÖÖ 
throw
ÜÜ 
new
ÜÜ %
EntityNotFoundException
ÜÜ 1
(
ÜÜ1 2
$str
ÜÜ2 :
,
ÜÜ: ;
doctorId
ÜÜ< D
)
ÜÜD E
;
ÜÜE F
}
áá 
return
ââ 
doctor
ââ 
;
ââ 
}
ää 	
private
åå 
static
åå 
void
åå (
ValidateDoctorAvailability
åå 6
(
åå6 7
Doctor
åå7 =
doctor
åå> D
)
ååD E
{
çç 	
if
éé 
(
éé 
!
éé 
doctor
éé 
.
éé 
IsActive
éé  
)
éé  !
{
èè 
throw
êê 
new
êê &
AppointmentRuleException
êê 2
(
êê2 3
$str
êê3 f
)
êêf g
;
êêg h
}
ëë 
}
íí 	
private
îî 
static
îî 
void
îî %
ValidateAppointmentDate
îî 3
(
îî3 4
DateTime
îî4 <
scheduledDate
îî= J
)
îîJ K
{
ïï 	
if
ññ 
(
ññ 
scheduledDate
ññ 
.
ññ 
Date
ññ "
<
ññ# $
DateTime
ññ% -
.
ññ- .
Today
ññ. 3
)
ññ3 4
{
óó 
throw
òò 
new
òò &
AppointmentRuleException
òò 2
(
òò2 3
$str
òò3 \
)
òò\ ]
;
òò] ^
}
ôô 
}
öö 	
private
úú 
static
úú 
void
úú 
ValidateTimeSlot
úú ,
(
úú, -
string
úú- 3
timeSlot
úú4 <
)
úú< =
{
ùù 	
if
ûû 
(
ûû 
string
ûû 
.
ûû  
IsNullOrWhiteSpace
ûû )
(
ûû) *
timeSlot
ûû* 2
)
ûû2 3
)
ûû3 4
{
üü 
throw
†† 
new
†† &
AppointmentRuleException
†† 2
(
††2 3
$str
††3 K
)
††K L
;
††L M
}
°° 
if
££ 
(
££ 
!
££ 
	TimeSlots
££ 
.
££ 
Slots
££  
.
££  !
Contains
££! )
(
££) *
timeSlot
££* 2
)
££2 3
)
££3 4
{
§§ 
throw
•• 
new
•• &
AppointmentRuleException
•• 2
(
••2 3
$str
••3 P
)
••P Q
;
••Q R
}
¶¶ 
}
ßß 	
private
©© 
static
©© 
void
©© (
ValidateCancellationReason
©© 6
(
©©6 7
string
©©7 =
reason
©©> D
)
©©D E
{
™™ 	
if
´´ 
(
´´ 
string
´´ 
.
´´  
IsNullOrWhiteSpace
´´ )
(
´´) *
reason
´´* 0
)
´´0 1
)
´´1 2
{
¨¨ 
throw
≠≠ 
new
≠≠ &
AppointmentRuleException
≠≠ 2
(
≠≠2 3
$str
≠≠3 U
)
≠≠U V
;
≠≠V W
}
ÆÆ 
if
∞∞ 
(
∞∞ 
reason
∞∞ 
.
∞∞ 
Trim
∞∞ 
(
∞∞ 
)
∞∞ 
.
∞∞ 
Length
∞∞ $
>
∞∞% &
$num
∞∞' *
)
∞∞* +
{
±± 
throw
≤≤ 
new
≤≤ &
AppointmentRuleException
≤≤ 2
(
≤≤2 3
$str
≤≤3 f
)
≤≤f g
;
≤≤g h
}
≥≥ 
}
¥¥ 	
}
µµ 
}∂∂ €
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Interface\IRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
	Interface# ,
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
:, -
class. 3
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
Task 
< 
T 
? 
> 
GetByIdAsync 
( 
int !
id" $
,$ %
CancellationToken& 7
ct8 :
=; <
default= D
)D E
;E F
Task		 
<		 
T		 
>		 
CreateAsync		 
(		 
T		 
entity		 $
,		$ %
CancellationToken		& 7
ct		8 :
=		; <
default		= D
)		D E
;		E F
Task 
< 
T 
? 
> 
UpdateAsync 
( 
int  
id! #
,# $
T% &
entity' -
,- .
CancellationToken/ @
ctA C
=D E
defaultF M
)M N
;N O
Task 
< 
T 
? 
> 
DeleteAsync 
( 
int  
id! #
,# $
CancellationToken% 6
ct7 9
=: ;
default< C
)C D
;D E
} 
} √

lC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Interface\IPatientRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
	Interface# ,
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
< 
bool 
> #
IsDuplicatePatientAsync *
(* +
string 
patientName 
, 
string		 
email		 
,		 
string

 
phoneNumber

 
,

 
DateTime 
dateOfBirth  
,  !
int 
? 
excludePatientId !
=" #
null$ (
,( )
CancellationToken 
ct  
=! "
default# *
)* +
;+ ,
Task 
< 
Patient 
? 
> $
GetByIdentityUserIdAsync /
(/ 0
string0 6
identityUserId7 E
,E F
CancellationTokenG X
ctY [
=\ ]
default^ e
)e f
;f g
} 
} ˘
qC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Interface\IHealthRecordRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
	Interface# ,
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
Task 
< 
List 
< 
HealthRecord 
> 
>  
GetByPatientIdAsync! 4
(4 5
int5 8
	patientId9 B
,B C
CancellationTokenD U
ctV X
=Y Z
default[ b
)b c
;c d
Task		 
<		 
List		 
<		 
HealthRecord		 
>		 
>		  
GetByDoctorIdAsync		! 3
(		3 4
int		4 7
doctorId		8 @
,		@ A
CancellationToken		B S
ct		T V
=		W X
default		Y `
)		` a
;		a b
Task 
< 
List 
< 
HealthRecord 
> 
>  #
GetByAppointmentIdAsync! 8
(8 9
int9 <
appointmentId= J
,J K
CancellationTokenL ]
ct^ `
=a b
defaultc j
)j k
;k l
Task 
< 
bool 
> &
ExistsByAppointmentIdAsync -
(- .
int. 1
appointmentId2 ?
,? @
CancellationTokenA R
ctS U
=V W
defaultX _
)_ `
;` a
} 
} Ó
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Impl\PatientRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
Impl# '
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
: 
base 
( 
context 
) 
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
bool 
> #
IsDuplicatePatientAsync  7
(7 8
string 

patientName 
, 
string 

email 
, 
string 

phoneNumber 
, 
DateTime 
dateOfBirth 
, 
int 
? 
excludePatientId	 
= 
null  
,  !
CancellationToken 
ct 
= 
default "
)" #
{ 
string 
!
normalizedPatientName  
=! "
patientName# .
.. /
ToUpperInvariant/ ?
(? @
)@ A
;A B
string 

normalizedEmail 
= 
email "
." #
ToUpperInvariant# 3
(3 4
)4 5
;5 6
return 

await 
_context 
. 
Patients "
." #
AnyAsync# +
(+ ,
patient, 3
=>4 6
patient 
. 
PatientName 
. 
ToUpper #
(# $
)$ %
==& (!
normalizedPatientName) >
&& 

patient 
. 
Email 
. 
ToUpper  
(  !
)! "
==# %
normalizedEmail& 5
&&   

patient   
.   
PhoneNumber   
==   !
phoneNumber  " -
&&!! 

patient!! 
.!! 
DateOfBirth!! 
.!! 
Date!! #
==!!$ &
dateOfBirth!!' 2
.!!2 3
Date!!3 7
&&"" 

("" 
!"" 
excludePatientId"" 
."" 
HasValue"" &
||""' )
patient""* 1
.""1 2
	PatientId""2 ;
!=""< >
excludePatientId""? O
.""O P
Value""P U
)""U V
,""V W
ct## 

)##
 
;## 
}$$ 
public&& 
async&& 
Task&& 
<&& 
Patient&& !
?&&! "
>&&" #$
GetByIdentityUserIdAsync&&$ <
(&&< =
string'' 

identityUserId'' 
,'' 
CancellationToken(( 
ct(( 
=(( 
default(( "
)((" #
{)) 	
return** 
await** 
_context** !
.**! "
Patients**" *
.++ 
FirstOrDefaultAsync++ $
(++$ %
p++% &
=>++' )
p++* +
.+++ ,
IdentityUserId++, :
==++; =
identityUserId++> L
,++L M
ct++N P
)++P Q
;++Q R
},, 	
}-- 
}.. ˛-
pC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Interface\IAppointmentRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
	Interface# ,
{ 
public 

	interface "
IAppointmentRepository +
:, -
IRepository. 9
<9 :
Appointment: E
>E F
{ 
Task 
< 
List 
< 
Appointment 
> 
> 
GetByPatientIdAsync  3
(3 4
int4 7
	patientId8 A
,A B
CancellationTokenC T
ctU W
=X Y
defaultZ a
)a b
;b c
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
 
GetByDoctorIdAsync

  2
(

2 3
int

3 6
doctorId

7 ?
,

? @
CancellationToken

A R
ct

S U
=

V W
default

X _
)

_ `
;

` a
Task 
< 
List 
< 
Appointment 
> 
> 
GetByStatusAsync  0
(0 1
AppointmentStatus1 B
statusC I
,I J
CancellationTokenK \
ct] _
=` a
defaultb i
)i j
;j k
Task 
< 
List 
< 
Appointment 
> 
> (
GetUpcomingAppointmentsAsync  <
(< =
CancellationToken= N
ctO Q
=R S
defaultT [
)[ \
;\ ]
Task 
< 
List 
< 
Appointment 
> 
> 3
'GetUpcomingAppointmentsByPatientIdAsync  G
(G H
intH K
	patientIdL U
,U V
CancellationTokenW h
cti k
=l m
defaultn u
)u v
;v w
Task 
< 
List 
< 
Appointment 
> 
> 2
&GetUpcomingAppointmentsByDoctorIdAsync  F
(F G
intG J
doctorIdK S
,S T
CancellationTokenU f
ctg i
=j k
defaultl s
)s t
;t u
Task 
< 
List 
< 
Appointment 
> 
> 2
&GetPendingAppointmentsByPatientIdAsync  F
(F G
intG J
	patientIdK T
,T U
CancellationTokenV g
cth j
=k l
defaultm t
)t u
;u v
Task 
< 
List 
< 
Appointment 
> 
> 1
%GetPendingAppointmentsByDoctorIdAsync  E
(E F
intF I
doctorIdJ R
,R S
CancellationTokenT e
ctf h
=i j
defaultk r
)r s
;s t
Task 
< 
List 
< 
Appointment 
> 
> 8
,GetTodayConfirmedAppointmentsByDoctorIdAsync  L
(L M
intM P
doctorIdQ Y
,Y Z
CancellationToken[ l
ctm o
=p q
defaultr y
)y z
;z {
Task 
< 
List 
< 
Appointment 
> 
> 4
(GetCancelledAppointmentsByPatientIdAsync  H
(H I
intI L
	patientIdM V
,V W
CancellationTokenX i
ctj l
=m n
defaulto v
)v w
;w x
Task 
< 
List 
< 
Appointment 
> 
> 3
'GetCancelledAppointmentsByDoctorIdAsync  G
(G H
intH K
doctorIdL T
,T U
CancellationTokenV g
cth j
=k l
defaultm t
)t u
;u v
Task 
< 
int 
> 7
+CountActiveAppointmentsByDoctorAndDateAsync =
(= >
int> A
doctorIdB J
,J K
DateTimeL T
dateU Y
,Y Z
CancellationToken[ l
ctm o
=p q
defaultr y
)y z
;z {
Task   
<   
bool   
>   
IsSlotBookedAsync   $
(  $ %
int  % (
doctorId  ) 1
,  1 2
DateTime  3 ;
date  < @
,  @ A
string  B H
timeSlot  I Q
,  Q R
CancellationToken  S d
ct  e g
=  h i
default  j q
)  q r
;  r s
Task"" 
<"" 
bool"" 
>"" <
0PatientHasActiveAppointmentWithDoctorOnDateAsync"" C
(""C D
int""D G
	patientId""H Q
,""Q R
int""S V
doctorId""W _
,""_ `
DateTime""a i
date""j n
,""n o
CancellationToken	""p Å
ct
""Ç Ñ
=
""Ö Ü
default
""á é
)
""é è
;
""è ê
Task$$ 
<$$ 
bool$$ 
>$$ 9
-PatientHasActiveAppointmentOnDateAndSlotAsync$$ @
($$@ A
int$$A D
	patientId$$E N
,$$N O
DateTime$$P X
date$$Y ]
,$$] ^
string$$_ e
timeSlot$$f n
,$$n o
CancellationToken	$$p Å
ct
$$Ç Ñ
=
$$Ö Ü
default
$$á é
)
$$é è
;
$$è ê
}%% 
}&& «,
_C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Impl\Repository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
Impl# '
{ 
public 

class 

Repository 
< 
T 
> 
:  
IRepository! ,
<, -
T- .
>. /
where0 5
T6 7
:8 9
class: ?
{

 
private 
readonly 
	DbContext "
_context# +
;+ ,
public 

Repository 
( 
	DbContext #
context$ +
)+ ,
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
T 
> 
CreateAsync (
(( )
T) *
entity+ 1
,1 2
CancellationToken3 D
ctE G
=H I
defaultJ Q
)Q R
{ 	
await 
_context 
. 
Set 
< 
T  
>  !
(! "
)" #
.# $
AddAsync$ ,
(, -
entity- 3
,3 4
ct5 7
)7 8
;8 9
await 
_context 
. 
SaveChangesAsync +
(+ ,
ct, .
). /
;/ 0
return 
entity 
; 
}   	
public"" 
async"" 
Task"" 
<"" 
T"" 
?"" 
>"" 
DeleteAsync"" )
("") *
int""* -
id"". 0
,""0 1
CancellationToken""2 C
ct""D F
=""G H
default""I P
)""P Q
{$$ 	
var&& 
existing&& 
=&& 
await&&  
_context&&! )
.&&) *
Set&&* -
<&&- .
T&&. /
>&&/ 0
(&&0 1
)&&1 2
.&&2 3
	FindAsync&&3 <
(&&< =
[&&= >
id&&> @
]&&@ A
,&&A B
ct&&C E
)&&E F
;&&F G
if(( 
((( 
existing(( 
is(( 
null((  
)((  !
{** 
return,, 
null,, 
;,, 
}.. 
_context00 
.00 
Set00 
<00 
T00 
>00 
(00 
)00 
.00 
Remove00 $
(00$ %
existing00% -
)00- .
;00. /
await22 
_context22 
.22 
SaveChangesAsync22 +
(22+ ,
ct22, .
)22. /
;22/ 0
return44 
existing44 
;44 
}66 	
public88 
async88 
Task88 
<88 
List88 
<88 
T88  
>88  !
>88! "
GetAllAsync88# .
(88. /
CancellationToken88/ @
ct88A C
=88D E
default88F M
)88M N
{:: 	
return<< 
await<< 
_context<< !
.<<! "
Set<<" %
<<<% &
T<<& '
><<' (
(<<( )
)<<) *
.<<* +
ToListAsync<<+ 6
(<<6 7
ct<<7 9
)<<9 :
;<<: ;
}>> 	
public@@ 
async@@ 
Task@@ 
<@@ 
T@@ 
?@@ 
>@@ 
GetByIdAsync@@ *
(@@* +
int@@+ .
id@@/ 1
,@@1 2
CancellationToken@@3 D
ct@@E G
=@@H I
default@@J Q
)@@Q R
{BB 	
varDD 
existingDD 
=DD 
awaitDD  
_contextDD! )
.DD) *
SetDD* -
<DD- .
TDD. /
>DD/ 0
(DD0 1
)DD1 2
.DD2 3
	FindAsyncDD3 <
(DD< =
[DD= >
idDD> @
]DD@ A
,DDA B
ctDDC E
)DDE F
;DDF G
returnFF 
existingFF 
;FF 
}HH 	
publicJJ 
asyncJJ 
TaskJJ 
<JJ 
TJJ 
?JJ 
>JJ 
UpdateAsyncJJ )
(JJ) *
intJJ* -
idJJ. 0
,JJ0 1
TJJ2 3
entityJJ4 :
,JJ: ;
CancellationTokenJJ< M
ctJJN P
=JJQ R
defaultJJS Z
)JJZ [
{LL 	
varNN 
existingNN 
=NN 
awaitNN  
_contextNN! )
.NN) *
SetNN* -
<NN- .
TNN. /
>NN/ 0
(NN0 1
)NN1 2
.NN2 3
	FindAsyncNN3 <
(NN< =
[NN= >
idNN> @
]NN@ A
,NNA B
ctNNC E
)NNE F
;NNF G
ifPP 
(PP 
existingPP 
isPP 
nullPP  
)PP  !
{RR 
returnTT 
nullTT 
;TT 
}VV 
_contextXX 
.XX 
EntryXX 
(XX 
existingXX #
)XX# $
.XX$ %
CurrentValuesXX% 2
.XX2 3
	SetValuesXX3 <
(XX< =
entityXX= C
)XXC D
;XXD E
awaitZZ 
_contextZZ 
.ZZ 
SaveChangesAsyncZZ +
(ZZ+ ,
ctZZ, .
)ZZ. /
;ZZ/ 0
return\\ 
existing\\ 
;\\ 
}^^ 	
}`` 
}bb ˘
kC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Interface\IDoctorRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
	Interface# ,
{ 
public 

	interface 
IDoctorRepository &
:' (
IRepository) 4
<4 5
Doctor5 ;
>; <
{ 
Task		 
<		 
List		 
<		 
Doctor		 
>		 
>		 
GetAllActiveAsync		 ,
(		, -
CancellationToken		- >
ct		? A
=		B C
default		D K
)		K L
;		L M
Task 
< 
List 
< 
Doctor 
> 
> $
GetBySpecialisationAsync 3
(3 4
SpecialisationType4 F
specialisationG U
,U V
CancellationTokenW h
cti k
=l m
defaultn u
)u v
;v w
Task 
< 
List 
< 
Doctor 
> 
> *
GetActiveBySpecialisationAsync 9
(9 :
SpecialisationType: L
specialisationM [
,[ \
CancellationToken] n
cto q
=r s
defaultt {
){ |
;| }
Task 
< 
bool 
> 
ExistsByEmailAsync %
(% &
string& ,
email- 2
,2 3
CancellationToken4 E
ctF H
=I J
defaultK R
)R S
;S T
Task 
< 
Doctor 
? 
> $
GetByIdentityUserIdAsync .
(. /
string/ 5
identityUserId6 D
,D E
CancellationTokenF W
ctX Z
=[ \
default] d
)d e
;e f
} 
} Ω,
kC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Impl\HealthRecordRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
Impl# '
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
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -
GetByPatientIdAsync. A
(A B
intB E
	patientIdF O
,O P
CancellationTokenQ b
ctc e
=f g
defaulth o
)o p
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
. 
Where 
( 
hr 
=> 
hr 
.  
	PatientId  )
==* ,
	patientId- 6
)6 7
. 
OrderByDescending "
(" #
hr# %
=>& (
hr) +
.+ ,
	VisitDate, 5
)5 6
. 
ToListAsync 
( 
ct 
)  
;  !
} 	
public 
async 
Task 
< 
List 
< 
HealthRecord +
>+ ,
>, -
GetByDoctorIdAsync. @
(@ A
intA D
doctorIdE M
,M N
CancellationTokenO `
cta c
=d e
defaultf m
)m n
{ 	
return 
await 
_context !
.! "
HealthRecords" /
. 
Include 
( 
hr 
=> 
hr !
.! "
Patient" )
)) *
.   
Include   
(   
hr   
=>   
hr   !
.  ! "
Doctor  " (
)  ( )
.!! 
Include!! 
(!! 
hr!! 
=>!! 
hr!! !
.!!! "
Appointment!!" -
)!!- .
."" 
Where"" 
("" 
hr"" 
=>"" 
hr"" 
.""  
DoctorId""  (
=="") +
doctorId"", 4
)""4 5
.## 
OrderByDescending## "
(##" #
hr### %
=>##& (
hr##) +
.##+ ,
	VisitDate##, 5
)##5 6
.$$ 
ToListAsync$$ 
($$ 
ct$$ 
)$$  
;$$  !
}%% 	
public'' 
async'' 
Task'' 
<'' 
List'' 
<'' 
HealthRecord'' +
>''+ ,
>'', -#
GetByAppointmentIdAsync''. E
(''E F
int''F I
appointmentId''J W
,''W X
CancellationToken''Y j
ct''k m
=''n o
default''p w
)''w x
{(( 	
return)) 
await)) 
_context)) !
.))! "
HealthRecords))" /
.** 
Include** 
(** 
hr** 
=>** 
hr** !
.**! "
Patient**" )
)**) *
.++ 
Include++ 
(++ 
hr++ 
=>++ 
hr++ !
.++! "
Doctor++" (
)++( )
.,, 
Include,, 
(,, 
hr,, 
=>,, 
hr,, !
.,,! "
Appointment,," -
),,- .
.-- 
Where-- 
(-- 
hr-- 
=>-- 
hr-- 
.--  
AppointmentId--  -
==--. 0
appointmentId--1 >
)--> ?
... 
ToListAsync.. 
(.. 
ct.. 
)..  
;..  !
}// 	
public11 
async11 
Task11 
<11 
bool11 
>11 &
ExistsByAppointmentIdAsync11  :
(11: ;
int11; >
appointmentId11? L
,11L M
CancellationToken11N _
ct11` b
=11c d
default11e l
)11l m
{22 	
return33 
await33 
_context33 !
.33! "
HealthRecords33" /
.44 
AnyAsync44 
(44 
hr44 
=>44 
hr44  "
.44" #
AppointmentId44# 0
==441 3
appointmentId444 A
,44A B
ct44C E
)44E F
;44F G
}55 	
}66 
}77 ≥&
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Impl\DoctorRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
Impl# '
{ 
public 

class 
DoctorRepository !
:" #

Repository$ .
<. /
Doctor/ 5
>5 6
,6 7
IDoctorRepository8 I
{ 
private 
readonly 
HealthAxisDbContext ,
_context- 5
;5 6
public 
DoctorRepository 
(  
HealthAxisDbContext  3
context4 ;
); <
:= >
base? C
(C D
contextD K
)K L
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
List 
< 
Doctor %
>% &
>& '
GetAllActiveAsync( 9
(9 :
CancellationToken: K
ctL N
=O P
defaultQ X
)X Y
{ 	
return 
await 
_context !
.! "
Doctors" )
. 
Where 
( 
d 
=> 
d 
. 
IsActive &
)& '
. 
ToListAsync 
( 
ct 
)  
;  !
} 	
public 
async 
Task 
< 
List 
< 
Doctor %
>% &
>& '$
GetBySpecialisationAsync( @
(@ A
SpecialisationTypeA S
specialisationT b
,b c
CancellationTokend u
ctv x
=y z
default	{ Ç
)
Ç É
{ 	
return 
await 
_context !
.! "
Doctors" )
.   
Where   
(   
d   
=>   
d   
.   
Specialisation   ,
==  - /
specialisation  0 >
)  > ?
.!! 
ToListAsync!! 
(!! 
ct!! 
)!!  
;!!  !
}"" 	
public$$ 
async$$ 
Task$$ 
<$$ 
List$$ 
<$$ 
Doctor$$ %
>$$% &
>$$& '*
GetActiveBySpecialisationAsync$$( F
($$F G
SpecialisationType$$G Y
specialisation$$Z h
,$$h i
CancellationToken$$j {
ct$$| ~
=	$$ Ä
default
$$Å à
)
$$à â
{%% 	
return&& 
await&& 
_context&& !
.&&! "
Doctors&&" )
.'' 
Where'' 
('' 
d'' 
=>'' 
d'' 
.'' 
IsActive'' &
&&''' )
d''* +
.''+ ,
Specialisation'', :
==''; =
specialisation''> L
)''L M
.(( 
ToListAsync(( 
((( 
ct(( 
)((  
;((  !
}** 	
public++ 
async++ 
Task++ 
<++ 
bool++ 
>++ 
ExistsByEmailAsync++  2
(++2 3
string++3 9
email++: ?
,++? @
CancellationToken++A R
ct++S U
=++V W
default++X _
)++_ `
{,, 	
string-- 
normalizedEmail-- "
=--# $
email--% *
.--* +
ToUpperInvariant--+ ;
(--; <
)--< =
;--= >
return// 
await// 
_context// !
.//! "
Doctors//" )
.00 
AnyAsync00 
(00 
d00 
=>00 
d00  
.00  !
Email00! &
.00& '
ToUpper00' .
(00. /
)00/ 0
==001 3
normalizedEmail004 C
,00C D
ct00E G
)00G H
;00H I
}11 	
public22 
async22 
Task22 
<22 
Doctor22  
?22  !
>22! "$
GetByIdentityUserIdAsync22# ;
(22; <
string33 

identityUserId33 
,33 
CancellationToken44 
ct44 
=44 
default44 "
)44" #
{55 	
return66 
await66 
_context66 !
.66! "
Doctors66" )
.77 
FirstOrDefaultAsync77 $
(77$ %
d77% &
=>77' )
d77* +
.77+ ,
IdentityUserId77, :
==77; =
identityUserId77> L
,77L M
ct77N P
)77P Q
;77Q R
}88 	
}:: 
};; ò∑
jC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Repository\Impl\AppointmentRepository.cs
	namespace 	
HealthCareApp
 
. 

Repository "
." #
Impl# '
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
public 
async 
Task 
< 
List 
< 
Appointment *
>* +
>+ ,
GetByPatientIdAsync- @
(@ A
intA D
	patientIdE N
,N O
CancellationTokenP a
ctb d
=e f
defaultg n
)n o
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
. 
Include 
( 
a 
=> 
a 
.  
Doctor  &
)& '
. 
Where 
( 
a 
=> 
a 
. 
	PatientId '
==( *
	patientId+ 4
)4 5
. 
OrderByDescending "
(" #
a# $
=>% '
a( )
.) *
ScheduledDate* 7
)7 8
. 
ToListAsync 
( 
ct 
)  
;  !
} 	
public 
async 
Task 
< 
List 
< 
Appointment *
>* +
>+ ,
GetByDoctorIdAsync- ?
(? @
int@ C
doctorIdD L
,L M
CancellationTokenN _
ct` b
=c d
defaulte l
)l m
{ 	
return 
await 
_context !
.! "
Appointments" .
. 
Include 
( 
a 
=> 
a 
.  
Patient  '
)' (
.   
Include   
(   
a   
=>   
a   
.    
Doctor    &
)  & '
.!! 
Where!! 
(!! 
a!! 
=>!! 
a!! 
.!! 
DoctorId!! &
==!!' )
doctorId!!* 2
)!!2 3
."" 
OrderByDescending"" "
(""" #
a""# $
=>""% '
a""( )
."") *
ScheduledDate""* 7
)""7 8
.## 
ToListAsync## 
(## 
ct## 
)##  
;##  !
}$$ 	
public&& 
async&& 
Task&& 
<&& 
List&& 
<&& 
Appointment&& *
>&&* +
>&&+ ,
GetByStatusAsync&&- =
(&&= >
AppointmentStatus&&> O
status&&P V
,&&V W
CancellationToken&&X i
ct&&j l
=&&m n
default&&o v
)&&v w
{'' 	
return(( 
await(( 
_context(( !
.((! "
Appointments((" .
.)) 
Include)) 
()) 
a)) 
=>)) 
a)) 
.))  
Patient))  '
)))' (
.** 
Include** 
(** 
a** 
=>** 
a** 
.**  
Doctor**  &
)**& '
.++ 
Where++ 
(++ 
a++ 
=>++ 
a++ 
.++ 
Status++ $
==++% '
status++( .
)++. /
.,, 
OrderByDescending,, "
(,," #
a,,# $
=>,,% '
a,,( )
.,,) *
ScheduledDate,,* 7
),,7 8
.-- 
ToListAsync-- 
(-- 
ct-- 
)--  
;--  !
}.. 	
public00 
async00 
Task00 
<00 
List00 
<00 
Appointment00 *
>00* +
>00+ ,(
GetUpcomingAppointmentsAsync00- I
(00I J
CancellationToken00J [
ct00\ ^
=00_ `
default00a h
)00h i
{11 	
return22 
await22 
_context22 !
.22! "
Appointments22" .
.33 
Include33 
(33 
a33 
=>33 
a33 
.33  
Patient33  '
)33' (
.44 
Include44 
(44 
a44 
=>44 
a44 
.44  
Doctor44  &
)44& '
.55 
Where55 
(55 
a55 
=>55 
a55 
.55 
ScheduledDate55 +
.55+ ,
Date55, 0
>=551 3
DateTime554 <
.55< =
Today55= B
&&66 
a66  
.66  !
Status66! '
!=66( *
AppointmentStatus66+ <
.66< =
	Cancelled66= F
&&77 
a77  
.77  !
Status77! '
!=77( *
AppointmentStatus77+ <
.77< =
	Completed77= F
)77F G
.88 
OrderBy88 
(88 
a88 
=>88 
a88 
.88  
ScheduledDate88  -
)88- .
.99 
ToListAsync99 
(99 
ct99 
)99  
;99  !
}:: 	
public<< 
async<< 
Task<< 
<<< 
List<< 
<<< 
Appointment<< *
><<* +
><<+ ,3
'GetUpcomingAppointmentsByPatientIdAsync<<- T
(<<T U
int<<U X
	patientId<<Y b
,<<b c
CancellationToken<<d u
ct<<v x
=<<y z
default	<<{ Ç
)
<<Ç É
{== 	
return>> 
await>> 
_context>> !
.>>! "
Appointments>>" .
.?? 
Include?? 
(?? 
a?? 
=>?? 
a?? 
.??  
Patient??  '
)??' (
.@@ 
Include@@ 
(@@ 
a@@ 
=>@@ 
a@@ 
.@@  
Doctor@@  &
)@@& '
.AA 
WhereAA 
(AA 
aAA 
=>AA 
aAA 
.AA 
	PatientIdAA '
==AA( *
	patientIdAA+ 4
&&BB 
aBB  
.BB  !
ScheduledDateBB! .
.BB. /
DateBB/ 3
>=BB4 6
DateTimeBB7 ?
.BB? @
TodayBB@ E
&&CC 
aCC  
.CC  !
StatusCC! '
!=CC( *
AppointmentStatusCC+ <
.CC< =
	CancelledCC= F
&&DD 
aDD  
.DD  !
StatusDD! '
!=DD( *
AppointmentStatusDD+ <
.DD< =
	CompletedDD= F
)DDF G
.EE 
OrderByEE 
(EE 
aEE 
=>EE 
aEE 
.EE  
ScheduledDateEE  -
)EE- .
.FF 
ToListAsyncFF 
(FF 
ctFF 
)FF  
;FF  !
}GG 	
publicII 
asyncII 
TaskII 
<II 
ListII 
<II 
AppointmentII *
>II* +
>II+ ,2
&GetUpcomingAppointmentsByDoctorIdAsyncII- S
(IIS T
intIIT W
doctorIdIIX `
,II` a
CancellationTokenIIb s
ctIIt v
=IIw x
default	IIy Ä
)
IIÄ Å
{JJ 	
returnKK 
awaitKK 
_contextKK !
.KK! "
AppointmentsKK" .
.LL 
IncludeLL 
(LL 
aLL 
=>LL 
aLL 
.LL  
PatientLL  '
)LL' (
.MM 
IncludeMM 
(MM 
aMM 
=>MM 
aMM 
.MM  
DoctorMM  &
)MM& '
.NN 
WhereNN 
(NN 
aNN 
=>NN 
aNN 
.NN 
DoctorIdNN &
==NN' )
doctorIdNN* 2
&&OO 
aOO  
.OO  !
ScheduledDateOO! .
.OO. /
DateOO/ 3
>=OO4 6
DateTimeOO7 ?
.OO? @
TodayOO@ E
&&PP 
aPP  
.PP  !
StatusPP! '
!=PP( *
AppointmentStatusPP+ <
.PP< =
	CancelledPP= F
&&QQ 
aQQ  
.QQ  !
StatusQQ! '
!=QQ( *
AppointmentStatusQQ+ <
.QQ< =
	CompletedQQ= F
)QQF G
.RR 
OrderByRR 
(RR 
aRR 
=>RR 
aRR 
.RR  
ScheduledDateRR  -
)RR- .
.SS 
ToListAsyncSS 
(SS 
ctSS 
)SS  
;SS  !
}TT 	
publicVV 
asyncVV 
TaskVV 
<VV 
ListVV 
<VV 
AppointmentVV *
>VV* +
>VV+ ,2
&GetPendingAppointmentsByPatientIdAsyncVV- S
(VVS T
intVVT W
	patientIdVVX a
,VVa b
CancellationTokenVVc t
ctVVu w
=VVx y
default	VVz Å
)
VVÅ Ç
{WW 	
returnXX 
awaitXX 
_contextXX !
.XX! "
AppointmentsXX" .
.YY 
IncludeYY 
(YY 
aYY 
=>YY 
aYY 
.YY  
PatientYY  '
)YY' (
.ZZ 
IncludeZZ 
(ZZ 
aZZ 
=>ZZ 
aZZ 
.ZZ  
DoctorZZ  &
)ZZ& '
.[[ 
Where[[ 
([[ 
a[[ 
=>[[ 
a[[ 
.[[ 
	PatientId[[ '
==[[( *
	patientId[[+ 4
&&\\ 
a\\  
.\\  !
Status\\! '
==\\( *
AppointmentStatus\\+ <
.\\< =
Pending\\= D
)\\D E
.]] 
OrderBy]] 
(]] 
a]] 
=>]] 
a]] 
.]]  
ScheduledDate]]  -
)]]- .
.^^ 
ToListAsync^^ 
(^^ 
ct^^ 
)^^  
;^^  !
}__ 	
publicaa 
asyncaa 
Taskaa 
<aa 
Listaa 
<aa 
Appointmentaa *
>aa* +
>aa+ ,1
%GetPendingAppointmentsByDoctorIdAsyncaa- R
(aaR S
intaaS V
doctorIdaaW _
,aa_ `
CancellationTokenaaa r
ctaas u
=aav w
defaultaax 
)	aa Ä
{bb 	
returncc 
awaitcc 
_contextcc !
.cc! "
Appointmentscc" .
.dd 
Includedd 
(dd 
add 
=>dd 
add 
.dd  
Patientdd  '
)dd' (
.ee 
Includeee 
(ee 
aee 
=>ee 
aee 
.ee  
Doctoree  &
)ee& '
.ff 
Whereff 
(ff 
aff 
=>ff 
aff 
.ff 
DoctorIdff &
==ff' )
doctorIdff* 2
&&gg 
agg  
.gg  !
Statusgg! '
==gg( *
AppointmentStatusgg+ <
.gg< =
Pendinggg= D
)ggD E
.hh 
OrderByhh 
(hh 
ahh 
=>hh 
ahh 
.hh  
ScheduledDatehh  -
)hh- .
.ii 
ToListAsyncii 
(ii 
ctii 
)ii  
;ii  !
}jj 	
publicll 
asyncll 
Taskll 
<ll 
Listll 
<ll 
Appointmentll *
>ll* +
>ll+ ,8
,GetTodayConfirmedAppointmentsByDoctorIdAsyncll- Y
(llY Z
intllZ ]
doctorIdll^ f
,llf g
CancellationTokenllh y
ctllz |
=ll} ~
default	ll Ü
)
llÜ á
{mm 	
returnnn 
awaitnn 
_contextnn !
.nn! "
Appointmentsnn" .
.oo 
Includeoo 
(oo 
aoo 
=>oo 
aoo 
.oo  
Patientoo  '
)oo' (
.pp 
Includepp 
(pp 
app 
=>pp 
app 
.pp  
Doctorpp  &
)pp& '
.qq 
Whereqq 
(qq 
aqq 
=>qq 
aqq 
.qq 
DoctorIdqq &
==qq' )
doctorIdqq* 2
&&rr 
arr  
.rr  !
ScheduledDaterr! .
.rr. /
Daterr/ 3
==rr4 6
DateTimerr7 ?
.rr? @
Todayrr@ E
&&ss 
ass  
.ss  !
Statusss! '
==ss( *
AppointmentStatusss+ <
.ss< =
	Confirmedss= F
)ssF G
.tt 
OrderBytt 
(tt 
att 
=>tt 
att 
.tt  
TimeSlottt  (
)tt( )
.uu 
ToListAsyncuu 
(uu 
ctuu 
)uu  
;uu  !
}vv 	
publicxx 
asyncxx 
Taskxx 
<xx 
Listxx 
<xx 
Appointmentxx *
>xx* +
>xx+ ,4
(GetCancelledAppointmentsByPatientIdAsyncxx- U
(xxU V
intxxV Y
	patientIdxxZ c
,xxc d
CancellationTokenxxe v
ctxxw y
=xxz {
default	xx| É
)
xxÉ Ñ
{yy 	
returnzz 
awaitzz 
_contextzz !
.zz! "
Appointmentszz" .
.{{ 
Include{{ 
({{ 
a{{ 
=>{{ 
a{{ 
.{{  
Patient{{  '
){{' (
.|| 
Include|| 
(|| 
a|| 
=>|| 
a|| 
.||  
Doctor||  &
)||& '
.}} 
Where}} 
(}} 
a}} 
=>}} 
a}} 
.}} 
	PatientId}} '
==}}( *
	patientId}}+ 4
&&~~ 
a~~  
.~~  !
Status~~! '
==~~( *
AppointmentStatus~~+ <
.~~< =
	Cancelled~~= F
)~~F G
. 
OrderByDescending "
(" #
a# $
=>% '
a( )
.) *
ScheduledDate* 7
)7 8
.
ÄÄ 
ToListAsync
ÄÄ 
(
ÄÄ 
ct
ÄÄ 
)
ÄÄ  
;
ÄÄ  !
}
ÅÅ 	
public
ÉÉ 
async
ÉÉ 
Task
ÉÉ 
<
ÉÉ 
List
ÉÉ 
<
ÉÉ 
Appointment
ÉÉ *
>
ÉÉ* +
>
ÉÉ+ ,5
'GetCancelledAppointmentsByDoctorIdAsync
ÉÉ- T
(
ÉÉT U
int
ÉÉU X
doctorId
ÉÉY a
,
ÉÉa b
CancellationToken
ÉÉc t
ct
ÉÉu w
=
ÉÉx y
defaultÉÉz Å
)ÉÉÅ Ç
{
ÑÑ 	
return
ÖÖ 
await
ÖÖ 
_context
ÖÖ !
.
ÖÖ! "
Appointments
ÖÖ" .
.
ÜÜ 
Include
ÜÜ 
(
ÜÜ 
a
ÜÜ 
=>
ÜÜ 
a
ÜÜ 
.
ÜÜ  
Patient
ÜÜ  '
)
ÜÜ' (
.
áá 
Include
áá 
(
áá 
a
áá 
=>
áá 
a
áá 
.
áá  
Doctor
áá  &
)
áá& '
.
àà 
Where
àà 
(
àà 
a
àà 
=>
àà 
a
àà 
.
àà 
DoctorId
àà &
==
àà' )
doctorId
àà* 2
&&
ââ 
a
ââ  
.
ââ  !
Status
ââ! '
==
ââ( *
AppointmentStatus
ââ+ <
.
ââ< =
	Cancelled
ââ= F
)
ââF G
.
ää 
OrderByDescending
ää "
(
ää" #
a
ää# $
=>
ää% '
a
ää( )
.
ää) *
ScheduledDate
ää* 7
)
ää7 8
.
ãã 
ToListAsync
ãã 
(
ãã 
ct
ãã 
)
ãã  
;
ãã  !
}
åå 	
public
éé 
async
éé 
Task
éé 
<
éé 
int
éé 
>
éé 9
+CountActiveAppointmentsByDoctorAndDateAsync
éé J
(
ééJ K
int
ééK N
doctorId
ééO W
,
ééW X
DateTime
ééY a
date
ééb f
,
ééf g
CancellationToken
ééh y
ct
ééz |
=
éé} ~
defaultéé Ü
)ééÜ á
{
èè 	
var
êê 
selectedDate
êê 
=
êê 
date
êê #
.
êê# $
Date
êê$ (
;
êê( )
return
íí 
await
íí 
_context
íí !
.
íí! "
Appointments
íí" .
.
ìì 

CountAsync
ìì 
(
ìì 
a
ìì 
=>
ìì  
a
ìì! "
.
ìì" #
DoctorId
ìì# +
==
ìì, .
doctorId
ìì/ 7
&&
îî! #
a
îî$ %
.
îî% &
ScheduledDate
îî& 3
.
îî3 4
Date
îî4 8
==
îî9 ;
selectedDate
îî< H
&&
ïï! #
a
ïï$ %
.
ïï% &
Status
ïï& ,
!=
ïï- /
AppointmentStatus
ïï0 A
.
ïïA B
	Cancelled
ïïB K
&&
ññ! #
a
ññ$ %
.
ññ% &
Status
ññ& ,
!=
ññ- /
AppointmentStatus
ññ0 A
.
ññA B
	Completed
ññB K
,
ññK L
ct
óó 
)
óó 
;
óó  
}
òò 	
public
öö 
async
öö 
Task
öö 
<
öö 
bool
öö 
>
öö 
IsSlotBookedAsync
öö  1
(
öö1 2
int
öö2 5
doctorId
öö6 >
,
öö> ?
DateTime
öö@ H
date
ööI M
,
ööM N
string
ööO U
timeSlot
ööV ^
,
öö^ _
CancellationToken
öö` q
ct
öör t
=
ööu v
default
ööw ~
)
öö~ 
{
õõ 	
var
úú 
selectedDate
úú 
=
úú 
date
úú #
.
úú# $
Date
úú$ (
;
úú( )
return
ûû 
await
ûû 
_context
ûû !
.
ûû! "
Appointments
ûû" .
.
üü 
AnyAsync
üü 
(
üü 
a
üü 
=>
üü 
a
üü  
.
üü  !
DoctorId
üü! )
==
üü* ,
doctorId
üü- 5
&&
†† !
a
††" #
.
††# $
ScheduledDate
††$ 1
.
††1 2
Date
††2 6
==
††7 9
selectedDate
††: F
&&
°° !
a
°°" #
.
°°# $
TimeSlot
°°$ ,
==
°°- /
timeSlot
°°0 8
&&
¢¢ !
a
¢¢" #
.
¢¢# $
Status
¢¢$ *
!=
¢¢+ -
AppointmentStatus
¢¢. ?
.
¢¢? @
	Cancelled
¢¢@ I
&&
££ !
a
££" #
.
££# $
Status
££$ *
!=
££+ -
AppointmentStatus
££. ?
.
££? @
	Completed
££@ I
,
££I J
ct
§§ 
)
§§ 
;
§§ 
}
•• 	
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
bool
ßß 
>
ßß >
0PatientHasActiveAppointmentWithDoctorOnDateAsync
ßß  P
(
ßßP Q
int
®® 
	patientId
®® 
,
®® 
int
©© 
doctorId
©© 
,
©© 
DateTime
™™ 
date
™™ 
,
™™ 
CancellationToken
´´ 
ct
´´  
=
´´! "
default
´´# *
)
´´* +
{
¨¨ 	
var
≠≠ 
selectedDate
≠≠ 
=
≠≠ 
date
≠≠ #
.
≠≠# $
Date
≠≠$ (
;
≠≠( )
return
ØØ 
await
ØØ 
_context
ØØ !
.
ØØ! "
Appointments
ØØ" .
.
∞∞ 
AnyAsync
∞∞ 
(
∞∞ 
a
∞∞ 
=>
∞∞ 
a
∞∞  
.
∞∞  !
	PatientId
∞∞! *
==
∞∞+ -
	patientId
∞∞. 7
&&
±± !
a
±±" #
.
±±# $
DoctorId
±±$ ,
==
±±- /
doctorId
±±0 8
&&
≤≤ !
a
≤≤" #
.
≤≤# $
ScheduledDate
≤≤$ 1
.
≤≤1 2
Date
≤≤2 6
==
≤≤7 9
selectedDate
≤≤: F
&&
≥≥ !
a
≥≥" #
.
≥≥# $
Status
≥≥$ *
!=
≥≥+ -
AppointmentStatus
≥≥. ?
.
≥≥? @
	Cancelled
≥≥@ I
&&
¥¥ !
a
¥¥" #
.
¥¥# $
Status
¥¥$ *
!=
¥¥+ -
AppointmentStatus
¥¥. ?
.
¥¥? @
	Completed
¥¥@ I
,
¥¥I J
ct
µµ 
)
µµ 
;
µµ 
}
∂∂ 	
public
∏∏ 
async
∏∏ 
Task
∏∏ 
<
∏∏ 
bool
∏∏ 
>
∏∏ ;
-PatientHasActiveAppointmentOnDateAndSlotAsync
∏∏  M
(
∏∏M N
int
ππ 
	patientId
ππ 
,
ππ 
DateTime
∫∫ 
date
∫∫ 
,
∫∫ 
string
ªª 
timeSlot
ªª 
,
ªª 
CancellationToken
ºº 
ct
ºº  
=
ºº! "
default
ºº# *
)
ºº* +
{
ΩΩ 	
var
ææ 
selectedDate
ææ 
=
ææ 
date
ææ #
.
ææ# $
Date
ææ$ (
;
ææ( )
return
¿¿ 
await
¿¿ 
_context
¿¿ !
.
¿¿! "
Appointments
¿¿" .
.
¡¡ 
AnyAsync
¡¡ 
(
¡¡ 
a
¡¡ 
=>
¡¡ 
a
¡¡  
.
¡¡  !
	PatientId
¡¡! *
==
¡¡+ -
	patientId
¡¡. 7
&&
¬¬ !
a
¬¬" #
.
¬¬# $
ScheduledDate
¬¬$ 1
.
¬¬1 2
Date
¬¬2 6
==
¬¬7 9
selectedDate
¬¬: F
&&
√√ !
a
√√" #
.
√√# $
TimeSlot
√√$ ,
==
√√- /
timeSlot
√√0 8
&&
ƒƒ !
a
ƒƒ" #
.
ƒƒ# $
Status
ƒƒ$ *
!=
ƒƒ+ -
AppointmentStatus
ƒƒ. ?
.
ƒƒ? @
	Cancelled
ƒƒ@ I
&&
≈≈ !
a
≈≈" #
.
≈≈# $
Status
≈≈$ *
!=
≈≈+ -
AppointmentStatus
≈≈. ?
.
≈≈? @
	Completed
≈≈@ I
,
≈≈I J
ct
∆∆ 
)
∆∆ 
;
∆∆ 
}
«« 	
}
»» 
}…… Õn
LC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Program.cs
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
. 
AddJsonOptions 
( 
options 
=> 
{ 
options 
. !
JsonSerializerOptions %
.% & 
PropertyNamingPolicy& :
=; <
System 
. 
Text 
. 
Json 
. 
JsonNamingPolicy -
.- .
	CamelCase. 7
;7 8
} 
) 
; 
builder 
. 
Services 
. 
AddDbContext 
< 
HealthAxisDbContext 1
>1 2
(2 3
options3 :
=>; =
options 
. 
UseSqlServer 
( 
builder  
.  !
Configuration! .
.. /
GetConnectionString/ B
(B C
$strC J
)J K
)K L
)L M
;M N
builder 
. 
Services 
. 
AddIdentity 
< 
IdentityUser )
,) *
IdentityRole+ 7
>7 8
(8 9
options9 @
=>A C
{   
options!! 
.!! 
User!! 
.!! 
RequireUniqueEmail!! #
=!!$ %
true!!& *
;!!* +
options## 
.## 
Password## 
.## 
RequireDigit## !
=##" #
true##$ (
;##( )
options$$ 
.$$ 
Password$$ 
.$$ 
RequireLowercase$$ %
=$$& '
true$$( ,
;$$, -
options%% 
.%% 
Password%% 
.%% 
RequireUppercase%% %
=%%& '
true%%( ,
;%%, -
options&& 
.&& 
Password&& 
.&& "
RequireNonAlphanumeric&& +
=&&, -
true&&. 2
;&&2 3
options'' 
.'' 
Password'' 
.'' 
RequiredLength'' #
=''$ %
$num''& '
;''' (
}(( 
)(( 
.)) $
AddEntityFrameworkStores)) 
<)) 
HealthAxisDbContext)) -
>))- .
()). /
)))/ 0
.** $
AddDefaultTokenProviders** 
(** 
)** 
;** 
builder-- 
.-- 
Services-- 
.-- 
AddAuthentication-- "
(--" #
JwtBearerDefaults--# 4
.--4 5 
AuthenticationScheme--5 I
)--I J
... 
AddJwtBearer.. 
(.. 
options.. 
=>.. 
{// 
var00 
jwt00 
=00 
builder00 
.00 
Configuration00 '
.00' (

GetSection00( 2
(002 3
$str003 8
)008 9
;009 :
options22 
.22 %
TokenValidationParameters22 )
=22* +
new22, /%
TokenValidationParameters220 I
{33 	
ValidateIssuer44 
=44 
true44 !
,44! "
ValidIssuer55 
=55 
jwt55 
[55 
$str55 &
]55& '
,55' (
ValidateAudience77 
=77 
true77 #
,77# $
ValidAudience88 
=88 
jwt88 
[88  
$str88  *
]88* +
,88+ ,
ValidateLifetime:: 
=:: 
true:: #
,::# $$
ValidateIssuerSigningKey<< $
=<<% &
true<<' +
,<<+ ,
IssuerSigningKey== 
=== 
new== " 
SymmetricSecurityKey==# 7
(==7 8
Encoding>> 
.>> 
UTF8>> 
.>> 
GetBytes>> &
(>>& '
jwt>>' *
[>>* +
$str>>+ 0
]>>0 1
!>>1 2
)>>2 3
)?? 
,?? 
	ClockSkewAA 
=AA 
TimeSpanAA  
.AA  !
ZeroAA! %
}BB 	
;BB	 

}CC 
)CC 
;CC 
builderEE 
.EE 
ServicesEE 
.EE 
AddSwaggerGenEE 
(EE 
optionsEE &
=>EE' )
{FF 
optionsGG 
.GG 

SwaggerDocGG 
(GG 
$strGG 
,GG 
newGG  
OpenApiInfoGG! ,
{HH 
TitleII 
=II 
$strII 
,II  
VersionJJ 
=JJ 
$strJJ 
}KK 
)KK 
;KK 
optionsMM 
.MM !
AddSecurityDefinitionMM !
(MM! "
$strMM" *
,MM* +
newMM, /!
OpenApiSecuritySchemeMM0 E
{NN 
TypeOO 
=OO 
SecuritySchemeTypeOO !
.OO! "
HttpOO" &
,OO& '
SchemePP 
=PP 
$strPP 
,PP 
BearerFormatQQ 
=QQ 
$strQQ 
,QQ 
DescriptionRR 
=RR 
$strRR A
}SS 
)SS 
;SS 
optionsUU 
.UU "
AddSecurityRequirementUU "
(UU" #
documentUU# +
=>UU, .
newUU/ 2&
OpenApiSecurityRequirementUU3 M
{VV 
[WW 	
newWW	 *
OpenApiSecuritySchemeReferenceWW +
(WW+ ,
$strWW, 4
,WW4 5
documentWW6 >
)WW> ?
]WW? @
=WWA B
[WWC D
]WWD E
}XX 
)XX 
;XX 
}YY 
)YY 
;YY 
builder\\ 
.\\ 
Services\\ 
.\\ 
AddAuthorization\\ !
(\\! "
)\\" #
;\\# $
builder__ 
.__ 
Services__ 
.__ 
	AddScoped__ 
<__ 
	DbContext__ $
,__$ %
HealthAxisDbContext__& 9
>__9 :
(__: ;
)__; <
;__< =
builderbb 
.bb 
Servicesbb 
.bb 
	AddScopedbb 
<bb 
IAuthServicebb '
,bb' (
AuthServicebb) 4
>bb4 5
(bb5 6
)bb6 7
;bb7 8
builderee 
.ee 
Servicesee 
.ee 
AddAutoMapperee 
(ee 
cfgee "
=>ee# %
{ff 
cfggg 
.gg 

AddProfilegg 
<gg 
MappingProfilegg !
>gg! "
(gg" #
)gg# $
;gg$ %
}hh 
)hh 
;hh 
builderkk 
.kk 
Serviceskk 
.kk 
	AddScopedkk 
(kk 
typeofkk !
(kk! "
IRepositorykk" -
<kk- .
>kk. /
)kk/ 0
,kk0 1
typeofkk2 8
(kk8 9

Repositorykk9 C
<kkC D
>kkD E
)kkE F
)kkF G
;kkG H
buildernn 
.nn 
Servicesnn 
.nn 
	AddScopednn 
<nn 
IPatientRepositorynn -
,nn- .
PatientRepositorynn/ @
>nn@ A
(nnA B
)nnB C
;nnC D
builderoo 
.oo 
Servicesoo 
.oo 
	AddScopedoo 
<oo 
IDoctorRepositoryoo ,
,oo, -
DoctorRepositoryoo. >
>oo> ?
(oo? @
)oo@ A
;ooA B
builderpp 
.pp 
Servicespp 
.pp 
	AddScopedpp 
<pp "
IAppointmentRepositorypp 1
,pp1 2!
AppointmentRepositorypp3 H
>ppH I
(ppI J
)ppJ K
;ppK L
builderqq 
.qq 
Servicesqq 
.qq 
	AddScopedqq 
<qq #
IHealthRecordRepositoryqq 2
,qq2 3"
HealthRecordRepositoryqq4 J
>qqJ K
(qqK L
)qqL M
;qqM N
builderss 
.ss 
Servicesss 
.ss 
	AddScopedss 
<ss 
IAuthServicess '
,ss' (
AuthServicess) 4
>ss4 5
(ss5 6
)ss6 7
;ss7 8
buildervv 
.vv 
Servicesvv 
.vv 
	AddScopedvv 
<vv 
IPatientServicevv *
,vv* +
PatientServicevv, :
>vv: ;
(vv; <
)vv< =
;vv= >
builderww 
.ww 
Servicesww 
.ww 
	AddScopedww 
<ww 
IDoctorServiceww )
,ww) *
DoctorServiceww+ 8
>ww8 9
(ww9 :
)ww: ;
;ww; <
builderxx 
.xx 
Servicesxx 
.xx 
	AddScopedxx 
<xx 
IAppointmentServicexx .
,xx. /
AppointmentServicexx0 B
>xxB C
(xxC D
)xxD E
;xxE F
builderyy 
.yy 
Servicesyy 
.yy 
	AddScopedyy 
<yy  
IHealthRecordServiceyy /
,yy/ 0
HealthRecordServiceyy1 D
>yyD E
(yyE F
)yyF G
;yyG H
builder|| 
.|| 
Services|| 
.|| 
AddExceptionHandler|| $
<||$ %"
GlobalExceptionHandler||% ;
>||; <
(||< =
)||= >
;||> ?
builder}} 
.}} 
Services}} 
.}} 
AddProblemDetails}} "
(}}" #
)}}# $
;}}$ %
builderÄÄ 
.
ÄÄ 
Services
ÄÄ 
.
ÄÄ %
AddEndpointsApiExplorer
ÄÄ (
(
ÄÄ( )
)
ÄÄ) *
;
ÄÄ* +
builderÅÅ 
.
ÅÅ 
Services
ÅÅ 
.
ÅÅ 
AddSwaggerGen
ÅÅ 
(
ÅÅ 
)
ÅÅ  
;
ÅÅ  !
constÉÉ 
string
ÉÉ 
BlazorCorsPolicy
ÉÉ 
=
ÉÉ 
$str
ÉÉ  2
;
ÉÉ2 3
builderÖÖ 
.
ÖÖ 
Services
ÖÖ 
.
ÖÖ 
AddCors
ÖÖ 
(
ÖÖ 
options
ÖÖ  
=>
ÖÖ! #
{ÜÜ 
options
áá 
.
áá 
	AddPolicy
áá 
(
áá 
BlazorCorsPolicy
áá &
,
áá& '
policy
áá( .
=>
áá/ 1
{
àà 
policy
ââ 
.
ââ 
WithOrigins
ââ 
(
ââ 
$str
ââ 3
)
ââ3 4
.
ää 
AllowAnyHeader
ää 
(
ää 
)
ää 
.
ãã 
AllowAnyMethod
ãã 
(
ãã 
)
ãã 
;
ãã  
}
åå 
)
åå 
;
åå 
}çç 
)
çç 
;
çç 
varêê 
app
êê 
=
êê 	
builder
êê
 
.
êê 
Build
êê 
(
êê 
)
êê 
;
êê 
usingìì 
(
ìì 
var
ìì 

scope
ìì 
=
ìì 
app
ìì 
.
ìì 
Services
ìì 
.
ìì  
CreateScope
ìì  +
(
ìì+ ,
)
ìì, -
)
ìì- .
{îî 
var
ïï 
roleManager
ïï 
=
ïï 
scope
ïï 
.
ïï 
ServiceProvider
ïï +
.
ïï+ , 
GetRequiredService
ïï, >
<
ïï> ?
RoleManager
ïï? J
<
ïïJ K
IdentityRole
ïïK W
>
ïïW X
>
ïïX Y
(
ïïY Z
)
ïïZ [
;
ïï[ \
var
óó 
userManager
óó 
=
óó 
scope
óó 
.
óó 
ServiceProvider
óó +
.
óó+ , 
GetRequiredService
óó, >
<
óó> ?
UserManager
óó? J
<
óóJ K
IdentityUser
óóK W
>
óóW X
>
óóX Y
(
óóY Z
)
óóZ [
;
óó[ \
await
ôô 	

RoleSeeder
ôô
 
.
ôô 
SeedRoleAsync
ôô "
(
ôô" #
roleManager
ôô# .
)
ôô. /
;
ôô/ 0
await
õõ 	
AdminSeeder
õõ
 
.
õõ 
SeedAdminAsync
õõ $
(
õõ$ %
userManager
õõ% 0
,
õõ0 1
roleManager
õõ2 =
,
õõ= >
builder
õõ? F
.
õõF G
Configuration
õõG T
)
õõT U
;
õõU V
}úú 
appûû 
.
ûû !
UseExceptionHandler
ûû 
(
ûû 
)
ûû 
;
ûû 
if°° 
(
°° 
app
°° 
.
°° 
Environment
°° 
.
°° 
IsDevelopment
°° !
(
°°! "
)
°°" #
)
°°# $
{¢¢ 
app
££ 
.
££ 

UseSwagger
££ 
(
££ 
)
££ 
;
££ 
app
§§ 
.
§§ 
UseSwaggerUI
§§ 
(
§§ 
)
§§ 
;
§§ 
}•• 
appßß 
.
ßß !
UseHttpsRedirection
ßß 
(
ßß 
)
ßß 
;
ßß 
app™™ 
.
™™ 
UseCors
™™ 
(
™™ 
BlazorCorsPolicy
™™ 
)
™™ 
;
™™ 
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
app≥≥ 
.
≥≥ 
Run
≥≥ 
(
≥≥ 
)
≥≥ 	
;
≥≥	 
¬
SC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Patient.cs
	namespace 	
HealthCareApp
 
. 
Models 
{ 
public 

class 
Patient 
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
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
RegularExpression	 
( 
$str 0
,0 1
ErrorMessage2 >
=? @
$str	A Ö
)
Ö Ü
]
Ü á
[ 	
	MinLength	 
( 
$num 
) 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
required 
string 
PatientName *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
[ 	
Required	 
] 
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
] 
public 

GenderType 
Gender  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
required 
string 
Email $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
] 
[ 	
Phone	 
] 
public 
required 
string 
PhoneNumber *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public!! 
string!! 
?!! 
InsuranceID!! "
{!!# $
get!!% (
;!!( )
set!!* -
;!!- .
}!!/ 0
public## 
string## 
?## 
IdentityUserId## %
{##& '
get##( +
;##+ ,
set##- 0
;##0 1
}##2 3
[%% 	

ForeignKey%%	 
(%% 
nameof%% 
(%% 
IdentityUserId%% )
)%%) *
)%%* +
]%%+ ,
public&& 
IdentityUser&& 
?&& 
IdentityUser&& )
{&&* +
get&&, /
;&&/ 0
set&&1 4
;&&4 5
}&&6 7
public(( 
DateTime(( 
CreatedDate(( #
{(($ %
get((& )
;(() *
set((+ .
;((. /
}((0 1
=((2 3
DateTime((4 <
.((< =
Now((= @
;((@ A
public** 
ICollection** 
<** 
Appointment** &
>**& '
Appointments**( 4
{**5 6
get**7 :
;**: ;
set**< ?
;**? @
}**A B
=**C D
new**E H
List**I M
<**M N
Appointment**N Y
>**Y Z
(**Z [
)**[ \
;**\ ]
public,, 
ICollection,, 
<,, 
HealthRecord,, '
>,,' (
HealthRecords,,) 6
{,,7 8
get,,9 <
;,,< =
set,,> A
;,,A B
},,C D
=,,E F
new,,G J
List,,K O
<,,O P
HealthRecord,,P \
>,,\ ]
(,,] ^
),,^ _
;,,_ `
}-- 
}.. ø
XC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\HealthRecord.cs
	namespace 	
HealthCareApp
 
. 
Models 
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
}		. /
[ 	
Required	 
] 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	

ForeignKey	 
( 
nameof 
( 
	PatientId $
)$ %
)% &
]& '
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
;4 5
public 
int 
? 
DoctorId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	

ForeignKey	 
( 
nameof 
( 
DoctorId #
)# $
)$ %
]% &
public 
Doctor 
? 
Doctor 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
Required	 
] 
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	

ForeignKey	 
( 
nameof 
( 
AppointmentId (
)( )
)) *
]* +
public 
Appointment 
Appointment &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
null7 ;
!; <
;< =
[ 	
Required	 
] 
public 
DateTime 
	VisitDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
] 
[   	
	MaxLength  	 
(   
$num   
)   
]   
public!! 
string!! 
	Diagnosis!! 
{!!  !
get!!" %
;!!% &
set!!' *
;!!* +
}!!, -
=!!. /
null!!0 4
!!!4 5
;!!5 6
[## 	
Required##	 
]## 
[$$ 	
	MaxLength$$	 
($$ 
$num$$ 
)$$ 
]$$ 
public%% 
string%% 
Prescription%% "
{%%# $
get%%% (
;%%( )
set%%* -
;%%- .
}%%/ 0
=%%1 2
null%%3 7
!%%7 8
;%%8 9
['' 	
	MaxLength''	 
('' 
$num'' 
)'' 
]'' 
public(( 
string(( 
?(( 
Notes(( 
{(( 
get(( "
;((" #
set(($ '
;((' (
}(() *
public** 
DateTime** 
CreatedDate** #
{**$ %
get**& )
;**) *
set**+ .
;**. /
}**0 1
=**2 3
DateTime**4 <
.**< =
Now**= @
;**@ A
}++ 
},, Ú
RC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Doctor.cs
	namespace 	
HealthCareApp
 
. 
Models 
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
] 
[ 	
	MinLength	 
( 
$num 
) 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
[ 	
RegularExpression	 
( 
$str 0
,0 1
ErrorMessage2 >
=? @
$str	A Ö
)
Ö Ü
]
Ü á
public 
string 

DoctorName  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
null1 5
!5 6
;6 7
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
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
;8 9
[ 	
Required	 
] 
public 
SpecialisationType !
Specialisation" 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage "
=# $
$str% P
)P Q
]Q R
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) K
)K L
]L M
public 
int 
ConsultationFee "
{# $
get% (
;( )
set* -
;- .
}/ 0
public!! 
string!! 
?!! 
IdentityUserId!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!2 3
[$$ 	
Required$$	 
]$$ 
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
public'' 
DateTime'' 
CreatedDate'' #
{''$ %
get''& )
;'') *
set''+ .
;''. /
}''0 1
=''2 3
DateTime''4 <
.''< =
Now''= @
;''@ A
public)) 
ICollection)) 
<)) 
Appointment)) &
>))& '
?))' (
Appointments))) 5
{))6 7
get))8 ;
;)); <
set))= @
;))@ A
}))B C
public++ 
ICollection++ 
<++ 
HealthRecord++ '
>++' (
?++( )
HealthRecords++* 7
{++8 9
get++: =
;++= >
set++? B
;++B C
}++D E
},, 
}-- ¢
XC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\AuthResponse.cs
	namespace 	
HealthCareApp
 
. 
Models 
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
public		 
int		 
	ExpiresIn		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
}

 
} ·
WC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Appointment.cs
	namespace 	
HealthCareApp
 
. 
Models 
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
}) *
[ 	

ForeignKey	 
( 
nameof 
( 
	PatientId $
)$ %
)% &
]& '
public 
Patient 
Patient 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	

ForeignKey	 
( 
nameof 
( 
DoctorId #
)# $
)$ %
]% &
public 
Doctor 
Doctor 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
[ 	
Required	 
] 
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
TimeSlot 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
null/ 3
!3 4
;4 5
[ 	
Required	 
] 
public   
AppointmentStatus    
Status  ! '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  4 5
["" 	
	MaxLength""	 
("" 
$num"" 
)"" 
]"" 
public## 
string## 
?## 
CancellationReason## )
{##* +
get##, /
;##/ 0
set##1 4
;##4 5
}##6 7
public%% 
DateTime%% 
CreatedDate%% #
{%%$ %
get%%& )
;%%) *
set%%+ .
;%%. /
}%%0 1
=%%2 3
DateTime%%4 <
.%%< =
Now%%= @
;%%@ A
public'' 
HealthRecord'' 
?'' 
HealthRecord'' )
{''* +
get'', /
;''/ 0
set''1 4
;''4 5
}''6 7
}(( 
})) ú
uC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260621154631_UpdateSeedDateTimeKind.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public 

partial 
class "
UpdateSeedDateTimeKind /
:0 1
	Migration2 ;
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
} î
qC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260618201917_Addpatientidentity.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public 

partial 
class 
Addpatientidentity +
:, -
	Migration. 7
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
} ≥ 
}C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260618200105_AddPatientIdentityUserRelation.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public 

partial 
class *
AddPatientIdentityUserRelation 7
:8 9
	Migration: C
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
$str &
,& '
table 
: 
$str !
,! "
type 
: 
$str %
,% &
nullable 
: 
true 
) 
;  
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str !
,! "
	keyColumn 
: 
$str &
,& '
keyValue 
: 
$num 
, 
column 
: 
$str (
,( )
value 
: 
null 
) 
; 
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str !
,! "
	keyColumn 
: 
$str &
,& '
keyValue 
: 
$num 
, 
column 
: 
$str (
,( )
value 
: 
null 
) 
; 
migrationBuilder!! 
.!! 

UpdateData!! '
(!!' (
table"" 
:"" 
$str"" !
,""! "
	keyColumn## 
:## 
$str## &
,##& '
keyValue$$ 
:$$ 
$num$$ 
,$$ 
column%% 
:%% 
$str%% (
,%%( )
value&& 
:&& 
null&& 
)&& 
;&& 
migrationBuilder(( 
.(( 
CreateIndex(( (
(((( )
name)) 
:)) 
$str)) 2
,))2 3
table** 
:** 
$str** !
,**! "
column++ 
:++ 
$str++ (
,++( )
unique,, 
:,, 
true,, 
,,, 
filter-- 
:-- 
$str-- 6
)--6 7
;--7 8
migrationBuilder// 
.// 
AddForeignKey// *
(//* +
name00 
:00 
$str00 >
,00> ?
table11 
:11 
$str11 !
,11! "
column22 
:22 
$str22 (
,22( )
principalTable33 
:33 
$str33  -
,33- .
principalColumn44 
:44  
$str44! %
)44% &
;44& '
}55 	
	protected88 
override88 
void88 
Down88  $
(88$ %
MigrationBuilder88% 5
migrationBuilder886 F
)88F G
{99 	
migrationBuilder:: 
.:: 
DropForeignKey:: +
(::+ ,
name;; 
:;; 
$str;; >
,;;> ?
table<< 
:<< 
$str<< !
)<<! "
;<<" #
migrationBuilder>> 
.>> 
	DropIndex>> &
(>>& '
name?? 
:?? 
$str?? 2
,??2 3
table@@ 
:@@ 
$str@@ !
)@@! "
;@@" #
migrationBuilderBB 
.BB 

DropColumnBB '
(BB' (
nameCC 
:CC 
$strCC &
,CC& '
tableDD 
:DD 
$strDD !
)DD! "
;DD" #
}EE 	
}FF 
}GG ß
{C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260618085527_RemoveDoctorVerificationFlow.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public 

partial 
class (
RemoveDoctorVerificationFlow 5
:6 7
	Migration8 A
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
. 

DropColumn '
(' (
name 
: 
$str *
,* +
table 
: 
$str  
)  !
;! "
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 
	AddColumn &
<& '
int' *
>* +
(+ ,
name 
: 
$str *
,* +
table 
: 
$str  
,  !
type 
: 
$str 
, 
nullable 
: 
false 
,  
defaultValue 
: 
$num 
)  
;  !
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
$str   ,
,  , -
value!! 
:!! 
$num!! 
)!! 
;!! 
migrationBuilder## 
.## 

UpdateData## '
(##' (
table$$ 
:$$ 
$str$$  
,$$  !
	keyColumn%% 
:%% 
$str%% %
,%%% &
keyValue&& 
:&& 
$num&& 
,&& 
column'' 
:'' 
$str'' ,
,'', -
value(( 
:(( 
$num(( 
)(( 
;(( 
migrationBuilder** 
.** 

UpdateData** '
(**' (
table++ 
:++ 
$str++  
,++  !
	keyColumn,, 
:,, 
$str,, %
,,,% &
keyValue-- 
:-- 
$num-- 
,-- 
column.. 
:.. 
$str.. ,
,.., -
value// 
:// 
$num// 
)// 
;// 
}00 	
}11 
}22 Æ'
xC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260617184259_DoctorApprovalFlowUpdated.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public 

partial 
class %
DoctorApprovalFlowUpdated 2
:3 4
	Migration5 >
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
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
, 
columns 
: 
new 
[ 
] 
{  
$str! 2
,2 3
$str4 ;
,; <
$str= Q
}R S
,S T
values 
: 
new 
object "
[" #
]# $
{% &
$num' +
,+ ,
$str- E
,E F
$numG H
}I J
)J K
;K L
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
, 
columns 
: 
new 
[ 
] 
{  
$str! (
,( )
$str* >
}? @
,@ A
values 
: 
new 
object "
[" #
]# $
{% &
$str' ?
,? @
$numA B
}C D
)D E
;E F
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str  
,  !
	keyColumn 
: 
$str %
,% &
keyValue 
: 
$num 
, 
columns 
: 
new 
[ 
] 
{  
$str! (
,( )
$str* >
}? @
,@ A
values   
:   
new   
object   "
[  " #
]  # $
{  % &
$str  ' ?
,  ? @
$num  A B
}  C D
)  D E
;  E F
}!! 	
	protected$$ 
override$$ 
void$$ 
Down$$  $
($$$ %
MigrationBuilder$$% 5
migrationBuilder$$6 F
)$$F G
{%% 	
migrationBuilder&& 
.&& 

UpdateData&& '
(&&' (
table'' 
:'' 
$str''  
,''  !
	keyColumn(( 
:(( 
$str(( %
,((% &
keyValue)) 
:)) 
$num)) 
,)) 
columns** 
:** 
new** 
[** 
]** 
{**  
$str**! 2
,**2 3
$str**4 ;
,**; <
$str**= Q
}**R S
,**S T
values++ 
:++ 
new++ 
object++ "
[++" #
]++# $
{++% &
$num++' *
,++* +
$str++, .
,++. /
$num++0 1
}++2 3
)++3 4
;++4 5
migrationBuilder-- 
.-- 

UpdateData-- '
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
,00 
columns11 
:11 
new11 
[11 
]11 
{11  
$str11! (
,11( )
$str11* >
}11? @
,11@ A
values22 
:22 
new22 
object22 "
[22" #
]22# $
{22% &
$str22' )
,22) *
$num22+ ,
}22- .
)22. /
;22/ 0
migrationBuilder44 
.44 

UpdateData44 '
(44' (
table55 
:55 
$str55  
,55  !
	keyColumn66 
:66 
$str66 %
,66% &
keyValue77 
:77 
$num77 
,77 
columns88 
:88 
new88 
[88 
]88 
{88  
$str88! (
,88( )
$str88* >
}88? @
,88@ A
values99 
:99 
new99 
object99 "
[99" #
]99# $
{99% &
$str99' )
,99) *
$num99+ ,
}99- .
)99. /
;99/ 0
}:: 	
};; 
}<< ñ(
vC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260617183838_DoctorApprovalFlowAdded.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public 

partial 
class #
DoctorApprovalFlowAdded 0
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
,% &
nullable 
: 
false 
,  
defaultValue 
: 
$str  
)  !
;! "
migrationBuilder 
. 
	AddColumn &
<& '
string' -
>- .
(. /
name 
: 
$str &
,& '
table 
: 
$str  
,  !
type 
: 
$str %
,% &
nullable 
: 
true 
) 
;  
migrationBuilder 
. 
	AddColumn &
<& '
int' *
>* +
(+ ,
name 
: 
$str *
,* +
table 
: 
$str  
,  !
type 
: 
$str 
, 
nullable 
: 
false 
,  
defaultValue 
: 
$num 
)  
;  !
migrationBuilder!! 
.!! 

UpdateData!! '
(!!' (
table"" 
:"" 
$str""  
,""  !
	keyColumn## 
:## 
$str## %
,##% &
keyValue$$ 
:$$ 
$num$$ 
,$$ 
columns%% 
:%% 
new%% 
[%% 
]%% 
{%%  
$str%%! (
,%%( )
$str%%* :
,%%: ;
$str%%< P
}%%Q R
,%%R S
values&& 
:&& 
new&& 
object&& "
[&&" #
]&&# $
{&&% &
$str&&' )
,&&) *
null&&+ /
,&&/ 0
$num&&1 2
}&&3 4
)&&4 5
;&&5 6
migrationBuilder(( 
.(( 

UpdateData(( '
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
,++ 
columns,, 
:,, 
new,, 
[,, 
],, 
{,,  
$str,,! (
,,,( )
$str,,* :
,,,: ;
$str,,< P
},,Q R
,,,R S
values-- 
:-- 
new-- 
object-- "
[--" #
]--# $
{--% &
$str--' )
,--) *
null--+ /
,--/ 0
$num--1 2
}--3 4
)--4 5
;--5 6
migrationBuilder// 
.// 

UpdateData// '
(//' (
table00 
:00 
$str00  
,00  !
	keyColumn11 
:11 
$str11 %
,11% &
keyValue22 
:22 
$num22 
,22 
columns33 
:33 
new33 
[33 
]33 
{33  
$str33! (
,33( )
$str33* :
,33: ;
$str33< P
}33Q R
,33R S
values44 
:44 
new44 
object44 "
[44" #
]44# $
{44% &
$str44' )
,44) *
null44+ /
,44/ 0
$num441 2
}443 4
)444 5
;445 6
}55 	
	protected88 
override88 
void88 
Down88  $
(88$ %
MigrationBuilder88% 5
migrationBuilder886 F
)88F G
{99 	
migrationBuilder:: 
.:: 

DropColumn:: '
(::' (
name;; 
:;; 
$str;; 
,;; 
table<< 
:<< 
$str<<  
)<<  !
;<<! "
migrationBuilder>> 
.>> 

DropColumn>> '
(>>' (
name?? 
:?? 
$str?? &
,??& '
table@@ 
:@@ 
$str@@  
)@@  !
;@@! "
migrationBuilderBB 
.BB 

DropColumnBB '
(BB' (
nameCC 
:CC 
$strCC *
,CC* +
tableDD 
:DD 
$strDD  
)DD  !
;DD! "
}EE 	
}FF 
}GG ˘≈
rC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260616114957_aspnetidentityadded.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public		 

partial		 
class		 
aspnetidentityadded		 ,
:		- .
	Migration		/ 8
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
}‡‡ ≤;
lC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260616114249_DropUserTable.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{		 
public 

partial 
class 
DropUserTable &
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
. 
	DropTable &
(& '
name 
: 
$str 
) 
; 
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str 
, 
columns 
: 
table 
=> !
new" %
{ 
UserId 
= 
table "
." #
Column# )
<) *
int* -
>- .
(. /
type/ 3
:3 4
$str5 :
,: ;
nullable< D
:D E
falseF K
)K L
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
CreatedDate 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
,\ ]
Email 
= 
table !
.! "
Column" (
<( )
string) /
>/ 0
(0 1
type1 5
:5 6
$str7 F
,F G
	maxLengthH Q
:Q R
$numS V
,V W
nullableX `
:` a
falseb g
)g h
,h i
PasswordHash  
=! "
table# (
.( )
Column) /
</ 0
string0 6
>6 7
(7 8
type8 <
:< =
$str> M
,M N
nullableO W
:W X
falseY ^
)^ _
,_ `
PasswordSalt    
=  ! "
table  # (
.  ( )
Column  ) /
<  / 0
byte  0 4
[  4 5
]  5 6
>  6 7
(  7 8
type  8 <
:  < =
$str  > N
,  N O
nullable  P X
:  X Y
false  Z _
)  _ `
,  ` a
ReferenceId!! 
=!!  !
table!!" '
.!!' (
Column!!( .
<!!. /
int!!/ 2
>!!2 3
(!!3 4
type!!4 8
:!!8 9
$str!!: ?
,!!? @
nullable!!A I
:!!I J
true!!K O
)!!O P
,!!P Q
Role"" 
="" 
table""  
.""  !
Column""! '
<""' (
int""( +
>""+ ,
("", -
type""- 1
:""1 2
$str""3 8
,""8 9
nullable"": B
:""B C
false""D I
)""I J
}## 
,## 
constraints$$ 
:$$ 
table$$ "
=>$$# %
{%% 
table&& 
.&& 

PrimaryKey&& $
(&&$ %
$str&&% /
,&&/ 0
x&&1 2
=>&&3 5
x&&6 7
.&&7 8
UserId&&8 >
)&&> ?
;&&? @
}'' 
)'' 
;'' 
migrationBuilder)) 
.)) 

InsertData)) '
())' (
table** 
:** 
$str** 
,** 
columns++ 
:++ 
new++ 
[++ 
]++ 
{++  
$str++! )
,++) *
$str+++ 8
,++8 9
$str++: A
,++A B
$str++C Q
,++Q R
$str++S a
,++a b
$str++c p
,++p q
$str++r x
}++y z
,++z {
values,, 
:,, 
new,, 
object,, "
[,," #
,,,# $
],,$ %
{-- 
{.. 
$num.. 
,.. 
new.. 
DateTime.. %
(..% &
$num..& *
,..* +
$num.., -
,..- .
$num../ 1
,..1 2
$num..3 4
,..4 5
$num..6 7
,..7 8
$num..9 :
,..: ;
$num..< =
,..= >
DateTimeKind..? K
...K L
Unspecified..L W
)..W X
,..X Y
$str..Z p
,..p q
$str	..r â
,
..â ä
new
..ã é
byte
..è ì
[
..ì î
]
..î ï
{
..ñ ó
$num
..ò ô
,
..ô ö
$num
..õ ú
,
..ú ù
$num
..û ü
,
..ü †
$num
..° ¢
,
..¢ £
$num
..§ •
}
..¶ ß
,
..ß ®
null
..© ≠
,
..≠ Æ
$num
..Ø ∞
}
..± ≤
,
..≤ ≥
{// 
$num// 
,// 
new// 
DateTime// %
(//% &
$num//& *
,//* +
$num//, -
,//- .
$num/// 1
,//1 2
$num//3 4
,//4 5
$num//6 7
,//7 8
$num//9 :
,//: ;
$num//< =
,//= >
DateTimeKind//? K
.//K L
Unspecified//L W
)//W X
,//X Y
$str//Z r
,//r s
$str	//t ç
,
//ç é
new
//è í
byte
//ì ó
[
//ó ò
]
//ò ô
{
//ö õ
$num
//ú ù
,
//ù û
$num
//ü †
,
//† °
$num
//¢ £
,
//£ §
$num
//• ¶
,
//¶ ß
$num
//® ™
}
//´ ¨
,
//¨ ≠
$num
//Æ Ø
,
//Ø ∞
$num
//± ≤
}
//≥ ¥
,
//¥ µ
{00 
$num00 
,00 
new00 
DateTime00 %
(00% &
$num00& *
,00* +
$num00, -
,00- .
$num00/ 1
,001 2
$num003 4
,004 5
$num006 7
,007 8
$num009 :
,00: ;
$num00< =
,00= >
DateTimeKind00? K
.00K L
Unspecified00L W
)00W X
,00X Y
$str00Z r
,00r s
$str	00t å
,
00å ç
new
00é ë
byte
00í ñ
[
00ñ ó
]
00ó ò
{
00ô ö
$num
00õ ù
,
00ù û
$num
00ü °
,
00° ¢
$num
00£ •
,
00• ¶
$num
00ß ©
,
00© ™
$num
00´ ≠
}
00Æ Ø
,
00Ø ∞
$num
00± ≤
,
00≤ ≥
$num
00¥ µ
}
00∂ ∑
}11 
)11 
;11 
migrationBuilder33 
.33 
CreateIndex33 (
(33( )
name44 
:44 
$str44 &
,44& '
table55 
:55 
$str55 
,55 
column66 
:66 
$str66 
,66  
unique77 
:77 
true77 
)77 
;77 
}88 	
}99 
}:: ∞;
kC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615050714_AddUserTable.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{		 
public 

partial 
class 
AddUserTable %
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
$str 
, 
columns 
: 
table 
=> !
new" %
{ 
UserId 
= 
table "
." #
Column# )
<) *
int* -
>- .
(. /
type/ 3
:3 4
$str5 :
,: ;
nullable< D
:D E
falseF K
)K L
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
Email 
= 
table !
.! "
Column" (
<( )
string) /
>/ 0
(0 1
type1 5
:5 6
$str7 F
,F G
	maxLengthH Q
:Q R
$numS V
,V W
nullableX `
:` a
falseb g
)g h
,h i
PasswordHash  
=! "
table# (
.( )
Column) /
</ 0
string0 6
>6 7
(7 8
type8 <
:< =
$str> M
,M N
nullableO W
:W X
falseY ^
)^ _
,_ `
PasswordSalt  
=! "
table# (
.( )
Column) /
</ 0
byte0 4
[4 5
]5 6
>6 7
(7 8
type8 <
:< =
$str> N
,N O
nullableP X
:X Y
falseZ _
)_ `
,` a
Role 
= 
table  
.  !
Column! '
<' (
int( +
>+ ,
(, -
type- 1
:1 2
$str3 8
,8 9
nullable: B
:B C
falseD I
)I J
,J K
ReferenceId 
=  !
table" '
.' (
Column( .
<. /
int/ 2
>2 3
(3 4
type4 8
:8 9
$str: ?
,? @
nullableA I
:I J
trueK O
)O P
,P Q
CreatedDate 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% /
,/ 0
x1 2
=>3 5
x6 7
.7 8
UserId8 >
)> ?
;? @
}   
)   
;   
migrationBuilder"" 
."" 

InsertData"" '
(""' (
table## 
:## 
$str## 
,## 
columns$$ 
:$$ 
new$$ 
[$$ 
]$$ 
{$$  
$str$$! )
,$$) *
$str$$+ 8
,$$8 9
$str$$: A
,$$A B
$str$$C Q
,$$Q R
$str$$S a
,$$a b
$str$$c p
,$$p q
$str$$r x
}$$y z
,$$z {
values%% 
:%% 
new%% 
object%% "
[%%" #
,%%# $
]%%$ %
{&& 
{'' 
$num'' 
,'' 
new'' 
DateTime'' %
(''% &
$num''& *
,''* +
$num'', -
,''- .
$num''/ 1
,''1 2
$num''3 4
,''4 5
$num''6 7
,''7 8
$num''9 :
,'': ;
$num''< =
,''= >
DateTimeKind''? K
.''K L
Unspecified''L W
)''W X
,''X Y
$str''Z p
,''p q
$str	''r â
,
''â ä
new
''ã é
byte
''è ì
[
''ì î
]
''î ï
{
''ñ ó
$num
''ò ô
,
''ô ö
$num
''õ ú
,
''ú ù
$num
''û ü
,
''ü †
$num
''° ¢
,
''¢ £
$num
''§ •
}
''¶ ß
,
''ß ®
null
''© ≠
,
''≠ Æ
$num
''Ø ∞
}
''± ≤
,
''≤ ≥
{(( 
$num(( 
,(( 
new(( 
DateTime(( %
(((% &
$num((& *
,((* +
$num((, -
,((- .
$num((/ 1
,((1 2
$num((3 4
,((4 5
$num((6 7
,((7 8
$num((9 :
,((: ;
$num((< =
,((= >
DateTimeKind((? K
.((K L
Unspecified((L W
)((W X
,((X Y
$str((Z r
,((r s
$str	((t ç
,
((ç é
new
((è í
byte
((ì ó
[
((ó ò
]
((ò ô
{
((ö õ
$num
((ú ù
,
((ù û
$num
((ü †
,
((† °
$num
((¢ £
,
((£ §
$num
((• ¶
,
((¶ ß
$num
((® ™
}
((´ ¨
,
((¨ ≠
$num
((Æ Ø
,
((Ø ∞
$num
((± ≤
}
((≥ ¥
,
((¥ µ
{)) 
$num)) 
,)) 
new)) 
DateTime)) %
())% &
$num))& *
,))* +
$num)), -
,))- .
$num))/ 1
,))1 2
$num))3 4
,))4 5
$num))6 7
,))7 8
$num))9 :
,)): ;
$num))< =
,))= >
DateTimeKind))? K
.))K L
Unspecified))L W
)))W X
,))X Y
$str))Z r
,))r s
$str	))t å
,
))å ç
new
))é ë
byte
))í ñ
[
))ñ ó
]
))ó ò
{
))ô ö
$num
))õ ù
,
))ù û
$num
))ü °
,
))° ¢
$num
))£ •
,
))• ¶
$num
))ß ©
,
))© ™
$num
))´ ≠
}
))Æ Ø
,
))Ø ∞
$num
))± ≤
,
))≤ ≥
$num
))¥ µ
}
))∂ ∑
}** 
)** 
;** 
migrationBuilder,, 
.,, 
CreateIndex,, (
(,,( )
name-- 
:-- 
$str-- &
,--& '
table.. 
:.. 
$str.. 
,.. 
column// 
:// 
$str// 
,//  
unique00 
:00 
true00 
)00 
;00 
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
.66 
	DropTable66 &
(66& '
name77 
:77 
$str77 
)77 
;77 
}88 	
}99 
}:: ´m
sC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615045634_AddHealthRecordTable.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{		 
public 

partial 
class  
AddHealthRecordTable -
:. /
	Migration0 9
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
. 
DropForeignKey +
(+ ,
name 
: 
$str 8
,8 9
table 
: 
$str %
)% &
;& '
migrationBuilder 
. 
DropForeignKey +
(+ ,
name 
: 
$str :
,: ;
table 
: 
$str %
)% &
;& '
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str %
,% &
columns 
: 
table 
=> !
new" %
{ 
HealthRecordId "
=# $
table% *
.* +
Column+ 1
<1 2
int2 5
>5 6
(6 7
type7 ;
:; <
$str= B
,B C
nullableD L
:L M
falseN S
)S T
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
	PatientId 
= 
table  %
.% &
Column& ,
<, -
int- 0
>0 1
(1 2
type2 6
:6 7
$str8 =
,= >
nullable? G
:G H
falseI N
)N O
,O P
DoctorId 
= 
table $
.$ %
Column% +
<+ ,
int, /
>/ 0
(0 1
type1 5
:5 6
$str7 <
,< =
nullable> F
:F G
trueH L
)L M
,M N
AppointmentId   !
=  " #
table  $ )
.  ) *
Column  * 0
<  0 1
int  1 4
>  4 5
(  5 6
type  6 :
:  : ;
$str  < A
,  A B
nullable  C K
:  K L
false  M R
)  R S
,  S T
	VisitDate!! 
=!! 
table!!  %
.!!% &
Column!!& ,
<!!, -
DateTime!!- 5
>!!5 6
(!!6 7
type!!7 ;
:!!; <
$str!!= H
,!!H I
nullable!!J R
:!!R S
false!!T Y
)!!Y Z
,!!Z [
	Diagnosis"" 
="" 
table""  %
.""% &
Column""& ,
<"", -
string""- 3
>""3 4
(""4 5
type""5 9
:""9 :
$str""; J
,""J K
	maxLength""L U
:""U V
$num""W Z
,""Z [
nullable""\ d
:""d e
false""f k
)""k l
,""l m
Prescription##  
=##! "
table### (
.##( )
Column##) /
<##/ 0
string##0 6
>##6 7
(##7 8
type##8 <
:##< =
$str##> M
,##M N
	maxLength##O X
:##X Y
$num##Z ]
,##] ^
nullable##_ g
:##g h
false##i n
)##n o
,##o p
Notes$$ 
=$$ 
table$$ !
.$$! "
Column$$" (
<$$( )
string$$) /
>$$/ 0
($$0 1
type$$1 5
:$$5 6
$str$$7 G
,$$G H
	maxLength$$I R
:$$R S
$num$$T X
,$$X Y
nullable$$Z b
:$$b c
true$$d h
)$$h i
,$$i j
CreatedDate%% 
=%%  !
table%%" '
.%%' (
Column%%( .
<%%. /
DateTime%%/ 7
>%%7 8
(%%8 9
type%%9 =
:%%= >
$str%%? J
,%%J K
nullable%%L T
:%%T U
false%%V [
)%%[ \
}&& 
,&& 
constraints'' 
:'' 
table'' "
=>''# %
{(( 
table)) 
.)) 

PrimaryKey)) $
())$ %
$str))% 7
,))7 8
x))9 :
=>)); =
x))> ?
.))? @
HealthRecordId))@ N
)))N O
;))O P
table** 
.** 

ForeignKey** $
(**$ %
name++ 
:++ 
$str++ K
,++K L
column,, 
:,, 
x,,  !
=>,," $
x,,% &
.,,& '
AppointmentId,,' 4
,,,4 5
principalTable-- &
:--& '
$str--( 6
,--6 7
principalColumn.. '
:..' (
$str..) 8
)..8 9
;..9 :
table// 
.// 

ForeignKey// $
(//$ %
name00 
:00 
$str00 A
,00A B
column11 
:11 
x11  !
=>11" $
x11% &
.11& '
DoctorId11' /
,11/ 0
principalTable22 &
:22& '
$str22( 1
,221 2
principalColumn33 '
:33' (
$str33) 3
)333 4
;334 5
table44 
.44 

ForeignKey44 $
(44$ %
name55 
:55 
$str55 C
,55C D
column66 
:66 
x66  !
=>66" $
x66% &
.66& '
	PatientId66' 0
,660 1
principalTable77 &
:77& '
$str77( 2
,772 3
principalColumn88 '
:88' (
$str88) 4
)884 5
;885 6
}99 
)99 
;99 
migrationBuilder;; 
.;; 

InsertData;; '
(;;' (
table<< 
:<< 
$str<< &
,<<& '
columns== 
:== 
new== 
[== 
]== 
{==  
$str==! 1
,==1 2
$str==3 B
,==B C
$str==D Q
,==Q R
$str==S ^
,==^ _
$str==` j
,==j k
$str==l s
,==s t
$str	==u Ä
,
==Ä Å
$str
==Ç ê
,
==ê ë
$str
==í ù
}
==û ü
,
==ü †
values>> 
:>> 
new>> 
object>> "
[>>" #
,>># $
]>>$ %
{?? 
{@@ 
$num@@ 
,@@ 
$num@@ 
,@@ 
new@@ 
DateTime@@  (
(@@( )
$num@@) -
,@@- .
$num@@/ 0
,@@0 1
$num@@2 4
,@@4 5
$num@@6 7
,@@7 8
$num@@9 :
,@@: ;
$num@@< =
,@@= >
$num@@? @
,@@@ A
DateTimeKind@@B N
.@@N O
Unspecified@@O Z
)@@Z [
,@@[ \
$str@@] m
,@@m n
$num@@o p
,@@p q
$str	@@r î
,
@@î ï
$num
@@ñ ó
,
@@ó ò
$str
@@ô ∏
,
@@∏ π
new
@@∫ Ω
DateTime
@@æ ∆
(
@@∆ «
$num
@@« À
,
@@À Ã
$num
@@Õ Œ
,
@@Œ œ
$num
@@– “
,
@@“ ”
$num
@@‘ ’
,
@@’ ÷
$num
@@◊ ÿ
,
@@ÿ Ÿ
$num
@@⁄ €
,
@@€ ‹
$num
@@› ﬁ
,
@@ﬁ ﬂ
DateTimeKind
@@‡ Ï
.
@@Ï Ì
Unspecified
@@Ì ¯
)
@@¯ ˘
}
@@˙ ˚
,
@@˚ ¸
{AA 
$numAA 
,AA 
$numAA 
,AA 
newAA 
DateTimeAA  (
(AA( )
$numAA) -
,AA- .
$numAA/ 0
,AA0 1
$numAA2 4
,AA4 5
$numAA6 7
,AA7 8
$numAA9 :
,AA: ;
$numAA< =
,AA= >
$numAA? @
,AA@ A
DateTimeKindAAB N
.AAN O
UnspecifiedAAO Z
)AAZ [
,AA[ \
$strAA] o
,AAo p
$numAAq r
,AAr s
$str	AAt ó
,
AAó ò
$num
AAô ö
,
AAö õ
$str
AAú ¡
,
AA¡ ¬
new
AA√ ∆
DateTime
AA« œ
(
AAœ –
$num
AA– ‘
,
AA‘ ’
$num
AA÷ ◊
,
AA◊ ÿ
$num
AAŸ €
,
AA€ ‹
$num
AA› ﬁ
,
AAﬁ ﬂ
$num
AA‡ ·
,
AA· ‚
$num
AA„ ‰
,
AA‰ Â
$num
AAÊ Á
,
AAÁ Ë
DateTimeKind
AAÈ ı
.
AAı ˆ
Unspecified
AAˆ Å
)
AAÅ Ç
}
AAÉ Ñ
,
AAÑ Ö
{BB 
$numBB 
,BB 
$numBB 
,BB 
newBB 
DateTimeBB  (
(BB( )
$numBB) -
,BB- .
$numBB/ 0
,BB0 1
$numBB2 4
,BB4 5
$numBB6 7
,BB7 8
$numBB9 :
,BB: ;
$numBB< =
,BB= >
$numBB? @
,BB@ A
DateTimeKindBBB N
.BBN O
UnspecifiedBBO Z
)BBZ [
,BB[ \
$strBB] u
,BBu v
nullBBw {
,BB{ |
$str	BB} £
,
BB£ §
$num
BB• ¶
,
BB¶ ß
$str
BB® ¿
,
BB¿ ¡
new
BB¬ ≈
DateTime
BB∆ Œ
(
BBŒ œ
$num
BBœ ”
,
BB” ‘
$num
BB’ ÷
,
BB÷ ◊
$num
BBÿ ⁄
,
BB⁄ €
$num
BB‹ ›
,
BB› ﬁ
$num
BBﬂ ‡
,
BB‡ ·
$num
BB‚ „
,
BB„ ‰
$num
BBÂ Ê
,
BBÊ Á
DateTimeKind
BBË Ù
.
BBÙ ı
Unspecified
BBı Ä
)
BBÄ Å
}
BBÇ É
}CC 
)CC 
;CC 
migrationBuilderEE 
.EE 
CreateIndexEE (
(EE( )
nameFF 
:FF 
$strFF 6
,FF6 7
tableGG 
:GG 
$strGG &
,GG& '
columnHH 
:HH 
$strHH '
,HH' (
uniqueII 
:II 
trueII 
)II 
;II 
migrationBuilderKK 
.KK 
CreateIndexKK (
(KK( )
nameLL 
:LL 
$strLL 1
,LL1 2
tableMM 
:MM 
$strMM &
,MM& '
columnNN 
:NN 
$strNN "
)NN" #
;NN# $
migrationBuilderPP 
.PP 
CreateIndexPP (
(PP( )
nameQQ 
:QQ 
$strQQ 2
,QQ2 3
tableRR 
:RR 
$strRR &
,RR& '
columnSS 
:SS 
$strSS #
)SS# $
;SS$ %
migrationBuilderUU 
.UU 
AddForeignKeyUU *
(UU* +
nameVV 
:VV 
$strVV 8
,VV8 9
tableWW 
:WW 
$strWW %
,WW% &
columnXX 
:XX 
$strXX "
,XX" #
principalTableYY 
:YY 
$strYY  )
,YY) *
principalColumnZZ 
:ZZ  
$strZZ! +
)ZZ+ ,
;ZZ, -
migrationBuilder\\ 
.\\ 
AddForeignKey\\ *
(\\* +
name]] 
:]] 
$str]] :
,]]: ;
table^^ 
:^^ 
$str^^ %
,^^% &
column__ 
:__ 
$str__ #
,__# $
principalTable`` 
:`` 
$str``  *
,``* +
principalColumnaa 
:aa  
$straa! ,
)aa, -
;aa- .
}bb 	
	protectedee 
overrideee 
voidee 
Downee  $
(ee$ %
MigrationBuilderee% 5
migrationBuilderee6 F
)eeF G
{ff 	
migrationBuildergg 
.gg 
DropForeignKeygg +
(gg+ ,
namehh 
:hh 
$strhh 8
,hh8 9
tableii 
:ii 
$strii %
)ii% &
;ii& '
migrationBuilderkk 
.kk 
DropForeignKeykk +
(kk+ ,
namell 
:ll 
$strll :
,ll: ;
tablemm 
:mm 
$strmm %
)mm% &
;mm& '
migrationBuilderoo 
.oo 
	DropTableoo &
(oo& '
namepp 
:pp 
$strpp %
)pp% &
;pp& '
migrationBuilderrr 
.rr 
AddForeignKeyrr *
(rr* +
namess 
:ss 
$strss 8
,ss8 9
tablett 
:tt 
$strtt %
,tt% &
columnuu 
:uu 
$struu "
,uu" #
principalTablevv 
:vv 
$strvv  )
,vv) *
principalColumnww 
:ww  
$strww! +
,ww+ ,
onDeletexx 
:xx 
ReferentialActionxx +
.xx+ ,
Cascadexx, 3
)xx3 4
;xx4 5
migrationBuilderzz 
.zz 
AddForeignKeyzz *
(zz* +
name{{ 
:{{ 
$str{{ :
,{{: ;
table|| 
:|| 
$str|| %
,||% &
column}} 
:}} 
$str}} #
,}}# $
principalTable~~ 
:~~ 
$str~~  *
,~~* +
principalColumn 
:  
$str! ,
,, -
onDelete
ÄÄ 
:
ÄÄ 
ReferentialAction
ÄÄ +
.
ÄÄ+ ,
Cascade
ÄÄ, 3
)
ÄÄ3 4
;
ÄÄ4 5
}
ÅÅ 	
}
ÇÇ 
}ÉÉ L
rC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615044709_AddAppointmentTable.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{		 
public 

partial 
class 
AddAppointmentTable ,
:- .
	Migration/ 8
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
$str $
,$ %
columns 
: 
table 
=> !
new" %
{ 
AppointmentId !
=" #
table$ )
.) *
Column* 0
<0 1
int1 4
>4 5
(5 6
type6 :
:: ;
$str< A
,A B
nullableC K
:K L
falseM R
)R S
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
	PatientId 
= 
table  %
.% &
Column& ,
<, -
int- 0
>0 1
(1 2
type2 6
:6 7
$str8 =
,= >
nullable? G
:G H
falseI N
)N O
,O P
DoctorId 
= 
table $
.$ %
Column% +
<+ ,
int, /
>/ 0
(0 1
type1 5
:5 6
$str7 <
,< =
nullable> F
:F G
falseH M
)M N
,N O
ScheduledDate !
=" #
table$ )
.) *
Column* 0
<0 1
DateTime1 9
>9 :
(: ;
type; ?
:? @
$strA L
,L M
nullableN V
:V W
falseX ]
)] ^
,^ _
TimeSlot 
= 
table $
.$ %
Column% +
<+ ,
string, 2
>2 3
(3 4
type4 8
:8 9
$str: H
,H I
	maxLengthJ S
:S T
$numU W
,W X
nullableY a
:a b
falsec h
)h i
,i j
Status 
= 
table "
." #
Column# )
<) *
int* -
>- .
(. /
type/ 3
:3 4
$str5 :
,: ;
nullable< D
:D E
falseF K
)K L
,L M
CancellationReason &
=' (
table) .
.. /
Column/ 5
<5 6
string6 <
>< =
(= >
type> B
:B C
$strD S
,S T
	maxLengthU ^
:^ _
$num` c
,c d
nullablee m
:m n
trueo s
)s t
,t u
CreatedDate 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
} 
, 
constraints 
: 
table "
=># %
{ 
table   
.   

PrimaryKey   $
(  $ %
$str  % 6
,  6 7
x  8 9
=>  : <
x  = >
.  > ?
AppointmentId  ? L
)  L M
;  M N
table!! 
.!! 

ForeignKey!! $
(!!$ %
name"" 
:"" 
$str"" @
,""@ A
column## 
:## 
x##  !
=>##" $
x##% &
.##& '
DoctorId##' /
,##/ 0
principalTable$$ &
:$$& '
$str$$( 1
,$$1 2
principalColumn%% '
:%%' (
$str%%) 3
,%%3 4
onDelete&&  
:&&  !
ReferentialAction&&" 3
.&&3 4
Cascade&&4 ;
)&&; <
;&&< =
table'' 
.'' 

ForeignKey'' $
(''$ %
name(( 
:(( 
$str(( B
,((B C
column)) 
:)) 
x))  !
=>))" $
x))% &
.))& '
	PatientId))' 0
,))0 1
principalTable** &
:**& '
$str**( 2
,**2 3
principalColumn++ '
:++' (
$str++) 4
,++4 5
onDelete,,  
:,,  !
ReferentialAction,," 3
.,,3 4
Cascade,,4 ;
),,; <
;,,< =
}-- 
)-- 
;-- 
migrationBuilder// 
.// 

InsertData// '
(//' (
table00 
:00 
$str00 %
,00% &
columns11 
:11 
new11 
[11 
]11 
{11  
$str11! 0
,110 1
$str112 F
,11F G
$str11H U
,11U V
$str11W a
,11a b
$str11c n
,11n o
$str11p 
,	11 Ä
$str
11Å â
,
11â ä
$str
11ã ï
}
11ñ ó
,
11ó ò
values22 
:22 
new22 
object22 "
[22" #
,22# $
]22$ %
{33 
{44 
$num44 
,44 
null44 
,44 
new44 "
DateTime44# +
(44+ ,
$num44, 0
,440 1
$num442 3
,443 4
$num445 7
,447 8
$num449 :
,44: ;
$num44< =
,44= >
$num44? @
,44@ A
$num44B C
,44C D
DateTimeKind44E Q
.44Q R
Unspecified44R ]
)44] ^
,44^ _
$num44` a
,44a b
$num44c d
,44d e
new44f i
DateTime44j r
(44r s
$num44s w
,44w x
$num44y z
,44z {
$num44| ~
,44~ 
$num
44Ä Å
,
44Å Ç
$num
44É Ñ
,
44Ñ Ö
$num
44Ü á
,
44á à
$num
44â ä
,
44ä ã
DateTimeKind
44å ò
.
44ò ô
Unspecified
44ô §
)
44§ •
,
44• ¶
$num
44ß ®
,
44® ©
$str
44™ ø
}
44¿ ¡
,
44¡ ¬
{55 
$num55 
,55 
null55 
,55 
new55 "
DateTime55# +
(55+ ,
$num55, 0
,550 1
$num552 3
,553 4
$num555 7
,557 8
$num559 :
,55: ;
$num55< =
,55= >
$num55? @
,55@ A
$num55B C
,55C D
DateTimeKind55E Q
.55Q R
Unspecified55R ]
)55] ^
,55^ _
$num55` a
,55a b
$num55c d
,55d e
new55f i
DateTime55j r
(55r s
$num55s w
,55w x
$num55y z
,55z {
$num55| ~
,55~ 
$num
55Ä Å
,
55Å Ç
$num
55É Ñ
,
55Ñ Ö
$num
55Ü á
,
55á à
$num
55â ä
,
55ä ã
DateTimeKind
55å ò
.
55ò ô
Unspecified
55ô §
)
55§ •
,
55• ¶
$num
55ß ®
,
55® ©
$str
55™ ø
}
55¿ ¡
,
55¡ ¬
{66 
$num66 
,66 
$str66 9
,669 :
new66; >
DateTime66? G
(66G H
$num66H L
,66L M
$num66N O
,66O P
$num66Q S
,66S T
$num66U V
,66V W
$num66X Y
,66Y Z
$num66[ \
,66\ ]
$num66^ _
,66_ `
DateTimeKind66a m
.66m n
Unspecified66n y
)66y z
,66z {
$num66| }
,66} ~
$num	66 Ä
,
66Ä Å
new
66Ç Ö
DateTime
66Ü é
(
66é è
$num
66è ì
,
66ì î
$num
66ï ñ
,
66ñ ó
$num
66ò ö
,
66ö õ
$num
66ú ù
,
66ù û
$num
66ü †
,
66† °
$num
66¢ £
,
66£ §
$num
66• ¶
,
66¶ ß
DateTimeKind
66® ¥
.
66¥ µ
Unspecified
66µ ¿
)
66¿ ¡
,
66¡ ¬
$num
66√ ƒ
,
66ƒ ≈
$str
66∆ €
}
66‹ ›
}77 
)77 
;77 
migrationBuilder99 
.99 
CreateIndex99 (
(99( )
name:: 
::: 
$str:: 0
,::0 1
table;; 
:;; 
$str;; %
,;;% &
column<< 
:<< 
$str<< "
)<<" #
;<<# $
migrationBuilder>> 
.>> 
CreateIndex>> (
(>>( )
name?? 
:?? 
$str?? 1
,??1 2
table@@ 
:@@ 
$str@@ %
,@@% &
columnAA 
:AA 
$strAA #
)AA# $
;AA$ %
}BB 	
	protectedEE 
overrideEE 
voidEE 
DownEE  $
(EE$ %
MigrationBuilderEE% 5
migrationBuilderEE6 F
)EEF G
{FF 	
migrationBuilderGG 
.GG 
	DropTableGG &
(GG& '
nameHH 
:HH 
$strHH $
)HH$ %
;HH% &
}II 	
}JJ 
}KK ﬁ 
mC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615044341_SeedDoctorData.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{		 
public 

partial 
class 
SeedDoctorData '
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
$str@ M
,M N
$strO [
,[ \
$str] g
,g h
$stri y
,y z
$str	{ é
}
è ê
,
ê ë
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
$num 
, 
new !
DateTime" *
(* +
$num+ /
,/ 0
$num1 2
,2 3
$num4 6
,6 7
$num8 9
,9 :
$num; <
,< =
$num> ?
,? @
$numA B
,B C
DateTimeKindD P
.P Q
UnspecifiedQ \
)\ ]
,] ^
$str_ k
,k l
truem q
,q r
$nums t
,t u
$numv x
}y z
,z {
{ 
$num 
, 
$num 
, 
new "
DateTime# +
(+ ,
$num, 0
,0 1
$num2 3
,3 4
$num5 7
,7 8
$num9 :
,: ;
$num< =
,= >
$num? @
,@ A
$numB C
,C D
DateTimeKindE Q
.Q R
UnspecifiedR ]
)] ^
,^ _
$str` l
,l m
truen r
,r s
$numt u
,u v
$numw y
}z {
,{ |
{ 
$num 
, 
$num 
, 
new !
DateTime" *
(* +
$num+ /
,/ 0
$num1 2
,2 3
$num4 6
,6 7
$num8 9
,9 :
$num; <
,< =
$num> ?
,? @
$numA B
,B C
DateTimeKindD P
.P Q
UnspecifiedQ \
)\ ]
,] ^
$str_ k
,k l
truem q
,q r
$nums t
,t u
$numv w
}x y
} 
) 
; 
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

DeleteData '
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
)!! 
;!! 
migrationBuilder## 
.## 

DeleteData## '
(##' (
table$$ 
:$$ 
$str$$  
,$$  !
	keyColumn%% 
:%% 
$str%% %
,%%% &
keyValue&& 
:&& 
$num&& 
)&& 
;&& 
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
},, 	
}-- 
}.. Ÿ
mC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615044005_AddDoctorTable.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public		 

partial		 
class		 
AddDoctorTable		 '
:		( )
	Migration		* 3
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
,K L
	maxLengthM V
:V W
$numX [
,[ \
nullable] e
:e f
falseg l
)l m
,m n
Specialisation "
=# $
table% *
.* +
Column+ 1
<1 2
int2 5
>5 6
(6 7
type7 ;
:; <
$str= B
,B C
nullableD L
:L M
falseN S
)S T
,T U
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
<2 3
int3 6
>6 7
(7 8
type8 <
:< =
$str> C
,C D
nullableE M
:M N
falseO T
)T U
,U V
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
,O P
CreatedDate 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 1
,1 2
x3 4
=>5 7
x8 9
.9 :
DoctorId: B
)B C
;C D
} 
) 
; 
} 	
	protected"" 
override"" 
void"" 
Down""  $
(""$ %
MigrationBuilder""% 5
migrationBuilder""6 F
)""F G
{## 	
migrationBuilder$$ 
.$$ 
	DropTable$$ &
($$& '
name%% 
:%% 
$str%% 
)%%  
;%%  !
}&& 	
}'' 
}(( ÷*
nC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615043705_SeedPatientData.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{		 
public 

partial 
class 
SeedPatientData (
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
$str !
,! "
columns 
: 
new 
[ 
] 
{  
$str! ,
,, -
$str. ;
,; <
$str= J
,J K
$strL S
,S T
$strU ]
,] ^
$str_ l
,l m
$strn {
,{ |
$str	} ä
}
ã å
,
å ç
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
, 
new 
DateTime %
(% &
$num& *
,* +
$num, -
,- .
$num/ 1
,1 2
$num3 4
,4 5
$num6 7
,7 8
$num9 :
,: ;
$num< =
,= >
DateTimeKind? K
.K L
UnspecifiedL W
)W X
,X Y
newZ ]
DateTime^ f
(f g
$numg k
,k l
$numm n
,n o
$nump r
,r s
$numt u
,u v
$numw x
,x y
$numz {
,{ |
$num} ~
,~ 
DateTimeKind
Ä å
.
å ç
Unspecified
ç ò
)
ò ô
,
ô ö
$str
õ ≥
,
≥ ¥
$num
µ ∂
,
∂ ∑
$str
∏ ¡
,
¡ ¬
$str
√ œ
,
œ –
$str
— ›
}
ﬁ ﬂ
,
ﬂ ‡
{ 
$num 
, 
new 
DateTime %
(% &
$num& *
,* +
$num, -
,- .
$num/ 1
,1 2
$num3 4
,4 5
$num6 7
,7 8
$num9 :
,: ;
$num< =
,= >
DateTimeKind? K
.K L
UnspecifiedL W
)W X
,X Y
newZ ]
DateTime^ f
(f g
$numg k
,k l
$numm n
,n o
$nump r
,r s
$numt u
,u v
$numw x
,x y
$numz {
,{ |
$num} ~
,~ 
DateTimeKind
Ä å
.
å ç
Unspecified
ç ò
)
ò ô
,
ô ö
$str
õ ¥
,
¥ µ
$num
∂ ∑
,
∑ ∏
$str
π ¬
,
¬ √
$str
ƒ —
,
— “
$str
” ﬂ
}
‡ ·
,
· ‚
{ 
$num 
, 
new 
DateTime %
(% &
$num& *
,* +
$num, -
,- .
$num/ 1
,1 2
$num3 4
,4 5
$num6 7
,7 8
$num9 :
,: ;
$num< =
,= >
DateTimeKind? K
.K L
UnspecifiedL W
)W X
,X Y
newZ ]
DateTime^ f
(f g
$numg k
,k l
$numm o
,o p
$numq r
,r s
$numt u
,u v
$numw x
,x y
$numz {
,{ |
$num} ~
,~ 
DateTimeKind
Ä å
.
å ç
Unspecified
ç ò
)
ò ô
,
ô ö
$str
õ ≤
,
≤ ≥
$num
¥ µ
,
µ ∂
null
∑ ª
,
ª º
$str
Ω »
,
» …
$str
  ÷
}
◊ ÿ
} 
) 
; 
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

DeleteData '
(' (
table 
: 
$str !
,! "
	keyColumn   
:   
$str   &
,  & '
keyValue!! 
:!! 
$num!! 
)!! 
;!! 
migrationBuilder## 
.## 

DeleteData## '
(##' (
table$$ 
:$$ 
$str$$ !
,$$! "
	keyColumn%% 
:%% 
$str%% &
,%%& '
keyValue&& 
:&& 
$num&& 
)&& 
;&& 
migrationBuilder(( 
.(( 

DeleteData(( '
(((' (
table)) 
:)) 
$str)) !
,))! "
	keyColumn** 
:** 
$str** &
,**& '
keyValue++ 
:++ 
$num++ 
)++ 
;++ 
},, 	
}-- 
}.. ë!
rC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Migrations\20260615043550_initialPatientTable.cs
	namespace 	
HealthCareApp
 
. 

Migrations "
{ 
public		 

partial		 
class		 
initialPatientTable		 ,
:		- .
	Migration		/ 8
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
$str  
,  !
columns 
: 
table 
=> !
new" %
{ 
	PatientId 
= 
table  %
.% &
Column& ,
<, -
int- 0
>0 1
(1 2
type2 6
:6 7
$str8 =
,= >
nullable? G
:G H
falseI N
)N O
. 

Annotation #
(# $
$str$ 8
,8 9
$str: @
)@ A
,A B
PatientName 
=  !
table" '
.' (
Column( .
<. /
string/ 5
>5 6
(6 7
type7 ;
:; <
$str= L
,L M
	maxLengthN W
:W X
$numY \
,\ ]
nullable^ f
:f g
falseh m
)m n
,n o
DateOfBirth 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
,\ ]
Gender 
= 
table "
." #
Column# )
<) *
int* -
>- .
(. /
type/ 3
:3 4
$str5 :
,: ;
nullable< D
:D E
falseF K
)K L
,L M
Email 
= 
table !
.! "
Column" (
<( )
string) /
>/ 0
(0 1
type1 5
:5 6
$str7 F
,F G
nullableH P
:P Q
falseR W
)W X
,X Y
PhoneNumber 
=  !
table" '
.' (
Column( .
<. /
string/ 5
>5 6
(6 7
type7 ;
:; <
$str= L
,L M
nullableN V
:V W
falseX ]
)] ^
,^ _
InsuranceID 
=  !
table" '
.' (
Column( .
<. /
string/ 5
>5 6
(6 7
type7 ;
:; <
$str= L
,L M
nullableN V
:V W
trueX \
)\ ]
,] ^
CreatedDate 
=  !
table" '
.' (
Column( .
<. /
DateTime/ 7
>7 8
(8 9
type9 =
:= >
$str? J
,J K
nullableL T
:T U
falseV [
)[ \
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
$str% 2
,2 3
x4 5
=>6 8
x9 :
.: ;
	PatientId; D
)D E
;E F
} 
) 
; 
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
.%% 
	DropTable%% &
(%%& '
name&& 
:&& 
$str&&  
)&&  !
;&&! "
}'' 	
}(( 
})) ¯$
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Middleware\GlobalExceptionHandler.cs
	namespace 	
HealthCareApp
 
. 

Middleware "
{		 
public

 

class

 "
GlobalExceptionHandler

 '
:

( )
IExceptionHandler

* ;
{ 
private 
readonly 
ILogger  
<  !"
GlobalExceptionHandler! 7
>7 8
_logger9 @
;@ A
public "
GlobalExceptionHandler %
(% &
ILogger& -
<- ."
GlobalExceptionHandler. D
>D E
loggerF L
)L M
{ 	
_logger 
= 
logger 
; 
} 	
public 
async 
	ValueTask 
< 
bool #
># $
TryHandleAsync% 3
(3 4
HttpContext 
httpContext #
,# $
	Exception 
	exception 
,  
CancellationToken 
cancellationToken /
)/ 0
{ 	
_logger 
. 
LogError 
( 
	exception 
, 
$str 9
,9 :
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
{ #
EntityNotFoundException '
ex( *
=>+ -
(   
StatusCodes    
.    !
Status404NotFound  ! 2
,  2 3
ex  4 6
.  6 7
Message  7 >
)  > ?
,  ? @
ConflictException"" !
ex""" $
=>""% '
(## 
StatusCodes##  
.##  !
Status409Conflict##! 2
,##2 3
ex##4 6
.##6 7
Message##7 >
)##> ?
,##? @$
AppointmentRuleException%% (
ex%%) +
=>%%, .
(&& 
StatusCodes&&  
.&&  !
Status400BadRequest&&! 4
,&&4 5
ex&&6 8
.&&8 9
Message&&9 @
)&&@ A
,&&A B%
HealthRecordRuleException(( )
ex((* ,
=>((- /
()) 
StatusCodes))  
.))  !
Status400BadRequest))! 4
,))4 5
ex))6 8
.))8 9
Message))9 @
)))@ A
,))A B$
ForbiddenAccessException** (
ex**) +
=>**, .
(++ 
StatusCodes++  
.++  !
Status403Forbidden++! 3
,++3 4
ex++5 7
.++7 8
Message++8 ?
)++? @
,++@ A!
BusinessRuleException,, %
ex,,& (
=>,,) +
(-- 
StatusCodes--  
.--  !
Status400BadRequest--! 4
,--4 5
ex--6 8
.--8 9
Message--9 @
)--@ A
,--A B"
HealthcareAppException// &
ex//' )
=>//* ,
(00 
StatusCodes00  
.00  !
Status400BadRequest00! 4
,004 5
ex006 8
.008 9
Message009 @
)00@ A
,00A B
_22 
=>22 
(33 
StatusCodes33  
.33  !(
Status500InternalServerError33! =
,33= >
$str33? n
)33n o
}44 
;44 
httpContext66 
.66 
Response66  
.66  !

StatusCode66! +
=66, -

statusCode66. 8
;668 9
httpContext77 
.77 
Response77  
.77  !
ContentType77! ,
=77- .
$str77/ A
;77A B
var99 
response99 
=99 
new99 
ErrorResponse99 ,
{:: 

StatusCode;; 
=;; 

statusCode;; '
,;;' (
Message<< 
=<< 
message<< !
,<<! "
	TimeStamp== 
=== 
DateTime== $
.==$ %
UtcNow==% +
,==+ ,
Path>> 
=>> 
httpContext>> "
.>>" #
Request>># *
.>>* +
Path>>+ /
}?? 
;?? 
awaitAA 
httpContextAA 
.AA 
ResponseAA &
.AA& '
WriteAsJsonAsyncAA' 7
(AA7 8
responseAA8 @
,AA@ A
cancellationTokenAAB S
)AAS T
;AAT U
returnCC 
trueCC 
;CC 
}DD 	
}EE 
}FF ËU
[C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Mapping\MappingProfile.cs
	namespace

 	
HealthCareApp


 
.

 
Mapping

 
{ 
public 

class 
MappingProfile 
:  !
Profile" )
{ 
private 
const 
string 

DateFormat '
=( )
$str* 6
;6 7
public 
MappingProfile 
( 
) 
{ 	
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
.  !
FullName! )
,) *
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
PatientName2 =
)= >
) 
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
InsuranceId! ,
,, -
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
InsuranceID2 =
)= >
) 
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
DateOfBirth! ,
,, -
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
DateOfBirth2 =
.= >
ToString> F
(F G

DateFormatG Q
)Q R
)R S
) 
.   
	ForMember   
(   
dest!! 
=>!! 
dest!!  
.!!  !
CreatedDate!!! ,
,!!, -
opt"" 
=>"" 
opt"" 
."" 
MapFrom"" &
(""& '
src""' *
=>""+ -
src"". 1
.""1 2
DateOfBirth""2 =
.""= >
ToString""> F
(""F G

DateFormat""G Q
)""Q R
)""R S
)## 
;## 
	CreateMap%% 
<%% 
CreatePatientDto%% &
,%%& '
Patient%%( /
>%%/ 0
(%%0 1
)%%1 2
.&& 
	ForMember&& 
(&& 
dest'' 
=>'' 
dest''  
.''  !
PatientName''! ,
,'', -
opt(( 
=>(( 
opt(( 
.(( 
MapFrom(( &
(((& '
src((' *
=>((+ -
src((. 1
.((1 2
FullName((2 :
)((: ;
))) 
.** 
	ForMember** 
(** 
dest++ 
=>++ 
dest++  
.++  !
InsuranceID++! ,
,++, -
opt,, 
=>,, 
opt,, 
.,, 
MapFrom,, &
(,,& '
src,,' *
=>,,+ -
src,,. 1
.,,1 2
InsuranceId,,2 =
),,= >
)-- 
;-- 
	CreateMap// 
<// 
UpdatePatientDto// &
,//& '
Patient//( /
>/// 0
(//0 1
)//1 2
.00 
	ForMember00 
(00 
dest11 
=>11 
dest11  
.11  !
PatientName11! ,
,11, -
opt22 
=>22 
opt22 
.22 
MapFrom22 &
(22& '
src22' *
=>22+ -
src22. 1
.221 2
FullName222 :
)22: ;
)33 
.44 
	ForMember44 
(44 
dest55 
=>55 
dest55  
.55  !
InsuranceID55! ,
,55, -
opt66 
=>66 
opt66 
.66 
MapFrom66 &
(66& '
src66' *
=>66+ -
src66. 1
.661 2
InsuranceId662 =
)66= >
)77 
;77 
	CreateMap99 
<99 
PatientRegisterDto99 (
,99( )
Patient99* 1
>991 2
(992 3
)993 4
.:: 
	ForMember:: 
(:: 
dest;; 
=>;; 
dest;;  
.;;  !
PatientName;;! ,
,;;, -
opt<< 
=><< 
opt<< 
.<< 
MapFrom<< &
(<<& '
src<<' *
=><<+ -
src<<. 1
.<<1 2
FullName<<2 :
)<<: ;
)== 
.>> 
	ForMember>> 
(>> 
dest?? 
=>?? 
dest??  
.??  !
InsuranceID??! ,
,??, -
opt@@ 
=>@@ 
opt@@ 
.@@ 
MapFrom@@ &
(@@& '
src@@' *
=>@@+ -
src@@. 1
.@@1 2
InsuranceId@@2 =
)@@= >
)AA 
;AA 
	CreateMapDD 
<DD 
DoctorDD 
,DD 
	DoctorDtoDD '
>DD' (
(DD( )
)DD) *
.EE 
	ForMemberEE 
(EE 
destFF 
=>FF 
destFF  
.FF  !
FullNameFF! )
,FF) *
optGG 
=>GG 
optGG 
.GG 
MapFromGG &
(GG& '
srcGG' *
=>GG+ -
srcGG. 1
.GG1 2

DoctorNameGG2 <
)GG< =
)HH 
;HH 
	CreateMapJJ 
<JJ 
CreateDoctorDtoJJ %
,JJ% &
DoctorJJ' -
>JJ- .
(JJ. /
)JJ/ 0
.KK 
	ForMemberKK 
(KK 
destLL 
=>LL 
destLL  
.LL  !

DoctorNameLL! +
,LL+ ,
optMM 
=>MM 
optMM 
.MM 
MapFromMM &
(MM& '
srcMM' *
=>MM+ -
srcMM. 1
.MM1 2
FullNameMM2 :
)MM: ;
)NN 
;NN 
	CreateMapPP 
<PP 
UpdateDoctorDtoPP %
,PP% &
DoctorPP' -
>PP- .
(PP. /
)PP/ 0
.QQ 
	ForMemberQQ 
(QQ 
destRR 
=>RR 
destRR  
.RR  !

DoctorNameRR! +
,RR+ ,
optSS 
=>SS 
optSS 
.SS 
MapFromSS &
(SS& '
srcSS' *
=>SS+ -
srcSS. 1
.SS1 2
FullNameSS2 :
)SS: ;
)TT 
;TT 
	CreateMapWW 
<WW 
AppointmentWW !
,WW! "
AppointmentDtoWW# 1
>WW1 2
(WW2 3
)WW3 4
.XX 
	ForMemberXX 
(XX 
destYY 
=>YY 
destYY  
.YY  !
PatientNameYY! ,
,YY, -
optZZ 
=>ZZ 
optZZ 
.ZZ 
MapFromZZ &
(ZZ& '
srcZZ' *
=>ZZ+ -
srcZZ. 1
.ZZ1 2
PatientZZ2 9
!=ZZ: <
nullZZ= A
?ZZB C
srcZZD G
.ZZG H
PatientZZH O
.ZZO P
PatientNameZZP [
:ZZ\ ]
nullZZ^ b
)ZZb c
)[[ 
.\\ 
	ForMember\\ 
(\\ 
dest]] 
=>]] 
dest]]  
.]]  !

DoctorName]]! +
,]]+ ,
opt^^ 
=>^^ 
opt^^ 
.^^ 
MapFrom^^ &
(^^& '
src^^' *
=>^^+ -
src^^. 1
.^^1 2
Doctor^^2 8
!=^^9 ;
null^^< @
?^^A B
src^^C F
.^^F G
Doctor^^G M
.^^M N

DoctorName^^N X
:^^Y Z
null^^[ _
)^^_ `
)__ 
.`` 
	ForMember`` 
(`` 
destaa 
=>aa 
destaa  
.aa  !
ScheduledDateaa! .
,aa. /
optbb 
=>bb 
optbb 
.bb 
MapFrombb &
(bb& '
srcbb' *
=>bb+ -
srcbb. 1
.bb1 2
ScheduledDatebb2 ?
.bb? @
ToStringbb@ H
(bbH I

DateFormatbbI S
)bbS T
)bbT U
)cc 
;cc 
	CreateMapee 
<ee 
BookAppointmentDtoee (
,ee( )
Appointmentee* 5
>ee5 6
(ee6 7
)ee7 8
;ee8 9
	CreateMapgg 
<gg  
UpdateAppointmentDtogg *
,gg* +
Appointmentgg, 7
>gg7 8
(gg8 9
)gg9 :
;gg: ;
	CreateMapjj 
<jj 
HealthRecordjj "
,jj" #
HealthRecordDtojj$ 3
>jj3 4
(jj4 5
)jj5 6
.kk 
	ForMemberkk 
(kk 
destll 
=>ll 
destll  
.ll  !
PatientNamell! ,
,ll, -
optmm 
=>mm 
optmm 
.mm 
MapFrommm &
(mm& '
srcmm' *
=>mm+ -
srcmm. 1
.mm1 2
Patientmm2 9
!=mm: <
nullmm= A
?mmB C
srcmmD G
.mmG H
PatientmmH O
.mmO P
PatientNamemmP [
:mm\ ]
nullmm^ b
)mmb c
)nn 
.oo 
	ForMemberoo 
(oo 
destpp 
=>pp 
destpp  
.pp  !

DoctorNamepp! +
,pp+ ,
optqq 
=>qq 
optqq 
.qq 
MapFromqq &
(qq& '
srcqq' *
=>qq+ -
srcqq. 1
.qq1 2
Doctorqq2 8
!=qq9 ;
nullqq< @
?qqA B
srcqqC F
.qqF G
DoctorqqG M
.qqM N

DoctorNameqqN X
:qqY Z
nullqq[ _
)qq_ `
)rr 
.ss 
	ForMemberss 
(ss 
desttt 
=>tt 
desttt  
.tt  !
	VisitDatett! *
,tt* +
optuu 
=>uu 
optuu 
.uu 
MapFromuu &
(uu& '
srcuu' *
=>uu+ -
srcuu. 1
.uu1 2
	VisitDateuu2 ;
.uu; <
ToStringuu< D
(uuD E

DateFormatuuE O
)uuO P
)uuP Q
)vv 
;vv 
	CreateMapxx 
<xx 
AddHealthRecordDtoxx (
,xx( )
HealthRecordxx* 6
>xx6 7
(xx7 8
)xx8 9
;xx9 :
	CreateMapzz 
<zz !
UpdateHealthRecordDtozz +
,zz+ ,
HealthRecordzz- 9
>zz9 :
(zz: ;
)zz; <
;zz< =
}{{ 	
}|| 
}}} Ó
iC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\HealthRecordRuleException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
public 

class %
HealthRecordRuleException *
:+ ,!
BusinessRuleException- B
{ 
public %
HealthRecordRuleException (
(( )
string) /
message0 7
)7 8
: 
base 
( 
message 
) 
{ 	
}		 	
}

 
} ”
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\HealthCareAppException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
public 

abstract 
class "
HealthcareAppException 0
:1 2
	Exception3 <
{ 
	protected "
HealthcareAppException (
(( )
string) /
message0 7
)7 8
: 
base 
( 
message 
) 
{ 	
} 	
	protected

 "
HealthcareAppException

 (
(

( )
string

) /
message

0 7
,

7 8
	Exception

9 B
innerException

C Q
)

Q R
: 
base 
( 
message 
, 
innerException *
)* +
{ 	
} 	
} 
} Ï
hC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\ForbiddenAccessException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
public 

class $
ForbiddenAccessException )
:* +"
HealthcareAppException, B
{ 
public $
ForbiddenAccessException '
(' (
string( .
message/ 6
)6 7
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
 ≥

gC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\EntityNotFoundException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
public 

class #
EntityNotFoundException (
:) *"
HealthcareAppException+ A
{ 
public 
string 

EntityName  
{! "
get# &
;& '
private( /
set0 3
;3 4
}5 6
public 
int 
EntityId 
{ 
get !
;! "
private# *
set+ .
;. /
}0 1
public		 #
EntityNotFoundException		 &
(		& '
string		' -

entityName		. 8
,		8 9
int		: =
entityId		> F
)		F G
:

 
base

 
(

 
$"

 
{

 

entityName

  
}

  !
$str

! *
{

* +
entityId

+ 3
}

3 4
$str

4 C
"

C D
)

D E
{ 	

EntityName 
= 

entityName #
;# $
EntityId 
= 
entityId 
;  
} 	
} 
} ˜
aC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\ConflictException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
[ 
Serializable 
] 
public 

class 
ConflictException "
:# $
	Exception% .
{ 
public 
ConflictException  
(  !
)! "
{ 	
} 	
public

 
ConflictException

  
(

  !
string

! '
?

' (
message

) 0
)

0 1
:

2 3
base

4 8
(

8 9
message

9 @
)

@ A
{ 	
} 	
public 
ConflictException  
(  !
string! '
?' (
message) 0
,0 1
	Exception2 ;
?; <
innerException= K
)K L
:M N
baseO S
(S T
messageT [
,[ \
innerException] k
)k l
{ 	
} 	
} 
} „
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\BusinessRuleException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
public 

class !
BusinessRuleException &
:' ("
HealthcareAppException) ?
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
 Î
hC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Exceptions\AppointmentRuleException.cs
	namespace 	
HealthCareApp
 
. 

Exceptions "
{ 
public 

class $
AppointmentRuleException )
:* +!
BusinessRuleException, A
{ 
public $
AppointmentRuleException '
(' (
string( .
message/ 6
)6 7
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
 ö
TC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Data\RoleSeeder.cs
	namespace 	
HealthCareApp
 
. 
Data 
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
Task  
SeedRoleAsync! .
(. /
RoleManager/ :
<: ;
IdentityRole; G
>G H
roleManagerI T
)T U
{ 	
string		 
[		 
]		 
roles		 
=		 
new		  
[		  !
]		! "
{		# $
$str		% ,
,		, -
$str		. 6
,		6 7
$str		8 A
}		B C
;		C D
foreach

 
(

 
var

 
role

 
in

  
roles

! &
)

& '
{ 
if 
( 
! 
await 
roleManager &
.& '
RoleExistsAsync' 6
(6 7
role7 ;
); <
)< =
{ 
await 
roleManager %
.% &
CreateAsync& 1
(1 2
new2 5
IdentityRole6 B
(B C
roleC G
)G H
)H I
;I J
} 
} 
} 	
} 
} ¸ú
]C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Data\HealthAxisDbContext.cs
	namespace 	
HealthCareApp
 
. 
Data 
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
Patients &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
DbSet 
< 
Doctor 
> 
Doctors $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
DbSet 
< 
Appointment  
>  !
Appointments" .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
DbSet 
< 
HealthRecord !
>! "
HealthRecords# 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
private 
static 
DateTime 
UtcDate  '
(' (
int( +
year, 0
,0 1
int2 5
month6 ;
,; <
int= @
dayA D
)D E
{ 	
return 
new 
DateTime 
(  
year  $
,$ %
month& +
,+ ,
day- 0
,0 1
$num2 3
,3 4
$num5 6
,6 7
$num8 9
,9 :
DateTimeKind; G
.G H
UtcH K
)K L
;L M
} 	
	protected 
override 
void 
OnModelCreating  /
(/ 0
ModelBuilder0 <
Builder= D
)D E
{ 	
base 
. 
OnModelCreating  
(  !
Builder! (
)( )
;) *
Builder"" 
."" 
Entity"" 
<"" 
Patient"" "
>""" #
(""# $
)""$ %
.## 
HasOne## 
(## 
p## 
=>## 
p## 
.## 
IdentityUser## +
)##+ ,
.$$ 
WithOne$$ 
($$ 
)$$ 
.%% 
HasForeignKey%% 
<%% 
Patient%% &
>%%& '
(%%' (
p%%( )
=>%%* ,
p%%- .
.%%. /
IdentityUserId%%/ =
)%%= >
.&& 
OnDelete&& 
(&& 
DeleteBehavior&& (
.&&( )
NoAction&&) 1
)&&1 2
;&&2 3
Builder)) 
.)) 
Entity)) 
<)) 
Appointment)) &
>))& '
())' (
)))( )
.** 
HasOne** 
(** 
a** 
=>** 
a** 
.** 
Patient** &
)**& '
.++ 
WithMany++ 
(++ 
p++ 
=>++ 
p++  
.++  !
Appointments++! -
)++- .
.,, 
HasForeignKey,, 
(,, 
a,,  
=>,,! #
a,,$ %
.,,% &
	PatientId,,& /
),,/ 0
.-- 
OnDelete-- 
(-- 
DeleteBehavior-- (
.--( )
NoAction--) 1
)--1 2
;--2 3
Builder// 
.// 
Entity// 
<// 
Appointment// &
>//& '
(//' (
)//( )
.00 
HasOne00 
(00 
a00 
=>00 
a00 
.00 
Doctor00 %
)00% &
.11 
WithMany11 
(11 
d11 
=>11 
d11  
.11  !
Appointments11! -
)11- .
.22 
HasForeignKey22 
(22 
a22  
=>22! #
a22$ %
.22% &
DoctorId22& .
)22. /
.33 
OnDelete33 
(33 
DeleteBehavior33 (
.33( )
NoAction33) 1
)331 2
;332 3
Builder66 
.66 
Entity66 
<66 
HealthRecord66 '
>66' (
(66( )
)66) *
.77 
HasOne77 
(77 
hr77 
=>77 
hr77  
.77  !
Patient77! (
)77( )
.88 
WithMany88 
(88 
p88 
=>88 
p88  
.88  !
HealthRecords88! .
)88. /
.99 
HasForeignKey99 
(99 
hr99 !
=>99" $
hr99% '
.99' (
	PatientId99( 1
)991 2
.:: 
OnDelete:: 
(:: 
DeleteBehavior:: (
.::( )
NoAction::) 1
)::1 2
;::2 3
Builder<< 
.<< 
Entity<< 
<<< 
HealthRecord<< '
><<' (
(<<( )
)<<) *
.== 
HasOne== 
(== 
hr== 
=>== 
hr==  
.==  !
Doctor==! '
)==' (
.>> 
WithMany>> 
(>> 
d>> 
=>>> 
d>>  
.>>  !
HealthRecords>>! .
)>>. /
.?? 
HasForeignKey?? 
(?? 
hr?? !
=>??" $
hr??% '
.??' (
DoctorId??( 0
)??0 1
.@@ 
OnDelete@@ 
(@@ 
DeleteBehavior@@ (
.@@( )
NoAction@@) 1
)@@1 2
;@@2 3
BuilderBB 
.BB 
EntityBB 
<BB 
HealthRecordBB '
>BB' (
(BB( )
)BB) *
.CC 
HasOneCC 
(CC 
hrCC 
=>CC 
hrCC  
.CC  !
AppointmentCC! ,
)CC, -
.DD 
WithOneDD 
(DD 
aDD 
=>DD 
aDD 
.DD  
HealthRecordDD  ,
)DD, -
.EE 
HasForeignKeyEE 
<EE 
HealthRecordEE +
>EE+ ,
(EE, -
hrEE- /
=>EE0 2
hrEE3 5
.EE5 6
AppointmentIdEE6 C
)EEC D
.FF 
OnDeleteFF 
(FF 
DeleteBehaviorFF (
.FF( )
NoActionFF) 1
)FF1 2
;FF2 3
BuilderII 
.II 
EntityII 
<II 
PatientII "
>II" #
(II# $
)II$ %
.II% &
HasDataII& -
(II- .
newJJ 
PatientJJ 
{KK 
	PatientIdLL 
=LL 
$numLL  !
,LL! "
PatientNameMM 
=MM  !
$strMM" .
,MM. /
DateOfBirthNN 
=NN  !
UtcDateNN" )
(NN) *
$numNN* .
,NN. /
$numNN0 1
,NN1 2
$numNN3 5
)NN5 6
,NN6 7
GenderOO 
=OO 

GenderTypeOO '
.OO' (
MaleOO( ,
,OO, -
EmailPP 
=PP 
$strPP 4
,PP4 5
PhoneNumberQQ 
=QQ  !
$strQQ" .
,QQ. /
InsuranceIDRR 
=RR  !
$strRR" +
,RR+ ,
IdentityUserIdSS "
=SS# $
nullSS% )
,SS) *
CreatedDateTT 
=TT  !
UtcDateTT" )
(TT) *
$numTT* .
,TT. /
$numTT0 1
,TT1 2
$numTT3 5
)TT5 6
}UU 
,UU 
newVV 
PatientVV 
{WW 
	PatientIdXX 
=XX 
$numXX  !
,XX! "
PatientNameYY 
=YY  !
$strYY" /
,YY/ 0
DateOfBirthZZ 
=ZZ  !
UtcDateZZ" )
(ZZ) *
$numZZ* .
,ZZ. /
$numZZ0 1
,ZZ1 2
$numZZ3 5
)ZZ5 6
,ZZ6 7
Gender[[ 
=[[ 

GenderType[[ '
.[[' (
Female[[( .
,[[. /
Email\\ 
=\\ 
$str\\ 5
,\\5 6
PhoneNumber]] 
=]]  !
$str]]" .
,]]. /
InsuranceID^^ 
=^^  !
$str^^" +
,^^+ ,
IdentityUserId__ "
=__# $
null__% )
,__) *
CreatedDate`` 
=``  !
UtcDate``" )
(``) *
$num``* .
,``. /
$num``0 1
,``1 2
$num``3 5
)``5 6
}aa 
,aa 
newbb 
Patientbb 
{cc 
	PatientIddd 
=dd 
$numdd  !
,dd! "
PatientNameee 
=ee  !
$stree" -
,ee- .
DateOfBirthff 
=ff  !
UtcDateff" )
(ff) *
$numff* .
,ff. /
$numff0 2
,ff2 3
$numff4 5
)ff5 6
,ff6 7
Gendergg 
=gg 

GenderTypegg '
.gg' (
Othergg( -
,gg- .
Emailhh 
=hh 
$strhh 3
,hh3 4
PhoneNumberii 
=ii  !
$strii" .
,ii. /
InsuranceIDjj 
=jj  !
nulljj" &
,jj& '
IdentityUserIdkk "
=kk# $
nullkk% )
,kk) *
CreatedDatell 
=ll  !
UtcDatell" )
(ll) *
$numll* .
,ll. /
$numll0 1
,ll1 2
$numll3 5
)ll5 6
}mm 
)nn 
;nn 
Builderqq 
.qq 
Entityqq 
<qq 
Doctorqq !
>qq! "
(qq" #
)qq# $
.qq$ %
HasDataqq% ,
(qq, -
newrr 
Doctorrr 
{ss 
DoctorIdtt 
=tt 
$numtt  
,tt  !

DoctorNameuu 
=uu  
$struu! -
,uu- .
Emailvv 
=vv 
$strvv 4
,vv4 5
Specialisationww "
=ww# $
SpecialisationTypeww% 7
.ww7 8
GeneralPractitionerww8 K
,wwK L
YearsOfExperiencexx %
=xx& '
$numxx( *
,xx* +
ConsultationFeeyy #
=yy$ %
$numyy& *
,yy* +
IsActivezz 
=zz 
truezz #
,zz# $
IdentityUserId{{ "
={{# $
null{{% )
,{{) *
CreatedDate|| 
=||  !
UtcDate||" )
(||) *
$num||* .
,||. /
$num||0 1
,||1 2
$num||3 5
)||5 6
}}} 
,}} 
new~~ 
Doctor~~ 
{ 
DoctorId
ÄÄ 
=
ÄÄ 
$num
ÄÄ  
,
ÄÄ  !

DoctorName
ÅÅ 
=
ÅÅ  
$str
ÅÅ! -
,
ÅÅ- .
Email
ÇÇ 
=
ÇÇ 
$str
ÇÇ 4
,
ÇÇ4 5
Specialisation
ÉÉ "
=
ÉÉ# $ 
SpecialisationType
ÉÉ% 7
.
ÉÉ7 8
Cardiologist
ÉÉ8 D
,
ÉÉD E
YearsOfExperience
ÑÑ %
=
ÑÑ& '
$num
ÑÑ( *
,
ÑÑ* +
ConsultationFee
ÖÖ #
=
ÖÖ$ %
$num
ÖÖ& *
,
ÖÖ* +
IsActive
ÜÜ 
=
ÜÜ 
true
ÜÜ #
,
ÜÜ# $
IdentityUserId
áá "
=
áá# $
null
áá% )
,
áá) *
CreatedDate
àà 
=
àà  !
UtcDate
àà" )
(
àà) *
$num
àà* .
,
àà. /
$num
àà0 1
,
àà1 2
$num
àà3 5
)
àà5 6
}
ââ 
,
ââ 
new
ää 
Doctor
ää 
{
ãã 
DoctorId
åå 
=
åå 
$num
åå  
,
åå  !

DoctorName
çç 
=
çç  
$str
çç! -
,
çç- .
Email
éé 
=
éé 
$str
éé 4
,
éé4 5
Specialisation
èè "
=
èè# $ 
SpecialisationType
èè% 7
.
èè7 8
Dermatologist
èè8 E
,
èèE F
YearsOfExperience
êê %
=
êê& '
$num
êê( )
,
êê) *
ConsultationFee
ëë #
=
ëë$ %
$num
ëë& )
,
ëë) *
IsActive
íí 
=
íí 
true
íí #
,
íí# $
IdentityUserId
ìì "
=
ìì# $
null
ìì% )
,
ìì) *
CreatedDate
îî 
=
îî  !
UtcDate
îî" )
(
îî) *
$num
îî* .
,
îî. /
$num
îî0 1
,
îî1 2
$num
îî3 5
)
îî5 6
}
ïï 
)
ññ 
;
ññ 
Builder
ôô 
.
ôô 
Entity
ôô 
<
ôô 
Appointment
ôô &
>
ôô& '
(
ôô' (
)
ôô( )
.
ôô) *
HasData
ôô* 1
(
ôô1 2
new
öö 
Appointment
öö 
{
õõ 
AppointmentId
úú !
=
úú" #
$num
úú$ %
,
úú% &
	PatientId
ùù 
=
ùù 
$num
ùù  !
,
ùù! "
DoctorId
ûû 
=
ûû 
$num
ûû  
,
ûû  !
ScheduledDate
üü !
=
üü" #
UtcDate
üü$ +
(
üü+ ,
$num
üü, 0
,
üü0 1
$num
üü2 3
,
üü3 4
$num
üü5 7
)
üü7 8
,
üü8 9
TimeSlot
†† 
=
†† 
$str
†† 4
,
††4 5
Status
°° 
=
°° 
AppointmentStatus
°° .
.
°°. /
Pending
°°/ 6
,
°°6 7 
CancellationReason
¢¢ &
=
¢¢' (
null
¢¢) -
,
¢¢- .
CreatedDate
££ 
=
££  !
UtcDate
££" )
(
££) *
$num
££* .
,
££. /
$num
££0 1
,
££1 2
$num
££3 5
)
££5 6
}
§§ 
,
§§ 
new
•• 
Appointment
•• 
{
¶¶ 
AppointmentId
ßß !
=
ßß" #
$num
ßß$ %
,
ßß% &
	PatientId
®® 
=
®® 
$num
®®  !
,
®®! "
DoctorId
©© 
=
©© 
$num
©©  
,
©©  !
ScheduledDate
™™ !
=
™™" #
UtcDate
™™$ +
(
™™+ ,
$num
™™, 0
,
™™0 1
$num
™™2 3
,
™™3 4
$num
™™5 7
)
™™7 8
,
™™8 9
TimeSlot
´´ 
=
´´ 
$str
´´ 4
,
´´4 5
Status
¨¨ 
=
¨¨ 
AppointmentStatus
¨¨ .
.
¨¨. /
	Confirmed
¨¨/ 8
,
¨¨8 9 
CancellationReason
≠≠ &
=
≠≠' (
null
≠≠) -
,
≠≠- .
CreatedDate
ÆÆ 
=
ÆÆ  !
UtcDate
ÆÆ" )
(
ÆÆ) *
$num
ÆÆ* .
,
ÆÆ. /
$num
ÆÆ0 1
,
ÆÆ1 2
$num
ÆÆ3 5
)
ÆÆ5 6
}
ØØ 
,
ØØ 
new
∞∞ 
Appointment
∞∞ 
{
±± 
AppointmentId
≤≤ !
=
≤≤" #
$num
≤≤$ %
,
≤≤% &
	PatientId
≥≥ 
=
≥≥ 
$num
≥≥  !
,
≥≥! "
DoctorId
¥¥ 
=
¥¥ 
$num
¥¥  
,
¥¥  !
ScheduledDate
µµ !
=
µµ" #
UtcDate
µµ$ +
(
µµ+ ,
$num
µµ, 0
,
µµ0 1
$num
µµ2 3
,
µµ3 4
$num
µµ5 7
)
µµ7 8
,
µµ8 9
TimeSlot
∂∂ 
=
∂∂ 
$str
∂∂ 4
,
∂∂4 5
Status
∑∑ 
=
∑∑ 
AppointmentStatus
∑∑ .
.
∑∑. /
	Cancelled
∑∑/ 8
,
∑∑8 9 
CancellationReason
∏∏ &
=
∏∏' (
$str
∏∏) I
,
∏∏I J
CreatedDate
ππ 
=
ππ  !
UtcDate
ππ" )
(
ππ) *
$num
ππ* .
,
ππ. /
$num
ππ0 1
,
ππ1 2
$num
ππ3 5
)
ππ5 6
}
∫∫ 
)
ªª 
;
ªª 
Builder
ææ 
.
ææ 
Entity
ææ 
<
ææ 
HealthRecord
ææ '
>
ææ' (
(
ææ( )
)
ææ) *
.
ææ* +
HasData
ææ+ 2
(
ææ2 3
new
øø 
HealthRecord
øø  
{
¿¿ 
HealthRecordId
¡¡ "
=
¡¡# $
$num
¡¡% &
,
¡¡& '
	PatientId
¬¬ 
=
¬¬ 
$num
¬¬  !
,
¬¬! "
DoctorId
√√ 
=
√√ 
$num
√√  
,
√√  !
AppointmentId
ƒƒ !
=
ƒƒ" #
$num
ƒƒ$ %
,
ƒƒ% &
	VisitDate
≈≈ 
=
≈≈ 
UtcDate
≈≈  '
(
≈≈' (
$num
≈≈( ,
,
≈≈, -
$num
≈≈. /
,
≈≈/ 0
$num
≈≈1 3
)
≈≈3 4
,
≈≈4 5
	Diagnosis
∆∆ 
=
∆∆ 
$str
∆∆  0
,
∆∆0 1
Prescription
««  
=
««! "
$str
««# B
,
««B C
Notes
»» 
=
»» 
$str
»» >
,
»»> ?
CreatedDate
…… 
=
……  !
UtcDate
……" )
(
……) *
$num
……* .
,
……. /
$num
……0 1
,
……1 2
$num
……3 5
)
……5 6
}
   
,
   
new
ÀÀ 
HealthRecord
ÀÀ  
{
ÃÃ 
HealthRecordId
ÕÕ "
=
ÕÕ# $
$num
ÕÕ% &
,
ÕÕ& '
	PatientId
ŒŒ 
=
ŒŒ 
$num
ŒŒ  !
,
ŒŒ! "
DoctorId
œœ 
=
œœ 
$num
œœ  
,
œœ  !
AppointmentId
–– !
=
––" #
$num
––$ %
,
––% &
	VisitDate
—— 
=
—— 
UtcDate
——  '
(
——' (
$num
——( ,
,
——, -
$num
——. /
,
——/ 0
$num
——1 3
)
——3 4
,
——4 5
	Diagnosis
““ 
=
““ 
$str
““  2
,
““2 3
Prescription
””  
=
””! "
$str
””# H
,
””H I
Notes
‘‘ 
=
‘‘ 
$str
‘‘ ?
,
‘‘? @
CreatedDate
’’ 
=
’’  !
UtcDate
’’" )
(
’’) *
$num
’’* .
,
’’. /
$num
’’0 1
,
’’1 2
$num
’’3 5
)
’’5 6
}
÷÷ 
,
÷÷ 
new
◊◊ 
HealthRecord
◊◊  
{
ÿÿ 
HealthRecordId
ŸŸ "
=
ŸŸ# $
$num
ŸŸ% &
,
ŸŸ& '
	PatientId
⁄⁄ 
=
⁄⁄ 
$num
⁄⁄  !
,
⁄⁄! "
DoctorId
€€ 
=
€€ 
null
€€ #
,
€€# $
AppointmentId
‹‹ !
=
‹‹" #
$num
‹‹$ %
,
‹‹% &
	VisitDate
›› 
=
›› 
UtcDate
››  '
(
››' (
$num
››( ,
,
››, -
$num
››. /
,
››/ 0
$num
››1 3
)
››3 4
,
››4 5
	Diagnosis
ﬁﬁ 
=
ﬁﬁ 
$str
ﬁﬁ  8
,
ﬁﬁ8 9
Prescription
ﬂﬂ  
=
ﬂﬂ! "
$str
ﬂﬂ# ;
,
ﬂﬂ; <
Notes
‡‡ 
=
‡‡ 
$str
‡‡ B
,
‡‡B C
CreatedDate
·· 
=
··  !
UtcDate
··" )
(
··) *
$num
··* .
,
··. /
$num
··0 1
,
··1 2
$num
··3 5
)
··5 6
}
‚‚ 
)
„„ 
;
„„ 
}
‰‰ 	
}
ÂÂ 
}ÊÊ ‹
UC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Data\AdminSeeder.cs
	namespace 	
HealthCareApp
 
. 
Data 
{ 
public 

static 
class 
AdminSeeder #
{ 
public 
static 
async 
Task  
SeedAdminAsync! /
(/ 0
UserManager 
< 
IdentityUser $
>$ %
userManager& 1
,1 2
RoleManager		 
<		 
IdentityRole		 $
>		$ %
roleManager		& 1
,		1 2
IConfiguration

 
configuration

 (
)

( )
{ 	
string 
	adminRole 
= 
$str &
;& '
string 
? 

adminEmail 
=  
configuration! .
[. /
$str/ @
]@ A
;A B
string 
? 
adminPassword !
=" #
configuration$ 1
[1 2
$str2 F
]F G
;G H
if 
( 
string 
. 
IsNullOrWhiteSpace )
() *

adminEmail* 4
)4 5
||6 8
string 
. 
IsNullOrWhiteSpace )
() *
adminPassword* 7
)7 8
)8 9
{ 
throw 
new %
InvalidOperationException 3
(3 4
$str r
)r s
;s t
} 
if 
( 
! 
await 
roleManager "
." #
RoleExistsAsync# 2
(2 3
	adminRole3 <
)< =
)= >
{ 
await 
roleManager !
.! "
CreateAsync" -
(- .
new. 1
IdentityRole2 >
(> ?
	adminRole? H
)H I
)I J
;J K
} 
var 
existingAdmin 
= 
await  %
userManager& 1
.1 2
FindByEmailAsync2 B
(B C

adminEmailC M
)M N
;N O
if   
(   
existingAdmin   
==    
null  ! %
)  % &
{!! 
var"" 
	adminUser"" 
="" 
new""  #
IdentityUser""$ 0
{## 
UserName$$ 
=$$ 

adminEmail$$ )
,$$) *
Email%% 
=%% 

adminEmail%% &
,%%& '
EmailConfirmed&& "
=&&# $
true&&% )
}'' 
;'' 
var)) 
result)) 
=)) 
await)) "
userManager))# .
.)). /
CreateAsync))/ :
()): ;
	adminUser)); D
,))D E
adminPassword))F S
)))S T
;))T U
if++ 
(++ 
result++ 
.++ 
	Succeeded++ $
)++$ %
{,, 
await-- 
userManager-- %
.--% &
AddToRoleAsync--& 4
(--4 5
	adminUser--5 >
,--> ?
	adminRole--@ I
)--I J
;--J K
}.. 
else// 
{00 
var11 
errors11 
=11  
string11! '
.11' (
Join11( ,
(11, -
$str11- 1
,111 2
result113 9
.119 :
Errors11: @
.11@ A
Select11A G
(11G H
e11H I
=>11J L
e11M N
.11N O
Description11O Z
)11Z [
)11[ \
;11\ ]
throw33 
new33 %
InvalidOperationException33 7
(337 8
$"44 
$str44 5
{445 6
errors446 <
}44< =
"44= >
)44> ?
;44? @
}55 
}66 
}77 	
}88 
}99 ≠S
cC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\PatientsController.cs
	namespace 	
HealthCareApp
 
. 
Controllers #
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public 

class 
PatientsController #
:$ %
ControllerBase& 4
{ 
private 
readonly 
IPatientService (
_patientService) 8
;8 9
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public 
PatientsController !
(! "
IPatientService 
patientService *
,* + 
IHealthRecordService  
healthRecordService! 4
)4 5
{ 	
_patientService 
= 
patientService ,
;, - 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes   
=   
JwtBearerDefaults   -
.  - . 
AuthenticationScheme  . B
,  B C
Roles!! 	
=!!
 
$str!! 
)!! 
]!! 
public"" 
async"" 
Task"" 
<"" 
IActionResult"" '
>""' (
GetAllPatients"") 7
(""7 8
[""8 9
	FromQuery""9 B
]""B C%
PatientPaginationQueryDto""D ]
query""^ c
)""c d
{## 	
var$$ 
patients$$ 
=$$ 
await$$  
_patientService$$! 0
.$$0 1$
GetAllPatientsPagedAsync$$1 I
($$I J
query$$J O
)$$O P
;$$P Q
return&& 
Ok&& 
(&& 
patients&& 
)&& 
;&&  
}'' 	
[++ 	
HttpGet++	 
(++ 
$str++ 
)++ 
]++ 
[,, 	
	Authorize,,	 
(,, !
AuthenticationSchemes-- !
=--" #
JwtBearerDefaults--$ 5
.--5 6 
AuthenticationScheme--6 J
,--J K
Roles.. 
=.. 
$str.. 
).. 
].. 
public// 
async// 
Task// 
<// 
IActionResult// '
>//' (
GetMyProfile//) 5
(//5 6
)//6 7
{00 	
var11 
identityUserId11 
=11  
User11! %
.11% &
	FindFirst11& /
(11/ 0

ClaimTypes110 :
.11: ;
NameIdentifier11; I
)11I J
?11J K
.11K L
Value11L Q
;11Q R
if33 
(33 
string33 
.33 
IsNullOrWhiteSpace33 )
(33) *
identityUserId33* 8
)338 9
)339 :
{44 
return55 
Unauthorized55 #
(55# $
new55$ '
{66 
Message77 
=77 
$str77 3
}88 
)88 
;88 
}99 
var;; 
patient;; 
=;; 
await;; 
_patientService;;  /
.;;/ 0
GetMyProfileAsync;;0 A
(;;A B
identityUserId;;B P
);;P Q
;;;Q R
return== 
Ok== 
(== 
patient== 
)== 
;== 
}>> 	
[BB 	
HttpPutBB	 
(BB 
$strBB 
)BB 
]BB 
[CC 	
	AuthorizeCC	 
(CC !
AuthenticationSchemesDD !
=DD" #
JwtBearerDefaultsDD$ 5
.DD5 6 
AuthenticationSchemeDD6 J
,DDJ K
RolesEE 
=EE 
$strEE 
)EE 
]EE 
publicFF 
asyncFF 
TaskFF 
<FF 
IActionResultFF '
>FF' (
UpdateMyProfileFF) 8
(FF8 9
[FF9 :
FromBodyFF: B
]FFB C
UpdatePatientDtoFFD T
requestFFU \
)FF\ ]
{GG 	
varHH 
identityUserIdHH 
=HH  
UserHH! %
.HH% &
	FindFirstHH& /
(HH/ 0

ClaimTypesHH0 :
.HH: ;
NameIdentifierHH; I
)HHI J
?HHJ K
.HHK L
ValueHHL Q
;HHQ R
ifJJ 
(JJ 
stringJJ 
.JJ 
IsNullOrWhiteSpaceJJ )
(JJ) *
identityUserIdJJ* 8
)JJ8 9
)JJ9 :
{KK 
returnLL 
UnauthorizedLL #
(LL# $
newLL$ '
{MM 
MessageNN 
=NN 
$strNN 3
}OO 
)OO 
;OO 
}PP 
varRR 
patientRR 
=RR 
awaitRR 
_patientServiceRR  /
.RR/ 0 
UpdateMyProfileAsyncRR0 D
(RRD E
identityUserIdRRE S
,RRS T
requestRRU \
)RR\ ]
;RR] ^
returnTT 
OkTT 
(TT 
patientTT 
)TT 
;TT 
}UU 	
[YY 	
HttpGetYY	 
(YY 
$strYY $
)YY$ %
]YY% &
[ZZ 	
	AuthorizeZZ	 
(ZZ !
AuthenticationSchemes[[ !
=[[" #
JwtBearerDefaults[[$ 5
.[[5 6 
AuthenticationScheme[[6 J
,[[J K
Roles\\ 
=\\ 
$str\\ 
)\\ 
]\\ 
public]] 
async]] 
Task]] 
<]] 
IActionResult]] '
>]]' (
GetMyHealthRecords]]) ;
(]]; <
)]]< =
{^^ 	
var__ 
identityUserId__ 
=__  
User__! %
.__% &
	FindFirst__& /
(__/ 0

ClaimTypes__0 :
.__: ;
NameIdentifier__; I
)__I J
?__J K
.__K L
Value__L Q
;__Q R
ifaa 
(aa 
stringaa 
.aa 
IsNullOrWhiteSpaceaa )
(aa) *
identityUserIdaa* 8
)aa8 9
)aa9 :
{bb 
returncc 
Unauthorizedcc #
(cc# $
newcc$ '
{dd 
Messageee 
=ee 
$stree 3
}ff 
)ff 
;ff 
}gg 
varii 
patientii 
=ii 
awaitii 
_patientServiceii  /
.ii/ 0
GetMyProfileAsyncii0 A
(iiA B
identityUserIdiiB P
)iiP Q
;iiQ R
varkk 
recordskk 
=kk 
awaitkk  
_healthRecordServicekk  4
.kk4 5,
 GetHealthRecordsByPatientIdAsynckk5 U
(kkU V
patientkkV ]
.kk] ^
	PatientIdkk^ g
)kkg h
;kkh i
returnmm 
Okmm 
(mm 
recordsmm 
)mm 
;mm 
}nn 	
[rr 	
HttpGetrr	 
(rr 
$strrr "
)rr" #
]rr# $
[ss 	
	Authorizess	 
(ss !
AuthenticationSchemestt !
=tt" #
JwtBearerDefaultstt$ 5
.tt5 6 
AuthenticationSchemett6 J
,ttJ K
Rolesuu 
=uu 
$struu 
)uu 
]uu 
publicvv 
asyncvv 
Taskvv 
<vv 
IActionResultvv '
>vv' (
GetPatientByIdvv) 7
(vv7 8
[vv8 9
	FromRoutevv9 B
]vvB C
intvvD G
	patientIdvvH Q
)vvQ R
{ww 	
varxx 
patientxx 
=xx 
awaitxx 
_patientServicexx  /
.xx/ 0
GetPatientByIdAsyncxx0 C
(xxC D
	patientIdxxD M
)xxM N
;xxN O
returnzz 
Okzz 
(zz 
patientzz 
)zz 
;zz 
}{{ 	
[ 	
HttpPut	 
( 
$str "
)" #
]# $
[
ÄÄ 	
	Authorize
ÄÄ	 
(
ÄÄ #
AuthenticationSchemes
ÅÅ !
=
ÅÅ" #
JwtBearerDefaults
ÅÅ$ 5
.
ÅÅ5 6"
AuthenticationScheme
ÅÅ6 J
,
ÅÅJ K
Roles
ÇÇ 
=
ÇÇ 
$str
ÇÇ 
)
ÇÇ 
]
ÇÇ 
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
ÉÉ' (
UpdatePatient
ÉÉ) 6
(
ÉÉ6 7
[
ÑÑ 
	FromRoute
ÑÑ 
]
ÑÑ 
int
ÑÑ 
	patientId
ÑÑ %
,
ÑÑ% &
[
ÖÖ 
FromBody
ÖÖ 
]
ÖÖ 
UpdatePatientDto
ÖÖ '
request
ÖÖ( /
)
ÖÖ/ 0
{
ÜÜ 	
var
áá 
patient
áá 
=
áá 
await
áá 
_patientService
áá  /
.
áá/ 0 
UpdatePatientAsync
áá0 B
(
ááB C
	patientId
ááC L
,
ááL M
request
ááN U
)
ááU V
;
ááV W
return
ââ 
Ok
ââ 
(
ââ 
patient
ââ 
)
ââ 
;
ââ 
}
ää 	
[
éé 	
HttpGet
éé	 
(
éé 
$str
éé 1
)
éé1 2
]
éé2 3
[
èè 	
	Authorize
èè	 
(
èè #
AuthenticationSchemes
êê !
=
êê" #
JwtBearerDefaults
êê$ 5
.
êê5 6"
AuthenticationScheme
êê6 J
,
êêJ K
Roles
ëë 
=
ëë 
$str
ëë "
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
íí' (%
GetPatientHealthRecords
íí) @
(
íí@ A
[
ííA B
	FromRoute
ííB K
]
ííK L
int
ííM P
	patientId
ííQ Z
)
ííZ [
{
ìì 	
var
îî 
records
îî 
=
îî 
await
îî "
_healthRecordService
îî  4
.
îî4 5.
 GetHealthRecordsByPatientIdAsync
îî5 U
(
îîU V
	patientId
îîV _
)
îî_ `
;
îî` a
return
ññ 
Ok
ññ 
(
ññ 
records
ññ 
)
ññ 
;
ññ 
}
óó 	
}
òò 
}ôô ìÖ
hC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\HealthRecordsController.cs
	namespace		 	
HealthCareApp		
 
.		 
Controllers		 #
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
] 
public 

class #
HealthRecordsController (
(( ) 
IHealthRecordService) =
service> E
)E F
:G H
ControllerBaseI W
{ 
private 
const 
string #
InvalidUserTokenMessage 4
=5 6
$str7 L
;L M
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes !
=" #
JwtBearerDefaults$ 5
.5 6 
AuthenticationScheme6 J
,J K
Roles 
= 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAllHealthRecords) <
(< =
)= >
{ 	
var 
records 
= 
await 
service  '
.' ($
GetAllHealthRecordsAsync( @
(@ A
)A B
;B C
return 
Ok 
( 
records 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
[   	
	Authorize  	 
(   !
AuthenticationSchemes!! !
=!!" #
JwtBearerDefaults!!$ 5
.!!5 6 
AuthenticationScheme!!6 J
,!!J K
Roles"" 
="" 
$str"" $
)""$ %
]""% &
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
GetMyHealthRecords##) ;
(##; <
)##< =
{$$ 	
var%% 
identityUserId%% 
=%%  
User%%! %
.%%% &
	FindFirst%%& /
(%%/ 0

ClaimTypes%%0 :
.%%: ;
NameIdentifier%%; I
)%%I J
?%%J K
.%%K L
Value%%L Q
;%%Q R
if'' 
('' 
string'' 
.'' 
IsNullOrWhiteSpace'' )
('') *
identityUserId''* 8
)''8 9
)''9 :
{(( 
return)) 
Unauthorized)) #
())# $
new))$ '
{** 
Message++ 
=++ #
InvalidUserTokenMessage++ 5
},, 
),, 
;,, 
}-- 
if// 
(// 
User// 
.// 
IsInRole// 
(// 
$str// '
)//' (
)//( )
{00 
var11 
records11 
=11 
await11 #
service11$ +
.11+ ,-
!GetMyHealthRecordsForPatientAsync11, M
(11M N
identityUserId11N \
)11\ ]
;11] ^
return33 
Ok33 
(33 
records33 !
)33! "
;33" #
}44 
if66 
(66 
User66 
.66 
IsInRole66 
(66 
$str66 &
)66& '
)66' (
{77 
var88 
records88 
=88 
await88 #
service88$ +
.88+ ,,
 GetMyHealthRecordsForDoctorAsync88, L
(88L M
identityUserId88M [
)88[ \
;88\ ]
return:: 
Ok:: 
(:: 
records:: !
)::! "
;::" #
};; 
return== 
Forbid== 
(== 
)== 
;== 
}>> 	
[BB 	
HttpGetBB	 
(BB 
$strBB '
)BB' (
]BB( )
[CC 	
	AuthorizeCC	 
(CC !
AuthenticationSchemesDD !
=DD" #
JwtBearerDefaultsDD$ 5
.DD5 6 
AuthenticationSchemeDD6 J
,DDJ K
RolesEE 
=EE 
$strEE *
)EE* +
]EE+ ,
publicFF 
asyncFF 
TaskFF 
<FF 
IActionResultFF '
>FF' (
GetHealthRecordByIdFF) <
(FF< =
[FF= >
	FromRouteFF> G
]FFG H
intFFI L
healthRecordIdFFM [
)FF[ \
{GG 	
ifHH 
(HH 
UserHH 
.HH 
IsInRoleHH 
(HH 
$strHH %
)HH% &
)HH& '
{II 
varJJ 
recordJJ 
=JJ 
awaitJJ "
serviceJJ# *
.JJ* +$
GetHealthRecordByIdAsyncJJ+ C
(JJC D
healthRecordIdJJD R
)JJR S
;JJS T
returnLL 
OkLL 
(LL 
recordLL  
)LL  !
;LL! "
}MM 
varOO 
identityUserIdOO 
=OO  
UserOO! %
.OO% &
	FindFirstOO& /
(OO/ 0

ClaimTypesOO0 :
.OO: ;
NameIdentifierOO; I
)OOI J
?OOJ K
.OOK L
ValueOOL Q
;OOQ R
ifQQ 
(QQ 
stringQQ 
.QQ 
IsNullOrWhiteSpaceQQ )
(QQ) *
identityUserIdQQ* 8
)QQ8 9
)QQ9 :
{RR 
returnSS 
UnauthorizedSS #
(SS# $
newSS$ '
{TT 
MessageUU 
=UU #
InvalidUserTokenMessageUU 5
}VV 
)VV 
;VV 
}WW 
ifYY 
(YY 
UserYY 
.YY 
IsInRoleYY 
(YY 
$strYY '
)YY' (
)YY( )
{ZZ 
var[[ 
record[[ 
=[[ 
await[[ "
service[[# *
.[[* +.
"GetHealthRecordByIdForPatientAsync[[+ M
([[M N
healthRecordId\\ "
,\\" #
identityUserId]] "
)]]" #
;]]# $
return__ 
Ok__ 
(__ 
record__  
)__  !
;__! "
}`` 
ifbb 
(bb 
Userbb 
.bb 
IsInRolebb 
(bb 
$strbb &
)bb& '
)bb' (
{cc 
vardd 
recorddd 
=dd 
awaitdd "
servicedd# *
.dd* +-
!GetHealthRecordByIdForDoctorAsyncdd+ L
(ddL M
healthRecordIdee "
,ee" #
identityUserIdff "
)ff" #
;ff# $
returnhh 
Okhh 
(hh 
recordhh  
)hh  !
;hh! "
}ii 
returnkk 
Forbidkk 
(kk 
)kk 
;kk 
}ll 	
[qq 	
HttpGetqq	 
(qq 
$strqq *
)qq* +
]qq+ ,
[rr 	
	Authorizerr	 
(rr !
AuthenticationSchemesss !
=ss" #
JwtBearerDefaultsss$ 5
.ss5 6 
AuthenticationSchemess6 J
,ssJ K
Rolestt 
=tt 
$strtt 
)tt 
]tt 
publicuu 
asyncuu 
Taskuu 
<uu 
IActionResultuu '
>uu' ('
GetHealthRecordsByPatientIduu) D
(uuD E
[uuE F
	FromRouteuuF O
]uuO P
intuuQ T
	patientIduuU ^
)uu^ _
{vv 	
varww 
recordsww 
=ww 
awaitww 
serviceww  '
.ww' (,
 GetHealthRecordsByPatientIdAsyncww( H
(wwH I
	patientIdwwI R
)wwR S
;wwS T
returnyy 
Okyy 
(yy 
recordsyy 
)yy 
;yy 
}zz 	
[~~ 	
HttpGet~~	 
(~~ 
$str~~ (
)~~( )
]~~) *
[ 	
	Authorize	 
( #
AuthenticationSchemes
ÄÄ !
=
ÄÄ" #
JwtBearerDefaults
ÄÄ$ 5
.
ÄÄ5 6"
AuthenticationScheme
ÄÄ6 J
,
ÄÄJ K
Roles
ÅÅ 
=
ÅÅ 
$str
ÅÅ 
)
ÅÅ 
]
ÅÅ 
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
IActionResult
ÇÇ '
>
ÇÇ' ((
GetHealthRecordsByDoctorId
ÇÇ) C
(
ÇÇC D
[
ÇÇD E
	FromRoute
ÇÇE N
]
ÇÇN O
int
ÇÇP S
doctorId
ÇÇT \
)
ÇÇ\ ]
{
ÉÉ 	
var
ÑÑ 
records
ÑÑ 
=
ÑÑ 
await
ÑÑ 
service
ÑÑ  '
.
ÑÑ' (-
GetHealthRecordsByDoctorIdAsync
ÑÑ( G
(
ÑÑG H
doctorId
ÑÑH P
)
ÑÑP Q
;
ÑÑQ R
return
ÜÜ 
Ok
ÜÜ 
(
ÜÜ 
records
ÜÜ 
)
ÜÜ 
;
ÜÜ 
}
áá 	
[
ãã 	
HttpGet
ãã	 
(
ãã 
$str
ãã 2
)
ãã2 3
]
ãã3 4
[
åå 	
	Authorize
åå	 
(
åå #
AuthenticationSchemes
çç !
=
çç" #
JwtBearerDefaults
çç$ 5
.
çç5 6"
AuthenticationScheme
çç6 J
,
ççJ K
Roles
éé 
=
éé 
$str
éé "
)
éé" #
]
éé# $
public
èè 
async
èè 
Task
èè 
<
èè 
IActionResult
èè '
>
èè' (-
GetHealthRecordsByAppointmentId
èè) H
(
èèH I
[
èèI J
	FromRoute
èèJ S
]
èèS T
int
èèU X
appointmentId
èèY f
)
èèf g
{
êê 	
if
ëë 
(
ëë 
User
ëë 
.
ëë 
IsInRole
ëë 
(
ëë 
$str
ëë %
)
ëë% &
)
ëë& '
{
íí 
var
ìì 
records
ìì 
=
ìì 
await
ìì #
service
ìì$ +
.
ìì+ ,2
$GetHealthRecordsByAppointmentIdAsync
ìì, P
(
ììP Q
appointmentId
ììQ ^
)
ìì^ _
;
ìì_ `
return
ïï 
Ok
ïï 
(
ïï 
records
ïï !
)
ïï! "
;
ïï" #
}
ññ 
var
òò 
identityUserId
òò 
=
òò  
User
òò! %
.
òò% &
	FindFirst
òò& /
(
òò/ 0

ClaimTypes
òò0 :
.
òò: ;
NameIdentifier
òò; I
)
òòI J
?
òòJ K
.
òòK L
Value
òòL Q
;
òòQ R
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
öö) *
identityUserId
öö* 8
)
öö8 9
)
öö9 :
{
õõ 
return
úú 
Unauthorized
úú #
(
úú# $
new
úú$ '
{
ùù 
Message
ûû 
=
ûû %
InvalidUserTokenMessage
ûû 5
}
üü 
)
üü 
;
üü 
}
†† 
var
¢¢ 
doctorRecords
¢¢ 
=
¢¢ 
await
¢¢  %
service
¢¢& -
.
¢¢- .;
-GetHealthRecordsByAppointmentIdForDoctorAsync
¢¢. [
(
¢¢[ \
appointmentId
££ 
,
££ 
identityUserId
§§ 
)
§§ 
;
§§  
return
¶¶ 
Ok
¶¶ 
(
¶¶ 
doctorRecords
¶¶ #
)
¶¶# $
;
¶¶$ %
}
ßß 	
[
´´ 	
HttpPost
´´	 
]
´´ 
[
¨¨ 	
	Authorize
¨¨	 
(
¨¨ #
AuthenticationSchemes
≠≠ !
=
≠≠" #
JwtBearerDefaults
≠≠$ 5
.
≠≠5 6"
AuthenticationScheme
≠≠6 J
,
≠≠J K
Roles
ÆÆ 
=
ÆÆ 
$str
ÆÆ 
)
ÆÆ 
]
ÆÆ 
public
ØØ 
async
ØØ 
Task
ØØ 
<
ØØ 
IActionResult
ØØ '
>
ØØ' (
AddHealthRecord
ØØ) 8
(
ØØ8 9
[
ØØ9 :
FromBody
ØØ: B
]
ØØB C 
AddHealthRecordDto
ØØD V
request
ØØW ^
)
ØØ^ _
{
∞∞ 	
var
±± 
identityUserId
±± 
=
±±  
User
±±! %
.
±±% &
	FindFirst
±±& /
(
±±/ 0

ClaimTypes
±±0 :
.
±±: ;
NameIdentifier
±±; I
)
±±I J
?
±±J K
.
±±K L
Value
±±L Q
;
±±Q R
if
≥≥ 
(
≥≥ 
string
≥≥ 
.
≥≥  
IsNullOrWhiteSpace
≥≥ )
(
≥≥) *
identityUserId
≥≥* 8
)
≥≥8 9
)
≥≥9 :
{
¥¥ 
return
µµ 
Unauthorized
µµ #
(
µµ# $
new
µµ$ '
{
∂∂ 
Message
∑∑ 
=
∑∑ %
InvalidUserTokenMessage
∑∑ 5
}
∏∏ 
)
∏∏ 
;
∏∏ 
}
ππ 
var
ªª 
record
ªª 
=
ªª 
await
ªª 
service
ªª &
.
ªª& '+
AddHealthRecordForDoctorAsync
ªª' D
(
ªªD E
request
ºº 
,
ºº 
identityUserId
ΩΩ 
)
ΩΩ 
;
ΩΩ  
return
øø 
CreatedAtAction
øø "
(
øø" #
nameof
¿¿ 
(
¿¿ !
GetHealthRecordById
¿¿ *
)
¿¿* +
,
¿¿+ ,
new
¡¡ 
{
¡¡ 
healthRecordId
¡¡ $
=
¡¡% &
record
¡¡' -
.
¡¡- .
HealthRecordId
¡¡. <
}
¡¡= >
,
¡¡> ?
record
¬¬ 
)
¬¬ 
;
¬¬ 
}
√√ 	
[
«« 	
HttpPut
««	 
(
«« 
$str
«« '
)
««' (
]
««( )
[
»» 	
	Authorize
»»	 
(
»» #
AuthenticationSchemes
…… !
=
……" #
JwtBearerDefaults
……$ 5
.
……5 6"
AuthenticationScheme
……6 J
,
……J K
Roles
   
=
   
$str
   
)
   
]
   
public
ÀÀ 
async
ÀÀ 
Task
ÀÀ 
<
ÀÀ 
IActionResult
ÀÀ '
>
ÀÀ' ( 
UpdateHealthRecord
ÀÀ) ;
(
ÀÀ; <
[
ÃÃ 
	FromRoute
ÃÃ 
]
ÃÃ 
int
ÃÃ 
healthRecordId
ÃÃ *
,
ÃÃ* +
[
ÕÕ 
FromBody
ÕÕ 
]
ÕÕ #
UpdateHealthRecordDto
ÕÕ ,
request
ÕÕ- 4
)
ÕÕ4 5
{
ŒŒ 	
var
œœ 
identityUserId
œœ 
=
œœ  
User
œœ! %
.
œœ% &
	FindFirst
œœ& /
(
œœ/ 0

ClaimTypes
œœ0 :
.
œœ: ;
NameIdentifier
œœ; I
)
œœI J
?
œœJ K
.
œœK L
Value
œœL Q
;
œœQ R
if
—— 
(
—— 
string
—— 
.
——  
IsNullOrWhiteSpace
—— )
(
——) *
identityUserId
——* 8
)
——8 9
)
——9 :
{
““ 
return
”” 
Unauthorized
”” #
(
””# $
new
””$ '
{
‘‘ 
Message
’’ 
=
’’ %
InvalidUserTokenMessage
’’ 5
}
÷÷ 
)
÷÷ 
;
÷÷ 
}
◊◊ 
var
ŸŸ 
record
ŸŸ 
=
ŸŸ 
await
ŸŸ 
service
ŸŸ &
.
ŸŸ& '.
 UpdateHealthRecordForDoctorAsync
ŸŸ' G
(
ŸŸG H
healthRecordId
⁄⁄ 
,
⁄⁄ 
request
€€ 
,
€€ 
identityUserId
‹‹ 
)
‹‹ 
;
‹‹  
return
ﬁﬁ 
Ok
ﬁﬁ 
(
ﬁﬁ 
record
ﬁﬁ 
)
ﬁﬁ 
;
ﬁﬁ 
}
ﬂﬂ 	
[
‚‚ 	

HttpDelete
‚‚	 
(
‚‚ 
$str
‚‚ *
)
‚‚* +
]
‚‚+ ,
[
„„ 	
	Authorize
„„	 
(
„„ #
AuthenticationSchemes
‰‰ !
=
‰‰" #
JwtBearerDefaults
‰‰$ 5
.
‰‰5 6"
AuthenticationScheme
‰‰6 J
,
‰‰J K
Roles
ÂÂ 
=
ÂÂ 
$str
ÂÂ 
)
ÂÂ 
]
ÂÂ 
public
ÊÊ 
async
ÊÊ 
Task
ÊÊ 
<
ÊÊ 
IActionResult
ÊÊ '
>
ÊÊ' ( 
DeleteHealthRecord
ÊÊ) ;
(
ÊÊ; <
[
ÊÊ< =
	FromRoute
ÊÊ= F
]
ÊÊF G
int
ÊÊH K
healthRecordId
ÊÊL Z
)
ÊÊZ [
{
ÁÁ 	
var
ËË 
record
ËË 
=
ËË 
await
ËË 
service
ËË &
.
ËË& '%
DeleteHealthRecordAsync
ËË' >
(
ËË> ?
healthRecordId
ËË? M
)
ËËM N
;
ËËN O
return
ÍÍ 
Ok
ÍÍ 
(
ÍÍ 
record
ÍÍ 
)
ÍÍ 
;
ÍÍ 
}
ÎÎ 	
}
ÏÏ 
}ÌÌ Ó.
bC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\DoctorsController.cs
	namespace 	
HealthCareApp
 
. 
Controllers #
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
class 
DoctorsController "
(" #
IDoctorService# 1
service2 9
)9 :
:; <
ControllerBase= K
{ 
[ 	
HttpGet	 
( 
$str 
) 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes !
=" #
JwtBearerDefaults$ 5
.5 6 
AuthenticationScheme6 J
,J K
Roles 
= 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetMyProfile) 5
(5 6
)6 7
{ 	
var 
identityUserId 
=  
User! %
.% &
	FindFirst& /
(/ 0

ClaimTypes0 :
.: ;
NameIdentifier; I
)I J
?J K
.K L
ValueL Q
;Q R
if 
( 
string 
. 
IsNullOrWhiteSpace )
() *
identityUserId* 8
)8 9
)9 :
{ 
return 
Unauthorized #
(# $
new$ '
{ 
Message 
= 
$str 3
} 
) 
; 
} 
var 
result 
= 
await 
service &
.& '
GetMyProfileAsync' 8
(8 9
identityUserId9 G
)G H
;H I
return!! 
Ok!! 
(!! 
result!! 
)!! 
;!! 
}"" 	
[&& 	
HttpGet&&	 
]&& 
['' 	
	Authorize''	 
('' !
AuthenticationSchemes(( !
=((" #
JwtBearerDefaults(($ 5
.((5 6 
AuthenticationScheme((6 J
,((J K
Roles)) 
=)) 
$str)) #
)))# $
]))$ %
public** 
async** 
Task** 
<** 
IActionResult** '
>**' (
GetAllActiveDoctors**) <
(**< =
)**= >
{++ 	
var,, 
result,, 
=,, 
await,, 
service,, &
.,,& '$
GetAllActiveDoctorsAsync,,' ?
(,,? @
),,@ A
;,,A B
return.. 
Ok.. 
(.. 
result.. 
).. 
;.. 
}// 	
[33 	
HttpGet33	 
(33 
$str33 !
)33! "
]33" #
[44 	
	Authorize44	 
(44 !
AuthenticationSchemes55 !
=55" #
JwtBearerDefaults55$ 5
.555 6 
AuthenticationScheme556 J
,55J K
Roles66 
=66 
$str66 #
)66# $
]66$ %
public77 
async77 
Task77 
<77 
IActionResult77 '
>77' (
GetDoctorById77) 6
(776 7
[777 8
	FromRoute778 A
]77A B
int77C F
doctorId77G O
)77O P
{88 	
var99 
result99 
=99 
await99 
service99 &
.99& '
GetDoctorByIdAsync99' 9
(999 :
doctorId99: B
)99B C
;99C D
return;; 
Ok;; 
(;; 
result;; 
);; 
;;; 
}<< 	
[@@ 	
HttpGet@@	 
(@@ 
$str@@ 2
)@@2 3
]@@3 4
[AA 	
	AuthorizeAA	 
(AA !
AuthenticationSchemesBB !
=BB" #
JwtBearerDefaultsBB$ 5
.BB5 6 
AuthenticationSchemeBB6 J
,BBJ K
RolesCC 
=CC 
$strCC #
)CC# $
]CC$ %
publicDD 
asyncDD 
TaskDD 
<DD 
IActionResultDD '
>DD' (,
 GetActiveDoctorsBySpecialisationDD) I
(DDI J
[EE 
	FromRouteEE 
]EE 
SpecialisationTypeEE *
specialisationEE+ 9
)EE9 :
{FF 	
varGG 
resultGG 
=GG 
awaitGG 
serviceGG &
.GG& '1
%GetActiveDoctorsBySpecialisationAsyncGG' L
(GGL M
specialisationGGM [
)GG[ \
;GG\ ]
returnII 
OkII 
(II 
resultII 
)II 
;II 
}JJ 	
[NN 	
HttpGetNN	 
(NN 
$strNN .
)NN. /
]NN/ 0
[OO 	
	AuthorizeOO	 
(OO !
AuthenticationSchemesPP !
=PP" #
JwtBearerDefaultsPP$ 5
.PP5 6 
AuthenticationSchemePP6 J
,PPJ K
RolesQQ 
=QQ 
$strQQ #
)QQ# $
]QQ$ %
publicRR 
asyncRR 
TaskRR 
<RR 
IActionResultRR '
>RR' (!
GetDoctorAvailabilityRR) >
(RR> ?
[RR? @
	FromRouteRR@ I
]RRI J
intRRK N
doctorIdRRO W
)RRW X
{SS 	
varTT 
resultTT 
=TT 
awaitTT 
serviceTT &
.TT& '&
GetDoctorAvailabilityAsyncTT' A
(TTA B
doctorIdTTB J
)TTJ K
;TTK L
returnVV 
OkVV 
(VV 
resultVV 
)VV 
;VV 
}WW 	
}XX 
}YY ı*
_C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\AuthController.cs
	namespace 	
HealthCareApp
 
. 
Controllers #
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public 

class 
AuthController 
(  
IAuthService  ,
service- 4
)4 5
:6 7
ControllerBase8 F
{ 
[ 	
HttpPost	 
( 
$str $
)$ %
]% &
[ 	
AllowAnonymous	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
RegisterPatient) 8
(8 9
PatientRegisterDto9 K
requestL S
)S T
{ 	
var 
( 
success 
, 
message !
,! "
	patientId# ,
), -
=. /
await0 5
service6 =
.= > 
RegisterPatientAsync> R
(R S
requestS Z
)Z [
;[ \
if 
( 
! 
success 
) 
{ 
return 

BadRequest !
(! "
new" %
{ 
Message 
= 
message %
} 
) 
; 
} 
return 
Ok 
( 
new 
{ 
Message   
=   
message   !
,  ! "
	PatientId!! 
=!! 
	patientId!! %
}"" 
)"" 
;"" 
}$$ 	
[&& 	
HttpPost&&	 
(&& 
$str&& 
)&& 
]&& 
['' 	
AllowAnonymous''	 
]'' 
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
var** 
(** 
success** 
,** 
message** !
,**! "
token**# (
,**( )
	expiresIn*** 3
)**3 4
=**5 6
await**7 <
service**= D
.**D E
Login**E J
(**J K
request**K R
)**R S
;**S T
if,, 
(,, 
!,, 
success,, 
),, 
{-- 
return.. 
Unauthorized.. #
(..# $
new..$ '
{// 
Message00 
=00 
message00 %
}11 
)11 
;11 
}22 
AuthResponse44 
response44 !
=44" #
new44$ '
AuthResponse44( 4
{55 
AccessToken66 
=66 
token66 #
,66# $
Message77 
=77 
message77 !
,77! "
	ExpiresIn88 
=88 
	expiresIn88 %
}99 
;99 
return;; 
Ok;; 
(;; 
response;; 
);; 
;;;  
}<< 	
[>> 	
HttpPost>>	 
(>> 
$str>> #
)>># $
]>>$ %
[?? 	
	Authorize??	 
(?? !
AuthenticationSchemes?? (
=??) *
JwtBearerDefaults??+ <
.??< = 
AuthenticationScheme??= Q
)??Q R
]??R S
public@@ 
async@@ 
Task@@ 
<@@ 
IActionResult@@ '
>@@' (
ChangePassword@@) 7
(@@7 8
ChangePasswordDto@@8 I
request@@J Q
)@@Q R
{AA 	
varBB 
userIdBB 
=BB 
UserBB 
.BB 
	FindFirstBB '
(BB' (
SystemBB( .
.BB. /
SecurityBB/ 7
.BB7 8
ClaimsBB8 >
.BB> ?

ClaimTypesBB? I
.BBI J
NameIdentifierBBJ X
)BBX Y
?BBY Z
.BBZ [
ValueBB[ `
;BB` a
ifDD 
(DD 
stringDD 
.DD 
IsNullOrWhiteSpaceDD )
(DD) *
userIdDD* 0
)DD0 1
)DD1 2
{EE 
returnFF 
UnauthorizedFF #
(FF# $
newFF$ '
{GG 
MessageHH 
=HH 
$strHH 3
}II 
)II 
;II 
}JJ 
varLL 
(LL 
successLL 
,LL 
messageLL !
)LL! "
=LL# $
awaitLL% *
serviceLL+ 2
.LL2 3
ChangePasswordAsyncLL3 F
(LLF G
userIdLLG M
,LLM N
requestLLO V
)LLV W
;LLW X
ifNN 
(NN 
!NN 
successNN 
)NN 
{OO 
returnPP 

BadRequestPP !
(PP! "
newPP" %
{QQ 
MessageRR 
=RR 
messageRR %
}SS 
)SS 
;SS 
}TT 
returnVV 
OkVV 
(VV 
newVV 
{WW 
MessageXX 
=XX 
messageXX !
}YY 
)YY 
;YY 
}ZZ 	
}[[ 
}\\ £©
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\AppointmentController.cs
	namespace 	
HealthCareApp
 
. 
Controllers #
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public 

class "
AppointmentsController '
(' (
IAppointmentService( ;
service< C
)C D
:E F
ControllerBaseG U
{ 
private 
const 
string #
InvalidUserTokenMessage 4
=5 6
$str7 L
;L M
private 
const 
string 
PatientRoleName ,
=- .
$str/ 8
;8 9
private 
const 
string 
DoctorRoleName +
=, -
$str. 6
;6 7
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes 
= 
JwtBearerDefaults .
.. / 
AuthenticationScheme/ C
,C D
Roles 

= 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAllAppointments) ;
(; <
[< =
	FromQuery= F
]F G)
AppointmentPaginationQueryDtoH e
queryf k
)k l
{ 	
var 
appointments 
= 
await $
service% ,
., -(
GetAllAppointmentsPagedAsync- I
(I J
queryJ O
)O P
;P Q
return 
Ok 
( 
appointments "
)" #
;# $
}   	
[## 	
HttpGet##	 
(## 
$str## 
)## 
]## 
[$$ 	
	Authorize$$	 
($$ !
AuthenticationSchemes%% !
=%%" #
JwtBearerDefaults%%$ 5
.%%5 6 
AuthenticationScheme%%6 J
,%%J K
Roles&& 
=&& 
$str&& $
)&&$ %
]&&% &
public'' 
async'' 
Task'' 
<'' 
IActionResult'' '
>''' (
GetMyAppointments'') :
('': ;
)''; <
{(( 	
var)) 
identityUserId)) 
=))  
User))! %
.))% &
	FindFirst))& /
())/ 0

ClaimTypes))0 :
.)): ;
NameIdentifier)); I
)))I J
?))J K
.))K L
Value))L Q
;))Q R
if++ 
(++ 
string++ 
.++ 
IsNullOrWhiteSpace++ )
(++) *
identityUserId++* 8
)++8 9
)++9 :
{,, 
return-- 
Unauthorized-- #
(--# $
new--$ '
{.. 
Message// 
=// #
InvalidUserTokenMessage// 5
}00 
)00 
;00 
}11 
if33 
(33 
User33 
.33 
IsInRole33 
(33 
PatientRoleName33 -
)33- .
)33. /
{44 
var55 
appointments55  
=55! "
await55# (
service55) 0
.550 1,
 GetMyAppointmentsForPatientAsync551 Q
(55Q R
identityUserId55R `
)55` a
;55a b
return77 
Ok77 
(77 
appointments77 &
)77& '
;77' (
}88 
if:: 
(:: 
User:: 
.:: 
IsInRole:: 
(:: 
DoctorRoleName:: ,
)::, -
)::- .
{;; 
var<< 
appointments<<  
=<<! "
await<<# (
service<<) 0
.<<0 1+
GetMyAppointmentsForDoctorAsync<<1 P
(<<P Q
identityUserId<<Q _
)<<_ `
;<<` a
return>> 
Ok>> 
(>> 
appointments>> &
)>>& '
;>>' (
}?? 
returnAA 
ForbidAA 
(AA 
)AA 
;AA 
}BB 	
[EE 	
HttpGetEE	 
(EE 
$strEE 
)EE 
]EE  
[FF 	
	AuthorizeFF	 
(FF !
AuthenticationSchemesGG !
=GG" #
JwtBearerDefaultsGG$ 5
.GG5 6 
AuthenticationSchemeGG6 J
,GGJ K
RolesHH 
=HH 
$strHH $
)HH$ %
]HH% &
publicII 
asyncII 
TaskII 
<II 
IActionResultII '
>II' (%
GetMyUpcomingAppointmentsII) B
(IIB C
)IIC D
{JJ 	
varKK 
identityUserIdKK 
=KK  
UserKK! %
.KK% &
	FindFirstKK& /
(KK/ 0

ClaimTypesKK0 :
.KK: ;
NameIdentifierKK; I
)KKI J
?KKJ K
.KKK L
ValueKKL Q
;KKQ R
ifMM 
(MM 
stringMM 
.MM 
IsNullOrWhiteSpaceMM )
(MM) *
identityUserIdMM* 8
)MM8 9
)MM9 :
{NN 
returnOO 
UnauthorizedOO #
(OO# $
newOO$ '
{PP 
MessageQQ 
=QQ #
InvalidUserTokenMessageQQ 5
}RR 
)RR 
;RR 
}SS 
ifUU 
(UU 
UserUU 
.UU 
IsInRoleUU 
(UU 
PatientRoleNameUU -
)UU- .
)UU. /
{VV 
varWW 
appointmentsWW  
=WW! "
awaitWW# (
serviceWW) 0
.WW0 14
(GetMyUpcomingAppointmentsForPatientAsyncWW1 Y
(WWY Z
identityUserIdWWZ h
)WWh i
;WWi j
returnYY 
OkYY 
(YY 
appointmentsYY &
)YY& '
;YY' (
}ZZ 
if\\ 
(\\ 
User\\ 
.\\ 
IsInRole\\ 
(\\ 
DoctorRoleName\\ ,
)\\, -
)\\- .
{]] 
var^^ 
appointments^^  
=^^! "
await^^# (
service^^) 0
.^^0 13
'GetMyUpcomingAppointmentsForDoctorAsync^^1 X
(^^X Y
identityUserId^^Y g
)^^g h
;^^h i
return`` 
Ok`` 
(`` 
appointments`` &
)``& '
;``' (
}aa 
returncc 
Forbidcc 
(cc 
)cc 
;cc 
}dd 	
[gg 	
HttpGetgg	 
(gg 
$strgg 
)gg 
]gg 
[hh 	
	Authorizehh	 
(hh !
AuthenticationSchemesii !
=ii" #
JwtBearerDefaultsii$ 5
.ii5 6 
AuthenticationSchemeii6 J
,iiJ K
Rolesjj 
=jj 
$strjj $
)jj$ %
]jj% &
publickk 
asynckk 
Taskkk 
<kk 
IActionResultkk '
>kk' ($
GetMyPendingAppointmentskk) A
(kkA B
)kkB C
{ll 	
varmm 
identityUserIdmm 
=mm  
Usermm! %
.mm% &
	FindFirstmm& /
(mm/ 0

ClaimTypesmm0 :
.mm: ;
NameIdentifiermm; I
)mmI J
?mmJ K
.mmK L
ValuemmL Q
;mmQ R
ifoo 
(oo 
stringoo 
.oo 
IsNullOrWhiteSpaceoo )
(oo) *
identityUserIdoo* 8
)oo8 9
)oo9 :
{pp 
returnqq 
Unauthorizedqq #
(qq# $
newqq$ '
{rr 
Messagess 
=ss #
InvalidUserTokenMessagess 5
}tt 
)tt 
;tt 
}uu 
ifww 
(ww 
Userww 
.ww 
IsInRoleww 
(ww 
PatientRoleNameww -
)ww- .
)ww. /
{xx 
varyy 
appointmentsyy  
=yy! "
awaityy# (
serviceyy) 0
.yy0 13
'GetMyPendingAppointmentsForPatientAsyncyy1 X
(yyX Y
identityUserIdyyY g
)yyg h
;yyh i
return{{ 
Ok{{ 
({{ 
appointments{{ &
){{& '
;{{' (
}|| 
if~~ 
(~~ 
User~~ 
.~~ 
IsInRole~~ 
(~~ 
DoctorRoleName~~ ,
)~~, -
)~~- .
{ 
var
ÄÄ 
appointments
ÄÄ  
=
ÄÄ! "
await
ÄÄ# (
service
ÄÄ) 0
.
ÄÄ0 14
&GetMyPendingAppointmentsForDoctorAsync
ÄÄ1 W
(
ÄÄW X
identityUserId
ÄÄX f
)
ÄÄf g
;
ÄÄg h
return
ÇÇ 
Ok
ÇÇ 
(
ÇÇ 
appointments
ÇÇ &
)
ÇÇ& '
;
ÇÇ' (
}
ÉÉ 
return
ÖÖ 
Forbid
ÖÖ 
(
ÖÖ 
)
ÖÖ 
;
ÖÖ 
}
ÜÜ 	
[
ââ 	
HttpGet
ââ	 
(
ââ 
$str
ââ %
)
ââ% &
]
ââ& '
[
ää 	
	Authorize
ää	 
(
ää #
AuthenticationSchemes
ãã !
=
ãã" #
JwtBearerDefaults
ãã$ 5
.
ãã5 6"
AuthenticationScheme
ãã6 J
,
ããJ K
Roles
åå 
=
åå 
$str
åå 
)
åå 
]
åå 
public
çç 
async
çç 
Task
çç 
<
çç 
IActionResult
çç '
>
çç' (-
GetMyTodayConfirmedAppointments
çç) H
(
ççH I
)
ççI J
{
éé 	
var
èè 
identityUserId
èè 
=
èè  
User
èè! %
.
èè% &
	FindFirst
èè& /
(
èè/ 0

ClaimTypes
èè0 :
.
èè: ;
NameIdentifier
èè; I
)
èèI J
?
èèJ K
.
èèK L
Value
èèL Q
;
èèQ R
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
ëë) *
identityUserId
ëë* 8
)
ëë8 9
)
ëë9 :
{
íí 
return
ìì 
Unauthorized
ìì #
(
ìì# $
new
ìì$ '
{
îî 
Message
ïï 
=
ïï %
InvalidUserTokenMessage
ïï 5
}
ññ 
)
ññ 
;
ññ 
}
óó 
var
ôô 
appointments
ôô 
=
ôô 
await
ôô $
service
ôô% ,
.
ôô, -;
-GetMyTodayConfirmedAppointmentsForDoctorAsync
ôô- Z
(
ôôZ [
identityUserId
ôô[ i
)
ôôi j
;
ôôj k
return
õõ 
Ok
õõ 
(
õõ 
appointments
õõ "
)
õõ" #
;
õõ# $
}
úú 	
[
üü 	
HttpGet
üü	 
(
üü 
$str
üü &
)
üü& '
]
üü' (
[
†† 	
	Authorize
††	 
(
†† #
AuthenticationSchemes
°° !
=
°°" #
JwtBearerDefaults
°°$ 5
.
°°5 6"
AuthenticationScheme
°°6 J
,
°°J K
Roles
¢¢ 
=
¢¢ 
$str
¢¢ *
)
¢¢* +
]
¢¢+ ,
public
££ 
async
££ 
Task
££ 
<
££ 
IActionResult
££ '
>
££' ( 
GetAppointmentById
££) ;
(
££; <
[
££< =
	FromRoute
££= F
]
££F G
int
££H K
appointmentId
££L Y
)
££Y Z
{
§§ 	
if
•• 
(
•• 
User
•• 
.
•• 
IsInRole
•• 
(
•• 
$str
•• %
)
••% &
)
••& '
{
¶¶ 
var
ßß 
appointment
ßß 
=
ßß  !
await
ßß" '
service
ßß( /
.
ßß/ 0%
GetAppointmentByIdAsync
ßß0 G
(
ßßG H
appointmentId
ßßH U
)
ßßU V
;
ßßV W
return
©© 
Ok
©© 
(
©© 
appointment
©© %
)
©©% &
;
©©& '
}
™™ 
if
¨¨ 
(
¨¨ 
User
¨¨ 
.
¨¨ 
IsInRole
¨¨ 
(
¨¨ 
PatientRoleName
¨¨ -
)
¨¨- .
)
¨¨. /
{
≠≠ 
var
ÆÆ 
identityUserId
ÆÆ "
=
ÆÆ# $
User
ÆÆ% )
.
ÆÆ) *
	FindFirst
ÆÆ* 3
(
ÆÆ3 4

ClaimTypes
ÆÆ4 >
.
ÆÆ> ?
NameIdentifier
ÆÆ? M
)
ÆÆM N
?
ÆÆN O
.
ÆÆO P
Value
ÆÆP U
;
ÆÆU V
if
∞∞ 
(
∞∞ 
string
∞∞ 
.
∞∞  
IsNullOrWhiteSpace
∞∞ -
(
∞∞- .
identityUserId
∞∞. <
)
∞∞< =
)
∞∞= >
{
±± 
return
≤≤ 
Unauthorized
≤≤ '
(
≤≤' (
new
≤≤( +
{
≥≥ 
Message
¥¥ 
=
¥¥  !%
InvalidUserTokenMessage
¥¥" 9
}
µµ 
)
µµ 
;
µµ 
}
∂∂ 
var
∏∏ 
appointment
∏∏ 
=
∏∏  !
await
∏∏" '
service
∏∏( /
.
∏∏/ 0/
!GetAppointmentByIdForPatientAsync
∏∏0 Q
(
∏∏Q R
appointmentId
ππ !
,
ππ! "
identityUserId
∫∫ "
)
∫∫" #
;
∫∫# $
return
ºº 
Ok
ºº 
(
ºº 
appointment
ºº %
)
ºº% &
;
ºº& '
}
ΩΩ 
if
øø 
(
øø 
User
øø 
.
øø 
IsInRole
øø 
(
øø 
DoctorRoleName
øø ,
)
øø, -
)
øø- .
{
¿¿ 
var
¡¡ 
identityUserId
¡¡ "
=
¡¡# $
User
¡¡% )
.
¡¡) *
	FindFirst
¡¡* 3
(
¡¡3 4

ClaimTypes
¡¡4 >
.
¡¡> ?
NameIdentifier
¡¡? M
)
¡¡M N
?
¡¡N O
.
¡¡O P
Value
¡¡P U
;
¡¡U V
if
√√ 
(
√√ 
string
√√ 
.
√√  
IsNullOrWhiteSpace
√√ -
(
√√- .
identityUserId
√√. <
)
√√< =
)
√√= >
{
ƒƒ 
return
≈≈ 
Unauthorized
≈≈ '
(
≈≈' (
new
≈≈( +
{
∆∆ 
Message
«« 
=
««  !%
InvalidUserTokenMessage
««" 9
}
»» 
)
»» 
;
»» 
}
…… 
var
ÀÀ 
appointment
ÀÀ 
=
ÀÀ  !
await
ÀÀ" '
service
ÀÀ( /
.
ÀÀ/ 0.
 GetAppointmentByIdForDoctorAsync
ÀÀ0 P
(
ÀÀP Q
appointmentId
ÃÃ !
,
ÃÃ! "
identityUserId
ÕÕ "
)
ÕÕ" #
;
ÕÕ# $
return
œœ 
Ok
œœ 
(
œœ 
appointment
œœ %
)
œœ% &
;
œœ& '
}
–– 
return
““ 
Forbid
““ 
(
““ 
)
““ 
;
““ 
}
”” 	
[
◊◊ 	
HttpGet
◊◊	 
(
◊◊ 
$str
◊◊ *
)
◊◊* +
]
◊◊+ ,
[
ÿÿ 	
	Authorize
ÿÿ	 
(
ÿÿ #
AuthenticationSchemes
ŸŸ !
=
ŸŸ" #
JwtBearerDefaults
ŸŸ$ 5
.
ŸŸ5 6"
AuthenticationScheme
ŸŸ6 J
,
ŸŸJ K
Roles
⁄⁄ 
=
⁄⁄ 
$str
⁄⁄ 
)
⁄⁄ 
]
⁄⁄ 
public
€€ 
async
€€ 
Task
€€ 
<
€€ 
IActionResult
€€ '
>
€€' ((
GetAppointmentsByPatientId
€€) C
(
€€C D
[
€€D E
	FromRoute
€€E N
]
€€N O
int
€€P S
	patientId
€€T ]
)
€€] ^
{
‹‹ 	
var
›› 
appointments
›› 
=
›› 
await
›› $
service
››% ,
.
››, --
GetAppointmentsByPatientIdAsync
››- L
(
››L M
	patientId
››M V
)
››V W
;
››W X
return
ﬂﬂ 
Ok
ﬂﬂ 
(
ﬂﬂ 
appointments
ﬂﬂ "
)
ﬂﬂ" #
;
ﬂﬂ# $
}
‡‡ 	
[
‰‰ 	
HttpGet
‰‰	 
(
‰‰ 
$str
‰‰ (
)
‰‰( )
]
‰‰) *
[
ÂÂ 	
	Authorize
ÂÂ	 
(
ÂÂ #
AuthenticationSchemes
ÊÊ !
=
ÊÊ" #
JwtBearerDefaults
ÊÊ$ 5
.
ÊÊ5 6"
AuthenticationScheme
ÊÊ6 J
,
ÊÊJ K
Roles
ÁÁ 
=
ÁÁ 
$str
ÁÁ 
)
ÁÁ 
]
ÁÁ 
public
ËË 
async
ËË 
Task
ËË 
<
ËË 
IActionResult
ËË '
>
ËË' ('
GetAppointmentsByDoctorId
ËË) B
(
ËËB C
[
ËËC D
	FromRoute
ËËD M
]
ËËM N
int
ËËO R
doctorId
ËËS [
)
ËË[ \
{
ÈÈ 	
var
ÍÍ 
appointments
ÍÍ 
=
ÍÍ 
await
ÍÍ $
service
ÍÍ% ,
.
ÍÍ, -,
GetAppointmentsByDoctorIdAsync
ÍÍ- K
(
ÍÍK L
doctorId
ÍÍL T
)
ÍÍT U
;
ÍÍU V
return
ÏÏ 
Ok
ÏÏ 
(
ÏÏ 
appointments
ÏÏ "
)
ÏÏ" #
;
ÏÏ# $
}
ÌÌ 	
[
 	
HttpGet
	 
(
 
$str
 "
)
" #
]
# $
[
ÒÒ 	
	Authorize
ÒÒ	 
(
ÒÒ #
AuthenticationSchemes
ÚÚ !
=
ÚÚ" #
JwtBearerDefaults
ÚÚ$ 5
.
ÚÚ5 6"
AuthenticationScheme
ÚÚ6 J
,
ÚÚJ K
Roles
ÛÛ 
=
ÛÛ 
$str
ÛÛ 
)
ÛÛ 
]
ÛÛ 
public
ÙÙ 
async
ÙÙ 
Task
ÙÙ 
<
ÙÙ 
IActionResult
ÙÙ '
>
ÙÙ' (%
GetAppointmentsByStatus
ÙÙ) @
(
ÙÙ@ A
[
ÙÙA B
	FromRoute
ÙÙB K
]
ÙÙK L
AppointmentStatus
ÙÙM ^
status
ÙÙ_ e
)
ÙÙe f
{
ıı 	
var
ˆˆ 
appointments
ˆˆ 
=
ˆˆ 
await
ˆˆ $
service
ˆˆ% ,
.
ˆˆ, -*
GetAppointmentsByStatusAsync
ˆˆ- I
(
ˆˆI J
status
ˆˆJ P
)
ˆˆP Q
;
ˆˆQ R
return
¯¯ 
Ok
¯¯ 
(
¯¯ 
appointments
¯¯ "
)
¯¯" #
;
¯¯# $
}
˘˘ 	
[
¸¸ 	
HttpGet
¸¸	 
(
¸¸ 
$str
¸¸ 
)
¸¸ 
]
¸¸ 
[
˝˝ 	
	Authorize
˝˝	 
(
˝˝ #
AuthenticationSchemes
˛˛ !
=
˛˛" #
JwtBearerDefaults
˛˛$ 5
.
˛˛5 6"
AuthenticationScheme
˛˛6 J
,
˛˛J K
Roles
ˇˇ 
=
ˇˇ 
$str
ˇˇ 
)
ˇˇ 
]
ˇˇ 
public
ÄÄ 
async
ÄÄ 
Task
ÄÄ 
<
ÄÄ 
IActionResult
ÄÄ '
>
ÄÄ' (%
GetUpcomingAppointments
ÄÄ) @
(
ÄÄ@ A
)
ÄÄA B
{
ÅÅ 	
var
ÇÇ 
appointments
ÇÇ 
=
ÇÇ 
await
ÇÇ $
service
ÇÇ% ,
.
ÇÇ, -*
GetUpcomingAppointmentsAsync
ÇÇ- I
(
ÇÇI J
)
ÇÇJ K
;
ÇÇK L
return
ÑÑ 
Ok
ÑÑ 
(
ÑÑ 
appointments
ÑÑ "
)
ÑÑ" #
;
ÑÑ# $
}
ÖÖ 	
[
ââ 	
HttpGet
ââ	 
(
ââ 
$str
ââ 3
)
ââ3 4
]
ââ4 5
[
ää 	
	Authorize
ää	 
(
ää #
AuthenticationSchemes
ãã !
=
ãã" #
JwtBearerDefaults
ãã$ 5
.
ãã5 6"
AuthenticationScheme
ãã6 J
,
ããJ K
Roles
åå 
=
åå 
$str
åå 
)
åå 
]
åå 
public
çç 
async
çç 
Task
çç 
<
çç 
IActionResult
çç '
>
çç' (0
"GetUpcomingAppointmentsByPatientId
çç) K
(
ççK L
[
ççL M
	FromRoute
ççM V
]
ççV W
int
ççX [
	patientId
çç\ e
)
ççe f
{
éé 	
var
èè 
appointments
èè 
=
èè 
await
èè $
service
èè% ,
.
èè, -5
'GetUpcomingAppointmentsByPatientIdAsync
èè- T
(
èèT U
	patientId
èèU ^
)
èè^ _
;
èè_ `
return
ëë 
Ok
ëë 
(
ëë 
appointments
ëë "
)
ëë" #
;
ëë# $
}
íí 	
[
ññ 	
HttpGet
ññ	 
(
ññ 
$str
ññ 1
)
ññ1 2
]
ññ2 3
[
óó 	
	Authorize
óó	 
(
óó #
AuthenticationSchemes
òò !
=
òò" #
JwtBearerDefaults
òò$ 5
.
òò5 6"
AuthenticationScheme
òò6 J
,
òòJ K
Roles
ôô 
=
ôô 
$str
ôô 
)
ôô 
]
ôô 
public
öö 
async
öö 
Task
öö 
<
öö 
IActionResult
öö '
>
öö' (/
!GetUpcomingAppointmentsByDoctorId
öö) J
(
ööJ K
[
ööK L
	FromRoute
ööL U
]
ööU V
int
ööW Z
doctorId
öö[ c
)
ööc d
{
õõ 	
var
úú 
appointments
úú 
=
úú 
await
úú $
service
úú% ,
.
úú, -4
&GetUpcomingAppointmentsByDoctorIdAsync
úú- S
(
úúS T
doctorId
úúT \
)
úú\ ]
;
úú] ^
return
ûû 
Ok
ûû 
(
ûû 
appointments
ûû "
)
ûû" #
;
ûû# $
}
üü 	
[
££ 	
HttpGet
££	 
(
££ 
$str
££ 2
)
££2 3
]
££3 4
[
§§ 	
	Authorize
§§	 
(
§§ #
AuthenticationSchemes
•• !
=
••" #
JwtBearerDefaults
••$ 5
.
••5 6"
AuthenticationScheme
••6 J
,
••J K
Roles
¶¶ 
=
¶¶ 
$str
¶¶ 
)
¶¶ 
]
¶¶ 
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
IActionResult
ßß '
>
ßß' (/
!GetPendingAppointmentsByPatientId
ßß) J
(
ßßJ K
[
ßßK L
	FromRoute
ßßL U
]
ßßU V
int
ßßW Z
	patientId
ßß[ d
)
ßßd e
{
®® 	
var
©© 
appointments
©© 
=
©© 
await
©© $
service
©©% ,
.
©©, -4
&GetPendingAppointmentsByPatientIdAsync
©©- S
(
©©S T
	patientId
©©T ]
)
©©] ^
;
©©^ _
return
´´ 
Ok
´´ 
(
´´ 
appointments
´´ "
)
´´" #
;
´´# $
}
¨¨ 	
[
∞∞ 	
HttpGet
∞∞	 
(
∞∞ 
$str
∞∞ 0
)
∞∞0 1
]
∞∞1 2
[
±± 	
	Authorize
±±	 
(
±± #
AuthenticationSchemes
≤≤ !
=
≤≤" #
JwtBearerDefaults
≤≤$ 5
.
≤≤5 6"
AuthenticationScheme
≤≤6 J
,
≤≤J K
Roles
≥≥ 
=
≥≥ 
$str
≥≥ 
)
≥≥ 
]
≥≥ 
public
¥¥ 
async
¥¥ 
Task
¥¥ 
<
¥¥ 
IActionResult
¥¥ '
>
¥¥' (.
 GetPendingAppointmentsByDoctorId
¥¥) I
(
¥¥I J
[
¥¥J K
	FromRoute
¥¥K T
]
¥¥T U
int
¥¥V Y
doctorId
¥¥Z b
)
¥¥b c
{
µµ 	
var
∂∂ 
appointments
∂∂ 
=
∂∂ 
await
∂∂ $
service
∂∂% ,
.
∂∂, -3
%GetPendingAppointmentsByDoctorIdAsync
∂∂- R
(
∂∂R S
doctorId
∂∂S [
)
∂∂[ \
;
∂∂\ ]
return
∏∏ 
Ok
∏∏ 
(
∏∏ 
appointments
∏∏ "
)
∏∏" #
;
∏∏# $
}
ππ 	
[
ΩΩ 	
HttpGet
ΩΩ	 
(
ΩΩ 
$str
ΩΩ 8
)
ΩΩ8 9
]
ΩΩ9 :
[
ææ 	
	Authorize
ææ	 
(
ææ #
AuthenticationSchemes
øø !
=
øø" #
JwtBearerDefaults
øø$ 5
.
øø5 6"
AuthenticationScheme
øø6 J
,
øøJ K
Roles
¿¿ 
=
¿¿ 
$str
¿¿ 
)
¿¿ 
]
¿¿ 
public
¡¡ 
async
¡¡ 
Task
¡¡ 
<
¡¡ 
IActionResult
¡¡ '
>
¡¡' (5
'GetTodayConfirmedAppointmentsByDoctorId
¡¡) P
(
¡¡P Q
[
¡¡Q R
	FromRoute
¡¡R [
]
¡¡[ \
int
¡¡] `
doctorId
¡¡a i
)
¡¡i j
{
¬¬ 	
var
√√ 
appointments
√√ 
=
√√ 
await
√√ $
service
√√% ,
.
√√, -:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
√√- Y
(
√√Y Z
doctorId
√√Z b
)
√√b c
;
√√c d
return
≈≈ 
Ok
≈≈ 
(
≈≈ 
appointments
≈≈ "
)
≈≈" #
;
≈≈# $
}
∆∆ 	
[
   	
HttpPost
  	 
]
   
[
ÀÀ 	
	Authorize
ÀÀ	 
(
ÀÀ #
AuthenticationSchemes
ÃÃ !
=
ÃÃ" #
JwtBearerDefaults
ÃÃ$ 5
.
ÃÃ5 6"
AuthenticationScheme
ÃÃ6 J
,
ÃÃJ K
Roles
ÕÕ 
=
ÕÕ 
$str
ÕÕ 
)
ÕÕ 
]
ÕÕ 
public
ŒŒ 
async
ŒŒ 
Task
ŒŒ 
<
ŒŒ 
IActionResult
ŒŒ '
>
ŒŒ' (
BookAppointment
ŒŒ) 8
(
ŒŒ8 9
[
ŒŒ9 :
FromBody
ŒŒ: B
]
ŒŒB C 
BookAppointmentDto
ŒŒD V
request
ŒŒW ^
)
ŒŒ^ _
{
œœ 	
var
–– 
identityUserId
–– 
=
––  
User
––! %
.
––% &
	FindFirst
––& /
(
––/ 0

ClaimTypes
––0 :
.
––: ;
NameIdentifier
––; I
)
––I J
?
––J K
.
––K L
Value
––L Q
;
––Q R
if
““ 
(
““ 
string
““ 
.
““  
IsNullOrWhiteSpace
““ )
(
““) *
identityUserId
““* 8
)
““8 9
)
““9 :
{
”” 
return
‘‘ 
Unauthorized
‘‘ #
(
‘‘# $
new
‘‘$ '
{
’’ 
Message
÷÷ 
=
÷÷ %
InvalidUserTokenMessage
÷÷ 5
}
◊◊ 
)
◊◊ 
;
◊◊ 
}
ÿÿ 
var
⁄⁄ 
appointment
⁄⁄ 
=
⁄⁄ 
await
⁄⁄ #
service
⁄⁄$ +
.
⁄⁄+ ,,
BookAppointmentForPatientAsync
⁄⁄, J
(
⁄⁄J K
request
€€ 
,
€€ 
identityUserId
‹‹ 
)
‹‹ 
;
‹‹  
return
ﬁﬁ 
CreatedAtAction
ﬁﬁ "
(
ﬁﬁ" #
nameof
ﬂﬂ 
(
ﬂﬂ  
GetAppointmentById
ﬂﬂ )
)
ﬂﬂ) *
,
ﬂﬂ* +
new
‡‡ 
{
‡‡ 
appointmentId
‡‡ #
=
‡‡$ %
appointment
‡‡& 1
.
‡‡1 2
AppointmentId
‡‡2 ?
}
‡‡@ A
,
‡‡A B
appointment
·· 
)
·· 
;
·· 
}
‚‚ 	
[
ÂÂ 	
HttpPut
ÂÂ	 
(
ÂÂ 
$str
ÂÂ &
)
ÂÂ& '
]
ÂÂ' (
[
ÊÊ 	
	Authorize
ÊÊ	 
(
ÊÊ #
AuthenticationSchemes
ÁÁ !
=
ÁÁ" #
JwtBearerDefaults
ÁÁ$ 5
.
ÁÁ5 6"
AuthenticationScheme
ÁÁ6 J
,
ÁÁJ K
Roles
ËË 
=
ËË 
$str
ËË 
)
ËË 
]
ËË 
public
ÈÈ 
async
ÈÈ 
Task
ÈÈ 
<
ÈÈ 
IActionResult
ÈÈ '
>
ÈÈ' (
UpdateAppointment
ÈÈ) :
(
ÈÈ: ;
[
ÍÍ 
	FromRoute
ÍÍ 
]
ÍÍ 
int
ÍÍ 
appointmentId
ÍÍ )
,
ÍÍ) *
[
ÎÎ 
FromBody
ÎÎ 
]
ÎÎ "
UpdateAppointmentDto
ÎÎ +
request
ÎÎ, 3
)
ÎÎ3 4
{
ÏÏ 	
var
ÌÌ 
appointment
ÌÌ 
=
ÌÌ 
await
ÌÌ #
service
ÌÌ$ +
.
ÌÌ+ ,$
UpdateAppointmentAsync
ÌÌ, B
(
ÌÌB C
appointmentId
ÌÌC P
,
ÌÌP Q
request
ÌÌR Y
)
ÌÌY Z
;
ÌÌZ [
return
ÔÔ 
Ok
ÔÔ 
(
ÔÔ 
appointment
ÔÔ !
)
ÔÔ! "
;
ÔÔ" #
}
 	
[
ÛÛ 	
HttpPut
ÛÛ	 
(
ÛÛ 
$str
ÛÛ .
)
ÛÛ. /
]
ÛÛ/ 0
[
ÙÙ 	
	Authorize
ÙÙ	 
(
ÙÙ #
AuthenticationSchemes
ıı !
=
ıı" #
JwtBearerDefaults
ıı$ 5
.
ıı5 6"
AuthenticationScheme
ıı6 J
,
ııJ K
Roles
ˆˆ 
=
ˆˆ 
$str
ˆˆ 
)
ˆˆ 
]
ˆˆ 
public
˜˜ 
async
˜˜ 
Task
˜˜ 
<
˜˜ 
IActionResult
˜˜ '
>
˜˜' ( 
ConfirmAppointment
˜˜) ;
(
˜˜; <
[
˜˜< =
	FromRoute
˜˜= F
]
˜˜F G
int
˜˜H K
appointmentId
˜˜L Y
)
˜˜Y Z
{
¯¯ 	
var
˘˘ 
identityUserId
˘˘ 
=
˘˘  
User
˘˘! %
.
˘˘% &
	FindFirst
˘˘& /
(
˘˘/ 0

ClaimTypes
˘˘0 :
.
˘˘: ;
NameIdentifier
˘˘; I
)
˘˘I J
?
˘˘J K
.
˘˘K L
Value
˘˘L Q
;
˘˘Q R
if
˚˚ 
(
˚˚ 
string
˚˚ 
.
˚˚  
IsNullOrWhiteSpace
˚˚ )
(
˚˚) *
identityUserId
˚˚* 8
)
˚˚8 9
)
˚˚9 :
{
¸¸ 
return
˝˝ 
Unauthorized
˝˝ #
(
˝˝# $
new
˝˝$ '
{
˛˛ 
Message
ˇˇ 
=
ˇˇ %
InvalidUserTokenMessage
ˇˇ 5
}
ÄÄ 
)
ÄÄ 
;
ÄÄ 
}
ÅÅ 
var
ÉÉ 
appointment
ÉÉ 
=
ÉÉ 
await
ÉÉ #
service
ÉÉ$ +
.
ÉÉ+ ,.
 ConfirmAppointmentForDoctorAsync
ÉÉ, L
(
ÉÉL M
appointmentId
ÑÑ 
,
ÑÑ 
identityUserId
ÖÖ 
)
ÖÖ 
;
ÖÖ  
return
áá 
Ok
áá 
(
áá 
appointment
áá !
)
áá! "
;
áá" #
}
àà 	
[
ãã 	
HttpPut
ãã	 
(
ãã 
$str
ãã /
)
ãã/ 0
]
ãã0 1
[
åå 	
	Authorize
åå	 
(
åå #
AuthenticationSchemes
çç !
=
çç" #
JwtBearerDefaults
çç$ 5
.
çç5 6"
AuthenticationScheme
çç6 J
,
ççJ K
Roles
éé 
=
éé 
$str
éé 
)
éé 
]
éé 
public
èè 
async
èè 
Task
èè 
<
èè 
IActionResult
èè '
>
èè' (!
CompleteAppointment
èè) <
(
èè< =
[
èè= >
	FromRoute
èè> G
]
èèG H
int
èèI L
appointmentId
èèM Z
)
èèZ [
{
êê 	
var
ëë 
identityUserId
ëë 
=
ëë  
User
ëë! %
.
ëë% &
	FindFirst
ëë& /
(
ëë/ 0

ClaimTypes
ëë0 :
.
ëë: ;
NameIdentifier
ëë; I
)
ëëI J
?
ëëJ K
.
ëëK L
Value
ëëL Q
;
ëëQ R
if
ìì 
(
ìì 
string
ìì 
.
ìì  
IsNullOrWhiteSpace
ìì )
(
ìì) *
identityUserId
ìì* 8
)
ìì8 9
)
ìì9 :
{
îî 
return
ïï 
Unauthorized
ïï #
(
ïï# $
new
ïï$ '
{
ññ 
Message
óó 
=
óó %
InvalidUserTokenMessage
óó 5
}
òò 
)
òò 
;
òò 
}
ôô 
var
õõ 
appointment
õõ 
=
õõ 
await
õõ #
service
õõ$ +
.
õõ+ ,/
!CompleteAppointmentForDoctorAsync
õõ, M
(
õõM N
appointmentId
úú 
,
úú 
identityUserId
ùù 
)
ùù 
;
ùù  
return
üü 
Ok
üü 
(
üü 
appointment
üü !
)
üü! "
;
üü" #
}
†† 	
[
§§ 	
HttpPut
§§	 
(
§§ 
$str
§§ 
)
§§ 
]
§§ 
[
•• 	
	Authorize
••	 
(
•• #
AuthenticationSchemes
¶¶ !
=
¶¶" #
JwtBearerDefaults
¶¶$ 5
.
¶¶5 6"
AuthenticationScheme
¶¶6 J
,
¶¶J K
Roles
ßß 
=
ßß 
$str
ßß *
)
ßß* +
]
ßß+ ,
public
®® 
async
®® 
Task
®® 
<
®® 
IActionResult
®® '
>
®®' (
CancelAppointment
®®) :
(
®®: ;
[
®®; <
FromBody
®®< D
]
®®D E"
CancelAppointmentDto
®®F Z
request
®®[ b
)
®®b c
{
©© 	
if
™™ 
(
™™ 
User
™™ 
.
™™ 
IsInRole
™™ 
(
™™ 
$str
™™ %
)
™™% &
)
™™& '
{
´´ 
var
¨¨ 
appointment
¨¨ 
=
¨¨  !
await
¨¨" '
service
¨¨( /
.
¨¨/ 0$
CancelAppointmentAsync
¨¨0 F
(
¨¨F G
request
¨¨G N
)
¨¨N O
;
¨¨O P
return
ÆÆ 
Ok
ÆÆ 
(
ÆÆ 
appointment
ÆÆ %
)
ÆÆ% &
;
ÆÆ& '
}
ØØ 
if
±± 
(
±± 
User
±± 
.
±± 
IsInRole
±± 
(
±± 
PatientRoleName
±± -
)
±±- .
)
±±. /
{
≤≤ 
var
≥≥ 
identityUserId
≥≥ "
=
≥≥# $
User
≥≥% )
.
≥≥) *
	FindFirst
≥≥* 3
(
≥≥3 4

ClaimTypes
≥≥4 >
.
≥≥> ?
NameIdentifier
≥≥? M
)
≥≥M N
?
≥≥N O
.
≥≥O P
Value
≥≥P U
;
≥≥U V
if
µµ 
(
µµ 
string
µµ 
.
µµ  
IsNullOrWhiteSpace
µµ -
(
µµ- .
identityUserId
µµ. <
)
µµ< =
)
µµ= >
{
∂∂ 
return
∑∑ 
Unauthorized
∑∑ '
(
∑∑' (
new
∑∑( +
{
∏∏ 
Message
ππ 
=
ππ  !%
InvalidUserTokenMessage
ππ" 9
}
∫∫ 
)
∫∫ 
;
∫∫ 
}
ªª 
var
ΩΩ 
appointment
ΩΩ 
=
ΩΩ  !
await
ΩΩ" '
service
ΩΩ( /
.
ΩΩ/ 0.
 CancelAppointmentForPatientAsync
ΩΩ0 P
(
ΩΩP Q
request
ææ 
,
ææ 
identityUserId
øø "
)
øø" #
;
øø# $
return
¡¡ 
Ok
¡¡ 
(
¡¡ 
appointment
¡¡ %
)
¡¡% &
;
¡¡& '
}
¬¬ 
if
ƒƒ 
(
ƒƒ 
User
ƒƒ 
.
ƒƒ 
IsInRole
ƒƒ 
(
ƒƒ 
DoctorRoleName
ƒƒ ,
)
ƒƒ, -
)
ƒƒ- .
{
≈≈ 
var
∆∆ 
identityUserId
∆∆ "
=
∆∆# $
User
∆∆% )
.
∆∆) *
	FindFirst
∆∆* 3
(
∆∆3 4

ClaimTypes
∆∆4 >
.
∆∆> ?
NameIdentifier
∆∆? M
)
∆∆M N
?
∆∆N O
.
∆∆O P
Value
∆∆P U
;
∆∆U V
if
»» 
(
»» 
string
»» 
.
»»  
IsNullOrWhiteSpace
»» -
(
»»- .
identityUserId
»». <
)
»»< =
)
»»= >
{
…… 
return
   
Unauthorized
   '
(
  ' (
new
  ( +
{
ÀÀ 
Message
ÃÃ 
=
ÃÃ  !%
InvalidUserTokenMessage
ÃÃ" 9
}
ÕÕ 
)
ÕÕ 
;
ÕÕ 
}
ŒŒ 
var
–– 
appointment
–– 
=
––  !
await
––" '
service
––( /
.
––/ 0-
CancelAppointmentForDoctorAsync
––0 O
(
––O P
request
—— 
,
—— 
identityUserId
““ "
)
““" #
;
““# $
return
‘‘ 
Ok
‘‘ 
(
‘‘ 
appointment
‘‘ %
)
‘‘% &
;
‘‘& '
}
’’ 
return
◊◊ 
Forbid
◊◊ 
(
◊◊ 
)
◊◊ 
;
◊◊ 
}
ÿÿ 	
[
€€ 	

HttpDelete
€€	 
(
€€ 
$str
€€ )
)
€€) *
]
€€* +
[
‹‹ 	
	Authorize
‹‹	 
(
‹‹ #
AuthenticationSchemes
›› !
=
››" #
JwtBearerDefaults
››$ 5
.
››5 6"
AuthenticationScheme
››6 J
,
››J K
Roles
ﬁﬁ 
=
ﬁﬁ 
$str
ﬁﬁ 
)
ﬁﬁ 
]
ﬁﬁ 
public
ﬂﬂ 
async
ﬂﬂ 
Task
ﬂﬂ 
<
ﬂﬂ 
IActionResult
ﬂﬂ '
>
ﬂﬂ' (
DeleteAppointment
ﬂﬂ) :
(
ﬂﬂ: ;
[
ﬂﬂ; <
	FromRoute
ﬂﬂ< E
]
ﬂﬂE F
int
ﬂﬂG J
appointmentId
ﬂﬂK X
)
ﬂﬂX Y
{
‡‡ 	
var
·· 
appointment
·· 
=
·· 
await
·· #
service
··$ +
.
··+ ,$
DeleteAppointmentAsync
··, B
(
··B C
appointmentId
··C P
)
··P Q
;
··Q R
return
„„ 
Ok
„„ 
(
„„ 
appointment
„„ !
)
„„! "
;
„„" #
}
‰‰ 	
}
ÂÂ 
}ÊÊ ß
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\AdminController.cs
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
Controllers

 #
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
( !
AuthenticationSchemes 
= 
JwtBearerDefaults  1
.1 2 
AuthenticationScheme2 F
,F G
Roles 
= 
$str 
) 
] 
public 

class 
AdminController  
(  !
IDoctorService! /
doctorService0 =
)= >
:? @
ControllerBaseA O
{ 
[ 	
HttpPost	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
CreateDoctor) 5
(5 6
[6 7
FromBody7 ?
]? @
CreateDoctorDtoA P
requestQ X
)X Y
{ 	
var 
result 
= 
await 
doctorService ,
., -$
CreateDoctorByAdminAsync- E
(E F
requestF M
)M N
;N O
return 
Ok 
( 
result 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAllDoctors) 6
(6 7
[7 8
	FromQuery8 A
]A B$
DoctorPaginationQueryDtoC [
query\ a
)a b
{ 	
var 
result 
= 
await 
doctorService ,
., -#
GetAllDoctorsPagedAsync- D
(D E
queryE J
)J K
;K L
return   
Ok   
(   
result   
)   
;   
}!! 	
[## 	
HttpPut##	 
(## 
$str## )
)##) *
]##* +
public$$ 
async$$ 
Task$$ 
<$$ 
IActionResult$$ '
>$$' (
UpdateDoctor$$) 5
($$5 6
[%% 
	FromRoute%% 
]%% 
int%% 
doctorId%% $
,%%$ %
[&& 
FromBody&& 
]&& 
UpdateDoctorDto&& &
request&&' .
)&&. /
{'' 	
var(( 
result(( 
=(( 
await(( 
doctorService(( ,
.((, -
UpdateDoctorAsync((- >
(((> ?
doctorId((? G
,((G H
request((I P
)((P Q
;((Q R
return** 
Ok** 
(** 
result** 
)** 
;** 
}++ 	
[-- 	

HttpDelete--	 
(-- 
$str-- ,
)--, -
]--- .
public.. 
async.. 
Task.. 
<.. 
IActionResult.. '
>..' (
DeleteDoctor..) 5
(..5 6
[..6 7
	FromRoute..7 @
]..@ A
int..B E
doctorId..F N
)..N O
{// 	
var00 
result00 
=00 
await00 
doctorService00 ,
.00, -
DeleteDoctorAsync00- >
(00> ?
doctorId00? G
)00G H
;00H I
return22 
Ok22 
(22 
result22 
)22 
;22 
}33 	
}44 
}55 