#!/bin/bash
rm *.nupkg
msbuild /property:Configuration=Release NHibernate.NpgSql.csproj
nuget pack NHibernate.NpgSql.nuspec
