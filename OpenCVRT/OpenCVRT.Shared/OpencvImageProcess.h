#pragma once

using namespace Windows::Storage::Streams;
using namespace Microsoft::WRL;
using namespace Platform::Runtime::InteropServices;
using namespace Windows::UI::Xaml::Media::Imaging;

namespace OpenCVRT
{
	public ref class OpencvImageProcess sealed
	{
	private:
		byte* GetPointerToPixelData(IBuffer^ buffer);
		inline void ThrowIfFailed(HRESULT hr);
	public:
		WriteableBitmap^ GetImageCorners(WriteableBitmap^ source);
		OpencvImageProcess();
	};
}
