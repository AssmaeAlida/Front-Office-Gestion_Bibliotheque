# Projet de Gestion de Bibliothèque

## Description
Ce projet de gestion de bibliothèque est conçu pour permettre aux adhérents de réserver des livres en ligne. Il offre plusieurs fonctionnalités clés telles que l'inscription des adhérents, la réservation de livres avec confirmation par email, et l'édition de bons de réservation en PDF.

## Fonctionnalités
- **Inscription des adhérents** : Permet aux utilisateurs de s'inscrire en tant qu'adhérents de la bibliothèque.
- **Réservation (Emprunt) de livres** : Les adhérents peuvent réserver des livres en ligne et recevoir une confirmation par email.
- **Edition PDF du bon de réservation** : Les adhérents peuvent télécharger un PDF de leur réservation.

## Technologies Utilisées
- **Backend** : ASP.NET Core en C#
- **Base de données** : MySQL
- **Frontend** : HTML, CSS, JavaScript
- **Envoi d'emails** : SMTP (Simple Mail Transfer Protocol)
- **Génération de PDF** : PdfSharpCore

## Prérequis
- **.NET Core SDK** : Assurez-vous d'avoir installé le SDK .NET Core.
- **MySQL** : Assurez-vous d'avoir une instance de MySQL en cours d'exécution.
- **SMTP Server** : Configuration d'un serveur SMTP pour l'envoi des emails de confirmation.

## Configuration
1. **Clonez le dépôt GitHub** :
    ```bash
    git clone https://github.com/AssmaeAlida/Front-Office-Gestion_Bibliotheque
    cd votre-repo
    ```

2. **Configuration de la base de données** :
    - Créez une base de données MySQL et mettez à jour la chaîne de connexion dans `appsettings.json`.

3. **Configurer l'envoi d'emails** :
    - Mettez à jour les paramètres SMTP dans `appsettings.json` avec vos informations de serveur SMTP.

4. **Restaurer les dépendances** :
    ```bash
    dotnet restore
    ```

5. **Appliquer les migrations de la base de données** :
    ```bash
    dotnet ef database update
    ```

6. **Exécuter l'application** :
    ```bash
    dotnet run
    ```

## Utilisation
- Accédez à l'application via `http://localhost:5000`.
- Inscrivez-vous en tant qu'adhérent.
- Connectez-vous et réservez des livres.
- Téléchargez le bon de réservation en PDF après avoir effectué une réservation.


## Licence
Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## Auteurs
- **Assmae A1lida* - *Développeur Principal* - https://github.com/AssmaeAlida .

---

Merci d'avoir utilisé notre application de gestion de bibliothèque !
