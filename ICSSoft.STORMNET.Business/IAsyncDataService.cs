namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    public interface IAsyncDataService : ICloneable
    {
        DataServiceOptions Options { get; set; }

        /// <summary>
        /// �������� ����������.
        /// </summary>
        ISecurityManager SecurityManager { get; }

        /// <summary>
        /// ������� ������ ������.
        /// </summary>
        IAuditService AuditService { get; }

        /// <summary>
        /// ������ ����.
        /// </summary>
        ICacheService CacheService { get; }

        /// <summary>
        /// ���������� ���������� �������� ��������������� �������.
        /// </summary>
        /// <param name="customizationStruct">��� ��������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ���������� ��������.
        /// </returns>
        Task<int> GetObjectsCount(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// �������� ������ ������� ������.
        /// </summary>
        /// <param name="dobject">������ ������, ������� ��������� ���������.</param>
        /// <param name="clearDataObject">������� �� ������.</param>
        /// <param name="checkExistingObject">��������� �� ������������� ������� � ���������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task LoadObject(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// �������� ������ ������� ������ �� �������������.
        /// </summary>
        /// <param name="dobject">������ ������, ������� ��������� ���������.</param>
        /// <param name="dataObjectView">�������������.</param>
        /// <param name="clearDataObject">������� �� ������.</param>
        /// <param name="checkExistingObject">��������� �� ������������� ������� � ���������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task LoadObject(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// �������� �������� ������ �� �������������.
        /// </summary>
        /// <param name="dataobjects">�������� �������.</param>
        /// <param name="dataObjectView">������������.</param>
        /// <param name="clearDataobject">������� �� ������������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task LoadObjects(IEnumerable<DataObject> dataobjects, View dataObjectView, bool clearDataobject = true);

        /// <summary>
        /// �������� �������� ������ �� �������������.
        /// </summary>
        /// <param name="dataObjectView">�������������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(View dataObjectView);

        /// <summary>
        /// �������� �������� ������ �� ���������� ��������������.
        /// </summary>
        /// <param name="dataObjectViews">������ �������������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<View> dataObjectViews);

        /// <summary>
        /// �������� �������� ������ �� ������� ��������.
        /// </summary>
        /// <param name="customizationStructs">������ ��������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<LoadingCustomizationStruct> customizationStructs);

        /// <summary>
        /// �������� �������� ������ �� �������������.
        /// </summary>
        /// <param name="dataObjectView">�������������.</param>
        /// <param name="changeViewForTypeDelegate">������� ��� ���������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// �������� �������� ������ �� ������� �������������.
        /// </summary>
        /// <param name="dataObjectViews">������ �������������.</param>
        /// <param name="changeViewForTypeDelegate">������� ��� ���������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<View> dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// �������� �������� ������ �� ������� ��������.
        /// </summary>
        /// <param name="customizationStructs">������ ��������.</param>
        /// <param name="changeViewForTypeDelegate">������� ��� ���������.</param>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<LoadingCustomizationStruct> customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <param name="customizationStruct">����������� ��������� ��� �������<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>��������� �������.</returns>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <param name="customizationStruct">����������� ��������� ��� �������<see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="State">��������� �������( ��� ����������� ������� ).</param>
        /// <returns></returns>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(LoadingCustomizationStruct customizationStruct, ref object State);

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <param name="State">��������� �������( ��� ����������� �������).</param>
        /// <returns></returns>
        /// <returns>
        /// ������ <see cref="Task"/>, �������������� ����������� ��������.
        /// ��������� �������� ��������� �������� ������.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(ref object State);

        /// <summary>
        /// ���������� ������� ������.
        /// </summary>
        /// <param name="dobject">������ ������, ������� ��������� ��������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task UpdateObject(ref DataObject dobject);

        /// <summary>
        /// ���������� �������� ������.
        /// </summary>
        /// <param name="objects">������� ������, ������� ��������� ��������.</param>
        /// <returns>������ <see cref="Task"/>, �������������� ����������� ��������.</returns>
        Task UpdateObjects(ref IEnumerable<DataObject> objects);
    }
}
