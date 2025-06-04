package org.vedruna.frogger.persistance.model;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDateTime;

@Getter
@Setter
@Entity
@Table(name = "game_sessions")
public class GameSession {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private int ronda;
    private int puntos;
    private String mapa;
    private String duracion;

    private LocalDateTime fecha = LocalDateTime.now();

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "username")  // Ahora sí se guarda en BBDD
    private String username;

    @PostLoad
    private void loadUsername() {
        if (user != null) {
            this.username = user.getUsername();
        }
    }
}
