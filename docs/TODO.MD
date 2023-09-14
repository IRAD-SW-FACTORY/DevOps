## Reviar con Patricio y Sebastian

1. Plan actual de github no soporta secretos a nivel de org para repos privados. Implica por ejemplo crear las credenciales de sonar en cada repo, no es tan terrible, pero ojo con dejar repos publicos!!!!
2. Plan de github no soporta: -Protected branches on all repos y Required reviewers en otros deseables para garantizar calidad.



## TODO

1. Agregar certificado ssl a sonarqube o rutear por iis el trafico y agregar cert en esa capa.




## Workflows propuestos

.Net framework 4.8 o inferior:

- Default:
1. Build
2. Test and Sonarqube

- Publish:
1. Build
2. Test and Sonarqube
3. Create Release Package
4. Deploy to environment (tag for production)

## Actions

TODO move logic to bat or cmd file , this will allow local testing of the scripts!

## Git flow and protections