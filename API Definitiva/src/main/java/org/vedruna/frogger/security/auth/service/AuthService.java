package org.vedruna.frogger.security.auth.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.vedruna.frogger.persistance.model.User;
import org.vedruna.frogger.persistance.repository.UserRepositoryI;
import org.vedruna.frogger.security.auth.dto.AuthResponseDTO;
import org.vedruna.frogger.security.auth.dto.LoginRequestDTO;
import org.vedruna.frogger.security.auth.dto.RegisterRequestDTO;

@Service
public class AuthService implements AuthServiceI {

    @Autowired
    private UserRepositoryI userRepo;

    @Autowired
    private JWTServiceImpl jwtService;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Autowired
    private AuthenticationManager authenticationManager;

    @Override
    public AuthResponseDTO login(LoginRequestDTO request) {
        authenticationManager.authenticate(
            new UsernamePasswordAuthenticationToken(request.getName(), request.getPassword())
        );

        User user = userRepo.findByUsername(request.getName())
                .orElseThrow(() -> new RuntimeException("Usuario no encontrado"));

        return new AuthResponseDTO(jwtService.getToken(user), user.getUsername(), user.getEmail());
    }

    @Override
    public void register(RegisterRequestDTO request) {
        User user = new User();
        user.setUsername(request.getName());
        user.setPassword(passwordEncoder.encode(request.getPassword()));
        user.setEmail(request.getEmail());

        userRepo.save(user);
    }
}
