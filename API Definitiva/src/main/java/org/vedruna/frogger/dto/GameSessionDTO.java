package org.vedruna.frogger.dto;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class GameSessionDTO {
    private String username;
    private int ronda;
    private int puntos;
    private String mapa;
    private String duracion;
}
