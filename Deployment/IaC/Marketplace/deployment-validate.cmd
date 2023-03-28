@echo off
set _my_datetime=%time%
set _my_datetime=%_my_datetime: =_%
set _my_datetime=%_my_datetime::=%
set _my_datetime=%_my_datetime:/=_%
set _my_datetime=%_my_datetime:.=%

@echo on

az deployment group validate -g rg-sdsd-it-ccw-dev-002 -f mainTemplate.json -p mainTemplate-example.json -n deployment-test-%_my_datetime%