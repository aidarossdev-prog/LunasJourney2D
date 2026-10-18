# 🌙 Luna's Journey

**Luna's Journey** es un videojuego 2D desarrollado en Unity como parte de la Licenciatura en Desarrollo de Sistemas Web de la Universidad de Guadalajara.

El proyecto presenta una aventura de fantasía ambientada en un bosque encantado durante la noche. La protagonista, Luna, es una joven exploradora que recorrerá distintos escenarios mientras interactúa con un entorno mágico.

## 🎮 Estado actual del proyecto

## ✨ Características actuales

- Escenario 2D con ambientación de bosque encantado.
- Escenario ampliado para permitir mayor desplazamiento del personaje.
- Personaje principal con animaciones Idle y Walk.
- Movimiento horizontal mediante teclado.
- Cambio automático de orientación del personaje.
- Sistema de físicas mediante Rigidbody2D y Collider2D.
- Colisiones con el terreno y límites laterales.
- Mecánica de salto con detección de suelo.
- Control de salto para evitar saltos consecutivos mientras Luna está en el aire.
- Prefab reutilizable para el proyectil mágico.
- FirePoint alineado con el personaje para generar los proyectiles.
- Sistema de disparo mágico según la dirección de Luna.
- Cámara 2D con proyección Orthographic.
- Seguimiento horizontal y fluido del personaje.
- Límites de cámara para evitar mostrar áreas fuera del escenario.
- Animaciones del personaje: Idle, Walk, ShootStart y ShootLoop.
- Sistema de disparo mágico continuo mediante la barra espaciadora.
- Animación de preparación del disparo con movimiento del brazo.
- Bucle de animación que mantiene el brazo levantado mientras se dispara.
- Generación de proyectiles mágicos a intervalos configurables.
- Disparo hacia izquierda y derecha según la orientación del personaje.
- Posibilidad de desplazarse y disparar simultáneamente.
- Transiciones entre animaciones controladas mediante Animator.

## 🎮 Controles

| Acción | Control |
|---|---|
| Mover a Luna | A/D o flechas izquierda/derecha |
| Saltar | W o flecha arriba |
| Disparar continuamente | Mantener barra espaciadora (Space) |
| Caminar y disparar | A/D + Space |

## 🎥 Configuración de cámara

Luna's Journey utiliza una cámara 2D con proyección Orthographic, configurada para mantener una vista proporcional del escenario.

La cámara realiza un seguimiento horizontal y fluido de Luna durante su desplazamiento, mientras mantiene fija su posición vertical. También cuenta con límites laterales para evitar mostrar áreas fuera del escenario.

- Proyección: Orthographic
- Orthographic Size: 4.705
- Seguimiento: horizontal y suavizado
- Límites horizontales: -16.72 / 16.72

## 🛠️ Tecnologías utilizadas

- Unity 6.5
- Universal 2D
- C#
- Unity Input System
- Animator Controller
- Rigidbody2D
- Collider2D
- Git
- GitHub

## 📁 Estructura principal

Assets
├── Animations
├── Prefabs
│   └── MagicProjectile
├── Scenes
│   └── MainScene
├── Scripts
│   ├── CameraFollow.cs
│   ├── LunaMovement.cs
│   ├── LunaShooting.cs
│   └── Projectile.cs
└── Sprites
    ├── Background
    ├── Character
    ├── Environment
    └── Projectiles

## ✨ Concepto visual

La dirección artística utiliza un estilo 2D ilustrado inspirado en cuentos de fantasía, con una atmósfera nocturna y mágica. Predominan los tonos azules, violetas y turquesas, acompañados de elementos luminosos.

## 🚧 Proyecto en desarrollo

Luna's Journey se encuentra actualmente en desarrollo. En las siguientes etapas se podrán incorporar nuevas mecánicas, animaciones, plataformas, obstáculos y elementos interactivos conforme avance el proyecto.

## 👩‍💻 Autora

**Aida Rosas Romero**  
Licenciatura en Desarrollo de Sistemas Web  
Universidad de Guadalajara
