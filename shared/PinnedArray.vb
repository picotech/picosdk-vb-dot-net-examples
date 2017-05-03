'========================================================================================
'	
'	Filename:			PinnedArray.vb
'
'	Description: 
'		This Class defines an object to hold an array in memory when 
'   	registering a data buffer with a PicoScope/PicoLog driver. 
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Imports System.Runtime.InteropServices

Namespace PinnedArray

    Public Class PinnedArray(Of t)
        Implements IDisposable

        Private _pinnedHandle As GCHandle
        Private _disposed As Boolean

        Public Sub New(ByVal arraySize As Integer)

            Me.New(New t(arraySize - 1) {})

        End Sub

        Public Sub New(array As t())

            _pinnedHandle = GCHandle.Alloc(array, GCHandleType.Pinned)

        End Sub

        Public ReadOnly Property Target() As t()

            Get

                Return DirectCast(_pinnedHandle.Target, t())

            End Get

        End Property

        Public Shared Widening Operator CType(a As PinnedArray(Of t)) As t()

            If a Is Nothing Then

                Return Nothing

            End If

            Return DirectCast(a._pinnedHandle.Target, t())

        End Operator

        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

            If _disposed Then

                Return

            End If

            _disposed = True

            ' Dispose of any IDisposable members
            If disposing Then

            End If

            _pinnedHandle.Free()

        End Sub

#Region " IDisposable Support "
        ' Do not change or add Overridable to these methods. 
        ' Put cleanup code in Dispose(ByVal disposing As Boolean). 
        Public Overloads Sub Dispose() Implements IDisposable.Dispose

            Dispose(True)
            GC.SuppressFinalize(Me)

        End Sub

        Protected Overrides Sub Finalize()

            Try

                Dispose(False)

            Finally

                MyBase.Finalize()

            End Try

        End Sub

#End Region

    End Class

End Namespace
