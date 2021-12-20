Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsEmailTourPackage

    Shared Function ShouldEmail(packageIssuedID As Int32) As Boolean
        If packageIssuedID = 0 Then Return True
        Dim re = True
        Using cn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select tl.comboitem [tourLocation], rs.comboitem [resort], pi.PackageIssuedID from t_Reservations r inner join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID " & _
                "inner join t_Package p on p.PackageID = pi.PackageID " & _
                "inner join t_PackageReservation pr on pr.packageid = p.packageid " & _
                "inner join t_ComboItems tl on tl.comboitemid = pr.locationid " & _
                "inner join t_comboitems rs on rs.comboitemid = pr.resortcompanyid " & _
                "where rs.comboitem in ('KCP') and tl.ComboItem in ('Williamsburg', 'KCP') and pi.packageissuedid={0}", packageIssuedID)
            Using cm = New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    Dim tl = String.Empty, r = String.Empty
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    If dt.Rows.Count() = 1 Then
                        tl = dt.Rows(0).Field(Of String)("tourLocation")
                        r = dt.Rows(0).Field(Of String)("resort")

                        If (tl.ToLower() = "Williamsburg".ToLower() Or tl.ToLower() = "KCP".ToLower()) And r.ToLower() = "KCP".ToLower() Then re = False
                    End If

                Catch ex As Exception
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return re
    End Function

End Class
