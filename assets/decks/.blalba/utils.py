def first_char_up(text: str) -> str:
    return "".join([x.upper() if i == 0 else x for i, x in enumerate(text)])