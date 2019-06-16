#!/usr/bin/env bash

docker build --target testrunner -t dotnetapp:test . & docker run --rm -v "$(pwd)"/tests/TestResults:/app/tests/TestResults dotnetapp:test