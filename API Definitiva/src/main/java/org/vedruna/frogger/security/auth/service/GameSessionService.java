package org.vedruna.frogger.security.auth.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.vedruna.frogger.dto.GameSessionDTO;
import org.vedruna.frogger.persistance.model.GameSession;
import org.vedruna.frogger.persistance.model.User;
import org.vedruna.frogger.persistance.repository.GameSessionRepository;
import org.vedruna.frogger.persistance.repository.UserRepositoryI;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class GameSessionService {

    private final GameSessionRepository gameSessionRepository;
    private final UserRepositoryI userRepository;

    public void saveGameSession(GameSessionDTO dto) {
        Optional<User> optionalUser = userRepository.findByUsername(dto.getUsername());
        if (optionalUser.isEmpty()) {
            throw new RuntimeException("Usuario no encontrado");
        }

        User user = optionalUser.get();

        GameSession session = new GameSession();
        session.setUser(user);
        session.setRonda(dto.getRonda());
        session.setPuntos(dto.getPuntos());
        session.setMapa(dto.getMapa());
        session.setDuracion(dto.getDuracion());

        gameSessionRepository.save(session);
    }
}
