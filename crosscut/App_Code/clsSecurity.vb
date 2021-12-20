Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class Security
    Public Function Check_Security(ByRef sArea As String, ByVal sGroups As String, ByVal sSID As String) As Boolean
        If InStr(sGroups, "MIS Department") > 0 Then
            Return True
        Else
            'Return False
            Return True
        End If
    End Function

    Public Function Is_Logged_On(ByRef sessObject As System.Web.SessionState.HttpSessionState) As Boolean
        Return sessObject.Item("UserID") <> ""
    End Function

    Public Function Is_Logged_On2(ByVal sUserID As String) As Boolean
        Return sUserID <> ""
    End Function

    'Load areas, actions, field lookups, and permission. The absense of permission is a denial of permission.
    'Need an area array, a field array for the loaded area, permissions per area, and permissions per field if applicable.


End Class
