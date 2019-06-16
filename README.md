# OpenTableRecognition
library to read an image that contains a table

## building
~~~
docker build --pull --t dotnetapp .
docker build --pull --target testrunner -t dotnetapp:test .
~~~

## testing
~~~
docker run --rm dotnetapp
docker run --rm -v "$(pwd)"/tests/TestResults:/app/tests/TestResults dotnetapp:test
~~~