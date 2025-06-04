package org.vedruna.frogger.service;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import org.vedruna.frogger.dto.UserDTO;
import org.vedruna.frogger.persistance.model.User;

public interface UserServiceI {
    UserDTO selectMyUser(User user);
    UserDTO getMyProfile(User user);
    UserDTO getProfileByName(String name);
    UserDTO getUserById(Integer userId);
    void deleteUser(Integer userId);
    

    // Nuevo método para buscar usuarios
    Page<UserDTO> searchUsersByName(String name, Pageable pageable);

    void followUser(Integer loggedUserId, Integer userIdToFollow);
    void unfollowUser(Integer loggedUserId, Integer userIdToUnfollow);
    Page<UserDTO> getFollowing(Integer userId, Pageable pageable);
    Page<UserDTO> getFollowers(Integer userId, Pageable pageable);

}