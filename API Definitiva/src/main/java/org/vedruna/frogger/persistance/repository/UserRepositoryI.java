package org.vedruna.frogger.persistance.repository;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import org.vedruna.frogger.persistance.model.User;
import java.util.Optional;


@Repository
public interface UserRepositoryI extends JpaRepository<User, Integer> {
    //https://docs.spring.io/spring-data/jpa/reference/jpa/query-methods.html
    Optional<User> findByUsername(String username);

    Page<User> findByUsernameStartingWith(String name, Pageable pageable);

    // Métodos para obtener seguidores y seguidos
    Page<User> findFollowingByUserId(Integer userId, Pageable pageable);
    Page<User> findFollowersByUserId(Integer userId, Pageable pageable);

} 