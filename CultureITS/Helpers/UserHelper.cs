using CultureITS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Helpers
{
    public static class UserHelper
    {
        // Добавляет метод по извлечению информации об авторизованном пользователе
        public static User GetUser(this HttpSessionStateBase session)
        {
            return (session["User"] as User);
        }

        // Добавляет метод по заданию информации об авторизованном пользователе
        public static void Authorize(this HttpSessionStateBase session, User user)
        {
            session["User"] = user;
        }

        // Добавляет метод по проверке на то, авторизован ли пользователь
        public static bool IsAuthorized(this HttpSessionStateBase session)
        {
            return session.GetUser() is User;
        }

        // Добавляет метод по выдаче статуса аккаунта (пользовательской роли)
        public static AccountStatus GetUserRole(this HttpSessionStateBase session)
        {
            return (session.IsAuthorized()) ? session.GetUser().UserRole : Models.AccountStatus.None;
        }
    }
}