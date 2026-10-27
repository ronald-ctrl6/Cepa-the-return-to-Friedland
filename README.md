# 🧅 Cepa: The Return to Friedland

> Plataformas 2D de acción y aventura en pixel art, hecho con Unity. Una cebolla diminuta debe atravesar tres hábitats infestados de insectos para rescatar a sus amigos "cebollitos" de un monstruo hambriento.

**Estructura y Arquitectura de Videojuegos** · *Universidad Fidélitas – II Cuatrimestre, 2026* · **Proyecto programado**

![Estado](https://img.shields.io/badge/estado-en%20desarrollo-yellow) ![Unity](https://img.shields.io/badge/Unity-2022.3.62f3-black?logo=unity) ![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white) ![Plataforma](https://img.shields.io/badge/plataforma-PC%20%7C%20WebGL-blue)

<!-- Agregar captura de pantalla o GIF del gameplay (assets/docs/gameplay.gif) -->
<!-- Agregar enlace a la demo jugable (WebGL) -->

La version actual es jugable, sin embargo, todavía se encuentra en desarrollo.
Link de itchio: https://rcalderon.itch.io/cepa-the-return-to-friedland

---

## Equipo – Grupo 5

| Integrante | Rol |
|---|---|
| **Lucas Ramírez** | Player Controller & Core Mechanics |
| **Josué Azofeifa** | AI, Obstacles & Logic Systems |
| **Cristofer Montero** | Level Design, UI & Assets Selection |
| **Ronald Calderón Mejía** | Git Architecture, Integration & QA |

**Profesora:** Díaz Luna Ariana María

---

## Historia

En "La Freidora", una aldea escondida entre la maleza, vivían felices las cebollas y los cebollinos. Un insecto gigante, movido por un hambre insaciable, destruyó la aldea con sus extremidades y sus ventiscas y capturó a todos los "cebollitos". Olvidó a uno: **Cepa**. Al ver lo ocurrido, Cepa decide cruzar distintos hábitats, derrotar al insecto y llevar a sus amigos de vuelta a casa.

## Objetivos del jugador

- Rescatar a todos los cebollitos para devolverles su hogar.
- Explorar los niveles en busca de secretos, objetos y cebollitos.
- Derrotar esbirros para ganar puntos y recuperar vida.
- Recolectar herramientas que faciliten el combate.
- Derrotar al enemigo principal.

---

## Mecánicas

### Controles (versión actual)

| Acción | Tecla |
|---|---|
| Mover | `A` / `D` o flechas |
| Saltar | `Espacio` (mantener = salto más alto) |
| Dash | `Shift izquierdo` (tras conseguir la botella de aceite) |
| Escalar | `W` / `S` o flechas en una escalera |

<!-- Confirmar las teclas del inventario (I) y la pausa (Esc) cuando estén implementadas -->

### Sistema de vida
Cepa tiene **3 corazones**. Los enemigos, proyectiles y caídas al vacío le restan 1 con un breve periodo de invulnerabilidad. Al llegar a 0 se carga la pantalla de Game Over. El estado (vida, monedas, gemas, rescates, habilidades) persiste entre escenas.

### Herramientas
| Objeto | Efecto | Estado |
|---|---|---|
| Botella de aceite | Dash terrestre y aéreo (y salto doble) | ✅ Implementado |
| Fósforo puro | Raíces más largas, golpes y gancho/látigo | 🗓️ Planificado |
| Lanza guisantes | Proyectiles contra enemigos voladores y obstáculos | 🗓️ Planificado |

### Coleccionables
Monedas (sueltan los enemigos), gemas (premian la exploración) y cebollitos (rescates, con contador `x/6` en el HUD).

---

## Enemigos

| Enemigo | Comportamiento |
|---|---|
| **Gusano come cebolla** | Común. Estados tranquilo/alerta según el cono de visión o al recibir daño. |
| **Mosca come cebolla** | Vigila desde el aire y dispara bolas de saliva cuando detecta al jugador; protege a los cebollitos. *(Implementada: detección por radio, disparo con cadencia y flotación.)* |
| **Jefe final (insecto)** | Tornados de viento, bolas de saliva, pisotones con daño en área y modo veloz cuando cambian sus ojos. |

---

## Niveles

Tres mundos de 5 pantallas (16:9), ampliables a futuro.

1. **La Aldea** – Tutorial de salto y movimiento. Se obtiene la botella de aceite. *Inspiración: Los Pitufos, Ori and the Blind Forest.*
2. **Jardín de las Cebollas** – Seis cebollitos que rescatar, cuevas y el fósforo puro. *Inspiración: Grounded, Hollow Knight.*
3. **Cueva de los Insectos** – El nivel más difícil: estética de basurero, lanza guisantes y jefe final. *Inspiración: Little Nightmares, It Takes Two, Pikmin.*

**Progreso actual:** Nivel 1 (5 pantallas) jugable; escenas base de los niveles 2 y 3 creadas.

---

## Interfaz

- **Menú principal:** Jugar, Continuar, Selección de niveles, Opciones, Créditos, Salir.
- **HUD:** corazones, capas de cebolla, cebollitos rescatados, monedas, gemas e inventario rápido.
- **Pausa, Victoria y Derrota:** resúmenes de desempeño (cebollitos, monedas, gemas, tiempo).

Los mockups del GDD son preliminares y pueden cambiar durante el desarrollo.

## Arte y sonido

Pixel art 2D con estética caricaturesca; paleta por nivel (flora → contaminación → basurero). Música dinámica que sube de tensión por nivel. Referencias: Kirby, Hollow Knight, Cuphead, Donkey Kong Country, Celeste, Katana Zero, Super Mario World, Mega Man 7.

> Esta entrega usa **placeholders visuales y de audio** de la Unity Asset Store y bancos libres para garantizar una ejecución estable. Los assets finales se harán en Aseprite.

---

## Tecnologías

| Área | Herramienta |
|---|---|
| Motor | Unity 2022.3.62f3 LTS (2D) |
| Lenguaje | C# |
| IDE | Visual Studio |
| Control de versiones | Git + GitHub (ramas por integrante) |
| Cámara | Cinemachine |
| UI | Unity UI (uGUI) + TextMeshPro |
| Arte | Aseprite *(planificado)* |

## Arquitectura del código (~2 300 líneas de C#, 30 scripts)

```
Assets/Scripts/
├── Character/
│   ├── Cepa/       PlayerMovement, PlayerClimbing, PlayerHealth, PersistentPlayer
│   └── Enemies/    EnemyController, EnemyPatrol, EnemyShooter, EnemyProjectile, EnemyHover, EnemyDamage
├── Core/           GameManager (singleton persistente), MusicManager, KillZone, MovingPlatform, MoveCamera, ScrollLayer
├── Objects/        ItemCollectible (moneda / gema / cebollito), PowerUpAceite
└── UI/             UIManager, ScoreManager, MainMenu, GameOver, Congrats, transiciones de escena
```

Decisiones destacadas: `GameManager` como singleton con `DontDestroyOnLoad` para conservar el progreso entre escenas, componentes de enemigo desacoplados (patrulla, disparo, daño y flotación por separado), y prefabs reutilizables para ítems, plataformas y puntos de respawn.

## Hoja de ruta

- [x] Movimiento, salto variable, dash y escaleras
- [x] Vida, respawn, Game Over y persistencia entre escenas
- [x] Enemigo volador con proyectiles
- [x] Coleccionables y HUD
- [x] Nivel 1 completo
- [ ] Fósforo puro y lanza guisantes
- [ ] Gusano y jefe final
- [ ] Niveles 2 y 3
- [ ] Pausa, inventario y selección de niveles
- [ ] Arte final y publicación

## 📄 Créditos

Proyecto académico. Inspirado en la obra de Masahiro Sakurai (Kirby). Assets de terceros: Cainos, Pixel Adventure, DEVNIK 2D, BoldPixels, Jungle Asset Pack y otros; ver licencias en cada paquete.
