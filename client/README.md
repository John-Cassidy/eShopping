# Client

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 17.0.9.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Commands

- [Angular CLI Cheat Sheet 1](https://www.digitalocean.com/community/tutorials/angular-angular-cli-reference)
- [Angular CLI Cheat Sheet 2](https://pankaj-kumar.medium.com/angular-cli-cheat-sheet-the-cli-command-an-angular-developer-should-know-42fad81992c9)
- [Angular CLI Cheat Sheet 3](https://malcoded.com/posts/angular-fundamentals-cli/#cheat-sheet)

```powershell

# install node_modules
npm install

# run frontend application
npx ng serve
# or
npm start //(default npm script inside package.json)

npx ng s --p <port no> --h <hostname>

# Some other helpful commands
ng lint # To lint and look for JavaScript errors
ng lint --format stylish # Linting and formatting the output
ng lint --fix # Lint and attempt to fix all the problems
ng build # to build a project in the dist folder
ng build ---target  # Target for which we want to build
ng build --prod # To build in production mode
ng test # To run spec files
ng test --codeCoverage --watch=false
ng e2e # To run e2e test cases
ng doc # To look for angular documentation
ng help # To get help on angular cli commands

# Components
ng generate component  # To generate new component
ng g c  # Short notation to generate component
ng g c  --flat # Want to generate folder name as well?
ng g c  --inline-template # Want to generate HTML file?
ng g c  -it # Short notation
ng g c  --inline-style # Want to generate css file?
ng g c  -is # Short notation
ng g c  --standalone  # Whether the generated component is standalone.
ng g c  --view-encapsulation # View encapsulation stratergy
ng g c  -ve # Short notation
ng g c  --change-detection # Change detection strategy
ng g c  --dry-run # To only report files and don't write them
ng g c  -d # Short notation
ng g c  -m  -d

# Name of module where we need to add component as dependency
Directives and services

ng generate directive  # To generate directive
ng g d  # short notation
ng g d  -d # To only report files and don't write them
ng generate service  # To generate service
ng g s  # short notation
ng g s  -d # To only report files and don't write them
ng g s  -m


# Name of module where we need to add service as dependency
Classes, Interface, pipe, and enums

ng generate class  # To generate class
ng g cl  # short notation
ng generate interface  # To generate interface
ng g i  # short notation
ng generate pipe  # To generate pipe
ng g p  # short notation
ng generate enum  # To generate enum
ng g e  # short notation


# Module and Routing
ng generate module  # To generate module

ng g m  # To short notation
ng g m  --skipTests trus -d # To skip generate spec file for the module
ng g m  --routing # To generate module with routing file
ng g guard  # To generate guard to route

```

## HttpClientModule

file: app.config.ts

```ts
import { ApplicationConfig } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideHttpClient()],
};
```
