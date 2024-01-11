### Proyecto final de Programacion GEOWALL-E

Este proyecto consta de una parte visual y otra logica que son unida por el una clase unica que es el handler que se ocupa de arracancar el compilador tomando como informacion los datos que envia la visual.

Handler: Consta de 4 propiedades y 1 metodo instancia el context que se usara como base dato de todo el servidor. tambien una lista de objetos tipo error para administrar los errores encontrados y por ultimo un geowall_e_program lugar que recopila toda una lista de acciones que de ejecutar el servidor estos conocidos como statement.

Dentro de la parte logica tenenmos el arbol que se compone por la clase ASTNode como raiz del mismo este posee un metodo general que valid usado para que cada clase hija pueda decir si la informacion guardada en ella es valida en correspondencia del lenguaje usado.Tenemos como hijos principales de la raiz a:
 
Expression: Clase dedicada a conceptualizar lo que definiremos como una expression en nuestro lenguaje posee como propiedad general un enum type que define el tipo de expression.

Statement: esta clase no posee propiedades generales solo sirve como para agrupar un grupo de clase que seran reconocidas como statement.

Geowall_e_Program: Clase dedicada para poder agrupar todas los statement  en un propiedad de tipo lista facilitando la interacciones y su posterior ejecucion. esta clase ya define su  metodo de validacion al en true siempre que todos los statement que se incluye en su lista sear informacion correcta.

Antes continuar describiendo los diferentes tipos de statement y expression que existen se debe adicionar un clase particular pues estafunciona como la base de datos para todo el servidor es decir:

Context:Que recopila todas las variables y funciones que se declara en el leguaje ademas de cuatros metodos que se usan para validar y guardar las variables y funciones en sus sistema respectivamente.
IContext: Creado con el objetivo de facilitar el uso de los metodos de context. es decir context hereda de esta interfaz.

Adentrandonos ahora  los diferente tipos de statement tenemos :

colorChange: que guarda la informacion de todo pedido de cambio de color en una propiedad de tipo enum color . ademas de tener un hijo que permiter reestablecer el colo predeterminado(negro). esta clase siempre sera valida.

Draw: Para el comando de dibujo donde cada instancia de esta clase guarda una expression de un tipo de figura en partcicular y  en un string su nombre. se define que estas instancias son validas sis sus figuras son validas.

FuncDef: clase que guarda en cada instancia una funcion definida su id en un string, su parametros en un array de string y su cuerpo lo guarda como una expression. esta clase se considera valida si el context considera valida la funcion.

import: clase que guarda la direcion de donde se va a extraer la info cada vez que se ejecuta el comando import.

matchDecl:Guarda la informacion una vez reconocido el comando , sus parametros en una lista de string y el cuerpo en una expression.

varDecl: clase que ejecuta lo mismo solo que para un comando diferentes su id como string y el tipo como enum. posee una clase hijo que tiene un proopiedad mas de tipo enum type.

vardef: clase que sigue los mismos pasos id como string , cuerpo como expression y tipo como enum type.

Ademas de los statements tenemos las expression las cuales abarca la conceptualizacionde figuras hasta operaciones aritmeticas, booleanas y de comparacion. ademas de abarca estructuras especiales de expressiones como es: 

FuncCall: este es un llamado de una funcion  y registra las los argumentos e ID de la misma ademas que valida si esta funcion ya existe registrada en el contexto.

IfThenElse: Estructura de tres expression que hace referencia a la espressiones boleanas guarda la condicion que debe cumplir y las dos posibles respuesta en caso de true o false.

letin: otra estructura que contiene un grupo de parametros y un cuerpo que puee ejecutar aciones tomando como base datos el context goblal y uno local  con sus parametros anteriores.

varcall: es el llamado de una variables registra su id .

Number: esta es una expression atomica y se considera una de las hoja del arbol pues solo guarda el valor del un numero.

logicNOT : REGRISTA UNA EXPRESSION.

Anteriormente explicamos que entre los tipos de expression podiamos definir figuras geometricas como:

CirleDef: guarda todos los parametros de un circulo como expression su radios y centro ademas de poseer un hijo que seria los arcos que guardan como extra sus puntos de origen y llegada tambien expression.

intersectiondef: Registra las dos  figuras que colisionan para poder analizar los puntos coincidentes.

Linedef: Registra los puntos con los que se define la linea es expression. posee dos hijo ray que segment no poseen caracteristica unica .
 
Measuredef: guarda los dos puntos que forman la recta para poder evaluar su distancia posteriormente.

pointdef: guarda su posiciones en el plano . cada eje en un expression.

seguency: guarda todos los valores que la conforman en una lista de expression y el tipo de seguencia que es.

Como ultimo contenido del arbol tenemos los exxpresiones binarias estan son aquella que poseen un operadador y dos miebros cada uno se considera una expression. de este tipo estan:

arithmeticexpression: recopila la suma , resta , multiplicacion division potencia y resto. cada son hijo de esta clase.

LogicEpression : recopila los operador and y or como sus hijos.

Comparissonexpression: este recopila mayor menor y mayor que menor que. ademas de igual y no igual.

Saliendo del arbol tenemos nuestro analizador sintactico el cual se compone por el lexer y parser adems de la clase token de la cual se basan las clases anteriores para trabajar. 

Token: alfabeto del lenguaje define el nombre de cada letra y su tipo .

Lexer: cojunto de clase que se dedica a a clasificar el codigo enviado por el visual y convertirlos en toquen para su posterior procesamiento por el parser.

parser: Cojunto de clases que usando el alfabeto anterirormente definido compone el AST .

para una mejor aclaracion de los diferentes metodos usado para lograr las funciones del parser y lexer se dara durante la exposicion.

Por la parte visual podemos decir que se uso el motor grafico godot del cual haciendo uso  de los metodos predeterminadoss del mismo se puedo facilitar el dibujo de cada figura al solo tener que  definir como se dibuja cada figura.
 se dara mas detalle durante la exposicion.