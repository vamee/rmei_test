Imports Microsoft.VisualBasic

Public Class GlobalVariables

    Public Shared Function GetValue(ByVal strVariableName As String) As String
        Dim Result As String = ""

        Dim Command As New Data.SqlClient.SqlCommand
        Command.Parameters.AddWithValue("@VariableName", strVariableName)

        Dim Sql As String = "SELECT VariableValue FROM ApplicationVariables WHERE VariableName = @VariableName"

        Result = Emagine.GetDbValue(Sql, Command).ToString

        Return Result
    End Function

End Class
