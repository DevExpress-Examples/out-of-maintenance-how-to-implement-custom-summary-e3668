Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Windows.Controls
Imports System.Xml.Serialization
Imports DevExpress.Xpf.PivotGrid

Namespace DXPivotGrid_CustomSummary
	Partial Public Class MainPage
		Inherits UserControl
        Private dataFileName As String = "nwind.xml"
		Private minSum As Integer = 50
		Public Sub New()
			InitializeComponent()

			' Parses an XML file and creates a collection of data items.
            Dim assembly As System.Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()
            Dim stream As Stream = assembly.GetManifestResourceStream(dataFileName)
			Dim s As New XmlSerializer(GetType(OrderData))
			Dim dataSource As Object = s.Deserialize(stream)

			' Binds a pivot grid to this collection.
			pivotGridControl1.DataSource = dataSource
		End Sub
        Private Sub pivotGridControl1_CustomSummary(ByVal sender As Object,
                                                    ByVal e As PivotCustomSummaryEventArgs)
            If e.DataField IsNot fieldFreight Then
                Return
            End If

            ' A variable which counts the number of orders whose freight cost exceeds $50.
            Dim order50Count As Integer = 0

            ' Get the record set corresponding to the current cell.
            Dim ds As PivotDrillDownDataSource = e.CreateDrillDownDataSource()

            ' Iterate through the records and count the orders.
            For i As Integer = 0 To ds.RowCount - 1
                Dim row As PivotDrillDownDataRow = ds(i)

                ' Get the order's total sum.
                Dim orderSum As Decimal = CDec(row(fieldFreight))
                If orderSum >= minSum Then
                    order50Count += 1
                End If
            Next i

            ' Calculate the percentage.
            If ds.RowCount > 0 Then
                e.CustomValue = CDec(order50Count) / ds.RowCount
            End If
        End Sub
	End Class
End Namespace