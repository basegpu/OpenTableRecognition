# OpenTableRecognition
library to read an image that contains a table

## building
~~~
docker build --pull -t dotnetapp .
docker build --pull --target testrunner -t dotnetapp:test .
~~~

## testing
~~~
docker run --rm -v "$(pwd)"/tests/TestResults:/app/tests/TestResults dotnetapp:test
~~~

## running app
~~~
docker run --rm dotnetapp
~~~

## bash scripts
building and running the tests
~~~
./buildAndTest.sh
~~~

building and running the app
~~~
./buildAndRun.sh
~~~
