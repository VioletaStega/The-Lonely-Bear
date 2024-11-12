Ya está atardeciendo y tengo mucha hambre...


-> elecciones

== elecciones
¿Qué debería hacer?
    +[Dormir] -> chosen("dormir la mona y mañana será otro día.")
    +[Buscar comida] -> chosen("comer algo. Verás que al final acabaré comiendome alguna de las setas que hay por aquí. Que degracia de vida...")
    +[Llorar] -> chosen("irme a llorar a la llorería.")
    
== chosen(decisionesOso)

Es cierto... debería {decisionesOso}.
Aunque también me siento bastante solo aquí en el bosque...

-> soledad

== soledad
¿Qué debería hacer?
    +[Es difícil encontrar compañía...] -> chosen2("Será mejor seguir solo... No necesito a nadie.")
    +[Cantar] -> chosen2("¿Y quién me va a entregar sus emociones? ¿Quién me va a pedir que nunca le abandone? ¿Quién me tapará esta noche si hace frío? ¿Quién me va a curar el corazón partió?")
    +[Seguiré buscando.] -> chosen2("No puedo rendirme... Seguro que encontraré alguien con quien poder compartir la deliciosa miel que nos ofrece la vida... Ay miel, que hambre tengo...")
    
== chosen2(soloAcompañado)
{soloAcompañado}
Bueno, será mejor que deje de hablar solo y continue con mi camino.

->END