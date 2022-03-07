void Decompose(uint source, double remain_size_product_d, uint remain_size_product, out uint result, out uint remain_source)
{
    result = source * remain_size_product_d;
    remain_source = source - result * remain_size_product;
}
