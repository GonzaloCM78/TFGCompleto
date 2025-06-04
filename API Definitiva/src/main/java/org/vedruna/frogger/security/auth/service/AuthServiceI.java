package org.vedruna.frogger.security.auth.service;

import org.vedruna.frogger.security.auth.dto.AuthResponseDTO;
import org.vedruna.frogger.security.auth.dto.LoginRequestDTO;
import org.vedruna.frogger.security.auth.dto.RegisterRequestDTO;

public interface AuthServiceI {
    AuthResponseDTO login(LoginRequestDTO request);
    void register(RegisterRequestDTO request);
}
