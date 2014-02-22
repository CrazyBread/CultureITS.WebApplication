using CultureITS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CultureITS.Helpers
{
    public static class UserHelper
    {
        // Добавляет метод по извлечению информации об авторизованном пользователе
        public static User GetUser(this HttpSessionState session)
        {
            return (session["User"] as User);
        }

        // Добавляет метод по заданию информации об авторизованном пользователе
        public static void Authorize(this HttpSessionState session, User user)
        {
            session["User"] = user;
        }

        // Добавляет метод по проверке на то, авторизован ли пользователь
        public static bool IsAuthorized(this HttpSessionState session)
        {
            return session.GetUser() is User;
        }

        // Добавляет метод по выдаче статуса аккаунта (пользовательской роли)
        public static AccountStatus GetUserRole(this HttpSessionState session)
        {
            return (session.IsAuthorized()) ? session.GetUser().UserRole : Models.AccountStatus.None;
        }
    }
}