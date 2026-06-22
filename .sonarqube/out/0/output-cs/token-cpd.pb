⁄
gC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IPatientService.cs
	namespace 	
HealthCareApp
 
. 
Services  
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
> 
GetAllPatientsAsync 2
(2 3
)3 4
;4 5
Task		 
<		 
PagedResponse		 
<		 

PatientDto		 %
>		% &
>		& '$
GetAllPatientsPagedAsync		( @
(		@ A%
PatientPaginationQueryDto		A Z
query		[ `
)		` a
;		a b
Task 
< 

PatientDto 
> 
GetPatientByIdAsync ,
(, -
int- 0
	patientId1 :
): ;
;; <
Task 
< 

PatientDto 
>  
RegisterPatientAsync -
(- .
CreatePatientDto. >
dto? B
)B C
;C D
Task 
< 

PatientDto 
> 
UpdatePatientAsync +
(+ ,
int, /
	patientId0 9
,9 :
UpdatePatientDto; K
dtoL O
)O P
;P Q
Task 
< 

PatientDto 
> 
GetMyProfileAsync *
(* +
string+ 1
identityUserId2 @
)@ A
;A B
Task 
< 

PatientDto 
>  
UpdateMyProfileAsync -
(- .
string. 4
identityUserId5 C
,C D
UpdatePatientDtoE U
dtoV Y
)Y Z
;Z [
} 
} ê!
lC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IHealthRecordService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public 

	interface  
IHealthRecordService )
{ 
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #$
GetAllHealthRecordsAsync$ <
(< =
)= >
;> ?
Task

 
<

 
HealthRecordDto

 
>

 $
GetHealthRecordByIdAsync

 6
(

6 7
int

7 :
healthRecordId

; I
)

I J
;

J K
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #,
 GetHealthRecordsByPatientIdAsync$ D
(D E
intE H
	patientIdI R
)R S
;S T
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #+
GetHealthRecordsByDoctorIdAsync$ C
(C D
intD G
doctorIdH P
)P Q
;Q R
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #0
$GetHealthRecordsByAppointmentIdAsync$ H
(H I
intI L
appointmentIdM Z
)Z [
;[ \
Task 
< 
HealthRecordDto 
>  
AddHealthRecordAsync 2
(2 3
AddHealthRecordDto3 E
dtoF I
)I J
;J K
Task 
< 
HealthRecordDto 
> #
UpdateHealthRecordAsync 5
(5 6
int6 9
healthRecordId: H
,H I!
UpdateHealthRecordDtoJ _
dto` c
)c d
;d e
Task 
< 
HealthRecordDto 
> #
DeleteHealthRecordAsync 5
(5 6
int6 9
healthRecordId: H
)H I
;I J
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #-
!GetMyHealthRecordsForPatientAsync$ E
(E F
stringF L
identityUserIdM [
)[ \
;\ ]
Task 
< 
HealthRecordDto 
> .
"GetHealthRecordByIdForPatientAsync @
(@ A
intA D
healthRecordIdE S
,S T
stringT Z
identityUserId[ i
)i j
;j k
Task 
< 
List 
< 
HealthRecordDto !
>! "
>" #,
 GetMyHealthRecordsForDoctorAsync$ D
(D E
stringE K
identityUserIdL Z
)Z [
;[ \
Task 
< 
HealthRecordDto 
> -
!GetHealthRecordByIdForDoctorAsync ?
(? @
int@ C
healthRecordIdD R
,R S
stringS Y
identityUserIdZ h
)h i
;i j
Task   
<   
List   
<   
HealthRecordDto   !
>  ! "
>  " #9
-GetHealthRecordsByAppointmentIdForDoctorAsync  $ Q
(  Q R
int  R U
appointmentId  V c
,  c d
string  d j
identityUserId  k y
)  y z
;  z {
Task"" 
<"" 
HealthRecordDto"" 
>"" )
AddHealthRecordForDoctorAsync"" ;
(""; <
AddHealthRecordDto""< N
dto""O R
,""R S
string""S Y
identityUserId""Z h
)""h i
;""i j
Task$$ 
<$$ 
HealthRecordDto$$ 
>$$ ,
 UpdateHealthRecordForDoctorAsync$$ >
($$> ?
int$$? B
healthRecordId$$C Q
,$$Q R!
UpdateHealthRecordDto$$S h
dto$$i l
,$$l m
string$$m s
identityUserId	$$t Ç
)
$$Ç É
;
$$É Ñ
}%% 
}&& Î
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
	namespace 	
HealthCareApp
 
. 
Services  
.  !
	Interface! *
{ 
public 

	interface 
IAuthService !
{ 
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
,* +
int, /
	PatientId0 9
)9 :
>: ; 
RegisterPatientAsync< P
(P Q
PatientRegisterDtoQ c
requestd k
)k l
;l m
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
* +
string

, 2
Token

3 8
,

8 9
int

: =
	ExpiresIn

> G
)

G H
>

H I
Login

J O
(

O P
LoginDto

P X
request

Y `
)

` a
;

a b
Task 
< 
( 
bool 
Success 
, 
string "
Message# *
)* +
>+ ,
ChangePasswordAsync- @
(@ A
stringA G
userIdH N
,N O
ChangePasswordDtoP a
requestb i
)i j
;j k
} 
} ’A
kC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Interface\IAppointmentService.cs
	namespace 	
HealthCareApp
 
. 
Services  
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
 
AppointmentDto

 
>

 #
GetAppointmentByIdAsync

 4
(

4 5
int

5 8
appointmentId

9 F
)

F G
;

G H
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
)P Q
;Q R
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "*
GetAppointmentsByDoctorIdAsync# A
(A B
intB E
doctorIdF N
)N O
;O P
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "(
GetAppointmentsByStatusAsync# ?
(? @
AppointmentStatus@ Q
statusR X
)X Y
;Y Z
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "(
GetUpcomingAppointmentsAsync# ?
(? @
)@ A
;A B
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "3
'GetUpcomingAppointmentsByPatientIdAsync# J
(J K
intK N
	patientIdO X
)X Y
;Y Z
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "2
&GetUpcomingAppointmentsByDoctorIdAsync# I
(I J
intJ M
doctorIdN V
)V W
;W X
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "2
&GetPendingAppointmentsByPatientIdAsync# I
(I J
intJ M
	patientIdN W
)W X
;X Y
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "1
%GetPendingAppointmentsByDoctorIdAsync# H
(H I
intI L
doctorIdM U
)U V
;V W
Task 
< 
List 
< 
AppointmentDto  
>  !
>! "8
,GetTodayConfirmedAppointmentsByDoctorIdAsync# O
(O P
intP S
doctorIdT \
)\ ]
;] ^
Task 
< 
AppointmentDto 
>  
BookAppointmentAsync 1
(1 2
BookAppointmentDto2 D
dtoE H
)H I
;I J
Task   
<   
AppointmentDto   
>   "
UpdateAppointmentAsync   3
(  3 4
int  4 7
appointmentId  8 E
,  E F 
UpdateAppointmentDto  G [
dto  \ _
)  _ `
;  ` a
Task"" 
<"" 
AppointmentDto"" 
>"" #
ConfirmAppointmentAsync"" 4
(""4 5
int""5 8
appointmentId""9 F
)""F G
;""G H
Task$$ 
<$$ 
AppointmentDto$$ 
>$$ $
CompleteAppointmentAsync$$ 5
($$5 6
int$$6 9
appointmentId$$: G
)$$G H
;$$H I
Task&& 
<&& 
AppointmentDto&& 
>&& "
CancelAppointmentAsync&& 3
(&&3 4 
CancelAppointmentDto&&4 H
dto&&I L
)&&L M
;&&M N
Task(( 
<(( 
AppointmentDto(( 
>(( "
DeleteAppointmentAsync(( 3
(((3 4
int((4 7
appointmentId((8 E
)((E F
;((F G
Task** 
<** 
List** 
<** 
AppointmentDto**  
>**  !
>**! ",
 GetMyAppointmentsForPatientAsync**# C
(**C D
string**D J
identityUserId**K Y
)**Y Z
;**Z [
Task,, 
<,, 
List,, 
<,, 
AppointmentDto,,  
>,,  !
>,,! "4
(GetMyUpcomingAppointmentsForPatientAsync,,# K
(,,K L
string,,L R
identityUserId,,S a
),,a b
;,,b c
Task.. 
<.. 
List.. 
<.. 
AppointmentDto..  
>..  !
>..! "3
'GetMyPendingAppointmentsForPatientAsync..# J
(..J K
string..K Q
identityUserId..R `
)..` a
;..a b
Task00 
<00 
AppointmentDto00 
>00 -
!GetAppointmentByIdForPatientAsync00 >
(00> ?
int00? B
appointmentId00C P
,00P Q
string00R X
identityUserId00Y g
)00g h
;00h i
Task22 
<22 
AppointmentDto22 
>22 *
BookAppointmentForPatientAsync22 ;
(22; <
BookAppointmentDto22< N
dto22O R
,22R S
string22T Z
identityUserId22[ i
)22i j
;22j k
Task44 
<44 
AppointmentDto44 
>44 ,
 CancelAppointmentForPatientAsync44 =
(44= > 
CancelAppointmentDto44> R
dto44S V
,44V W
string44X ^
identityUserId44_ m
)44m n
;44n o
Task66 
<66 
List66 
<66 
AppointmentDto66  
>66  !
>66! "+
GetMyAppointmentsForDoctorAsync66# B
(66B C
string66C I
identityUserId66J X
)66X Y
;66Y Z
Task88 
<88 
List88 
<88 
AppointmentDto88  
>88  !
>88! "3
'GetMyUpcomingAppointmentsForDoctorAsync88# J
(88J K
string88K Q
identityUserId88R `
)88` a
;88a b
Task:: 
<:: 
List:: 
<:: 
AppointmentDto::  
>::  !
>::! "2
&GetMyPendingAppointmentsForDoctorAsync::# I
(::I J
string::J P
identityUserId::Q _
)::_ `
;::` a
Task<< 
<<< 
List<< 
<<< 
AppointmentDto<<  
><<  !
><<! "9
-GetMyTodayConfirmedAppointmentsForDoctorAsync<<# P
(<<P Q
string<<Q W
identityUserId<<X f
)<<f g
;<<g h
Task>> 
<>> 
AppointmentDto>> 
>>> ,
 GetAppointmentByIdForDoctorAsync>> =
(>>= >
int>>> A
appointmentId>>B O
,>>O P
string>>Q W
identityUserId>>X f
)>>f g
;>>g h
Task@@ 
<@@ 
AppointmentDto@@ 
>@@ ,
 ConfirmAppointmentForDoctorAsync@@ =
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
>BB -
!CompleteAppointmentForDoctorAsyncBB >
(BB> ?
intBB? B
appointmentIdBBC P
,BBP Q
stringBBR X
identityUserIdBBY g
)BBg h
;BBh i
TaskDD 
<DD 
AppointmentDtoDD 
>DD +
CancelAppointmentForDoctorAsyncDD <
(DD< = 
CancelAppointmentDtoDD= Q
dtoDDR U
,DDU V
stringDDV \
identityUserIdDD] k
)DDk l
;DDl m
TaskFF 
<FF 
PagedResponseFF 
<FF 
AppointmentDtoFF )
>FF) *
>FF* +(
GetAllAppointmentsPagedAsyncFF, H
(FFH I)
AppointmentPaginationQueryDtoFFI f
queryFFg l
)FFl m
;FFm n
}GG 
}HH ÖÀ
aC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\PatientService.cs
	namespace 	
HealthCareApp
 
. 
Services  
{ 
public		 

class		 
PatientService		 
(		  
IPatientRepository		  2

repository		3 =
,		= >
IMapper		? F
mapper		G M
)		M N
:		O P
IPatientService		Q `
{

 
public 
async 
Task 
< 
List 
< 

PatientDto )
>) *
>* +
GetAllPatientsAsync, ?
(? @
)@ A
{ 	
var 
patients 
= 
await  

repository! +
.+ ,
GetAllAsync, 7
(7 8
)8 9
;9 :
return 
mapper 
. 
Map 
< 
List "
<" #

PatientDto# -
>- .
>. /
(/ 0
patients0 8
)8 9
;9 :
} 	
public 
async 
Task 
< 
PagedResponse '
<' (

PatientDto( 2
>2 3
>3 4$
GetAllPatientsPagedAsync5 M
(M N%
PatientPaginationQueryDtoN g
queryh m
)m n
{ 	
if 
( 
query 
is 
null 
) 
{ 
query 
= 
new %
PatientPaginationQueryDto 5
(5 6
)6 7
;7 8
} 
int 

pageNumber 
= 
query "
." #

PageNumber# -
<=. 0
$num1 2
?3 4
$num5 6
:7 8
query9 >
.> ?

PageNumber? I
;I J
int 
pageSize 
= 
query  
.  !
PageSize! )
<=* ,
$num- .
?/ 0
$num1 3
:4 5
query6 ;
.; <
PageSize< D
;D E
pageSize 
= 
pageSize 
>  !
$num" %
?& '
$num( +
:, -
pageSize. 6
;6 7
var 
patients 
= 
await  

repository! +
.+ ,
GetAllAsync, 7
(7 8
)8 9
;9 :
var!! 
filteredPatients!!  
=!!! "
patients!!# +
.!!+ ,
AsEnumerable!!, 8
(!!8 9
)!!9 :
;!!: ;
if## 
(## 
!## 
string## 
.## 
IsNullOrWhiteSpace## *
(##* +
query##+ 0
.##0 1

SearchTerm##1 ;
)##; <
)##< =
{$$ 
string%% 

searchTerm%% !
=%%" #
query%%$ )
.%%) *

SearchTerm%%* 4
.%%4 5
Trim%%5 9
(%%9 :
)%%: ;
;%%; <
filteredPatients''  
=''! "
filteredPatients''# 3
.''3 4
Where''4 9
(''9 :
p'': ;
=>''< >
p(( 
.(( 
PatientName(( !
.((! "
Contains((" *
(((* +

searchTerm((+ 5
,((5 6
StringComparison((7 G
.((G H
OrdinalIgnoreCase((H Y
)((Y Z
||(([ ]
p)) 
.)) 
Email)) 
.)) 
Contains)) $
())$ %

searchTerm))% /
,))/ 0
StringComparison))1 A
.))A B
OrdinalIgnoreCase))B S
)))S T
||))U W
p** 
.** 
PhoneNumber** !
.**! "
Contains**" *
(*** +

searchTerm**+ 5
,**5 6
StringComparison**7 G
.**G H
OrdinalIgnoreCase**H Y
)**Y Z
||**[ ]
(++ 
!++ 
string++ 
.++ 
IsNullOrWhiteSpace++ /
(++/ 0
p++0 1
.++1 2
InsuranceID++2 =
)++= >
&&++? A
p,, 
.,, 
InsuranceID,, "
.,," #
Contains,,# +
(,,+ ,

searchTerm,,, 6
,,,6 7
StringComparison,,8 H
.,,H I
OrdinalIgnoreCase,,I Z
),,Z [
),,[ \
),,\ ]
;,,] ^
}-- 
if// 
(// 
query// 
.// 
Gender// 
is// 
not//  #
null//$ (
)//( )
{00 
filteredPatients11  
=11! "
filteredPatients11# 3
.113 4
Where114 9
(119 :
p11: ;
=>11< >
p22 
.22 
Gender22 
==22 
query22  %
.22% &
Gender22& ,
.22, -
Value22- 2
)222 3
;223 4
}33 
if55 
(55 
query55 
.55 
HasInsurance55 "
is55# %
not55& )
null55* .
)55. /
{66 
if77 
(77 
query77 
.77 
HasInsurance77 &
.77& '
Value77' ,
)77, -
{88 
filteredPatients99 $
=99% &
filteredPatients99' 7
.997 8
Where998 =
(99= >
p99> ?
=>99@ B
!:: 
string:: 
.::  
IsNullOrWhiteSpace::  2
(::2 3
p::3 4
.::4 5
InsuranceID::5 @
)::@ A
)::A B
;::B C
};; 
else<< 
{== 
filteredPatients>> $
=>>% &
filteredPatients>>' 7
.>>7 8
Where>>8 =
(>>= >
p>>> ?
=>>>@ B
string?? 
.?? 
IsNullOrWhiteSpace?? 1
(??1 2
p??2 3
.??3 4
InsuranceID??4 ?
)??? @
)??@ A
;??A B
}@@ 
}AA 
intCC 
totalRecordsCC 
=CC 
filteredPatientsCC /
.CC/ 0
CountCC0 5
(CC5 6
)CC6 7
;CC7 8
varEE 
pagedPatientsEE 
=EE 
filteredPatientsEE  0
.FF 
OrderByFF 
(FF 
pFF 
=>FF 
pFF 
.FF  
	PatientIdFF  )
)FF) *
.GG 
SkipGG 
(GG 
(GG 

pageNumberGG !
-GG" #
$numGG$ %
)GG% &
*GG' (
pageSizeGG) 1
)GG1 2
.HH 
TakeHH 
(HH 
pageSizeHH 
)HH 
.II 
ToListII 
(II 
)II 
;II 
varKK 
mappedPatientsKK 
=KK  
mapperKK! '
.KK' (
MapKK( +
<KK+ ,
ListKK, 0
<KK0 1

PatientDtoKK1 ;
>KK; <
>KK< =
(KK= >
pagedPatientsKK> K
)KKK L
;KKL M
returnMM 
newMM 
PagedResponseMM $
<MM$ %

PatientDtoMM% /
>MM/ 0
{NN 
ItemsOO 
=OO 
mappedPatientsOO &
,OO& '

PageNumberPP 
=PP 

pageNumberPP '
,PP' (
PageSizeQQ 
=QQ 
pageSizeQQ #
,QQ# $
TotalRecordsRR 
=RR 
totalRecordsRR +
,RR+ ,

TotalPagesSS 
=SS 
(SS 
intSS !
)SS! "
MathSS" &
.SS& '
CeilingSS' .
(SS. /
totalRecordsSS/ ;
/SS< =
(SS> ?
doubleSS? E
)SSE F
pageSizeSSF N
)SSN O
}TT 
;TT 
}UU 	
publicVV 
asyncVV 
TaskVV 
<VV 

PatientDtoVV $
>VV$ %
GetPatientByIdAsyncVV& 9
(VV9 :
intVV: =
	patientIdVV> G
)VVG H
{WW 	
ValidatePatientIdXX 
(XX 
	patientIdXX '
)XX' (
;XX( )
varZZ 
patientZZ 
=ZZ 
awaitZZ 

repositoryZZ  *
.ZZ* +
GetByIdAsyncZZ+ 7
(ZZ7 8
	patientIdZZ8 A
)ZZA B
;ZZB C
if\\ 
(\\ 
patient\\ 
is\\ 
null\\ 
)\\  
{]] 
throw^^ 
new^^ #
EntityNotFoundException^^ 1
(^^1 2
$str^^2 ;
,^^; <
	patientId^^= F
)^^F G
;^^G H
}__ 
returnaa 
mapperaa 
.aa 
Mapaa 
<aa 

PatientDtoaa (
>aa( )
(aa) *
patientaa* 1
)aa1 2
;aa2 3
}bb 	
publicdd 
asyncdd 
Taskdd 
<dd 

PatientDtodd $
>dd$ % 
RegisterPatientAsyncdd& :
(dd: ;
CreatePatientDtodd; K
dtoddL O
)ddO P
{ee 	$
ValidateCreatePatientDtoff $
(ff$ %
dtoff% (
)ff( )
;ff) *
stringhh 
patientNamehh 
=hh  
dtohh! $
.hh$ %
FullNamehh% -
.hh- .
Trimhh. 2
(hh2 3
)hh3 4
.hh4 5
ToLowerhh5 <
(hh< =
)hh= >
;hh> ?
stringii 
emailii 
=ii 
dtoii 
.ii 
Emailii $
.ii$ %
Trimii% )
(ii) *
)ii* +
.ii+ ,
ToLowerii, 3
(ii3 4
)ii4 5
;ii5 6
stringjj 
phoneNumberjj 
=jj  
dtojj! $
.jj$ %
PhoneNumberjj% 0
.jj0 1
Trimjj1 5
(jj5 6
)jj6 7
;jj7 8
DateTimekk 
dateOfBirthkk  
=kk! "
dtokk# &
.kk& '
DateOfBirthkk' 2
.kk2 3
Datekk3 7
;kk7 8
boolmm 
	duplicatemm 
=mm 
awaitmm "

repositorymm# -
.mm- .#
IsDuplicatePatientAsyncmm. E
(mmE F
patientNamenn 
,nn 
emailoo 
,oo 
phoneNumberpp 
,pp 
dateOfBirthqq 
)qq 
;qq 
ifss 
(ss 
	duplicatess 
)ss 
{tt 
throwuu 
newuu 
ConflictExceptionuu +
(uu+ ,
$struu, \
)uu\ ]
;uu] ^
}vv 
varxx 
patientxx 
=xx 
mapperxx  
.xx  !
Mapxx! $
<xx$ %
Patientxx% ,
>xx, -
(xx- .
dtoxx. 1
)xx1 2
;xx2 3
patientzz 
.zz 
DateOfBirthzz 
=zz  !
dateOfBirthzz" -
;zz- .
patient{{ 
.{{ 
CreatedDate{{ 
={{  !
DateTime{{" *
.{{* +
Now{{+ .
;{{. /
var}} 
savedPatient}} 
=}} 
await}} $

repository}}% /
.}}/ 0
CreateAsync}}0 ;
(}}; <
patient}}< C
)}}C D
;}}D E
return 
mapper 
. 
Map 
< 

PatientDto (
>( )
() *
savedPatient* 6
)6 7
;7 8
}
ÄÄ 	
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 

PatientDto
ÇÇ $
>
ÇÇ$ % 
UpdatePatientAsync
ÇÇ& 8
(
ÇÇ8 9
int
ÇÇ9 <
	patientId
ÇÇ= F
,
ÇÇF G
UpdatePatientDto
ÇÇH X
dto
ÇÇY \
)
ÇÇ\ ]
{
ÉÉ 	
ValidatePatientId
ÑÑ 
(
ÑÑ 
	patientId
ÑÑ '
)
ÑÑ' (
;
ÑÑ( )&
ValidateUpdatePatientDto
ÜÜ $
(
ÜÜ$ %
dto
ÜÜ% (
)
ÜÜ( )
;
ÜÜ) *
var
àà 
existingPatient
àà 
=
àà  !
await
àà" '

repository
àà( 2
.
àà2 3
GetByIdAsync
àà3 ?
(
àà? @
	patientId
àà@ I
)
ààI J
;
ààJ K
if
ää 
(
ää 
existingPatient
ää 
is
ää  "
null
ää# '
)
ää' (
{
ãã 
throw
åå 
new
åå %
EntityNotFoundException
åå 1
(
åå1 2
$str
åå2 ;
,
åå; <
	patientId
åå= F
)
ååF G
;
ååG H
}
çç 
string
èè 
patientName
èè 
=
èè  
dto
èè! $
.
èè$ %
FullName
èè% -
.
èè- .
Trim
èè. 2
(
èè2 3
)
èè3 4
.
èè4 5
ToLower
èè5 <
(
èè< =
)
èè= >
;
èè> ?
string
êê 
email
êê 
=
êê 
dto
êê 
.
êê 
Email
êê $
.
êê$ %
Trim
êê% )
(
êê) *
)
êê* +
.
êê+ ,
ToLower
êê, 3
(
êê3 4
)
êê4 5
;
êê5 6
string
ëë 
phoneNumber
ëë 
=
ëë  
dto
ëë! $
.
ëë$ %
PhoneNumber
ëë% 0
.
ëë0 1
Trim
ëë1 5
(
ëë5 6
)
ëë6 7
;
ëë7 8
DateTime
íí 
dateOfBirth
íí  
=
íí! "
dto
íí# &
.
íí& '
DateOfBirth
íí' 2
.
íí2 3
Date
íí3 7
;
íí7 8
bool
îî 
	duplicate
îî 
=
îî 
await
îî "

repository
îî# -
.
îî- .%
IsDuplicatePatientAsync
îî. E
(
îîE F
patientName
ïï 
,
ïï 
email
ññ 
,
ññ 
phoneNumber
óó 
,
óó 
dateOfBirth
òò 
,
òò 
	patientId
ôô 
)
ôô 
;
ôô 
if
õõ 
(
õõ 
	duplicate
õõ 
)
õõ 
{
úú 
throw
ùù 
new
ùù 
ConflictException
ùù +
(
ùù+ ,
$str
ùù, b
)
ùùb c
;
ùùc d
}
ûû 
var
†† 
patient
†† 
=
†† 
mapper
††  
.
††  !
Map
††! $
<
††$ %
Patient
††% ,
>
††, -
(
††- .
dto
††. 1
)
††1 2
;
††2 3
patient
¢¢ 
.
¢¢ 
	PatientId
¢¢ 
=
¢¢ 
	patientId
¢¢  )
;
¢¢) *
patient
££ 
.
££ 
DateOfBirth
££ 
=
££  !
dateOfBirth
££" -
;
££- .
patient
§§ 
.
§§ 
CreatedDate
§§ 
=
§§  !
existingPatient
§§" 1
.
§§1 2
CreatedDate
§§2 =
;
§§= >
var
¶¶ 
updatedPatient
¶¶ 
=
¶¶  
await
¶¶! &

repository
¶¶' 1
.
¶¶1 2
UpdateAsync
¶¶2 =
(
¶¶= >
	patientId
¶¶> G
,
¶¶G H
patient
¶¶I P
)
¶¶P Q
;
¶¶Q R
if
®® 
(
®® 
updatedPatient
®® 
is
®® !
null
®®" &
)
®®& '
{
©© 
throw
™™ 
new
™™ %
EntityNotFoundException
™™ 1
(
™™1 2
$str
™™2 ;
,
™™; <
	patientId
™™= F
)
™™F G
;
™™G H
}
´´ 
return
≠≠ 
mapper
≠≠ 
.
≠≠ 
Map
≠≠ 
<
≠≠ 

PatientDto
≠≠ (
>
≠≠( )
(
≠≠) *
updatedPatient
≠≠* 8
)
≠≠8 9
;
≠≠9 :
}
ÆÆ 	
private
∞∞ 
void
∞∞ 
ValidatePatientId
∞∞ &
(
∞∞& '
int
∞∞' *
	patientId
∞∞+ 4
)
∞∞4 5
{
±± 	
if
≤≤ 
(
≤≤ 
	patientId
≤≤ 
<=
≤≤ 
$num
≤≤ 
)
≤≤ 
{
≥≥ 
throw
¥¥ 
new
¥¥ #
BusinessRuleException
¥¥ /
(
¥¥/ 0
$str
¥¥0 [
)
¥¥[ \
;
¥¥\ ]
}
µµ 
}
∂∂ 	
private
∏∏ 
void
∏∏ &
ValidateCreatePatientDto
∏∏ -
(
∏∏- .
CreatePatientDto
∏∏. >
dto
∏∏? B
)
∏∏B C
{
ππ 	
if
∫∫ 
(
∫∫ 
dto
∫∫ 
is
∫∫ 
null
∫∫ 
)
∫∫ 
{
ªª 
throw
ºº 
new
ºº #
BusinessRuleException
ºº /
(
ºº/ 0
$str
ºº0 O
)
ººO P
;
ººP Q
}
ΩΩ )
ValidatePatientCommonFields
øø '
(
øø' (
dto
¿¿ 
.
¿¿ 
FullName
¿¿ 
,
¿¿ 
dto
¡¡ 
.
¡¡ 
DateOfBirth
¡¡ 
,
¡¡  
dto
¬¬ 
.
¬¬ 
Email
¬¬ 
,
¬¬ 
dto
√√ 
.
√√ 
PhoneNumber
√√ 
)
√√  
;
√√  !
}
ƒƒ 	
private
∆∆ 
void
∆∆ &
ValidateUpdatePatientDto
∆∆ -
(
∆∆- .
UpdatePatientDto
∆∆. >
dto
∆∆? B
)
∆∆B C
{
«« 	
if
»» 
(
»» 
dto
»» 
is
»» 
null
»» 
)
»» 
{
…… 
throw
   
new
   #
BusinessRuleException
   /
(
  / 0
$str
  0 O
)
  O P
;
  P Q
}
ÀÀ )
ValidatePatientCommonFields
ÕÕ '
(
ÕÕ' (
dto
ŒŒ 
.
ŒŒ 
FullName
ŒŒ 
,
ŒŒ 
dto
œœ 
.
œœ 
DateOfBirth
œœ 
,
œœ  
dto
–– 
.
–– 
Email
–– 
,
–– 
dto
—— 
.
—— 
PhoneNumber
—— 
)
——  
;
——  !
}
““ 	
private
‘‘ 
void
‘‘ )
ValidatePatientCommonFields
‘‘ 0
(
‘‘0 1
string
’’ 
patientName
’’ 
,
’’ 
DateTime
÷÷ 
dateOfBirth
÷÷  
,
÷÷  !
string
◊◊ 
email
◊◊ 
,
◊◊ 
string
ÿÿ 
phoneNumber
ÿÿ 
)
ÿÿ 
{
ŸŸ 	
if
⁄⁄ 
(
⁄⁄ 
string
⁄⁄ 
.
⁄⁄  
IsNullOrWhiteSpace
⁄⁄ )
(
⁄⁄) *
patientName
⁄⁄* 5
)
⁄⁄5 6
)
⁄⁄6 7
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
‹‹0 K
)
‹‹K L
;
‹‹L M
}
›› 
if
ﬂﬂ 
(
ﬂﬂ 
dateOfBirth
ﬂﬂ 
.
ﬂﬂ 
Date
ﬂﬂ  
<
ﬂﬂ! "
new
ﬂﬂ# &
DateTime
ﬂﬂ' /
(
ﬂﬂ/ 0
$num
ﬂﬂ0 4
,
ﬂﬂ4 5
$num
ﬂﬂ6 7
,
ﬂﬂ7 8
$num
ﬂﬂ9 :
)
ﬂﬂ: ;
)
ﬂﬂ; <
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
··0 ]
)
··] ^
;
··^ _
}
‚‚ 
if
‰‰ 
(
‰‰ 
dateOfBirth
‰‰ 
.
‰‰ 
Date
‰‰  
>
‰‰! "
DateTime
‰‰# +
.
‰‰+ ,
Today
‰‰, 1
)
‰‰1 2
{
ÂÂ 
throw
ÊÊ 
new
ÊÊ #
BusinessRuleException
ÊÊ /
(
ÊÊ/ 0
$str
ÊÊ0 X
)
ÊÊX Y
;
ÊÊY Z
}
ÁÁ 
if
ÈÈ 
(
ÈÈ 
string
ÈÈ 
.
ÈÈ  
IsNullOrWhiteSpace
ÈÈ )
(
ÈÈ) *
email
ÈÈ* /
)
ÈÈ/ 0
)
ÈÈ0 1
{
ÍÍ 
throw
ÎÎ 
new
ÎÎ #
BusinessRuleException
ÎÎ /
(
ÎÎ/ 0
$str
ÎÎ0 L
)
ÎÎL M
;
ÎÎM N
}
ÏÏ 
if
ÓÓ 
(
ÓÓ 
string
ÓÓ 
.
ÓÓ  
IsNullOrWhiteSpace
ÓÓ )
(
ÓÓ) *
phoneNumber
ÓÓ* 5
)
ÓÓ5 6
)
ÓÓ6 7
{
ÔÔ 
throw
 
new
 #
BusinessRuleException
 /
(
/ 0
$str
0 K
)
K L
;
L M
}
ÒÒ 
}
ÚÚ 	
public
ÙÙ 
async
ÙÙ 
Task
ÙÙ 
<
ÙÙ 

PatientDto
ÙÙ $
>
ÙÙ$ %
GetMyProfileAsync
ÙÙ& 7
(
ÙÙ7 8
string
ÙÙ8 >
identityUserId
ÙÙ? M
)
ÙÙM N
{ıı 
if
ˆˆ 
(
ˆˆ 
string
ˆˆ 
.
ˆˆ  
IsNullOrWhiteSpace
ˆˆ !
(
ˆˆ! "
identityUserId
ˆˆ" 0
)
ˆˆ0 1
)
ˆˆ1 2
{
˜˜ 
throw
¯¯ 
new
¯¯ #
BusinessRuleException
¯¯ '
(
¯¯' (
$str
¯¯( A
)
¯¯A B
;
¯¯B C
}
˘˘ 
var
˚˚ 
patient
˚˚ 
=
˚˚ 
await
˚˚ 

repository
˚˚ "
.
˚˚" #&
GetByIdentityUserIdAsync
˚˚# ;
(
˚˚; <
identityUserId
˚˚< J
)
˚˚J K
;
˚˚K L
if
˝˝ 
(
˝˝ 
patient
˝˝ 
is
˝˝ 
null
˝˝ 
)
˝˝ 
{
˛˛ 
throw
ˇˇ 
new
ˇˇ %
EntityNotFoundException
ˇˇ )
(
ˇˇ) *
$str
ˇˇ* N
,
ˇˇN O
$num
ˇˇP Q
)
ˇˇQ R
;
ˇˇR S
}
ÄÄ 
return
ÇÇ 

mapper
ÇÇ 
.
ÇÇ 
Map
ÇÇ 
<
ÇÇ 

PatientDto
ÇÇ  
>
ÇÇ  !
(
ÇÇ! "
patient
ÇÇ" )
)
ÇÇ) *
;
ÇÇ* +
}ÉÉ 
publicÖÖ 
async
ÖÖ 
Task
ÖÖ 
<
ÖÖ 

PatientDto
ÖÖ 
>
ÖÖ "
UpdateMyProfileAsync
ÖÖ 2
(
ÖÖ2 3
string
ÖÖ3 9
identityUserId
ÖÖ: H
,
ÖÖH I
UpdatePatientDto
ÖÖJ Z
dto
ÖÖ[ ^
)
ÖÖ^ _
{ÜÜ 
if
áá 
(
áá 
string
áá 
.
áá  
IsNullOrWhiteSpace
áá !
(
áá! "
identityUserId
áá" 0
)
áá0 1
)
áá1 2
{
àà 
throw
ââ 
new
ââ #
BusinessRuleException
ââ '
(
ââ' (
$str
ââ( A
)
ââA B
;
ââB C
}
ää 
if
åå 
(
åå 
dto
åå 
is
åå 
null
åå 
)
åå 
{
çç 
throw
éé 
new
éé #
BusinessRuleException
éé '
(
éé' (
$str
éé( G
)
ééG H
;
ééH I
}
èè 
if
ëë 
(
ëë 
dto
ëë 
.
ëë 
DateOfBirth
ëë 
.
ëë 
Date
ëë 
>
ëë 
DateTime
ëë '
.
ëë' (
Today
ëë( -
)
ëë- .
{
íí 
throw
ìì 
new
ìì #
BusinessRuleException
ìì '
(
ìì' (
$str
ìì( P
)
ììP Q
;
ììQ R
}
îî 
var
ññ 
existingPatient
ññ 
=
ññ 
await
ññ 

repository
ññ  *
.
ññ* +&
GetByIdentityUserIdAsync
ññ+ C
(
ññC D
identityUserId
ññD R
)
ññR S
;
ññS T
if
òò 
(
òò 
existingPatient
òò 
is
òò 
null
òò 
)
òò  
{
ôô 
throw
öö 
new
öö %
EntityNotFoundException
öö )
(
öö) *
$str
öö* N
,
ööN O
$num
ööP Q
)
ööQ R
;
ööR S
}
õõ 
var
ùù 
patient
ùù 
=
ùù 
mapper
ùù 
.
ùù 
Map
ùù 
<
ùù 
Patient
ùù $
>
ùù$ %
(
ùù% &
dto
ùù& )
)
ùù) *
;
ùù* +
patient
üü 
.
üü 
	PatientId
üü 
=
üü 
existingPatient
üü '
.
üü' (
	PatientId
üü( 1
;
üü1 2
patient
†† 
.
†† 
IdentityUserId
†† 
=
†† 
existingPatient
†† ,
.
††, -
IdentityUserId
††- ;
;
††; <
patient
°° 
.
°° 
Email
°° 
=
°° 
existingPatient
°° #
.
°°# $
Email
°°$ )
;
°°) *
patient
¢¢ 
.
¢¢ 
CreatedDate
¢¢ 
=
¢¢ 
existingPatient
¢¢ )
.
¢¢) *
CreatedDate
¢¢* 5
;
¢¢5 6
var
§§ 
updatedPatient
§§ 
=
§§ 
await
§§ 

repository
§§ )
.
§§) *
UpdateAsync
§§* 5
(
§§5 6
existingPatient
§§6 E
.
§§E F
	PatientId
§§F O
,
§§O P
patient
§§Q X
)
§§X Y
;
§§Y Z
if
¶¶ 
(
¶¶ 
updatedPatient
¶¶ 
is
¶¶ 
null
¶¶ 
)
¶¶ 
{
ßß 
throw
®® 
new
®® %
EntityNotFoundException
®® )
(
®®) *
$str
®®* 3
,
®®3 4
existingPatient
®®5 D
.
®®D E
	PatientId
®®E N
)
®®N O
;
®®O P
}
©© 
return
´´ 

mapper
´´ 
.
´´ 
Map
´´ 
<
´´ 

PatientDto
´´  
>
´´  !
(
´´! "
updatedPatient
´´" 0
)
´´0 1
;
´´1 2
}¨¨ 
}
≠≠ 
}ÆÆ ›‘
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\HealthRecordService.cs
	namespace		 	
HealthCareApp		
 
.		 
Services		  
{

 
public 

class 
HealthRecordService $
($ %#
IHealthRecordRepository "
healthRecordRepository  6
,6 7
IPatientRepository 
patientRepository ,
,, -
IDoctorRepository 
doctorRepository *
,* +"
IAppointmentRepository !
appointmentRepository 4
,4 5
IMapper 
mapper 
) 
:  
IHealthRecordService .
{ 
public 
async 
Task 
< 
List 
< 
HealthRecordDto .
>. /
>/ 0$
GetAllHealthRecordsAsync1 I
(I J
)J K
{ 	
var 
healthRecords 
= 
await  %"
healthRecordRepository& <
.< =
GetAllAsync= H
(H I
)I J
;J K
return 
mapper 
. 
Map 
< 
List "
<" #
HealthRecordDto# 2
>2 3
>3 4
(4 5
healthRecords5 B
)B C
;C D
} 	
public 
async 
Task 
< 
HealthRecordDto )
>) *$
GetHealthRecordByIdAsync+ C
(C D
intD G
healthRecordIdH V
)V W
{ 	"
ValidateHealthRecordId "
(" #
healthRecordId# 1
)1 2
;2 3
var 
healthRecord 
= 
await $"
healthRecordRepository% ;
.; <
GetByIdAsync< H
(H I
healthRecordIdI W
)W X
;X Y
if 
( 
healthRecord 
is 
null  $
)$ %
{   
throw!! 
new!! #
EntityNotFoundException!! 1
(!!1 2
$str!!2 @
,!!@ A
healthRecordId!!B P
)!!P Q
;!!Q R
}"" 
return$$ 
mapper$$ 
.$$ 
Map$$ 
<$$ 
HealthRecordDto$$ -
>$$- .
($$. /
healthRecord$$/ ;
)$$; <
;$$< =
}%% 	
public'' 
async'' 
Task'' 
<'' 
List'' 
<'' 
HealthRecordDto'' .
>''. /
>''/ 0,
 GetHealthRecordsByPatientIdAsync''1 Q
(''Q R
int''R U
	patientId''V _
)''_ `
{(( 	
await)) &
ValidatePatientExistsAsync)) ,
()), -
	patientId))- 6
)))6 7
;))7 8
var++ 
healthRecords++ 
=++ 
await++  %"
healthRecordRepository++& <
.++< =
GetByPatientIdAsync++= P
(++P Q
	patientId++Q Z
)++Z [
;++[ \
return-- 
mapper-- 
.-- 
Map-- 
<-- 
List-- "
<--" #
HealthRecordDto--# 2
>--2 3
>--3 4
(--4 5
healthRecords--5 B
)--B C
;--C D
}.. 	
public00 
async00 
Task00 
<00 
List00 
<00 
HealthRecordDto00 .
>00. /
>00/ 0+
GetHealthRecordsByDoctorIdAsync001 P
(00P Q
int00Q T
doctorId00U ]
)00] ^
{11 	
await22 %
ValidateDoctorExistsAsync22 +
(22+ ,
doctorId22, 4
)224 5
;225 6
var44 
healthRecords44 
=44 
await44  %"
healthRecordRepository44& <
.44< =
GetByDoctorIdAsync44= O
(44O P
doctorId44P X
)44X Y
;44Y Z
return66 
mapper66 
.66 
Map66 
<66 
List66 "
<66" #
HealthRecordDto66# 2
>662 3
>663 4
(664 5
healthRecords665 B
)66B C
;66C D
}77 	
public99 
async99 
Task99 
<99 
List99 
<99 
HealthRecordDto99 .
>99. /
>99/ 00
$GetHealthRecordsByAppointmentIdAsync991 U
(99U V
int99V Y
appointmentId99Z g
)99g h
{:: 	
await;; )
GetAppointmentEntityByIdAsync;; /
(;;/ 0
appointmentId;;0 =
);;= >
;;;> ?
var== 
healthRecords== 
=== 
await==  %"
healthRecordRepository==& <
.==< =#
GetByAppointmentIdAsync=== T
(==T U
appointmentId==U b
)==b c
;==c d
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
(??4 5
healthRecords??5 B
)??B C
;??C D
}@@ 	
publicBB 
asyncBB 
TaskBB 
<BB 
HealthRecordDtoBB )
>BB) * 
AddHealthRecordAsyncBB+ ?
(BB? @
AddHealthRecordDtoBB@ R
dtoBBS V
)BBV W
{CC 	
ifDD 
(DD 
dtoDD 
isDD 
nullDD 
)DD 
{EE 
throwFF 
newFF %
HealthRecordRuleExceptionFF 3
(FF3 4
$strFF4 Y
)FFY Z
;FFZ [
}GG 
ValidatePatientIdII 
(II 
dtoII !
.II! "
	PatientIdII" +
)II+ ,
;II, -!
ValidateAppointmentIdJJ !
(JJ! "
dtoJJ" %
.JJ% &
AppointmentIdJJ& 3
)JJ3 4
;JJ4 5
varLL 
patientLL 
=LL 
awaitLL 
patientRepositoryLL  1
.LL1 2
GetByIdAsyncLL2 >
(LL> ?
dtoLL? B
.LLB C
	PatientIdLLC L
)LLL M
;LLM N
ifNN 
(NN 
patientNN 
isNN 
nullNN 
)NN  
{OO 
throwPP 
newPP #
EntityNotFoundExceptionPP 1
(PP1 2
$strPP2 ;
,PP; <
dtoPP= @
.PP@ A
	PatientIdPPA J
)PPJ K
;PPK L
}QQ 
varSS 
appointmentSS 
=SS 
awaitSS #!
appointmentRepositorySS$ 9
.SS9 :
GetByIdAsyncSS: F
(SSF G
dtoSSG J
.SSJ K
AppointmentIdSSK X
)SSX Y
;SSY Z
ifUU 
(UU 
appointmentUU 
isUU 
nullUU #
)UU# $
{VV 
throwWW 
newWW #
EntityNotFoundExceptionWW 1
(WW1 2
$strWW2 ?
,WW? @
dtoWWA D
.WWD E
AppointmentIdWWE R
)WWR S
;WWS T
}XX 
ifZZ 
(ZZ 
appointmentZZ 
.ZZ 
	PatientIdZZ %
!=ZZ& (
dtoZZ) ,
.ZZ, -
	PatientIdZZ- 6
)ZZ6 7
{[[ 
throw\\ 
new\\ %
HealthRecordRuleException\\ 3
(\\3 4
$str\\4 j
)\\j k
;\\k l
}]] 
if__ 
(__ 
dto__ 
.__ 
DoctorId__ 
is__ 
not__  #
null__$ (
)__( )
{`` 
ValidateDoctorIdaa  
(aa  !
dtoaa! $
.aa$ %
DoctorIdaa% -
.aa- .
Valueaa. 3
)aa3 4
;aa4 5
varcc 
doctorcc 
=cc 
awaitcc "
doctorRepositorycc# 3
.cc3 4
GetByIdAsynccc4 @
(cc@ A
dtoccA D
.ccD E
DoctorIdccE M
.ccM N
ValueccN S
)ccS T
;ccT U
ifee 
(ee 
doctoree 
isee 
nullee "
)ee" #
{ff 
throwgg 
newgg #
EntityNotFoundExceptiongg 5
(gg5 6
$strgg6 >
,gg> ?
dtogg@ C
.ggC D
DoctorIdggD L
.ggL M
ValueggM R
)ggR S
;ggS T
}hh 
ifjj 
(jj 
appointmentjj 
.jj  
DoctorIdjj  (
!=jj) +
dtojj, /
.jj/ 0
DoctorIdjj0 8
.jj8 9
Valuejj9 >
)jj> ?
{kk 
throwll 
newll %
HealthRecordRuleExceptionll 7
(ll7 8
$strll8 m
)llm n
;lln o
}mm 
}nn 
ifpp 
(pp 
appointmentpp 
.pp 
Statuspp "
==pp# %
AppointmentStatuspp& 7
.pp7 8
	Cancelledpp8 A
)ppA B
{qq 
throwrr 
newrr %
HealthRecordRuleExceptionrr 3
(rr3 4
$strrr4 p
)rrp q
;rrq r
}ss 
ifuu 
(uu 
appointmentuu 
.uu 
Statusuu "
==uu# %
AppointmentStatusuu& 7
.uu7 8
Pendinguu8 ?
)uu? @
{vv 
throwww 
newww %
HealthRecordRuleExceptionww 3
(ww3 4
$strww4 n
)wwn o
;wwo p
}xx 
ifzz 
(zz 
appointmentzz 
.zz 
Statuszz "
==zz# %
AppointmentStatuszz& 7
.zz7 8
	Completedzz8 A
)zzA B
{{{ 
throw|| 
new|| %
HealthRecordRuleException|| 3
(||3 4
$str||4 w
)||w x
;||x y
}}} 
if 
( 
appointment 
. 
Status "
!=# %
AppointmentStatus& 7
.7 8
	Confirmed8 A
)A B
{
ÄÄ 
throw
ÅÅ 
new
ÅÅ '
HealthRecordRuleException
ÅÅ 3
(
ÅÅ3 4
$str
ÅÅ4 q
)
ÅÅq r
;
ÅÅr s
}
ÇÇ 
if
ÑÑ 
(
ÑÑ 
appointment
ÑÑ 
.
ÑÑ 
ScheduledDate
ÑÑ )
.
ÑÑ) *
Date
ÑÑ* .
>
ÑÑ/ 0
DateTime
ÑÑ1 9
.
ÑÑ9 :
Today
ÑÑ: ?
)
ÑÑ? @
{
ÖÖ 
throw
ÜÜ 
new
ÜÜ '
HealthRecordRuleException
ÜÜ 3
(
ÜÜ3 4
$str
ÜÜ4 p
)
ÜÜp q
;
ÜÜq r
}
áá 
if
ââ 
(
ââ 
dto
ââ 
.
ââ 
	VisitDate
ââ 
.
ââ 
Date
ââ "
!=
ââ# %
appointment
ââ& 1
.
ââ1 2
ScheduledDate
ââ2 ?
.
ââ? @
Date
ââ@ D
)
ââD E
{
ää 
throw
ãã 
new
ãã '
HealthRecordRuleException
ãã 3
(
ãã3 4
$str
ãã4 k
)
ããk l
;
ããl m
}
åå 
var
éé  
healthRecordExists
éé "
=
éé# $
await
éé% *$
healthRecordRepository
éé+ A
.
ééA B(
ExistsByAppointmentIdAsync
ééB \
(
éé\ ]
dto
éé] `
.
éé` a
AppointmentId
ééa n
)
één o
;
ééo p
if
êê 
(
êê  
healthRecordExists
êê "
)
êê" #
{
ëë 
throw
íí 
new
íí 
ConflictException
íí +
(
íí+ ,
$str
íí, `
)
íí` a
;
íía b
}
ìì &
ValidateHealthRecordText
ïï $
(
ïï$ %
dto
ïï% (
.
ïï( )
	Diagnosis
ïï) 2
,
ïï2 3
dto
ïï4 7
.
ïï7 8
Prescription
ïï8 D
,
ïïD E
dto
ïïF I
.
ïïI J
Notes
ïïJ O
)
ïïO P
;
ïïP Q
var
óó 
healthRecord
óó 
=
óó 
mapper
óó %
.
óó% &
Map
óó& )
<
óó) *
HealthRecord
óó* 6
>
óó6 7
(
óó7 8
dto
óó8 ;
)
óó; <
;
óó< =
healthRecord
ôô 
.
ôô 
	PatientId
ôô "
=
ôô# $
appointment
ôô% 0
.
ôô0 1
	PatientId
ôô1 :
;
ôô: ;
healthRecord
öö 
.
öö 
DoctorId
öö !
=
öö" #
appointment
öö$ /
.
öö/ 0
DoctorId
öö0 8
;
öö8 9
healthRecord
õõ 
.
õõ 
AppointmentId
õõ &
=
õõ' (
appointment
õõ) 4
.
õõ4 5
AppointmentId
õõ5 B
;
õõB C
healthRecord
úú 
.
úú 
CreatedDate
úú $
=
úú% &
DateTime
úú' /
.
úú/ 0
Now
úú0 3
;
úú3 4
var
ûû 
savedHealthRecord
ûû !
=
ûû" #
await
ûû$ )$
healthRecordRepository
ûû* @
.
ûû@ A
CreateAsync
ûûA L
(
ûûL M
healthRecord
ûûM Y
)
ûûY Z
;
ûûZ [
appointment
†† 
.
†† 
Status
†† 
=
††  
AppointmentStatus
††! 2
.
††2 3
	Completed
††3 <
;
††< =
await
¢¢ #
appointmentRepository
¢¢ '
.
¢¢' (
UpdateAsync
¢¢( 3
(
¢¢3 4
appointment
¢¢4 ?
.
¢¢? @
AppointmentId
¢¢@ M
,
¢¢M N
appointment
¢¢O Z
)
¢¢Z [
;
¢¢[ \
return
§§ 
mapper
§§ 
.
§§ 
Map
§§ 
<
§§ 
HealthRecordDto
§§ -
>
§§- .
(
§§. /
savedHealthRecord
§§/ @
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
ßß 
HealthRecordDto
ßß )
>
ßß) *%
UpdateHealthRecordAsync
ßß+ B
(
ßßB C
int
ßßC F
healthRecordId
ßßG U
,
ßßU V#
UpdateHealthRecordDto
ßßW l
dto
ßßm p
)
ßßp q
{
®® 	$
ValidateHealthRecordId
©© "
(
©©" #
healthRecordId
©©# 1
)
©©1 2
;
©©2 3
if
´´ 
(
´´ 
dto
´´ 
is
´´ 
null
´´ 
)
´´ 
{
¨¨ 
throw
≠≠ 
new
≠≠ '
HealthRecordRuleException
≠≠ 3
(
≠≠3 4
$str
≠≠4 Y
)
≠≠Y Z
;
≠≠Z [
}
ÆÆ 
var
∞∞ "
existingHealthRecord
∞∞ $
=
∞∞% &
await
∞∞' ,$
healthRecordRepository
∞∞- C
.
∞∞C D
GetByIdAsync
∞∞D P
(
∞∞P Q
healthRecordId
∞∞Q _
)
∞∞_ `
;
∞∞` a
if
≤≤ 
(
≤≤ "
existingHealthRecord
≤≤ $
is
≤≤% '
null
≤≤( ,
)
≤≤, -
{
≥≥ 
throw
¥¥ 
new
¥¥ %
EntityNotFoundException
¥¥ 1
(
¥¥1 2
$str
¥¥2 @
,
¥¥@ A
healthRecordId
¥¥B P
)
¥¥P Q
;
¥¥Q R
}
µµ 
var
∑∑ 
appointment
∑∑ 
=
∑∑ 
await
∑∑ ##
appointmentRepository
∑∑$ 9
.
∑∑9 :
GetByIdAsync
∑∑: F
(
∑∑F G"
existingHealthRecord
∑∑G [
.
∑∑[ \
AppointmentId
∑∑\ i
)
∑∑i j
;
∑∑j k
if
ππ 
(
ππ 
appointment
ππ 
is
ππ 
null
ππ #
)
ππ# $
{
∫∫ 
throw
ªª 
new
ªª %
EntityNotFoundException
ªª 1
(
ªª1 2
$str
ªª2 ?
,
ªª? @"
existingHealthRecord
ªªA U
.
ªªU V
AppointmentId
ªªV c
)
ªªc d
;
ªªd e
}
ºº 
if
ææ 
(
ææ 
appointment
ææ 
.
ææ 
ScheduledDate
ææ )
.
ææ) *
Date
ææ* .
>
ææ/ 0
DateTime
ææ1 9
.
ææ9 :
Today
ææ: ?
)
ææ? @
{
øø 
throw
¿¿ 
new
¿¿ '
HealthRecordRuleException
¿¿ 3
(
¿¿3 4
$str
¿¿4 r
)
¿¿r s
;
¿¿s t
}
¡¡ 
if
√√ 
(
√√ 
dto
√√ 
.
√√ 
	VisitDate
√√ 
.
√√ 
Date
√√ "
!=
√√# %
appointment
√√& 1
.
√√1 2
ScheduledDate
√√2 ?
.
√√? @
Date
√√@ D
)
√√D E
{
ƒƒ 
throw
≈≈ 
new
≈≈ '
HealthRecordRuleException
≈≈ 3
(
≈≈3 4
$str
≈≈4 k
)
≈≈k l
;
≈≈l m
}
∆∆ &
ValidateHealthRecordText
»» $
(
»»$ %
dto
»»% (
.
»»( )
	Diagnosis
»») 2
,
»»2 3
dto
»»4 7
.
»»7 8
Prescription
»»8 D
,
»»D E
dto
»»F I
.
»»I J
Notes
»»J O
)
»»O P
;
»»P Q
mapper
   
.
   
Map
   
(
   
dto
   
,
   "
existingHealthRecord
   0
)
  0 1
;
  1 2"
existingHealthRecord
ÃÃ  
.
ÃÃ  !
HealthRecordId
ÃÃ! /
=
ÃÃ0 1
healthRecordId
ÃÃ2 @
;
ÃÃ@ A
var
ŒŒ !
updatedHealthRecord
ŒŒ #
=
ŒŒ$ %
await
ŒŒ& +$
healthRecordRepository
ŒŒ, B
.
ŒŒB C
UpdateAsync
ŒŒC N
(
ŒŒN O
healthRecordId
œœ 
,
œœ "
existingHealthRecord
–– $
)
––$ %
;
––% &
if
““ 
(
““ !
updatedHealthRecord
““ #
is
““$ &
null
““' +
)
““+ ,
{
”” 
throw
‘‘ 
new
‘‘ %
EntityNotFoundException
‘‘ 1
(
‘‘1 2
$str
‘‘2 @
,
‘‘@ A
healthRecordId
‘‘B P
)
‘‘P Q
;
‘‘Q R
}
’’ 
return
◊◊ 
mapper
◊◊ 
.
◊◊ 
Map
◊◊ 
<
◊◊ 
HealthRecordDto
◊◊ -
>
◊◊- .
(
◊◊. /!
updatedHealthRecord
◊◊/ B
)
◊◊B C
;
◊◊C D
}
ÿÿ 	
public
⁄⁄ 
async
⁄⁄ 
Task
⁄⁄ 
<
⁄⁄ 
HealthRecordDto
⁄⁄ )
>
⁄⁄) *%
DeleteHealthRecordAsync
⁄⁄+ B
(
⁄⁄B C
int
⁄⁄C F
healthRecordId
⁄⁄G U
)
⁄⁄U V
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
ﬁﬁ !
deletedHealthRecord
ﬁﬁ #
=
ﬁﬁ$ %
await
ﬁﬁ& +$
healthRecordRepository
ﬁﬁ, B
.
ﬁﬁB C
DeleteAsync
ﬁﬁC N
(
ﬁﬁN O
healthRecordId
ﬁﬁO ]
)
ﬁﬁ] ^
;
ﬁﬁ^ _
if
‡‡ 
(
‡‡ !
deletedHealthRecord
‡‡ #
is
‡‡$ &
null
‡‡' +
)
‡‡+ ,
{
·· 
throw
‚‚ 
new
‚‚ %
EntityNotFoundException
‚‚ 1
(
‚‚1 2
$str
‚‚2 @
,
‚‚@ A
healthRecordId
‚‚B P
)
‚‚P Q
;
‚‚Q R
}
„„ 
return
ÂÂ 
mapper
ÂÂ 
.
ÂÂ 
Map
ÂÂ 
<
ÂÂ 
HealthRecordDto
ÂÂ -
>
ÂÂ- .
(
ÂÂ. /!
deletedHealthRecord
ÂÂ/ B
)
ÂÂB C
;
ÂÂC D
}
ÊÊ 	
public
ÍÍ 
async
ÍÍ 
Task
ÍÍ 
<
ÍÍ 
List
ÍÍ 
<
ÍÍ 
HealthRecordDto
ÍÍ .
>
ÍÍ. /
>
ÍÍ/ 0/
!GetMyHealthRecordsForPatientAsync
ÍÍ1 R
(
ÍÍR S
string
ÍÍS Y
identityUserId
ÍÍZ h
)
ÍÍh i
{
ÎÎ 	
var
ÏÏ 
patient
ÏÏ 
=
ÏÏ 
await
ÏÏ %
GetLoggedInPatientAsync
ÏÏ  7
(
ÏÏ7 8
identityUserId
ÏÏ8 F
)
ÏÏF G
;
ÏÏG H
var
ÓÓ 
healthRecords
ÓÓ 
=
ÓÓ 
await
ÓÓ  %$
healthRecordRepository
ÓÓ& <
.
ÓÓ< =!
GetByPatientIdAsync
ÓÓ= P
(
ÓÓP Q
patient
ÓÓQ X
.
ÓÓX Y
	PatientId
ÓÓY b
)
ÓÓb c
;
ÓÓc d
return
 
mapper
 
.
 
Map
 
<
 
List
 "
<
" #
HealthRecordDto
# 2
>
2 3
>
3 4
(
4 5
healthRecords
5 B
)
B C
;
C D
}
ÒÒ 	
public
ÛÛ 
async
ÛÛ 
Task
ÛÛ 
<
ÛÛ 
HealthRecordDto
ÛÛ )
>
ÛÛ) *0
"GetHealthRecordByIdForPatientAsync
ÛÛ+ M
(
ÛÛM N
int
ÙÙ 
healthRecordId
ÙÙ 
,
ÙÙ 
string
ıı 
identityUserId
ıı !
)
ıı! "
{
ˆˆ 	$
ValidateHealthRecordId
˜˜ "
(
˜˜" #
healthRecordId
˜˜# 1
)
˜˜1 2
;
˜˜2 3
var
˘˘ 
patient
˘˘ 
=
˘˘ 
await
˘˘ %
GetLoggedInPatientAsync
˘˘  7
(
˘˘7 8
identityUserId
˘˘8 F
)
˘˘F G
;
˘˘G H
var
˚˚ 
healthRecord
˚˚ 
=
˚˚ 
await
˚˚ $$
healthRecordRepository
˚˚% ;
.
˚˚; <
GetByIdAsync
˚˚< H
(
˚˚H I
healthRecordId
˚˚I W
)
˚˚W X
;
˚˚X Y
if
˝˝ 
(
˝˝ 
healthRecord
˝˝ 
is
˝˝ 
null
˝˝  $
)
˝˝$ %
{
˛˛ 
throw
ˇˇ 
new
ˇˇ %
EntityNotFoundException
ˇˇ 1
(
ˇˇ1 2
$str
ˇˇ2 @
,
ˇˇ@ A
healthRecordId
ˇˇB P
)
ˇˇP Q
;
ˇˇQ R
}
ÄÄ 
if
ÇÇ 
(
ÇÇ 
healthRecord
ÇÇ 
.
ÇÇ 
	PatientId
ÇÇ &
!=
ÇÇ' )
patient
ÇÇ* 1
.
ÇÇ1 2
	PatientId
ÇÇ2 ;
)
ÇÇ; <
{
ÉÉ 
throw
ÑÑ 
new
ÑÑ &
ForbiddenAccessException
ÑÑ 2
(
ÑÑ2 3
$str
ÑÑ3 g
)
ÑÑg h
;
ÑÑh i
}
ÖÖ 
return
áá 
mapper
áá 
.
áá 
Map
áá 
<
áá 
HealthRecordDto
áá -
>
áá- .
(
áá. /
healthRecord
áá/ ;
)
áá; <
;
áá< =
}
àà 	
public
åå 
async
åå 
Task
åå 
<
åå 
List
åå 
<
åå 
HealthRecordDto
åå .
>
åå. /
>
åå/ 0.
 GetMyHealthRecordsForDoctorAsync
åå1 Q
(
ååQ R
string
ååR X
identityUserId
ååY g
)
ååg h
{
çç 	
var
éé 
doctor
éé 
=
éé 
await
éé $
GetLoggedInDoctorAsync
éé 5
(
éé5 6
identityUserId
éé6 D
)
ééD E
;
ééE F
var
êê 
healthRecords
êê 
=
êê 
await
êê  %$
healthRecordRepository
êê& <
.
êê< = 
GetByDoctorIdAsync
êê= O
(
êêO P
doctor
êêP V
.
êêV W
DoctorId
êêW _
)
êê_ `
;
êê` a
return
íí 
mapper
íí 
.
íí 
Map
íí 
<
íí 
List
íí "
<
íí" #
HealthRecordDto
íí# 2
>
íí2 3
>
íí3 4
(
íí4 5
healthRecords
íí5 B
)
ííB C
;
ííC D
}
ìì 	
public
ïï 
async
ïï 
Task
ïï 
<
ïï 
HealthRecordDto
ïï )
>
ïï) */
!GetHealthRecordByIdForDoctorAsync
ïï+ L
(
ïïL M
int
ññ 
healthRecordId
ññ 
,
ññ 
string
óó 
identityUserId
óó !
)
óó! "
{
òò 	$
ValidateHealthRecordId
ôô "
(
ôô" #
healthRecordId
ôô# 1
)
ôô1 2
;
ôô2 3
var
õõ 
doctor
õõ 
=
õõ 
await
õõ $
GetLoggedInDoctorAsync
õõ 5
(
õõ5 6
identityUserId
õõ6 D
)
õõD E
;
õõE F
var
ùù 
healthRecord
ùù 
=
ùù 
await
ùù $$
healthRecordRepository
ùù% ;
.
ùù; <
GetByIdAsync
ùù< H
(
ùùH I
healthRecordId
ùùI W
)
ùùW X
;
ùùX Y
if
üü 
(
üü 
healthRecord
üü 
is
üü 
null
üü  $
)
üü$ %
{
†† 
throw
°° 
new
°° %
EntityNotFoundException
°° 1
(
°°1 2
$str
°°2 @
,
°°@ A
healthRecordId
°°B P
)
°°P Q
;
°°Q R
}
¢¢ 
if
§§ 
(
§§ 
healthRecord
§§ 
.
§§ 
DoctorId
§§ %
!=
§§& (
doctor
§§) /
.
§§/ 0
DoctorId
§§0 8
)
§§8 9
{
•• 
throw
¶¶ 
new
¶¶ &
ForbiddenAccessException
¶¶ 2
(
¶¶2 3
$str
¶¶3 f
)
¶¶f g
;
¶¶g h
}
ßß 
return
©© 
mapper
©© 
.
©© 
Map
©© 
<
©© 
HealthRecordDto
©© -
>
©©- .
(
©©. /
healthRecord
©©/ ;
)
©©; <
;
©©< =
}
™™ 	
public
¨¨ 
async
¨¨ 
Task
¨¨ 
<
¨¨ 
List
¨¨ 
<
¨¨ 
HealthRecordDto
¨¨ .
>
¨¨. /
>
¨¨/ 0;
-GetHealthRecordsByAppointmentIdForDoctorAsync
¨¨1 ^
(
¨¨^ _
int
≠≠ 
appointmentId
≠≠ 
,
≠≠ 
string
ÆÆ 
identityUserId
ÆÆ !
)
ÆÆ! "
{
ØØ 	#
ValidateAppointmentId
∞∞ !
(
∞∞! "
appointmentId
∞∞" /
)
∞∞/ 0
;
∞∞0 1
var
≤≤ 
doctor
≤≤ 
=
≤≤ 
await
≤≤ $
GetLoggedInDoctorAsync
≤≤ 5
(
≤≤5 6
identityUserId
≤≤6 D
)
≤≤D E
;
≤≤E F
var
¥¥ 
appointment
¥¥ 
=
¥¥ 
await
¥¥ ##
appointmentRepository
¥¥$ 9
.
¥¥9 :
GetByIdAsync
¥¥: F
(
¥¥F G
appointmentId
¥¥G T
)
¥¥T U
;
¥¥U V
if
∂∂ 
(
∂∂ 
appointment
∂∂ 
is
∂∂ 
null
∂∂ #
)
∂∂# $
{
∑∑ 
throw
∏∏ 
new
∏∏ %
EntityNotFoundException
∏∏ 1
(
∏∏1 2
$str
∏∏2 ?
,
∏∏? @
appointmentId
∏∏A N
)
∏∏N O
;
∏∏O P
}
ππ 
if
ªª 
(
ªª 
appointment
ªª 
.
ªª 
DoctorId
ªª $
!=
ªª% '
doctor
ªª( .
.
ªª. /
DoctorId
ªª/ 7
)
ªª7 8
{
ºº 
throw
ΩΩ 
new
ΩΩ &
ForbiddenAccessException
ΩΩ 2
(
ΩΩ2 3
$str
ΩΩ3 w
)
ΩΩw x
;
ΩΩx y
}
ææ 
var
¿¿ 
healthRecords
¿¿ 
=
¿¿ 
await
¿¿  %$
healthRecordRepository
¿¿& <
.
¿¿< =%
GetByAppointmentIdAsync
¿¿= T
(
¿¿T U
appointmentId
¿¿U b
)
¿¿b c
;
¿¿c d
return
¬¬ 
mapper
¬¬ 
.
¬¬ 
Map
¬¬ 
<
¬¬ 
List
¬¬ "
<
¬¬" #
HealthRecordDto
¬¬# 2
>
¬¬2 3
>
¬¬3 4
(
¬¬4 5
healthRecords
¬¬5 B
)
¬¬B C
;
¬¬C D
}
√√ 	
public
≈≈ 
async
≈≈ 
Task
≈≈ 
<
≈≈ 
HealthRecordDto
≈≈ )
>
≈≈) *+
AddHealthRecordForDoctorAsync
≈≈+ H
(
≈≈H I 
AddHealthRecordDto
∆∆ 
dto
∆∆ "
,
∆∆" #
string
«« 
identityUserId
«« !
)
««! "
{
»» 	
if
…… 
(
…… 
dto
…… 
is
…… 
null
…… 
)
…… 
{
   
throw
ÀÀ 
new
ÀÀ '
HealthRecordRuleException
ÀÀ 3
(
ÀÀ3 4
$str
ÀÀ4 Y
)
ÀÀY Z
;
ÀÀZ [
}
ÃÃ 
var
ŒŒ 
doctor
ŒŒ 
=
ŒŒ 
await
ŒŒ $
GetLoggedInDoctorAsync
ŒŒ 5
(
ŒŒ5 6
identityUserId
ŒŒ6 D
)
ŒŒD E
;
ŒŒE F#
ValidateAppointmentId
–– !
(
––! "
dto
––" %
.
––% &
AppointmentId
––& 3
)
––3 4
;
––4 5
var
““ 
appointment
““ 
=
““ 
await
““ ##
appointmentRepository
““$ 9
.
““9 :
GetByIdAsync
““: F
(
““F G
dto
““G J
.
““J K
AppointmentId
““K X
)
““X Y
;
““Y Z
if
‘‘ 
(
‘‘ 
appointment
‘‘ 
is
‘‘ 
null
‘‘ #
)
‘‘# $
{
’’ 
throw
÷÷ 
new
÷÷ %
EntityNotFoundException
÷÷ 1
(
÷÷1 2
$str
÷÷2 ?
,
÷÷? @
dto
÷÷A D
.
÷÷D E
AppointmentId
÷÷E R
)
÷÷R S
;
÷÷S T
}
◊◊ 
if
ŸŸ 
(
ŸŸ 
appointment
ŸŸ 
.
ŸŸ 
DoctorId
ŸŸ $
!=
ŸŸ% '
doctor
ŸŸ( .
.
ŸŸ. /
DoctorId
ŸŸ/ 7
)
ŸŸ7 8
{
⁄⁄ 
throw
€€ 
new
€€ &
ForbiddenAccessException
€€ 2
(
€€2 3
$str
€€3 t
)
€€t u
;
€€u v
}
‹‹ 
if
ﬁﬁ 
(
ﬁﬁ 
dto
ﬁﬁ 
.
ﬁﬁ 
	PatientId
ﬁﬁ 
!=
ﬁﬁ  
appointment
ﬁﬁ! ,
.
ﬁﬁ, -
	PatientId
ﬁﬁ- 6
)
ﬁﬁ6 7
{
ﬂﬂ 
throw
‡‡ 
new
‡‡ '
HealthRecordRuleException
‡‡ 3
(
‡‡3 4
$str
‡‡4 o
)
‡‡o p
;
‡‡p q
}
·· 
if
„„ 
(
„„ 
dto
„„ 
.
„„ 
DoctorId
„„ 
is
„„ 
not
„„  #
null
„„$ (
&&
„„) +
dto
„„, /
.
„„/ 0
DoctorId
„„0 8
.
„„8 9
Value
„„9 >
!=
„„? A
doctor
„„B H
.
„„H I
DoctorId
„„I Q
)
„„Q R
{
‰‰ 
throw
ÂÂ 
new
ÂÂ &
ForbiddenAccessException
ÂÂ 2
(
ÂÂ2 3
$str
ÂÂ3 j
)
ÂÂj k
;
ÂÂk l
}
ÊÊ 
dto
ËË 
.
ËË 
DoctorId
ËË 
=
ËË 
doctor
ËË !
.
ËË! "
DoctorId
ËË" *
;
ËË* +
return
ÍÍ 
await
ÍÍ "
AddHealthRecordAsync
ÍÍ -
(
ÍÍ- .
dto
ÍÍ. 1
)
ÍÍ1 2
;
ÍÍ2 3
}
ÎÎ 	
public
ÌÌ 
async
ÌÌ 
Task
ÌÌ 
<
ÌÌ 
HealthRecordDto
ÌÌ )
>
ÌÌ) *.
 UpdateHealthRecordForDoctorAsync
ÌÌ+ K
(
ÌÌK L
int
ÓÓ 
healthRecordId
ÓÓ 
,
ÓÓ #
UpdateHealthRecordDto
ÔÔ !
dto
ÔÔ" %
,
ÔÔ% &
string
 
identityUserId
 !
)
! "
{
ÒÒ 	$
ValidateHealthRecordId
ÚÚ "
(
ÚÚ" #
healthRecordId
ÚÚ# 1
)
ÚÚ1 2
;
ÚÚ2 3
if
ÙÙ 
(
ÙÙ 
dto
ÙÙ 
is
ÙÙ 
null
ÙÙ 
)
ÙÙ 
{
ıı 
throw
ˆˆ 
new
ˆˆ '
HealthRecordRuleException
ˆˆ 3
(
ˆˆ3 4
$str
ˆˆ4 Y
)
ˆˆY Z
;
ˆˆZ [
}
˜˜ 
var
˘˘ 
doctor
˘˘ 
=
˘˘ 
await
˘˘ $
GetLoggedInDoctorAsync
˘˘ 5
(
˘˘5 6
identityUserId
˘˘6 D
)
˘˘D E
;
˘˘E F
var
˚˚ 
healthRecord
˚˚ 
=
˚˚ 
await
˚˚ $$
healthRecordRepository
˚˚% ;
.
˚˚; <
GetByIdAsync
˚˚< H
(
˚˚H I
healthRecordId
˚˚I W
)
˚˚W X
;
˚˚X Y
if
˝˝ 
(
˝˝ 
healthRecord
˝˝ 
is
˝˝ 
null
˝˝  $
)
˝˝$ %
{
˛˛ 
throw
ˇˇ 
new
ˇˇ %
EntityNotFoundException
ˇˇ 1
(
ˇˇ1 2
$str
ˇˇ2 @
,
ˇˇ@ A
healthRecordId
ˇˇB P
)
ˇˇP Q
;
ˇˇQ R
}
ÄÄ 
if
ÇÇ 
(
ÇÇ 
healthRecord
ÇÇ 
.
ÇÇ 
DoctorId
ÇÇ %
!=
ÇÇ& (
doctor
ÇÇ) /
.
ÇÇ/ 0
DoctorId
ÇÇ0 8
)
ÇÇ8 9
{
ÉÉ 
throw
ÑÑ 
new
ÑÑ &
ForbiddenAccessException
ÑÑ 2
(
ÑÑ2 3
$str
ÑÑ3 f
)
ÑÑf g
;
ÑÑg h
}
ÖÖ 
return
áá 
await
áá %
UpdateHealthRecordAsync
áá 0
(
áá0 1
healthRecordId
áá1 ?
,
áá? @
dto
ááA D
)
ááD E
;
ááE F
}
àà 	
private
åå 
async
åå 
Task
åå 
<
åå 
Patient
åå "
>
åå" #%
GetLoggedInPatientAsync
åå$ ;
(
åå; <
string
åå< B
identityUserId
ååC Q
)
ååQ R
{
çç 	
if
éé 
(
éé 
string
éé 
.
éé  
IsNullOrWhiteSpace
éé )
(
éé) *
identityUserId
éé* 8
)
éé8 9
)
éé9 :
{
èè 
throw
êê 
new
êê #
BusinessRuleException
êê /
(
êê/ 0
$str
êê0 I
)
êêI J
;
êêJ K
}
ëë 
var
ìì 
patient
ìì 
=
ìì 
await
ìì 
patientRepository
ìì  1
.
ìì1 2&
GetByIdentityUserIdAsync
ìì2 J
(
ììJ K
identityUserId
ììK Y
)
ììY Z
;
ììZ [
if
ïï 
(
ïï 
patient
ïï 
is
ïï 
null
ïï 
)
ïï  
{
ññ 
throw
óó 
new
óó %
EntityNotFoundException
óó 1
(
óó1 2
$str
óó2 V
,
óóV W
$num
óóX Y
)
óóY Z
;
óóZ [
}
òò 
return
öö 
patient
öö 
;
öö 
}
õõ 	
private
ùù 
async
ùù 
Task
ùù 
<
ùù 
Doctor
ùù !
>
ùù! "$
GetLoggedInDoctorAsync
ùù# 9
(
ùù9 :
string
ùù: @
identityUserId
ùùA O
)
ùùO P
{
ûû 	
if
üü 
(
üü 
string
üü 
.
üü  
IsNullOrWhiteSpace
üü )
(
üü) *
identityUserId
üü* 8
)
üü8 9
)
üü9 :
{
†† 
throw
°° 
new
°° #
BusinessRuleException
°° /
(
°°/ 0
$str
°°0 I
)
°°I J
;
°°J K
}
¢¢ 
var
§§ 
doctor
§§ 
=
§§ 
await
§§ 
doctorRepository
§§ /
.
§§/ 0&
GetByIdentityUserIdAsync
§§0 H
(
§§H I
identityUserId
§§I W
)
§§W X
;
§§X Y
if
¶¶ 
(
¶¶ 
doctor
¶¶ 
is
¶¶ 
null
¶¶ 
)
¶¶ 
{
ßß 
throw
®® 
new
®® %
EntityNotFoundException
®® 1
(
®®1 2
$str
®®2 U
,
®®U V
$num
®®W X
)
®®X Y
;
®®Y Z
}
©© 
return
´´ 
doctor
´´ 
;
´´ 
}
¨¨ 	
private
ÆÆ 
async
ÆÆ 
Task
ÆÆ 
<
ÆÆ 
Appointment
ÆÆ &
>
ÆÆ& '+
GetAppointmentEntityByIdAsync
ÆÆ( E
(
ÆÆE F
int
ÆÆF I
appointmentId
ÆÆJ W
)
ÆÆW X
{
ØØ 	#
ValidateAppointmentId
∞∞ !
(
∞∞! "
appointmentId
∞∞" /
)
∞∞/ 0
;
∞∞0 1
var
≤≤ 
appointment
≤≤ 
=
≤≤ 
await
≤≤ ##
appointmentRepository
≤≤$ 9
.
≤≤9 :
GetByIdAsync
≤≤: F
(
≤≤F G
appointmentId
≤≤G T
)
≤≤T U
;
≤≤U V
if
¥¥ 
(
¥¥ 
appointment
¥¥ 
is
¥¥ 
null
¥¥ #
)
¥¥# $
{
µµ 
throw
∂∂ 
new
∂∂ %
EntityNotFoundException
∂∂ 1
(
∂∂1 2
$str
∂∂2 ?
,
∂∂? @
appointmentId
∂∂A N
)
∂∂N O
;
∂∂O P
}
∑∑ 
return
ππ 
appointment
ππ 
;
ππ 
}
∫∫ 	
private
ºº 
async
ºº 
Task
ºº (
ValidatePatientExistsAsync
ºº 5
(
ºº5 6
int
ºº6 9
	patientId
ºº: C
)
ººC D
{
ΩΩ 	
ValidatePatientId
ææ 
(
ææ 
	patientId
ææ '
)
ææ' (
;
ææ( )
var
¿¿ 
patient
¿¿ 
=
¿¿ 
await
¿¿ 
patientRepository
¿¿  1
.
¿¿1 2
GetByIdAsync
¿¿2 >
(
¿¿> ?
	patientId
¿¿? H
)
¿¿H I
;
¿¿I J
if
¬¬ 
(
¬¬ 
patient
¬¬ 
is
¬¬ 
null
¬¬ 
)
¬¬  
{
√√ 
throw
ƒƒ 
new
ƒƒ %
EntityNotFoundException
ƒƒ 1
(
ƒƒ1 2
$str
ƒƒ2 ;
,
ƒƒ; <
	patientId
ƒƒ= F
)
ƒƒF G
;
ƒƒG H
}
≈≈ 
}
∆∆ 	
private
»» 
async
»» 
Task
»» '
ValidateDoctorExistsAsync
»» 4
(
»»4 5
int
»»5 8
doctorId
»»9 A
)
»»A B
{
…… 	
ValidateDoctorId
   
(
   
doctorId
   %
)
  % &
;
  & '
var
ÃÃ 
doctor
ÃÃ 
=
ÃÃ 
await
ÃÃ 
doctorRepository
ÃÃ /
.
ÃÃ/ 0
GetByIdAsync
ÃÃ0 <
(
ÃÃ< =
doctorId
ÃÃ= E
)
ÃÃE F
;
ÃÃF G
if
ŒŒ 
(
ŒŒ 
doctor
ŒŒ 
is
ŒŒ 
null
ŒŒ 
)
ŒŒ 
{
œœ 
throw
–– 
new
–– %
EntityNotFoundException
–– 1
(
––1 2
$str
––2 :
,
––: ;
doctorId
––< D
)
––D E
;
––E F
}
—— 
}
““ 	
private
‘‘ 
void
‘‘ $
ValidateHealthRecordId
‘‘ +
(
‘‘+ ,
int
‘‘, /
healthRecordId
‘‘0 >
)
‘‘> ?
{
’’ 	
if
÷÷ 
(
÷÷ 
healthRecordId
÷÷ 
<=
÷÷ !
$num
÷÷" #
)
÷÷# $
{
◊◊ 
throw
ÿÿ 
new
ÿÿ '
HealthRecordRuleException
ÿÿ 3
(
ÿÿ3 4
$str
ÿÿ4 e
)
ÿÿe f
;
ÿÿf g
}
ŸŸ 
}
⁄⁄ 	
private
‹‹ 
void
‹‹ 
ValidatePatientId
‹‹ &
(
‹‹& '
int
‹‹' *
	patientId
‹‹+ 4
)
‹‹4 5
{
›› 	
if
ﬁﬁ 
(
ﬁﬁ 
	patientId
ﬁﬁ 
<=
ﬁﬁ 
$num
ﬁﬁ 
)
ﬁﬁ 
{
ﬂﬂ 
throw
‡‡ 
new
‡‡ '
HealthRecordRuleException
‡‡ 3
(
‡‡3 4
$str
‡‡4 _
)
‡‡_ `
;
‡‡` a
}
·· 
}
‚‚ 	
private
‰‰ 
void
‰‰ 
ValidateDoctorId
‰‰ %
(
‰‰% &
int
‰‰& )
doctorId
‰‰* 2
)
‰‰2 3
{
ÂÂ 	
if
ÊÊ 
(
ÊÊ 
doctorId
ÊÊ 
<=
ÊÊ 
$num
ÊÊ 
)
ÊÊ 
{
ÁÁ 
throw
ËË 
new
ËË '
HealthRecordRuleException
ËË 3
(
ËË3 4
$str
ËË4 ^
)
ËË^ _
;
ËË_ `
}
ÈÈ 
}
ÍÍ 	
private
ÏÏ 
void
ÏÏ #
ValidateAppointmentId
ÏÏ *
(
ÏÏ* +
int
ÏÏ+ .
appointmentId
ÏÏ/ <
)
ÏÏ< =
{
ÌÌ 	
if
ÓÓ 
(
ÓÓ 
appointmentId
ÓÓ 
<=
ÓÓ  
$num
ÓÓ! "
)
ÓÓ" #
{
ÔÔ 
throw
 
new
 '
HealthRecordRuleException
 3
(
3 4
$str
4 c
)
c d
;
d e
}
ÒÒ 
}
ÚÚ 	
private
ÙÙ 
void
ÙÙ &
ValidateHealthRecordText
ÙÙ -
(
ÙÙ- .
string
ıı 
	diagnosis
ıı 
,
ıı 
string
ˆˆ 
prescription
ˆˆ 
,
ˆˆ  
string
˜˜ 
?
˜˜ 
notes
˜˜ 
)
˜˜ 
{
¯¯ 	
if
˘˘ 
(
˘˘ 
string
˘˘ 
.
˘˘  
IsNullOrWhiteSpace
˘˘ )
(
˘˘) *
	diagnosis
˘˘* 3
)
˘˘3 4
)
˘˘4 5
{
˙˙ 
throw
˚˚ 
new
˚˚ '
HealthRecordRuleException
˚˚ 3
(
˚˚3 4
$str
˚˚4 U
)
˚˚U V
;
˚˚V W
}
¸¸ 
if
˛˛ 
(
˛˛ 
	diagnosis
˛˛ 
.
˛˛ 
Trim
˛˛ 
(
˛˛ 
)
˛˛  
.
˛˛  !
Length
˛˛! '
>
˛˛( )
$num
˛˛* -
)
˛˛- .
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ '
HealthRecordRuleException
ÄÄ 3
(
ÄÄ3 4
$str
ÄÄ4 g
)
ÄÄg h
;
ÄÄh i
}
ÅÅ 
if
ÉÉ 
(
ÉÉ 
string
ÉÉ 
.
ÉÉ  
IsNullOrWhiteSpace
ÉÉ )
(
ÉÉ) *
prescription
ÉÉ* 6
)
ÉÉ6 7
)
ÉÉ7 8
{
ÑÑ 
throw
ÖÖ 
new
ÖÖ '
HealthRecordRuleException
ÖÖ 3
(
ÖÖ3 4
$str
ÖÖ4 X
)
ÖÖX Y
;
ÖÖY Z
}
ÜÜ 
if
àà 
(
àà 
prescription
àà 
.
àà 
Trim
àà !
(
àà! "
)
àà" #
.
àà# $
Length
àà$ *
>
àà+ ,
$num
àà- 0
)
àà0 1
{
ââ 
throw
ää 
new
ää '
HealthRecordRuleException
ää 3
(
ää3 4
$str
ää4 j
)
ääj k
;
ääk l
}
ãã 
if
çç 
(
çç 
!
çç 
string
çç 
.
çç  
IsNullOrWhiteSpace
çç *
(
çç* +
notes
çç+ 0
)
çç0 1
&&
çç2 4
notes
çç5 :
.
çç: ;
Trim
çç; ?
(
çç? @
)
çç@ A
.
ççA B
Length
ççB H
>
ççI J
$num
ççK O
)
ççO P
{
éé 
throw
èè 
new
èè '
HealthRecordRuleException
èè 3
(
èè3 4
$str
èè4 g
)
èèg h
;
èèh i
}
êê 
}
ëë 	
}
íí 
}ìì Â¯
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\DoctorService.cs
	namespace

 	
HealthCareApp


 
.

 
Services

  
{ 
public 

class 
DoctorService 
( 
IDoctorRepository 

repository $
,$ %
IMapper 
mapper 
, 
UserManager 
< 
IdentityUser  
>  !
userManager" -
,- .
RoleManager 
< 
IdentityRole  
>  !
roleManager" -
)- .
:/ 0
IDoctorService1 ?
{ 
private 
const 
string 
DoctorEntityName -
=. /
$str0 8
;8 9
public 
async 
Task 
< 
List 
< 
	DoctorDto (
>( )
>) *
GetAllDoctorsAsync+ =
(= >
)> ?
{ 	
var 
doctors 
= 
await 

repository  *
.* +
GetAllAsync+ 6
(6 7
)7 8
;8 9
return 
mapper 
. 
Map 
< 
List "
<" #
	DoctorDto# ,
>, -
>- .
(. /
doctors/ 6
)6 7
;7 8
} 	
public 
async 
Task 
< 
PagedResponse '
<' (
	DoctorDto( 1
>1 2
>2 3#
GetAllDoctorsPagedAsync4 K
(K L$
DoctorPaginationQueryDtoL d
querye j
)j k
{ 	
if 
( 
query 
is 
null 
) 
{   
query!! 
=!! 
new!! $
DoctorPaginationQueryDto!! 4
(!!4 5
)!!5 6
;!!6 7
}"" 
int$$ 

pageNumber$$ 
=$$ 
query$$ "
.$$" #

PageNumber$$# -
<=$$. 0
$num$$1 2
?$$3 4
$num$$5 6
:$$7 8
query$$9 >
.$$> ?

PageNumber$$? I
;$$I J
int&& 
pageSize&& 
=&& 
query&&  
.&&  !
PageSize&&! )
<=&&* ,
$num&&- .
?&&/ 0
$num&&1 3
:&&4 5
query&&6 ;
.&&; <
PageSize&&< D
;&&D E
pageSize(( 
=(( 
pageSize(( 
>((  !
$num((" %
?((& '
$num((( +
:((, -
pageSize((. 6
;((6 7
var** 
doctors** 
=** 
await** 

repository**  *
.*** +
GetAllAsync**+ 6
(**6 7
)**7 8
;**8 9
var,, 
filteredDoctors,, 
=,,  !
doctors,," )
.,,) *
AsEnumerable,,* 6
(,,6 7
),,7 8
;,,8 9
if.. 
(.. 
!.. 
string.. 
... 
IsNullOrWhiteSpace.. *
(..* +
query..+ 0
...0 1

SearchTerm..1 ;
)..; <
)..< =
{// 
string00 

searchTerm00 !
=00" #
query00$ )
.00) *

SearchTerm00* 4
.004 5
Trim005 9
(009 :
)00: ;
;00; <
filteredDoctors22 
=22  !
filteredDoctors22" 1
.221 2
Where222 7
(227 8
d228 9
=>22: <
d33 
.33 

DoctorName33  
.33  !
Contains33! )
(33) *

searchTerm33* 4
,334 5
StringComparison336 F
.33F G
OrdinalIgnoreCase33G X
)33X Y
||33Z \
d44 
.44 
Email44 
.44 
Contains44 $
(44$ %

searchTerm44% /
,44/ 0
StringComparison441 A
.44A B
OrdinalIgnoreCase44B S
)44S T
)44T U
;44U V
}55 
if77 
(77 
query77 
.77 
Specialisation77 $
is77% '
not77( +
null77, 0
)770 1
{88 
filteredDoctors99 
=99  !
filteredDoctors99" 1
.991 2
Where992 7
(997 8
d998 9
=>99: <
d:: 
.:: 
Specialisation:: $
==::% '
query::( -
.::- .
Specialisation::. <
.::< =
Value::= B
)::B C
;::C D
};; 
if== 
(== 
query== 
.== 
IsActive== 
is== !
not==" %
null==& *
)==* +
{>> 
filteredDoctors?? 
=??  !
filteredDoctors??" 1
.??1 2
Where??2 7
(??7 8
d??8 9
=>??: <
d@@ 
.@@ 
IsActive@@ 
==@@ !
query@@" '
.@@' (
IsActive@@( 0
.@@0 1
Value@@1 6
)@@6 7
;@@7 8
}AA 
intCC 
totalRecordsCC 
=CC 
filteredDoctorsCC .
.CC. /
CountCC/ 4
(CC4 5
)CC5 6
;CC6 7
varEE 
pagedDoctorsEE 
=EE 
filteredDoctorsEE .
.FF 
OrderByFF 
(FF 
dFF 
=>FF 
dFF 
.FF  
DoctorIdFF  (
)FF( )
.GG 
SkipGG 
(GG 
(GG 

pageNumberGG !
-GG" #
$numGG$ %
)GG% &
*GG' (
pageSizeGG) 1
)GG1 2
.HH 
TakeHH 
(HH 
pageSizeHH 
)HH 
.II 
ToListII 
(II 
)II 
;II 
varKK 
mappedDoctorsKK 
=KK 
mapperKK  &
.KK& '
MapKK' *
<KK* +
ListKK+ /
<KK/ 0
	DoctorDtoKK0 9
>KK9 :
>KK: ;
(KK; <
pagedDoctorsKK< H
)KKH I
;KKI J
returnMM 
newMM 
PagedResponseMM $
<MM$ %
	DoctorDtoMM% .
>MM. /
{NN 
ItemsOO 
=OO 
mappedDoctorsOO %
,OO% &

PageNumberPP 
=PP 

pageNumberPP '
,PP' (
PageSizeQQ 
=QQ 
pageSizeQQ #
,QQ# $
TotalRecordsRR 
=RR 
totalRecordsRR +
,RR+ ,

TotalPagesSS 
=SS 
(SS 
intSS !
)SS! "
MathSS" &
.SS& '
CeilingSS' .
(SS. /
totalRecordsSS/ ;
/SS< =
(SS> ?
doubleSS? E
)SSE F
pageSizeSSF N
)SSN O
}TT 
;TT 
}UU 	
publicWW 
asyncWW 
TaskWW 
<WW 
ListWW 
<WW 
	DoctorDtoWW (
>WW( )
>WW) *$
GetAllActiveDoctorsAsyncWW+ C
(WWC D
)WWD E
{XX 	
varYY 
doctorsYY 
=YY 
awaitYY 

repositoryYY  *
.YY* +
GetAllActiveAsyncYY+ <
(YY< =
)YY= >
;YY> ?
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
>^^# $
GetDoctorByIdAsync^^% 7
(^^7 8
int^^8 ;
doctorId^^< D
)^^D E
{__ 	
ValidateDoctorId`` 
(`` 
doctorId`` %
)``% &
;``& '
varbb 
doctorbb 
=bb 
awaitbb 

repositorybb )
.bb) *
GetByIdAsyncbb* 6
(bb6 7
doctorIdbb7 ?
)bb? @
;bb@ A
ifdd 
(dd 
doctordd 
isdd 
nulldd 
)dd 
{ee 
throwff 
newff #
EntityNotFoundExceptionff 1
(ff1 2
DoctorEntityNameff2 B
,ffB C
doctorIdffD L
)ffL M
;ffM N
}gg 
returnii 
mapperii 
.ii 
Mapii 
<ii 
	DoctorDtoii '
>ii' (
(ii( )
doctorii) /
)ii/ 0
;ii0 1
}jj 	
publicll 
asyncll 
Taskll 
<ll 
Listll 
<ll 
	DoctorDtoll (
>ll( )
>ll) *+
GetDoctorsBySpecialisationAsyncll+ J
(llJ K
SpecialisationTypellK ]
specialisationll^ l
)lll m
{mm 	
varnn 
doctorsnn 
=nn 
awaitnn 

repositorynn  *
.nn* +$
GetBySpecialisationAsyncnn+ C
(nnC D
specialisationnnD R
)nnR S
;nnS T
returnpp 
mapperpp 
.pp 
Mappp 
<pp 
Listpp "
<pp" #
	DoctorDtopp# ,
>pp, -
>pp- .
(pp. /
doctorspp/ 6
)pp6 7
;pp7 8
}qq 	
publicss 
asyncss 
Taskss 
<ss 
Listss 
<ss 
	DoctorDtoss (
>ss( )
>ss) *1
%GetActiveDoctorsBySpecialisationAsyncss+ P
(ssP Q
SpecialisationTypessQ c
specialisationssd r
)ssr s
{tt 	
varuu 
doctorsuu 
=uu 
awaituu 

repositoryuu  *
.uu* +*
GetActiveBySpecialisationAsyncuu+ I
(uuI J
specialisationuuJ X
)uuX Y
;uuY Z
returnww 
mapperww 
.ww 
Mapww 
<ww 
Listww "
<ww" #
	DoctorDtoww# ,
>ww, -
>ww- .
(ww. /
doctorsww/ 6
)ww6 7
;ww7 8
}xx 	
publiczz 
asynczz 
Taskzz 
<zz $
DoctorCreatedResponseDtozz 2
>zz2 3$
CreateDoctorByAdminAsynczz4 L
(zzL M
CreateDoctorDtozzM \
dtozz] `
)zz` a
{{{ 	#
ValidateCreateDoctorDto|| #
(||# $
dto||$ '
)||' (
;||( )
string~~ 
normalizedEmail~~ "
=~~# $
dto~~% (
.~~( )
Email~~) .
.~~. /
Trim~~/ 3
(~~3 4
)~~4 5
.~~5 6
ToLower~~6 =
(~~= >
)~~> ?
;~~? @
bool
ÄÄ 
doctorEmailExists
ÄÄ "
=
ÄÄ# $
await
ÄÄ% *

repository
ÄÄ+ 5
.
ÄÄ5 6 
ExistsByEmailAsync
ÄÄ6 H
(
ÄÄH I
normalizedEmail
ÄÄI X
)
ÄÄX Y
;
ÄÄY Z
if
ÇÇ 
(
ÇÇ 
doctorEmailExists
ÇÇ !
)
ÇÇ! "
{
ÉÉ 
throw
ÑÑ 
new
ÑÑ 
ConflictException
ÑÑ +
(
ÑÑ+ ,
$str
ÑÑ, V
)
ÑÑV W
;
ÑÑW X
}
ÖÖ 
var
áá "
existingIdentityUser
áá $
=
áá% &
await
áá' ,
userManager
áá- 8
.
áá8 9
FindByEmailAsync
áá9 I
(
ááI J
normalizedEmail
ááJ Y
)
ááY Z
;
ááZ [
if
ââ 
(
ââ "
existingIdentityUser
ââ $
is
ââ% '
not
ââ( +
null
ââ, 0
)
ââ0 1
{
ää 
throw
ãã 
new
ãã 
ConflictException
ãã +
(
ãã+ ,
$str
ãã, ]
)
ãã] ^
;
ãã^ _
}
åå 
string
éé 
temporaryPassword
éé $
=
éé% &'
GenerateTemporaryPassword
éé' @
(
éé@ A
dto
ééA D
.
ééD E
FullName
ééE M
)
ééM N
;
ééN O
var
êê 
identityUser
êê 
=
êê 
new
êê "
IdentityUser
êê# /
{
ëë 
UserName
íí 
=
íí 
normalizedEmail
íí *
,
íí* +
Email
ìì 
=
ìì 
normalizedEmail
ìì '
,
ìì' (
EmailConfirmed
îî 
=
îî  
true
îî! %
}
ïï 
;
ïï 
var
óó 
createUserResult
óó  
=
óó! "
await
óó# (
userManager
óó) 4
.
óó4 5
CreateAsync
óó5 @
(
óó@ A
identityUser
óóA M
,
óóM N
temporaryPassword
óóO `
)
óó` a
;
óóa b
if
ôô 
(
ôô 
!
ôô 
createUserResult
ôô !
.
ôô! "
	Succeeded
ôô" +
)
ôô+ ,
{
öö 
var
õõ 
errors
õõ 
=
õõ 
string
õõ #
.
õõ# $
Join
õõ$ (
(
õõ( )
$str
õõ) ,
,
õõ, -
createUserResult
õõ. >
.
õõ> ?
Errors
õõ? E
.
õõE F
Select
õõF L
(
õõL M
e
õõM N
=>
õõO Q
e
õõR S
.
õõS T
Description
õõT _
)
õõ_ `
)
õõ` a
;
õõa b
throw
úú 
new
úú #
BusinessRuleException
úú /
(
úú/ 0
errors
úú0 6
)
úú6 7
;
úú7 8
}
ùù 
if
üü 
(
üü 
!
üü 
await
üü 
roleManager
üü "
.
üü" #
RoleExistsAsync
üü# 2
(
üü2 3
$str
üü3 ;
)
üü; <
)
üü< =
{
†† 
await
°° 
roleManager
°° !
.
°°! "
CreateAsync
°°" -
(
°°- .
new
°°. 1
IdentityRole
°°2 >
(
°°> ?
$str
°°? G
)
°°G H
)
°°H I
;
°°I J
}
¢¢ 
var
§§ 

roleResult
§§ 
=
§§ 
await
§§ "
userManager
§§# .
.
§§. /
AddToRoleAsync
§§/ =
(
§§= >
identityUser
§§> J
,
§§J K
$str
§§L T
)
§§T U
;
§§U V
if
¶¶ 
(
¶¶ 
!
¶¶ 

roleResult
¶¶ 
.
¶¶ 
	Succeeded
¶¶ %
)
¶¶% &
{
ßß 
await
®® 
userManager
®® !
.
®®! "
DeleteAsync
®®" -
(
®®- .
identityUser
®®. :
)
®®: ;
;
®®; <
var
™™ 
errors
™™ 
=
™™ 
string
™™ #
.
™™# $
Join
™™$ (
(
™™( )
$str
™™) ,
,
™™, -

roleResult
™™. 8
.
™™8 9
Errors
™™9 ?
.
™™? @
Select
™™@ F
(
™™F G
e
™™G H
=>
™™I K
e
™™L M
.
™™M N
Description
™™N Y
)
™™Y Z
)
™™Z [
;
™™[ \
throw
´´ 
new
´´ #
BusinessRuleException
´´ /
(
´´/ 0
errors
´´0 6
)
´´6 7
;
´´7 8
}
¨¨ 
var
ÆÆ 
doctor
ÆÆ 
=
ÆÆ 
mapper
ÆÆ 
.
ÆÆ  
Map
ÆÆ  #
<
ÆÆ# $
Doctor
ÆÆ$ *
>
ÆÆ* +
(
ÆÆ+ ,
dto
ÆÆ, /
)
ÆÆ/ 0
;
ÆÆ0 1
doctor
∞∞ 
.
∞∞ 

DoctorName
∞∞ 
=
∞∞ 
dto
∞∞  #
.
∞∞# $
FullName
∞∞$ ,
.
∞∞, -
Trim
∞∞- 1
(
∞∞1 2
)
∞∞2 3
;
∞∞3 4
doctor
±± 
.
±± 
Email
±± 
=
±± 
normalizedEmail
±± *
;
±±* +
doctor
≤≤ 
.
≤≤ 
YearsOfExperience
≤≤ $
=
≤≤% &(
CalculateYearsOfExperience
≤≤' A
(
≤≤A B
dto
≤≤B E
.
≤≤E F
PracticeStartDate
≤≤F W
)
≤≤W X
;
≤≤X Y
doctor
≥≥ 
.
≥≥ 
IsActive
≥≥ 
=
≥≥ 
true
≥≥ "
;
≥≥" #
doctor
¥¥ 
.
¥¥ 
IdentityUserId
¥¥ !
=
¥¥" #
identityUser
¥¥$ 0
.
¥¥0 1
Id
¥¥1 3
;
¥¥3 4
doctor
µµ 
.
µµ 
CreatedDate
µµ 
=
µµ  
DateTime
µµ! )
.
µµ) *
Now
µµ* -
;
µµ- .
var
∑∑ 
savedDoctor
∑∑ 
=
∑∑ 
await
∑∑ #

repository
∑∑$ .
.
∑∑. /
CreateAsync
∑∑/ :
(
∑∑: ;
doctor
∑∑; A
)
∑∑A B
;
∑∑B C
return
ππ 
new
ππ &
DoctorCreatedResponseDto
ππ /
{
∫∫ 
DoctorId
ªª 
=
ªª 
savedDoctor
ªª &
.
ªª& '
DoctorId
ªª' /
,
ªª/ 0

DoctorName
ºº 
=
ºº 
savedDoctor
ºº (
.
ºº( )

DoctorName
ºº) 3
,
ºº3 4
Email
ΩΩ 
=
ΩΩ 
savedDoctor
ΩΩ #
.
ΩΩ# $
Email
ΩΩ$ )
,
ΩΩ) *
TemporaryPassword
ææ !
=
ææ" #
temporaryPassword
ææ$ 5
,
ææ5 6
Message
øø 
=
øø 
$str
øø @
}
¿¿ 
;
¿¿ 
}
¡¡ 	
public
√√ 
async
√√ 
Task
√√ 
<
√√ 
	DoctorDto
√√ #
>
√√# $
UpdateDoctorAsync
√√% 6
(
√√6 7
int
√√7 :
doctorId
√√; C
,
√√C D
UpdateDoctorDto
√√E T
dto
√√U X
)
√√X Y
{
ƒƒ 	
ValidateDoctorId
≈≈ 
(
≈≈ 
doctorId
≈≈ %
)
≈≈% &
;
≈≈& '%
ValidateUpdateDoctorDto
«« #
(
««# $
dto
««$ '
)
««' (
;
««( )
var
…… 
existingDoctor
…… 
=
……  
await
……! &

repository
……' 1
.
……1 2
GetByIdAsync
……2 >
(
……> ?
doctorId
……? G
)
……G H
;
……H I
if
ÀÀ 
(
ÀÀ 
existingDoctor
ÀÀ 
is
ÀÀ !
null
ÀÀ" &
)
ÀÀ& '
{
ÃÃ 
throw
ÕÕ 
new
ÕÕ %
EntityNotFoundException
ÕÕ 1
(
ÕÕ1 2
$str
ÕÕ2 :
,
ÕÕ: ;
doctorId
ÕÕ< D
)
ÕÕD E
;
ÕÕE F
}
ŒŒ 
var
–– 
doctor
–– 
=
–– 
mapper
–– 
.
––  
Map
––  #
<
––# $
Doctor
––$ *
>
––* +
(
––+ ,
dto
––, /
)
––/ 0
;
––0 1
doctor
““ 
.
““ 
DoctorId
““ 
=
““ 
doctorId
““ &
;
““& '
doctor
”” 
.
”” 
Email
”” 
=
”” 
existingDoctor
”” )
.
””) *
Email
””* /
;
””/ 0
doctor
‘‘ 
.
‘‘ 
IdentityUserId
‘‘ !
=
‘‘" #
existingDoctor
‘‘$ 2
.
‘‘2 3
IdentityUserId
‘‘3 A
;
‘‘A B
doctor
’’ 
.
’’ 
YearsOfExperience
’’ $
=
’’% &(
CalculateYearsOfExperience
’’' A
(
’’A B
dto
’’B E
.
’’E F
PracticeStartDate
’’F W
)
’’W X
;
’’X Y
doctor
÷÷ 
.
÷÷ 
CreatedDate
÷÷ 
=
÷÷  
existingDoctor
÷÷! /
.
÷÷/ 0
CreatedDate
÷÷0 ;
;
÷÷; <
var
ÿÿ 
updatedDoctor
ÿÿ 
=
ÿÿ 
await
ÿÿ  %

repository
ÿÿ& 0
.
ÿÿ0 1
UpdateAsync
ÿÿ1 <
(
ÿÿ< =
doctorId
ÿÿ= E
,
ÿÿE F
doctor
ÿÿG M
)
ÿÿM N
;
ÿÿN O
if
⁄⁄ 
(
⁄⁄ 
updatedDoctor
⁄⁄ 
is
⁄⁄  
null
⁄⁄! %
)
⁄⁄% &
{
€€ 
throw
‹‹ 
new
‹‹ %
EntityNotFoundException
‹‹ 1
(
‹‹1 2
$str
‹‹2 :
,
‹‹: ;
doctorId
‹‹< D
)
‹‹D E
;
‹‹E F
}
›› 
return
ﬂﬂ 
mapper
ﬂﬂ 
.
ﬂﬂ 
Map
ﬂﬂ 
<
ﬂﬂ 
	DoctorDto
ﬂﬂ '
>
ﬂﬂ' (
(
ﬂﬂ( )
updatedDoctor
ﬂﬂ) 6
)
ﬂﬂ6 7
;
ﬂﬂ7 8
}
‡‡ 	
public
‚‚ 
async
‚‚ 
Task
‚‚ 
<
‚‚ 
	DoctorDto
‚‚ #
>
‚‚# $
DeleteDoctorAsync
‚‚% 6
(
‚‚6 7
int
‚‚7 :
doctorId
‚‚; C
)
‚‚C D
{
„„ 	
ValidateDoctorId
‰‰ 
(
‰‰ 
doctorId
‰‰ %
)
‰‰% &
;
‰‰& '
var
ÊÊ 
deletedDoctor
ÊÊ 
=
ÊÊ 
await
ÊÊ  %

repository
ÊÊ& 0
.
ÊÊ0 1
DeleteAsync
ÊÊ1 <
(
ÊÊ< =
doctorId
ÊÊ= E
)
ÊÊE F
;
ÊÊF G
if
ËË 
(
ËË 
deletedDoctor
ËË 
is
ËË  
null
ËË! %
)
ËË% &
{
ÈÈ 
throw
ÍÍ 
new
ÍÍ %
EntityNotFoundException
ÍÍ 1
(
ÍÍ1 2
$str
ÍÍ2 :
,
ÍÍ: ;
doctorId
ÍÍ< D
)
ÍÍD E
;
ÍÍE F
}
ÎÎ 
return
ÌÌ 
mapper
ÌÌ 
.
ÌÌ 
Map
ÌÌ 
<
ÌÌ 
	DoctorDto
ÌÌ '
>
ÌÌ' (
(
ÌÌ( )
deletedDoctor
ÌÌ) 6
)
ÌÌ6 7
;
ÌÌ7 8
}
ÓÓ 	
public
 
async
 
Task
 
<
 
	DoctorDto
 #
>
# $
GetMyProfileAsync
% 6
(
6 7
string
7 =
identityUserId
> L
)
L M
{
ÒÒ 	
if
ÚÚ 
(
ÚÚ 
string
ÚÚ 
.
ÚÚ  
IsNullOrWhiteSpace
ÚÚ )
(
ÚÚ) *
identityUserId
ÚÚ* 8
)
ÚÚ8 9
)
ÚÚ9 :
{
ÛÛ 
throw
ÙÙ 
new
ÙÙ #
BusinessRuleException
ÙÙ /
(
ÙÙ/ 0
$str
ÙÙ0 I
)
ÙÙI J
;
ÙÙJ K
}
ıı 
var
˜˜ 
doctor
˜˜ 
=
˜˜ 
await
˜˜ 

repository
˜˜ )
.
˜˜) *&
GetByIdentityUserIdAsync
˜˜* B
(
˜˜B C
identityUserId
˜˜C Q
)
˜˜Q R
;
˜˜R S
if
˘˘ 
(
˘˘ 
doctor
˘˘ 
is
˘˘ 
null
˘˘ 
)
˘˘ 
{
˙˙ 
throw
˚˚ 
new
˚˚ %
EntityNotFoundException
˚˚ 1
(
˚˚1 2
$str
˚˚2 U
,
˚˚U V
$num
˚˚W X
)
˚˚X Y
;
˚˚Y Z
}
¸¸ 
return
˛˛ 
mapper
˛˛ 
.
˛˛ 
Map
˛˛ 
<
˛˛ 
	DoctorDto
˛˛ '
>
˛˛' (
(
˛˛( )
doctor
˛˛) /
)
˛˛/ 0
;
˛˛0 1
}
ˇˇ 	
public
ÅÅ 
async
ÅÅ 
Task
ÅÅ 
<
ÅÅ 
List
ÅÅ 
<
ÅÅ 
string
ÅÅ %
>
ÅÅ% &
>
ÅÅ& '(
GetDoctorAvailabilityAsync
ÅÅ( B
(
ÅÅB C
int
ÅÅC F
doctorId
ÅÅG O
)
ÅÅO P
{
ÇÇ 	
ValidateDoctorId
ÉÉ 
(
ÉÉ 
doctorId
ÉÉ %
)
ÉÉ% &
;
ÉÉ& '
var
ÖÖ 
doctor
ÖÖ 
=
ÖÖ 
await
ÖÖ 

repository
ÖÖ )
.
ÖÖ) *
GetByIdAsync
ÖÖ* 6
(
ÖÖ6 7
doctorId
ÖÖ7 ?
)
ÖÖ? @
;
ÖÖ@ A
if
áá 
(
áá 
doctor
áá 
is
áá 
null
áá 
)
áá 
{
àà 
throw
ââ 
new
ââ %
EntityNotFoundException
ââ 1
(
ââ1 2
$str
ââ2 :
,
ââ: ;
doctorId
ââ< D
)
ââD E
;
ââE F
}
ää 
if
åå 
(
åå 
!
åå 
doctor
åå 
.
åå 
IsActive
åå  
)
åå  !
{
çç 
throw
éé 
new
éé #
BusinessRuleException
éé /
(
éé/ 0
$str
éé0 h
)
ééh i
;
ééi j
}
èè 
return
ëë 
	TimeSlots
ëë 
.
ëë 
Slots
ëë "
.
ëë" #
ToList
ëë# )
(
ëë) *
)
ëë* +
;
ëë+ ,
}
íí 	
private
îî 
static
îî 
void
îî 
ValidateDoctorId
îî ,
(
îî, -
int
îî- 0
doctorId
îî1 9
)
îî9 :
{
ïï 	
if
ññ 
(
ññ 
doctorId
ññ 
<=
ññ 
$num
ññ 
)
ññ 
{
óó 
throw
òò 
new
òò #
BusinessRuleException
òò /
(
òò/ 0
$str
òò0 Z
)
òòZ [
;
òò[ \
}
ôô 
}
öö 	
private
úú 
void
úú %
ValidateCreateDoctorDto
úú ,
(
úú, -
CreateDoctorDto
úú- <
dto
úú= @
)
úú@ A
{
ùù 	
if
ûû 
(
ûû 
dto
ûû 
is
ûû 
null
ûû 
)
ûû 
{
üü 
throw
†† 
new
†† #
BusinessRuleException
†† /
(
††/ 0
$str
††0 N
)
††N O
;
††O P
}
°° (
ValidateDoctorCommonFields
££ &
(
££& '
dto
§§ 
.
§§ 
FullName
§§ 
,
§§ 
dto
•• 
.
•• 
Email
•• 
,
•• 
dto
¶¶ 
.
¶¶ 
PracticeStartDate
¶¶ %
,
¶¶% &
dto
ßß 
.
ßß 
ConsultationFee
ßß #
)
ßß# $
;
ßß$ %
}
®® 	
private
™™ 
void
™™ %
ValidateUpdateDoctorDto
™™ ,
(
™™, -
UpdateDoctorDto
™™- <
dto
™™= @
)
™™@ A
{
´´ 	
if
¨¨ 
(
¨¨ 
dto
¨¨ 
is
¨¨ 
null
¨¨ 
)
¨¨ 
{
≠≠ 
throw
ÆÆ 
new
ÆÆ #
BusinessRuleException
ÆÆ /
(
ÆÆ/ 0
$str
ÆÆ0 N
)
ÆÆN O
;
ÆÆO P
}
ØØ (
ValidateDoctorCommonFields
±± &
(
±±& '
dto
≤≤ 
.
≤≤ 
FullName
≤≤ 
,
≤≤ 
null
≥≥ 
,
≥≥ 
dto
¥¥ 
.
¥¥ 
PracticeStartDate
¥¥ %
,
¥¥% &
dto
µµ 
.
µµ 
ConsultationFee
µµ #
)
µµ# $
;
µµ$ %
}
∂∂ 	
private
∏∏ 
static
∏∏ 
void
∏∏ (
ValidateDoctorCommonFields
∏∏ 6
(
∏∏6 7
string
ππ 
fullName
ππ 
,
ππ 
string
∫∫ 
?
∫∫ 
email
∫∫ 
,
∫∫ 
DateTime
ªª 
practiceStartDate
ªª &
,
ªª& '
decimal
ºº 
consultationFee
ºº #
)
ºº# $
{
ΩΩ 	
if
ææ 
(
ææ 
string
ææ 
.
ææ  
IsNullOrWhiteSpace
ææ )
(
ææ) *
fullName
ææ* 2
)
ææ2 3
)
ææ3 4
{
øø 
throw
¿¿ 
new
¿¿ #
BusinessRuleException
¿¿ /
(
¿¿/ 0
$str
¿¿0 O
)
¿¿O P
;
¿¿P Q
}
¡¡ 
if
√√ 
(
√√ 
email
√√ 
is
√√ 
not
√√ 
null
√√ !
&&
√√" $
string
√√% +
.
√√+ , 
IsNullOrWhiteSpace
√√, >
(
√√> ?
email
√√? D
)
√√D E
)
√√E F
{
ƒƒ 
throw
≈≈ 
new
≈≈ #
BusinessRuleException
≈≈ /
(
≈≈/ 0
$str
≈≈0 K
)
≈≈K L
;
≈≈L M
}
∆∆ 
if
»» 
(
»» 
practiceStartDate
»» !
.
»»! "
Date
»»" &
>
»»' (
DateTime
»») 1
.
»»1 2
Today
»»2 7
)
»»7 8
{
…… 
throw
   
new
   #
BusinessRuleException
   /
(
  / 0
$str
  0 ^
)
  ^ _
;
  _ `
}
ÀÀ 
if
ÕÕ 
(
ÕÕ 
consultationFee
ÕÕ 
<
ÕÕ  !
$num
ÕÕ" #
)
ÕÕ# $
{
ŒŒ 
throw
œœ 
new
œœ #
BusinessRuleException
œœ /
(
œœ/ 0
$str
œœ0 V
)
œœV W
;
œœW X
}
–– 
}
—— 	
private
”” 
static
”” 
int
”” (
CalculateYearsOfExperience
”” 5
(
””5 6
DateTime
””6 >
practiceStartDate
””? P
)
””P Q
{
‘‘ 	
int
’’ 
years
’’ 
=
’’ 
DateTime
’’  
.
’’  !
Today
’’! &
.
’’& '
Year
’’' +
-
’’, -
practiceStartDate
’’. ?
.
’’? @
Year
’’@ D
;
’’D E
if
◊◊ 
(
◊◊ 
practiceStartDate
◊◊ !
.
◊◊! "
Date
◊◊" &
>
◊◊' (
DateTime
◊◊) 1
.
◊◊1 2
Today
◊◊2 7
.
◊◊7 8
AddYears
◊◊8 @
(
◊◊@ A
-
◊◊A B
years
◊◊B G
)
◊◊G H
)
◊◊H I
{
ÿÿ 
years
ŸŸ 
--
ŸŸ 
;
ŸŸ 
}
⁄⁄ 
return
‹‹ 
years
‹‹ 
;
‹‹ 
}
›› 	
private
ﬂﬂ 
static
ﬂﬂ 
string
ﬂﬂ '
GenerateTemporaryPassword
ﬂﬂ 7
(
ﬂﬂ7 8
string
ﬂﬂ8 >

doctorName
ﬂﬂ? I
)
ﬂﬂI J
{
‡‡ 	
string
·· 
cleanedName
·· 
=
··  
new
··! $
string
··% +
(
··+ ,

doctorName
‚‚ 
.
„„ 
Where
„„ 
(
„„ 
char
„„ 
.
„„  
IsLetter
„„  (
)
„„( )
.
‰‰ 
Take
‰‰ 
(
‰‰ 
$num
‰‰ 
)
‰‰ 
.
ÂÂ 
ToArray
ÂÂ 
(
ÂÂ 
)
ÂÂ 
)
ÂÂ 
;
ÂÂ  
if
ÁÁ 
(
ÁÁ 
string
ÁÁ 
.
ÁÁ  
IsNullOrWhiteSpace
ÁÁ )
(
ÁÁ) *
cleanedName
ÁÁ* 5
)
ÁÁ5 6
)
ÁÁ6 7
{
ËË 
cleanedName
ÈÈ 
=
ÈÈ 
$str
ÈÈ &
;
ÈÈ& '
}
ÍÍ 
string
ÏÏ 
formattedName
ÏÏ  
=
ÏÏ! "
char
ÌÌ 
.
ÌÌ 
ToUpper
ÌÌ 
(
ÌÌ 
cleanedName
ÌÌ (
[
ÌÌ( )
$num
ÌÌ) *
]
ÌÌ* +
)
ÌÌ+ ,
+
ÌÌ- .
cleanedName
ÌÌ/ :
.
ÌÌ: ;
	Substring
ÌÌ; D
(
ÌÌD E
$num
ÌÌE F
)
ÌÌF G
.
ÌÌG H
ToLower
ÌÌH O
(
ÌÌO P
)
ÌÌP Q
;
ÌÌQ R
return
ÔÔ 
$"
ÔÔ 
{
ÔÔ 
formattedName
ÔÔ #
}
ÔÔ# $
$str
ÔÔ$ %
{
ÔÔ% &
DateTime
ÔÔ& .
.
ÔÔ. /
Today
ÔÔ/ 4
.
ÔÔ4 5
Year
ÔÔ5 9
}
ÔÔ9 :
"
ÔÔ: ;
;
ÔÔ; <
}
 	
}
ÒÒ 
}ÚÚ ≥}
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
}≤≤ ¯º
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Services\Impl\AppointmentService.cs
	namespace		 	
HealthCareApp		
 
.		 
Services		  
.		  !
Impl		! %
{

 
public 

class 
AppointmentService #
(# $"
IAppointmentRepository !
appointmentRepository 4
,4 5
IPatientRepository 
patientRepository ,
,, -
IDoctorRepository 
doctorRepository *
,* +#
IHealthRecordRepository "
healthRecordRepository  6
,6 7
IMapper 
mapper 
) 
: 
IAppointmentService -
{ 
private 
const 
string !
AppointmentEntityName 2
=3 4
$str5 B
;B C
public 
async 
Task 
< 
List 
< 
AppointmentDto -
>- .
>. /#
GetAllAppointmentsAsync0 G
(G H
)H I
{ 	
var 
appointments 
= 
await $!
appointmentRepository% :
.: ;
GetAllAsync; F
(F G
)G H
;H I
return 
mapper 
. 
Map 
< 
List "
<" #
AppointmentDto# 1
>1 2
>2 3
(3 4
appointments4 @
)@ A
;A B
} 	
public 
async 
Task 
< 
PagedResponse '
<' (
AppointmentDto( 6
>6 7
>7 8(
GetAllAppointmentsPagedAsync9 U
(U V)
AppointmentPaginationQueryDtoV s
queryt y
)y z
{ 
query 
??= 
new )
AppointmentPaginationQueryDto 7
(7 8
)8 9
;9 :
int 

pageNumber 
= 
query "
." #

PageNumber# -
<=. 0
$num1 2
?3 4
$num5 6
:7 8
query9 >
.> ?

PageNumber? I
;I J
int   
pageSize   
=   
query   
.   
PageSize   !
<=  " $
$num  % &
?  ' (
$num  ) +
:  , -
query  . 3
.  3 4
PageSize  4 <
;  < =
pageSize"" 
="" 
pageSize"" 
>"" 
$num"" 
?"" 
$num""  #
:""$ %
pageSize""& .
;"". /
var$$ 
appointments$$ 
=$$ 
await$$ !
appointmentRepository$$ 2
.$$2 3
GetAllAsync$$3 >
($$> ?
)$$? @
;$$@ A
var&&  
filteredAppointments&& 
=&& 
appointments&& +
.&&+ ,
AsEnumerable&&, 8
(&&8 9
)&&9 :
;&&: ;
if(( 
((( 
!(( 	
string((	 
.(( 
IsNullOrWhiteSpace(( "
(((" #
query((# (
.((( )

SearchTerm(() 3
)((3 4
)((4 5
{)) 
string** 

searchTerm** 
=** 
query** !
.**! "

SearchTerm**" ,
.**, -
Trim**- 1
(**1 2
)**2 3
;**3 4 
filteredAppointments,, 
=,,  
filteredAppointments,, 3
.,,3 4
Where,,4 9
(,,9 :
a,,: ;
=>,,< >
(-- 
a-- 
.-- 
Patient-- 
!=-- 
null-- 
&&-- !
a.. 
... 
Patient.. 
... 
PatientName.. "
..." #
Contains..# +
(..+ ,

searchTerm.., 6
,..6 7
StringComparison..8 H
...H I
OrdinalIgnoreCase..I Z
)..Z [
)..[ \
||..] _
(// 
a// 
.// 
Doctor// 
!=// 
null// 
&&//  
a00 
.00 
Doctor00 
.00 

DoctorName00  
.00  !
Contains00! )
(00) *

searchTerm00* 4
,004 5
StringComparison006 F
.00F G
OrdinalIgnoreCase00G X
)00X Y
)00Y Z
||00[ ]
a11 
.11 
TimeSlot11 
.11 
Contains11 
(11  

searchTerm11  *
,11* +
StringComparison11, <
.11< =
OrdinalIgnoreCase11= N
)11N O
||11P R
(22 
!22 
string22 
.22 
IsNullOrWhiteSpace22 '
(22' (
a22( )
.22) *
CancellationReason22* <
)22< =
&&22> @
a33 
.33 
CancellationReason33 !
.33! "
Contains33" *
(33* +

searchTerm33+ 5
,335 6
StringComparison337 G
.33G H
OrdinalIgnoreCase33H Y
)33Y Z
)33Z [
)33[ \
;33\ ]
}44 
if66 
(66 
query66 
.66 
	PatientId66 
is66 
not66 
null66 #
)66# $
{77  
filteredAppointments88 
=88  
filteredAppointments88 3
.883 4
Where884 9
(889 :
a88: ;
=>88< >
a99 
.99 
	PatientId99 
==99 
query99  
.99  !
	PatientId99! *
.99* +
Value99+ 0
)990 1
;991 2
}:: 
if<< 
(<< 
query<< 
.<< 
DoctorId<< 
is<< 
not<< 
null<< "
)<<" #
{==  
filteredAppointments>> 
=>>  
filteredAppointments>> 3
.>>3 4
Where>>4 9
(>>9 :
a>>: ;
=>>>< >
a?? 
.?? 
DoctorId?? 
==?? 
query?? 
.??  
DoctorId??  (
.??( )
Value??) .
)??. /
;??/ 0
}@@ 
ifBB 
(BB 
queryBB 
.BB 
StatusBB 
isBB 
notBB 
nullBB  
)BB  !
{CC  
filteredAppointmentsDD 
=DD  
filteredAppointmentsDD 3
.DD3 4
WhereDD4 9
(DD9 :
aDD: ;
=>DD< >
aEE 
.EE 
StatusEE 
==EE 
queryEE 
.EE 
StatusEE $
.EE$ %
ValueEE% *
)EE* +
;EE+ ,
}FF 
ifHH 
(HH 
queryHH 
.HH 
ScheduledDateHH 
isHH 
notHH "
nullHH# '
)HH' (
{II  
filteredAppointmentsJJ 
=JJ  
filteredAppointmentsJJ 3
.JJ3 4
WhereJJ4 9
(JJ9 :
aJJ: ;
=>JJ< >
aKK 
.KK 
ScheduledDateKK 
.KK 
DateKK  
==KK! #
queryKK$ )
.KK) *
ScheduledDateKK* 7
.KK7 8
ValueKK8 =
.KK= >
DateKK> B
)KKB C
;KKC D
}LL 
ifNN 
(NN 
queryNN 
.NN 
UpcomingOnlyNN 
isNN 
notNN !
nullNN" &
&&NN' )
queryNN* /
.NN/ 0
UpcomingOnlyNN0 <
.NN< =
ValueNN= B
)NNB C
{OO  
filteredAppointmentsPP 
=PP  
filteredAppointmentsPP 3
.PP3 4
WherePP4 9
(PP9 :
aPP: ;
=>PP< >
aQQ 
.QQ 
ScheduledDateQQ 
.QQ 
DateQQ  
>=QQ! #
DateTimeQQ$ ,
.QQ, -
TodayQQ- 2
&&QQ3 5
aRR 
.RR 
StatusRR 
!=RR 
AppointmentStatusRR )
.RR) *
	CancelledRR* 3
&&RR4 6
aSS 
.SS 
StatusSS 
!=SS 
AppointmentStatusSS )
.SS) *
	CompletedSS* 3
)SS3 4
;SS4 5
}TT 
intVV 
totalRecordsVV 
=VV  
filteredAppointmentsVV +
.VV+ ,
CountVV, 1
(VV1 2
)VV2 3
;VV3 4
varXX 
pagedAppointmentsXX 
=XX  
filteredAppointmentsXX 0
.YY 	
OrderByDescendingYY	 
(YY 
aYY 
=>YY 
aYY  !
.YY! "
ScheduledDateYY" /
)YY/ 0
.ZZ 	
ThenByZZ	 
(ZZ 
aZZ 
=>ZZ 
aZZ 
.ZZ 
TimeSlotZZ 
)ZZ  
.[[ 	
Skip[[	 
([[ 
([[ 

pageNumber[[ 
-[[ 
$num[[ 
)[[ 
*[[  
pageSize[[! )
)[[) *
.\\ 	
Take\\	 
(\\ 
pageSize\\ 
)\\ 
.]] 	
ToList]]	 
(]] 
)]] 
;]] 
var__ 
mappedAppointments__ 
=__ 
mapper__ #
.__# $
Map__$ '
<__' (
List__( ,
<__, -
AppointmentDto__- ;
>__; <
>__< =
(__= >
pagedAppointments__> O
)__O P
;__P Q
returnaa 

newaa 
PagedResponseaa 
<aa 
AppointmentDtoaa +
>aa+ ,
{bb 
Itemscc 
=cc 
mappedAppointmentscc "
,cc" #

PageNumberdd 
=dd 

pageNumberdd 
,dd  
PageSizeee 
=ee 
pageSizeee 
,ee 
TotalRecordsff 
=ff 
totalRecordsff #
,ff# $

TotalPagesgg 
=gg 
(gg 
intgg 
)gg 
Mathgg 
.gg 
Ceilinggg &
(gg& '
totalRecordsgg' 3
/gg4 5
(gg6 7
doublegg7 =
)gg= >
pageSizegg> F
)ggF G
}hh 
;hh 
}ii 
publickk 
asynckk 
Taskkk 
<kk 
AppointmentDtokk (
>kk( )#
GetAppointmentByIdAsynckk* A
(kkA B
intkkB E
appointmentIdkkF S
)kkS T
{ll 	!
ValidateAppointmentIdmm !
(mm! "
appointmentIdmm" /
)mm/ 0
;mm0 1
varoo 
appointmentoo 
=oo 
awaitoo #!
appointmentRepositoryoo$ 9
.oo9 :
GetByIdAsyncoo: F
(ooF G
appointmentIdooG T
)ooT U
;ooU V
ifqq 
(qq 
appointmentqq 
isqq 
nullqq #
)qq# $
{rr 
throwss 
newss #
EntityNotFoundExceptionss 1
(ss1 2!
AppointmentEntityNamess2 G
,ssG H
appointmentIdssI V
)ssV W
;ssW X
}tt 
returnvv 
mappervv 
.vv 
Mapvv 
<vv 
AppointmentDtovv ,
>vv, -
(vv- .
appointmentvv. 9
)vv9 :
;vv: ;
}ww 	
publicyy 
asyncyy 
Taskyy 
<yy 
Listyy 
<yy 
AppointmentDtoyy -
>yy- .
>yy. /+
GetAppointmentsByPatientIdAsyncyy0 O
(yyO P
intyyP S
	patientIdyyT ]
)yy] ^
{zz 	
await{{ &
ValidatePatientExistsAsync{{ ,
({{, -
	patientId{{- 6
){{6 7
;{{7 8
var}} 
appointments}} 
=}} 
await}} $!
appointmentRepository}}% :
.}}: ;
GetByPatientIdAsync}}; N
(}}N O
	patientId}}O X
)}}X Y
;}}Y Z
return 
mapper 
. 
Map 
< 
List "
<" #
AppointmentDto# 1
>1 2
>2 3
(3 4
appointments4 @
)@ A
;A B
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
ÇÇ. /,
GetAppointmentsByDoctorIdAsync
ÇÇ0 N
(
ÇÇN O
int
ÇÇO R
doctorId
ÇÇS [
)
ÇÇ[ \
{
ÉÉ 	
await
ÑÑ '
ValidateDoctorExistsAsync
ÑÑ +
(
ÑÑ+ ,
doctorId
ÑÑ, 4
)
ÑÑ4 5
;
ÑÑ5 6
var
ÜÜ 
appointments
ÜÜ 
=
ÜÜ 
await
ÜÜ $#
appointmentRepository
ÜÜ% :
.
ÜÜ: ; 
GetByDoctorIdAsync
ÜÜ; M
(
ÜÜM N
doctorId
ÜÜN V
)
ÜÜV W
;
ÜÜW X
return
àà 
mapper
àà 
.
àà 
Map
àà 
<
àà 
List
àà "
<
àà" #
AppointmentDto
àà# 1
>
àà1 2
>
àà2 3
(
àà3 4
appointments
àà4 @
)
àà@ A
;
ààA B
}
ââ 	
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
ãã 
AppointmentDto
ãã -
>
ãã- .
>
ãã. /*
GetAppointmentsByStatusAsync
ãã0 L
(
ããL M
AppointmentStatus
ããM ^
status
ãã_ e
)
ããe f
{
åå 	
var
çç 
appointments
çç 
=
çç 
await
çç $#
appointmentRepository
çç% :
.
çç: ;
GetByStatusAsync
çç; K
(
ççK L
status
ççL R
)
ççR S
;
ççS T
return
èè 
mapper
èè 
.
èè 
Map
èè 
<
èè 
List
èè "
<
èè" #
AppointmentDto
èè# 1
>
èè1 2
>
èè2 3
(
èè3 4
appointments
èè4 @
)
èè@ A
;
èèA B
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
List
íí 
<
íí 
AppointmentDto
íí -
>
íí- .
>
íí. /*
GetUpcomingAppointmentsAsync
íí0 L
(
ííL M
)
ííM N
{
ìì 	
var
îî 
appointments
îî 
=
îî 
await
îî $#
appointmentRepository
îî% :
.
îî: ;*
GetUpcomingAppointmentsAsync
îî; W
(
îîW X
)
îîX Y
;
îîY Z
return
ññ 
mapper
ññ 
.
ññ 
Map
ññ 
<
ññ 
List
ññ "
<
ññ" #
AppointmentDto
ññ# 1
>
ññ1 2
>
ññ2 3
(
ññ3 4
appointments
ññ4 @
)
ññ@ A
;
ññA B
}
óó 	
public
ôô 
async
ôô 
Task
ôô 
<
ôô 
List
ôô 
<
ôô 
AppointmentDto
ôô -
>
ôô- .
>
ôô. /5
'GetUpcomingAppointmentsByPatientIdAsync
ôô0 W
(
ôôW X
int
ôôX [
	patientId
ôô\ e
)
ôôe f
{
öö 	
await
õõ (
ValidatePatientExistsAsync
õõ ,
(
õõ, -
	patientId
õõ- 6
)
õõ6 7
;
õõ7 8
var
ùù 
appointments
ùù 
=
ùù 
await
ùù $#
appointmentRepository
ùù% :
.
ùù: ;5
'GetUpcomingAppointmentsByPatientIdAsync
ùù; b
(
ùùb c
	patientId
ùùc l
)
ùùl m
;
ùùm n
return
üü 
mapper
üü 
.
üü 
Map
üü 
<
üü 
List
üü "
<
üü" #
AppointmentDto
üü# 1
>
üü1 2
>
üü2 3
(
üü3 4
appointments
üü4 @
)
üü@ A
;
üüA B
}
†† 	
public
¢¢ 
async
¢¢ 
Task
¢¢ 
<
¢¢ 
List
¢¢ 
<
¢¢ 
AppointmentDto
¢¢ -
>
¢¢- .
>
¢¢. /4
&GetUpcomingAppointmentsByDoctorIdAsync
¢¢0 V
(
¢¢V W
int
¢¢W Z
doctorId
¢¢[ c
)
¢¢c d
{
££ 	
await
§§ '
ValidateDoctorExistsAsync
§§ +
(
§§+ ,
doctorId
§§, 4
)
§§4 5
;
§§5 6
var
¶¶ 
appointments
¶¶ 
=
¶¶ 
await
¶¶ $#
appointmentRepository
¶¶% :
.
¶¶: ;4
&GetUpcomingAppointmentsByDoctorIdAsync
¶¶; a
(
¶¶a b
doctorId
¶¶b j
)
¶¶j k
;
¶¶k l
return
®® 
mapper
®® 
.
®® 
Map
®® 
<
®® 
List
®® "
<
®®" #
AppointmentDto
®®# 1
>
®®1 2
>
®®2 3
(
®®3 4
appointments
®®4 @
)
®®@ A
;
®®A B
}
©© 	
public
´´ 
async
´´ 
Task
´´ 
<
´´ 
List
´´ 
<
´´ 
AppointmentDto
´´ -
>
´´- .
>
´´. /4
&GetPendingAppointmentsByPatientIdAsync
´´0 V
(
´´V W
int
´´W Z
	patientId
´´[ d
)
´´d e
{
¨¨ 	
await
≠≠ (
ValidatePatientExistsAsync
≠≠ ,
(
≠≠, -
	patientId
≠≠- 6
)
≠≠6 7
;
≠≠7 8
var
ØØ 
appointments
ØØ 
=
ØØ 
await
ØØ $#
appointmentRepository
ØØ% :
.
ØØ: ;4
&GetPendingAppointmentsByPatientIdAsync
ØØ; a
(
ØØa b
	patientId
ØØb k
)
ØØk l
;
ØØl m
return
±± 
mapper
±± 
.
±± 
Map
±± 
<
±± 
List
±± "
<
±±" #
AppointmentDto
±±# 1
>
±±1 2
>
±±2 3
(
±±3 4
appointments
±±4 @
)
±±@ A
;
±±A B
}
≤≤ 	
public
¥¥ 
async
¥¥ 
Task
¥¥ 
<
¥¥ 
List
¥¥ 
<
¥¥ 
AppointmentDto
¥¥ -
>
¥¥- .
>
¥¥. /3
%GetPendingAppointmentsByDoctorIdAsync
¥¥0 U
(
¥¥U V
int
¥¥V Y
doctorId
¥¥Z b
)
¥¥b c
{
µµ 	
await
∂∂ '
ValidateDoctorExistsAsync
∂∂ +
(
∂∂+ ,
doctorId
∂∂, 4
)
∂∂4 5
;
∂∂5 6
var
∏∏ 
appointments
∏∏ 
=
∏∏ 
await
∏∏ $#
appointmentRepository
∏∏% :
.
∏∏: ;3
%GetPendingAppointmentsByDoctorIdAsync
∏∏; `
(
∏∏` a
doctorId
∏∏a i
)
∏∏i j
;
∏∏j k
return
∫∫ 
mapper
∫∫ 
.
∫∫ 
Map
∫∫ 
<
∫∫ 
List
∫∫ "
<
∫∫" #
AppointmentDto
∫∫# 1
>
∫∫1 2
>
∫∫2 3
(
∫∫3 4
appointments
∫∫4 @
)
∫∫@ A
;
∫∫A B
}
ªª 	
public
ΩΩ 
async
ΩΩ 
Task
ΩΩ 
<
ΩΩ 
List
ΩΩ 
<
ΩΩ 
AppointmentDto
ΩΩ -
>
ΩΩ- .
>
ΩΩ. /:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
ΩΩ0 \
(
ΩΩ\ ]
int
ΩΩ] `
doctorId
ΩΩa i
)
ΩΩi j
{
ææ 	
await
øø '
ValidateDoctorExistsAsync
øø +
(
øø+ ,
doctorId
øø, 4
)
øø4 5
;
øø5 6
var
¡¡ 
appointments
¡¡ 
=
¡¡ 
await
¡¡ $#
appointmentRepository
¡¡% :
.
¡¡: ;:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
¡¡; g
(
¡¡g h
doctorId
¡¡h p
)
¡¡p q
;
¡¡q r
return
√√ 
mapper
√√ 
.
√√ 
Map
√√ 
<
√√ 
List
√√ "
<
√√" #
AppointmentDto
√√# 1
>
√√1 2
>
√√2 3
(
√√3 4
appointments
√√4 @
)
√√@ A
;
√√A B
}
ƒƒ 	
public
∆∆ 
async
∆∆ 
Task
∆∆ 
<
∆∆ 
AppointmentDto
∆∆ (
>
∆∆( )"
BookAppointmentAsync
∆∆* >
(
∆∆> ? 
BookAppointmentDto
∆∆? Q
dto
∆∆R U
)
∆∆U V
{
«« 	
if
»» 
(
»» 
dto
»» 
is
»» 
null
»» 
)
»» 
{
…… 
throw
   
new
   &
AppointmentRuleException
   2
(
  2 3
$str
  3 V
)
  V W
;
  W X
}
ÀÀ 
await
ÕÕ (
ValidatePatientExistsAsync
ÕÕ ,
(
ÕÕ, -
dto
ÕÕ- 0
.
ÕÕ0 1
	PatientId
ÕÕ1 :
)
ÕÕ: ;
;
ÕÕ; <
var
œœ 
doctor
œœ 
=
œœ 
await
œœ '
ValidateDoctorExistsAsync
œœ 8
(
œœ8 9
dto
œœ9 <
.
œœ< =
DoctorId
œœ= E
)
œœE F
;
œœF G(
ValidateDoctorAvailability
—— &
(
——& '
doctor
——' -
)
——- .
;
——. /%
ValidateAppointmentDate
”” #
(
””# $
dto
””$ '
.
””' (
ScheduledDate
””( 5
)
””5 6
;
””6 7
ValidateTimeSlot
’’ 
(
’’ 
dto
’’  
.
’’  !
TimeSlot
’’! )
)
’’) *
;
’’* +
var
◊◊ 
isSlotBooked
◊◊ 
=
◊◊ 
await
◊◊ $#
appointmentRepository
◊◊% :
.
◊◊: ;
IsSlotBookedAsync
◊◊; L
(
◊◊L M
dto
ÿÿ 
.
ÿÿ 
DoctorId
ÿÿ 
,
ÿÿ 
dto
ŸŸ 
.
ŸŸ 
ScheduledDate
ŸŸ !
.
ŸŸ! "
Date
ŸŸ" &
,
ŸŸ& '
dto
⁄⁄ 
.
⁄⁄ 
TimeSlot
⁄⁄ 
)
⁄⁄ 
;
⁄⁄ 
if
‹‹ 
(
‹‹ 
isSlotBooked
‹‹ 
)
‹‹ 
{
›› 
throw
ﬁﬁ 
new
ﬁﬁ 
ConflictException
ﬁﬁ +
(
ﬁﬁ+ ,
$str
ﬁﬁ, g
)
ﬁﬁg h
;
ﬁﬁh i
}
ﬂﬂ 
var
··  
patientHasSameSlot
·· "
=
··# $
await
··% *#
appointmentRepository
··+ @
.
··@ A;
-PatientHasActiveAppointmentOnDateAndSlotAsync
··A n
(
··n o
dto
‚‚ 
.
‚‚ 
	PatientId
‚‚ 
,
‚‚ 
dto
„„ 
.
„„ 
ScheduledDate
„„ !
.
„„! "
Date
„„" &
,
„„& '
dto
‰‰ 
.
‰‰ 
TimeSlot
‰‰ 
)
‰‰ 
;
‰‰ 
if
ÊÊ 
(
ÊÊ  
patientHasSameSlot
ÊÊ "
)
ÊÊ" #
{
ÁÁ 
throw
ËË 
new
ËË 
ConflictException
ËË +
(
ËË+ ,
$str
ËË, j
)
ËËj k
;
ËËk l
}
ÈÈ 
var
ÎÎ -
patientHasAppointmentWithDoctor
ÎÎ /
=
ÎÎ0 1
await
ÎÎ2 7#
appointmentRepository
ÎÎ8 M
.
ÎÎM N>
0PatientHasActiveAppointmentWithDoctorOnDateAsync
ÎÎN ~
(
ÎÎ~ 
dto
ÏÏ 
.
ÏÏ 
	PatientId
ÏÏ 
,
ÏÏ 
dto
ÌÌ 
.
ÌÌ 
DoctorId
ÌÌ 
,
ÌÌ 
dto
ÓÓ 
.
ÓÓ 
ScheduledDate
ÓÓ !
.
ÓÓ! "
Date
ÓÓ" &
)
ÓÓ& '
;
ÓÓ' (
if
 
(
 -
patientHasAppointmentWithDoctor
 /
)
/ 0
{
ÒÒ 
throw
ÚÚ 
new
ÚÚ 
ConflictException
ÚÚ +
(
ÚÚ+ ,
$str
ÚÚ, ~
)
ÚÚ~ 
;ÚÚ Ä
}
ÛÛ 
var
ıı 
appointment
ıı 
=
ıı 
mapper
ıı $
.
ıı$ %
Map
ıı% (
<
ıı( )
Appointment
ıı) 4
>
ıı4 5
(
ıı5 6
dto
ıı6 9
)
ıı9 :
;
ıı: ;
appointment
˜˜ 
.
˜˜ 
ScheduledDate
˜˜ %
=
˜˜& '
dto
˜˜( +
.
˜˜+ ,
ScheduledDate
˜˜, 9
.
˜˜9 :
Date
˜˜: >
;
˜˜> ?
appointment
¯¯ 
.
¯¯ 
Status
¯¯ 
=
¯¯  
AppointmentStatus
¯¯! 2
.
¯¯2 3
Pending
¯¯3 :
;
¯¯: ;
appointment
˘˘ 
.
˘˘  
CancellationReason
˘˘ *
=
˘˘+ ,
null
˘˘- 1
;
˘˘1 2
appointment
˙˙ 
.
˙˙ 
CreatedDate
˙˙ #
=
˙˙$ %
DateTime
˙˙& .
.
˙˙. /
Now
˙˙/ 2
;
˙˙2 3
var
¸¸ 
savedAppointment
¸¸  
=
¸¸! "
await
¸¸# (#
appointmentRepository
¸¸) >
.
¸¸> ?
CreateAsync
¸¸? J
(
¸¸J K
appointment
¸¸K V
)
¸¸V W
;
¸¸W X
return
˛˛ 
mapper
˛˛ 
.
˛˛ 
Map
˛˛ 
<
˛˛ 
AppointmentDto
˛˛ ,
>
˛˛, -
(
˛˛- .
savedAppointment
˛˛. >
)
˛˛> ?
;
˛˛? @
}
ˇˇ 	
public
ÅÅ 
async
ÅÅ 
Task
ÅÅ 
<
ÅÅ 
AppointmentDto
ÅÅ (
>
ÅÅ( )$
UpdateAppointmentAsync
ÅÅ* @
(
ÅÅ@ A
int
ÅÅA D
appointmentId
ÅÅE R
,
ÅÅR S"
UpdateAppointmentDto
ÅÅT h
dto
ÅÅi l
)
ÅÅl m
{
ÇÇ 	#
ValidateAppointmentId
ÉÉ !
(
ÉÉ! "
appointmentId
ÉÉ" /
)
ÉÉ/ 0
;
ÉÉ0 1
if
ÖÖ 
(
ÖÖ 
dto
ÖÖ 
is
ÖÖ 
null
ÖÖ 
)
ÖÖ 
{
ÜÜ 
throw
áá 
new
áá &
AppointmentRuleException
áá 2
(
áá2 3
$str
áá3 V
)
ááV W
;
ááW X
}
àà 
var
ää !
existingAppointment
ää #
=
ää$ %
await
ää& +#
appointmentRepository
ää, A
.
ääA B
GetByIdAsync
ääB N
(
ääN O
appointmentId
ääO \
)
ää\ ]
;
ää] ^
if
åå 
(
åå !
existingAppointment
åå #
is
åå$ &
null
åå' +
)
åå+ ,
{
çç 
throw
éé 
new
éé %
EntityNotFoundException
éé 1
(
éé1 2
$str
éé2 ?
,
éé? @
appointmentId
ééA N
)
ééN O
;
ééO P
}
èè 
await
ëë (
ValidatePatientExistsAsync
ëë ,
(
ëë, -
dto
ëë- 0
.
ëë0 1
	PatientId
ëë1 :
)
ëë: ;
;
ëë; <
var
ìì 
doctor
ìì 
=
ìì 
await
ìì '
ValidateDoctorExistsAsync
ìì 8
(
ìì8 9
dto
ìì9 <
.
ìì< =
DoctorId
ìì= E
)
ììE F
;
ììF G(
ValidateDoctorAvailability
ïï &
(
ïï& '
doctor
ïï' -
)
ïï- .
;
ïï. /%
ValidateAppointmentDate
óó #
(
óó# $
dto
óó$ '
.
óó' (
ScheduledDate
óó( 5
)
óó5 6
;
óó6 7
ValidateTimeSlot
ôô 
(
ôô 
dto
ôô  
.
ôô  !
TimeSlot
ôô! )
)
ôô) *
;
ôô* +
bool
õõ 
	slotTaken
õõ 
=
õõ 
await
õõ "#
appointmentRepository
õõ# 8
.
õõ8 9
IsSlotBookedAsync
õõ9 J
(
õõJ K
dto
úú 
.
úú 
DoctorId
úú 
,
úú 
dto
ùù 
.
ùù 
ScheduledDate
ùù !
.
ùù! "
Date
ùù" &
,
ùù& '
dto
ûû 
.
ûû 
TimeSlot
ûû 
)
ûû 
;
ûû 
bool
†† 
sameExistingSlot
†† !
=
††" #!
existingAppointment
°° #
.
°°# $
DoctorId
°°$ ,
==
°°- /
dto
°°0 3
.
°°3 4
DoctorId
°°4 <
&&
°°= ?!
existingAppointment
¢¢ #
.
¢¢# $
ScheduledDate
¢¢$ 1
.
¢¢1 2
Date
¢¢2 6
==
¢¢7 9
dto
¢¢: =
.
¢¢= >
ScheduledDate
¢¢> K
.
¢¢K L
Date
¢¢L P
&&
¢¢Q S!
existingAppointment
££ #
.
££# $
TimeSlot
££$ ,
==
££- /
dto
££0 3
.
££3 4
TimeSlot
££4 <
;
££< =
if
•• 
(
•• 
	slotTaken
•• 
&&
•• 
!
•• 
sameExistingSlot
•• .
)
••. /
{
¶¶ 
throw
ßß 
new
ßß 
ConflictException
ßß +
(
ßß+ ,
$str
ßß, g
)
ßßg h
;
ßßh i
}
®® 
mapper
™™ 
.
™™ 
Map
™™ 
(
™™ 
dto
™™ 
,
™™ !
existingAppointment
™™ /
)
™™/ 0
;
™™0 1!
existingAppointment
¨¨ 
.
¨¨  
AppointmentId
¨¨  -
=
¨¨. /
appointmentId
¨¨0 =
;
¨¨= >!
existingAppointment
≠≠ 
.
≠≠  
ScheduledDate
≠≠  -
=
≠≠. /
dto
≠≠0 3
.
≠≠3 4
ScheduledDate
≠≠4 A
.
≠≠A B
Date
≠≠B F
;
≠≠F G
var
ØØ  
updatedAppointment
ØØ "
=
ØØ# $
await
ØØ% *#
appointmentRepository
ØØ+ @
.
ØØ@ A
UpdateAsync
ØØA L
(
ØØL M
appointmentId
∞∞ 
,
∞∞ !
existingAppointment
±± #
)
±±# $
;
±±$ %
if
≥≥ 
(
≥≥  
updatedAppointment
≥≥ "
is
≥≥# %
null
≥≥& *
)
≥≥* +
{
¥¥ 
throw
µµ 
new
µµ %
EntityNotFoundException
µµ 1
(
µµ1 2
$str
µµ2 ?
,
µµ? @
appointmentId
µµA N
)
µµN O
;
µµO P
}
∂∂ 
return
∏∏ 
mapper
∏∏ 
.
∏∏ 
Map
∏∏ 
<
∏∏ 
AppointmentDto
∏∏ ,
>
∏∏, -
(
∏∏- . 
updatedAppointment
∏∏. @
)
∏∏@ A
;
∏∏A B
}
ππ 	
public
ªª 
async
ªª 
Task
ªª 
<
ªª 
AppointmentDto
ªª (
>
ªª( )%
ConfirmAppointmentAsync
ªª* A
(
ªªA B
int
ªªB E
appointmentId
ªªF S
)
ªªS T
{
ºº 	#
ValidateAppointmentId
ΩΩ !
(
ΩΩ! "
appointmentId
ΩΩ" /
)
ΩΩ/ 0
;
ΩΩ0 1
var
øø 
appointment
øø 
=
øø 
await
øø ##
appointmentRepository
øø$ 9
.
øø9 :
GetByIdAsync
øø: F
(
øøF G
appointmentId
øøG T
)
øøT U
;
øøU V
if
¡¡ 
(
¡¡ 
appointment
¡¡ 
is
¡¡ 
null
¡¡ #
)
¡¡# $
{
¬¬ 
throw
√√ 
new
√√ %
EntityNotFoundException
√√ 1
(
√√1 2
$str
√√2 ?
,
√√? @
appointmentId
√√A N
)
√√N O
;
√√O P
}
ƒƒ 
if
∆∆ 
(
∆∆ 
appointment
∆∆ 
.
∆∆ 
Status
∆∆ "
==
∆∆# %
AppointmentStatus
∆∆& 7
.
∆∆7 8
	Cancelled
∆∆8 A
)
∆∆A B
{
«« 
throw
»» 
new
»» 
ConflictException
»» +
(
»»+ ,
$str
»», X
)
»»X Y
;
»»Y Z
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
	Completed
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
ÕÕ, ^
)
ÕÕ^ _
;
ÕÕ_ `
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
	Confirmed
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
““, O
)
““O P
;
““P Q
}
”” 
appointment
’’ 
.
’’ 
Status
’’ 
=
’’  
AppointmentStatus
’’! 2
.
’’2 3
	Confirmed
’’3 <
;
’’< =
appointment
÷÷ 
.
÷÷  
CancellationReason
÷÷ *
=
÷÷+ ,
null
÷÷- 1
;
÷÷1 2
var
ÿÿ  
updatedAppointment
ÿÿ "
=
ÿÿ# $
await
ÿÿ% *#
appointmentRepository
ÿÿ+ @
.
ÿÿ@ A
UpdateAsync
ÿÿA L
(
ÿÿL M
appointmentId
ŸŸ 
,
ŸŸ 
appointment
⁄⁄ 
)
⁄⁄ 
;
⁄⁄ 
if
‹‹ 
(
‹‹  
updatedAppointment
‹‹ "
is
‹‹# %
null
‹‹& *
)
‹‹* +
{
›› 
throw
ﬁﬁ 
new
ﬁﬁ %
EntityNotFoundException
ﬁﬁ 1
(
ﬁﬁ1 2
$str
ﬁﬁ2 ?
,
ﬁﬁ? @
appointmentId
ﬁﬁA N
)
ﬁﬁN O
;
ﬁﬁO P
}
ﬂﬂ 
return
·· 
mapper
·· 
.
·· 
Map
·· 
<
·· 
AppointmentDto
·· ,
>
··, -
(
··- . 
updatedAppointment
··. @
)
··@ A
;
··A B
}
‚‚ 	
public
‰‰ 
async
‰‰ 
Task
‰‰ 
<
‰‰ 
AppointmentDto
‰‰ (
>
‰‰( )&
CompleteAppointmentAsync
‰‰* B
(
‰‰B C
int
‰‰C F
appointmentId
‰‰G T
)
‰‰T U
{
ÂÂ 	#
ValidateAppointmentId
ÊÊ !
(
ÊÊ! "
appointmentId
ÊÊ" /
)
ÊÊ/ 0
;
ÊÊ0 1
var
ËË 
appointment
ËË 
=
ËË 
await
ËË ##
appointmentRepository
ËË$ 9
.
ËË9 :
GetByIdAsync
ËË: F
(
ËËF G
appointmentId
ËËG T
)
ËËT U
;
ËËU V
if
ÍÍ 
(
ÍÍ 
appointment
ÍÍ 
is
ÍÍ 
null
ÍÍ #
)
ÍÍ# $
{
ÎÎ 
throw
ÏÏ 
new
ÏÏ %
EntityNotFoundException
ÏÏ 1
(
ÏÏ1 2
$str
ÏÏ2 ?
,
ÏÏ? @
appointmentId
ÏÏA N
)
ÏÏN O
;
ÏÏO P
}
ÌÌ 
if
ÔÔ 
(
ÔÔ 
appointment
ÔÔ 
.
ÔÔ 
Status
ÔÔ "
==
ÔÔ# %
AppointmentStatus
ÔÔ& 7
.
ÔÔ7 8
	Cancelled
ÔÔ8 A
)
ÔÔA B
{
 
throw
ÒÒ 
new
ÒÒ 
ConflictException
ÒÒ +
(
ÒÒ+ ,
$str
ÒÒ, X
)
ÒÒX Y
;
ÒÒY Z
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
	Completed
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
ˆˆ, O
)
ˆˆO P
;
ˆˆP Q
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
!=
˘˘# %
AppointmentStatus
˘˘& 7
.
˘˘7 8
	Confirmed
˘˘8 A
)
˘˘A B
{
˙˙ 
throw
˚˚ 
new
˚˚ &
AppointmentRuleException
˚˚ 2
(
˚˚2 3
$str
˚˚3 b
)
˚˚b c
;
˚˚c d
}
¸¸ 
appointment
˛˛ 
.
˛˛ 
Status
˛˛ 
=
˛˛  
AppointmentStatus
˛˛! 2
.
˛˛2 3
	Completed
˛˛3 <
;
˛˛< =
var
ÄÄ  
updatedAppointment
ÄÄ "
=
ÄÄ# $
await
ÄÄ% *#
appointmentRepository
ÄÄ+ @
.
ÄÄ@ A
UpdateAsync
ÄÄA L
(
ÄÄL M
appointmentId
ÅÅ 
,
ÅÅ 
appointment
ÇÇ 
)
ÇÇ 
;
ÇÇ 
if
ÑÑ 
(
ÑÑ  
updatedAppointment
ÑÑ "
is
ÑÑ# %
null
ÑÑ& *
)
ÑÑ* +
{
ÖÖ 
throw
ÜÜ 
new
ÜÜ %
EntityNotFoundException
ÜÜ 1
(
ÜÜ1 2
$str
ÜÜ2 ?
,
ÜÜ? @
appointmentId
ÜÜA N
)
ÜÜN O
;
ÜÜO P
}
áá 
return
ââ 
mapper
ââ 
.
ââ 
Map
ââ 
<
ââ 
AppointmentDto
ââ ,
>
ââ, -
(
ââ- . 
updatedAppointment
ââ. @
)
ââ@ A
;
ââA B
}
ää 	
public
åå 
async
åå 
Task
åå 
<
åå 
AppointmentDto
åå (
>
åå( )$
CancelAppointmentAsync
åå* @
(
åå@ A"
CancelAppointmentDto
ååA U
dto
ååV Y
)
ååY Z
{
çç 	
if
éé 
(
éé 
dto
éé 
is
éé 
null
éé 
)
éé 
{
èè 
throw
êê 
new
êê &
AppointmentRuleException
êê 2
(
êê2 3
$str
êê3 W
)
êêW X
;
êêX Y
}
ëë #
ValidateAppointmentId
ìì !
(
ìì! "
dto
ìì" %
.
ìì% &
AppointmentId
ìì& 3
)
ìì3 4
;
ìì4 5(
ValidateCancellationReason
ïï &
(
ïï& '
dto
ïï' *
.
ïï* +
Reason
ïï+ 1
)
ïï1 2
;
ïï2 3
var
óó 
appointment
óó 
=
óó 
await
óó ##
appointmentRepository
óó$ 9
.
óó9 :
GetByIdAsync
óó: F
(
óóF G
dto
óóG J
.
óóJ K
AppointmentId
óóK X
)
óóX Y
;
óóY Z
if
ôô 
(
ôô 
appointment
ôô 
is
ôô 
null
ôô #
)
ôô# $
{
öö 
throw
õõ 
new
õõ %
EntityNotFoundException
õõ 1
(
õõ1 2
$str
õõ2 ?
,
õõ? @
dto
õõA D
.
õõD E
AppointmentId
õõE R
)
õõR S
;
õõS T
}
úú 
if
ûû 
(
ûû 
appointment
ûû 
.
ûû 
Status
ûû "
==
ûû# %
AppointmentStatus
ûû& 7
.
ûû7 8
	Completed
ûû8 A
)
ûûA B
{
üü 
throw
†† 
new
†† 
ConflictException
†† +
(
††+ ,
$str
††, X
)
††X Y
;
††Y Z
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
	Cancelled
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
••, O
)
••O P
;
••P Q
}
¶¶ 
appointment
®® 
.
®® 
Status
®® 
=
®®  
AppointmentStatus
®®! 2
.
®®2 3
	Cancelled
®®3 <
;
®®< =
appointment
©© 
.
©©  
CancellationReason
©© *
=
©©+ ,
dto
©©- 0
.
©©0 1
Reason
©©1 7
.
©©7 8
Trim
©©8 <
(
©©< =
)
©©= >
;
©©> ?
var
´´  
updatedAppointment
´´ "
=
´´# $
await
´´% *#
appointmentRepository
´´+ @
.
´´@ A
UpdateAsync
´´A L
(
´´L M
dto
¨¨ 
.
¨¨ 
AppointmentId
¨¨ !
,
¨¨! "
appointment
≠≠ 
)
≠≠ 
;
≠≠ 
if
ØØ 
(
ØØ  
updatedAppointment
ØØ "
is
ØØ# %
null
ØØ& *
)
ØØ* +
{
∞∞ 
throw
±± 
new
±± %
EntityNotFoundException
±± 1
(
±±1 2
$str
±±2 ?
,
±±? @
dto
±±A D
.
±±D E
AppointmentId
±±E R
)
±±R S
;
±±S T
}
≤≤ 
return
¥¥ 
mapper
¥¥ 
.
¥¥ 
Map
¥¥ 
<
¥¥ 
AppointmentDto
¥¥ ,
>
¥¥, -
(
¥¥- . 
updatedAppointment
¥¥. @
)
¥¥@ A
;
¥¥A B
}
µµ 	
public
∑∑ 
async
∑∑ 
Task
∑∑ 
<
∑∑ 
AppointmentDto
∑∑ (
>
∑∑( )$
DeleteAppointmentAsync
∑∑* @
(
∑∑@ A
int
∑∑A D
appointmentId
∑∑E R
)
∑∑R S
{
∏∏ 	#
ValidateAppointmentId
ππ !
(
ππ! "
appointmentId
ππ" /
)
ππ/ 0
;
ππ0 1
var
ªª 
appointment
ªª 
=
ªª 
await
ªª ##
appointmentRepository
ªª$ 9
.
ªª9 :
GetByIdAsync
ªª: F
(
ªªF G
appointmentId
ªªG T
)
ªªT U
;
ªªU V
if
ΩΩ 
(
ΩΩ 
appointment
ΩΩ 
is
ΩΩ 
null
ΩΩ #
)
ΩΩ# $
{
ææ 
throw
øø 
new
øø %
EntityNotFoundException
øø 1
(
øø1 2
$str
øø2 ?
,
øø? @
appointmentId
øøA N
)
øøN O
;
øøO P
}
¿¿ 
var
¬¬ 
hasHealthRecord
¬¬ 
=
¬¬  !
await
¬¬" '$
healthRecordRepository
¬¬( >
.
¬¬> ?(
ExistsByAppointmentIdAsync
¬¬? Y
(
¬¬Y Z
appointmentId
¬¬Z g
)
¬¬g h
;
¬¬h i
if
ƒƒ 
(
ƒƒ 
hasHealthRecord
ƒƒ 
)
ƒƒ  
{
≈≈ 
throw
∆∆ 
new
∆∆ 
ConflictException
∆∆ +
(
∆∆+ ,
$str
∆∆, |
)
∆∆| }
;
∆∆} ~
}
«« 
var
……  
deletedAppointment
…… "
=
……# $
await
……% *#
appointmentRepository
……+ @
.
……@ A
DeleteAsync
……A L
(
……L M
appointmentId
……M Z
)
……Z [
;
……[ \
if
ÀÀ 
(
ÀÀ  
deletedAppointment
ÀÀ "
is
ÀÀ# %
null
ÀÀ& *
)
ÀÀ* +
{
ÃÃ 
throw
ÕÕ 
new
ÕÕ %
EntityNotFoundException
ÕÕ 1
(
ÕÕ1 2
$str
ÕÕ2 ?
,
ÕÕ? @
appointmentId
ÕÕA N
)
ÕÕN O
;
ÕÕO P
}
ŒŒ 
return
–– 
mapper
–– 
.
–– 
Map
–– 
<
–– 
AppointmentDto
–– ,
>
––, -
(
––- . 
deletedAppointment
––. @
)
––@ A
;
––A B
}
—— 	
public
”” 
async
”” 
Task
”” 
<
”” 
List
”” 
<
”” 
AppointmentDto
”” -
>
””- .
>
””. /.
 GetMyAppointmentsForPatientAsync
””0 P
(
””P Q
string
””Q W
identityUserId
””X f
)
””f g
{
‘‘ 	
var
’’ 
patient
’’ 
=
’’ 
await
’’ %
GetLoggedInPatientAsync
’’  7
(
’’7 8
identityUserId
’’8 F
)
’’F G
;
’’G H
var
◊◊ 
appointments
◊◊ 
=
◊◊ 
await
◊◊ $#
appointmentRepository
◊◊% :
.
◊◊: ;!
GetByPatientIdAsync
◊◊; N
(
◊◊N O
patient
◊◊O V
.
◊◊V W
	PatientId
◊◊W `
)
◊◊` a
;
◊◊a b
return
ŸŸ 
mapper
ŸŸ 
.
ŸŸ 
Map
ŸŸ 
<
ŸŸ 
List
ŸŸ "
<
ŸŸ" #
AppointmentDto
ŸŸ# 1
>
ŸŸ1 2
>
ŸŸ2 3
(
ŸŸ3 4
appointments
ŸŸ4 @
)
ŸŸ@ A
;
ŸŸA B
}
⁄⁄ 	
public
‹‹ 
async
‹‹ 
Task
‹‹ 
<
‹‹ 
List
‹‹ 
<
‹‹ 
AppointmentDto
‹‹ -
>
‹‹- .
>
‹‹. /6
(GetMyUpcomingAppointmentsForPatientAsync
‹‹0 X
(
‹‹X Y
string
‹‹Y _
identityUserId
‹‹` n
)
‹‹n o
{
›› 	
var
ﬁﬁ 
patient
ﬁﬁ 
=
ﬁﬁ 
await
ﬁﬁ %
GetLoggedInPatientAsync
ﬁﬁ  7
(
ﬁﬁ7 8
identityUserId
ﬁﬁ8 F
)
ﬁﬁF G
;
ﬁﬁG H
var
‡‡ 
appointments
‡‡ 
=
‡‡ 
await
‡‡ $#
appointmentRepository
‡‡% :
.
‡‡: ;5
'GetUpcomingAppointmentsByPatientIdAsync
‡‡; b
(
‡‡b c
patient
‡‡c j
.
‡‡j k
	PatientId
‡‡k t
)
‡‡t u
;
‡‡u v
return
‚‚ 
mapper
‚‚ 
.
‚‚ 
Map
‚‚ 
<
‚‚ 
List
‚‚ "
<
‚‚" #
AppointmentDto
‚‚# 1
>
‚‚1 2
>
‚‚2 3
(
‚‚3 4
appointments
‚‚4 @
)
‚‚@ A
;
‚‚A B
}
„„ 	
public
ÂÂ 
async
ÂÂ 
Task
ÂÂ 
<
ÂÂ 
List
ÂÂ 
<
ÂÂ 
AppointmentDto
ÂÂ -
>
ÂÂ- .
>
ÂÂ. /5
'GetMyPendingAppointmentsForPatientAsync
ÂÂ0 W
(
ÂÂW X
string
ÂÂX ^
identityUserId
ÂÂ_ m
)
ÂÂm n
{
ÊÊ 	
var
ÁÁ 
patient
ÁÁ 
=
ÁÁ 
await
ÁÁ %
GetLoggedInPatientAsync
ÁÁ  7
(
ÁÁ7 8
identityUserId
ÁÁ8 F
)
ÁÁF G
;
ÁÁG H
var
ÈÈ 
appointments
ÈÈ 
=
ÈÈ 
await
ÈÈ $#
appointmentRepository
ÈÈ% :
.
ÈÈ: ;4
&GetPendingAppointmentsByPatientIdAsync
ÈÈ; a
(
ÈÈa b
patient
ÈÈb i
.
ÈÈi j
	PatientId
ÈÈj s
)
ÈÈs t
;
ÈÈt u
return
ÎÎ 
mapper
ÎÎ 
.
ÎÎ 
Map
ÎÎ 
<
ÎÎ 
List
ÎÎ "
<
ÎÎ" #
AppointmentDto
ÎÎ# 1
>
ÎÎ1 2
>
ÎÎ2 3
(
ÎÎ3 4
appointments
ÎÎ4 @
)
ÎÎ@ A
;
ÎÎA B
}
ÏÏ 	
public
ÓÓ 
async
ÓÓ 
Task
ÓÓ 
<
ÓÓ 
AppointmentDto
ÓÓ (
>
ÓÓ( )/
!GetAppointmentByIdForPatientAsync
ÓÓ* K
(
ÓÓK L
int
ÔÔ 
appointmentId
ÔÔ 
,
ÔÔ 
string
 
identityUserId
 !
)
! "
{
ÒÒ 	#
ValidateAppointmentId
ÚÚ !
(
ÚÚ! "
appointmentId
ÚÚ" /
)
ÚÚ/ 0
;
ÚÚ0 1
var
ÙÙ 
patient
ÙÙ 
=
ÙÙ 
await
ÙÙ %
GetLoggedInPatientAsync
ÙÙ  7
(
ÙÙ7 8
identityUserId
ÙÙ8 F
)
ÙÙF G
;
ÙÙG H
var
ˆˆ 
appointment
ˆˆ 
=
ˆˆ 
await
ˆˆ ##
appointmentRepository
ˆˆ$ 9
.
ˆˆ9 :
GetByIdAsync
ˆˆ: F
(
ˆˆF G
appointmentId
ˆˆG T
)
ˆˆT U
;
ˆˆU V
if
¯¯ 
(
¯¯ 
appointment
¯¯ 
is
¯¯ 
null
¯¯ #
)
¯¯# $
{
˘˘ 
throw
˙˙ 
new
˙˙ %
EntityNotFoundException
˙˙ 1
(
˙˙1 2
$str
˙˙2 ?
,
˙˙? @
appointmentId
˙˙A N
)
˙˙N O
;
˙˙O P
}
˚˚ 
if
˝˝ 
(
˝˝ 
appointment
˝˝ 
.
˝˝ 
	PatientId
˝˝ %
!=
˝˝& (
patient
˝˝) 0
.
˝˝0 1
	PatientId
˝˝1 :
)
˝˝: ;
{
˛˛ 
throw
ˇˇ 
new
ˇˇ &
ForbiddenAccessException
ˇˇ 2
(
ˇˇ2 3
$str
ˇˇ3 e
)
ˇˇe f
;
ˇˇf g
}
ÄÄ 
return
ÇÇ 
mapper
ÇÇ 
.
ÇÇ 
Map
ÇÇ 
<
ÇÇ 
AppointmentDto
ÇÇ ,
>
ÇÇ, -
(
ÇÇ- .
appointment
ÇÇ. 9
)
ÇÇ9 :
;
ÇÇ: ;
}
ÉÉ 	
public
ÖÖ 
async
ÖÖ 
Task
ÖÖ 
<
ÖÖ 
AppointmentDto
ÖÖ (
>
ÖÖ( ),
BookAppointmentForPatientAsync
ÖÖ* H
(
ÖÖH I 
BookAppointmentDto
ÜÜ 
dto
ÜÜ "
,
ÜÜ" #
string
áá 
identityUserId
áá !
)
áá! "
{
àà 	
if
ââ 
(
ââ 
dto
ââ 
is
ââ 
null
ââ 
)
ââ 
{
ää 
throw
ãã 
new
ãã &
AppointmentRuleException
ãã 2
(
ãã2 3
$str
ãã3 V
)
ããV W
;
ããW X
}
åå 
var
éé 
patient
éé 
=
éé 
await
éé %
GetLoggedInPatientAsync
éé  7
(
éé7 8
identityUserId
éé8 F
)
ééF G
;
ééG H
dto
íí 
.
íí 
	PatientId
íí 
=
íí 
patient
íí #
.
íí# $
	PatientId
íí$ -
;
íí- .
return
îî 
await
îî "
BookAppointmentAsync
îî -
(
îî- .
dto
îî. 1
)
îî1 2
;
îî2 3
}
ïï 	
public
óó 
async
óó 
Task
óó 
<
óó 
AppointmentDto
óó (
>
óó( ).
 CancelAppointmentForPatientAsync
óó* J
(
óóJ K"
CancelAppointmentDto
òò  
dto
òò! $
,
òò$ %
string
ôô 
identityUserId
ôô !
)
ôô! "
{
öö 	
if
õõ 
(
õõ 
dto
õõ 
is
õõ 
null
õõ 
)
õõ 
{
úú 
throw
ùù 
new
ùù &
AppointmentRuleException
ùù 2
(
ùù2 3
$str
ùù3 W
)
ùùW X
;
ùùX Y
}
ûû #
ValidateAppointmentId
†† !
(
††! "
dto
††" %
.
††% &
AppointmentId
††& 3
)
††3 4
;
††4 5
var
¢¢ 
patient
¢¢ 
=
¢¢ 
await
¢¢ %
GetLoggedInPatientAsync
¢¢  7
(
¢¢7 8
identityUserId
¢¢8 F
)
¢¢F G
;
¢¢G H
var
§§ 
appointment
§§ 
=
§§ 
await
§§ ##
appointmentRepository
§§$ 9
.
§§9 :
GetByIdAsync
§§: F
(
§§F G
dto
§§G J
.
§§J K
AppointmentId
§§K X
)
§§X Y
;
§§Y Z
if
¶¶ 
(
¶¶ 
appointment
¶¶ 
is
¶¶ 
null
¶¶ #
)
¶¶# $
{
ßß 
throw
®® 
new
®® %
EntityNotFoundException
®® 1
(
®®1 2
$str
®®2 ?
,
®®? @
dto
®®A D
.
®®D E
AppointmentId
®®E R
)
®®R S
;
®®S T
}
©© 
if
´´ 
(
´´ 
appointment
´´ 
.
´´ 
	PatientId
´´ %
!=
´´& (
patient
´´) 0
.
´´0 1
	PatientId
´´1 :
)
´´: ;
{
¨¨ 
throw
≠≠ 
new
≠≠ &
ForbiddenAccessException
≠≠ 2
(
≠≠2 3
$str
≠≠3 e
)
≠≠e f
;
≠≠f g
}
ÆÆ 
return
∞∞ 
await
∞∞ $
CancelAppointmentAsync
∞∞ /
(
∞∞/ 0
dto
∞∞0 3
)
∞∞3 4
;
∞∞4 5
}
±± 	
public
≤≤ 
async
≤≤ 
Task
≤≤ 
<
≤≤ 
List
≤≤ 
<
≤≤ 
AppointmentDto
≤≤ -
>
≤≤- .
>
≤≤. /-
GetMyAppointmentsForDoctorAsync
≤≤0 O
(
≤≤O P
string
≤≤P V
identityUserId
≤≤W e
)
≤≤e f
{
≥≥ 	
var
¥¥ 
doctor
¥¥ 
=
¥¥ 
await
¥¥ $
GetLoggedInDoctorAsync
¥¥ 5
(
¥¥5 6
identityUserId
¥¥6 D
)
¥¥D E
;
¥¥E F
var
∂∂ 
appointments
∂∂ 
=
∂∂ 
await
∂∂ $#
appointmentRepository
∂∂% :
.
∂∂: ; 
GetByDoctorIdAsync
∂∂; M
(
∂∂M N
doctor
∂∂N T
.
∂∂T U
DoctorId
∂∂U ]
)
∂∂] ^
;
∂∂^ _
return
∏∏ 
mapper
∏∏ 
.
∏∏ 
Map
∏∏ 
<
∏∏ 
List
∏∏ "
<
∏∏" #
AppointmentDto
∏∏# 1
>
∏∏1 2
>
∏∏2 3
(
∏∏3 4
appointments
∏∏4 @
)
∏∏@ A
;
∏∏A B
}
ππ 	
public
∫∫ 
async
∫∫ 
Task
∫∫ 
<
∫∫ 
List
∫∫ 
<
∫∫ 
AppointmentDto
∫∫ -
>
∫∫- .
>
∫∫. /5
'GetMyUpcomingAppointmentsForDoctorAsync
∫∫0 W
(
∫∫W X
string
∫∫X ^
identityUserId
∫∫_ m
)
∫∫m n
{
ªª 	
var
ºº 
doctor
ºº 
=
ºº 
await
ºº $
GetLoggedInDoctorAsync
ºº 5
(
ºº5 6
identityUserId
ºº6 D
)
ººD E
;
ººE F
var
ææ 
appointments
ææ 
=
ææ 
await
ææ $#
appointmentRepository
ææ% :
.
ææ: ;4
&GetUpcomingAppointmentsByDoctorIdAsync
ææ; a
(
ææa b
doctor
ææb h
.
ææh i
DoctorId
ææi q
)
ææq r
;
æær s
return
¿¿ 
mapper
¿¿ 
.
¿¿ 
Map
¿¿ 
<
¿¿ 
List
¿¿ "
<
¿¿" #
AppointmentDto
¿¿# 1
>
¿¿1 2
>
¿¿2 3
(
¿¿3 4
appointments
¿¿4 @
)
¿¿@ A
;
¿¿A B
}
¡¡ 	
public
√√ 
async
√√ 
Task
√√ 
<
√√ 
List
√√ 
<
√√ 
AppointmentDto
√√ -
>
√√- .
>
√√. /4
&GetMyPendingAppointmentsForDoctorAsync
√√0 V
(
√√V W
string
√√W ]
identityUserId
√√^ l
)
√√l m
{
ƒƒ 	
var
≈≈ 
doctor
≈≈ 
=
≈≈ 
await
≈≈ $
GetLoggedInDoctorAsync
≈≈ 5
(
≈≈5 6
identityUserId
≈≈6 D
)
≈≈D E
;
≈≈E F
var
«« 
appointments
«« 
=
«« 
await
«« $#
appointmentRepository
««% :
.
««: ;3
%GetPendingAppointmentsByDoctorIdAsync
««; `
(
««` a
doctor
««a g
.
««g h
DoctorId
««h p
)
««p q
;
««q r
return
…… 
mapper
…… 
.
…… 
Map
…… 
<
…… 
List
…… "
<
……" #
AppointmentDto
……# 1
>
……1 2
>
……2 3
(
……3 4
appointments
……4 @
)
……@ A
;
……A B
}
   	
public
ÃÃ 
async
ÃÃ 
Task
ÃÃ 
<
ÃÃ 
List
ÃÃ 
<
ÃÃ 
AppointmentDto
ÃÃ -
>
ÃÃ- .
>
ÃÃ. /;
-GetMyTodayConfirmedAppointmentsForDoctorAsync
ÃÃ0 ]
(
ÃÃ] ^
string
ÃÃ^ d
identityUserId
ÃÃe s
)
ÃÃs t
{
ÕÕ 	
var
ŒŒ 
doctor
ŒŒ 
=
ŒŒ 
await
ŒŒ $
GetLoggedInDoctorAsync
ŒŒ 5
(
ŒŒ5 6
identityUserId
ŒŒ6 D
)
ŒŒD E
;
ŒŒE F
var
–– 
appointments
–– 
=
–– 
await
–– $#
appointmentRepository
––% :
.
––: ;:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
––; g
(
––g h
doctor
––h n
.
––n o
DoctorId
––o w
)
––w x
;
––x y
return
““ 
mapper
““ 
.
““ 
Map
““ 
<
““ 
List
““ "
<
““" #
AppointmentDto
““# 1
>
““1 2
>
““2 3
(
““3 4
appointments
““4 @
)
““@ A
;
““A B
}
”” 	
public
’’ 
async
’’ 
Task
’’ 
<
’’ 
AppointmentDto
’’ (
>
’’( ).
 GetAppointmentByIdForDoctorAsync
’’* J
(
’’J K
int
÷÷ 
appointmentId
÷÷ 
,
÷÷ 
string
◊◊ 

identityUserId
◊◊ 
)
◊◊ 
{
ÿÿ 	#
ValidateAppointmentId
ŸŸ !
(
ŸŸ! "
appointmentId
ŸŸ" /
)
ŸŸ/ 0
;
ŸŸ0 1
var
€€ 
doctor
€€ 
=
€€ 
await
€€ $
GetLoggedInDoctorAsync
€€ 5
(
€€5 6
identityUserId
€€6 D
)
€€D E
;
€€E F
var
›› 
appointment
›› 
=
›› 
await
›› ##
appointmentRepository
››$ 9
.
››9 :
GetByIdAsync
››: F
(
››F G
appointmentId
››G T
)
››T U
;
››U V
if
ﬂﬂ 
(
ﬂﬂ 
appointment
ﬂﬂ 
is
ﬂﬂ 
null
ﬂﬂ #
)
ﬂﬂ# $
{
‡‡ 
throw
·· 
new
·· %
EntityNotFoundException
·· 1
(
··1 2
$str
··2 ?
,
··? @
appointmentId
··A N
)
··N O
;
··O P
}
‚‚ 
if
‰‰ 
(
‰‰ 
appointment
‰‰ 
.
‰‰ 
DoctorId
‰‰ $
!=
‰‰% '
doctor
‰‰( .
.
‰‰. /
DoctorId
‰‰/ 7
)
‰‰7 8
{
ÂÂ 
throw
ÊÊ 
new
ÊÊ &
ForbiddenAccessException
ÊÊ 2
(
ÊÊ2 3
$str
ÊÊ3 d
)
ÊÊd e
;
ÊÊe f
}
ÁÁ 
return
ÈÈ 
mapper
ÈÈ 
.
ÈÈ 
Map
ÈÈ 
<
ÈÈ 
AppointmentDto
ÈÈ ,
>
ÈÈ, -
(
ÈÈ- .
appointment
ÈÈ. 9
)
ÈÈ9 :
;
ÈÈ: ;
}
ÍÍ 	
public
ÏÏ 
async
ÏÏ 
Task
ÏÏ 
<
ÏÏ 
AppointmentDto
ÏÏ (
>
ÏÏ( ).
 ConfirmAppointmentForDoctorAsync
ÏÏ* J
(
ÏÏJ K
int
ÌÌ 
appointmentId
ÌÌ 
,
ÌÌ 
string
ÓÓ 

identityUserId
ÓÓ 
)
ÓÓ 
{
ÔÔ 	#
ValidateAppointmentId
 !
(
! "
appointmentId
" /
)
/ 0
;
0 1
var
ÚÚ 
doctor
ÚÚ 
=
ÚÚ 
await
ÚÚ $
GetLoggedInDoctorAsync
ÚÚ 5
(
ÚÚ5 6
identityUserId
ÚÚ6 D
)
ÚÚD E
;
ÚÚE F
var
ÙÙ 
appointment
ÙÙ 
=
ÙÙ 
await
ÙÙ ##
appointmentRepository
ÙÙ$ 9
.
ÙÙ9 :
GetByIdAsync
ÙÙ: F
(
ÙÙF G
appointmentId
ÙÙG T
)
ÙÙT U
;
ÙÙU V
if
ˆˆ 
(
ˆˆ 
appointment
ˆˆ 
is
ˆˆ 
null
ˆˆ #
)
ˆˆ# $
{
˜˜ 
throw
¯¯ 
new
¯¯ %
EntityNotFoundException
¯¯ 1
(
¯¯1 2
$str
¯¯2 ?
,
¯¯? @
appointmentId
¯¯A N
)
¯¯N O
;
¯¯O P
}
˘˘ 
if
˚˚ 
(
˚˚ 
appointment
˚˚ 
.
˚˚ 
DoctorId
˚˚ $
!=
˚˚% '
doctor
˚˚( .
.
˚˚. /
DoctorId
˚˚/ 7
)
˚˚7 8
{
¸¸ 
throw
˝˝ 
new
˝˝ &
ForbiddenAccessException
˝˝ 2
(
˝˝2 3
$str
˝˝3 e
)
˝˝e f
;
˝˝f g
}
˛˛ 
return
ÄÄ 
await
ÄÄ %
ConfirmAppointmentAsync
ÄÄ 0
(
ÄÄ0 1
appointmentId
ÄÄ1 >
)
ÄÄ> ?
;
ÄÄ? @
}
ÅÅ 	
public
ÉÉ 
async
ÉÉ 
Task
ÉÉ 
<
ÉÉ 
AppointmentDto
ÉÉ (
>
ÉÉ( )/
!CompleteAppointmentForDoctorAsync
ÉÉ* K
(
ÉÉK L
int
ÑÑ 
appointmentId
ÑÑ 
,
ÑÑ 
string
ÖÖ 

identityUserId
ÖÖ 
)
ÖÖ 
{
ÜÜ 	#
ValidateAppointmentId
áá !
(
áá! "
appointmentId
áá" /
)
áá/ 0
;
áá0 1
var
ââ 
doctor
ââ 
=
ââ 
await
ââ $
GetLoggedInDoctorAsync
ââ 5
(
ââ5 6
identityUserId
ââ6 D
)
ââD E
;
ââE F
var
ãã 
appointment
ãã 
=
ãã 
await
ãã ##
appointmentRepository
ãã$ 9
.
ãã9 :
GetByIdAsync
ãã: F
(
ããF G
appointmentId
ããG T
)
ããT U
;
ããU V
if
çç 
(
çç 
appointment
çç 
is
çç 
null
çç #
)
çç# $
{
éé 
throw
èè 
new
èè %
EntityNotFoundException
èè 1
(
èè1 2
$str
èè2 ?
,
èè? @
appointmentId
èèA N
)
èèN O
;
èèO P
}
êê 
if
íí 
(
íí 
appointment
íí 
.
íí 
DoctorId
íí $
!=
íí% '
doctor
íí( .
.
íí. /
DoctorId
íí/ 7
)
íí7 8
{
ìì 
throw
îî 
new
îî &
ForbiddenAccessException
îî 2
(
îî2 3
$str
îî3 f
)
îîf g
;
îîg h
}
ïï 
return
óó 
await
óó &
CompleteAppointmentAsync
óó 1
(
óó1 2
appointmentId
óó2 ?
)
óó? @
;
óó@ A
}
òò 	
public
öö 
async
öö 
Task
öö 
<
öö 
AppointmentDto
öö (
>
öö( )-
CancelAppointmentForDoctorAsync
öö* I
(
ööI J"
CancelAppointmentDto
õõ 
dto
õõ 
,
õõ 
string
úú 

identityUserId
úú 
)
úú 
{
ùù 	
if
ûû 
(
ûû 
dto
ûû 
is
ûû 
null
ûû 
)
ûû 
{
üü 
throw
†† 
new
†† &
AppointmentRuleException
†† 2
(
††2 3
$str
††3 W
)
††W X
;
††X Y
}
°° #
ValidateAppointmentId
££ !
(
££! "
dto
££" %
.
££% &
AppointmentId
££& 3
)
££3 4
;
££4 5
var
•• 
doctor
•• 
=
•• 
await
•• $
GetLoggedInDoctorAsync
•• 5
(
••5 6
identityUserId
••6 D
)
••D E
;
••E F
var
ßß 
appointment
ßß 
=
ßß 
await
ßß ##
appointmentRepository
ßß$ 9
.
ßß9 :
GetByIdAsync
ßß: F
(
ßßF G
dto
ßßG J
.
ßßJ K
AppointmentId
ßßK X
)
ßßX Y
;
ßßY Z
if
©© 
(
©© 
appointment
©© 
is
©© 
null
©© #
)
©©# $
{
™™ 
throw
´´ 
new
´´ %
EntityNotFoundException
´´ 1
(
´´1 2
$str
´´2 ?
,
´´? @
dto
´´A D
.
´´D E
AppointmentId
´´E R
)
´´R S
;
´´S T
}
¨¨ 
if
ÆÆ 
(
ÆÆ 
appointment
ÆÆ 
.
ÆÆ 
DoctorId
ÆÆ $
!=
ÆÆ% '
doctor
ÆÆ( .
.
ÆÆ. /
DoctorId
ÆÆ/ 7
)
ÆÆ7 8
{
ØØ 
throw
∞∞ 
new
∞∞ &
ForbiddenAccessException
∞∞ 2
(
∞∞2 3
$str
∞∞3 d
)
∞∞d e
;
∞∞e f
}
±± 
return
≥≥ 
await
≥≥ $
CancelAppointmentAsync
≥≥ /
(
≥≥/ 0
dto
≥≥0 3
)
≥≥3 4
;
≥≥4 5
}
¥¥ 	
private
µµ 
async
µµ 
Task
µµ 
<
µµ 
Doctor
µµ !
>
µµ! "$
GetLoggedInDoctorAsync
µµ# 9
(
µµ9 :
string
µµ: @
identityUserId
µµA O
)
µµO P
{
∂∂ 	
if
∑∑ 
(
∑∑ 
string
∑∑ 
.
∑∑  
IsNullOrWhiteSpace
∑∑ )
(
∑∑) *
identityUserId
∑∑* 8
)
∑∑8 9
)
∑∑9 :
{
∏∏ 
throw
ππ 
new
ππ #
BusinessRuleException
ππ /
(
ππ/ 0
$str
ππ0 I
)
ππI J
;
ππJ K
}
∫∫ 
var
ºº 
doctor
ºº 
=
ºº 
await
ºº 
doctorRepository
ºº /
.
ºº/ 0&
GetByIdentityUserIdAsync
ºº0 H
(
ººH I
identityUserId
ººI W
)
ººW X
;
ººX Y
if
ææ 
(
ææ 
doctor
ææ 
is
ææ 
null
ææ 
)
ææ 
{
øø 
throw
¿¿ 
new
¿¿ %
EntityNotFoundException
¿¿ 1
(
¿¿1 2
$str
¿¿2 U
,
¿¿U V
$num
¿¿W X
)
¿¿X Y
;
¿¿Y Z
}
¡¡ 
return
√√ 
doctor
√√ 
;
√√ 
}
ƒƒ 	
private
≈≈ 
async
≈≈ 
Task
≈≈ 
<
≈≈ 
Patient
≈≈ "
>
≈≈" #%
GetLoggedInPatientAsync
≈≈$ ;
(
≈≈; <
string
≈≈< B
identityUserId
≈≈C Q
)
≈≈Q R
{
∆∆ 	
if
«« 
(
«« 
string
«« 
.
««  
IsNullOrWhiteSpace
«« )
(
««) *
identityUserId
««* 8
)
««8 9
)
««9 :
{
»» 
throw
…… 
new
…… #
BusinessRuleException
…… /
(
……/ 0
$str
……0 I
)
……I J
;
……J K
}
   
var
ÃÃ 
patient
ÃÃ 
=
ÃÃ 
await
ÃÃ 
patientRepository
ÃÃ  1
.
ÃÃ1 2&
GetByIdentityUserIdAsync
ÃÃ2 J
(
ÃÃJ K
identityUserId
ÃÃK Y
)
ÃÃY Z
;
ÃÃZ [
if
ŒŒ 
(
ŒŒ 
patient
ŒŒ 
is
ŒŒ 
null
ŒŒ 
)
ŒŒ  
{
œœ 
throw
–– 
new
–– %
EntityNotFoundException
–– 1
(
––1 2
$str
––2 V
,
––V W
$num
––X Y
)
––Y Z
;
––Z [
}
—— 
return
”” 
patient
”” 
;
”” 
}
‘‘ 	
private
’’ 
static
’’ 
void
’’ #
ValidateAppointmentId
’’ 1
(
’’1 2
int
’’2 5
appointmentId
’’6 C
)
’’C D
{
÷÷ 	
if
◊◊ 
(
◊◊ 
appointmentId
◊◊ 
<=
◊◊  
$num
◊◊! "
)
◊◊" #
{
ÿÿ 
throw
ŸŸ 
new
ŸŸ &
AppointmentRuleException
ŸŸ 2
(
ŸŸ2 3
$str
ŸŸ3 b
)
ŸŸb c
;
ŸŸc d
}
⁄⁄ 
}
€€ 	
private
›› 
static
›› 
void
›› 
ValidatePatientId
›› -
(
››- .
int
››. 1
	patientId
››2 ;
)
››; <
{
ﬁﬁ 	
if
ﬂﬂ 
(
ﬂﬂ 
	patientId
ﬂﬂ 
<=
ﬂﬂ 
$num
ﬂﬂ 
)
ﬂﬂ 
{
‡‡ 
throw
·· 
new
·· &
AppointmentRuleException
·· 2
(
··2 3
$str
··3 ^
)
··^ _
;
··_ `
}
‚‚ 
}
„„ 	
private
ÂÂ 
static
ÂÂ 
void
ÂÂ 
ValidateDoctorId
ÂÂ ,
(
ÂÂ, -
int
ÂÂ- 0
doctorId
ÂÂ1 9
)
ÂÂ9 :
{
ÊÊ 	
if
ÁÁ 
(
ÁÁ 
doctorId
ÁÁ 
<=
ÁÁ 
$num
ÁÁ 
)
ÁÁ 
{
ËË 
throw
ÈÈ 
new
ÈÈ &
AppointmentRuleException
ÈÈ 2
(
ÈÈ2 3
$str
ÈÈ3 ]
)
ÈÈ] ^
;
ÈÈ^ _
}
ÍÍ 
}
ÎÎ 	
private
ÌÌ 
async
ÌÌ 
Task
ÌÌ (
ValidatePatientExistsAsync
ÌÌ 5
(
ÌÌ5 6
int
ÌÌ6 9
	patientId
ÌÌ: C
)
ÌÌC D
{
ÓÓ 	
ValidatePatientId
ÔÔ 
(
ÔÔ 
	patientId
ÔÔ '
)
ÔÔ' (
;
ÔÔ( )
var
ÒÒ 
patient
ÒÒ 
=
ÒÒ 
await
ÒÒ 
patientRepository
ÒÒ  1
.
ÒÒ1 2
GetByIdAsync
ÒÒ2 >
(
ÒÒ> ?
	patientId
ÒÒ? H
)
ÒÒH I
;
ÒÒI J
if
ÛÛ 
(
ÛÛ 
patient
ÛÛ 
is
ÛÛ 
null
ÛÛ 
)
ÛÛ  
{
ÙÙ 
throw
ıı 
new
ıı %
EntityNotFoundException
ıı 1
(
ıı1 2
$str
ıı2 ;
,
ıı; <
	patientId
ıı= F
)
ııF G
;
ııG H
}
ˆˆ 
}
˜˜ 	
private
˘˘ 
async
˘˘ 
Task
˘˘ 
<
˘˘ 
Doctor
˘˘ !
>
˘˘! "'
ValidateDoctorExistsAsync
˘˘# <
(
˘˘< =
int
˘˘= @
doctorId
˘˘A I
)
˘˘I J
{
˙˙ 	
ValidateDoctorId
˚˚ 
(
˚˚ 
doctorId
˚˚ %
)
˚˚% &
;
˚˚& '
var
˝˝ 
doctor
˝˝ 
=
˝˝ 
await
˝˝ 
doctorRepository
˝˝ /
.
˝˝/ 0
GetByIdAsync
˝˝0 <
(
˝˝< =
doctorId
˝˝= E
)
˝˝E F
;
˝˝F G
if
ˇˇ 
(
ˇˇ 
doctor
ˇˇ 
is
ˇˇ 
null
ˇˇ 
)
ˇˇ 
{
ÄÄ 
throw
ÅÅ 
new
ÅÅ %
EntityNotFoundException
ÅÅ 1
(
ÅÅ1 2
$str
ÅÅ2 :
,
ÅÅ: ;
doctorId
ÅÅ< D
)
ÅÅD E
;
ÅÅE F
}
ÇÇ 
return
ÑÑ 
doctor
ÑÑ 
;
ÑÑ 
}
ÖÖ 	
private
áá 
static
áá 
void
áá (
ValidateDoctorAvailability
áá 6
(
áá6 7
Doctor
áá7 =
doctor
áá> D
)
ááD E
{
àà 	
if
ââ 
(
ââ 
!
ââ 
doctor
ââ 
.
ââ 
IsActive
ââ  
)
ââ  !
{
ää 
throw
ãã 
new
ãã &
AppointmentRuleException
ãã 2
(
ãã2 3
$str
ãã3 f
)
ããf g
;
ããg h
}
åå 
}
çç 	
private
èè 
static
èè 
void
èè %
ValidateAppointmentDate
èè 3
(
èè3 4
DateTime
èè4 <
scheduledDate
èè= J
)
èèJ K
{
êê 	
if
ëë 
(
ëë 
scheduledDate
ëë 
.
ëë 
Date
ëë "
<
ëë# $
DateTime
ëë% -
.
ëë- .
Today
ëë. 3
)
ëë3 4
{
íí 
throw
ìì 
new
ìì &
AppointmentRuleException
ìì 2
(
ìì2 3
$str
ìì3 \
)
ìì\ ]
;
ìì] ^
}
îî 
}
ïï 	
private
óó 
static
óó 
void
óó 
ValidateTimeSlot
óó ,
(
óó, -
string
óó- 3
timeSlot
óó4 <
)
óó< =
{
òò 	
if
ôô 
(
ôô 
string
ôô 
.
ôô  
IsNullOrWhiteSpace
ôô )
(
ôô) *
timeSlot
ôô* 2
)
ôô2 3
)
ôô3 4
{
öö 
throw
õõ 
new
õõ &
AppointmentRuleException
õõ 2
(
õõ2 3
$str
õõ3 K
)
õõK L
;
õõL M
}
úú 
if
ûû 
(
ûû 
!
ûû 
	TimeSlots
ûû 
.
ûû 
Slots
ûû  
.
ûû  !
Contains
ûû! )
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
††3 P
)
††P Q
;
††Q R
}
°° 
}
¢¢ 	
private
§§ 
static
§§ 
void
§§ (
ValidateCancellationReason
§§ 6
(
§§6 7
string
§§7 =
reason
§§> D
)
§§D E
{
•• 	
if
¶¶ 
(
¶¶ 
string
¶¶ 
.
¶¶  
IsNullOrWhiteSpace
¶¶ )
(
¶¶) *
reason
¶¶* 0
)
¶¶0 1
)
¶¶1 2
{
ßß 
throw
®® 
new
®® &
AppointmentRuleException
®® 2
(
®®2 3
$str
®®3 U
)
®®U V
;
®®V W
}
©© 
if
´´ 
(
´´ 
reason
´´ 
.
´´ 
Trim
´´ 
(
´´ 
)
´´ 
.
´´ 
Length
´´ $
>
´´% &
$num
´´' *
)
´´* +
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
≠≠3 f
)
≠≠f g
;
≠≠g h
}
ÆÆ 
}
ØØ 	
}
∞∞ 
}±± €
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
} ˘
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
} ˛-
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
}bb Ó
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
}.. Ω,
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
}77 ò∑
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
}…… ≥&
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
};; »f
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
ÅÅ  !
varÉÉ 
app
ÉÉ 
=
ÉÉ 	
builder
ÉÉ
 
.
ÉÉ 
Build
ÉÉ 
(
ÉÉ 
)
ÉÉ 
;
ÉÉ 
usingÜÜ 
(
ÜÜ 
var
ÜÜ 

scope
ÜÜ 
=
ÜÜ 
app
ÜÜ 
.
ÜÜ 
Services
ÜÜ 
.
ÜÜ  
CreateScope
ÜÜ  +
(
ÜÜ+ ,
)
ÜÜ, -
)
ÜÜ- .
{áá 
var
àà 
roleManager
àà 
=
àà 
scope
àà 
.
àà 
ServiceProvider
àà +
.
àà+ , 
GetRequiredService
àà, >
<
àà> ?
RoleManager
àà? J
<
ààJ K
IdentityRole
ààK W
>
ààW X
>
ààX Y
(
ààY Z
)
ààZ [
;
àà[ \
var
ää 
userManager
ää 
=
ää 
scope
ää 
.
ää 
ServiceProvider
ää +
.
ää+ , 
GetRequiredService
ää, >
<
ää> ?
UserManager
ää? J
<
ääJ K
IdentityUser
ääK W
>
ääW X
>
ääX Y
(
ääY Z
)
ääZ [
;
ää[ \
await
åå 	

RoleSeeder
åå
 
.
åå 
SeedRoleAsync
åå "
(
åå" #
roleManager
åå# .
)
åå. /
;
åå/ 0
await
éé 	
AdminSeeder
éé
 
.
éé 
SeedAdminAsync
éé $
(
éé$ %
userManager
éé% 0
,
éé0 1
roleManager
éé2 =
,
éé= >
builder
éé? F
.
ééF G
Configuration
ééG T
)
ééT U
;
ééU V
}èè 
appëë 
.
ëë !
UseExceptionHandler
ëë 
(
ëë 
)
ëë 
;
ëë 
ifîî 
(
îî 
app
îî 
.
îî 
Environment
îî 
.
îî 
IsDevelopment
îî !
(
îî! "
)
îî" #
)
îî# $
{ïï 
app
ññ 
.
ññ 

UseSwagger
ññ 
(
ññ 
)
ññ 
;
ññ 
app
óó 
.
óó 
UseSwaggerUI
óó 
(
óó 
)
óó 
;
óó 
}òò 
appöö 
.
öö !
UseHttpsRedirection
öö 
(
öö 
)
öö 
;
öö 
appúú 
.
úú 
UseAuthentication
úú 
(
úú 
)
úú 
;
úú 
appûû 
.
ûû 
UseAuthorization
ûû 
(
ûû 
)
ûû 
;
ûû 
app†† 
.
†† 
MapControllers
†† 
(
†† 
)
†† 
;
†† 
app¢¢ 
.
¢¢ 
Run
¢¢ 
(
¢¢ 
)
¢¢ 	
;
¢¢	 
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
},, ÿ
aC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\UpdatePatientDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
UpdatePatientDto !
{		 
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! H
)

H I
]

I J
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* X
)X Y
]Y Z
public 
required 
string 
FullName '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
Required	 
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
Range	 
( 
typeof 
( 
DateTime 
) 
,  
$str! -
,- .
$str/ ;
,; <
ErrorMessage 
= 
$str K
)K L
]L M
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! F
)F G
]G H
public 

GenderType 
Gender  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
RegularExpression	 
( 
$str )
,) *
ErrorMessage+ 7
=8 9
$str: h
)h i
]i j
public 
required 
string 
PhoneNumber *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
[ 	
Required	 
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
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
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) V
)V W
]W X
public 
required 
string 
InsuranceId *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
}   
}!! ÷
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\UpdateHealthRecordDto.cs
	namespace 	
SharedClasses
 
. 
Dtos 
{ 
public 

class !
UpdateHealthRecordDto &
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! N
)N O
]O P
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* ]
)] ^
]^ _
public		 
required		 
string		 
	Diagnosis		 (
{		) *
get		+ .
;		. /
set		0 3
;		3 4
}		5 6
[ 	
Required	 
( 
ErrorMessage 
=  
$str! Q
)Q R
]R S
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* `
)` a
]a b
public 
required 
string 
Prescription +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage (
=) *
$str+ ^
)^ _
]_ `
public 
required 
string 
Notes $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
] 
public 
DateTime 
	VisitDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} Ø
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\UpdateDoctorDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
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
=		  
$str		! G
)		G H
]		H I
[

 	
StringLength

	 
(

 
$num

 
,

 
ErrorMessage

 '
=

( )
$str

* W
)

W X
]

X Y
public 
required 
string 
FullName '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
Required	 
( 
ErrorMessage 
=  
$str! M
)M N
]N O
public 
SpecialisationType !
Specialisation" 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
[ 	
Required	 
( 
ErrorMessage 
=  
$str! Q
)Q R
]R S
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
PracticeStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
[ 	
Required	 
( 
ErrorMessage 
=  
$str! E
)E F
]F G
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) Z
)Z [
][ \
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
IsActive 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ﬁ
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\UpdateAppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class  
UpdateAppointmentDto %
{ 
public		 
string		 
?		 
PatientName		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		/ 0
public 
string 
? 

DoctorName !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
] 
public 
string 
? 
AppointmentId $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
( 
ErrorMessage 
=  
$str! R
)R S
]S T
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ Z
)Z [
][ \
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! Q
)Q R
]R S
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ Y
)Y Z
]Z [
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
$str! M
)M N
]N O
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
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
=- .
string/ 5
.5 6
Empty6 ;
;; <
} 
}   ÷
\C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\RegisterDto.cs
	namespace 	
HealthCareApp
 
. 
Models 
. 
Dtos #
{ 
public 

class 
RegisterDto 
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% <
)< =
]= >
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
;		8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 7
)7 8
]8 9
public 
string 
Password 
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
=  
$str! ?
)? @
]@ A
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
} 
} Û 
cC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\PatientRegisterDto.cs
	namespace 	
HealthCareApp
 
. 
Models 
. 
Dtos #
{ 
public 

class 
PatientRegisterDto #
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! H
)H I
]I J
[		 	
StringLength			 
(		 
$num		 
,		 
ErrorMessage		 '
=		( )
$str		* X
)		X Y
]		Y Z
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
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
Range	 
( 
typeof 
( 
DateTime 
) 
,  
$str! -
,- .
$str/ ;
,; <
ErrorMessage 
= 
$str K
)K L
]L M
public 
DateTime 
DateOfBirth #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! F
)F G
]G H
public 

GenderType 
Gender  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) V
)V W
]W X
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) V
)V W
]W X
public 
string 
InsuranceId !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[   	
Required  	 
(   
ErrorMessage   
=    
$str  ! ;
)  ; <
]  < =
public!! 
string!! 
Password!! 
{!!  
get!!! $
;!!$ %
set!!& )
;!!) *
}!!+ ,
=!!- .
string!!/ 5
.!!5 6
Empty!!6 ;
;!!; <
[## 	
Required##	 
(## 
ErrorMessage## 
=##  
$str##! ?
)##? @
]##@ A
public$$ 
string$$ 
ConfirmPassword$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$2 3
=$$4 5
string$$6 <
.$$< =
Empty$$= B
;$$B C
}%% 
}&& Û
[C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\PatientDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 

PatientDto 
{ 
public 
int 
	PatientId 
{ 
get "
;" #
set$ '
;' (
}) *
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! H
)

H I
]

I J
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* X
)X Y
]Y Z
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
;; <
public 
string 
DateOfBirth !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
$str! F
)F G
]G H
public 

GenderType 
Gender  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
RegularExpression	 
( 
$str )
,) *
ErrorMessage+ 7
=8 9
$str: h
)h i
]i j
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
[ 	
Required	 
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) V
)V W
]W X
public 
string 
InsuranceId !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
string 
CreatedDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
}   
}!! Ñ
uC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\Pagination\PatientPaginationQueryDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class %
PatientPaginationQueryDto *
:+ ,
PaginationQueryDto- ?
{ 
public 
string 
? 

SearchTerm !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 

GenderType		 
?		 
Gender		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
public 
bool 
? 
HasInsurance !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ‘
nC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\Pagination\PaginationQueryDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
PaginationQueryDto #
{ 
public 
int 

PageNumber 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
$num. /
;/ 0
public 
int 
PageSize 
{ 
get !
;! "
set# &
;& '
}( )
=* +
$num, .
;. /
} 
}		 ‡	
iC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\Pagination\PagedResponse.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
PagedResponse 
< 
T  
>  !
{ 
public 
List 
< 
T 
> 
Items 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
new- 0
(0 1
)1 2
;2 3
public 
int 

PageNumber 
{ 
get  #
;# $
set% (
;( )
}* +
public		 
int		 
PageSize		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public 
int 
TotalRecords 
{  !
get" %
;% &
set' *
;* +
}, -
public 
int 

TotalPages 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} é
tC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\Pagination\DoctorPaginationQueryDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class $
DoctorPaginationQueryDto )
:* +
PaginationQueryDto, >
{ 
public 
SpecialisationType !
?! "
Specialisation# 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
public		 
bool		 
?		 
IsActive		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
public 
string 
? 

SearchTerm !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ñ
yC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\Pagination\AppointmentPaginationQueryDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class )
AppointmentPaginationQueryDto .
:/ 0
PaginationQueryDto1 C
{ 
public 
string 
? 

SearchTerm !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
int		 
?		 
	PatientId		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
public 
int 
? 
DoctorId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
AppointmentStatus  
?  !
Status" (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
DateTime 
? 
ScheduledDate &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
bool 
? 
UpcomingOnly !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} Ã	
YC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\LoginDto.cs
	namespace 	
HealthCareApp
 
. 
Models 
. 
Dtos #
{ 
public 

class 
LoginDto 
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% <
)< =
]= >
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
;		8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 7
)7 8
]8 9
public 
string 
Password 
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
} 
} á
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\HealthRecordDto.cs
	namespace 	
SharedClasses
 
. 
Dtos 
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
string		 
?		 
PatientName		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		/ 0
public 
int 
? 
DoctorId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
? 

DoctorName !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
	VisitDate 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
string0 6
.6 7
Empty7 <
;< =
public 
string 
	Diagnosis 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
string0 6
.6 7
Empty7 <
;< =
public 
string 
Prescription "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
public 
string 
? 
Notes 
{ 
get "
;" #
set$ '
;' (
}) *
} 
} ¿
^C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\ErrorResponse.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
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
} Ã
ZC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\DoctorDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
	DoctorDto 
{ 
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! G
)

G H
]

H I
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* W
)W X
]X Y
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
=  
$str! M
)M N
]N O
public 
SpecialisationType !
Specialisation" 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
int 
YearsOfExperience $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
( 
ErrorMessage 
=  
$str! E
)E F
]F G
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) Z
)Z [
][ \
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
} °
iC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\DoctorCreatedResponseDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class $
DoctorCreatedResponseDto )
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
string 

DoctorName  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
string1 7
.7 8
Empty8 =
;= >
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
string 
TemporaryPassword '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
=6 7
string8 >
.> ?
Empty? D
;D E
public 
string 
Message 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
} 
} ”
aC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\CreatePatientDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
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
=		  
$str		! H
)		H I
]		I J
[

 	
StringLength

	 
(

 
$num

 
,

 
ErrorMessage

 '
=

( )
$str

* X
)

X Y
]

Y Z
public 
required 
string 
FullName '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
Required	 
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
Range	 
( 
typeof 
( 
DateTime 
) 
,  
$str! -
,- .
$str/ ;
,; <
ErrorMessage 
= 
$str K
)K L
]L M
public 
DateTime 
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
$str! F
)F G
]G H
public 

GenderType 
Gender  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) V
)V W
]W X
public 
required 
string 
PhoneNumber *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
[ 	
Required	 
( 
ErrorMessage 
=  
$str! L
)L M
]M N
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
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
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage &
=' (
$str) V
)V W
]W X
public 
required 
string 
InsuranceId *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
}   
}!! †
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\CreateDoctorDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
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
=		  
$str		! G
)		G H
]		H I
[

 	
StringLength

	 
(

 
$num

 
,

 
ErrorMessage

 '
=

( )
$str

* W
)

W X
]

X Y
public 
required 
string 
FullName '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
Required	 
( 
ErrorMessage 
=  
$str! K
)K L
]L M
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% J
)J K
]K L
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
;8 9
[ 	
Required	 
( 
ErrorMessage 
=  
$str! M
)M N
]N O
public 
SpecialisationType !
Specialisation" 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
[ 	
Required	 
( 
ErrorMessage 
=  
$str! Q
)Q R
]R S
[ 	
DataType	 
( 
DataType 
. 
Date 
)  
]  !
public 
DateTime 
PracticeStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
[ 	
Required	 
( 
ErrorMessage 
=  
$str! E
)E F
]F G
[ 	
Range	 
( 
$num 
, 
$num 
, 
ErrorMessage &
=' (
$str) Z
)Z [
][ \
public 
decimal 
ConsultationFee &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} —
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\ConfirmAppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class !
ConfirmAppointmentDto &
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! X
)X Y
]Y Z
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ Y
)Y Z
]Z [
public		 
int		 
DoctorId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
}

 
} ”
gC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\CompleteAppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class "
CompleteAppointmentDto '
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! X
)X Y
]Y Z
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ Y
)Y Z
]Z [
public		 
int		 
DoctorId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
}

 
} æ
bC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\ChangePasswordDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
ChangePasswordDto "
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! @
)@ A
]A B
public 
string 
CurrentPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! <
)

< =
]

= >
public 
string 
NewPassword !
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
;> ?
[ 	
Required	 
( 
ErrorMessage 
=  
$str! D
)D E
]E F
public 
string 
ConfirmNewPassword (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
=7 8
string9 ?
.? @
Empty@ E
;E F
} 
} ˙
eC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\CancelAppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class  
CancelAppointmentDto %
{ 
[ 	
Required	 
] 
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
(

 
ErrorMessage

 
=

  
$str

! Z
)

Z [
]

[ \
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* _
)_ `
]` a
public 
required 
string 
Reason %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} õ
cC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\BookAppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
BookAppointmentDto #
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! R
)R S
]S T
[		 	
Range			 
(		 
$num		 
,		 
int		 
.		 
MaxValue		 
,		 
ErrorMessage		  ,
=		- .
$str		/ Z
)		Z [
]		[ \
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
[ 	
Required	 
( 
ErrorMessage 
=  
$str! Q
)Q R
]R S
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ Y
)Y Z
]Z [
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
$str! E
)E F
]F G
public 
DateTime 
ScheduledDate %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
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
} 
} ò
_C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\AppointmentDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
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
public 
string 
? 
PatientName "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
int 
DoctorId 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
? 

DoctorName !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
ScheduledDate #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
string4 :
.: ;
Empty; @
;@ A
public 
string 
TimeSlot 
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
;; <
public 
AppointmentStatus  
Status! '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
? 
CancellationReason )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
} 
} ·
cC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Models\Dtos\AddHealthRecordDto.cs
	namespace 	
HealthCareApp
 
. 
Dtos 
{ 
public 

class 
AddHealthRecordDto #
{ 
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
}) *
public 
int 
? 
DoctorId 
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
=  
$str! ^
)^ _
]_ `
[ 	
Range	 
( 
$num 
, 
int 
. 
MaxValue 
, 
ErrorMessage  ,
=- .
$str/ ^
)^ _
]_ `
public 
int 
AppointmentId  
{! "
get# &
;& '
set( +
;+ ,
}- .
[ 	
Required	 
( 
ErrorMessage 
=  
$str! F
)F G
]G H
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* ]
)] ^
]^ _
public 
required 
string 
	Diagnosis (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
[ 	
Required	 
( 
ErrorMessage 
=  
$str! W
)W X
]X Y
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* `
)` a
]a b
public 
required 
string 
Prescription +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage (
=) *
$str+ ^
)^ _
]_ `
public 
required 
string 
Notes $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[   	
Required  	 
]   
public!! 
DateTime!! 
	VisitDate!! !
{!!" #
get!!$ '
;!!' (
set!!) ,
;!!, -
}!!. /
}## 
}$$ Ú
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
	namespace 	
HealthCareApp
 
. 

Middleware "
{ 
public 

class "
GlobalExceptionHandler '
:( )
IExceptionHandler* ;
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
$str 9
,9 :
	exception 
. 
Message !
)! "
;" #
var 
( 

statusCode 
, 
message $
)$ %
=& '
	exception( 1
switch2 8
{ #
EntityNotFoundException '
ex( *
=>+ -
( 
StatusCodes  
.  !
Status404NotFound! 2
,2 3
ex4 6
.6 7
Message7 >
)> ?
,? @
ConflictException   !
ex  " $
=>  % '
(!! 
StatusCodes!!  
.!!  !
Status409Conflict!!! 2
,!!2 3
ex!!4 6
.!!6 7
Message!!7 >
)!!> ?
,!!? @$
AppointmentRuleException## (
ex##) +
=>##, .
($$ 
StatusCodes$$  
.$$  !
Status400BadRequest$$! 4
,$$4 5
ex$$6 8
.$$8 9
Message$$9 @
)$$@ A
,$$A B%
HealthRecordRuleException&& )
ex&&* ,
=>&&- /
('' 
StatusCodes''  
.''  !
Status400BadRequest''! 4
,''4 5
ex''6 8
.''8 9
Message''9 @
)''@ A
,''A B$
ForbiddenAccessException(( (
ex(() +
=>((, .
()) 
StatusCodes))  
.))  !
Status403Forbidden))! 3
,))3 4
ex))5 7
.))7 8
Message))8 ?
)))? @
,))@ A!
BusinessRuleException** %
ex**& (
=>**) +
(++ 
StatusCodes++  
.++  !
Status400BadRequest++! 4
,++4 5
ex++6 8
.++8 9
Message++9 @
)++@ A
,++A B"
HealthcareAppException-- &
ex--' )
=>--* ,
(.. 
StatusCodes..  
...  !
Status400BadRequest..! 4
,..4 5
ex..6 8
...8 9
Message..9 @
)..@ A
,..A B
_00 
=>00 
(11 
StatusCodes11  
.11  !(
Status500InternalServerError11! =
,11= >
$str11? n
)11n o
}22 
;22 
httpContext44 
.44 
Response44  
.44  !

StatusCode44! +
=44, -

statusCode44. 8
;448 9
httpContext55 
.55 
Response55  
.55  !
ContentType55! ,
=55- .
$str55/ A
;55A B
var77 
response77 
=77 
new77 
ErrorResponse77 ,
{88 

StatusCode99 
=99 

statusCode99 '
,99' (
Message:: 
=:: 
message:: !
,::! "
	TimeStamp;; 
=;; 
DateTime;; $
.;;$ %
UtcNow;;% +
,;;+ ,
Path<< 
=<< 
httpContext<< "
.<<" #
Request<<# *
.<<* +
Path<<+ /
}== 
;== 
await?? 
httpContext?? 
.?? 
Response?? &
.??& '
WriteAsJsonAsync??' 7
(??7 8
response??8 @
,??@ A
cancellationToken??B S
)??S T
;??T U
returnAA 
trueAA 
;AA 
}BB 	
}CC 
}DD ËU
[C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Mapping\MappingProfile.cs
	namespace 	
HealthCareApp
 
. 
Mapping 
{ 
public		 

class		 
MappingProfile		 
:		  !
Profile		" )
{

 
private 
const 
string 

DateFormat '
=( )
$str* 6
;6 7
public 
MappingProfile 
( 
) 
{ 	
	CreateMap 
< 
Patient 
, 

PatientDto )
>) *
(* +
)+ ,
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
FullName! )
,) *
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
PatientName2 =
)= >
) 
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
InsuranceId! ,
,, -
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
InsuranceID2 =
)= >
) 
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
DateOfBirth! ,
,, -
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
DateOfBirth2 =
.= >
ToString> F
(F G

DateFormatG Q
)Q R
)R S
) 
. 
	ForMember 
( 
dest 
=> 
dest  
.  !
CreatedDate! ,
,, -
opt 
=> 
opt 
. 
MapFrom &
(& '
src' *
=>+ -
src. 1
.1 2
DateOfBirth2 =
.= >
ToString> F
(F G

DateFormatG Q
)Q R
)R S
)   
;   
	CreateMap"" 
<"" 
CreatePatientDto"" &
,""& '
Patient""( /
>""/ 0
(""0 1
)""1 2
.## 
	ForMember## 
(## 
dest$$ 
=>$$ 
dest$$  
.$$  !
PatientName$$! ,
,$$, -
opt%% 
=>%% 
opt%% 
.%% 
MapFrom%% &
(%%& '
src%%' *
=>%%+ -
src%%. 1
.%%1 2
FullName%%2 :
)%%: ;
)&& 
.'' 
	ForMember'' 
('' 
dest(( 
=>(( 
dest((  
.((  !
InsuranceID((! ,
,((, -
opt)) 
=>)) 
opt)) 
.)) 
MapFrom)) &
())& '
src))' *
=>))+ -
src)). 1
.))1 2
InsuranceId))2 =
)))= >
)** 
;** 
	CreateMap,, 
<,, 
UpdatePatientDto,, &
,,,& '
Patient,,( /
>,,/ 0
(,,0 1
),,1 2
.-- 
	ForMember-- 
(-- 
dest.. 
=>.. 
dest..  
...  !
PatientName..! ,
,.., -
opt// 
=>// 
opt// 
.// 
MapFrom// &
(//& '
src//' *
=>//+ -
src//. 1
.//1 2
FullName//2 :
)//: ;
)00 
.11 
	ForMember11 
(11 
dest22 
=>22 
dest22  
.22  !
InsuranceID22! ,
,22, -
opt33 
=>33 
opt33 
.33 
MapFrom33 &
(33& '
src33' *
=>33+ -
src33. 1
.331 2
InsuranceId332 =
)33= >
)44 
;44 
	CreateMap66 
<66 
PatientRegisterDto66 (
,66( )
Patient66* 1
>661 2
(662 3
)663 4
.77 
	ForMember77 
(77 
dest88 
=>88 
dest88  
.88  !
PatientName88! ,
,88, -
opt99 
=>99 
opt99 
.99 
MapFrom99 &
(99& '
src99' *
=>99+ -
src99. 1
.991 2
FullName992 :
)99: ;
):: 
.;; 
	ForMember;; 
(;; 
dest<< 
=><< 
dest<<  
.<<  !
InsuranceID<<! ,
,<<, -
opt== 
=>== 
opt== 
.== 
MapFrom== &
(==& '
src==' *
=>==+ -
src==. 1
.==1 2
InsuranceId==2 =
)=== >
)>> 
;>> 
	CreateMapAA 
<AA 
DoctorAA 
,AA 
	DoctorDtoAA '
>AA' (
(AA( )
)AA) *
.BB 
	ForMemberBB 
(BB 
destCC 
=>CC 
destCC  
.CC  !
FullNameCC! )
,CC) *
optDD 
=>DD 
optDD 
.DD 
MapFromDD &
(DD& '
srcDD' *
=>DD+ -
srcDD. 1
.DD1 2

DoctorNameDD2 <
)DD< =
)EE 
;EE 
	CreateMapGG 
<GG 
CreateDoctorDtoGG %
,GG% &
DoctorGG' -
>GG- .
(GG. /
)GG/ 0
.HH 
	ForMemberHH 
(HH 
destII 
=>II 
destII  
.II  !

DoctorNameII! +
,II+ ,
optJJ 
=>JJ 
optJJ 
.JJ 
MapFromJJ &
(JJ& '
srcJJ' *
=>JJ+ -
srcJJ. 1
.JJ1 2
FullNameJJ2 :
)JJ: ;
)KK 
;KK 
	CreateMapMM 
<MM 
UpdateDoctorDtoMM %
,MM% &
DoctorMM' -
>MM- .
(MM. /
)MM/ 0
.NN 
	ForMemberNN 
(NN 
destOO 
=>OO 
destOO  
.OO  !

DoctorNameOO! +
,OO+ ,
optPP 
=>PP 
optPP 
.PP 
MapFromPP &
(PP& '
srcPP' *
=>PP+ -
srcPP. 1
.PP1 2
FullNamePP2 :
)PP: ;
)QQ 
;QQ 
	CreateMapTT 
<TT 
AppointmentTT !
,TT! "
AppointmentDtoTT# 1
>TT1 2
(TT2 3
)TT3 4
.UU 
	ForMemberUU 
(UU 
destVV 
=>VV 
destVV  
.VV  !
PatientNameVV! ,
,VV, -
optWW 
=>WW 
optWW 
.WW 
MapFromWW &
(WW& '
srcWW' *
=>WW+ -
srcWW. 1
.WW1 2
PatientWW2 9
!=WW: <
nullWW= A
?WWB C
srcWWD G
.WWG H
PatientWWH O
.WWO P
PatientNameWWP [
:WW\ ]
nullWW^ b
)WWb c
)XX 
.YY 
	ForMemberYY 
(YY 
destZZ 
=>ZZ 
destZZ  
.ZZ  !

DoctorNameZZ! +
,ZZ+ ,
opt[[ 
=>[[ 
opt[[ 
.[[ 
MapFrom[[ &
([[& '
src[[' *
=>[[+ -
src[[. 1
.[[1 2
Doctor[[2 8
!=[[9 ;
null[[< @
?[[A B
src[[C F
.[[F G
Doctor[[G M
.[[M N

DoctorName[[N X
:[[Y Z
null[[[ _
)[[_ `
)\\ 
.]] 
	ForMember]] 
(]] 
dest^^ 
=>^^ 
dest^^  
.^^  !
ScheduledDate^^! .
,^^. /
opt__ 
=>__ 
opt__ 
.__ 
MapFrom__ &
(__& '
src__' *
=>__+ -
src__. 1
.__1 2
ScheduledDate__2 ?
.__? @
ToString__@ H
(__H I

DateFormat__I S
)__S T
)__T U
)`` 
;`` 
	CreateMapbb 
<bb 
BookAppointmentDtobb (
,bb( )
Appointmentbb* 5
>bb5 6
(bb6 7
)bb7 8
;bb8 9
	CreateMapdd 
<dd  
UpdateAppointmentDtodd *
,dd* +
Appointmentdd, 7
>dd7 8
(dd8 9
)dd9 :
;dd: ;
	CreateMapgg 
<gg 
HealthRecordgg "
,gg" #
HealthRecordDtogg$ 3
>gg3 4
(gg4 5
)gg5 6
.hh 
	ForMemberhh 
(hh 
destii 
=>ii 
destii  
.ii  !
PatientNameii! ,
,ii, -
optjj 
=>jj 
optjj 
.jj 
MapFromjj &
(jj& '
srcjj' *
=>jj+ -
srcjj. 1
.jj1 2
Patientjj2 9
!=jj: <
nulljj= A
?jjB C
srcjjD G
.jjG H
PatientjjH O
.jjO P
PatientNamejjP [
:jj\ ]
nulljj^ b
)jjb c
)kk 
.ll 
	ForMemberll 
(ll 
destmm 
=>mm 
destmm  
.mm  !

DoctorNamemm! +
,mm+ ,
optnn 
=>nn 
optnn 
.nn 
MapFromnn &
(nn& '
srcnn' *
=>nn+ -
srcnn. 1
.nn1 2
Doctornn2 8
!=nn9 ;
nullnn< @
?nnA B
srcnnC F
.nnF G
DoctornnG M
.nnM N

DoctorNamennN X
:nnY Z
nullnn[ _
)nn_ `
)oo 
.pp 
	ForMemberpp 
(pp 
destqq 
=>qq 
destqq  
.qq  !
	VisitDateqq! *
,qq* +
optrr 
=>rr 
optrr 
.rr 
MapFromrr &
(rr& '
srcrr' *
=>rr+ -
srcrr. 1
.rr1 2
	VisitDaterr2 ;
.rr; <
ToStringrr< D
(rrD E

DateFormatrrE O
)rrO P
)rrP Q
)ss 
;ss 
	CreateMapuu 
<uu 
AddHealthRecordDtouu (
,uu( )
HealthRecorduu* 6
>uu6 7
(uu7 8
)uu8 9
;uu9 :
	CreateMapww 
<ww !
UpdateHealthRecordDtoww +
,ww+ ,
HealthRecordww- 9
>ww9 :
(ww: ;
)ww; <
;ww< =
}xx 	
}yy 
}zz Ó
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
} „
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
 ˜
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
} Î
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
 ı
SC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Enums\UserRole.cs
	namespace 	
HealthCareApp
 
. 
Enums 
{ 
public 

enum 
UserRole 
{ 
Admin 
, 
Patient 
, 
Doctor 
} 
}		 «
]C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Enums\SpecialisationType.cs
	namespace 	
HealthCareApp
 
. 
Enums 
{ 
public 

enum 
SpecialisationType "
{ 
Endocrinologist 
, 

Oncologist 
, 
Gynecologist 
, 
OrthopedicSurgeon 
, 
Psychiatrist		 
,		 
Pediatrician

 
,

 
Neurologist 
, 
Dermatologist 
, 
Cardiologist 
, 
GeneralPractitioner 
} 
} û
UC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Enums\GenderType.cs
	namespace 	
HealthCareApp
 
. 
Enums 
{ 
public 

enum 

GenderType 
{ 
Male 
, 
Female 
, 
Transgender		 
,		 
Other

 
} 
} ¥
\C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Enums\AppointmentStatus.cs
	namespace 	
HealthCareApp
 
. 
Enums 
{ 
public 

enum 
AppointmentStatus !
{ 
Pending 
, 
	Confirmed 
, 
	Cancelled 
, 
	Completed 
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
}99 ÅS
cC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\PatientsController.cs
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
PatientsController #
:$ %
ControllerBase& 4
{ 
private 
readonly 
IPatientService (
_patientService) 8
;8 9
private 
readonly  
IHealthRecordService - 
_healthRecordService. B
;B C
public 
PatientsController !
(! "
IPatientService 
patientService *
,* + 
IHealthRecordService  
healthRecordService! 4
)4 5
{ 	
_patientService 
= 
patientService ,
;, - 
_healthRecordService  
=! "
healthRecordService# 6
;6 7
} 	
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes 
= 
JwtBearerDefaults -
.- . 
AuthenticationScheme. B
,B C
Roles 	
=
 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAllPatients) 7
(7 8
[8 9
	FromQuery9 B
]B C%
PatientPaginationQueryDtoD ]
query^ c
)c d
{ 	
var   
patients   
=   
await    
_patientService  ! 0
.  0 1$
GetAllPatientsPagedAsync  1 I
(  I J
query  J O
)  O P
;  P Q
return"" 
Ok"" 
("" 
patients"" 
)"" 
;""  
}## 	
['' 	
HttpGet''	 
('' 
$str'' 
)'' 
]'' 
[(( 	
	Authorize((	 
((( !
AuthenticationSchemes)) !
=))" #
JwtBearerDefaults))$ 5
.))5 6 
AuthenticationScheme))6 J
,))J K
Roles** 
=** 
$str** 
)** 
]** 
public++ 
async++ 
Task++ 
<++ 
IActionResult++ '
>++' (
GetMyProfile++) 5
(++5 6
)++6 7
{,, 	
var-- 
identityUserId-- 
=--  
User--! %
.--% &
	FindFirst--& /
(--/ 0

ClaimTypes--0 :
.--: ;
NameIdentifier--; I
)--I J
?--J K
.--K L
Value--L Q
;--Q R
if// 
(// 
string// 
.// 
IsNullOrWhiteSpace// )
(//) *
identityUserId//* 8
)//8 9
)//9 :
{00 
return11 
Unauthorized11 #
(11# $
new11$ '
{22 
Message33 
=33 
$str33 3
}44 
)44 
;44 
}55 
var77 
patient77 
=77 
await77 
_patientService77  /
.77/ 0
GetMyProfileAsync770 A
(77A B
identityUserId77B P
)77P Q
;77Q R
return99 
Ok99 
(99 
patient99 
)99 
;99 
}:: 	
[>> 	
HttpPut>>	 
(>> 
$str>> 
)>> 
]>> 
[?? 	
	Authorize??	 
(?? !
AuthenticationSchemes@@ !
=@@" #
JwtBearerDefaults@@$ 5
.@@5 6 
AuthenticationScheme@@6 J
,@@J K
RolesAA 
=AA 
$strAA 
)AA 
]AA 
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
UpdateMyProfileBB) 8
(BB8 9
[BB9 :
FromBodyBB: B
]BBB C
UpdatePatientDtoBBD T
requestBBU \
)BB\ ]
{CC 	
varDD 
identityUserIdDD 
=DD  
UserDD! %
.DD% &
	FindFirstDD& /
(DD/ 0

ClaimTypesDD0 :
.DD: ;
NameIdentifierDD; I
)DDI J
?DDJ K
.DDK L
ValueDDL Q
;DDQ R
ifFF 
(FF 
stringFF 
.FF 
IsNullOrWhiteSpaceFF )
(FF) *
identityUserIdFF* 8
)FF8 9
)FF9 :
{GG 
returnHH 
UnauthorizedHH #
(HH# $
newHH$ '
{II 
MessageJJ 
=JJ 
$strJJ 3
}KK 
)KK 
;KK 
}LL 
varNN 
patientNN 
=NN 
awaitNN 
_patientServiceNN  /
.NN/ 0 
UpdateMyProfileAsyncNN0 D
(NND E
identityUserIdNNE S
,NNS T
requestNNU \
)NN\ ]
;NN] ^
returnPP 
OkPP 
(PP 
patientPP 
)PP 
;PP 
}QQ 	
[UU 	
HttpGetUU	 
(UU 
$strUU $
)UU$ %
]UU% &
[VV 	
	AuthorizeVV	 
(VV !
AuthenticationSchemesWW !
=WW" #
JwtBearerDefaultsWW$ 5
.WW5 6 
AuthenticationSchemeWW6 J
,WWJ K
RolesXX 
=XX 
$strXX 
)XX 
]XX 
publicYY 
asyncYY 
TaskYY 
<YY 
IActionResultYY '
>YY' (
GetMyHealthRecordsYY) ;
(YY; <
)YY< =
{ZZ 	
var[[ 
identityUserId[[ 
=[[  
User[[! %
.[[% &
	FindFirst[[& /
([[/ 0

ClaimTypes[[0 :
.[[: ;
NameIdentifier[[; I
)[[I J
?[[J K
.[[K L
Value[[L Q
;[[Q R
if]] 
(]] 
string]] 
.]] 
IsNullOrWhiteSpace]] )
(]]) *
identityUserId]]* 8
)]]8 9
)]]9 :
{^^ 
return__ 
Unauthorized__ #
(__# $
new__$ '
{`` 
Messageaa 
=aa 
$straa 3
}bb 
)bb 
;bb 
}cc 
varee 
patientee 
=ee 
awaitee 
_patientServiceee  /
.ee/ 0
GetMyProfileAsyncee0 A
(eeA B
identityUserIdeeB P
)eeP Q
;eeQ R
vargg 
recordsgg 
=gg 
awaitgg  
_healthRecordServicegg  4
.gg4 5,
 GetHealthRecordsByPatientIdAsyncgg5 U
(ggU V
patientggV ]
.gg] ^
	PatientIdgg^ g
)ggg h
;ggh i
returnii 
Okii 
(ii 
recordsii 
)ii 
;ii 
}jj 	
[nn 	
HttpGetnn	 
(nn 
$strnn "
)nn" #
]nn# $
[oo 	
	Authorizeoo	 
(oo !
AuthenticationSchemespp !
=pp" #
JwtBearerDefaultspp$ 5
.pp5 6 
AuthenticationSchemepp6 J
,ppJ K
Rolesqq 
=qq 
$strqq 
)qq 
]qq 
publicrr 
asyncrr 
Taskrr 
<rr 
IActionResultrr '
>rr' (
GetPatientByIdrr) 7
(rr7 8
[rr8 9
	FromRouterr9 B
]rrB C
intrrD G
	patientIdrrH Q
)rrQ R
{ss 	
vartt 
patienttt 
=tt 
awaittt 
_patientServicett  /
.tt/ 0
GetPatientByIdAsynctt0 C
(ttC D
	patientIdttD M
)ttM N
;ttN O
returnvv 
Okvv 
(vv 
patientvv 
)vv 
;vv 
}ww 	
[{{ 	
HttpPut{{	 
({{ 
$str{{ "
){{" #
]{{# $
[|| 	
	Authorize||	 
(|| !
AuthenticationSchemes}} !
=}}" #
JwtBearerDefaults}}$ 5
.}}5 6 
AuthenticationScheme}}6 J
,}}J K
Roles~~ 
=~~ 
$str~~ 
)~~ 
]~~ 
public 
async 
Task 
< 
IActionResult '
>' (
UpdatePatient) 6
(6 7
[
ÄÄ 
	FromRoute
ÄÄ 
]
ÄÄ 
int
ÄÄ 
	patientId
ÄÄ %
,
ÄÄ% &
[
ÅÅ 
FromBody
ÅÅ 
]
ÅÅ 
UpdatePatientDto
ÅÅ '
request
ÅÅ( /
)
ÅÅ/ 0
{
ÇÇ 	
var
ÉÉ 
patient
ÉÉ 
=
ÉÉ 
await
ÉÉ 
_patientService
ÉÉ  /
.
ÉÉ/ 0 
UpdatePatientAsync
ÉÉ0 B
(
ÉÉB C
	patientId
ÉÉC L
,
ÉÉL M
request
ÉÉN U
)
ÉÉU V
;
ÉÉV W
return
ÖÖ 
Ok
ÖÖ 
(
ÖÖ 
patient
ÖÖ 
)
ÖÖ 
;
ÖÖ 
}
ÜÜ 	
[
ää 	
HttpGet
ää	 
(
ää 
$str
ää 1
)
ää1 2
]
ää2 3
[
ãã 	
	Authorize
ãã	 
(
ãã #
AuthenticationSchemes
åå !
=
åå" #
JwtBearerDefaults
åå$ 5
.
åå5 6"
AuthenticationScheme
åå6 J
,
ååJ K
Roles
çç 
=
çç 
$str
çç "
)
çç" #
]
çç# $
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
éé' (%
GetPatientHealthRecords
éé) @
(
éé@ A
[
ééA B
	FromRoute
ééB K
]
ééK L
int
ééM P
	patientId
ééQ Z
)
ééZ [
{
èè 	
var
êê 
records
êê 
=
êê 
await
êê "
_healthRecordService
êê  4
.
êê4 5.
 GetHealthRecordsByPatientIdAsync
êê5 U
(
êêU V
	patientId
êêV _
)
êê_ `
;
êê` a
return
íí 
Ok
íí 
(
íí 
records
íí 
)
íí 
;
íí 
}
ìì 	
}
îî 
}ïï ìÖ
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
public 

class 
AuthController 
(  
IAuthService  ,
service- 4
)4 5
:6 7
ControllerBase8 F
{ 
[ 	
HttpPost	 
( 
$str $
)$ %
]% &
[ 	
AllowAnonymous	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
RegisterPatient) 8
(8 9
PatientRegisterDto9 K
requestL S
)S T
{ 	
var 
( 
success 
, 
message !
,! "
	patientId# ,
), -
=. /
await0 5
service6 =
.= > 
RegisterPatientAsync> R
(R S
requestS Z
)Z [
;[ \
if 
( 
! 
success 
) 
{ 
return 

BadRequest !
(! "
new" %
{ 
Message 
= 
message %
} 
) 
; 
} 
return 
Ok 
( 
new 
{ 
Message 
= 
message !
,! "
	PatientId 
= 
	patientId %
}   
)   
;   
}"" 	
[$$ 	
HttpPost$$	 
($$ 
$str$$ 
)$$ 
]$$ 
[%% 	
AllowAnonymous%%	 
]%% 
public&& 
async&& 
Task&& 
<&& 
IActionResult&& '
>&&' (
Login&&) .
(&&. /
LoginDto&&/ 7
request&&8 ?
)&&? @
{'' 	
var(( 
((( 
success(( 
,(( 
message(( !
,((! "
token((# (
,((( )
	expiresIn((* 3
)((3 4
=((5 6
await((7 <
service((= D
.((D E
Login((E J
(((J K
request((K R
)((R S
;((S T
if** 
(** 
!** 
success** 
)** 
{++ 
return,, 
Unauthorized,, #
(,,# $
new,,$ '
{-- 
Message.. 
=.. 
message.. %
}// 
)// 
;// 
}00 
AuthResponse22 
response22 !
=22" #
new22$ '
AuthResponse22( 4
{33 
AccessToken44 
=44 
token44 #
,44# $
Message55 
=55 
message55 !
,55! "
	ExpiresIn66 
=66 
	expiresIn66 %
}77 
;77 
return99 
Ok99 
(99 
response99 
)99 
;99  
}:: 	
[<< 	
HttpPost<<	 
(<< 
$str<< #
)<<# $
]<<$ %
[== 	
	Authorize==	 
(== !
AuthenticationSchemes== (
===) *
JwtBearerDefaults==+ <
.==< = 
AuthenticationScheme=== Q
)==Q R
]==R S
public>> 
async>> 
Task>> 
<>> 
IActionResult>> '
>>>' (
ChangePassword>>) 7
(>>7 8
ChangePasswordDto>>8 I
request>>J Q
)>>Q R
{?? 	
var@@ 
userId@@ 
=@@ 
User@@ 
.@@ 
	FindFirst@@ '
(@@' (
System@@( .
.@@. /
Security@@/ 7
.@@7 8
Claims@@8 >
.@@> ?

ClaimTypes@@? I
.@@I J
NameIdentifier@@J X
)@@X Y
?@@Y Z
.@@Z [
Value@@[ `
;@@` a
ifBB 
(BB 
stringBB 
.BB 
IsNullOrWhiteSpaceBB )
(BB) *
userIdBB* 0
)BB0 1
)BB1 2
{CC 
returnDD 
UnauthorizedDD #
(DD# $
newDD$ '
{EE 
MessageFF 
=FF 
$strFF 3
}GG 
)GG 
;GG 
}HH 
varJJ 
(JJ 
successJJ 
,JJ 
messageJJ !
)JJ! "
=JJ# $
awaitJJ% *
serviceJJ+ 2
.JJ2 3
ChangePasswordAsyncJJ3 F
(JJF G
userIdJJG M
,JJM N
requestJJO V
)JJV W
;JJW X
ifLL 
(LL 
!LL 
successLL 
)LL 
{MM 
returnNN 

BadRequestNN !
(NN! "
newNN" %
{OO 
MessagePP 
=PP 
messagePP %
}QQ 
)QQ 
;QQ 
}RR 
returnTT 
OkTT 
(TT 
newTT 
{UU 
MessageVV 
=VV 
messageVV !
}WW 
)WW 
;WW 
}XX 	
}YY 
}ZZ ç©
fC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\AppointmentController.cs
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
public 

class "
AppointmentsController '
(' (
IAppointmentService( ;
service< C
)C D
:E F
ControllerBaseG U
{ 
private 
const 
string #
InvalidUserTokenMessage 4
=5 6
$str7 L
;L M
private 
const 
string 
PatientRoleName ,
=- .
$str/ 8
;8 9
private 
const 
string 
DoctorRoleName +
=, -
$str. 6
;6 7
[ 	
HttpGet	 
] 
[ 	
	Authorize	 
( !
AuthenticationSchemes 
= 
JwtBearerDefaults .
.. / 
AuthenticationScheme/ C
,C D
Roles 

= 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAllAppointments) ;
(; <
[< =
	FromQuery= F
]F G)
AppointmentPaginationQueryDtoH e
queryf k
)k l
{ 	
var 
appointments 
= 
await $
service% ,
., -(
GetAllAppointmentsPagedAsync- I
(I J
queryJ O
)O P
;P Q
return 
Ok 
( 
appointments "
)" #
;# $
} 	
[!! 	
HttpGet!!	 
(!! 
$str!! 
)!! 
]!! 
["" 	
	Authorize""	 
("" !
AuthenticationSchemes## !
=##" #
JwtBearerDefaults##$ 5
.##5 6 
AuthenticationScheme##6 J
,##J K
Roles$$ 
=$$ 
$str$$ $
)$$$ %
]$$% &
public%% 
async%% 
Task%% 
<%% 
IActionResult%% '
>%%' (
GetMyAppointments%%) :
(%%: ;
)%%; <
{&& 	
var'' 
identityUserId'' 
=''  
User''! %
.''% &
	FindFirst''& /
(''/ 0

ClaimTypes''0 :
.'': ;
NameIdentifier''; I
)''I J
?''J K
.''K L
Value''L Q
;''Q R
if)) 
()) 
string)) 
.)) 
IsNullOrWhiteSpace)) )
())) *
identityUserId))* 8
)))8 9
)))9 :
{** 
return++ 
Unauthorized++ #
(++# $
new++$ '
{,, 
Message-- 
=-- #
InvalidUserTokenMessage-- 5
}.. 
).. 
;.. 
}// 
if11 
(11 
User11 
.11 
IsInRole11 
(11 
PatientRoleName11 -
)11- .
)11. /
{22 
var33 
appointments33  
=33! "
await33# (
service33) 0
.330 1,
 GetMyAppointmentsForPatientAsync331 Q
(33Q R
identityUserId33R `
)33` a
;33a b
return55 
Ok55 
(55 
appointments55 &
)55& '
;55' (
}66 
if88 
(88 
User88 
.88 
IsInRole88 
(88 
DoctorRoleName88 ,
)88, -
)88- .
{99 
var:: 
appointments::  
=::! "
await::# (
service::) 0
.::0 1+
GetMyAppointmentsForDoctorAsync::1 P
(::P Q
identityUserId::Q _
)::_ `
;::` a
return<< 
Ok<< 
(<< 
appointments<< &
)<<& '
;<<' (
}== 
return?? 
Forbid?? 
(?? 
)?? 
;?? 
}@@ 	
[CC 	
HttpGetCC	 
(CC 
$strCC 
)CC 
]CC  
[DD 	
	AuthorizeDD	 
(DD !
AuthenticationSchemesEE !
=EE" #
JwtBearerDefaultsEE$ 5
.EE5 6 
AuthenticationSchemeEE6 J
,EEJ K
RolesFF 
=FF 
$strFF $
)FF$ %
]FF% &
publicGG 
asyncGG 
TaskGG 
<GG 
IActionResultGG '
>GG' (%
GetMyUpcomingAppointmentsGG) B
(GGB C
)GGC D
{HH 	
varII 
identityUserIdII 
=II  
UserII! %
.II% &
	FindFirstII& /
(II/ 0

ClaimTypesII0 :
.II: ;
NameIdentifierII; I
)III J
?IIJ K
.IIK L
ValueIIL Q
;IIQ R
ifKK 
(KK 
stringKK 
.KK 
IsNullOrWhiteSpaceKK )
(KK) *
identityUserIdKK* 8
)KK8 9
)KK9 :
{LL 
returnMM 
UnauthorizedMM #
(MM# $
newMM$ '
{NN 
MessageOO 
=OO #
InvalidUserTokenMessageOO 5
}PP 
)PP 
;PP 
}QQ 
ifSS 
(SS 
UserSS 
.SS 
IsInRoleSS 
(SS 
PatientRoleNameSS -
)SS- .
)SS. /
{TT 
varUU 
appointmentsUU  
=UU! "
awaitUU# (
serviceUU) 0
.UU0 14
(GetMyUpcomingAppointmentsForPatientAsyncUU1 Y
(UUY Z
identityUserIdUUZ h
)UUh i
;UUi j
returnWW 
OkWW 
(WW 
appointmentsWW &
)WW& '
;WW' (
}XX 
ifZZ 
(ZZ 
UserZZ 
.ZZ 
IsInRoleZZ 
(ZZ 
DoctorRoleNameZZ ,
)ZZ, -
)ZZ- .
{[[ 
var\\ 
appointments\\  
=\\! "
await\\# (
service\\) 0
.\\0 13
'GetMyUpcomingAppointmentsForDoctorAsync\\1 X
(\\X Y
identityUserId\\Y g
)\\g h
;\\h i
return^^ 
Ok^^ 
(^^ 
appointments^^ &
)^^& '
;^^' (
}__ 
returnaa 
Forbidaa 
(aa 
)aa 
;aa 
}bb 	
[ee 	
HttpGetee	 
(ee 
$stree 
)ee 
]ee 
[ff 	
	Authorizeff	 
(ff !
AuthenticationSchemesgg !
=gg" #
JwtBearerDefaultsgg$ 5
.gg5 6 
AuthenticationSchemegg6 J
,ggJ K
Roleshh 
=hh 
$strhh $
)hh$ %
]hh% &
publicii 
asyncii 
Taskii 
<ii 
IActionResultii '
>ii' ($
GetMyPendingAppointmentsii) A
(iiA B
)iiB C
{jj 	
varkk 
identityUserIdkk 
=kk  
Userkk! %
.kk% &
	FindFirstkk& /
(kk/ 0

ClaimTypeskk0 :
.kk: ;
NameIdentifierkk; I
)kkI J
?kkJ K
.kkK L
ValuekkL Q
;kkQ R
ifmm 
(mm 
stringmm 
.mm 
IsNullOrWhiteSpacemm )
(mm) *
identityUserIdmm* 8
)mm8 9
)mm9 :
{nn 
returnoo 
Unauthorizedoo #
(oo# $
newoo$ '
{pp 
Messageqq 
=qq #
InvalidUserTokenMessageqq 5
}rr 
)rr 
;rr 
}ss 
ifuu 
(uu 
Useruu 
.uu 
IsInRoleuu 
(uu 
PatientRoleNameuu -
)uu- .
)uu. /
{vv 
varww 
appointmentsww  
=ww! "
awaitww# (
serviceww) 0
.ww0 13
'GetMyPendingAppointmentsForPatientAsyncww1 X
(wwX Y
identityUserIdwwY g
)wwg h
;wwh i
returnyy 
Okyy 
(yy 
appointmentsyy &
)yy& '
;yy' (
}zz 
if|| 
(|| 
User|| 
.|| 
IsInRole|| 
(|| 
DoctorRoleName|| ,
)||, -
)||- .
{}} 
var~~ 
appointments~~  
=~~! "
await~~# (
service~~) 0
.~~0 12
&GetMyPendingAppointmentsForDoctorAsync~~1 W
(~~W X
identityUserId~~X f
)~~f g
;~~g h
return
ÄÄ 
Ok
ÄÄ 
(
ÄÄ 
appointments
ÄÄ &
)
ÄÄ& '
;
ÄÄ' (
}
ÅÅ 
return
ÉÉ 
Forbid
ÉÉ 
(
ÉÉ 
)
ÉÉ 
;
ÉÉ 
}
ÑÑ 	
[
áá 	
HttpGet
áá	 
(
áá 
$str
áá %
)
áá% &
]
áá& '
[
àà 	
	Authorize
àà	 
(
àà #
AuthenticationSchemes
ââ !
=
ââ" #
JwtBearerDefaults
ââ$ 5
.
ââ5 6"
AuthenticationScheme
ââ6 J
,
ââJ K
Roles
ää 
=
ää 
$str
ää 
)
ää 
]
ää 
public
ãã 
async
ãã 
Task
ãã 
<
ãã 
IActionResult
ãã '
>
ãã' (-
GetMyTodayConfirmedAppointments
ãã) H
(
ããH I
)
ããI J
{
åå 	
var
çç 
identityUserId
çç 
=
çç  
User
çç! %
.
çç% &
	FindFirst
çç& /
(
çç/ 0

ClaimTypes
çç0 :
.
çç: ;
NameIdentifier
çç; I
)
ççI J
?
ççJ K
.
ççK L
Value
ççL Q
;
ççQ R
if
èè 
(
èè 
string
èè 
.
èè  
IsNullOrWhiteSpace
èè )
(
èè) *
identityUserId
èè* 8
)
èè8 9
)
èè9 :
{
êê 
return
ëë 
Unauthorized
ëë #
(
ëë# $
new
ëë$ '
{
íí 
Message
ìì 
=
ìì %
InvalidUserTokenMessage
ìì 5
}
îî 
)
îî 
;
îî 
}
ïï 
var
óó 
appointments
óó 
=
óó 
await
óó $
service
óó% ,
.
óó, -;
-GetMyTodayConfirmedAppointmentsForDoctorAsync
óó- Z
(
óóZ [
identityUserId
óó[ i
)
óói j
;
óój k
return
ôô 
Ok
ôô 
(
ôô 
appointments
ôô "
)
ôô" #
;
ôô# $
}
öö 	
[
ùù 	
HttpGet
ùù	 
(
ùù 
$str
ùù &
)
ùù& '
]
ùù' (
[
ûû 	
	Authorize
ûû	 
(
ûû #
AuthenticationSchemes
üü !
=
üü" #
JwtBearerDefaults
üü$ 5
.
üü5 6"
AuthenticationScheme
üü6 J
,
üüJ K
Roles
†† 
=
†† 
$str
†† *
)
††* +
]
††+ ,
public
°° 
async
°° 
Task
°° 
<
°° 
IActionResult
°° '
>
°°' ( 
GetAppointmentById
°°) ;
(
°°; <
[
°°< =
	FromRoute
°°= F
]
°°F G
int
°°H K
appointmentId
°°L Y
)
°°Y Z
{
¢¢ 	
if
££ 
(
££ 
User
££ 
.
££ 
IsInRole
££ 
(
££ 
$str
££ %
)
££% &
)
££& '
{
§§ 
var
•• 
appointment
•• 
=
••  !
await
••" '
service
••( /
.
••/ 0%
GetAppointmentByIdAsync
••0 G
(
••G H
appointmentId
••H U
)
••U V
;
••V W
return
ßß 
Ok
ßß 
(
ßß 
appointment
ßß %
)
ßß% &
;
ßß& '
}
®® 
if
™™ 
(
™™ 
User
™™ 
.
™™ 
IsInRole
™™ 
(
™™ 
PatientRoleName
™™ -
)
™™- .
)
™™. /
{
´´ 
var
¨¨ 
identityUserId
¨¨ "
=
¨¨# $
User
¨¨% )
.
¨¨) *
	FindFirst
¨¨* 3
(
¨¨3 4

ClaimTypes
¨¨4 >
.
¨¨> ?
NameIdentifier
¨¨? M
)
¨¨M N
?
¨¨N O
.
¨¨O P
Value
¨¨P U
;
¨¨U V
if
ÆÆ 
(
ÆÆ 
string
ÆÆ 
.
ÆÆ  
IsNullOrWhiteSpace
ÆÆ -
(
ÆÆ- .
identityUserId
ÆÆ. <
)
ÆÆ< =
)
ÆÆ= >
{
ØØ 
return
∞∞ 
Unauthorized
∞∞ '
(
∞∞' (
new
∞∞( +
{
±± 
Message
≤≤ 
=
≤≤  !%
InvalidUserTokenMessage
≤≤" 9
}
≥≥ 
)
≥≥ 
;
≥≥ 
}
¥¥ 
var
∂∂ 
appointment
∂∂ 
=
∂∂  !
await
∂∂" '
service
∂∂( /
.
∂∂/ 0/
!GetAppointmentByIdForPatientAsync
∂∂0 Q
(
∂∂Q R
appointmentId
∑∑ !
,
∑∑! "
identityUserId
∏∏ "
)
∏∏" #
;
∏∏# $
return
∫∫ 
Ok
∫∫ 
(
∫∫ 
appointment
∫∫ %
)
∫∫% &
;
∫∫& '
}
ªª 
if
ΩΩ 
(
ΩΩ 
User
ΩΩ 
.
ΩΩ 
IsInRole
ΩΩ 
(
ΩΩ 
DoctorRoleName
ΩΩ ,
)
ΩΩ, -
)
ΩΩ- .
{
ææ 
var
øø 
identityUserId
øø "
=
øø# $
User
øø% )
.
øø) *
	FindFirst
øø* 3
(
øø3 4

ClaimTypes
øø4 >
.
øø> ?
NameIdentifier
øø? M
)
øøM N
?
øøN O
.
øøO P
Value
øøP U
;
øøU V
if
¡¡ 
(
¡¡ 
string
¡¡ 
.
¡¡  
IsNullOrWhiteSpace
¡¡ -
(
¡¡- .
identityUserId
¡¡. <
)
¡¡< =
)
¡¡= >
{
¬¬ 
return
√√ 
Unauthorized
√√ '
(
√√' (
new
√√( +
{
ƒƒ 
Message
≈≈ 
=
≈≈  !%
InvalidUserTokenMessage
≈≈" 9
}
∆∆ 
)
∆∆ 
;
∆∆ 
}
«« 
var
…… 
appointment
…… 
=
……  !
await
……" '
service
……( /
.
……/ 0.
 GetAppointmentByIdForDoctorAsync
……0 P
(
……P Q
appointmentId
   !
,
  ! "
identityUserId
ÀÀ "
)
ÀÀ" #
;
ÀÀ# $
return
ÕÕ 
Ok
ÕÕ 
(
ÕÕ 
appointment
ÕÕ %
)
ÕÕ% &
;
ÕÕ& '
}
ŒŒ 
return
–– 
Forbid
–– 
(
–– 
)
–– 
;
–– 
}
—— 	
[
’’ 	
HttpGet
’’	 
(
’’ 
$str
’’ *
)
’’* +
]
’’+ ,
[
÷÷ 	
	Authorize
÷÷	 
(
÷÷ #
AuthenticationSchemes
◊◊ !
=
◊◊" #
JwtBearerDefaults
◊◊$ 5
.
◊◊5 6"
AuthenticationScheme
◊◊6 J
,
◊◊J K
Roles
ÿÿ 
=
ÿÿ 
$str
ÿÿ 
)
ÿÿ 
]
ÿÿ 
public
ŸŸ 
async
ŸŸ 
Task
ŸŸ 
<
ŸŸ 
IActionResult
ŸŸ '
>
ŸŸ' ((
GetAppointmentsByPatientId
ŸŸ) C
(
ŸŸC D
[
ŸŸD E
	FromRoute
ŸŸE N
]
ŸŸN O
int
ŸŸP S
	patientId
ŸŸT ]
)
ŸŸ] ^
{
⁄⁄ 	
var
€€ 
appointments
€€ 
=
€€ 
await
€€ $
service
€€% ,
.
€€, --
GetAppointmentsByPatientIdAsync
€€- L
(
€€L M
	patientId
€€M V
)
€€V W
;
€€W X
return
›› 
Ok
›› 
(
›› 
appointments
›› "
)
››" #
;
››# $
}
ﬁﬁ 	
[
‚‚ 	
HttpGet
‚‚	 
(
‚‚ 
$str
‚‚ (
)
‚‚( )
]
‚‚) *
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
ÊÊ' ('
GetAppointmentsByDoctorId
ÊÊ) B
(
ÊÊB C
[
ÊÊC D
	FromRoute
ÊÊD M
]
ÊÊM N
int
ÊÊO R
doctorId
ÊÊS [
)
ÊÊ[ \
{
ÁÁ 	
var
ËË 
appointments
ËË 
=
ËË 
await
ËË $
service
ËË% ,
.
ËË, -,
GetAppointmentsByDoctorIdAsync
ËË- K
(
ËËK L
doctorId
ËËL T
)
ËËT U
;
ËËU V
return
ÍÍ 
Ok
ÍÍ 
(
ÍÍ 
appointments
ÍÍ "
)
ÍÍ" #
;
ÍÍ# $
}
ÎÎ 	
[
ÓÓ 	
HttpGet
ÓÓ	 
(
ÓÓ 
$str
ÓÓ "
)
ÓÓ" #
]
ÓÓ# $
[
ÔÔ 	
	Authorize
ÔÔ	 
(
ÔÔ #
AuthenticationSchemes
 !
=
" #
JwtBearerDefaults
$ 5
.
5 6"
AuthenticationScheme
6 J
,
J K
Roles
ÒÒ 
=
ÒÒ 
$str
ÒÒ 
)
ÒÒ 
]
ÒÒ 
public
ÚÚ 
async
ÚÚ 
Task
ÚÚ 
<
ÚÚ 
IActionResult
ÚÚ '
>
ÚÚ' (%
GetAppointmentsByStatus
ÚÚ) @
(
ÚÚ@ A
[
ÚÚA B
	FromRoute
ÚÚB K
]
ÚÚK L
AppointmentStatus
ÚÚM ^
status
ÚÚ_ e
)
ÚÚe f
{
ÛÛ 	
var
ÙÙ 
appointments
ÙÙ 
=
ÙÙ 
await
ÙÙ $
service
ÙÙ% ,
.
ÙÙ, -*
GetAppointmentsByStatusAsync
ÙÙ- I
(
ÙÙI J
status
ÙÙJ P
)
ÙÙP Q
;
ÙÙQ R
return
ˆˆ 
Ok
ˆˆ 
(
ˆˆ 
appointments
ˆˆ "
)
ˆˆ" #
;
ˆˆ# $
}
˜˜ 	
[
˙˙ 	
HttpGet
˙˙	 
(
˙˙ 
$str
˙˙ 
)
˙˙ 
]
˙˙ 
[
˚˚ 	
	Authorize
˚˚	 
(
˚˚ #
AuthenticationSchemes
¸¸ !
=
¸¸" #
JwtBearerDefaults
¸¸$ 5
.
¸¸5 6"
AuthenticationScheme
¸¸6 J
,
¸¸J K
Roles
˝˝ 
=
˝˝ 
$str
˝˝ 
)
˝˝ 
]
˝˝ 
public
˛˛ 
async
˛˛ 
Task
˛˛ 
<
˛˛ 
IActionResult
˛˛ '
>
˛˛' (%
GetUpcomingAppointments
˛˛) @
(
˛˛@ A
)
˛˛A B
{
ˇˇ 	
var
ÄÄ 
appointments
ÄÄ 
=
ÄÄ 
await
ÄÄ $
service
ÄÄ% ,
.
ÄÄ, -*
GetUpcomingAppointmentsAsync
ÄÄ- I
(
ÄÄI J
)
ÄÄJ K
;
ÄÄK L
return
ÇÇ 
Ok
ÇÇ 
(
ÇÇ 
appointments
ÇÇ "
)
ÇÇ" #
;
ÇÇ# $
}
ÉÉ 	
[
áá 	
HttpGet
áá	 
(
áá 
$str
áá 3
)
áá3 4
]
áá4 5
[
àà 	
	Authorize
àà	 
(
àà #
AuthenticationSchemes
ââ !
=
ââ" #
JwtBearerDefaults
ââ$ 5
.
ââ5 6"
AuthenticationScheme
ââ6 J
,
ââJ K
Roles
ää 
=
ää 
$str
ää 
)
ää 
]
ää 
public
ãã 
async
ãã 
Task
ãã 
<
ãã 
IActionResult
ãã '
>
ãã' (0
"GetUpcomingAppointmentsByPatientId
ãã) K
(
ããK L
[
ããL M
	FromRoute
ããM V
]
ããV W
int
ããX [
	patientId
ãã\ e
)
ããe f
{
åå 	
var
çç 
appointments
çç 
=
çç 
await
çç $
service
çç% ,
.
çç, -5
'GetUpcomingAppointmentsByPatientIdAsync
çç- T
(
ççT U
	patientId
ççU ^
)
çç^ _
;
çç_ `
return
èè 
Ok
èè 
(
èè 
appointments
èè "
)
èè" #
;
èè# $
}
êê 	
[
îî 	
HttpGet
îî	 
(
îî 
$str
îî 1
)
îî1 2
]
îî2 3
[
ïï 	
	Authorize
ïï	 
(
ïï #
AuthenticationSchemes
ññ !
=
ññ" #
JwtBearerDefaults
ññ$ 5
.
ññ5 6"
AuthenticationScheme
ññ6 J
,
ññJ K
Roles
óó 
=
óó 
$str
óó 
)
óó 
]
óó 
public
òò 
async
òò 
Task
òò 
<
òò 
IActionResult
òò '
>
òò' (/
!GetUpcomingAppointmentsByDoctorId
òò) J
(
òòJ K
[
òòK L
	FromRoute
òòL U
]
òòU V
int
òòW Z
doctorId
òò[ c
)
òòc d
{
ôô 	
var
öö 
appointments
öö 
=
öö 
await
öö $
service
öö% ,
.
öö, -4
&GetUpcomingAppointmentsByDoctorIdAsync
öö- S
(
ööS T
doctorId
ööT \
)
öö\ ]
;
öö] ^
return
úú 
Ok
úú 
(
úú 
appointments
úú "
)
úú" #
;
úú# $
}
ùù 	
[
°° 	
HttpGet
°°	 
(
°° 
$str
°° 2
)
°°2 3
]
°°3 4
[
¢¢ 	
	Authorize
¢¢	 
(
¢¢ #
AuthenticationSchemes
££ !
=
££" #
JwtBearerDefaults
££$ 5
.
££5 6"
AuthenticationScheme
££6 J
,
££J K
Roles
§§ 
=
§§ 
$str
§§ 
)
§§ 
]
§§ 
public
•• 
async
•• 
Task
•• 
<
•• 
IActionResult
•• '
>
••' (/
!GetPendingAppointmentsByPatientId
••) J
(
••J K
[
••K L
	FromRoute
••L U
]
••U V
int
••W Z
	patientId
••[ d
)
••d e
{
¶¶ 	
var
ßß 
appointments
ßß 
=
ßß 
await
ßß $
service
ßß% ,
.
ßß, -4
&GetPendingAppointmentsByPatientIdAsync
ßß- S
(
ßßS T
	patientId
ßßT ]
)
ßß] ^
;
ßß^ _
return
©© 
Ok
©© 
(
©© 
appointments
©© "
)
©©" #
;
©©# $
}
™™ 	
[
ÆÆ 	
HttpGet
ÆÆ	 
(
ÆÆ 
$str
ÆÆ 0
)
ÆÆ0 1
]
ÆÆ1 2
[
ØØ 	
	Authorize
ØØ	 
(
ØØ #
AuthenticationSchemes
∞∞ !
=
∞∞" #
JwtBearerDefaults
∞∞$ 5
.
∞∞5 6"
AuthenticationScheme
∞∞6 J
,
∞∞J K
Roles
±± 
=
±± 
$str
±± 
)
±± 
]
±± 
public
≤≤ 
async
≤≤ 
Task
≤≤ 
<
≤≤ 
IActionResult
≤≤ '
>
≤≤' (.
 GetPendingAppointmentsByDoctorId
≤≤) I
(
≤≤I J
[
≤≤J K
	FromRoute
≤≤K T
]
≤≤T U
int
≤≤V Y
doctorId
≤≤Z b
)
≤≤b c
{
≥≥ 	
var
¥¥ 
appointments
¥¥ 
=
¥¥ 
await
¥¥ $
service
¥¥% ,
.
¥¥, -3
%GetPendingAppointmentsByDoctorIdAsync
¥¥- R
(
¥¥R S
doctorId
¥¥S [
)
¥¥[ \
;
¥¥\ ]
return
∂∂ 
Ok
∂∂ 
(
∂∂ 
appointments
∂∂ "
)
∂∂" #
;
∂∂# $
}
∑∑ 	
[
ªª 	
HttpGet
ªª	 
(
ªª 
$str
ªª 8
)
ªª8 9
]
ªª9 :
[
ºº 	
	Authorize
ºº	 
(
ºº #
AuthenticationSchemes
ΩΩ !
=
ΩΩ" #
JwtBearerDefaults
ΩΩ$ 5
.
ΩΩ5 6"
AuthenticationScheme
ΩΩ6 J
,
ΩΩJ K
Roles
ææ 
=
ææ 
$str
ææ 
)
ææ 
]
ææ 
public
øø 
async
øø 
Task
øø 
<
øø 
IActionResult
øø '
>
øø' (5
'GetTodayConfirmedAppointmentsByDoctorId
øø) P
(
øøP Q
[
øøQ R
	FromRoute
øøR [
]
øø[ \
int
øø] `
doctorId
øøa i
)
øøi j
{
¿¿ 	
var
¡¡ 
appointments
¡¡ 
=
¡¡ 
await
¡¡ $
service
¡¡% ,
.
¡¡, -:
,GetTodayConfirmedAppointmentsByDoctorIdAsync
¡¡- Y
(
¡¡Y Z
doctorId
¡¡Z b
)
¡¡b c
;
¡¡c d
return
√√ 
Ok
√√ 
(
√√ 
appointments
√√ "
)
√√" #
;
√√# $
}
ƒƒ 	
[
»» 	
HttpPost
»»	 
]
»» 
[
…… 	
	Authorize
……	 
(
…… #
AuthenticationSchemes
   !
=
  " #
JwtBearerDefaults
  $ 5
.
  5 6"
AuthenticationScheme
  6 J
,
  J K
Roles
ÀÀ 
=
ÀÀ 
$str
ÀÀ 
)
ÀÀ 
]
ÀÀ 
public
ÃÃ 
async
ÃÃ 
Task
ÃÃ 
<
ÃÃ 
IActionResult
ÃÃ '
>
ÃÃ' (
BookAppointment
ÃÃ) 8
(
ÃÃ8 9
[
ÃÃ9 :
FromBody
ÃÃ: B
]
ÃÃB C 
BookAppointmentDto
ÃÃD V
request
ÃÃW ^
)
ÃÃ^ _
{
ÕÕ 	
var
ŒŒ 
identityUserId
ŒŒ 
=
ŒŒ  
User
ŒŒ! %
.
ŒŒ% &
	FindFirst
ŒŒ& /
(
ŒŒ/ 0

ClaimTypes
ŒŒ0 :
.
ŒŒ: ;
NameIdentifier
ŒŒ; I
)
ŒŒI J
?
ŒŒJ K
.
ŒŒK L
Value
ŒŒL Q
;
ŒŒQ R
if
–– 
(
–– 
string
–– 
.
––  
IsNullOrWhiteSpace
–– )
(
––) *
identityUserId
––* 8
)
––8 9
)
––9 :
{
—— 
return
““ 
Unauthorized
““ #
(
““# $
new
““$ '
{
”” 
Message
‘‘ 
=
‘‘ %
InvalidUserTokenMessage
‘‘ 5
}
’’ 
)
’’ 
;
’’ 
}
÷÷ 
var
ÿÿ 
appointment
ÿÿ 
=
ÿÿ 
await
ÿÿ #
service
ÿÿ$ +
.
ÿÿ+ ,,
BookAppointmentForPatientAsync
ÿÿ, J
(
ÿÿJ K
request
ŸŸ 
,
ŸŸ 
identityUserId
⁄⁄ 
)
⁄⁄ 
;
⁄⁄  
return
‹‹ 
CreatedAtAction
‹‹ "
(
‹‹" #
nameof
›› 
(
››  
GetAppointmentById
›› )
)
››) *
,
››* +
new
ﬁﬁ 
{
ﬁﬁ 
appointmentId
ﬁﬁ #
=
ﬁﬁ$ %
appointment
ﬁﬁ& 1
.
ﬁﬁ1 2
AppointmentId
ﬁﬁ2 ?
}
ﬁﬁ@ A
,
ﬁﬁA B
appointment
ﬂﬂ 
)
ﬂﬂ 
;
ﬂﬂ 
}
‡‡ 	
[
„„ 	
HttpPut
„„	 
(
„„ 
$str
„„ &
)
„„& '
]
„„' (
[
‰‰ 	
	Authorize
‰‰	 
(
‰‰ #
AuthenticationSchemes
ÂÂ !
=
ÂÂ" #
JwtBearerDefaults
ÂÂ$ 5
.
ÂÂ5 6"
AuthenticationScheme
ÂÂ6 J
,
ÂÂJ K
Roles
ÊÊ 
=
ÊÊ 
$str
ÊÊ 
)
ÊÊ 
]
ÊÊ 
public
ÁÁ 
async
ÁÁ 
Task
ÁÁ 
<
ÁÁ 
IActionResult
ÁÁ '
>
ÁÁ' (
UpdateAppointment
ÁÁ) :
(
ÁÁ: ;
[
ËË 
	FromRoute
ËË 
]
ËË 
int
ËË 
appointmentId
ËË )
,
ËË) *
[
ÈÈ 
FromBody
ÈÈ 
]
ÈÈ "
UpdateAppointmentDto
ÈÈ +
request
ÈÈ, 3
)
ÈÈ3 4
{
ÍÍ 	
var
ÎÎ 
appointment
ÎÎ 
=
ÎÎ 
await
ÎÎ #
service
ÎÎ$ +
.
ÎÎ+ ,$
UpdateAppointmentAsync
ÎÎ, B
(
ÎÎB C
appointmentId
ÎÎC P
,
ÎÎP Q
request
ÎÎR Y
)
ÎÎY Z
;
ÎÎZ [
return
ÌÌ 
Ok
ÌÌ 
(
ÌÌ 
appointment
ÌÌ !
)
ÌÌ! "
;
ÌÌ" #
}
ÓÓ 	
[
ÒÒ 	
HttpPut
ÒÒ	 
(
ÒÒ 
$str
ÒÒ .
)
ÒÒ. /
]
ÒÒ/ 0
[
ÚÚ 	
	Authorize
ÚÚ	 
(
ÚÚ #
AuthenticationSchemes
ÛÛ !
=
ÛÛ" #
JwtBearerDefaults
ÛÛ$ 5
.
ÛÛ5 6"
AuthenticationScheme
ÛÛ6 J
,
ÛÛJ K
Roles
ÙÙ 
=
ÙÙ 
$str
ÙÙ 
)
ÙÙ 
]
ÙÙ 
public
ıı 
async
ıı 
Task
ıı 
<
ıı 
IActionResult
ıı '
>
ıı' ( 
ConfirmAppointment
ıı) ;
(
ıı; <
[
ıı< =
	FromRoute
ıı= F
]
ııF G
int
ııH K
appointmentId
ııL Y
)
ııY Z
{
ˆˆ 	
var
˜˜ 
identityUserId
˜˜ 
=
˜˜  
User
˜˜! %
.
˜˜% &
	FindFirst
˜˜& /
(
˜˜/ 0

ClaimTypes
˜˜0 :
.
˜˜: ;
NameIdentifier
˜˜; I
)
˜˜I J
?
˜˜J K
.
˜˜K L
Value
˜˜L Q
;
˜˜Q R
if
˘˘ 
(
˘˘ 
string
˘˘ 
.
˘˘  
IsNullOrWhiteSpace
˘˘ )
(
˘˘) *
identityUserId
˘˘* 8
)
˘˘8 9
)
˘˘9 :
{
˙˙ 
return
˚˚ 
Unauthorized
˚˚ #
(
˚˚# $
new
˚˚$ '
{
¸¸ 
Message
˝˝ 
=
˝˝ %
InvalidUserTokenMessage
˝˝ 5
}
˛˛ 
)
˛˛ 
;
˛˛ 
}
ˇˇ 
var
ÅÅ 
appointment
ÅÅ 
=
ÅÅ 
await
ÅÅ #
service
ÅÅ$ +
.
ÅÅ+ ,.
 ConfirmAppointmentForDoctorAsync
ÅÅ, L
(
ÅÅL M
appointmentId
ÇÇ 
,
ÇÇ 
identityUserId
ÉÉ 
)
ÉÉ 
;
ÉÉ  
return
ÖÖ 
Ok
ÖÖ 
(
ÖÖ 
appointment
ÖÖ !
)
ÖÖ! "
;
ÖÖ" #
}
ÜÜ 	
[
ââ 	
HttpPut
ââ	 
(
ââ 
$str
ââ /
)
ââ/ 0
]
ââ0 1
[
ää 	
	Authorize
ää	 
(
ää #
AuthenticationSchemes
ãã !
=
ãã" #
JwtBearerDefaults
ãã$ 5
.
ãã5 6"
AuthenticationScheme
ãã6 J
,
ããJ K
Roles
åå 
=
åå 
$str
åå 
)
åå 
]
åå 
public
çç 
async
çç 
Task
çç 
<
çç 
IActionResult
çç '
>
çç' (!
CompleteAppointment
çç) <
(
çç< =
[
çç= >
	FromRoute
çç> G
]
ççG H
int
ççI L
appointmentId
ççM Z
)
ççZ [
{
éé 	
var
èè 
identityUserId
èè 
=
èè  
User
èè! %
.
èè% &
	FindFirst
èè& /
(
èè/ 0

ClaimTypes
èè0 :
.
èè: ;
NameIdentifier
èè; I
)
èèI J
?
èèJ K
.
èèK L
Value
èèL Q
;
èèQ R
if
ëë 
(
ëë 
string
ëë 
.
ëë  
IsNullOrWhiteSpace
ëë )
(
ëë) *
identityUserId
ëë* 8
)
ëë8 9
)
ëë9 :
{
íí 
return
ìì 
Unauthorized
ìì #
(
ìì# $
new
ìì$ '
{
îî 
Message
ïï 
=
ïï %
InvalidUserTokenMessage
ïï 5
}
ññ 
)
ññ 
;
ññ 
}
óó 
var
ôô 
appointment
ôô 
=
ôô 
await
ôô #
service
ôô$ +
.
ôô+ ,/
!CompleteAppointmentForDoctorAsync
ôô, M
(
ôôM N
appointmentId
öö 
,
öö 
identityUserId
õõ 
)
õõ 
;
õõ  
return
ùù 
Ok
ùù 
(
ùù 
appointment
ùù !
)
ùù! "
;
ùù" #
}
ûû 	
[
¢¢ 	
HttpPut
¢¢	 
(
¢¢ 
$str
¢¢ 
)
¢¢ 
]
¢¢ 
[
££ 	
	Authorize
££	 
(
££ #
AuthenticationSchemes
§§ !
=
§§" #
JwtBearerDefaults
§§$ 5
.
§§5 6"
AuthenticationScheme
§§6 J
,
§§J K
Roles
•• 
=
•• 
$str
•• *
)
••* +
]
••+ ,
public
¶¶ 
async
¶¶ 
Task
¶¶ 
<
¶¶ 
IActionResult
¶¶ '
>
¶¶' (
CancelAppointment
¶¶) :
(
¶¶: ;
[
¶¶; <
FromBody
¶¶< D
]
¶¶D E"
CancelAppointmentDto
¶¶F Z
request
¶¶[ b
)
¶¶b c
{
ßß 	
if
®® 
(
®® 
User
®® 
.
®® 
IsInRole
®® 
(
®® 
$str
®® %
)
®®% &
)
®®& '
{
©© 
var
™™ 
appointment
™™ 
=
™™  !
await
™™" '
service
™™( /
.
™™/ 0$
CancelAppointmentAsync
™™0 F
(
™™F G
request
™™G N
)
™™N O
;
™™O P
return
¨¨ 
Ok
¨¨ 
(
¨¨ 
appointment
¨¨ %
)
¨¨% &
;
¨¨& '
}
≠≠ 
if
ØØ 
(
ØØ 
User
ØØ 
.
ØØ 
IsInRole
ØØ 
(
ØØ 
PatientRoleName
ØØ -
)
ØØ- .
)
ØØ. /
{
∞∞ 
var
±± 
identityUserId
±± "
=
±±# $
User
±±% )
.
±±) *
	FindFirst
±±* 3
(
±±3 4

ClaimTypes
±±4 >
.
±±> ?
NameIdentifier
±±? M
)
±±M N
?
±±N O
.
±±O P
Value
±±P U
;
±±U V
if
≥≥ 
(
≥≥ 
string
≥≥ 
.
≥≥  
IsNullOrWhiteSpace
≥≥ -
(
≥≥- .
identityUserId
≥≥. <
)
≥≥< =
)
≥≥= >
{
¥¥ 
return
µµ 
Unauthorized
µµ '
(
µµ' (
new
µµ( +
{
∂∂ 
Message
∑∑ 
=
∑∑  !%
InvalidUserTokenMessage
∑∑" 9
}
∏∏ 
)
∏∏ 
;
∏∏ 
}
ππ 
var
ªª 
appointment
ªª 
=
ªª  !
await
ªª" '
service
ªª( /
.
ªª/ 0.
 CancelAppointmentForPatientAsync
ªª0 P
(
ªªP Q
request
ºº 
,
ºº 
identityUserId
ΩΩ "
)
ΩΩ" #
;
ΩΩ# $
return
øø 
Ok
øø 
(
øø 
appointment
øø %
)
øø% &
;
øø& '
}
¿¿ 
if
¬¬ 
(
¬¬ 
User
¬¬ 
.
¬¬ 
IsInRole
¬¬ 
(
¬¬ 
DoctorRoleName
¬¬ ,
)
¬¬, -
)
¬¬- .
{
√√ 
var
ƒƒ 
identityUserId
ƒƒ "
=
ƒƒ# $
User
ƒƒ% )
.
ƒƒ) *
	FindFirst
ƒƒ* 3
(
ƒƒ3 4

ClaimTypes
ƒƒ4 >
.
ƒƒ> ?
NameIdentifier
ƒƒ? M
)
ƒƒM N
?
ƒƒN O
.
ƒƒO P
Value
ƒƒP U
;
ƒƒU V
if
∆∆ 
(
∆∆ 
string
∆∆ 
.
∆∆  
IsNullOrWhiteSpace
∆∆ -
(
∆∆- .
identityUserId
∆∆. <
)
∆∆< =
)
∆∆= >
{
«« 
return
»» 
Unauthorized
»» '
(
»»' (
new
»»( +
{
…… 
Message
   
=
    !%
InvalidUserTokenMessage
  " 9
}
ÀÀ 
)
ÀÀ 
;
ÀÀ 
}
ÃÃ 
var
ŒŒ 
appointment
ŒŒ 
=
ŒŒ  !
await
ŒŒ" '
service
ŒŒ( /
.
ŒŒ/ 0-
CancelAppointmentForDoctorAsync
ŒŒ0 O
(
ŒŒO P
request
œœ 
,
œœ 
identityUserId
–– "
)
––" #
;
––# $
return
““ 
Ok
““ 
(
““ 
appointment
““ %
)
““% &
;
““& '
}
”” 
return
’’ 
Forbid
’’ 
(
’’ 
)
’’ 
;
’’ 
}
÷÷ 	
[
ŸŸ 	

HttpDelete
ŸŸ	 
(
ŸŸ 
$str
ŸŸ )
)
ŸŸ) *
]
ŸŸ* +
[
⁄⁄ 	
	Authorize
⁄⁄	 
(
⁄⁄ #
AuthenticationSchemes
€€ !
=
€€" #
JwtBearerDefaults
€€$ 5
.
€€5 6"
AuthenticationScheme
€€6 J
,
€€J K
Roles
‹‹ 
=
‹‹ 
$str
‹‹ 
)
‹‹ 
]
‹‹ 
public
›› 
async
›› 
Task
›› 
<
›› 
IActionResult
›› '
>
››' (
DeleteAppointment
››) :
(
››: ;
[
››; <
	FromRoute
››< E
]
››E F
int
››G J
appointmentId
››K X
)
››X Y
{
ﬁﬁ 	
var
ﬂﬂ 
appointment
ﬂﬂ 
=
ﬂﬂ 
await
ﬂﬂ #
service
ﬂﬂ$ +
.
ﬂﬂ+ ,$
DeleteAppointmentAsync
ﬂﬂ, B
(
ﬂﬂB C
appointmentId
ﬂﬂC P
)
ﬂﬂP Q
;
ﬂﬂQ R
return
·· 
Ok
·· 
(
·· 
appointment
·· !
)
··! "
;
··" #
}
‚‚ 	
}
„„ 
}‰‰ ß
`C:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Controllers\AdminController.cs
	namespace 	
HealthCareApp
 
. 
Controllers #
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
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
( !
AuthenticationSchemes 
= 
JwtBearerDefaults  1
.1 2 
AuthenticationScheme2 F
,F G
Roles 
= 
$str 
) 
] 
public 

class 
AdminController  
(  !
IDoctorService! /
doctorService0 =
)= >
:? @
ControllerBaseA O
{ 
[ 	
HttpPost	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
CreateDoctor) 5
(5 6
[6 7
FromBody7 ?
]? @
CreateDoctorDtoA P
requestQ X
)X Y
{ 	
var 
result 
= 
await 
doctorService ,
., -$
CreateDoctorByAdminAsync- E
(E F
requestF M
)M N
;N O
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
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAllDoctors) 6
(6 7
[7 8
	FromQuery8 A
]A B$
DoctorPaginationQueryDtoC [
query\ a
)a b
{ 	
var 
result 
= 
await 
doctorService ,
., -#
GetAllDoctorsPagedAsync- D
(D E
queryE J
)J K
;K L
return 
Ok 
( 
result 
) 
; 
} 	
[   	
HttpPut  	 
(   
$str   )
)  ) *
]  * +
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' (
UpdateDoctor!!) 5
(!!5 6
["" 
	FromRoute"" 
]"" 
int"" 
doctorId"" $
,""$ %
[## 
FromBody## 
]## 
UpdateDoctorDto## &
request##' .
)##. /
{$$ 	
var%% 
result%% 
=%% 
await%% 
doctorService%% ,
.%%, -
UpdateDoctorAsync%%- >
(%%> ?
doctorId%%? G
,%%G H
request%%I P
)%%P Q
;%%Q R
return'' 
Ok'' 
('' 
result'' 
)'' 
;'' 
}(( 	
[** 	

HttpDelete**	 
(** 
$str** ,
)**, -
]**- .
public++ 
async++ 
Task++ 
<++ 
IActionResult++ '
>++' (
DeleteDoctor++) 5
(++5 6
[++6 7
	FromRoute++7 @
]++@ A
int++B E
doctorId++F N
)++N O
{,, 	
var-- 
result-- 
=-- 
await-- 
doctorService-- ,
.--, -
DeleteDoctorAsync--- >
(--> ?
doctorId--? G
)--G H
;--H I
return// 
Ok// 
(// 
result// 
)// 
;// 
}00 	
}11 
}22 ∂	
XC:\Users\310476\Desktop\HealthCareApp\HealthCareApp\HealthCareApp\Constants\TimeSlots.cs
	namespace 	
HealthCareApp
 
. 
	Constants !
{ 
public 

static 
class 
	TimeSlots !
{ 
public 
static 
IReadOnlyList #
<# $
string$ *
>* +
Slots, 1
{2 3
get4 7
;7 8
}9 :
=; <
new= @
ListA E
<E F
stringF L
>L M
{ 	
$str

 !
,

! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
,! "
$str !
} 	
.	 


AsReadOnly
 
( 
) 
; 
} 
} 