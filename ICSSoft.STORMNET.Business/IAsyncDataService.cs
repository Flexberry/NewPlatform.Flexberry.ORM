namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    public interface IAsyncDataService : ICloneable
    {
        //����� ��? DataServiceOptions Options { get; set; }

        /// <summary>
        /// ���������� ���������� �������� ��������������� �������.
        /// </summary>
        /// <param name="customizationStruct">��� ��������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ���������� ��������.
        /// </returns>
        Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// �������� ������ ������� ������.
        /// </summary>
        /// <param name="dobject">������ ������, ������� ��������� ���������.</param>
        /// <param name="clearDataObject">������� �� ������.</param>
        /// <param name="checkExistingObject">��������� �� ������������� ������� � ��������� (�������� ���������� ���� ������� ���).</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task LoadObjectAsync(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// �������� ������ ������� ������ �� �������������.
        /// </summary>
        /// <param name="dobject">������ ������, ������� ��������� ���������.</param>
        /// <param name="dataObjectView">�������������.</param>
        /// <param name="clearDataObject">������� ����������� ������� (��. <see cref="ICSSoft.STORMNET.DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">��������� �� ������������� ������� � ��������� (�������� ���������� ���� ������� ���).</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task LoadObjectAsync(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// �������� �������� ������ �� �������������.
        /// </summary>
        /// <param name="dataobjects">�������� �������.</param>
        /// <param name="dataObjectView">������������.</param>
        /// <param name="clearDataobject">������� ����������� ������� (��. <see cref="ICSSoft.STORMNET.DataObject.Clear"/>).</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task LoadObjectsAsync(IEnumerable<DataObject> dataobjects, View dataObjectView, bool clearDataobject = true);

        /// <summary>
        /// �������� �������� ������ �� �������������.
        /// </summary>
        /// <param name="dataObjectView">�������������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(View dataObjectView);

        /// <summary>
        /// �������� �������� ������ �� ���������� ��������������.
        /// </summary>
        /// <param name="dataObjectViews">������ �������������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<View> dataObjectViews);

        /// <summary>
        /// �������� �������� ������ �� ������� ��������.
        /// </summary>
        /// <param name="customizationStructs">������ ��������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<LoadingCustomizationStruct> customizationStructs);

        /// <summary>
        /// �������� �������� ������ �� �������������.
        /// </summary>
        /// <param name="dataObjectView">�������������.</param>
        /// <param name="changeViewForTypeDelegate">�������, ������������ ������������� � ����������� �� ���� ������� (������������ ����� ���������� ������ DataObject'�� ����� �������� �� ���������� ����� - ����. ��� ������������ DataObject'��).</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// �������� �������� ������ �� ������� �������������.
        /// </summary>
        /// <param name="dataObjectViews">������ �������������.</param>
        /// <param name="changeViewForTypeDelegate">�������, ������������ ������������� � ����������� �� ���� ������� (������������ ����� ���������� ������ DataObject'�� ����� �������� �� ���������� ����� - ����. ��� ������������ DataObject'��).</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<View> dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// �������� �������� ������ �� ������� ��������.
        /// </summary>
        /// <param name="customizationStructs">������ ��������.</param>
        /// <param name="changeViewForTypeDelegate">�������, ������������ ������������� � ����������� �� ���� ������� (������������ ����� ���������� ������ DataObject'�� ����� �������� �� ���������� ����� - ����. ��� ������������ DataObject'��).</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<LoadingCustomizationStruct> customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <param name="customizationStruct">����������� ��������� ��� ������� <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <param name="customizationStruct">����������� ��������� ��� ������� <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="State">��������� ������� (��� ����������� �������).</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        //Task<IEnumerable<DataObject>> LoadObjects(LoadingCustomizationStruct customizationStruct, ref object State);

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <param name="State">��������� �������( ��� ����������� �������).</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        //Task<IEnumerable<DataObject>> LoadObjects(ref object State);

        /// <summary>
        /// ���������� ������� ������.
        /// </summary>
        /// <param name="dobject">������ ������, ������� ��������� ��������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task<DataObject> UpdateObjectAsync(DataObject dobject);

        /// <summary>
        /// ���������� �������� ������.
        /// </summary>
        /// <param name="objects">������� ������, ������� ��������� ��������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task<IEnumerable<DataObject>> UpdateObjectsAsync(IEnumerable<DataObject> objects);
    }
}
