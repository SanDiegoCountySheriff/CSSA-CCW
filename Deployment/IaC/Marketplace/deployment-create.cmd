@echo off
set _my_datetime=%time%
set _my_datetime=%_my_datetime: =_%
set _my_datetime=%_my_datetime::=%
set _my_datetime=%_my_datetime:/=_%
set _my_datetime=%_my_datetime:.=%

@echo on

call az account set -s sdsd-dev
call az deployment group create -g rg-sdsd-it-ccw-dev-002 -f mainTemplate-test.json -p mainTemplate-example.json -n deployment-test-%_my_datetime%