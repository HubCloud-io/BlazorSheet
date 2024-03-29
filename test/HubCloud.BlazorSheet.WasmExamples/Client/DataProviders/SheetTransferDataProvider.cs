﻿using HubCloud.BlazorSheet.Core.Dto;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.WasmExamples.Shared.Models;

namespace Company.WebApplication1.DataProviders;

public class SheetTransferDataProvider
{
    private readonly RequestHelper _requestHelper;

    public SheetTransferDataProvider(HttpClient httpClient)
    {
        _requestHelper = new RequestHelper(httpClient);
    }
    
    public async Task<ResponseDto<Sheet>> BuildAsync(SheetTransferCallDto dto)
    {
        var url = $"/SheetTransfer/Build";
        var responseDto = await _requestHelper.MakePostAsync<ResponseDto<Sheet>>(url, dto);

        return responseDto;
    }
    
    public async Task<ResponseDto<SheetDto>> BuildByDtoAsync(SheetTransferCallDto dto)
    {
        var url = $"/SheetTransfer/BuildByDto";
        var responseDto = await _requestHelper.MakePostAsync<ResponseDto<SheetDto>>(url, dto);

        return responseDto;
    }
}