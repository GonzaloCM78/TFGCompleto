package org.vedruna.frogger.persistance.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import org.vedruna.frogger.persistance.model.GameSession;

@Repository
public interface GameSessionRepository extends JpaRepository<GameSession, Long> {
}
