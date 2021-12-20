Imports System.Data.SqlClient

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(String.Format("insert into t_event values('reservationid', {0}, null, null, null, null, '{1}', null, null)", New Random().Next(), DateTime.Now), cn)

                Try
                    cn.Open()
                    cm.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try

            End Using
        End Using
    End Sub

End Class