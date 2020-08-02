## Получение лицензии Unity

 1. Сделать ветку `license`
 2. Указать переменную `UNITY_VERSION` в github-secrets. В моём случае это **2019.3.15f1**
 3. После того как джоба закончиться скачать файл с запросом лицензии
 4. Сходить на [license.unity3d.com](https://license.unity3d.com/manual) и загрузить туда полученный файл.
 5. Содержимое файла надо сохранить в github-secret. `UNITY_LICENSE`

 ## Deploy to github pages

 1. https://github.com/peaceiris/actions-gh-pages#%EF%B8%8F-first-deployment-with-github_token
 