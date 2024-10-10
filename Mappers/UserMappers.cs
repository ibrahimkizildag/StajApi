using Api.Dtos.User;
using Api.Models;

namespace Api.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this UserModel userModel)
        {
            return new UserDto
            {
                //ID = userModel.ID,
                ADI = userModel.ADI,
                SOYADI = userModel.SOYADI,
                KULLANICI_ADI = userModel.KULLANICI_ADI,
                SAFEWORD = userModel.SAFEWORD,
                SIFRE = userModel.Password
            };
        }
        public static UserModel ToUserFromCreateDto(this CreateUserRequestDto userDto)
        {
            return new UserModel
            {
               // ID = userDto.ID,
                ADI = userDto.ADI,
                SOYADI = userDto.SOYADI,
                KULLANICI_ADI = userDto.KULLANICI_ADI,
                SAFEWORD = userDto.SAFEWORD,
                Password = userDto.SIFRE
            };
        }
    }

}
