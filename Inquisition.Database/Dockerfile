FROM microsoft/mssql-server-linux:latest AS base
CMD ["/opt/mssql-tools/bin/sqlcmd", "-S localhost -U SA -P Pa55word!"]
CMD ["/opt/mssql-tools/bin/sqlcmd", "-q 'CREATE DATABASE InquisitionDB'"]
CMD ["/opt/mssql-tools/bin/sqlcmd", "exit"]