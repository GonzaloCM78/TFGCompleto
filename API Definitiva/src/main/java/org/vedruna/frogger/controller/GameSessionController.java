package org.vedruna.frogger.controller;

import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.vedruna.frogger.dto.GameSessionDTO;
import org.vedruna.frogger.security.auth.service.GameSessionService;

@RestController
@RequestMapping("/api/v1/session")
@RequiredArgsConstructor
public class GameSessionController {

    private final GameSessionService gameSessionService;

    @PostMapping("/save")
    public ResponseEntity<?> saveGameSession(@RequestBody GameSessionDTO dto) {
        try {
            gameSessionService.saveGameSession(dto);
            return ResponseEntity.ok("Sesión guardada correctamente");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Error al guardar la sesión: " + e.getMessage());
        }
    }
}
